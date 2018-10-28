// <copyright file="LambertConicConformalProjection.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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
    /// Represents a Lambert Conic Conformal Projection.
    /// </summary>
    public abstract class LambertConicConformalProjection : CoordinateProjection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformalProjection" /> class.
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
        protected LambertConicConformalProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Computes the T parameter value.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The T value for the specified latitude.</returns>
        protected Double ComputeTValue(Double latitude)
        {
            return Math.Tan(Math.PI / 4 - latitude / 2) / Math.Pow((1 - this.Ellipsoid.Eccentricity * Math.Sin(latitude)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(latitude)), this.Ellipsoid.Eccentricity / 2);
        }

        /// <summary>
        /// Computes the latitude.
        /// </summary>
        /// <param name="t">The t parameter value.</param>
        /// <returns>The computed latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</returns>
        protected Double ComputeLatitude(Double t)
        {
            Double phi = Math.PI / 2 - 2 * Math.Atan(t);
            for (Int32 i = 0; i < 10; i++)
            {
                phi = Math.PI / 2 - 2 * Math.Atan(t * Math.Pow((1 - this.Ellipsoid.Eccentricity * Math.Sin(phi)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(phi)), this.Ellipsoid.Eccentricity / 2));
            }

            return phi;
        }
    }
}
