// <copyright file="DriverOperation.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Storage
{
    /// <summary>
    /// Defines possible driver operations.
    /// </summary>
    public enum DriverOperation
    {
        /// <summary>
        /// Indicates the creation of a new item.
        /// </summary>
        Create,

        /// <summary>
        /// Indicates reading of an item.
        /// </summary>
        Read,

        /// <summary>
        /// Indicates update of an item with the same storage space.
        /// </summary>
        Update,

        /// <summary>
        /// Indicates update of an item with the extension of storage space.
        /// </summary>
        UpdateExtend,

        /// <summary>
        /// Indicates update of an item with the reduction of storage space.
        /// </summary>
        UpdateReduce,

        /// <summary>
        /// Indicates removal of an item.
        /// </summary>
        Delete
    }
}
