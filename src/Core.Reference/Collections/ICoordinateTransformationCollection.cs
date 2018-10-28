// <copyright file="ICoordinateTransformationCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Reference;

    /// <summary>
    /// Defines queries for <see cref="ICoordinateTransformationCollection{CoordinateType}" /> collections.
    /// </summary>
    /// <typeparam name="CoordinateType">The type of the coordinate.</typeparam>
    public interface ICoordinateTransformationCollection<CoordinateType> : IReferenceCollection<CoordinateTransformation<CoordinateType>>
    {
        /// <summary>
        /// Returns a collection with items matching the specified properties.
        /// </summary>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <returns>The collection of items with the specified properties.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// </exception>
        IEnumerable<CoordinateTransformation<CoordinateType>> WithProperties(CoordinateReferenceSystem source, CoordinateReferenceSystem target);

        /// <summary>
        /// Returns a collection with items matching the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <returns>The collection of items with the specified properties.</returns>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The area of use is null.
        /// </exception>
        IEnumerable<CoordinateTransformation<CoordinateType>> WithProperties(CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, AreaOfUse areaOfUse);

        /// <summary>
        /// Returns a collection with items matching the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <returns>The collection of items with the specified properties.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        IEnumerable<CoordinateTransformation<CoordinateType>> WithProperties(CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse);
    }
}
