// <copyright file="IdentifiedObjectAttribute.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Indicates that the class implements an identified object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class IdentifiedObjectAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedObjectAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IdentifiedObjectAttribute(String identifier, String name)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            this.Identifier = identifier;
            this.Name = name ?? String.Empty;
        }

        /// <summary>
        /// Gets the identifier of the instance.
        /// </summary>
        /// <value>The identifier of the instance.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the name of the instance.
        /// </summary>
        /// <value>The name of the instance.</value>
        public String Name { get; private set; }
    }
}
