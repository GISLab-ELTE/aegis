// <copyright file="VerticalPerspectiveProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Vertical Perspective projection.
    /// </summary>
    [IdentifiedObject("EPSG::9838", "Vertical Perspective")]
    public class VerticalPerspectiveProjection : CoordinateProjection
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
        /// View point height.
        /// </summary>
        private readonly Double viewPointHeight;

        /// <summary>
        /// Radius of prime vertical curvature at origin.
        /// </summary>
        private readonly Double originRadiosOfPrimeVerticalCurvature;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalPerspectiveProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public VerticalPerspectiveProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalPerspectiveProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public VerticalPerspectiveProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.VerticalPerspectiveProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfTopocentricOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfTopocentricOrigin);
            this.longitudeOfTopocentricOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfTopocentricOrigin);
            this.ellipsoidalHeightOfTopocentricOrigin = this.GetParameterValue(CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin);
            this.viewPointHeight = this.GetParameterValue(CoordinateOperationParameters.ViewpointHeight);

            this.originRadiosOfPrimeVerticalCurvature = this.Ellipsoid.RadiusOfPrimeVerticalCurvature(this.latitudeOfTopocentricOrigin);
        }

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
            Double v = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Cos(this.latitudeOfTopocentricOrigin) - Math.Cos(latitude) * Math.Sin(this.latitudeOfTopocentricOrigin) * Math.Cos(longitude - this.longitudeOfTopocentricOrigin)) + this.Ellipsoid.EccentricitySquare * (this.originRadiosOfPrimeVerticalCurvature * Math.Sin(this.latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Cos(this.latitudeOfTopocentricOrigin);
            Double w = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Sin(this.latitudeOfTopocentricOrigin) + Math.Cos(latitude) * Math.Cos(this.latitudeOfTopocentricOrigin) * Math.Cos(longitude - this.longitudeOfTopocentricOrigin)) + this.Ellipsoid.EccentricitySquare * (this.originRadiosOfPrimeVerticalCurvature * Math.Sin(this.latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Sin(this.latitudeOfTopocentricOrigin) - (this.ellipsoidalHeightOfTopocentricOrigin + this.originRadiosOfPrimeVerticalCurvature);

            Double easting = u * this.viewPointHeight / (this.viewPointHeight - w);
            Double northing = v * this.viewPointHeight / (this.viewPointHeight - w);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            return GeoCoordinate.Undefined;
        }
    }
}