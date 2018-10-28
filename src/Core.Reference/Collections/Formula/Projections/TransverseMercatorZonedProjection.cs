// <copyright file="TransverseMercatorZonedProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Zoned Transverse Mercator Projection.
    /// </summary>
    [IdentifiedObject("EPSG::9824", "Transverse Mercator Zoned Grid System")]
    public class TransverseMercatorZonedProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        private readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Initial longitude.
        /// </summary>
        private readonly Double initialLongitude;

        /// <summary>
        /// Zone width.
        /// </summary>
        private readonly Double zoneWidth;

        /// <summary>
        /// False easting.
        /// </summary>
        private readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        private readonly Double falseNorthing;

        /// <summary>
        /// Scale factor at natural origin.
        /// </summary>
        private readonly Double scaleFactorAtNaturalOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double bM0;

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
        /// Initializes a new instance of the <see cref="TransverseMercatorZonedProjection" /> class.
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
        public TransverseMercatorZonedProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseMercatorZonedProjection" /> class.
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
        public TransverseMercatorZonedProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.TransverseMercatorZonedProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.initialLongitude = this.GetParameterBaseValue(CoordinateOperationParameters.InitialLongitude);
            this.zoneWidth = this.GetParameterBaseValue(CoordinateOperationParameters.ZoneWidth);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.scaleFactorAtNaturalOrigin = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin);

            this.e2 = this.Ellipsoid.EccentricitySquare;
            this.e4 = Math.Pow(this.Ellipsoid.Eccentricity, 4);
            this.e6 = Math.Pow(this.Ellipsoid.Eccentricity, 6);
            this.e8 = Math.Pow(this.Ellipsoid.Eccentricity, 8);
            this.bM0 = this.ComputeM(this.latitudeOfNaturalOrigin);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue > 0 ? coordinate.Longitude.BaseValue : coordinate.Longitude.BaseValue + Math.PI;

            Int32 zoneNumber = Convert.ToInt32(Math.Floor((lambda + this.initialLongitude + this.zoneWidth) / this.zoneWidth));
            Double longitudeOfNaturalOrigin = zoneNumber * this.zoneWidth - this.initialLongitude + this.zoneWidth / 2;

            Double t = Calculator.Tan4(phi);
            Double c = this.e2 * Calculator.Cos2(phi) / (1 - this.e2);
            Double a = (lambda - longitudeOfNaturalOrigin) * Math.Cos(phi);
            Double m = this.ComputeM(phi);

            Double easting = zoneNumber * 1E6 + this.falseEasting + this.scaleFactorAtNaturalOrigin * this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * (a + (1 - t + c) * Math.Pow(a, 3) / 6 + (5 - 18 * t + t * t + 72 * c - 58 * this.Ellipsoid.SecondEccentricitySquare) * Math.Pow(a, 5) / 120);
            Double northing = this.falseNorthing + this.scaleFactorAtNaturalOrigin * (m - this.bM0 + this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Math.Tan(phi) * (a * a / 2 + (5 - t + 9 * c + 4 * c * c) * Math.Pow(a, 4) / 24 + (61 - 58 * t + t * t + 600 * c - 330 * this.Ellipsoid.SecondEccentricitySquare) * Math.Pow(a, 6) / 720));

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Int32 zoneNumber = Convert.ToInt32(Math.Floor(coordinate.X - this.falseEasting) / 1E6);

            Double m1 = this.bM0 + (coordinate.Y - this.falseNorthing);
            Double nu1 = m1 / (this.Ellipsoid.SemiMajorAxis.BaseValue * (1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256));
            Double e1 = (1 - Math.Sqrt(1 - this.e2)) / (1 + Math.Sqrt(1 - this.e2));
            Double phi1 = nu1 + (3 * e1 / 2 - 27 * Math.Pow(e1, 3) / 32) * Math.Sin(2 * nu1) +
                                (21 * e1 * e1 / 16 - 55 * Math.Pow(e1, 4) / 32) * Math.Sin(4 * nu1) +
                                (151 * Math.Pow(e1, 3) / 96) * Math.Sin(6 * nu1) + (1097 * Math.Pow(e1, 4) / 512) * Math.Sin(8 * nu1);
            Double d = (coordinate.X - (this.falseEasting + zoneNumber * 1E6)) / (this.Ellipsoid.RadiusOfMeridianCurvature(phi1) * this.scaleFactorAtNaturalOrigin);
            Double t1 = Calculator.Tan2(phi1);
            Double c1 = this.Ellipsoid.SecondEccentricitySquare * Calculator.Cos2(phi1);

            Double phi = phi1 - (this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi1) * Math.Tan(phi1) / this.Ellipsoid.RadiusOfMeridianCurvature(phi1)) * (d * d / 2 - (1 + 3 * t1) * Math.Pow(d, 4) / 24) +
                                (61 + 90 * t1 + 298 * c1 + 45 * t1 * t1 - 252 * this.Ellipsoid.SecondEccentricitySquare - 3 * c1 * c1 * Math.Pow(d, 6) / 720);
            Double lambda = (coordinate.X - (this.falseEasting + zoneNumber * 1E6)) / (this.Ellipsoid.RadiusOfMeridianCurvature(phi1) * this.scaleFactorAtNaturalOrigin);

            return new GeoCoordinate(phi, lambda);
        }

        /// <summary>
        /// Computes the M value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The M Value.</returns>
        protected Double ComputeM(Double latitude)
        {
            return this.Ellipsoid.SemiMajorAxis.BaseValue * ((1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * latitude - (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * latitude) +
                                                        (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * latitude) - 35 * this.e6 / 3072 * Math.Sin(6 * latitude));
        }
    }
}
