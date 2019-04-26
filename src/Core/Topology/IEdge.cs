// <copyright file="IEdge.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;

    /// <summary>
    /// Defines an edge of the topology graph.
    /// </summary>
    /// <author>Máté Cserép</author>
    public interface IEdge
    {
        /// <summary>
        /// Gets the first halfedge that corresponds to the edge.
        /// </summary>
        IHalfedge HalfedgeA { get; }

        /// <summary>
        /// Gets the second halfedge that corresponds to the edge.
        /// </summary>
        IHalfedge HalfedgeB { get; }

        /// <summary>
        /// Gets the first vertex on the edge.
        /// </summary>
        IVertex VertexA { get; }

        /// <summary>
        /// Gets the second vertex on the edge.
        /// </summary>
        IVertex VertexB { get; }

        /// <summary>
        /// Gets the first face adjacent to the edge.
        /// </summary>
        /// <value>The first face adjacent to the edge, or <c>null</c> if there is no face.</value>
        IFace FaceA { get; }

        /// <summary>
        /// Gets the second face adjacent to the edge.
        /// </summary>
        /// <value>The second face adjacent to the edge, or <c>null</c> if there is no face.</value>
        IFace FaceB { get; }

        /// <summary>
        /// Gets the graph the edge belongs to.
        /// </summary>
        IHalfedgeGraph Graph { get; }

        /// <summary>
        /// Gets the identifiers of the edge.
        /// </summary>
        ISet<Int32> Identifiers { get; }

        /// <summary>
        /// Gets a value indicating whether the edge is on the boundary of the graph.
        /// </summary>
        /// <value><c>true</c> if the edge is on the boundary of the graph; otherwise, <c>false</c>.</value>
        Boolean OnBoundary { get; }
    }
}