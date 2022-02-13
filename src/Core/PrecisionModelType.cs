// <copyright file="PrecisionModelType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    /// <summary>
    /// Defines the types of precision models.
    /// </summary>
    public enum PrecisionModelType
    {
        /// <summary>
        /// Double precision floating point representation.
        /// </summary>
        Floating,

        /// <summary>
        /// Single precision floating point representation.
        /// </summary>
        FloatingSingle,

        /// <summary>
        /// Fixed representation using fixed number of decimal places.
        /// </summary>
        Fixed,
    }
}
