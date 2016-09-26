// <copyright file="BinarySearchTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections.SearchTree
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ELTE.AEGIS.Collections.Resources;

    /// <summary>
    /// Represents a binary search tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class BinarySearchTree<TKey, TValue> : ISearchTree<TKey, TValue>
    {
        #region Protected fields

        /// <summary>
        /// The root node.
        /// </summary>
        protected Node root;

        /// <summary>
        /// The number of nodes.
        /// </summary>
        protected Int32 nodeCount;

        /// <summary>
        /// The version of the tree.
        /// </summary>
        protected Int32 version;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySearchTree{TKey, TValue}" /> class.
        /// </summary>
        public BinarySearchTree()
        {
            this.root = null;
            this.nodeCount = 0;
            this.version = 0;
            this.Comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySearchTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        public BinarySearchTree(IComparer<TKey> comparer)
        {
            this.root = null;
            this.nodeCount = 0;
            this.version = 0;

            this.Comparer = comparer ?? Comparer<TKey>.Default;
        }

        #endregion

        #region ISearchTree properties

        /// <summary>
        /// Gets the number of elements actually contained in the search tree.
        /// </summary>
        /// <value>The number of elements actually contained in the search tree.</value>
        public Int32 Count { get { return this.nodeCount; } }

        /// <summary>
        /// Gets the height of the search tree.
        /// </summary>
        /// <value>The height of the search tree.</value>
        public virtual Int32 Height { get { return GetTreeHeight(this.root); } }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the <see cref="IComparer{T}" /> that is used to determine order of keys for the tree.
        /// </summary>
        /// <value>The <see cref="IComparer{T}" /> generic interface implementation that is used to determine order of keys for the current search tree and to provide hash values for the keys.</value>
        public IComparer<TKey> Comparer { get; private set; }

        #endregion

        #region ISearchTree methods

        /// <summary>
        /// Searches the search tree for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value of the element with the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        /// <exception cref="System.ArgumentException">The tree does not contain the specified key.</exception>
        public TValue Search(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), Messages.KeyIsNull);

            Node node = this.SearchNode(key);
            if (node == null)
                throw new ArgumentException(Messages.KeyNotExists, nameof(key));

            return node.Value;
        }

        /// <summary>
        /// Searches the <see cref="ISearchTree{TKey, TValue}" /> for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value of the element with the specified key.</param>
        /// <returns><c>true</c> if the <see cref="ISearchTree{TKey, TValue}" /> contains the element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        public Boolean TrySearch(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), Messages.KeyIsNull);

            Node node = this.SearchNode(key);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }

            value = node.Value;
            return true;
        }

        /// <summary>
        /// Determines whether the <see cref="ISearchTree{TKey, TValue}" /> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="ISearchTree{TKey, TValue}" />.</param>
        /// <returns><c>true</c> if the <see cref="ISearchTree{TKey, TValue}" /> contains the element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean Contains(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), Messages.KeyIsNull);

            return this.SearchNode(key) != null;
        }

        /// <summary>
        /// Inserts the specified key/value pair to the tree.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the tree.</exception>
        public virtual void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), Messages.KeyIsNull);

            if (this.root == null)
            {
                this.root = new Node { Key = key, Value = value };
                this.nodeCount++;
                this.version++;
                return;
            }

            Node node = this.SearchNodeForInsertion(key);
            if (node == null)
                throw new ArgumentException(Messages.KeyExists, nameof(key));

            if (this.Comparer.Compare(key, node.Key) < 0)
                node.LeftChild = new Node { Key = key, Value = value, Parent = node };
            else
                node.RightChild = new Node { Key = key, Value = value, Parent = node };

            this.nodeCount++;
            this.version++;
        }

        /// <summary>
        /// Removes an element with the specified key from the search tree.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the search tree contains the element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        public virtual Boolean Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), Messages.KeyIsNull);

            Node node = this.SearchNode(key);
            if (node == null)
                return false;

            this.RemoveNode(node);

            this.nodeCount--;
            this.version++;
            return true;
        }

        /// <summary>
        /// Removes all elements from the search tree.
        /// </summary>
        public virtual void Clear()
        {
            this.root = null;
            this.nodeCount = 0;
            this.version++;
        }

        /// <summary>
        /// Returns a search tree enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="ISearchTreeEnumerator{TKey, TValue}" /> object that can be used to iterate through the collection.</returns>
        public ISearchTreeEnumerator<TKey, TValue> GetTreeEnumerator()
        {
            return new SearchTreeEnumerator(this);
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator{KeyValuePair{TKey, TValue}}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Searches the tree for an element with a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The element with the key or <c>null</c> if the key is not within the tree.</returns>
        protected Node SearchNode(TKey key)
        {
            Node node = this.root;
            Int32 compare;

            while (node != null)
            {
                compare = this.Comparer.Compare(key, node.Key);

                if (compare == 0)
                    break;
                node = compare < 0 ? node.LeftChild : node.RightChild;
            }

            return node;
        }

        /// <summary>
        ///  Searches the tree for an element under witch a new element with a specific key can be inserted.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <returns>The element or <c>null</c> if an element with the same key already exists in the tree.</returns>
        protected Node SearchNodeForInsertion(TKey key)
        {
            Node parent = null;
            Node node = this.root;
            Int32 compare;

            while (node != null)
            {
                parent = node;
                compare = this.Comparer.Compare(key, node.Key);

                if (compare == 0)
                    return null;

                node = compare < 0 ? node.LeftChild : node.RightChild;
            }

            return parent;
        }

        /// <summary>
        /// Removes an element from the tree.
        /// </summary>
        /// <param name="node">The node.</param>
        protected void RemoveNode(Node node)
        {
            if (node == null)
                return;

            // case of no children the node should by simply detached
            if (node.LeftChild == null && node.RightChild == null)
            {
                this.RemoveNodeWithNoChild(node);
                return;
            }

            // case of one child
            if (node.LeftChild == null || node.RightChild == null)
            {
                this.RemoveNodeWithOneChild(node);
                return;
            }

            // case of two children
            Node successor = node.RightChild;
            while (successor.LeftChild != null)
                successor = successor.LeftChild;

            node.Key = successor.Key;
            node.Value = successor.Value;

            // check the right child of the successor (left child is null after the loop)
            if (successor.RightChild == null)
            {
                this.RemoveNodeWithNoChild(successor);
            }
            else
            {
                this.RemoveNodeWithOneChild(successor);
            }
        }

        /// <summary>
        /// Removes a node that has no children.
        /// </summary>
        /// <param name="node">The node.</param>
        protected virtual void RemoveNodeWithNoChild(Node node)
        {
            if (node == null)
                return;

            if (node == this.root)
            {
                this.root = null;
                return;
            }

            if (node.Parent.LeftChild == node)
                node.Parent.LeftChild = null;
            else
                node.Parent.RightChild = null;

            node.Parent = null;
        }

        /// <summary>
        /// Removes a node that has one child.
        /// </summary>
        /// <param name="node">The node.</param>
        protected virtual void RemoveNodeWithOneChild(Node node)
        {
            if (node == null)
                return;

            Node successor = node.LeftChild ?? node.RightChild;

            node.Key = successor.Key;
            node.Value = successor.Value;
            node.LeftChild = successor.LeftChild;
            node.RightChild = successor.RightChild;

            if (node.LeftChild != null)
                node.LeftChild.Parent = node;
            if (node.RightChild != null)
                node.RightChild.Parent = node;

            successor.LeftChild = successor.RightChild = successor.Parent = null;
        }

        /// <summary>
        /// Rotates a subtree to the left.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>
        /// <returns>The root node of the rotated subtree.</returns>
        protected virtual Node RotateLeft(Node node)
        {
            if (node == null)
                return null;

            Node rightChild = node.RightChild;
            Node rightLeftChild = rightChild.LeftChild;
            Node parentNode = node.Parent;

            rightChild.Parent = parentNode;
            rightChild.LeftChild = node;
            node.RightChild = rightLeftChild;
            node.Parent = rightChild;

            if (rightLeftChild != null)
            {
                rightLeftChild.Parent = node;
            }

            if (node == this.root)
            {
                this.root = rightChild;
            }
            else if (parentNode.RightChild == node)
            {
                parentNode.RightChild = rightChild;
            }
            else
            {
                parentNode.LeftChild = rightChild;
            }

            return rightChild;
        }

        /// <summary>
        /// Rotates a subtree to the right.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>
        /// <returns>The root node of the rotated subtree.</returns>
        protected virtual Node RotateRight(Node node)
        {
            if (node == null)
                return null;

            Node leftChild = node.LeftChild;
            Node leftRightChild = leftChild.RightChild;
            Node parentNode = node.Parent;

            leftChild.Parent = parentNode;
            leftChild.RightChild = node;
            node.LeftChild = leftRightChild;
            node.Parent = leftChild;

            if (leftRightChild != null)
            {
                leftRightChild.Parent = node;
            }

            if (node == this.root)
            {
                this.root = leftChild;
            }
            else if (parentNode.LeftChild == node)
            {
                parentNode.LeftChild = leftChild;
            }
            else
            {
                parentNode.RightChild = leftChild;
            }

            return leftChild;
        }

        #endregion

        #region Protected static methods

        /// <summary>
        /// Gets the height of the tree.
        /// </summary>
        /// <param name="node">The starting node.</param>
        /// <returns>The height of the tree.</returns>
        protected static Int32 GetTreeHeight(Node node)
        {
            if (node == null)
                return -1;

            Int32 leftHeight = GetTreeHeight(node.LeftChild) + 1;
            Int32 rightHeight = GetTreeHeight(node.RightChild) + 1;

            if (leftHeight >= rightHeight)
                return leftHeight;
            else
                return rightHeight;
        }

        /// <summary>
        /// Gets the sibling of a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The sibling of <paramref name="node" />.</returns>
        protected static Node GetSiblingNode(Node node)
        {
            if (node == null || node.Parent == null)
                return null;

            if (node.Parent.LeftChild != null && node.Parent.LeftChild == node)
                return node.Parent.RightChild;

            return node.Parent.LeftChild;
        }

        #endregion

        #region Public types

        /// <summary>
        /// Enumerates the elements of a search tree.
        /// </summary>
        /// <remarks>
        /// The enumerator performs an in-order traversal of the search tree thereby resulting in key/values pairs ordered according to the specified comparator of the search tree.
        /// </remarks>
        public sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
        {
            #region Private fields

            /// <summary>
            /// The tree that is enumerated.
            /// </summary>
            private BinarySearchTree<TKey, TValue> localTree;

            /// <summary>
            /// The version at which the enumerator was instantiated.
            /// </summary>
            private Int32 localVersion;

            /// <summary>
            /// The stack containing cached nodes.
            /// </summary>
            private Stack<Node> stack;

            /// <summary>
            /// The current item.
            /// </summary>
            private KeyValuePair<TKey, TValue> current;

            /// <summary>
            /// The current node.
            /// </summary>
            private Node currentNode;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator" /> class.
            /// </summary>
            /// <param name="tree">The tree.</param>
            /// <exception cref="System.ArgumentNullException">The tree is null.</exception>
            internal Enumerator(BinarySearchTree<TKey, TValue> tree)
            {
                if (tree == null)
                    throw new ArgumentNullException(nameof(tree), Messages.TreeIsNull);

                this.localTree = tree;
                this.localVersion = tree.version;

                this.stack = new Stack<Node>();
                this.currentNode = tree.root;
                this.current = default(KeyValuePair<TKey, TValue>);
            }

            #endregion

            #region IEnumerator properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            public KeyValuePair<TKey, TValue> Current
            {
                get { return this.current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.-
            /// </value>
            Object IEnumerator.Current
            {
                get { return this.current; }
            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                while (this.currentNode != null)
                {
                    this.stack.Push(this.currentNode);
                    this.currentNode = this.currentNode.LeftChild;
                }

                if (this.stack.Count == 0)
                {
                    this.current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                this.currentNode = this.stack.Pop();
                this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                this.currentNode = this.currentNode.RightChild;
                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                this.stack.Clear();
                this.currentNode = this.localTree.root;
                this.current = default(KeyValuePair<TKey, TValue>);
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }

            #endregion
        }

        /// <summary>
        /// Enumerates the elements of a search tree in multiple directions.
        /// </summary>
        public sealed class SearchTreeEnumerator : ISearchTreeEnumerator<TKey, TValue>
        {
            #region Private fields

            /// <summary>
            /// The tree that is enumerated.
            /// </summary>
            private BinarySearchTree<TKey, TValue> localTree;

            /// <summary>
            /// The version at which the enumerator was instantiated.
            /// </summary>
            private Int32 localVersion;

            /// <summary>
            /// The current item.
            /// </summary>
            private KeyValuePair<TKey, TValue> current;

            /// <summary>
            /// The current node.
            /// </summary>
            private Node currentNode;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SearchTreeEnumerator" /> class.
            /// </summary>
            /// <param name="tree">The tree.</param>
            /// <exception cref="System.ArgumentNullException">The tree is null.</exception>
            internal SearchTreeEnumerator(BinarySearchTree<TKey, TValue> tree)
            {
                if (tree == null)
                    throw new ArgumentNullException(nameof(tree), Messages.TreeIsNull);

                this.localTree = tree;
                this.localVersion = tree.version;
                this.current = default(KeyValuePair<TKey, TValue>);
                this.currentNode = null;
            }

            #endregion

            #region IEnumerator properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            public KeyValuePair<TKey, TValue> Current
            {
                get { return this.current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            object IEnumerator.Current
            {
                get { return this.current; }
            }

            #endregion

            #region ISearchTreeEnumerator methods

            /// <summary>
            /// Advances the enumerator to the previous element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MovePrev()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                if (this.currentNode == null)
                    return false;

                if (this.currentNode.LeftChild != null)
                {
                    this.currentNode = this.currentNode.LeftChild;

                    while (this.currentNode.RightChild != null)
                    {
                        this.currentNode = this.currentNode.RightChild;
                    }

                    this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                    return true;
                }

                while (this.currentNode.Parent != null && this.currentNode.Parent.LeftChild == this.currentNode)
                {
                    this.currentNode = this.currentNode.Parent;
                }

                if (this.currentNode.Parent != null && this.currentNode.Parent.RightChild == this.currentNode)
                {
                    this.currentNode = this.currentNode.Parent;
                    this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                    return true;
                }
                else
                {
                    this.current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }
            }

            /// <summary>
            /// Advances the enumerator to the minimal element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the minimal element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveMin()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                if (this.localTree.root == null)
                {
                    this.currentNode = null;
                    this.current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                this.currentNode = this.localTree.root;

                while (this.currentNode.LeftChild != null)
                {
                    this.currentNode = this.currentNode.LeftChild;
                }

                this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                return true;
            }

            /// <summary>
            /// Advances the enumerator to the maximal element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the maximal element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveMax()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                if (this.localTree.root == null)
                {
                    this.currentNode = null;
                    this.current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                this.currentNode = this.localTree.root;

                while (this.currentNode.RightChild != null)
                {
                    this.currentNode = this.currentNode.RightChild;
                }

                this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                return true;
            }

            /// <summary>
            /// Advances the enumerator to the root element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the root element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveRoot()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                if (this.localTree.root == null)
                {
                    this.currentNode = null;
                    this.current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                this.currentNode = this.localTree.root;
                this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                return true;
            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                if (this.currentNode == null)
                    return false;

                if (this.currentNode.RightChild != null)
                {
                    this.currentNode = this.currentNode.RightChild;

                    while (this.currentNode.LeftChild != null)
                    {
                        this.currentNode = this.currentNode.LeftChild;
                    }

                    this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                    return true;
                }

                while (this.currentNode.Parent != null && this.currentNode.Parent.RightChild == this.currentNode)
                {
                    this.currentNode = this.currentNode.Parent;
                }

                if (this.currentNode.Parent != null && this.currentNode.Parent.LeftChild == this.currentNode)
                {
                    this.currentNode = this.currentNode.Parent;
                    this.current = new KeyValuePair<TKey, TValue>(this.currentNode.Key, this.currentNode.Value);
                    return true;
                }
                else
                {
                    this.current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (this.localVersion != this.localTree.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                this.currentNode = null;
                this.current = default(KeyValuePair<TKey, TValue>);
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }

            #endregion
        }

        #endregion

        #region Protected types

        /// <summary>
        /// Represents a node of the search tree.
        /// </summary>
        protected class Node
        {
            #region Public properties

            /// <summary>
            /// Gets or sets the key of the node.
            /// </summary>
            /// <value>The key of the node.</value>
            public TKey Key { get; set; }

            /// <summary>
            /// Gets or sets the value of the node.
            /// </summary>
            /// <value>The value of the node.</value>
            public TValue Value { get; set; }

            /// <summary>
            /// Gets or sets the parent node.
            /// </summary>
            /// <value>The parent node.</value>
            public Node Parent { get; set; }

            /// <summary>
            /// Gets or sets the left child node.
            /// </summary>
            /// <value>The left child node.</value>
            public Node LeftChild { get; set; }

            /// <summary>
            /// Gets or sets the right child node.
            /// </summary>
            /// <value>The right child node.</value>
            public Node RightChild { get; set; }

            #endregion
        }

        #endregion
    }
}
