// <copyright file="CoordinateOperationMethod.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a coordinate operation method.
    /// </summary>
    public class CoordinateOperationMethod : IdentifiedObject
    {
        #region Private fields

        /// <summary>
        /// The array of parameters.
        /// </summary>
        private readonly CoordinateOperationParameter[] parameters;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="isReversible">Indicates whether the operation is reversible.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationMethod(String identifier, String name, Boolean isReversible, params CoordinateOperationParameter[] parameters)
            : this(identifier, name, null, null, isReversible, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="isReversible">Indicates whether the operation is reversible.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationMethod(String identifier, String name, String remarks, String[] aliases, Boolean isReversible, params CoordinateOperationParameter[] parameters)
            : base(identifier, name, remarks, aliases)
        {
            this.IsReversible = isReversible;
            this.parameters = parameters;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a value indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        public Boolean IsReversible { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The array containing the parameters of the method.</value>
        public IReadOnlyList<CoordinateOperationParameter> Parameters { get { return this.parameters; } }

        #endregion
    }
}
