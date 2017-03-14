// <copyright file="GeoVector.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Numerics;

    /// <summary>
    /// Represents a geographic vector.
    /// </summary>
    public class GeoVector : IEquatable<GeoVector>
    {
        /// <summary>
        /// Represents the zero <see cref="GeoVector" /> value. This field is constant.
        /// </summary>
        public static readonly GeoVector ZeroVector = new GeoVector(Angle.FromRadian(0), Length.FromMetre(0));

        /// <summary>
        /// Represents the undefined <see cref="GeoVector" /> value. This field is constant.
        /// </summary>
        public static readonly GeoVector Undefined = new GeoVector(Angle.FromRadian(Double.NaN), Length.FromMetre(Double.NaN));

        /// <summary>
        /// Defines the string format for coordinate vectors. This field is constant.
        /// </summary>
        private const String CoordinateVectorStringFormat = "({0}, {1})";

        /// <summary>
        /// Defines the string for invalid coordinate vectors. This field is constant.
        /// </summary>
        private const String InvalidCoordinateVectorString = "INVALID";

        /// <summary>
        /// Defines the string for null coordinate vectors. This field is constant.
        /// </summary>
        private const String NullCoordinateVectorString = "NULL";

        /// <summary>
        /// The azimuth of the vector.
        /// </summary>
        private readonly Angle azimuth;

        /// <summary>
        /// The length of the vector.
        /// </summary>
        private readonly Length length;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoVector" /> class.
        /// </summary>
        /// <param name="azimuth">The azimuth.</param>
        /// <param name="length">The length.</param>
        public GeoVector(Angle azimuth, Length length)
        {
            if (length.Value < 0)
            {
                this.azimuth = new Angle(-azimuth.Value % (2 * Math.PI / azimuth.Unit.BaseMultiple), azimuth.Unit);
                this.length = -length;
            }
            else
            {
                this.azimuth = new Angle(azimuth.Value % (2 * Math.PI / azimuth.Unit.BaseMultiple), azimuth.Unit);
                this.length = length;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoVector" /> class.
        /// </summary>
        /// <param name="azimuth">The azimuth (in radians).</param>
        /// <param name="length">The length (in meter).</param>
        public GeoVector(Double azimuth, Double length)
        {
            if (length < 0)
            {
                this.azimuth = Angle.FromRadian(-azimuth % (2 * Math.PI));
                this.length = Length.FromMetre(-length);
            }
            else
            {
                this.azimuth = Angle.FromRadian(azimuth % (2 * Math.PI));
                this.length = Length.FromMetre(length);
            }
        }

        /// <summary>
        /// Gets the azimuth of the vector.
        /// </summary>
        /// <value>The azimuth of a vector.</value>
        public Angle Azimuth { get { return this.azimuth; } }

        /// <summary>
        /// Gets the distance of the vector.
        /// </summary>
        /// <value>The distance of a vector.</value>
        public Length Distance { get { return this.length; } }

        /// <summary>
        /// Gets a value indicating whether the geographic vector is null.
        /// </summary>
        /// <value><c>true</c> if the length of the vector are 0; otherwise, <c>false</c>.</value>
        public Boolean IsNull { get { return this.length.Equals(Length.Zero); } }

        /// <summary>
        /// Gets a value indicating whether the geographic vector is valid.
        /// </summary>
        /// <value><c>true</c> if the azimuth and length are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(this.azimuth.Value) && !Double.IsNaN(this.length.Value); } }

        /// <summary>
        /// Indicates whether this instance and a specified other geographic vector are equal.
        /// </summary>
        /// <param name="other">Another geographic vector to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(GeoVector other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.azimuth.Equals(other.azimuth) && this.length.Equals(other.length);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            return this.Equals(obj as GeoVector);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (this.azimuth.GetHashCode() >> 2) ^ this.length.GetHashCode() ^ 57615137;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            if (!this.IsValid)
                return InvalidCoordinateVectorString;
            if (this.IsNull)
                return NullCoordinateVectorString;

            return String.Format(CultureInfo.InvariantCulture, CoordinateVectorStringFormat, this.azimuth, this.length);
        }

        /// <summary>
        /// Indicates whether the specified geographic vectors are equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(GeoVector first, GeoVector second)
        {
            if (ReferenceEquals(second, null))
                return ReferenceEquals(first, null);
            if (ReferenceEquals(first, null))
                return false;

            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified geographic vectors are not equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(GeoVector first, GeoVector second)
        {
            return !(first == second);
        }
    }
}
