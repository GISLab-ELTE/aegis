// <copyright file="CoordinateOperation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Collections;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a generic coordinate operation between different coordinate types.
    /// </summary>
    /// <typeparam name="SourceType">The type of the source.</typeparam>
    /// <typeparam name="ResultType">The type of the result.</typeparam>
    /// <remarks>
    /// A mathematical operation on coordinates that transforms or converts coordinates to another coordinate reference system.
    /// Many but not all coordinate operations (from CRS A to CRS B) also uniquely define the inverse coordinate operation (from CRS B to CRS A).
    /// In some cases, the coordinate operation method algorithm for the inverse coordinate operation is the same as for the forward algorithm,
    /// but the signs of some coordinate operation parameter values have to be reversed. In other cases, different algorithms are required for
    /// the forward and inverse coordinate operations, but the same coordinate operation parameter values are used.
    /// If (some) entirely different parameter values are needed, a different coordinate operation shall be defined.
    /// </remarks>
    public abstract class CoordinateOperation<SourceType, ResultType> : IdentifiedObject
    {
        /// <summary>
        /// The dictionary of parameters.
        /// </summary>
        private readonly IDictionary<CoordinateOperationParameter, Object> parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperation{SourceType, ResultType}" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// </exception>
        protected CoordinateOperation(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, remarks, aliases)
        {
            this.Method = method ?? throw new ArgumentNullException(nameof(method));

            if (parameters != null)
            {
                this.parameters = new Dictionary<CoordinateOperationParameter, Object>(method.Parameters.Count);

                // only keep the parameters which apply according to the method
                foreach (CoordinateOperationParameter parameter in parameters.Keys)
                {
                    if (method.Parameters.Contains(parameter))
                        this.parameters.Add(parameter, parameters[parameter]);
                }
            }
            else
            {
                this.parameters = new Dictionary<CoordinateOperationParameter, Object>();
            }
        }

        /// <summary>
        /// Gets the method associated with the operation.
        /// </summary>
        /// <value>The associated coordinate operation method.</value>
        public CoordinateOperationMethod Method { get; private set; }

        /// <summary>
        /// Gets the parameters of the operation.
        /// </summary>
        /// <value>The parameters of the operation stored as key/value pairs.</value>
        public IReadOnlyDictionary<CoordinateOperationParameter, Object> Parameters { get { return this.parameters.AsReadOnly(); } }

        /// <summary>
        /// Gets a value indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        public Boolean IsReversible { get { return this.Method.IsReversible; } }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public ResultType Forward(SourceType coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            return this.ComputeForward(coordinate);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">No coordinates are specified.</exception>
        public ResultType[] Forward(params SourceType[] coordinates)
        {
            if (coordinates == null || coordinates.Length == 0)
                throw new ArgumentNullException(nameof(coordinates), ReferenceMessages.CoordinatesAreNull);

            ResultType[] result = new ResultType[coordinates.Length];
            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index] = this.ComputeForward(coordinates[index]);
            }

            return result;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">No coordinates are specified.</exception>
        public ResultType[] Forward(IEnumerable<SourceType> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates), ReferenceMessages.CoordinatesAreNull);

            ResultType[] result = new ResultType[coordinates.Count()];
            Int32 index = 0;
            foreach (SourceType location in coordinates)
            {
                result[index] = this.ComputeForward(location);
                index++;
            }

            return result;
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        /// <exception cref="System.NotSupportedException">Coordinate operation is not reversible.</exception>
        public SourceType Reverse(ResultType coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            if (!this.Method.IsReversible)
                throw new NotSupportedException(ReferenceMessages.OperationNotReversible);

            return this.ComputeReverse(coordinate);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.NotSupportedException">Coordinate operation is not reversible.</exception>
        /// <exception cref="System.ArgumentNullException">No coordinates are specified.</exception>
        public SourceType[] Reverse(params ResultType[] coordinates)
        {
            if (!this.Method.IsReversible)
                throw new NotSupportedException(ReferenceMessages.OperationNotReversible);

            if (coordinates == null || coordinates.Length == 0)
                throw new ArgumentNullException(nameof(coordinates), ReferenceMessages.CoordinatesAreNull);

            SourceType[] result = new SourceType[coordinates.Length];
            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index] = this.ComputeReverse(coordinates[index]);
            }

            return result;
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.NotSupportedException">Coordinate operation is not reversible.</exception>
        /// <exception cref="System.ArgumentNullException">No coordinates are specified.</exception>
        public SourceType[] Reverse(IEnumerable<ResultType> coordinates)
        {
            if (!this.Method.IsReversible)
                throw new NotSupportedException(ReferenceMessages.OperationNotReversible);

            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates), ReferenceMessages.CoordinatesAreNull);

            SourceType[] result = new SourceType[coordinates.Count()];
            Int32 index = 0;
            foreach (ResultType coordinate in coordinates)
            {
                result[index] = this.ComputeReverse(coordinate);
                index++;
            }

            return result;
        }

        /// <summary>
        /// Sets the specified parameter value.
        /// </summary>
        /// <param name="parameter">The coordinate operation parameter.</param>
        /// <param name="value">The value.</param>
        protected void SetParameterValue(CoordinateOperationParameter parameter, Object value)
        {
            if (parameter == null)
                return;

            if (value == null && this.parameters.ContainsKey(parameter))
                this.parameters.Remove(parameter);

            this.parameters[parameter] = value;
        }

        /// <summary>
        /// Returns the base value of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The base value of the parameter.</returns>
        protected Double GetParameterBaseValue(CoordinateOperationParameter parameter)
        {
            if (!this.parameters.ContainsKey(parameter))
                return 0;

            if (this.parameters[parameter] is Angle)
                return ((Angle)this.parameters[parameter]).BaseValue;

            if (this.parameters[parameter] is Length)
                return ((Length)this.parameters[parameter]).BaseValue;

            if (this.parameters[parameter] is IConvertible)
                return Convert.ToDouble(this.parameters[parameter]);

            return Double.NaN;
        }

        /// <summary>
        /// Returns the value of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The value of the parameter.</returns>
        protected Double GetParameterValue(CoordinateOperationParameter parameter)
        {
            if (!this.parameters.ContainsKey(parameter))
                return 0;

            if (this.parameters[parameter] is Angle)
                return ((Angle)this.parameters[parameter]).Value;

            if (this.parameters[parameter] is Length)
                return ((Length)this.parameters[parameter]).Value;

            if (this.parameters[parameter] is IConvertible)
                return Convert.ToDouble(this.parameters[parameter]);

            return Double.NaN;
        }

        /// <summary>
        /// Returns the path value of the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The path value of the parameter.</returns>
        protected String GetParameterPathValue(CoordinateOperationParameter parameter)
        {
            if (!this.parameters.ContainsKey(parameter))
                return null;

            if (this.parameters[parameter] is String)
                return this.parameters[parameter] as String;

            return null;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected abstract ResultType ComputeForward(SourceType coordinate);

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected abstract SourceType ComputeReverse(ResultType coordinate);
    }
}
