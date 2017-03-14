// <copyright file="EventComparer.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Algorithms.SweepLines
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a comparer for <see cref="Event" /> instances.
    /// </summary>
    public sealed class EventComparer : IComparer<Event>
    {
        /// <summary>
        /// Stores an inner <see cref="CoordinateComparer" /> instance.
        /// </summary>
        private readonly CoordinateComparer coordinateComparer = new CoordinateComparer();

        /// <summary>
        /// Compares two <see cref="Event" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <remarks>
        /// Events primarily compared by their vertex coordinate, secondarily by their type.
        /// </remarks>
        /// <param name="x">The first <see cref="Event" /> to compare.</param>
        /// <param name="y">The second <see cref="Event" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        public Int32 Compare(Event x, Event y)
        {
            if (x == null || y == null)
                return 0;

            Int32 result = this.coordinateComparer.Compare(x.Vertex, y.Vertex);
            if (result != 0)
                return result;

            EndPointEvent ex = x as EndPointEvent;
            EndPointEvent ey = y as EndPointEvent;
            IntersectionEvent ix = x as IntersectionEvent;
            IntersectionEvent iy = y as IntersectionEvent;

            if (ex != null && ey != null)
            {
                result = ex.Type.CompareTo(ey.Type);
                if (result == 0)
                    result = ex.Edge.CompareTo(ey.Edge);
            }
            else if (ix != null && iy != null)
            {
                if (result == 0)
                    result = this.coordinateComparer.Compare(ix.Below.LeftCoordinate, iy.Below.LeftCoordinate);
                if (result == 0)
                    result = this.coordinateComparer.Compare(ix.Above.LeftCoordinate, iy.Above.LeftCoordinate);
                if (result == 0)
                    result = this.coordinateComparer.Compare(ix.Below.RightCoordinate, iy.Below.RightCoordinate);
                if (result == 0)
                    result = this.coordinateComparer.Compare(ix.Above.RightCoordinate, iy.Above.RightCoordinate);
                if (result == 0)
                    result = ix.Below.Edge.CompareTo(iy.Below.Edge);
                if (result == 0)
                    result = ix.Above.Edge.CompareTo(iy.Above.Edge);
                if (result == 0)
                    result = ix.IsClosing.CompareTo(iy.IsClosing);
            }
            else if (ex != null && iy != null)
            {
                result = ex.Type == EventType.Left ? -1 : 1;
            }
            else if (ix != null && ey != null)
            {
                result = ey.Type == EventType.Left ? 1 : -1;
            }

            return result;
        }
    }
}