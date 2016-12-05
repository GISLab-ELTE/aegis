// <copyright file="Meridian.cs" company="Eötvös Loránd University (ELTE)">
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
    using ELTE.AEGIS.Numerics;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a meridian.
    /// </summary>
    public class Meridian : IdentifiedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Meridian" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="longitude">The longitude.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public Meridian(String identifier, String name, Angle longitude)
            : this(identifier, name, null, null, longitude)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Meridian" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="longitude">The longitude.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public Meridian(String identifier, String name, String remarks, String[] aliases, Angle longitude)
            : base(identifier, name, remarks, aliases)
        {
            this.Longitude = longitude;
        }

        /// <summary>
        /// Gets the longitude angle of the meridian.
        /// </summary>
        /// <value>The longitude angle of the meridian.</value>
        public Angle Longitude { get; private set; }

        /// <summary>
        /// Creates a meridian from the longitude specified in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="longitude">The longitude (in degrees).</param>
        /// <returns>The meridian based on the specified longitude.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Longitude value must be between -180 and 180 degrees.</exception>
        public static Meridian FromDegrees(String identifier, String name, Double longitude)
        {
            if (longitude < -180 || longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude), ReferenceMessages.LongitudeOutOfRangeDegrees);

            return new Meridian(identifier, name, Angle.FromDegree(longitude));
        }

        /// <summary>
        /// Creates a meridian from the longitude specified in radians.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The meridian based on the specified longitude.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Longitude value must be between -PI and +PI radians.</exception>
        public static Meridian FromRadian(String identifier, String name, Double longitude)
        {
            if (longitude < -Math.PI || longitude > Math.PI)
                throw new ArgumentOutOfRangeException(nameof(longitude), ReferenceMessages.LongitudeOutOfRangeRadians);

            return new Meridian(identifier, name, Angle.FromRadian(longitude));
        }
    }
}
