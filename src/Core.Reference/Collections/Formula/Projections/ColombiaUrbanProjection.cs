// <copyright file="ColombiaUrbanProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Numerics;

    /// <summary>
    /// Represents a Colombia Urban Projection.
    /// </summary>
    [IdentifiedObject("EPSG::1052", "Colombia Urban Projection")]
    public class ColombiaUrbanProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        protected readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        protected readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// False easting.
        /// </summary>
        protected readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        protected readonly Double falseNorthing;

        /// <summary>
        /// False northing.
        /// </summary>
        protected readonly Double projectionPlaneOriginHeight;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double rho0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double nu0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColombiaUrbanProjection" /> class.
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
        public ColombiaUrbanProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColombiaUrbanProjection" /> class.
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
        public ColombiaUrbanProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.ColombiaUrbanProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.projectionPlaneOriginHeight = this.GetParameterValue(CoordinateOperationParameters.ProjectionPlaneOriginHeight);

            Double sinLatitude = Math.Sin(this.latitudeOfNaturalOrigin);
            this.rho0 = this.Ellipsoid.SemiMajorAxis.Value * (1 - this.Ellipsoid.EccentricitySquare / Math.Pow(1 - this.Ellipsoid.EccentricitySquare * sinLatitude * sinLatitude, 1.5));
            this.nu0 = this.Ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Sqrt(this.latitudeOfNaturalOrigin) * Math.Sin(this.latitudeOfNaturalOrigin));
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
            Double nu = this.Ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(phi), 2));
            Double bA = 1 + this.projectionPlaneOriginHeight / this.nu0;
            Double midLatitude = (phi + this.latitudeOfNaturalOrigin) / 2;
            Double rhoM = this.Ellipsoid.SemiMajorAxis.Value * (1 - this.Ellipsoid.EccentricitySquare) / Math.Pow(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(midLatitude), 2), 1.5);
            Double bG = 1 + this.projectionPlaneOriginHeight / rhoM;
            Double bB = Math.Tan(this.latitudeOfNaturalOrigin) / (2 * this.rho0 * this.nu0);

            Double easting = this.falseEasting + bA * nu * Math.Cos(phi) * (lambda - this.longitudeOfNaturalOrigin);
            Double northing = this.falseNorthing + bG * this.rho0 * ((phi - this.latitudeOfNaturalOrigin) + bB * Math.Pow(lambda - this.longitudeOfNaturalOrigin, 2) * nu * nu * Math.Pow(Math.Cos(phi), 2));

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double bB = Math.Tan(this.latitudeOfNaturalOrigin) / (2 * this.rho0 * this.nu0);
            Double bC = 1 + this.projectionPlaneOriginHeight / this.Ellipsoid.SemiMajorAxis.Value;
            Double bD = this.rho0 * (1 + this.projectionPlaneOriginHeight / this.Ellipsoid.SemiMajorAxis.Value * (1 - this.Ellipsoid.EccentricitySquare));

            Double phi = this.latitudeOfNaturalOrigin + (coordinate.Y - this.falseNorthing) / bD - bB * Math.Pow((coordinate.X - this.falseEasting) / bC, 2);
            Double nu = this.Ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(phi), 2));
            Double lambda = this.longitudeOfNaturalOrigin + (coordinate.X - this.falseEasting) / (bC * nu * Math.Cos(phi));

            return new GeoCoordinate(phi, lambda);
        }
    }
}
