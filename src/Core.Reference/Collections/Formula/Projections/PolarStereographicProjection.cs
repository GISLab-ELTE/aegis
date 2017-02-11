// <copyright file="PolarStereographicProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Numerics;

    /// <summary>
    /// Represents a Polar Stereographic Projection.
    /// </summary>
    public abstract class PolarStereographicProjection : CoordinateProjection
    {
        /// <summary>
        /// The operation aspect.
        /// </summary>
        protected OperationAspect operationAspect;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarStereographicProjection" /> class.
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
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected PolarStereographicProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Computes the t value.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The t value.</returns>
        protected Double ComputeT(Double latitude)
        {
            Double t = 0.0;

            switch (this.operationAspect)
            {
                case OperationAspect.NorthPolar:
                    t = Math.Tan(Math.PI / 4 - latitude / 2) * Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(latitude)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(latitude)), this.Ellipsoid.Eccentricity / 2);
                    break;
                case OperationAspect.SouthPolar:
                    t = Math.Tan(Math.PI / 4 + latitude / 2) / Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(latitude)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(latitude)), this.Ellipsoid.Eccentricity / 2);
                    break;
            }

            return t;
        }
    }
}
