// <copyright file="MolodenskyTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a Molodensky Transformation.
    /// </summary>
    [IdentifiedObject("EPSG::9604", "Molodensky")]
    public class MolodenskyTransformation : CoordinateTransformation<GeoCoordinate>
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
        /// Semi-major axis length difference.
        /// </summary>
        private readonly Double semiMajorAxisLengthDifference;

        /// <summary>
        /// Flattening difference.
        /// </summary>
        private readonly Double flatteningDifference;

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid model of Earth.</value>
        public Ellipsoid Ellipsoid { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MolodenskyTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected MolodenskyTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters,
                                           CoordinateReferenceSystem source, CoordinateReferenceSystem target, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, source, target, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MolodenskyTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected MolodenskyTransformation(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters,
                                           CoordinateReferenceSystem source, CoordinateReferenceSystem target, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.MolodenskyTransformation, parameters, source, target, areaOfUse)
        {
            this.Ellipsoid = ellipsoid ?? throw new ArgumentNullException(nameof(ellipsoid));
            this.xAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.XAxisTranslation);
            this.yAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.YAxisTranslation);
            this.zAxisTranslation = this.GetParameterValue(CoordinateOperationParameters.ZAxisTranslation);
            this.semiMajorAxisLengthDifference = this.GetParameterValue(CoordinateOperationParameters.SemiMajorAxisLengthDifference);
            this.flatteningDifference = this.GetParameterValue(CoordinateOperationParameters.FlatteningDifference);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double d = this.Ellipsoid.SemiMajorAxis.BaseValue * this.flatteningDifference + this.Ellipsoid.Flattening * this.semiMajorAxisLengthDifference;
            Double deltaPhi = (-this.xAxisTranslation * Math.Sin(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue) - this.yAxisTranslation * Math.Sin(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue) + this.zAxisTranslation * Math.Cos(coordinate.Latitude.BaseValue) + d) / this.Ellipsoid.RadiusOfMeridianCurvature(coordinate.Latitude.BaseValue);
            Double deltaLambda = (-this.xAxisTranslation * Math.Sin(coordinate.Longitude.BaseValue) + this.yAxisTranslation * Math.Cos(coordinate.Longitude.BaseValue)) / this.Ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude.BaseValue) / Math.Cos(coordinate.Latitude.BaseValue);
            Double deltaH = this.xAxisTranslation * Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue) + this.yAxisTranslation * Math.Cos(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue) + d * Calculator.Sin2(coordinate.Latitude.BaseValue) - this.semiMajorAxisLengthDifference;

            return new GeoCoordinate(coordinate.Latitude.BaseValue + deltaPhi, coordinate.Longitude.BaseValue + deltaLambda, coordinate.Height.BaseValue + deltaH);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(GeoCoordinate coordinate)
        {
            return GeoCoordinate.Undefined;
        }
    }
}
