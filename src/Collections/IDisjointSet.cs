// <copyright file="IDisjointSet.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Defines behavior of a disjoint-set data structure.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sets.</typeparam>
    /// <remarks>
    /// In computing, a disjoint-set data structure, also called a union–find data structure or merge–find set,
    /// is a data structure that keeps track of a set of elements partitioned into a number of disjoint (non-overlapping) subsets.
    /// </remarks>
    /// <author>Dávid Kis</author>
    public interface IDisjointSet<T> : IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Gets the number of elements in the disjoint-set.
        /// </summary>
        Int32 Count { get; }

        /// <summary>
        /// Gets the number of disjoint sets.
        /// </summary>
        Int32 SetCount { get; }

        /// <summary>
        /// Makes a set containing only the given element.
        /// </summary>
        /// <remarks>
        /// If the element is already in one of the subsets, nothing happens.
        /// </remarks>
        /// <param name="item">The element to add to the set.</param>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        void MakeSet(T item);

        /// <summary>
        /// Determine which subset a particular element is in.
        /// </summary>
        /// <param name="item">The element to locate in the set.</param>
        /// <returns>The representative for the subset containing <paramref name="item" />.</returns>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        /// <exception cref="System.ArgumentException">The item is not present in any set.</exception>
        T FindSet(T item);

        /// <summary>
        /// Tries to determine which subset a particular element is in.
        /// </summary>
        /// <param name="item">The element to locate in the set.</param>
        /// <param name="representative">The representative for the subset containing <paramref name="item" />.</param>
        /// <returns><c>true</c> if the element was successfully located within the disjoint set; otherwise, <c>false</c>.</returns>
        Boolean TryFindSet(T item, out T representative);

        /// <summary>
        /// Joins two subsets into a single subset.
        /// </summary>
        /// <remarks>
        /// If the elements are already in one subset, nothing happens.
        /// </remarks>
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
        void JoinSets(T first, T second);

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
        Boolean TryJoinSets(T first, T second);

        /// <summary>
        /// Removes all element from the disjoint-set.
        /// </summary>
        void Clear();
    }
}