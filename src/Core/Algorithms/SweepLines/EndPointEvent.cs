// <copyright file="EndPointEvent.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms.SweepLines
{
    using System;

    /// <summary>
    /// Represents an endpoint event.
    /// </summary>
    public class EndPointEvent : Event
    {
        #region Public fields

        /// <summary>
        /// Gets or sets the polygon edge associated with the event.
        /// </summary>
        /// <value>The polygon edge associated with the event.</value>
        public Int32 Edge { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        /// <value>The event type.</value>
        public EventType Type { get; set; }

        #endregion
    }
}
