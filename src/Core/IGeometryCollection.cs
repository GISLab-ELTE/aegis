// <copyright file="IGeometryCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for generic geometry collections in coordinate space.
    /// </summary>
    /// <remarks>
    /// A geometry collection is a geometric object that is a collection of some number of geometric objects. Generally there are no constraints on its items other than the reference system and precision model of the items must match.
    /// </remarks>
    public interface IGeometryCollection : IGeometry, IEnumerable
    {
        /// <summary>
        /// Gets the number of geometries contained in the <see cref="IGeometryCollection" />.
        /// </summary>
        /// <value>
        /// The number of geometries contained in the <see cref="IGeometryCollection" />.
        /// </value>
        Int32 Count { get; }

        /// <summary>
        /// Gets a geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to get.</param>
        /// <returns>The geometry at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        IGeometry this[Int32 index] { get; }
    }

    /// <summary>
    /// Defines behavior for generic geometry collections in coordinate space.
    /// </summary>
    /// <typeparam name="GeometryType">The type of geometry.</typeparam>
    public interface IGeometryCollection<out GeometryType> : IGeometryCollection, IEnumerable<GeometryType>
        where GeometryType : IGeometry
    {
        /// <summary>
        /// Gets a geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to get.</param>
        /// <returns>The geometry at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        new GeometryType this[Int32 index] { get; }
    }
}