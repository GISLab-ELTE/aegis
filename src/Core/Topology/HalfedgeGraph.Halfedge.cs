// <copyright file="HalfedgeGraph.Halfedge.cs" company="Eötvös Loránd University (ELTE)">
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
        /// Represents a halfedge of the topology graph.
        /// </summary>
        /// <author>Máté Cserép</author>
        private class Halfedge : IHalfedge
        {
            #region Public properties

            /// <summary>
            /// Gets or sets the opposite halfedge.
            /// </summary>
            public Halfedge Opposite { get; set; }

            /// <summary>
            /// Gets the next halfedge.
            /// </summary>
            public Halfedge Next { get; set; }

            /// <summary>
            /// Gets or sets the previous halfedge.
            /// </summary>
            public Halfedge Previous { get; set; }

            /// <summary>
            /// Gets the vertex the halfedge points to.
            /// </summary>
            public Vertex ToVertex { get; set; }

            /// <summary>
            /// Gets or sets the vertex the halfedge originates from.
            /// </summary>
            public Vertex FromVertex
            {
                get { return Opposite.ToVertex; }
            }

            /// <summary>
            /// Gets or sets the edge corresponding to the halfedge.
            /// </summary>
            public Edge Edge { get; set; }

            /// <summary>
            /// Gets or sets the face corresponding to the halfedge.
            /// </summary>
            public Face Face { get; set; }

            /// <summary>
            /// Gets the graph the halfedge belongs to.
            /// </summary>
            public HalfedgeGraph Graph
            {
                get { return ToVertex.Graph; }
            }

            /// <summary>
            /// Gets or sets the index of this halfedge in the internal halfedge list of the graph.
            /// </summary>
            public Int32 Index { get; set; }

            #endregion

            #region IHalfedge properties

            /// <summary>
            /// Gets a value indicating whether the halfedge is on the boundary of the graph.
            /// </summary>
            /// <value><c>true</c> if the halfedge is on the boundary of the graph; otherwise, <c>false</c>.</value>
            public Boolean OnBoundary
            {
                get { return Face == null; }
            }

            #endregion

            #region IHalfedge properties (explicit)

            /// <summary>
            /// Gets the opposite halfedge.
            /// </summary>
            IHalfedge IHalfedge.Opposite
            {
                get { return Opposite; }
            }

            /// <summary>
            /// Gets the next halfedge.
            /// </summary>
            IHalfedge IHalfedge.Next
            {
                get { return Next; }
            }

            /// <summary>
            /// Gets the previous halfedge.
            /// </summary>
            IHalfedge IHalfedge.Previous
            {
                get { return Previous; }
            }

            /// <summary>
            /// Gets the vertex the halfedge points to.
            /// </summary>
            IVertex IHalfedge.ToVertex
            {
                get { return ToVertex; }
            }

            /// <summary>
            /// Gets the vertex the halfedge originates from.
            /// </summary>
            IVertex IHalfedge.FromVertex
            {
                get { return FromVertex; }
            }

            /// <summary>
            /// Gets the edge corresponding to the halfedge.
            /// </summary>
            IEdge IHalfedge.Edge
            {
                get { return Edge; }
            }

            /// <summary>
            /// Gets the face corresponding to the halfedge.
            /// </summary>
            IFace IHalfedge.Face
            {
                get { return Face; }
            }

            /// <summary>
            /// Gets the graph the halfedge belongs to.
            /// </summary>
            IHalfedgeGraph IHalfedge.Graph
            {
                get { return Graph; }
            }

            #endregion
        }
    }
}