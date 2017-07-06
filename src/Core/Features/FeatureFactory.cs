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

namespace AEGIS.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a factory for producing <see cref="Feature" /> instances.
    /// </summary>
    public class FeatureFactory : Factory, IFeatureFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureFactory" /> class.
        /// </summary>
        public FeatureFactory()
            : base()
        {
        }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The unique identifier of the feature.</param>
        /// <returns>The feature produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IFeature CreateFeature(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            return new Feature(identifier, null, null);
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
                throw new ArgumentNullException(nameof(identifier));
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry));

            return new Feature(identifier, geometry, null);
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
                throw new ArgumentNullException(nameof(identifier));
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry));
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            return new Feature(identifier, geometry, attributes);
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
                throw new ArgumentNullException(nameof(identifier));
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            return new Feature(identifier, null, attributes);
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
                throw new ArgumentNullException(nameof(other));

            return new Feature(other.Identifier, other.Geometry, other.Attributes);
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
                throw new ArgumentNullException(nameof(identifier));

            return new FeatureCollection(identifier, null);
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
                throw new ArgumentNullException(nameof(identifier));
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));

            return new FeatureCollection(identifier, attributes);
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
                throw new ArgumentNullException(nameof(identifier));
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return new FeatureCollection(identifier, attributes, collection);
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
                throw new ArgumentNullException(nameof(identifier));
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return new FeatureCollection(identifier, null, collection);
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
                throw new ArgumentNullException(nameof(other));

            return new FeatureCollection(other.Identifier, other.Attributes, other);
        }
    }
}
