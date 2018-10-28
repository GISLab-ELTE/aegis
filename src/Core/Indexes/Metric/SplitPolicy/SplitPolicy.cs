// <copyright file="SplitPolicy.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Creates an <see cref="ISplitPolicy{DATA}"/> using a custom <see cref="IPromotePolicy{DATA}"/>
    /// and <see cref="IPartitionPolicy{DATA}"/>.
    /// </summary>
    /// <typeparam name="DATA">The type of the data.</typeparam>
    /// <seealso cref="AEGIS.Indexes.Metric.SplitPolicy.ISplitPolicy{DATA}" />
    public class SplitPolicy<DATA> : ISplitPolicy<DATA>
    {
        private readonly IPromotePolicy<DATA> promotePolicy;
        private readonly IPartitionPolicy<DATA> partitionPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitPolicy{DATA}"/> class.
        /// </summary>
        /// <param name="promotePolicy">The promote policy.</param>
        /// <param name="partitionPolicy">The partition policy.</param>
        /// <exception cref="ArgumentNullException">
        /// promotePolicy
        /// or
        /// partitionPolicy
        /// </exception>
        public SplitPolicy(IPromotePolicy<DATA> promotePolicy, IPartitionPolicy<DATA> partitionPolicy)
        {
            this.promotePolicy = promotePolicy ?? throw new ArgumentNullException(nameof(promotePolicy));
            this.partitionPolicy = partitionPolicy ?? throw new ArgumentNullException(nameof(partitionPolicy));
        }

        /// <summary>
        /// Allocates all points of an old <c>Node</c> to one of the newly created clusters based
        /// their designated promoted items (previously calculated by an <see cref="IPromotePolicy{DATA}"/>.
        /// </summary>
        /// <param name="promoted">The promoted items previously calculated.</param>
        /// <param name="dataSet">The data set to be paritioned.</param>
        /// <param name="distanceMetric">The distance metric to be used.</param>
        /// <returns>the original data points partitioned into two new clusters of data points</returns>
        public Tuple<ISet<DATA>, ISet<DATA>> Partition(Tuple<DATA, DATA> promoted, ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric)
        {
            return this.partitionPolicy.Partition(promoted, dataSet, distanceMetric);
        }

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
        public Tuple<DATA, DATA> Promote(ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric)
        {
            return this.promotePolicy.Promote(dataSet, distanceMetric);
        }
    }
}
