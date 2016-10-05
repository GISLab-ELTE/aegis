// <copyright file="IBasicPoint.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Defines properties of basic point geometries.
    /// </summary>
    /// <remarks>
    /// A point is a 0-dimensional geometric object and represents a single location in coordinate space. The location is defined as a <see cref="Coordinate" /> instance.
    /// </remarks>
    public interface IBasicPoint : IBasicGeometry
    {
        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        Double X { get; }

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        Double Y { get; }

        /// <summary>
        /// Gets the Z coordinate.
        /// </summary>
        /// <value>The Z coordinate.</value>
        Double Z { get; }

        /// <summary>
        /// Gets the coordinate associated with the point.
        /// </summary>
        /// <value>The coordinate associated with the point.</value>
        Coordinate Coordinate { get; }
    }
}
