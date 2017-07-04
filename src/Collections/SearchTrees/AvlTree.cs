// <copyright file="AvlTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Collections.SearchTrees
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Collections.Resources;

    /// <summary>
    /// Represents an AVL tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class AvlTree<TKey, TValue> : BinarySearchTree<TKey, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        public AvlTree()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        public AvlTree(IComparer<TKey> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// Gets the height of the search tree.
        /// </summary>
        /// <value>The height of the search tree.</value>
        public override Int32 Height
        {
            get
            {
                return this.root == null ? -1 : (this.root as AvlNode).Height;
            }
        }

        /// <summary>
        /// Inserts the specified key/value pair to the tree.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the tree.</exception>
        public override void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (this.root == null)
            {
                this.root = new AvlNode { Key = key, Value = value, Height = 0, Balance = 0 };
                this.nodeCount++;
                this.version++;
                return;
            }

            AvlNode node = this.SearchNodeForInsertion(key) as AvlNode;
            if (node == null)
                throw new ArgumentException(CollectionMessages.KeyExists, nameof(key));

            if (this.Comparer.Compare(key, node.Key) < 0)
            {
                node.LeftChild = new AvlNode { Key = key, Value = value, Parent = node, Height = 0, Balance = 0 };
            }
            else
            {
                node.RightChild = new AvlNode { Key = key, Value = value, Parent = node, Height = 0, Balance = 0 };
            }

            this.Balance(node);
            this.nodeCount++;
            this.version++;
        }

        /// <summary>
        /// Removes a node that has no children.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void RemoveNodeWithNoChild(BinarySearchTree<TKey, TValue>.Node node)
        {
            if (node == null)
                return;

            AvlNode parent = node.Parent as AvlNode;

            base.RemoveNodeWithNoChild(node);

            if (parent != null)
                this.Balance(parent);
        }

        /// <summary>
        /// Removes a node that has one child.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void RemoveNodeWithOneChild(BinarySearchTree<TKey, TValue>.Node node)
        {
            if (node == null)
                return;

            base.RemoveNodeWithOneChild(node);

            this.Balance(node as AvlNode);
        }

        /// <summary>
        /// Balances a subtree to comply with AVL property.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>
        protected void Balance(AvlNode node)
        {
            while (node != null)
            {
                node.Balance = GetBalance(node);

                Int32 newHeight = GetHeight(node);
                if (newHeight == node.Height)
                    return;

                node.Height = newHeight;

                switch (node.Balance)
                {
                    case -2:
                        switch ((node.LeftChild as AvlNode).Balance)
                        {
                            case 0: // --,=
                            case -1: // --,-
                                node = this.RotateRight(node) as AvlNode;
                                (node.RightChild as AvlNode).Height = GetHeight(node.RightChild);
                                (node.RightChild as AvlNode).Balance = GetBalance(node.RightChild);
                                node.Height = GetHeight(node);
                                node.Balance = GetBalance(node);
                                break;
                            case 1: // --,+
                                this.RotateLeft(node.LeftChild);
                                node = this.RotateRight(node) as AvlNode;
                                (node.LeftChild as AvlNode).Height = GetHeight(node.LeftChild);
                                (node.LeftChild as AvlNode).Balance = GetBalance(node.LeftChild);
                                (node.RightChild as AvlNode).Height = GetHeight(node.RightChild);
                                (node.RightChild as AvlNode).Balance = GetBalance(node.RightChild);
                                node.Height = GetHeight(node);
                                node.Balance = GetBalance(node);
                                break;
                        }

                        break;
                    case 2:
                        switch ((node.RightChild as AvlNode).Balance)
                        {
                            case 0: // ++,=
                            case 1: // ++,+
                                node = this.RotateLeft(node) as AvlNode;
                                (node.LeftChild as AvlNode).Height = GetHeight(node.LeftChild);
                                (node.LeftChild as AvlNode).Balance = GetBalance(node.LeftChild);
                                node.Height = GetHeight(node);
                                node.Balance = GetBalance(node);
                                break;
                            case -1: // ++,-
                                this.RotateRight(node.RightChild);
                                node = this.RotateLeft(node) as AvlNode;
                                (node.LeftChild as AvlNode).Height = GetHeight(node.LeftChild);
                                (node.LeftChild as AvlNode).Balance = GetBalance(node.LeftChild);
                                (node.RightChild as AvlNode).Height = GetHeight(node.RightChild);
                                (node.RightChild as AvlNode).Balance = GetBalance(node.RightChild);
                                node.Height = GetHeight(node);
                                node.Balance = GetBalance(node);
                                break;
                        }

                        break;
                }

                node = node.Parent as AvlNode;
            }
        }

        /// <summary>
        /// Gets the height of a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The height of the node.</returns>
        private static Int32 GetHeight(Node node)
        {
            Int32 leftHeight = (node.LeftChild == null) ? -1 : (node.LeftChild as AvlNode).Height;
            Int32 rightHeight = (node.RightChild == null) ? -1 : (node.RightChild as AvlNode).Height;

            return Math.Max(rightHeight, leftHeight) + 1;
        }

        /// <summary>
        /// Gets the balance of a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The balance of the node.</returns>
        private static Int32 GetBalance(Node node)
        {
            Int32 leftHeight = (node.LeftChild == null) ? -1 : (node.LeftChild as AvlNode).Height;
            Int32 rightHeight = (node.RightChild == null) ? -1 : (node.RightChild as AvlNode).Height;

            return rightHeight - leftHeight;
        }

        /// <summary>
        /// Represents a node of the AVL tree.
        /// </summary>
        protected class AvlNode : Node
        {
            /// <summary>
            /// Gets or sets the height of the subtree starting with the node.
            /// </summary>
            /// <value>The height of the subtree starting with the node..</value>
            public Int32 Height { get; set; }

            /// <summary>
            /// Gets or sets the balance of the node.
            /// </summary>
            /// <value>The balance of the node.</value>
            public Int32 Balance { get; set; }
        }
    }
}
