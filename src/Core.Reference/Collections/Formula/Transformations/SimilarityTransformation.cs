// <copyright file="SimilarityTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a similarity transformation.
    /// </summary>
    [IdentifiedObject("AEGIS::9621", "Similarity Transformation")]
    public class SimilarityTransformation : CoordinateTransformation<Coordinate>
    {
        /// <summary>
        /// Ordinate 1 of evaluation point in target CRS.
        /// </summary>
        private readonly Double ordinate1OfEvaluationPointInTarget;

        /// <summary>
        /// Ordinate 2 of evaluation point in target CRS.
        /// </summary>
        private readonly Double ordinate2OfEvaluationPointInTarget;

        /// <summary>
        /// Scale difference.
        /// </summary>
        private readonly Double scaleDifference;

        /// <summary>
        /// Rotation angle of source coordinate.
        /// </summary>
        private readonly Double rotationAngleOfSourceCoordinate;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double m;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarityTransformation" /> class.
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
        public SimilarityTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                        CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarityTransformation" /> class.
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
        public SimilarityTransformation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                        CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.SimilarityTransformation, parameters, source, target, areaOfUse)
        {
            this.ordinate1OfEvaluationPointInTarget = this.GetParameterValue(CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget);
            this.ordinate2OfEvaluationPointInTarget = this.GetParameterValue(CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget);
            this.scaleDifference = this.GetParameterValue(CoordinateOperationParameters.ScaleDifference);
            this.rotationAngleOfSourceCoordinate = this.GetParameterBaseValue(CoordinateOperationParameters.XAxisRotation);
            this.m = 1 + this.scaleDifference * 1E-6;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            Double x = this.ordinate1OfEvaluationPointInTarget + (coordinate.X * this.m * Math.Cos(this.rotationAngleOfSourceCoordinate)) +
                                                             (coordinate.Y * this.m * Math.Sin(this.rotationAngleOfSourceCoordinate));

            Double y = this.ordinate2OfEvaluationPointInTarget - (coordinate.X * this.m * Math.Sin(this.rotationAngleOfSourceCoordinate)) +
                                                             (coordinate.Y * this.m * Math.Cos(this.rotationAngleOfSourceCoordinate));

            return new Coordinate(x, y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = ((coordinate.X - this.ordinate1OfEvaluationPointInTarget) * Math.Cos(this.rotationAngleOfSourceCoordinate) -
                       (coordinate.Y - this.ordinate2OfEvaluationPointInTarget) * Math.Sin(this.rotationAngleOfSourceCoordinate)) / this.m;

            Double y = ((coordinate.X - this.ordinate1OfEvaluationPointInTarget) * Math.Sin(this.rotationAngleOfSourceCoordinate) +
                       (coordinate.Y - this.ordinate2OfEvaluationPointInTarget) * Math.Cos(this.rotationAngleOfSourceCoordinate)) / this.m;

            return new Coordinate(x, y);
        }
    }
}
