// <copyright file="IHeap.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines behavior of heaps containing key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the heap.</typeparam>
    /// <typeparam name="TValue">The type of the values in the heap.</typeparam>
    /// <remarks>
    /// A heap is a specialized tree-based data structure that satisfies the heap property:
    /// If A is a parent node of B then key(A) is ordered with respect to key(B) with the same ordering applying across the heap.
    /// </remarks>
    public interface IHeap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        /// <summary>
        /// Gets the number of elements actually contained in the heap.
        /// </summary>
        /// <value>The number of elements actually contained in the heap</value>
        Int32 Count { get; }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <value>The number of elements that the heap can contain before resizing is required.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Capacity is set to a value that is less than <see cref="Count" />.</exception>
        /// <exception cref="System.OutOfMemoryException">There is not enough memory available on the system.</exception>
        Int32 Capacity { get; set; }

        /// <summary>
        /// Gets the value at the top of the heap without removing it.
        /// </summary>
        /// <value>The value at the beginning of the heap.</value>
        TValue Peek { get; }

        /// <summary>
        /// Inserts the specified key and value to the heap.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        void Insert(TKey key, TValue value);

        /// <summary>
        /// Removes and returns the value at the top of the heap.
        /// </summary>
        /// <returns>The value that is removed from the top of the heap.</returns>
        /// <exception cref="System.InvalidOperationException">The heap is empty.</exception>
        TValue RemovePeek();

        /// <summary>
        /// Determines whether the heap contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the heap.</param>
        /// <returns><c>true</c> if the heap contains an element with the specified key; otherwise, <c>false</c>.</returns>
        Boolean Contains(TKey key);

        /// <summary>
        /// Removes all keys and values from the heap.
        /// </summary>
        void Clear();
    }
}
