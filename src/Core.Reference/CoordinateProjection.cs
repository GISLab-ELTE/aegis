// <copyright file="CoordinateProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a coordinate projection.
    /// </summary>
    public abstract class CoordinateProjection : CoordinateConversion<GeoCoordinate, Coordinate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateProjection" /> class.
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
        public CoordinateProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid));
            CoordinateOperationParameter[] parameterArray = this.Parameters.Keys.ToArray();

            // remeasure all length parameters to ellipsoid unit
            foreach (CoordinateOperationParameter parameter in parameterArray)
            {
                if ((this.Parameters[parameter] is Length) && ((Length)this.Parameters[parameter]).Unit != ellipsoid.SemiMajorAxis.Unit)
                    this.SetParameterValue(parameter, ((Length)this.Parameters[parameter]).ToUnit(ellipsoid.SemiMajorAxis.Unit));
            }

            this.AreaOfUse = areaOfUse ?? throw new ArgumentNullException(nameof(areaOfUse));
            this.Ellipsoid = ellipsoid;
        }

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid used by the operation.</value>
        public Ellipsoid Ellipsoid { get; private set; }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the operation is applicable.</value>
        public AreaOfUse AreaOfUse { get; private set; }
    }
}
