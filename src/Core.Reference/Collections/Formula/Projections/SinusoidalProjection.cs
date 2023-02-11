// <copyright file="SinusoidalProjection.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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

    /// <summary>
    /// Represents a Sinusoidal Projection.
    /// </summary>
    /// <author>Péter Rónai</author>
    [IdentifiedObject("ESRI::53008", "Sinusoidal Projection")]
    public class SinusoidalProjection : CoordinateProjection
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
        /// The longitude of natural origin.
        /// </summary>
        private readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        private readonly Double sphereRadius;

        /// <summary>
        /// Initializes a new instance of the <see cref="SinusoidalProjection" /> class.
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
        public SinusoidalProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, null, null, CoordinateOperationMethods.SinusoidalProjection, parameters, ellipsoid, areaOfUse)
        {
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.sphereRadius = ellipsoid.SemiMajorAxis.BaseValue;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">the coordinate to be forward transformed</param>
        /// <returns>the transformed coordinate</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            // source: Map Projections - A Working Manual by John P. Snyder (1987) pages 247-248

            Double latitude = coordinate.Latitude.BaseValue;
            Double longitude = coordinate.Longitude.BaseValue;
            Double ellipsoidCorrection = this.Ellipsoid.IsSphere ? 1 : Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(latitude), 2));
            Double x = this.sphereRadius * this.ComputeLongitudeDelta(longitude) * Math.Cos(latitude) / ellipsoidCorrection;
            Double y = this.Ellipsoid.IsSphere ? this.sphereRadius * latitude : this.ComputeYForEllipsoid(latitude);
            return new Coordinate(this.falseEasting + x, this.falseNorthing + y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">the coordinate to be reverse transformed</param>
        /// <returns>the transformed coordinate</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: Map Projections - A Working Manual by John P. Snyder (1987) pages 247-248

            Double x = coordinate.X - this.falseEasting;
            Double y = coordinate.Y - this.falseNorthing;

            if (this.Ellipsoid.IsSphere)
            {
                return this.ComputeReverseSphere(x, y);
            }
            else
            {
                return this.ComputeReverseEllipsoid(x, y);
            }
        }

        /// <summary>
        /// Computes the reverse sphere coordinate.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The resulting coordinate.</returns>
        private GeoCoordinate ComputeReverseSphere(Double x, Double y)
        {
            Double latitude = y / this.sphereRadius;
            Double longitude;

            if (latitude == Math.PI / 2 || latitude == -Math.PI / 2)
            {
                longitude = this.longitudeOfNaturalOrigin;
            }
            else
            {
                longitude = this.longitudeOfNaturalOrigin + x / (this.sphereRadius * Math.Cos(latitude));
            }

            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Computes the reverse ellipsoid coordinate.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The resulting coordinate.</returns>
        private GeoCoordinate ComputeReverseEllipsoid(Double x, Double y)
        {
            Double modifiedEccentricity = this.ComputeModifiedEccentricity();
            Double mu = this.ComputeMu(y);
            Double latitude = mu +
                (3 * modifiedEccentricity / 2 - 27 * Math.Pow(modifiedEccentricity, 3) / 32) * Math.Sin(2 * mu) +
                (21 * modifiedEccentricity * modifiedEccentricity / 16 - 55 * Math.Pow(modifiedEccentricity, 4) / 32) * Math.Sin(4 * mu) +
                (151 * Math.Pow(modifiedEccentricity, 3) / 96) * Math.Sin(6 * mu) +
                (1097 * Math.Pow(modifiedEccentricity, 4) / 512) * Math.Sin(8 * mu);
            Double longitude = this.longitudeOfNaturalOrigin +
                x * Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(latitude), 2)) / (this.sphereRadius * Math.Cos(latitude));
            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Computes the longitude delta.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The longitude delta.</returns>
        private Double ComputeLongitudeDelta(Double longitude)
        {
            Angle threshold = Angle.FromDegree(180);
            Angle diff = Angle.FromRadian(longitude) - Angle.FromRadian(this.longitudeOfNaturalOrigin);
            Angle correction = Angle.FromDegree(diff > threshold ? -360 : (diff < -threshold ? 360 : 0));
            return (diff + correction).BaseValue;
        }

        /// <summary>
        /// Computes the Y coordinate for ellipsoid.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The Y coordinate.</returns>
        private Double ComputeYForEllipsoid(Double latitude)
        {
            Double e2 = this.Ellipsoid.EccentricitySquare;
            Double e4 = Math.Pow(this.Ellipsoid.EccentricitySquare, 2);
            Double e6 = Math.Pow(this.Ellipsoid.EccentricitySquare, 3);
            return this.sphereRadius * ((1 - e2 / 4 - 3 * e4 / 64 - 5 * e6 / 256) * latitude - (3 * e2 / 8 + 3 * e4 / 32 + 45 * e6 / 1024) * Math.Sin(2 * latitude) + (15 * e4 / 256 + 45 * e6 / 1024) * Math.Sin(4 * latitude) - (35 * e6 / 3072) * Math.Sin(6 * latitude));
        }

        /// <summary>
        /// Computes the modified eccentricity.
        /// </summary>
        /// <returns>The modified eccentricity.</returns>
        private Double ComputeModifiedEccentricity()
        {
            return (1 - Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare)) / (1 + Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare));
        }

        /// <summary>
        /// Computes the Mu parameter.
        /// </summary>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The Mu parameter.</returns>
        private Double ComputeMu(Double y)
        {
            Double e2 = this.Ellipsoid.EccentricitySquare;
            Double e4 = Math.Pow(this.Ellipsoid.EccentricitySquare, 2);
            Double e6 = Math.Pow(this.Ellipsoid.EccentricitySquare, 3);
            return y / (this.sphereRadius * (1 - e2 / 4 - 3 * e4 / 64 - 5 * e6 / 256));
        }
    }
}
