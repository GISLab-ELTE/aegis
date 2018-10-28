// <copyright file="IStoredAttributeCollectionFactory.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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
    /// Defines behavior for factories producing <see cref="IAttributeCollection" /> instances located in stores.
    /// </summary>
    public interface IStoredAttributeCollectionFactory : IAttributeCollectionFactory
    {
        /// <summary>
        /// Gets the attribute driver of the factory.
        /// </summary>
        /// <value>The attribute driver of the factory.</value>
        IAttributeDriver Driver { get; }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The produced attribute collection.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IAttributeCollection CreateCollection(String identifier);

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="source">The source collection.</param>
        /// <returns>The produced attribute collection.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The source is null.
        /// </exception>
        IAttributeCollection CreateCollection(String identifier, IAttributeCollection source);
    }
}
