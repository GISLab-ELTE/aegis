// <copyright file="ICoordinateIndex.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior of coordinate indexes.
    /// </summary>
    public interface ICoordinateIndex
    {
        /// <summary>
        /// Gets a value indicating whether the index is read-only.
        /// </summary>
        /// <value><c>true</c> if the index is read-only; otherwise, <c>false</c>.</value>
        Boolean IsReadOnly { get; }

        /// <summary>
        /// Gets the number of indexed geometries.
        /// </summary>
        /// <value>The number of indexed geometries.</value>
        Int32 NumberOfGeometries { get; }

        /// <summary>
        /// Adds a coordinate to the index.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Add(Coordinate coordinate);

        /// <summary>
        /// Adds multiple coordinates to the index.
        /// </summary>
        /// <param name="coordinates">The coordinate collection.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Add(IEnumerable<Coordinate> coordinates);

        /// <summary>
        /// Searches the index for any coordinates contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of coordinates located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        IEnumerable<Coordinate> Search(Envelope envelope);

        /// <summary>
        /// Determines whether the specified coordinate is indexed.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the specified coordinate is indexed; otherwise <c>false</c>.</returns>
        Boolean Contains(Coordinate coordinate);

        /// <summary>
        /// Removes the specified coordinate from the index.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate is indexed; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Coordinate coordinate);

        /// <summary>
        /// Removes all coordinates from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any coordinates are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope);

        /// <summary>
        /// Removes all coordinates from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="coordinates">The list of coordinates within the envelope.</param>
        /// <returns><c>true</c> if any coordinates are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope, out List<Coordinate> coordinates);

        /// <summary>
        /// Clears all coordinates from the index.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Clear();
    }
}
