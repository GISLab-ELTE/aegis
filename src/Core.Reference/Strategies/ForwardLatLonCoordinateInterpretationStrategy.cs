// <copyright file="ForwardLatLonCoordinateInterpretationStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Strategies
{
    using System;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a forward coordinate interpretation for (latitude, longitude) representation.
    /// </summary>
    public class ForwardLatLonCoordinateInterpretationStrategy : ICoordinateOperationStrategy<GeoCoordinate, Coordinate>
    {
        /// <summary>
        /// The coordinate reference system. This field is read-only.
        /// </summary>
        protected readonly CoordinateReferenceSystem referenceSystem;

        /// <summary>
        /// The unit of measurement of the latitude. This field is read-only.
        /// </summary>
        protected readonly UnitOfMeasurement latitudeUnit;

        /// <summary>
        /// The unit of measurement of the longitude. This field is read-only.
        /// </summary>
        protected readonly UnitOfMeasurement longitudeUnit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForwardLatLonCoordinateInterpretationStrategy" /> class.
        /// </summary>
        /// <param name="referenceSystem">The coordinate reference system.</param>
        /// <exception cref="System.ArgumentNullException">The reference system is null.</exception>
        /// <exception cref="System.ArgumentException">The dimension of the coordinate system is less than expected.</exception>
        public ForwardLatLonCoordinateInterpretationStrategy(CoordinateReferenceSystem referenceSystem)
        {
            if (referenceSystem == null)
                throw new ArgumentNullException(nameof(referenceSystem));
            if (referenceSystem.CoordinateSystem.Dimension < 2)
                throw new ArgumentException(ReferenceMessages.CoordinateSystemDimensionLessThanExpected, nameof(referenceSystem));

            this.referenceSystem = referenceSystem;
            this.latitudeUnit = this.referenceSystem.CoordinateSystem.GetAxis(0).Unit;
            this.longitudeUnit = this.referenceSystem.CoordinateSystem.GetAxis(1).Unit;
        }

        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        public ReferenceSystem Source { get { return this.referenceSystem; } }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        public ReferenceSystem Target { get { return this.referenceSystem; } }

        /// <summary>
        /// Applies the strategy on the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public virtual Coordinate Apply(GeoCoordinate coordinate)
        {
            return new Coordinate(coordinate.Latitude.GetValue(this.latitudeUnit), coordinate.Longitude.GetValue(this.longitudeUnit));
        }
    }
}
