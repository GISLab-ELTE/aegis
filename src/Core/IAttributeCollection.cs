// <copyright file="IAttributeCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines behavior of a collection of attributes.
    /// </summary>
    public interface IAttributeCollection : IDictionary<String, Object>
    {
        /// <summary>
        /// Gets the factory of the attribute collection.
        /// </summary>
        /// <value>The factory implementation the attribute collection was constructed by.</value>
        IAttributeCollectionFactory Factory { get; }
    }
}
