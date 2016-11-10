// <copyright file="CoordinateOperationStrategyFactory.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Reference.Collections;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a factory for producing <see cref="ICoordinateOperationStrategy" /> instances.
    /// </summary>
    public class CoordinateOperationStrategyFactory
    {
        /// <summary>
        /// The expected latitude string. This field is constant.
        /// </summary>
        private const String ExpectedLatitudeString = "lat";

        /// <summary>
        /// The expected longitude string. This field is constant.
        /// </summary>
        private const String ExpectedLongitudeString = "long";

        /// <summary>
        /// The underlying reference collection container. This field is read-only.
        /// </summary>
        private readonly IReferenceCollectionContainer collectionContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationStrategyFactory" /> class.
        /// </summary>
        /// <param name="collectionContainer">The collection container.</param>
        /// <exception cref="System.ArgumentNullException">The collection container is null.</exception>
        public CoordinateOperationStrategyFactory(IReferenceCollectionContainer collectionContainer)
        {
            if (collectionContainer == null)
                throw new ArgumentNullException(nameof(collectionContainer), Messages.CollectionContainerIsNull);

            this.collectionContainer = collectionContainer;
        }

        /// <summary>
        /// Creates a strategy for converting coordinates between reference systems.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>The produced transformation strategy.</returns>
        /// <exception cref="System.NotSupportedException">Conversion does not exist between the specified reference systems.</exception>
        public ICoordinateOperationStrategy CreateStrategy(ReferenceSystem source, ReferenceSystem target)
        {
            ICoordinateOperationStrategy<Coordinate, GeoCoordinate> conversionToGeographic = null;
            ICoordinateOperationStrategy<GeoCoordinate, GeoCoordinate> transformation = null;
            ICoordinateOperationStrategy<GeoCoordinate, Coordinate> conversionFromGeographic = null;

            switch (source.Type)
            {
                case ReferenceSystemType.Projected:
                    conversionToGeographic = new ReverseCoordinateProjectionStrategy(source as ProjectedCoordinateReferenceSystem);
                    break;
                case ReferenceSystemType.Geographic2D:
                case ReferenceSystemType.Geographic3D:
                    conversionToGeographic = this.CreateReverseConversion(source as CoordinateReferenceSystem);
                    break;
            }

            switch (target.Type)
            {
                case ReferenceSystemType.Projected:
                    conversionFromGeographic = new ForwardCoordinateProjectionStrategy(target as ProjectedCoordinateReferenceSystem);
                    break;
                case ReferenceSystemType.Geographic2D:
                case ReferenceSystemType.Geographic3D:
                    conversionFromGeographic = this.CreateForwardConversion(target as CoordinateReferenceSystem);
                    break;
            }

            // if no transformation is needed
            if (conversionFromGeographic.TargetReferenceSystem.Equals(conversionToGeographic.SourceReferenceSystem))
                return new CoordinateConversionStrategy(conversionToGeographic, conversionFromGeographic);

            GeographicCoordinateReferenceSystem conversionTargetReferenceSystem = conversionToGeographic.TargetReferenceSystem as GeographicCoordinateReferenceSystem;
            GeographicCoordinateReferenceSystem conversionSourceReferenceSystem = conversionFromGeographic.SourceReferenceSystem as GeographicCoordinateReferenceSystem;

            // load matching forward transformation
            IEnumerable<CoordinateTransformation<GeoCoordinate>> transformations = this.collectionContainer.GeoCoordinateTransformations.WithProperties(conversionTargetReferenceSystem, conversionSourceReferenceSystem);
            if (transformations.Any())
            {
                transformation = new ForwardGeographicCoordinateTransformationStrategy(conversionTargetReferenceSystem, conversionSourceReferenceSystem, transformations.First());
                return new CompoundCoordinateConversionStrategy(conversionToGeographic, transformation, conversionFromGeographic);
            }

            // if none found, load matching reverse transformation
            transformations = this.collectionContainer.GeoCoordinateTransformations.WithProperties(conversionSourceReferenceSystem, conversionTargetReferenceSystem);
            if (transformations.Any())
            {
                transformation = new ReverseGeographicCoordinateTransformationStrategy(conversionTargetReferenceSystem, conversionSourceReferenceSystem, transformations.First());
                return new CompoundCoordinateConversionStrategy(conversionToGeographic, transformation, conversionFromGeographic);
            }

            throw new NotSupportedException(Messages.ConversionDoesNotExistBetweenReferenceSystems);
        }

        /// <summary>
        /// Creates a strategy for converting Cartesian coordinates to geographic coordinates.
        /// </summary>
        /// <param name="referenceSystem">The coordinate reference system.</param>
        /// <returns>The produced conversion strategy.</returns>
        private ICoordinateOperationStrategy<Coordinate, GeoCoordinate> CreateReverseConversion(CoordinateReferenceSystem referenceSystem)
        {
            // HACK: The order of coordinate system axis is determined based on axis name (part). This should be replaced with a better method.

            if (referenceSystem.CoordinateSystem.GetAxis(0).Name.Contains(ExpectedLatitudeString) &&
                referenceSystem.CoordinateSystem.GetAxis(1).Name.Contains(ExpectedLongitudeString))
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ReverseLatLonHiCoordinateInterpretationStrategy(referenceSystem);
                }
                else
                {
                    return new ReverseLatLonCoordinateInterpretationStrategy(referenceSystem);
                }
            }
            else
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ReverseLonLatHiCoordinateInterpretationStrategy(referenceSystem);
                }
                else
                {
                    return new ReverseLonLatCoordinateIntepretationStrategy(referenceSystem);
                }
            }
        }

        /// <summary>
        /// Creates a strategy for converting geographic coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="referenceSystem">The coordinate reference system.</param>
        /// <returns>The produced conversion strategy.</returns>
        private ICoordinateOperationStrategy<GeoCoordinate, Coordinate> CreateForwardConversion(CoordinateReferenceSystem referenceSystem)
        {
            // HACK: The order of coordinate system axis is determined based on axis name (part). This should be replaced with a better method.

            if (referenceSystem.CoordinateSystem.GetAxis(0).Name.Contains(ExpectedLatitudeString) &&
                referenceSystem.CoordinateSystem.GetAxis(1).Name.Contains(ExpectedLongitudeString))
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ForwardLatLonHiCoordinateIntepretationStrategy(referenceSystem);
                }
                else
                {
                    return new ForwardLatLonCoordinateInterpretationStrategy(referenceSystem);
                }
            }
            else
            {
                if (referenceSystem.Dimension == 3)
                {
                    return new ForwardLonLatHiCoordinateInterpretationStrategy(referenceSystem);
                }
                else
                {
                    return new ForwardLonLatCoordinateIntepretationStrategy(referenceSystem);
                }
            }
        }
    }
}
