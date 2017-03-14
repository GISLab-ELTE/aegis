// <copyright file="LambertConicConformal2SPProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Conic Conformal Projection (with 2 Standard Parallels).
    /// </summary>
    [IdentifiedObject("EPSG::9802", "Lambert Conic Conformal (2SP)")]
    public class LambertConicConformal2SPProjection : LambertConicConformalProjection
    {
        /// <summary>
        /// Latitude of false origin.
        /// </summary>
        protected readonly Double latitudeOfFalseOrigin;

        /// <summary>
        /// Longitude of false origin.
        /// </summary>
        protected readonly Double longitudeOfFalseOrigin;

        /// <summary>
        /// Latitude of1st standard parallel.
        /// </summary>
        protected readonly Double latitudeOf1stStandardParallel;

        /// <summary>
        /// Latitude of2nd standard parallel.
        /// </summary>
        protected readonly Double latitudeOf2ndStandardParallel;

        /// <summary>
        /// Easting at false origin.
        /// </summary>
        protected readonly Double eastingAtFalseOrigin;

        /// <summary>
        /// Northing at false origin.
        /// </summary>
        protected readonly Double northingAtFalseOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double n;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double f;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
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
        public LambertConicConformal2SPProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, CoordinateOperationMethods.LambertConicConformal2SPProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
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
        public LambertConicConformal2SPProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertConicConformal2SPProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPProjection" /> class.
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
        protected LambertConicConformal2SPProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfFalseOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfFalseOrigin);
            this.longitudeOfFalseOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfFalseOrigin);
            this.latitudeOf1stStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOf1stStandardParallel);
            this.latitudeOf2ndStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOf2ndStandardParallel);
            this.eastingAtFalseOrigin = this.GetParameterValue(CoordinateOperationParameters.EastingAtFalseOrigin);
            this.northingAtFalseOrigin = this.GetParameterValue(CoordinateOperationParameters.NorthingAtFalseOrigin);

            Double m1 = Math.Cos(this.latitudeOf1stStandardParallel) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOf1stStandardParallel));
            Double m2 = Math.Cos(this.latitudeOf2ndStandardParallel) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOf2ndStandardParallel));
            Double t1 = this.ComputeTValue(this.latitudeOf1stStandardParallel);
            Double t2 = this.ComputeTValue(this.latitudeOf2ndStandardParallel);
            this.n = (Math.Log(m1) - Math.Log(m2)) / (Math.Log(t1) - Math.Log(t2));
            this.f = m1 / (this.n * Math.Pow(t1, this.n));
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double t = this.ComputeTValue(coordinate.Latitude.BaseValue);
            Double r = this.ComputeRValue(t);
            Double theta = this.n * (coordinate.Longitude.BaseValue - this.longitudeOfFalseOrigin);
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
            Double tF = this.ComputeTValue(this.latitudeOfFalseOrigin);
            Double rF = this.ComputeRFValue(tF);
            Double theta = Math.Atan((coordinate.X - this.eastingAtFalseOrigin) / (rF - (coordinate.Y - this.northingAtFalseOrigin)));
            Double r = Math.Sign(this.n) * Math.Sqrt(Math.Pow(coordinate.X - this.eastingAtFalseOrigin, 2) + Math.Pow(rF - (coordinate.Y - this.northingAtFalseOrigin), 2));
            Double t = this.ComputeTReverseValue(r);

            Double phi = this.ComputeLatitude(t);
            Double lambda = this.ComputeLongitude(theta);

            return new GeoCoordinate(phi, lambda);
        }

        /// <summary>
        /// Computes the rF value based on the tF value.
        /// </summary>
        /// <param name="tF">The tF value.</param>
        /// <returns>The rF value.</returns>
        protected virtual Double ComputeRFValue(Double tF)
        {
            return this.Ellipsoid.SemiMajorAxis.Value * this.f * Math.Pow(tF, this.n);
        }

        /// <summary>
        /// Computes the r value based on the t value.
        /// </summary>
        /// <param name="t">The t value.</param>
        /// <returns>The r value</returns>
        protected virtual Double ComputeRValue(Double t)
        {
            return this.Ellipsoid.SemiMajorAxis.Value * this.f * Math.Pow(t, this.n);
        }

        /// <summary>
        /// Computes the t value for the reverse projection based on the r value.
        /// </summary>
        /// <param name="r">The r value.</param>
        /// <returns>The t value.</returns>
        protected virtual Double ComputeTReverseValue(Double r)
        {
            return Math.Pow(r / (this.Ellipsoid.SemiMajorAxis.Value * this.f), 1 / this.n);
        }

        /// <summary>
        /// Compute the coordinate easting and northing based on the R and Theta values.
        /// </summary>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        protected virtual void ComputeCoordinate(Double r, Double theta, out Double easting, out Double northing)
        {
            Double tF = this.ComputeTValue(this.latitudeOfFalseOrigin);
            Double rF = this.ComputeRFValue(tF);
            easting = this.eastingAtFalseOrigin + r * Math.Sin(theta);
            northing = this.northingAtFalseOrigin + rF - r * Math.Cos(theta);
        }

        /// <summary>
        /// Computes the longitude.
        /// </summary>
        /// <param name="theta">The Theta value.</param>
        /// <returns>The longitude.</returns>
        protected virtual Double ComputeLongitude(Double theta)
        {
            return theta / this.n + this.longitudeOfFalseOrigin;
        }
    }
}
