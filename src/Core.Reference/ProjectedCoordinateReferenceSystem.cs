// <copyright file="ProjectedCoordinateReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a projected coordinate reference system.
    /// </summary>
    /// <remarks>
    /// A derived coordinate reference system which has a geodetic coordinate reference system as its base CRS and is converted using a map projection.
    /// </remarks>
    public class ProjectedCoordinateReferenceSystem : CoordinateReferenceSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectedCoordinateReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="baseReferenceSystem">The base reference system.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The base reference system is null.
        /// or
        /// The coordinate system is null.
        /// or
        /// The projection is null.
        /// </exception>
        public ProjectedCoordinateReferenceSystem(String identifier, String name, GeographicCoordinateReferenceSystem baseReferenceSystem, CoordinateSystem coordinateSystem, AreaOfUse areaOfUse, CoordinateProjection projection)
            : this(identifier, name, null, null, null, baseReferenceSystem, coordinateSystem, areaOfUse, projection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectedCoordinateReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="baseReferenceSystem">The base reference system.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The coordinate system is null.
        /// or
        /// The datum is null.
        /// or
        /// The area of use is null.
        /// or
        /// The base reference system is null.
        /// or
        /// The projection is null.
        /// </exception>
        public ProjectedCoordinateReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, GeographicCoordinateReferenceSystem baseReferenceSystem, CoordinateSystem coordinateSystem, AreaOfUse areaOfUse, CoordinateProjection projection)
            : base(identifier, name, remarks, aliases, scope, coordinateSystem, baseReferenceSystem != null ? baseReferenceSystem.Datum : null, areaOfUse)
        {
            if (baseReferenceSystem == null)
                throw new ArgumentNullException(nameof(baseReferenceSystem), ReferenceMessages.BaseReferenceSystemIsNull);
            if (projection == null)
                throw new ArgumentNullException(nameof(projection), ReferenceMessages.ProjectionIsNull);

            this.BaseReferenceSystem = baseReferenceSystem;
            this.Projection = projection;
        }

        /// <summary>
        /// Gets the type of the reference system.
        /// </summary>
        /// <value>The type of the reference system.</value>
        public override ReferenceSystemType Type { get { return ReferenceSystemType.Projected; } }

        /// <summary>
        /// Gets the datum of the coordinate reference system.
        /// </summary>
        /// <value>The datum of the coordinate reference system.</value>
        public new GeodeticDatum Datum { get { return base.Datum as GeodeticDatum; } }

        /// <summary>
        /// Gets the base geographic coordinate reference system.
        /// </summary>
        /// <value>The base geographic coordinate reference system </value>
        public GeographicCoordinateReferenceSystem BaseReferenceSystem { get; private set; }

        /// <summary>
        /// Gets the coordinate projection used by the reference system.
        /// </summary>
        /// <value>The coordinate projection used by the reference system.</value>
        public CoordinateProjection Projection { get; private set; }
    }
}
