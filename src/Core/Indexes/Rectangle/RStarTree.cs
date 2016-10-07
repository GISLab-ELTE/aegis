// <copyright file="RStarTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Indexes.Rectangle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a 3D R*-Tree, which contains a collection of <see cref="IGeometry" /> instances.
    /// </summary>
    public class RStarTree : RTree
    {
        #region Private fields

        /// <summary>
        /// Indicates the levels where reinsertion happened during an insertion.
        /// </summary>
        private Boolean[] visitedLevels;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RStarTree" /> class.
        /// </summary>
        public RStarTree()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RStarTree" /> class.
        /// </summary>
        /// <param name="minChildren">The minimum number of children contained in a node.</param>
        /// <param name="maxChildren">The maximum number of children contained in a node.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The minimum number of child nodes is less than 1.
        /// or
        /// The maximum number of child nodes is less than or equal to the minimum number of child nodes.
        /// </exception>
        public RStarTree(Int32 minChildren, Int32 maxChildren)
            : base(minChildren, maxChildren) { }

        #endregion

        #region Protected methods

        /// <summary>
        /// Adds a node into the tree on a specified height.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="height">The height where the node should be inserted.</param>
        protected override void AddNode(Node node, Int32 height)
        {
            this.visitedLevels = new Boolean[this.Height];
            this.InsertInSpecifiedHeight(node, height, height);
        }

        /// <summary>
        /// Splits a node into two nodes.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="firstNode">The first produced node.</param>
        /// <param name="secondNode">The second produced node.</param>
        protected override void SplitNode(Node overflownNode, out Node firstNode, out Node secondNode)
        {
            if (overflownNode == null)
            {
                firstNode = null;
                secondNode = null;
                return;
            }

            List<Node> alongChosenAxis = this.ChooseSplitAxis(overflownNode);

            Int32 minIndex = this.MinChildren;
            Double minOverlap = ComputeOverlap(Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, this.MinChildren).Select(x => x.Envelope)),
                                               Envelope.FromEnvelopes(alongChosenAxis.GetRange(this.MinChildren, alongChosenAxis.Count - this.MinChildren).Select(x => x.Envelope)));

            for (Int32 childIndex = this.MinChildren + 1; childIndex <= this.MaxChildren - this.MinChildren + 1; childIndex++)
            {
                Double actOverlap = ComputeOverlap(Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, childIndex).Select(x => x.Envelope)),
                                                   Envelope.FromEnvelopes(alongChosenAxis.GetRange(childIndex, alongChosenAxis.Count - childIndex).Select(x => x.Envelope)));

                if (minOverlap > actOverlap)
                {
                    minOverlap = actOverlap;
                    minIndex = childIndex;
                }
                else if (minOverlap == actOverlap)
                {
                    Double minArea = Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, minIndex).Select(x => x.Envelope)).Surface +
                                     Envelope.FromEnvelopes(alongChosenAxis.GetRange(minIndex, alongChosenAxis.Count - minIndex).Select(x => x.Envelope)).Surface;

                    Double actArea = Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, childIndex).Select(x => x.Envelope)).Surface +
                                     Envelope.FromEnvelopes(alongChosenAxis.GetRange(childIndex, alongChosenAxis.Count - childIndex).Select(x => x.Envelope)).Surface;

                    if (minArea > actArea)
                        minIndex = childIndex;
                }
            }

            firstNode = (overflownNode.Parent != null) ? new Node(overflownNode.Parent) : new Node(overflownNode.MaxChildren);
            secondNode = (overflownNode.Parent != null) ? new Node(overflownNode.Parent) : new Node(overflownNode.MaxChildren);

            foreach (Node node in alongChosenAxis.GetRange(0, minIndex))
                firstNode.AddChild(node);

            foreach (Node node in alongChosenAxis.GetRange(minIndex, alongChosenAxis.Count - minIndex))
                secondNode.AddChild(node);
        }

        /// <summary>
        /// Chooses a node where a new node should be added.
        /// </summary>
        /// <param name="envelope">The envelope of the new node.</param>
        /// <param name="height">The height where the new node should be added.</param>
        /// <returns>The node where the new node should be added.</returns>
        protected override Node ChooseNodeToAdd(Envelope envelope, Int32 height)
        {
            // source: The R*-tree: An Efficient and Robust Access Method for Points and Rectangles, page 4, method name in the paper: ChooseSubTree

            Node node = this.Root;
            Int32 currentHeight = 0;
            while (!node.IsLeafContainer && currentHeight != height)
            {
                // if the child pointer points to leaf containers, determine the minimum overlap cost
                if (node.ChildrenCount > 0 && node.Children[0].IsLeafContainer)
                {
                    // determine the exact minimum overlap cost
                    node = ChooseNode(node.Children, envelope);

                    // another option is to determine the NEARLY minimum overlap cost
                }
                else
                {
                    // determine the minimum area cost
                    Double minimumEnlargement = Double.MaxValue;
                    Double minimumSurface = Double.MaxValue;

                    foreach (Node child in node.Children)
                    {
                        Double enlargement = child.ComputeEnlargement(envelope);
                        if (minimumEnlargement > enlargement)
                        {
                            minimumEnlargement = enlargement;
                            node = child;
                        }
                        else if (minimumEnlargement == enlargement)
                        {
                            Double surface = child.Envelope.Surface;
                            if (minimumSurface > surface)
                            {
                                minimumSurface = surface;
                                node = child;
                            }
                        }
                    }
                }

                currentHeight++;
            }

            return node;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Inserts a node into the tree on a specified height.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="height">The height where the node should be inserted.</param>
        /// <param name="startHeight">If the tree was grown during the reinsert, then the original height of the tree.</param>
        private void InsertInSpecifiedHeight(Node node, Int32 height, Int32 startHeight)
        {
            Node nodeToInsert = this.ChooseNodeToAdd(node.Envelope, height);

            if (nodeToInsert == this.Root && nodeToInsert.ChildrenCount == 0)
                this.Height = 1;

            if (height == -1)
            {
                height = this.Height - 1;
                startHeight = height;
            }

            nodeToInsert.AddChild(node);

            if (nodeToInsert.IsOverflown)
            {
                Boolean canReInsert;
                Boolean splitted;

                do
                {
                    canReInsert = startHeight > 0 && !this.visitedLevels[startHeight];
                    this.visitedLevels[startHeight] = true;

                    splitted = this.OverflowTreatment(nodeToInsert, canReInsert, height) && nodeToInsert.Parent.IsOverflown;

                    nodeToInsert = nodeToInsert.Parent;
                    height--;
                }
                while (splitted);
            }

            if (nodeToInsert != null)
                this.AdjustTree(nodeToInsert);
        }

        /// <summary>
        /// Reinserts or splits an overflown node.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="canReinsert">Indicates whether the elements of the node can reinserted.</param>
        /// <param name="height">The height of the overflown node.</param>
        /// <returns><c>true</c> if splitting was performed; otherwise, <c>false</c>.</returns>
        private Boolean OverflowTreatment(Node overflownNode, Boolean canReinsert, Int32 height)
        {
            if (canReinsert)
            {
                // reinsert
                this.ReInsert(overflownNode, height);
                return false;
            }
            else
            {
                // split
                Node first, second;
                this.SplitNode(overflownNode, out first, out second);

                if (overflownNode.Parent != null)
                {
                    overflownNode.Parent.RemoveChild(overflownNode);
                    overflownNode.Parent.AddChild(first);
                    overflownNode.Parent.AddChild(second);
                }
                else
                {
                    // case when the root is split
                    this.Root = new Node(overflownNode.MaxChildren);
                    this.Root.AddChild(first);
                    this.Root.AddChild(second);
                    this.Height++;
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Reinserts children from the overflown node into the tree.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="height">The height.</param>
        private void ReInsert(Node overflownNode, Int32 height)
        {
            // the number of elements we will reinsert
            // experiments shown that the 30% of the maximum children yields the best performance
            Int32 count = Convert.ToInt32(Math.Round(0.3 * this.MaxChildren));

            List<Node> nodesInOrder = overflownNode.Children.OrderByDescending(x => Coordinate.Distance(x.Envelope.Center, overflownNode.Envelope.Center)).Take(count).ToList();

            for (Int32 index = 0; index < count; index++)
                overflownNode.Children.Remove(nodesInOrder[index]);

            this.AdjustTree(overflownNode);

            Int32 startHeight = this.Height;

            for (Int32 index = 0; index < count; index++)
                this.InsertInSpecifiedHeight(nodesInOrder[index], height + (this.Height - startHeight), height);
        }

        /// <summary>
        /// Chooses the axis along which the split must be performed.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <returns>The nodes sorted by the axis along which the split must be performed</returns>
        private List<Node> ChooseSplitAxis(Node overflownNode)
        {
            Boolean isPlanar = overflownNode.Envelope.IsPlanar;

            Double sumX, sumY, sumZ;
            sumX = sumY = sumZ = 0;

            List<Node> orderedByX = overflownNode.Children.OrderBy(x => x.Envelope.MinX).ThenBy(x => x.Envelope.MaxX).ToList();
            List<Node> orderedByY = overflownNode.Children.OrderBy(x => x.Envelope.MinY).ThenBy(x => x.Envelope.MaxY).ToList();
            List<Node> orderedByZ = null;

            if (!isPlanar)
                orderedByZ = overflownNode.Children.OrderBy(x => x.Envelope.MinX).ThenBy(x => x.Envelope.MaxZ).ToList();

            for (Int32 childIndex = this.MinChildren; childIndex <= this.MaxChildren - this.MinChildren + 1; childIndex++)
            {
                sumX += ComputeMargin(Envelope.FromEnvelopes(orderedByX.GetRange(0, childIndex).Select(x => x.Envelope))) +
                        ComputeMargin(Envelope.FromEnvelopes(orderedByX.GetRange(childIndex, orderedByX.Count - childIndex).Select(x => x.Envelope)));

                sumY += ComputeMargin(Envelope.FromEnvelopes(orderedByY.GetRange(0, childIndex).Select(x => x.Envelope))) +
                        ComputeMargin(Envelope.FromEnvelopes(orderedByY.GetRange(childIndex, orderedByY.Count - childIndex).Select(x => x.Envelope)));

                if (!isPlanar)
                {
                    sumZ += ComputeMargin(Envelope.FromEnvelopes(orderedByZ.GetRange(0, childIndex).Select(x => x.Envelope))) +
                            ComputeMargin(Envelope.FromEnvelopes(orderedByZ.GetRange(childIndex, orderedByZ.Count - childIndex).Select(x => x.Envelope)));
                }
            }

            if (!isPlanar && sumZ < sumX && sumZ < sumY)
                return orderedByZ;

            if (sumX <= sumY)
                return orderedByX;
            else
                return orderedByY;
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Chooses which node should we enlarge with the new envelope to minimize the overlap between the nodes
        /// </summary>
        /// <param name="entries">The nodes from we want to choose.</param>
        /// <param name="newEnvelope">The new envelope.</param>
        /// <returns>The chosen node.</returns>
        private static Node ChooseNode(List<Node> entries, Envelope newEnvelope)
        {
            Node minimalEntry = entries[0];
            Envelope enlargedEnvelope = Envelope.FromEnvelopes(minimalEntry.Envelope, newEnvelope);
            Double minimalOverlap = entries.Sum(x => x != entries[0] ? ComputeOverlap(enlargedEnvelope, x.Envelope) : 0);

            for (Int32 index = 1; index < entries.Count; index++)
            {
                enlargedEnvelope = Envelope.FromEnvelopes(entries[index].Envelope, newEnvelope);
                Double overlap = entries.Sum(x => x != entries[index] ? ComputeOverlap(enlargedEnvelope, x.Envelope) : 0);

                if (overlap < minimalOverlap)
                {
                    minimalOverlap = overlap;
                    minimalEntry = entries[index];
                }
                else if (overlap == minimalOverlap)
                {
                    if (entries[index].ComputeEnlargement(newEnvelope) < minimalEntry.ComputeEnlargement(newEnvelope))
                        minimalEntry = entries[index];
                }
            }

            return minimalEntry;
        }

        /// <summary>
        /// Computes the margin of an envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The margin.</returns>
        private static Double ComputeMargin(Envelope envelope)
        {
            return 2 * ((envelope.MaxX - envelope.MinX) + (envelope.MaxY - envelope.MinY) + (envelope.MaxZ - envelope.MinZ));
        }

        /// <summary>
        /// Computes the overlap of two envelopes.
        /// </summary>
        /// <param name="first">The first envelope.</param>
        /// <param name="second">The second envelope.</param>
        /// <returns>Overlap of the two envelopes</returns>
        private static Double ComputeOverlap(Envelope first, Envelope second)
        {
            Double minX, minY, minZ, maxX, maxY, maxZ;

            minX = Math.Max(first.MinX, second.MinX);
            minY = Math.Max(first.MinY, second.MinY);
            minZ = Math.Max(first.MinZ, second.MinZ);
            maxX = Math.Min(first.MaxX, second.MaxX);
            maxY = Math.Min(first.MaxY, second.MaxY);
            maxZ = Math.Min(first.MaxZ, second.MaxZ);

            if (minX > maxX || minY > maxY || minZ > maxZ)
                return 0;

            if (minZ == maxZ)
                return (maxX - minX) * (maxY - minY);
            else
                return (2 * (maxX - minX) * (maxY - minY)) + (2 * (maxX - minX) * (maxZ - minZ)) + (2 * (maxY - minY) * (maxZ - minZ));
        }

        #endregion
    }
}
