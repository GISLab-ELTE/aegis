// <copyright file="IDriver.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a driver enabling reading and writing of geographic data.
    /// </summary>
    public interface IDriver : IDisposable
    {
        /// <summary>
        /// Gets the format of the driver.
        /// </summary>
        /// <value>The driver format.</value>
        DriverFormat Format { get; }

        /// <summary>
        /// Gets the parameters of the driver.
        /// </summary>
        /// <value>The read-only dictionary of driver parameters.</value>
        IReadOnlyDictionary<DriverParameter, Object> Parameters { get; }

        /// <summary>
        /// Gets the array of supported operations.
        /// </summary>
        /// <value>The array of supported operations.</value>
        DriverOperation[] SupportedOperations { get; }

        /// <summary>
        /// Gets the data handling of the driver.
        /// </summary>
        /// <value>The data handling of the driver.</value>
        DataHandling DataHandling { get; }

        /// <summary>
        /// Gets a value indicating whether the store contains the specified feature identifier.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns><c>true</c> if the store contains the specified feature identifier; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Boolean ContainsIdentifier(String identifier);

        /// <summary>
        /// Gets the identifier of the store.
        /// </summary>
        /// <returns>The identifier of the store.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        String GetIdentifier();

        /// <summary>
        /// Gets the feature identifiers from the store.
        /// </summary>
        /// <returns>The list of feature identifiers within the store.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IReadOnlyList<String> GetIdentifiers();

        /// <summary>
        /// Creates a new feature identifier in the store.
        /// </summary>
        /// <returns>The feature identifier created by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        String CreateIdentifier();
    }
}
