// <copyright file="GreinerHormannAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.ObjectModel;
    using System.Linq;
    using ELTE.AEGIS.Collections;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents the Greiner-Hormann clipping algorithm for determining internal and external polygon segments of two subject polygons.
    /// </summary>
    /// <remarks>
    /// The algorithm is capable of clipping arbitrary polygons, including concave polygons and holes, however the self-intersection of polygons are not supported.
    /// The Greiner-Hormann algorithm is based on the Weiler-Atherton algorithm, but with a different implementation approach.
    /// The algorithm was extended to handle degenerate cases without vertex perturbation based on the proposal of Dae Hyun Kim and Myoung-Jun Kim [1].
    /// Degenerate cases consists of polygons with joint edges and polygons touching but not intersecting each other.
    /// The case of self-intersecting polygons might be implemented based on the ideas of Anurag Chakraborty [2].
    /// References:
    /// [1] D. H. Kim, M-J. Kim, An Extension of Polygon Clipping To Resolve Degenerate Cases, 2006
    /// [2] A. Chakraborty, An Extension Of Weiler-Atherton Algorithm To Cope With The Self-intersecting Polygon, 2014
    /// </remarks>
    public class GreinerHormannAlgorithm
    {
        #region Private types

        /// <summary>
        /// Defines the kinds of the intersection points.
        /// </summary>
        private enum IntersectionMode
        {
            /// <summary>
            /// Indicates that the position is not a real intersection point.
            /// </summary>
            None,

            /// <summary>
            /// Indicates that the intersection is an entry point.
            /// </summary>
            Entry,

            /// <summary>
            /// Indicates that the intersection is an exit point.
            /// </summary>
            Exit,

            /// <summary>
            /// Indicates that the intersection is a boundary point.
            /// </summary>
            /// <remarks>
            /// Boundary intersections are entry-exit or exit-entry intersections at a single position.
            /// </remarks>
            Boundary,
        }

        /// <summary>
        /// Represents the descriptor of an intersection point.
        /// </summary>
        private class Intersection
        {
            #region Public properties

            /// <summary>
            /// Gets or sets the position of the intersection.
            /// </summary>
            /// <value>The position.</value>
            public Coordinate Position { get; set; }

            /// <summary>
            /// Gets or sets the link to corresponding node for the first subject polygon.
            /// </summary>
            public LinkedListNode<Coordinate> NodeA { get; set; }

            /// <summary>
            /// Gets or sets the link to corresponding node for the second subject polygon.
            /// </summary>
            public LinkedListNode<Coordinate> NodeB { get; set; }

            /// <summary>
            /// Gets or sets the kind of the intersection.
            /// </summary>
            public IntersectionMode Mode { get; set; }

            /// <summary>
            /// Gets or sets the next intersection in the first subject polygon.
            /// </summary>
            public Intersection NextA { get; set; }

            /// <summary>
            /// Gets or sets the next intersection in the second subject polygon.
            /// </summary>
            public Intersection NextB { get; set; }

            #endregion
        }

        /// <summary>
        /// Represents a collection of intersection elements indexed by their positions.
        /// </summary>
        private class IntersectionCollection : KeyedCollection<Coordinate, Intersection>
        {
            #region KeyedCollection methods

            /// <summary>
            /// Extracts the key from the specified element.
            /// </summary>
            /// <returns>
            /// The key for the specified element.
            /// </returns>
            /// <param name="item">The element from which to extract the key.</param>
            protected override Coordinate GetKeyForItem(Intersection item)
            {
                return item.Position;
            }

            #endregion
        }

        /// <summary>
        /// Represents a polygon clip.
        /// </summary>
        private class PolygonClip
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="PolygonClip" /> class.
            /// </summary>
            /// <param name="shell">The shell.</param>
            public PolygonClip(IReadOnlyList<Coordinate> shell)
            {
                this.Shell = shell;
                this.Holes = new List<IReadOnlyList<Coordinate>>();
            }

            #endregion

            #region Public properties

            /// <summary>
            /// Gets the shell of the clip.
            /// </summary>
            public IReadOnlyList<Coordinate> Shell { get; private set; }

            /// <summary>
            /// Gets the holes of the clip.
            /// </summary>
            public List<IReadOnlyList<Coordinate>> Holes { get; private set; }

            #endregion

            #region Public methods

            /// <summary>
            /// Adds a new hole to the polygon clip.
            /// </summary>
            /// <param name="hole">The hole.</param>
            /// <remarks>
            /// The method will reverse the orientation of the hole when necessary.
            /// </remarks>
            public void AddHole(IReadOnlyList<Coordinate> hole)
            {
                if (PolygonAlgorithms.Orientation(hole) == Orientation.Counterclockwise)
                {
                    hole = hole.Reverse();
                }

                this.Holes.Add(hole);
            }

            /// <summary>
            /// Returns the <see cref="PolygonClip" /> as a <see cref="IBasicPolygon" />.
            /// </summary>
            /// <returns>The converted result polygon.</returns>
            public IBasicPolygon ToPolygon()
            {
                return new BasicProxyPolygon(this.Shell, this.Holes);
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The first polygon.
        /// </summary>
        private readonly IBasicPolygon polygonA;

        /// <summary>
        /// The second polygon.
        /// </summary>
        private readonly IBasicPolygon polygonB;

        /// <summary>
        /// The shell of the first polygon as a linked list.
        /// </summary>
        private LinkedList<Coordinate> listA;

        /// <summary>
        /// The shell of the second polygon as a linked list.
        /// </summary>
        private LinkedList<Coordinate> listB;

        /// <summary>
        /// The array containing the holes of the first polygon.
        /// </summary>
        /// <remarks>
        /// The holes are reversed into counterclockwise orientation in this list.
        /// </remarks>
        private List<IBasicLineString> holesA;

        /// <summary>
        /// The array containing the holes of the second polygon.
        /// </summary>
        /// <remarks>
        /// The holes are reversed into counterclockwise orientation in this list.
        /// </remarks>
        private List<IBasicLineString> holesB;

        /// <summary>
        /// The intersection collection.
        /// </summary>
        private IntersectionCollection intersections;

        /// <summary>
        /// The list of internal clips.
        /// </summary>
        private List<PolygonClip> internalClips;

        /// <summary>
        /// The list of internal polygons.
        /// </summary>
        private List<IBasicPolygon> internalPolygons;

        /// <summary>
        /// The list of external clips for the first polygon.
        /// </summary>
        private List<PolygonClip> externalClipsA;

        /// <summary>
        /// The list of external polygons for the first polygon.
        /// </summary>
        private List<IBasicPolygon> externalPolygonsA;

        /// <summary>
        /// The list of external clips for the second polygon.
        /// </summary>
        private List<PolygonClip> externalClipsB;

        /// <summary>
        /// The list of external polygons for the second polygon.
        /// </summary>
        private List<IBasicPolygon> externalPolygonsB;

        /// <summary>
        /// A value indicating whether the algorithm has computed the result.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// A value indicating whether to compute the external clips.
        /// </summary>
        private Boolean computeExternalClips;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GreinerHormannAlgorithm" /> class.
        /// </summary>
        /// <param name="first">The first polygon.</param>
        /// <param name="second">The second polygon.</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public GreinerHormannAlgorithm(IBasicPolygon first, IBasicPolygon second, Boolean computeExternalClips, PrecisionModel precisionModel)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstPolygonIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondPolygonIsNull);

            this.polygonA = first;
            this.polygonB = second;
            this.computeExternalClips = computeExternalClips;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreinerHormannAlgorithm" /> class.
        /// </summary>
        /// <param name="first">The shell of the first polygon.</param>
        /// <param name="second">The shell of the second polygon.</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public GreinerHormannAlgorithm(IReadOnlyList<Coordinate> first, IReadOnlyList<Coordinate> second, Boolean computeExternalClips, PrecisionModel precisionModel)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstPolygonIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondPolygonIsNull);

            this.polygonA = new BasicProxyPolygon(first);
            this.polygonB = new BasicProxyPolygon(second);
            this.computeExternalClips = computeExternalClips;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreinerHormannAlgorithm" /> class.
        /// </summary>
        /// <param name="firstShell">The shell of the first polygon (in counter-clockwise order).</param>
        /// <param name="firstHoles">The holes in the first polygon (in clockwise order).</param>
        /// <param name="secondShell">The shell of the second polygon (in counter-clockwise order).</param>
        /// <param name="secondHoles">The holes in the second polygon (in clockwise order).</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.</exception>
        public GreinerHormannAlgorithm(IReadOnlyList<Coordinate> firstShell, IEnumerable<IReadOnlyList<Coordinate>> firstHoles,
                                       IReadOnlyList<Coordinate> secondShell, IEnumerable<IReadOnlyList<Coordinate>> secondHoles,
                                       Boolean computeExternalClips, PrecisionModel precisionModel)
        {
            if (firstShell == null)
                throw new ArgumentNullException(nameof(firstShell), Messages.FirstPolygonIsNull);
            if (secondShell == null)
                throw new ArgumentNullException(nameof(secondShell), Messages.SecondPolygonIsNull);

            this.polygonA = new BasicProxyPolygon(firstShell, firstHoles);
            this.polygonB = new BasicProxyPolygon(secondShell, secondHoles);
            this.computeExternalClips = computeExternalClips;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.hasResult = false;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the first polygon.
        /// </summary>
        /// <value>The first polygon.</value>
        public IBasicPolygon FirstPolygon { get { return this.polygonA; } }

        /// <summary>
        /// Gets the second polygon.
        /// </summary>
        /// <value>The second polygon.</value>
        public IBasicPolygon SecondPolygon { get { return this.polygonB; } }

        /// <summary>
        /// Gets or sets a value indicating whether a value indicating whether to compute the external clips of the polygons.
        /// </summary>
        /// <value><c>true</c> if the algorithm should compute the external clips of the polygons, otherwise <c>false</c>.</value>
        public Boolean ComputeExternalClips
        {
            get
            {
                return this.computeExternalClips;
            }

            set
            {
                if (this.computeExternalClips != value)
                {
                    this.computeExternalClips = value;

                    if (this.computeExternalClips)
                    {
                        this.hasResult = false;
                    }
                    else
                    {
                        this.externalClipsA = null;
                        this.externalClipsB = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the internal clips.
        /// </summary>
        /// <value>The list of polygons representing the internal clips of the two subject polygons.</value>
        public IReadOnlyList<IBasicPolygon> InternalPolygons
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();

                if (this.internalPolygons == null)
                    this.internalPolygons = this.internalClips.Select(clip => clip.ToPolygon()).ToList();

                return this.internalPolygons;
            }
        }

        /// <summary>
        /// Gets the external clips for the first polygon.
        /// </summary>
        /// <value>The list of polygons representing the external clips of the first subject polygon.</value>
        public IReadOnlyList<IBasicPolygon> ExternalFirstPolygons
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();

                if (this.externalPolygonsA == null)
                    this.externalPolygonsA = this.externalClipsA.Select(clip => clip.ToPolygon()).ToList();

                return this.externalPolygonsA;
            }
        }

        /// <summary>
        /// Gets the external clips for the second polygon.
        /// </summary>
        /// <value>The list of polygons representing the external clips of the second subject polygon.</value>
        public IReadOnlyList<IBasicPolygon> ExternalSecondPolygons
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();

                if (this.externalPolygonsB == null)
                    this.externalPolygonsB = this.externalClipsB.Select(clip => clip.ToPolygon()).ToList();

                return this.externalPolygonsB;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the result of the algorithm.
        /// </summary>
        public void Compute()
        {
            this.Initialize(); // initialize the algorithm
            this.FindIntersections(); // determine the intersections of the polygon shells
            this.CategorizeIntersections(); // categorize the type of the intersections
            this.LinkIntersections(); // link the intersections to the succeeding ones
            this.AddHoleIntersections(); // add the vertexes of hole intersections to the polygon shells

            // intersection point exist
            if (this.intersections.Any(intersection => intersection.Mode == IntersectionMode.Entry))
            {
                this.ComputeInternalClips();

                if (this.computeExternalClips)
                {
                    this.ComputeExternalClipsA();
                    this.ComputeExternalClipsB();
                }

                this.intersections.Clear();
            }
            else
            {
                // no intersection point found
                this.ComputeCompleteClips();
            }

            // holes exist
            if (this.holesA.Count > 0 || this.holesB.Count > 0)
            {
                // compute the intersection of the holes in the subject polygons, holes of the internal clips are added to the polygons through the process
                this.ComputeHoles();

                // resolves hole degeneracies in the external clips, adds external holes to the appropriate polygons
                if (this.computeExternalClips)
                {
                    this.ComputeExternalHoles(this.externalClipsA, this.holesA);
                    this.ComputeExternalHoles(this.externalClipsB, this.holesB);
                }
            }

            this.hasResult = true;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="first">The first polygon.</param>
        /// <param name="second">The second polygon.</param>
        /// <returns>The collection of polygon clips.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public static IEnumerable<IBasicPolygon> Clip(IBasicPolygon first, IBasicPolygon second)
        {
            return new GreinerHormannAlgorithm(first, second, false, null).InternalPolygons;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="first">The first polygon.</param>
        /// <param name="second">The second polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The collection of polygon clips.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public static IEnumerable<IBasicPolygon> Clip(IBasicPolygon first, IBasicPolygon second, PrecisionModel precisionModel)
        {
            return new GreinerHormannAlgorithm(first, second, false, precisionModel).InternalPolygons;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="first">The shell of the first polygon.</param>
        /// <param name="second">The shell of the second polygon.</param>
        /// <returns>The collection of polygon clips.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public static IEnumerable<IBasicPolygon> Clip(IReadOnlyList<Coordinate> first, IReadOnlyList<Coordinate> second)
        {
            return new GreinerHormannAlgorithm(first, second, false, null).InternalPolygons;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="first">The shell of the first polygon.</param>
        /// <param name="second">The shell of the second polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The collection of polygon clips.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public static IEnumerable<IBasicPolygon> Clip(IReadOnlyList<Coordinate> first, IReadOnlyList<Coordinate> second, PrecisionModel precisionModel)
        {
            return new GreinerHormannAlgorithm(first, second, false, precisionModel).InternalPolygons;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="firstShell">The shell of the first polygon (in counter-clockwise order).</param>
        /// <param name="firstHoles">The holes in the first polygon (in clockwise order).</param>
        /// <param name="secondShell">The shell of the second polygon (in counter-clockwise order).</param>
        /// <param name="secondHoles">The holes in the second polygon (in clockwise order).</param>
        /// <returns>The collection of polygon clips.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public static IEnumerable<IBasicPolygon> Clip(IReadOnlyList<Coordinate> firstShell, IEnumerable<IReadOnlyList<Coordinate>> firstHoles, IReadOnlyList<Coordinate> secondShell, IEnumerable<IReadOnlyList<Coordinate>> secondHoles)
        {
            return new GreinerHormannAlgorithm(firstShell, firstHoles, secondShell, secondHoles, false, null).InternalPolygons;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="firstShell">The shell of the first polygon (in counter-clockwise order).</param>
        /// <param name="firstHoles">The holes in the first polygon (in clockwise order).</param>
        /// <param name="secondShell">The shell of the second polygon (in counter-clockwise order).</param>
        /// <param name="secondHoles">The holes in the second polygon (in clockwise order).</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The collection of polygon clips.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first polygon is invalid.
        /// or
        /// The second polygon is invalid.
        /// </exception>
        public static IEnumerable<IBasicPolygon> Clip(IReadOnlyList<Coordinate> firstShell, IEnumerable<IReadOnlyList<Coordinate>> firstHoles, IReadOnlyList<Coordinate> secondShell, IEnumerable<IReadOnlyList<Coordinate>> secondHoles, PrecisionModel precisionModel)
        {
            return new GreinerHormannAlgorithm(firstShell, firstHoles, secondShell, secondHoles, false, precisionModel).InternalPolygons;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the computation.
        /// </summary>
        private void Initialize()
        {
            // initialize results
            this.intersections = new IntersectionCollection();
            this.internalClips = new List<PolygonClip>();
            this.externalClipsA = new List<PolygonClip>();
            this.externalClipsB = new List<PolygonClip>();

            // initialize lists
            this.listA = new LinkedList<Coordinate>(this.polygonA.Shell.Elements());
            this.listA.RemoveLast();
            this.listB = new LinkedList<Coordinate>(this.polygonB.Shell.Elements());
            this.listB.RemoveLast();

            // initialize holes
            this.holesA = new List<IBasicLineString>(this.polygonA.Holes.Select(hole => new BasicProxyLineString(hole.Reverse())));
            this.holesB = new List<IBasicLineString>(this.polygonB.Holes.Select(hole => new BasicProxyLineString(hole.Reverse())));
        }

        /// <summary>
        /// Finds the intersections of the polygon shells.
        /// </summary>
        private void FindIntersections()
        {
            if (Envelope.Disjoint(this.polygonA.Shell, this.polygonB.Shell))
                return;

            BentleyOttmannAlgorithm algorithm = new BentleyOttmannAlgorithm(new IReadOnlyList<Coordinate>[] { this.polygonA.Shell, this.polygonB.Shell }, this.PrecisionModel);
            IReadOnlyList<Coordinate> positions = algorithm.Intersections;
            IReadOnlyList<Tuple<Int32, Int32>> edges = algorithm.EdgeIndexes;

            for (Int32 posIndex = 0; posIndex < positions.Count; ++posIndex)
            {
                if (this.intersections.Contains(positions[posIndex]))
                    continue;

                // process the first polygon
                LinkedListNode<Coordinate> nodeA, nodeB;
                if (!this.polygonA.Shell.Contains(positions[posIndex]))
                {
                    // insert intersection point into vertex lists
                    LinkedListNode<Coordinate> location = this.listA.Find(this.polygonA.Shell[edges[posIndex].Item1]);

                    // find the proper position for the intersection point when multiple intersection occurs on a single edge
                    while (location.Next != null && this.intersections.Contains(location.Next.Value) && (positions[posIndex] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;

                    nodeA = this.listA.AddAfter(location, positions[posIndex]);
                }
                else
                {
                    nodeA = this.listA.Find(positions[posIndex]);
                }

                // process the second polygon
                if (!this.polygonB.Shell.Contains(positions[posIndex]))
                {
                    LinkedListNode<Coordinate> location = this.listB.Find(this.polygonB.Shell[edges[posIndex].Item2 - this.polygonA.Shell.Count]);

                    while (location.Next != null && this.intersections.Contains(location.Next.Value) && (positions[posIndex] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;

                    nodeB = this.listB.AddAfter(location, positions[posIndex]);
                }
                else
                {
                    nodeB = this.listB.Find(positions[posIndex]);
                }

                this.intersections.Add(new Intersection { Position = positions[posIndex], NodeA = nodeA, NodeB = nodeB });
            }
        }

        /// <summary>
        /// Finds the intersections of the subject polygons.
        /// </summary>
        private void CategorizeIntersections()
        {
            // If intersection points exist.
            if (this.intersections.Count == 0)
                return;

            // Entering / Exiting intersection point separation (from A to B).
            LinkedListNode<Coordinate> currentNode = this.listA.First;
            while (currentNode != null)
            {
                if (this.intersections.Contains(currentNode.Value))
                {
                    LinkedListNode<Coordinate> prevNode = currentNode.Previous ?? this.listA.Last;
                    LinkedListNode<Coordinate> nextNode = currentNode.Next ?? this.listA.First;

                    Coordinate prevEdge = LineAlgorithms.Centroid(currentNode.Value, prevNode.Value, this.PrecisionModel);
                    Coordinate nextEdge = LineAlgorithms.Centroid(currentNode.Value, nextNode.Value, this.PrecisionModel);

                    RelativeLocation prevLocation = WindingNumberAlgorithm.Location(this.polygonB.Shell, prevEdge, this.PrecisionModel);
                    RelativeLocation nextLocation = WindingNumberAlgorithm.Location(this.polygonB.Shell, nextEdge, this.PrecisionModel);

                    // Entry
                    if ((prevLocation == RelativeLocation.Exterior && nextLocation == RelativeLocation.Interior) ||
                        (prevLocation == RelativeLocation.Exterior && nextLocation == RelativeLocation.Boundary) ||
                        (prevLocation == RelativeLocation.Boundary && nextLocation == RelativeLocation.Interior))
                    {
                        this.intersections[currentNode.Value].Mode = IntersectionMode.Entry;
                    }

                    // Exit
                    else if ((prevLocation == RelativeLocation.Interior && nextLocation == RelativeLocation.Exterior) ||
                             (prevLocation == RelativeLocation.Boundary && nextLocation == RelativeLocation.Exterior) ||
                             (prevLocation == RelativeLocation.Interior && nextLocation == RelativeLocation.Boundary))
                    {
                        this.intersections[currentNode.Value].Mode = IntersectionMode.Exit;
                    }

                    // Entry + Exit / Exit + Entry
                    else if ((prevLocation == RelativeLocation.Interior && nextLocation == RelativeLocation.Interior) ||
                             (prevLocation == RelativeLocation.Exterior && nextLocation == RelativeLocation.Exterior))
                    {
                        this.intersections[currentNode.Value].Mode = IntersectionMode.Boundary;
                    }

                    // Boundary -> Boundary
                    else
                    {
                        this.intersections.Remove(currentNode.Value);
                    }
                }

                currentNode = currentNode.Next;
            }
        }

        /// <summary>
        /// Links the intersections to each other in a succeeding order.
        /// </summary>
        private void LinkIntersections()
        {
            if (this.intersections.Count == 0)
                return;

            // Process the first polygon.
            Intersection firstIntersection = null;
            Intersection lastIntersection = null;
            LinkedListNode<Coordinate> currentNode = this.listA.First;

            while (currentNode != null)
            {
                if (this.intersections.Contains(currentNode.Value))
                {
                    if (lastIntersection == null)
                        firstIntersection = this.intersections[currentNode.Value];
                    else
                        lastIntersection.NextA = this.intersections[currentNode.Value];
                    lastIntersection = this.intersections[currentNode.Value];
                }

                currentNode = currentNode.Next;
            }

            lastIntersection.NextA = firstIntersection;

            // Process the second polygon.
            firstIntersection = null;
            lastIntersection = null;
            currentNode = this.listB.First;

            while (currentNode != null)
            {
                if (this.intersections.Contains(currentNode.Value))
                {
                    if (lastIntersection == null)
                        firstIntersection = this.intersections[currentNode.Value];
                    else
                        lastIntersection.NextB = this.intersections[currentNode.Value];
                    lastIntersection = this.intersections[currentNode.Value];
                }

                currentNode = currentNode.Next;
            }

            lastIntersection.NextB = firstIntersection;
        }

        /// <summary>
        /// Adds the intersection points of the holes and polygon shells to the linked lists containing the polygons' vertexes.
        /// </summary>
        private void AddHoleIntersections()
        {
            foreach (IBasicLineString holeB in this.holesB)
                this.AddHoleIntersections(this.listA, holeB);

            foreach (IBasicLineString holeA in this.holesA)
                this.AddHoleIntersections(this.listB, holeA);
        }

        /// <summary>
        /// Adds intersection points of a polygon shell and a hole to the linked list containing the polygon's vertexes.
        /// </summary>
        /// <param name="polygon">The linked list of the polygon shell.</param>
        /// <param name="hole">The hole.</param>
        private void AddHoleIntersections(LinkedList<Coordinate> polygon, IEnumerable<Coordinate> hole)
        {
            if (Envelope.Disjoint(polygon, hole))
                return;

            IList<Coordinate> shell = new List<Coordinate>(polygon);
            if (shell.Count > 0)
                shell.Add(polygon.First.Value);

            BentleyOttmannAlgorithm algorithm = new BentleyOttmannAlgorithm(new List<IEnumerable<Coordinate>> { shell, hole }, this.PrecisionModel);
            IReadOnlyList<Coordinate> positions = algorithm.Intersections;
            IReadOnlyList<Tuple<Int32, Int32>> edges = algorithm.EdgeIndexes;

            for (Int32 posIndex = 0; posIndex < positions.Count; ++posIndex)
            {
                if (polygon.Contains(positions[posIndex]))
                    continue;

                // Insert intersection point into vertex lists
                LinkedListNode<Coordinate> location = polygon.Find(shell[edges[posIndex].Item1]);

                // Find the proper position for the intersection point when multiple intersection occurs on a single edge
                while (location.Next != null && (positions[posIndex] - location.Value).Length > (location.Next.Value - location.Value).Length)
                    location = location.Next;

                polygon.AddAfter(location, positions[posIndex]);
            }
        }

        /// <summary>
        /// Compute the internal clips.
        /// </summary>
        private void ComputeInternalClips()
        {
            // start form the entry points, last ones in a queue and follow the first subject polygon
            List<Coordinate> startPositions = this.intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry && intersection.Mode != intersection.NextA.Mode).Select(intersection => intersection.Position).ToList();

            while (startPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = this.intersections[startPositions[0]].NodeA;
                Boolean isFollowingA = true;

                shell.Add(current.Value);
                startPositions.Remove(current.Value);

                do
                {
                    current = current.Next ?? current.List.First;
                    shell.Add(current.Value);

                    // trace and remove inner loops
                    Int32 match = shell.FindLastIndex(shell.Count - 2, shell.Count - 2, coordinate => coordinate == current.Value);
                    if (match >= 0)
                    {
                        IReadOnlyList<Coordinate> subResult = Collection.GetRange(shell, match, shell.Count - match);
                        if (subResult.Count > 3)
                            this.internalClips.Add(new PolygonClip(subResult));
                        shell.RemoveRange(match, shell.Count - match - 1);
                    }

                    if (this.intersections.Contains(current.Value))
                    {
                        if (this.intersections[current.Value].Mode == IntersectionMode.Entry)
                            isFollowingA = true;
                        else if (this.intersections[current.Value].Mode == IntersectionMode.Exit)
                            isFollowingA = false;

                        current = isFollowingA ? this.intersections[current.Value].NodeA : this.intersections[current.Value].NodeB;
                        startPositions.Remove(current.Value);
                    }
                }
                while (current.Value != shell[0]);

                if (shell.Count > 3)
                    this.internalClips.Add(new PolygonClip(shell));
            }
        }

        /// <summary>
        /// Compute the external clips for the first polygon.
        /// </summary>
        private void ComputeExternalClipsA()
        {
            // start form the exit points, last ones in a queue and follow the first subject polygon
            List<Coordinate> startPositions = this.intersections.Where(intersection => intersection.Mode == IntersectionMode.Exit && intersection.Mode != intersection.NextA.Mode).Select(intersection => intersection.Position).ToList();

            while (startPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = this.intersections[startPositions[0]].NodeA;
                Boolean isFollowingA = true;

                shell.Add(current.Value);
                startPositions.Remove(current.Value);

                do
                {
                    if (isFollowingA)
                        current = current.Next ?? current.List.First;
                    else
                        current = current.Previous ?? current.List.Last;
                    shell.Add(current.Value);

                    // trace and remove inner loops
                    Int32 match = shell.FindLastIndex(shell.Count - 2, shell.Count - 2, coordinate => coordinate == current.Value);
                    if (match >= 0)
                    {
                        IReadOnlyList<Coordinate> subResult = Collection.GetRange(shell, match, shell.Count - match);
                        if (subResult.Count > 3)
                            this.internalClips.Add(new PolygonClip(subResult));
                        shell.RemoveRange(match, shell.Count - match - 1);
                    }

                    if (this.intersections.Contains(current.Value))
                    {
                        if (this.intersections[current.Value].Mode == IntersectionMode.Entry)
                            isFollowingA = false;
                        else if (this.intersections[current.Value].Mode == IntersectionMode.Exit)
                            isFollowingA = true;

                        current = isFollowingA ? this.intersections[current.Value].NodeA : this.intersections[current.Value].NodeB;
                        startPositions.Remove(current.Value);
                    }
                }
                while (current.Value != shell[0]);

                if (shell.Count > 3)
                    this.externalClipsA.Add(new PolygonClip(shell));
            }
        }

        /// <summary>
        /// Compute the external clips for the second polygon.
        /// </summary>
        private void ComputeExternalClipsB()
        {
            // start form the entry points, last ones in a queue and follow the second subject polygon
            List<Coordinate> checkPositions = this.intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry && intersection.Mode != intersection.NextB.Mode).Select(intersection => intersection.Position).ToList();

            while (checkPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = this.intersections[checkPositions[0]].NodeB;
                Boolean isFollowingA = false;

                shell.Add(current.Value);
                checkPositions.Remove(current.Value);

                do
                {
                    if (!isFollowingA)
                        current = current.Next ?? current.List.First;
                    else
                        current = current.Previous ?? current.List.Last;
                    shell.Add(current.Value);

                    // trace and remove inner loops
                    Int32 match = shell.FindLastIndex(shell.Count - 2, shell.Count - 2, coordinate => coordinate == current.Value);
                    if (match >= 0)
                    {
                        IReadOnlyList<Coordinate> subResult = Collection.GetRange(shell, match, shell.Count - match);
                        if (subResult.Count > 3)
                            this.internalClips.Add(new PolygonClip(subResult));
                        shell.RemoveRange(match, shell.Count - match - 1);
                    }

                    if (this.intersections.Contains(current.Value))
                    {
                        if (this.intersections[current.Value].Mode == IntersectionMode.Entry)
                            isFollowingA = false;
                        else if (this.intersections[current.Value].Mode == IntersectionMode.Exit)
                            isFollowingA = true;

                        current = isFollowingA ? this.intersections[current.Value].NodeA : this.intersections[current.Value].NodeB;
                        checkPositions.Remove(current.Value);
                    }
                }
                while (current.Value != shell[0]);

                if (shell.Count > 3)
                    this.externalClipsB.Add(new PolygonClip(shell));
            }
        }

        /// <summary>
        /// Completes complete clips (in case of no intersection).
        /// </summary>
        private void ComputeCompleteClips()
        {
            Boolean isAinB = this.polygonA.Shell.All(position => !WindingNumberAlgorithm.InExterior(this.polygonB.Shell, position, this.PrecisionModel));
            Boolean isBinA = this.polygonB.Shell.All(position => !WindingNumberAlgorithm.InExterior(this.polygonA.Shell, position, this.PrecisionModel));

            List<Coordinate> finalShellA = this.listA.ToList();
            finalShellA.Add(finalShellA[0]);

            List<Coordinate> finalShellB = this.listB.ToList();
            finalShellB.Add(finalShellB[0]);

            PolygonClip finalPolygonA = new PolygonClip(finalShellA);
            PolygonClip finalPolygonB = new PolygonClip(finalShellB);

            if (isAinB && isBinA)
            {
                // A equals B
                this.internalClips.Add(finalPolygonA);
            }
            else if (isAinB)
            {
                // B contains A
                finalPolygonB.AddHole(finalPolygonA.Shell);
                this.internalClips.Add(finalPolygonA);
                this.externalClipsB.Add(finalPolygonB);
            }
            else if (isBinA)
            {
                // A contains B
                finalPolygonA.AddHole(finalPolygonB.Shell);
                this.internalClips.Add(finalPolygonB);
                this.externalClipsA.Add(finalPolygonA);
            }
            else
            {
                // A and B are distinct
                this.externalClipsA.Add(finalPolygonA);
                this.externalClipsB.Add(finalPolygonB);
            }
        }

        /// <summary>
        /// Compute the intersection of the holes in the subject polygons.
        /// </summary>
        /// <remarks>Holes of the internal clips are added to the polygons through the process.</remarks>
        private void ComputeHoles()
        {
            if (Envelope.Disjoint(this.polygonA.Shell, this.polygonB.Shell))
                return;

            List<PolygonClip> externalB = new List<PolygonClip>();

            // intersect holes in the first polygon with the internal shells
            if (this.holesA.Count > 0)
            {
                List<PolygonClip> processedPolygons = new List<PolygonClip>();
                while (this.internalClips.Count > 0)
                {
                    Boolean intersected = false;
                    for (Int32 i = 0; i < this.holesA.Count; ++i)
                    {
                        // internal parts cannot have holes inside them at this point
                        GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(this.holesA[i], this.internalClips[0].Shell, true, this.PrecisionModel);
                        algorithm.Compute();
                        if (algorithm.internalClips.Any())
                        {
                            intersected = true;

                            externalB.AddRange(algorithm.internalClips);
                            this.holesA.AddRange(algorithm.externalClipsA.Select(polygon => new BasicProxyLineString(polygon.Shell)));
                            this.internalClips.AddRange(algorithm.externalClipsB);

                            this.holesA.RemoveAt(i);
                            break;
                        }
                    }

                    if (!intersected)
                        processedPolygons.Add(this.internalClips[0]);
                    this.internalClips.RemoveAt(0);
                }

                this.internalClips = processedPolygons;
            }

            // intersect holes in the second polygon with the internal polygon
            if (this.holesB.Count > 0)
            {
                List<PolygonClip> processedPolygons = new List<PolygonClip>();
                while (this.internalClips.Count > 0)
                {
                    Boolean intersected = false;
                    for (Int32 i = 0; i < this.holesB.Count; ++i)
                    {
                        GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(this.internalClips[0].Shell, this.internalClips[0].Holes, this.holesB[i], null, true, this.PrecisionModel);
                        algorithm.Compute();
                        if (algorithm.internalClips.Any())
                        {
                            intersected = true;

                            this.externalClipsA.AddRange(algorithm.internalClips);
                            this.internalClips.AddRange(algorithm.externalClipsA);
                            this.holesB.AddRange(algorithm.externalClipsB.Select(polygon => new BasicProxyLineString(polygon.Shell)));

                            this.holesB.RemoveAt(i);
                            break;
                        }
                    }

                    if (!intersected)
                        processedPolygons.Add(this.internalClips[0]);
                    this.internalClips.RemoveAt(0);
                }

                this.internalClips = processedPolygons;
            }

            while (externalB.Count > 0)
            {
                Boolean intersected = false;
                for (Int32 i = 0; i < this.holesB.Count; ++i)
                {
                    GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(externalB[0].Shell, this.holesB[i], true, this.PrecisionModel);
                    algorithm.Compute();
                    if (algorithm.internalClips.Count > 0)
                    {
                        intersected = true;

                        // clips in algorithm.internalClips are intersection of the already existing holes in the internals, therefore they are already covered
                        this.externalClipsB.AddRange(algorithm.externalClipsA);
                        this.holesB.AddRange(algorithm.externalClipsB.Select(polygon => new BasicProxyLineString(polygon.Shell)));

                        this.holesB.RemoveAt(i);
                        break;
                    }
                }

                if (!intersected)
                    this.externalClipsB.Add(externalB[0]);
                externalB.RemoveAt(0);
            }
        }

        /// <summary>
        /// Resolves hole degeneracies in the external clips.
        /// Adds external holes to the appropriate polygons.
        /// </summary>
        /// <remarks>Holes touching boundary of their shells are degenerate.</remarks>
        /// <param name="polygons">Polygons (without holes) to process.</param>
        /// <param name="holes">Possibly degenerate holes to process and locate.</param>
        private void ComputeExternalHoles(List<PolygonClip> polygons, List<IBasicLineString> holes)
        {
            List<IBasicLineString> processedHoles = new List<IBasicLineString>();
            while (holes.Count > 0)
            {
                Boolean intersected = false;
                for (Int32 i = 0; i < polygons.Count; ++i)
                {
                    if (ShamosHoeyAlgorithm.Intersects(new[] { polygons[i].Shell, holes[0] }, this.PrecisionModel))
                    {
                        GreinerHormannAlgorithm clipping = new GreinerHormannAlgorithm(polygons[i].Shell, holes[0], true, this.PrecisionModel);
                        clipping.Compute();

                        polygons.AddRange(clipping.externalClipsA);
                        polygons.RemoveAt(i);

                        intersected = true;
                        break;
                    }
                }

                if (!intersected)
                    processedHoles.Add(holes[0]);
                holes.RemoveAt(0);
            }

            holes.AddRange(processedHoles);

            foreach (IBasicLineString hole in holes)
            {
                PolygonClip containerClip = polygons.First(clip => hole.All(coordinate => !WindingNumberAlgorithm.InExterior(clip.Shell, coordinate, this.PrecisionModel)));
                containerClip.AddHole(hole);
            }
        }

        #endregion
    }
}
