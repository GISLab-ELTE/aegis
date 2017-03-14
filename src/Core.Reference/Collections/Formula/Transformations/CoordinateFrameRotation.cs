// <copyright file="CoordinateFrameRotation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Coordinate Frame Rotation transformation.
    /// </summary>
    [IdentifiedObject("EPSG::1032", "Coordinate Frame Rotation (geocentric domain)")]
    public class CoordinateFrameRotation : HelmertTransformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateFrameRotation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public CoordinateFrameRotation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                       GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateFrameRotation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public CoordinateFrameRotation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                       GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.CoordinateFrameRotationGeocentricDomain, parameters, source, target, areaOfUse)
        {
            this.xAxisRotation = -this.xAxisRotation;
            this.yAxisRotation = -this.yAxisRotation;
            this.zAxisRotation = -this.zAxisRotation;

            Double det = 1 + this.xAxisRotation * this.xAxisRotation + this.yAxisRotation * this.yAxisRotation + this.zAxisRotation * this.zAxisRotation;
            this.inverseParams = new Double[3, 3];
            this.inverseParams[0, 0] = 1 / det * (1 + this.xAxisRotation * this.xAxisRotation);
            this.inverseParams[0, 1] = 1 / det * (this.zAxisRotation + this.xAxisRotation * this.yAxisRotation);
            this.inverseParams[0, 2] = 1 / det * (this.xAxisRotation * this.zAxisRotation - this.yAxisRotation);
            this.inverseParams[1, 0] = 1 / det * (this.xAxisRotation * this.yAxisRotation - this.zAxisRotation);
            this.inverseParams[1, 1] = 1 / det * (1 + this.yAxisRotation * this.yAxisRotation);
            this.inverseParams[1, 2] = 1 / det * (this.xAxisRotation + this.yAxisRotation * this.zAxisRotation);
            this.inverseParams[2, 0] = 1 / det * (this.xAxisRotation * this.zAxisRotation + this.yAxisRotation);
            this.inverseParams[2, 1] = 1 / det * (this.yAxisRotation * this.zAxisRotation - this.xAxisRotation);
            this.inverseParams[2, 2] = 1 / det * (1 + this.zAxisRotation * this.zAxisRotation);
        }
    }
}
