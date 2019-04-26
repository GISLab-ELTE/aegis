// <copyright file="IVertex.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines a vertex of the topology graph.
    /// </summary>
    /// <author>Máté Cserép</author>
    public interface IVertex
    {
        /// <summary>
        /// Gets the halfedge that originates from the vertex.
        /// </summary>
        IHalfedge Halfedge { get; }

        /// <summary>
        /// Gets the graph the vertex belongs to.
        /// </summary>
        IHalfedgeGraph Graph { get; }

        /// <summary>
        /// Gets the identifiers of the vertex.
        /// </summary>
        ISet<Int32> Identifiers { get; }

        /// <summary>
        /// Gets the position of the vertex.
        /// </summary>
        Coordinate Position { get; }

        /// <summary>
        /// Gets a value indicating whether the vertex is on the boundary of the graph.
        /// </summary>
        /// <value><c>true</c> if the vertex is on the boundary of the graph; otherwise, <c>false</c>.</value>
        Boolean OnBoundary { get; }

        /// <summary>
        /// Gets the collection of the halfedges originating from the vertex.
        /// </summary>
        IEnumerable<IHalfedge> Halfedges { get; }

        /// <summary>
        /// Gets the collection of the vertices in the one ring neighborhood.
        /// </summary>
        IEnumerable<IVertex> Vertices { get; }

        /// <summary>
        /// Gets the collection of edges connected to the vertex.
        /// </summary>
        IEnumerable<IEdge> Edges { get; }

        /// <summary>
        /// Gets the collection of faces bounding with the vertex.
        /// </summary>
        IEnumerable<IFace> Faces { get; }

        /// <summary>
        /// Searches for a halfedge pointing to a vertex from this vertex.
        /// </summary>
        /// <param name="vertex">A vertex pointed to by the halfedge to search for.</param>
        /// <returns>The halfedge from this vertex to the specified vertex. If none exists, returns <c>null</c>.</returns>
        IHalfedge FindHalfedgeTo(IVertex vertex);

        /// <summary>
        /// Searches for the halfedge pointing to the specified face from this vertex.
        /// </summary>
        /// <param name="face">The face the halfedge to find points to.</param>
        /// <returns>The halfedge if it is found, otherwise <c>null</c>.</returns>
        IHalfedge FindHalfedgeTo(IFace face);

        /// <summary>
        /// Searches for the edge associated with the specified vertex.
        /// </summary>
        /// <param name="vertex">A vertex sharing an edge with this vertex.</param>
        /// <returns>The edge if it is found, otherwise <c>null</c>.</returns>
        IEdge FindEdgeTo(IVertex vertex);
    }
}