// <copyright file="ShamosHoeyAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Algorithms.SweepLines;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a type for performing the Shamos-Hoey Algorithm for determining intersection of line strings.
    /// </summary>
    /// <remarks>
    /// The algorithm assumes that the polygon is valid, and coordinates are in the same plane.
    /// </remarks>
    public class ShamosHoeyAlgorithm
    {
        /// <summary>
        /// The event queue.
        /// </summary>
        private PresortedEventQueue eventQueue;

        /// <summary>
        /// The sweep line.
        /// </summary>
        private SweepLine sweepLine;

        /// <summary>
        /// A value indicating whether the result is computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// The result of the algorithm.
        /// </summary>
        private Boolean result;

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets a value indicating whether gets the result.
        /// </summary>
        /// <value><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</value>
        public Boolean Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();
                return this.result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.eventQueue = new PresortedEventQueue(source);
            this.sweepLine = new SweepLine(source, precisionModel);
            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShamosHoeyAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ShamosHoeyAlgorithm(IEnumerable<IEnumerable<Coordinate>> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.eventQueue = new PresortedEventQueue(source);
            this.sweepLine = new SweepLine(source, precisionModel);
            this.hasResult = false;
        }

        /// <summary>
        /// Computes whether one or more line strings specified by coordinates intersects with each other.
        /// </summary>
        public void Compute()
        {
            EndPointEvent e = this.eventQueue.Next();
            SweepLineSegment segment;

            while (e != null)
            {
                switch (e.Type)
                {
                    case EventType.Left:
                        segment = this.sweepLine.Add(e);
                        if (segment.Above != null && this.sweepLine.IsAdjacent(segment.Edge, segment.Above.Edge) && segment.LeftCoordinate == segment.Above.LeftCoordinate)
                            this.sweepLine.Intersect(segment, segment.Above);
                        else if (segment.Below != null && this.sweepLine.IsAdjacent(segment.Edge, segment.Below.Edge) && segment.LeftCoordinate == segment.Below.LeftCoordinate)
                            this.sweepLine.Intersect(segment, segment.Below);

                        if (segment.Above != null && !this.sweepLine.IsAdjacent(segment.Edge, segment.Above.Edge) &&
                            LineAlgorithms.Intersects(segment.LeftCoordinate, segment.RightCoordinate,
                                                      segment.Above.LeftCoordinate, segment.Above.RightCoordinate,
                                                      this.PrecisionModel))
                        {
                            this.hasResult = true;
                            this.result = true;
                            return;
                        }

                        if (segment.Below != null && !this.sweepLine.IsAdjacent(segment.Edge, segment.Below.Edge) &&
                            LineAlgorithms.Intersects(segment.LeftCoordinate, segment.RightCoordinate,
                                                      segment.Below.LeftCoordinate, segment.Below.RightCoordinate,
                                                      this.PrecisionModel))
                        {
                            this.hasResult = true;
                            this.result = true;
                            return;
                        }

                        break;
                    case EventType.Right:
                        segment = this.sweepLine.Search(e);
                        if (segment != null)
                        {
                            if (segment.Above != null && segment.Below != null && !this.sweepLine.IsAdjacent(segment.Below.Edge, segment.Above.Edge) &&
                                LineAlgorithms.Intersects(segment.Above.LeftCoordinate, segment.Above.RightCoordinate,
                                                          segment.Below.LeftCoordinate, segment.Below.RightCoordinate,
                                                          this.PrecisionModel))
                            {
                                this.hasResult = true;
                                this.result = true;
                                return;
                            }

                            this.sweepLine.Remove(segment);
                        }

                        break;
                }

                e = this.eventQueue.Next();
            }

            this.hasResult = true;
            this.result = false;
        }

        /// <summary>
        /// Determines whether a line string specified by coordinates intersects with itself.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IEnumerable<Coordinate> source)
        {
            return new ShamosHoeyAlgorithm(source, null).Result;
        }

        /// <summary>
        /// Determines whether a line string specified by coordinates intersects with itself.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            return new ShamosHoeyAlgorithm(source, precisionModel).Result;
        }

        /// <summary>
        /// Determines whether line strings specified by coordinates intersect.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IEnumerable<IEnumerable<Coordinate>> source)
        {
            return new ShamosHoeyAlgorithm(source, null).Result;
        }

        /// <summary>
        /// Determines whether line strings specified by coordinates intersect.
        /// </summary>
        /// <param name="source">The collection of coordinates representing multiple line strings.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the specified line strings intersect; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static Boolean Intersects(IEnumerable<IEnumerable<Coordinate>> source, PrecisionModel precisionModel)
        {
            return new ShamosHoeyAlgorithm(source, precisionModel).Result;
        }
    }
}
