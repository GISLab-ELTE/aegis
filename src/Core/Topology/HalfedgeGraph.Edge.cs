// <copyright file="HalfedgeGraph.Edge.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    public partial class HalfedgeGraph
    {
        /// <summary>
        /// Represents an edge of the topology graph.
        /// </summary>
        /// <author>Máté Cserép</author>
        private class Edge : IEdge
        {
            #region Public properties

            /// <summary>
            /// Gets or sets the first halfedge that corresponds to the edge.
            /// </summary>
            public Halfedge HalfedgeA { get; set; }

            /// <summary>
            /// Gets or sets the second halfedge that corresponds to the edge.
            /// </summary>
            public Halfedge HalfedgeB
            {
                get { return HalfedgeA.Opposite; }

                set { HalfedgeA.Opposite = value; }
            }

            /// <summary>
            /// Gets the first vertex on the edge.
            /// </summary>
            public Vertex VertexA
            {
                get { return HalfedgeA.ToVertex; }
            }

            /// <summary>
            /// Gets the second vertex on the edge.
            /// </summary>
            public Vertex VertexB
            {
                get { return HalfedgeA.Opposite.ToVertex; }
            }

            /// <summary>
            /// Gets the first face adjacent to the edge.
            /// </summary>
            /// <value>The first face adjacent to the edge, or <c>null</c> if there is no face.</value>
            public Face FaceA
            {
                get { return HalfedgeA.Face; }
            }

            /// <summary>
            /// Gets the second face adjacent to the edge.
            /// </summary>
            /// <value>The second face adjacent to the edge, or <c>null</c> if there is no face.</value>
            public Face FaceB
            {
                get { return HalfedgeA.Opposite.Face; }
            }

            /// <summary>
            /// Gets the graph the edge belongs to.
            /// </summary>
            public HalfedgeGraph Graph
            {
                get { return HalfedgeA.Graph; }
            }

            /// <summary>
            /// Gets or sets the index of this edge in the internal edge list of the graph.
            /// </summary>
            public Int32 Index { get; set; }

            #endregion

            #region IEdge properties

            /// <summary>
            /// Gets the identifiers of the edge.
            /// </summary>
            /// <remarks>The identifiers of the edge are the intersection of identifiers of the edge's endpoint vertices.</remarks>
            public ISet<Int32> Identifiers
            {
                get
                {
                    ISet<Int32> result = new HashSet<Int32>(VertexA.Identifiers);
                    result.IntersectWith(VertexB.Identifiers);
                    return result;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the edge is on the boundary of the graph.
            /// </summary>
            /// <value>
            /// <c>true</c> if the edge is on the boundary of the graph; otherwise, <c>false</c>.
            /// </value>
            public Boolean OnBoundary
            {
                get { return HalfedgeA.OnBoundary || HalfedgeA.Opposite.OnBoundary; }
            }

            #endregion

            #region IEdge properties (explicit)

            /// <summary>
            /// Gets the first halfedge that corresponds to the edge.
            /// </summary>
            IHalfedge IEdge.HalfedgeA
            {
                get { return HalfedgeA; }
            }

            /// <summary>
            /// Gets the second halfedge that corresponds to the edge.
            /// </summary>
            IHalfedge IEdge.HalfedgeB
            {
                get { return HalfedgeB; }
            }

            /// <summary>
            /// Gets the first vertex on the edge.
            /// </summary>
            IVertex IEdge.VertexA
            {
                get { return VertexA; }
            }

            /// <summary>
            /// Gets the second vertex on the edge.
            /// </summary>
            IVertex IEdge.VertexB
            {
                get { return VertexB; }
            }

            /// <summary>
            /// Gets the first face adjacent to the edge.
            /// </summary>
            /// <value>
            /// The first face adjacent to the edge, or <c>null</c> if there is no face.
            /// </value>
            IFace IEdge.FaceA
            {
                get { return FaceA; }
            }

            /// <summary>
            /// Gets the second face adjacent to the edge.
            /// </summary>
            /// <value>
            /// The second face adjacent to the edge, or <c>null</c> if there is no face.
            /// </value>
            IFace IEdge.FaceB
            {
                get { return FaceB; }
            }

            /// <summary>
            /// Gets the graph the edge belongs to.
            /// </summary>
            IHalfedgeGraph IEdge.Graph
            {
                get { return Graph; }
            }

            #endregion
        }
    }
}
