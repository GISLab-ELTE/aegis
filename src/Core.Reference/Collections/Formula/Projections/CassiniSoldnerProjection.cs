// <copyright file="CassiniSoldnerProjection.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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
    /// Represents a Cassini-Soldner projection.
    /// </summary>
    [IdentifiedObject("EPSG::9806", "Cassini-Soldner")]
    public class CassiniSoldnerProjection : CoordinateProjection
    {
        #region Protected fields

        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        protected readonly Double latitudeOfNaturalOrigin;

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
        /// Operation constant.
        /// </summary>
        protected readonly Double M0;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        protected readonly Double e2;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        protected readonly Double e4;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        protected readonly Double e6;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        protected readonly Double e8;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CassiniSoldnerProjection" /> class.
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
        public CassiniSoldnerProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, CoordinateOperationMethods.CassiniSoldnerProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CassiniSoldnerProjection" /> class.
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
        public CassiniSoldnerProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.CassiniSoldnerProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CassiniSoldnerProjection" /> class.
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
        protected CassiniSoldnerProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.e2 = this.Ellipsoid.EccentricitySquare;
            this.e4 = Math.Pow(this.Ellipsoid.Eccentricity, 4);
            this.e6 = Math.Pow(this.Ellipsoid.Eccentricity, 6);
            this.e8 = Math.Pow(this.Ellipsoid.Eccentricity, 8);
            this.M0 = this.Ellipsoid.SemiMajorAxis.Value * ((1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * this.latitudeOfNaturalOrigin -
                                                       (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * this.latitudeOfNaturalOrigin) +
                                                       (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * this.latitudeOfNaturalOrigin) -
                                                       35 * this.e6 / 3072 * Math.Sin(6 * this.latitudeOfNaturalOrigin));
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double bM = this.Ellipsoid.SemiMajorAxis.Value * ((1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * phi -
                                                         (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * phi) +
                                                         (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * phi) -
                                                         35 * this.e6 / 3072 * Math.Sin(6 * phi));

            Double bC = this.e2 * Calculator.Cos2(phi) / (1 - this.e2);
            Double bT = Calculator.Tan2(phi);
            Double bA = (lambda - this.longitudeOfNaturalOrigin) * Math.Cos(phi);
            Double bX = bM - this.M0 + this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Math.Tan(phi) * (bA * bA / 2 + (5 - bT + 6 * bC) * Math.Pow(bA, 4) / 24);

            Double easting = this.falseEasting + this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * (bA - bT * Math.Pow(bA, 3) / 6 - (8 - bT + 8 * bC) * bT * Math.Pow(bA, 5) / 120);
            Double northing = this.ComputeNorthing(coordinate, bX);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double bM1 = this.ComputeM1(coordinate);
            Double nu1 = bM1 / (this.Ellipsoid.SemiMajorAxis.Value * (1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256));
            Double e1 = (1 - Math.Sqrt(1 - this.e2)) / (1 + Math.Sqrt(1 - this.e2));
            Double phi1 = nu1 + (3 * e1 / 2 - 27 * Math.Pow(e1, 3) / 32) * Math.Sin(2 * nu1) +
                                (21 * Math.Pow(e1, 2) / 16 - 55 * Math.Pow(e1, 4) / 32) * Math.Sin(4 * nu1) +
                                (151 * Math.Pow(e1, 3) / 96) * Math.Sin(6 * nu1) + (1097 * Math.Pow(e1, 4) / 512) * Math.Sin(8 * nu1);
            Double bD = (coordinate.X - this.falseEasting) / this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi1);
            Double bT1 = Calculator.Tan2(phi1);

            Double phi = phi1 - (this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi1) * Math.Tan(phi1) / this.Ellipsoid.RadiusOfMeridianCurvature(phi1)) * (bD * bD / 2 - (1 + 3 * bT1) * Math.Pow(bD, 4) / 24);
            Double lambda = this.longitudeOfNaturalOrigin + (bD - bT1 * Math.Pow(bD, 3) / 3 + (1 + 3 * bT1) * bT1 * Math.Pow(bD, 5) / 15) / Math.Cos(phi1);

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the northing.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="x">The X value.</param>
        /// <returns>The northing.</returns>
        protected virtual Double ComputeNorthing(GeoCoordinate coordinate, Double x)
        {
            return this.falseNorthing + x;
        }

        /// <summary>
        /// Computes the M1 parameter.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The M1 value.</returns>
        protected virtual Double ComputeM1(Coordinate coordinate)
        {
            return this.M0 + (coordinate.Y - this.falseNorthing);
        }

        #endregion
    }
}
