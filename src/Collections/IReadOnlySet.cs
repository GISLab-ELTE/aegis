// <copyright file="IReadOnlySet.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Provides the base interface for the abstraction of read-only sets.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public interface IReadOnlySet<T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a proper subset of other; otherwise, <c>false/c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsProperSubsetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a proper superset of other; otherwise, <c>false/c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsProperSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a subset of other; otherwise, <c>false/c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsSubsetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a superset of other; otherwise, <c>false/c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool IsSupersetOf(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set and other share at least one common element; otherwise, <c>false/c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool Overlaps(IEnumerable<T> other);

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is equal to other; otherwise, <c>false/c>.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="other"/> is null.</exception>
        bool SetEquals(IEnumerable<T> other);
    }
}
