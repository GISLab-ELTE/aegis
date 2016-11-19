// <copyright file="IStoredFeatureFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage
{
    using System;

    /// <summary>
    /// Defines behavior for factories producing <see cref="IFeature" /> instances located in stores.
    /// </summary>
    public interface IStoredFeatureFactory : IFeatureFactory
    {
        #region Properties

        /// <summary>
        /// Gets the attribute collection factory.
        /// </summary>
        /// <value>The attribute collection factory.</value>
        new IStoredAttributeCollectionFactory AttributeCollectionFactory { get; }

        /// <summary>
        /// Gets the geometry factory.
        /// </summary>
        /// <value>The geometry factory.</value>
        new IStoredGeometryFactory GeometryFactory { get; }

        /// <summary>
        /// Gets the feature driver of the factory.
        /// </summary>
        /// <value>The feature driver of the factory.</value>
        IFeatureDriver Driver { get; }

        #endregion
    }
}
