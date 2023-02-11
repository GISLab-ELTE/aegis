// <copyright file="BentleyOttmannAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
//
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

namespace AEGIS.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Algorithms.SweepLines;
    using AEGIS.Resources;

    /// <summary>
    /// Represents the Bentley-Ottmann Algorithm for determining intersection of line strings.
    /// </summary>
    /// <remarks>
    /// The Bentley–Ottmann algorithm is a sweep line algorithm for listing all crossings in a set of line segments.
    /// It extends the Shamos–Hoey algorithm, a similar previous algorithm for testing whether or not a set of line segments has any crossings.
    /// For an input consisting of n line segments with k crossings, the Bentley–Ottmann algorithm takes time O((n + k) log n).
    /// In cases where k = o(n^2 / log n), this is an improvement on a naive algorithm that tests every pair of segments, which takes O(n^2).
    /// </remarks>
    /// <author>Roberto Giachetta, Máté Cserép</author>
    public class BentleyOttmannAlgorithm
    {
        /// <summary>
        /// The event queue.
        /// </summary>
        private readonly EventQueue eventQueue;

        /// <summary>
        /// The sweep line.
        /// </summary>
        private readonly SweepLine sweepLine;

        /// <summary>
        /// The list of intersection coordinates.
        /// </summary>
        private List<Coordinate> intersections;

        /// <summary>
        /// The list of intersection edge indexes in the source list.
        /// </summary>
        private List<Tuple<Int32, Int32>> edgeIndexes;

        /// <summary>
        /// A values indicating whether the result was already computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.eventQueue = new EventQueue(source);
            this.sweepLine = new SweepLine(source, this.PrecisionModel);
            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyOttmannAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyOttmannAlgorithm(IEnumerable<IEnumerable<Coordinate>> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            List<Coordinate> coordinates = new List<Coordinate>();
            foreach (IEnumerable<Coordinate> linestring in source)
                coordinates.AddRange(linestring);

            this.Source = coordinates;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.eventQueue = new EventQueue(source);
            this.sweepLine = new SweepLine(source, this.PrecisionModel);
            this.hasResult = false;
        }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the collection of source coordinates.
        /// </summary>
        /// <value>The collection of source coordinates.</value>
        public IEnumerable<Coordinate> Source { get; private set; }

        /// <summary>
        /// Gets the intersection coordinates.
        /// </summary>
        /// <value>The read-only list of intersection coordinates.</value>
        public IReadOnlyList<Coordinate> Intersections
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();
                return this.intersections;
            }
        }

        /// <summary>
        /// Gets the indexes of intersecting edges.
        /// </summary>
        /// <value>The read-only list of intersecting edge indexes with respect to source coordinates.</value>
        /// <remarks>
        /// Indexes are assigned sequentially to the input edges from zero, skipping a number when starting a new line string.
        /// </remarks>
        public IReadOnlyList<Tuple<Int32, Int32>> EdgeIndexes
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();
                return this.edgeIndexes;
            }
        }

        /// <summary>
        /// Computes the intersection coordinates of a line string.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <returns>The read-only list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> Intersection(IEnumerable<Coordinate> source)
        {
            return new BentleyOttmannAlgorithm(source, null).Intersections;
        }

        /// <summary>
        /// Computes the intersection coordinates of a line string.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The read-only list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> Intersection(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            return new BentleyOttmannAlgorithm(source, precisionModel).Intersections;
        }

        /// <summary>
        /// Computes the intersection coordinates of multiple line strings.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <returns>The read-only list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> Intersection(IEnumerable<IList<Coordinate>> source)
        {
            return new BentleyOttmannAlgorithm(source, null).Intersections;
        }

        /// <summary>
        /// Computes the intersection coordinates of multiple line strings.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The read-only list of intersection coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> Intersection(IEnumerable<IList<Coordinate>> source, PrecisionModel precisionModel)
        {
            return new BentleyOttmannAlgorithm(source, precisionModel).Intersections;
        }

        /// <summary>
        /// Computes the intersection of one or more line strings.
        /// </summary>
        public void Compute()
        {
            this.intersections = new List<Coordinate>();
            this.edgeIndexes = new List<Tuple<Int32, Int32>>();
            Event currentEvent = this.eventQueue.Next();

            while (currentEvent != null)
            {
                EndPointEvent endPointEvent = currentEvent as EndPointEvent;

                if (endPointEvent != null)
                {
                    this.ProcessEndPointEvent(endPointEvent);
                }

                IntersectionEvent intersectionEvent = currentEvent as IntersectionEvent;

                if (intersectionEvent != null)
                {
                    // intersection point event: switch the two concerned segments and check for possible intersection with their below and above segments
                    this.ProcessIntersectionEvent(intersectionEvent);
                }

                currentEvent = this.eventQueue.Next();
            }

            this.hasResult = true;
        }

        /// <summary>
        /// Processes the intersection event.
        /// </summary>
        /// <param name="intersectionEvent">The intersection event.</param>
        private void ProcessIntersectionEvent(IntersectionEvent intersectionEvent)
        {
            SweepLineSegment segment;
            Intersection intersection;

            /*
             * segment order before intersection: segmentBelow <-> segmentAbove <-> segment <-> segmentAboveAbove
             * segment order after intersection:  segmentBelow <-> segment <-> segmentAbove <-> segmentAboveAbove
             */
            segment = intersectionEvent.Above;
            SweepLineSegment segmentAbove = intersectionEvent.Below;

            // dandle closing intersection points when segments (partially) overlap each other
            if (intersectionEvent.IsClosing)
            {
                if (!this.sweepLine.IsAdjacent(segment.Edge, segmentAbove.Edge))
                {
                    this.intersections.Add(intersectionEvent.Vertex);
                    this.edgeIndexes.Add(Tuple.Create(Math.Min(segment.Edge, segmentAbove.Edge),
                                                  Math.Max(segment.Edge, segmentAbove.Edge)));
                }
            }

            // it is possible that the previously detected intersection point is not a real intersection, because a new segment started between them,
            // therefore a repeated check is necessary to carry out
            else if (this.sweepLine.Add(intersectionEvent))
            {
                if (!this.sweepLine.IsAdjacent(segment.Edge, segmentAbove.Edge))
                {
                    this.intersections.Add(intersectionEvent.Vertex);
                    this.edgeIndexes.Add(Tuple.Create(Math.Min(segment.Edge, segmentAbove.Edge),
                                              Math.Max(segment.Edge, segmentAbove.Edge)));

                    intersection = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate, segmentAbove.LeftCoordinate, segmentAbove.RightCoordinate, this.PrecisionModel);

                    if (intersection.Type == IntersectionType.Interval)
                    {
                        IntersectionEvent newIntersectionEvent = new IntersectionEvent
                        {
                            Vertex = intersection.End,
                            Below = segment,
                            Above = segmentAbove,
                            IsClosing = true,
                        };
                        this.eventQueue.Add(newIntersectionEvent);
                    }
                }

                if (segmentAbove.Above != null)
                {
                    intersection = LineAlgorithms.Intersection(segmentAbove.LeftCoordinate, segmentAbove.RightCoordinate, segmentAbove.Above.LeftCoordinate, segmentAbove.Above.RightCoordinate, this.PrecisionModel);

                    if (intersection != null && intersection.Coordinate.X >= intersectionEvent.Vertex.X)
                    {
                        IntersectionEvent newIntersectionEvent = new IntersectionEvent
                        {
                            Vertex = intersection.Coordinate,
                            Below = segmentAbove,
                            Above = segmentAbove.Above,
                        };
                        if (!this.eventQueue.Contains(newIntersectionEvent))
                            this.eventQueue.Add(newIntersectionEvent);
                    }
                }

                if (segment.Below != null)
                {
                    intersection = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate, this.PrecisionModel);

                    if (intersection != null && intersection.Coordinate.X >= intersectionEvent.Vertex.X)
                    {
                        IntersectionEvent newIntersectionEvent = new IntersectionEvent
                        {
                            Vertex = intersection.Coordinate,
                            Below = segment.Below,
                            Above = segment,
                        };
                        if (!this.eventQueue.Contains(newIntersectionEvent))
                            this.eventQueue.Add(newIntersectionEvent);
                    }
                }
            }
        }

        /// <summary>
        /// Processes the end point event.
        /// </summary>
        /// <param name="endPointEvent">The end point event.</param>
        private void ProcessEndPointEvent(EndPointEvent endPointEvent)
        {
            SweepLineSegment segment;
            Intersection intersection;

            switch (endPointEvent.Type)
            {
                // left endpoint event: check for possible intersection with below and / or above segments
                case EventType.Left:
                    segment = this.sweepLine.Add(endPointEvent);

                    if (segment.Above != null)
                    {
                        intersection = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate, segment.Above.LeftCoordinate, segment.Above.RightCoordinate, this.PrecisionModel);

                        if (intersection != null)
                        {
                            IntersectionEvent intersectionEvent = new IntersectionEvent
                            {
                                Vertex = intersection.Coordinate,
                                Below = segment,
                                Above = segment.Above,
                            };
                            this.eventQueue.Add(intersectionEvent);
                        }
                    }

                    if (segment.Below != null)
                    {
                        intersection = LineAlgorithms.Intersection(segment.LeftCoordinate, segment.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate, this.PrecisionModel);

                        if (intersection != null)
                        {
                            IntersectionEvent intersectionEvent = new IntersectionEvent
                            {
                                Vertex = intersection.Coordinate,
                                Below = segment.Below,
                                Above = segment,
                            };
                            this.eventQueue.Add(intersectionEvent);
                        }
                    }

                    break;

                // right endpoint event: check for possible intersection of the below and above segments
                case EventType.Right:
                    segment = this.sweepLine.Search(endPointEvent);

                    if (segment != null)
                    {
                        if (segment.Above != null && segment.Below != null)
                        {
                            intersection = LineAlgorithms.Intersection(segment.Above.LeftCoordinate, segment.Above.RightCoordinate, segment.Below.LeftCoordinate, segment.Below.RightCoordinate, this.PrecisionModel);

                            if (intersection != null)
                            {
                                IntersectionEvent intersectionEvent = new IntersectionEvent
                                {
                                    Vertex = intersection.Coordinate,
                                    Below = segment.Below,
                                    Above = segment.Above,
                                };
                                if (!this.eventQueue.Contains(intersectionEvent))
                                    this.eventQueue.Add(intersectionEvent);
                            }
                        }

                        this.sweepLine.Remove(segment);
                    }

                    break;
            }
        }
    }
}
