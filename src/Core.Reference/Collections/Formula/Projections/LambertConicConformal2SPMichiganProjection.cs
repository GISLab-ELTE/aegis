// <copyright file="LambertConicConformal2SPMichiganProjection.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a Lambert Conic Conformal (2 SP Michigan) Projection.
    /// </summary>
    [IdentifiedObject("EPSG::1051", "Lambert Conic Conformal (2SP Michigan)")]
    public class LambertConicConformal2SPMichiganProjection : LambertConicConformal2SPProjection
    {
        /// <summary>
        /// Ellipsoid scaling factor.
        /// </summary>
        protected readonly Double ellipsoidScalingFactor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPMichiganProjection" /> class.
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
        public LambertConicConformal2SPMichiganProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPMichiganProjection" /> class.
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
        public LambertConicConformal2SPMichiganProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertConicConformal2SPMichiganProjection, parameters, ellipsoid, areaOfUse)
        {
            this.ellipsoidScalingFactor = this.GetParameterValue(CoordinateOperationParameters.EllipsoidScalingFactor);
        }

        /// <summary>
        /// Computes the rF value based on the tF value.
        /// </summary>
        /// <param name="tF">The tF value.</param>
        /// <returns>
        /// The rF value.
        /// </returns>
        protected override Double ComputeRFValue(Double tF)
        {
            return this.Ellipsoid.SemiMajorAxis.Value * this.ellipsoidScalingFactor * this.f * Math.Pow(tF, this.n);
        }

        /// <summary>
        /// Computes the r value based on the t value.
        /// </summary>
        /// <param name="t">The t value.</param>
        /// <returns>
        /// The r value
        /// </returns>
        protected override Double ComputeRValue(Double t)
        {
            return this.Ellipsoid.SemiMajorAxis.Value * this.ellipsoidScalingFactor * this.f * Math.Pow(t, this.n);
        }

        /// <summary>
        /// Computes the t value for the reverse projection based on the r value.
        /// </summary>
        /// <param name="r">The r value.</param>
        /// <returns>
        /// The t value.
        /// </returns>
        protected override Double ComputeTReverseValue(Double r)
        {
            return Math.Pow(r / (this.Ellipsoid.SemiMajorAxis.Value * this.ellipsoidScalingFactor * this.f), 1 / this.n);
        }
    }
}
