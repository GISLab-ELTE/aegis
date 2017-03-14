// <copyright file="HotineObliqueMercatorAProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Numerics;

    /// <summary>
    /// Represents a Hotine Oblique Mercator (Variant A) projection.
    /// </summary>
    [IdentifiedObject("EPSG::9812", "Hotine Oblique Mercator (variant A)")]
    public class HotineObliqueMercatorAProjection : HotineObliqueMercatorProjection
    {
        /// <summary>
        /// False easting.
        /// </summary>
        private readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        private readonly Double falseNorthing;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotineObliqueMercatorAProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public HotineObliqueMercatorAProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotineObliqueMercatorAProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public HotineObliqueMercatorAProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.HotineObliqueMercatorAProjection, parameters, ellipsoid, areaOfUse)
        {
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double u, v;
            this.ComputeUV(coordinate.Latitude.BaseValue, coordinate.Longitude.BaseValue, out u, out v);

            Double easting = v * Math.Cos(this.angleFromRectifiedToSkewGrid) + u * Math.Sin(this.angleFromRectifiedToSkewGrid) + this.falseEasting;
            Double northing = u * Math.Cos(this.angleFromRectifiedToSkewGrid) - v * Math.Sin(this.angleFromRectifiedToSkewGrid) + this.falseNorthing;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double v = (coordinate.X - this.falseEasting) * Math.Cos(this.angleFromRectifiedToSkewGrid) - (coordinate.Y - this.falseNorthing) * Math.Sin(this.angleFromRectifiedToSkewGrid);
            Double u = (coordinate.Y - this.falseNorthing) * Math.Cos(this.angleFromRectifiedToSkewGrid) + (coordinate.X - this.falseEasting) * Math.Sin(this.angleFromRectifiedToSkewGrid);
            Double latitude, longitude;

            this.ComputeLatitudeLongitude(u, v, out latitude, out longitude);

            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Computes the u value.
        /// </summary>
        /// <param name="s">The S value.</param>
        /// <param name="v">The V value.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The u value.</returns>
        protected override Double ComputeU(Double s, Double v, Double longitude)
        {
            return this.a * Math.Atan((s * Math.Cos(this.gammaO) + v * Math.Sin(this.gammaO)) / Math.Cos(this.b * (longitude - this.lambdaO))) / this.b;
        }
    }
}
