// <copyright file="MersenneTwisterRandomGeneratorTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics.Randomizers
{
    using System;
    using AEGIS.Numerics.Randomizers;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="MersenneTwisterRandomGenerator" /> class.
    /// </summary>
    /// <author>Dóra Papp</author>
    [TestFixture]
    public class MersenneTwisterRandomGeneratorTest
    {
        private MersenneTwisterRandomGenerator generator;
        private Int32 numberOfGeneratedNumbers;
        private Int32 numberOfIntervals;
        private Double allowedError;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.generator = new MersenneTwisterRandomGenerator(5);
            this.numberOfGeneratedNumbers = 1000000;
            this.numberOfIntervals = 100;
            this.allowedError = 0.03;
        }

        /// <summary>
        /// Tests the <see cref="MersenneTwisterRandomGenerator.NextDouble()" /> method.
        /// </summary>
        [Test]
        public void MersenneTwisterRandomGeneratorNextDoubleTest()
        {
            Int64[] counter = new Int64[this.numberOfIntervals];
            Int32 numbersPerInterval = this.numberOfGeneratedNumbers / this.numberOfIntervals;
            Int32 lowerBound = (Int32)(numbersPerInterval * (1 - this.allowedError));
            Int32 upperBound = (Int32)(numbersPerInterval * (1 + this.allowedError));

            for (Int32 number = 0; number < this.numberOfGeneratedNumbers; ++number)
            {
                Double actualGeneratedNumber = this.generator.NextDouble();

                for (Int32 interval = 1; interval <= this.numberOfIntervals; ++interval)
                {
                    if (actualGeneratedNumber < (Double)interval / this.numberOfIntervals)
                    {
                        ++counter[interval - 1];
                        break;
                    }
                }
            }

            for (Int32 interval = 0; interval < this.numberOfIntervals; ++interval)
            {
                counter[interval].ShouldBeInRange(lowerBound, upperBound);
            }
        }
    }
}
