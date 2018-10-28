// <copyright file="IReferenceSystemDriver.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage
{
    using System;

    /// <summary>
    /// Defines a driver for reading and writing reference systems of features.
    /// </summary>
    public interface IReferenceSystemDriver : IDriver
    {
        /// <summary>
        /// Gets a value indicating whether a single reference system is specified for all features.
        /// </summary>
        /// <value><c>true</c> if all features have the same reference system; otherwise, <c>null</c>.</value>
        Boolean SingleReferenceSystem { get; }

        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void CreateReferenceSystem(String identifier, IReferenceSystem referenceSystem);

        /// <summary>
        /// Reads the reference system of all features.
        /// </summary>
        /// <returns>The reference system of the features if a single reference system is specified; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IReferenceSystem ReadReferenceSystem();

        /// <summary>
        /// Reads the reference system of a feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The reference system read by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IReferenceSystem ReadReferenceSystem(String identifier);

        /// <summary>
        /// Updates the reference system of all features.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateReferenceSystem(IReferenceSystem referenceSystem);

        /// <summary>
        /// Updates the reference system of a feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateReferenceSystem(String identifier, IReferenceSystem referenceSystem);

        /// <summary>
        /// Deletes the reference system of a feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns><c>true</c> if the reference system was successfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Boolean DeleteReferenceSystem(String identifier);
    }
}
