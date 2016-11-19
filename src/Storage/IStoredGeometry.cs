// <copyright file="IStoredGeometry.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for geometries located in stores.
    /// </summary>
    public interface IStoredGeometry : IGeometry
    {
        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        new IStoredGeometryFactory Factory { get; }

        /// <summary>
        /// Gets the feature identifier.
        /// </summary>
        /// <value>The unique feature identifier within the store.</value>
        String Identifier { get; }

        /// <summary>
        /// Gets the collection of indexes within the feature.
        /// </summary>
        /// <value>The collection of indexes which determine the location of the geometry within the feature.</value>
        IEnumerable<Int32> Indexes { get; }

        /// <summary>
        /// Gets the driver of the geometry.
        /// </summary>
        /// <value>The driver of the geometry.</value>
        IGeometryDriver Driver { get; }
    }
}
