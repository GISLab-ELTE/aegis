// <copyright file="SplitPolicies.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes.Metric.SplitPolicy
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AEGIS.Resources;

    public static class SplitPolicies
    {
        /// <summary>
        /// A partition policy which's main goal is to reduce the radiuses of the nodes.
        /// </summary>
        /// <remarks>
        /// The advantage of this policy is that the radius of a node will be the minimum possible, thus reducing the chance
        /// of overlapping. The disadvantage is that it results in unbalanced splits, thus resulting in trees with bigger heights.
        /// </remarks>
        /// <typeparam name="T">the type of the data points</typeparam>
        /// <returns>a partition policy with the aforementioned properties</returns>
        public static IPartitionPolicy<T> GeneralizedHyperplanePartition<T>() => new GeneralizedHyperplanePartitionPolicy<T>();

        /// <summary>
        /// A partition policy which prefers perfectly balanced nodes upon split.
        /// </summary>
        /// <remarks>
        /// The advantage of this policy is trivial: it results in perfectly balanced nodes. For that reason, the height of the tree
        /// will be the minimum possible, which can decrease search time. There is a disadvantage though: when the density of the data points
        /// is highly varying (the points are concentrated in particular areas, and sparse in others), it can happen that data points get
        /// assigned to a node which's promoted item is far from the data points, thus resulting in bigger radiuses. This leads to a higher
        /// chance of overlapping. When the density of data points is highly varying it is better to use a <see cref="GeneralizedHyperplanePartition{T}"/>.
        /// </remarks>
        /// <typeparam name="T">the type of the data points</typeparam>
        /// <returns>a partition policy with the aforementioned properties</returns>
        public static IPartitionPolicy<T> BalancedPartition<T>() => new BalancedPartitionPolicy<T>();

        /// <summary>
        /// Chooses the two data points from the <c>dataSet</c> which are the farthest from each other.
        /// </summary>
        /// <typeparam name="T">the type of data points</typeparam>
        /// <returns>a promote policy with the aforementioned properties</returns>
        public static IPromotePolicy<T> MaximumDistancePromote<T>() => new MaximumDistancePromotePolicy<T>();

        /// <summary>
        /// A split policy which's main goal is to reduce the radiuses of the nodes.
        /// </summary>
        /// <remarks>
        /// The advantage of this policy is that the radius of a node will be the minimum possible, thus reducing the chance
        /// of overlapping. The disadvantage is that it results in unbalanced splits, thus resulting in trees with bigger heights.
        /// </remarks>
        /// <typeparam name="T">the type of the data points</typeparam>
        /// <returns>A split policy instance with the aforementioned properties.</returns>
        public static ISplitPolicy<T> MinimumRadiusSplitPolicy<T>() => new SplitPolicy<T>(MaximumDistancePromote<T>(), GeneralizedHyperplanePartition<T>());

        /// <summary>
        /// A split policy which prefers perfectly balanced nodes upon split.
        /// </summary>
        /// <remarks>
        /// The advantage of this policy is trivial: it results in perfectly balanced nodes. For that reason, the height of the tree
        /// will be the minimum possible, which can decrease search time. There is a disadvantage though: when the density of the data points
        /// is highly varying (the points are concentrated in particular areas, and sparse in others), it can happen that data points get
        /// assigned to a node which's promoted item is far from the data points, thus resulting in bigger radiuses. This leads to a higher
        /// chance of overlapping. When the density of data points is highly varying it is better to use a <see cref="MinimumRadiusSplitPolicy{T}"/>.
        /// </remarks>
        /// <typeparam name="T">the type of the data points</typeparam>
        /// <returns>A split policy instance with the aforementioned properties.</returns>
        public static ISplitPolicy<T> BalancedSplitPolicy<T>() => new SplitPolicy<T>(MaximumDistancePromote<T>(), BalancedPartition<T>());

        /// <summary>
        /// A partition policy which's main goal is to reduce the radiuses of the nodes.
        /// </summary>
        /// <remarks>
        /// The advantage of this policy is that the radius of a node will be the minimum possible, thus reducing the chance
        /// of overlapping. The disadvantage is that it results in unbalanced splits, thus resulting in trees with bigger heights.
        /// </remarks>
        /// <typeparam name="T">the type of the data points</typeparam>
        /// <seealso cref="AEGIS.Indexes.Metric.SplitPolicy.IPartitionPolicy{T}" />
        public class GeneralizedHyperplanePartitionPolicy<T> : IPartitionPolicy<T>
        {
            public Tuple<ISet<T>, ISet<T>> Partition(Tuple<T, T> promoted, ICollection<T> dataSet, DistanceMetric<T> distanceMetric)
            {
                ISet<T> first = new HashSet<T>();
                ISet<T> second = new HashSet<T>();

                foreach (T data in dataSet)
                {
                    if (distanceMetric.Invoke(data, promoted.Item1) <= distanceMetric.Invoke(data, promoted.Item2))
                        first.Add(data);
                    else
                        second.Add(data);
                }

                return new Tuple<ISet<T>, ISet<T>>(first, second);
            }
        }

        /// <summary>
        /// A partition policy which prefers perfectly balanced nodes upon split.
        /// </summary>
        /// <remarks>
        /// The advantage of this policy is trivial: it results in perfectly balanced nodes. For that reason, the height of the tree
        /// will be the minimum possible, which can decrease search time. There is a disadvantage though: when the density of the data points
        /// is highly varying (the points are concentrated in particular areas, and sparse in others), it can happen that data points get
        /// assigned to a node which's promoted item is far from the data points, thus resulting in bigger radiuses. This leads to a higher
        /// chance of overlapping. When the density of data points is highly varying it is better to use a <see cref="GeneralizedHyperplanePartition{T}"/>.
        /// </remarks>
        /// <typeparam name="T">the type of the data points</typeparam>
        /// <seealso cref="AEGIS.Indexes.Metric.SplitPolicy.IPartitionPolicy{T}" />
        public class BalancedPartitionPolicy<T> : IPartitionPolicy<T>
        {
            public Tuple<ISet<T>, ISet<T>> Partition(Tuple<T, T> promoted, ICollection<T> dataSet, DistanceMetric<T> distanceMetric)
            {
                List<T> queue1 = new List<T>(dataSet);
                queue1.Sort((data1, data2) =>
                {
                    Double distance1 = distanceMetric.Invoke(data1, promoted.Item1);
                    Double distance2 = distanceMetric.Invoke(data2, promoted.Item1);
                    return distance1.CompareTo(distance2);
                });

                List<T> queue2 = new List<T>(dataSet);
                queue2.Sort((data1, data2) =>
                {
                    Double distance1 = distanceMetric.Invoke(data1, promoted.Item2);
                    Double distance2 = distanceMetric.Invoke(data2, promoted.Item2);
                    return distance1.CompareTo(distance2);
                });

                ISet<T> first = new HashSet<T>();
                ISet<T> second = new HashSet<T>();

                int index1 = 0;
                int index2 = 0;

                while (index1 < queue1.Count || index2 != queue2.Count)
                {
                    while (index1 < queue1.Count)
                    {
                        T data = queue1[index1++];
                        if (!second.Contains(data))
                        {
                            first.Add(data);
                            break;
                        }
                    }

                    while (index2 < queue2.Count)
                    {
                        T data = queue2[index2++];
                        if (!first.Contains(data))
                        {
                            second.Add(data);
                            break;
                        }
                    }
                }

                return new Tuple<ISet<T>, ISet<T>>(first, second);
            }
        }

        /// <summary>
        /// Chooses the two data points from the <c>dataSet</c> which are the farthest from each other.
        /// </summary>
        /// <typeparam name="T">the type of data points</typeparam>
        /// <seealso cref="AEGIS.Indexes.Metric.SplitPolicy.IPromotePolicy{T}" />
        public class MaximumDistancePromotePolicy<T> : IPromotePolicy<T>
        {
            /// <summary>
            /// Chooses the two items from the <c>dataSet</c> which are the farthest from each other.
            /// </summary>
            /// <param name="dataSet">The data set.</param>
            /// <param name="distanceMetric">The distance metric.</param>
            /// <returns>
            /// a pair of promoted items
            /// </returns>
            public Tuple<T, T> Promote(ICollection<T> dataSet, DistanceMetric<T> distanceMetric)
            {
                T[] data = new T[dataSet.Count];
                dataSet.CopyTo(data, 0);
                T first = data[0];
                T second = data[1];
                Double maxDistance = distanceMetric.Invoke(first, second);
                for (int i = 0; i < data.Length; i++)
                {
                    for (int j = i + 1; j < data.Length; j++)
                    {
                        Double distance = distanceMetric.Invoke(data[i], data[j]);
                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            first = data[i];
                            second = data[j];
                        }
                    }
                }

                return new Tuple<T, T>(first, second);
            }
        }
    }
}
