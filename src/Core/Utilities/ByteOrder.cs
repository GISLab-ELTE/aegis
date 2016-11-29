﻿// <copyright file="ByteOrder.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Utilities
{
    /// <summary>
    /// Defines byte-orders.
    /// </summary>
    public enum ByteOrder
    {
        /// <summary>
        /// Indicates little-endian byte-order.
        /// </summary>
        LittleEndian,

        /// <summary>
        /// Indicates big-endian byte-order.
        /// </summary>
        BigEndian
    }
}