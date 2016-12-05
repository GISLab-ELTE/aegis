// <copyright file="BonneProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    using ELTE.AEGIS.Reference.Collections.Formula;

    /// <summary>
    /// Represents a Bonne projection.
    /// </summary>
    [IdentifiedObject("EPSG::9827", "Bonne")]
    public class BonneProjection : CoordinateProjection
    {
        /// <summary>
        /// False easting.
        /// </summary>
        protected readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        protected readonly Double falseNorthing;

        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        protected readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        protected readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double e4;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double e6;

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneProjection" /> class.
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
        public BonneProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneProjection" /> class.
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
        public BonneProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.Bonne, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneProjection" /> class.
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
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected BonneProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);

            this.e4 = Math.Pow(this.Ellipsoid.Eccentricity, 4);
            this.e6 = Math.Pow(this.Ellipsoid.Eccentricity, 6);
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

            Double m = this.Computem(phi);
            Double m0 = this.Computem(this.latitudeOfNaturalOrigin);
            Double bM = this.ComputeM(phi);
            Double bM0 = this.ComputeM(this.latitudeOfNaturalOrigin);

            Double rho = this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin) + bM0 - bM;
            Double bT = this.Ellipsoid.SemiMajorAxis.Value * m * (lambda - this.longitudeOfNaturalOrigin) / rho;

            Double easting = (rho * Math.Sin(bT)) + this.falseEasting;
            Double northing = (this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin) - rho * Math.Cos(bT)) + this.falseNorthing;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = coordinate.X - this.falseEasting;
            Double y = coordinate.Y - this.falseNorthing;

            return this.ComputeReverseInternal(x, y);
        }

        /// <summary>
        /// Computes the m value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The m value.</returns>
        protected Double Computem(Double latitude)
        {
            return Math.Cos(latitude) / Math.Pow(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(latitude), 0.5);
        }

        /// <summary>
        /// Computes the M value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The M value.</returns>
        protected Double ComputeM(Double latitude)
        {
            return this.Ellipsoid.SemiMajorAxis.BaseValue * ((1 - this.Ellipsoid.EccentricitySquare / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * latitude -
                                                        (3 * this.Ellipsoid.EccentricitySquare / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * latitude) +
                                                        (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * latitude) -
                                                        35 * this.e6 / 3072 * Math.Sin(6 * latitude));
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected GeoCoordinate ComputeReverseInternal(Double x, Double y)
        {
            Double m0 = this.Computem(this.latitudeOfNaturalOrigin);
            Double bM0 = this.ComputeM(this.latitudeOfNaturalOrigin);

            Double rho = Math.Sign(this.latitudeOfNaturalOrigin) * Math.Sqrt(x * x + Math.Pow(this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin) - y, 2));
            Double bM = this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin) + bM0 - rho;
            Double mu = bM / (this.Ellipsoid.SemiMajorAxis.Value * (1 - this.Ellipsoid.EccentricitySquare / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256));
            Double e1 = (1 - Math.Pow(1 - this.Ellipsoid.EccentricitySquare, 0.5)) / (1 + Math.Pow(1 - this.Ellipsoid.EccentricitySquare, 0.5));
            Double phi = mu +
                         (3 * e1 / 2 - 27 * Math.Pow(e1, 3) / 32) * Math.Sin(2 * mu) +
                         (21 * e1 * e1 / 16 - 55 * Math.Pow(e1, 4) / 32) * Math.Sin(4 * mu) +
                         (151 * Math.Pow(e1, 3) / 96) * Math.Sin(6 * mu) +
                         (1097 * Math.Pow(e1, 4) / 512) * Math.Sin(8 * mu);
            Double m = this.Computem(phi);

            if (Math.Abs(Math.Abs(phi) - Math.PI / 2) < 0.0001)
            {
                return new GeoCoordinate(phi, this.longitudeOfNaturalOrigin);
            }

            Double lambda;
            if (this.latitudeOfNaturalOrigin >= 0)
            {
                lambda = this.longitudeOfNaturalOrigin + rho *
                         Math.Atan(x / this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin) - y) /
                         this.Ellipsoid.SemiMajorAxis.Value * m;
            }
            else
            {
                lambda = this.longitudeOfNaturalOrigin + rho *
                         Math.Atan(-x / (y - this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin))) /
                         this.Ellipsoid.SemiMajorAxis.Value * m;
            }

            return new GeoCoordinate(phi, lambda);
        }
    }
}
