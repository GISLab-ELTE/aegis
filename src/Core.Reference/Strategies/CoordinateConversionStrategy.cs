// <copyright file="CoordinateConversionStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Strategies
{
    using System;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a coordinate conversion strategy.
    /// </summary>
    /// <remarks>
    /// A coordinate conversion strategy is composed of two conversions (converting to geographic, and from geographic coordinates).
    /// </remarks>
    public class CoordinateConversionStrategy : ICoordinateOperationStrategy
    {
        /// <summary>
        /// The conversion to geographic coordinate. This field is read-only.
        /// </summary>
        private ICoordinateOperationStrategy<Coordinate, GeoCoordinate> conversionToGeographic;

        /// <summary>
        /// The conversion from geographic coordinate. This field is read-only.
        /// </summary>
        private ICoordinateOperationStrategy<GeoCoordinate, Coordinate> conversionFromGeographic;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateConversionStrategy" /> class.
        /// </summary>
        /// <param name="conversionToGeographic">The conversion to geographic coordinate.</param>
        /// <param name="conversionFromGeographic">The conversion from geographic coordinate.</param>
        /// <exception cref="System.ArgumentNullException">The conversion is null.</exception>
        public CoordinateConversionStrategy(ICoordinateOperationStrategy<Coordinate, GeoCoordinate> conversionToGeographic, ICoordinateOperationStrategy<GeoCoordinate, Coordinate> conversionFromGeographic)
        {
            if (conversionToGeographic == null)
                throw new ArgumentNullException(nameof(conversionToGeographic));
            this.conversionFromGeographic = conversionFromGeographic;
            this.conversionToGeographic = conversionToGeographic ?? throw new ArgumentNullException(nameof(conversionFromGeographic));
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
            return this.conversionFromGeographic.Apply(this.conversionToGeographic.Apply(coordinate));
        }
    }
}
