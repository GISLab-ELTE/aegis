// <copyright file="TransverseMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Transverse Mercator Projection.
    /// </summary>
    [IdentifiedObject("EPSG::9807", "Transverse Mercator")]
    public class TransverseMercatorProjection : CoordinateProjection
    {
        #region Private fields

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
        /// Scale factor at natural origin.
        /// </summary>
        private readonly Double scaleFactorAtNaturalOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double bB;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double[] h;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double[] invertedH;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double bMO;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseMercatorProjection" /> class.
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
        public TransverseMercatorProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseMercatorProjection" /> class.
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
        public TransverseMercatorProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.TransverseMercatorProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.scaleFactorAtNaturalOrigin = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin);

            Double n = this.Ellipsoid.Flattening / (2 - this.Ellipsoid.Flattening);
            Double nSquare = n * n;
            this.bB = this.Ellipsoid.SemiMajorAxis.BaseValue / (1 + n) * (1 + nSquare / 4 + Math.Pow(n, 4) / 64);
            this.h = new Double[]
            {
                n / 2 - 2 * nSquare / 3 + 5 * Math.Pow(n, 3) / 16 + 41 * Math.Pow(n, 4) / 180,
                13 * nSquare / 48 - 3 * Math.Pow(n, 3) / 5 + 437 * Math.Pow(n, 4) / 1440,
                17 * Math.Pow(n, 3) / 480 - 37 * Math.Pow(n, 4) / 840,
                4397 * Math.Pow(n, 4) / 161280
            };

            this.invertedH = new Double[]
            {
                n / 2 - 2 * nSquare / 3 + 37 * Math.Pow(n, 3) / 96 + 1 * Math.Pow(n, 4) / 360,
                1 * nSquare / 48 - 1 * Math.Pow(n, 3) / 15 + 557 * Math.Pow(n, 4) / 1440,
                61 * Math.Pow(n, 3) / 240 - 103 * Math.Pow(n, 4) / 140,
                49561 * Math.Pow(n, 4) / 161280
            };

            if (this.latitudeOfNaturalOrigin == 0)
            {
                this.bMO = 0;
            }
            else if (Math.Abs(this.latitudeOfNaturalOrigin - Math.PI / 2) <= 1E-10)
            {
                this.bMO = this.bB * Math.PI / 2;
            }
            else if (Math.Abs(this.latitudeOfNaturalOrigin + Math.PI / 2) <= 1E-10)
            {
                this.bMO = -this.bB * Math.PI / 2;
            }
            else
            {
                Double qO = Calculator.Asinh(Math.Tan(this.latitudeOfNaturalOrigin)) - this.Ellipsoid.Eccentricity * Calculator.Atanh(this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfNaturalOrigin));
                Double xiO0 = Math.Atan(Math.Sinh(qO));
                this.bMO = this.bB * (xiO0 + this.h[0] * Math.Sin(2 * xiO0) + this.h[1] * Math.Sin(4 * xiO0) + this.h[2] * Math.Sin(6 * xiO0) + this.h[3] * Math.Sin(8 * xiO0));
            }
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double q = Calculator.Asinh(Math.Tan(coordinate.Latitude.BaseValue)) - this.Ellipsoid.Eccentricity * Calculator.Atanh(this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue));
            Double beta = Math.Atan(Math.Sinh(q));
            Double eta0 = Calculator.Atanh(Math.Cos(beta) * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin));
            Double xi0 = Math.Asin(Math.Sin(beta) * Math.Cosh(eta0));

            Double eta = eta0 + this.h[0] * Math.Cos(2 * xi0) * Math.Sinh(2 * eta0) + this.h[1] * Math.Cos(4 * xi0) * Math.Sinh(4 * eta0) + this.h[2] * Math.Cos(6 * xi0) * Math.Sinh(6 * eta0) + this.h[3] * Math.Cos(8 * xi0) * Math.Sinh(8 * eta0);
            Double xi = xi0 + this.h[0] * Math.Sin(2 * xi0) * Math.Cosh(2 * eta0) + this.h[1] * Math.Sin(4 * xi0) * Math.Cosh(4 * eta0) + this.h[2] * Math.Sin(6 * xi0) * Math.Cosh(6 * eta0) + this.h[3] * Math.Sin(8 * xi0) * Math.Cosh(8 * eta0);

            return new Coordinate(this.falseEasting + this.scaleFactorAtNaturalOrigin * this.bB * eta, this.falseNorthing + this.scaleFactorAtNaturalOrigin * (this.bB * xi - this.bMO));
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double eta = (coordinate.X - this.falseEasting) / (this.bB * this.scaleFactorAtNaturalOrigin);
            Double xi = (coordinate.Y - this.falseNorthing + this.scaleFactorAtNaturalOrigin * this.bMO) / (this.bB * this.scaleFactorAtNaturalOrigin);

            Double xi0 = xi - this.invertedH[0] * Math.Sin(2 * xi) * Math.Cosh(2 * eta) - this.invertedH[1] * Math.Sin(4 * xi) * Math.Cosh(4 * eta) - this.invertedH[2] * Math.Sin(6 * xi) * Math.Cosh(6 * eta) - this.invertedH[3] * Math.Sin(8 * xi) * Math.Cosh(8 * eta);
            Double eta0 = eta - this.invertedH[0] * Math.Cos(2 * xi) * Math.Sinh(2 * eta) - this.invertedH[1] * Math.Cos(4 * xi) * Math.Sinh(4 * eta) - this.invertedH[2] * Math.Cos(6 * xi) * Math.Sinh(6 * eta) - this.invertedH[3] * Math.Sinh(8 * xi) * Math.Sinh(8 * eta);

            Double beta = Math.Asin(Math.Sin(xi0) / Math.Cosh(eta0));

            Double q1 = Calculator.Asinh(Math.Tan(beta));
            Double q2 = q1 + (this.Ellipsoid.Eccentricity * Calculator.Atanh(this.Ellipsoid.Eccentricity * Math.Tanh(q1)));

            for (Int32 iteration = 0; iteration < 1E4 && q2 < q1 && Math.Abs(q1 - q2) > 1E-4; iteration++)
            {
                q2 = q1 + (this.Ellipsoid.Eccentricity * Calculator.Atanh(this.Ellipsoid.Eccentricity * Math.Tanh(q2)));
            }

            return new GeoCoordinate(Math.Atan(Math.Sinh(q2)), this.longitudeOfNaturalOrigin + Math.Asin(Math.Tanh(eta0) / Math.Cos(beta)));
        }

        #endregion
    }
}
