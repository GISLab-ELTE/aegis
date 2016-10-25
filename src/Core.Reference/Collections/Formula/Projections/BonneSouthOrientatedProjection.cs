// <copyright file="BonneSouthOrientatedProjection.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a Bonne South Orientated projection.
    /// </summary>
    [IdentifiedObject("EPSG::9828", "Bonne South Orientated")]
    public class BonneSouthOrientatedProjection : BonneProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneSouthOrientatedProjection" /> class.
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
        public BonneSouthOrientatedProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneSouthOrientatedProjection" /> class.
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
        public BonneSouthOrientatedProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.BonneSouthOrientated, parameters, ellipsoid, areaOfUse)
        {
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
            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double m = this.Computem(phi);
            Double m0 = this.Computem(this.latitudeOfNaturalOrigin);
            Double bM = this.ComputeM(phi);
            Double bM0 = this.ComputeM(this.latitudeOfNaturalOrigin);

            Double rho = this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin) + bM0 - bM;
            Double bT = this.Ellipsoid.SemiMajorAxis.Value * m * (lambda - this.longitudeOfNaturalOrigin) / rho;

            Double westing = this.falseEasting - (rho * Math.Sin(bT));
            Double southing = this.falseNorthing - (this.Ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(this.latitudeOfNaturalOrigin) - rho * Math.Cos(bT));

            return new Coordinate(westing, southing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = this.falseEasting - coordinate.X;
            Double y = this.falseNorthing - coordinate.Y;

            return this.ComputeReverseInternal(x, y);
        }

        #endregion
    }
}
