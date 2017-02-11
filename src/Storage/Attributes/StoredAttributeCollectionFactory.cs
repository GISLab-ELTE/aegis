// <copyright file="StoredAttributeCollectionFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Attributes
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Collections.Resources;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a factory producing <see cref="IAttributeCollection" /> instances located in stores.
    /// </summary>
    public class StoredAttributeCollectionFactory : Factory, IStoredAttributeCollectionFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredAttributeCollectionFactory" /> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <exception cref="System.ArgumentNullException">The driver is null.</exception>
        public StoredAttributeCollectionFactory(IAttributeDriver driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver), ELTE.AEGIS.Storage.Resources.StorageMessages.DriverIsNull);

            this.Driver = driver;
        }

        /// <summary>
        /// Gets the attribute driver of the factory.
        /// </summary>
        /// <value>The attribute driver of the factory.</value>
        public IAttributeDriver Driver { get; private set; }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <returns>The produced attribute collection.</returns>
        public IAttributeCollection CreateCollection()
        {
            return new StoredAttributeCollection(this, this.Driver.CreateIdentifier());
        }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <returns>The produced attribute collection.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IAttributeCollection CreateCollection(IDictionary<String, Object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CollectionMessages.SourceIsNull);

            StoredAttributeCollection collection = new StoredAttributeCollection(this, this.Driver.CreateIdentifier());
            foreach (String key in source.Keys)
            {
                collection.Add(key, source[key]);
            }

            return collection;
        }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <returns>The produced attribute collection.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IAttributeCollection CreateCollection(IAttributeCollection source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CollectionMessages.SourceIsNull);

            if (source is IStoredAttributeCollection)
            {
                return new StoredAttributeCollection(this, (source as IStoredAttributeCollection).Identifier);
            }
            else
            {
                return this.CreateCollection(source as IDictionary<String, Object>);
            }
        }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The produced attribute collection.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IAttributeCollection CreateCollection(String identifier)
        {
            return new StoredAttributeCollection(this, identifier);
        }

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
        public IAttributeCollection CreateCollection(String identifier, IAttributeCollection source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CollectionMessages.SourceIsNull);

            StoredAttributeCollection collection = new StoredAttributeCollection(this, identifier);

            foreach (String key in source.Keys)
            {
                collection.Add(key, source[key]);
            }

            return collection;
        }
    }
}
