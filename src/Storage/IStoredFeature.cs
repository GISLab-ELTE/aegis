// <copyright file="IStoredFeature.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage
{
    /// <summary>
    /// Defines properties of geographic features located in a store.
    /// </summary>
    public interface IStoredFeature : IFeature
    {
        /// <summary>
        /// Gets the driver of the feature.
        /// </summary>
        /// <value>The driver the feature.</value>
        IFeatureDriver Driver { get; }

        /// <summary>
        /// Gets the factory of the feature.
        /// </summary>
        /// <value>The factory implementation the feature was constructed by.</value>
        new IStoredFeatureFactory Factory { get; }
    }
}
