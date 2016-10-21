// <copyright file="GeographicCoordinateReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a geodetic reference system.
    /// </summary>
    /// <remarks>
    /// A coordinate reference system associated with a geodetic datum.
    /// </remarks>
    public class GeographicCoordinateReferenceSystem : CoordinateReferenceSystem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicCoordinateReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="datum">The datum.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The coordinate system is null.
        /// or
        /// The datum is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public GeographicCoordinateReferenceSystem(String identifier, String name, CoordinateSystem coordinateSystem, GeodeticDatum datum, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, null, coordinateSystem, datum, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicCoordinateReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="datum">The datum.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The coordinate system is null.
        /// or
        /// The datum is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public GeographicCoordinateReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, CoordinateSystem coordinateSystem, GeodeticDatum datum, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, scope, coordinateSystem, datum, areaOfUse)
        {
        }

        #endregion

        #region Public ReferenceSystem properties

        /// <summary>
        /// Gets the type of the reference system.
        /// </summary>
        /// <value>The type of the reference system.</value>
        public override ReferenceSystemType Type { get { return (this.CoordinateSystem.Dimension == 3) ? ReferenceSystemType.Geographic3D : ReferenceSystemType.Geographic2D; } }

        #endregion

        #region Public CoordinateReferenceSystem Properties

        /// <summary>
        /// Gets the datum of the coordinate reference system.
        /// </summary>
        /// <value>The datum of the coordinate reference system.</value>
        public new GeodeticDatum Datum { get { return base.Datum as GeodeticDatum; } }

        #endregion
    }
}
