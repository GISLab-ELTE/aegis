// <copyright file="ISpatialIndex.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Indexes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior of spatial indices.
    /// </summary>
    public interface ISpatialIndex
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
        /// Adds a geometry to the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Add(IGeometry geometry);

        /// <summary>
        /// Adds multiple geometries to the index.
        /// </summary>
        /// <param name="collection">The geometry collection.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Add(IEnumerable<IGeometry> collection);

        /// <summary>
        /// Searches the index for any geometries contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of geometries located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        IEnumerable<IGeometry> Search(Envelope envelope);

        /// <summary>
        /// Determines whether the specified geometry is indexed.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the specified geometry is indexed; otherwise <c>false</c>.</returns>
        Boolean Contains(IGeometry geometry);

        /// <summary>
        /// Removes the specified geometry from the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the geometry is indexed; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(IGeometry geometry);

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope);

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="geometries">The list of geometries within the envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        Boolean Remove(Envelope envelope, out List<IGeometry> geometries);

        /// <summary>
        /// Clears all geometries from the index.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The index is read-only.</exception>
        void Clear();
    }
}
