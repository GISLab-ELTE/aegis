// <copyright file="Geographic3DTo2DConversion.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a geographic 3D to 2D conversion.
    /// </summary>
    [IdentifiedObject("EPSG::9659", "Geographic3D to 2D conversion")]
    public class Geographic3DTo2DConversion : CoordinateConversion<GeoCoordinate, GeoCoordinate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Geographic3DTo2DConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public Geographic3DTo2DConversion(String identifier, String name)
            : this(identifier, name, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Geographic3DTo2DConversion" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public Geographic3DTo2DConversion(String identifier, String name, String remarks, String[] aliases)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.Geographic3DTo2DConversion, null)
        {
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeForward(GeoCoordinate coordinate)
        {
            return new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(GeoCoordinate coordinate)
        {
            return coordinate;
        }
    }
}
