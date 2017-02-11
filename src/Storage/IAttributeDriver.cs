// <copyright file="IAttributeDriver.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a driver for reading and writing attributes of features.
    /// </summary>
    public interface IAttributeDriver : IDriver
    {
        /// <summary>
        /// Gets the attribute factory.
        /// </summary>
        /// <value>The attribute factory used by the driver.</value>
        IGeometryFactory Factory { get; }

        /// <summary>
        /// Creates an attribute collection for the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The attribute collection created by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IAttributeCollection CreateAttributes(String identifier);

        /// <summary>
        /// Determines whether the store contains the specified attribute.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the store contains the specified attribute; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Boolean ContainsAttribute(String identifier, String key);

        /// <summary>
        /// Reads the number of attributes of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The number of attributes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Int32 ReadAttributeCount(String identifier);

        /// <summary>
        /// Reads the attribute keys of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The collection of attribute keys.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IEnumerable<String> ReadAttributeKeys(String identifier);

        /// <summary>
        /// Reads the attributes of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The attribute collection.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IAttributeCollection ReadAttributes(String identifier);

        /// <summary>
        /// Reads an attribute of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="key">The key of the attribute.</param>
        /// <returns>The value of the specified attribute.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Object ReadAttribute(String identifier, String key);

        /// <summary>
        /// Updates the attributes of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="attributes">The collection of attribute.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateAttributes(String identifier, IAttributeCollection attributes);

        /// <summary>
        /// Updates an attribute of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="key">The key of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateAttribute(String identifier, String key, Object value);

        /// <summary>
        /// Deletes all attributes of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteAttributes(String identifier);

        /// <summary>
        /// Deletes an attribute of the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="key">The key of the attribute.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteAttribute(String identifier, String key);
    }
}
