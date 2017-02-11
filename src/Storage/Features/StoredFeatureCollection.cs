// <copyright file="StoredFeatureCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ELTE.AEGIS.Collections.Resources;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a feature located in a store.
    /// </summary>
    public class StoredFeatureCollection : IStoredFeatureCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredFeatureCollection" /> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="identifier">The identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredFeatureCollection(IFeatureDriver driver, String identifier)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver), ELTE.AEGIS.Storage.Resources.StorageMessages.DriverIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            this.Factory = new StoredFeatureFactory(driver);
            this.Identifier = identifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredFeatureCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredFeatureCollection(IStoredFeatureFactory factory, String identifier)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            this.Factory = factory;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        /// <returns>The number of elements contained in the collection.</returns>
        public Int32 Count { get { return this.Driver.ReadFeatureCount(this.Identifier); } }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <value><c>true</c> if the collection is read-only; otherwise, <c>false</c>.</value>
        public Boolean IsReadOnly { get { return this.Driver.SupportedOperations.Length == 1 && this.Driver.SupportedOperations[0] == DriverOperation.Read; } }

        /// <summary>
        /// Gets the attribute collection of the feature.
        /// </summary>
        /// <value>The collection of attribute.</value>
        public IAttributeCollection Attributes { get { return this.Driver.AttributeDriver.ReadAttributes(this.Identifier); } }

        /// <summary>
        /// Gets the geometry of the feature.
        /// </summary>
        /// <value>The geometry of the feature.</value>
        public IGeometry Geometry { get { return this.Driver.GeometryDriver.ReadGeometry(this.Identifier); } }

        /// <summary>
        /// Gets the unique identifier of the feature.
        /// </summary>
        /// <value>The identifier of the feature.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the factory of the feature.
        /// </summary>
        /// <value>The factory implementation the feature was constructed by.</value>
        IFeatureFactory IFeature.Factory { get { return this.Factory; } }

        /// <summary>
        /// Gets the feature identifiers within the collection.
        /// </summary>
        /// <value>The read-only list of feature identifiers within the collection.</value>
        public IEnumerable<String> Identifiers { get { return this.Driver.GetIdentifiers(this.Identifier); } }

        /// <summary>
        /// Gets the driver of the feature.
        /// </summary>
        /// <value>The driver the feature.</value>
        public IFeatureDriver Driver { get { return this.Factory.Driver; } }

        /// <summary>
        /// Gets the factory of the feature.
        /// </summary>
        /// <value>The factory implementation the feature was constructed by.</value>
        public IStoredFeatureFactory Factory { get; private set; }

        /// <summary>
        /// Gets the feature with the specified identifier.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The feature at the specified index if the feature exists; otherwise, <c>null</c>.</returns>
        public IFeature this[String identifier]
        {
            get
            {
                if (identifier == null)
                    return null;

                if (!this.Driver.ContainsIdentifier(identifier))
                    return null;

                return this.Driver.ReadFeature(identifier);
            }
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The object to add to the collection.</param>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        /// <exception cref="System.ArgumentException">An item with the same identifier already exists in the collection.</exception>
        public void Add(IFeature item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), CollectionMessages.ItemIsNull);
            if (this.Driver.ContainsIdentifier(item.Identifier))
                throw new ArgumentException(CoreMessages.ItemIdentifierExists, nameof(item));

            this.Driver.UpdateFeature(item.Identifier, item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            foreach (String identifier in this.Driver.GetIdentifiers(this.Identifier))
                this.Driver.DeleteFeature(identifier);
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The feature to locate in the collection.</param>
        /// <returns><c>true</c> if <paramref name="item" /> is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        public Boolean Contains(IFeature item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), CollectionMessages.ItemIsNull);

            return this.Driver.ContainsIdentifier(item.Identifier);
        }

        /// <summary>
        /// Copies the geometries of the geometry list to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from geometry list.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The index is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
        public void CopyTo(IFeature[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), CollectionMessages.ArrayIsNull);
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), CollectionMessages.IndexIsLessThan0);
            if (arrayIndex + this.Count > array.Length)
                throw new ArgumentException(CollectionMessages.ArrayIndexIsGreaterThanSpace, nameof(array));

            foreach (String identifier in this.Driver.GetIdentifiers(this.Identifier))
            {
                array[arrayIndex] = this.Driver.ReadFeature(identifier);
                arrayIndex++;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        public Boolean Remove(IFeature item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), CollectionMessages.ItemIsNull);

            if (!this.Driver.ContainsIdentifier(item.Identifier))
                return false;

            this.Driver.DeleteFeature(item.Identifier);
            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{IFeature}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<IFeature> GetEnumerator()
        {
            foreach (String identifier in this.Driver.GetIdentifiers(this.Identifier))
                yield return this.Driver.ReadFeature(identifier);
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
