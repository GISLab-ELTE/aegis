// <copyright file="EquidistantCylindricalProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    using ELTE.AEGIS.Numerics.Integral;

    /// <summary>
    /// Represents an Equidistant Cylindrical Projection.
    /// </summary>
    [IdentifiedObject("EPSG::1028", "Equidistant Cylindrical")]
    public class EquidistantCylindricalProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of 1st standard parallel.
        /// </summary>
        private readonly Double latitudeOf1stStadardParallel;

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
        private readonly Double nu1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquidistantCylindricalProjection" /> class.
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
        public EquidistantCylindricalProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, CoordinateOperationMethods.EquidistantCylindricalProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquidistantCylindricalProjection" /> class.
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
        public EquidistantCylindricalProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.EquidistantCylindricalProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquidistantCylindricalProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected EquidistantCylindricalProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOf1stStadardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOf1stStandardParallel);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.nu1 = this.Ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOf1stStadardParallel));
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double easting, northing;

            if (this.Ellipsoid.IsSphere)
            {
                easting = this.falseEasting + this.Ellipsoid.SemiMajorAxis.Value * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin) * Math.Cos(this.latitudeOf1stStadardParallel);
                northing = this.falseNorthing + this.Ellipsoid.SemiMajorAxis.Value * coordinate.Latitude.BaseValue;
            }
            else
            {
                Double latitude = coordinate.Latitude.BaseValue;
                Double m = this.Ellipsoid.SemiMajorAxis.Value * (1 - this.Ellipsoid.EccentricitySquare) * SimpsonsMethod.ComputeIntegral(phi => Math.Pow(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(phi), -1.5), 0, latitude, 100);

                easting = this.falseEasting + this.nu1 * Math.Cos(this.latitudeOf1stStadardParallel) * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                northing = this.falseNorthing + m;
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
            Double phi, lambda;

            if (this.Ellipsoid.IsSphere)
            {
                phi = (coordinate.Y - this.falseNorthing) / this.Ellipsoid.SemiMajorAxis.Value;
                lambda = this.longitudeOfNaturalOrigin + (coordinate.X - this.falseEasting) / this.Ellipsoid.SemiMajorAxis.Value / Math.Cos(this.latitudeOf1stStadardParallel);
            }
            else
            {
                Double x = coordinate.X - this.falseEasting;
                Double y = coordinate.Y - this.falseNorthing;

                Double mu = y / (this.Ellipsoid.SemiMajorAxis.Value * (1
                    - 1d / 4 * this.Ellipsoid.EccentricitySquare
                    - 3d / 64 * Math.Pow(this.Ellipsoid.Eccentricity, 4)
                    - 5d / 256 * Math.Pow(this.Ellipsoid.Eccentricity, 6)
                    - 175d / 16384 * Math.Pow(this.Ellipsoid.Eccentricity, 8)
                    - 441d / 65536 * Math.Pow(this.Ellipsoid.Eccentricity, 10)
                    - 4851d / 1048576 * Math.Pow(this.Ellipsoid.Eccentricity, 12)
                    - 14157d / 4194304 * Math.Pow(this.Ellipsoid.Eccentricity, 14)));
                Double n = (1 - Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare)) / (1 + Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare));

                lambda = this.longitudeOfNaturalOrigin + x / (this.nu1 * Math.Cos(this.latitudeOf1stStadardParallel));
                phi = mu
                    + (3d / 2 * n - 27d / 32 * Math.Pow(n, 3) + 269d / 512 * Math.Pow(n, 5) - 6607d / 24576 * Math.Pow(n, 7)) * Math.Sin(2 * mu)
                    + (21d / 16 * Math.Pow(n, 2) - 55d / 32 * Math.Pow(n, 4) + 6759d / 4096 * Math.Pow(n, 6)) * Math.Sin(4 * mu)
                    + (151d / 96 * Math.Pow(n, 3) - 417d / 128 * Math.Pow(n, 5) + 87963d / 20480 * Math.Pow(n, 7)) * Math.Sin(6 * mu)
                    + (1097d / 512 * Math.Pow(n, 4) - 15543d / 2560 * Math.Pow(n, 6)) * Math.Sin(8 * mu)
                    + (8011d / 2560 * Math.Pow(n, 5) - 69119d / 6144 * Math.Pow(n, 7)) * Math.Sin(10 * mu)
                    + (293393d / 61440 * Math.Pow(n, 6)) * Math.Sin(12 * mu)
                    + (6845701d / 860160 * Math.Pow(n, 7)) * Math.Sin(14 * mu);
            }

            return new GeoCoordinate(phi, lambda);
        }
    }
}
