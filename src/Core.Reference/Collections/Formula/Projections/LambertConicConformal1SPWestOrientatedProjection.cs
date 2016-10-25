// <copyright file="LambertConicConformal1SPWestOrientatedProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Conic Conformal (1SP West Orientated) projection.
    /// </summary>
    [IdentifiedObject("AEGIS::9826", "Lambert Conic Conformal (West Orientated)")]
    public class LambertConicConformal1SPWestOrientatedProjection : LambertConicConformal1SPProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPWestOrientatedProjection" /> class.
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
        public LambertConicConformal1SPWestOrientatedProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPWestOrientatedProjection" /> class.
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
        public LambertConicConformal1SPWestOrientatedProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertConicConformal1SPWestOrientatedProjection, parameters, ellipsoid, areaOfUse)
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
            easting = this.falseEasting - r * Math.Sin(theta);
            northing = this.falseNorthing + this.r0 - r * Math.Cos(theta);
        }

        /// <summary>
        /// Compute the transformations parameters R and Theta based on the easting and northing values.
        /// </summary>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        protected override void ComputeParams(Double easting, Double northing, out Double r, out Double theta)
        {
            theta = Math.Atan((this.falseEasting - easting) / (this.r0 - (northing - this.falseNorthing)));
            r = Math.Sign(this.n) * Math.Sqrt(Math.Pow(this.falseEasting - easting, 2) + Math.Pow(this.r0 - (northing - this.falseNorthing), 2));
        }

        #endregion
    }
}
