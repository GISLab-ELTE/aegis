// <copyright file="RTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes.Rectangle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a 3D R-Tree, which contains a collection of <see cref="IBasicGeometry" /> instances.
    /// </summary>
    public class RTree : ISpatialIndex
    {
        /// <summary>
        /// Represents a node of the R-tree.
        /// </summary>
        protected class Node
        {
            /// <summary>
            /// The envelope of the node.
            /// </summary>
            private Envelope envelope;

            /// <summary>
            /// The parent node.
            /// </summary>
            private Node parent;

            /// <summary>
            /// Initializes a new instance of the <see cref="Node" /> class.
            /// </summary>
            /// <param name="maxChildren">The maximum number of entries that can be contained in the node.</param>
            public Node(Int32 maxChildren)
            {
                this.MaxChildren = maxChildren;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node" /> class.
            /// </summary>
            /// <param name="parent">The parent of the node.</param>
            public Node(Node parent)
            {
                this.Parent = parent;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node" /> class.
            /// </summary>
            /// <param name="geometry">The <see cref="IBasicGeometry" /> contained by the node.</param>
            /// <param name="parent">The parent of the node.</param>
            public Node(IBasicGeometry geometry, Node parent = null)
                : this(parent)
            {
                this.Geometry = geometry;
            }

            /// <summary>
            /// Gets or sets the parent node.
            /// </summary>
            /// <value>The parent node.</value>
            public Node Parent
            {
                get
                {
                    return this.parent;
                }

                set
                {
                    this.parent = value;
                    if (this.MaxChildren == 0 && this.parent != null)
                        this.MaxChildren = this.parent.MaxChildren;
                }
            }

            /// <summary>
            /// Gets the child nodes.
            /// </summary>
            /// <value>The list of child nodes.</value>
            public List<Node> Children { get; private set; }

            /// <summary>
            /// Gets the geometry contained in the node.
            /// </summary>
            /// <value>The geometry contained in the node if the node is a leaf node; otherwise, <c>null</c>.</value>
            public IBasicGeometry Geometry { get; private set; }

            /// <summary>
            /// Gets the minimum bounding envelope of the node.
            /// </summary>
            /// <value>The minimum bounding envelope of all descendant geometries.</value>
            public Envelope Envelope
            {
                get
                {
                    return this.Geometry != null ? this.Geometry.Envelope : this.envelope;
                }

                private set
                {
                    this.envelope = value;
                }
            }

            /// <summary>
            /// Gets the maximum number of the children that can be contained by the node.
            /// </summary>
            /// <value>The maximum number of the children that can be contained by the node.</value>
            public Int32 MaxChildren { get; private set; }

            /// <summary>
            /// Gets the number of children contained by the node.
            /// </summary>
            /// <value>The number of the children contained by the node.</value>
            public Int32 ChildrenCount { get { return this.Children != null ? this.Children.Count : 0; } }

            /// <summary>
            /// Gets a value indicating whether the node contains only leaf nodes.
            /// </summary>
            /// <value><c>true</c> if the node is a leaf, or has leaf children; otherwise, <c>false</c>.</value>
            public Boolean IsLeafContainer { get { return !this.IsLeaf && (this.Children == null || this.Children[0].IsLeaf); } }

            /// <summary>
            /// Gets a value indicating whether the node is a leaf node.
            /// </summary>
            /// <value><c>true</c> if the node contains a  otherwise, <c>false</c>.</value>
            public Boolean IsLeaf { get { return this.Geometry != null; } }

            /// <summary>
            /// Gets a value indicating whether the node is full.
            /// </summary>
            /// <value><c>true</c> if the node contains the maximum number of elements; otherwise, <c>false</c>.</value>
            public Boolean IsFull { get { return this.ChildrenCount == this.MaxChildren; } }

            /// <summary>
            /// Gets a value indicating whether the node is overflown.
            /// </summary>
            /// <value><c>true</c> if the node contains more than the maximum number of elements; otherwise, <c>false</c>.
            /// </value>
            public Boolean IsOverflown { get { return this.ChildrenCount > this.MaxChildren; } }

            /// <summary>
            /// Adds a new child to the node.
            /// </summary>
            /// <param name="child">The child node.</param>
            public void AddChild(Node child)
            {
                if (child == null)
                    return;

                child.Parent = this;
                if (this.Children == null)
                    this.Children = new List<Node>(this.MaxChildren);

                this.Children.Add(child);
                this.CorrectBounding(child.Envelope);
            }

            /// <summary>
            /// Removes a child of the node.
            /// </summary>
            /// <param name="node">The node to be removed.</param>
            public void RemoveChild(Node node)
            {
                this.Children.Remove(node);

                if (this.ChildrenCount == 0)
                    this.Children = null;

                this.CorrectBounding();
            }

            /// <summary>
            /// Corrects the node's minimum bounding envelope.
            /// </summary>
            /// <param name="enlargingEnvelope">The envelope the node needs to be extended with, or <c>null</c>, if the envelope needs to be shrank.</param>
            public void CorrectBounding(Envelope enlargingEnvelope = null)
            {
                if (enlargingEnvelope == null)
                {
                    // possible element removing from the children
                    this.Envelope = (this.ChildrenCount > 0) ? Envelope.FromEnvelopes(this.Children.Select(x => x.Envelope)) : null;
                    return;
                }

                if (this.Envelope == null)
                    this.Envelope = enlargingEnvelope;
                else if (!this.Envelope.Contains(enlargingEnvelope))
                    this.Envelope = Envelope.FromEnvelopes(new Envelope[] { this.Envelope, enlargingEnvelope });
            }

            /// <summary>
            /// Computes how much the envelope's surface is enlarged with the parameter envelope.
            /// </summary>
            /// <param name="envelope">The envelope the node needs to be enlarged with.</param>
            /// <returns>The enlargement.</returns>
            public Double ComputeEnlargement(Envelope envelope)
            {
                if (this.Envelope.Contains(envelope))
                    return 0;

                Envelope enlarged = Envelope.FromEnvelopes(new Envelope[] { this.Envelope, envelope });

                return enlarged.Surface - this.Envelope.Surface;
            }
        }

        /// <summary>
        /// The default minimum child count. This field is constant.
        /// </summary>
        private const Int32 DefaultMinChildCount = 8;

        /// <summary>
        /// The default maximum child count. This field is constant.
        /// </summary>
        private const Int32 DefaultMaxChildCount = 12;

        /// <summary>
        /// Initializes a new instance of the <see cref="RTree" /> class.
        /// </summary>
        public RTree()
            : this(DefaultMinChildCount, DefaultMaxChildCount) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RTree" /> class.
        /// </summary>
        /// <param name="minChildren">The minimum number of child nodes.</param>
        /// <param name="maxChildren">The maximum number of child nodes.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The minimum number of child nodes is less than 1.
        /// or
        /// The maximum number of child nodes is equal to or less than the minimum number of child nodes.
        /// </exception>
        public RTree(Int32 minChildren, Int32 maxChildren)
        {
            if (minChildren < 1)
                throw new ArgumentOutOfRangeException(nameof(minChildren), CoreMessages.MinimumNumberOfChildNodesIsLessThan1);
            if (minChildren >= maxChildren)
                throw new ArgumentOutOfRangeException(nameof(maxChildren), CoreMessages.MaximumNumberOfChildNodesIsEqualToMinimum);

            this.Root = new Node(maxChildren);
            this.NumberOfGeometries = 0;
            this.Height = 0;
            this.MinChildren = minChildren;
        }

        /// <summary>
        /// Gets a value indicating whether the index is read-only.
        /// </summary>
        /// <value><c>true</c> if the index is read-only; otherwise, <c>false</c>.</value>
        public Boolean IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets the number of indexed geometries.
        /// </summary>
        /// <value>The number of indexed geometries.</value>
        public Int32 NumberOfGeometries { get; private set; }

        /// <summary>
        /// Gets the maximum number of children contained in a node.
        /// </summary>
        /// <value>The maximum number of children contained in a node.</value>
        public Int32 MaxChildren { get { return this.Root.MaxChildren; } }

        /// <summary>
        /// Gets the minimum number of children contained in a node.
        /// </summary>
        /// <value>The minimum number of children contained in a node.</value>
        public Int32 MinChildren { get; private set; }

        /// <summary>
        /// Gets or sets gets the height of the tree.
        /// </summary>
        /// <value>The number of levels in the tree under the root node.</value>
        public Int32 Height { get; protected set; }

        /// <summary>
        /// Gets or sets gets the root node.
        /// </summary>
        protected Node Root { get; set; }

        /// <summary>
        /// Adds a geometry to the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public void Add(IBasicGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry));

            this.AddNode(new Node(geometry));
            this.NumberOfGeometries++;
        }

        /// <summary>
        /// Adds multiple geometries to the index.
        /// </summary>
        /// <param name="collection">The geometry collection.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public void Add(IEnumerable<IBasicGeometry> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (IBasicGeometry geometry in collection)
            {
                if (geometry != null)
                {
                    this.AddNode(new Node(geometry));
                    this.NumberOfGeometries++;
                }
            }
        }

        /// <summary>
        /// Searches the index for any geometries contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of geometries located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public IEnumerable<IBasicGeometry> Search(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return this.SearchNode(this.Root, envelope);
        }

        /// <summary>
        /// Determines whether the specified geometry is indexed.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the specified geometry is indexed; otherwise <c>false</c>.</returns>
        public virtual Boolean Contains(IBasicGeometry geometry)
        {
            if (geometry == null)
                return false;

            return this.ContainsGeometry(geometry, this.Root);
        }

        /// <summary>
        /// Removes the specified geometry from the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the geometry is indexed; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public virtual Boolean Remove(IBasicGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry));

            return this.RemoveGeometry(geometry);
        }

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Remove(Envelope envelope)
        {
            List<IBasicGeometry> geometries;

            return this.Remove(envelope, out geometries);
        }

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="geometries">The list of geometries within the envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Remove(Envelope envelope, out List<IBasicGeometry> geometries)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            geometries = this.SearchNode(this.Root, envelope).ToList();

            if (!geometries.Any())
                return false;

            foreach (IBasicGeometry geometry in geometries)
            {
                this.RemoveGeometry(geometry);
            }

            return true;
        }

        /// <summary>
        /// Clears all geometries from the index.
        /// </summary>
        public void Clear()
        {
            this.Root = new Node(this.Root.MaxChildren);
            this.Height = 0;
            this.NumberOfGeometries = 0;
        }

        /// <summary>
        /// Adds a node into the tree on a specified height.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="height">The height where the node should be inserted.</param>
        protected virtual void AddNode(Node node, Int32 height = -1)
        {
            Node nodeToInsert = this.ChooseNodeToAdd(node.Envelope, height);

            if (!nodeToInsert.IsFull)
            {
                if (nodeToInsert == this.Root && nodeToInsert.ChildrenCount == 0)
                    this.Height = 1;

                nodeToInsert.AddChild(node);
                this.AdjustTree(nodeToInsert);
            }
            else
            {
                nodeToInsert.AddChild(node);

                Node firstNode, secondNode;
                this.SplitNode(nodeToInsert, out firstNode, out secondNode);
                this.AdjustTree(firstNode, secondNode, nodeToInsert);
            }
        }

        /// <summary>
        /// Collects all geometries inside an envelope.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of geometries which are inside the envelope.</returns>
        protected IEnumerable<IBasicGeometry> SearchNode(Node node, Envelope envelope)
        {
            if (node.Children == null)
                yield break;

            if (node.IsLeafContainer)
            {
                foreach (Node leaf in node.Children)
                {
                    if (envelope.Contains(leaf.Geometry.Envelope))
                        yield return leaf.Geometry;
                }
            }
            else
            {
                foreach (Node child in node.Children)
                {
                    if (envelope.Intersects(child.Envelope))
                    {
                        foreach (IBasicGeometry geometry in this.SearchNode(child, envelope))
                            yield return geometry;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the tree contains a geometry in a specified subtree.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="node">The root node of the subtree.</param>
        /// <returns><c>true</c>, if the element contains the geometry, otherwise, <c>false</c>.</returns>
        protected Boolean ContainsGeometry(IBasicGeometry geometry, Node node)
        {
            if (node.IsLeafContainer && node.Children != null && node.Children.Any(x => x.Geometry == geometry))
                return true;

            Boolean result = false;

            for (Int32 childIndex = 0; childIndex < node.ChildrenCount && !result; childIndex++)
            {
                if (!node.Envelope.Contains(geometry.Envelope))
                    continue;

                result = this.ContainsGeometry(geometry, node.Children[childIndex]);
            }

            return result;
        }

        /// <summary>
        /// Removes the specified geometry from the tree.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c>, if the tree contains the geometry, otherwise, <c>false</c>.</returns>
        protected virtual Boolean RemoveGeometry(IBasicGeometry geometry)
        {
            Node leafContainer = null;
            this.FindLeafContainer(geometry, this.Root, ref leafContainer);

            if (leafContainer == null)
                return false;

            Node nodeToRemove = leafContainer.Children.First(x => x.Geometry == geometry);

            leafContainer.RemoveChild(nodeToRemove);

            this.CondenseTree(leafContainer);

            while (this.Root.ChildrenCount == 1 && !this.Root.IsLeafContainer)
            {
                this.Root = this.Root.Children[0];
                this.Root.Parent = null;
                this.Height--;
            }

            if (this.Root.ChildrenCount == 0) // occurs when the last element is removed
                this.Clear();

            this.NumberOfGeometries--;
            return true;
        }

        /// <summary>
        /// Adjusts the tree after insertion, and corrects the bounding envelope of the nodes.
        /// </summary>
        /// <param name="node">The node where the adjustment starts.</param>
        /// <param name="splitted">The second part of the node if the original node was split.</param>
        /// <param name="nodeToRemove">The original node which should be removed if the original node was split.</param>
        protected void AdjustTree(Node node, Node splitted = null, Node nodeToRemove = null)
        {
            Node firstNode = node;
            Node secondNode = splitted;

            while (firstNode.Parent != null)
            {
                Node parent = firstNode.Parent;

                parent.CorrectBounding(firstNode.Envelope);

                if (secondNode == null)
                {
                    firstNode = parent;
                }
                else
                {
                    if (nodeToRemove != null)
                    {
                        parent.RemoveChild(nodeToRemove);
                        parent.AddChild(firstNode);
                    }

                    if (!parent.IsFull)
                    {
                        parent.AddChild(secondNode);
                        firstNode = parent;
                        secondNode = null;
                    }
                    else
                    {
                        parent.AddChild(secondNode);
                        this.SplitNode(parent, out firstNode, out secondNode);

                        nodeToRemove = parent;
                    }
                }
            }

            // create new root node if the root is split
            if (secondNode != null)
            {
                this.Root = new Node(firstNode.MaxChildren);
                this.Root.AddChild(firstNode);
                this.Root.AddChild(secondNode);

                this.Height++;
            }
        }

        /// <summary>
        /// Splits a node into two nodes.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="firstNode">The first produced node.</param>
        /// <param name="secondNode">The second produced node.</param>
        protected virtual void SplitNode(Node overflownNode, out Node firstNode, out Node secondNode)
        {
            firstNode = null;
            secondNode = null;

            if (overflownNode == null)
                return;

            Node firstSeed, secondSeed;
            this.PickSeeds(overflownNode.Children, out firstSeed, out secondSeed);

            firstNode = (overflownNode.Parent != null) ? new Node(overflownNode.Parent) : new Node(overflownNode.MaxChildren);
            secondNode = (overflownNode.Parent != null) ? new Node(overflownNode.Parent) : new Node(overflownNode.MaxChildren);

            firstNode.AddChild(firstSeed);
            secondNode.AddChild(secondSeed);

            overflownNode.Children.Remove(firstSeed);
            overflownNode.Children.Remove(secondSeed);

            while (overflownNode.ChildrenCount > 0)
            {
                Node node = this.PickNext(overflownNode.Children);

                if (firstNode.ChildrenCount + overflownNode.ChildrenCount <= this.MinChildren)
                {
                    firstNode.AddChild(node);
                }
                else if (secondNode.ChildrenCount + overflownNode.ChildrenCount <= this.MinChildren)
                {
                    secondNode.AddChild(node);
                }
                else
                {
                    Double firstEnlargement = firstNode.ComputeEnlargement(node.Envelope);
                    Double secondEnlargement = secondNode.ComputeEnlargement(node.Envelope);

                    if (firstEnlargement < secondEnlargement)
                    {
                        firstNode.AddChild(node);
                    }
                    else if (firstEnlargement > secondEnlargement)
                    {
                        secondNode.AddChild(node);
                    }
                    else
                    {
                        if (firstNode.Envelope.Surface < secondNode.Envelope.Surface)
                            firstNode.AddChild(node);
                        else
                            secondNode.AddChild(node);
                    }
                }
            }
        }

        /// <summary>
        /// Chooses seed elements for splitting a node using the linear cost algorithm.
        /// </summary>
        /// <param name="nodes">The nodes contained by the overflown node.</param>
        /// <param name="firstNode">The first seed node.</param>
        /// <param name="secondNode">The second seed node.</param>
        protected virtual void PickSeeds(IList<Node> nodes, out Node firstNode, out Node secondNode)
        {
            // the linear cost algorithm chooses the two points which are
            // the furthest away from each other considering the three axis

            Node highestLowX = nodes[0];
            Node highestLowY = nodes[0];
            Node highestLowZ = nodes[0];

            for (Int32 nodeIndex = 1; nodeIndex < nodes.Count; nodeIndex++)
            {
                if (highestLowX.Envelope.MinX < nodes[nodeIndex].Envelope.MinX)
                    highestLowX = nodes[nodeIndex];
                if (highestLowY.Envelope.MinY < nodes[nodeIndex].Envelope.MinY)
                    highestLowY = nodes[nodeIndex];
                if (highestLowZ.Envelope.MinZ < nodes[nodeIndex].Envelope.MinZ)
                    highestLowZ = nodes[nodeIndex];
            }

            Node lowestHighX = nodes.FirstOrDefault(x => x != highestLowX);
            Node lowestHighY = nodes.FirstOrDefault(x => x != highestLowY);
            Node lowestHighZ = nodes.FirstOrDefault(x => x != highestLowZ);

            Double minX, minY, minZ;
            minX = minY = minZ = Double.MaxValue;

            Double maxX, maxY, maxZ;
            maxX = maxY = maxZ = Double.MinValue;

            foreach (Node node in nodes)
            {
                if (node.Envelope.MaxX < lowestHighX.Envelope.MaxX && node != highestLowX)
                    lowestHighX = node;

                if (node.Envelope.MaxY < lowestHighY.Envelope.MaxY && node != highestLowY)
                    lowestHighY = node;

                if (node.Envelope.MaxZ < lowestHighZ.Envelope.MaxZ && node != highestLowZ)
                    lowestHighZ = node;

                if (node.Envelope.MinX < minX)
                    minX = node.Envelope.MinX;
                if (node.Envelope.MaxX > maxX)
                    maxX = node.Envelope.MaxX;

                if (node.Envelope.MinY < minY)
                    minY = node.Envelope.MinY;
                if (node.Envelope.MaxY > maxY)
                    maxY = node.Envelope.MaxY;

                if (node.Envelope.MinZ < minZ)
                    minZ = node.Envelope.MinZ;
                if (node.Envelope.MaxZ > maxZ)
                    maxZ = node.Envelope.MaxZ;
            }

            Double normalizedDistanceX = (highestLowX.Envelope.MinX - lowestHighX.Envelope.MaxX) / (maxX - minX);
            Double normalizedDistanceY = (highestLowY.Envelope.MinY - lowestHighY.Envelope.MaxY) / (maxY - minY);

            if (normalizedDistanceX >= normalizedDistanceY)
            {
                firstNode = lowestHighX;
                secondNode = highestLowX;
            }
            else
            {
                firstNode = lowestHighY;
                secondNode = highestLowY;
            }

            if (!lowestHighZ.Envelope.IsPlanar || !highestLowZ.Envelope.IsPlanar)
            {
                Double normalizedDistanceZ = (highestLowZ.Envelope.MinZ - lowestHighZ.Envelope.MaxZ) / (maxZ - minZ);

                if ((firstNode == lowestHighX && normalizedDistanceZ > normalizedDistanceX) ||
                    (firstNode == lowestHighY && normalizedDistanceZ > normalizedDistanceX))
                {
                    firstNode = lowestHighZ;
                    secondNode = highestLowZ;
                }
            }
        }

        /// <summary>
        /// Chooses the next node for uploading the split nodes with the original node's children.
        /// </summary>
        /// <param name="nodes">The children of the original node.</param>
        /// <returns>The chosen next node.</returns>
        protected virtual Node PickNext(IList<Node> nodes)
        {
            Node result = nodes[0];
            nodes.RemoveAt(0);
            return result;
        }

        /// <summary>
        /// Chooses a node where a new node should be added.
        /// </summary>
        /// <param name="envelope">The envelope of the new node.</param>
        /// <param name="height">The height where the new node should be added.</param>
        /// <returns>The node where the new node should be added.</returns>
        protected virtual Node ChooseNodeToAdd(Envelope envelope, Int32 height)
        {
            Node currentNode = this.Root;
            Int32 currentHeight = 0;

            while (!currentNode.IsLeafContainer && currentHeight != height)
            {
                Double minimumEnlargement = Double.MaxValue;
                Double minimumSurface = Double.MaxValue;
                Node selectedLeaf = null;
                foreach (Node child in currentNode.Children)
                {
                    Double enlargement = child.ComputeEnlargement(envelope);
                    if (minimumEnlargement > enlargement)
                    {
                        minimumEnlargement = enlargement;
                        selectedLeaf = child;
                    }
                    else if (minimumEnlargement == enlargement)
                    {
                        Double surface = child.Envelope.Surface;
                        if (minimumSurface > surface)
                        {
                            minimumSurface = surface;
                            selectedLeaf = child;
                        }
                    }
                }

                currentNode = selectedLeaf;
                currentHeight++;
            }

            return currentNode;
        }

        /// <summary>
        /// Searches the leaf container which contains the leaf node.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="node">The node where the search starts.</param>
        /// <param name="resultLeafContainer">The found leaf container.</param>
        private void FindLeafContainer(IBasicGeometry geometry, Node node, ref Node resultLeafContainer)
        {
            if (!node.IsLeafContainer)
            {
                foreach (Node child in node.Children)
                {
                    if (child.Envelope.Contains(geometry.Envelope))
                        this.FindLeafContainer(geometry, child, ref resultLeafContainer);
                }
            }
            else
            {
                foreach (Node child in node.Children)
                {
                    if (child.Geometry == geometry && resultLeafContainer == null)
                        resultLeafContainer = node;
                }
            }
        }

        /// <summary>
        /// Condenses the tree after removing a leaf node.
        /// </summary>
        /// <param name="leafContainerNode">The node which contained the removed node.</param>
        private void CondenseTree(Node leafContainerNode)
        {
            Dictionary<Node, Int32> deletedNodes = new Dictionary<Node, Int32>();
            Node currentNode = leafContainerNode;

            Int32 height = this.Height - 1;

            while (currentNode.Parent != null)
            {
                Node parent = currentNode.Parent;
                if (currentNode.ChildrenCount < this.MinChildren)
                {
                    parent.RemoveChild(currentNode);

                    if (currentNode.Children != null)
                    {
                        foreach (Node node in currentNode.Children)
                            deletedNodes.Add(node, height);
                    }
                }
                else
                {
                    currentNode.CorrectBounding();
                }

                currentNode = parent;
                height--;
            }

            Int32 startHeight = this.Height;
            foreach (KeyValuePair<Node, Int32> deletedNode in deletedNodes)
                this.AddNode(deletedNode.Key, deletedNode.Value + (this.Height - startHeight));
        }
    }
}
