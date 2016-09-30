// <copyright file="DisjointSetForest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Resources;

    /// <summary>
    /// Represents a disjoint-set data structure.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sets.</typeparam>
    /// <remarks>
    /// In computing, a disjoint-set data structure, also called a union–find data structure or merge–find set,
    /// is a data structure that keeps track of a set of elements partitioned into a number of disjoint (non-overlapping) subsets.
    /// This implementation of the <see cref="IDisjointSet{TElement}" /> interface is the Disjoint-set forests that are data structures
    /// where each set is represented by a tree data structure, in which each node holds a reference to its parent node.
    /// </remarks>
    public class DisjointSetForest<T> : IDisjointSet<T>, IEnumerable, IEnumerable<T>
    {
        #region Private fields

        /// <summary>
        /// The parent of the element in the tree.
        /// </summary>
        private Dictionary<T, T> parent;

        /// <summary>
        /// The rank of the subset containing the element.
        /// </summary>
        private Dictionary<T, Int32> rank;

        #endregion

        #region IDisjointSet properties

        /// <summary>
        /// Gets the number of elements in the disjoint-set forest.
        /// </summary>
        public Int32 Count
        {
            get
            {
                return this.parent.Keys.Count;
            }
        }

        /// <summary>
        /// Gets the number of disjoint sets.
        /// </summary>
        public Int32 SetCount { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSetForest{TElement}" /> class that is empty and has the default initial capacity.
        /// </summary>
        public DisjointSetForest()
        {
            this.parent = new Dictionary<T, T>();
            this.rank = new Dictionary<T, Int32>();
            this.SetCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSetForest{TElement}" /> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="DisjointSetForest{TElement}" /> can contain, without resizing.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public DisjointSetForest(Int32 capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), Messages.CapacityLessThan0);

            this.parent = new Dictionary<T, T>(capacity);
            this.rank = new Dictionary<T, Int32>(capacity);
            this.SetCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSetForest{TElement}" /> class, that contains singletons(set with one element) from the specified <see cref="IEnumerable{TElement}" />.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{TElement}" /> whose elements are gonna be singletons in the new <see cref="DisjointSetForest{TElement}" />.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public DisjointSetForest(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), Messages.SourceIsNull);

            this.parent = new Dictionary<T, T>();
            this.rank = new Dictionary<T, Int32>();
            this.SetCount = 0;
            foreach (T element in source)
            {
                this.MakeSet(element);
            }
        }

        #endregion

        #region IDisjointSet methods

        /// <summary>
        /// Makes a set containing only the given element.
        /// </summary>
        /// <remarks>
        /// If the element is already in one of the subsets, nothing happens.
        /// </remarks>
        /// <param name="item">The element to add to the set.</param>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        public void MakeSet(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), Messages.ItemIsNull);

            if (!this.parent.ContainsKey(item))
            {
                this.parent[item] = item;
                this.rank[item] = 0;
                this.SetCount++;
            }
        }

        /// <summary>
        /// Determine which subset a particular value is in.
        /// </summary>
        /// <param name="item">The element.</param>
        /// <returns>The representative for the subset containing <paramref name="item" />.</returns>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        /// <exception cref="System.ArgumentException">The item is not present in any set.</exception>
        public T FindSet(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), Messages.ItemIsNull);

            T representative;

            if (!this.TryFindSet(item, out representative))
                throw new ArgumentException(Messages.ItemIsNotPresentInAnySet, nameof(item));

            return representative;
        }

        /// <summary>
        /// Tries to determine which subset a particular element is in.
        /// </summary>
        /// <param name="item">The element to locate in the set.</param>
        /// <param name="representative">The representative for the subset containing <paramref name="item" />.</param>
        /// <returns><c>true</c> if the element was successfully located within the disjoint set; otherwise, <c>false</c>.</returns>
        public Boolean TryFindSet(T item, out T representative)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), Messages.ItemIsNull);

            representative = default(T);

            if (!this.parent.ContainsKey(item))
                return false;

            if (this.parent[item].Equals(item))
            {
                representative = item;
            }
            else
            {
                this.parent[item] = this.FindSet(this.parent[item]);
                representative = this.parent[item];
            }

            return true;
        }

        /// <summary>
        /// Joins two subsets into a single subset.
        /// </summary>
        /// <param name="first">Element from the first subset.</param>
        /// <param name="second">Element from the second subset.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The second item is null.
        /// or
        /// The second item is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first item is not present in any set.
        /// or
        /// The second item is not present in any set.
        /// </exception>
        public void JoinSets(T first, T second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstItemIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondItemIsNull);
            if (!this.parent.ContainsKey(first))
                throw new ArgumentException(Messages.FirstItemIsNotPresentInAnySet, nameof(first));
            if (!this.parent.ContainsKey(second))
                throw new ArgumentException(Messages.SecondItemIsNotPresentInAnySet, nameof(second));

            this.InternalJoinSets(first, second);
        }

        /// <summary>
        /// Tries to join two subsets into a single subset.
        /// </summary>
        /// <remarks>
        /// If the elements are already in one subset, nothing happens.
        /// </remarks>
        /// <param name="first">Element from the first subset.</param>
        /// <param name="second">Element from the second subset.</param>
        /// <returns><c>true</c> if the elements were successfully located; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Element <paramref name="first" /> is null.
        /// or
        /// Element <paramref name="second" /> is null.
        /// </exception>
        public Boolean TryJoinSets(T first, T second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstItemIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondItemIsNull);
            if (!this.parent.ContainsKey(first))
                return false;
            if (!this.parent.ContainsKey(second))
                return false;

            this.InternalJoinSets(first, second);
            return true;
        }

        /// <summary>
        /// Removes all element from the disjoint-set.
        /// </summary>
        public void Clear()
        {
            this.parent.Clear();
            this.rank.Clear();
            this.SetCount = 0;
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection in subset order.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{TElement}" /> object that can be used to iterate through the collection in subset order.</returns>
        public IEnumerable<T> GetOrderedEnumerator()
        {
            foreach (IGrouping<T, KeyValuePair<T, T>> pair in this.parent.Select(x => new KeyValuePair<T, T>(x.Key, this.FindSet(x.Key))).GroupBy(x => x.Value))
            {
                foreach (KeyValuePair<T, T> keyValuePair in pair)
                {
                    yield return keyValuePair.Key;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{TElement}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T element in this.parent.Keys)
            {
                yield return element;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Joins two subsets into a single subset.
        /// </summary>
        /// <param name="first">Element from the first subset.</param>
        /// <param name="second">Element from the second subset.</param>
        /// <remarks>
        /// If the elements are already in one subset, nothing happens.
        /// </remarks>
        private void InternalJoinSets(T first, T second)
        {
            T xRoot = this.FindSet(first);
            T yRoot = this.FindSet(second);
            int xRank = this.rank[xRoot];
            int yRank = this.rank[yRoot];

            if (xRoot.Equals(yRoot))
                return;

            this.SetCount--;

            if (xRank < yRank)
            {
                this.parent[xRoot] = yRoot;
            }
            else if (yRank < xRank)
            {
                this.parent[yRoot] = xRoot;
            }
            else
            {
                this.parent[xRoot] = yRoot;
                this.rank[xRoot]++;
            }
        }

        #endregion
    }
}