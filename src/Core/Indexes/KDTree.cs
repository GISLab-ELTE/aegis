namespace AEGIS.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a KDTree coordinate index.
    /// The tree is balanced when created, but adding or removing elements might leave it unbalanced. Perform the <see cref="KDTree.RebalanceTree"/> operation to rebalance the tree.
    /// </summary>
    public class KDTree : ICoordinateIndex
    {
        /// <summary>
        /// Represents a node of the KDTree.
        /// </summary>
        protected class KDTreeNode
        {
            /// <summary>
            /// The splitting dimension of this node.
            /// </summary>
            private readonly int splitDimension;

            /// <summary>
            /// The coordinate stored in this node.
            /// </summary>
            private Coordinate point;

            /// <summary>
            /// The left child of this node.
            /// </summary>
            private KDTreeNode leftChild;

            /// <summary>
            /// The right child of this node.
            /// </summary>
            private KDTreeNode rightChild;

            /// <summary>
            /// Initializes a new instance of the <see cref="KDTreeNode"/> class.
            /// </summary>
            /// <param name="point">The point to be stored in this node.</param>
            /// <param name="splitDimension">The splitting dimension of this node.</param>
            /// <param name="totalDimensions">The total number of dimensions of the tree.</param>
            public KDTreeNode(Coordinate point, int splitDimension, int totalDimensions)
            {
                this.point = point;
                this.leftChild = null;
                this.rightChild = null;
                this.splitDimension = splitDimension;
                this.TotalDimensions = totalDimensions;
            }

            /// <summary>
            /// Gets the point stored in this node.
            /// </summary>
            public Coordinate Point
            {
                get
                {
                    return this.point;
                }
            }

            /// <summary>
            /// Gets or sets the total dimensions of the tree.
            /// </summary>
            public int TotalDimensions { get; set; }

            /// <summary>
            /// Gets a value indicating whether this node has a left child.
            /// </summary>
            public Boolean LeftChildExists { get { return this.leftChild != null; } }

            /// <summary>
            /// Gets a value indicating whether this node has a right child.
            /// </summary>
            public Boolean RightChildExists { get { return this.rightChild != null; } }

            /// <summary>
            /// Gets the number of geometries stored in this node and all its descendants.
            /// </summary>
            public int NumberOfGeometries
            {
                get
                {
                    int count = 0;

                    if (this.LeftChildExists)
                        count += this.leftChild.NumberOfGeometries;
                    if (this.RightChildExists)
                        count += this.rightChild.NumberOfGeometries;

                    count += 1;
                    return count;
                }
            }

            /// <summary>
            /// Adds a coordinate to the subtree of this node.
            /// </summary>
            /// <param name="coordinate">The coordinate to be added.</param>
            public void Add(Coordinate coordinate)
            {
                if (coordinate.Equals(this.Point))
                    throw new ArgumentException("Cannot add the same point twice.", "coordinate");

                // Determining whether the coordinate should be in the left or the right subtree of this node.
                Comparison<Coordinate> splitDimensionComparison = this.GetComparisonForDimension(this.splitDimension);
                int comparisonResult = splitDimensionComparison(coordinate, this.point);

                if (comparisonResult < 0)
                {
                    // If left child exists, add it to left child's subtree, otherwise create left child with the coordinate.
                    if (this.LeftChildExists)
                        this.leftChild.Add(coordinate);
                    else
                        this.leftChild = new KDTreeNode(coordinate, this.splitDimension % this.TotalDimensions + 1, this.TotalDimensions);
                }
                else
                {
                    // If right child exists, add it to right child's subtree, otherwise create right child with the coordinate.
                    if (this.RightChildExists)
                        this.rightChild.Add(coordinate);
                    else
                        this.rightChild = new KDTreeNode(coordinate, this.splitDimension % this.TotalDimensions + 1, this.TotalDimensions);
                }
            }

            /// <summary>
            /// Determines whether this node's subtree contains a coordinate.
            /// </summary>
            /// <param name="coordinate">The coordinate we are looking for.</param>
            /// <returns>A value indicating whether this node's subtree contains the coordinate.</returns>
            public bool Contains(Coordinate coordinate)
            {
                if (coordinate.Equals(this.Point))
                    return true;

                Comparison<Coordinate> splitDimensionComparison = this.GetComparisonForDimension(this.splitDimension);
                int comparisonResult = splitDimensionComparison(coordinate, this.point);

                if (comparisonResult < 0 && this.LeftChildExists)
                    return this.leftChild.Contains(coordinate);
                if (this.RightChildExists)
                    return this.rightChild.Contains(coordinate);

                return false;
            }

            /// <summary>
            /// Attempts to remove a coordinate from this node's subtree.
            /// </summary>
            /// <param name="coordinate">The coordinate to be removed.</param>
            /// <returns>A value indicating whether the remove was successful.</returns>
            public bool Remove(Coordinate coordinate)
            {
                Comparison<Coordinate> splitDimensionComparison = this.GetComparisonForDimension(this.splitDimension);
                int comparisonResult = splitDimensionComparison(coordinate, this.point);

                if (comparisonResult < 0)
                {
                    if (this.leftChild == null)
                        return false;

                    if (this.leftChild.Point.Equals(coordinate))
                    {
                        // If we are removing the left child, we need to recreate the left subtree to ensure that the tree's constraints are still true after the remove.
                        List<Coordinate> pointsToRecreate = this.leftChild.AllCoordinatesFromSubTree();
                        pointsToRecreate.Remove(coordinate);
                        this.InitializeLeftSubTree(pointsToRecreate);

                        return true;
                    }
                    else
                    {
                        return this.leftChild.Remove(coordinate);
                    }
                }
                else
                {
                    if (this.rightChild == null)
                        return false;

                    if (this.rightChild.Point.Equals(coordinate))
                    {
                        // If we are removing the right child, we need to recreate the left subtree to ensure that the tree's constraints are still true after the remove.
                        List<Coordinate> pointsToRecreate = this.rightChild.AllCoordinatesFromSubTree();
                        pointsToRecreate.Remove(coordinate);
                        this.InitializeRightSubTree(pointsToRecreate);

                        return true;
                    }
                    else
                    {
                        return this.rightChild.Remove(coordinate);
                    }
                }
            }

            /// <summary>
            /// Searches this node's subtree for any coordinate in the specified envelope.
            /// </summary>
            /// <param name="envelope">The envelope in which the search is performed.</param>
            /// <returns>All coordinates that are in the subtree of this node and the envelope.</returns>
            public IEnumerable<Coordinate> Search(Envelope envelope)
            {
                List<Coordinate> results = new List<Coordinate>();

                if (this.LeftChildShouldBeSearched(envelope))
                    results.AddRange(this.leftChild.Search(envelope));
                if (this.RightChildShouldBeSearched(envelope))
                    results.AddRange(this.rightChild.Search(envelope));

                if (envelope.Contains(this.Point))
                    results.Add(this.Point);

                return results;
            }

            /// <summary>
            /// Gets all coordinates stored in the subtree rooted at this node.
            /// </summary>
            /// <returns>All coordinates stored in the subtree rooted at this node.</returns>
            public List<Coordinate> AllCoordinatesFromSubTree()
            {
                List<Coordinate> coordinates = new List<Coordinate>();

                if (this.LeftChildExists)
                    coordinates.AddRange(this.leftChild.AllCoordinatesFromSubTree());
                if (this.RightChildExists)
                    coordinates.AddRange(this.rightChild.AllCoordinatesFromSubTree());

                coordinates.Add(this.Point);

                return coordinates;
            }

            /// <summary>
            /// Initializes the subtrees of this node.
            /// </summary>
            /// <param name="leftPoints">The coordinates to be stored in the left subtree of this node.</param>
            /// <param name="rightPoints">The coordinates to be stored in the right subtree of this node</param>
            public void InitializeSubTree(List<Coordinate> leftPoints, List<Coordinate> rightPoints)
            {
                this.InitializeLeftSubTree(leftPoints);
                this.InitializeRightSubTree(rightPoints);
            }

            /// <summary>
            /// Performs a search for a nearest neighbour in the subtree rooted at this node.
            /// </summary>
            /// <param name="searchPoint">The point whose nearest neghbour is searched for.</param>
            /// <returns>The nearest neighbour of the specified coordinate.</returns>
            public Coordinate NearestNeighbourSearch(Coordinate searchPoint)
            {
                Comparison<Coordinate> splitDimensionComparison = this.GetComparisonForDimension(this.splitDimension);
                int comparisonResult = splitDimensionComparison(searchPoint, this.point);
                Coordinate currentBest;

                // We recurse down in the tree to the first leaf, always going left or right based on the comparison by the split dimension of the current node.
                if (comparisonResult < 0)
                {
                    if (this.LeftChildExists)
                        currentBest = this.leftChild.NearestNeighbourSearch(searchPoint);
                    else
                        return this.Point;
                }
                else
                {
                    if (this.RightChildExists)
                        currentBest = this.rightChild.NearestNeighbourSearch(searchPoint);
                    else
                        return this.Point;
                }

                // When we recurse up the tree we check whether the coordinate stored in this node is closer than the best we have found so far.
                if (Coordinate.Distance(this.Point, searchPoint) < Coordinate.Distance(currentBest, searchPoint))
                    currentBest = this.Point;

                // We check whether the OTHER subtree of this node (which we did NOT use to recurse down to the leaf nodes) could contain a node closer than the current best.
                if (this.DistanceInDimension(searchPoint, this.Point, this.splitDimension) < Coordinate.Distance(searchPoint, currentBest))
                {
                    Coordinate otherSubTreesBest = null;

                    // If the other subtree could contain a closer coordinate, we perform the same search on the other subtree.
                    if (comparisonResult < 0 && this.RightChildExists)
                        otherSubTreesBest = this.rightChild.NearestNeighbourSearch(searchPoint);
                    if (comparisonResult > 0 && this.LeftChildExists)
                        otherSubTreesBest = this.leftChild.NearestNeighbourSearch(searchPoint);

                    // If we performed the search of the other subtree, we check if the result from that subtree is better than the one we have already found.
                    if (otherSubTreesBest != null)
                    {
                        currentBest = Coordinate.Distance(currentBest, searchPoint) < Coordinate.Distance(otherSubTreesBest, searchPoint)
                            ? currentBest
                            : otherSubTreesBest;
                    }
                }

                return currentBest;
            }

            /// <summary>
            /// Determines whether the left subtree should be searched.
            /// </summary>
            /// <param name="envelope">The envelope in which the search is performed.</param>
            /// <returns>A value indicating whether the left subtree of this node has to be searched.</returns>
            private bool LeftChildShouldBeSearched(Envelope envelope)
            {
                if (this.leftChild == null)
                    return false;

                if (this.splitDimension == 1)
                {
                    return this.leftChild.Point.X < envelope.MinX
                                    ? false
                                    : true;
                }

                if (this.splitDimension == 2)
                {
                    return this.leftChild.Point.Y < envelope.MinY
                                    ? false
                                    : true;
                }

                if (this.splitDimension == 3)
                {
                    return this.leftChild.Point.Z < envelope.MinZ
                                    ? false
                                    : true;
                }

                return true;
            }

            /// <summary>
            /// Determines whether the right subtree should be searched.
            /// </summary>
            /// <param name="envelope">The envelope in which the search is performed.</param>
            /// <returns>A value indicating whether the right subtree of this node has to be searched.</returns>
            private bool RightChildShouldBeSearched(Envelope envelope)
            {
                if (this.rightChild == null)
                    return false;

                if (this.splitDimension == 1)
                {
                    return this.rightChild.Point.X > envelope.MaxX
                                    ? false
                                    : true;
                }

                if (this.splitDimension == 2)
                {
                    return this.rightChild.Point.Y > envelope.MaxY
                                    ? false
                                    : true;
                }

                if (this.splitDimension == 3)
                {
                    return this.rightChild.Point.Z > envelope.MaxZ
                                    ? false
                                    : true;
                }

                return true;
            }

            /// <summary>
            /// Initializes the left subtree of this node.
            /// </summary>
            /// <param name="coordinates">The coordinates to be stored in the left subtree of this node.</param>
            private void InitializeLeftSubTree(List<Coordinate> coordinates)
            {
                if (coordinates.Count == 0)
                {
                    this.leftChild = null;
                    return;
                }

                int nextSplitDimension = this.splitDimension % this.TotalDimensions + 1;
                Comparison<Coordinate> coordinateComparison = this.GetComparisonForDimension(nextSplitDimension);
                coordinates.Sort(coordinateComparison);

                Coordinate median = coordinates[coordinates.Count / 2];
                this.leftChild = new KDTreeNode(median, nextSplitDimension, this.TotalDimensions);

                List<Coordinate> leftCoordinatesOfChild = coordinates.GetRange(0, coordinates.Count / 2);
                List<Coordinate> rightCoordinatesOfChild = coordinates.GetRange(coordinates.Count / 2 + 1, (int)Math.Ceiling(coordinates.Count / 2.0) - 1);
                this.leftChild.InitializeSubTree(leftCoordinatesOfChild, rightCoordinatesOfChild);
            }

            /// <summary>
            /// Initializes the right subtree of this node.
            /// </summary>
            /// <param name="coordinates">The coordinates to be stored in the right subtree of this node.</param>
            private void InitializeRightSubTree(List<Coordinate> coordinates)
            {
                if (coordinates.Count == 0)
                {
                    this.rightChild = null;
                    return;
                }

                int nextSplitDimension = this.splitDimension % this.TotalDimensions + 1;
                Comparison<Coordinate> coordinateComparison = this.GetComparisonForDimension(nextSplitDimension);
                coordinates.Sort(coordinateComparison);

                Coordinate median = coordinates[coordinates.Count / 2];
                this.rightChild = new KDTreeNode(median, nextSplitDimension, this.TotalDimensions);

                List<Coordinate> leftCoordinatesOfChild = coordinates.GetRange(0, coordinates.Count / 2);
                List<Coordinate> rightCoordinatesOfChild = coordinates.GetRange(coordinates.Count / 2 + 1, (int)Math.Ceiling(coordinates.Count / 2.0) - 1);
                this.rightChild.InitializeSubTree(leftCoordinatesOfChild, rightCoordinatesOfChild);
            }

            /// <summary>
            /// Gets a Comparison for coordinates based on the dimension on which the comparison should be performed.
            /// </summary>
            /// <param name="dimension">The dimension on which the comparison will compare the coordinates.</param>
            /// <returns>A comparison for the specified dimension.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">The dimension is not 1, 2 or 3.</exception>
            private Comparison<Coordinate> GetComparisonForDimension(int dimension)
            {
                switch (dimension)
                {
                    case 1:
                        return (p1, p2) => p1.X.CompareTo(p2.X);
                    case 2:
                        return (p1, p2) => p1.Y.CompareTo(p2.Y);
                    case 3:
                        return (p1, p2) => p1.Z.CompareTo(p2.Z);
                    default:
                        throw new ArgumentOutOfRangeException("dimension", "The dimension must be between 1 and 3");
                }
            }

            /// <summary>
            /// Determines the distance of two coordinates based only on a single dimension.
            /// </summary>
            /// <param name="first">The first coordinate.</param>
            /// <param name="second">The second coordinate.</param>
            /// <param name="dimension">The dimension on which the comparison will be performed.</param>
            /// <returns>The distance of the two coordinates in the specified dimension.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">The dimension is not 1, 2 or 3.</exception>
            private double DistanceInDimension(Coordinate first, Coordinate second, int dimension)
            {
                switch (dimension)
                {
                    case 1:
                        return Math.Abs(first.X - second.X);
                    case 2:
                        return Math.Abs(first.Y - second.Y);
                    case 3:
                        return Math.Abs(first.Z - second.Z);
                    default:
                        throw new ArgumentOutOfRangeException("dimension", "The dimension must be between 1 and 3");
                }
            }
        }

        /// <summary>
        /// The number of dimensions of the tree.
        /// </summary>
        private readonly int treeDimensions;

        /// <summary>
        /// The root of the tree.
        /// </summary>
        private KDTreeNode root;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDTree"/> class.
        /// </summary>
        /// <param name="points">The points to create the tree from.</param>
        /// <param name="treeDimensions">The dimensions of the tree.</param>
        /// <exception cref="System.ArgumentNullException">The points are null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The treeDimensions is not 2 or 3.</exception>
        public KDTree(IEnumerable<Coordinate> points, int treeDimensions)
        {
            if (points == null)
                throw new ArgumentNullException("coordinates", "The coordinates must not be null.");

            if (treeDimensions != 2 && treeDimensions != 3)
                throw new ArgumentOutOfRangeException("treeDimensions", "The tree's dimensions must be 2 or 3.");

            this.treeDimensions = treeDimensions;

            this.InitializeTree(points.ToList());
        }

        /// <summary>
        /// Initializes the tree.
        /// </summary>
        /// <param name="coordinates">The list of coordinates to create the tree from.</param>
        private void InitializeTree(List<Coordinate> coordinates)
        {
            // If there are no coordinates specified, leave the tree empty.
            if (coordinates.Count == 0)
                return;

            // Sorting the input points by the X coordinate
            coordinates.Sort((p1, p2) => p1.X.CompareTo(p2.X));

            // Setting up the root to be the median by the X coordinate
            Coordinate median = coordinates[coordinates.Count / 2];
            this.root = new KDTreeNode(median, 1, this.TreeDimension);

            // Add all input points to the tree
            this.root.InitializeSubTree(coordinates.GetRange(0, coordinates.Count / 2), coordinates.GetRange(coordinates.Count / 2 + 1, (int)Math.Ceiling(coordinates.Count / 2.0) - 1));
        }

        /// <summary>
        /// Gets the tree's dimension.
        /// </summary>
        public int TreeDimension { get { return this.treeDimensions; } }

        /// <summary>
        /// Gets a value indicating whether the tree is empty.
        /// </summary>
        public bool IsEmpty { get { return this.root == null; } }

        /// <summary>
        /// Gets a value indicating whether the tree is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of geometries stored in the tree.
        /// </summary>
        public int NumberOfGeometries
        {
            get
            {
                return this.IsEmpty ? 0 : this.root.NumberOfGeometries;
            }
        }

        /// <summary>
        /// Adds a coordinate to the tree.
        /// </summary>
        /// <param name="coordinate">The coordinate to be added.</param>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        /// <exception cref="System.InvalidOperationException">The tree is read-only.</exception>
        /// <exception cref="System.ArgumentException">The coordinate is already in the tree.</exception>
        public void Add(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "The coordinate is null.");
            if (this.IsReadOnly)
                throw new InvalidOperationException("The tree is read-only. Adding is not allowed.");

            if (this.IsEmpty)
            {
                this.root = new KDTreeNode(coordinate, 1, this.TreeDimension);
            }
            else
            {
                this.root.Add(coordinate);
            }
        }

        /// <summary>
        /// Adds multiple coordinates to the tree.
        /// </summary>
        /// <param name="coordinates">The coordinates to be added.</param>
        /// <exception cref="System.ArgumentNullException">The coordinates is null, or it contains a null element.</exception>
        /// <exception cref="System.InvalidOperationException">The tree is read-only.</exception>
        /// <exception cref="System.ArgumentException">A coordinate in coordinates is already in the tree.</exception>
        public void Add(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The coordinates are null.");

            foreach (Coordinate coordinate in coordinates)
                this.Add(coordinate);
        }

        public void Clear()
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("The tree is read-only. Clearing is not allowed.");

            this.root = null;
        }

        /// <summary>
        /// Returns a value indicating whether the tree contains the coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>True if the tree contains the coordinate, false otherwise.</returns>
        public bool Contains(Coordinate coordinate)
        {
            return coordinate == null || this.IsEmpty
                    ? false
                    : this.root.Contains(coordinate);
        }

        /// <summary>
        /// Attempts to remove a coordinate from the tree.
        /// </summary>
        /// <param name="coordinate">The coordinate to be removed.</param>
        /// <returns>A value indicating whether the remove was successful.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public bool Remove(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "The coordinate is null.");

            if (this.IsEmpty)
                return false;

            if (this.root.Point.Equals(coordinate))
            {
                if (this.NumberOfGeometries == 1)
                {
                    this.Clear();
                    return true;
                }

                List<Coordinate> points = this.root.AllCoordinatesFromSubTree();
                points.Remove(coordinate);
                this.Clear();
                this.InitializeTree(points);
                return true;
            }

            return this.root.Remove(coordinate);
        }

        /// <summary>
        /// Attempts to remove all coordinates inside an envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>A value indicating whether any coordinates were removed.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public bool Remove(Envelope envelope)
        {
            bool result = false;
            IEnumerable<Coordinate> coordinates = this.Search(envelope);
            foreach (Coordinate coordinate in coordinates)
            {
                if (this.Remove(coordinate))
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Attempts to remove all coordinates inside an envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="coordinates">The coordinates which were removed.</param>
        /// <returns>A value indicating whether any coordinates were removed.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public bool Remove(Envelope envelope, out List<Coordinate> coordinates)
        {
            bool result = false;
            coordinates = this.Search(envelope).ToList();
            foreach (Coordinate coordinate in coordinates)
            {
                if (this.Remove(coordinate))
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Searches the tree for all coordinates inside the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>All coordinates from the envelope which are in the tree.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public IEnumerable<Coordinate> Search(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "The envelope is null.");

            if (this.IsEmpty)
                return new List<Coordinate>();

            return this.root.Search(envelope);
        }

        /// <summary>
        /// Searches the tree for the nearest neighbour of the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The nearest neighbour of the specified coordinate.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        /// <exception cref="System.InvalidOperationException">The tree is empty.</exception>
        public Coordinate NearestNeighbourSearch(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "The coordinate is null.");
            if (this.IsEmpty)
                throw new InvalidOperationException("Cannot perform nearest neighbour search on an empty tree.");

            return this.root.NearestNeighbourSearch(coordinate);
        }

        /// <summary>
        /// Rebalances the tree. The tree is balanced when created, but adding or removing elements might leave it unbalanced.
        /// The rebalancing requires the complete reconstruction of the tree, therefore it can be a costly operation.
        /// Because of this cost, it is not automatically done after each add or remove, but can be rebalanced at any time if required.
        /// </summary>
        public void RebalanceTree()
        {
            List<Coordinate> allCoordinates = this.root.AllCoordinatesFromSubTree();
            this.Clear();
            this.InitializeTree(allCoordinates);
        }
    }
}
