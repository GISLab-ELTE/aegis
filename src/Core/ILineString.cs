// <copyright file="ILineString.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;

    /// <summary>
    /// Defines behavior for line string geometries.
    /// </summary>
    /// <remarks>
    /// A line string is a curve with linear interpolation between points. Each consecutive pair of points defines a Line segment.
    /// </remarks>
    public interface ILineString : ICurve, IBasicLineString
    {
        /// <summary>
        /// Gets a value indicating whether the curve is closed.
        /// </summary>
        /// <value><c>true</c> if the starting and ending coordinates are equal; otherwise, <c>false</c>.</value>
        new Boolean IsClosed { get; }

        /// <summary>
        /// Sets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to set.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        void SetCoordinate(Int32 index, Coordinate coordinate);

        /// <summary>
        /// Adds a coordinate to the end of the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        void Add(Coordinate coordinate);

        /// <summary>
        /// Inserts a coordinate into the line string at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        /// <exception cref="System.ArgumentException">The coordinate is not valid.</exception>
        void Insert(Int32 index, Coordinate coordinate);

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        Boolean Remove(Coordinate coordinate);

        /// <summary>
        /// Removes the coordinate at the specified index from the line string.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        void RemoveAt(Int32 index);

        /// <summary>
        /// Removes all coordinates from the line string.
        /// </summary>
        void Clear();
    }
}
