// <copyright file="ISearchTree.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior of search trees containing key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the heap.</typeparam>
    /// <typeparam name="TValue">The type of the values in the heap.</typeparam>
    public interface ISearchTree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        /// <summary>
        /// Gets the number of elements actually contained in the search tree.
        /// </summary>
        /// <value>The number of elements actually contained in the search tree.</value>
        Int32 Count { get; }

        /// <summary>
        /// Gets the height of the search tree.
        /// </summary>
        /// <value>The height of the search tree.</value>
        Int32 Height { get; }

        /// <summary>
        /// Searches the search tree for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value of the element with the specified key.</returns>
        TValue Search(TKey key);

        /// <summary>
        /// Searches the search tree for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value of the element with the specified key.</param>
        /// <returns><c>true</c> if the search tree contains the element with the specified key; otherwise, <c>false</c>.</returns>
        Boolean TrySearch(TKey key, out TValue value);

        /// <summary>
        /// Determines whether the search tree contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the search tree.</param>
        /// <returns><c>true</c> if the search tree contains the element with the specified key; otherwise, <c>false</c>.</returns>
        Boolean Contains(TKey key);

        /// <summary>
        /// Adds the specified key and value to the tree.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        void Insert(TKey key, TValue value);

        /// <summary>
        /// Removes an element with the specified key from the search tree.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the search tree contains the element with the specified key; otherwise, <c>false</c>.</returns>
        Boolean Remove(TKey key);

        /// <summary>
        /// Removes all elements from the search tree.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns a search tree enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="ISearchTreeEnumerator{TKey, TValue}" /> object that can be used to iterate through the collection.</returns>
        ISearchTreeEnumerator<TKey, TValue> GetTreeEnumerator();
    }
}
