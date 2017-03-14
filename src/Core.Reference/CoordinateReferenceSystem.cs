// <copyright file="CoordinateReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a coordinate reference system.
    /// </summary>
    public abstract class CoordinateReferenceSystem : ReferenceSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope of the reference system.</param>
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
        protected CoordinateReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, CoordinateSystem coordinateSystem, Datum datum, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, scope)
        {
            if (coordinateSystem == null)
                throw new ArgumentNullException(nameof(coordinateSystem), ReferenceMessages.CoordinateSystemIsNull);
            if (datum == null)
                throw new ArgumentNullException(nameof(datum), ReferenceMessages.DatumIsNull);
            if (areaOfUse == null)
                throw new ArgumentNullException(nameof(areaOfUse), ReferenceMessages.AreaOfUseIsNull);

            this.CoordinateSystem = coordinateSystem;
            this.Datum = datum;
            this.AreaOfUse = areaOfUse;
        }

        /// <summary>
        /// Gets the dimension of the reference system.
        /// </summary>
        /// <value>The dimension of the reference system.</value>
        public override Int32 Dimension { get { return this.CoordinateSystem.Dimension; } }

        /// <summary>
        /// Gets the coordinate system.
        /// </summary>
        /// <value>The coordinate system.</value>
        public CoordinateSystem CoordinateSystem { get; private set; }

        /// <summary>
        /// Gets the datum of the coordinate reference system.
        /// </summary>
        /// <value>The datum of the coordinate reference system.</value>
        public Datum Datum { get; private set; }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the reference system is applicable.</value>
        public AreaOfUse AreaOfUse { get; private set; }
    }
}