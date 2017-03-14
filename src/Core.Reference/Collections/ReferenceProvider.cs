// <copyright file="ReferenceProvider.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections
{
    using System;
    using SimpleInjector;

    /// <summary>
    /// Represents a container of reference collections.
    /// </summary>
    /// <remarks>
    /// This type uses an underlying IoC container to specify realization of different reference collections.
    /// All implementing types must ensure that all the required collections are registered in the underlying container after construction.
    /// </remarks>
    public abstract class ReferenceProvider : IReferenceProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceProvider" /> class.
        /// </summary>
        protected ReferenceProvider()
        {
            this.Container = new Container();
        }

        /// <summary>
        /// Gets the collection of <see cref="AreaOfUse" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="AreaOfUse" /> instances.</value>
        public IReferenceCollection<AreaOfUse> AreasOfUse
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<AreaOfUse>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CompoundReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CompoundReferenceSystem" /> instances.</value>
        public IReferenceCollection<CompoundReferenceSystem> CompoundReferenceSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<CompoundReferenceSystem>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateOperationMethod" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateOperationMethod" /> instances.</value>
        public IReferenceCollection<CoordinateOperationMethod> CoordinateOperationMethods
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<CoordinateOperationMethod>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateOperationParameter" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateOperationParameter" /> instances.</value>
        public IReferenceCollection<CoordinateOperationParameter> CoordinateOperationParameters
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<CoordinateOperationParameter>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateProjection" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateProjection" /> instances.</value>
        public ICoordinateProjectionCollection CoordinateProjections
        {
            get
            {
                return this.Container.GetInstance<ICoordinateProjectionCollection>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateReferenceSystem" /> instances.</value>
        public IReferenceCollection<CoordinateReferenceSystem> CoordinateReferenceSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<CoordinateReferenceSystem>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateSystemAxis" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateSystemAxis" /> instances.</value>
        public ICoordinateSystemAxisCollection CoordinateSystemAxes
        {
            get
            {
                return this.Container.GetInstance<ICoordinateSystemAxisCollection>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateSystem" /> instances.</value>
        public IReferenceCollection<CoordinateSystem> CoordinateSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<CoordinateSystem>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateTransformation{Coordinate}" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateTransformation{Coordinate}" /> instances.</value>
        public ICoordinateTransformationCollection<Coordinate> CoordinateTransformations
        {
            get
            {
                return this.Container.GetInstance<ICoordinateTransformationCollection<Coordinate>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="Datum" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="Datum" /> instances.</value>
        public IReferenceCollection<Datum> Datums
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<Datum>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="Ellipsoid" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="Ellipsoid" /> instances.</value>
        public IReferenceCollection<Ellipsoid> Ellipsoids
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<Ellipsoid>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="GeocentricCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="GeocentricCoordinateReferenceSystem" /> instances.</value>
        public IReferenceCollection<GeocentricCoordinateReferenceSystem> GeocentricCoordinateReferenceSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<GeocentricCoordinateReferenceSystem>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="CoordinateTransformation{GeoCoordinate}" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="CoordinateTransformation{GeoCoordinate}" /> instances.</value>
        public ICoordinateTransformationCollection<GeoCoordinate> GeoCoordinateTransformations
        {
            get
            {
                return this.Container.GetInstance<ICoordinateTransformationCollection<GeoCoordinate>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="GeodeticDatum" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="GeodeticDatum" /> instances.</value>
        public IReferenceCollection<GeodeticDatum> GeodeticDatums
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<GeodeticDatum>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="GeographicCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="GeographicCoordinateReferenceSystem" /> instances.</value>
        public IReferenceCollection<GeographicCoordinateReferenceSystem> GeographicCoordinateReferenceSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<GeographicCoordinateReferenceSystem>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="Meridian" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="Meridian" /> instances.</value>
        public IReferenceCollection<Meridian> Meridians
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<Meridian>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="ProjectedCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="ProjectedCoordinateReferenceSystem" /> instances.</value>
        public IReferenceCollection<ProjectedCoordinateReferenceSystem> ProjectedCoordinateReferenceSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<ProjectedCoordinateReferenceSystem>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="ReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="ReferenceSystem" /> instances.</value>
        public IReferenceCollection<ReferenceSystem> ReferenceSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<ReferenceSystem>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="UnitOfMeasurement" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="UnitOfMeasurement" /> instances.</value>
        public IReferenceCollection<UnitOfMeasurement> UnitsOfMeasurement
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<UnitOfMeasurement>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="VerticalDatum" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="VerticalDatum" /> instances.</value>
        public IReferenceCollection<VerticalDatum> VerticalDatums
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<VerticalDatum>>();
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="VerticalCoordinateReferenceSystem" /> instances.
        /// </summary>
        /// <value>The registered collection of <see cref="VerticalCoordinateReferenceSystem" /> instances.</value>
        public IReferenceCollection<VerticalCoordinateReferenceSystem> VerticalReferenceSystems
        {
            get
            {
                return this.Container.GetInstance<IReferenceCollection<VerticalCoordinateReferenceSystem>>();
            }
        }

        /// <summary>
        /// Gets the underlying inversion of control container.
        /// </summary>
        protected Container Container { get; private set; }
    }
}
