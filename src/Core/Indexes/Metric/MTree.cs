// <copyright file="MTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes.Metric
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using AEGIS.Indexes.Metric.SplitPolicy;
    using AEGIS.Resources;

    /// <summary>
    /// Represents an arbitrary dimensional M-Tree which can hold any type of data with a distance metric defined.
    /// </summary>
    /// <typeparam name="T">type of data to be indexed</typeparam>
    public class MTree<T>
    {
        /// <summary>
        /// Represents a result item from a search query.
        /// </summary>
        /// <typeparam name="DATA">The type of the data contained.</typeparam>
        public class ResultItem<DATA>
        {
            /// <summary>
            /// Gets the distance from the data point which was the query item during the search.
            /// </summary>
            /// <value>
            /// The distance from the query item.
            /// </value>
            public Double Distance { get; private set; }

            /// <summary>
            /// Gets the result item.
            /// </summary>
            /// <value>
            /// The result item.
            /// </value>
            public DATA Item { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ResultItem{DATA}"/> class.
            /// </summary>
            /// <param name="item">The item of the result (the data point in the tree).</param>
            /// <param name="distance">The distance of <c>item</c> from the query item used in the search.</param>
            public ResultItem(DATA item, Double distance)
            {
                this.Item = item;
                this.Distance = distance;
            }
        }

        /// <summary>
        /// Implements a lazy loaded search query by implementing the <see cref="IEnumerable"/> interface.
        ///
        /// Results are loaded while iterating over an instance of this class.
        /// </summary>
        private class SearchQuery : IEnumerable<ResultItem<T>>
        {
            public SearchQuery(MTree<T> tree, T data, Double range, Int32 limit)
            {
                this.tree = tree;
                this.data = data;
                this.range = range;
                this.limit = limit;
            }

            private readonly MTree<T> tree;
            private readonly T data;
            private readonly Double range;
            private readonly Int32 limit;

            private class ResultEnumerator : IEnumerator<ResultItem<T>>
            {
                private class ItemWithDistances : IComparable<ItemWithDistances>
                {
                    public Node Item { get; private set; }

                    public Double Distance { get; private set; }

                    public Double MinDistance { get; private set; }

                    public ItemWithDistances(Node item, Double distance, Double minDistance)
                    {
                        this.Item = item;
                        this.Distance = distance;
                        this.MinDistance = minDistance;
                    }

                    public Int32 CompareTo(ItemWithDistances other)
                    {
                        if (this.MinDistance < other.MinDistance)
                            return -1;
                        if (this.MinDistance > other.MinDistance)
                            return 1;
                        return 0;
                    }
                }

                private readonly SearchQuery searchQuery;

                private ResultItem<T> nextResultItem;
                private Boolean finished;
                private List<ItemWithDistances> pendingList;
                private Double nextPendingMinDistance;
                private List<ItemWithDistances> nearestList;
                private Int32 yieldedCount;

                public ResultEnumerator(SearchQuery searchQuery)
                {
                    this.searchQuery = searchQuery;
                    this.Reset();
                }

                public ResultItem<T> Current { get { return this.nextResultItem; } }

                object IEnumerator.Current { get { return this.nextResultItem; } }

                public void Dispose()
                {
                    // do nothing
                }

                public Boolean MoveNext()
                {
                    if (this.finished)
                        return false;

                    this.FetchNext();

                    if (this.finished)
                        return false;

                    return true;
                }

                public void Reset()
                {
                    this.nextResultItem = null;
                    this.finished = false;
                    this.pendingList = new List<ItemWithDistances>();
                    this.nearestList = new List<ItemWithDistances>();
                    this.yieldedCount = 0;

                    if (this.searchQuery.tree.root == null)
                    {
                        this.finished = true;
                        return;
                    }

                    Double distance = this.searchQuery.tree.distanceMetric.Invoke(this.searchQuery.data, this.searchQuery.tree.root.Data);
                    Double minDistance = Math.Max(distance - this.searchQuery.tree.root.Radius, 0.0D);

                    this.pendingList.Add(new ItemWithDistances(this.searchQuery.tree.root, distance, minDistance));
                    this.nextPendingMinDistance = minDistance;
                }

                private void FetchNext()
                {
                    if (this.finished || this.yieldedCount >= this.searchQuery.limit)
                    {
                        this.finished = true;
                        return;
                    }

                    while (this.pendingList.Count > 0 || this.nearestList.Count > 0)
                    {
                        if (this.PrepareNextNearest())
                        {
                            return;
                        }

                        ItemWithDistances pending = this.pendingList[0];
                        this.pendingList.RemoveAt(0);
                        Node node = pending.Item;

                        foreach (Node child in node.Children.Values)
                        {
                            if (Math.Abs(pending.Distance - child.DistanceFromParent) - child.Radius <= this.searchQuery.range)
                            {
                                Double childDistance = this.searchQuery.tree.distanceMetric.Invoke(this.searchQuery.data, child.Data);
                                Double childMinDistance = Math.Max(childDistance - child.Radius, 0.0D);
                                if (childMinDistance <= this.searchQuery.range)
                                {
                                    if (child.IsEntry)
                                        this.AddToListInOrder(this.nearestList, new ItemWithDistances(child, childDistance, childMinDistance));
                                    else
                                        this.AddToListInOrder(this.pendingList, new ItemWithDistances(child, childDistance, childMinDistance));
                                }
                            }
                        }

                        if (this.pendingList.Count == 0)
                            this.nextPendingMinDistance = Double.PositiveInfinity;
                        else
                            this.nextPendingMinDistance = this.pendingList[0].MinDistance;
                    }

                    this.finished = true;
                }

                private void AddToListInOrder(List<ItemWithDistances> list, ItemWithDistances newItem)
                {
                    Int32 index = list.FindIndex(item => item.MinDistance > newItem.MinDistance);
                    if (index >= 0)
                        list.Insert(index, newItem);
                    else
                        list.Add(newItem);
                }

                private Boolean PrepareNextNearest()
                {
                    if (this.nearestList.Count > 0)
                    {
                        ItemWithDistances nextNearest = this.nearestList[0];
                        if (nextNearest.Distance <= this.nextPendingMinDistance)
                        {
                            this.nearestList.RemoveAt(0);
                            this.nextResultItem = new ResultItem<T>(nextNearest.Item.Data, nextNearest.Distance);
                            this.yieldedCount++;
                            return true;
                        }
                    }

                    return false;
                }
            }

            public IEnumerator<ResultItem<T>> GetEnumerator()
            {
                return new ResultEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ResultEnumerator(this);
            }
        }

        /// <summary>
        /// Thrown when a node overflows during adding a child node.
        /// </summary>
        private class SplitNodeReplacement : Exception
        {
            public Node[] NewNodes { get; private set; }

            public SplitNodeReplacement(params Node[] newNodes)
            {
                this.NewNodes = newNodes;
            }
        }

        /// <summary>
        /// Thorwn when the root node needs to be replaced because of an underflow propagated to the root item.
        /// </summary>
        private class RootNodeReplacement : Exception
        {
            public Node NewRoot { get; private set; }

            public Boolean ItemRemoved { get; private set; }

            public RootNodeReplacement(Boolean itemRemoved)
                : this(null, itemRemoved) { }

            public RootNodeReplacement(Node newRoot, Boolean itemRemoved)
            {
                this.NewRoot = newRoot;
                this.ItemRemoved = itemRemoved;
            }
        }

        /// <summary>
        /// Represents a node of the M-tree.
        /// </summary>
        private class Node
        {
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
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="tree">the tree which contains this <see cref="Node"/></param>
            /// <param name="data">the data to be contained within this node (can be actual or routing object)</param>
            /// <param name="isEntry"><c>true</c> if this <see cref="Node"/> holds an actual object (not a routing object)</param>
            public Node(MTree<T> tree, T data, Boolean isEntry = false)
            {
                this.Tree = tree;
                this.Data = data;
                this.DistanceFromParent = -1;
                this.Radius = 0;
                this.IsEntry = isEntry;
            }

            public T Data { get; private set; }

            public Double Radius { get; private set; }

            public Double DistanceFromParent { get; private set; }

            public MTree<T> Tree { get; private set; }

            public Boolean IsEntry { get; private set; }

            public IDictionary<T, Node> Children
            {
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

            public virtual void AddChild(Node newChild, Double distance)
            {
                Stack<ChildWithDistance> newChildren = new Stack<ChildWithDistance>();
                newChildren.Push(new ChildWithDistance(newChild, distance));

                while (newChildren.Count > 0)
                {
                    ChildWithDistance cwd = newChildren.Pop();
                    newChild = cwd.Child;
                    distance = cwd.Distance;

                    if (this.Children.ContainsKey(newChild.Data))
                    {
                        Node existingChild = this.Children[newChild.Data];

                        foreach (Node grandChild in newChild.Children.Values)
                        {
                            existingChild.AddChild(grandChild, grandChild.DistanceFromParent);
                        }

                        newChild.Children.Clear();

                        try
                        {
                            existingChild.CheckMaxCapacity();
                        }
                        catch (SplitNodeReplacement e)
                        {
                            this.Children.Remove(existingChild.Data);

                            foreach (Node newNode in e.NewNodes)
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

            protected Int32 GetMinCapacity()
            {
                if (this.Tree.root == this)
                    return this is LeafNode ? 1 : 2;
                return this.Tree.MinChildren;
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
                if (this.ChildrenCount > this.Tree.MaxChildren)
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

        /// <summary>
        /// Represents a leaf node of the M-Tree.
        /// </summary>
        /// <remarks>
        ///     Leaf nodes contain "entries".
        ///     Entries are <see cref="Node"/> instances, and their only purpose is to hold data points which are injected by the user.
        ///     The structure therefore is as follows: The bottom of the tree holds <c>Entries</c>. Entries are instances of <see cref="Node"/>.
        ///     They can be distinguished from other types of nodes using the <see cref="Node.IsEntry"/> property.
        ///     Entries are kept in <see cref="LeafNode"/>s. <see cref="LeafNode"/>s are kept in <see cref="Node"/>s which are not entries and not <see cref="LeafNode"/>s.
        /// </remarks>
        private class LeafNode : Node
        {
            public LeafNode(MTree<T> tree, T data)
                : base(tree, data) { }

            public override void AddChild(Node child, double distance)
            {
                if (this.Children.ContainsKey(child.Data))
                    throw new ArgumentException(CoreMessages.ItemWasAlreadyInTree);
                this.Children.Add(child.Data, child);
                this.UpdateMetrics(child, distance);
            }

            protected override void DoAddData(T data, double distance)
            {
                this.AddChild(new Node(this.Tree, data, true), distance);
            }

            protected override Boolean DoRemoveData(T data, double distance)
            {
                Boolean removed = this.Children.Remove(data);

                if (this.Tree.root == this && this.ChildrenCount < this.GetMinCapacity())
                {
                    throw new RootNodeReplacement(removed);
                }

                return removed;
            }

            protected override Node NewSplitNodeReplacement(T data)
            {
                return new LeafNode(this.Tree, data);
            }
        }

        private const Int32 DefaultMinChildren = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="MTree{T}"/> class.
        /// </summary>
        /// <param name="distanceMetric">
        ///     The distance metric to be used to calculate distances between data points.
        ///
        ///     The distance metric must satisfy the following properties:
        ///     <list type="bullet">
        ///         <item>
        ///             <description><c>d(a,b) = d(b,a)</c> for every <c>a</c> and <c>b</c> points (symmetry)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,a) = 0</c> and <c>d(a,b) &gt; 0</c> for every <c>a != b</c> points (non-negativity)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,b) &lt;= d(a,c) + d(c,b)</c> for every <c>a</c>, <c>b</c> and <c>c</c> points (triangle inequality)</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <c>distanceMetric</c> is missing
        /// </exception>
        public MTree(DistanceMetric<T> distanceMetric)
            : this(distanceMetric, SplitPolicies.BalancedSplitPolicy<T>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MTree{T}"/> class.
        /// </summary>
        /// <param name="distanceMetric">
        ///     The distance metric to be used to calculate distances between data points.
        ///
        ///     The distance metric must satisfy the following properties:
        ///     <list type="bullet">
        ///         <item>
        ///             <description><c>d(a,b) = d(b,a)</c> for every <c>a</c> and <c>b</c> points (symmetry)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,a) = 0</c> and <c>d(a,b) &gt; 0</c> for every <c>a != b</c> points (non-negativity)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,b) &lt;= d(a,c) + d(c,b)</c> for every <c>a</c>, <c>b</c> and <c>c</c> points (triangle inequality)</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="splitPolicy">The split policy to use. It consist of a partition and a promotion policy. <see cref="SplitPolicy{DATA}"/></param>
        /// <exception cref="ArgumentNullException">
        /// <c>distanceMetric</c> is missing
        /// or
        /// <c>splitPolicy</c> is missing
        /// </exception>
        public MTree(DistanceMetric<T> distanceMetric, ISplitPolicy<T> splitPolicy)
            : this(DefaultMinChildren, 2 * DefaultMinChildren + 1, distanceMetric, splitPolicy) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MTree{T}"/> class.
        /// </summary>
        /// <param name="minChildren">The minimum number of children nodes for a node.</param>
        /// <param name="maxChildren">The maximum number of children nodes for a node.</param>
        /// <param name="distanceMetric">
        ///     The distance metric to be used to calculate distances between data points.
        ///
        ///     The distance metric must satisfy the following properties:
        ///     <list type="bullet">
        ///         <item>
        ///             <description><c>d(a,b) = d(b,a)</c> for every <c>a</c> and <c>b</c> points (symmetry)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,a) = 0</c> and <c>d(a,b) &gt; 0</c> for every <c>a != b</c> points (non-negativity)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,b) &lt;= d(a,c) + d(c,b)</c> for every <c>a</c>, <c>b</c> and <c>c</c> points (triangle inequality)</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <c>minChildren</c> is less than 1
        /// or
        /// <c>maxChildren</c> is less than <c>minChildren</c>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <c>distanceMetric</c> is missing
        /// </exception>
        public MTree(Int32 minChildren, Int32 maxChildren, DistanceMetric<T> distanceMetric)
            : this(minChildren, maxChildren, distanceMetric, SplitPolicies.BalancedSplitPolicy<T>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MTree{T}"/> class.
        /// </summary>
        /// <param name="minChildren">The minimum number of children nodes for a node.</param>
        /// <param name="maxChildren">The maximum number of children nodes for a node.</param>
        /// <param name="distanceMetric">
        ///     The distance metric to be used to calculate distances between data points.
        ///
        ///     The distance metric must satisfy the following properties:
        ///     <list type="bullet">
        ///         <item>
        ///             <description><c>d(a,b) = d(b,a)</c> for every <c>a</c> and <c>b</c> points (symmetry)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,a) = 0</c> and <c>d(a,b) &gt; 0</c> for every <c>a != b</c> points (non-negativity)</description>
        ///         </item>
        ///         <item>
        ///             <description><c>d(a,b) &lt;= d(a,c) + d(c,b)</c> for every <c>a</c>, <c>b</c> and <c>c</c> points (triangle inequality)</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="splitPolicy">The split policy to use. It consist of a partition and a promotion policy. <see cref="SplitPolicy{DATA}"/></param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <c>minChildren</c> is less than 1
        /// or
        /// <c>maxChildren</c> is less than <c>minChildren</c>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <c>distanceMetric</c> is missing
        /// or
        /// <c>splitPolicy</c> is missing
        /// </exception>
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
            this.MinChildren = minChildren;
            this.MaxChildren = maxChildren;
            this.distanceMetric = distanceMetric ?? throw new ArgumentNullException(nameof(distanceMetric));
            this.splitPolicy = splitPolicy ?? throw new ArgumentNullException(nameof(splitPolicy));
            this.NumberOfDataItems = 0;
        }

        /// <summary>
        /// Gets the minimum number of children for a node.
        /// </summary>
        /// <value>
        /// The minimum number of children nodes for a node.
        /// </value>
        public Int32 MinChildren { get; private set; }

        /// <summary>
        /// Gets the maximum number of children for node.
        /// </summary>
        /// <value>
        /// The maximum number of children for a node.
        /// </value>
        public Int32 MaxChildren { get; private set; }

        /// <summary>
        /// Gets the number of data items currently contained within the index.
        /// </summary>
        /// <value>
        /// The number of data items currently contained within this index.
        /// </value>
        public Int32 NumberOfDataItems { get; private set; }

        private readonly DistanceMetric<T> distanceMetric;
        private readonly ISplitPolicy<T> splitPolicy;
        private Node root;

        /// <summary>
        /// Adds a new data point to the index.
        /// </summary>
        /// <param name="data">The new data point to be added.</param>
        /// <exception cref="System.ArgumentNullException">The <c>data</c> is null.</exception>
        /// <exception cref="ArgumentException">The <c>data</c> was already indexed by the tree.</exception>
        public void Add(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (this.root == null)
            {
                this.root = new LeafNode(this, data);
                this.root.AddData(data, 0D);
            }
            else
            {
                Double distance = this.distanceMetric.Invoke(data, this.root.Data);
                try
                {
                    this.root.AddData(data, distance);
                }
                catch (SplitNodeReplacement e)
                {
                    Node newRoot = new Node(this, data);
                    this.root = newRoot;
                    foreach (Node newNode in e.NewNodes)
                    {
                        distance = this.distanceMetric.Invoke(this.root.Data, newNode.Data);
                        this.root.AddChild(newNode, distance);
                    }
                }
            }

            this.NumberOfDataItems++;
        }

        /// <summary>
        /// Adds multiple data points to the index.
        /// </summary>
        /// <param name="collection">
        ///     The collection of new data points to added.
        ///     Only non-null items will be considered.
        /// </param>
        /// <exception cref="ArgumentNullException">The <c>collection</c> is null.</exception>
        /// <exception cref="ArgumentException">One of the items in the <c>collection</c> was already indexed by the tree.</exception>
        public void Add(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (T data in collection)
            {
                if (data != null)
                {
                    this.Add(data);
                }
            }
        }

        /// <summary>
        /// Clears all data items from the index.
        /// </summary>
        public void Clear()
        {
            this.root = null;
            this.NumberOfDataItems = 0;
        }

        /// <summary>
        /// Remove a given data point from the index.
        /// </summary>
        /// <param name="data">The data point to be removed.</param>
        /// <returns><c>true</c> if the data point has been successfully removed; otherwise <c>false</c></returns>
        /// <exception cref="ArgumentNullException">The <c>data</c> is null.</exception>
        public Boolean Remove(T data)
        {
            Boolean removed = false;

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (this.root == null)
                return false;

            Double distanceFromRoot = this.distanceMetric.Invoke(data, this.root.Data);
            try
            {
                removed = this.root.RemoveData(data, distanceFromRoot);
            }
            catch (RootNodeReplacement e)
            {
                this.root = e.NewRoot;
                removed = e.ItemRemoved;
            }

            if (removed)
                this.NumberOfDataItems--;
            return removed;
        }

        /// <summary>
        /// Determines whether this index contains the specified data.
        /// </summary>
        /// <param name="data">The data to be checked.</param>
        /// <returns>
        ///   <c>true</c> if the index contains the specified data; otherwise, <c>false</c>
        /// </returns>
        public Boolean Contains(T data)
        {
            IEnumerator<ResultItem<T>> enumerator = this.Search(data, 0.0001D, 1).GetEnumerator();
            return enumerator.MoveNext() && enumerator.Current.Item.Equals(data);
        }

        /// <summary>
        /// Initiates a nerest neighbor search within this tree.
        /// </summary>
        /// <param name="queryData">
        ///     The query data. This can be any data (it doesn't have to be present in the tree).
        ///     The nearest neighbors of this data point will be looked up within the tree.
        /// </param>
        /// <param name="limit">(optional) The maximum number of data items returned.</param>
        /// <returns>The search results. Results are loaded just-in-time while traversing the enumerator.</returns>
        public IEnumerable<ResultItem<T>> Search(T queryData, Int32 limit = Int32.MaxValue)
        {
            return this.Search(queryData, Double.PositiveInfinity, limit);
        }

        /// <summary>
        /// Initiates a nerest neighbor search within this tree.
        /// </summary>
        /// <param name="queryData">
        ///     The query data. This can be any data (it doesn't have to be present in the tree).
        ///     The nearest neighbors of this data point will be looked up within the tree.
        /// </param>
        /// <param name="range">The range (radius) of the search. Data will be searched within this radius of <c>queryData</c>.</param>
        /// <param name="limit">(optional) The maximum number of data items returned.</param>
        /// <returns>The search results. Results are loaded just-in-time while traversing the enumerator.</returns>
        /// <exception cref="ArgumentNullException">If <c>queryData</c> is null.</exception>
        public IEnumerable<ResultItem<T>> Search(T queryData, Double range, Int32 limit = Int32.MaxValue)
        {
            if (queryData == null)
                throw new ArgumentNullException(nameof(queryData));

            return new SearchQuery(this, queryData, range, limit);
        }
    }
}
