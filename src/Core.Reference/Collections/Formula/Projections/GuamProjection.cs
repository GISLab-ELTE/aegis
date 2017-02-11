// <copyright file="GuamProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Numerics;

    /// <summary>
    /// Represents a Guam projection.
    /// </summary>
    [IdentifiedObject("AEGIS::9831", "Guam Projection")]
    public class GuamProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        private readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        private readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// False easting.
        /// </summary>
        private readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        private readonly Double falseNorthing;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double m0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double e1;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e2;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e4;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e6;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e8;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuamProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public GuamProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuamProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public GuamProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.GuamProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.e2 = this.Ellipsoid.EccentricitySquare;
            this.e4 = Math.Pow(this.Ellipsoid.Eccentricity, 4);
            this.e6 = Math.Pow(this.Ellipsoid.Eccentricity, 6);
            this.e8 = Math.Pow(this.Ellipsoid.Eccentricity, 8);

            this.m0 = this.ComputeM(this.latitudeOfNaturalOrigin);
            this.e1 = (1 - Math.Sqrt(1 - this.e2)) / (1 + Math.Sqrt(1 - this.e2));
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double x = this.Ellipsoid.SemiMajorAxis.BaseValue * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin) * Math.Cos(coordinate.Latitude.BaseValue) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(coordinate.Latitude.BaseValue));

            Double m = this.ComputeM(coordinate.Latitude.BaseValue);

            Double easting = this.falseEasting + x;
            Double northing = this.falseNorthing + m - this.m0 + (x * x * Math.Tan(coordinate.Latitude.BaseValue) * Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(coordinate.Latitude.BaseValue)) / (2 * this.Ellipsoid.SemiMajorAxis.BaseValue));

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double m, mu, phi = this.latitudeOfNaturalOrigin;

            for (Int32 iteration = 0; iteration < 3; iteration++)
            {
                Double deltaX = coordinate.X - this.falseEasting;
                m = this.m0 + coordinate.Y - this.falseNorthing - deltaX * deltaX * Math.Tan(phi) * Math.Sqrt(1 - this.e2 * Calculator.Sin2(phi)) / (2 * this.Ellipsoid.SemiMajorAxis.BaseValue);

                mu = m / this.Ellipsoid.SemiMajorAxis.BaseValue * (1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256);

                phi = mu + (3 * this.e1 / 2 - 27 * Math.Pow(this.e1, 3) / 32) * Math.Sin(2 * mu) + (21 * this.e1 * this.e1 / 16 - 55 * Math.Pow(this.e1, 4) / 32) * Math.Sin(4 * mu) + (151 * Math.Pow(this.e1, 3) / 96) * Math.Sin(6 * mu) + (1097 * Math.Pow(this.e1, 4) / 512) * Math.Sin(8 * mu);
            }

            Double lambda = this.longitudeOfNaturalOrigin + ((coordinate.X - this.falseEasting) * Math.Sqrt(1 - this.e2 * Calculator.Sin2(phi)) / (this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Cos(phi)));

            return new GeoCoordinate(phi, lambda);
        }

        /// <summary>
        /// Computes the M value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The M Value.</returns>
        private Double ComputeM(Double latitude)
        {
            return this.Ellipsoid.SemiMajorAxis.BaseValue * ((1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * latitude - (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * latitude) + (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * latitude) - 35 * this.e6 / 3072 * Math.Sin(6 * latitude));
        }
    }
}
