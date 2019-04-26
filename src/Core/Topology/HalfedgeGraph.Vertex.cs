// <copyright file="HalfedgeGraph.Vertex.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    public partial class HalfedgeGraph
    {
        /// <summary>
        /// Represents a vertex of the topology graph.
        /// </summary>
        /// <author>Máté Cserép</author>
        private class Vertex : IVertex
        {
            #region Public properties

            /// <summary>
            /// A halfedge that originates from the vertex.
            /// </summary>
            public Halfedge Halfedge { get; set; }

            /// <summary>
            /// The graph the vertex belongs to.
            /// </summary>
            public HalfedgeGraph Graph { get; set; }

            /// <summary>
            /// The index of this face in the internal vertex list of the graph.
            /// </summary>
            public Int32 Index { get; set; }

            #endregion

            #region Public iterators

            /// <summary>
            /// An iterator for the halfedges originating from the vertex.
            /// </summary>
            public IEnumerable<Halfedge> Halfedges
            {
                get
                {
                    Halfedge halfedge = Halfedge;

                    if (halfedge == null)
                        yield break;

                    do
                    {
                        yield return halfedge;
                        halfedge = halfedge.Opposite.Next;
                    } while (halfedge != Halfedge);

                }
            }

            /// <summary>
            /// An iterator for the vertices in the one ring neighborhood.
            /// </summary>
            public IEnumerable<Vertex> Vertices
            {
                get { return Halfedges.Select(h => h.ToVertex); }
            }

            /// <summary>
            /// An iterator for edges connected to the vertex.
            /// </summary>
            public IEnumerable<Edge> Edges
            {
                get { return Halfedges.Select(h => h.Edge); }
            }

            /// <summary>
            /// An iterator for the faces bounding with the vertex.
            /// </summary>
            public IEnumerable<Face> Faces
            {
                get { return Halfedges.Where(h => h.Face != null).Select(h => h.Face); }
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Vertex"/> class.
            /// </summary>
            /// <param name="position">The position of the vertex.</param>
            public Vertex(Coordinate position)
            {
                Position = position;
                Identifiers = new HashSet<Int32>();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Vertex"/> class.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="identifiers">The identifiers of the vertex.</param>
            public Vertex(Coordinate position, ISet<Int32> identifiers)
            {
                Position = position;
                Identifiers = new HashSet<Int32>(identifiers);
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Searches for a halfedge pointing to a vertex from this vertex.
            /// </summary>
            /// <param name="vertex">A vertex pointed to by the halfedge to search for.</param>
            /// <returns>The halfedge from this vertex to the specified vertex. If none exists, returns null.</returns>
            public Halfedge FindHalfedgeTo(IVertex vertex)
            {
                return Halfedges.FirstOrDefault(h => h.ToVertex == vertex);
            }

            /// <summary>
            /// Searches for the halfedge pointing to the specified face from this vertex.
            /// </summary>
            /// <param name="face">The face the halfedge to find points to.</param>
            /// <returns>The halfedge if it is found, otherwise null.</returns>
            public Halfedge FindHalfedgeTo(IFace face)
            {
                return Halfedges.FirstOrDefault(h => h.Face == face);
            }

            /// <summary>
            /// Searches for the edge associated with the specified vertex.
            /// </summary>
            /// <param name="vertex">A vertex sharing an edge with this vertex.</param>
            /// <returns>The edge if it is found, otherwise null.</returns>
            public Edge FindEdgeTo(IVertex vertex)
            {
                return Halfedges.Where(h => h.ToVertex == vertex).Select(h => h.Edge).FirstOrDefault();
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

            #region IVertex properties

            /// <summary>
            /// Gets or sets the identifiers of the vertex.
            /// </summary>
            public ISet<Int32> Identifiers { get; set; }

            /// <summary>
            /// Gets or sets the position of the vertex.
            /// </summary>
            public Coordinate Position { get; set; }

            /// <summary>
            /// Checks whether the vertex is on the boundary of the graph.
            /// </summary>
            public Boolean OnBoundary
            {
                get
                {
                    if (Halfedge == null)
                        return true;

                    // Search adjacent faces for any that are null
                    return Edges.Any(e => e.OnBoundary);
                }
            }

            #endregion

            #region IVertex properties (explicit)

            /// <summary>
            /// A halfedge that originates from the vertex.
            /// </summary>
            IHalfedge IVertex.Halfedge
            {
                get { return Halfedge; }
            }

            /// <summary>
            /// The graph the vertex belongs to.
            /// </summary>
            IHalfedgeGraph IVertex.Graph
            {
                get { return Graph; }
            }

            /// <summary>
            /// An iterator for the halfedges originating from the vertex.
            /// </summary>
            IEnumerable<IHalfedge> IVertex.Halfedges
            {
                get { return Halfedges; }
            }

            /// <summary>
            /// An iterator for the vertices in the one ring neighborhood.
            /// </summary>
            IEnumerable<IVertex> IVertex.Vertices
            {
                get { return Vertices; }
            }

            /// <summary>
            /// An iterator for edges connected to the vertex.
            /// </summary>
            IEnumerable<IEdge> IVertex.Edges
            {
                get { return Edges; }
            }

            /// <summary>
            /// An iterator for the faces bounding with the vertex.
            /// </summary>
            IEnumerable<IFace> IVertex.Faces
            {
                get { return Faces; }
            }

            /// <summary>
            /// Searches for a halfedge pointing to a vertex from this vertex.
            /// </summary>
            /// <param name="vertex">A vertex pointed to by the halfedge to search for.</param>
            /// <returns>The halfedge from this vertex to the specified vertex. If none exists, returns null.</returns>
            IHalfedge IVertex.FindHalfedgeTo(IVertex vertex)
            {
                return FindHalfedgeTo(vertex);
            }

            /// <summary>
            /// Searches for the halfedge pointing to the specified face from this vertex.
            /// </summary>
            /// <param name="face">The face the halfedge to find points to.</param>
            /// <returns>The halfedge if it is found, otherwise null.</returns>
            IHalfedge IVertex.FindHalfedgeTo(IFace face)
            {
                return FindHalfedgeTo(face);
            }

            /// <summary>
            /// Searches for the edge associated with the specified vertex.
            /// </summary>
            /// <param name="vertex">A vertex sharing an edge with this vertex.</param>
            /// <returns>The edge if it is found, otherwise null.</returns>
            IEdge IVertex.FindEdgeTo(IVertex vertex)
            {
                return FindEdgeTo(vertex);
            }

            #endregion
        }
    }
}