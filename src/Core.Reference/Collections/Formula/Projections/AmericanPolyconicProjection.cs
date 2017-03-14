// <copyright file="AmericanPolyconicProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Numerics;

    /// <summary>
    /// Represents an American Polyconic projection.
    /// </summary>
    [IdentifiedObject("EPSG::9818", "American Polyconic")]
    public class AmericanPolyconicProjection : CoordinateProjection
    {
        /// <summary>
        /// Represents the limit for iterative computation. This field is constant.
        /// </summary>
        private const Int32 IterationLimit = 1000;

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
        private readonly Double m0;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e2;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e4;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e6;

        /// <summary>
        /// Ellipsoid eccentricity.
        /// </summary>
        private readonly Double e8;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmericanPolyconicProjection" /> class.
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
        public AmericanPolyconicProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmericanPolyconicProjection" /> class.
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
        public AmericanPolyconicProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.AmericanPolyconicProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.e2 = this.Ellipsoid.EccentricitySquare;
            this.e4 = Math.Pow(this.Ellipsoid.Eccentricity, 4);
            this.e6 = Math.Pow(this.Ellipsoid.Eccentricity, 6);
            this.e8 = Math.Pow(this.Ellipsoid.Eccentricity, 8);
            this.m0 = this.ComputeM(this.latitudeOfNaturalOrigin);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            if (Math.Abs(coordinate.Latitude.BaseValue - this.latitudeOfNaturalOrigin) > Math.PI / 2)
                return Coordinate.Undefined;

            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double bL = (lambda - this.longitudeOfNaturalOrigin) * Math.Sin(phi);
            Double bM = this.ComputeM(phi);

            Double easting, northing;
            if (coordinate.Latitude.BaseValue == 0)
            {
                easting = this.falseEasting + this.Ellipsoid.SemiMajorAxis.BaseValue * (lambda - this.longitudeOfNaturalOrigin);
                northing = this.falseNorthing - this.m0;
            }
            else
            {
                easting = this.falseEasting + this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Calculator.Cot(phi) * Math.Sin(bL);
                northing = this.falseNorthing + bM - this.m0 + this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Calculator.Cot(phi) * (1 - Math.Cos(bL));
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

            if (this.m0 == (coordinate.Y - this.falseNorthing))
            {
                phi = 0;
                lambda = this.longitudeOfNaturalOrigin + (coordinate.X - this.falseEasting) / this.Ellipsoid.SemiMajorAxis.BaseValue;
            }
            else
            {
                Double bA = (this.m0 + coordinate.Y - this.falseNorthing) / this.Ellipsoid.SemiMajorAxis.BaseValue;
                Double x = coordinate.X - this.falseEasting;
                Double bB = Math.Pow(bA, 2) + x * x / (this.Ellipsoid.SemiMajorAxis.BaseValue * this.Ellipsoid.SemiMajorAxis.BaseValue);

                Double bC, bH, bM, bJ, phi1, phi2 = bA;

                Int32 iteration = 1;
                do
                {
                    phi1 = phi2;
                    bC = Math.Sqrt(1 - this.e2 * Calculator.Sin2(phi1)) * Math.Tan(phi1);
                    bM = this.ComputeM(phi1);

                    // collection of H: Snyder, J.P.: Map Projections - A Working Manual, page 130

                    bH = 1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256
                           - 2 * (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Cos(2 * phi1)
                           + 4 * (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Cos(4 * phi1)
                           - 6 * (35 * this.e6 / 3072) * Math.Cos(6 * phi1);
                    bJ = bM / this.Ellipsoid.SemiMajorAxis.BaseValue;

                    phi2 = phi1 - (bA * (bC * bJ + 1) - bJ - bC * (bJ * bJ + bB) / 2) /
                                  (this.e2 * Math.Sin(2 * phi1) * (bJ * bJ + bB - 2 * bA * bJ) / (4 * bC) + (bA - bJ) * (bC * bH - 2 / Math.Sin(2 * phi1)) - bH);
                    iteration++;
                }
                while (iteration < IterationLimit && Math.Abs(phi2 - phi1) > 0.001);

                // check for convergence
                if (iteration == IterationLimit)
                    return GeoCoordinate.Undefined;

                phi = phi2;
                lambda = this.longitudeOfNaturalOrigin + Math.Asin((coordinate.X - this.falseEasting) * bC / this.Ellipsoid.SemiMajorAxis.BaseValue) / Math.Sin(phi);
            }

            return new GeoCoordinate(phi, lambda);
        }

        /// <summary>
        /// Computes the M value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The M Value.</returns>
        protected Double ComputeM(Double latitude)
        {
            return this.Ellipsoid.SemiMajorAxis.BaseValue * ((1 - this.e2 / 4 - 3 * this.e4 / 64 - 5 * this.e6 / 256) * latitude -
                                                         (3 * this.e2 / 8 + 3 * this.e4 / 32 + 45 * this.e6 / 1024) * Math.Sin(2 * latitude) +
                                                         (15 * this.e4 / 256 + 45 * this.e6 / 1024) * Math.Sin(4 * latitude) -
                                                         35 * this.e6 / 3072 * Math.Sin(6 * latitude));
        }
    }
}
