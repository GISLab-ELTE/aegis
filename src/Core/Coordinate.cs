// <copyright file="Coordinate.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using AEGIS.Numerics;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a coordinate in Cartesian coordinate space.
    /// </summary>
    public class Coordinate : IEquatable<Coordinate>, IEquatable<CoordinateVector>
    {
        /// <summary>
        /// The string format for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringFormat = "({0} {1} {2})";

        /// <summary>
        /// The string for invalid coordinates. This field is constant.
        /// </summary>
        private const String InvalidCoordinateString = "INVALID";

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate" /> class.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        public Coordinate(Double x, Double y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate" /> class.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        public Coordinate(Double x, Double y, Double z)
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
        /// Gets a value indicating whether the coordinate is empty.
        /// </summary>
        /// <value><c>true</c> if all component are <c>0</c>; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return this.Z == 0 && this.Y == 0 && this.X == 0; } }

        /// <summary>
        /// Gets a value indicating whether the coordinate is valid.
        /// </summary>
        /// <value><c>true</c> if all component are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return !Double.IsNaN(this.X) && !Double.IsNaN(this.Y) && !Double.IsNaN(this.Z); } }

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="Coordinate" /> are equal.
        /// </summary>
        /// <param name="other">Another <see cref="Coordinate" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Coordinate other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="CoordinateVector" /> are equal.
        /// </summary>
        /// <param name="other">Another <see cref="CoordinateVector" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(CoordinateVector other)
        {
            if (other is null)
                return false;

            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is Coordinate otherCoordinate)
                return this.X == otherCoordinate.X && this.Y == otherCoordinate.Y && this.Z == otherCoordinate.Z;

            if (obj is CoordinateVector otherVector)
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
            return this.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public String ToString(IFormatProvider provider)
        {
            if (!this.IsValid)
                return InvalidCoordinateString;

            return String.Format(provider, CoordinateStringFormat, this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Sums the specified <see cref="Coordinate" /> instance with a <see cref="CoordinateVector" /> instance.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The sum of the <see cref="Coordinate" /> and <see cref="CoordinateVector" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The coordinate is null.
        /// or
        /// The vector is null.
        /// </exception>
        public static Coordinate operator +(Coordinate coordinate, CoordinateVector vector)
        {
            if (coordinate is null)
                throw new ArgumentNullException(nameof(coordinate));
            if (vector is null)
                throw new ArgumentNullException(nameof(vector));

            return new Coordinate(coordinate.X + vector.X, coordinate.Y + vector.Y, coordinate.Z + vector.Z);
        }

        /// <summary>
        /// Extracts the specified <see cref="Coordinate" /> instance with a <see cref="CoordinateVector" /> instance.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The extract of the <see cref="Coordinate" /> and <see cref="CoordinateVector" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The coordinate is null.
        /// or
        /// The vector is null.
        /// </exception>
        public static Coordinate operator -(Coordinate coordinate, CoordinateVector vector)
        {
            if (coordinate is null)
                throw new ArgumentNullException(nameof(coordinate));
            if (vector is null)
                throw new ArgumentNullException(nameof(vector));

            return new Coordinate(coordinate.X - vector.X, coordinate.Y - vector.Y, coordinate.Z - vector.Z);
        }

        /// <summary>
        /// Extracts the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The extract of the two <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first coordinate is null.
        /// or
        /// The second coordinate is null.
        /// </exception>
        public static CoordinateVector operator -(Coordinate first, Coordinate second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            return new CoordinateVector(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Coordinate" /> instances are equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two <see cref="Coordinate" /> instances represent the same value; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first coordinate is null.
        /// or
        /// The second coordinate is null.
        /// </exception>
        public static Boolean operator ==(Coordinate first, Coordinate second)
        {
            if (second is null)
                return first is null;
            if (first is null)
                return false;

            return first.X == second.X && first.Y == second.Y && first.Z == second.Z;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Coordinate" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two <see cref="Coordinate" /> instances do not represent the same value; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first coordinate is null.
        /// or
        /// The second coordinate is null.
        /// </exception>
        public static Boolean operator !=(Coordinate first, Coordinate second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Converts the specified <see cref="CoordinateVector" /> instance to <see cref="Coordinate" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The <see cref="Coordinate" /> equivalent of the specified <see cref="CoordinateVector" /> instance.</returns>
        public static explicit operator Coordinate(CoordinateVector vector)
        {
            if (vector == null)
                return null;

            return new Coordinate(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Computes the distance between the two coordinates.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The distance between the two <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first coordinate is null.
        /// or
        /// The second coordinate is null.
        /// </exception>
        public static Double Distance(Coordinate first, Coordinate second)
        {
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            if (first.Equals(second))
                return 0;

            Double x = first.X - second.X;
            Double y = first.Y - second.Y;
            Double z = first.Z - second.Z;

            return Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">The array of coordinates is null.</exception>
        public static Coordinate Centroid(params Coordinate[] coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            if (coordinates.Length == 0)
                return null;

            Double sumX = 0, sumY = 0, sumZ = 0;
            for (Int32 coordinateIndex = 0; coordinateIndex < coordinates.Length; coordinateIndex++)
            {
                if (coordinates[coordinateIndex] is null)
                    continue;

                sumX += coordinates[coordinateIndex].X;
                sumY += coordinates[coordinateIndex].Y;
                sumZ += coordinates[coordinateIndex].Z;
            }

            sumX /= coordinates.Length;
            sumY /= coordinates.Length;
            sumZ /= coordinates.Length;

            return new Coordinate(sumX, sumY, sumZ);
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of coordinates is null.</exception>
        public static Coordinate Centroid(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            if (!coordinates.Any())
                return null;

            Double sumX = 0, sumY = 0, sumZ = 0;
            Int32 count = 0;
            foreach (Coordinate coordinate in coordinates)
            {
                if (coordinate is null)
                    continue;

                sumX += coordinate.X;
                sumY += coordinate.Y;
                sumZ += coordinate.Z;
                count++;
            }

            return new Coordinate(sumX / count, sumY / count, sumZ / count);
        }

        /// <summary>
        /// Computes the centroid of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The centroid of the specified <see cref="Coordinate" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">coordinates;The coordinate collection is null.</exception>
        public static Coordinate Centroid(IEnumerable<Coordinate> coordinates, PrecisionModel precisionModel)
        {
            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            return precisionModel.MakePrecise(Centroid(coordinates));
        }

        /// <summary>
        /// Computes the planar orientation of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="origin">The coordinate of origin.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The orientation of the second <see cref="Coordinate" /> to the first with respect to origin.</returns>
        public static Orientation Orientation(Coordinate origin, Coordinate first, Coordinate second)
        {
            return Orientation(origin, first, second, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the planar orientation of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="origin">The coordinate of origin.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The orientation of the second <see cref="Coordinate" /> to the first with respect to origin.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The origin is null.
        /// or
        /// The first coordinate is null.
        /// or
        /// The second coordinate is null.
        /// </exception>
        public static Orientation Orientation(Coordinate origin, Coordinate first, Coordinate second, PrecisionModel precisionModel)
        {
            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            if (origin is null)
                throw new ArgumentNullException(nameof(origin));
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            Double det = (first.X - origin.X) * (second.Y - origin.Y) - (first.Y - origin.Y) * (second.X - origin.X);

            if (Double.IsNaN(det))
                return AEGIS.Orientation.Undefined;

            if (Math.Abs(det) <= precisionModel.Tolerance(origin, first, second))
                return AEGIS.Orientation.Collinear;

            if (det > 0)
                return AEGIS.Orientation.Counterclockwise;
            else
                return AEGIS.Orientation.Clockwise;
        }

        /// <summary>
        /// Computes the angle of the specified <see cref="Coordinate" /> instances.
        /// </summary>
        /// <param name="origin">The coordinate of origin.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The angle between first and second <see cref="Coordinate" /> with respect to origin.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The origin is null.
        /// or
        /// The first coordinate is null.
        /// or
        /// The second coordinate is null.
        /// </exception>
        public static Double Angle(Coordinate origin, Coordinate first, Coordinate second)
        {
            if (origin is null)
                throw new ArgumentNullException(nameof(origin));
            if (first is null)
                throw new ArgumentNullException(nameof(first));
            if (second is null)
                throw new ArgumentNullException(nameof(second));

            Double distanceOriginFirst = Distance(origin, first);
            Double distanceOriginSecond = Distance(origin, second);
            Double distanceFirstSecond = Distance(first, second);

            return Math.Acos((distanceOriginFirst * distanceOriginFirst + distanceOriginSecond * distanceOriginSecond - distanceFirstSecond * distanceFirstSecond) / (2 * distanceOriginFirst * distanceOriginSecond));
        }

        /// <summary>
        /// Returns the coordinate matching the dimension of the specified reference system.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The coordinate matching the dimension of the specified reference system.</returns>
        public static Coordinate WithDimension(Coordinate coordinate, IReferenceSystem referenceSystem)
        {
            if (referenceSystem == null || referenceSystem.Dimension >= 3)
                return coordinate;

            switch (referenceSystem.Dimension)
            {
                case 2:
                    return new Coordinate(coordinate.X, coordinate.Y);
                case 1:
                    return new Coordinate(coordinate.X, 0);
                default:
                    return new Coordinate(0, 0);
            }
        }

        /// <summary>
        /// Returns the coordinate matching the dimension of the specified reference system.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>The coordinate matching the dimension of the specified reference system.</returns>
        public static Coordinate WithDimension(Double x, Double y, Double z, IReferenceSystem referenceSystem)
        {
            if (referenceSystem == null || referenceSystem.Dimension >= 3)
                return new Coordinate(x, y, z);

            switch (referenceSystem.Dimension)
            {
                case 2:
                    return new Coordinate(x, y);
                case 1:
                    return new Coordinate(x, 0);
                default:
                    return new Coordinate(0, 0);
            }
        }
    }
}
