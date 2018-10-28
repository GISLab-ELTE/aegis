// <copyright file="LambertCylindricalEqualAreaSphericalProjection.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents the Lambert Cylindrical Equal Area (spherical case) projection.
    /// </summary>
    [IdentifiedObject("EPSG::9834", "Lambert Cylindrical Equal Area (spherical case)")]
    public class LambertCylindricalEqualAreaSphericalProjection : LambertCylindricalEqualAreaEllipsoidalProjection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaSphericalProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public LambertCylindricalEqualAreaSphericalProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaSphericalProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public LambertCylindricalEqualAreaSphericalProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertCylindricalEqualAreaSphericalProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double easting = this.falseEasting +
                             this.Ellipsoid.SemiMajorAxis.Value * (coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin) *
                             Math.Cos(this.latitudeOf1stStandardParallel);
            Double northing = this.falseNorthing +
                              this.Ellipsoid.SemiMajorAxis.Value * Math.Sin(coordinate.Latitude.BaseValue) /
                              Math.Cos(this.latitudeOf1stStandardParallel);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double phi = Math.Asin((coordinate.Y / this.Ellipsoid.SemiMajorAxis.Value) * Math.Cos(this.latitudeOf1stStandardParallel));
            Double lambda = coordinate.X / (this.Ellipsoid.SemiMajorAxis.Value * Math.Cos(this.latitudeOf1stStandardParallel)) +
                            this.longitudeOfNaturalOrigin;

            return new GeoCoordinate(phi, lambda);
        }
    }
}