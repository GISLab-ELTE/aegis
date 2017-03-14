// <copyright file="LocalReferenceSystemCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of <see cref="ReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalReferenceSystemCollection : IReferenceCollection<ReferenceSystem>
    {
        /// <summary>
        /// The compound reference system collection. This field is read-only.
        /// </summary>
        private readonly IReferenceCollection<CompoundReferenceSystem> compoundCollection;

        /// <summary>
        /// The coordinate reference system collection. This field is read-only.
        /// </summary>
        private readonly IReferenceCollection<CoordinateReferenceSystem> coordinateCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalReferenceSystemCollection" /> class.
        /// </summary>
        /// <param name="compoundCollection">The compound reference system collection.</param>
        /// <param name="coordinateCollection">The coordinate reference system collection.</param>
        public LocalReferenceSystemCollection(IReferenceCollection<CompoundReferenceSystem> compoundCollection, IReferenceCollection<CoordinateReferenceSystem> coordinateCollection)
        {
            this.compoundCollection = compoundCollection;
            this.coordinateCollection = coordinateCollection;
        }

        /// <summary>
        /// Gets the item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The item with the specified authority and code; or <c>null</c> if no item exists with the specified authority and code.</returns>
        public ReferenceSystem this[String authority, Int32 code]
        {
            get
            {
                ReferenceSystem system = this.compoundCollection[authority, code];
                if (system != null)
                    return system;

                return this.coordinateCollection[authority, code];
            }
        }

        /// <summary>
        /// Gets the item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The item with the specified identifier; or <c>null</c> if no item exists with the specified identifier.</returns>
        public ReferenceSystem this[String identifier]
        {
            get
            {
                ReferenceSystem system = this.compoundCollection[identifier];
                if (system != null)
                    return system;

                return this.coordinateCollection[identifier];
            }
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<ReferenceSystem> WithIdentifier(String identifier)
        {
            foreach (CompoundReferenceSystem item in this.compoundCollection.WithIdentifier(identifier))
                yield return item;
            foreach (CoordinateReferenceSystem item in this.coordinateCollection.WithIdentifier(identifier))
                yield return item;
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<ReferenceSystem> WithName(String name)
        {
            foreach (CompoundReferenceSystem item in this.compoundCollection.WithName(name))
                yield return item;
            foreach (CoordinateReferenceSystem item in this.coordinateCollection.WithName(name))
                yield return item;
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<ReferenceSystem> WithMatchingIdentifier(String identifier)
        {
            foreach (CompoundReferenceSystem item in this.compoundCollection.WithMatchingIdentifier(identifier))
                yield return item;
            foreach (CoordinateReferenceSystem item in this.coordinateCollection.WithMatchingIdentifier(identifier))
                yield return item;
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<ReferenceSystem> WithMatchingName(String name)
        {
            foreach (CompoundReferenceSystem item in this.compoundCollection.WithMatchingName(name))
                yield return item;
            foreach (CoordinateReferenceSystem item in this.coordinateCollection.WithMatchingName(name))
                yield return item;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ReferenceSystem> GetEnumerator()
        {
            foreach (CompoundReferenceSystem item in this.compoundCollection)
                yield return item;
            foreach (CoordinateReferenceSystem item in this.coordinateCollection)
                yield return item;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
