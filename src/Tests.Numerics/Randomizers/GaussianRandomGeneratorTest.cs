// <copyright file="GaussianRandomGeneratorTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Numerics.Randomizers;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="GaussianRandomGenerator" /> class.
    /// </summary>
    [TestFixture]
    public class GaussianRandomGeneratorTest
    {
        private GaussianRandomGenerator generator;
        private Int32 numberOfGeneratedNumbers;
        private Double[] mean;
        private Double[] stdDeviation;
        private Double[] error;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.generator = new GaussianRandomGenerator();
            this.numberOfGeneratedNumbers = 100000;
            this.mean = new Double[] { 0, 1, 10, -10, 100, -100, 10000, -10000, 10, 10 };
            this.stdDeviation = new Double[] { 1, 1, 5, 5, 10, 10, 100, 100, 0.1, 0.1 };
            this.error = new Double[] { 0.01, 0.1, 0.1, 0.1, 1, 1, 1, 1, 0.01, 0.01 };
        }

        /// <summary>
        /// Tests the <see cref="GaussianRandomGenerator.NextDouble(Double, Double)" /> method.
        /// </summary>
        [Test]
        public void GaussianRandomGeneratorNextDoubleTest()
        {
            // numbers between 0 and 1
            List<Double> generatedNumbers = Enumerable.Range(0, this.numberOfGeneratedNumbers).Select(number => this.generator.NextDouble()).ToList();

            // range test
            for (Int32 i = 0; i < this.numberOfGeneratedNumbers; i++)
            {
                generatedNumbers[i].ShouldBeGreaterThanOrEqualTo(0);
                generatedNumbers[i].ShouldBeLessThan(1);
            }

            // numbers with specified mean and std. variation
            for (Int32 i = 0; i < this.mean.Length; i++)
            {
                generatedNumbers = Enumerable.Range(0, this.numberOfGeneratedNumbers).Select(number => this.generator.NextDouble(this.mean[i], this.stdDeviation[i])).ToList();

                // mean test
                Double mean = generatedNumbers.Sum() / this.numberOfGeneratedNumbers;
                mean.ShouldBe(this.mean[i], this.error[i]);

                // std. deviation test
                Double stdDeviation = Math.Sqrt(generatedNumbers.Sum(number => (number - mean) * (number - mean)) / this.numberOfGeneratedNumbers);

                stdDeviation.ShouldBe(this.stdDeviation[i], this.error[i]);
            }
        }
    }
}
