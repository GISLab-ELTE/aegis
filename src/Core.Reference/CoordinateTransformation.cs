// <copyright file="CoordinateTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a coordinate transformation.
    /// </summary>
    /// <typeparam name="CoordinateType">The type of the coordinate.</typeparam>
    /// <remarks>
    /// A coordinate operation through which the input and output coordinates are referenced to different datums.
    /// The parameters of a coordinate transformation are empirically derived from data containing the coordinates of a series of points in both coordinate reference systems.
    /// This computational process is usually "over-determined", allowing derivation of error (or accuracy) estimates for the coordinate transformation.
    /// Also, the stochastic nature of the parameters may result in multiple (different) versions of the same coordinate transformations between the same source and target CRSs.
    /// </remarks>
    public abstract class CoordinateTransformation<CoordinateType> : CoordinateOperation<CoordinateType, CoordinateType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateTransformation{CoordinateType}" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected CoordinateTransformation(String identifier, String name, String remarks, String[] aliases,
                                           CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters,
                                           CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), ReferenceMessages.SourceCoordinateReferenceSystemIsNull);
            if (target == null)
                throw new ArgumentNullException(nameof(target), ReferenceMessages.TargetCoordinateReferenceSystemIsNull);
            if (areaOfUse == null)
                throw new ArgumentNullException(nameof(areaOfUse), ReferenceMessages.AreaOfUseIsNull);

            this.Source = source;
            this.Target = target;
            this.AreaOfUse = areaOfUse;
        }

        /// <summary>
        /// Gets the source coordinate reference system.
        /// </summary>
        /// <value>The source coordinate reference system.</value>
        public CoordinateReferenceSystem Source { get; private set; }

        /// <summary>
        /// Gets the target coordinate reference system.
        /// </summary>
        /// <value>The target coordinate reference system.</value>
        public CoordinateReferenceSystem Target { get; private set; }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the operation is applicable.</value>
        public AreaOfUse AreaOfUse { get; private set; }
    }
}
