// <copyright file="AttributeCollectionFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Attributes
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a factory for attribute collections.
    /// </summary>
    public class AttributeCollectionFactory : Factory, IAttributeCollectionFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollectionFactory" /> class.
        /// </summary>
        public AttributeCollectionFactory() { }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <returns>The produced attribute collection.</returns>
        public virtual IAttributeCollection CreateCollection() { return new AttributeCollection(); }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <returns>The produced attribute collection.</returns>
        public virtual IAttributeCollection CreateCollection(IDictionary<String, Object> source)
        {
            if (source == null)
                return null;

            return new AttributeCollection(source);
        }

        /// <summary>
        /// Creates an attribute collection.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <returns>The produced attribute collection.</returns>
        public virtual IAttributeCollection CreateCollection(IAttributeCollection source)
        {
            if (source == null)
                return null;

            return new AttributeCollection(source);
        }
    }
}
