// <copyright file="RemoveMode.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Topology
{
    /// <summary>
    /// Defines the removal mode for objects contained by a topology graph.
    /// </summary>
    /// <author>Máté Cserép</author>
    public enum RemoveMode
    {
        /// <summary>
        /// Removes the specified object from the topology graph if it is not part of another objects.
        /// When the specified object is used by other objects, the removal is rejected.
        /// </summary>
        Normal,

        /// <summary>
        /// Forces to remove all objects from the topology graph which uses the specified object.
        /// The removal is never rejected.
        /// </summary>
        Forced,

        /// <summary>
        /// In addition to <see cref="Forced"/>, possibly produced isolated vertices are also removed.
        /// </summary>
        Clean
    }
}
