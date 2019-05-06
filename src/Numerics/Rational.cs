// <copyright file="Rational.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Numerics
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using AEGIS.Numerics.Approximation;
    using AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a rational number.
    /// </summary>
    /// <remarks>
    /// A rational number is composed of two 64 bit integers.
    /// </remarks>
    public partial struct Rational : IComparable<Rational>, IComparable, IEquatable<Rational>
    {
        /// <summary>
        /// Represents the rational value that is not a number. This field is read-only.
        /// </summary>
        public static readonly Rational NaN = new Rational(0, 0);

        /// <summary>
        /// Represents the zero rational value. This field is read-only.
        /// </summary>
        public static readonly Rational Zero = new Rational(0, 1);

        /// <summary>
        /// Represents the smallest positive rational value that is greater than zero. This field is read-only.
        /// </summary>
        public static readonly Rational Epsilon = new Rational(1, Int64.MaxValue);

        /// <summary>
        /// Represents the smallest possible rational value. This field is read-only.
        /// </summary>
        public static readonly Rational MinValue = new Rational(Int64.MinValue, 1);

        /// <summary>
        /// Represents the larges possible rational value. This field is read-only.
        /// </summary>
        public static readonly Rational MaxValue = new Rational(Int64.MaxValue, 1);

        /// <summary>
        /// Represents the rational value of positive infinity. This field is read-only.
        /// </summary>
        public static readonly Rational PositiveInfinity = new Rational(1, 0);

        /// <summary>
        /// Represents the rational value of negative infinity. This field is read-only.
        /// </summary>
        public static readonly Rational NegativeInfinity = new Rational(-1, 0);

        /// <summary>
        /// The upper limit on the number of approximations for creating continued fractions. This field is constant.
        /// </summary>
        private const Int32 ApproximationIterationLimit = 100;

        /// <summary>
        /// The string format of a rational number. This field is constant.
        /// </summary>
        private const String RationalStringFormat = "{0}/{1}";

        /// <summary>
        /// The string format of a not a number rational number. This field is constant.
        /// </summary>
        private const String NaNStringFormat = "NaN";

        /// <summary>
        /// The numerator. This field is read-only.
        /// </summary>
        private readonly Int64 numerator;

        /// <summary>
        /// The denominator. This field is read-only.
        /// </summary>
        private readonly Int64 denominator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational" /> struct.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The denominator is -9223372036854775808.</exception>
        public Rational(Int64 numerator, Int64 denominator)
        {
            if (denominator == 0)
            {
                this.numerator = 0;
                this.denominator = 0;

                if (numerator > 0)
                    this.numerator = 1;
                if (numerator < 0)
                    this.numerator = -1;

                return;
            }

            if (numerator == 0)
            {
                this.numerator = 0;
                this.denominator = 1;
                return;
            }

            if (denominator == Int64.MinValue)
                throw new ArgumentOutOfRangeException(nameof(denominator), NumericsMessages.DenominatorIsInt64Min);

            if (denominator < 0)
            {
                this.numerator = -numerator;
                this.denominator = -denominator;
            }
            else
            {
                this.numerator = numerator;
                this.denominator = denominator;
            }

            Int64 divisor = (Int64)Calculator.GreatestCommonDivisor(this.numerator, this.denominator);
            this.numerator /= divisor;
            this.denominator /= divisor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational" /> struct.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        public Rational(Int64 numerator)
            : this(numerator, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational"/> struct.
        /// </summary>
        /// <param name="other">The other rational number.</param>
        public Rational(Rational other)
            : this(other.numerator, other.denominator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rational"/> struct.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public Rational(Int64 numerator, Rational denominator)
            : this(numerator * denominator.denominator, denominator.numerator)
        {
        }

        /// <summary>
        /// Gets the numerator.
        /// </summary>
        /// <value>A 32-bit signed integer representing the numerator of the rational number.</value>
        public Int64 Numerator
        {
            get { return this.numerator; }
        }

        /// <summary>
        /// Gets the denominator.
        /// </summary>
        /// <value>A 32-bit signed integer representing the denominator of the rational number.</value>
        public Int64 Denominator
        {
            get { return this.denominator; }
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
            if (Single.IsNaN(value))
                return Rational.NaN;
            if (value == 0)
                return Rational.Zero;

            return FromSingle(value, Math.Abs(value) / 10000000);
        }

        /// <summary>
        /// Converts the specified value to a rational number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rational equivalent of <paramref name="value" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The value is too large or too small to be converted to rational.</exception>
        public static explicit operator Rational(Double value)
        {
            if (Double.IsNaN(value))
                return Rational.NaN;
            if (value == 0)
                return Rational.Zero;

            return FromDouble(value, Math.Abs(value) / 10000000);
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
        /// Indicates whether the first specified rational number is less than the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is less than the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Rational rational, Int64 value)
        {
            return rational.CompareTo(value) < 0;
        }

        /// <summary>
        /// Indicates whether the first specified rational number is greater than the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is greater than the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Int64 value, Rational rational)
        {
            return rational.CompareTo(value) > 0;
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
        /// Indicates whether the first specified rational number is greater than the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is greater than the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Rational rational, Int64 value)
        {
            return rational.CompareTo(value) > 0;
        }

        /// <summary>
        /// Indicates whether the first specified rational number is less than the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is less than the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Int64 value, Rational rational)
        {
            return rational.CompareTo(value) < 0;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is less than or equal to the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is less than or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Rational first, Rational second)
        {
            return first.CompareTo(second) <= 0;
        }

        /// <summary>
        /// Indicates whether the first specified rational number is less than or equal to the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is less than or equal to the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Rational rational, Int64 value)
        {
            return rational.CompareTo(value) <= 0;
        }

        /// <summary>
        /// Indicates whether the first specified rational number is greater or equal to the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is greater or equal to the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Int64 value, Rational rational)
        {
            return rational.CompareTo(value) >= 0;
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Rational" /> instance is greater than or equal to the second.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns><c>true</c> if the first <see cref="Rational" /> instance is greater than or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Rational first, Rational second)
        {
            return first.CompareTo(second) >= 0;
        }

        /// <summary>
        /// Indicates whether the first specified rational number is less than or equal to the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is less than or equal to the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Int64 value, Rational rational)
        {
            return rational.CompareTo(value) <= 0;
        }

        /// <summary>
        /// Indicates whether the first specified rational number is greater or equal to the integer value.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer value.</param>
        /// <returns><c>true</c> if the rational number is greater or equal to the integer value; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Rational rational, Int64 value)
        {
            return rational.CompareTo(value) >= 0;
        }

        /// <summary>
        /// Sums the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The sum of the two <see cref="Rational" /> instances.</returns>
        /// <exception cref="System.OverflowException">
        /// Numerator is too large to evaluate.
        /// or
        /// Denominator is too large to evaluate.
        /// </exception>
        public static Rational operator +(Rational first, Rational second)
        {
            if (IsNaN(first) || IsNaN(second))
                return Rational.NaN;

            if (IsZero(first))
                return second;

            if (IsZero(second))
                return first;

            try
            {
                if (first.Denominator == second.Denominator)
                {
                    return new Rational(checked(first.numerator + second.numerator), first.denominator);
                }

                return new Rational(checked(first.Numerator * second.Denominator + second.Numerator * first.Denominator), checked(first.Denominator * second.Denominator));
            }
            catch (OverflowException)
            {
                return EvaluateBigRational((BigInteger)first.Numerator * second.Denominator + second.Numerator * first.Denominator, (BigInteger)first.Denominator * second.Denominator);
            }
        }

        /// <summary>
        /// Sums the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The sum of the rational and integer numbers.</returns>
        /// <exception cref="System.OverflowException">
        /// Numerator is too large to evaluate.
        /// </exception>
        public static Rational operator +(Rational rational, Int64 value)
        {
            return rational + (Rational)value;
        }

        /// <summary>
        /// Sums the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The sum of the rational and integer numbers.</returns>
        /// <exception cref="System.OverflowException">
        /// Numerator is too large to evaluate.
        /// </exception>
        public static Rational operator +(Int64 value, Rational rational)
        {
            return (Rational)value + rational;
        }

        /// <summary>
        /// Extracts the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The extract of the two <see cref="Rational" /> instances.</returns>
        public static Rational operator -(Rational first, Rational second)
        {
            return first + (-second);
        }

        /// <summary>
        /// Extracts the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The extract of the rational and integer numbers.</returns>
        /// <exception cref="System.OverflowException">
        /// Numerator is too large to evaluate.
        /// </exception>
        public static Rational operator -(Rational rational, Int64 value)
        {
            return rational - (Rational)value;
        }

        /// <summary>
        /// Extracts the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The extract of the rational and integer numbers.</returns>
        /// <exception cref="System.OverflowException">
        /// Numerator is too large to evaluate.
        /// </exception>
        public static Rational operator -(Int64 value, Rational rational)
        {
            return (Rational)value - rational;
        }

        /// <summary>
        /// Inverts the specified <see cref="Rational" />.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <returns>The inverted <see cref="Rational" />.</returns>
        public static Rational operator -(Rational rational)
        {
            return new Rational(-1 * rational.numerator, rational.denominator);
        }

        /// <summary>
        /// Multiplies the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The product of the specified <see cref="Rational" /> instances.</returns>
        /// <exception cref="System.OverflowException">
        /// Numerator is too large to evaluate.
        /// or
        /// Denominator is too large to evaluate.
        /// </exception>
        public static Rational operator *(Rational first, Rational second)
        {
            if (IsNaN(first) || IsNaN(second))
                return Rational.NaN;
            if (IsZero(first) || IsZero(second))
                return Rational.Zero;

            try
            {
                return new Rational(checked(first.numerator * second.numerator), checked(first.denominator * second.denominator));
            }
            catch (OverflowException)
            {
                return EvaluateBigRational((BigInteger)first.numerator * second.numerator, (BigInteger)first.denominator * second.denominator);
            }
        }

        /// <summary>
        /// Multiplies the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The product of the rational and integer numbers.</returns>
        public static Rational operator *(Rational rational, Int64 value)
        {
            return rational * (Rational)value;
        }

        /// <summary>
        /// Multiplies the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The product of the rational and integer numbers.</returns>
        public static Rational operator *(Int64 value, Rational rational)
        {
            return (Rational)value * rational;
        }

        /// <summary>
        /// Divides the specified <see cref="Rational" /> instances.
        /// </summary>
        /// <param name="first">The first rational number.</param>
        /// <param name="second">The second rational number.</param>
        /// <returns>The quotient of the specified <see cref="Rational" /> instances.</returns>
        public static Rational operator /(Rational first, Rational second)
        {
            if (IsNaN(first) || IsNaN(second) || IsZero(second))
                return Rational.NaN;
            if (IsZero(first))
                return Rational.Zero;
            if (IsInfinity(first))
            {
                if (IsInfinity(second))
                    return Rational.NaN;

                return Math.Sign(second.numerator) * first;
            }

            if (IsInfinity(second))
                return Rational.Zero;

            try
            {
                return new Rational(checked(first.numerator * second.denominator), checked(first.denominator * second.numerator));
            }
            catch (OverflowException)
            {
                return EvaluateBigRational((BigInteger)first.numerator * second.denominator, (BigInteger)first.denominator * second.numerator);
            }
        }

        /// <summary>
        /// Divides the specified rational and integer numbers.
        /// </summary>
        /// <param name="rational">The rational number.</param>
        /// <param name="value">The integer.</param>
        /// <returns>The quotient of the rational and integer numbers.</returns>
        public static Rational operator /(Rational rational, Int64 value)
        {
            return rational / (Rational)value;
        }

        /// <summary>
        /// Divides the specified rational and integer numbers.
        /// </summary>
        /// <param name="value">The integer.</param>
        /// <param name="rational">The rational number.</param>
        /// <returns>The quotient of the rational and integer numbers.</returns>
        public static Rational operator /(Int64 value, Rational rational)
        {
            return (Rational)value / rational;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings:
        /// Value Meaning Less than zero This instance is less than <paramref name="other" />.
        /// Zero This instance is equal to <paramref name="other" />.
        /// Greater than zero This instance is greater than <paramref name="other" />.
        /// </returns>
        public Int32 CompareTo(Rational other)
        {
            if (this.denominator == other.denominator)
            {
                if (IsNaN(this))
                    return IsNaN(other) ? 0 : -1;
                if (IsNaN(other))
                    return 1;

                return this.numerator.CompareTo(other.numerator);
            }

            if (this.denominator == 0)
                return this.numerator == 1 ? 1 : -1;

            if (other.denominator == 0)
                return other.numerator == 1 ? -1 : 1;

            try
            {
                return checked(this.numerator * other.denominator).CompareTo(checked(other.numerator * this.denominator));
            }
            catch (OverflowException)
            {
                BigInteger first = (BigInteger)this.numerator * other.denominator;
                BigInteger second = (BigInteger)other.numerator * this.denominator;
                return first.CompareTo(second);
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings:
        /// Value Meaning Less than zero This instance is less than <paramref name="obj" />.
        /// Zero This instance is equal to <paramref name="obj" />.
        /// Greater than zero This instance is greater than <paramref name="obj" />.</returns>
        /// <exception cref="System.ArgumentException">The object is not comparable with a rational number.</exception>
        public Int32 CompareTo(Object obj)
        {
            if (ReferenceEquals(obj, null))
                return 1;

            if (obj is Rational)
                return this.CompareTo((Rational)obj);

            try
            {
                Int64 number = Convert.ToInt64(obj);

                try
                {
                    return this.numerator.CompareTo(checked(number * this.denominator));
                }
                catch (OverflowException)
                {
                    return -Math.Sign(number);
                }
            }
            catch
            {
                throw new ArgumentException(NumericsMessages.ObjectIsNotComparableWithRational, nameof(obj));
            }
        }

        /// <summary>
        /// Indicates whether this instance and a specified other rational number are equal.
        /// </summary>
        /// <param name="other">Another rational number to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Rational other)
        {
            return this.numerator == other.numerator && this.denominator == other.denominator;
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

            if (obj is Rational)
                return this.Equals((Rational)obj);

            if (this.denominator == 1)
            {
                try
                {
                    Int64 number = Convert.ToInt64(obj);

                    return this.numerator.Equals(number);
                }
                catch
                {
                }
            }

            return false;
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
            if (IsNaN(this))
                return NaNStringFormat;

            if (this.numerator == 0)
                return 0.ToString(CultureInfo.InvariantCulture);

            if (this.denominator == 1)
                return this.numerator.ToString(CultureInfo.InvariantCulture);

            return String.Format(CultureInfo.InvariantCulture, RationalStringFormat, this.numerator, this.denominator);
        }

        /// <summary>
        /// Returns a value indicating whether the specified rational is not a number (NaN).
        /// </summary>
        /// <param name="number">The rational number.</param>
        /// <returns><c>true</c> if the value of <paramref name="number"/> is equal to <see cref="Rational.NaN"/>; otherwise, <c>false</c>.</returns>
        public static Boolean IsNaN(Rational number)
        {
            return number.numerator == 0 && number.denominator == 0;
        }

        /// <summary>
        /// Returns a value indicating whether the specified rational is zero.
        /// </summary>
        /// <param name="number">The rational number.</param>
        /// <returns><c>true</c> if the value of <paramref name="number"/> is equal to <see cref="Rational.Zero"/>; otherwise, <c>false</c>.</returns>
        public static Boolean IsZero(Rational number)
        {
            return number.numerator == 0 && number.denominator == 1;
        }

        /// <summary>
        /// Returns a value indicating whether the specified rational is infinity.
        /// </summary>
        /// <param name="number">The rational number.</param>
        /// <returns><c>true</c> if the value of <paramref name="number"/> is equal to <see cref="Rational.PositiveInfinity"/> or <see cref="Rational.NegativeInfinity"/>; otherwise, <c>false</c>.</returns>
        public static Boolean IsInfinity(Rational number)
        {
            return Math.Abs(number.numerator) == 1 && number.denominator == 0;
        }

        /// <summary>
        /// Computes the absolute value of a rational number.
        /// </summary>
        /// <param name="number">The rational number.</param>
        /// <returns>The absolute value of <paramref name="number"/>.</returns>
        public static Rational Abs(Rational number)
        {
            if (number.Numerator >= 0)
                return number;

            return new Rational(-number.Numerator, number.Denominator);
        }

        /// <summary>
        /// Creates a rational number from the specified single-precision floating point number.
        /// </summary>
        /// <param name="value">The number to approximate.</param>
        /// <param name="precision">The precision of approximation.</param>
        /// <returns>The nearest rational number to <paramref name="value"/>.</returns>
        public static Rational FromSingle(Single value, Single precision)
        {
            return FromDouble(value, precision);
        }

        /// <summary>
        /// Creates a rational number from the specified double-precision floating point number.
        /// </summary>
        /// <param name="value">The number to approximate.</param>
        /// <param name="precision">The precision of approximation.</param>
        /// <returns>The nearest rational number to <paramref name="value"/>.</returns>
        public static Rational FromDouble(Double value, Double precision)
        {
            if (Math.Abs(value) > Int64.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), NumericsMessages.RationalConversionOutOfRange);

            return new ContinuedFractionApproximator(ApproximationIterationLimit).GetNearestRational(value, precision);
        }

        /// <summary>
        /// Evaluates a rational value with overflowing numerator or denominator.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns>The evaluated rational number.</returns>
        /// <exception cref="System.OverflowException">
        /// Numerator is too large to evaluate.
        /// or
        /// Denominator is too large to evaluate.
        /// </exception>
        private static Rational EvaluateBigRational(BigInteger numerator, BigInteger denominator)
        {
            Int32 sign = BigInteger.Abs(numerator) == numerator ? 1 : -1;
            numerator = BigInteger.Abs(numerator);

            BigInteger divisor = Calculator.GreatestCommonDivisor(numerator, denominator);
            numerator = numerator / divisor;
            denominator = denominator / divisor;

            if (numerator / denominator >= Int64.MaxValue)
                return sign * Rational.PositiveInfinity;

            if (numerator > Int64.MaxValue)
                throw new OverflowException(NumericsMessages.NumeratorTooLargeToEvaluate);
            if (denominator > Int64.MaxValue)
                throw new OverflowException(NumericsMessages.DenominatorTooLargeToEvaluate);

            return new Rational(sign * (Int64)numerator, (Int64)denominator);
        }
    }
}
