// <copyright file="VerticalCoordinateReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a vertical coordinate reference system.
    /// </summary>
    /// <remarks>
    /// A 1D coordinate reference system used for recording heights or depths. Vertical CRSs make use of the direction of gravity to define the concept of height or depth, but the relationship with gravity may not be straightforward.
    /// By implication, ellipsoidal heights (h) cannot be captured in a vertical coordinate reference system. Ellipsoidal heights cannot exist independently, but only as inseparable part of a 3D coordinate tuple defined in a geodetic 3D coordinate reference system.
    /// </remarks>
    public class VerticalCoordinateReferenceSystem : CoordinateReferenceSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalCoordinateReferenceSystem" /> class.
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
        public VerticalCoordinateReferenceSystem(String identifier, String name, CoordinateSystem coordinateSystem, VerticalDatum datum, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, null, coordinateSystem, datum, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalCoordinateReferenceSystem" /> class.
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
        public VerticalCoordinateReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, CoordinateSystem coordinateSystem, VerticalDatum datum, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, scope, coordinateSystem, datum, areaOfUse)
        {
        }

        /// <summary>
        /// Gets the type of the reference system.
        /// </summary>
        /// <value>The type of the reference system.</value>
        public override ReferenceSystemType Type { get { return ReferenceSystemType.Vertical; } }

        /// <summary>
        /// Gets the datum of the coordinate reference system.
        /// </summary>
        /// <value>The datum of the coordinate reference system.</value>
        public new VerticalDatum Datum { get { return base.Datum as VerticalDatum; } }
    }
}
