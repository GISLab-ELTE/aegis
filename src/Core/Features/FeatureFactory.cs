// <copyright file="FeatureFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a factory for producing <see cref="Feature" /> instances.
    /// </summary>
    public class FeatureFactory : Factory, IFeatureFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureFactory" /> class.
        /// </summary>
        /// <param name="geometryFactory">The geometry factory.</param>
        /// <param name="attributeFactory">The attribute factory.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry factory is null.
        /// or
        /// The attribute factory is null.
        /// </exception>
        public FeatureFactory(IGeometryFactory geometryFactory, IAttributeCollectionFactory attributeFactory)
            : base(geometryFactory, attributeFactory)
        {
            if (geometryFactory == null)
                throw new ArgumentNullException(nameof(geometryFactory), CoreMessages.GeometryFactoryIsNull);
            if (attributeFactory == null)
                throw new ArgumentNullException(nameof(attributeFactory), CoreMessages.AttributeFactoryIsNull);
        }

        /// <summary>
        /// Gets the attribute collection factory.
        /// </summary>
        /// <value>The attribute collection factory.</value>
        public IAttributeCollectionFactory AttributeCollectionFactory { get { return this.GetFactory<IAttributeCollectionFactory>(); } }

        /// <summary>
        /// Gets the geometry factory.
        /// </summary>
        /// <value>The geometry factory.</value>
        public IGeometryFactory GeometryFactory { get { return this.GetFactory<IGeometryFactory>(); } }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The unique identifier of the feature.</param>
        /// <returns>The feature produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IFeature CreateFeature(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            return new Feature(this, identifier, null, null);
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
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry), CoreMessages.GeometryIsNull);

            return new Feature(this, identifier, geometry, null);
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
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry), CoreMessages.GeometryIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), CoreMessages.AttributeCollectionIsNull);

            return new Feature(this, identifier, geometry, attributes);
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
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), CoreMessages.AttributeCollectionIsNull);

            return new Feature(this, identifier, null, attributes);
        }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="other">The other feature.</param>
        /// <returns>The feature matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The feature is null.</exception>
        public IFeature CreateFeature(IFeature other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.FeatureIsNull);

            return new Feature(this, other.Identifier, other.Geometry, other.Attributes);
        }

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="identifier">The unique identifier of the collection.</param>
        /// <returns>The feature collection produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IFeatureCollection CreateCollection(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            return new FeatureCollection(this, identifier, null);
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
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), CoreMessages.AttributeCollectionIsNull);

            return new FeatureCollection(this, identifier, attributes);
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
        public IFeatureCollection CreateCollection(String identifier, IAttributeCollection attributes, IEnumerable<IFeature> collection)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), CoreMessages.AttributeCollectionIsNull);
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CoreMessages.CollectionIsNull);

            return new FeatureCollection(this, identifier, attributes, collection);
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
        public IFeatureCollection CreateCollection(String identifier, IEnumerable<IFeature> collection)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CoreMessages.CollectionIsNull);

            return new FeatureCollection(this, identifier, null, collection);
        }

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="other">The other feature collection.</param>
        /// <returns>The feature collection matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The feature collection is null.</exception>
        public IFeatureCollection CreateCollection(IFeatureCollection other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.FeatureCollectionIsNull);

            return new FeatureCollection(this, other.Identifier, null, other);
        }
    }
}
