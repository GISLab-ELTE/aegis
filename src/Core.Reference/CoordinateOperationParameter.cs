// <copyright file="CoordinateOperationParameter.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    using System;

    /// <summary>
    /// Represents a coordinate operation parameter.
    /// </summary>
    public class CoordinateOperationParameter : IdentifiedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationParameter" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationParameter(String identifier, String name, String description)
            : this(identifier, name, null, null, description)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationParameter" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationParameter(String identifier, String name, String remarks, String[] aliases, String description)
            : base(identifier, name, remarks, aliases)
        {
            this.Description = description ?? String.Empty;
        }

        /// <summary>
        /// Gets the description of the parameter.
        /// </summary>
        /// <value>The description of the parameter.</value>
        public String Description { get; private set; }
    }
}
