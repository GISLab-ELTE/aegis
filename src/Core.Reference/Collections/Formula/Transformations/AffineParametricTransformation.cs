// <copyright file="AffineParametricTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents an affine parametric transformation.
    /// </summary>
    [IdentifiedObject("EPSG::9624 ", "Affine parametric transformation")]
    public class AffineParametricTransformation : CoordinateTransformation<Coordinate>
    {
        /// <summary>
        /// A0 parameter.
        /// </summary>
        private readonly Double a0;

        /// <summary>
        /// A1 parameter.
        /// </summary>
        private readonly Double a1;

        /// <summary>
        /// A2 parameter.
        /// </summary>
        private readonly Double a2;

        /// <summary>
        /// B0 parameter.
        /// </summary>
        private readonly Double b0;

        /// <summary>
        /// B1 parameter.
        /// </summary>
        private readonly Double b1;

        /// <summary>
        /// B2 parameter.
        /// </summary>
        private readonly Double b2;

        /// <summary>
        /// Inverted A1 parameter.
        /// </summary>
        private readonly Double a0Inv;

        /// <summary>
        /// Inverted A0 parameter.
        /// </summary>
        private readonly Double a1Inv;

        /// <summary>
        /// Inverted A2 parameter.
        /// </summary>
        private readonly Double a2Inv;

        /// <summary>
        /// Inverted B0 parameter.
        /// </summary>
        private readonly Double b0Inv;

        /// <summary>
        /// Inverted B2 parameter.
        /// </summary>
        private readonly Double b1Inv;

        /// <summary>
        /// Inverted A0 parameter.
        /// </summary>
        private readonly Double b2Inv;

        /// <summary>
        /// Initializes a new instance of the <see cref="AffineParametricTransformation" /> class.
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
        public AffineParametricTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                              CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffineParametricTransformation" /> class.
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
        public AffineParametricTransformation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                              CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.AffineParametricTransformation, parameters, source, target, areaOfUse)
        {
            this.a0 = this.GetParameterValue(CoordinateOperationParameters.A0);
            this.a1 = this.GetParameterValue(CoordinateOperationParameters.A1);
            this.a2 = this.GetParameterValue(CoordinateOperationParameters.A2);
            this.b0 = this.GetParameterValue(CoordinateOperationParameters.B0);
            this.b1 = this.GetParameterValue(CoordinateOperationParameters.B1);
            this.b2 = this.GetParameterValue(CoordinateOperationParameters.B2);

            Double d = this.a1 * this.b2 - this.a2 * this.b1;
            this.a0Inv = (this.a2 * this.b0 - this.b2 * this.a0) / d;
            this.b0Inv = (this.b1 * this.a0 - this.a1 * this.b0) / d;
            this.a1Inv = this.b2 / d;
            this.a2Inv = -this.a2 / d;
            this.b1Inv = -this.b1 / d;
            this.b2Inv = this.a1 / d;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            return new Coordinate(this.a0 + this.a1 * coordinate.X + this.a2 * coordinate.Y, this.b0 + this.b1 * coordinate.X + this.b2 * coordinate.Y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            return new Coordinate(this.a0Inv + this.a1Inv * coordinate.X + this.a2Inv * coordinate.Y, this.b0Inv + this.b1Inv * coordinate.X + this.b2Inv * coordinate.Y);
        }
    }
}
