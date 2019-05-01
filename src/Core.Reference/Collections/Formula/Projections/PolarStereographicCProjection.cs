// <copyright file="PolarStereographicCProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Polar Stereographic (Variant C) Projection.
    /// </summary>
    [IdentifiedObject("EPSG::9830", "Polar Stereographic (variant C)")]
    public class PolarStereographicCProjection : PolarStereographicProjection
    {
        /// <summary>
        /// Latitude of standard parallel.
        /// </summary>
        private readonly Double latitudeOfStandardParallel;

        /// <summary>
        /// Longitude of origin.
        /// </summary>
        private readonly Double longitudeOfOrigin;

        /// <summary>
        /// Easting at false origin.
        /// </summary>
        private readonly Double eastingAtFalseOrigin;

        /// <summary>
        /// Northing at false origin.
        /// </summary>
        private readonly Double northingAtFalseOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double roF;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double tF;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarStereographicCProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public PolarStereographicCProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarStereographicCProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public PolarStereographicCProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.PolarStereographicCProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfStandardParallel);
            this.longitudeOfOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfOrigin);
            this.eastingAtFalseOrigin = this.GetParameterValue(CoordinateOperationParameters.EastingAtFalseOrigin);
            this.northingAtFalseOrigin = this.GetParameterValue(CoordinateOperationParameters.NorthingAtFalseOrigin);

            this.operationAspect = (this.latitudeOfStandardParallel >= 0) ? OperationAspect.NorthPolar : OperationAspect.SouthPolar;

            Double mF;

            mF = Math.Cos(this.latitudeOfStandardParallel) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfStandardParallel));

            if (this.operationAspect == OperationAspect.NorthPolar)
            {
                this.tF = Math.Tan(Math.PI / 4 - this.latitudeOfStandardParallel / 2) / Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)), this.Ellipsoid.Eccentricity / 2);
            }
            else
            {
                this.tF = Math.Tan(Math.PI / 4 + this.latitudeOfStandardParallel / 2) / Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)), this.Ellipsoid.Eccentricity / 2);
            }

            this.roF = this.Ellipsoid.SemiMajorAxis.Value * mF;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double easting = 0, northing = 0;
            Double t = this.ComputeT(coordinate.Latitude.BaseValue);
            Double ro = (this.roF * t) / this.tF;

            switch (this.operationAspect)
            {
                case OperationAspect.NorthPolar:
                    easting = this.eastingAtFalseOrigin + ro * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
                    northing = this.northingAtFalseOrigin + this.roF - ro * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
                    break;
                case OperationAspect.SouthPolar:
                    easting = this.eastingAtFalseOrigin + ro * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
                    northing = this.northingAtFalseOrigin - this.roF + ro * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
                    break;
            }

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double ksi = 0, longitude = 0, latitude;
            Double deltaX = coordinate.X - this.eastingAtFalseOrigin, deltaY;
            double t;
            double phi;
            switch (this.operationAspect)
            {
                case OperationAspect.NorthPolar:
                    deltaY = coordinate.Y - this.northingAtFalseOrigin - this.roF;
                    phi = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    t = phi * this.tF / this.roF;
                    ksi = Math.PI / 2 - 2 * Math.Atan(t);

                    if (coordinate.X == this.eastingAtFalseOrigin)
                        longitude = this.longitudeOfOrigin;
                    else
                        longitude = this.longitudeOfOrigin + Math.Atan2(coordinate.X - this.eastingAtFalseOrigin, this.northingAtFalseOrigin + this.roF - coordinate.Y);
                    break;
                case OperationAspect.SouthPolar:
                    deltaY = coordinate.Y - this.northingAtFalseOrigin + this.roF;
                    phi = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                    t = phi * this.tF / this.roF;
                    ksi = 2 * Math.Atan(t) - Math.PI / 2;

                    if (coordinate.X == this.eastingAtFalseOrigin)
                        longitude = this.longitudeOfOrigin;
                    else
                        longitude = this.longitudeOfOrigin + Math.Atan2(coordinate.X - this.eastingAtFalseOrigin, coordinate.Y - this.northingAtFalseOrigin + this.roF);
                    break;
            }

            latitude = ksi +
                       (this.Ellipsoid.EccentricitySquare / 2 + 5 * Math.Pow(this.Ellipsoid.Eccentricity, 4) / 24 + Math.Pow(this.Ellipsoid.Eccentricity, 6) / 12 + 13 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 360) * Math.Sin(2 * ksi) +
                       (7 * Math.Pow(this.Ellipsoid.Eccentricity, 4) / 48 + 29 * Math.Pow(this.Ellipsoid.Eccentricity, 6) / 240 + 811 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 11520) * Math.Sin(4 * ksi) +
                       (7 * Math.Pow(this.Ellipsoid.Eccentricity, 6) / 120 + 81 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 1120) * Math.Sin(6 * ksi) +
                       (4279 * Math.Pow(this.Ellipsoid.Eccentricity, 8) / 161280) * Math.Sin(8 * ksi);

            return new GeoCoordinate(latitude, longitude);
        }
    }
}
