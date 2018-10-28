// <copyright file="RelativeLocation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines relative spatial locations with respect to another spatial object.
    /// </summary>
    public enum RelativeLocation
    {
        /// <summary>
        /// Indicates that the object is inside.
        /// </summary>
        Interior,

        /// <summary>
        /// Indicates that the object is on the border.
        /// </summary>
        Boundary,

        /// <summary>
        /// Indicates that the object is outside.
        /// </summary>
        Exterior,

        /// <summary>
        /// Indicates that the location cannot be determined.
        /// </summary>
        Undefined
    }
}
