// <copyright file="RationalTest.cs" company="Eötvös Loránd University (ELTE)">
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

#pragma warning disable SA1131 // Use readable conditions
#pragma warning disable CS1718 // Comparison made to same variable

namespace AEGIS.Tests.Numerics
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Numerics;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Rational" /> class.
    /// </summary>
    [TestFixture]
    public class RationalTest
    {
        /// <summary>
        /// Tests constant values of the <see cref="Rational" /> class.
        /// </summary>
        [Test]
        public void RationalValuesTest()
        {
            Rational rational = Rational.Zero;
            rational.Numerator.ShouldBe(0);
            rational.Denominator.ShouldBe(1);

            rational = Rational.NaN;
            rational.Numerator.ShouldBe(0);
            rational.Denominator.ShouldBe(0);

            rational = Rational.Epsilon;
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(Int64.MaxValue);

            rational = Rational.MinValue;
            rational.Numerator.ShouldBe(Int64.MinValue);
            rational.Denominator.ShouldBe(1);

            rational = Rational.MaxValue;
            rational.Numerator.ShouldBe(Int64.MaxValue);
            rational.Denominator.ShouldBe(1);

            rational = Rational.PositiveInfinity;
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(0);

            rational = Rational.NegativeInfinity;
            rational.Numerator.ShouldBe(-1);
            rational.Denominator.ShouldBe(0);
        }

        /// <summary>
        /// Tests constructors of the <see cref="Rational" /> class.
        /// </summary>
        [Test]
        public void RationalConstructorTest()
        {
            // generic value
            Rational rational = new Rational(13, 17);
            rational.Numerator.ShouldBe(13);
            rational.Denominator.ShouldBe(17);

            // truncated values
            rational = new Rational(1024, 2048);
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(2);

            rational = new Rational(35, 25);
            rational.Numerator.ShouldBe(7);
            rational.Denominator.ShouldBe(5);

            // zero value
            rational = new Rational(0, 123);
            rational.Numerator.ShouldBe(0);
            rational.Denominator.ShouldBe(1);

            // negative values
            rational = new Rational(-5, 7);
            rational.Numerator.ShouldBe(-5);
            rational.Denominator.ShouldBe(7);

            rational = new Rational(25, -35);
            rational.Numerator.ShouldBe(-5);
            rational.Denominator.ShouldBe(7);

            rational = new Rational(-30, -42);
            rational.Numerator.ShouldBe(5);
            rational.Denominator.ShouldBe(7);

            // only numerator
            rational = new Rational(5);
            rational.Numerator.ShouldBe(5);
            rational.Denominator.ShouldBe(1);

            rational = new Rational(-5);
            rational.Numerator.ShouldBe(-5);
            rational.Denominator.ShouldBe(1);

            // copy constructor
            rational = new Rational(new Rational(1, 2));
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(2);

            // maximum/minimum value
            rational = new Rational(Int64.MaxValue, Int64.MaxValue);
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(1);

            rational = new Rational(-Int64.MaxValue, Int64.MaxValue);
            rational.Numerator.ShouldBe(-1);
            rational.Denominator.ShouldBe(1);

            rational = new Rational(Int64.MinValue, Int64.MaxValue);
            rational.Numerator.ShouldBe(Int64.MinValue);
            rational.Denominator.ShouldBe(Int64.MaxValue);

            rational = new Rational(Int64.MaxValue, 1);
            rational.Numerator.ShouldBe(Int64.MaxValue);
            rational.Denominator.ShouldBe(1);

            rational = new Rational(Int64.MinValue, 1);
            rational.Numerator.ShouldBe(Int64.MinValue);
            rational.Denominator.ShouldBe(1);

            rational = new Rational(0, Int64.MinValue);
            rational.Numerator.ShouldBe(0);
            rational.Denominator.ShouldBe(1);

            Should.Throw<ArgumentOutOfRangeException>(() => rational = new Rational(1, Int64.MinValue));
        }

        /// <summary>
        /// Tests cast operators of the <see cref="Rational" /> class.
        /// </summary>
        [Test]
        public void RationalCastTest()
        {
            // integer
            Rational rational = (Rational)1;
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(1);
            ((Int32)rational).ShouldBe(1);

            rational = (Rational)13;
            rational.Numerator.ShouldBe(13);
            rational.Denominator.ShouldBe(1);
            ((Int32)rational).ShouldBe(13);

            rational = (Rational)(-13);
            rational.Numerator.ShouldBe(-13);
            rational.Denominator.ShouldBe(1);
            ((Int32)rational).ShouldBe(-13);

            // single
            rational = (Rational)0.0f;
            rational.Numerator.ShouldBe(0);
            rational.Denominator.ShouldBe(1);
            ((Single)rational).ShouldBe(0);

            rational = (Rational)1.5f;
            rational.Numerator.ShouldBe(3);
            rational.Denominator.ShouldBe(2);
            ((Single)rational).ShouldBe(1.5f);

            rational = (Rational)(1 / 3.0f);
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(3);
            ((Single)rational).ShouldBe(1 / 3.0f);

            rational = (Rational)1.25f;
            rational.Numerator.ShouldBe(5);
            rational.Denominator.ShouldBe(4);
            ((Single)rational).ShouldBe(1.25f);

            rational = (Rational)(-1.25f);
            rational.Numerator.ShouldBe(-5);
            rational.Denominator.ShouldBe(4);
            ((Single)rational).ShouldBe(-1.25f);

            rational = (Rational)Single.NaN;
            Rational.IsNaN(rational);
            ((Single)rational).ShouldBe(Single.NaN);

            // double
            rational = (Rational)0.0;
            rational.Numerator.ShouldBe(0);
            rational.Denominator.ShouldBe(1);
            ((Double)rational).ShouldBe(0);

            rational = (Rational)1.5;
            rational.Numerator.ShouldBe(3);
            rational.Denominator.ShouldBe(2);
            ((Double)rational).ShouldBe(1.5);

            rational = (Rational)(1 / 3.0);
            rational.Numerator.ShouldBe(1);
            rational.Denominator.ShouldBe(3);
            ((Double)rational).ShouldBe(1 / 3.0);

            rational = (Rational)1.25;
            rational.Numerator.ShouldBe(5);
            rational.Denominator.ShouldBe(4);
            ((Double)rational).ShouldBe(1.25);

            rational = (Rational)(-1.25);
            rational.Numerator.ShouldBe(-5);
            rational.Denominator.ShouldBe(4);
            ((Double)rational).ShouldBe(-1.25);

            rational = (Rational)Double.NaN;
            Rational.IsNaN(rational);
            ((Double)rational).ShouldBe(Double.NaN);

            Should.Throw<ArgumentOutOfRangeException>(() => rational = (Rational)((Double)Int64.MaxValue * 2));
        }

        /// <summary>
        /// Tests equality of <see cref="Rational" /> instances.
        /// </summary>
        [Test]
        public void RationalEqualityTest()
        {
            Rational rational = new Rational(1, 2);
            Rational equal = new Rational(2, 4);
            Rational notEqual = new Rational(3, 1);

            // operators
            (rational == rational).ShouldBeTrue();
            (rational == equal).ShouldBeTrue();
            (rational == notEqual).ShouldBeFalse();

            (rational != equal).ShouldBeFalse();
            (rational != notEqual).ShouldBeTrue();

            // equality with rational
            rational.Equals(rational).ShouldBeTrue();
            rational.Equals(equal).ShouldBeTrue();
            rational.Equals(notEqual).ShouldBeFalse();

            // equality with object
            rational.Equals((Object)rational).ShouldBeTrue();
            rational.Equals((Object)equal).ShouldBeTrue();
            rational.Equals((Object)notEqual).ShouldBeFalse();
            rational.Equals(null).ShouldBeFalse();
            rational.Equals(new Object()).ShouldBeFalse();
            Rational.Zero.Equals(new Object()).ShouldBeFalse();
            Rational.Zero.Equals(0).ShouldBeTrue();
            Rational.MinValue.Equals(Int64.MinValue).ShouldBeTrue();
            Rational.MaxValue.Equals(Int64.MaxValue).ShouldBeTrue();

            // extrema
            Rational positiveInfinity = new Rational(10, 0);
            positiveInfinity.Equals(Rational.PositiveInfinity).ShouldBeTrue();

            Rational negativeInfinity = new Rational(-10, 0);
            negativeInfinity.Equals(Rational.NegativeInfinity).ShouldBeTrue();

            Rational.PositiveInfinity.Equals(Rational.PositiveInfinity).ShouldBeTrue();
            Rational.NegativeInfinity.Equals(Rational.NegativeInfinity).ShouldBeTrue();
            Rational.NaN.Equals(Rational.NaN).ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="Rational.GetHashCode()"/> method.
        /// </summary>
        [Test]
        public void RationalGetHashCodeTest()
        {
            Rational first = new Rational(1, 2);
            Rational second = new Rational(2, 4);
            Rational third = new Rational(1, 3);

            first.GetHashCode().ShouldBe(second.GetHashCode());
            first.GetHashCode().ShouldNotBe(third.GetHashCode());
        }

        /// <summary>
        /// Tests comparison of <see cref="Rational"/> instances.
        /// </summary>
        [Test]
        public void RationalComparisonTest()
        {
            Rational rational = new Rational(1, 2);
            Rational equal = new Rational(2, 4);
            Rational less = new Rational(1, 3);
            Rational greater = new Rational(2, 3);

            // equality with rational
            rational.CompareTo(rational).ShouldBe(0);
            rational.CompareTo(equal).ShouldBe(0);
            rational.CompareTo(less).ShouldBe(1);
            rational.CompareTo(greater).ShouldBe(-1);

            (rational < rational).ShouldBeFalse();
            (rational > rational).ShouldBeFalse();
            (rational < less).ShouldBeFalse();
            (rational > less).ShouldBeTrue();
            (rational < greater).ShouldBeTrue();
            (rational > greater).ShouldBeFalse();

            (rational <= rational).ShouldBeTrue();
            (rational >= rational).ShouldBeTrue();
            (rational <= less).ShouldBeFalse();
            (rational >= less).ShouldBeTrue();
            (rational <= greater).ShouldBeTrue();
            (rational >= greater).ShouldBeFalse();

            // equality with integer
            (rational <= 1).ShouldBeTrue();
            (rational >= 0).ShouldBeTrue();
            (rational <= 0).ShouldBeFalse();
            (rational >= 1).ShouldBeFalse();
            (rational < 1).ShouldBeTrue();
            (rational > 0).ShouldBeTrue();
            (rational < 0).ShouldBeFalse();
            (rational > 1).ShouldBeFalse();

            (0 <= rational).ShouldBeTrue();
            (1 >= rational).ShouldBeTrue();
            (1 <= rational).ShouldBeFalse();
            (0 >= rational).ShouldBeFalse();
            (0 < rational).ShouldBeTrue();
            (1 > rational).ShouldBeTrue();
            (1 < rational).ShouldBeFalse();
            (0 > rational).ShouldBeFalse();

            rational.CompareTo(0).ShouldBe(1);
            rational.CompareTo(1).ShouldBe(-1);
            rational.CompareTo(Int64.MinValue).ShouldBe(1);
            rational.CompareTo(Int64.MaxValue).ShouldBe(-1);
            Rational.Zero.CompareTo(0).ShouldBe(0);
            Rational.MinValue.CompareTo(Int64.MinValue).ShouldBe(0);
            Rational.MaxValue.CompareTo(Int64.MaxValue).ShouldBe(0);
            new Rational(9223372036854775801, Int64.MaxValue - 1).CompareTo(new Rational(9223372036854775797, Int64.MaxValue)).ShouldBe(1);
            new Rational(9223372036854775797, Int64.MaxValue - 1).CompareTo(new Rational(9223372036854775801, Int64.MaxValue)).ShouldBe(-1);

            // equality with object
            rational.CompareTo((Object)equal).ShouldBe(0);
            rational.CompareTo((Object)less).ShouldBe(1);
            rational.CompareTo((Object)greater).ShouldBe(-1);

            // extrema
            rational.CompareTo(Rational.PositiveInfinity).ShouldBe(-1);
            rational.CompareTo(Rational.NegativeInfinity).ShouldBe(1);
            rational.CompareTo(Rational.NaN).ShouldBe(1);
            rational.CompareTo(null).ShouldBe(1);
            Rational.PositiveInfinity.CompareTo(Rational.PositiveInfinity).ShouldBe(0);
            Rational.NegativeInfinity.CompareTo(Rational.NegativeInfinity).ShouldBe(0);
            Rational.NaN.CompareTo(rational).ShouldBe(-1);

            Rational.PositiveInfinity.CompareTo(Rational.NaN).ShouldBe(1);
            Rational.NegativeInfinity.CompareTo(Rational.NaN).ShouldBe(1);
            Rational.PositiveInfinity.CompareTo(new Rational(1, 1)).ShouldBe(1);
            Rational.NegativeInfinity.CompareTo(new Rational(1, 1)).ShouldBe(-1);
            Rational.PositiveInfinity.CompareTo(Rational.PositiveInfinity).ShouldBe(0);
            Rational.NegativeInfinity.CompareTo(Rational.NegativeInfinity).ShouldBe(0);
            Rational.NaN.CompareTo(Rational.NaN).ShouldBe(0);

            Rational.NaN.CompareTo(Rational.PositiveInfinity).ShouldBe(-1);
            Rational.NaN.CompareTo(Rational.NegativeInfinity).ShouldBe(-1);
            Rational.NaN.CompareTo(Rational.NaN).ShouldBe(0);

            new Rational(Int64.MaxValue - 1, 1).CompareTo(Rational.MaxValue).ShouldBe(-1);
            new Rational(Int64.MinValue + 1, 1).CompareTo(Rational.MinValue).ShouldBe(1);
            new Rational(Int64.MaxValue / 2, 1).CompareTo(Rational.MaxValue).ShouldBe(-1);
            new Rational(Int64.MinValue / 2, 1).CompareTo(Rational.MinValue).ShouldBe(1);

            Rational.MaxValue.CompareTo(new Rational(Int64.MaxValue - 1, 1)).ShouldBe(1);
            Rational.MinValue.CompareTo(new Rational(Int64.MinValue + 1, 1)).ShouldBe(-1);
            Rational.MaxValue.CompareTo(new Rational(Int64.MaxValue / 2, 1)).ShouldBe(1);
            Rational.MinValue.CompareTo(new Rational(Int64.MinValue / 2, 1)).ShouldBe(-1);

            // exceptions
            Should.Throw<ArgumentException>(() => rational.CompareTo(new Object()));
        }

        /// <summary>
        /// Tests addition operators of the <see cref="Rational" /> class.
        /// </summary>
        [Test]
        public void RationalAdditionTest()
        {
            // rationals
            (new Rational(1, 11) + new Rational(2, 11)).ShouldBe(new Rational(3, 11));
            (new Rational(2, 11) + new Rational(1, 11)).ShouldBe(new Rational(3, 11));
            (new Rational(1, 2) + new Rational(2, 4)).ShouldBe(new Rational(1, 1));
            (new Rational(1, 3) + new Rational(1, 2)).ShouldBe(new Rational(5, 6));
            (new Rational(1, 3) + new Rational(1, 2)).ShouldBe(new Rational(5, 6));
            (Rational.Zero + new Rational(1, 2)).ShouldBe(new Rational(1, 2));
            (new Rational(1, 2) + Rational.Zero).ShouldBe(new Rational(1, 2));
            (new Rational(1, 3) + new Rational(-2, 3)).ShouldBe(new Rational(-1, 3));
            (new Rational(-6, 7) + new Rational(6, 7)).ShouldBe(new Rational(0, 1));

            // extrema
            (Rational.MaxValue + new Rational(1, 1)).ShouldBe(Rational.PositiveInfinity);
            (new Rational(-1, 1) + Rational.MinValue).ShouldBe(Rational.NegativeInfinity);

            (Rational.PositiveInfinity + new Rational(2, 11)).ShouldBe(Rational.PositiveInfinity);
            (new Rational(1, 2) + Rational.PositiveInfinity).ShouldBe(Rational.PositiveInfinity);
            (Rational.NegativeInfinity + new Rational(2, 11)).ShouldBe(Rational.NegativeInfinity);
            (new Rational(1, 2) + Rational.NegativeInfinity).ShouldBe(Rational.NegativeInfinity);
            (Rational.PositiveInfinity + Rational.Epsilon).ShouldBe(Rational.PositiveInfinity);
            (Rational.Epsilon + Rational.PositiveInfinity).ShouldBe(Rational.PositiveInfinity);
            (Rational.NegativeInfinity + Rational.Epsilon).ShouldBe(Rational.NegativeInfinity);
            (Rational.Epsilon + Rational.NegativeInfinity).ShouldBe(Rational.NegativeInfinity);

            // NaN
            (Rational.PositiveInfinity + Rational.NegativeInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN + new Rational(1, 2)).ShouldBe(Rational.NaN);
            (Rational.NaN + new Rational(-1, 2)).ShouldBe(Rational.NaN);
            (new Rational(1, 2) + Rational.NaN).ShouldBe(Rational.NaN);
            (new Rational(-1, 2) + Rational.NaN).ShouldBe(Rational.NaN);
            (Rational.NaN + Rational.Epsilon).ShouldBe(Rational.NaN);
            (Rational.NaN + Rational.PositiveInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN + Rational.NegativeInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN + Rational.NaN).ShouldBe(Rational.NaN);

            // with integer
            (1 + new Rational(1, 2)).ShouldBe(new Rational(3, 2));
            (new Rational(1, 2) + 1).ShouldBe(new Rational(3, 2));

            // exceptions
            Rational rational;
            Should.Throw<OverflowException>(() => rational = new Rational(Int64.MaxValue - 1, Int64.MaxValue) + new Rational(1, 1));
            Should.Throw<OverflowException>(() => rational = new Rational(Int64.MaxValue - 1, Int64.MaxValue) + new Rational(1, Int64.MaxValue / 2));
        }

        /// <summary>
        /// Tests subtraction operators of the <see cref="Rational" /> class.
        /// </summary>
        [Test]
        public void RationalSubtractionTest()
        {
            // rationals
            (new Rational(1, 11) - new Rational(2, 11)).ShouldBe(new Rational(-1, 11));
            (new Rational(2, 11) - new Rational(1, 11)).ShouldBe(new Rational(1, 11));
            (new Rational(1, 2) - new Rational(2, 4)).ShouldBe(Rational.Zero);
            (new Rational(1, 3) - new Rational(1, 2)).ShouldBe(new Rational(-1, 6));
            (new Rational(1, 3) - new Rational(1, 2)).ShouldBe(new Rational(-1, 6));
            (Rational.Zero - new Rational(1, 2)).ShouldBe(new Rational(-1, 2));
            (new Rational(1, 2) - Rational.Zero).ShouldBe(new Rational(1, 2));
            (new Rational(1, 3) - new Rational(-2, 3)).ShouldBe(new Rational(1, 1));
            (new Rational(-6, 7) - new Rational(6, 7)).ShouldBe(new Rational(-12, 7));

            // extrema
            (Rational.MaxValue - new Rational(-1, 1)).ShouldBe(Rational.PositiveInfinity);
            (Rational.MinValue - new Rational(1, 1)).ShouldBe(Rational.NegativeInfinity);

            (Rational.PositiveInfinity - new Rational(2, 11)).ShouldBe(Rational.PositiveInfinity);
            (new Rational(1, 2) - Rational.PositiveInfinity).ShouldBe(Rational.NegativeInfinity);
            (Rational.NegativeInfinity - new Rational(2, 11)).ShouldBe(Rational.NegativeInfinity);
            (new Rational(1, 2) - Rational.NegativeInfinity).ShouldBe(Rational.PositiveInfinity);
            (Rational.PositiveInfinity - Rational.Epsilon).ShouldBe(Rational.PositiveInfinity);
            (Rational.Epsilon - Rational.PositiveInfinity).ShouldBe(Rational.NegativeInfinity);
            (Rational.NegativeInfinity - Rational.Epsilon).ShouldBe(Rational.NegativeInfinity);
            (Rational.Epsilon - Rational.NegativeInfinity).ShouldBe(Rational.PositiveInfinity);

            // NaN
            (Rational.PositiveInfinity - Rational.PositiveInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN - new Rational(1, 2)).ShouldBe(Rational.NaN);
            (Rational.NaN - new Rational(-1, 2)).ShouldBe(Rational.NaN);
            (new Rational(1, 2) - Rational.NaN).ShouldBe(Rational.NaN);
            (new Rational(-1, 2) - Rational.NaN).ShouldBe(Rational.NaN);
            (Rational.NaN - Rational.Epsilon).ShouldBe(Rational.NaN);
            (Rational.NaN - Rational.PositiveInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN - Rational.NegativeInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN - Rational.NaN).ShouldBe(Rational.NaN);

            // with integer
            (1 - new Rational(1, 2)).ShouldBe(new Rational(1, 2));
            (new Rational(1, 2) - 1).ShouldBe(new Rational(-1, 2));
        }

        /// <summary>
        /// Tests multiplication operators of the <see cref="Rational" /> class.
        /// </summary>
        [Test]
        public void RationalMultiplicationTest()
        {
            (new Rational(1, 2) * new Rational(2, 1)).ShouldBe(new Rational(1, 1));
            (new Rational(1, 2) * new Rational(1, 2)).ShouldBe(new Rational(1, 4));
            (new Rational(1, 2) * new Rational(2, 3)).ShouldBe(new Rational(1, 3));
            (new Rational(-1, 2) * new Rational(2, 3)).ShouldBe(new Rational(-1, 3));
            (new Rational(1, 2) * new Rational(-2, 3)).ShouldBe(new Rational(-1, 3));
            (new Rational(100, 1) * new Rational(1, 100)).ShouldBe(new Rational(1, 1));
            (new Rational(Int64.MaxValue - 1, Int64.MaxValue) * new Rational(Int64.MaxValue, Int64.MaxValue - 1)).ShouldBe(new Rational(1, 1));

            (Rational.Zero * new Rational(1, 100)).ShouldBe(Rational.Zero);
            (new Rational(1, 100) * Rational.Zero).ShouldBe(Rational.Zero);

            // extrema
            (Rational.PositiveInfinity * new Rational(1, 100)).ShouldBe(Rational.PositiveInfinity);
            (Rational.NegativeInfinity * new Rational(1, 100)).ShouldBe(Rational.NegativeInfinity);
            (Rational.PositiveInfinity * new Rational(-1, 100)).ShouldBe(Rational.NegativeInfinity);
            (Rational.NegativeInfinity * new Rational(-1, 100)).ShouldBe(Rational.PositiveInfinity);
            (Rational.PositiveInfinity * Rational.PositiveInfinity).ShouldBe(Rational.PositiveInfinity);
            (Rational.NegativeInfinity * Rational.NegativeInfinity).ShouldBe(Rational.PositiveInfinity);
            (Rational.PositiveInfinity * Rational.NegativeInfinity).ShouldBe(Rational.NegativeInfinity);
            (Rational.NegativeInfinity * Rational.PositiveInfinity).ShouldBe(Rational.NegativeInfinity);

            // NaN
            (Rational.NaN * Rational.Epsilon).ShouldBe(Rational.NaN);
            (Rational.NaN * Rational.PositiveInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN * Rational.NegativeInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN * Rational.NaN).ShouldBe(Rational.NaN);

            // with integer
            (new Rational(1, 2) * 2).ShouldBe(new Rational(1, 1));
            (2 * new Rational(1, 2)).ShouldBe(new Rational(1, 1));

            // exceptions
            Rational rational;
            Should.Throw<OverflowException>(() => rational = new Rational(9223372036854775801, 1) * new Rational(9223372036854775797, Int64.MaxValue));
            Should.Throw<OverflowException>(() => rational = new Rational(Int64.MaxValue, 9223372036854775797) * new Rational(1, 9223372036854775797));
        }

        /// <summary>
        /// Tests division operators of the <see cref="Rational" /> class.
        /// </summary>
        [Test]
        public void RationalDivisionTest()
        {
            (new Rational(1, 2) / new Rational(2, 1)).ShouldBe(new Rational(1, 4));
            (new Rational(1, 2) / new Rational(1, 2)).ShouldBe(new Rational(1, 1));
            (new Rational(1, 2) / new Rational(2, 3)).ShouldBe(new Rational(3, 4));
            (new Rational(-1, 2) / new Rational(2, 3)).ShouldBe(new Rational(-3, 4));
            (new Rational(1, 2) / new Rational(-2, 3)).ShouldBe(new Rational(-3, 4));
            (new Rational(100, 1) / new Rational(100, 1)).ShouldBe(new Rational(1, 1));
            (new Rational(Int64.MaxValue - 1, Int64.MaxValue) / new Rational(Int64.MaxValue - 1, Int64.MaxValue)).ShouldBe(new Rational(1, 1));

            (Rational.Zero / new Rational(1, 100)).ShouldBe(Rational.Zero);
            (new Rational(1, 100) / Rational.Zero).ShouldBe(Rational.NaN);

            // extrema
            (Rational.PositiveInfinity / new Rational(1, 100)).ShouldBe(Rational.PositiveInfinity);
            (Rational.NegativeInfinity / new Rational(1, 100)).ShouldBe(Rational.NegativeInfinity);
            (Rational.PositiveInfinity / new Rational(-1, 100)).ShouldBe(Rational.NegativeInfinity);
            (Rational.NegativeInfinity / new Rational(-1, 100)).ShouldBe(Rational.PositiveInfinity);
            (Rational.PositiveInfinity / Rational.PositiveInfinity).ShouldBe(Rational.NaN);
            (Rational.NegativeInfinity / Rational.NegativeInfinity).ShouldBe(Rational.NaN);
            (Rational.PositiveInfinity / Rational.NegativeInfinity).ShouldBe(Rational.NaN);
            (Rational.NegativeInfinity / Rational.PositiveInfinity).ShouldBe(Rational.NaN);
            (new Rational(1, 1) / Rational.PositiveInfinity).ShouldBe(Rational.Zero);
            (new Rational(1, 1) / Rational.NegativeInfinity).ShouldBe(Rational.Zero);

            // NaN
            (Rational.NaN / Rational.Epsilon).ShouldBe(Rational.NaN);
            (Rational.NaN / Rational.PositiveInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN / Rational.NegativeInfinity).ShouldBe(Rational.NaN);
            (Rational.NaN / Rational.NaN).ShouldBe(Rational.NaN);

            // with integer
            (new Rational(1, 2) / 2).ShouldBe(new Rational(1, 4));
            (2 / new Rational(1, 2)).ShouldBe(new Rational(4, 1));

            // exceptions
            Rational rational;
            Should.Throw<OverflowException>(() => rational = new Rational(9223372036854775801, Int64.MaxValue) / new Rational(Int64.MaxValue, 9223372036854775797));
        }

        /// <summary>
        /// Tests the <see cref="Rational.ToString()" /> method.
        /// </summary>
        [Test]
        public void RationalToStringTest()
        {
            new Rational(1, 2).ToString().ShouldBe("1/2");
            new Rational(2, 1).ToString().ShouldBe("2");
            Rational.Zero.ToString().ShouldBe("0");
            Rational.NaN.ToString().ShouldBe("NaN");
        }

        /// <summary>
        /// Tests the <see cref="Rational.Abs(Rational)" /> method.
        /// </summary>
        [Test]
        public void RationalAbsTest()
        {
            Rational.Abs(new Rational(1, 2)).ShouldBe(new Rational(1, 2));
            Rational.Abs(new Rational(-1, 2)).ShouldBe(new Rational(1, 2));

            Rational.Abs(Rational.NegativeInfinity).ShouldBe(Rational.PositiveInfinity);
            Rational.Abs(Rational.PositiveInfinity).ShouldBe(Rational.PositiveInfinity);
            Rational.Abs(Rational.NaN).ShouldBe(Rational.NaN);
        }
    }
}