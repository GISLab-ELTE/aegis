// <copyright file="GeocentricToTopocentricConversion.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a geocentric to topocentric conversion.
    /// </summary>
    [IdentifiedObject("EPSG::9836", "Geocentric/topocentric conversion")]
    public class GeocentricToTopocentricConversion : CoordinateConversion<Coordinate, Coordinate>
    {
        /// <summary>
        /// Geocentric X of topocentric origin.
        /// </summary>
        private readonly Double geocentricXOfTopocentricOrigin;

        /// <summary>
        /// Geocentric Y of topocentric origin.
        /// </summary>
        private readonly Double geocentricYOfTopocentricOrigin;

        /// <summary>
        /// Geocentric Z of topocentric origin.
        /// </summary>
        private readonly Double geocentricZOfTopocentricOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double sinLamda0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double cosLamda0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double sinFi0;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double cosFi0;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocentricToTopocentricConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public GeocentricToTopocentricConversion(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : this(identifier, name, null, null, parameters, ellipsoid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocentricToTopocentricConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public GeocentricToTopocentricConversion(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.GeocentricToTopocentricConversion, parameters)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid));

            this.geocentricXOfTopocentricOrigin = this.GetParameterValue(CoordinateOperationParameters.GeocenticXOfTopocentricOrigin);
            this.geocentricYOfTopocentricOrigin = this.GetParameterValue(CoordinateOperationParameters.GeocenticYOfTopocentricOrigin);
            this.geocentricZOfTopocentricOrigin = this.GetParameterValue(CoordinateOperationParameters.GeocenticZOfTopocentricOrigin);

            GeoCoordinate geographicOrigin = this.ComputeGeographicOrigin(new Coordinate(this.geocentricXOfTopocentricOrigin, this.geocentricYOfTopocentricOrigin, this.geocentricZOfTopocentricOrigin), ellipsoid);

            this.sinLamda0 = Math.Sin(geographicOrigin.Longitude.BaseValue);
            this.cosLamda0 = Math.Cos(geographicOrigin.Longitude.BaseValue);

            this.sinFi0 = Math.Sin(geographicOrigin.Latitude.BaseValue);
            this.cosFi0 = Math.Cos(geographicOrigin.Latitude.BaseValue);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            Double dx = coordinate.X - this.geocentricXOfTopocentricOrigin;
            Double dy = coordinate.Y - this.geocentricYOfTopocentricOrigin;
            Double dz = coordinate.Z - this.geocentricZOfTopocentricOrigin;

            Double u = -1 * dx * this.sinLamda0 + dy * this.cosLamda0;
            Double v = -1 * dx * this.sinFi0 * this.cosLamda0 - dy * this.sinFi0 * this.sinLamda0 + dz * this.cosFi0;
            Double w = dx * this.cosFi0 * this.cosLamda0 + dy * this.cosFi0 * this.sinLamda0 + dz * this.sinFi0;
            return new Coordinate(u, v, w);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = this.geocentricXOfTopocentricOrigin - coordinate.X * this.sinLamda0 - coordinate.Y * this.sinFi0 * this.cosLamda0 + coordinate.Z * this.cosFi0 * this.cosLamda0;
            Double y = this.geocentricYOfTopocentricOrigin + coordinate.X * this.cosLamda0 - coordinate.Y * this.sinFi0 * this.sinLamda0 + coordinate.Z * this.cosFi0 * this.sinLamda0;
            Double z = this.geocentricZOfTopocentricOrigin + coordinate.Y * this.cosFi0 + coordinate.Z * this.sinFi0;

            return new Coordinate(x, y, z);
        }

        /// <summary>
        /// Computes the geographic origin.
        /// </summary>
        /// <param name="geocentricOrigin">The geocentric origin.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The geographic origin.</returns>
        private GeoCoordinate ComputeGeographicOrigin(Coordinate geocentricOrigin, Ellipsoid ellipsoid)
        {
            Double p = Math.Sqrt(geocentricOrigin.X * geocentricOrigin.X + geocentricOrigin.Y * geocentricOrigin.Y);
            Double q = Math.Atan(geocentricOrigin.Z * ellipsoid.SemiMajorAxis.Value / p / ellipsoid.SemiMinorAxis.Value);
            Double phi = Math.Atan((geocentricOrigin.Z + ellipsoid.SecondEccentricity * ellipsoid.SecondEccentricity * ellipsoid.SemiMinorAxis.Value * Calculator.Sin3(q)) / (p - ellipsoid.EccentricitySquare * ellipsoid.SemiMajorAxis.Value * Calculator.Cos3(q)));
            Double nu = ellipsoid.RadiusOfPrimeVerticalCurvature(phi);
            Double lambda = Math.Atan(geocentricOrigin.Y / geocentricOrigin.X);
            Double height = geocentricOrigin.X * Calculator.Sec(lambda) * Calculator.Sec(phi) - nu;

            return new GeoCoordinate(phi, lambda, height);
        }
    }
}
