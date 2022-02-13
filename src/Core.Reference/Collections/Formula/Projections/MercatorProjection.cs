// <copyright file="MercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Numerics;

    /// <summary>
    /// Represents a general Mercator projection.
    /// </summary>
    public abstract class MercatorProjection : CoordinateProjection
    {
        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        protected readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// False easting.
        /// </summary>
        protected readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        protected readonly Double falseNorthing;

        /// <summary>
        /// Scale factor at natural origin.
        /// </summary>
        protected Double scaleFactorAtNaturalOrigin;

        /// <summary>
        /// The radius of the ellipsoid.
        /// </summary>
        protected Double ellipsoidRadius;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected Double[] inverseParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="MercatorProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected MercatorProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.ellipsoidRadius = this.Ellipsoid.SemiMajorAxis.Value;
            this.inverseParams = new Double[]
            {
                (this.Ellipsoid.EccentricitySquare / 2 + 5 * Math.Pow(this.Ellipsoid.Eccentricity, 4) / 24 + Math.Pow(this.Ellipsoid.Eccentricity, 6) / 12 + 13 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 360),
                (7 * Math.Pow(this.Ellipsoid.Eccentricity, 4) / 48 + 29 * Math.Pow(this.Ellipsoid.Eccentricity, 6) / 240 + 811 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 11520),
                (7 * Math.Pow(this.Ellipsoid.Eccentricity, 6) / 120 + 81 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 1120),
                4279 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 161280,
            };
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double easting, northing;

            if (coordinate.Latitude.BaseValue > 3.07)
                return null;

            if (this.Ellipsoid.IsSphere)
            {
                easting = this.falseEasting + this.ellipsoidRadius * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                northing = this.falseNorthing + this.ellipsoidRadius * Math.Log(Math.Tan(Math.PI / 4 + coordinate.Latitude.BaseValue / 2));
            }
            else
            {
                easting = this.falseEasting + this.ellipsoidRadius * this.scaleFactorAtNaturalOrigin * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                northing = this.falseNorthing + this.ellipsoidRadius * this.scaleFactorAtNaturalOrigin * Math.Log(Math.Tan(Math.PI / 4 + coordinate.Latitude.BaseValue / 2) * Math.Pow((1 - this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)), this.Ellipsoid.Eccentricity / 2));
            }

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double phi, lambda;

            if (this.Ellipsoid.IsSphere)
            {
                phi = Math.PI / 2 - 2 * Math.Atan(Math.Exp((this.falseNorthing - coordinate.Y) / this.ellipsoidRadius));
                lambda = (coordinate.X - this.falseEasting) / this.ellipsoidRadius + this.longitudeOfNaturalOrigin;
            }
            else
            {
                Double t = Math.Pow(Math.E, (this.falseNorthing - coordinate.Y) / (this.ellipsoidRadius * this.scaleFactorAtNaturalOrigin));
                Double xi = Math.PI / 2 - 2 * Math.Atan(t);

                lambda = (coordinate.X - this.falseEasting) / (this.ellipsoidRadius * this.scaleFactorAtNaturalOrigin) + this.longitudeOfNaturalOrigin;
                phi = xi + this.inverseParams[0] * Math.Sin(2 * xi)
                         + this.inverseParams[1] * Math.Sin(4 * xi)
                         + this.inverseParams[2] * Math.Sin(6 * xi)
                         + this.inverseParams[3] * Math.Sin(8 * xi);
            }

            return new GeoCoordinate(phi, lambda);
        }
    }
}
