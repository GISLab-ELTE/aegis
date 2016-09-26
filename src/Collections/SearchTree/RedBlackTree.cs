// <copyright file="RedBlackTree.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using ELTE.AEGIS.Collections.Resources;

    /// <summary>
    /// Represents a red-black tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class RedBlackTree<TKey, TValue> : BinarySearchTree<TKey, TValue>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RedBlackTree{TKey, TValue}" /> class.
        /// </summary>
        public RedBlackTree()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedBlackTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        public RedBlackTree(IComparer<TKey> comparer)
            : base(comparer)
        {
        }

        #endregion

        #region ISearchTree methods

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
                throw new ArgumentNullException(nameof(key), Messages.KeyIsNull);

            if (this.root == null)
            {
                this.root = new RedBlackNode { Key = key, Value = value, Color = NodeColor.Black };
                this.nodeCount++;
                this.version++;
                return;
            }

            RedBlackNode node = this.SearchNodeForInsertion(key) as RedBlackNode;
            if (node == null)
                throw new ArgumentException(Messages.KeyExists, nameof(key));

            if (this.Comparer.Compare(key, node.Key) < 0)
            {
                node.LeftChild = new RedBlackNode { Key = key, Value = value, Parent = node, Color = NodeColor.Red };
                this.BalanceInsert(node.LeftChild as RedBlackNode);
            }
            else
            {
                node.RightChild = new RedBlackNode { Key = key, Value = value, Parent = node, Color = NodeColor.Red };
                this.BalanceInsert(node.RightChild as RedBlackNode);
            }

            this.nodeCount++;
            this.version++;
        }

        /// <summary>
        /// Removes an element with the specified key from the search tree.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the search tree contains the element with the specified key; otherwise, <c>false</c>. </returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        public override Boolean Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), Messages.KeyIsNull);

            Node node = this.SearchNode(key);
            if (node == null)
                return false;

            if (node.LeftChild != null && node.RightChild != null)
            {
                Node temp = node.LeftChild;
                while (temp.RightChild != null)
                    temp = temp.RightChild;

                Node successor = temp;
                node.Key = successor.Key;
                node.Value = successor.Value;
                node = successor;
            }

            Node child = node.RightChild == null ? node.LeftChild : node.RightChild;

            RedBlackNode redBlackNode = node as RedBlackNode;
            if (redBlackNode.Color == NodeColor.Black)
            {
                if (child != null)
                    redBlackNode.Color = (child as RedBlackNode).Color;
                this.BalanceRemove(redBlackNode);
            }

            if (node.Parent == null)
                this.root = child;
            else
                if (node == node.Parent.LeftChild)
                    node.Parent.LeftChild = child;
                else
                    node.Parent.RightChild = child;

            if (child != null)
                child.Parent = node.Parent;

            this.nodeCount--;
            this.version++;
            return true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Balances the tree after insertion.
        /// </summary>
        /// <param name="node">The node.</param>
        private void BalanceInsert(RedBlackNode node)
        {
            RedBlackNode uncle;

            // case 1: root inserted
            if (node.Parent == null)
            {
                node.Color = NodeColor.Black;
                return;
            }

            // case 2: parent is black
            if ((node.Parent as RedBlackNode).Color == NodeColor.Black)
            {
                return; // no correction needed
            }

            // case 3: parent and uncle are both red
            if ((uncle = GetSiblingNode(node.Parent) as RedBlackNode) != null && uncle.Color == NodeColor.Red)
            {
                (node.Parent as RedBlackNode).Color = NodeColor.Black;
                uncle.Color = NodeColor.Black;
                (node.Parent.Parent as RedBlackNode).Color = NodeColor.Red;

                this.BalanceInsert(node.Parent.Parent as RedBlackNode);
                return;
            }

            // case 4: parent is red, uncle is null or black, node is a right child, parent if left child
            if (node == node.Parent.RightChild && node.Parent == node.Parent.Parent.LeftChild)
            {
                this.RotateLeft(node.Parent);
                node = node.LeftChild as RedBlackNode;
            }
            else if (node == node.Parent.LeftChild && node.Parent == node.Parent.Parent.RightChild)
            {
                this.RotateRight(node.Parent);
                node = node.RightChild as RedBlackNode;
            }

            // case 5: parent is red, uncle is null or black, node is a left child, parent is left child
            (node.Parent as RedBlackNode).Color = NodeColor.Black;
            (node.Parent.Parent as RedBlackNode).Color = NodeColor.Red;
            if (node == node.Parent.LeftChild)
                this.RotateRight(node.Parent.Parent);
            else
                this.RotateLeft(node.Parent.Parent);
        }

        /// <summary>
        /// Balances the tree after removal.
        /// </summary>
        /// <param name="node">The node.</param>
        private void BalanceRemove(RedBlackNode node)
        {
            if (node == null || node.Parent == null) // case 1: the node is the root
                return;

            RedBlackNode parent = node.Parent as RedBlackNode;
            RedBlackNode sibling = GetSiblingNode(node) as RedBlackNode;

            // case 2: the sibling is red
            if (sibling != null && sibling.Color == NodeColor.Red)
            {
                parent.Color = NodeColor.Red;
                sibling.Color = NodeColor.Black;
                if (node == node.Parent.LeftChild)
                    this.RotateLeft(node.Parent);
                else
                    this.RotateRight(node.Parent);
                return;
            }

            // case 3: the sibling, the parent and the children of the sibling are black
            if (parent.Color == NodeColor.Black && sibling != null && sibling.Color == NodeColor.Black &&
                sibling.LeftChild != null && (sibling.LeftChild as RedBlackNode).Color == NodeColor.Black &&
                 sibling.RightChild != null && (sibling.RightChild as RedBlackNode).Color == NodeColor.Black)
            {
                sibling.Color = NodeColor.Red;
                this.BalanceRemove(parent);
                return;
            }

            // case 4: the parent is black, the sibling and the children of the sibling are black
            if (parent.Color == NodeColor.Red && sibling != null && sibling.Color == NodeColor.Black &&
                sibling.LeftChild != null && (sibling.LeftChild as RedBlackNode).Color == NodeColor.Black &&
                 sibling.RightChild != null && (sibling.RightChild as RedBlackNode).Color == NodeColor.Black)
            {
                sibling.Color = NodeColor.Red;
                parent.Color = NodeColor.Black;
                return;
            }

            // case 5: the sibling and the right child of the sibling are black, the left child of the sibling is red (transitions into case 6)
            if (sibling != null && sibling.Color == NodeColor.Black)
            {
                if (node == node.Parent.LeftChild && sibling != null && sibling.Color == NodeColor.Black &&
                    sibling.LeftChild != null && (sibling.LeftChild as RedBlackNode).Color == NodeColor.Red &&
                     sibling.RightChild != null && (sibling.RightChild as RedBlackNode).Color == NodeColor.Black)
                {
                    sibling.Color = NodeColor.Red;
                    (sibling.LeftChild as RedBlackNode).Color = NodeColor.Black;
                    this.RotateRight(sibling);
                }
                else if (node == node.Parent.RightChild && sibling != null && sibling.Color == NodeColor.Black &&
                    sibling.LeftChild != null && (sibling.LeftChild as RedBlackNode).Color == NodeColor.Black &&
                     sibling.RightChild != null && (sibling.RightChild as RedBlackNode).Color == NodeColor.Red)
                {
                    sibling.Color = NodeColor.Red;
                    (sibling.RightChild as RedBlackNode).Color = NodeColor.Black;
                    this.RotateLeft(sibling);
                }
            }

            // case 6: the sibling is black, the right child of the sibling is red
            if (sibling != null)
            {
                sibling.Color = parent.Color;
                parent.Color = NodeColor.Black;

                if (node == node.Parent.LeftChild)
                {
                    if (sibling.RightChild != null)
                        (sibling.RightChild as RedBlackNode).Color = NodeColor.Black;
                    this.RotateLeft(parent);
                }
                else
                {
                    if (sibling.LeftChild != null)
                        (sibling.LeftChild as RedBlackNode).Color = NodeColor.Black;
                    this.RotateRight(parent);
                }
            }

            parent.Color = NodeColor.Black;
        }

        #endregion

        #region Private types

        /// <summary>
        /// Defines the node colors.
        /// </summary>
        private enum NodeColor
        {
            /// <summary>
            /// Indicates that the node is marked red.
            /// </summary>
            Red,

            /// <summary>
            /// Indicates that the node is marked black.
            /// </summary>
            Black
        }

        /// <summary>
        /// Represents a node of the red-black tree.
        /// </summary>
        private class RedBlackNode : Node
        {
            #region Public fields

            /// <summary>
            /// The color of the node.
            /// </summary>
            public NodeColor Color;

            #endregion
        }

        #endregion
    }
}
