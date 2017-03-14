// <copyright file="LabordeObliqueMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Numerics;
    using AEGIS.Numerics;

    /// <summary>
    /// Represents a Laborde Oblique Mercator projection.
    /// </summary>
    [IdentifiedObject("EPSG::9813", "Laborde Oblique Mercator")]
    public class LabordeObliqueMercatorProjection : ObliqueMercatorProjection
    {
        /// <summary>
        /// Epsilon (for terminating the iteration).
        /// </summary>
        private const Double Epsilon = 1E-11;

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
        private readonly Double c;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double fiS;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double r;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabordeObliqueMercatorProjection" /> class.
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
        public LabordeObliqueMercatorProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabordeObliqueMercatorProjection" /> class.
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
        public LabordeObliqueMercatorProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.LabordeObliqueMercatorProjection, parameters, ellipsoid, areaOfUse)
        {
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.fiS = Math.Asin(Math.Sin(this.latitudeOfProjectionCentre) / this.b);
            this.r = ellipsoid.SemiMajorAxis.BaseValue * this.scaleFactorOnInitialLine * (Math.Sqrt(1 - ellipsoid.EccentricitySquare) / (1 - ellipsoid.EccentricitySquare * Numerics.Calculator.Sin2(this.latitudeOfProjectionCentre)));
            this.c = Math.Log(Math.Tan(Math.PI / 4 + this.fiS / 2)) -
                 this.b * Math.Log(Math.Tan(Math.PI / 4 + this.latitudeOfProjectionCentre / 2) * Math.Pow((1 - ellipsoid.Eccentricity * Math.Sin(this.latitudeOfProjectionCentre)) / (1 + ellipsoid.Eccentricity * Math.Sin(this.latitudeOfProjectionCentre)), ellipsoid.Eccentricity / 2));
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double l = this.b * (coordinate.Longitude.BaseValue - this.longitudeOfProjectionCentre);
            Double q = this.c + this.b * Math.Log(Math.Tan(Math.PI / 4 + coordinate.Latitude.BaseValue / 2) * Math.Pow((1 - this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)), this.Ellipsoid.Eccentricity / 2));
            Double p = 2 * Math.Atan(Math.Pow(Math.E, q)) - Math.PI / 2;
            Double u = Math.Cos(p) * Math.Cos(l) * Math.Cos(this.fiS) + Math.Sin(p) * Math.Sin(this.fiS);
            Double v = Math.Cos(p) * Math.Cos(l) * Math.Sin(this.fiS) - Math.Sin(p) * Math.Cos(this.fiS);
            Double w = Math.Cos(p) * Math.Sin(l);
            Double d = Math.Sqrt(u * u + v * v);

            Double l2 = 0.0;
            Double p2 = 0.0;
            if (Math.Abs(d) > Epsilon)
            {
                l2 = 2 * Math.Atan(v / (u + d));
                p2 = Math.Atan(w / d);
            }
            else
            {
                p2 = Math.Sign(w) * Math.PI / 2;
            }

            Complex h = new Complex(-1 * l2, Math.Log(Math.Tan(Math.PI / 4 + p2 / 2)));
            Complex g = new Complex((1 - Math.Cos(2 * this.azimuthOfInitialLine)) / 12, Math.Sin(2 * this.azimuthOfInitialLine) / 12);

            Complex temp = h + (g * (h * h * h));

            Double easting = this.falseEasting + this.r * temp.Imaginary;
            Double northing = this.falseNorthing + this.r * temp.Real;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Complex g = new Complex((1 - Math.Cos(2 * this.azimuthOfInitialLine)) / 12, Math.Sin(2 * this.azimuthOfInitialLine) / 12);
            Complex h0 = new Complex((coordinate.Y - this.falseNorthing) / this.r, (coordinate.X - this.falseEasting) / this.r);
            Complex h1 = h0 / (h0 + g * h0 * h0 * h0);

            while (Math.Abs((h0 - h1 - g * (h1 * h1 * h1)).Real) > Epsilon)
            {
                h1 = (h0 + 2 * g * (h1 * h1 * h1)) / (3 * g * (h1 * h1) + 1);
            }

            Double l1 = -1 * h1.Real;
            Double p1 = 2 * Math.Atan(Math.Pow(Math.E, h1.Imaginary)) - Math.PI / 2;
            Double u1 = Math.Cos(p1) * Math.Cos(l1) * Math.Cos(this.fiS) + Math.Cos(p1) * Math.Sin(l1) * Math.Sin(this.fiS);

            Double v1 = Math.Sin(p1);
            Double w1 = Math.Cos(p1) * Math.Cos(l1) * Math.Sin(this.fiS) - Math.Cos(p1) * Math.Sin(l1) * Math.Cos(this.fiS);
            Double d = Math.Sqrt(u1 * u1 + v1 * v1);

            Double l = 0;
            Double p = 0;
            if (Math.Abs(d) > Epsilon)
            {
                l = 2 * Math.Atan(v1 / (u1 + d));
                p = Math.Atan(w1 / d);
            }
            else
            {
                l = 0;
                p = Math.Sign(w1) * Math.PI / 2;
            }

            Double lambda = this.longitudeOfProjectionCentre + (l / this.b);
            Double q1 = (Math.Log(Math.Tan(Math.PI / 4 + p / 2)) - this.c) / this.b;

            Double phi = 2 * Math.Atan(Math.Pow(Math.E, q1)) - Math.PI / 2;
            Double phiPre = 0f;

            do
            {
                phiPre = phi;
                phi = 2 * Math.Atan(Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(phi)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(phi)), this.Ellipsoid.Eccentricity / 2) * Math.Pow(Math.E, q1)) - Math.PI / 2;
            }
            while (Math.Abs(phiPre - phi) > Epsilon);

            return new GeoCoordinate(phi, lambda);
        }
    }
}
