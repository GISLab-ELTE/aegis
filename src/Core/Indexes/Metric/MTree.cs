// <copyright file="MTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes.Metric
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AEGIS.Resources;

    public class MTree<T>
    { 

        public delegate Double DistanceMetric<in DATA>(DATA a, DATA b);

        public interface ISplitPolicy<DATA> : IPromotePolicy<DATA>, IPartitionPolicy<DATA>
        {
        }

        public interface IPromotePolicy<DATA>
        {
            Tuple<DATA, DATA> Promote(ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric);
        }

        public interface IPartitionPolicy<DATA>
        {
            Tuple<ISet<DATA>, ISet<DATA>> Partition(Tuple<DATA, DATA> promoted, ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric);
        }

        public class SplitPolicy<DATA> : ISplitPolicy<DATA>
        {
            private readonly IPromotePolicy<DATA> promotePolicy;
            private readonly IPartitionPolicy<DATA> partitionPolicy;

            public SplitPolicy(IPromotePolicy<DATA> promotePolicy, IPartitionPolicy<DATA> partitionPolicy)
            {
                this.promotePolicy = promotePolicy ?? throw new ArgumentNullException(nameof(promotePolicy), CoreMessages.PromotePolicyIsMissing);
                this.partitionPolicy = partitionPolicy ?? throw new ArgumentNullException(nameof(partitionPolicy), CoreMessages.PartitionPolicyIsMissing);
            }

            public Tuple<ISet<DATA>, ISet<DATA>> Partition(Tuple<DATA, DATA> promoted, ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric)
            {
                return this.partitionPolicy.Partition(promoted, dataSet, distanceMetric);
            }

            public Tuple<DATA, DATA> Promote(ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric)
            {
                return this.promotePolicy.Promote(dataSet, distanceMetric);
            }
        }

        public class GeneralizedHyperplane<DATA> : IPartitionPolicy<DATA>
        {
            public Tuple<ISet<DATA>, ISet<DATA>> Partition(Tuple<DATA, DATA> promoted, ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric)
            {
                ISet<DATA> first = new HashSet<DATA>();
                ISet<DATA> second = new HashSet<DATA>();

                foreach (DATA data in dataSet)
                {
                    if (distanceMetric.Invoke(data, promoted.Item1) <= distanceMetric.Invoke(data, promoted.Item2))
                        first.Add(data);
                    else
                        second.Add(data);
                }

                return new Tuple<ISet<DATA>, ISet<DATA>>(first, second);
            }
        }

        /// <summary>
        /// Represents a node of the M-tree.
        /// </summary>
        protected class Node
        {
            protected class SplitNodeReplacement : Exception
            {
                public Node[] NewNodes { get; private set; }

                public SplitNodeReplacement(params Node[] newNodes)
                {
                    this.NewNodes = newNodes;
                }
            }

            protected class RootNodeReplacement : Exception
            {
                public Node NewRoot { get; private set; }

                public Boolean ItemRemoved { get; private set; }

                public RootNodeReplacement() { }

                public RootNodeReplacement(Node newRoot, Boolean itemRemoved)
                {
                    this.NewRoot = newRoot;
                    this.ItemRemoved = itemRemoved;
                }
            }

            protected class NodeUnderflow : Exception
            {
                public Boolean ItemRemoved { get; private set; }

                public NodeUnderflow(Boolean itemRemoved)
                {
                    this.ItemRemoved = itemRemoved;
                }
            }

            protected class CandidateChild
            {
                public Node Node { get; private set; }

                public Double Distance { get; private set; }

                public Double Metric { get; private set; }

                public CandidateChild(Node node, Double distance, Double metric)
                {
                    this.Node = node;
                    this.Distance = distance;
                    this.Metric = metric;
                }

            }

            protected class ChildWithDistance
            {
                public Node Child { get; private set; }

                public Double Distance { get; private set; }

                public ChildWithDistance(Node child, Double distance)
                {
                    this.Child = child;
                    this.Distance = distance;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node" /> class.
            /// </summary>
            /// <param name="geometry">The data contained by the node.</param>
            /// <param name="parent">The parent of the node.</param>
            public Node(MTree<T> tree, T data)
            {
                this.Tree = tree;
                this.Data = data;
                this.DistanceFromParent = -1;
                this.Radius = 0;
            }


            public T Data { get; private set; }

            public Double Radius { get; private set; }

            public Double DistanceFromParent { get; private set; }

            public MTree<T> Tree { get; private set; }

            public IDictionary<T, Node> Children {
                get
                {
                    if (this.children == null)
                        this.children = new Dictionary<T, Node>();
                    return this.children;
                }
            }

            private IDictionary<T, Node> children;

            /// <summary>
            /// Gets the number of children contained by the node.
            /// </summary>
            /// <value>The number of the children contained by the node.</value>
            public Int32 ChildrenCount { get { return this.Children != null ? this.Children.Count : 0; } }

            public void AddData(T data, Double distance)
            {
                this.DoAddData(data, distance);
                this.CheckMaxCapacity();
            }

            public Boolean RemoveData(T data, Double distance)
            {
                Boolean found = this.DoRemoveData(data, distance);

                if (this.ChildrenCount < this.GetMinCapacity())
                {
                    if (this.Tree.root == this)
                    {
                        IEnumerator<Node> e = this.Children.Values.GetEnumerator();
                        e.MoveNext();
                        Node theChild = e.Current;
                        Node newRoot;
                        if (theChild is LeafNode)
                            newRoot = new LeafNode(this.Tree, theChild.Data);
                        else
                            newRoot = new Node(this.Tree, theChild.Data);

                        foreach (Node grandChild in theChild.Children.Values)
                        {
                            distance = this.Tree.distanceMetric.Invoke(newRoot.Data, grandChild.Data);
                            newRoot.AddChild(grandChild, distance);
                        }

                        theChild.Children.Clear();

                        throw new RootNodeReplacement(newRoot, found);
                    }

                    throw new NodeUnderflow(found);
                }

                return found;
            }

            protected virtual void DoAddData(T data, Double distance)
            {
                CandidateChild minRadiusIncreaseNeeded = new CandidateChild(null, -1.0D, Double.PositiveInfinity);
                CandidateChild nearestDistance = new CandidateChild(null, -1.0D, Double.PositiveInfinity);

                foreach (Node child in this.Children.Values)
                {
                    Double childDistance = this.Tree.distanceMetric.Invoke(child.Data, data);
                    if (childDistance > child.Radius)
                    {
                        Double radiusIncrease = childDistance - child.Radius;
                        if (radiusIncrease < minRadiusIncreaseNeeded.Metric)
                            minRadiusIncreaseNeeded = new CandidateChild(child, childDistance, radiusIncrease);
                    }
                    else
                    {
                        if (childDistance < nearestDistance.Metric)
                            nearestDistance = new CandidateChild(child, childDistance, childDistance);
                    }
                }

                CandidateChild chosen = (nearestDistance.Node != null) ? nearestDistance : minRadiusIncreaseNeeded;

                try
                {
                    chosen.Node.AddData(data, chosen.Distance);
                    this.UpdateRadius(chosen.Node);
                }
                catch (SplitNodeReplacement e)
                {
                    this.Children.Remove(chosen.Node.Data);

                    foreach (Node newChild in e.NewNodes)
                    {
                        distance = this.Tree.distanceMetric.Invoke(this.Data, newChild.Data);
                        this.AddChild(newChild, distance);
                    }
                }
            }

            protected virtual Boolean DoRemoveData(T data, Double distance)
            {
                foreach (Node child in this.Children.Values)
                {
                    if (Math.Abs(distance - child.DistanceFromParent) <= child.Radius)
                    {
                        Double distanceToChild = this.Tree.distanceMetric.Invoke(data, child.Data);
                        if (distanceToChild <= child.Radius)
                        {
                            try
                            {
                                Boolean removed = child.RemoveData(data, distanceToChild);
                                if (removed)
                                    this.UpdateRadius(child);
                                return removed;
                            }
                            catch (NodeUnderflow e)
                            {
                                Node expandedChild = this.BalanceChildren(child);
                                this.UpdateRadius(expandedChild);
                                return e.ItemRemoved;
                            }
                        }
                    }
                }

                return false;
            }

            protected virtual Node NewSplitNodeReplacement(T data)
            {
                return new Node(this.Tree, data);
            }

            protected virtual void AddChild(Node newChild, Double distance)
            {
                Stack<ChildWithDistance> newChildren = new Stack<ChildWithDistance>();
                newChildren.Push(new ChildWithDistance(newChild, distance));

                while(newChildren.Count > 0)
                {
                    ChildWithDistance cwd = newChildren.Pop();
                    newChild = cwd.Child;
                    distance = cwd.Distance;

                    if(this.Children.ContainsKey(newChild.Data))
                    {
                        Node existingChild = this.Children[newChild.Data];

                        foreach(Node grandChild in newChild.Children.Values)
                        {
                            existingChild.AddChild(grandChild, grandChild.DistanceFromParent);
                        }

                        newChild.Children.Clear();

                        try
                        {
                            existingChild.CheckMaxCapacity();
                        }
                        catch(SplitNodeReplacement e)
                        {
                            this.Children.Remove(existingChild.Data);

                            foreach(Node newNode in e.NewNodes)
                            {
                                distance = this.Tree.distanceMetric.Invoke(this.Data, newNode.Data);
                                newChildren.Push(new ChildWithDistance(newNode, distance));
                            }
                        }
                    }
                    else
                    {
                        this.Children.Add(newChild.Data, newChild);
                        this.UpdateMetrics(newChild, distance);
                    }
                }
            }

            protected Int32 GetMinCapacity()
            {
                if (this.Tree.root == this)
                    return this is LeafNode ? 1 : 2;
                return this.Tree.minChildren;
            }

            protected void UpdateMetrics(Node child, Double distance)
            {
                child.DistanceFromParent = distance;
                this.UpdateRadius(child);
            }

            /// <summary>
            /// Tries to find another child which can donate a grand-child to <c>theChild</c>.
            /// </summary>
            /// <param name="theChild">The child.</param>
            /// <returns>the expanded child</returns>
            private Node BalanceChildren(Node theChild)
            {
                Node nearestDonor = null;
                Double distanceNearestDonor = Double.PositiveInfinity;

                Node nearestMergeCandidate = null;
                Double distanceNearestMergeCandidate = Double.PositiveInfinity;

                foreach (Node anotherChild in this.Children.Values)
                {
                    if (anotherChild == theChild)
                        continue;

                    Double distance = this.Tree.distanceMetric.Invoke(theChild.Data, anotherChild.Data);
                    if (anotherChild.ChildrenCount > anotherChild.GetMinCapacity())
                    {
                        if (distance < distanceNearestDonor)
                        {
                            distanceNearestDonor = distance;
                            nearestDonor = anotherChild;
                        }
                    }
                    else
                    {
                        if (distance < distanceNearestMergeCandidate)
                        {
                            distanceNearestMergeCandidate = distance;
                            nearestMergeCandidate = anotherChild;
                        }
                    }
                }

                if (nearestDonor == null)
                {
                    // Merge
                    foreach (Node grandChild in theChild.Children.Values)
                    {
                        Double distance = this.Tree.distanceMetric.Invoke(grandChild.Data, nearestMergeCandidate.Data);
                        nearestMergeCandidate.AddChild(grandChild, distance);
                    }

                    this.Children.Remove(theChild.Data);
                    return nearestMergeCandidate;
                }
                else
                {
                    Node nearestGrandChild = null;
                    Double nearestGrandChildDistance = Double.PositiveInfinity;

                    foreach (Node grandChild in nearestDonor.Children.Values)
                    {
                        Double distance = this.Tree.distanceMetric.Invoke(grandChild.Data, theChild.Data);
                        if (distance < nearestGrandChildDistance)
                        {
                            nearestGrandChildDistance = distance;
                            nearestGrandChild = grandChild;
                        }
                    }

                    nearestDonor.Children.Remove(nearestGrandChild.Data);
                    theChild.AddChild(nearestGrandChild, nearestGrandChildDistance);
                    return theChild;
                }
            }

            private void UpdateRadius(Node child)
            {
                this.Radius = Math.Max(this.Radius, child.DistanceFromParent + child.Radius);
            }

            private void CheckMaxCapacity()
            {
                if(this.ChildrenCount > this.Tree.maxChildren)
                {
                    DistanceMetric<T> distanceMetric = this.Tree.distanceMetric;
                    Tuple<T, T> promotions = this.Tree.splitPolicy.Promote(this.Children.Keys, distanceMetric);
                    Tuple<ISet<T>, ISet<T>> partitions = this.Tree.splitPolicy.Partition(promotions, this.Children.Keys, distanceMetric);

                    Node newNode0 = this.GetNewNode(promotions.Item1, partitions.Item1);
                    Node newNode1 = this.GetNewNode(promotions.Item2, partitions.Item2);

                    throw new SplitNodeReplacement(newNode0, newNode1);
                }
            }

            private Node GetNewNode(T promoted, ISet<T> partition)
            {
                Node newNode = this.NewSplitNodeReplacement(promoted);

                foreach (T data in partition)
                {
                    Node child = this.Children[data];
                    this.Children.Remove(data);
                    Double distance = this.Tree.distanceMetric.Invoke(promoted, data);
                    newNode.AddChild(child, distance);
                }

                return newNode;
            }
        }

        protected class LeafNode : Node
        {
            public LeafNode(MTree<T> tree, T data)
                : base(tree, data) { }

            protected override void AddChild(Node child, double distance)
            {
                if (this.Children.ContainsKey(child.Data))
                    throw new ArgumentException(CoreMessages.ItemWasAlreadyInTree);
                this.Children.Add(child.Data, child);
                this.UpdateMetrics(child, distance);
            }

            protected override void DoAddData(T data, double distance)
            {
                this.AddChild(new Node(this.Tree, data), distance);
            }

            protected override Boolean DoRemoveData(T data, double distance)
            {
                Boolean removed = this.Children.Remove(data);

                if (this.Tree.root == this && this.ChildrenCount < this.GetMinCapacity())
                {
                    throw new RootNodeReplacement();
                }

                return removed;
            }

            protected override Node NewSplitNodeReplacement(T data)
            {
                return new LeafNode(this.Tree, data);
            }
        }

        private const Int32 DefaultMinChildren = 50;

        public MTree(
            Int32 minChildren,
            Int32 maxChildren,
            DistanceMetric<T> distanceMetric,
            ISplitPolicy<T> splitPolicy)
        {
            if (minChildren < 1)
                throw new ArgumentOutOfRangeException(nameof(minChildren), CoreMessages.MinimumNumberOfChildNodesIsLessThan1);
            if (minChildren >= maxChildren)
                throw new ArgumentOutOfRangeException(nameof(maxChildren), CoreMessages.MaximumNumberOfChildNodesIsEqualToMinimum);
            this.minChildren = minChildren;
            this.maxChildren = maxChildren;
            this.distanceMetric = distanceMetric ?? throw new ArgumentNullException(nameof(distanceMetric), CoreMessages.DistanceMetricIsMissing);
            this.splitPolicy = splitPolicy ?? throw new ArgumentNullException(nameof(splitPolicy), CoreMessages.SplitPolicyIsMissing);
        }

        private readonly DistanceMetric<T> distanceMetric;
        private readonly ISplitPolicy<T> splitPolicy;
        private readonly Int32 minChildren;
        private readonly Int32 maxChildren;
        private Node root;
    }
}
