// <copyright file="P6LeftHandedSeismicBinGridTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a P6 left handed seismic bin grid transformation.
    /// </summary>
    [IdentifiedObject("AEGIS::1049", "P6 (I = J-90°) seismic bin grid transformation")]
    public class P6LeftHandedSeismicBinGridTransformation : P6SeismicBinGridTransformation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="P6LeftHandedSeismicBinGridTransformation" /> class.
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
        public P6LeftHandedSeismicBinGridTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                                        CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="P6LeftHandedSeismicBinGridTransformation" /> class.
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
        public P6LeftHandedSeismicBinGridTransformation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                                        CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.P6LeftHandedSeismicBinGridTransformation, parameters, source, target, areaOfUse)
        {
            this.orientation = Orientation.LeftHanded;
        }

        #endregion
    }
}
