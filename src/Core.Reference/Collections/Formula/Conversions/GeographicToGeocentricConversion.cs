// <copyright file="GeographicToGeocentricConversion.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Numerics;

    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a geographic to geocentric conversion.
    /// </summary>
    [IdentifiedObject("EPSG::9602", "Geographic/geocentric conversion")]
    public class GeographicToGeocentricConversion : CoordinateConversion<GeoCoordinate, Coordinate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicToGeocentricConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public GeographicToGeocentricConversion(String identifier, String name, Ellipsoid ellipsoid)
            : this(identifier, name, null, null, ellipsoid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicToGeocentricConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public GeographicToGeocentricConversion(String identifier, String name, String remarks, String[] aliases, Ellipsoid ellipsoid)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.GeographicToGeocentricConversion, null)
        {
            this.Ellipsoid = ellipsoid ?? throw new ArgumentNullException(nameof(ellipsoid));
        }

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid used by the operation.</value>
        public Ellipsoid Ellipsoid { get; }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double lambda = coordinate.Longitude.BaseValue;
            Double phi = coordinate.Latitude.BaseValue;
            Double h = coordinate.Height.BaseValue;
            Double nu = this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi);
            Double x = (nu + h) * Math.Cos(phi) * Math.Cos(lambda);
            Double y = (nu + h) * Math.Cos(phi) * Math.Sin(lambda);
            Double z = ((1 - this.Ellipsoid.EccentricitySquare) * nu + h) * Math.Sin(phi);

            return new Coordinate(x, y, z);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // Bowring's formula

            Double p = Math.Sqrt(coordinate.X * coordinate.X + coordinate.Y * coordinate.Y);
            Double q = Math.Atan(coordinate.Z * this.Ellipsoid.SemiMajorAxis.Value / p / this.Ellipsoid.SemiMinorAxis.Value);
            Double phi = Math.Atan((coordinate.Z + this.Ellipsoid.SecondEccentricitySquare * this.Ellipsoid.SemiMinorAxis.Value * Calculator.Sin3(q)) / (p - this.Ellipsoid.EccentricitySquare * this.Ellipsoid.SemiMajorAxis.Value * Calculator.Cos3(q)));
            Double nu = this.Ellipsoid.RadiusOfPrimeVerticalCurvature(phi);
            Double lambda = Math.Atan(coordinate.Y / coordinate.X);
            Double height = coordinate.X * Calculator.Sec(lambda) * Calculator.Sec(phi) - nu;

            return new GeoCoordinate(phi, lambda, height);
        }
    }
}
