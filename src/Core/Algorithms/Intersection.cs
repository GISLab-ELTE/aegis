// <copyright file="Intersection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents an intersection.
    /// </summary>
    public class Intersection
    {
        /// <summary>
        /// Gets the type of the intersection.
        /// </summary>
        /// <value>The type.</value>
        public IntersectionType Type { get; private set; }

        /// <summary>
        /// Gets the intersection coordinate.
        /// </summary>
        /// <value>The intersection coordinate.</value>
        public Coordinate Coordinate { get { return this.Start; } }

        /// <summary>
        /// Gets the starting coordinate.
        /// </summary>
        /// <value>The starting coordinate.</value>
        public Coordinate Start { get; private set; }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The ending coordinate.</value>
        public Coordinate End { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Intersection" /> class.
        /// </summary>
        /// <param name="coordinate">The intersection coordinate.</param>
        public Intersection(Coordinate coordinate)
        {
            if (coordinate == null || !coordinate.IsValid)
                this.Type = IntersectionType.None;
            else
                this.Type = IntersectionType.Coordinate;

            this.Start = coordinate;
            this.End = coordinate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Intersection" /> class.
        /// </summary>
        /// <param name="start">The starting coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        public Intersection(Coordinate start, Coordinate end)
        {
            if (start == null || end == null || !start.IsValid || !end.IsValid)
            {
                this.Type = IntersectionType.None;
            }
            else if (start == end)
            {
                this.Type = IntersectionType.Coordinate;
            }
            else
            {
                this.Type = IntersectionType.Interval;
            }

            this.Start = start;
            this.End = end;
        }
    }
}
