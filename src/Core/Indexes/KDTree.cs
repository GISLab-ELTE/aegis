// <copyright file="KDTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a a-d tree coordinate index.
    /// </summary>
    /// <remarks>
    /// A k-d tree (short for k-dimensional tree) is a space-partitioning data structure for organizing coordinates in a k-dimensional space.
    /// The tree is balanced when created, but adding or removing elements might leave it unbalanced.
    /// Perform the <see cref="KDTree.RebalanceTree"/> operation to rebalance the tree.
    /// </remarks>
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
            private readonly Int32 splitDimension;

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
            /// <param name="coordinate">The coordinate to be stored in this node.</param>
            /// <param name="splitDimension">The splitting dimension of this node.</param>
            /// <param name="numberOfDimensions">The total number of dimensions of the tree.</param>
            public KDTreeNode(Coordinate coordinate, Int32 splitDimension, Int32 numberOfDimensions)
            {
                this.Coordinate = coordinate;
                this.leftChild = null;
                this.rightChild = null;
                this.splitDimension = splitDimension;
                this.NumberOfDimensions = numberOfDimensions;
            }

            /// <summary>
            /// Gets the coordinate stored in this node.
            /// </summary>
            public Coordinate Coordinate { get; private set; }

            /// <summary>
            /// Gets or sets the number of dimensions of the tree.
            /// </summary>
            /// <remarks>
            /// Represents the number of dimensions (K) of the KD-Tree.
            /// </remarks>
            public Int32 NumberOfDimensions { get; set; }

            /// <summary>
            /// Gets a value indicating whether this node has a left child.
            /// </summary>
            public Boolean LeftChildExists { get { return this.leftChild != null; } }

            /// <summary>
            /// Gets a value indicating whether this node has a right child.
            /// </summary>
            public Boolean RightChildExists { get { return this.rightChild != null; } }

            /// <summary>
            /// Adds a coordinate to the subtree of this node.
            /// </summary>
            /// <param name="coordinate">The coordinate to be added.</param>
            /// <returns><c>true</c> if the coordinate could be soccessfully inserted; otherwise, <c>false</c>.</returns>
            public Boolean Add(Coordinate coordinate)
            {
                if (coordinate.Equals(this.Coordinate))
                    return false;

                // Determining whether the coordinate should be in the left or the right subtree of this node.
                Comparison<Coordinate> splitDimensionComparison = this.GetComparisonForDimension(this.splitDimension);
                int comparisonResult = splitDimensionComparison(coordinate, this.Coordinate);

                if (comparisonResult < 0)
                {
                    // If left child exists, add it to left child's subtree, otherwise create left child with the coordinate.
                    if (this.LeftChildExists)
                        this.leftChild.Add(coordinate);
                    else
                        this.leftChild = new KDTreeNode(coordinate, this.splitDimension % this.NumberOfDimensions + 1, this.NumberOfDimensions);
                }
                else
                {
                    // If right child exists, add it to right child's subtree, otherwise create right child with the coordinate.
                    if (this.RightChildExists)
                        this.rightChild.Add(coordinate);
                    else
                        this.rightChild = new KDTreeNode(coordinate, this.splitDimension % this.NumberOfDimensions + 1, this.NumberOfDimensions);
                }

                return true;
            }

            /// <summary>
            /// Determines whether this node's subtree contains a coordinate.
            /// </summary>
            /// <param name="coordinate">The coordinate we are looking for.</param>
            /// <returns>A value indicating whether this node's subtree contains the coordinate.</returns>
            public Boolean Contains(Coordinate coordinate)
            {
                if (coordinate.Equals(this.Coordinate))
                    return true;

                Comparison<Coordinate> splitDimensionComparison = this.GetComparisonForDimension(this.splitDimension);
                Int32 comparisonResult = splitDimensionComparison(coordinate, this.Coordinate);

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
                Int32 comparisonResult = splitDimensionComparison(coordinate, this.Coordinate);
                List<Coordinate> coordinatesToRecreate = new List<Coordinate>();

                if (comparisonResult < 0)
                {
                    if (this.leftChild == null)
                        return false;

                    if (this.leftChild.Coordinate.Equals(coordinate))
                    {
                        // If we are removing the left child, we need to recreate the left subtree to ensure that the tree's constraints are still true after the remove.
                        this.leftChild.AllCoordinatesFromSubTree(coordinatesToRecreate);
                        coordinatesToRecreate.Remove(coordinate);
                        this.InitializeLeftSubTree(coordinatesToRecreate);

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

                    if (this.rightChild.Coordinate.Equals(coordinate))
                    {
                        // If we are removing the right child, we need to recreate the left subtree to ensure that the tree's constraints are still true after the remove.
                        this.rightChild.AllCoordinatesFromSubTree(coordinatesToRecreate);
                        coordinatesToRecreate.Remove(coordinate);
                        this.InitializeRightSubTree(coordinatesToRecreate);

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

                if (envelope.Contains(this.Coordinate))
                    results.Add(this.Coordinate);

                return results;
            }

            /// <summary>
            /// Gets all coordinates stored in the subtree rooted at this node.
            /// </summary>
            /// <param name="coordinates">The list of coordinates to which the method will add its results.</param>
            public void AllCoordinatesFromSubTree(List<Coordinate> coordinates)
            {
                if (this.LeftChildExists)
                    this.leftChild.AllCoordinatesFromSubTree(coordinates);
                if (this.RightChildExists)
                    this.rightChild.AllCoordinatesFromSubTree(coordinates);

                coordinates.Add(this.Coordinate);
            }

            /// <summary>
            /// Initializes the subtrees of this node.
            /// </summary>
            /// <param name="leftCoordinates">The coordinates to be stored in the left subtree of this node.</param>
            /// <param name="rightCoordinates">The coordinates to be stored in the right subtree of this node.</param>
            public void InitializeSubTree(List<Coordinate> leftCoordinates, List<Coordinate> rightCoordinates)
            {
                this.InitializeLeftSubTree(leftCoordinates);
                this.InitializeRightSubTree(rightCoordinates);
            }

            /// <summary>
            /// Performs a search for a nearest neighbour in the subtree rooted at this node.
            /// </summary>
            /// <param name="searchCoordinate">The coordinate whose nearest neghbour is searched for.</param>
            /// <returns>The nearest neighbour of the specified coordinate.</returns>
            public Coordinate NearestNeighbourSearch(Coordinate searchCoordinate)
            {
                Comparison<Coordinate> splitDimensionComparison = this.GetComparisonForDimension(this.splitDimension);
                Int32 comparisonResult = splitDimensionComparison(searchCoordinate, this.Coordinate);
                Coordinate currentBest;

                // We recurse down in the tree to the first leaf, always going left or right based on the comparison by the split dimension of the current node.
                if (comparisonResult < 0)
                {
                    if (this.LeftChildExists)
                        currentBest = this.leftChild.NearestNeighbourSearch(searchCoordinate);
                    else
                        return this.Coordinate;
                }
                else
                {
                    if (this.RightChildExists)
                        currentBest = this.rightChild.NearestNeighbourSearch(searchCoordinate);
                    else
                        return this.Coordinate;
                }

                // When we recurse up the tree we check whether the coordinate stored in this node is closer than the best we have found so far.
                if (Coordinate.Distance(this.Coordinate, searchCoordinate) < Coordinate.Distance(currentBest, searchCoordinate))
                    currentBest = this.Coordinate;

                // We check whether the OTHER subtree of this node (which we did NOT use to recurse down to the leaf nodes) could contain a node closer than the current best.
                if (this.DistanceInDimension(searchCoordinate, this.Coordinate, this.splitDimension) < Coordinate.Distance(searchCoordinate, currentBest))
                {
                    Coordinate otherSubTreesBest = null;

                    // If the other subtree could contain a closer coordinate, we perform the same search on the other subtree.
                    if (comparisonResult < 0 && this.RightChildExists)
                        otherSubTreesBest = this.rightChild.NearestNeighbourSearch(searchCoordinate);
                    if (comparisonResult > 0 && this.LeftChildExists)
                        otherSubTreesBest = this.leftChild.NearestNeighbourSearch(searchCoordinate);

                    // If we performed the search of the other subtree, we check if the result from that subtree is better than the one we have already found.
                    if (otherSubTreesBest != null)
                    {
                        currentBest = Coordinate.Distance(currentBest, searchCoordinate) < Coordinate.Distance(otherSubTreesBest, searchCoordinate)
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
                    return this.leftChild.Coordinate.X >= envelope.MinX;

                if (this.splitDimension == 2)
                    return this.leftChild.Coordinate.Y >= envelope.MinY;

                return this.leftChild.Coordinate.Z >= envelope.MinZ;
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
                    return this.rightChild.Coordinate.X <= envelope.MaxX;

                if (this.splitDimension == 2)
                    return this.rightChild.Coordinate.Y <= envelope.MaxY;

                return this.rightChild.Coordinate.Z <= envelope.MaxZ;
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

                Int32 nextSplitDimension = this.splitDimension % this.NumberOfDimensions + 1;
                Comparison<Coordinate> coordinateComparison = this.GetComparisonForDimension(nextSplitDimension);
                coordinates.Sort(coordinateComparison);

                Coordinate median = coordinates[coordinates.Count / 2];
                this.leftChild = new KDTreeNode(median, nextSplitDimension, this.NumberOfDimensions);

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

                Int32 nextSplitDimension = this.splitDimension % this.NumberOfDimensions + 1;
                Comparison<Coordinate> coordinateComparison = this.GetComparisonForDimension(nextSplitDimension);
                coordinates.Sort(coordinateComparison);

                Coordinate median = coordinates[coordinates.Count / 2];
                this.rightChild = new KDTreeNode(median, nextSplitDimension, this.NumberOfDimensions);

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
                if (dimension == 1)
                    return (p1, p2) => p1.X.CompareTo(p2.X);
                if (dimension == 2)
                    return (p1, p2) => p1.Y.CompareTo(p2.Y);

                return (p1, p2) => p1.Z.CompareTo(p2.Z);
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
                if (dimension == 1)
                    return Math.Abs(first.X - second.X);
                if (dimension == 2)
                    return Math.Abs(first.Y - second.Y);

                return Math.Abs(first.Z - second.Z);
            }
        }

        /// <summary>
        /// The root of the tree.
        /// </summary>
        private KDTreeNode root;

        /// <summary>
        /// Initializes a new instance of the <see cref="KDTree"/> class.
        /// </summary>
        /// <param name="coordinates">The coordinates to create the tree from.</param>
        /// <param name="treeDimensions">The dimensions of the tree.</param>
        /// <exception cref="System.ArgumentNullException">The collection of coordinates is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The tree dimensions is not 2 or 3.</exception>
        public KDTree(IEnumerable<Coordinate> coordinates, Int32 treeDimensions)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            if (treeDimensions > 3)
                throw new ArgumentOutOfRangeException(nameof(treeDimensions), CoreMessages.DimensionIsGreaterThan3);
            if (treeDimensions < 2)
                throw new ArgumentOutOfRangeException(nameof(treeDimensions), CoreMessages.DimensionIsLessThan2);

            this.TreeDimension = treeDimensions;

            this.InitializeTree(coordinates.ToList());
        }

        /// <summary>
        /// Gets the dimension of the tree.
        /// </summary>
        /// <remarks>
        /// This is the total number of dimensions on which the tree iterates from level to level,
        /// which ideally corresponds to the dimension of Coordinates we wish to add to the tree.
        /// In the current implementation the possible values are 2 and 3.
        /// </remarks>
        public Int32 TreeDimension { get; private set; }

        /// <summary>
        /// Gets the number of indexed geometries.
        /// </summary>
        /// <value>The number of indexed geometries.</value>
        public Int32 NumberOfGeometries { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the tree is empty.
        /// </summary>
        public Boolean IsEmpty { get { return this.root == null; } }

        /// <summary>
        /// Gets a value indicating whether the tree is readonly.
        /// </summary>
        public Boolean IsReadOnly { get { return false; } }

        /// <summary>
        /// Adds a coordinate to the tree.
        /// </summary>
        /// <param name="coordinate">The coordinate to be added.</param>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public void Add(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            this.NumberOfGeometries++;

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
        /// <exception cref="System.ArgumentException">A coordinate in coordinates is already in the tree.</exception>
        public void Add(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            foreach (Coordinate coordinate in coordinates)
                this.Add(coordinate);
        }

        /// <summary>
        /// Clears all stored elements from the index, resulting in an empty tree.
        /// </summary>
        public void Clear()
        {
            this.NumberOfGeometries = 0;
            this.root = null;
        }

        /// <summary>
        /// Returns a value indicating whether the tree contains the coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the specified coordinate is indexed; otherwise <c>false</c>.</returns>
        public Boolean Contains(Coordinate coordinate)
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
        public Boolean Remove(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            if (this.IsEmpty)
                return false;

            Boolean removeSuccessful = false;

            if (this.root.Coordinate.Equals(coordinate))
            {
                if (this.NumberOfGeometries == 1)
                {
                    this.Clear();
                    return true;
                }

                List<Coordinate> allCoordiantes = new List<Coordinate>(this.NumberOfGeometries);
                this.root.AllCoordinatesFromSubTree(allCoordiantes);
                removeSuccessful = allCoordiantes.Remove(coordinate);
                if (removeSuccessful)
                {
                    this.Clear();
                    this.InitializeTree(allCoordiantes);
                }

                return removeSuccessful;
            }

            removeSuccessful = this.root.Remove(coordinate);

            if (removeSuccessful)
                this.NumberOfGeometries--;

            return removeSuccessful;
        }

        /// <summary>
        /// Attempts to remove all coordinates inside an envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>A value indicating whether any coordinates were removed.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Remove(Envelope envelope)
        {
            Boolean result = false;
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
        public Boolean Remove(Envelope envelope, out List<Coordinate> coordinates)
        {
            Boolean result = false;
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
                throw new ArgumentNullException(nameof(envelope));

            if (this.IsEmpty)
                return new List<Coordinate>();

            return this.root.Search(envelope);
        }

        /// <summary>
        /// Searches the tree for the nearest neighbour of the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The nearest neighbour of the specified coordinate, or <c>null</c> if the tree is empty.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public Coordinate SearchNearest(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));
            if (this.IsEmpty)
                return null;

            return this.root.NearestNeighbourSearch(coordinate);
        }

        /// <summary>
        /// Rebalances the tree.
        /// </summary>
        /// <remarks>
        /// The tree is balanced when created, but adding or removing elements might leave it unbalanced.
        /// The rebalancing requires the complete reconstruction of the tree, therefore it can be a costly operation.
        /// Because of this cost, it is not automatically done after each add or remove, but can be rebalanced at any time when needed.
        /// </remarks>
        public void RebalanceTree()
        {
            List<Coordinate> allCoordinates = new List<Coordinate>(this.NumberOfGeometries);
            this.root.AllCoordinatesFromSubTree(allCoordinates);
            this.Clear();
            this.InitializeTree(allCoordinates);
        }

        /// <summary>
        /// Initializes the tree.
        /// </summary>
        /// <param name="coordinates">The list of coordinates to create the tree from.</param>
        private void InitializeTree(List<Coordinate> coordinates)
        {
            this.NumberOfGeometries = coordinates.Count;

            // If there are no coordinates specified, leave the tree empty.
            if (coordinates.Count == 0)
                return;

            // Sorting the input coordinates by the X coordinate
            coordinates.Sort((p1, p2) => p1.X.CompareTo(p2.X));

            // Setting up the root to be the median by the X coordinate
            Coordinate median = coordinates[coordinates.Count / 2];
            this.root = new KDTreeNode(median, 1, this.TreeDimension);

            // Add all input coordinates to the tree
            this.root.InitializeSubTree(coordinates.GetRange(0, coordinates.Count / 2), coordinates.GetRange(coordinates.Count / 2 + 1, (int)Math.Ceiling(coordinates.Count / 2.0) - 1));
        }
    }
}
