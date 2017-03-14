// <copyright file="GeoCoordinate.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Globalization;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a geographic coordinate.
    /// </summary>
    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        /// <summary>
        /// Represents the empty <see cref="GeoCoordinate" /> value. This field is constant.
        /// </summary>
        public static readonly GeoCoordinate Empty = new GeoCoordinate(0, 0, 0);

        /// <summary>
        /// Represents the undefined <see cref="GeoCoordinate" /> value. This field is constant.
        /// </summary>
        public static readonly GeoCoordinate Undefined = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN);

        /// <summary>
        /// The string format for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringFormat = "({0}, {1}, {2})";

        /// <summary>
        /// The string for empty coordinates. This field is constant.
        /// </summary>
        private const String EmptyCoordinateString = "EMPTY";

        /// <summary>
        /// The string for invalid coordinates. This field is constant.
        /// </summary>
        private const String InvalidCoordinateString = "INVALID";

        /// <summary>
        /// The latitude.
        /// </summary>
        private readonly Angle latitude;

        /// <summary>
        /// The longitude.
        /// </summary>
        private readonly Angle longitude;

        /// <summary>
        /// The height.
        /// </summary>
        private readonly Length height;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> class.
        /// </summary>
        /// <param name="latitude">The geographic latitude (in radians).</param>
        /// <param name="longitude">The geographic longitude (in radians).</param>
        public GeoCoordinate(Double latitude, Double longitude)
        {
            this.latitude = new Angle(latitude, UnitsOfMeasurement.Radian);
            this.longitude = new Angle(longitude, UnitsOfMeasurement.Radian);
            this.height = Length.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> class.
        /// </summary>
        /// <param name="latitude">The geographic latitude (in radians).</param>
        /// <param name="longitude">The geographic longitude (in radians).</param>
        /// <param name="height">The height (in meter).</param>
        public GeoCoordinate(Double latitude, Double longitude, Double height)
        {
            this.latitude = new Angle(latitude, UnitsOfMeasurement.Radian);
            this.longitude = new Angle(longitude, UnitsOfMeasurement.Radian);
            this.height = new Length(height, UnitsOfMeasurement.Metre);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> class.
        /// </summary>
        /// <param name="latitude">The geographic latitude.</param>
        /// <param name="longitude">The geographic longitude.</param>
        public GeoCoordinate(Angle latitude, Angle longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.height = Length.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinate" /> class.
        /// </summary>
        /// <param name="latitude">The geographic latitude.</param>
        /// <param name="longitude">The geographic longitude.</param>
        /// <param name="height">The height.</param>
        public GeoCoordinate(Angle latitude, Angle longitude, Length height)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.height = height;
        }

        /// <summary>
        /// Gets the geographic latitude.
        /// </summary>
        /// <value>The geographic latitude.</value>
        public Angle Latitude { get { return this.latitude; } }

        /// <summary>
        /// Gets the geographic longitude.
        /// </summary>
        /// <value>The geographic longitude.</value>
        public Angle Longitude { get { return this.longitude; } }

        /// <summary>
        /// Gets the geographic height.
        /// </summary>
        /// <value>The geographic height.</value>
        public Length Height { get { return this.height; } }

        /// <summary>
        /// Gets a value indicating whether the geographic coordinate is empty.
        /// </summary>
        /// <value><c>true</c> if all coordinates are <c>0</c>; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return this.latitude.Equals(Angle.Zero) && this.longitude.Equals(Angle.Zero) && this.height.Equals(Length.Zero); } }

        /// <summary>
        /// Gets a value indicating whether the geographic coordinate is valid.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers and within the globe; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(this.longitude.Value) && !Double.IsNaN(this.latitude.Value) && Math.Abs(this.longitude.BaseValue) <= Math.PI && Math.Abs(this.latitude.BaseValue) <= Math.PI / 2; } }

        /// <summary>
        /// Converts the coordinate to a valid globe value.
        /// </summary>
        /// <returns>The equivalent valid geographic coordinate.</returns>
        public GeoCoordinate ToGlobeValid()
        {
            return new GeoCoordinate(this.latitude.BaseValue % Math.PI / 2, this.longitude.BaseValue % Math.PI, this.height.BaseValue);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="angularUnit">The angular unit of conversion.</param>
        /// <param name="lengthUnit">The length unit of conversion.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions in the specified units.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The angular unit of measurement is null.
        /// or
        /// The length unit of measurement is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The angular unit of measurement is invalid.
        /// or
        /// The length unit of measurement is invalid.
        /// </exception>
        public String ToString(UnitOfMeasurement angularUnit, UnitOfMeasurement lengthUnit)
        {
            if (angularUnit == null)
                throw new ArgumentNullException(nameof(angularUnit), ReferenceMessages.AngularUnitOfMeasurementIsNull);

            if (angularUnit.Type != UnitQuantityType.Angle)
                throw new ArgumentException(ReferenceMessages.AngularUnitOfMeasurementIsInvalid, nameof(angularUnit));

            if (lengthUnit == null)
                throw new ArgumentNullException(nameof(lengthUnit), ReferenceMessages.LengthUnitOfMeasurementIsNull);

            if (lengthUnit.Type != UnitQuantityType.Length)
                throw new ArgumentException(ReferenceMessages.LengthUnitOfMeasurementIsInvalid, nameof(lengthUnit));

            if (!this.IsValid)
                return InvalidCoordinateString;

            if (this.IsEmpty)
                return EmptyCoordinateString;

            return String.Format(CultureInfo.InvariantCulture, CoordinateStringFormat, this.latitude.ToString(angularUnit), this.longitude.ToString(angularUnit), this.height.ToString(lengthUnit));
        }

        /// <summary>
        /// Indicates whether this instance and a specified other geographic coordinate are equal.
        /// </summary>
        /// <param name="other">Another geographic coordinate to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(GeoCoordinate other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.latitude.Equals(other.latitude) && this.longitude.Equals(other.longitude) && this.height.Equals(other.height);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return this.Equals(obj as GeoCoordinate);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (this.latitude.GetHashCode() / 94541) ^ (this.longitude.GetHashCode() / 5347) ^ this.height.GetHashCode();
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            if (!this.IsValid)
                return InvalidCoordinateString;

            if (this.IsEmpty)
                return EmptyCoordinateString;

            return String.Format(CultureInfo.InvariantCulture, CoordinateStringFormat, this.latitude, this.longitude, this.height);
        }

        /// <summary>
        /// Indicates whether the specified geographic coordinates are equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two geographic coordinates represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(GeoCoordinate first, GeoCoordinate second)
        {
            if (ReferenceEquals(second, null))
                return ReferenceEquals(first, null);
            if (ReferenceEquals(first, null))
                return false;

            return first.Latitude == second.Latitude && first.Longitude == second.Longitude && first.Height == second.Height;
        }

        /// <summary>
        /// Indicates whether the specified geographic coordinates are not equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two geographic coordinates do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(GeoCoordinate first, GeoCoordinate second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="GeoCoordinate" /> to <see cref="Coordinate" />.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The result of the conversion.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public static explicit operator Coordinate(GeoCoordinate coordinate)
        {
            if (ReferenceEquals(coordinate, null))
                throw new ArgumentNullException(nameof(coordinate), ReferenceMessages.CoordinateIsNull);

            return new Coordinate(coordinate.latitude.Value, coordinate.Longitude.Value, coordinate.Height.Value);
        }
    }
}
