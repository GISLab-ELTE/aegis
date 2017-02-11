// <copyright file="ModifiedAzimuthalEquidistantProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Modified Azimuthal Equidistant projection.
    /// </summary>
    [IdentifiedObject("AEGIS::9832", "Modified Azimuthal Equidistant Projection")]
    public class ModifiedAzimuthalEquidistantProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        private readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        private readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// False easting.
        /// </summary>
        private readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        private readonly Double falseNorthing;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double g;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedAzimuthalEquidistantProjection" /> class.
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
        public ModifiedAzimuthalEquidistantProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedAzimuthalEquidistantProjection" /> class.
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
        public ModifiedAzimuthalEquidistantProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.ModifiedAzimuthalEquidistantProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.g = this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfNaturalOrigin) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double psi = Math.Atan((1 - this.Ellipsoid.EccentricitySquare) * Math.Tan(coordinate.Latitude.BaseValue) + this.Ellipsoid.EccentricitySquare * this.Ellipsoid.RadiusOfPrimeVerticalCurvature(this.latitudeOfNaturalOrigin) * Math.Sin(this.latitudeOfNaturalOrigin) / (this.Ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Latitude.BaseValue)));

            Double alpha = Math.Atan(Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin) / (Math.Cos(this.latitudeOfNaturalOrigin) * Math.Tan(psi) - Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin)));

            Double h = this.Ellipsoid.Eccentricity * Math.Cos(this.latitudeOfNaturalOrigin) * Math.Cos(alpha) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare);

            Double s;
            if (Math.Sin(alpha) == 0)
                s = Math.Asin(Math.Cos(this.latitudeOfNaturalOrigin) * Math.Sin(psi) - Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(psi)) * Math.Sign(Math.Cos(alpha));
            else
                s = Math.Asin(Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin) * Math.Cos(psi) / Math.Sin(alpha));

            Double hSquare = h * h;

            Double c = this.Ellipsoid.RadiusOfPrimeVerticalCurvature(this.latitudeOfNaturalOrigin) * s * ((1 - s * s * hSquare * (1 - hSquare) / 6) + ((Math.Pow(s, 3) / 8) * this.g * h * (1 - 2 * hSquare)) + (Math.Pow(s, 4) / 120) * (hSquare * (4 - 7 * hSquare) - 3 * this.g * this.g * (1 - 7 * hSquare)) - ((Math.Pow(s, 5) / 48) * this.g * h));

            Double easting = this.falseEasting + c * Math.Sin(alpha);
            Double northing = this.falseNorthing + c * Math.Cos(alpha);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double deltaX = coordinate.X - this.falseEasting;
            Double deltaY = coordinate.Y - this.falseNorthing;
            Double c = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            Double alpha = Math.Atan((coordinate.X - this.falseEasting) / (coordinate.Y - this.falseNorthing));
            Double a = -1 * this.Ellipsoid.EccentricitySquare * Calculator.Cos2(this.latitudeOfNaturalOrigin) * Calculator.Cos2(alpha) / (1 - this.Ellipsoid.EccentricitySquare);
            Double b = 3 * this.Ellipsoid.EccentricitySquare * (1 - a) * Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(this.latitudeOfNaturalOrigin) * Math.Cos(alpha) / (1 - this.Ellipsoid.EccentricitySquare);
            Double d = c / this.Ellipsoid.RadiusOfPrimeVerticalCurvature(this.latitudeOfNaturalOrigin);
            Double j = d - (a * (1 + a) * Math.Pow(d, 3) / 6) - (b * (1 + 3 * a) * Math.Pow(d, 4) / 24);
            Double k = 1 - (a * j * j / 2) - (b * Math.Pow(j, 3) / 6);
            Double psi = Math.Asin(Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(j) + Math.Cos(this.latitudeOfNaturalOrigin) * Math.Sin(j) * Math.Cos(alpha));

            Double latitude = Math.Atan((1 - this.Ellipsoid.EccentricitySquare * k * Math.Sin(this.latitudeOfNaturalOrigin) / Math.Sin(psi)) * Math.Tan(psi) / (1 - this.Ellipsoid.EccentricitySquare));
            Double longitude = this.longitudeOfNaturalOrigin + Math.Asin(Math.Sin(alpha) * Math.Sin(j) / Math.Cos(psi));

            return new GeoCoordinate(latitude, longitude);
        }
    }
}
