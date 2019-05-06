// <copyright file="FibonacciHeap.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Collections.Resources;

    /// <summary>
    /// Represents a Fibonacci heap data structure containing key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the heap.</typeparam>
    /// <typeparam name="TValue">The type of the values in the heap.</typeparam>
    /// <remarks>
    /// A heap is a specialized tree-based data structure that satisfies the heap property:
    /// If A is a parent node of B then key(A) is ordered with respect to key(B) with the same ordering applying across the heap.
    /// This implementation of the <see cref="TIHeap{TKey, TValue}" /> interface uses the default ordering scheme of the key type if not specified differently by providing an instance of the  <see cref="IComparer{T}" /> interface.
    /// For example, the default ordering provides a min heap in case of number types.
    /// The storage of the key/value pairs is array based with O(1) insertion and O(rank(H)) removal time where rank(H) is maximum rank of any node in heap H.
    /// </remarks>
    public sealed class FibonacciHeap<TKey, TValue> : IHeap<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        /// <summary>
        /// The root list.
        /// </summary>
        private readonly LinkedList<FibonacciHeapNode> rootList;

        /// <summary>
        /// The comparer.
        /// </summary>
        private readonly IComparer<TKey> comparer;

        /// <summary>
        /// The size.
        /// </summary>
        private Int32 size;

        /// <summary>
        /// The peek linked list node.
        /// </summary>
        private LinkedListNode<FibonacciHeapNode> peekLinkedListNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="FibonacciHeap{TKey, TValue}" /> class.
        /// </summary>
        public FibonacciHeap()
            : this(Comparer<TKey>.Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FibonacciHeap{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{KeyValuePair{TKey, TValue}}" /> whose elements are copied to the new <see cref="FibonacciHeap{TKey, TValue}" />.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public FibonacciHeap(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : this(Comparer<TKey>.Default)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (KeyValuePair<TKey, TValue> element in collection)
                this.Insert(element.Key, element.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FibonacciHeap{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public FibonacciHeap(IEnumerable<KeyValuePair<TKey, TValue>> source, IComparer<TKey> comparer)
            : this(comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (KeyValuePair<TKey, TValue> element in source)
                this.Insert(element.Key, element.Value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FibonacciHeap{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        public FibonacciHeap(IComparer<TKey> comparer)
        {
            this.comparer = comparer ?? Comparer<TKey>.Default;

            this.rootList = new LinkedList<FibonacciHeapNode>();
            this.size = 0;
            this.peekLinkedListNode = null;
        }

        /// <summary>
        /// Gets the number of elements actually contained in the heap.
        /// </summary>
        /// <value>The number of elements actually contained in the heap.</value>
        public Int32 Count { get { return this.size; } }

        /// <summary>
        /// Gets the value at the top of the heap without removing it.
        /// </summary>
        /// <value>The value at the beginning of the heap.</value>
        /// <exception cref="System.InvalidOperationException">The heap is empty.</exception>
        public TValue Peek
        {
            get
            {
                if (this.size == 0)
                    throw new InvalidOperationException(CollectionMessages.HeapIsEmpty);

                return this.peekLinkedListNode.Value.Value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <value>The number of elements that the heap can contain before resizing is required.</value>
        Int32 IHeap<TKey, TValue>.Capacity { get { return Int32.MaxValue; } set { } }

        /// <summary>
        /// Gets the <see cref="IComparer{T}" /> that is used to determine order of keys for the heap.
        /// </summary>
        /// <value>The <see cref="IComparer{T}" /> generic interface implementation that is used to determine order of keys for the current heap and to provide hash values for the keys.</value>
        public IComparer<TKey> Comparer { get { return this.comparer; } }

        /// <summary>
        /// Inserts the specified key and value to the heap.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            this.size++;

            FibonacciHeapNode node = new FibonacciHeapNode(key, value);
            LinkedListNode<FibonacciHeapNode> linkedListNode = this.rootList.AddLast(node);

            if (this.peekLinkedListNode == null ||
                this.comparer.Compare(linkedListNode.Value.Key, this.peekLinkedListNode.Value.Key) < 0)
            {
                this.peekLinkedListNode = linkedListNode;
            }
        }

        /// <summary>
        /// Removes and returns the value at the top of the heap.
        /// </summary>
        /// <returns>The value that is removed from the top of the heap.</returns>
        /// <exception cref="System.InvalidOperationException">The heap is empty.</exception>
        public TValue RemovePeek()
        {
            if (this.size == 0)
                throw new InvalidOperationException(CollectionMessages.HeapIsEmpty);

            TValue result = this.peekLinkedListNode.Value.Value;
            FibonacciHeapNode peek = this.peekLinkedListNode.Value;

            this.size--;

            foreach (FibonacciHeapNode child in peek.Children)
                this.rootList.AddLast(child);

            this.rootList.Remove(this.peekLinkedListNode);

            this.peekLinkedListNode = this.rootList.First;

            if (this.size > 0)
            {
                this.MoveMinimumToPeek();

                Int32 count = this.rootList.Max(node => node.Rank) + 1;
                List<LinkedListNode<FibonacciHeapNode>> rankOfChildren = Enumerable.Repeat<LinkedListNode<FibonacciHeapNode>>(null, count).ToList();

                for (LinkedListNode<FibonacciHeapNode> node = this.rootList.First; node != null; node = node.Next)
                {
                    LinkedListNode<FibonacciHeapNode> currentNode = node;
                    LinkedListNode<FibonacciHeapNode> rankNode = rankOfChildren[node.Value.Rank];
                    while (rankNode != null)
                    {
                        rankOfChildren[currentNode.Value.Rank] = null;

                        LinkedListNode<FibonacciHeapNode> min, max;

                        if (this.comparer.Compare(currentNode.Value.Key, rankNode.Value.Key) < 0)
                        {
                            min = currentNode;
                            max = rankNode;
                        }
                        else
                        {
                            min = rankNode;
                            max = currentNode;
                        }

                        min.Value.AddChild(max.Value);
                        max.Value.SetParent(min.Value);

                        this.rootList.Remove(max);

                        currentNode = min;

                        if (min.Value.Rank == rankOfChildren.Count)
                            rankOfChildren.Add(null);

                        rankNode = rankOfChildren[min.Value.Rank];
                    }

                    rankOfChildren[currentNode.Value.Rank] = currentNode;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether the heap contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the heap.</param>
        /// <returns><c>true</c> if the heap contains an element with the specified key; otherwise, <c>false</c>.</returns>
        public Boolean Contains(TKey key)
        {
            return this.rootList.Any(tree => this.comparer.Compare(key, tree.Key) == 0);
        }

        /// <summary>
        /// Removes all keys and values from the heap.
        /// </summary>
        public void Clear()
        {
            this.size = 0;
            this.rootList.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{KeyValuePair{TKey, TValue}}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.rootList.SelectMany(FlattenNodes)
                            .Select(element => new KeyValuePair<TKey, TValue>(element.Key, element.Value))
                            .GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Flattens the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The collection of flattened nodes.</returns>
        private static IEnumerable<FibonacciHeapNode> FlattenNodes(FibonacciHeapNode node)
        {
            yield return node;

            foreach (FibonacciHeapNode child in node.Children)
            {
                foreach (FibonacciHeapNode flattenChild in FlattenNodes(child))
                {
                    yield return flattenChild;
                }
            }
        }

        /// <summary>
        /// Moves the minimum node to the peek.
        /// </summary>
        private void MoveMinimumToPeek()
        {
            for (LinkedListNode<FibonacciHeapNode> element = this.rootList.First; element != null; element = element.Next)
            {
                FibonacciHeapNode root = element.Value;

                if (this.comparer.Compare(root.Key, this.peekLinkedListNode.Value.Key) < 0)
                    this.peekLinkedListNode = element;
            }
        }

        /// <summary>
        /// Represents a Fibonacci heap node.
        /// </summary>
        private class FibonacciHeapNode
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FibonacciHeapNode" /> class.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <param name="isMarked">A value indicating whether the node is marked.</param>
            public FibonacciHeapNode(TKey key, TValue value, Boolean isMarked = false)
            {
                this.Children = new List<FibonacciHeapNode>();
                this.Key = key;
                this.Value = value;
                this.IsMarked = isMarked;
            }

            /// <summary>
            /// Gets the key.
            /// </summary>
            /// <value>The key.</value>
            public TKey Key { get; private set; }

            /// <summary>
            /// Gets the value.
            /// </summary>
            /// <value>The value.</value>
            public TValue Value { get; private set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is marked.
            /// </summary>
            /// <value><c>true</c> if this instance is marked; otherwise, <c>false</c>.</value>
            public Boolean IsMarked { get; set; }

            /// <summary>
            /// Gets or sets the parent.
            /// </summary>
            /// <value>The parent.</value>
            public FibonacciHeapNode Parent { get; protected set; }

            /// <summary>
            /// Gets the list of children.
            /// </summary>
            /// <value>The list of children.</value>
            public List<FibonacciHeapNode> Children { get; private set; }

            /// <summary>
            /// Gets the rank.
            /// </summary>
            /// <value>The rank.</value>
            public Int32 Rank { get { return this.Children.Count; } }

            /// <summary>
            /// Sets the parent node.
            /// </summary>
            /// <param name="parent">The parent.</param>
            public void SetParent(FibonacciHeapNode parent)
            {
                if (this.Parent != null)
                    this.Parent.Children.Remove(this);

                this.Parent = parent;
            }

            /// <summary>
            /// Adds a node to the children nodes.
            /// </summary>
            /// <param name="child">The child.</param>
            public void AddChild(FibonacciHeapNode child)
            {
                this.Children.Add(child);
            }
        }
    }
}
