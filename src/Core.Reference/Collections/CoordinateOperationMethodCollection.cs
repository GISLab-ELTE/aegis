// <copyright file="CoordinateOperationMethodCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AEGIS.Reference.Collections.Formula;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="CoordinateOperationMethod" /> instances.
    /// </summary>
    public class CoordinateOperationMethodCollection : IReferenceCollection<CoordinateOperationMethod>
    {
        /// <summary>
        /// The collection of all <see cref="CoordinateOperationMethod" /> instances. This field is read-only.
        /// </summary>
        private readonly SortedDictionary<String, CoordinateOperationMethod> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationMethodCollection" /> class.
        /// </summary>
        public CoordinateOperationMethodCollection()
        {
            this.collection = new SortedDictionary<String, CoordinateOperationMethod>();

            foreach (CoordinateOperationMethod unit in typeof(CoordinateOperationMethods).GetRuntimeProperties().Select(property => property.GetValue(null, null) as CoordinateOperationMethod))
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
        public CoordinateOperationMethod this[String authority, Int32 code]
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
        public CoordinateOperationMethod this[String identifier]
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
        public IEnumerable<CoordinateOperationMethod> WithIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            return this.collection.Values.WithIdentifier(identifier);
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateOperationMethod> WithName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.collection.Values.WithName(name);
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateOperationMethod> WithMatchingIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            return this.collection.Values.WithMatchingIdentifier(identifier);
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateOperationMethod> WithMatchingName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.collection.Values.WithMatchingName(name);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{CoordinateOperationMethod}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<CoordinateOperationMethod> GetEnumerator()
        {
            foreach (CoordinateOperationMethod parameter in this.collection.Values)
                yield return parameter;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (CoordinateOperationMethod parameter in this.collection.Values)
                yield return parameter;
        }
    }
}
