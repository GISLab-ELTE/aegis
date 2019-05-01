// <copyright file="GeneralPolynomialTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Reflection;
    using AEGIS.Numerics;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a general polynomial transformation.
    /// </summary>
    public abstract class GeneralPolynomialTransformation : CoordinateTransformation<Coordinate>
    {
        /// <summary>
        /// Represents a coefficient.
        /// </summary>
        private struct Coefficient : IEquatable<Coefficient>
        {
            /// <summary>
            /// The U degree.
            /// </summary>
            private readonly Int32 u;

            /// <summary>
            /// The V degree.
            /// </summary>
            private readonly Int32 v;

            /// <summary>
            /// Initializes a new instance of the <see cref="Coefficient" /> struct.
            /// </summary>
            /// <param name="u">The U degree.</param>
            /// <param name="v">The V degree.</param>
            public Coefficient(Int32 u, Int32 v)
            {
                this.u = u;
                this.v = v;
            }

            /// <summary>
            /// Gets the U degree.
            /// </summary>
            /// <value>The u degree.</value>
            public Int32 U { get { return this.u; } }

            /// <summary>
            /// Gets the U degree.
            /// </summary>
            /// <value>The u degree.</value>
            public Int32 V { get { return this.v; } }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
            public Boolean Equals(Coefficient other)
            {
                return this.u == other.u && this.v == other.v;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
            public override Int32 GetHashCode()
            {
                return (this.u.GetHashCode() << 3) ^ this.v.GetHashCode();
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
            public override Boolean Equals(Object obj)
            {
                return (obj is Coefficient) && this.Equals((Coefficient)obj);
            }

            /// <summary>
            /// Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> containing a fully qualified type name.</returns>
            public override String ToString()
            {
                return "U" + this.u + "V" + this.v;
            }
        }

        /// <summary>
        /// Ordinate 1 of evaluation point in source.
        /// </summary>
        private readonly Double ordinate1OfEvaluationPointInSource;

        /// <summary>
        /// Ordinate 2 of evaluation point in source.
        /// </summary>
        private readonly Double ordinate2OfEvaluationPointInSource;

        /// <summary>
        /// Ordinate 1 of evaluation point in target.
        /// </summary>
        private readonly Double ordinate1OfEvaluationPointInTarget;

        /// <summary>
        /// Ordinate 2 of evaluation point in target.
        /// </summary>
        private readonly Double ordinate2OfEvaluationPointInTarget;

        /// <summary>
        /// Scaling factor for source coordinate differences.
        /// </summary>
        private readonly Double scalingFactorForSourceCoordinateDifferences;

        /// <summary>
        /// Scaling factor for target coordinate differences.
        /// </summary>
        private readonly Double scalingFactorForTargetCoordinateDifferences;

        /// <summary>
        /// The collection of A parameters.
        /// </summary>
        private readonly Dictionary<Coefficient, Double> aParametersDictionary;

        /// <summary>
        /// The collection of B parameters.
        /// </summary>
        private readonly Dictionary<Coefficient, Double> bParametersDicitonary;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPolynomialTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="degree">The degree of the polynomial.</param>
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
        /// <exception cref="System.ArgumentException">No general polynomial transformation is available for the given degree.</exception>
        protected GeneralPolynomialTransformation(String identifier, String name, String remarks, String[] aliases, Int32 degree, IDictionary<CoordinateOperationParameter, Object> parameters,
                                                  CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, GetMethod(degree), parameters, source, target, areaOfUse)
        {
            this.ordinate1OfEvaluationPointInSource = this.GetParameterValue(CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource);
            this.ordinate2OfEvaluationPointInSource = this.GetParameterValue(CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource);
            this.ordinate1OfEvaluationPointInTarget = this.GetParameterValue(CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget);
            this.ordinate2OfEvaluationPointInTarget = this.GetParameterValue(CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget);
            this.scalingFactorForSourceCoordinateDifferences = this.GetParameterValue(CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences);
            this.scalingFactorForTargetCoordinateDifferences = this.GetParameterValue(CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences);

            this.Degree = degree;
            this.aParametersDictionary = new Dictionary<Coefficient, Double>();
            this.bParametersDicitonary = new Dictionary<Coefficient, Double>();

            foreach (CoordinateOperationParameter parameter in this.Method.Parameters)
            {
                if (parameter.Name[0] == 'A')
                {
                    if (parameter.Name[1] == '0')
                        this.aParametersDictionary.Add(new Coefficient(0, 0), this.GetParameterValue(parameter));
                    else
                        this.aParametersDictionary.Add(new Coefficient(Int32.Parse(parameter.Name[2].ToString()), Int32.Parse(parameter.Name[4].ToString())), this.GetParameterValue(parameter));
                }

                if (parameter.Name[0] == 'B')
                {
                    if (parameter.Name[1] == '0')
                        this.bParametersDicitonary.Add(new Coefficient(0, 0), this.GetParameterValue(parameter));
                    else
                        this.bParametersDicitonary.Add(new Coefficient(Int32.Parse(parameter.Name[2].ToString()), Int32.Parse(parameter.Name[4].ToString())), this.GetParameterValue(parameter));
                }
            }
        }

        /// <summary>
        /// Gets the degree of the polynomial.
        /// </summary>
        /// <value>The degree of the polynomial.</value>
        public Int32 Degree { get; }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            Double u = this.scalingFactorForSourceCoordinateDifferences * (coordinate.X - this.ordinate1OfEvaluationPointInSource);
            Double v = this.scalingFactorForSourceCoordinateDifferences * (coordinate.Y - this.ordinate2OfEvaluationPointInSource);
            Double dX = this.aParametersDictionary[new Coefficient(0, 0)];
            Double dY = this.bParametersDicitonary[new Coefficient(0, 0)];

            for (Int32 degree = 1; degree <= this.Degree; degree++)
            {
                for (Int32 vDegree = 0; vDegree <= degree; vDegree++)
                {
                    Int32 uDegree = degree - vDegree;
                    Coefficient key = new Coefficient(uDegree, vDegree);

                    dX += this.aParametersDictionary[key] * Math.Pow(u, uDegree) * Math.Pow(v, vDegree);
                    dY += this.bParametersDicitonary[key] * Math.Pow(u, uDegree) * Math.Pow(v, vDegree);
                }
            }

            return new Coordinate(coordinate.X - this.ordinate1OfEvaluationPointInSource + this.ordinate1OfEvaluationPointInTarget + dX / this.scalingFactorForTargetCoordinateDifferences,
                                  coordinate.Y - this.ordinate2OfEvaluationPointInSource + this.ordinate2OfEvaluationPointInTarget + dY / this.scalingFactorForTargetCoordinateDifferences);
        }

        /// <summary>
        /// Gets the coordinate operation method.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <returns>The coordinate operation method for <paramref name="degree" />.</returns>
        /// <exception cref="System.ArgumentException">No general polynomial transformation is available for the given degree.</exception>
        private static CoordinateOperationMethod GetMethod(Int32 degree)
        {
            FieldInfo fieldInfo = typeof(CoordinateOperationMethods).GetField("GeneralPolynomial" + degree);
            if (fieldInfo == null)
                throw new ArgumentException(ReferenceMessages.PolynomialTransformationDegreeIsInvalid, nameof(degree));

            return fieldInfo.GetValue(null) as CoordinateOperationMethod;
        }
    }
}
