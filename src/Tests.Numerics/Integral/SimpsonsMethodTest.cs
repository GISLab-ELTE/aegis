// <copyright file="SimpsonsMethodTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Numerics.Integral
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AEGIS.Numerics.Integral;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="SimpsonsMethod" /> class.
    /// </summary>
    [TestFixture]
    public class SimpsonsMethodTest
    {
        /// <summary>
        /// Tests the <see cref="SimpsonsMethod.ComputeIntegral(Func{Double, Double}, Double, Double, Int32)" /> method.
        /// </summary>
        [Test]
        public void SimpsonMethodComputeIntegralTest()
        {
            SimpsonsMethod.ComputeIntegral(x => x, 0, 0, 2).ShouldBe(0, 0.0001);
            SimpsonsMethod.ComputeIntegral(x => x, 0, 5000, 5000000).ShouldBe(12500000, 0.0001);
            SimpsonsMethod.ComputeIntegral(x => x, 0, 6000, 6000000).ShouldBe(18000000, 0.0001);
            SimpsonsMethod.ComputeIntegral(x => Math.Pow(x, 3), 0, 1, 100).ShouldBe(0.25, 0.0001);
            SimpsonsMethod.ComputeIntegral(x => 1 / x, 1, 100, 1000).ShouldBe(4.605170, 0.0001);

            Should.Throw<ArgumentNullException>(() => SimpsonsMethod.ComputeIntegral(null, 0, 10, 10));
            Should.Throw<ArgumentException>(() => SimpsonsMethod.ComputeIntegral(x => x, 0, 10, 0));
            Should.Throw<ArgumentException>(() => SimpsonsMethod.ComputeIntegral(x => x, 0, 10, 1));
            Should.Throw<ArgumentException>(() => SimpsonsMethod.ComputeIntegral(x => x, 0, 10, -10));
        }
    }
}
