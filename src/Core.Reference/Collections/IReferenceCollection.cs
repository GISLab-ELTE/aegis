// <copyright file="IReferenceCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for reference collections.
    /// </summary>
    /// <typeparam name="ReferenceType">The type of the reference.</typeparam>
    public interface IReferenceCollection<out ReferenceType> : IEnumerable<ReferenceType>
        where ReferenceType : IdentifiedObject
    {
        /// <summary>
        /// Gets the item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The item with the specified authority and code; or <c>null</c> if no item exists with the specified authority and code.</returns>
        ReferenceType this[String authority, Int32 code] { get; }

        /// <summary>
        /// Gets the item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The item with the specified identifier; or <c>null</c> if no item exists with the specified identifier.</returns>
        ReferenceType this[String identifier] { get; }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IEnumerable<ReferenceType> WithIdentifier(String identifier);

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        IEnumerable<ReferenceType> WithName(String name);

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IEnumerable<ReferenceType> WithMatchingIdentifier(String identifier);

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        IEnumerable<ReferenceType> WithMatchingName(String name);
    }
}
