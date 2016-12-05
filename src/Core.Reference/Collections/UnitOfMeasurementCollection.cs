// <copyright file="UnitOfMeasurementCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="UnitOfMeasurement" /> instances.
    /// </summary>
    public class UnitOfMeasurementCollection : IReferenceCollection<UnitOfMeasurement>
    {
        /// <summary>
        /// The collection of all <see cref="UnitOfMeasurement" /> instances. This field is read-only.
        /// </summary>
        private readonly SortedDictionary<String, UnitOfMeasurement> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfMeasurementCollection" /> class.
        /// </summary>
        public UnitOfMeasurementCollection()
        {
            this.collection = new SortedDictionary<String, UnitOfMeasurement>();

            foreach (UnitOfMeasurement unit in typeof(UnitsOfMeasurement).GetTypeInfo().DeclaredProperties.Where(property => property.Name != "All").Select(property => property.GetValue(null, null) as UnitOfMeasurement))
            {
                this.collection.Add(unit.Identifier, unit);
            }
        }

        /// <summary>
        /// Gets the instance with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The instance with the specified authority and code.</returns>
        public UnitOfMeasurement this[String authority, Int32 code]
        {
            get
            {
                if (authority == null)
                    return null;

                return this[IdentifiedObject.GetIdentifier(authority, code)];
            }
        }

        /// <summary>
        /// Gets the instance with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The instance with the specified identifier.</returns>
        public UnitOfMeasurement this[String identifier]
        {
            get
            {
                if (identifier == null)
                    return null;

                if (!this.collection.ContainsKey(identifier))
                    return null;

                return this.collection[identifier];
            }
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<UnitOfMeasurement> WithIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            return this.collection.Values.WithIdentifier(identifier);
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<UnitOfMeasurement> WithName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            return this.collection.Values.WithName(name);
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<UnitOfMeasurement> WithMatchingIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            return this.collection.Values.WithMatchingIdentifier(identifier);
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<UnitOfMeasurement> WithMatchingName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            return this.collection.Values.WithMatchingName(name);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{UnitOfMeasurement}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<UnitOfMeasurement> GetEnumerator()
        {
            foreach (UnitOfMeasurement unit in this.collection.Values)
                yield return unit;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
