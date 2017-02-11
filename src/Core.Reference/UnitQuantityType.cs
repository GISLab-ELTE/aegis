// <copyright file="UnitQuantityType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Defines the measurement quantity types.
    /// </summary>
    public enum UnitQuantityType
    {
        /// <summary>
        /// Indicates that the unit is a length measure.
        /// </summary>
        Length,

        /// <summary>
        /// Indicates that the unit is an angular measure.
        /// </summary>
        Angle,

        /// <summary>
        /// Indicates that the unit is a time measure.
        /// </summary>
        Time,

        /// <summary>
        /// Indicates that the unit is a scale measure.
        /// </summary>
        Scale
    }
}
