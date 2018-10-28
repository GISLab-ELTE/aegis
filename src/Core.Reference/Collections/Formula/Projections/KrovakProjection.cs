// <copyright file="KrovakProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Krovak Projection.
    /// </summary>
    [IdentifiedObject("AEGIS::9819", "Krovak Projection")]
    public class KrovakProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of projection centre.
        /// </summary>
        protected readonly Double latitudeOfProjectionCentre;

        /// <summary>
        /// Longitude of origin.
        /// </summary>
        protected readonly Double longitudeOfOrigin;

        /// <summary>
        /// Co-latitude of cone axis.
        /// </summary>
        protected readonly Double coLatitudeOfConeAxis;

        /// <summary>
        /// Latitude of pseudo standard parallel.
        /// </summary>
        protected readonly Double latitudeOfPseudoStandardParallel;

        /// <summary>
        /// Scale factor on pseudo standard parallel.
        /// </summary>
        protected readonly Double scaleFactorOnPseudoStandardParallel;

        /// <summary>
        /// False easting.
        /// </summary>
        protected readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        protected readonly Double falseNorthing;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double A;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double B;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double gammaO;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double tO;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double n;

        /// <summary>
        /// Operation constant.
        /// </summary>
        protected readonly Double rO;

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public KrovakProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public KrovakProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.KrovakProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakProjection" /> class.
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
        protected KrovakProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfProjectionCentre = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfProjectionCentre);
            this.longitudeOfOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfOrigin);
            this.coLatitudeOfConeAxis = this.GetParameterBaseValue(CoordinateOperationParameters.CoLatitudeOfConeAxis);
            this.latitudeOfPseudoStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfPseudoStandardParallel);
            this.scaleFactorOnPseudoStandardParallel = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.A = this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Pow(1 - this.Ellipsoid.EccentricitySquare, 0.5) / (1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfProjectionCentre));
            this.B = Math.Pow(1 + ((this.Ellipsoid.EccentricitySquare * Calculator.Cos4(this.latitudeOfProjectionCentre)) / (1 - this.Ellipsoid.EccentricitySquare)), 0.5);
            this.gammaO = Math.Asin(Math.Sin(this.latitudeOfProjectionCentre) / this.B);
            this.tO = Math.Tan(Math.PI / 4 + this.gammaO / 2) * Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfProjectionCentre)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfProjectionCentre)), this.Ellipsoid.Eccentricity * this.B / 2) / Math.Pow(Math.Tan(Math.PI / 4 + this.latitudeOfProjectionCentre / 2), this.B);
            this.n = Math.Sin(this.latitudeOfPseudoStandardParallel);
            this.rO = this.scaleFactorOnPseudoStandardParallel * this.A / Math.Tan(this.latitudeOfPseudoStandardParallel);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double u = 2 * (Math.Atan(this.tO * Math.Pow(Math.Tan(coordinate.Latitude.BaseValue / 2 + Math.PI / 4), this.B) / Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)), this.Ellipsoid.Eccentricity * this.B / 2)) - Math.PI / 4);
            Double v = this.B * (this.longitudeOfOrigin - coordinate.Longitude.BaseValue);
            Double t = Math.Asin(Math.Cos(this.coLatitudeOfConeAxis) * Math.Sin(u) + Math.Sin(this.coLatitudeOfConeAxis) * Math.Cos(u) * Math.Cos(v));
            Double d = Math.Asin(Math.Cos(u) * Math.Sin(v) / Math.Cos(t));
            Double theta = this.n * d;
            Double r = this.rO * Math.Pow(Math.Tan((Math.PI / 4) + (this.latitudeOfPseudoStandardParallel / 2)), this.n) / Math.Pow(Math.Tan(t / 2 + Math.PI / 4), this.n);
            Double xP = r * Math.Cos(theta);
            Double yP = r * Math.Sin(theta);

            return new Coordinate(yP + this.falseEasting, xP + this.falseNorthing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double xP = coordinate.Y - this.falseNorthing;
            Double yP = coordinate.X - this.falseEasting;
            Double r = Math.Pow(Math.Pow(xP, 2) + Math.Pow(yP, 2), 0.5);
            Double teta = Math.Atan(yP / xP);
            Double d = teta / Math.Sin(this.latitudeOfPseudoStandardParallel);
            Double t = 2 * (Math.Atan(Math.Pow(this.rO / r, 1 / this.n) * Math.Tan(Math.PI / 4 + this.latitudeOfPseudoStandardParallel / 2)) - Math.PI / 4);
            Double u = Math.Asin(Math.Cos(this.coLatitudeOfConeAxis) * Math.Sin(t) - Math.Sin(this.coLatitudeOfConeAxis) * Math.Cos(t) * Math.Cos(d));
            Double v = Math.Asin(Math.Cos(t) * Math.Sin(d) / Math.Cos(u));

            Double lambda = this.longitudeOfOrigin - (v / this.B);

            Double phiJ;
            Double phiJ1 = u;

            for (Int32 i = 0; i < 3; i++)
            {
                phiJ = 2 * (Math.Atan(Math.Pow(this.tO, -1 / this.B) * Math.Pow(Math.Tan(u / 2 + Math.PI / 4), 1 / this.B) * Math.Pow((1 + this.Ellipsoid.Eccentricity * Math.Sin(phiJ1)) / (1 - this.Ellipsoid.Eccentricity * Math.Sin(phiJ1)), this.Ellipsoid.Eccentricity / 2)) - Math.PI / 4);
                phiJ1 = phiJ;
            }

            return new GeoCoordinate(phiJ1, lambda);
        }
    }
}