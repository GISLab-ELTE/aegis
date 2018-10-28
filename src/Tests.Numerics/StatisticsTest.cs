// <copyright file="StatisticsTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Numerics;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Statistics" /> class.
    /// </summary>
    [TestFixture]
    public class StatisticsTest
    {
        /// <summary>
        /// Tests the <see cref="Statistics.Correlation(IEnumerable{Double}, IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsCorrelationCollectionsTest()
        {
            Statistics.Correlation(new Double[0], new Double[0]).ShouldBe(0);
            Statistics.Correlation(new Double[] { 1 }, new Double[] { 1 }).ShouldBe(0);
            Statistics.Correlation(new Double[] { 1, 1 }, new Double[] { 1, 1 }).ShouldBe(0);
            Statistics.Correlation(new Double[] { 5.54, 5.54 }, new Double[] { 7.2, 7.2 }).ShouldBe(0);
            Statistics.Correlation(Enumerable.Range(0, 10).Select(value => 2.0 * value), Enumerable.Range(0, 10).Select(value => Convert.ToDouble(value))).ShouldBe(1);
            Statistics.Correlation(new Double[] { -1, 7, 54, 5.7, -5, 5.7, 10 }, new Double[] { 2, -8, 5, 15, 7.874, 8.2, -20 }).ShouldBe(-0.0037, 0.01);

            Should.Throw<ArgumentNullException>(() => Statistics.Correlation(null, new Double[0]));
            Should.Throw<ArgumentNullException>(() => Statistics.Correlation(new Double[0], null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Correlation(Tuple{Double, Double}[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsCorrelationTupleTest()
        {
            Statistics.Correlation(new List<Tuple<Double, Double>> { Tuple.Create(0.0, 0.0) }).ShouldBe(0);
            Statistics.Correlation(new List<Tuple<Double, Double>> { Tuple.Create(1.0, 1.0) }).ShouldBe(0);
            Statistics.Correlation(new List<Tuple<Double, Double>> { Tuple.Create(1.0, 1.0), Tuple.Create(1.0, 1.0) }).ShouldBe(0);
            Statistics.Correlation(Enumerable.Range(0, 10).Select(value => Tuple.Create(value * 2.0, (Double)value))).ShouldBe(1);

            Tuple<Double, Double>[] data = new Tuple<Double, Double>[]
            {
                Tuple.Create(-1.0, 2.0), Tuple.Create(7.0, -8.0),
                Tuple.Create(54.0, 5.0), Tuple.Create(5.7, 15.0), Tuple.Create(-5.0, 7.874), Tuple.Create(5.7, 8.2),
                Tuple.Create(10.0, -20.0)
            };
            Statistics.Correlation(data).ShouldBe(-0.0037, 0.0001);

            Should.Throw<ArgumentNullException>(() => Statistics.Correlation(null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Covariance(IEnumerable{Double}, IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsCovarianceCollectionsTest()
        {
            Statistics.Covariance(new Double[0], new Double[0]).ShouldBe(0);
            Statistics.Covariance(new Double[] { 1 }, new Double[] { 1 }).ShouldBe(0);
            Statistics.Covariance(new Double[] { 1, 1 }, new Double[] { 1, 1 }).ShouldBe(0);
            Statistics.Covariance(new Double[] { 5.54, 5.54 }, new Double[] { 7.2, 7.2 }).ShouldBe(0);
            Statistics.Covariance(Enumerable.Range(0, 10).Select(value => 2.0 * value), Enumerable.Range(0, 10).Select(value => Convert.ToDouble(value))).ShouldBe(16.5);
            Statistics.Covariance(new Double[] { -1, 7, 54, 5.7, -5, 5.7, 10 }, new Double[] { 2, -8, 5, 15, 7.874, 8.2, -20 }).ShouldBe(-0.73, 0.01);

            Should.Throw<ArgumentNullException>(() => Statistics.Covariance(null, new Double[0]));
            Should.Throw<ArgumentNullException>(() => Statistics.Covariance(new Double[0], null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Covariance(IEnumerable{Tuple{Double, Double}})" /> method.
        /// </summary>
        [Test]
        public void StatisticsCovarianceTupleTest()
        {
            Statistics.Covariance(new Tuple<Double, Double>[0]).ShouldBe(0);
            Statistics.Covariance(new List<Tuple<Double, Double>> { Tuple.Create(1.0, 1.0) }).ShouldBe(0);
            Statistics.Covariance(new List<Tuple<Double, Double>> { Tuple.Create(1.0, 1.0), Tuple.Create(1.0, 1.0) }).ShouldBe(0);
            Statistics.Covariance(new List<Tuple<Double, Double>> { Tuple.Create(5.54, 7.2), Tuple.Create(5.54, 7.2) }).ShouldBe(0);
            Statistics.Covariance(Enumerable.Range(0, 10).Select(value => Tuple.Create(value * 2.0, (Double)value))).ShouldBe(16.5);
            Statistics.Covariance(new Tuple<Double, Double>[]
            {
                Tuple.Create(-1.0, 2.0), Tuple.Create(7.0, -8.0), Tuple.Create(54.0, 5.0),
                Tuple.Create(5.7, 15.0), Tuple.Create(-5.0, 7.874), Tuple.Create(5.7, 8.2),
                Tuple.Create(10.0, -20.0)
            }).ShouldBe(-0.73, 0.01);

            Should.Throw<ArgumentNullException>(() => Statistics.Covariance(null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Mean(Double[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsMeanArrayTest()
        {
            Statistics.Mean(1, 2, 3, 4, 5).ShouldBe(3);
            Statistics.Mean(12, 55, 74, 79, 90).ShouldBe(62);
            Statistics.Mean(-10, -10, -10).ShouldBe(-10);
            Statistics.Mean(0, 1).ShouldBe(0.5);
            Statistics.Mean(10, 30, -40).ShouldBe(0);
            Statistics.Mean(14, 7, 9, 5, -15, -95, 47).ShouldBe(-4);
            Statistics.Mean(125).ShouldBe(125);

            Statistics.Mean(new Double[0]).ShouldBe(Double.NaN);
            Statistics.Mean(new Double[0]).ShouldBe(Double.NaN);

            Should.Throw<ArgumentNullException>(() => Statistics.Mean(null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Mean(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsMeanCollectionTest()
        {
            Statistics.Mean(new List<Double> { 1, 3, 5 }).ShouldBe(3);
            Statistics.Mean(Enumerable.Range(0, 10).Select(value => Convert.ToDouble(value))).ShouldBe(4.5);

            Should.Throw<ArgumentNullException>(() => Statistics.Mean((IEnumerable<Double>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Median(Double[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsMedianArrayTest()
        {
            Statistics.Median(1, 2, 3).ShouldBe(2);
            Statistics.Median(1, 1, 1).ShouldBe(1);
            Statistics.Median(1, 1, 1, 1, 2, 3).ShouldBe(1);
            Statistics.Median(-2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9).ShouldBe(3.5);

            Should.Throw<ArgumentNullException>(() => Statistics.Median(null));
            Should.Throw<ArgumentException>(() => Statistics.Median(new Double[] { }));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Median(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsMedianCollectionTest()
        {
            Statistics.Median(Enumerable.Repeat(4.0, 100)).ShouldBe(4);
            Statistics.Median(Enumerable.Range(0, 9).Select(value => Convert.ToDouble(value))).ShouldBe(4);
            Statistics.Median(new List<Double> { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).ShouldBe(3.5);
            Statistics.Median(new List<Double> { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8 }).ShouldBe(3);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.Median((IEnumerable<Double>)null));
            Should.Throw<ArgumentException>(() => Statistics.Median(new List<Double> { }));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.StandardErrorOfMean(Double[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsStandardErrorOfMeanArrayTest()
        {
            Statistics.StandardErrorOfMean(new Double[0]).ShouldBe(0);
            Statistics.StandardErrorOfMean(4.25).ShouldBe(0);
            Statistics.StandardErrorOfMean(1.7, -43, 7.984, 12, -12, 0, 1.7).ShouldBe(6.49, 0.01);
            Statistics.StandardErrorOfMean(5.1, 5.1, 5.1).ShouldBe(0);

            Should.Throw<ArgumentNullException>(() => Statistics.StandardErrorOfMean(null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.StandardErrorOfMean(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsStandardErrorOfMeanCollectionTest()
        {
            Statistics.StandardErrorOfMean(new List<Double> { }).ShouldBe(0);
            Statistics.StandardErrorOfMean(Enumerable.Range(0, 5).Select(value => value * 2.0)).ShouldBe(1.26, 0.01);
            Statistics.StandardErrorOfMean(new List<Double> { 1.7, -43, 7.984, 12, -12, 0, 1.7 }).ShouldBe(6.49, 0.01);

            Should.Throw<ArgumentNullException>(() => Statistics.StandardErrorOfMean((IEnumerable<Double>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.CorrectedStandardDeviation(Double[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsCorrectedStandardDeviationArrayTest()
        {
            Statistics.CorrectedStandardDeviation(new Double[0]).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(1.253).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(600, 470, 170, 430, 300).ShouldBe(164.71, 0.01);
            Statistics.CorrectedStandardDeviation(1, 3, 8, 3, 7, 11, 8, 3, 9, 10).ShouldBe(3.49, 0.01);
            Statistics.CorrectedStandardDeviation(-50, -7, 12, 17, 28).ShouldBe(30.68, 0.01);
            Statistics.CorrectedStandardDeviation(-5, 7, 11.2, 103.7, -98).ShouldBe(71.56, 0.01);

            Should.Throw<ArgumentNullException>(() => Statistics.CorrectedStandardDeviation(null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.CorrectedStandardDeviation(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsCorrectedStandardDeviationTest()
        {
            Statistics.CorrectedStandardDeviation(new List<Double>()).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Double> { 1.253 }).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(164.71, 0.01);
            Statistics.CorrectedStandardDeviation(Enumerable.Range(0, 11).Select(value => Convert.ToDouble(value))).ShouldBe(3.31, 0.01);
            Statistics.CorrectedStandardDeviation(new List<Double>()).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Double> { 1 }).ShouldBe(0);
            Statistics.CorrectedStandardDeviation(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(164.71, 0.01);
            Statistics.CorrectedStandardDeviation(Enumerable.Repeat(12.0, 20)).ShouldBe(0);

            Should.Throw<ArgumentNullException>(() => Statistics.CorrectedStandardDeviation((IEnumerable<Double>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.StandardDeviation(Double[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsStandardDeviationArrayTest()
        {
            Statistics.StandardDeviation(new Double[0]).ShouldBe(0);
            Statistics.StandardDeviation(1.253).ShouldBe(0);
            Statistics.StandardDeviation(600, 470, 170, 430, 300).ShouldBe(147.32, 0.01);
            Statistics.StandardDeviation(1, 3, 8, 3, 7, 11, 8, 3, 9, 10).ShouldBe(3.31, 0.01);
            Statistics.StandardDeviation(-50, -7, 12, 17, 28).ShouldBe(27.44, 0.01);
            Statistics.StandardDeviation(-5, 7, 11.2, 103.7, -98).ShouldBe(64, 0.01);

            Should.Throw<ArgumentNullException>(() => Statistics.StandardDeviation(null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.StandardDeviation(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsStandardDeviationCollectionTest()
        {
            Statistics.StandardDeviation(new List<Double>()).ShouldBe(0);
            Statistics.StandardDeviation(new List<Double> { 1.253 }).ShouldBe(0);
            Statistics.StandardDeviation(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(147.32, 0.01);
            Statistics.StandardDeviation(Enumerable.Range(0, 11).Select(value => Convert.ToDouble(value))).ShouldBe(3.16, 0.01);
            Statistics.StandardDeviation(new List<Double>()).ShouldBe(0);
            Statistics.StandardDeviation(new List<Double> { 1 }).ShouldBe(0);
            Statistics.StandardDeviation(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(147.32, 0.01);
            Statistics.StandardDeviation(Enumerable.Repeat(12.0, 20)).ShouldBe(0);

            Should.Throw<ArgumentNullException>(() => Statistics.StandardDeviation((IEnumerable<Double>)null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.OneSampleTTest(IEnumerable{Double}, Double)" /> method.
        /// </summary>
        [Test]
        public void StatisticsOneSampleTTestTest()
        {
            // small populations
            Statistics.OneSampleTTest(new Double[] { }, 0).ShouldBe(Double.NaN);
            Statistics.OneSampleTTest(new Double[] { 0, 0 }, 1).ShouldBe(0);
            Statistics.OneSampleTTest(Enumerable.Repeat(2.0, 10), 2).ShouldBe(0);
            Statistics.OneSampleTTest(Enumerable.Range(1, 10).Select(value => Convert.ToDouble(value)), 0).ShouldBe(5.75, 0.01);
            Statistics.OneSampleTTest(Enumerable.Range(1, 10).Select(value => Convert.ToDouble(value)), 5).ShouldBe(0.52, 0.01);
            Statistics.OneSampleTTest(new Double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 }, 30).ShouldBe(0.739, 0.01);
            Statistics.OneSampleTTest(new Double[] { -5, 7, 15.25, 97 }, 50).ShouldBe(-0.92, 0.01);
            Statistics.OneSampleTTest(new Double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, -5).ShouldBe(9.92, 0.01);

            // large populations
            Statistics.OneSampleTTest(Enumerable.Repeat(2.0, 50).Select(value => Convert.ToDouble(value)), 2).ShouldBe(0);
            Statistics.OneSampleTTest(Enumerable.Range(1, 100).Select(value => Convert.ToDouble(value)), 50).ShouldBe(0.17, 0.01);
            Statistics.OneSampleTTest(Enumerable.Range(1, 100).Select(value => Convert.ToDouble(value)), 100).ShouldBe(-17.15, 0.01);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.OneSampleTTest(null, 0));
            Should.Throw<ArgumentNullException>(() => Statistics.OneSampleTTest(null, 0));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.TwoSampleTTest(IEnumerable{Double}, IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsTwoSampleTTestTest()
        {
            // small population
            Statistics.TwoSampleTTest(new Double[] { }, new Double[] { }).ShouldBe(Double.NaN);
            Statistics.TwoSampleTTest(Enumerable.Repeat(2.0, 10), Enumerable.Repeat(2.0, 10)).ShouldBe(0);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 10).Select(value => Convert.ToDouble(value)), Enumerable.Range(1, 10).Select(value => Convert.ToDouble(value))).ShouldBe(0, 0.01);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 10).Select(value => Convert.ToDouble(value)), Enumerable.Range(1, 10).Select(value => 2.0 * value)).ShouldBe(-2.56, 0.01);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 10).Select(value => 2.0 * value), Enumerable.Range(1, 10).Select(value => Convert.ToDouble(value))).ShouldBe(2.56, 0.01);
            Statistics.TwoSampleTTest(new Double[] { 1, 3, 5 }, new Double[] { 1, 3, 5 }).ShouldBe(0);
            Statistics.TwoSampleTTest(new Double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 }, new Double[] { 29.89, 29.93, 29.72, 29.98, 30.02, 29.98 }).ShouldBe(1.959, 0.01);
            Statistics.TwoSampleTTest(new Double[] { -5, 7, 15.25, 97 }, new Double[] { 100, -19, 20.5, 97 }).ShouldBe(-0.56, 0.01);
            Statistics.TwoSampleTTest(new Double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new Double[] { -10, -9, -8, -7, -6, -5, -4, -3, -2, -1 }).ShouldBe(7.38, 0.01);

            // large populations
            Statistics.TwoSampleTTest(Enumerable.Repeat(2.0, 50), Enumerable.Repeat(2.0, 50)).ShouldBe(0);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 100).Select(value => Convert.ToDouble(value)), Enumerable.Range(1, 100).Select(value => Convert.ToDouble(value))).ShouldBe(0, 0.01);
            Statistics.TwoSampleTTest(Enumerable.Range(1, 100).Select(value => Convert.ToDouble(value)), Enumerable.Range(1, 100).Select(value => 2.0 * value)).ShouldBe(-7.82, 0.01);

            // exceptions
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(null, new Double[0]));
            Should.Throw<ArgumentNullException>(() => Statistics.TwoSampleTTest(new Double[0], null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Variance(Double[])" /> method.
        /// </summary>
        [Test]
        public void StatisticsVarianceArrayTest()
        {
            Statistics.Variance(new Double[0]).ShouldBe(0);
            Statistics.Variance(1.253).ShouldBe(0);
            Statistics.Variance(600, 470, 170, 430, 300).ShouldBe(21704, 0.01);
            Statistics.Variance(1, 3, 8, 3, 7, 11, 8, 3, 9, 10).ShouldBe(11.01, 0.01);
            Statistics.Variance(-50, -7, 12, 17, 28).ShouldBe(753.2, 0.01);
            Statistics.Variance(-5, 7, 11.2, 103.7, -98).ShouldBe(4097.14, 0.01);

            Should.Throw<ArgumentNullException>(() => Statistics.Variance(null));
        }

        /// <summary>
        /// Tests the <see cref="Statistics.Variance(IEnumerable{Double})" /> method.
        /// </summary>
        [Test]
        public void StatisticsVarianceCollectionTest()
        {
            Statistics.Variance(new List<Double>()).ShouldBe(0);
            Statistics.Variance(new List<Double> { 1.253 }).ShouldBe(0);
            Statistics.Variance(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(21704, 0.01);
            Statistics.Variance(Enumerable.Range(0, 11).Select(value => Convert.ToDouble(value))).ShouldBe(10, 0.01);
            Statistics.Variance(new List<Double>()).ShouldBe(0);
            Statistics.Variance(new List<Double> { 1 }).ShouldBe(0);
            Statistics.Variance(new List<Double> { 600, 470, 170, 430, 300 }).ShouldBe(21704, 0.01);
            Statistics.Variance(Enumerable.Repeat(12.0, 20)).ShouldBe(0);

            Should.Throw<ArgumentNullException>(() => Statistics.Variance((IEnumerable<Double>)null));
        }
    }
}
