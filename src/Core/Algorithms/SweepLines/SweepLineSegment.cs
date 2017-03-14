// <copyright file="SweepLineSegment.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a Sweep line segment.
    /// </summary>
    public sealed class SweepLineSegment : IEquatable<SweepLineSegment>
    {
        /// <summary>
        /// Gets or sets the edge associated with the segment.
        /// </summary>
        public Int32 Edge { get; set; }

        /// <summary>
        /// Gets or sets the left coordinate of the segment.
        /// </summary>
        public Coordinate LeftCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the right coordinate of the segment.
        /// </summary>
        public Coordinate RightCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the segment above this segment.
        /// </summary>
        public SweepLineSegment Above { get; set; }

        /// <summary>
        /// Gets or sets the segment below this segment.
        /// </summary>
        public SweepLineSegment Below { get; set; }

        /// <summary>
        /// Indicates whether this instance and a specified sweep line segment are equal.
        /// </summary>
        /// <param name="other">The sweep line segment to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(SweepLineSegment other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.Edge.Equals(other.Edge) && this.LeftCoordinate.Equals(other.LeftCoordinate) && this.RightCoordinate.Equals(other.RightCoordinate);
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(object obj)
        {
            return this.Equals(obj as SweepLineSegment);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return this.Edge.GetHashCode() >> 4 ^ this.LeftCoordinate.GetHashCode() >> 2 ^ this.RightCoordinate.GetHashCode();
        }
    }
}
