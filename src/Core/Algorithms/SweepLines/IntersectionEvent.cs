// <copyright file="IntersectionEvent.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Algorithms.SweepLines
{
    using System;

    /// <summary>
    /// Represents an intersection event.
    /// </summary>
    public class IntersectionEvent : Event
    {
        /// <summary>
        /// Gets or sets the below <see cref="SweepLineSegment" /> instance intersecting at this event.
        /// </summary>
        /// <value>The below <see cref="SweepLineSegment" /> instance intersecting at this event.</value>
        public SweepLineSegment Below { get; set; }

        /// <summary>
        /// Gets or sets the above <see cref="SweepLineSegment" /> instance intersecting at this event.
        /// </summary>
        /// <value>The above <see cref="SweepLineSegment" /> instance intersecting at this event.</value>
        public SweepLineSegment Above { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether the event is a closing point for the intersection.
        /// </summary>
        /// <value><c>true</c> if the event is a closing point for the intersection; otherwise, <c>false</c>.</value>
        public Boolean IsClosing { get; set; }
    }
}
