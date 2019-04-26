// <copyright file="IHalfedge.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines a halfedge of the topology graph.
    /// </summary>
    /// <author>Máté Cserép</author>
    public interface IHalfedge
    {
        /// <summary>
        /// Gets the opposite halfedge.
        /// </summary>
        IHalfedge Opposite { get; }

        /// <summary>
        /// Gets the next halfedge.
        /// </summary>
        IHalfedge Next { get; }

        /// <summary>
        /// Gets the previous halfedge.
        /// </summary>
        IHalfedge Previous { get; }

        /// <summary>
        /// Gets the vertex the halfedge points to.
        /// </summary>
        IVertex ToVertex { get;  }

        /// <summary>
        /// Gets the vertex the halfedge originates from.
        /// </summary>
        IVertex FromVertex { get; }

        /// <summary>
        /// Gets the edge corresponding to the halfedge.
        /// </summary>
        IEdge Edge { get; }

        /// <summary>
        /// Gets the face corresponding to the halfedge.
        /// </summary>
        IFace Face { get; }

        /// <summary>
        /// Gets the graph the halfedge belongs to.
        /// </summary>
        IHalfedgeGraph Graph { get; }

        /// <summary>
        /// Gets a value indicating whether the halfedge is on the boundary of the graph.
        /// </summary>
        /// <value><c>true</c> if the halfedge is on the boundary of the graph; otherwise, <c>false</c>.</value>
        Boolean OnBoundary { get; }
    }
}