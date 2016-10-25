// <copyright file="LambertConicConformal1SPProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Conic Conformal Projection (with 1 Standard Parallel).
    /// </summary>
    [IdentifiedObject("EPSG::9801", "Lambert Conic Conformal (1SP)")]
    public class LambertConicConformal1SPProjection : LambertConicConformalProjection
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
        /// Scale factor at natural origin.
        /// </summary>
        protected readonly Double scaleFactorAtNaturalOrigin;

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
        protected readonly Double r0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double n;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double f;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPProjection" /> class.
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
        public LambertConicConformal1SPProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, CoordinateOperationMethods.LambertConicConformal1SPProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPProjection" /> class.
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
        public LambertConicConformal1SPProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertConicConformal1SPProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPProjection" /> class.
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
        protected LambertConicConformal1SPProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.scaleFactorAtNaturalOrigin = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            Double m0 = Math.Cos(this.latitudeOfNaturalOrigin) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfNaturalOrigin));
            Double t0 = this.ComputeTValue(this.latitudeOfNaturalOrigin);
            this.n = Math.Sin(this.latitudeOfNaturalOrigin);
            this.f = m0 / (this.n * Math.Pow(t0, this.n));
            this.r0 = this.Ellipsoid.SemiMajorAxis.Value * this.f * Math.Pow(t0, this.n) * this.scaleFactorAtNaturalOrigin;
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
            Double t = this.ComputeTValue(coordinate.Latitude.BaseValue);
            Double r = this.Ellipsoid.SemiMajorAxis.Value * this.f * Math.Pow(t, this.n) * this.scaleFactorAtNaturalOrigin;
            Double theta = this.n * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
            Double easting, northing;

            this.ComputeCoordinate(r, theta, out easting, out northing);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double r, theta;
            this.ComputeParams(coordinate.X, coordinate.Y, out r, out theta);

            Double t = Math.Pow(r / (this.Ellipsoid.SemiMajorAxis.Value * this.scaleFactorAtNaturalOrigin * this.f), 1 / this.n);

            Double phi = this.ComputeLatitude(t);
            Double lambda = theta / this.n + this.longitudeOfNaturalOrigin;

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Compute the coordinate easting and northing based on the R and Theta values.
        /// </summary>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        protected virtual void ComputeCoordinate(Double r, Double theta, out Double easting, out Double northing)
        {
            easting = this.falseEasting + r * Math.Sin(theta);
            northing = this.falseNorthing + this.r0 - r * Math.Cos(theta);
        }

        /// <summary>
        /// Compute the transformations parameters R and Theta based on the easting and northing values.
        /// </summary>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        protected virtual void ComputeParams(Double easting, Double northing, out Double r, out Double theta)
        {
            theta = Math.Atan((easting - this.falseEasting) / (this.r0 - (northing - this.falseNorthing)));
            r = Math.Sign(this.n) * Math.Sqrt(Math.Pow(easting - this.falseEasting, 2) + Math.Pow(this.r0 - (northing - this.falseNorthing), 2));
        }

        #endregion
    }
}
