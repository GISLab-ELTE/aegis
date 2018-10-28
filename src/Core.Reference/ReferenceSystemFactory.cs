// <copyright file="ReferenceSystemFactory.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using AEGIS.Reference.Collections;
    using AEGIS.Reference.Collections.Local;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a factory for producing <see cref="ReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This implementation of <see cref="IGeometryFactory" /> produces reference systems as defined by the OGC SRC standard.
    /// The factory uses an underlying <see cref="IReferenceProvider" /> instance for production.
    /// If not specified differently, the local collection of references is used to create the reference system.
    /// </remarks>
    public class ReferenceSystemFactory : Factory, IReferenceSystemFactory
    {
        /// <summary>
        /// The underlying reference provider. This field is read-only.
        /// </summary>
        private readonly IReferenceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceSystemFactory" /> class.
        /// </summary>
        public ReferenceSystemFactory()
            : this(new LocalReferenceProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceSystemFactory" /> class.
        /// </summary>
        /// <param name="provider">The reference provider.</param>
        /// <exception cref="System.ArgumentNullException">The provider is null.</exception>
        public ReferenceSystemFactory(IReferenceProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

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
        public IReferenceSystem CreateReferenceSystem(String identifier, String name)
        {
            return this.provider.ReferenceSystems.WithIdentifier(identifier).WithName(name).FirstOrDefault();
        }

        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="identifier">The identifier of the reference system.</param>
        /// <returns>The produced reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IReferenceSystem CreateReferenceSystemFromIdentifier(String identifier)
        {
            return this.provider.ReferenceSystems.WithIdentifier(identifier).FirstOrDefault();
        }

        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="name">The name of the reference system.</param>
        /// <returns>The produced reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IReferenceSystem CreateReferenceSystemFromName(String name)
        {
            return this.provider.ReferenceSystems.WithName(name).FirstOrDefault();
        }
    }
}
