// <copyright file="IHalfedgeGraph.cs" company="Eötvös Loránd University (ELTE)">
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
    /// A halfedge graph data structure that stores topology.
    /// </summary>
    /// <author>Máté Cserép</author>
    public interface IHalfedgeGraph
    {
        #region Properties

        /// <summary>
        /// Gets the collection of halfedges in the graph.
        /// </summary>
        IEnumerable<IHalfedge> Halfedges { get; }

        /// <summary>
        /// Gets the collection of vertices in the graph.
        /// </summary>
        IEnumerable<IVertex> Vertices { get; }

        /// <summary>
        /// Gets the collection of edges in the graph.
        /// </summary>
        IEnumerable<IEdge> Edges { get; }

        /// <summary>
        /// Gets the collection of faces in the graph.
        /// </summary>
        IEnumerable<IFace> Faces { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Removes all elements from the graph.
        /// </summary>
        void Clear();

        /// <summary>
        /// Trims internal data structures to their current size.
        /// </summary>
        /// <remarks>
        /// Call this method to prevent excess memory usage when the graph is done being built.
        /// </remarks>
        void TrimExcess();

        /// <summary>
        /// Checks halfedge connections to verify that a valid topology graph was constructed.
        /// </summary>
        /// <remarks>
        /// Checking for proper topology in every case when a face is added would slow down
        /// graph construction significantly, so this method may be called once when a graph
        /// is complete to ensure that topology is manifold (with boundary).
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the graph is non-manifold.</exception>
        void VerifyTopology();

        /// <summary>
        /// Adds an (isolated) vertex to the graph.
        /// </summary>
        /// <remarks>
        /// When a vertex already exists at the given position, it will be returned instead of creating a new one.
        /// </remarks>
        /// <param name="position">The position of the vertex.</param>
        /// <param name="identifiers">The identifiers of the vertex.</param>
        /// <returns>The vertex created by this method.</returns>
        IVertex AddVertex(Coordinate position, ISet<Int32> identifiers = null);

        /// <summary>
        /// Adds a face to the graph. The new face must appropriately fit to the existing topology graph without any overlap.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="identifiers">The identifiers of the polygon.</param>
        /// <returns>The face created by this method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// A hole is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell does not contain at least 3 different coordinates.
        /// or
        /// A hole does not contain at least 3 different coordinates.
        /// </exception>
        IFace AddFace(IBasicPolygon polygon, ISet<Int32> identifiers = null);

        /// <summary>
        /// Adds a face to the graph. The new face must appropriately fit to the existing topology graph without any overlap.
        /// </summary>
        /// <remarks>
        /// Please note, that for this method the vertices of the shell must be given in counter-clockwise order, while the vertices of the holes in clockwise order.
        /// </remarks>
        /// <param name="shell">The vertices of the shell in counter-clockwise order.</param>
        /// <param name="holes">The vertices of the holes in clockwise order.</param>
        /// <param name="identifiers">The identifiers of the face.</param>
        /// <returns>The face created by this method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// A hole is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell does not contain at least 3 different coordinates.
        /// or
        /// A hole does not contain at least 3 different coordinates.
        /// </exception>
        IFace AddFace(IReadOnlyList<Coordinate> shell, List<IReadOnlyList<Coordinate>> holes = null, ISet<Int32> identifiers = null);

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <remarks>
        /// The algorithm may be forced by the <see cref="mode"/> parameter to remove the adjacent faces of the vertex.
        /// </remarks>
        /// <param name="position">The position of the vertex to remove.</param>
        /// <param name="mode">The mode of the removal.</param>
        /// <returns><c>true</c> when the coordinate to remove exists in the graph; otherwise <c>false</c>.</returns>
        Boolean RemoveVertex(Coordinate position, RemoveMode mode = RemoveMode.Normal);

        /// <summary>
        /// Removes a face from the graph.
        /// </summary>
        /// <remarks>
        /// The forced removal mode makes no difference for this method in contrast to the normal mode.
        /// </remarks>
        /// <param name="face">The face to remove.</param>
        /// <param name="mode">The mode of the removal.</param>
        void RemoveFace(IFace face, RemoveMode mode = RemoveMode.Clean);

        /// <summary>
        /// Merges a face into the graph, resolving face overlapping.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="identifiers">The identifiers of the face.</param>
        /// <returns>The collection of faces created by the merge operation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// A hole is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell does not contain at least 3 different coordinates.
        /// or
        /// The first and the last coordinates of the shell are not equal.
        /// or
        /// A hole does not contain at least 3 different coordinates.
        /// or
        /// The first and the last coordinates of a hole are not equal.
        /// </exception>
        ICollection<IFace> MergeFace(IBasicPolygon polygon, ISet<Int32> identifiers = null);

        /// <summary>
        /// Merges a face into the graph, resolving face overlapping.
        /// </summary>
        /// <remarks>
        ///  Please note, that for this method the vertices of the shell must be given in counter-clockwise order, while the vertices of the holes in clockwise order.
        /// </remarks>
        /// <param name="shell">The vertices of the shell in counter-clockwise order.</param>
        /// <param name="holes">The vertices of the holes in clockwise order.</param>
        /// <param name="identifiers">The identifiers of the face.</param>
        /// <returns>The collection of faces created by the merge operation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// A hole is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The shell does not contain at least 3 different coordinates.
        /// or
        /// The first and the last coordinates of the shell are not equal.
        /// or
        /// A hole does not contain at least 3 different coordinates.
        /// or
        /// The first and the last coordinates of a hole are not equal.
        /// </exception>
        ICollection<IFace> MergeFace(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes = null, ISet<Int32> identifiers = null);

        /// <summary>
        /// Merges another graph into the current instance.
        /// </summary>
        /// <param name="other">The other graph.</param>
        /// <exception cref="System.ArgumentNullException">The other graph is null.</exception>
        void MergeGraph(IHalfedgeGraph other);

        #endregion

        #region IGeometry support methods

        /// <summary>
        /// Adds a (supported type of) geometry to the graph. The new geometry must appropriately fit to the existing topology graph without any overlap.
        /// </summary>
        /// <remarks>
        /// The supported types are <see cref="IPoint"/>, <see cref="ILinearRing"/>, <see cref="IPolygon"/>, <see cref="IMultiPoint"/>, <see cref="IMultiPolygon"/> and <see cref="IGeometryCollection{T}"/>.
        /// </remarks>
        /// <param name="geometry">The geometry to add.</param>
        void AddGeometry(IGeometry geometry);

        /// <summary>
        /// Adds an (isolated) point to the graph.
        /// </summary>
        /// <remarks>
        /// When a point already exists at the given position, it will be returned instead of creating a new one.
        /// </remarks>
        /// <param name="point">The point to add.</param>
        /// <returns>The vertex created by this method.</returns>
        IVertex AddPoint(IPoint point);

        /// <summary>
        /// Adds a linear ring to the graph.
        /// </summary>
        /// <param name="linearRing">The linear ring to add.</param>
        /// <returns>The face created by this method.</returns>
        IFace AddLinearRing(ILinearRing linearRing);

        /// <summary>
        /// Adds a polygon to the graph.
        /// </summary>
        /// <param name="polygon">The polygon to add.</param>
        /// <returns>The face created by this method.</returns>
        IFace AddPolygon(IPolygon polygon);

        /// <summary>
        /// Adds multiple (isolated) points to the graph.
        /// </summary>
        /// <param name="multiPoint">The points to add.</param>
        /// <returns>The vertices created by this method.</returns>
        IVertex[] AddMultiPoint(IMultiPoint multiPoint);

        /// <summary>
        /// Adds multiple polygons to the graph.
        /// </summary>
        /// <param name="multiPolygon">The polygons to add.</param>
        /// <returns>The faces created by this method.</returns>
        IFace[] AddMultiPolygon(IMultiPolygon multiPolygon);

        /// <summary>
        /// Merges a (supported type of) geometry into the graph, resolving geometry overlapping.
        /// </summary>
        /// <remarks>
        /// The supported types are <see cref="IPoint" />, <see cref="ILinearRing" />, <see cref="IPolygon" />, <see cref="IMultiPoint" />,  <see cref="IMultiPolygon"/> and <see cref="IGeometryCollection{T}"/>.
        /// </remarks>
        /// <param name="geometry">The geometry to merge.</param>
        void MergeGeometry(IGeometry geometry);

        /// <summary>
        /// Merges a linear ring into the graph.
        /// </summary>
        /// <param name="linearRing">The linear ring to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        ICollection<IFace> MergeLinearRing(ILinearRing linearRing);

        /// <summary>
        /// Merges a polygon into the graph.
        /// </summary>
        /// <param name="polygon">The polygon to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        ICollection<IFace> MergePolygon(IPolygon polygon);

        /// <summary>
        /// Merges multiple polygons into the graph.
        /// </summary>
        /// <param name="multiPolygon">The polygons to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        ICollection<IFace> MergeMultiPolygon(IMultiPolygon multiPolygon);

        /// <summary>
        /// Converts the graph to a collection of geometries.
        /// </summary>
        /// <param name="factory">The geometry factory used to produce the polygon.</param>
        /// <returns>A collection of geometries representing the topology graph.</returns>
        IGeometry ToGeometry(IGeometryFactory factory = null);

        #endregion
    }
}