// <copyright file="ICoordinateProjectionCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines queries for <see cref="CoordinateProjection" /> collections.
    /// </summary>
    public interface ICoordinateProjectionCollection : IReferenceCollection<CoordinateProjection>
    {
        /// <summary>
        /// Gets the first item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The item with the specified authority, code and ellipsoid.</returns>
        CoordinateProjection this[String authority, Int32 code, Ellipsoid ellipsoid] { get; }

        /// <summary>
        /// Gets the first item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The item with the specified identifier and ellipsoid.</returns>
        CoordinateProjection this[String identifier, Ellipsoid ellipsoid] { get; }

        /// <summary>
        /// Gets the first item with the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The instance with the specified identifier.</returns>
        CoordinateProjection this[CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, AreaOfUse areaOfUse, Ellipsoid ellipsoid] { get; }

        /// <summary>
        /// Returns a collection with items matching the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The collection of items with the specified properties.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The area of use is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        IEnumerable<CoordinateProjection> WithProperties(CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, AreaOfUse areaOfUse, Ellipsoid ellipsoid);
    }
}
