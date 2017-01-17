// <copyright file="IReferenceProvider.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections
{
    using System;

    /// <summary>
    /// Defines properties for reference providers.
    /// </summary>
    public interface IReferenceProvider
    {
        /// <summary>
        /// Gets the collection of <see cref="AreaOfUse" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="AreaOfUse" /> instances.</value>
        IReferenceCollection<AreaOfUse> AreasOfUse { get; }

        /// <summary>
        /// Gets the collection of <see cref="CompoundReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CompoundReferenceSystem" /> instances.</value>
        IReferenceCollection<CompoundReferenceSystem> CompoundReferenceSystems { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateOperationMethod" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateOperationMethod" /> instances.</value>
        IReferenceCollection<CoordinateOperationMethod> CoordinateOperationMethods { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateOperationParameter" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateOperationParameter" /> instances.</value>
        IReferenceCollection<CoordinateOperationParameter> CoordinateOperationParameters { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateProjection" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateProjection" /> instances.</value>
        ICoordinateProjectionCollection CoordinateProjections { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateReferenceSystem" /> instances.</value>
        IReferenceCollection<CoordinateReferenceSystem> CoordinateReferenceSystems { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateSystemAxis" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateSystemAxis" /> instances.</value>
        ICoordinateSystemAxisCollection CoordinateSystemAxes { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateSystem" /> instances.</value>
        IReferenceCollection<CoordinateSystem> CoordinateSystems { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateTransformation{Coordinate}" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateTransformation{Coordinate}" /> instances.</value>
        ICoordinateTransformationCollection<Coordinate> CoordinateTransformations { get; }

        /// <summary>
        /// Gets the collection of <see cref="Datum" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="Datum" /> instances.</value>
        IReferenceCollection<Datum> Datums { get; }

        /// <summary>
        /// Gets the collection of <see cref="Ellipsoid" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="Ellipsoid" /> instances.</value>
        IReferenceCollection<Ellipsoid> Ellipsoids { get; }

        /// <summary>
        /// Gets the collection of <see cref="GeocentricCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="GeocentricCoordinateReferenceSystem" /> instances.</value>
        IReferenceCollection<GeocentricCoordinateReferenceSystem> GeocentricCoordinateReferenceSystems { get; }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateTransformation{GeoCoordinate}" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateTransformation{GeoCoordinate}" /> instances.</value>
        ICoordinateTransformationCollection<GeoCoordinate> GeoCoordinateTransformations { get; }

        /// <summary>
        /// Gets the collection of <see cref="GeodeticDatum" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="GeodeticDatum" /> instances.</value>
        IReferenceCollection<GeodeticDatum> GeodeticDatums { get; }

        /// <summary>
        /// Gets the collection of <see cref="GeographicCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="GeographicCoordinateReferenceSystem" /> instances.</value>
        IReferenceCollection<GeographicCoordinateReferenceSystem> GeographicCoordinateReferenceSystems { get; }

        /// <summary>
        /// Gets the collection of <see cref="Meridian" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="Meridian" /> instances.</value>
        IReferenceCollection<Meridian> Meridians { get; }

        /// <summary>
        /// Gets the collection of <see cref="ProjectedCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="ProjectedCoordinateReferenceSystem" /> instances.</value>
        IReferenceCollection<ProjectedCoordinateReferenceSystem> ProjectedCoordinateReferenceSystems { get; }

        /// <summary>
        /// Gets the collection of <see cref="ReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="ReferenceSystem" /> instances.</value>
        IReferenceCollection<ReferenceSystem> ReferenceSystems { get; }

        /// <summary>
        /// Gets the collection of <see cref="UnitOfMeasurement" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="UnitOfMeasurement" /> instances.</value>
        IReferenceCollection<UnitOfMeasurement> UnitsOfMeasurement { get; }

        /// <summary>
        /// Gets the collection of <see cref="VerticalDatum" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="VerticalDatum" /> instances.</value>
        IReferenceCollection<VerticalDatum> VerticalDatums { get; }

        /// <summary>
        /// Gets the collection of <see cref="VerticalCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="VerticalCoordinateReferenceSystem" /> instances.</value>
        IReferenceCollection<VerticalCoordinateReferenceSystem> VerticalReferenceSystems { get; }
    }
}
