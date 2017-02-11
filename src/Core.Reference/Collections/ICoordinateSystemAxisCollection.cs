// <copyright file="ICoordinateSystemAxisCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines queries for <see cref="CoordinateSystemAxis" /> collections.
    /// </summary>
    public interface ICoordinateSystemAxisCollection : IReferenceCollection<CoordinateSystemAxis>
    {
        /// <summary>
        /// Gets the item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The item with the specified authority and code.</returns>
        CoordinateSystemAxis this[String authority, Int32 code, AxisDirection direction, UnitOfMeasurement unit] { get; }

        /// <summary>
        /// Gets the item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The item with the specified identifier.</returns>
        CoordinateSystemAxis this[String identifier, AxisDirection direction, UnitOfMeasurement unit] { get; }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified identifier with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IEnumerable<CoordinateSystemAxis> WithIdentifier(String identifier, AxisDirection direction, UnitOfMeasurement unit);

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified name with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        IEnumerable<CoordinateSystemAxis> WithName(String name, AxisDirection direction, UnitOfMeasurement unit);

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression off the identifier.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified identifier expression with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IEnumerable<CoordinateSystemAxis> WithMatchingIdentifier(String identifier, AxisDirection direction, UnitOfMeasurement unit);

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression off the name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified name expression with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        IEnumerable<CoordinateSystemAxis> WithMatchingName(String name, AxisDirection direction, UnitOfMeasurement unit);
    }
}
