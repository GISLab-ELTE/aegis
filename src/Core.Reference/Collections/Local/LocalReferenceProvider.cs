// <copyright file="LocalReferenceProvider.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Local
{
    using System;
    using SimpleInjector;

    /// <summary>
    /// Represents a container of local reference collections.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalReferenceProvider : ReferenceProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalReferenceProvider" /> class.
        /// </summary>
        public LocalReferenceProvider()
        {
            this.Container.RegisterSingleton<IReferenceCollection<AreaOfUse>, LocalAreaOfUseCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<CompoundReferenceSystem>, LocalCompoundReferenceSystemCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<CoordinateOperationMethod>, CoordinateOperationMethodCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<CoordinateOperationParameter>, CoordinateOperationParameterCollection>();
            this.Container.RegisterSingleton<ICoordinateProjectionCollection, LocalCoordinateProjectionCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<CoordinateReferenceSystem>, LocalCoordinateReferenceSystemCollection>();
            this.Container.RegisterSingleton<ICoordinateSystemAxisCollection, LocalCoordinateSystemAxisCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<CoordinateSystem>, LocalCoordinateSystemCollection>();
            this.Container.RegisterSingleton<ICoordinateTransformationCollection<Coordinate>, LocalCoordinateTransformationCollection<Coordinate>>();
            this.Container.RegisterSingleton<IReferenceCollection<Datum>, LocalDatumCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<Ellipsoid>, LocalEllipsoidCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<GeocentricCoordinateReferenceSystem>, LocalGeocentricCoordinateReferenceSystemCollection>();
            this.Container.RegisterSingleton<ICoordinateTransformationCollection<GeoCoordinate>, LocalCoordinateTransformationCollection<GeoCoordinate>>();
            this.Container.RegisterSingleton<IReferenceCollection<GeodeticDatum>, LocalGeodeticDatumCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<GeographicCoordinateReferenceSystem>, LocalGeographicCoordinateReferenceSystemCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<Meridian>, LocalMeridianCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<ProjectedCoordinateReferenceSystem>, LocalProjectedCoordinateReferenceSystemCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<ReferenceSystem>, LocalReferenceSystemCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<UnitOfMeasurement>, UnitOfMeasurementCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<VerticalCoordinateReferenceSystem>, LocalVerticalCoordinateReferenceSystemCollection>();
            this.Container.RegisterSingleton<IReferenceCollection<VerticalDatum>, LocalVerticalDatumCollection>();
        }
    }
}
