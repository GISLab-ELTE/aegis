// <copyright file="Point.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;

    /// <summary>
    /// Represents a point geometry in Cartesian coordinate space.
    /// </summary>
    public class Point : Geometry, IPoint
    {
        /// <summary>
        /// The string format for points. This field is constant.
        /// </summary>
        private const String PointStringFormat = "POINT ({0} {1} {2})";

        /// <summary>
        /// The coordinate.
        /// </summary>
        private Coordinate coordinate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Point" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public Point(PrecisionModel precisionModel, IReferenceSystem referenceSystem, Double x, Double y, Double z)
            : base(precisionModel, referenceSystem)
        {
            this.coordinate = new Coordinate(this.PrecisionModel.MakePrecise(x), this.PrecisionModel.MakePrecise(y), this.PrecisionModel.MakePrecise(z));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point" /> class.
        /// </summary>
        /// <param name="factory">The factory of the point.</param>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public Point(IGeometryFactory factory, Double x, Double y, Double z)
            : base(factory)
        {
            this.coordinate = new Coordinate(this.PrecisionModel.MakePrecise(x), this.PrecisionModel.MakePrecise(y), this.PrecisionModel.MakePrecise(z));
        }

        /// <summary>
        /// Gets the inherent dimension of the point.
        /// </summary>
        /// <value><c>0</c>, which is the defined dimension of a point.</value>
        public override sealed Int32 Dimension { get { return 0; } }

        /// <summary>
        /// Gets the spatial dimension of the point.
        /// </summary>
        /// <value>The spatial dimension of the point. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        public override Int32 SpatialDimension { get { return this.coordinate.Z != 0 ? 3 : 2; } }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding envelope of the geometry.</value>
        public override Envelope Envelope
        {
            get
            {
                return new Envelope(this.coordinate.X, this.coordinate.X, this.coordinate.Y, this.coordinate.Y, this.coordinate.Z, this.coordinate.Z);
            }
        }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public override IGeometry Boundary { get { return null; } }

        /// <summary>
        /// Gets the centroid of the point.
        /// </summary>
        /// <value>The centroid of the point.</value>
        public override sealed Coordinate Centroid { get { return this.coordinate; } }

        /// <summary>
        /// Gets a value indicating whether the point is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty { get { return this.coordinate.IsEmpty; } }

        /// <summary>
        /// Gets a value indicating whether the point is simple.
        /// </summary>
        /// <value><c>true</c>, as a point is always considered to be simple.</value>
        public override sealed Boolean IsSimple { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the point is valid.
        /// </summary>
        /// <value><c>true</c> if the <see cref="Coordinate" /> associated with the point is valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid { get { return this.coordinate.IsValid; } }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        public virtual Double X
        {
            get
            {
                return this.coordinate.X;
            }

            set
            {
                Double preciseValue = this.PrecisionModel.MakePrecise(value);
                if (this.coordinate.X != preciseValue)
                {
                    this.coordinate = new Coordinate(preciseValue, this.coordinate.Y, this.coordinate.Z);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        public virtual Double Y
        {
            get
            {
                return this.coordinate.Y;
            }

            set
            {
                Double preciseValue = this.PrecisionModel.MakePrecise(value);
                if (this.coordinate.Y != preciseValue)
                {
                    this.coordinate = new Coordinate(this.coordinate.X, preciseValue, this.coordinate.Z);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Z coordinate.
        /// </summary>
        /// <value>The Z coordinate.</value>
        public virtual Double Z
        {
            get
            {
                return this.coordinate.Z;
            }

            set
            {
                Double preciseValue = this.PrecisionModel.MakePrecise(value);
                if (this.coordinate.Z != preciseValue)
                {
                    this.coordinate = new Coordinate(this.coordinate.X, this.coordinate.Y, preciseValue);
                }
            }
        }

        /// <summary>
        /// Gets or sets the coordinate associated with the point.
        /// </summary>
        /// <value>The coordinate associated with the point.</value>
        public Coordinate Coordinate
        {
            get
            {
                return this.coordinate;
            }

            set
            {
                Coordinate preciseValue = this.PrecisionModel.MakePrecise(value);
                if (!this.coordinate.Equals(preciseValue))
                {
                    this.coordinate = preciseValue;
                }
            }
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return String.Format(provider, PointStringFormat, this.Coordinate.X, this.Coordinate.Y, this.Coordinate.Z);
        }
    }
}
