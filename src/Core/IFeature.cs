// <copyright file="IFeature.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;

    /// <summary>
    /// Defines properties of geographic features.
    /// </summary>
    /// <remarks>
    /// A feature is an object that can have a geographic location and other properties, known as attributes.
    /// </remarks>
    public interface IFeature
    {
        /// <summary>
        /// Gets the attribute collection of the feature.
        /// </summary>
        /// <value>The collection of attribute.</value>
        IAttributeCollection Attributes { get; }

        /// <summary>
        /// Gets the factory of the feature.
        /// </summary>
        /// <value>The factory implementation the feature was constructed by.</value>
        IFeatureFactory Factory { get; }

        /// <summary>
        /// Gets the geometry of the feature.
        /// </summary>
        /// <value>The geometry of the feature.</value>
        IGeometry Geometry { get; }

        /// <summary>
        /// Gets the unique identifier of the feature.
        /// </summary>
        /// <value>The identifier of the feature.</value>
        String Identifier { get; }
    }
}
