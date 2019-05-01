// <copyright file="MolodenskyBadekasTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Molodensky-Badekas Transformation.
    /// </summary>
    [IdentifiedObject("EPSG::1034", "Molodensky-Badekas (geocentric domain)")]
    public class MolodenskyBadekasTransformation : CoordinateTransformation<Coordinate>
    {
        /// <summary>
        /// X axis translation.
        /// </summary>
        private readonly Double xAxisTranslation;

        /// <summary>
        /// Y axis translation.
        /// </summary>
        private readonly Double yAxisTranslation;

        /// <summary>
        /// Z axis translation.
        /// </summary>
        private readonly Double zAxisTranslation;

        /// <summary>
        /// X axis rotation.
        /// </summary>
        private readonly Double xAxisRotation;

        /// <summary>
        /// Y axis rotation.
        /// </summary>
        private readonly Double yAxisRotation;

        /// <summary>
        /// Z axis rotation.
        /// </summary>
        private readonly Double zAxisRotation;

        /// <summary>
        /// Scale difference.
        /// </summary>
        private readonly Double scaleDifference;

        /// <summary>
        /// Ordinate 1 of evaluation point.
        /// </summary>
        private readonly Double ordinate1OfEvaluationPoint;

        /// <summary>
        /// Ordinate 2 of evaluation point.
        /// </summary>
        private readonly Double ordinate2OfEvaluationPoint;

        /// <summary>
        /// Ordinate 3 of evaluation point.
        /// </summary>
        private readonly Double ordinate3OfEvaluationPoint;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double m;

        /// <summary>
        /// Initializes a new instance of the <see cref="MolodenskyBadekasTransformation" /> class.
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
        protected MolodenskyBadekasTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                                  GeocentricCoordinateReferenceSystem source, GeocentricCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MolodenskyBadekasTransformation" /> class.
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
        protected MolodenskyBadekasTransformation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                                  GeocentricCoordinateReferenceSystem source, GeocentricCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.MolodenskyBadekasTransformation, parameters, source, target, areaOfUse)
        {
            this.xAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.XAxisTranslation);
            this.yAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.YAxisTranslation);
            this.zAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.ZAxisTranslation);
            this.xAxisRotation = this.GetParameterValue(CoordinateOperationParameters.XAxisRotation);
            this.yAxisRotation = this.GetParameterValue(CoordinateOperationParameters.YAxisRotation);
            this.zAxisRotation = this.GetParameterValue(CoordinateOperationParameters.ZAxisRotation);
            this.scaleDifference = this.GetParameterValue(CoordinateOperationParameters.ScaleDifference);
            this.ordinate1OfEvaluationPoint = this.GetParameterValue(CoordinateOperationParameters.Ordinate1OfEvaluationPoint);
            this.ordinate2OfEvaluationPoint = this.GetParameterValue(CoordinateOperationParameters.Ordinate2OfEvaluationPoint);
            this.ordinate3OfEvaluationPoint = this.GetParameterValue(CoordinateOperationParameters.Ordinate3OfEvaluationPoint);

            this.m = 1 + this.scaleDifference * 1E-6;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            Double x = this.m * ((coordinate.X - this.ordinate1OfEvaluationPoint) - this.zAxisRotation * (coordinate.Y - this.ordinate2OfEvaluationPoint) + this.yAxisRotation * (coordinate.Z - this.ordinate3OfEvaluationPoint)) + this.ordinate1OfEvaluationPoint + this.xAxisTranslation;
            Double y = this.m * (this.zAxisRotation * (coordinate.X - this.ordinate1OfEvaluationPoint) + (coordinate.Y - this.ordinate2OfEvaluationPoint) - this.xAxisRotation * (coordinate.Z - this.ordinate3OfEvaluationPoint)) + this.ordinate2OfEvaluationPoint + this.yAxisTranslation;
            Double z = this.m * (-this.yAxisRotation * (coordinate.X - this.ordinate1OfEvaluationPoint) + this.xAxisRotation * (coordinate.Y - this.ordinate2OfEvaluationPoint) + (coordinate.Z - this.ordinate3OfEvaluationPoint)) + this.ordinate3OfEvaluationPoint + this.zAxisTranslation;

            return new Coordinate(x, y, z);
        }
    }
}
