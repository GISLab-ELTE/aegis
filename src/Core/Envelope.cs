// <copyright file="Envelope.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AEGIS.Collections;
    using AEGIS.Resources;

    /// <summary>
    /// Represents an envelope in Cartesian coordinate space.
    /// </summary>
    public class Envelope : IEquatable<Envelope>
    {
        /// <summary>
        /// Represents the infinite <see cref="Envelope" /> value. This field is read-only.
        /// </summary>
        public static readonly Envelope Infinity = new Envelope(Double.NegativeInfinity, Double.PositiveInfinity, Double.PositiveInfinity, Double.NegativeInfinity, Double.NegativeInfinity, Double.PositiveInfinity);

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
        /// The minimal coordinate in all dimensions.
        /// </summary>
        private readonly Coordinate minimum;

        /// <summary>
        /// The maximal coordinate in all dimensions.
        /// </summary>
        private readonly Coordinate maximum;

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class.
        /// </summary>
        /// <param name="firstX">The first X coordinate.</param>
        /// <param name="secondX">The second X coordinate.</param>
        /// <param name="firstY">The first Y coordinate.</param>
        /// <param name="secondY">The second Y coordinate.</param>
        public Envelope(Double firstX, Double secondX, Double firstY, Double secondY)
        {
            this.maximum = new Coordinate(Math.Max(firstX, secondX), Math.Max(firstY, secondY));
            this.minimum = new Coordinate(Math.Min(firstX, secondX), Math.Min(firstY, secondY));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class.
        /// </summary>
        /// <param name="firstX">The first X coordinate.</param>
        /// <param name="secondX">The second X coordinate.</param>
        /// <param name="firstY">The first Y coordinate.</param>
        /// <param name="secondY">The second Y coordinate.</param>
        /// <param name="firstZ">The first Z coordinate.</param>
        /// <param name="secondZ">The second Z coordinate.</param>
        public Envelope(Double firstX, Double secondX, Double firstY, Double secondY, Double firstZ, Double secondZ)
        {
            this.maximum = new Coordinate(Math.Max(firstX, secondX), Math.Max(firstY, secondY), Math.Max(firstZ, secondZ));
            this.minimum = new Coordinate(Math.Min(firstX, secondX), Math.Min(firstY, secondY), Math.Min(firstZ, secondZ));
        }

        /// <summary>
        /// Gets the minimum X value.
        /// </summary>
        /// <value>The minimum X value.</value>
        public Double MinX { get { return this.minimum.X; } }

        /// <summary>
        /// Gets the minimum Y value.
        /// </summary>
        /// <value>The minimum Y value.</value>
        public Double MinY { get { return this.minimum.Y; } }

        /// <summary>
        /// Gets the minimum Z value.
        /// </summary>
        /// <value>The minimum Z value.</value>
        public Double MinZ { get { return this.minimum.Z; } }

        /// <summary>
        /// Gets the maximum X value.
        /// </summary>
        /// <value>The maximum X value.</value>
        public Double MaxX { get { return this.maximum.X; } }

        /// <summary>
        /// Gets the maximum Y value.
        /// </summary>
        /// <value>The maximum Y value.</value>
        public Double MaxY { get { return this.maximum.Y; } }

        /// <summary>
        /// Gets the maximum Z value.
        /// </summary>
        /// <value>The maximum Z value.</value>
        public Double MaxZ { get { return this.maximum.Z; } }

        /// <summary>
        /// Gets the minimal coordinate in all dimensions.
        /// </summary>
        /// <value>The minimal coordinate in all dimensions.</value>
        public Coordinate Minimum { get { return this.minimum; } }

        /// <summary>
        /// Gets the maximal coordinate in all dimensions.
        /// </summary>
        /// <value>The maximal coordinate in all dimensions.</value>
        public Coordinate Maximum { get { return this.maximum; } }

        /// <summary>
        /// Gets the center coordinate in all dimensions.
        /// </summary>
        /// <value>The center coordinate in all dimensions.</value>
        public Coordinate Center { get { return new Coordinate((this.minimum.X + this.maximum.X) / 2, (this.minimum.Y + this.maximum.Y) / 2, (this.minimum.Z + this.maximum.Z) / 2); } }

        /// <summary>
        /// Gets a value indicating whether the extent of the instance is zero in all dimensions.
        /// </summary>
        /// <value><c>true</c> if the extent is zero in all dimensions; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return this.minimum.Equals(this.maximum); } }

        /// <summary>
        /// Gets a value indicating whether the instance has valid coordinates.
        /// </summary>
        /// <value><c>true</c> if all coordinates are numbers; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return this.minimum.IsValid && this.maximum.IsValid; } }

        /// <summary>
        /// Gets a value indicating whether the instance has zero extent in the Z dimension.
        /// </summary>
        /// <value><c>true</c> if the instance has zero extent in the Z dimension; otherwise, <c>false</c>.</value>
        public Boolean IsPlanar { get { return this.minimum.Z == this.maximum.Z; } }

        /// <summary>
        /// Gets the surface of the rectangle.
        /// </summary>
        /// <value>The surface of the rectangle. In case of planar rectangles, the surface equals the area.</value>
        public Double Surface
        {
            get
            {
                if (this.IsPlanar)
                    return (this.maximum.X - this.minimum.X) * (this.maximum.Y - this.minimum.Y);
                else
                    return 2 * (this.maximum.X - this.minimum.X) * (this.maximum.Y - this.minimum.Y) + 2 * (this.maximum.X - this.minimum.X) * (this.maximum.Z - this.minimum.Z) + 2 * (this.maximum.Y - this.minimum.Y) * (this.maximum.Z - this.minimum.Z);
            }
        }

        /// <summary>
        /// Gets the volume of the rectangle.
        /// </summary>
        /// <value>The volume of the rectangle. The volume is zero in case of planar rectangles.</value>
        public Double Volume { get { return (this.maximum.X - this.minimum.X) * (this.maximum.Y - this.minimum.Y) * (this.maximum.Z - this.minimum.Z); } }

        /// <summary>
        /// Determines whether the envelope contains the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public Boolean Contains(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            return this.minimum.X <= coordinate.X && coordinate.X <= this.maximum.X &&
                   this.minimum.Y <= coordinate.Y && coordinate.Y <= this.maximum.Y &&
                   this.minimum.Z <= coordinate.Z && coordinate.Z <= this.maximum.Z;
        }

        /// <summary>
        /// Determines whether the instance contains another envelope.
        /// </summary>
        /// <param name="envelope">The other envelope.</param>
        /// <returns><c>true</c> if the envelope contains <paramref name="envelope" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Contains(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return this.minimum.X <= envelope.minimum.X && envelope.maximum.X <= this.maximum.X &&
                   this.minimum.Y <= envelope.minimum.Y && envelope.maximum.Y <= this.maximum.Y &&
                   this.minimum.Z <= envelope.minimum.Z && envelope.maximum.Z <= this.maximum.Z;
        }

        /// <summary>
        /// Determines whether the instance crosses another envelope.
        /// </summary>
        /// <param name="envelope">The other envelope.</param>
        /// <returns><c>true</c> if the envelope crosses <paramref name="envelope" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Crosses(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return !this.Disjoint(envelope) && !this.Equals(envelope);
        }

        /// <summary>
        /// Determines whether the instance is disjoint from another envelope.
        /// </summary>
        /// <param name="envelope">The other envelope.</param>
        /// <returns><c>true</c> if the envelope is disjoint from <paramref name="envelope" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Disjoint(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return this.maximum.X < envelope.minimum.X || this.minimum.X > envelope.maximum.X ||
                   this.maximum.Y < envelope.minimum.Y || this.minimum.Y > envelope.maximum.Y ||
                   this.maximum.Z < envelope.minimum.Z || this.minimum.Z > envelope.maximum.Z;
        }

        /// <summary>
        /// Determines whether the instance intersects another envelope.
        /// </summary>
        /// <param name="envelope">The other envelope.</param>
        /// <returns><c>true</c> if the envelope intersects <paramref name="envelope" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Intersects(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return !this.Disjoint(envelope);
        }

        /// <summary>
        /// Determines whether the instance overlaps another envelope.
        /// </summary>
        /// <param name="envelope">The other envelope.</param>
        /// <returns><c>true</c> if the envelope overlaps <paramref name="envelope" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Overlaps(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return !this.Disjoint(envelope) && !this.Equals(envelope);
        }

        /// <summary>
        /// Determines whether the instance touches another envelope.
        /// </summary>
        /// <param name="envelope">The other envelope.</param>
        /// <returns><c>true</c> if the envelope touches <paramref name="envelope" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Touches(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return !this.Disjoint(envelope) && (this.minimum.X == envelope.maximum.X || this.maximum.X == envelope.minimum.X ||
                                        this.minimum.Y == envelope.maximum.Y || this.maximum.Y == envelope.minimum.Y ||
                                        this.minimum.Z == envelope.maximum.Z || this.maximum.Z == envelope.minimum.Z);
        }

        /// <summary>
        /// Determines whether the instance is within another envelope.
        /// </summary>
        /// <param name="envelope">The other envelope.</param>
        /// <returns><c>true</c> if the envelope is within <paramref name="envelope" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Within(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException(nameof(envelope));

            return envelope.Contains(this);
        }

        /// <summary>
        /// Indicates whether this instance and a specified envelope are equal.
        /// </summary>
        /// <param name="other">The envelope to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Envelope other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.minimum.Equals(other.minimum) && this.maximum.Equals(other.maximum);
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            Envelope envelopeObj = obj as Envelope;

            return envelopeObj != null && this.minimum == envelopeObj.minimum && this.maximum == envelopeObj.maximum;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (this.minimum.GetHashCode() >> 2) ^ this.maximum.GetHashCode() ^ 190130741;
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
                return String.Format(CultureInfo.InvariantCulture, EmptyEnvelopeString, this.minimum.X, this.minimum.Y, this.minimum.Z);

            return String.Format(CultureInfo.InvariantCulture, EnvelopeStringFormat, this.minimum.X, this.minimum.Y, this.minimum.Z, this.maximum.X, this.maximum.Y, this.maximum.Z);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The <see cref="Envelope" /> created from the coordinates.</returns>
        public static Envelope FromCoordinates(params Coordinate[] coordinates)
        {
            if (coordinates == null || !coordinates.AnyElement())
                return null;

            Double minX = Double.MaxValue, minY = Double.MaxValue, minZ = Double.MaxValue, maxX = Double.MinValue, maxY = Double.MinValue, maxZ = Double.MinValue;
            foreach (Coordinate coordinate in coordinates)
            {
                if (coordinate == null)
                    continue;

                if (coordinate.X < minX)
                    minX = coordinate.X;
                if (coordinate.Y < minY)
                    minY = coordinate.Y;
                if (coordinate.Z < minZ)
                    minZ = coordinate.Z;
                if (coordinate.X > maxX)
                    maxX = coordinate.X;
                if (coordinate.Y > maxY)
                    maxY = coordinate.Y;
                if (coordinate.Z > maxZ)
                    maxZ = coordinate.Z;
            }

            return new Envelope(minX, maxX, minY, maxY, minZ, maxZ);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The <see cref="Envelope" /> created from the coordinates.</returns>
        public static Envelope FromCoordinates(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null || !coordinates.AnyElement())
                return null;

            Double minX = Double.MaxValue, minY = Double.MaxValue, minZ = Double.MaxValue, maxX = Double.MinValue, maxY = Double.MinValue, maxZ = Double.MinValue;
            foreach (Coordinate coordinate in coordinates)
            {
                if (coordinate == null)
                    continue;

                if (coordinate.X < minX)
                    minX = coordinate.X;
                if (coordinate.Y < minY)
                    minY = coordinate.Y;
                if (coordinate.Z < minZ)
                    minZ = coordinate.Z;
                if (coordinate.X > maxX)
                    maxX = coordinate.X;
                if (coordinate.Y > maxY)
                    maxY = coordinate.Y;
                if (coordinate.Z > maxZ)
                    maxZ = coordinate.Z;
            }

            return new Envelope(minX, maxX, minY, maxY, minZ, maxZ);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on other instances.
        /// </summary>
        /// <param name="envelopes">The envelopes.</param>
        /// <returns>The <see cref="Envelope" /> created from the envelopes.</returns>
        public static Envelope FromEnvelopes(params Envelope[] envelopes)
        {
            if (envelopes == null || !envelopes.AnyElement())
                return null;

            return new Envelope(envelopes.Min(envelope => envelope.minimum.X), envelopes.Max(envelope => envelope.maximum.X),
                                envelopes.Min(envelope => envelope.minimum.Y), envelopes.Max(envelope => envelope.maximum.Y),
                                envelopes.Min(envelope => envelope.minimum.Z), envelopes.Max(envelope => envelope.maximum.Z));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope" /> class based on other instances.
        /// </summary>
        /// <param name="envelopes">The envelopes.</param>
        /// <returns>The <see cref="Envelope" /> created from the envelopes.</returns>
        public static Envelope FromEnvelopes(IEnumerable<Envelope> envelopes)
        {
            if (envelopes == null || !envelopes.AnyElement())
                return null;

            return new Envelope(envelopes.Min(envelope => envelope.minimum.X), envelopes.Max(envelope => envelope.maximum.X),
                                envelopes.Min(envelope => envelope.minimum.Y), envelopes.Max(envelope => envelope.maximum.Y),
                                envelopes.Min(envelope => envelope.minimum.Z), envelopes.Max(envelope => envelope.maximum.Z));
        }

        /// <summary>
        /// Determines whether the envelope contains the specified coordinate.
        /// </summary>
        /// <param name="first">The first coordinate of the envelope.</param>
        /// <param name="second">The second coordinate of the envelope.</param>
        /// <param name="coordinate">The examined coordinate.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate first, Coordinate second, Coordinate coordinate)
        {
            return Math.Min(first.X, second.X) <= coordinate.X && coordinate.X <= Math.Max(first.X, second.X) &&
                   Math.Min(first.Y, second.Y) <= coordinate.Y && coordinate.Y <= Math.Max(first.Y, second.Y) &&
                   Math.Min(first.Z, second.Z) <= coordinate.Z && coordinate.Z <= Math.Max(first.Z, second.Z);
        }

        /// <summary>
        /// Determines whether the first envelope contains the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> contains the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return Math.Min(first.X, second.X) <= Math.Min(third.X, fourth.X) && Math.Max(third.X, fourth.X) <= Math.Max(first.X, second.X) &&
                   Math.Min(first.Y, second.Y) <= Math.Min(third.Y, fourth.Y) && Math.Max(third.Y, fourth.Y) <= Math.Max(first.Y, second.Y) &&
                   Math.Min(first.Z, second.Z) <= Math.Min(third.Z, fourth.Z) && Math.Max(third.Z, fourth.Z) <= Math.Max(first.Z, second.Z);
        }

        /// <summary>
        /// Determines whether the envelope contains the specified coordinate.
        /// </summary>
        /// <param name="envelope">The collection of coordinates that define the envelope.</param>
        /// <param name="coordinate">The examined coordinate.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="envelope"/> contains <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(IEnumerable<Coordinate> envelope, Coordinate coordinate)
        {
            if (envelope == null)
                return false;

            Double minX = Double.MaxValue, minY = Double.MaxValue, minZ = Double.MaxValue, maxX = Double.MinValue, maxY = Double.MinValue, maxZ = Double.MinValue;
            foreach (Coordinate envelopeCoordinate in envelope)
            {
                if (envelopeCoordinate == null)
                    continue;

                if (envelopeCoordinate.X < minX)
                    minX = envelopeCoordinate.X;
                if (envelopeCoordinate.Y < minY)
                    minY = envelopeCoordinate.Y;
                if (envelopeCoordinate.Z < minZ)
                    minZ = envelopeCoordinate.Z;
                if (envelopeCoordinate.X > maxX)
                    maxX = envelopeCoordinate.X;
                if (envelopeCoordinate.Y > maxY)
                    maxY = envelopeCoordinate.Y;
                if (envelopeCoordinate.Z > maxZ)
                    maxZ = envelopeCoordinate.Z;
            }

            return coordinate.X >= minX && coordinate.X <= maxX && coordinate.Y >= minY && coordinate.Y <= maxY && coordinate.Z >= minZ && coordinate.Z <= maxZ;
        }

        /// <summary>
        /// Determines whether the first envelope crosses the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> crosses the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Crosses(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return !Disjoint(first, second, third, fourth) && !Equals(first, second, third, fourth);
        }

        /// <summary>
        /// Determines whether the first envelope is disjoint from the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> is disjoint from the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Disjoint(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return Math.Max(first.X, second.X) < Math.Min(third.X, fourth.X) || Math.Min(first.X, second.X) > Math.Max(third.X, fourth.X) ||
                   Math.Max(first.Y, second.Y) < Math.Min(third.Y, fourth.Y) || Math.Min(first.Y, second.Y) > Math.Max(third.Y, fourth.Y) ||
                   Math.Max(first.Z, second.Z) < Math.Min(third.Z, fourth.Z) || Math.Min(first.Z, second.Z) > Math.Max(third.Z, fourth.Z);
        }

        /// <summary>
        /// Determines whether the first envelope is disjoint from the second envelope.
        /// </summary>
        /// <param name="first">The collection of coordinates that defines the first envelope.</param>
        /// <param name="second">The collection of coordinates that defines the second envelope.</param>
        /// <returns><c>true</c> if the first envelope is disjoint from the second envelope; otherwise, <c>false</c>.</returns>
        public static Boolean Disjoint(IEnumerable<Coordinate> first, IEnumerable<Coordinate> second)
        {
            Double firstMinX = Double.MaxValue, firstMinY = Double.MaxValue, firstMinZ = Double.MaxValue, firstMaxX = Double.MinValue, firstMaxY = Double.MinValue, firstMaxZ = Double.MinValue;
            foreach (Coordinate coordinate in first)
            {
                if (coordinate == null)
                    continue;

                if (coordinate.X < firstMinX)
                    firstMinX = coordinate.X;
                if (coordinate.Y < firstMinY)
                    firstMinY = coordinate.Y;
                if (coordinate.Z < firstMinZ)
                    firstMinZ = coordinate.Z;
                if (coordinate.X > firstMaxX)
                    firstMaxX = coordinate.X;
                if (coordinate.Y > firstMaxY)
                    firstMaxY = coordinate.Y;
                if (coordinate.Z > firstMaxZ)
                    firstMaxZ = coordinate.Z;
            }

            Double secondMinX = Double.MaxValue, secondMinY = Double.MaxValue, secondMinZ = Double.MaxValue, secondMaxX = Double.MinValue, secondMaxY = Double.MinValue, secondMaxZ = Double.MinValue;
            foreach (Coordinate coordinate in second)
            {
                if (coordinate == null)
                    continue;

                if (coordinate.X < secondMinX)
                    secondMinX = coordinate.X;
                if (coordinate.Y < secondMinY)
                    secondMinY = coordinate.Y;
                if (coordinate.Z < secondMinZ)
                    secondMinZ = coordinate.Z;
                if (coordinate.X > secondMaxX)
                    secondMaxX = coordinate.X;
                if (coordinate.Y > secondMaxY)
                    secondMaxY = coordinate.Y;
                if (coordinate.Z > secondMaxZ)
                    secondMaxZ = coordinate.Z;
            }

            return firstMaxX < secondMinX || firstMinX > secondMaxX || firstMaxY < secondMinY || firstMinY > secondMaxY || firstMaxZ < secondMinZ || firstMinZ > secondMaxZ;
        }

        /// <summary>
        /// Determines whether the first envelope is equal to the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> is equal to the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Equals(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return Math.Min(first.X, second.X) == Math.Min(third.X, fourth.X) && Math.Max(third.X, fourth.X) == Math.Max(first.X, second.X) &&
                   Math.Min(first.Y, second.Y) == Math.Min(third.Y, fourth.Y) && Math.Max(third.Y, fourth.Y) == Math.Max(first.Y, second.Y) &&
                   Math.Min(first.Z, second.Z) == Math.Min(third.Z, fourth.Z) && Math.Max(third.Z, fourth.Z) <= Math.Max(first.Z, second.Z);
        }

        /// <summary>
        /// Determines whether the first envelope intersects the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> intersects the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return !Disjoint(first, second, third, fourth);
        }

        /// <summary>
        /// Determines whether the first envelope intersects the second envelope.
        /// </summary>
        /// <param name="first">The collection of coordinates that defines the first envelope.</param>
        /// <param name="second">The collection of coordinates that defines the second envelope.</param>
        /// <returns><c>true</c> if the first envelope intersects the second envelope; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(IEnumerable<Coordinate> first, IEnumerable<Coordinate> second)
        {
            return !Disjoint(first, second);
        }

        /// <summary>
        /// Determines whether the first envelope overlaps the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> overlaps the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Overlaps(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return !Disjoint(first, second, third, fourth) && !Equals(first, second, third, fourth);
        }

        /// <summary>
        /// Determines whether the first envelope touches the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> touches the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Touches(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return !Disjoint(first, second, third, fourth) && (Math.Min(first.X, second.X) == Math.Max(third.X, fourth.X) || Math.Max(first.X, second.X) == Math.Min(third.X, fourth.X) ||
                                                               Math.Min(first.Y, second.Y) == Math.Max(third.Y, fourth.Y) || Math.Max(first.Y, second.Y) == Math.Min(third.Y, fourth.Y) ||
                                                               Math.Min(first.Z, second.Z) == Math.Max(third.Z, fourth.Z) || Math.Max(first.Z, second.Z) == Math.Min(third.Z, fourth.Z));
        }

        /// <summary>
        /// Determines whether the first envelope is within the second envelope.
        /// </summary>
        /// <param name="first">The first coordinate of the first envelope.</param>
        /// <param name="second">The second coordinate of the first envelope.</param>
        /// <param name="third">The first coordinate of the second envelope.</param>
        /// <param name="fourth">The second coordinate of the second envelope.</param>
        /// <returns><c>true</c> if the envelope defined by <paramref name="first" /> and <paramref name="second" /> is within the envelope defined by <paramref name="third" /> and <paramref name="fourth" />; otherwise, <c>false</c>.</returns>
        public static Boolean Within(Coordinate first, Coordinate second, Coordinate third, Coordinate fourth)
        {
            return Contains(third, fourth, first, second);
        }
    }
}
