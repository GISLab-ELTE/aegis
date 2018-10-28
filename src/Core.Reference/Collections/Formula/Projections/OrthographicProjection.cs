// <copyright file="OrthographicProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Numerics;

    /// <summary>
    /// Represents an Orthographic Projection.
    /// </summary>
    [IdentifiedObject("EPSG::9840", "Orthographic")]
    public class OrthographicProjection : CoordinateProjection
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
        /// Initializes a new instance of the <see cref="OrthographicProjection" /> class.
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
        public OrthographicProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrthographicProjection" /> class.
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
        public OrthographicProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.OrthographicProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;
            Double sinPhi = Math.Sin(phi);
            Double sinLatitude = Math.Sin(this.latitudeOfNaturalOrigin);
            Double nu = this.Ellipsoid.SemiMajorAxis.BaseValue / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * sinPhi * sinPhi);
            Double nu0 = this.Ellipsoid.SemiMajorAxis.BaseValue / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * sinLatitude * sinLatitude);

            Double easting = this.falseEasting + nu * Math.Cos(phi) * Math.Sin(lambda - this.longitudeOfNaturalOrigin);
            Double northing = this.falseNorthing + nu * (Math.Sin(phi) * Math.Cos(this.latitudeOfNaturalOrigin) - Math.Cos(phi) * Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(lambda - this.longitudeOfNaturalOrigin)) + this.Ellipsoid.EccentricitySquare * (nu0 * Math.Sin(this.latitudeOfNaturalOrigin) - nu * Math.Sin(phi)) * Math.Cos(this.latitudeOfNaturalOrigin);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double phi = this.latitudeOfNaturalOrigin;
            Double lambda = this.longitudeOfNaturalOrigin;
            Double phi0 = coordinate.X;
            Double lambda0 = coordinate.Y;
            Double nu0 = this.Ellipsoid.SemiMajorAxis.BaseValue / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(this.latitudeOfNaturalOrigin), 2));

            while (Math.Abs(phi0 - phi) > 0.00000001 || Math.Abs(lambda0 - lambda) > 0.00000001)
            {
                phi0 = phi;
                lambda0 = lambda;
                Double nu = this.Ellipsoid.SemiMajorAxis.BaseValue / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(phi0), 2));
                Double rho = this.Ellipsoid.SemiMajorAxis.BaseValue * (1 - this.Ellipsoid.EccentricitySquare) / Math.Pow(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(phi0), 2), 1.5);

                Double easting = this.falseEasting + nu * Math.Cos(phi0) * Math.Sin(lambda0 - this.longitudeOfNaturalOrigin);
                Double northing = this.falseNorthing + nu * (Math.Sin(phi0) * Math.Cos(this.latitudeOfNaturalOrigin) - Math.Cos(phi0) * Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(lambda0 - this.longitudeOfNaturalOrigin)) + this.Ellipsoid.EccentricitySquare * (nu0 * Math.Sin(this.latitudeOfNaturalOrigin) - nu * Math.Sin(phi0)) * Math.Cos(this.latitudeOfNaturalOrigin);
                Double j11 = -rho * Math.Sin(phi0) * Math.Sin(lambda0 - this.longitudeOfNaturalOrigin);
                Double j12 = nu * Math.Cos(phi0) * Math.Cos(lambda0 - this.longitudeOfNaturalOrigin);
                Double j21 = rho * (Math.Cos(phi0) * Math.Cos(this.latitudeOfNaturalOrigin) + Math.Sin(phi0) * Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(lambda0 - this.longitudeOfNaturalOrigin));
                Double j22 = nu * Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(phi0) * Math.Sin(lambda0 - this.longitudeOfNaturalOrigin);

                Double d = j11 * j22 - j12 * j21;
                Double deltaE = coordinate.X - easting;
                Double deltaN = coordinate.Y - northing;

                phi = phi0 + (j22 * deltaE - j12 * deltaN) / d;
                lambda = lambda0 + (-j21 * deltaE + j11 * deltaN) / d;
            }

            return new GeoCoordinate(phi, lambda);
        }
    }
}
