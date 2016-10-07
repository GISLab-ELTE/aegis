// <copyright file="ICurve.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for curve geometries.
    /// </summary>
    /// <remarks>
    /// A curve is a one-dimensional geometric object usually stored as a sequence of points, with the subtype of the curve specifying the form of the interpolation between the points.
    /// </remarks>
    public interface ICurve : IGeometry
    {
        /// <summary>
        /// Gets a value indicating whether the curve is closed.
        /// </summary>
        /// <value><c>true</c> if the starting and ending coordinates are equal; otherwise, <c>false</c>.</value>
        Boolean IsClosed { get; }

        /// <summary>
        /// Gets a value indicating whether the curve is a ring.
        /// </summary>
        /// <value><c>true</c> if the curve is simple and closed; otherwise, <c>false</c>.</value>
        Boolean IsRing { get; }

        /// <summary>
        /// Gets the length of the curve.
        /// </summary>
        /// <value>The length of the curve.</value>
        Double Length { get; }

        /// <summary>
        /// Gets the staring point.
        /// </summary>
        /// <value>The first point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        IPoint StartPoint { get; }

        /// <summary>
        /// Gets the ending point.
        /// </summary>
        /// <value>The last point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        IPoint EndPoint { get; }
    }
}
