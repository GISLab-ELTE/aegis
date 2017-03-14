// <copyright file="LambertConicNearConformalProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Conic Near-Conformal projection.
    /// </summary>
    [IdentifiedObject("EPSG::9817", "Lambert Conic Near-Conformal")]
    public class LambertConicNearConformalProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        protected readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        protected readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// Scale factor at natural origin
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
        protected readonly Double A;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double a;

        /// <summary>
        /// Operation constant.
        /// </summary>

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double b;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double c;

        /// <summary>
        /// Operation constant.
        /// </summary>

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double d;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double e;

        /// <summary>
        /// Operation constant.
        /// </summary>

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double s0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double r0;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicNearConformalProjection" /> class.
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
        public LambertConicNearConformalProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicNearConformalProjection" /> class.
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
        public LambertConicNearConformalProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertConicNearConformalProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.scaleFactorAtNaturalOrigin = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            Double n = this.Ellipsoid.Flattening / (2 - this.Ellipsoid.Flattening);
            this.A = 1 / (6 * this.Ellipsoid.RadiusOfMeridianCurvature(this.latitudeOfNaturalOrigin) * this.Ellipsoid.RadiusOfPrimeVerticalCurvature(this.latitudeOfNaturalOrigin));
            this.a = this.Ellipsoid.SemiMajorAxis.BaseValue * (1 - n + 5 * (Math.Pow(n, 2) - Math.Pow(n, 3)) / 4 + 81 * (Math.Pow(n, 4) - Math.Pow(n, 5)) / 64) * Calculator.DegreeToRadian;
            this.b = 3 * this.Ellipsoid.SemiMajorAxis.BaseValue * (n - Math.Pow(n, 2) + 7 * (Math.Pow(n, 3) - Math.Pow(n, 4)) / 8 + 55 * Math.Pow(n, 5) / 64) / 2;
            this.c = 15 * this.Ellipsoid.SemiMajorAxis.BaseValue * (Math.Pow(n, 2) - Math.Pow(n, 3) + 3 * (Math.Pow(n, 4) - Math.Pow(n, 5)) / 4) / 16;
            this.d = 35 * this.Ellipsoid.SemiMajorAxis.BaseValue * (Math.Pow(n, 3) - Math.Pow(n, 4) + 11 * Math.Pow(n, 5) / 16) / 48;
            this.e = 315 * this.Ellipsoid.SemiMajorAxis.BaseValue * (Math.Pow(n, 4) - Math.Pow(n, 5)) / 512;
            this.r0 = this.scaleFactorAtNaturalOrigin * this.Ellipsoid.RadiusOfPrimeVerticalCurvature(this.latitudeOfNaturalOrigin) / Math.Tan(this.latitudeOfNaturalOrigin);
            this.s0 = this.a * (this.latitudeOfNaturalOrigin * Calculator.RadianToDegree) - this.b * Math.Sin(2 * this.latitudeOfNaturalOrigin) + this.c * Math.Sin(4 * this.latitudeOfNaturalOrigin) - this.d * Math.Sin(6 * this.latitudeOfNaturalOrigin) + this.e * Math.Sin(8 * this.latitudeOfNaturalOrigin);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double s = this.a * (coordinate.Latitude.BaseValue * Calculator.RadianToDegree) - this.b * Math.Sin(2 * coordinate.Latitude.BaseValue) + this.c * Math.Sin(4 * coordinate.Latitude.BaseValue) - this.d * Math.Sin(6 * coordinate.Latitude.BaseValue) + this.e * Math.Sin(8 * coordinate.Latitude.BaseValue);
            Double m = s - this.s0;
            Double bM = this.scaleFactorAtNaturalOrigin * (m + this.A * Math.Pow(m, 3));
            Double r = this.r0 - bM;
            Double theta = (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin) * Math.Sin(this.latitudeOfNaturalOrigin);

            Double easting = this.falseEasting + r * Math.Sin(theta);
            Double northing = this.falseNorthing + bM + r * Math.Sin(theta) * Math.Tan(theta / 2);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double theta_ = Math.Atan((coordinate.X - this.falseEasting) / (this.r0 - (coordinate.Y - this.falseNorthing)));
            Double rInv = Math.Sign(this.latitudeOfNaturalOrigin) * Math.Sqrt(Math.Pow(coordinate.X - this.falseEasting, 2) + Math.Pow(this.r0 - (coordinate.Y - this.falseNorthing), 2));
            Double bMInv = this.r0 - rInv;

            Double mInv = bMInv - (bMInv - this.scaleFactorAtNaturalOrigin * bMInv - this.scaleFactorAtNaturalOrigin * this.A * Math.Pow(bMInv, 3)) / (-1 * this.scaleFactorAtNaturalOrigin - 3 * this.scaleFactorAtNaturalOrigin * this.A * Math.Pow(bMInv, 2));
            Double phiInv = this.latitudeOfNaturalOrigin + mInv / this.a * Calculator.DegreeToRadian;
            Double sInv = this.a * (phiInv * Calculator.RadianToDegree) - this.b * Math.Sin(2 * phiInv) + this.c * Math.Sin(4 * phiInv) - this.d * Math.Sin(6 * phiInv) + this.e * Math.Sin(8 * phiInv);
            Double dsInv = this.a * Calculator.RadianToDegree - 2 * this.b * Math.Cos(2 * phiInv) + 4 * this.c * Math.Cos(4 * phiInv) - 6 * this.d * Math.Cos(6 * phiInv) + 8 * this.e * Math.Cos(8 * phiInv);

            Double phi = phiInv - (mInv + this.s0 - sInv) / (-1 * dsInv);
            Double lambda = this.longitudeOfNaturalOrigin + theta_ / Math.Sin(this.latitudeOfNaturalOrigin);

            return new GeoCoordinate(phi, lambda);
        }
    }
}
