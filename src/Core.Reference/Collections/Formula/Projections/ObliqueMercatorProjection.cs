// <copyright file="ObliqueMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents the Hotine Oblique Mercator projections.
    /// </summary>
    public abstract class ObliqueMercatorProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of projection centre.
        /// </summary>
        protected readonly Double latitudeOfProjectionCentre;

        /// <summary>
        /// Latitude of projection centre.
        /// </summary>
        protected readonly Double longitudeOfProjectionCentre;

        /// <summary>
        /// Scale factor on initial line.
        /// </summary>
        protected readonly Double scaleFactorOnInitialLine;

        /// <summary>
        /// Azimuth of initial line.
        /// </summary>
        protected readonly Double azimuthOfInitialLine;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double b;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObliqueMercatorProjection" /> class.
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
        protected ObliqueMercatorProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfProjectionCentre = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfProjectionCentre);
            this.longitudeOfProjectionCentre = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfProjectionCentre);
            this.scaleFactorOnInitialLine = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorOnInitialLine);
            this.azimuthOfInitialLine = this.GetParameterBaseValue(CoordinateOperationParameters.AzimuthOfInitialLine);

            this.b = Math.Sqrt(1 + (this.Ellipsoid.EccentricitySquare * Calculator.Cos4(this.latitudeOfProjectionCentre) / (1 - this.Ellipsoid.EccentricitySquare)));
        }
    }
}
