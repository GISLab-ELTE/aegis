// <copyright file="AxisDirection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines directions for coordinate system axis.
    /// </summary>
    /// <remarks>
    /// The direction of positive increase in the coordinate value for a coordinate system axis.
    /// </remarks>
    public enum AxisDirection
    {
        /// <summary>
        /// Other direction.
        /// </summary>
        Other,

        /// <summary>
        /// North direction.
        /// </summary>
        North,

        /// <summary>
        /// North, north east direction.
        /// </summary>
        NorthNorthEast,

        /// <summary>
        /// North east direction.
        /// </summary>
        NorthEast,

        /// <summary>
        /// East, north east direction.
        /// </summary>
        EastNorthEast,

        /// <summary>
        /// East direction.
        /// </summary>
        East,

        /// <summary>
        /// East, south east direction.
        /// </summary>
        EastSouthEast,

        /// <summary>
        /// South east direction.
        /// </summary>
        SouthEast,

        /// <summary>
        /// South, south east direction.
        /// </summary>
        SouthSouthEast,

        /// <summary>
        /// South direction.
        /// </summary>
        South,

        /// <summary>
        /// South, south west direction.
        /// </summary>
        SouthSouthWest,

        /// <summary>
        /// South west direction.
        /// </summary>
        SouthWest,

        /// <summary>
        /// West, south west direction.
        /// </summary>
        WestSouthWest,

        /// <summary>
        /// West direction.
        /// </summary>
        West,

        /// <summary>
        /// West, north west direction.
        /// </summary>
        WestNorthWest,

        /// <summary>
        /// North west direction.
        /// </summary>
        NorthWest,

        /// <summary>
        /// North, north west direction.
        /// </summary>
        NorthNorthWest,

        /// <summary>
        /// Up direction.
        /// </summary>
        Up,

        /// <summary>
        /// Down direction.
        /// </summary>
        Down,

        /// <summary>
        /// Geocentric X direction.
        /// </summary>
        GeocentricX,

        /// <summary>
        /// Geocentric Y direction.
        /// </summary>
        GeocentricY,

        /// <summary>
        /// Geocentric Z direction.
        /// </summary>
        GeocentricZ,

        /// <summary>
        /// Future direction.
        /// </summary>
        Future,

        /// <summary>
        /// Past direction.
        /// </summary>
        Past,

        /// <summary>
        /// Column positive direction.
        /// </summary>
        ColumnPositive,

        /// <summary>
        /// Column negative direction.
        /// </summary>
        ColumnNegative,

        /// <summary>
        /// Row positive direction.
        /// </summary>
        RowPositive,

        /// <summary>
        /// Row negative direction.
        /// </summary>
        RowNegative,

        /// <summary>
        /// Display right direction.
        /// </summary>
        DisplayRight,

        /// <summary>
        /// Display left direction.
        /// </summary>
        DisplayLeft,

        /// <summary>
        /// Display up direction.
        /// </summary>
        DisplayUp,

        /// <summary>
        /// Display down direction.
        /// </summary>
        DisplayDown,

        /// <summary>
        /// Undefined direction.
        /// </summary>
        Undefined
    }
}
