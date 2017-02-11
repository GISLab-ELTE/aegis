// <copyright file="HelmertTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Helmert Transformation (with 7 parameters).
    /// </summary>
    public abstract class HelmertTransformation : CoordinateTransformation<Coordinate>
    {
        /// <summary>
        /// X axis translation.
        /// </summary>
        protected Double xAxisTranslation;

        /// <summary>
        /// Y axis translation.
        /// </summary>
        protected Double yAxisTranslation;

        /// <summary>
        /// Z axis translation.
        /// </summary>
        protected Double zAxisTranslation;

        /// <summary>
        /// X axis rotation.
        /// </summary>
        protected Double xAxisRotation;

        /// <summary>
        /// Y axis rotation.
        /// </summary>
        protected Double yAxisRotation;

        /// <summary>
        /// Z axis rotation.
        /// </summary>
        protected Double zAxisRotation;

        /// <summary>
        /// Scale difference.
        /// </summary>
        protected Double scaleDifference;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected Double m;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected Double[,] inverseParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelmertTransformation" /> class.
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
        protected HelmertTransformation(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, source, target, areaOfUse)
        {
            this.xAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.XAxisTranslation);
            this.yAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.YAxisTranslation);
            this.zAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.ZAxisTranslation);
            this.xAxisRotation = this.GetParameterValue(CoordinateOperationParameters.XAxisRotation);
            this.yAxisRotation = this.GetParameterValue(CoordinateOperationParameters.YAxisRotation);
            this.zAxisRotation = this.GetParameterValue(CoordinateOperationParameters.ZAxisRotation);
            this.scaleDifference = this.GetParameterValue(CoordinateOperationParameters.ScaleDifference);

            this.m = 1 + this.scaleDifference * 1E-6;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            Double x = this.m * (coordinate.X - this.zAxisRotation * coordinate.Y + this.yAxisRotation * coordinate.Z) + this.xAxisTranslation;
            Double y = this.m * (this.zAxisRotation * coordinate.X + coordinate.Y - this.xAxisRotation * coordinate.Z) + this.yAxisTranslation;
            Double z = this.m * (-this.yAxisRotation * coordinate.X + this.xAxisRotation * coordinate.Y + coordinate.Z) + this.zAxisTranslation;

            return new Coordinate(x, y, z);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = 1 / this.m * (this.inverseParams[0, 0] * (coordinate.X - this.xAxisTranslation) + this.inverseParams[0, 1] * (coordinate.Y - this.yAxisTranslation) + this.inverseParams[0, 2] * (coordinate.Z - this.zAxisTranslation));
            Double y = 1 / this.m * (this.inverseParams[1, 0] * (coordinate.X - this.xAxisTranslation) + this.inverseParams[1, 1] * (coordinate.Y - this.yAxisTranslation) + this.inverseParams[1, 2] * (coordinate.Z - this.zAxisTranslation));
            Double z = 1 / this.m * (this.inverseParams[2, 0] * (coordinate.X - this.xAxisTranslation) + this.inverseParams[2, 1] * (coordinate.Y - this.yAxisTranslation) + this.inverseParams[2, 2] * (coordinate.Z - this.zAxisTranslation));

            return new Coordinate(x, y, z);
        }
    }
}
