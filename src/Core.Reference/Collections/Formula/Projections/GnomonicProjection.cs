// <copyright file="GnomonicProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Gnomonic Projection.
    /// </summary>
    [IdentifiedObject("AEGIS::735137", "Gnomonic Projection")]
    public class GnomonicProjection : CoordinateProjection
    {
        /// <summary>
        /// False easting.
        /// </summary>
        private readonly Double falseEasting;

        /// <summary>
        /// False northing
        /// </summary>
        private readonly Double falseNorthing;

        /// <summary>
        /// The latitude of the projection center.
        /// </summary>
        private readonly Double latitudeOfProjectionCentre;

        /// <summary>
        /// The longitude of the projection center.
        /// </summary>
        private readonly Double longitudeOfProjectionCentre;

        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        private readonly Double sphereRadius;

        /// <summary>
        /// Initializes a new instance of the <see cref="GnomonicProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use, where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameters which are not specified.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public GnomonicProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, null, null, CoordinateOperationMethods.GnomonicProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfProjectionCentre = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfProjectionCentre);
            this.longitudeOfProjectionCentre = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfProjectionCentre);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.sphereRadius = ellipsoid.SemiMajorAxis.Value;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">the coordinate to be forward transformed</param>
        /// <returns>the transformed coordinate</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double latitude = coordinate.Latitude.BaseValue;
            Double longitude = coordinate.Longitude.BaseValue;
            Double longitudeDelta = this.GetLongitudeDelta(longitude);
            Double angularDistanceFromProjectionCentre = this.GetAngularDistanceFromProjectionCentre(latitude, longitude);
            Double x = this.sphereRadius / angularDistanceFromProjectionCentre * (Math.Cos(latitude) * Math.Sin(longitudeDelta));
            Double y = this.sphereRadius / angularDistanceFromProjectionCentre * (Math.Cos(this.latitudeOfProjectionCentre) * Math.Sin(latitude) - Math.Sin(this.latitudeOfProjectionCentre) * Math.Cos(latitude) * Math.Cos(longitudeDelta));
            return new Coordinate(this.falseEasting + x, this.falseNorthing + y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">the coordinate to be reverse transformed</param>
        /// <returns>the transformed coordinate</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = coordinate.X - this.falseEasting;
            Double y = coordinate.Y - this.falseNorthing;
            Double rho = Math.Sqrt(x * x + y * y);

            if (rho == 0)
            {
                // this is a corner case as defined by the working manual:
                // when rho == 0, then the rest of the equations are indeterminate, but longitude, and latitude are exactly on the center of the projection
                return new GeoCoordinate(this.latitudeOfProjectionCentre, this.longitudeOfProjectionCentre);
            }

            Double c = Math.Atan(rho / this.sphereRadius);
            Double latitude = Math.Asin(Math.Cos(c) * Math.Sin(this.latitudeOfProjectionCentre) + ((y * Math.Sin(c) * Math.Cos(this.latitudeOfProjectionCentre)) / rho));
            Double longitude = this.longitudeOfProjectionCentre + Math.Atan(x * Math.Sin(c) / (rho * Math.Cos(this.latitudeOfProjectionCentre) * Math.Cos(c) - y * Math.Sin(this.latitudeOfProjectionCentre) * Math.Sin(c)));
            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Returns the angular distance of a given point on the Earth's surface from the projection center.
        /// </summary>
        /// <remarks>
        /// This simplified calculation is specific to the Gnomonic Projection (e.g. it doesn't take the Spheroid into account).
        /// </remarks>
        /// <param name="latitude">Latitude of the geographic coordinate (in radians).</param>
        /// <param name="longitude">Longitude of the geographic coordinate (in radians).</param>
        /// <returns>The angular distance from the projection center (in radians).</returns>
        private Double GetAngularDistanceFromProjectionCentre(Double latitude, Double longitude)
        {
            return Math.Sin(this.latitudeOfProjectionCentre) * Math.Sin(latitude) + Math.Cos(this.latitudeOfProjectionCentre) * Math.Cos(latitude) * Math.Cos(this.GetLongitudeDelta(longitude));
        }

        /// <summary>
        /// Returns the longitude delta.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The longitude delta.</returns>
        private Double GetLongitudeDelta(Double longitude)
        {
            Angle threshold = Angle.FromDegree(180);
            Angle diff = Angle.FromRadian(longitude) - Angle.FromRadian(this.longitudeOfProjectionCentre);
            Angle correction = Angle.FromDegree(diff > threshold ? -360 : (diff < -threshold ? 360 : 0));
            return (diff + correction).BaseValue;
        }
    }
}
