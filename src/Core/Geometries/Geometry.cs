// <copyright file="Geometry.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a general geometry in Cartesian coordinate space.
    /// </summary>
    /// <remarks>
    /// Geometry is the root class of the hierarchy.
    /// </remarks>
    public abstract class Geometry : IGeometry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        protected Geometry(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
        {
            this.Factory = new GeometryFactory(precisionModel, referenceSystem);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geometry" /> class.
        /// </summary>
        /// <param name="factory">The factory of the geometry.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        protected Geometry(IGeometryFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);

            this.Factory = factory;
        }

        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        public IGeometryFactory Factory { get; private set; }

        /// <summary>
        /// Gets the precision model of the geometry.
        /// </summary>
        /// <value>The precision model of the geometry.</value>
        public PrecisionModel PrecisionModel { get { return this.Factory.PrecisionModel; } }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>The inherent dimension of the geometry.</value>
        public abstract Int32 Dimension { get; }

        /// <summary>
        /// Gets the coordinate dimension of the geometry.
        /// </summary>
        /// <value>The coordinate dimension of the geometry. The coordinate dimension is equal to the dimension of the reference system, if provided.</value>
        public virtual Int32 CoordinateDimension { get { return (this.ReferenceSystem != null) ? this.ReferenceSystem.Dimension : this.SpatialDimension; } }

        /// <summary>
        /// Gets the spatial dimension of the geometry.
        /// </summary>
        /// <value>The spatial dimension of the geometry. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        public virtual Int32 SpatialDimension { get { return (this.Envelope.Minimum.Z != 0 || this.Envelope.Maximum.Z != 0) ? 3 : 2; } }

        /// <summary>
        /// Gets the reference system of the geometry.
        /// </summary>
        /// <value>The reference system of the geometry.</value>
        public IReferenceSystem ReferenceSystem { get { return this.Factory.ReferenceSystem; } }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding envelope of the geometry.</value>
        public abstract Envelope Envelope { get; }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public abstract IGeometry Boundary { get; }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public abstract Coordinate Centroid { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public abstract Boolean IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public abstract Boolean IsSimple { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public abstract Boolean IsValid { get; }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override sealed String ToString()
        {
            return this.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public abstract String ToString(IFormatProvider provider);
    }
}
