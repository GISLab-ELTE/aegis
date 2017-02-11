// <copyright file="Statistics.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Numerics.Resources;

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
        public static Double Correlation(IEnumerable<Byte> first, IEnumerable<Byte> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<SByte> first, IEnumerable<SByte> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<Int16> first, IEnumerable<Int16> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<Int32> first, IEnumerable<Int32> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<Int64> first, IEnumerable<Int64> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<UInt16> first, IEnumerable<UInt16> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<UInt32> first, IEnumerable<UInt32> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<UInt64> first, IEnumerable<UInt64> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
        public static Double Correlation(IEnumerable<Single> first, IEnumerable<Single> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double covariance = Covariance(first, second);

            if (covariance == 0)
                return 0;

            return covariance / (StandardDeviation(first) * StandardDeviation(second));
        }

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
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

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
        public static Double Correlation(this IEnumerable<Tuple<Byte, Byte>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<SByte, SByte>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<Int16, Int16>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<Int32, Int32>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<Int64, Int64>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<UInt16, UInt16>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<UInt32, UInt32>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<UInt64, UInt64>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(this IEnumerable<Tuple<Single, Single>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
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
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<Byte, Byte>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<SByte, SByte>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<Int16, Int16>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<Int32, Int32>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<Int64, Int64>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<UInt16, UInt16>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<UInt32, UInt32>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<UInt64, UInt64>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<Single, Single>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Correlation(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the correlation of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The correlation.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Correlation(params Tuple<Double, Double>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

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
        public static Double Covariance(this IEnumerable<Byte> first, IEnumerable<Byte> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<Byte> firstEnum = first.GetEnumerator();
            IEnumerator<Byte> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<SByte> first, IEnumerable<SByte> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<SByte> firstEnum = first.GetEnumerator();
            IEnumerator<SByte> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<Int16> first, IEnumerable<Int16> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<Int16> firstEnum = first.GetEnumerator();
            IEnumerator<Int16> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<Int32> first, IEnumerable<Int32> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<Int32> firstEnum = first.GetEnumerator();
            IEnumerator<Int32> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<Int64> first, IEnumerable<Int64> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<Int64> firstEnum = first.GetEnumerator();
            IEnumerator<Int64> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<UInt16> first, IEnumerable<UInt16> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<UInt16> firstEnum = first.GetEnumerator();
            IEnumerator<UInt16> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<UInt32> first, IEnumerable<UInt32> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<UInt32> firstEnum = first.GetEnumerator();
            IEnumerator<UInt32> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<UInt64> first, IEnumerable<UInt64> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<UInt64> firstEnum = first.GetEnumerator();
            IEnumerator<UInt64> secondEnum = second.GetEnumerator();

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
        public static Double Covariance(this IEnumerable<Single> first, IEnumerable<Single> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            if (!first.Any() || !second.Any())
                return 0;

            Double firstMean = Mean(first);
            Double secondMean = Mean(second);

            IEnumerator<Single> firstEnum = first.GetEnumerator();
            IEnumerator<Single> secondEnum = second.GetEnumerator();

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
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

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
        public static Double Covariance(this IEnumerable<Tuple<Byte, Byte>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<SByte, SByte>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<Int16, Int16>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<Int32, Int32>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<Int64, Int64>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<UInt16, UInt16>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<UInt32, UInt32>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<UInt64, UInt64>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(this IEnumerable<Tuple<Single, Single>> valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
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
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<Byte, Byte>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<SByte, SByte>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<Int16, Int16>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<Int32, Int32>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<Int64, Int64>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<UInt16, UInt16>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<UInt32, UInt32>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<UInt64, UInt64>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<Single, Single>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the covariance of the specified value pairs.
        /// </summary>
        /// <param name="valuePairs">The value pairs.</param>
        /// <returns>The covariance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of value pairs is null.</exception>
        public static Double Covariance(params Tuple<Double, Double>[] valuePairs)
        {
            if (valuePairs == null)
                throw new ArgumentNullException(nameof(valuePairs), NumericsMessages.ValuePairCollectionIsNull);

            return Covariance(valuePairs.Select(value => value.Item1), valuePairs.Select(value => value.Item2));
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<Byte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Byte> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<SByte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<SByte> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<Int16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int16> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int32> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<Int64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int64> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<UInt16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt16> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<UInt32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt32> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<UInt64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt64> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<Single> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Single> enumerator = values.GetEnumerator();

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
        /// <param name="values">The collection of values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

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
        public static Double Mean(params Byte[] values)
        {
            return Mean(values as IEnumerable<Byte>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params SByte[] values)
        {
            return Mean(values as IEnumerable<SByte>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params Int16[] values)
        {
            return Mean(values as IEnumerable<Int16>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params Int32[] values)
        {
            return Mean(values as IEnumerable<Int32>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params Int64[] values)
        {
            return Mean(values as IEnumerable<Int64>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params UInt16[] values)
        {
            return Mean(values as IEnumerable<UInt16>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params UInt32[] values)
        {
            return Mean(values as IEnumerable<UInt32>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params UInt64[] values)
        {
            return Mean(values as IEnumerable<UInt64>);
        }

        /// <summary>
        /// Computes the mean of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The mean of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Mean(params Single[] values)
        {
            return Mean(values as IEnumerable<Single>);
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
        public static Double Median(this IEnumerable<Byte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            Byte[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<SByte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            SByte[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<Int16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            Int16[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            Int32[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<Int64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            Int64[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<UInt16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            UInt16[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<UInt32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            UInt32[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<UInt64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            UInt64[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2.0;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the median of the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The median of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<Single> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            Single[] orderedValues = values.ToArray();

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
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Double Median(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

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
        /// <exception cref="System.ArgumentException">The collection of values is empty.</exception>
        public static Decimal Median(this IEnumerable<Decimal> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                throw new ArgumentException(NumericsMessages.ValueCollectionIsEmpty, nameof(values));

            Decimal[] orderedValues = values.ToArray();

            Array.Sort(orderedValues);

            if (orderedValues.Length % 2 == 0)
                return (orderedValues[orderedValues.Length / 2 - 1] + orderedValues[orderedValues.Length / 2]) / 2;

            return orderedValues[orderedValues.Length / 2];
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(this IEnumerable<Byte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Byte> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<SByte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<SByte> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<Int16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int16> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int32> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<Int64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int64> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<UInt16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt16> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<UInt32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt32> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<UInt64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt64> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<Single> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Single> enumerator = values.GetEnumerator();

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
        public static Double CorrectedStandardDeviation(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

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
        public static Double CorrectedStandardDeviation(params Byte[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<Byte>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params SByte[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<SByte>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params Int16[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<Int16>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params Int32[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<Int32>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params Int64[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<Int64>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params UInt16[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<UInt16>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params UInt32[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<UInt32>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params UInt64[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<UInt64>);
        }

        /// <summary>
        /// Computes the corrected standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The corrected standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double CorrectedStandardDeviation(params Single[] values)
        {
            return CorrectedStandardDeviation(values as IEnumerable<Single>);
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
        public static Double StandardDeviation(this IEnumerable<Byte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<SByte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<Int16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<Int64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<UInt16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<UInt32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<UInt64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(this IEnumerable<Single> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
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
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            return Math.Sqrt(Variance(values));
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params Byte[] values)
        {
            return StandardDeviation(values as IEnumerable<Byte>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params SByte[] values)
        {
            return StandardDeviation(values as IEnumerable<SByte>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params Int16[] values)
        {
            return StandardDeviation(values as IEnumerable<Int16>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params Int32[] values)
        {
            return StandardDeviation(values as IEnumerable<Int32>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params Int64[] values)
        {
            return StandardDeviation(values as IEnumerable<Int64>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params UInt16[] values)
        {
            return StandardDeviation(values as IEnumerable<UInt16>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params UInt32[] values)
        {
            return StandardDeviation(values as IEnumerable<UInt32>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params UInt64[] values)
        {
            return StandardDeviation(values as IEnumerable<UInt64>);
        }

        /// <summary>
        /// Computes the standard deviation of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The standard deviation of the values.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardDeviation(params Single[] values)
        {
            return StandardDeviation(values as IEnumerable<Single>);
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
        public static Double StandardErrorOfMean(this IEnumerable<Byte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<Byte> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<SByte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<SByte> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<Int16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<Int16> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<Int32> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<Int64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<Int64> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<UInt16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<UInt16> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<UInt32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<UInt32> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<UInt64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<UInt64> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<Single> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (!values.Any())
                return 0;

            Double mean = Mean(values);
            Double sum = 0;
            Int32 count = 0;

            IEnumerator<Single> enumerator = values.GetEnumerator();

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
        public static Double StandardErrorOfMean(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

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
        public static Double StandardErrorOfMean(params Byte[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<Byte>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params SByte[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<SByte>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params Int16[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<Int16>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params Int32[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<Int32>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params Int64[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<Int64>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params UInt16[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<UInt16>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params UInt32[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<UInt32>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params UInt64[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<UInt64>);
        }

        /// <summary>
        /// Computes the standard error of the mean for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The error of the mean.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double StandardErrorOfMean(params Single[] values)
        {
            return StandardErrorOfMean(values as IEnumerable<Single>);
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
        public static Double OneSampleTTest(IEnumerable<Byte> values, Byte targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Byte> enumerator = values.GetEnumerator();

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

            sum = 0;
            enumerator = values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                sum += (enumerator.Current - mean) * (enumerator.Current - mean);
            }

            // use corrected standard deviation in case of small population
            if (count < 30)
                return (mean - targetValue) / (Math.Sqrt(sum / (count - 1)) / Math.Sqrt(count));

            return (mean - targetValue) / (Math.Sqrt(sum) / count);
        }

        /// <summary>
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<SByte> values, SByte targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<SByte> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<Int16> values, Int16 targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int16> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<Int32> values, Int32 targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int32> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<Int64> values, Int64 targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int64> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<UInt16> values, UInt16 targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt16> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<UInt32> values, UInt32 targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt32> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<UInt64> values, UInt64 targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt64> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<Single> values, Single targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Single> enumerator = values.GetEnumerator();

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
        /// Computes a one-sample T-test.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <param name="targetValue">The target value.</param>
        /// <returns>The result of the one-sample T-test.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double OneSampleTTest(IEnumerable<Double> values, Double targetValue)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

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
        public static Double TwoSampleTTest(IEnumerable<Byte> first, IEnumerable<Byte> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<Byte> firstEnumerator = first.GetEnumerator();
            IEnumerator<Byte> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<SByte> first, IEnumerable<SByte> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<SByte> firstEnumerator = first.GetEnumerator();
            IEnumerator<SByte> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<Int16> first, IEnumerable<Int16> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<Int16> firstEnumerator = first.GetEnumerator();
            IEnumerator<Int16> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<Int32> first, IEnumerable<Int32> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<Int32> firstEnumerator = first.GetEnumerator();
            IEnumerator<Int32> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<Int64> first, IEnumerable<Int64> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<Int64> firstEnumerator = first.GetEnumerator();
            IEnumerator<Int64> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<UInt16> first, IEnumerable<UInt16> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<UInt16> firstEnumerator = first.GetEnumerator();
            IEnumerator<UInt16> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<UInt32> first, IEnumerable<UInt32> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<UInt32> firstEnumerator = first.GetEnumerator();
            IEnumerator<UInt32> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<UInt64> first, IEnumerable<UInt64> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<UInt64> firstEnumerator = first.GetEnumerator();
            IEnumerator<UInt64> secondEnumerator = second.GetEnumerator();

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
        public static Double TwoSampleTTest(IEnumerable<Single> first, IEnumerable<Single> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

            IEnumerator<Single> firstEnumerator = first.GetEnumerator();
            IEnumerator<Single> secondEnumerator = second.GetEnumerator();

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
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondCollectionIsNull);

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
        public static Double Variance(this IEnumerable<Byte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Byte> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<SByte> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<SByte> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<Int16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int16> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int32> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<Int64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Int64> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<UInt16> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt16> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<UInt32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt32> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<UInt64> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<UInt64> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<Single> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            IEnumerator<Single> enumerator = values.GetEnumerator();

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
        public static Double Variance(this IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

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
        public static Double Variance(params Byte[] values)
        {
            return Variance(values as IEnumerable<Byte>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params SByte[] values)
        {
            return Variance(values as IEnumerable<SByte>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params Int16[] values)
        {
            return Variance(values as IEnumerable<Int16>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params Int32[] values)
        {
            return Variance(values as IEnumerable<Int32>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params Int64[] values)
        {
            return Variance(values as IEnumerable<Int64>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params UInt16[] values)
        {
            return Variance(values as IEnumerable<UInt16>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params UInt32[] values)
        {
            return Variance(values as IEnumerable<UInt32>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params UInt64[] values)
        {
            return Variance(values as IEnumerable<UInt64>);
        }

        /// <summary>
        /// Computes the variance of the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The variance.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Double Variance(params Single[] values)
        {
            return Variance(values as IEnumerable<Single>);
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
