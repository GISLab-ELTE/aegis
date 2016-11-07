// <copyright file="FeatureCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Features
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a collection of features.
    /// </summary>
    public class FeatureCollection : IFeatureCollection
    {
        #region Private fields

        /// <summary>
        /// The list of features.
        /// </summary>
        private Dictionary<String, IFeature> items;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="attributes">The attribute collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public FeatureCollection(FeatureFactory factory, String identifier, IAttributeCollection attributes)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), Messages.FactoryIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);

            this.Factory = factory;
            this.Identifier = identifier;
            this.Attributes = (attributes == null) ? null : attributes.Factory.Equals(this.Factory.AttributeCollectionFactory) ? attributes : this.Factory.AttributeCollectionFactory.CreateCollection(attributes);
            this.items = new Dictionary<String, IFeature>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="attributes">The attribute collection.</param>
        /// <param name="collection">The collection of features.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// or
        /// The collection is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The collection contains one or more duplicate identifiers.</exception>
        public FeatureCollection(FeatureFactory factory, String identifier, IAttributeCollection attributes, IEnumerable<IFeature> collection)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), Messages.FactoryIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), Messages.CollectionIsNull);

            this.Factory = factory;
            this.Identifier = identifier;
            this.Attributes = (attributes == null) ? null : attributes.Factory.Equals(this.Factory.AttributeCollectionFactory) ? attributes : this.Factory.AttributeCollectionFactory.CreateCollection(attributes);

            this.items = new Dictionary<String, IFeature>();
            foreach (IFeature feature in collection)
            {
                if (feature == null)
                    continue;

                if (this.items.ContainsKey(feature.Identifier))
                    throw new ArgumentException(nameof(collection), Messages.CollectionContainsDuplicateIdentifiers);

               this.items.Add(feature.Identifier, feature.Factory.Equals(this.Factory) ? feature : this.Factory.CreateFeature(feature));
            }
        }

        #endregion

        #region IFeature properties

        /// <summary>
        /// Gets the attribute collection of the feature.
        /// </summary>
        /// <value>The collection of attribute.</value>
        public IAttributeCollection Attributes { get; private set; }

        /// <summary>
        /// Gets the factory of the feature collection.
        /// </summary>
        /// <value>The factory the feature collection was constructed by.</value>
        public IFeatureFactory Factory { get; private set; }

        /// <summary>
        /// Gets the geometry of the feature.
        /// </summary>
        /// <value>The geometry of the feature.</value>
        public IGeometry Geometry { get { return this.Factory.GeometryFactory.CreateGeometryCollection(this.items.Values.Select(feature => feature.Geometry)); } }

        /// <summary>
        /// Gets the unique identifier of the feature collection.
        /// </summary>
        /// <value>The identifier of the feature collection.</value>
        public String Identifier { get; private set; }

        #endregion

        #region IFeatureCollection properties

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        /// <returns>The number of elements contained in the collection.</returns>
        public Int32 Count { get { return this.items.Count; } }

        /// <summary>
        /// Gets the feature identifiers within the collection.
        /// </summary>
        /// <value>The read-only list of feature identifiers within the collection.</value>
        public IEnumerable<String> Identifiers { get { return this.items.Keys; } }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <returns><c>true</c> if the collection is read-only; otherwise, <c>false</c>.</returns>
        public Boolean IsReadOnly { get { return false; } }

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

                IFeature feature = null;
                if (!this.items.TryGetValue(identifier, out feature))
                    return null;

                return feature;
            }
        }

        #endregion

        #region IFeatureCollection methods

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The object to add to the collection.</param>
        /// <exception cref="System.ArgumentNullException">The item is null.</exception>
        /// <exception cref="System.ArgumentException">An item with the same identifier already exists in the collection.</exception>
        public void Add(IFeature item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), Messages.ItemIsNull);
            if (this.items.ContainsKey(item.Identifier))
                throw new ArgumentException(Messages.ItemIdentifierExists, nameof(item));

            this.items.Add(item.Identifier, item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
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
                throw new ArgumentNullException(nameof(item), Messages.ItemIsNull);

            return this.items.ContainsKey(item.Identifier) && this.items[item.Identifier].Equals(item);
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
                throw new ArgumentNullException(nameof(array), Messages.ArrayIsNull);
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), Messages.IndexIsLessThan0);
            if (arrayIndex + this.items.Count > array.Length)
                throw new ArgumentException(Messages.ArrayIndexIsGreaterThanSpace, nameof(array));

            this.items.Values.ToArray().CopyTo(array, arrayIndex);
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
                throw new ArgumentNullException(nameof(item), Messages.ItemIsNull);

            if (!this.items.ContainsKey(item.Identifier) || !this.items[item.Identifier].Equals(item))
                return false;

            return this.items.Remove(item.Identifier);
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{IFeature}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<IFeature> GetEnumerator()
        {
            foreach (IFeature feature in this.items.Values)
                yield return feature;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
