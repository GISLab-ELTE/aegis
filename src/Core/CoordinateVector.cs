// <copyright file="CoordinateVector.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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

namespace AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using AEGIS.Numerics;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a vector in Cartesian coordinate space.
    /// </summary>
    public class CoordinateVector : IEquatable<Coordinate>, IEquatable<CoordinateVector>
    {
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateVector" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public CoordinateVector(Double x, Double y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateVector" /> class.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public CoordinateVector(Double x, Double y, Double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Gets the X component.
        /// </summary>
        /// <value>The X component.</value>
        public Double X { get; }

        /// <summary>
        /// Gets the Y component.
        /// </summary>
        /// <value>The Y component.</value>
        public Double Y { get; }

        /// <summary>
        /// Gets the Z component.
        /// </summary>
        /// <value>The Z component.</value>
        public Double Z { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CoordinateVector" /> is null.
        /// </summary>
        /// <value><c>true</c> if all component are 0; otherwise, <c>false</c>.</value>
        public Boolean IsNull { get { return this.X == 0 && this.Y == 0 && this.Z == 0; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CoordinateVector" /> is valid.
        /// </summary>
        /// <value><c>true</c> if all component are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(this.X) && !Double.IsNaN(this.Y) && !Double.IsNaN(this.Z); } }

        /// <summary>
        /// Gets the length of the <see cref="CoordinateVector" />.
        /// </summary>
        /// <value>The length of the vector.</value>
        public Double Length { get { return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z); } }

        /// <summary>
        /// Normalizes the <see cref="CoordinateVector" /> instance.
        /// </summary>
        /// <returns>The normalized <see cref="CoordinateVector" /> with identical direction.</returns>
        public CoordinateVector Normalize()
        {
            Double length = this.Length;
            return new CoordinateVector(this.X / length, this.Y / length, this.Z / length);
        }

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="Coordinate" /> are equal.
        /// </summary>
        /// <param name="other">Another <see cref="Coordinate" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Coordinate other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
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

            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

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
                return this.X == otherCoordinate.X && this.Y == otherCoordinate.Y && this.Z == otherCoordinate.Z;

            CoordinateVector otherVector = obj as CoordinateVector;

            if (otherVector != null)
                return this.X == otherVector.X && this.Y == otherVector.Y && this.Z == otherVector.Z;

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (this.X.GetHashCode() >> 4) ^ (this.Y.GetHashCode() >> 2) ^ this.Z.GetHashCode() ^ 57600017;
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

            return String.Format(CultureInfo.InvariantCulture, CoordinateVectorStringFormat, this.X, this.Y, this.Z);
        }

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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            return new CoordinateVector(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            return new CoordinateVector(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
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
                throw new ArgumentNullException(nameof(vector));

            return new CoordinateVector(scalar * vector.X, scalar * vector.Y, scalar * vector.Z);
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
                throw new ArgumentNullException(nameof(vector));

            return new CoordinateVector(scalar * vector.X, scalar * vector.Y, scalar * vector.Z);
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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            return first.X * second.X + first.Y * second.Y + first.Z * second.Z;
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

            return first.X == second.X && first.Y == second.Y && first.Z == second.Z;
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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            if (precision == null)
                precision = PrecisionModel.Default;

            return Math.Abs(first.X * second.Y - first.Y * second.X) <= precision.Tolerance(first, second) &&
                   Math.Abs(first.X * second.Z - first.Z * second.Z) <= precision.Tolerance(first, second);
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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            if (precision == null)
                precision = PrecisionModel.Default;

            return first.X * second.X + first.Y * second.Y + first.Z * second.Z <= precision.Tolerance(first, second);
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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            Double x = first.X - second.X;
            Double y = first.Y - second.Y;
            Double z = first.Z - second.Z;

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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            return first.X * second.X + first.Y * second.Y + first.Z * second.Z;
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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            return first.X * second.Y - first.Y * second.X;
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
                throw new ArgumentNullException(nameof(first));
            if (ReferenceEquals(second, null))
                throw new ArgumentNullException(nameof(second));

            return new CoordinateVector(first.Y * second.Z - first.Z * second.Y, first.Z * second.X - first.X * second.Z, first.X * second.Y - first.Y * second.X);
        }
    }
}
