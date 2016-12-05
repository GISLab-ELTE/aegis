// <copyright file="HyperbolicCassiniSoldnerProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Hyperbolic Cassini-Soldner projection.
    /// </summary>
    [IdentifiedObject("EPSG::9833", "Hyperbolic Cassini-Soldner")]
    public class HyperbolicCassiniSoldnerProjection : CassiniSoldnerProjection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HyperbolicCassiniSoldnerProjection" /> class.
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
        public HyperbolicCassiniSoldnerProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperbolicCassiniSoldnerProjection" /> class.
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
        public HyperbolicCassiniSoldnerProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.HyperbolicCassiniSoldnerProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Computes the northing.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="x">The X value.</param>
        /// <returns>The northing.</returns>
        protected override Double ComputeNorthing(GeoCoordinate coordinate, Double x)
        {
            return this.falseNorthing + x - (Math.Pow(x, 3) / (6 * this.Ellipsoid.RadiusOfMeridianCurvature(coordinate.Latitude.BaseValue) * this.Ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude.BaseValue)));
        }

        /// <summary>
        /// Computes the M1 parameter.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The M1 value.</returns>
        protected override Double ComputeM1(Coordinate coordinate)
        {
            Double phi1 = this.latitudeOfNaturalOrigin + (coordinate.Y - this.falseNorthing) / 315320;
            Double ro = this.Ellipsoid.RadiusOfMeridianCurvature(phi1);
            Double v1 = this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi1);
            Double q_ = Math.Pow(coordinate.Y - this.falseNorthing, 3) / (6 * ro * v1);
            Double q = Math.Pow(coordinate.Y - this.falseNorthing + q_, 3) / (6 * ro * v1);

            return this.M0 + (coordinate.Y - this.falseNorthing) + q;
        }
    }
}
