// <copyright file="HotineObliqueMercatorBProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    using ELTE.AEGIS.Numerics;

    /// <summary>
    /// Represents a Hotine Oblique Mercator (Variant B) projection.
    /// </summary>
    [IdentifiedObject("EPSG::9815", "Hotine Oblique Mercator (variant B)")]
    public class HotineObliqueMercatorBProjection : HotineObliqueMercatorProjection
    {
        /// <summary>
        /// Easting at projection centre.
        /// </summary>
        private readonly Double eastingAtProjectionCentre;

        /// <summary>
        /// Northing at projection centre.
        /// </summary>
        private readonly Double northingAtProjectionCentre;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotineObliqueMercatorBProjection" /> class.
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
        public HotineObliqueMercatorBProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotineObliqueMercatorBProjection" /> class.
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
        public HotineObliqueMercatorBProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.HotineObliqueMercatorBProjection, parameters, ellipsoid, areaOfUse)
        {
            this.eastingAtProjectionCentre = this.GetParameterValue(CoordinateOperationParameters.EastingAtProjectionCentre);
            this.northingAtProjectionCentre = this.GetParameterValue(CoordinateOperationParameters.NorthingAtProjectionCentre);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double u, v;
            this.ComputeUV(coordinate.Latitude.BaseValue, coordinate.Longitude.BaseValue, out u, out v);

            Double easting = v * Math.Cos(this.angleFromRectifiedToSkewGrid) + u * Math.Sin(this.angleFromRectifiedToSkewGrid) + this.eastingAtProjectionCentre;
            Double northing = u * Math.Cos(this.angleFromRectifiedToSkewGrid) - v * Math.Sin(this.angleFromRectifiedToSkewGrid) + this.northingAtProjectionCentre;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double v = (coordinate.X - this.eastingAtProjectionCentre) * Math.Cos(this.angleFromRectifiedToSkewGrid) - (coordinate.Y - this.northingAtProjectionCentre) * Math.Sin(this.angleFromRectifiedToSkewGrid);
            Double u = (coordinate.Y - this.northingAtProjectionCentre) * Math.Cos(this.angleFromRectifiedToSkewGrid) + (coordinate.X - this.eastingAtProjectionCentre) * Math.Sin(this.angleFromRectifiedToSkewGrid) + Math.Abs(this.uC) * Math.Sign(this.latitudeOfProjectionCentre);
            Double latitude, longitude;

            this.ComputeLatitudeLongitude(u, v, out latitude, out longitude);

            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Computes the u value.
        /// </summary>
        /// <param name="s">The S value.</param>
        /// <param name="v">The V value.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>
        /// The u value.
        /// </returns>
        protected override Double ComputeU(Double s, Double v, Double longitude)
        {
            return (Math.Abs(this.azimuthOfInitialLine - Math.PI / 2) <= 1E-10) ?
                       ((longitude == this.longitudeOfProjectionCentre) ? 0 : this.a * Math.Atan((s * Math.Cos(this.gammaO) + v * Math.Sin(this.gammaO)) / Math.Cos(this.b * (longitude - this.lambdaO))) / this.b - Math.Abs(this.uC) * Math.Sign(this.latitudeOfProjectionCentre) * Math.Sign(this.longitudeOfProjectionCentre - longitude)) :
                       this.a * Math.Atan((s * Math.Cos(this.gammaO) + v * Math.Sin(this.gammaO)) / Math.Cos(this.b * (longitude - this.lambdaO))) / this.b - Math.Abs(this.uC) * Math.Sign(this.latitudeOfProjectionCentre);
        }
    }
}
