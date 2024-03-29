﻿// <copyright file="Orientation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines the possible spatial orientations.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Indicates that the objects are collinear.
        /// </summary>
        Collinear,

        /// <summary>
        /// Indicates that the object is oriented clockwise.
        /// </summary>
        Clockwise,

        /// <summary>
        /// Indicates that the object is oriented counterclockwise.
        /// </summary>
        Counterclockwise,

        /// <summary>
        /// Indicates that the orientation is undefined.
        /// </summary>
        Undefined,
    }
}
