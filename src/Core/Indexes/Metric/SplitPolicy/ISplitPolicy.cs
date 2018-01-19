// <copyright file="ISplitPolicy.cs" company="Eötvös Loránd University (ELTE)">
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

    public interface ISplitPolicy<DATA> : IPromotePolicy<DATA>, IPartitionPolicy<DATA>
    {
    }

    public interface IPromotePolicy<DATA>
    {
        Tuple<DATA, DATA> Promote(ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric);
    }

    public interface IPartitionPolicy<DATA>
    {
        Tuple<ISet<DATA>, ISet<DATA>> Partition(Tuple<DATA, DATA> promoted, ICollection<DATA> dataSet, DistanceMetric<DATA> distanceMetric);
    }

    public delegate Double DistanceMetric<in DATA>(DATA a, DATA b);
}
