// <copyright file="StoredFeatureFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Features
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Resources;
    using ELTE.AEGIS.Storage.Attributes;
    using ELTE.AEGIS.Storage.Geometries;

    /// <summary>
    /// Represents a factory producing <see cref="IFeature" /> instances located in stores.
    /// </summary>
    public class StoredFeatureFactory : Factory, IStoredFeatureFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredFeatureFactory" /> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <exception cref="System.ArgumentNullException">The driver is null.</exception>
        public StoredFeatureFactory(IFeatureDriver driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver), ELTE.AEGIS.Storage.Resources.Messages.DriverIsNull);

            this.Driver = driver;

            StoredAttributeCollectionFactory attributeFactory = new StoredAttributeCollectionFactory(driver.AttributeDriver);
            StoredGeometryFactory geometryFactory = new StoredGeometryFactory(driver.GeometryDriver);

            this.EnsureFactory<IAttributeCollectionFactory>(attributeFactory);
            this.EnsureFactory<IStoredAttributeCollectionFactory>(attributeFactory);
            this.EnsureFactory<IGeometryFactory>(geometryFactory);
            this.EnsureFactory<IStoredGeometryFactory>(geometryFactory);
        }

        #endregion

        #region IFeatureFactory properties

        /// <summary>
        /// Gets the attribute collection factory.
        /// </summary>
        /// <value>The attribute collection factory.</value>
        IAttributeCollectionFactory IFeatureFactory.AttributeCollectionFactory { get { return this.GetFactory<IAttributeCollectionFactory>(); } }

        /// <summary>
        /// Gets the geometry factory.
        /// </summary>
        /// <value>The geometry factory.</value>
        IGeometryFactory IFeatureFactory.GeometryFactory { get { return this.GetFactory<IGeometryFactory>(); } }

        #endregion

        #region IStoredFeatureFactory properties

        /// <summary>
        /// Gets the attribute collection factory.
        /// </summary>
        /// <value>The attribute collection factory.</value>
        public IStoredAttributeCollectionFactory AttributeCollectionFactory { get { return this.GetFactory<IStoredAttributeCollectionFactory>(); } }

        /// <summary>
        /// Gets the geometry factory.
        /// </summary>
        /// <value>The geometry factory.</value>
        public IStoredGeometryFactory GeometryFactory { get { return this.GetFactory<IStoredGeometryFactory>(); } }

        /// <summary>
        /// Gets the feature driver of the factory.
        /// </summary>
        /// <value>The feature driver of the factory.</value>
        public IFeatureDriver Driver { get; private set; }

        #endregion

        #region Factory methods for features

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The unique identifier of the feature.</param>
        /// <returns>The feature produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IFeature CreateFeature(String identifier)
        {
            return new StoredFeature(this, identifier);
        }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The unique identifier of the feature.</param>
        /// <param name="geometry">The geometry of the feature.</param>
        /// <returns>The feature produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The geometry is null.
        /// </exception>
        public IFeature CreateFeature(String identifier, IGeometry geometry)
        {
            (this.GeometryFactory as IStoredGeometryFactory).CreateGeometry(identifier, geometry);

            return new StoredFeature(this, identifier);
        }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The unique identifier of the feature.</param>
        /// <param name="geometry">The geometry of the feature.</param>
        /// <param name="attributes">The attributes of the feature.</param>
        /// <returns>The feature produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The geometry is null.
        /// or
        /// The attribute collection is null.
        /// </exception>
        public IFeature CreateFeature(String identifier, IGeometry geometry, IAttributeCollection attributes)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry), Messages.GeometryIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), Messages.AttributeCollectionIsNull);

            (this.AttributeCollectionFactory as IStoredAttributeCollectionFactory).CreateCollection(identifier, attributes);
            (this.GeometryFactory as IStoredGeometryFactory).CreateGeometry(identifier, geometry);

            return new StoredFeature(this, identifier);
        }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The unique identifier of the feature.</param>
        /// <param name="attributes">The attributes of the feature.</param>
        /// <returns>The feature produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The attribute collection is null.
        /// </exception>
        public IFeature CreateFeature(String identifier, IAttributeCollection attributes)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), Messages.AttributeCollectionIsNull);

            (this.AttributeCollectionFactory as IStoredAttributeCollectionFactory).CreateCollection(identifier, attributes);

            return new StoredFeature(this, identifier);
        }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="other">The other feature.</param>
        /// <returns>The feature matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other feature is null.</exception>
        public IFeature CreateFeature(IFeature other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), Messages.OtherFeatureIsNull);

            if (other is IStoredFeature)
            {
                return new StoredFeature(this, other.Identifier);
            }
            else
            {
                (this.AttributeCollectionFactory as IStoredAttributeCollectionFactory).CreateCollection(other.Identifier, other.Attributes);
                (this.GeometryFactory as IStoredGeometryFactory).CreateGeometry(other.Identifier, other.Geometry);

                return new StoredFeature(this, other.Identifier);
            }
        }

        #endregion

        #region Factory methods for feature collections

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="identifier">The unique identifier of the collection.</param>
        /// <returns>The feature collection produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IFeatureCollection CreateCollection(String identifier)
        {
            return new StoredFeatureCollection(this, identifier);
        }

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="identifier">The unique identifier of the collection.</param>
        /// <param name="attributes">The attributes of the collection.</param>
        /// <returns>The feature collection produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The attribute collection is null.
        /// </exception>
        public IFeatureCollection CreateCollection(String identifier, IAttributeCollection attributes)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), Messages.AttributeCollectionIsNull);

            (this.AttributeCollectionFactory as IStoredAttributeCollectionFactory).CreateCollection(identifier, attributes);

            return new StoredFeatureCollection(this, identifier);
        }

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="identifier">The unique identifier of the collection.</param>
        /// <param name="attributes">The attributes of the collection.</param>
        /// <param name="collection">The collection of features.</param>
        /// <returns>The feature collection produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The attribute collection is null.
        /// or
        /// The collection is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The collection contains one or more duplicate identifiers.</exception>
        public IFeatureCollection CreateCollection(String identifier, IAttributeCollection attributes, IEnumerable<IFeature> collection)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), Messages.AttributeCollectionIsNull);
            if (collection == null)
                throw new ArgumentNullException(Messages.CollectionIsNull, nameof(collection));

            (this.AttributeCollectionFactory as IStoredAttributeCollectionFactory).CreateCollection(identifier, attributes);

            StoredFeatureCollection featureCollection = new StoredFeatureCollection(this, identifier);
            List<String> storedIdentifiers = new List<String>();

            foreach (IFeature feature in collection)
            {
                if (feature == null)
                    continue;

                if (storedIdentifiers.Contains(feature.Identifier))
                    throw new ArgumentException(nameof(collection), Messages.CollectionContainsDuplicateIdentifiers);

                featureCollection.Add(feature);
                storedIdentifiers.Add(feature.Identifier);
            }

            return featureCollection;
        }

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="identifier">The unique identifier of the collection.</param>
        /// <param name="collection">The collection of features.</param>
        /// <returns>The feature collection produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The collection is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The collection contains one or more duplicate identifiers.</exception>
        public IFeatureCollection CreateCollection(String identifier, IEnumerable<IFeature> collection)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), Messages.CollectionIsNull);

            StoredFeatureCollection featureCollection = new StoredFeatureCollection(this, identifier);
            List<String> storedIdentifiers = new List<String>();

            foreach (IFeature feature in collection)
            {
                if (feature == null)
                    continue;

                if (storedIdentifiers.Contains(feature.Identifier))
                    throw new ArgumentException(Messages.CollectionContainsDuplicateIdentifiers, nameof(collection));

                featureCollection.Add(feature);
                storedIdentifiers.Add(feature.Identifier);
            }

            return featureCollection;
        }

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="other">The other feature collection.</param>
        /// <returns>The feature collection matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public IFeatureCollection CreateCollection(IFeatureCollection other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), Messages.CollectionIsNull);

            return this.CreateCollection(other.Identifier, other.Attributes, other);
        }

        #endregion
    }
}
