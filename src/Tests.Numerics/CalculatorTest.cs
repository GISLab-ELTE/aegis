// <copyright file="CalculatorTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics
{
    using System;
    using System.Numerics;
    using AEGIS.Numerics;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Calculator" /> class.
    /// </summary>
    [TestFixture]
    public class CalculatorTest
    {
        /// <summary>
        /// Tests the <see cref="Calculator.Fraction(Single)" /> method.
        /// </summary>
        [Test]
        public void CalculatorFractionSingleTest()
        {
            Calculator.Fraction(0.001f).ShouldBe(0.001f, 1e-4);
            Calculator.Fraction(1.55f).ShouldBe(0.55f, 1e-4);
            Calculator.Fraction(7f).ShouldBe(0.00f, 1e-4);
            Calculator.Fraction(-7f).ShouldBe(0.00f, 1e-4);
            Calculator.Fraction(-1.55f).ShouldBe(-0.55f, 1e-4);
            Calculator.Fraction(-0.01f).ShouldBe(-0.01f, 1e-4);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Fraction(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorFractionDoubleTest()
        {
            Calculator.Fraction(0.001).ShouldBe(0.001, 1e-10);
            Calculator.Fraction(1.55).ShouldBe(0.55, 1e-10);
            Calculator.Fraction(7.0).ShouldBe(0.00, 1e-10);
            Calculator.Fraction(-7.0).ShouldBe(0.00, 1e-10);
            Calculator.Fraction(-1.55).ShouldBe(-0.55, 1e-10);
            Calculator.Fraction(-0.01).ShouldBe(-0.01, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.OrderOfMagnitude(Int64)" /> method.
        /// </summary>
        [Test]
        public void CalculatorOrderOfMagnitudeInt64Test()
        {
            Calculator.OrderOfMagnitude(0).ShouldBe(0);
            Calculator.OrderOfMagnitude(1).ShouldBe(0);
            Calculator.OrderOfMagnitude(-1).ShouldBe(0);
            Calculator.OrderOfMagnitude(100).ShouldBe(2);
            Calculator.OrderOfMagnitude(-100).ShouldBe(2);
            Calculator.OrderOfMagnitude(999).ShouldBe(2);
            Calculator.OrderOfMagnitude(-999).ShouldBe(2);
            Calculator.OrderOfMagnitude(1000).ShouldBe(3);
            Calculator.OrderOfMagnitude(-1000).ShouldBe(3);
            Calculator.OrderOfMagnitude(Int64.MaxValue).ShouldBe(18);
            Calculator.OrderOfMagnitude(Int64.MinValue).ShouldBe(18);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.OrderOfMagnitude(UInt64)" /> method.
        /// </summary>
        [Test]
        public void CalculatorOrderOfMagnitudeUInt64Test()
        {
            Calculator.OrderOfMagnitude(0UL).ShouldBe(0);
            Calculator.OrderOfMagnitude(1UL).ShouldBe(0);
            Calculator.OrderOfMagnitude(100UL).ShouldBe(2);
            Calculator.OrderOfMagnitude(999UL).ShouldBe(2);
            Calculator.OrderOfMagnitude(1000UL).ShouldBe(3);
            Calculator.OrderOfMagnitude(UInt64.MaxValue).ShouldBe(19);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.OrderOfMagnitude(Rational)" /> method.
        /// </summary>
        [Test]
        public void CalculatorOrderOfMagnitudeRationalTest()
        {
            Calculator.OrderOfMagnitude(Rational.Zero).ShouldBe(0);
            Calculator.OrderOfMagnitude(new Rational(1, 1)).ShouldBe(0);
            Calculator.OrderOfMagnitude(new Rational(100, 1)).ShouldBe(2);
            Calculator.OrderOfMagnitude(new Rational(99, 1)).ShouldBe(1);
            Calculator.OrderOfMagnitude(new Rational(100, 10)).ShouldBe(1);
            Calculator.OrderOfMagnitude(new Rational(100, 11)).ShouldBe(0);
            Calculator.OrderOfMagnitude(new Rational(99, 10)).ShouldBe(0);
            Calculator.OrderOfMagnitude(new Rational(1, 10)).ShouldBe(-1);
            Calculator.OrderOfMagnitude(new Rational(1, 10000)).ShouldBe(-4);
            Calculator.OrderOfMagnitude(new Rational(-1, 1)).ShouldBe(0);
            Calculator.OrderOfMagnitude(new Rational(-100, 1)).ShouldBe(2);
            Calculator.OrderOfMagnitude(new Rational(-99, 1)).ShouldBe(1);
            Calculator.OrderOfMagnitude(new Rational(-100, 10)).ShouldBe(1);
            Calculator.OrderOfMagnitude(new Rational(-100, 11)).ShouldBe(0);
            Calculator.OrderOfMagnitude(new Rational(-99, 10)).ShouldBe(0);
            Calculator.OrderOfMagnitude(new Rational(-1, 10)).ShouldBe(-1);
            Calculator.OrderOfMagnitude(new Rational(-1, 10000)).ShouldBe(-4);
            Calculator.OrderOfMagnitude(Rational.MaxValue).ShouldBe(18);
            Calculator.OrderOfMagnitude(Rational.MinValue).ShouldBe(18);

            Calculator.OrderOfMagnitude(Rational.NaN).ShouldBe(0);
            Calculator.OrderOfMagnitude(Rational.PositiveInfinity).ShouldBe(0);
            Calculator.OrderOfMagnitude(Rational.NegativeInfinity).ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.AbsMax(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAbsMaxTest()
        {
            Calculator.AbsMax(-57.678, 18.45, 13.01, 0, 200, -245).ShouldBe(245);
            Calculator.AbsMax(-57.678, 57.856, 11.111, 0, 20, -0, 0).ShouldBe(57.856);
            Calculator.AbsMax(Double.NaN).ShouldBe(Double.NaN);
            Calculator.AbsMax(Double.NegativeInfinity, 0, 57, 4).ShouldBe(Double.PositiveInfinity);
            Calculator.AbsMax(Double.NegativeInfinity, 0, 57, 4, Double.PositiveInfinity).ShouldBe(Double.PositiveInfinity);
            Calculator.AbsMax((Double[])null).ShouldBe(Double.PositiveInfinity);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.AbsMin(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAbsMinTest()
        {
            Calculator.AbsMin(-57.678, 18.45, 13.01, 0, 200, -245).ShouldBe(0);
            Calculator.AbsMin(-57.678, 57.856, 11.111, 0, 20, -0, 0).ShouldBe(0);
            Calculator.AbsMin(Double.NaN).ShouldBe(Double.NaN);
            Calculator.AbsMin(Double.NegativeInfinity, 0, 57, 4).ShouldBe(0);
            Calculator.AbsMin(Double.NegativeInfinity, 0, 57, 4, Double.PositiveInfinity).ShouldBe(0);
            Calculator.AbsMin((Double[])null).ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Factorial(Int32)" /> method.
        /// </summary>
        [Test]
        public void CalculatorFactorialTest()
        {
            Calculator.Factorial(0).ShouldBe(1);
            Calculator.Factorial(1).ShouldBe(1);
            Calculator.Factorial(2).ShouldBe(2);
            Calculator.Factorial(3).ShouldBe(6);
            Calculator.Factorial(5).ShouldBe(120);
            Calculator.Factorial(10).ShouldBe(3628800);
            Calculator.Factorial(20).ShouldBe(2432902008176640000);
            Calculator.Factorial(171).ShouldBe(Double.PositiveInfinity);

            Should.Throw<ArgumentOutOfRangeException>(() => Calculator.Factorial(-1));
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Gamma(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorGammaTest()
        {
            Calculator.Gamma(5.6).ShouldBe(61.5539150062892670342628016328, 0.0000000000001);
            Calculator.Gamma(4).ShouldBe(6, 0.00000000000001);
            Calculator.Gamma(17.432786).ShouldBe(70794796680408.184132649758153230595573203898806, 0.1);
            Calculator.Gamma(17.4).ShouldBe(64524993678768.475176512444528832088723820561940);
            Calculator.Gamma(17.2).ShouldBe(36698964629326.666366399049040308611484289683410, 0.1);
            Calculator.Gamma(17.4327).ShouldBe(70777572425326.219973716843329090056968590230874, 1);
            Calculator.Gamma(7.84932).ShouldBe(3725.5363220501936325763777399300694165140295715708192, 0.00000000001);
            Calculator.Gamma(1.79342).ShouldBe(0.9296536910011532941878683112978521231640028664002138, 0.000000000000001);
            Calculator.Gamma(0.78).ShouldBe(1.1874709053741035003665987584542707907235290398108421, 0.000000000000001);
            Calculator.Gamma(21.5).ShouldBe(11082798113786903841.710590978006853895159991111, 100000);
            Calculator.Gamma(21.4378).ShouldBe(9171594978637877217.1951336804437033178076357804, 10000);
            Calculator.Gamma(0.1234).ShouldBe(7.6363851985070193846874769204366944252526236697467540, 0.00000000000001);
            Calculator.Gamma(-8.64).ShouldBe(-0.00002137352460747696486247515808363540929072012897, 0.0000000000000000001);

            Should.Throw<ArgumentOutOfRangeException>(() => Calculator.Gamma(-5));
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sum(Int32, Int32)" /> method.
        /// </summary>
        [Test]
        public void CalculatorSumInt32Test()
        {
            Calculator.Sum(0, 0).ShouldBe(0);
            Calculator.Sum(100, 100).ShouldBe(100);
            Calculator.Sum(1, -2).ShouldBe(0);
            Calculator.Sum(-5, -2).ShouldBe(-14);
            Calculator.Sum(5, 8).ShouldBe(26);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sum(Int64, Int64)" /> method.
        /// </summary>
        [Test]
        public void CalculatorSumInt64Test()
        {
            Calculator.Sum((Int64)0, 0).ShouldBe(0);
            Calculator.Sum((Int64)100, 100).ShouldBe(100);
            Calculator.Sum((Int64)1, -2).ShouldBe(0);
            Calculator.Sum((Int64)(-5), -2).ShouldBe(-14);
            Calculator.Sum((Int64)5, 8).ShouldBe(26);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sum(Int32, Int32, Func{Int32, Int32})" /> method.
        /// </summary>
        [Test]
        public void CalculatorSumWithFuncInt32Test()
        {
            Calculator.Sum(0, 0, x => x + 5).ShouldBe(5);
            Calculator.Sum(0, 1, x => x + 5).ShouldBe(11);
            Calculator.Sum(0, 5, x => x + 5).ShouldBe(45);
            Calculator.Sum(-10, -8, x => x + 5).ShouldBe(-12);
            Calculator.Sum(5, 0, x => x + 5).ShouldBe(0);

            Calculator.Sum(5, 5, x => x * x).ShouldBe(25);
            Calculator.Sum(0, 3, x => x * x).ShouldBe(14);
            Calculator.Sum(-2, 2, x => x * x).ShouldBe(10);
            Calculator.Sum(2, 0, x => x * x).ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sum(Int32, Int32, Func{Int32, Int64})" /> method.
        /// </summary>
        [Test]
        public void CalculatorSumWithFuncInt64Test()
        {
            Calculator.Sum(2, 2, x => x * x * x).ShouldBe(8);
            Calculator.Sum(3, 5, x => x * x * x).ShouldBe(216);
            Calculator.Sum(-3, 1, x => x * x * x).ShouldBe(-35);
            Calculator.Sum(-50, 50, x => x * x * x).ShouldBe(0);
            Calculator.Sum(2, 1, x => x * x * x).ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sum(Int32, Int32, Func{Int32, Single})" /> method.
        /// </summary>
        [Test]
        public void CalculatorSumWithFuncSingleTest()
        {
            Calculator.Sum(2, 2, x => (Single)x / 2).ShouldBe(1.0f);
            Calculator.Sum(1, 5, x => (Single)x / 2).ShouldBe(7.5f);
            Calculator.Sum(3, 11, x => (Single)x / 2).ShouldBe(31.5f);
            Calculator.Sum(-5, 2, x => (Single)x / 2).ShouldBe(-6f);
            Calculator.Sum(-50, 51, x => (Single)x / 2).ShouldBe(25.5f);
            Calculator.Sum(2, 1, x => (Single)x / 2).ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sum(Int32, Int32, Func{Int32, Double})" /> method.
        /// </summary>
        [Test]
        public void CalculatorSumWithFuncDoubleTest()
        {
            Calculator.Sum(2, 2, x => Math.Sqrt(x)).ShouldBe(1.41421356237, 1e-10);
            Calculator.Sum(1, 7, x => Math.Sqrt(x)).ShouldBe(13.4775734012, 1e-10);
            Calculator.Sum(2, 1, x => Math.Sqrt(x)).ShouldBe(0, 1e-10);

            Calculator.Sum(2, 2, x => Math.Sin(x)).ShouldBe(0.90929742682, 1e-10);
            Calculator.Sum(1, 4, x => Math.Sin(x)).ShouldBe(1.13508592438, 1e-10);
            Calculator.Sum(-4, -2, x => Math.Sin(x)).ShouldBe(-0.2936149395, 1e-10);
            Calculator.Sum(2, 1, x => Math.Sin(x)).ShouldBe(0, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Hypot(Double, Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorHypotTest()
        {
            Calculator.Hypot(1, 1).ShouldBe(Math.Sqrt(2));
            Calculator.Hypot(3, 4).ShouldBe(5);
            Calculator.Hypot(0, 0).ShouldBe(0);
            Calculator.Hypot(0, 3).ShouldBe(3);
            Calculator.Hypot(3, 0).ShouldBe(3);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sec(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorSecTest()
        {
            Calculator.Sec(Math.PI / 3).ShouldBe(2, 1e-10);
            Calculator.Sec(-Math.PI / 4).ShouldBe(Math.Sqrt(2), 1e-10);
            Calculator.Sec(Math.PI).ShouldBe(-1, 1e-10);
            Calculator.Sec(0).ShouldBe(1, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Csc(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorCscTest()
        {
            Calculator.Csc(Math.PI / 3).ShouldBe(2 / Math.Sqrt(3), 1e-10);
            Calculator.Csc(-Math.PI / 4).ShouldBe(-Math.Sqrt(2), 1e-10);
            Calculator.Csc(Math.PI / 2).ShouldBe(1, 1e-10);
            Calculator.Csc(0).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Cot(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorCotTest()
        {
            Calculator.Cot(Math.PI / 3).ShouldBe(1 / Math.Sqrt(3), 1e-10);
            Calculator.Cot(-Math.PI / 4).ShouldBe(-1, 1e-10);
            Calculator.Cot(Math.PI / 2).ShouldBe(0, 1e-10);
            Calculator.Cot(0).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Asinh(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAsinhTest()
        {
            Calculator.Asinh(Math.PI).ShouldBe(1.86229574331, 1e-10);
            Calculator.Asinh(-Math.PI / 2).ShouldBe(-1.2334031175, 1e-10);
            Calculator.Asinh(Math.PI / 3).ShouldBe(0.91435665539, 1e-10);
            Calculator.Asinh(-Math.PI / 4).ShouldBe(-0.7212254887, 1e-10);
            Calculator.Asinh(2 * Math.PI).ShouldBe(2.53729750137, 1e-10);
            Calculator.Asinh(0).ShouldBe(0, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Acosh(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAcoshTest()
        {
            Calculator.Acosh(Math.PI).ShouldBe(1.81152627246, 1e-10);
            Calculator.Acosh(2 * Math.PI).ShouldBe(2.52463065993, 1e-10);
            Calculator.Acosh(Math.PI / 3).ShouldBe(0.30604210861, 1e-10);
            Calculator.Acosh(1).ShouldBe(0, 1e-10);
            Calculator.Acosh(0).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Atanh(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAtanhTest()
        {
            Calculator.Atanh(0).ShouldBe(0, 1e-10);
            Calculator.Atanh(0.1).ShouldBe(0.10033534773, 1e-10);
            Calculator.Atanh(-0.2).ShouldBe(-0.2027325540, 1e-10);
            Calculator.Atanh(0.5).ShouldBe(0.54930614433, 1e-10);
            Calculator.Atanh(-0.9).ShouldBe(-1.4722194895, 1e-10);
            Calculator.Atanh(-1).ShouldBe(Double.NegativeInfinity);
            Calculator.Atanh(1).ShouldBe(Double.PositiveInfinity);
            Calculator.Atanh(-15).ShouldBe(Double.NaN);
            Calculator.Atanh(15).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Acoth(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAcothTest()
        {
            Calculator.Acoth(Math.PI).ShouldBe(0.32976531495, 1e-10);
            Calculator.Acoth(-Math.PI / 2).ShouldBe(-0.7524692671, 1e-10);
            Calculator.Acoth(Math.PI / 3).ShouldBe(1.88494253942, 1e-10);
            Calculator.Acoth(-5 * Math.PI).ShouldBe(-0.0637481910, 1e-10);
            Calculator.Acoth(-1).ShouldBe(Double.NegativeInfinity);
            Calculator.Acoth(1).ShouldBe(Double.PositiveInfinity);
            Calculator.Acoth(0).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Asech(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAsechTest()
        {
            Calculator.Asech(0.5).ShouldBe(1.31695789692, 1e-10);
            Calculator.Asech(0.1).ShouldBe(2.99322284612, 1e-10);
            Calculator.Asech(0.7).ShouldBe(0.89558809952, 1e-10);
            Calculator.Asech(1).ShouldBe(0, 1e-10);
            Calculator.Asech(0).ShouldBe(Double.PositiveInfinity);
            Calculator.Asech(Math.PI).ShouldBe(Double.NaN);
            Calculator.Asech(-5).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Acsch(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorAcschTest()
        {
            Calculator.Acsch(Math.PI).ShouldBe(0.31316588045, 1e-10);
            Calculator.Acsch(-Math.PI / 3).ShouldBe(-0.8491423010, 1e-10);
            Calculator.Acsch(Math.PI / 8).ShouldBe(1.66435606267, 1e-10);
            Calculator.Acsch(5 * Math.PI).ShouldBe(0.06361905342, 1e-10);
            Calculator.Acsch(0).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sin2(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorSin2Test()
        {
            Calculator.Sin2(Math.PI).ShouldBe(0, 1e-10);
            Calculator.Sin2(Math.PI / 2).ShouldBe(1, 1e-10);
            Calculator.Sin2(Math.PI / 4).ShouldBe(0.5, 1e-10);
            Calculator.Sin2(-Math.PI / 3).ShouldBe(0.75, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sin3(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorSin3Test()
        {
            Calculator.Sin3(Math.PI).ShouldBe(0, 1e-10);
            Calculator.Sin3(Math.PI / 2).ShouldBe(1, 1e-10);
            Calculator.Sin3(Math.PI / 4).ShouldBe(0.35355339059, 1e-10);
            Calculator.Sin3(-Math.PI / 3).ShouldBe(-0.6495190528, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sin4(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorSin4Test()
        {
            Calculator.Sin4(Math.PI).ShouldBe(0, 1e-10);
            Calculator.Sin4(Math.PI / 2).ShouldBe(1, 1e-10);
            Calculator.Sin4(Math.PI / 4).ShouldBe(0.25, 1e-10);
            Calculator.Sin4(-Math.PI / 3).ShouldBe(0.5625, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Sinc(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorSincTest()
        {
            Calculator.Sinc(Math.PI).ShouldBe(-0.0435986286, 1e-10);
            Calculator.Sinc(0.5).ShouldBe(0.63661977236, 1e-10);
            Calculator.Sinc(0.2).ShouldBe(0.93548928378, 1e-10);
            Calculator.Sinc(-5.7).ShouldBe(-0.0451786153, 1e-10);
            Calculator.Sinc(0).ShouldBe(1, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Cos2(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorCos2Test()
        {
            Calculator.Cos2(Math.PI).ShouldBe(1, 1e-10);
            Calculator.Cos2(Math.PI / 2).ShouldBe(0, 1e-10);
            Calculator.Cos2(Math.PI / 4).ShouldBe(0.5, 1e-10);
            Calculator.Cos2(-Math.PI / 3).ShouldBe(0.25, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Cos3(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorCos3Test()
        {
            Calculator.Cos3(Math.PI).ShouldBe(-1, 1e-10);
            Calculator.Cos3(Math.PI / 2).ShouldBe(0, 1e-10);
            Calculator.Cos3(Math.PI / 4).ShouldBe(0.35355339059, 1e-10);
            Calculator.Cos3(-Math.PI / 3).ShouldBe(0.125, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Cos4(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorCos4Test()
        {
            Calculator.Cos4(Math.PI).ShouldBe(1, 1e-10);
            Calculator.Cos4(Math.PI / 2).ShouldBe(0, 1e-10);
            Calculator.Cos4(Math.PI / 4).ShouldBe(0.25, 1e-10);
            Calculator.Cos4(-Math.PI / 3).ShouldBe(0.0625, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Tan2(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorTan2Test()
        {
            Calculator.Tan2(0).ShouldBe(0, 1e-10);
            Calculator.Tan2(Math.PI / 3).ShouldBe(3, 1e-10);
            Calculator.Tan2(-Math.PI / 4).ShouldBe(1, 1e-10);
            Calculator.Tan2(Math.PI).ShouldBe(0, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Tan3(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorTan3Test()
        {
            Calculator.Tan3(0).ShouldBe(0, 1e-10);
            Calculator.Tan3(Math.PI / 3).ShouldBe(5.19615242270, 1e-10);
            Calculator.Tan3(-Math.PI / 4).ShouldBe(-1, 1e-10);
            Calculator.Tan3(Math.PI).ShouldBe(0, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Tan4(Double)" /> method.
        /// </summary>
        [Test]
        public void CalculatorTan4Test()
        {
            Calculator.Tan4(0).ShouldBe(0, 1e-10);
            Calculator.Tan4(Math.PI / 3).ShouldBe(9, 1e-10);
            Calculator.Tan4(-Math.PI / 4).ShouldBe(1, 1e-10);
            Calculator.Tan4(Math.PI).ShouldBe(0, 1e-10);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.GreatestCommonDivisor(Int32, Int32)" /> method.
        /// </summary>
        [Test]
        public void CalculatorGreatestCommonDivisorInt32Test()
        {
            // one or both arguments is 1
            Calculator.GreatestCommonDivisor(1, 1).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(1, 10).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(18, 1).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(-5, 1).ShouldBe(1U);

            // exactly one argument is 0
            Calculator.GreatestCommonDivisor(1, 0).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(0, 8).ShouldBe(8U);

            // both arguments are 0
            Calculator.GreatestCommonDivisor(0, 0).ShouldBe(0U);

            // regular cases
            Calculator.GreatestCommonDivisor(5, 5).ShouldBe(5U);
            Calculator.GreatestCommonDivisor(15, 33).ShouldBe(3U);
            Calculator.GreatestCommonDivisor(25, 7).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(250, 15).ShouldBe(5U);

            // one or both arguments are negative
            Calculator.GreatestCommonDivisor(5, -5).ShouldBe(5U);
            Calculator.GreatestCommonDivisor(15, -5).ShouldBe(5U);
            Calculator.GreatestCommonDivisor(-7, -5).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(35, -7).ShouldBe(7U);

            // maximum/minimum value
            Calculator.GreatestCommonDivisor(Int32.MaxValue, Int32.MaxValue).ShouldBe((UInt32)Int32.MaxValue);
            Calculator.GreatestCommonDivisor(Int32.MinValue, Int32.MinValue).ShouldBe(unchecked((UInt32)Int32.MinValue));
            Calculator.GreatestCommonDivisor(Int32.MaxValue, Int32.MinValue).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(Int32.MinValue, Int32.MaxValue).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(Int32.MinValue, 2).ShouldBe(2U);
            Calculator.GreatestCommonDivisor(2, Int32.MinValue).ShouldBe(2U);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.GreatestCommonDivisor(UInt32, UInt32)" /> method.
        /// </summary>
        [Test]
        public void CalculatorGreatestCommonDivisorUInt32Test()
        {
            // one or both arguments is 1
            Calculator.GreatestCommonDivisor(1U, 1U).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(1U, 10U).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(18U, 1U).ShouldBe(1U);

            // exactly one argument is 0
            Calculator.GreatestCommonDivisor(1U, 0U).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(0U, 8U).ShouldBe(8U);

            // both arguments are 0
            Calculator.GreatestCommonDivisor(0U, 0U).ShouldBe(0U);

            // regular cases
            Calculator.GreatestCommonDivisor(5U, 5U).ShouldBe(5U);
            Calculator.GreatestCommonDivisor(15U, 33U).ShouldBe(3U);
            Calculator.GreatestCommonDivisor(25U, 7U).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(250U, 15U).ShouldBe(5U);

            // maximum value
            Calculator.GreatestCommonDivisor(UInt32.MaxValue, UInt32.MaxValue).ShouldBe(UInt32.MaxValue);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.GreatestCommonDivisor(Int64, Int64)" /> method.
        /// </summary>
        [Test]
        public void CalculatorGreatestCommonDivisorInt64Test()
        {
            // one or both arguments is 1
            Calculator.GreatestCommonDivisor(1L, 1L).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(1L, 10L).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(18L, 1L).ShouldBe(1UL);

            // exactly one argument is 0
            Calculator.GreatestCommonDivisor(1L, 0L).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(0L, 8L).ShouldBe(8UL);

            // both arguments are 0
            Calculator.GreatestCommonDivisor(0L, 0L).ShouldBe(0UL);

            // regular cases
            Calculator.GreatestCommonDivisor(5L, 5L).ShouldBe(5UL);
            Calculator.GreatestCommonDivisor(15L, 33L).ShouldBe(3UL);
            Calculator.GreatestCommonDivisor(25L, 7L).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(250L, 15L).ShouldBe(5UL);

            // one or both arguments are negative
            Calculator.GreatestCommonDivisor(5L, -5L).ShouldBe(5UL);
            Calculator.GreatestCommonDivisor(15L, -5L).ShouldBe(5UL);
            Calculator.GreatestCommonDivisor(-7L, -5L).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(35L, -7L).ShouldBe(7UL);

            // maximum/minimum value
            Calculator.GreatestCommonDivisor(Int64.MaxValue, Int64.MaxValue).ShouldBe((UInt64)Int64.MaxValue);
            Calculator.GreatestCommonDivisor(Int64.MinValue, Int64.MinValue).ShouldBe(unchecked((UInt64)Int64.MinValue));
            Calculator.GreatestCommonDivisor(Int64.MaxValue, Int64.MinValue).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(Int64.MinValue, Int64.MaxValue).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(Int64.MinValue, 2).ShouldBe(2UL);
            Calculator.GreatestCommonDivisor(2, Int64.MinValue).ShouldBe(2UL);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.GreatestCommonDivisor(UInt64, UInt64)" /> method.
        /// </summary>
        [Test]
        public void CalculatorGreatestCommonDivisorUInt64Test()
        {
            // one or both arguments is 1
            Calculator.GreatestCommonDivisor(1UL, 1UL).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(1UL, 10UL).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(18UL, 1UL).ShouldBe(1UL);

            // exactly one argument is 0
            Calculator.GreatestCommonDivisor(1UL, 0UL).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(0UL, 8UL).ShouldBe(8UL);

            // both arguments are 0
            Calculator.GreatestCommonDivisor(0UL, 0UL).ShouldBe(0UL);

            // regular cases
            Calculator.GreatestCommonDivisor(5UL, 5UL).ShouldBe(5UL);
            Calculator.GreatestCommonDivisor(15UL, 33UL).ShouldBe(3UL);
            Calculator.GreatestCommonDivisor(25UL, 7UL).ShouldBe(1UL);
            Calculator.GreatestCommonDivisor(250UL, 15UL).ShouldBe(5UL);

            // maximum value
            Calculator.GreatestCommonDivisor(UInt64.MaxValue, UInt64.MaxValue).ShouldBe(UInt64.MaxValue);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.GreatestCommonDivisor(BigInteger, BigInteger)" /> method.
        /// </summary>
        [Test]
        public void CalculatorGreatestCommonDivisorBigIntegerTest()
        {
            // one or both arguments is 1
            Calculator.GreatestCommonDivisor((BigInteger)1, 1).ShouldBe(1U);
            Calculator.GreatestCommonDivisor((BigInteger)1, 10).ShouldBe(1U);
            Calculator.GreatestCommonDivisor((BigInteger)18, 1).ShouldBe(1U);
            Calculator.GreatestCommonDivisor(-5, (BigInteger)1).ShouldBe(1U);

            // exactly one argument is 0
            Calculator.GreatestCommonDivisor((BigInteger)1, 0).ShouldBe(1U);
            Calculator.GreatestCommonDivisor((BigInteger)0, 8).ShouldBe(8U);

            // both arguments are 0
            Calculator.GreatestCommonDivisor((BigInteger)0, 0).ShouldBe(0U);

            // regular cases
            Calculator.GreatestCommonDivisor((BigInteger)5, 5).ShouldBe(5U);
            Calculator.GreatestCommonDivisor((BigInteger)15, 33).ShouldBe(3U);
            Calculator.GreatestCommonDivisor((BigInteger)25, 7).ShouldBe(1U);
            Calculator.GreatestCommonDivisor((BigInteger)250, 15).ShouldBe(5U);

            // one or both arguments are negative
            Calculator.GreatestCommonDivisor((BigInteger)5, -5).ShouldBe(5U);
            Calculator.GreatestCommonDivisor((BigInteger)15, -5).ShouldBe(5U);
            Calculator.GreatestCommonDivisor((BigInteger)(-7), -5).ShouldBe(1U);
            Calculator.GreatestCommonDivisor((BigInteger)35, -7).ShouldBe(7U);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.LeastCommonMultiple(Int64, Int64)"/> method.
        /// </summary>
        [Test]
        public void CalculatorLeastCommonMultipleTest()
        {
            // one or both arguments are 1
            Calculator.LeastCommonMultiple(1, 1).ShouldBe(1);
            Calculator.LeastCommonMultiple(1, 10).ShouldBe(10);
            Calculator.LeastCommonMultiple(18, 1).ShouldBe(18);

            // exactly one argument is zero
            Calculator.LeastCommonMultiple(0, 7).ShouldBe(0);
            Calculator.LeastCommonMultiple(194, 0).ShouldBe(0);

            // both arguments are 0
            Calculator.LeastCommonMultiple(0, 0).ShouldBe(0);

            // regular cases
            Calculator.LeastCommonMultiple(5, 5).ShouldBe(5);
            Calculator.LeastCommonMultiple(15, 33).ShouldBe(165);
            Calculator.LeastCommonMultiple(5, 10).ShouldBe(10);
            Calculator.LeastCommonMultiple(25, 7).ShouldBe(175);

            // one or more arguments are negative
            Calculator.LeastCommonMultiple(-5, 1).ShouldBe(5);
            Calculator.LeastCommonMultiple(-5, -5).ShouldBe(5);
            Calculator.LeastCommonMultiple(12, -6).ShouldBe(12);
            Calculator.LeastCommonMultiple(-33, -11).ShouldBe(33);
        }

        /// <summary>
        /// Tests the <see cref="Calculator.Binomial(Int32, Int32)" /> method.
        /// </summary>
        [Test]
        public void CalculatorBinomialTest()
        {
            Calculator.Binomial(18, 13).ShouldBe(8568);
            Calculator.Binomial(60, 30).ShouldBe(118264581564861424);
            Calculator.Binomial(0, 0).ShouldBe(1);
            Calculator.Binomial(5, 0).ShouldBe(1);
            Calculator.Binomial(0, 5).ShouldBe(0);
            Calculator.Binomial(30, 30).ShouldBe(1);
            Calculator.Binomial(30, 31).ShouldBe(0);

            Should.Throw<ArgumentOutOfRangeException>(() => Calculator.Binomial(-1, 1));
            Should.Throw<ArgumentOutOfRangeException>(() => Calculator.Binomial(1, -1));
        }
    }
}
