// <copyright file="CoordinateComparer.cs" company="Eötvös Loránd University (ELTE)">
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a comparer for <see cref="Coordinate" /> instances.
    /// </summary>
    public class CoordinateComparer : IComparer<Coordinate>
    {
        /// <summary>
        /// Compares two <see cref="Coordinate" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="Coordinate" /> to compare.</param>
        /// <param name="y">The second <see cref="Coordinate" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        public Int32 Compare(Coordinate x, Coordinate y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            if (!x.IsValid || !y.IsValid)
                return 0;
            if (x.X < y.X)
                return -1;
            if (x.X > y.X)
                return 1;
            if (x.Y < y.Y)
                return -1;
            return x.Y > y.Y ? 1 : 0;
        }
    }
}
