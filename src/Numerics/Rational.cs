// <copyright file="Rational.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics
{
    using System;
    using System.Globalization;
    using ELTE.AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a rational number.
    /// </summary>
    /// <remarks>
    /// A rational number is composed of two 32 bit integers. A rational number is considered invalid, if the denominator is zero.
    /// </remarks>
    public struct Rational : IComparable<Rational>, IComparable, IEquatable<Rational>
    {
        /// <summary>
        /// Represents the zero rational value. This field is read-only.
        /// </summary>
        public static readonly Rational Zero = new Rational(0, 1);

        /// <summary>
        /// The string format of a rational number. This field is constant.
        /// </summary>
        private const String RationalStringFormat = "{0}/{1}";

        /// <summary>
        /// The numerator. This field is read-only.
        /// </summary>
        private readonly Int32 numerator;

        /// <summary>
        /// The denominator. This field is read-only.
        /// </summary>
        private readonly Int32 denominator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational" /> struct.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <exception cref="System.ArgumentException">The denominator is 0.</exception>
        public Rational(Int32 numerator, Int32 denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException(NumericsMessages.DenomatorIs0, nameof(denominator));
            }

            if (numerator == 0)
            {
                this.numerator = 0;
                this.denominator = 1;
            }
            else
            {
                Int32 sign = ((numerator > 0 && denominator > 0) || (numerator < 0 && denominator < 0)) ? 1 : -1;
                this.numerator = sign * Math.Abs(numerator);
                this.denominator = Math.Abs(denominator);
                Int32 divisor = Calculator.GreatestCommonDivisor(this.numerator, this.denominator);
                this.numerator /= divisor;
                this.denominator /= divisor;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational" /> struct.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        public Rational(Int32 numerator)
            : this(numerator, 1)
        {
        }

        /// <summary>
        /// Gets the numerator.
        /// </summary>
        /// <value>A 32-bit signed integer representing the numerator of the rational number.</value>
        public Int32 Numerator
        {
            get { return this.numerator; }
        }

        /// <summary>
        /// Gets the denominator.
        /// </summary>
        /// <value>A 32-bit signed integer representing the denominator of the rational number.</value>
        public Int32 Denominator
        {
            get { return this.denominator; }
        }

        /// <summary>
        /// Determines whether the specified rational is valid.
        /// </summary>
        /// <param name="value">The rational.</param>
        /// <returns><c>true</c> if the denominator of the ration is not zero; otherwise <c>false</c>.</returns>
        public Boolean IsValid(Rational value)
        {
            return value.denominator != 0;
        }

        /// <summary>
        /// Converts the specified value to a rational number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rational equivalent of <paramref name="value" />.</returns>
        public static explicit operator Rational(Int32 value)
        {
            return new Rational(value);
        }

        /// <summary>
        /// Converts the specified value to a rational number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rational equivalent of <paramref name="value" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The value is too large or too small to be converted to rational.</exception>
        public static explicit operator Rational(Single value)
        {
            if (Math.Abs(value) > Int32.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), NumericsMessages.RationalConversionOutOfRange);

            if (Math.Abs(value) > (1 << 20))
                return new Rational((Int32)value, 1);

            if (Math.Abs(value) > (1 << 10))
                return new Rational((Int32)(value * (1 << 10)), 1 << 10);

            return new Rational((Int32)(value * (1 << 20)), 1 << 20);
        }

        /// <summary>
        /// Converts the specified value to a rational number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rational equivalent of <paramref name="value" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The value is too large or too small to be converted to rational.</exception>
        public static explicit operator Rational(Double value)
        {
            if (Math.Abs(value) > Int32.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), NumericsMessages.RationalConversionOutOfRange);

            if (Math.Abs(value) > (1 << 20))
                return new Rational((Int32)value, 1);

            if (Math.Abs(value) > (1 << 10))
                return new Rational((Int32)(value * (1 << 10)), 1 << 10);

            return new Rational((Int32)(value * (1 << 20)), 1 << 20);
        }

        /// <summary>
        /// Converts the specified value to a 32 bit signed integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The 32 bit signed integer equivalent of <paramref name="value" />.</returns>
        public static explicit operator Int32(Rational value)
        {
            return value.numerator / value.denominator;
        }

        /// <summary>
        /// Converts the specified value to a 64 bit signed integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The 64 bit signed integer equivalent of <paramref name="value" />.</returns>
        public static explicit operator Int64(Rational value)
        {
            return value.numerator / value.denominator;
        }

        /// <summary>
        /// Converts the specified value to a single precision floating point number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The single precision floating point number equivalent of <paramref name="value" />.</returns>
        public static explicit operator Single(Rational value)
        {
            return (Single)value.numerator / value.denominator;
        }

        /// <summary>
        /// Converts the specified value to a double precision floating point number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The double precision floating point number equivalent of <paramref name="value" />.</returns>
        public static explicit operator Double(Rational value)
        {
            return (Double)value.numerator / value.denominator;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Rational" /> instances are equal.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Rational first, Rational second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Rational" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Rational first, Rational second)
        {
            return !first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is less than the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is less than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Rational first, Rational second)
        {
            return first.CompareTo(second) < 0;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is greater than the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is greater than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Rational first, Rational second)
        {
            return first.CompareTo(second) > 0;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is smaller or equal to the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is smaller or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Rational first, Rational second)
        {
            return first.CompareTo(second) <= 0;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is greater or equal to the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is greater or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Rational first, Rational second)
        {
            return first.CompareTo(second) >= 0;
        }

        /// <summary>
        /// Sums the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The sum of the two <see cref="Rational" /> instances.</returns>
        public static Rational operator +(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero))
                return second;

            if (second.Equals(Rational.Zero))
                return first;

            if (first.Denominator == second.Denominator)
                return new Rational(first.Numerator + second.Numerator, first.Denominator);

            return new Rational(first.Numerator * second.Denominator + second.Numerator * first.Denominator, first.Denominator * second.Denominator);
        }

        /// <summary>
        /// Sums the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The sum of the rational and integer numbers.</returns>
        public static Rational operator +(Rational rational, Int32 value)
        {
            if (rational.Equals(Rational.Zero))
                return new Rational(value);

            if (value == 0)
                return rational;

            return new Rational(rational.Numerator + value * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Sums the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The sum of the rational and integer numbers.</returns>
        public static Rational operator +(Int32 value, Rational rational)
        {
            if (rational.Equals(Rational.Zero))
                return new Rational(value);

            if (value == 0)
                return rational;

            return new Rational(rational.Numerator + value * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Extracts the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The extract of the two <see cref="Rational" /> instances.</returns>
        public static Rational operator -(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero))
                return -second;

            if (second.Equals(Rational.Zero))
                return first;

            if (first.Denominator == second.Denominator)
                return new Rational(first.Numerator - second.Numerator, first.Denominator);

            return new Rational(first.Numerator * second.Denominator - second.Numerator * first.Denominator, first.Denominator * second.Denominator);
        }

        /// <summary>
        /// Extracts the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The extract of the rational and integer numbers.</returns>
        public static Rational operator -(Rational rational, Int32 value)
        {
            if (rational.Equals(Rational.Zero))
                return new Rational(value);

            if (value == 0)
                return rational;

            return new Rational(rational.Numerator - value * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Extracts the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The extract of the rational and integer numbers.</returns>
        public static Rational operator -(Int32 value, Rational rational)
        {
            if (rational.Equals(Rational.Zero))
                return new Rational(value);

            if (value == 0)
                return rational;

            return new Rational(rational.Numerator - value * rational.Denominator, rational.Denominator);
        }

        /// <summary>
        /// Inverts the specified <see cref="Rational" />.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <returns>The inverted <see cref="Rational" />.</returns>
        public static Rational operator -(Rational rational)
        {
            return new Rational(-1 * rational.Numerator, rational.Denominator);
        }

        /// <summary>
        /// Multiplies the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The product of the specified <see cref="Rational" /> instances.</returns>
        public static Rational operator *(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero) || second.Equals(Rational.Zero))
                return Rational.Zero;

            return new Rational(first.Numerator * second.Numerator, first.Denominator * second.Denominator);
        }

        /// <summary>
        /// Multiplies the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The product of the rational and integer numbers.</returns>
        public static Rational operator *(Rational rational, Int32 value)
        {
            if (rational.Equals(Rational.Zero) || value == 0)
                return Rational.Zero;

            return new Rational(rational.Numerator * value, rational.Denominator);
        }

        /// <summary>
        /// Multiplies the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The product of the rational and integer numbers.</returns>
        public static Rational operator *(Int32 value, Rational rational)
        {
            if (rational.Equals(Rational.Zero) || value == 0)
                return Rational.Zero;

            return new Rational(rational.Numerator * value, rational.Denominator);
        }

        /// <summary>
        /// Divides the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The quotient of the specified <see cref="Rational" /> instances.</returns>
        public static Rational operator /(Rational first, Rational second)
        {
            if (first.Equals(Rational.Zero))
                return Rational.Zero;

            return new Rational(first.Numerator * second.Denominator, first.Denominator * second.Numerator);
        }

        /// <summary>
        /// Divides the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The quotient of the rational and integer numbers.</returns>
        public static Rational operator /(Rational rational, Int32 value)
        {
            if (rational.Equals(Rational.Zero))
                return Rational.Zero;

            return new Rational(rational.Numerator, rational.Denominator * value);
        }

        /// <summary>
        /// Divides the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The quotient of the rational and integer numbers.</returns>
        public static Rational operator /(Int32 value, Rational rational)
        {
            if (value == 0)
                return Rational.Zero;

            return new Rational(rational.Denominator * value, rational.Numerator);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="other" />. Zero This instance is equal to <paramref name="other" />. Greater than zero This instance is greater than <paramref name="other" />.</returns>
        public Int32 CompareTo(Rational other)
        {
            if (ReferenceEquals(other, this))
                return 0;

            return (1.0 * this.numerator / this.denominator).CompareTo(1.0 * other.numerator / other.denominator);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj" />. Zero This instance is equal to <paramref name="obj" />. Greater than zero This instance is greater than <paramref name="obj" />.</returns>
        /// <exception cref="System.ArgumentNullException">The object is null.</exception>
        /// <exception cref="System.ArgumentException">The object is not comparable with a rational number.</exception>
        public Int32 CompareTo(Object obj)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException(nameof(obj), NumericsMessages.ObjectIsNull);
            if (ReferenceEquals(obj, this))
                return 0;

            if (obj is Rational)
                return this.CompareTo((Rational)obj);

            IComparable comparable = obj as IComparable;

            if (comparable != null)
                return comparable.CompareTo(1.0 * this.numerator / this.denominator);

            throw new ArgumentException(NumericsMessages.ObjectIsNotComparableWithRational, "obj");
        }

        /// <summary>
        /// Indicates whether this instance and a specified other rational number are equal.
        /// </summary>
        /// <param name="other">Another rational number to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Rational other)
        {
            if (ReferenceEquals(other, this))
                return true;

            return (1.0 * this.numerator / this.denominator).Equals(1.0 * other.numerator / other.denominator);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(obj, this))
                return true;

            if (obj is Rational)
                return this.Equals((Rational)obj);

            return obj.Equals((Single)this.numerator / this.denominator);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (this.numerator.GetHashCode() >> 2) ^ this.denominator.GetHashCode() ^ 295200001;
            }
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the value and unit symbol of the instance.</returns>
        public override String ToString()
        {
            if (this.numerator == 0)
                return 0.ToString(CultureInfo.InvariantCulture);

            if (this.denominator == 1)
                return this.numerator.ToString(CultureInfo.InvariantCulture);

            return String.Format(CultureInfo.InvariantCulture, RationalStringFormat, this.numerator, this.denominator);
        }
    }
}
