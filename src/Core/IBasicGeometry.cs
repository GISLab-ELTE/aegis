// <copyright file="IBasicGeometry.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;

    /// <summary>
    /// Defines properties of basic geometries.
    /// </summary>
    /// <remarks>
    /// A basic geometry is a simplification of the general geometries described by the Open Geospatial Consortium (OGC) Simple feature access (SFA) standard.
    /// Basic geometries serve as lightweight, read-only objects, which define basic properties used by geometric algorithms.
    /// </remarks>
    public interface IBasicGeometry
    {
        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>The inherent dimension of the geometry.</value>
        Int32 Dimension { get; }

        /// <summary>
        /// Gets the minimum bounding <see cref="Envelope" /> of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        Envelope Envelope { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        Boolean IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        Boolean IsValid { get; }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        String ToString(IFormatProvider provider);
    }
}
