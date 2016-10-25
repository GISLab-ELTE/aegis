// <copyright file="AlbersEqualAreaProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an Albers Equal Area projection.
    /// </summary>
    [IdentifiedObject("EPSG::9822", "Albers Equal Area")]
    public class AlbersEqualAreaProjection : CoordinateProjection
    {
        #region Private fields

        /// <summary>
        /// Easting at false origin.
        /// </summary>
        private readonly Double eastingAtFalseOrigin;

        /// <summary>
        /// Northing at false origin.
        /// </summary>
        private readonly Double northingAtFalseOrigin;

        /// <summary>
        /// Latitude of false origin.
        /// </summary>
        private readonly Double latitudeOfFalseOrigin;

        /// <summary>
        /// Latitude of 1st standard parallel.
        /// </summary>
        private readonly Double latitudeOf1stStandardParallel;

        /// <summary>
        /// Latitude of 2nd standard parallel.
        /// </summary>
        private readonly Double latitudeOf2ndStandardParallel;

        /// <summary>
        /// Longitude of false origin.
        /// </summary>
        private readonly Double longitudeOfFalseOrigin;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e;

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
        /// Operation constant.
        /// </summary>
        private readonly Double alpha0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double n;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double c;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbersEqualAreaProjection" /> class.
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
        public AlbersEqualAreaProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbersEqualAreaProjection" /> class.
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
        public AlbersEqualAreaProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.AlbersEqualAreaProjection, parameters, ellipsoid, areaOfUse)
        {
            this.eastingAtFalseOrigin = this.GetParameterValue(CoordinateOperationParameters.EastingAtFalseOrigin);
            this.northingAtFalseOrigin = this.GetParameterValue(CoordinateOperationParameters.NorthingAtFalseOrigin);
            this.latitudeOfFalseOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfFalseOrigin);
            this.latitudeOf1stStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOf1stStandardParallel);
            this.latitudeOf2ndStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOf2ndStandardParallel);
            this.longitudeOfFalseOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfFalseOrigin);

            this.e = this.Ellipsoid.Eccentricity;
            this.e2 = this.Ellipsoid.EccentricitySquare;
            this.e4 = Math.Pow(this.Ellipsoid.Eccentricity, 4);
            this.e6 = Math.Pow(this.Ellipsoid.Eccentricity, 6);

            Double m1 = this.ComputeM(this.latitudeOf1stStandardParallel);
            Double m2 = this.ComputeM(this.latitudeOf2ndStandardParallel);

            this.alpha0 = this.ComputeAlpha(this.latitudeOfFalseOrigin);
            Double alpha1 = this.ComputeAlpha(this.latitudeOf1stStandardParallel);
            Double alpha2 = this.ComputeAlpha(this.latitudeOf2ndStandardParallel);

            this.n = this.ComputeN(m1, m2, alpha1, alpha2);
            this.c = this.ComputeC(m1, this.n, alpha1);
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
            Double alpha = this.ComputeAlpha(phi);

            Double theta = this.n * (lambda - this.longitudeOfFalseOrigin);
            Double rho = (this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Pow(this.c - this.n * alpha, 0.5)) / this.n;
            Double rho0 = (this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Pow(this.c - this.n * this.alpha0, 0.5)) / this.n;

            Double easting = this.eastingAtFalseOrigin + (rho * Math.Sin(theta));
            Double northing = this.northingAtFalseOrigin + rho0 - (rho * Math.Cos(theta));

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double rho0 = (this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Pow(this.c - this.n * this.alpha0, 0.5)) / this.n;
            Double deltaX = coordinate.X - this.eastingAtFalseOrigin;
            Double deltaRho = rho0 - (coordinate.Y - this.northingAtFalseOrigin);

            Double rho = Math.Sqrt(deltaX * deltaX + deltaRho * deltaRho);
            Double theta = Math.Atan((coordinate.X - this.eastingAtFalseOrigin) / (rho0 - (coordinate.Y - this.northingAtFalseOrigin)));
            Double alpha_ = (this.c - (rho * rho * this.n * this.n / (this.Ellipsoid.SemiMajorAxis.BaseValue * this.Ellipsoid.SemiMajorAxis.BaseValue))) / this.n;
            Double beta_ = Math.Asin(alpha_ / (1 - ((1 - this.e2) / (2 * this.e))) * Math.Log((1 - this.e) / (1 + this.e)));

            Double lambda = this.longitudeOfFalseOrigin + (theta / this.n);
            Double phi = beta_ + (((this.e2 / 3) + (31 * this.e4 / 180) + (517 * this.e6 / 5040)) * Math.Sin(2 * beta_)) +
                                 (((23 * this.e4 / 360) + (251 * this.e6 / 3780)) * Math.Sin(4 * beta_)) +
                                 ((761 * this.e6 / 45360) * Math.Sin(6 * beta_));

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Private utility methods

        /// <summary>
        /// Computes the Alpha value.
        /// </summary>
        /// <param name="phi">The latitude (expressed in radian).</param>
        /// <returns>The Alpha value.</returns>
        private Double ComputeAlpha(Double phi)
        {
            return (1 - this.e2) * ((Math.Sin(phi) / (1 - this.e2 * Calculator.Sin2(phi))) - (1 / (2 * this.e)) * Math.Log((1 - this.e * Math.Sin(phi)) / (1 + this.e * Math.Sin(phi))));
        }

        /// <summary>
        /// Computes the m value.
        /// </summary>
        /// <param name="phi">The latitude (expressed in radian).</param>
        /// <returns>The m value.</returns>
        private Double ComputeM(Double phi)
        {
            return Math.Cos(phi) / Math.Pow(1 - this.e2 * Calculator.Sin2(phi), 0.5);
        }

        /// <summary>
        /// Computes the C value.
        /// </summary>
        /// <param name="m1">The m1.</param>
        /// <param name="n">The n.</param>
        /// <param name="alpha1">The alpha1.</param>
        /// <returns>The Alpha value.</returns>
        private Double ComputeC(Double m1, Double n, Double alpha1)
        {
            return m1 * m1 + (n * alpha1);
        }

        /// <summary>
        /// Computes the Alpha value.
        /// </summary>
        /// <param name="m1">The first M value.</param>
        /// <param name="m2">The second M value.</param>
        /// <param name="alpha1">The first alpha value.</param>
        /// <param name="alpha2">The second alpha value.</param>
        /// <returns>The Alpha value.</returns>
        private Double ComputeN(Double m1, Double m2, Double alpha1, Double alpha2)
        {
            return (m1 * m1 - m2 * m2) / (alpha2 - alpha1);
        }

        #endregion
    }
}
