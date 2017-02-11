// <copyright file="ICoordinateOperationStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Strategies
{
    using ELTE.AEGIS.Reference;

    /// <summary>
    /// Defines behavior of coordinate operation strategies.
    /// </summary>
    public interface ICoordinateOperationStrategy : ICoordinateOperationStrategy<Coordinate, Coordinate>
    {
    }

    /// <summary>
    /// Defines behavior of generic coordinate operation strategies.
    /// </summary>
    /// <typeparam name="SourceType">The source coordinate type.</typeparam>
    /// <typeparam name="ResultType">The result coordinate type.</typeparam>
    public interface ICoordinateOperationStrategy<SourceType, ResultType>
    {
        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        ReferenceSystem SourceReferenceSystem { get; }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        ReferenceSystem TargetReferenceSystem { get; }

        /// <summary>
        /// Applies the strategy on the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        ResultType Apply(SourceType coordinate);
    }
}
