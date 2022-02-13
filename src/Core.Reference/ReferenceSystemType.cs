// <copyright file="ReferenceSystemType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    /// <summary>
    /// Defines the types of coordinate reference systems.
    /// </summary>
    public enum ReferenceSystemType
    {
        /// <summary>
        /// Unknown reference system.
        /// </summary>
        Unknown,

        /// <summary>
        /// Compound reference system.
        /// </summary>
        Compound,

        /// <summary>
        /// Geocentric reference system.
        /// </summary>
        Geocentric,

        /// <summary>
        /// Geographic 2D reference system.
        /// </summary>
        Geographic2D,

        /// <summary>
        /// Geographic 3D reference system.
        /// </summary>
        Geographic3D,

        /// <summary>
        /// Grid reference system.
        /// </summary>
        Grid,

        /// <summary>
        /// Projected reference system.
        /// </summary>
        Projected,

        /// <summary>
        /// Temporal reference system.
        /// </summary>
        Temporal,

        /// <summary>
        /// User defined reference system.
        /// </summary>
        UserDefined,

        /// <summary>
        /// Vertical reference system.
        /// </summary>
        Vertical,
    }
}