// <copyright file="HalfedgeGraph.FaceType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Topology
{
    using System;

    /// <summary>
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    public partial class HalfedgeGraph
    {
        /// <summary>
        /// Defines the type of a face (shell, hole or both).
        /// </summary>
        /// <author>Máté Cserép</author>
        [Flags]
        public enum FaceType
        {
            /// <summary>
            /// The face is a shell and not a hole.
            /// </summary>
            Shell = 1,

            /// <summary>
            /// The face is a hole and not a shell.
            /// </summary>
            Hole = 2,

            /// <summary>
            /// The face is both a shell and a hole.
            /// </summary>
            Both = 3
        }
    }
}
