﻿// <copyright file="PolarStereographicBProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Polar Stereographic (Variant B) Projection.
    /// </summary>
    /// <author>András Fábián</author>
    [IdentifiedObject("EPSG::9829", "Polar Stereographic (variant B)")]
    public class PolarStereographicBProjection : PolarStereographicProjection
    {
        /// <summary>
        /// Latitude of 1st standard parallel.
        /// </summary>
        private readonly Double latitudeOfStandardParallel;

        /// <summary>
        /// Longitude of origin.
        /// </summary>
        private readonly Double longitudeOfOrigin;

        /// <summary>
        /// false easting.
        /// </summary>
        private readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        private readonly Double falseNorthing;

        /// <summary>
        /// Scale factor at natural origin.
        /// </summary>
        private readonly Double scaleFactorAtNaturalOrigin;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarStereographicBProjection" /> class.
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
        public PolarStereographicBProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarStereographicBProjection" /> class.
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
        public PolarStereographicBProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.PolarStereographicBProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfStandardParallel);
            this.longitudeOfOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.operationAspect = (this.latitudeOfStandardParallel >= 0) ? OperationAspect.NorthPolar : OperationAspect.SouthPolar;

            Double mF, tF;

            mF = Math.Cos(this.latitudeOfStandardParallel) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfStandardParallel));

            if (this.operationAspect == OperationAspect.NorthPolar)
            {
                tF = Math.Tan(Math.PI / 4 - this.latitudeOfStandardParallel / 2) / Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)), this.Ellipsoid.Eccentricity / 2);
            }
            else
            {
                tF = Math.Tan(Math.PI / 4 + this.latitudeOfStandardParallel / 2) / Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfStandardParallel)), this.Ellipsoid.Eccentricity / 2);
            }

            this.scaleFactorAtNaturalOrigin = mF * Math.Sqrt(Math.Pow(1 + this.Ellipsoid.Eccentricity, 1 + this.Ellipsoid.Eccentricity) * Math.Pow(1 - this.Ellipsoid.Eccentricity, 1 - this.Ellipsoid.Eccentricity)) / (2 * tF);
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
            Double phi = 2 * this.Ellipsoid.SemiMajorAxis.BaseValue * this.scaleFactorAtNaturalOrigin * t / Math.Sqrt(Math.Pow(1 + this.Ellipsoid.Eccentricity, 1 + this.Ellipsoid.Eccentricity) * Math.Pow(1 - this.Ellipsoid.Eccentricity, 1 - this.Ellipsoid.Eccentricity));

            switch (this.operationAspect)
            {
                case OperationAspect.NorthPolar:
                    easting = this.falseEasting + phi * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
                    northing = this.falseNorthing - phi * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
                    break;
                case OperationAspect.SouthPolar:
                    easting = this.falseEasting + phi * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
                    northing = this.falseNorthing + phi * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfOrigin);
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
            Double x = coordinate.X - this.falseEasting;
            Double y = coordinate.Y - this.falseNorthing;
            Double phi = Math.Sqrt(x * x + y * y);
            Double t = phi * Math.Sqrt(Math.Pow(1 + this.Ellipsoid.Eccentricity, 1 + this.Ellipsoid.Eccentricity) * Math.Pow(1 - this.Ellipsoid.Eccentricity, 1 - this.Ellipsoid.Eccentricity)) / (2 * this.Ellipsoid.SemiMajorAxis.BaseValue * this.scaleFactorAtNaturalOrigin);

            Double ksi = 0, longitude = 0, latitude;

            switch (this.operationAspect)
            {
                case OperationAspect.NorthPolar:
                    ksi = Math.PI / 2 - 2 * Math.Atan(t);

                    if (coordinate.X == this.falseEasting)
                        longitude = this.longitudeOfOrigin;
                    else
                        longitude = this.longitudeOfOrigin + Math.Atan2(x, this.falseNorthing - coordinate.Y);
                    break;
                case OperationAspect.SouthPolar:
                    ksi = 2 * Math.Atan(t) - Math.PI / 2;

                    if (coordinate.X == this.falseEasting)
                        longitude = this.longitudeOfOrigin;
                    else
                        longitude = this.longitudeOfOrigin + Math.Atan2(x, y);
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
