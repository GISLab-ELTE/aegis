// <copyright file="IGeometry.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for geometries in coordinate space.
    /// </summary>
    /// <remarks>
    /// Geometries are the fundamental building blocks for geographic data storage as described by the Open Geospatial Consortium (OGC) Simple Feature Access (SFA) standard.
    /// </remarks>
    public interface IGeometry : IBasicGeometry
    {
        /// <summary>
        /// Gets the precision model of the geometry.
        /// </summary>
        /// <value>The precision model of the geometry.</value>
        PrecisionModel PrecisionModel { get; }

        /// <summary>
        /// Gets the reference system of the geometry.
        /// </summary>
        /// <value>The reference system of the geometry.</value>
        IReferenceSystem ReferenceSystem { get; }

        /// <summary>
        /// Gets the coordinate dimension of the geometry.
        /// </summary>
        /// <value>The coordinate dimension of the geometry. The coordinate dimension is equal to the dimension of the reference system, if provided.</value>
        Int32 CoordinateDimension { get; }

        /// <summary>
        /// Gets the spatial dimension of the geometry.
        /// </summary>
        /// <value>The spatial dimension of the geometry. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        Int32 SpatialDimension { get; }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        IGeometry Boundary { get; }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        Coordinate Centroid { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        Boolean IsSimple { get; }
    }
}
