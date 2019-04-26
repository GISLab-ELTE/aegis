// <copyright file="HalfedgeGraph.Face.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using AEGIS.Geometries;

    /// <summary>
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    public partial class HalfedgeGraph
    {
        /// <summary>
        /// Represents a face of the topology graph.
        /// </summary>
        /// <author>Máté Cserép</author>
        private class Face : IFace
        {
            #region Public properties

            /// <summary>
            /// Gets or sets the bounding halfedge that belongs to the graph.
            /// </summary>
            public Halfedge Halfedge { get; set; }

            /// <summary>
            /// Gets the graph the face belongs to.
            /// </summary>
            public HalfedgeGraph Graph
            {
                get { return Halfedge.Graph; }
            }

            /// <summary>
            /// Gets or sets the collection of holes in the face.
            /// </summary>
            public IList<Face> Holes { get; set; }

            /// <summary>
            /// Gets the collection of the bounding halfedges.
            /// </summary>
            public IEnumerable<Halfedge> Halfedges
            {
                get
                {
                    Halfedge halfedge = Halfedge;

                    do
                    {
                        yield return halfedge;
                        halfedge = halfedge.Next;
                    } while (halfedge != Halfedge);
                }
            }

            /// <summary>
            /// Gets the collection of the bounding vertices.
            /// </summary>
            public IEnumerable<Vertex> Vertices
            {
                get { return Halfedges.Select(h => h.ToVertex); }
            }

            /// <summary>
            /// Gets the collection of the bounding edges.
            /// </summary>
            public IEnumerable<Edge> Edges
            {
                get { return Halfedges.Select(h => h.Edge); }
            }

            /// <summary>
            /// Gets the collection of the adjacent faces.
            /// </summary>
            public IEnumerable<Face> Faces
            {
                get { return Halfedges.Select(h => h.Opposite.Face); }
            }

            /// <summary>
            /// Gets or sets the type of the face (shell, hole or both).
            /// </summary>
            /// <remarks>
            /// A face can be a standalone face and a hole of another face at the same time.
            /// </remarks>
            public FaceType Type { get; set; }

            /// <summary>
            /// Gets or sets the index of this edge in the internal face list of the graph.
            /// </summary>
            public Int32 Index { get; set; }

            #endregion

            #region IFace properties

            /// <summary>
            /// Gets the identifiers of the face.
            /// </summary>
            /// <remarks>
            /// The identifiers of the face is the intersection of identifiers of the face's vertices.
            /// </remarks>
            public ISet<Int32> Identifiers
            {
                get
                {
                    return Vertices.Skip(1).Aggregate(Vertices.First().Identifiers, (current, vertex) =>
                    {
                        ISet<Int32> result = new HashSet<Int32>(current);
                        result.IntersectWith(vertex.Identifiers);
                        return result;
                    });
                }
            }

            /// <summary>
            /// Checks whether the face is on the boundary of the graph.
            /// </summary>
            public Boolean OnBoundary
            {
                get { return Halfedges.Any(h => h.Opposite.OnBoundary); }
            }

            /// <summary>
            /// Determines whether two faces are adjacent.
            /// </summary>
            /// <param name="face">The other face to test.</param>
            /// <returns><c>true</c> if <paramref name="face" /> and this are adjacent; otherwise, <c>false</c>.</returns>
            public Boolean IsAdjacent(IFace face)
            {
                return Faces.Contains(face);
            }

            #endregion

            #region IFace properties (explicit)

            /// <summary>
            /// Gets the bounding halfedge that belongs to the graph.
            /// </summary>
            IHalfedge IFace.Halfedge
            {
                get { return Halfedge; }
            }

            /// <summary>
            /// Gets the graph the face belongs to.
            /// </summary>
            IHalfedgeGraph IFace.Graph
            {
                get { return Graph; }
            }

            /// <summary>
            /// Gets the collection of the bounding halfedges.
            /// </summary>
            IEnumerable<IHalfedge> IFace.Halfedges
            {
                get { return Halfedges; }
            }

            /// <summary>
            /// Gets the collection of the bounding vertices.
            /// </summary>
            IEnumerable<IVertex> IFace.Vertices
            {
                get { return Vertices; }
            }

            /// <summary>
            /// Gets the collection of the bounding edges.
            /// </summary>
            IEnumerable<IEdge> IFace.Edges
            {
                get { return Edges; }
            }

            /// <summary>
            /// Gets the collection of the adjacent faces.
            /// </summary>
            IEnumerable<IFace> IFace.Faces
            {
                get { return Faces; }
            }

            /// <summary>
            /// Gets the collection of holes in the face.
            /// </summary>
            IEnumerable<IFace> IFace.Holes
            {
                get { return Holes; }
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Face"/> class.
            /// </summary>
            public Face()
            {
                Holes = new List<Face>();
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Searches for the halfedge pointing to the specified vertex, bounding face.
            /// </summary>
            /// <param name="vertex">The vertex the halfedge to find points to.</param>
            /// <returns>The halfedge if it is found; otherwise <c>null</c>.</returns>
            public Halfedge FindHalfedgeTo(IVertex vertex)
            {
                return Halfedges.FirstOrDefault(h => h.ToVertex == vertex);
            }

            /// <summary>
            /// Searches for the edge associated with the specified face.
            /// </summary>
            /// <param name="face">A face sharing an edge with this face.</param>
            /// <returns>The edge if it is found; otherwise <c>null</c>.</returns>
            public Edge FindEdgeTo(IFace face)
            {
                return Halfedges.Where(h => h.Opposite.Face == face).Select(h => h.Edge).FirstOrDefault();
            }

            /// <summary>
            /// Searches for an indexed halfedge by iterating.
            /// </summary>
            /// <param name="index">The index of the halfedge to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified halfedge.</returns>
            public Halfedge GetHalfedge(Int32 index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", index, "The given index cannot be negative.");

                try
                {
                    return Halfedges.Skip(index).First();
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentOutOfRangeException("index", index, "The given index is too large.");
                }
            }

            /// <summary>
            /// Searches for an indexed vertex by iterating.
            /// </summary>
            /// <param name="index">The index of the vertex to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified vertex.</returns>
            public Vertex GetVertex(Int32 index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", index, "The given index cannot be negative.");

                try
                {
                    return Vertices.Skip(index).First();
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentOutOfRangeException("index", index, "The given index is too large.");
                }
            }

            /// <summary>
            /// Searches for an indexed edge by iterating.
            /// </summary>
            /// <param name="index">The index of the edge to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified edge.</returns>
            public Edge GetEdge(Int32 index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", index, "The given index cannot be negative.");

                try
                {
                    return Edges.Skip(index).First();
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentOutOfRangeException("index", index, "The given index is too large.");
                }
            }

            /// <summary>
            /// Searches for an indexed face by iterating.
            /// </summary>
            /// <param name="index">The index of the face to return.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is negative or too large.</exception>
            /// <returns>The specified face.</returns>
            public Face GetFace(Int32 index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", index, "The given index cannot be negative.");

                try
                {
                    return Faces.Skip(index).First();
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentOutOfRangeException("index", index, "The given index is too large.");
                }
            }

            #endregion

            #region IFace methods

            /// <summary>
            /// Converts the face to a SFA polygon.
            /// </summary>
            /// <param name="factory">The geometry factory used to produce the polygon.</param>
            /// <returns>The polygon geometry representing the face.</returns>
            public IPolygon ToGeometry(IGeometryFactory factory = null)
            {
                if (factory == null)
                    factory = new GeometryFactory();

                return factory.CreatePolygon(Vertices.Select(vertex => vertex.Position),
                                             Holes.Select(hole => hole.Vertices.Select(vertex => vertex.Position).Reverse()));
            }

            #endregion

            #region IFace methods (explicit)

            /// <summary>
            /// Searches for the halfedge pointing to the specified vertex, bounding face.
            /// </summary>
            /// <param name="vertex">The vertex the halfedge to find points to.</param>
            /// <returns>The halfedge if it is found; otherwise <c>null</c>.</returns>
            IHalfedge IFace.FindHalfedgeTo(IVertex vertex)
            {
                return FindHalfedgeTo(vertex);
            }

            /// <summary>
            /// Searches for the edge associated with the specified face.
            /// </summary>
            /// <param name="face">A face sharing an edge with this face.</param>
            /// <returns>The edge if it is found; otherwise <c>null</c>.</returns>
            IEdge IFace.FindEdgeTo(IFace face)
            {
                return FindEdgeTo(face);
            }

            #endregion
        }
    }
}