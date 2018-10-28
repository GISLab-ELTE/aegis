// <copyright file="Statistics.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Numerics.Resources;

    /// <summary>
    /// Defines basic statistical calculations.
    /// </summary>
    public static class Statistics
    {
        /// <summary>
        /// Computes the correlation of the values within the specified collections.
        /// </summary>
        /// <param name="first">The first collection of values.</param>
        /// <param name="second">The second collection of values.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first collection is null.
        /// or
        /// The second collection is null.
        /// </exception>
        public static Double Correlation(IEnumerable<Double> first, IEnumerable<Double> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<Double, Double>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs));

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the values within the specified collections.
        /// </summary>
        /// <param name="first">The first collection of values.</param>
        /// <param name="second">The second collection of values.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first collection is null.
        /// or
        /// The second collection is null.
        /// </exception>
        public static Double Covariance(this IEnumerable<Double> first, IEnumerable<Double> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<Double> firstEnum = first.GetEnumerator();
            IEnumerator<Double> secondEnum = second.GetEnumerator();

            Double sum = 0;
            Int32 count = 0;
            while (firstEnum.MoveNext() && secondEnum.MoveNext())
            {
                sum += (firstEnum.Current - firstMean) * (secondEnum.Current - secondMean);
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<Double, Double>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs));

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            IEnumerator<Double> enumerator = values.GetEnumerator();

            Double sum = 0;
            Int32 count = 0;
            while (enumerator.MoveNext())
            {
                sum += enumerator.Current;
                count++;
            }

            if (count == 0)
                return Double.NaN;

            return sum / count;
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params Double[] values)
        {
            return Mean(values as IEnumerable<Double>);
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            Double[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Median(params Double[] values)
        {
            return Median(values as IEnumerable<Double>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            IEnumerator<Double> enumerator = values.GetEnumerator();

            Double sum = 0;
            Int32 count = 0;
            while (enumerator.MoveNext())
            {
                sum += enumerator.Current;
                count++;
            }

            if (count < 2)
                return 0;

            Double mean = sum / count;
            enumerator = values.GetEnumerator();
            sum = 0;

            while (enumerator.MoveNext())
            {
                sum += (enumerator.Current - mean) * (enumerator.Current - mean);
            }

            return Math.Sqrt(sum / (count - 1));
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params Double[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<Double>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params Double[] values)
        {
            return StandardDeviation(values as IEnumerable<Double>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<Double> enumerator = values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                sum += (enumerator.Current - mean) * (enumerator.Current - mean);
                count++;
            }

            if (count == 1)
                return 0;

            return Math.Sqrt(sum) / count;
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params Double[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<Double>);
        }

        /// <summary>
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<Double> values, Double targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            IEnumerator<Double> enumerator = values.GetEnumerator();

            Double sum = 0;
            Int32 count = 0;
            while (enumerator.MoveNext())
            {
                sum += enumerator.Current;
                count++;
            }

            if (count == 0)
                return Double.NaN;

            Double mean = sum / count;

            if (mean == targetValue)
                return 0;

            sum = 0;
            enumerator = values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                sum += (enumerator.Current - mean) * (enumerator.Current - mean);
            }

            if (sum == 0)
                return 0;

            // use corrected standard deviation in case of small population
            if (count < 30)
                return (mean - targetValue) / (Math.Sqrt(sum / (count - 1)) / Math.Sqrt(count));

            return (mean - targetValue) / (Math.Sqrt(sum) / count);
        }

        /// <summary>
        /// Computes a two-sample T-test.
        /// </summary>
        /// <param name="first">The first collection of values.</param>
        /// <param name="second">The second collection of values.</param>
        /// <returns>The result of the two-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first collection is null.
        /// or
        /// The second collection is null.
        /// </exception>
        public static Double TwoSampleTTest(IEnumerable<Double> first, IEnumerable<Double> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            IEnumerator<Double> firstEnumerator = first.GetEnumerator();
            IEnumerator<Double> secondEnumerator = second.GetEnumerator();

            Double firstSum = 0;
            Int32 firstCount = 0;
            while (firstEnumerator.MoveNext())
            {
                firstSum += firstEnumerator.Current;
                firstCount++;
            }

            Double secondSum = 0;
            Int32 secondCount = 0;
            while (secondEnumerator.MoveNext())
            {
                secondSum += secondEnumerator.Current;
                secondCount++;
            }

            if (firstCount == 0 || secondCount == 0)
                return Double.NaN;

            Double firstMean = firstSum / firstCount;
            Double secondMean = secondSum / secondCount;

            if (firstMean == secondMean)
                return 0;

            firstSum = 0;
            firstEnumerator = first.GetEnumerator();

            while (firstEnumerator.MoveNext())
            {
                firstSum += (firstEnumerator.Current - firstMean) * (firstEnumerator.Current - firstMean);
            }

            secondSum = 0;
            secondEnumerator = second.GetEnumerator();

            while (secondEnumerator.MoveNext())
            {
                secondSum += (secondEnumerator.Current - secondMean) * (secondEnumerator.Current - secondMean);
            }

            // use corrected standard deviation in case of small population
            if (firstCount < 30 || secondCount < 30)
                return (firstMean - secondMean) / Math.Sqrt(firstSum / (firstCount - 1) / firstCount + secondSum / (secondCount - 1) / secondCount);

            return (firstMean - secondMean) / Math.Sqrt(firstSum / firstCount / firstCount + secondSum / secondCount / secondCount);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            IEnumerator<Double> enumerator = values.GetEnumerator();

            Double sum = 0;
            Int32 count = 0;
            while (enumerator.MoveNext())
            {
                sum += enumerator.Current;
                count++;
            }

            if (count < 2)
                return 0;

            Double mean = sum / count;
            enumerator = values.GetEnumerator();
            sum = 0;

            while (enumerator.MoveNext())
            {
                sum += (enumerator.Current - mean) * (enumerator.Current - mean);
            }

            return sum / count;
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params Double[] values)
        {
            return Variance(values as IEnumerable<Double>);
        }
    }
}
