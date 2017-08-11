// <copyright file="IFeatureCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Defines properties of geographic feature collections.
    /// </summary>
    public interface IFeatureCollection : IFeature, ICollection<IFeature>
    {
        /// <summary>
        /// Gets the feature identifiers within the collection.
        /// </summary>
        /// <value>The read-only list of feature identifiers within the collection.</value>
        IEnumerable<String> Identifiers { get; }

        /// <summary>
        /// Gets the feature with the specified identifier.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The feature at the specified index if the feature exists; otherwise, <c>null</c>.</returns>
        IFeature this[String identifier] { get; }
    }
}
