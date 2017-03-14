// <copyright file="CompoundCoordinateOperationMethod.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a compound coordinate operation method.
    /// </summary>
    public class CompoundCoordinateOperationMethod : CoordinateOperationMethod
    {
        /// <summary>
        /// The array of methods.
        /// </summary>
        private readonly CoordinateOperationMethod[] methods;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundCoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="isReversible">The is reversible.</param>
        /// <param name="methods">The internal methods of the operation.</param>
        /// <exception cref="System.ArgumentNullException">No methods are specified.</exception>
        public CompoundCoordinateOperationMethod(String identifier, String name, Boolean isReversible, params CoordinateOperationMethod[] methods)
            : base(identifier, name, isReversible, null)
        {
            if (methods == null || methods.Length == 0)
                throw new ArgumentNullException(nameof(methods), ReferenceMessages.NoMethodsSpecified);

            this.methods = methods;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundCoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="isReversible">The is reversible.</param>
        /// <param name="methods">The internal methods of the operation.</param>
        /// <exception cref="System.ArgumentNullException">No methods are specified.</exception>
        public CompoundCoordinateOperationMethod(String identifier, String name, String remarks, String[] aliases, Boolean isReversible, params CoordinateOperationMethod[] methods)
            : base(identifier, name, remarks, aliases, isReversible, null)
        {
            if (methods == null || methods.Length == 0)
                throw new ArgumentNullException(nameof(methods), ReferenceMessages.NoMethodsSpecified);

            this.methods = methods;
        }

        /// <summary>
        /// Gets the internal methods of the compound coordinate operation.
        /// </summary>
        /// <value>The read-only list of the internal methods.</value>
        public IReadOnlyList<CoordinateOperationMethod> Methods { get { return this.methods; } }
    }
}
