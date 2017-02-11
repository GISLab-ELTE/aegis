// <copyright file="VerticalDatum.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;

    /// <summary>
    /// Defines a vertical datum.
    /// </summary>
    public class VerticalDatum : Datum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalDatum" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="anchorPoint">The anchor point.</param>
        /// <param name="realizationEpoch">The realization epoch.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public VerticalDatum(String identifier, String name, String anchorPoint, String realizationEpoch, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, anchorPoint, realizationEpoch, null, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalDatum" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="anchorPoint">The anchor point.</param>
        /// <param name="realizationEpoch">The realization epoch.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public VerticalDatum(String identifier, String name, String remarks, String[] aliases, String anchorPoint, String realizationEpoch, String scope, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, anchorPoint, realizationEpoch, scope)
        {
            this.AreaOfUse = areaOfUse;
        }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the geodetic datum is applicable.</value>
        public AreaOfUse AreaOfUse { get; private set; }

        /// <summary>
        /// Gets the subtype of the vertical datum.
        /// </summary>
        /// <value>The subtype of the vertical datum.</value>
        public VerticalDatumType Type
        {
            get
            {
                if (this.Authority == "EPSG")
                {
                    UInt16 code = UInt16.Parse(this.Identifier.Substring(5, 9));
                    if (code >= 5000 && code < 5099)
                        return VerticalDatumType.Ellipsoidal;
                    else if (code >= 5100 && code < 5899)
                        return VerticalDatumType.Orthometric;
                    else
                        return VerticalDatumType.Unknown;
                }
                else
                {
                    return VerticalDatumType.Unknown;
                }
            }
        }
    }
}
