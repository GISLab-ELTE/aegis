// <copyright file="AttributeCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Attributes
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a collection of attributes.
    /// </summary>
    public class AttributeCollection : Dictionary<String, Object>, IAttributeCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        public AttributeCollection()
            : this(new AttributeCollectionFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="AttributeCollection" /> can contain.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public AttributeCollection(Int32 capacity)
            : this(new AttributeCollectionFactory(), capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{String, Object}" /> whose elements are copied to the new <see cref="AttributeCollection" />.</param>
        public AttributeCollection(IDictionary<String, Object> source)
            : this(new AttributeCollectionFactory(), source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="source">The <see cref="IAttributeCollection" /> whose elements are copied to the new <see cref="AttributeCollection" />.</param>
        public AttributeCollection(IAttributeCollection source)
            : this(new AttributeCollectionFactory(), source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory of the collection.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public AttributeCollection(AttributeCollectionFactory factory)
            : base()
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);

            this.Factory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory of the collection.</param>
        /// <param name="capacity">The initial number of elements that the <see cref="AttributeCollection" /> can contain.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public AttributeCollection(AttributeCollectionFactory factory, Int32 capacity)
            : base(capacity)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);

            this.Factory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory of the collection.</param>
        /// <param name="source">The <see cref="IDictionary{String, Object}" /> whose elements are copied to the new <see cref="AttributeCollection" />.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The source is null.
        /// </exception>
        public AttributeCollection(AttributeCollectionFactory factory, IDictionary<String, Object> source)
            : base(source)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);

            this.Factory = factory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory of the collection.</param>
        /// <param name="source">The <see cref="IAttributeCollection" /> whose elements are copied to the new <see cref="AttributeCollection" />.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The source is null.
        /// </exception>
        public AttributeCollection(AttributeCollectionFactory factory, IAttributeCollection source)
            : base(source)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);

            this.Factory = factory;
        }

        /// <summary>
        /// Gets the factory of the attribute collection.
        /// </summary>
        /// <value>The factory implementation the attribute collection was constructed by.</value>
        public IAttributeCollectionFactory Factory { get; private set; }
    }
}
