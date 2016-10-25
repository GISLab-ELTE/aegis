// <copyright file="LambertConicConformal2SPBelgiumProjection.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a Lambert Conic Conformal (2SP Belgium) projection.
    /// </summary>
    [IdentifiedObject("AEGIS::9803", "Lambert Conic Conformal (2SP Belgium)")]
    public class LambertConicConformal2SPBelgiumProjection : LambertConicConformal2SPProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPBelgiumProjection" /> class.
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
        public LambertConicConformal2SPBelgiumProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPBelgiumProjection" /> class.
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
        public LambertConicConformal2SPBelgiumProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertConicConformal2SPBelgiumProjection, parameters, ellipsoid, areaOfUse)
        {
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
        protected override void ComputeCoordinate(Double r, Double theta, out Double easting, out Double northing)
        {
            Double tF = this.ComputeTValue(this.latitudeOfFalseOrigin);
            Double rF = this.ComputeRFValue(tF);
            easting = this.eastingAtFalseOrigin + r * Math.Sin(theta - Angle.FromArcSecond(29.2985).BaseValue);
            northing = this.northingAtFalseOrigin + rF - r * Math.Cos(theta - Angle.FromArcSecond(29.2985).BaseValue);
        }

        /// <summary>
        /// Computes the longitude.
        /// </summary>
        /// <param name="theta">The Theta value.</param>
        /// <returns>The longitude.</returns>
        protected override Double ComputeLongitude(Double theta)
        {
            return ((theta + Angle.FromArcSecond(29.2985).BaseValue) / this.n) + this.longitudeOfFalseOrigin;
        }

        #endregion
    }
}
