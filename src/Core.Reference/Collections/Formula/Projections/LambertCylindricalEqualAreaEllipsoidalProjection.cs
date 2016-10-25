// <copyright file="LambertCylindricalEqualAreaEllipsoidalProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents the Lambert Cylindrical Equal Area (ellipsoidal case) projection.
    /// </summary>
    [IdentifiedObject("EPSG::9835", "Lambert Cylindrical Equal Area (ellipsoidal case)")]
    public class LambertCylindricalEqualAreaEllipsoidalProjection : CoordinateProjection
    {
        #region Protected fields

        /// <summary>
        /// False easting.
        /// </summary>
        protected readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        protected readonly Double falseNorthing;

        /// <summary>
        /// Latitude of the 1st standard parallel.
        /// </summary>
        protected readonly Double latitudeOf1stStandardParallel;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        protected readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// Projection constant.
        /// </summary>
        protected readonly Double k0;

        /// <summary>
        /// Projection constant.
        /// </summary>
        protected readonly Double qP;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaEllipsoidalProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public LambertCylindricalEqualAreaEllipsoidalProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, CoordinateOperationMethods.LambertCylindricalEqualAreaEllipsoidalProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaEllipsoidalProjection" /> class.
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
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public LambertCylindricalEqualAreaEllipsoidalProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertCylindricalEqualAreaEllipsoidalProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaEllipsoidalProjection" /> class.
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
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected LambertCylindricalEqualAreaEllipsoidalProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.latitudeOf1stStandardParallel = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOf1stStandardParallel);

            this.k0 = Math.Cos(this.latitudeOf1stStandardParallel) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(this.latitudeOf1stStandardParallel), 2));
            this.qP = (1 - this.Ellipsoid.EccentricitySquare) *
                  (Math.Sin(Math.PI / 2) / (1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(Math.PI / 2), 2)) - (1 / (2 * this.Ellipsoid.Eccentricity)) *
                   Math.Log((1 - this.Ellipsoid.Eccentricity * Math.Sin(Math.PI / 2)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(Math.PI / 2))));
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            // source: John P. Snyder: Map Projections, page 81

            Double easting, northing;

            Double q = (1 - this.Ellipsoid.EccentricitySquare) *
                       (Math.Sin(coordinate.Latitude.BaseValue) / (1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(coordinate.Latitude.BaseValue), 2)) - (1 / (2 * this.Ellipsoid.Eccentricity)) *
                        Math.Log((1 - this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue))));

            easting = this.falseEasting + this.Ellipsoid.SemiMajorAxis.Value * this.k0 * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
            northing = this.falseNorthing + this.Ellipsoid.SemiMajorAxis.Value * q / (2 * this.k0);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: John P. Snyder: Map Projections, page 81

            Double lambda = this.longitudeOfNaturalOrigin + coordinate.X / (this.Ellipsoid.SemiMajorAxis.Value * this.k0);

            Double beta = Math.Asin(2 * coordinate.Y * this.k0 / (this.Ellipsoid.SemiMajorAxis.Value * this.qP));
            Double betaC = Math.Atan(Math.Tan(beta) / Math.Cos(lambda - this.longitudeOfNaturalOrigin));
            Double qC = this.qP * Math.Sin(betaC);

            Double phiC = 0;
            Double phi = 0;

            do
            {
                phi = phiC;
                phiC = phiC + Math.Pow(1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(phiC), 2), 2) / (2 * Math.Cos(phiC)) *
                              (qC / (1 - this.Ellipsoid.EccentricitySquare) - Math.Sin(phiC) / (1 - this.Ellipsoid.EccentricitySquare * Math.Pow(Math.Sin(phiC), 2)) +
                               1 / (2 * this.Ellipsoid.Eccentricity) * Math.Log((1 - this.Ellipsoid.Eccentricity * Math.Sin(phiC)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(phiC))));
            }
            while (Math.Abs(phiC - phi) > 0.000001);

            phi = phiC;

            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}