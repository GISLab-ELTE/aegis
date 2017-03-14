// <copyright file="IReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines general behavior of reference systems.
    /// </summary>
    public interface IReferenceSystem
    {
        /// <summary>
        /// Gets the number of dimensions.
        /// </summary>
        Int32 Dimension { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        String Identifier { get; }

        /// <summary>
        /// Gets the authority.
        /// </summary>
        /// <value>The authority responsible for the object if provided; otherwise, <c>Empty</c>.</value>
        String Authority { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code by which the object is identified in the authority's domain if provided; otherwise, <c>0</c>.</value>
        Int32 Code { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        String Name { get; }
    }
}
