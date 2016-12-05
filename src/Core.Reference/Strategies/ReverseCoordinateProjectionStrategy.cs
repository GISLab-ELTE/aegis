// <copyright file="ReverseCoordinateProjectionStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Strategies
{
    using System;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a reverse coordinate projection strategy.
    /// </summary>
    public class ReverseCoordinateProjectionStrategy : ICoordinateOperationStrategy<Coordinate, GeoCoordinate>
    {
        /// <summary>
        /// The target reference system. This field is read-only.
        /// </summary>
        private readonly ProjectedCoordinateReferenceSystem source;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseCoordinateProjectionStrategy" /> class.
        /// </summary>
        /// <param name="source">The source reference system.</param>
        /// <exception cref="System.ArgumentNullException">The reference system is null.</exception>
        public ReverseCoordinateProjectionStrategy(ProjectedCoordinateReferenceSystem source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), ReferenceMessages.ReferenceSystemIsNull);

            this.source = source;
        }

        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        public ReferenceSystem SourceReferenceSystem { get { return this.source; } }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        public ReferenceSystem TargetReferenceSystem { get { return this.source.BaseReferenceSystem; } }

        /// <summary>
        /// Applies the strategy on the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public GeoCoordinate Apply(Coordinate coordinate)
        {
            return this.source.Projection.Reverse(coordinate);
        }
    }
}
