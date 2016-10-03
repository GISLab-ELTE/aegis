// <copyright file="CoordinateVector.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using ELTE.AEGIS.Numerics;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a vector in Cartesian coordinate space.
    /// </summary>
    public class CoordinateVector : IEquatable<Coordinate>, IEquatable<CoordinateVector>
    {
        #region Public instances

        /// <summary>
        /// Represents the null <see cref="CoordinateVector" /> value. This field is constant.
        /// </summary>
        public static readonly CoordinateVector NullVector = new CoordinateVector(0, 0, 0);

        #endregion

        #region Private constants

        /// <summary>
        /// Defines the string format for coordinate vectors. This field is constant.
        /// </summary>
        private const String CoordinateVectorStringFormat = "({0}, {1}, {2})";

        /// <summary>
        /// Defines the string for invalid coordinate vectors. This field is constant.
        /// </summary>
        private const String InvalidCoordinateVectorString = "INVALID";

        /// <summary>
        /// Defines the string for null coordinate vectors. This field is constant.
        /// </summary>
        private const String NullCoordinateVectorString = "NULL";

        #endregion

        #region Private fields

        /// <summary>
        /// The X component.
        /// </summary>
        private readonly Double x;

        /// <summary>
        /// The Y component.
        /// </summary>
        private readonly Double y;

        /// <summary>
        /// The Z component.
        /// </summary>
        private readonly Double z;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateVector" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public CoordinateVector(Double x, Double y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateVector" /> class.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public CoordinateVector(Double x, Double y, Double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the X component.
        /// </summary>
        /// <value>The X component.</value>
        public Double X { get { return this.x; } }

        /// <summary>
        /// Gets the Y component.
        /// </summary>
        /// <value>The Y component.</value>
        public Double Y { get { return this.y; } }

        /// <summary>
        /// Gets the Z component.
        /// </summary>
        /// <value>The Z component.</value>
        public Double Z { get { return this.z; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CoordinateVector" /> is null.
        /// </summary>
        /// <value><c>true</c> if all component are 0; otherwise, <c>false</c>.</value>
        public Boolean IsNull { get { return this.x == 0 && this.y == 0 && this.z == 0; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CoordinateVector" /> is valid.
        /// </summary>
        /// <value><c>true</c> if all component are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(this.x) && !Double.IsNaN(this.y) && !Double.IsNaN(this.z); } }

        /// <summary>
        /// Gets the length of the <see cref="CoordinateVector" />.
        /// </summary>
        /// <value>The length of the vector.</value>
        public Double Length { get { return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z); } }

        #endregion

        #region Public methods

        /// <summary>
        /// Normalizes the <see cref="CoordinateVector" /> instance.
        /// </summary>
        /// <returns>The normalized <see cref="CoordinateVector" /> with identical direction.</returns>
        public CoordinateVector Normalize()
        {
            Double length = this.Length;
            return new CoordinateVector(this.x / length, this.y / length, this.z / length);
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="Coordinate" /> are equal.
        /// </summary>
        /// <param name="other">Another <see cref="Coordinate" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Coordinate other)
        {
            return this.x == other.X && this.y == other.Y && this.z == other.Z;
        }

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="CoordinateVector" /> are equal.
        /// </summary>
        /// <param name="other">Another <see cref="CoordinateVector" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(CoordinateVector other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.x == other.x && this.y == other.y && this.z == other.z;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            Coordinate otherCoordinate = obj as Coordinate;

            if (otherCoordinate != null)
                return this.x == otherCoordinate.X && this.y == otherCoordinate.Y && this.z == otherCoordinate.Z;

            CoordinateVector otherVector = obj as CoordinateVector;

            if (otherVector != null)
                return this.x == otherVector.x && this.y == otherVector.y && this.z == otherVector.z;

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (this.x.GetHashCode() >> 4) ^ (this.y.GetHashCode() >> 2) ^ this.z.GetHashCode() ^ 57600017;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            if (this.IsNull)
                return NullCoordinateVectorString;
            if (!this.IsValid)
                return InvalidCoordinateVectorString;

            return String.Format(CultureInfo.InvariantCulture, CoordinateVectorStringFormat, this.x, this.y, this.z);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Sums the specified vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static CoordinateVector operator +(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            return new CoordinateVector(first.x + second.x, first.y + second.y, first.z + second.z);
        }

        /// <summary>
        /// Extracts the specified vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The extract of the vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static CoordinateVector operator -(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            return new CoordinateVector(first.x - second.x, first.y - second.y, first.z - second.z);
        }

        /// <summary>
        /// Multiplies the specified <see cref="CoordinateVector" /> instance with a scalar.
        /// </summary>
        /// <param name="scalar">The scalar value.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The scalar multiple of the <see cref="System.Double" /> and vectors.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static CoordinateVector operator *(Double scalar, CoordinateVector vector)
        {
            if (ReferenceEquals(vector, null))
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);

            return new CoordinateVector(scalar * vector.x, scalar * vector.y, scalar * vector.z);
        }

        /// <summary>
        /// Multiplies the specified <see cref="CoordinateVector" /> instance with a scalar.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>The scalar multiple of the <see cref="System.Double" /> and vectors.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static CoordinateVector operator *(CoordinateVector vector, Double scalar)
        {
            if (ReferenceEquals(vector, null))
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);

            return new CoordinateVector(scalar * vector.x, scalar * vector.y, scalar * vector.z);
        }

        /// <summary>
        /// Multiplies the specified vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The scalar multiple of the vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Double operator *(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            return first.x * second.x + first.y * second.y + first.z * second.z;
        }

        /// <summary>
        /// Indicates whether the specified vectors are equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two vectors represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(second, null))
                return ReferenceEquals(first, null);
            if (ReferenceEquals(first, null))
                return false;

            return first.x == second.X && first.y == second.y && first.z == second.z;
        }

        /// <summary>
        /// Indicates whether the specified vectors are not equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two vectors do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(CoordinateVector first, CoordinateVector second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Converts the specified <see cref="Coordinate" /> instance to <see cref="CoordinateVector" />.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The <see cref="CoordinateVector" /> equivalent of the specified <see cref="Coordinate" /> instance.</returns>
        public static explicit operator CoordinateVector(Coordinate coordinate)
        {
            if (ReferenceEquals(coordinate, null))
                return null;

            return new CoordinateVector(coordinate.X, coordinate.Y, coordinate.Z);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether the two vectors are parallel.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two vectors are parallel; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Boolean IsParallel(CoordinateVector first, CoordinateVector second)
        {
            return IsParallel(first, second, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the two vectors are parallel.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two vectors are parallel; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Boolean IsParallel(CoordinateVector first, CoordinateVector second, PrecisionModel precision)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            if (precision == null)
                precision = PrecisionModel.Default;

            return Math.Abs(first.x * second.y - first.y * second.x) <= precision.Tolerance(first, second) &&
                   Math.Abs(first.x * second.z - first.z * second.z) <= precision.Tolerance(first, second);
        }

        /// <summary>
        /// Determines whether the two vectors are perpendicular.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the two vectors are perpendicular; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Boolean IsPerpendicular(CoordinateVector first, CoordinateVector second)
        {
            return IsPerpendicular(first, second, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the two vectors are perpendicular.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two vectors are perpendicular; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Boolean IsPerpendicular(CoordinateVector first, CoordinateVector second, PrecisionModel precision)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            if (precision == null)
                precision = PrecisionModel.Default;

            return first.x * second.x + first.y * second.y + first.z * second.z <= precision.Tolerance(first, second);
        }

        /// <summary>
        /// Computes the distance between the two vectors.
        /// </summary>
        /// <param name="firstX">The X value of the first vector.</param>
        /// <param name="firstY">The Y value of the first vector.</param>
        /// <param name="secondX">The X value of the second vector.</param>
        /// <param name="secondY">The Y value of the second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static Double Distance(Double firstX, Double firstY, Double secondX, Double secondY)
        {
            Double x = firstX - secondX;
            Double y = firstY - secondY;

            return Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// Computes the distance between the two vectors.
        /// </summary>
        /// <param name="firstX">The X value of the first vector.</param>
        /// <param name="firstY">The Y value of the first vector.</param>
        /// <param name="firstZ">The Z value of the first vector.</param>
        /// <param name="secondX">The X value of the second vector.</param>
        /// <param name="secondY">The Y value of the second vector.</param>
        /// <param name="secondZ">The Z value of the second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static Double Distance(Double firstX, Double firstY, Double firstZ, Double secondX, Double secondY, Double secondZ)
        {
            Double x = firstX - secondX;
            Double y = firstY - secondY;
            Double z = firstZ - secondZ;

            return Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Computes the distance between two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Double Distance(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            Double x = first.x - second.x;
            Double y = first.y - second.y;
            Double z = first.z - second.z;

            return Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Computes the dot product of two vectors.
        /// </summary>
        /// <param name="firstX">The X value of the first vector.</param>
        /// <param name="firstY">The Y value of the first vector.</param>
        /// <param name="secondX">The X value of the second vector.</param>
        /// <param name="secondY">The Y value of the second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static Double DotProduct(Double firstX, Double firstY, Double secondX, Double secondY)
        {
            return firstX * secondX + firstY * secondY;
        }

        /// <summary>
        /// Computes the dot product of two vectors.
        /// </summary>
        /// <param name="firstX">The X value of the first vector.</param>
        /// <param name="firstY">The Y value of the first vector.</param>
        /// <param name="firstZ">The Z value of the first vector.</param>
        /// <param name="secondX">The X value of the second vector.</param>
        /// <param name="secondY">The Y value of the second vector.</param>
        /// <param name="secondZ">The Z value of the second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static Double DotProduct(Double firstX, Double firstY, Double firstZ, Double secondX, Double secondY, Double secondZ)
        {
            return firstX * secondX + firstY * secondY + firstZ * secondZ;
        }

        /// <summary>
        /// Computes the dot product of two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Double DotProduct(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            return first.x * second.x + first.y * second.y + first.z * second.z;
        }

        /// <summary>
        /// Computes the perp dot product of two vectors.
        /// </summary>
        /// <param name="firstX">The X value of the first vector.</param>
        /// <param name="firstY">The Y value of the first vector.</param>
        /// <param name="secondX">The X value of the second vector.</param>
        /// <param name="secondY">The Y value of the second vector.</param>
        /// <returns>The perp dot product of two vectors.</returns>
        public static Double PerpDotProduct(Double firstX, Double firstY, Double secondX, Double secondY)
        {
            return firstX * secondY - firstY * secondX;
        }

        /// <summary>
        /// Computes the perp dot product of two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The perp dot product of two vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static Double PerpDotProduct(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            return first.x * second.y - first.y * second.x;
        }

        /// <summary>
        /// Computes the dot product of two vectors.
        /// </summary>
        /// <param name="firstX">The X value of the first vector.</param>
        /// <param name="firstY">The Y value of the first vector.</param>
        /// <param name="firstZ">The Z value of the first vector.</param>
        /// <param name="secondX">The X value of the second vector.</param>
        /// <param name="secondY">The Y value of the second vector.</param>
        /// <param name="secondZ">The Z value of the second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        public static CoordinateVector CrossProduct(Double firstX, Double firstY, Double firstZ, Double secondX, Double secondY, Double secondZ)
        {
            return new CoordinateVector(firstY * secondZ - firstZ * secondY, firstZ * secondX - firstX * secondZ, firstX * secondY - firstY * secondX);
        }

        /// <summary>
        /// Computes the cross product of two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The cross product of two vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        public static CoordinateVector CrossProduct(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null))
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            return new CoordinateVector(first.y * second.z - first.z * second.y, first.z * second.x - first.x * second.z, first.x * second.y - first.y * second.x);
        }

        #endregion
    }
}
