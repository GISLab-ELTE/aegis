// <copyright file="GeoEnvelope.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ELTE.AEGIS.Numerics;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a geographic envelope.
    /// </summary>
    public class GeoEnvelope : IEquatable<GeoEnvelope>
    {
        /// <summary>
        /// Represents the undefined <see cref="GeoEnvelope" /> value. This field is read-only.
        /// </summary>
        public static readonly GeoEnvelope Undefined = new GeoEnvelope(Angle.Undefined, Angle.Undefined, Angle.Undefined, Angle.Undefined, Length.Undefined, Length.Undefined);

        /// <summary>
        /// Represents the <see cref="GeoEnvelope" /> value for the globe. This field is read-only.
        /// </summary>
        public static readonly GeoEnvelope Globe = new GeoEnvelope(Angle.FromRadian(-Math.PI / 2), Angle.FromRadian(Math.PI / 2), Angle.FromRadian(-Math.PI), Angle.FromRadian(Math.PI), Length.NegativeInfinity, Length.PositiveInfinity);

        /// <summary>
        /// The string format for envelopes. This field is constant.
        /// </summary>
        private const String EnvelopeStringFormat = "({0} {1} {2}, {3} {4} {5})";

        /// <summary>
        /// The string for empty envelopes. This field is constant.
        /// </summary>
        private const String EmptyEnvelopeString = "EMPTY ({0} {1} {2})";

        /// <summary>
        /// The string for invalid envelopes. This field is constant.
        /// </summary>
        private const String InvalidEnvelopeString = "INVALID";

        /// <summary>
        /// The minimal coordinate.
        /// </summary>
        private readonly GeoCoordinate minimum;

        /// <summary>
        /// Gets the maximal coordinate.
        /// </summary>
        private readonly GeoCoordinate maximum;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoEnvelope" /> class.
        /// </summary>
        /// <param name="firstLatitude">The first latitude.</param>
        /// <param name="secondLatitude">The second latitude.</param>
        /// <param name="firstLongitude">The first longitude.</param>
        /// <param name="secondLongitude">The second longitude.</param>
        /// <param name="firstHeight">The first height.</param>
        /// <param name="secondHeight">The second height.</param>
        public GeoEnvelope(Angle firstLatitude, Angle secondLatitude, Angle firstLongitude, Angle secondLongitude, Length firstHeight, Length secondHeight)
        {
            this.maximum = new GeoCoordinate(Angle.Min(firstLatitude, secondLatitude), Angle.Max(firstLongitude, secondLongitude), Length.Max(firstHeight, secondHeight));
            this.minimum = new GeoCoordinate(Angle.Min(firstLatitude, secondLatitude), Angle.Min(firstLongitude, secondLongitude), Length.Min(firstHeight, secondHeight));
        }

        /// <summary>
        /// Gets the minimal coordinate.
        /// </summary>
        /// <value>The minimal coordinate.</value>
        public GeoCoordinate Minimum { get { return this.minimum; } }

        /// <summary>
        /// Gets the maximal coordinate.
        /// </summary>
        /// <value>The maximal coordinate.</value>
        public GeoCoordinate Maximum { get { return this.maximum; } }

        /// <summary>
        /// Gets a value indicating whether the extent of the instance is zero.
        /// </summary>
        /// <value><c>true</c> if the extent is zero; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return this.minimum.Equals(this.maximum); } }

        /// <summary>
        /// Gets a value indicating whether the instance has valid coordinates.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers and within the globe; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return this.minimum.IsValid && this.maximum.IsValid; } }

        /// <summary>
        /// Gets a value indicating whether the instance has zero extent in the Z dimension.
        /// </summary>
        /// <value><c>true</c> if the instance has zero extent in the Z dimension; otherwise, <c>false</c>.</value>
        public Boolean IsPlanar { get { return this.minimum.Height == this.maximum.Height; } }

        /// <summary>
        /// Determines whether the envelope contains the specified geographic coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public Boolean Contains(GeoCoordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate), ReferenceMessages.CoordinateIsNull);

            return this.minimum.Latitude <= coordinate.Latitude && coordinate.Latitude <= this.maximum.Latitude &&
                   this.minimum.Longitude <= coordinate.Longitude && coordinate.Longitude <= this.maximum.Longitude &&
                   this.minimum.Height <= coordinate.Height && coordinate.Height <= this.maximum.Height;
        }

        /// <summary>
        /// Determines whether the instance contains another envelope.
        /// </summary>
        /// <param name="other">The other envelope.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="other" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The other envelope is null.</exception>
        public Boolean Contains(GeoEnvelope other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), ReferenceMessages.OtherEnvelopeIsNull);

            return this.minimum.Latitude <= other.minimum.Latitude && other.maximum.Latitude <= this.maximum.Latitude &&
                   this.minimum.Longitude <= other.minimum.Longitude && other.maximum.Longitude <= this.maximum.Longitude &&
                   this.minimum.Height <= other.minimum.Height && other.maximum.Height <= this.maximum.Height;
        }

        /// <summary>
        /// Indicates whether this instance and a specified geographic envelope are equal.
        /// </summary>
        /// <param name="another">The <see cref="GeoEnvelope" /> to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(GeoEnvelope another)
        {
            if (ReferenceEquals(null, another))
                return false;
            if (ReferenceEquals(this, another))
                return true;

            return this.minimum.Equals(another.minimum) && this.maximum.Equals(another.maximum);
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return this.Equals(obj as GeoEnvelope);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (this.minimum.GetHashCode() >> 2) ^ this.maximum.GetHashCode() ^ 190107161;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>The <see cref="System.String" /> containing the coordinates of the instance.</returns>
        public override String ToString()
        {
            if (!this.IsValid)
                return InvalidEnvelopeString;

            if (this.IsEmpty)
                return String.Format(CultureInfo.InvariantCulture, EmptyEnvelopeString, this.minimum.Latitude, this.minimum.Longitude, this.minimum.Height);

            return String.Format(CultureInfo.InvariantCulture, EnvelopeStringFormat, this.minimum.Latitude, this.minimum.Longitude, this.minimum.Height, this.maximum.Latitude, this.maximum.Longitude, this.maximum.Height);
        }
    }
}
