// <copyright file="GeodeticDatum.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    using System;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a geodetic datum.
    /// </summary>
    /// <remarks>
    /// A geodetic datum defines the location and precise orientation in three-dimensional space of a defined ellipsoid (or sphere) that approximates the shape of the earth, or of a Cartesian coordinate system centered in this ellipsoid (or sphere).
    /// </remarks>
    public class GeodeticDatum : Datum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeodeticDatum" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="anchorPoint">The anchor point.</param>
        /// <param name="realizationEpoch">The realization epoch.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="primeMeridian">The prime meridian.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The area of use is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The prime meridian is null.
        /// </exception>
        public GeodeticDatum(String identifier, String name, String anchorPoint, String realizationEpoch, AreaOfUse areaOfUse, Ellipsoid ellipsoid, Meridian primeMeridian)
            : this(identifier, name, null, null, anchorPoint, realizationEpoch, null, areaOfUse, ellipsoid, primeMeridian)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeodeticDatum" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="anchorPoint">The anchor point.</param>
        /// <param name="realizationEpoch">The realization epoch.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="primeMeridian">The prime meridian.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The area of use is null.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The prime meridian is null.
        /// </exception>
        public GeodeticDatum(String identifier, String name, String remarks, String[] aliases, String anchorPoint, String realizationEpoch, String scope, AreaOfUse areaOfUse, Ellipsoid ellipsoid, Meridian primeMeridian)
            : base(identifier, name, remarks, aliases, anchorPoint, realizationEpoch, scope)
        {
            if (areaOfUse == null)
                throw new ArgumentNullException(nameof(areaOfUse), ReferenceMessages.AreaOfUseIsNull);
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);
            if (primeMeridian == null)
                throw new ArgumentNullException(nameof(primeMeridian), ReferenceMessages.PrimeMeridianIsNull);

            this.AreaOfUse = areaOfUse;
            this.Ellipsoid = ellipsoid;
            this.PrimeMeridian = primeMeridian;
        }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the geodetic datum is applicable.</value>
        public virtual AreaOfUse AreaOfUse { get; private set; }

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid (or sphere) representing the shape of Earth.</value>
        public Ellipsoid Ellipsoid { get; private set; }

        /// <summary>
        /// Gets the prime meridian.
        /// </summary>
        /// <value>The prime meridian.</value>
        public Meridian PrimeMeridian { get; private set; }
    }
}
