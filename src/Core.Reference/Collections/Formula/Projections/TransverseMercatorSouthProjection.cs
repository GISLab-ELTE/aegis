// <copyright file="TransverseMercatorSouthProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Transverse Mercator (South Orientated) Projection.
    /// </summary>
    [IdentifiedObject("EPSG::9808", "Transverse Mercator (South Orientated)")]
    public class TransverseMercatorSouthProjection : CoordinateProjection
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
        /// Scale factor at natural origin.
        /// </summary>
        private readonly Double scaleFactorAtNaturalOrigin;

        /// <summary>
        /// Latitude of 1st standard parallel.
        /// </summary>
        private readonly Double latitudeOf1stStandardParallel;

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
        /// Operation constant.
        /// </summary>
        private Double bM0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseMercatorSouthProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public TransverseMercatorSouthProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseMercatorSouthProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public TransverseMercatorSouthProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.TransverseMercatorSouthProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.scaleFactorAtNaturalOrigin = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin);
            this.latitudeOf1stStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOf1stStandardParallel);

            this.e2 = Math.Pow(this.Ellipsoid.Eccentricity, 2);
            this.e4 = Math.Pow(this.Ellipsoid.Eccentricity, 4);
            this.e6 = Math.Pow(this.Ellipsoid.Eccentricity, 6);
            this.e8 = Math.Pow(this.Ellipsoid.Eccentricity, 8);
            this.bM0 = this.Ellipsoid.SemiMajorAxis.Value * ((1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * this.latitudeOfNaturalOrigin -
                                                       (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * this.latitudeOfNaturalOrigin) +
                                                       (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * this.latitudeOfNaturalOrigin) -
                                                       35 * this.e6 / 3072 * Math.Sin(6 * this.latitudeOfNaturalOrigin));
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;
            Double bT = Math.Pow(Math.Tan(phi), 2);
            Double bC = this.e2 * Math.Pow(Math.Cos(phi), 2) / (1 - this.e2);
            Double bA = (lambda - this.longitudeOfNaturalOrigin) * Math.Cos(phi);
            Double nu = this.Ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - this.e2 * Math.Sin(phi) * Math.Sin(phi));
            Double bM = this.Ellipsoid.SemiMajorAxis.Value * ((1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * phi -
                                                     (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * phi) +
                                                     (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * phi) -
                                                     35 * this.e6 / 3072 * Math.Sin(6 * phi));
            Double westing = this.falseEasting - this.scaleFactorAtNaturalOrigin * nu * (bA + (1 - bT + bC) * Math.Pow(bA, 3) / 6 + (5 - 18 * bT + bT * bT + 72 * bC - 58 * this.Ellipsoid.SecondEccentricitySquare) * Math.Pow(bA, 5) / 120);
            Double southing = this.falseNorthing - this.scaleFactorAtNaturalOrigin * (bM - this.bM0 + nu * Math.Tan(phi) * (bA * bA / 2 + (5 - bT + 9 * bC + 4 * bC * bC) * Math.Pow(bA, 4) / 24 + (61 - 58 * bT + bT * bT + 600 * bC - 330 * this.Ellipsoid.SecondEccentricitySquare) * Math.Pow(bA, 6) / 720));

            return new Coordinate(westing, southing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double westing = coordinate.X;
            Double southing = coordinate.Y;
            Double bM1 = this.bM0 - (southing - this.falseNorthing) / this.scaleFactorAtNaturalOrigin;
            Double e1 = (1 - Math.Sqrt(1 - this.e2)) / (1 + Math.Sqrt(1 - this.e2));
            Double mu1 = bM1 / (this.Ellipsoid.SemiMajorAxis.Value * (1 - this.e2 / 4 - 3 * Math.Pow(this.Ellipsoid.Eccentricity, 4) / 64 - 5 * Math.Pow(this.Ellipsoid.Eccentricity, 6) / 256));
            Double phi1 = mu1 + (3 * e1 / 2 - 27 * Math.Pow(e1, 3) / 32) * Math.Sin(2 * mu1) +
                                (21 * Math.Pow(e1, 2) / 16 - 55 * Math.Pow(e1, 4) / 32) * Math.Sin(4 * mu1) +
                                (151 * Math.Pow(e1, 3) / 96) * Math.Sin(6 * mu1) + (1097 * Math.Pow(e1, 4) / 512) * Math.Sin(8 * mu1);
            Double nu1 = this.Ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - this.e2 * Math.Pow(Math.Sin(phi1), 2));
            Double rho1 = this.Ellipsoid.SemiMajorAxis.Value * (1 - this.e2) / Math.Pow(1 - this.e2 * Math.Pow(Math.Sin(phi1), 2), 1.5);
            Double bT1 = Math.Pow(Math.Tan(phi1), 2);
            Double bC1 = this.Ellipsoid.SecondEccentricitySquare * Math.Pow(Math.Cos(this.scaleFactorAtNaturalOrigin), 2);
            Double e2 = this.e2 / (1 - this.e2);
            Double bD = -(westing - this.falseEasting) / (nu1 * this.scaleFactorAtNaturalOrigin);
            Double phi = phi1 - (nu1 * Math.Tan(phi1) / rho1) * (bD * bD / 2 -
                (5 + 3 * bT1 + 10 * bC1 - 4 * bC1 * bC1 - 9 * this.Ellipsoid.SecondEccentricitySquare) * Math.Pow(bD, 4) / 24 +
                (61 + 90 * bT1 + 298 * bC1 + 45 * bT1 * bT1 - 252 * this.Ellipsoid.SecondEccentricitySquare - 3 * bC1 * bC1) * Math.Pow(bD, 6) / 720);
            Double lambda = this.longitudeOfNaturalOrigin + (bD - (1 + 2 * bT1 + bC1) * Math.Pow(bD, 3) / 6 +
                (5 - 2 * bC1 + 28 * bT1 - 3 * bC1 * bC1 + 8 * this.Ellipsoid.SecondEccentricitySquare + 24 * bT1 * bT1) * Math.Pow(bD, 5) / 120) / Math.Cos(phi1);

            return new GeoCoordinate(phi, lambda);
        }
    }
}
