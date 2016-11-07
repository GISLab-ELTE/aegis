// <copyright file="IFeatureFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for factories producing <see cref="IFeature" /> instances.
    /// </summary>
    public interface IFeatureFactory : IFactory
    {
        #region Properties

        /// <summary>
        /// Gets the attribute collection factory.
        /// </summary>
        /// <value>The attribute collection factory.</value>
        IAttributeCollectionFactory AttributeCollectionFactory { get; }

        /// <summary>
        /// Gets the geometry factory.
        /// </summary>
        /// <value>The geometry factory.</value>
        IGeometryFactory GeometryFactory { get; }

        #endregion

        #region Factory methods for features

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The unique identifier of the feature.</param>
        /// <returns>The feature produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IFeature CreateFeature(String identifier);

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
        IFeature CreateFeature(String identifier, IGeometry geometry);

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
        IFeature CreateFeature(String identifier, IGeometry geometry, IAttributeCollection attributes);

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
        IFeature CreateFeature(String identifier, IAttributeCollection attributes);

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="other">The other feature.</param>
        /// <returns>The feature matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other feature is null.</exception>
        IFeature CreateFeature(IFeature other);

        #endregion

        #region Factory methods for feature collections

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="identifier">The unique identifier of the collection.</param>
        /// <returns>The feature collection produced by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IFeatureCollection CreateCollection(String identifier);

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
        IFeatureCollection CreateCollection(String identifier, IAttributeCollection attributes);

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
        IFeatureCollection CreateCollection(String identifier, IAttributeCollection attributes, IEnumerable<IFeature> collection);

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
        IFeatureCollection CreateCollection(String identifier, IEnumerable<IFeature> collection);

        /// <summary>
        /// Creates a feature collection.
        /// </summary>
        /// <param name="other">The other feature collection.</param>
        /// <returns>The feature collection matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other feature collection is null.</exception>
        IFeatureCollection CreateCollection(IFeatureCollection other);

        #endregion
    }
}
