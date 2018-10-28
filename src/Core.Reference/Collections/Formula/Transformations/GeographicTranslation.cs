// <copyright file="GeographicTranslation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a geographic translation.
    /// </summary>
    [IdentifiedObject("EPSG::9619", "Geographic2D offsets")]
    public class GeographicTranslation : CoordinateTransformation<GeoCoordinate>
    {
        /// <summary>
        /// The latitude offset.
        /// </summary>
        private readonly Double latitudeOffset;

        /// <summary>
        /// The longitude offset.
        /// </summary>
        private readonly Double longitudeOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicTranslation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public GeographicTranslation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                     GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicTranslation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public GeographicTranslation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                     GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.Geographic2DOffsets, parameters, source, target, areaOfUse)
        {
            this.latitudeOffset = this.GetParameterValue(CoordinateOperationParameters.LatitudeOffset);
            this.longitudeOffset = this.GetParameterValue(CoordinateOperationParameters.LongitudeOffset);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeForward(GeoCoordinate coordinate)
        {
            return new GeoCoordinate(coordinate.Latitude.BaseValue + this.latitudeOffset, coordinate.Longitude.BaseValue + this.longitudeOffset);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(GeoCoordinate coordinate)
        {
            return new GeoCoordinate(coordinate.Latitude.BaseValue - this.latitudeOffset, coordinate.Longitude.BaseValue - this.longitudeOffset);
        }
    }
}
