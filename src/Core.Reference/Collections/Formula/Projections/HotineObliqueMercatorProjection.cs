// <copyright file="HotineObliqueMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Hotine Oblique Mercator projection.
    /// </summary>
    public abstract class HotineObliqueMercatorProjection : ObliqueMercatorProjection
    {
        #region Protected fields

        /// <summary>
        /// Angle from rectified to skew grid.
        /// </summary>
        protected readonly Double angleFromRectifiedToSkewGrid;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double a;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double h;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double gammaO;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double lambdaO;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double uC;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double[] inverseParams;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HotineObliqueMercatorProjection" /> class.
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
        protected HotineObliqueMercatorProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.angleFromRectifiedToSkewGrid = this.GetParameterBaseValue(CoordinateOperationParameters.AngleFromRectifiedToSkewGrid);

            this.a = this.Ellipsoid.SemiMajorAxis.Value * this.b * this.scaleFactorOnInitialLine * Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare) / (1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfProjectionCentre));
            Double tO = Math.Tan(Math.PI / 4 - this.latitudeOfProjectionCentre / 2) / Math.Pow((1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfProjectionCentre)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfProjectionCentre)), this.Ellipsoid.Eccentricity / 2);
            Double d = this.b * Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare) / Math.Cos(this.latitudeOfProjectionCentre) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfProjectionCentre));
            Double f = d + Math.Sqrt(Math.Max(d * d, 1) - 1) * Math.Sign(this.latitudeOfProjectionCentre) * Math.Sign(this.latitudeOfProjectionCentre);
            this.h = f * Math.Pow(tO, this.b);
            Double g = (f - 1 / f) / 2;

            this.gammaO = Math.Asin(Math.Sin(this.azimuthOfInitialLine) / d);
            this.lambdaO = this.longitudeOfProjectionCentre - Math.Asin(g * Math.Tan(this.gammaO)) / this.b;

            if (this.azimuthOfInitialLine == Math.PI)
            {
                this.uC = this.Ellipsoid.SemiMajorAxis.Value * (this.longitudeOfProjectionCentre - this.lambdaO);
            }
            else
            {
                this.uC = (Math.Abs(this.azimuthOfInitialLine - Math.PI / 2) <= 1E-10) ?
                      this.a * (this.longitudeOfProjectionCentre - this.lambdaO) :
                      (this.a / this.b) * Math.Atan(Math.Sqrt(Math.Max(d * d, 1) - 1) / Math.Cos(this.azimuthOfInitialLine)) * Math.Sign(this.latitudeOfProjectionCentre);
            }

            this.inverseParams = new Double[]
            {
                this.Ellipsoid.EccentricitySquare / 2 + 5 * Math.Pow(this.Ellipsoid.Eccentricity, 4) / 24 + Math.Pow(this.Ellipsoid.Eccentricity, 6) / 12 + 13 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 360,
                7 * Math.Pow(this.Ellipsoid.Eccentricity, 4) / 48 + 29 * Math.Pow(this.Ellipsoid.Eccentricity, 6) / 240 + 811 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 11520,
                7 * Math.Pow(this.Ellipsoid.Eccentricity, 6) / 120 + 81 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 1120,
                4279 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 161280
            };
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the u and v values.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="u">The u value.</param>
        /// <param name="v">The v value.</param>
        protected void ComputeUV(Double latitude, Double longitude, out Double u, out Double v)
        {
            Double t = Math.Tan(Math.PI / 4 - latitude / 2) / Math.Pow((1 - this.Ellipsoid.Eccentricity * Math.Sin(latitude)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(latitude)), this.Ellipsoid.Eccentricity / 2);
            Double q = this.h / Math.Pow(t, this.b);
            Double bS = (q - 1 / q) / 2;
            Double bT = (q + 1 / q) / 2;
            Double bV = Math.Sin(this.b * (longitude - this.lambdaO));
            Double bU = (-bV * Math.Cos(this.gammaO) + bS * Math.Sin(this.gammaO)) / bT;

            u = this.ComputeU(bS, bV, longitude);
            v = this.a * Math.Log((1 - bU) / (1 + bU)) / (2 * this.b);
        }

        /// <summary>
        /// Computes the u value.
        /// </summary>
        /// <param name="s">The S value.</param>
        /// <param name="v">The V value.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The u value.</returns>
        protected abstract Double ComputeU(Double s, Double v, Double longitude);

        /// <summary>
        /// Computes the latitude and longitude.
        /// </summary>
        /// <param name="u">The u value.</param>
        /// <param name="v">The v value.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        protected void ComputeLatitudeLongitude(Double u, Double v, out Double latitude, out Double longitude)
        {
            Double q = Math.Exp(-this.b * v / this.a);
            Double bS = (q - 1 / q) / 2;
            Double bT = (q + 1 / q) / 2;
            Double bV = Math.Sin(this.b * u / this.a);
            Double bU = (bV * Math.Cos(this.gammaO) + bS * Math.Sin(this.gammaO)) / bT;
            Double t = Math.Pow(this.h / Math.Sqrt((1 + bU) / (1 - bU)), 1 / this.b);
            Double chi = Math.PI / 2 - 2 * Math.Atan(t);

            latitude = chi + this.inverseParams[0] * Math.Sin(2 * chi) + this.inverseParams[1] * Math.Sin(4 * chi) + this.inverseParams[2] * Math.Sin(6 * chi) + this.inverseParams[3] * Math.Sin(8 * chi);
            longitude = this.lambdaO - Math.Atan((bS * Math.Cos(this.gammaO) - bV * Math.Sin(this.gammaO)) / Math.Cos(this.b * u / this.a)) / this.b;
        }

        #endregion
    }
}
