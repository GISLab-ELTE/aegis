// <copyright file="SweepLine.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2017 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Algorithms.SweepLines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Algorithms;
    using ELTE.AEGIS.Collections;
    using ELTE.AEGIS.Collections.SearchTree;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a sweep line.
    /// </summary>
    public sealed class SweepLine
    {
        /// <summary>
        /// Defines the intersection between two sweep line segments.
        /// </summary>
        private enum SweepLineIntersection
        {
            /// <summary>
            /// Indicates that the intersection does not exist.
            /// </summary>
            NotExists,

            /// <summary>
            /// Indicates that the intersection was not passed.
            /// </summary>
            NotPassed,

            /// <summary>
            /// Indicates that the intersection was passed.
            /// </summary>
            Passed
        }

        /// <summary>
        /// Represents a comparer for <see cref="SweepLineSegment" /> instances.
        /// </summary>
        private sealed class SweepLineSegmentComparer : IComparer<SweepLineSegment>
        {
            /// <summary>
            /// Stores the precision model.
            /// </summary>
            private readonly PrecisionModel precisionModel;

            /// <summary>
            /// Stores the intersections that has already been processed at the current sweep line position.
            /// </summary>
            /// <remarks>
            /// Intersection points are registered in the containing set with both possible ordering.
            /// </remarks>
            private readonly HashSet<Tuple<SweepLineSegment, SweepLineSegment>> intersections;

            /// <summary>
            /// Stores the horizontal (X coordinate) position of the sweep line.
            /// </summary>
            private Double sweepLinePosition;

            /// <summary>
            /// Initializes a new instance of the <see cref="SweepLineSegmentComparer" /> class.
            /// </summary>
            /// <param name="precisionModel">The precision model.</param>
            public SweepLineSegmentComparer(PrecisionModel precisionModel)
            {
                this.precisionModel = precisionModel ?? PrecisionModel.Default;
                this.sweepLinePosition = Double.MinValue;
                this.intersections = new HashSet<Tuple<SweepLineSegment, SweepLineSegment>>();
            }

            /// <summary>
            /// Compares two <see cref="SweepLineSegment" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <remarks>
            /// The comparator applies a above-below relationship between the arguments, where a "greater" segment is above the another one.
            /// </remarks>
            /// <param name="first">The first <see cref="SweepLineSegment" /> to compare.</param>
            /// <param name="second">The second <see cref="SweepLineSegment" /> to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
            /// <exception cref="System.InvalidOperationException">The segments do not overlap.</exception>
            public Int32 Compare(SweepLineSegment first, SweepLineSegment second)
            {
                // Comparing non-overlapping segments is not supported.
                if (first.RightCoordinate.X < second.LeftCoordinate.X || first.LeftCoordinate.X > second.RightCoordinate.X)
                    throw new InvalidOperationException(CoreMessages.SegmentsDoNotOverlap);

                // The segments intersect.
                SweepLineIntersection intersection = this.GetIntersection(first, second);
                if (intersection != SweepLineIntersection.NotExists)
                {
                    CoordinateVector xDiff = first.RightCoordinate - first.LeftCoordinate;
                    Double xGradient = xDiff.X == 0 ? Double.MaxValue : xDiff.Y / xDiff.X;

                    CoordinateVector yDiff = second.RightCoordinate - second.LeftCoordinate;
                    Double yGradient = yDiff.X == 0 ? Double.MaxValue : yDiff.Y / yDiff.X;

                    Int32 result = yGradient.CompareTo(xGradient);
                    if (result == 0)
                        result = first.LeftCoordinate.X.CompareTo(second.LeftCoordinate.X);
                    if (result == 0)
                        result = second.LeftCoordinate.Y.CompareTo(first.LeftCoordinate.Y);
                    if (result == 0)
                        result = first.RightCoordinate.X.CompareTo(second.RightCoordinate.X);
                    if (result == 0)
                        result = second.RightCoordinate.Y.CompareTo(first.RightCoordinate.Y);
                    if (result == 0)
                        result = second.Edge.CompareTo(first.Edge);
                    if (intersection == SweepLineIntersection.Passed)
                        result *= -1;
                    return result;
                }

                // the segments do not intersect
                if (first.LeftCoordinate.X < second.LeftCoordinate.X)
                {
                    Double[] verticalCollection = new[] { first.LeftCoordinate.Y, first.RightCoordinate.Y };
                    Coordinate verticalLineStart = new Coordinate(second.LeftCoordinate.X, verticalCollection.Min());
                    Coordinate verticalLineEnd = new Coordinate(second.LeftCoordinate.X, verticalCollection.Max());
                    Intersection startIntersection = LineAlgorithms.Intersection(first.LeftCoordinate, first.RightCoordinate, verticalLineStart, verticalLineEnd, this.precisionModel);

                    // due to precision tolerance degeneracy we might not found the intersection
                    return startIntersection != null ? startIntersection.Coordinate.Y.CompareTo(second.LeftCoordinate.Y) : ((first.LeftCoordinate.Y + first.RightCoordinate.Y) / 2.0).CompareTo(second.LeftCoordinate.Y);
                }

                if (first.LeftCoordinate.X > second.LeftCoordinate.X)
                {
                    Double[] verticalCollection = new[] { second.LeftCoordinate.Y, second.RightCoordinate.Y };
                    Coordinate verticalLineStart = new Coordinate(first.LeftCoordinate.X, verticalCollection.Min());
                    Coordinate verticalLineEnd = new Coordinate(first.LeftCoordinate.X, verticalCollection.Max());
                    Intersection startIntersection = LineAlgorithms.Intersection(verticalLineStart, verticalLineEnd, second.LeftCoordinate, second.RightCoordinate, this.precisionModel);

                    return startIntersection != null ? first.LeftCoordinate.Y.CompareTo(startIntersection.Coordinate.Y) : first.LeftCoordinate.Y.CompareTo((second.LeftCoordinate.Y + second.RightCoordinate.Y) / 2.0);
                }

                // first.LeftCoordinate.X == second.LeftCoordinate.X
                return first.LeftCoordinate.Y.CompareTo(second.LeftCoordinate.Y);
            }

            /// <summary>
            /// Gets the intersection type between the two given sweep line segments.
            /// </summary>
            /// <param name="x">First sweep line segment.</param>
            /// <param name="y">Second sweep line segment.</param>
            /// <returns>The type of intersection that exists between the two sweep line segments, considering the position of the sweep line.</returns>
            public SweepLineIntersection GetIntersection(SweepLineSegment x, SweepLineSegment y)
            {
                Intersection intersection = LineAlgorithms.Intersection(x.LeftCoordinate, x.RightCoordinate, y.LeftCoordinate, y.RightCoordinate, this.precisionModel);

                if (intersection == null || intersection.Type == IntersectionType.None)
                    return SweepLineIntersection.NotExists;

                if (intersection.Coordinate.X < this.sweepLinePosition)
                    return SweepLineIntersection.Passed;

                if (intersection.Coordinate.X > this.sweepLinePosition)
                    return SweepLineIntersection.NotPassed;

                return (this.intersections.Contains(Tuple.Create(x, y)) || this.intersections.Contains(Tuple.Create(y, x))) ? SweepLineIntersection.Passed : SweepLineIntersection.NotPassed;
            }

            /// <summary>
            /// Registers a passed intersection point by the sweep line between the two given arguments.
            /// </summary>
            /// <remarks>
            /// The position of the sweep line is updated when necessary.
            /// </remarks>
            /// <param name="x">First sweep line segment.</param>
            /// <param name="y">Second sweep line segment.</param>
            public void PassIntersection(SweepLineSegment x, SweepLineSegment y)
            {
                Intersection intersection = LineAlgorithms.Intersection(x.LeftCoordinate, x.RightCoordinate, y.LeftCoordinate, y.RightCoordinate, this.precisionModel);

                if (intersection == null)
                    return;

                if (intersection.Coordinate.X > this.sweepLinePosition)
                {
                    this.sweepLinePosition = intersection.Coordinate.X;
                    this.intersections.Clear();
                }

                this.intersections.Add(Tuple.Create(x, y));
                this.intersections.Add(Tuple.Create(y, x));
            }
        }

        /// <summary>
        /// Represents a sweep line tree.
        /// </summary>
        private sealed class SweepLineTree : AvlTree<SweepLineSegment, SweepLineSegment>
        {
            /// <summary>
            /// The currently selected node.
            /// </summary>
            private Node currentNode;

            /// <summary>
            /// Initializes a new instance of the <see cref="SweepLineTree" /> class.
            /// </summary>
            /// <param name="precisionModel">The precision model.</param>
            public SweepLineTree(PrecisionModel precisionModel)
                : base(new SweepLineSegmentComparer(precisionModel))
            {
                this.currentNode = null;
            }

            /// <summary>
            /// Gets the current sweep line segment.
            /// </summary>
            public SweepLineSegment Current { get { return this.currentNode?.Key; } }

            /// <summary>
            /// Inserts the specified segment to the tree.
            /// </summary>
            /// <param name="segment">The segment.</param>
            /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
            public void Insert(SweepLineSegment segment)
            {
                if (segment == null)
                    throw new ArgumentNullException(nameof(segment), CoreMessages.SegmentIsNull);

                if (this.root == null)
                {
                    this.root = new AvlNode { Key = segment, Value = segment, Balance = 0 };
                    this.currentNode = this.root;
                    this.nodeCount++;
                    this.version++;
                    return;
                }

                AvlNode node = this.SearchNodeForInsertion(segment) as AvlNode;
                if (node == null)
                    return;

                if (this.Comparer.Compare(segment, node.Key) < 0)
                {
                    node.LeftChild = new AvlNode { Key = segment, Value = segment, Parent = node, Balance = 0 };
                    node.Balance = -1;
                    this.currentNode = node.LeftChild;
                }
                else
                {
                    node.RightChild = new AvlNode { Key = segment, Value = segment, Parent = node, Balance = 0 };
                    node.Balance = 1;
                    this.currentNode = node.RightChild;
                }

                this.Balance(node);
                this.nodeCount++;
                this.version++;
            }

            /// <summary>
            /// Sets the current segment.
            /// </summary>
            /// <param name="segment">The segment.</param>
            /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
            public void SetCurrent(SweepLineSegment segment)
            {
                if (segment == null)
                    throw new ArgumentNullException(nameof(segment), CoreMessages.SegmentIsNull);

                this.currentNode = this.SearchNode(segment);
            }

            /// <summary>
            /// Gets the previous (below) <see cref="SweepLineSegment" /> for the current segment.
            /// </summary>
            /// <returns>The previous segment.</returns>
            public SweepLineSegment GetPrevious()
            {
                if (this.currentNode == null)
                    return null;

                Node prevNode = this.currentNode;
                if (prevNode.LeftChild != null)
                {
                    prevNode = prevNode.LeftChild;

                    while (prevNode.RightChild != null)
                    {
                        prevNode = prevNode.RightChild;
                    }

                    return prevNode.Key;
                }

                while (prevNode.Parent != null && prevNode.Parent.LeftChild == prevNode)
                {
                    prevNode = prevNode.Parent;
                }

                if (prevNode.Parent != null && prevNode.Parent.RightChild == prevNode)
                {
                    return prevNode.Parent.Key;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Gets the next (above) <see cref="SweepLineSegment" /> for the <see cref="Current" /> one.
            /// </summary>
            /// <returns>The next segment.</returns>
            public SweepLineSegment GetNext()
            {
                if (this.currentNode == null)
                    return null;

                Node nextNode = this.currentNode;
                if (nextNode.RightChild != null)
                {
                    nextNode = nextNode.RightChild;

                    while (nextNode.LeftChild != null)
                    {
                        nextNode = nextNode.LeftChild;
                    }

                    return nextNode.Key;
                }

                while (nextNode.Parent != null && nextNode.Parent.RightChild == nextNode)
                {
                    nextNode = nextNode.Parent;
                }

                if (nextNode.Parent != null && nextNode.Parent.LeftChild == nextNode)
                {
                    return nextNode.Parent.Key;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// The source coordinates.
        /// </summary>
        private readonly List<Coordinate> source;

        /// <summary>
        /// The list of endpoint indexes.
        /// </summary>
        private readonly List<Int32> endpoints;

        /// <summary>
        /// The sweep line tree.
        /// </summary>
        private readonly SweepLineTree tree;

        /// <summary>
        /// The coordinate comparer.
        /// </summary>
        private readonly IComparer<Coordinate> coordinateComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SweepLine" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public SweepLine(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            this.source = new List<Coordinate>(source.Elements());
            this.tree = new SweepLineTree(precisionModel ?? PrecisionModel.Default);
            this.coordinateComparer = new CoordinateComparer();

            if (this.source.Count >= 2 && this.source[0] == this.source[this.source.Count - 1])
                this.endpoints = new List<Int32> { 0, this.source.Count - 2 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SweepLine" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public SweepLine(IEnumerable<IEnumerable<Coordinate>> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            List<Coordinate> sourceList = new List<Coordinate>();

            this.endpoints = new List<Int32>();

            foreach (IEnumerable<Coordinate> collection in source)
            {
                Int32 collectionCount = collection.Count();
                if (collection == null || collectionCount < 2)
                    continue;

                Boolean firstEqualsLast = collection.FirstElement() == collection.LastElement();

                if (firstEqualsLast)
                    this.endpoints.Add(sourceList.Count);

                sourceList.AddRange(collection.Elements());

                if (firstEqualsLast)
                    this.endpoints.Add(sourceList.Count - 2);
            }

            this.source = sourceList;

            this.tree = new SweepLineTree(precisionModel);
            this.coordinateComparer = new CoordinateComparer();
        }

        /// <summary>
        /// Gets a value indicating whether gets whether the source of the <see cref="SweepLine" /> contains any closed line string.
        /// </summary>
        private Boolean HasSourcedClosed
        {
            get { return this.endpoints != null && this.endpoints.Count > 0; }
        }

        /// <summary>
        /// Adds a new endpoint event to the sweep line.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The sweep line segment created by addition of <paramref name="e" />.</returns>
        /// <exception cref="System.ArgumentNullException">The event is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The edge of the event is less than 0.
        /// or
        /// The edge of the event is greater than the number of edges in the source.
        /// </exception>
        public SweepLineSegment Add(EndPointEvent e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e), CoreMessages.EventIsNull);
            if (e.Edge < 0)
                throw new ArgumentException(CoreMessages.EventEdgeIsLessThan0, nameof(e));
            if (e.Edge >= this.source.Count - 1)
                throw new ArgumentException(CoreMessages.EventEdgeIsGreaterThanNumberOfEdges, nameof(e));

            SweepLineSegment segment = new SweepLineSegment { Edge = e.Edge };

            if (this.coordinateComparer.Compare(this.source[e.Edge], this.source[e.Edge + 1]) <= 0)
            {
                segment.LeftCoordinate = this.source[e.Edge];
                segment.RightCoordinate = this.source[e.Edge + 1];
            }
            else
            {
                segment.LeftCoordinate = this.source[e.Edge + 1];
                segment.RightCoordinate = this.source[e.Edge];
            }

            this.tree.Insert(segment);
            SweepLineSegment segmentAbove = this.tree.GetNext();
            SweepLineSegment segmentBelow = this.tree.GetPrevious();
            if (segmentAbove != null)
            {
                segment.Above = segmentAbove;
                segment.Above.Below = segment;
            }

            if (segmentBelow != null)
            {
                segment.Below = segmentBelow;
                segment.Below.Above = segment;
            }

            return segment;
        }

        /// <summary>
        /// Adds a new intersection event to the sweep line.
        /// Performs the order modifying effect of a possible intersection point between two directly adjacent segments.
        /// </summary>
        /// <remarks>
        /// An intersection event may become invalid if the order of the segments were altered or the intersection point has been already passed by the sweep line since the enqueuing of the event.
        /// This method is safe to be applied for invalid intersections.
        /// </remarks>
        /// <param name="e">The event.</param>
        /// <returns><c>true</c> if a new, valid intersection point was found and passed between the segments; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The event is null.</exception>
        /// <exception cref="InvalidOperationException">The segments do not intersect each other.</exception>
        public Boolean Add(IntersectionEvent e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e), CoreMessages.EventIsNull);

            return this.Intersect(e.Below, e.Above);
        }

        /// <summary>
        /// Searches the sweep line for an endpoint event.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The segment associated with the event.</returns>
        /// <exception cref="System.ArgumentNullException">The event is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The edge of the event is less than 0.
        /// or
        /// The edge of the event is greater than the number of edges in the source.
        /// </exception>
        public SweepLineSegment Search(EndPointEvent e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e), CoreMessages.EventIsNull);
            if (e.Edge < 0)
                throw new ArgumentException(CoreMessages.EventEdgeIsLessThan0, nameof(e));
            if (e.Edge >= this.source.Count - 1)
                throw new ArgumentException(CoreMessages.EventEdgeIsGreaterThanNumberOfEdges, nameof(e));

            SweepLineSegment segment = new SweepLineSegment() { Edge = e.Edge };

            if (this.coordinateComparer.Compare(this.source[e.Edge], this.source[e.Edge + 1]) < 0)
            {
                segment.LeftCoordinate = this.source[e.Edge];
                segment.RightCoordinate = this.source[e.Edge + 1];
            }
            else
            {
                segment.LeftCoordinate = this.source[e.Edge + 1];
                segment.RightCoordinate = this.source[e.Edge];
            }

            SweepLineSegment segmentResult;
            if (this.tree.TrySearch(segment, out segmentResult))
                return segmentResult;
            else
                return null;
        }

        /// <summary>
        /// Removes the specified sweep line segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        public void Remove(SweepLineSegment segment)
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment), CoreMessages.SegmentIsNull);

            this.tree.SetCurrent(segment);
            if (this.tree.Current == null)
                return;

            SweepLineSegment segmentAbove = this.tree.GetNext();
            SweepLineSegment segmentBelow = this.tree.GetPrevious();

            if (segmentAbove != null)
                segmentAbove.Below = segment.Below;
            if (segmentBelow != null)
                segmentBelow.Above = segment.Above;

            this.tree.Remove(segment);
        }

        /// <summary>
        /// Performs the order modifying effect of a possible intersection point between two directly adjacent segments.
        /// </summary>
        /// <remarks>
        /// An intersection event may become invalid if the order of the segments were altered or the intersection point has been already passed by the sweep line since the enqueuing of the event.
        /// This method is safe to be applied for invalid intersections.
        /// </remarks>
        /// <param name="x">First segment.</param>
        /// <param name="y">Second segment.</param>
        /// <returns><c>true</c> if a new, valid intersection point was found and passed between <paramref name="x" /> and <paramref name="y" />; otherwise <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">The segments do not intersect each other.</exception>
        public Boolean Intersect(SweepLineSegment x, SweepLineSegment y)
        {
            if (x == null || y == null)
                return false;

            SweepLineSegmentComparer comparer = (SweepLineSegmentComparer)this.tree.Comparer;
            SweepLineIntersection intersection = comparer.GetIntersection(x, y);

            if (intersection == SweepLineIntersection.NotExists)
                throw new InvalidOperationException(CoreMessages.SegmentsDoNotIntersect);
            if (intersection == SweepLineIntersection.Passed)
                return false;

            /*
             * Segment order before intersection: belowBelow <-> below <-> above <-> aboveAbove
             * Segment order after intersection:  belowBelow <-> above <-> below <-> aboveAbove
             */
            SweepLineSegment below, above;
            if (x.Above == y)
            {
                below = x;
                above = y;
            }
            else if (y.Above == x)
            {
                below = y;
                above = x;
            }
            else
            {
                return false;
            }

            this.tree.Remove(x);
            this.tree.Remove(y);
            comparer.PassIntersection(x, y);
            this.tree.Insert(x);
            this.tree.Insert(y);

            SweepLineSegment belowBelow = below.Below;
            SweepLineSegment aboveAbove = above.Above;

            below.Above = aboveAbove;
            below.Below = above;
            above.Below = belowBelow;
            above.Above = below;

            if (belowBelow != null)
                belowBelow.Above = above;
            if (aboveAbove != null)
                aboveAbove.Below = below;

            return true;
        }

        /// <summary>
        /// Determines whether two edges of a line string or a polygon shell are adjacent.
        /// </summary>
        /// <param name="xEdge">Identifier of the first edge.</param>
        /// <param name="yEdge">Identifier of the second edge.</param>
        /// <returns><c>true</c> if the two edges are adjacent; otherwise, <c>false</c>.</returns>
        public Boolean IsAdjacent(Int32 xEdge, Int32 yEdge)
        {
            if (Math.Abs(xEdge - yEdge) == 1)
                return true;

            if (this.HasSourcedClosed)
            {
                Int32 xIndex = this.endpoints.IndexOf(xEdge),
                      yIndex = this.endpoints.IndexOf(yEdge);
                if (xIndex >= 0 && yIndex >= 0 && Math.Abs(xIndex - yIndex) == 1 &&
                    ((xIndex < yIndex && xIndex % 2 == 0) || (xIndex > yIndex && yIndex % 2 == 0)))
                    return true;
            }

            return false;
        }
    }
}
