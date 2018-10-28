// <copyright file="IReferenceSystemFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines behavior for factories producing <see cref="IReferenceSystem" /> instances.
    /// </summary>
    public interface IReferenceSystemFactory : IFactory
    {
        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="identifier">The identifier of the reference system.</param>
        /// <param name="name">The name of the reference system.</param>
        /// <returns>The produced reference system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// </exception>
        IReferenceSystem CreateReferenceSystem(String identifier, String name);

        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="identifier">The identifier of the reference system.</param>
        /// <returns>The produced reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IReferenceSystem CreateReferenceSystemFromIdentifier(String identifier);

        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="name">The name of the reference system.</param>
        /// <returns>The produced reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        IReferenceSystem CreateReferenceSystemFromName(String name);
    }
}
