// <copyright file="ForwardGeographicCoordinateTransformationStrategy.cs" company="Eötvös Loránd University (ELTE)">
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
    using System;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a forward geographic coordinate transformation strategy.
    /// </summary>
    public class ForwardGeographicCoordinateTransformationStrategy : ICoordinateOperationStrategy<GeoCoordinate, GeoCoordinate>
    {
        /// <summary>
        /// The source reference system. This field is read-only.
        /// </summary>
        private readonly GeographicCoordinateReferenceSystem source;

        /// <summary>
        /// The target reference system. This field is read-only.
        /// </summary>
        private readonly GeographicCoordinateReferenceSystem target;

        /// <summary>
        /// The geographic transformation. This field is read-only.
        /// </summary>
        private readonly CoordinateTransformation<GeoCoordinate> transformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForwardGeographicCoordinateTransformationStrategy" /> class.
        /// </summary>
        /// <param name="source">The source reference system.</param>
        /// <param name="target">The target reference system.</param>
        /// <param name="transformation">The geographic transformation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source reference system is null.
        /// or
        /// The target reference system is null.
        /// or
        /// The transformation is null.
        /// </exception>
        public ForwardGeographicCoordinateTransformationStrategy(GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, CoordinateTransformation<GeoCoordinate> transformation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), ReferenceMessages.SourceReferenceSystemIsNull);
            if (target == null)
                throw new ArgumentNullException(nameof(target), ReferenceMessages.TargetReferenceSystemIsNull);
            if (transformation == null)
                throw new ArgumentNullException(nameof(transformation), ReferenceMessages.TransformationIsNull);

            this.source = source;
            this.target = target;
            this.transformation = transformation;
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
        public ReferenceSystem TargetReferenceSystem { get { return this.target; } }

        /// <summary>
        /// Applies the strategy on the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public GeoCoordinate Apply(GeoCoordinate coordinate)
        {
            return this.transformation.Forward(coordinate);
        }
    }
}
