// <copyright file="StatisticsTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Numerics;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Statistics" /> class.
    /// </summary>
    [TestFixture]
    public class StatisticsTest
    {
        /// <summary>
        /// Tests the <see cref="Statistics.Correlation(Tuple{Double, Double}[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsCorrelationTest()
        {
            // using tuple array parameter
            Statistics.Correlation(Tuple.Create(0.0, 0.0)).ShouldBe(0);
            Statistics.Correlation(Tuple.Create(1.0, 1.0)).ShouldBe(0);
            Statistics.Correlation(Tuple.Create(1.0, 1.0), Tuple.Create(1.0, 1.0)).ShouldBe(0);

            Tuple<Double, Double>[] data = new Tuple<Double, Double>[]
            {
                Tuple.Create(-1.0, 2.0), Tuple.Create(7.0, -8.0),
                Tuple.Create(54.0, 5.0), Tuple.Create(5.7, 15.0), Tuple.Create(-5.0, 7.874), Tuple.Create(5.7, 8.2),
                Tuple.Create(10.0, -20.0)
            };
            Statistics.Correlation(data).ShouldBe(-0.0037, 0.0001);

            // using tuple collection parameter
            Statistics.Correlation(new List<Tuple<Double, Double>> { Tuple.Create(1.0, 1.0) }).ShouldBe(0);
            Statistics.Correlation(Enumerable.Range(0, 10).Select(value => Tuple.Create(value * 2.0, (Double)value))).ShouldBe(1);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.Correlation((Tuple<Double, Double>[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Correlation((IEnumerable<Tuple<Double, Double>>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Covariance(Tuple{Double, Double}[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsCovarianceTest()
        {
            // using tuple array parameter
            Statistics.Covariance(new Tuple<Double, Double>[0]).ShouldBe(0);
            Statistics.Covariance(Tuple.Create(1.0, 1.0)).ShouldBe(0);
            Statistics.Covariance(Tuple.Create(1.0, 1.0), Tuple.Create(1.0, 1.0)).ShouldBe(0);

            Tuple<Double, Double>[] data = new Tuple<Double, Double>[]
            {
                Tuple.Create(-1.0, 2.0), Tuple.Create(7.0, -8.0), Tuple.Create(54.0, 5.0),
                Tuple.Create(5.7, 15.0), Tuple.Create(-5.0, 7.874), Tuple.Create(5.7, 8.2),
                Tuple.Create(10.0, -20.0)
            };
            Math.Round(Statistics.Covariance(data), 2).ShouldBe(-0.73);

            // using tuple collection parameter
            Statistics.Covariance(new List<Tuple<Double, Double>> { Tuple.Create(1.0, 1.0) }).ShouldBe(0);
            Statistics.Covariance(new List<Tuple<Double, Double>> { Tuple.Create(5.54, 7.2), Tuple.Create(5.54, 7.2) }).ShouldBe(0);
            Statistics.Covariance(Enumerable.Range(0, 10).Select(value => Tuple.Create(value * 2.0, (Double)value))).ShouldBe(16.5);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.Covariance((Tuple<Double, Double>[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Covariance((IEnumerable<Tuple<Double, Double>>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Mean(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsMeanTest()
        {
            // generic cases
            Statistics.Mean(125).ShouldBe(125);
            Statistics.Mean(1, 2, 3, 4, 5).ShouldBe(3);
            Statistics.Mean(12, 55, 74, 79, 90).ShouldBe(62);
            Statistics.Mean(-10, -10, -10).ShouldBe(-10);
            Statistics.Mean(0, 1).ShouldBe(0.5);

            // using collection parameter
            Statistics.Mean(10, 30, -40).ShouldBe(0);
            Statistics.Mean(Enumerable.Range(0, 10)).ShouldBe(4.5);
            Statistics.Mean(14, 7, 9, 5, -15, -95, 47).ShouldBe(-4);

            // different types
            Statistics.Mean(new List<SByte> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<Int16> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<Int32> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<Int64> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<Byte> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<UInt16> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<UInt32> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<UInt64> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<Single> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(new List<Double> { 1, 3, 5 }).ShouldBe(3);

            // empty collections
            Statistics.Mean(new SByte[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new Int16[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new Int32[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new Int64[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new Byte[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new UInt16[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new UInt32[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new UInt64[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new Double[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new Double[0]).ShouldBe(Double.NaN);

            // null values
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((SByte[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((Int16[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((Int32[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((Int64[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((Byte[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((UInt16[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((UInt32[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((UInt64[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((Single[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Mean((Double[])null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Median(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsMedianTest()
        {
            // using array parameter
            Statistics.Median(Enumerable.Repeat(4, 100)).ShouldBe(4);
            Statistics.Median(Enumerable.Range(0, 9)).ShouldBe(4);
            Statistics.Median(new List<Int32> { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).ShouldBe(3.5);

            // using collection parameter
            Statistics.Median(Enumerable.Range(0, 9)).ShouldBe(4);
            Statistics.Median(new List<Double> { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8 }).ShouldBe(3);

            // exceptions
            Should.Throw<ArgumentException>(() => Statistics.Median(new List<Double> { }));
            Should.Throw<ArgumentException>(() => Statistics.Median(new List<Int32> { }));
            Should.Throw<ArgumentNullException>(() => Statistics.Median((IEnumerable<Double>)null));
            Should.Throw<ArgumentNullException>(() => Statistics.Median((IEnumerable<Int32>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.StandardErrorOfMean(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsStandardErrorOfMeanTest()
        {
            // using array parameter
            Statistics.StandardErrorOfMean(new Double[0]).ShouldBe(0);
            Statistics.StandardErrorOfMean(4.25).ShouldBe(0);
            Statistics.StandardErrorOfMean(1.7, -43, 7.984, 12, -12, 0, 1.7).ShouldBe(6.49, 0.01);
            Statistics.StandardErrorOfMean(5.1, 5.1, 5.1).ShouldBe(0);

            // using collection parameter
            Statistics.StandardErrorOfMean(new List<Double> { }).ShouldBe(0);
            Statistics.StandardErrorOfMean(Enumerable.Range(0, 5).Select(value => value * 2)).ShouldBe(1.26, 0.01);
            Statistics.StandardErrorOfMean(new List<Double> { 1.7, -43, 7.984, 12, -12, 0, 1.7 }).ShouldBe(6.49, 0.01);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.StandardErrorOfMean((Double[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.StandardErrorOfMean((IEnumerable<Double>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.CorrectedStandardDeviation(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsCorrectedStandardDeviationTest()
        {
            // using array parameter
            Statistics.CorrectedStandardDeviation(new Double[0]).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(1.253).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(600, 470, 170, 430, 300).ShouldBe(164.71, 0.01);
            Statistics.CorrectedStandardDeviation(1, 3, 8, 3, 7, 11, 8, 3, 9, 10).ShouldBe(3.49, 0.01);
            Statistics.CorrectedStandardDeviation(-50, -7, 12, 17, 28).ShouldBe(30.68, 0.01);
            Statistics.CorrectedStandardDeviation(-5, 7, 11.2, 103.7, -98).ShouldBe(71.56, 0.01);

            // using collection parameter
            Statistics.CorrectedStandardDeviation(new List<Double>()).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Double> { 1.253 }).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(164.71, 0.01);
            Statistics.CorrectedStandardDeviation(Enumerable.Range(0, 11)).ShouldBe(3.31, 0.01);

            Statistics.CorrectedStandardDeviation(new List<Int32>()).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Int32> { 1 }).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Int32> { 600, 470, 170, 430, 300 }).ShouldBe(164.71, 0.01);
            Statistics.CorrectedStandardDeviation(Enumerable.Repeat(12, 20)).ShouldBe(0);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.CorrectedStandardDeviation((Double[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.CorrectedStandardDeviation((IEnumerable<Double>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.StandardDeviation(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsStandardDeviationTest()
        {
            // using array parameter
            Statistics.StandardDeviation(new Double[0]).ShouldBe(0);
            Statistics.StandardDeviation(1.253).ShouldBe(0);
            Statistics.StandardDeviation(600, 470, 170, 430, 300).ShouldBe(147.32, 0.01);
            Statistics.StandardDeviation(1, 3, 8, 3, 7, 11, 8, 3, 9, 10).ShouldBe(3.31, 0.01);
            Statistics.StandardDeviation(-50, -7, 12, 17, 28).ShouldBe(27.44, 0.01);
            Statistics.StandardDeviation(-5, 7, 11.2, 103.7, -98).ShouldBe(64, 0.01);

            // using collection parameter
            Statistics.StandardDeviation(new List<Double>()).ShouldBe(0);
            Statistics.StandardDeviation(new List<Double> { 1.253 }).ShouldBe(0);
            Statistics.StandardDeviation(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(147.32, 0.01);
            Statistics.StandardDeviation(Enumerable.Range(0, 11)).ShouldBe(3.16, 0.01);

            Statistics.StandardDeviation(new List<Int32>()).ShouldBe(0);
            Statistics.StandardDeviation(new List<Int32> { 1 }).ShouldBe(0);
            Statistics.StandardDeviation(new List<Int32> { 600, 470, 170, 430, 300 }).ShouldBe(147.32, 0.01);
            Statistics.StandardDeviation(Enumerable.Repeat(12, 20)).ShouldBe(0);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.StandardDeviation((Double[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.StandardDeviation((IEnumerable<Double>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.OneSampleTTest(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsOneSampleTTestTest()
        {
            // small populations
            Statistics.OneSampleTTest(new Double[] { }, 0).ShouldBe(Double.NaN);
            Statistics.OneSampleTTest(Enumerable.Repeat(2, 10), 2).ShouldBe(0);
            Statistics.OneSampleTTest(Enumerable.Range(1, 10), 0).ShouldBe(5.75, 0.01);
            Statistics.OneSampleTTest(Enumerable.Range(1, 10), 5).ShouldBe(0.52, 0.01);
            Statistics.OneSampleTTest(new Double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 }, 30).ShouldBe(0.739, 0.01);
            Statistics.OneSampleTTest(new Double[] { -5, 7, 15.25, 97 }, 50).ShouldBe(-0.92, 0.01);
            Statistics.OneSampleTTest(new Double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, -5).ShouldBe(9.92, 0.01);

            // large populations
            Statistics.OneSampleTTest(Enumerable.Repeat(2, 50), 2).ShouldBe(0);
            Statistics.OneSampleTTest(Enumerable.Range(1, 100), 50).ShouldBe(0.17, 0.01);
            Statistics.OneSampleTTest(Enumerable.Range(1, 100), 100).ShouldBe(-17.15, 0.01);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.OneSampleTTest((Double[])null, 0));
            Should.Throw<ArgumentNullException>(() => Statistics.OneSampleTTest((IEnumerable<Double>)null, 0));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.TwoSampleTTest(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsTwoSampleTTestTest()
        {
            // small population
            Statistics.TwoSampleTTest(new Double[] { }, new Double[] { }).ShouldBe(Double.NaN);
            Statistics.TwoSampleTTest(Enumerable.Repeat(2, 10), Enumerable.Repeat(2, 10)).ShouldBe(0);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 10), Enumerable.Range(1, 10)).ShouldBe(0, 0.01);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 10), Enumerable.Range(1, 10).Select(value => 2 * value)).ShouldBe(-2.56, 0.01);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 10).Select(value => 2 * value), Enumerable.Range(1, 10)).ShouldBe(2.56, 0.01);
            Statistics.TwoSampleTTest(new Double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 }, new Double[] { 29.89, 29.93, 29.72, 29.98, 30.02, 29.98 }).ShouldBe(1.959, 0.01);
            Statistics.TwoSampleTTest(new Double[] { -5, 7, 15.25, 97 }, new Double[] { 100, -19, 20.5, 97 }).ShouldBe(-0.56, 0.01);
            Statistics.TwoSampleTTest(new Double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new Double[] { -10, -9, -8, -7, -6, -5, -4, -3, -2, -1 }).ShouldBe(7.38, 0.01);

            // large populations
            Statistics.TwoSampleTTest(Enumerable.Repeat(2, 50), Enumerable.Repeat(2, 50)).ShouldBe(0);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 100), Enumerable.Range(1, 100)).ShouldBe(0, 0.01);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 100), Enumerable.Range(1, 100).Select(value => 2 * value)).ShouldBe(-7.82, 0.01);

            // different types
            Statistics.TwoSampleTTest(new List<SByte> { 1, 3, 5 }, new List<SByte> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<Int16> { 1, 3, 5 }, new List<Int16> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<Int32> { 1, 3, 5 }, new List<Int32> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<Int64> { 1, 3, 5 }, new List<Int64> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<Byte> { 1, 3, 5 }, new List<Byte> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<UInt16> { 1, 3, 5 }, new List<UInt16> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<UInt32> { 1, 3, 5 }, new List<UInt32> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<UInt64> { 1, 3, 5 }, new List<UInt64> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<Single> { 1, 3, 5 }, new List<Single> { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new List<Double> { 1, 3, 5 }, new List<Double> { 1, 3, 5 }).ShouldBe(0);

            // null values
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (SByte[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (Int16[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (Int32[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (Int64[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (Byte[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (UInt16[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (UInt32[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (UInt64[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (Single[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, (Double[])null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Variance(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsVarianceTest()
        {
            // using array parameter
            Statistics.Variance(new Double[0]).ShouldBe(0);
            Statistics.Variance(1.253).ShouldBe(0);
            Statistics.Variance(600, 470, 170, 430, 300).ShouldBe(21704, 0.01);
            Statistics.Variance(1, 3, 8, 3, 7, 11, 8, 3, 9, 10).ShouldBe(11.01, 0.01);
            Statistics.Variance(-50, -7, 12, 17, 28).ShouldBe(753.2, 0.01);
            Statistics.Variance(-5, 7, 11.2, 103.7, -98).ShouldBe(4097.14, 0.01);

            // using collection parameter
            Statistics.Variance(new List<Double>()).ShouldBe(0);
            Statistics.Variance(new List<Double> { 1.253 }).ShouldBe(0);
            Statistics.Variance(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(21704, 0.01);
            Statistics.Variance(Enumerable.Range(0, 11)).ShouldBe(10, 0.01);

            Statistics.Variance(new List<Int32>()).ShouldBe(0);
            Statistics.Variance(new List<Int32> { 1 }).ShouldBe(0);
            Statistics.Variance(new List<Int32> { 600, 470, 170, 430, 300 }).ShouldBe(21704, 0.01);
            Statistics.Variance(Enumerable.Repeat(12, 20)).ShouldBe(0);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.Variance((Double[])null));
            Should.Throw<ArgumentNullException>(() => Statistics.Variance((IEnumerable<Double>)null));
        }
    }
}
