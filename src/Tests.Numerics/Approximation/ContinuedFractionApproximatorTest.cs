// <copyright file="ContinuedFractionApproximatorTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics.Approximation
{
    using System;
    using AEGIS.Numerics;
    using AEGIS.Numerics.Approximation;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="ContinuedFractionApproximator" /> class.
    /// </summary>
    [TestFixture]
    public class ContinuedFractionApproximatorTest
    {
        /// <summary>
        /// Tests the constructor of the <see cref="ContinuedFractionApproximator" /> class.
        /// </summary>
        [Test]
        public void ContinuedFractionApproximatorConstructorTest()
        {
            ContinuedFractionApproximator app = new ContinuedFractionApproximator(100);
            app.IterationLimit.ShouldBe(100);

            Should.Throw<ArgumentOutOfRangeException>(() => app = new ContinuedFractionApproximator(0));
            Should.Throw<ArgumentOutOfRangeException>(() => app = new ContinuedFractionApproximator(-1));
        }

        /// <summary>
        /// Test the <see cref="ContinuedFractionApproximator.GetNearestRational(Double, Double)" /> method.
        /// </summary>
        [Test]
        public void ContinuedFractionApproximatorGetNearestRationalTest()
        {
            ContinuedFractionApproximator app = new ContinuedFractionApproximator(100);
            Rational nearest = app.GetNearestRational(1 / 4.0, 0.00000001);
            ((Double)nearest).ShouldBe(1 / 4.0, 0.00000001);

            nearest = app.GetNearestRational(5 / 4.0, 0.00000001);
            ((Double)nearest).ShouldBe(5 / 4.0, 0.00000001);

            nearest = app.GetNearestRational(0, 0.00000001);
            ((Double)nearest).ShouldBe(0, 0.00000001);

            nearest = app.GetNearestRational(1.0, 0.00000001);
            ((Double)nearest).ShouldBe(1.0, 0.00000001);

            nearest = app.GetNearestRational(1 / 3.0, 0.00000001);
            ((Double)nearest).ShouldBe(1 / 3.0, 0.00000001);

            nearest = app.GetNearestRational(4 / 3.0, 0.000000000001);
            ((Double)nearest).ShouldBe(4 / 3.0, 0.00000001);

            nearest = app.GetNearestRational(-4 / 3.0, 0.00000001);
            ((Double)nearest).ShouldBe(-4 / 3.0, 0.00000001);

            nearest = app.GetNearestRational(11 / 7.0, 0.00000001);
            ((Double)nearest).ShouldBe(11 / 7.0, 0.00000001);

            nearest = app.GetNearestRational(-971 / 1579.0, 0.00000001);
            ((Double)nearest).ShouldBe(-971 / 1579.0, 0.00000001);

            nearest = app.GetNearestRational(3 / 12289.0, 0.00000001);
            ((Double)nearest).ShouldBe(3 / 12289.0, 0.00000001);

            nearest = app.GetNearestRational(-30223 / 3.0, 0.00000001);
            ((Double)nearest).ShouldBe(-30223 / 3.0, 0.00000001);

            nearest = app.GetNearestRational(4503599627370496, 0.00000001);
            ((Double)nearest).ShouldBe(4503599627370496, 0.00000001);

            nearest = app.GetNearestRational(-4503599627370496, 0.00000001);
            ((Double)nearest).ShouldBe(-4503599627370496, 0.00000001);

            Should.Throw<ArgumentOutOfRangeException>(() => app.GetNearestRational(4503599627370497, 0.00000001));
            Should.Throw<ArgumentOutOfRangeException>(() => app.GetNearestRational(-4503599627370497, 0.00000001));
            Should.Throw<ArgumentOutOfRangeException>(() => app.GetNearestRational(1.0, 0));
        }
    }
}
