// <copyright file="IFeatureDriver.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a driver for reading and writing features.
    /// </summary>
    public interface IFeatureDriver : IDriver
    {
        /// <summary>
        /// Gets the driver of the feature attributes.
        /// </summary>
        /// <value>The attribute driver.</value>
        IAttributeDriver AttributeDriver { get; }

        /// <summary>
        /// Gets the driver for the feature geometries.
        /// </summary>
        /// <value>The geometry driver.</value>
        IGeometryDriver GeometryDriver { get; }

        /// <summary>
        /// Gets the feature factory.
        /// </summary>
        /// <value>The feature factory used by the driver.</value>
        IFeatureFactory Factory { get; }

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <returns>The feature created by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IFeature CreateFeature();

        /// <summary>
        /// Creates a feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="geometry">The geometry of the feature.</param>
        /// <param name="attributes">The attributes of the feature.</param>
        /// <returns>The feature created by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IFeature CreateFeature(String identifier, IGeometry geometry, IAttributeCollection attributes);

        /// <summary>
        /// Reads the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The feature read by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IFeature ReadFeature(String identifier);

        /// <summary>
        /// Reads the number of features for the specified collection.
        /// </summary>
        /// <param name="identifier">The feature collection identifier.</param>
        /// <returns>The number of features within the specified collection.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        Int32 ReadFeatureCount(String identifier);

        /// <summary>
        /// Gets the feature identifiers from the collection.
        /// </summary>
        /// <param name="identifier">The feature collection identifier.</param>
        /// <returns>The list of feature identifiers within the specified collection.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IReadOnlyList<String> GetIdentifiers(String identifier);

        /// <summary>
        /// Updates the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="feature">The replacing feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The feature is null.
        /// </exception>
        void UpdateFeature(String identifier, IFeature feature);

        /// <summary>
        /// Deletes the specified feature.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        void DeleteFeature(String identifier);
    }
}
