// <copyright file="ISurface.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;

    /// <summary>
    /// Defines behavior for surface geometries.
    /// </summary>
    /// <remarks>
    /// A surface is a 2-dimensional geometric object.
    /// </remarks>
    public interface ISurface : IGeometry
    {
        /// <summary>
        /// Gets a value indicating whether the surface is convex.
        /// </summary>
        /// <value><c>true</c> if the surface is convex; otherwise, <c>false</c>.</value>
        Boolean IsConvex { get; }

        /// <summary>
        /// Gets a value indicating whether the surface is divided.
        /// </summary>
        /// <value><c>true</c> if the surface is divided; otherwise, <c>false</c>.</value>
        Boolean IsDivided { get; }

        /// <summary>
        /// Gets a value indicating whether the surface is whole.
        /// </summary>
        /// <value><c>true</c> if the surface is whole; otherwise, <c>false</c>.</value>
        Boolean IsWhole { get; }

        /// <summary>
        /// Gets the area of the surface.
        /// </summary>
        /// <value>The area of the surface.</value>
        Double Area { get; }

        /// <summary>
        /// Gets the perimeter of the surface.
        /// </summary>
        /// <value>The perimeter of the surface.</value>
        Double Perimeter { get; }
    }
}
