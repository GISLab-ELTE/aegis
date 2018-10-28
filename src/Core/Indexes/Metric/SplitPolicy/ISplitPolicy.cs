// <copyright file="ISplitPolicy.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes.Metric.SplitPolicy
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a policy to be used when splitting a <c>Node</c> of an <see cref="MTree{T}"/>.
    /// </summary>
    /// <remarks>
    /// It consists of two parts a promotion policy which describes how to choose center points for the new clusters (promoted items),
    /// and a split policy which allocates all points in the old <c>Node</c> to one of the newly created clusters based
    /// their designated promoted items. For premade split policies, see <see cref="SplitPolicies"/>. To create a custom
    /// split policy using a <see cref="IPromotePolicy{DATA}"/> and a <see cref="IPartitionPolicy{DATA}"/> use the class
    /// <see cref="SplitPolicy{DATA}"/>.
    /// </remarks>
    /// <typeparam name="DATA">The type of the data.</typeparam>
    /// <seealso cref="SplitPolicies" />
    /// <seealso cref="SplitPolicy{DATA}" />
    /// <seealso cref="AEGIS.Indexes.Metric.SplitPolicy.IPromotePolicy{DATA}" />
    /// <seealso cref="AEGIS.Indexes.Metric.SplitPolicy.IPartitionPolicy{DATA}" />
    public interface ISplitPolicy<DATA> : IPromotePolicy<DATA>, IPartitionPolicy<DATA>
    {
    }

    /// <summary>
    /// Describes how to choose center points for the new clusters (promoted items).
    /// </summary>
    /// <remarks>
    /// It is not required for the two promoted items to be chosen from <c>dataSet</c> for the <see cref="MTree{T}"/> to
    /// work correctly. The promoted items can also be "synthetic", but this is discouraged due to the fact that instantiation of
    /// such data points should not be a concern for the tree.
    /// </remarks>
    /// <typeparam name="DATA">The type of the data.</typeparam>
    public interface IPromotePolicy<DATA>
    {
        /// <summary>
        /// Chooses or creates two promoted data items for the <c>dataSet</c>.
        /// </summary>
        /// <remarks>
        /// Promoted data items behave much like center points for clusters in clustering algorithms.
        /// Choosing two promoted items is recommended over creating new ones, due to the fact that
        /// the instantiation of data points should remain unknown to the tree.
        /// </remarks>
        /// <param name="dataSet">The data set.</param>
        /// <param name="distanceMetric">The distance metric.</param>
        /// <returns>a pair of promoted items</returns>
        Tuple<DATA, DATA> Promote(ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric);
    }

    /// <summary>
    /// Allocates all points of an old <c>Node</c> to one of the newly created clusters based
    /// their designated promoted items (previously calculated by an <see cref="IPromotePolicy{DATA}"/>.
    /// </summary>
    /// <typeparam name="DATA">The type of the data.</typeparam>
    public interface IPartitionPolicy<DATA>
    {
        /// <summary>
        /// Allocates all points of an old <c>Node</c> to one of the newly created clusters based
        /// their designated promoted items (previously calculated by an <see cref="IPromotePolicy{DATA}"/>.
        /// </summary>
        /// <param name="promoted">The promoted items previously calculated.</param>
        /// <param name="dataSet">The data set to be paritioned.</param>
        /// <param name="distanceMetric">The distance metric to be used.</param>
        /// <returns>the original data points partitioned into two new clusters of data points</returns>
        Tuple<ISet<DATA>, ISet<DATA>> Partition(Tuple<DATA, DATA> promoted, ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric);
    }

    /// <summary>
    /// Represents a distance metric.
    /// </summary>
    /// <typeparam name="DATA">The supported data type of the distance metric.</typeparam>
    /// <param name="a">an instance of <c>DATA</c></param>
    /// <param name="b">an other instance of <c>DATA</c></param>
    /// <returns>the distance between <c>a</c> and <c>b</c></returns>
    public delegate Double DistanceMetric<in DATA>(DATA a, DATA b);
}
