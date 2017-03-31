// <copyright file="StoredPoint.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a point geometry located in a store.
    /// </summary>
    public class StoredPoint : StoredGeometry, IPoint
    {
        /// <summary>
        /// The string format for points. This field is constant.
        /// </summary>
        private const String PointStringFormat = "POINT ({0} {1} {2})";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredPoint" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredPoint(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier)
            : base(precisionModel, referenceSystem, driver, identifier, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredPoint" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="index">The index of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredPoint(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier, Int32 index)
            : base(precisionModel, referenceSystem, driver, identifier, new Int32[] { index })
        {
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
        public override Int32 SpatialDimension { get { return this.Coordinate.Z != 0 ? 3 : 2; } }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public override IGeometry Boundary { get { return null; } }

        /// <summary>
        /// Gets the centroid of the point.
        /// </summary>
        /// <value>The centroid of the point.</value>
        public override sealed Coordinate Centroid { get { return this.Coordinate; } }

        /// <summary>
        /// Gets a value indicating whether the point is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty { get { return this.Coordinate.IsEmpty; } }

        /// <summary>
        /// Gets a value indicating whether the point is simple.
        /// </summary>
        /// <value><c>true</c>, as a point is always considered to be simple.</value>
        public override sealed Boolean IsSimple { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the point is valid.
        /// </summary>
        /// <value><c>true</c> if the <see cref="Coordinate" /> associated with the point is valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid { get { return this.Coordinate.IsValid; } }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        public virtual Double X
        {
            get
            {
                return this.Coordinate.X;
            }

            set
            {
                Double preciseValue = this.PrecisionModel.MakePrecise(value);
                Coordinate currentCoordinate = this.ReadCoordinate(0);

                if (currentCoordinate.X != preciseValue)
                {
                    this.UpdateCoordinate(new Coordinate(preciseValue, currentCoordinate.Y, currentCoordinate.Z), 0);
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
                return this.Coordinate.Y;
            }

            set
            {
                Double preciseValue = this.PrecisionModel.MakePrecise(value);
                Coordinate currentCoordinate = this.ReadCoordinate(0);

                if (currentCoordinate.Y != preciseValue)
                {
                    this.UpdateCoordinate(new Coordinate(currentCoordinate.X, preciseValue, currentCoordinate.Z), 0);
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
                return this.Coordinate.Z;
            }

            set
            {
                Double preciseValue = this.PrecisionModel.MakePrecise(value);
                Coordinate currentCoordinate = this.ReadCoordinate(0);

                if (currentCoordinate.Z != preciseValue)
                {
                    this.UpdateCoordinate(new Coordinate(currentCoordinate.X, currentCoordinate.Y, preciseValue), 0);
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
                return this.ReadCoordinate(0);
            }

            set
            {
                this.UpdateCoordinate(this.PrecisionModel.MakePrecise(value), 0);
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
