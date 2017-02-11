// <copyright file="CompoundCoordinateConversionStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Strategies
{
    using System;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a compound coordinate conversion strategy.
    /// </summary>
    /// <remarks>
    /// A compound coordinate conversion strategy is composed of a geographic transformation and two conversions (converting to geographic, and from geographic coordinates).
    /// </remarks>
    public class CompoundCoordinateConversionStrategy : ICoordinateOperationStrategy
    {
        /// <summary>
        /// The conversion to geographic coordinate. This field is read-only.
        /// </summary>
        private readonly ICoordinateOperationStrategy<Coordinate, GeoCoordinate> conversionToGeographic;

        /// <summary>
        /// The geographic transformation. This field is read-only.
        /// </summary>
        private readonly ICoordinateOperationStrategy<GeoCoordinate, GeoCoordinate> geographicTransformation;

        /// <summary>
        /// The conversion from geographic coordinate. This field is read-only.
        /// </summary>
        private readonly ICoordinateOperationStrategy<GeoCoordinate, Coordinate> conversionFromGeographic;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundCoordinateConversionStrategy" /> class.
        /// </summary>
        /// <param name="conversionToGeographic">The conversion to geographic coordinate.</param>
        /// <param name="geographicTransformation">The geographic transformation.</param>
        /// <param name="conversionFromGeographic">The conversion from geographic coordinate.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The conversion is null.
        /// or
        /// The transformation is null.
        /// </exception>
        public CompoundCoordinateConversionStrategy(ICoordinateOperationStrategy<Coordinate, GeoCoordinate> conversionToGeographic, ICoordinateOperationStrategy<GeoCoordinate, GeoCoordinate> geographicTransformation, ICoordinateOperationStrategy<GeoCoordinate, Coordinate> conversionFromGeographic)
        {
            if (conversionToGeographic == null)
                throw new ArgumentNullException(nameof(conversionToGeographic), ReferenceMessages.ConversionIsNull);
            if (conversionToGeographic == null)
                throw new ArgumentNullException(nameof(geographicTransformation), ReferenceMessages.TransformationIsNull);
            if (conversionToGeographic == null)
                throw new ArgumentNullException(nameof(conversionFromGeographic), ReferenceMessages.ConversionIsNull);

            this.conversionFromGeographic = conversionFromGeographic;
            this.geographicTransformation = geographicTransformation;
            this.conversionToGeographic = conversionToGeographic;
        }

        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        public ReferenceSystem SourceReferenceSystem { get { return this.conversionToGeographic.SourceReferenceSystem; } }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        public ReferenceSystem TargetReferenceSystem { get { return this.conversionFromGeographic.TargetReferenceSystem; } }

        /// <summary>
        /// Applies the strategy on the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public Coordinate Apply(Coordinate coordinate)
        {
            return this.conversionFromGeographic.Apply(this.geographicTransformation.Apply(this.conversionToGeographic.Apply(coordinate)));
        }
    }
}
