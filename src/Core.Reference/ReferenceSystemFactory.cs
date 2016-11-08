// <copyright file="ReferenceSystemFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Reference.Collections;
    using ELTE.AEGIS.Reference.Collections.Local;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a factory for producing <see cref="ReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This implementation of <see cref="IGeometryFactory" /> produces reference systems as defined by the OGC SRC standard.
    /// The factory uses an underlying <see cref="IReferenceCollectionContainer" /> instance for production.
    /// If not specified differently, the local collection of references is used to create the reference system.
    /// </remarks>
    public class ReferenceSystemFactory : Factory, IReferenceSystemFactory
    {
        /// <summary>
        /// The underrlying reference collection container. This field is read-only.
        /// </summary>
        private readonly IReferenceCollectionContainer collectionContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceSystemFactory" /> class.
        /// </summary>
        public ReferenceSystemFactory()
            : this(new LocalReferenceCollectionContainer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceSystemFactory" /> class.
        /// </summary>
        /// <param name="collectionContainer">The collection container.</param>
        /// <exception cref="System.ArgumentNullException">The collection container is null.</exception>
        public ReferenceSystemFactory(IReferenceCollectionContainer collectionContainer)
        {
            if (collectionContainer == null)
                throw new ArgumentNullException(nameof(collectionContainer), Messages.CollectionContainerIsNull);

            this.collectionContainer = collectionContainer;
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
            return this.collectionContainer.ReferenceSystems.WithIdentifier(identifier).WithName(name).FirstOrDefault();
        }

        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="identifier">The identifier of the reference system.</param>
        /// <returns>The produced reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IReferenceSystem CreateReferenceSystemFromIdentifier(String identifier)
        {
            return this.collectionContainer.ReferenceSystems.WithIdentifier(identifier).FirstOrDefault();
        }

        /// <summary>
        /// Creates a reference system.
        /// </summary>
        /// <param name="name">The name of the reference system.</param>
        /// <returns>The produced reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IReferenceSystem CreateReferenceSystemFromName(String name)
        {
            return this.collectionContainer.ReferenceSystems.WithName(name).FirstOrDefault();
        }
    }
}
