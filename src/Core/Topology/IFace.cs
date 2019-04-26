// <copyright file="IFace.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines a face of the topology graph.
    /// </summary>
    /// <author>Máté Cserép</author>
    public interface IFace
    {
        /// <summary>
        /// Gets the bounding halfedge that belongs to the graph.
        /// </summary>
        IHalfedge Halfedge { get; }

        /// <summary>
        /// Gets the graph the face belongs to.
        /// </summary>
        IHalfedgeGraph Graph { get; }

        /// <summary>
        /// Gets the identifiers of the face.
        /// </summary>
        ISet<Int32> Identifiers { get; }

        /// <summary>
        /// Gets a value indicating whether the edge is on the boundary of the graph.
        /// </summary>
        /// <value><c>true</c> if the edge is on the boundary of the graph; otherwise, <c>false</c>.</value>
        Boolean OnBoundary { get; }

        /// <summary>
        /// Gets the collection of the bounding halfedges.
        /// </summary>
        IEnumerable<IHalfedge> Halfedges { get; }

        /// <summary>
        /// Gets the collection of the bounding vertices.
        /// </summary>
        IEnumerable<IVertex> Vertices { get; }

        /// <summary>
        /// Gets the collection of the bounding edges.
        /// </summary>
        IEnumerable<IEdge> Edges { get; }

        /// <summary>
        /// Gets the collection of the adjacent faces.
        /// </summary>
        IEnumerable<IFace> Faces { get; }

        /// <summary>
        /// Gets the collection of holes in the face.
        /// </summary>
        IEnumerable<IFace> Holes { get; }

        /// <summary>
        /// Searches for the halfedge pointing to the specified vertex, bounding face.
        /// </summary>
        /// <param name="vertex">The vertex the halfedge to find points to.</param>
        /// <returns>The halfedge if it is found; otherwise <c>null</c>.</returns>
        IHalfedge FindHalfedgeTo(IVertex vertex);

        /// <summary>
        /// Searches for the edge associated with the specified face.
        /// </summary>
        /// <param name="face">A face sharing an edge with this face.</param>
        /// <returns>The edge if it is found; otherwise <c>null</c>.</returns>
        IEdge FindEdgeTo(IFace face);

        /// <summary>
        /// Determines whether two faces are adjacent.
        /// </summary>
        /// <param name="face">The other face to test.</param>
        /// <returns><c>true</c> if <paramref name="face"/> and this are adjacent; otherwise, <c>false</c>.</returns>
        Boolean IsAdjacent(IFace face);

        #region IGeometry compatibility methods

        /// <summary>
        /// Converts the face to a polygon.
        /// </summary>
        /// <param name="factory">The geometry factory used to produce the polygon.</param>
        /// <returns>The polygon geometry representing the face.</returns>
        IPolygon ToGeometry(IGeometryFactory factory);

        #endregion
    }
}