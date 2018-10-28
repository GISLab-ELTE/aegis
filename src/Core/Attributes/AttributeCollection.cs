// <copyright file="AttributeCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Resources;

    /// <summary>
    /// Represents a collection of attributes.
    /// </summary>
    public class AttributeCollection : Dictionary<String, Object>, IAttributeCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        public AttributeCollection()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="AttributeCollection" /> can contain.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public AttributeCollection(Int32 capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeCollection" /> class.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{String, Object}" /> whose elements are copied to the new <see cref="AttributeCollection" />.</param>
        public AttributeCollection(IDictionary<String, Object> source)
            : base(source)
        {
        }
    }
}
