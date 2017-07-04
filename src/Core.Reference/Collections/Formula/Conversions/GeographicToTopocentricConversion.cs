// <copyright file="GeographicToTopocentricConversion.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;

    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a geographic to topocentric conversion.
    /// </summary>
    [IdentifiedObject("EPSG::9837", "Geographic/topocentric conversion")]
    public class GeographicToTopocentricConversion : CoordinateConversion<GeoCoordinate, Coordinate>
    {
        /// <summary>
        /// Latitude of topocentric origin.
        /// </summary>
        private readonly Double latitudeOfTopocentricOrigin;

        /// <summary>
        /// Longitude of topocentric origin.
        /// </summary>
        private readonly Double longitudeOfTopocentricOrigin;

        /// <summary>
        /// Ellipsoidal height of topocentric origin.
        /// </summary>
        private readonly Double ellipsoidalHeightOfTopocentricOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double originRadiousOfPrimeVerticalCurvature;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Coordinate originGeocentricCoordinate;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double sinLamda0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double cosLamda0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double sinFi0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double cosFi0;

        /// <summary>
        /// The conversion from geographic to geocentric coordinates.
        /// </summary>
        private readonly GeographicToGeocentricConversion conversion;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicToTopocentricConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public GeographicToTopocentricConversion(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : this(identifier, name, null, null, parameters, ellipsoid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicToTopocentricConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public GeographicToTopocentricConversion(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.GeographicToTopocentricConversion, parameters)
        {
            this.latitudeOfTopocentricOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfTopocentricOrigin);
            this.longitudeOfTopocentricOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfTopocentricOrigin);
            this.ellipsoidalHeightOfTopocentricOrigin = this.GetParameterValue(CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin);

            this.Ellipsoid = ellipsoid ?? throw new ArgumentNullException(nameof(ellipsoid));

            this.originRadiousOfPrimeVerticalCurvature = this.Ellipsoid.RadiusOfPrimeVerticalCurvature(this.latitudeOfTopocentricOrigin);

            this.conversion = new GeographicToGeocentricConversion(identifier, name, this.Ellipsoid);

            this.originGeocentricCoordinate = this.conversion.Forward(new GeoCoordinate(this.latitudeOfTopocentricOrigin, this.longitudeOfTopocentricOrigin, this.ellipsoidalHeightOfTopocentricOrigin));

            this.sinLamda0 = Math.Sin(this.longitudeOfTopocentricOrigin);
            this.cosLamda0 = Math.Cos(this.longitudeOfTopocentricOrigin);

            this.sinFi0 = Math.Sin(this.latitudeOfTopocentricOrigin);
            this.cosFi0 = Math.Cos(this.latitudeOfTopocentricOrigin);
        }

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid used by the operation.</value>
        public Ellipsoid Ellipsoid { get; private set; }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double primeVerticalCurvature = this.Ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude).BaseValue;

            Double height = coordinate.Height.BaseValue;
            Double latitude = coordinate.Latitude.BaseValue;
            Double longitude = coordinate.Longitude.BaseValue;

            Double u = (height + primeVerticalCurvature) * Math.Cos(latitude) * Math.Sin(longitude - this.longitudeOfTopocentricOrigin);
            Double v = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Cos(this.latitudeOfTopocentricOrigin) - Math.Cos(latitude) * Math.Sin(this.latitudeOfTopocentricOrigin) * Math.Cos(longitude - this.longitudeOfTopocentricOrigin)) + this.Ellipsoid.EccentricitySquare * (this.originRadiousOfPrimeVerticalCurvature * Math.Sin(this.latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Cos(this.latitudeOfTopocentricOrigin);
            Double w = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Sin(this.latitudeOfTopocentricOrigin) + Math.Cos(latitude) * Math.Cos(this.latitudeOfTopocentricOrigin) * Math.Cos(longitude - this.longitudeOfTopocentricOrigin)) + this.Ellipsoid.EccentricitySquare * (this.originRadiousOfPrimeVerticalCurvature * Math.Sin(this.latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Sin(this.latitudeOfTopocentricOrigin) - (this.ellipsoidalHeightOfTopocentricOrigin + this.originRadiousOfPrimeVerticalCurvature);

            return new Coordinate(u, v, w);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = this.originGeocentricCoordinate.X - coordinate.X * this.sinLamda0 - coordinate.Y * this.sinFi0 * this.cosLamda0 + coordinate.Z * this.cosFi0 * this.cosLamda0;
            Double y = this.originGeocentricCoordinate.Y + coordinate.X * this.cosLamda0 - coordinate.Y * this.sinFi0 * this.sinLamda0 + coordinate.Z * this.cosFi0 * this.sinLamda0;
            Double z = this.originGeocentricCoordinate.Z + coordinate.Y * this.cosFi0 + coordinate.Z * this.sinFi0;

            return this.conversion.Reverse(new Coordinate(x, y, z));
        }
    }
}