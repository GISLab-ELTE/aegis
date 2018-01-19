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
        public static IPartitionPolicy<T> GeneralizedHyperplanePartition<T>() => new GeneralizedHyperplanePartitionPolicy<T>();

        public static IPartitionPolicy<T> BalancedPartition<T>() => new BalancedPartitionPolicy<T>();

        public static IPromotePolicy<T> MaximumDistancePromote<T>() => new MaximumDistancePromotePolicy<T>();

        public static IPromotePolicy<T> RandomPromote<T>() => new RandomPromotePolicy<T>();

        public static ISplitPolicy<T> LowCostSplitPolicy<T>() => new SplitPolicy<T>(RandomPromote<T>(), GeneralizedHyperplanePartition<T>());

        public static ISplitPolicy<T> SmartSplitPolicy<T>() => new SplitPolicy<T>(MaximumDistancePromote<T>(), BalancedPartition<T>());

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

        public class MaximumDistancePromotePolicy<T> : IPromotePolicy<T>
        {
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

        public class RandomPromotePolicy<T> : IPromotePolicy<T>
        {
            public Tuple<T, T> Promote(ICollection<T> dataSet, DistanceMetric<T> distanceMetric)
            {
                IEnumerator<T> enumerator = dataSet.GetEnumerator();
                enumerator.MoveNext();
                T first = enumerator.Current;
                enumerator.MoveNext();
                T second = enumerator.Current;
                return new Tuple<T, T>(first, second);
            }
        }
    }
}
