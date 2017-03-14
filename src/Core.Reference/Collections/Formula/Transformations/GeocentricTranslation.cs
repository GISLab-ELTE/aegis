// <copyright file="GeocentricTranslation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a geocentric translation.
    /// </summary>
    [IdentifiedObject("EPSG::1031", "Geocentric translations (geocentric domain)")]
    public class GeocentricTranslation : CoordinateTransformation<Coordinate>
    {
        /// <summary>
        /// X axis translation.
        /// </summary>
        private readonly Double xAxisTranslation;

        /// <summary>
        /// Y axis translation.
        /// </summary>
        private readonly Double yAxisTranslation;

        /// <summary>
        /// Z axis translation.
        /// </summary>
        private readonly Double zAxisTranslation;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocentricTranslation" /> class.
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
        public GeocentricTranslation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                     CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocentricTranslation" /> class.
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
        public GeocentricTranslation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                     CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.GeocentricTranslationGeocentricDomain, parameters, source, target, areaOfUse)
        {
            this.xAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.XAxisTranslation);
            this.yAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.YAxisTranslation);
            this.zAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.ZAxisTranslation);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            return new Coordinate(coordinate.X + this.xAxisTranslation, coordinate.Y + this.yAxisTranslation, coordinate.Z + this.zAxisTranslation);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            return new Coordinate(coordinate.X - this.xAxisTranslation, coordinate.Y - this.yAxisTranslation, coordinate.Z - this.zAxisTranslation);
        }
    }
}
