// <copyright file="HalfedgeGraph.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using AEGIS.Algorithms;
    using AEGIS.Geometries;

    /// <summary>
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    /// <remarks>
    /// The implementation of the core topological model was inspired by Alexander Kolliopoulos's <a href="http://www.dgp.toronto.edu/~alexk/">Lydos library</a>.
    /// </remarks>
    /// <author>Máté Cserép</author>
    public sealed partial class HalfedgeGraph : IHalfedgeGraph
    {
        #region Private fields

        /// <summary>
        /// Stores the collection of halfedges in the graph.
        /// </summary>
        private List<Halfedge> _halfedges = new List<Halfedge>();

        /// <summary>
        /// Stores the collection of vertices in the graph.
        /// </summary>
        private Dictionary<Coordinate, Vertex> _vertices = new Dictionary<Coordinate, Vertex>();

        /// <summary>
        /// Stores the collection of edges in the graph.
        /// </summary>
        private List<Edge> _edges = new List<Edge>();

        /// <summary>
        /// Stores the collection of faces in the graph.
        /// </summary>
        private List<Face> _faces = new List<Face>();

        /// <summary>
        /// Stores the geometry identifier provider.
        /// </summary>
        private readonly IIdentifierProvider _identifierProvider;

        /// <summary>
        /// Stores the precision model.
        /// </summary>
        private readonly PrecisionModel _precisionModel;

        #endregion

        #region IHalfedgeGraph properties

        /// <summary>
        /// Gets the collection of halfedges in the graph.
        /// </summary>
        public IEnumerable<IHalfedge> Halfedges
        {
            get { return _halfedges.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the collection of vertices in the graph.
        /// </summary>
        public IEnumerable<IVertex> Vertices
        {
            get { return _vertices.Values; }
        }

        /// <summary>
        /// Gets the collection of edges in the graph.
        /// </summary>
        public IEnumerable<IEdge> Edges
        {
            get { return _edges.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the collection of faces in the graph.
        /// </summary>
        public IEnumerable<IFace> Faces
        {
            get { return _faces.Where(face => face.Type.HasFlag(FaceType.Shell)); }
        }

        #endregion

        #region IHalfedgeGraph methods

        /// <summary>
        /// Removes all elements from the graph.
        /// </summary>
        public void Clear()
        {
            _edges.Clear();
            _faces.Clear();
            _halfedges.Clear();
            _vertices.Clear();
        }

        /// <summary>
        /// Trims internal data structures to their current size.
        /// </summary>
        /// <remarks>
        /// Call this method to prevent excess memory usage when the graph is done being built.
        /// </remarks>
        public void TrimExcess()
        {
            _edges.TrimExcess();
            _faces.TrimExcess();
            _halfedges.TrimExcess();
        }

        /// <summary>
        /// Checks halfedge connections to verify that a valid topology graph was constructed.
        /// </summary>
        /// <remarks>
        /// Checking for proper topology in every case when a face is added would slow down
        /// graph construction significantly, so this method may be called once when a graph
        /// is complete to ensure that topology is manifold (with boundary).
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the graph is non-manifold.</exception>
        public void VerifyTopology()
        {
            foreach (Halfedge halfedge in _halfedges)
            {
                if (halfedge != halfedge.Opposite.Opposite)
                    throw new InvalidOperationException("A halfedge's opposite's opposite is not itself.");

                if (halfedge.Edge != halfedge.Opposite.Edge)
                    throw new InvalidOperationException("Opposite halfedges do not belong to the same edge.");

                if (halfedge.ToVertex.Halfedge.Opposite.ToVertex != halfedge.ToVertex)
                    throw new InvalidOperationException("The halfedge-vertex mapping is corrupted.");

                if (halfedge.Previous.Next != halfedge)
                    throw new InvalidOperationException("A halfedge's previous next is not itself.");

                if (halfedge.Next.Previous != halfedge)
                    throw new InvalidOperationException("A halfedge's next previous is not itself.");

                if (halfedge.Next.Face != halfedge.Face)
                    throw new InvalidOperationException("Adjacent halfedges do not belong to the same face.");

                // Make sure each halfedge is reachable from the vertex it originates from.
                if (!halfedge.FromVertex.Halfedges.Contains(halfedge))
                    throw new InvalidOperationException("A halfedge is not reachable from the vertex it originates from.");
            }

            foreach (Face face in _faces)
            {
                // Retrieve whether the face is contained as a hole by another face.
                Boolean isHole = _faces.SelectMany(f => f.Holes).Contains(face);
                if (isHole && !face.Type.HasFlag(FaceType.Hole))
                    throw new InvalidOperationException("A hole is not marked as hole.");
                if (!isHole && face.Type.HasFlag(FaceType.Hole))
                    throw new InvalidOperationException("A not hole shell is marked as hole.");
            }
        }

        /// <summary>
        /// Adds an (isolated) vertex to the graph.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <param name="identifiers">The identifiers of the vertex.</param>
        /// <returns>The vertex created by this method.</returns>
        /// <remarks>When a vertex already exists at the given position, it will be returned instead of creating a new one.</remarks>
        public IVertex AddVertex(Coordinate position, ISet<Int32> identifiers = null)
        {
            Vertex vertex = GetVertex(position);
            if (identifiers != null)
                vertex.Identifiers.UnionWith(identifiers);
            return vertex;
        }

        /// <summary>
        /// Adds a face to the graph. The new face must appropriately fit to the existing topology graph without any overlap.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
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
        public IFace AddFace(IBasicPolygon polygon, ISet<Int32> identifiers = null)
        {
            return AddFace(polygon.Shell, polygon.Holes.ToList<IReadOnlyList<Coordinate>>(), identifiers);
        }

        /// <summary>
        /// Adds a face to the graph. The new face must appropriately fit to the existing topology graph without any overlap.
        /// </summary>
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
        /// <remarks>Please note, that for this method the vertices of the shell must be given in counter-clockwise order, while the vertices of the holes in clockwise order.</remarks>
        public IFace AddFace(IReadOnlyList<Coordinate> shell, List<IReadOnlyList<Coordinate>> holes = null, ISet<Int32> identifiers = null)
        {
            // Shell validation
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (shell.Count > 1 && shell[0].Equals(shell[shell.Count - 1]))
                shell = shell.Take(shell.Count - 1).ToList();
            if (shell.Count < 3)
                throw new ArgumentException("The shell does not contain at least 3 different coordinates.", "shell");

            // Holes validation
            if (holes != null)
            {
                for (Int32 i = 0; i < holes.Count; ++i)
                {
                    if (holes[i] == null)
                        throw new ArgumentNullException("holes", "A hole is null.");
                    if (holes[i].Count > 1 && holes[i][0].Equals(holes[i][holes[i].Count - 1]))
                        holes[i] = holes[i].Take(holes[i].Count - 1).ToList();
                    if (holes[i].Count < 3)
                        throw new ArgumentException("A hole does not contain at least 3 different coordinates.", "holes");
                }
            }

            Face shellFace = GetFace(shell, FaceType.Shell);
            if(identifiers != null)
                foreach (Vertex vertex in shellFace.Vertices)
                    vertex.Identifiers.UnionWith(identifiers);
            if (holes != null)
            {
                foreach (IReadOnlyList<Coordinate> hole in holes)
                {
                    Face holeFace = GetFace(hole.Reverse(), FaceType.Hole);
                    if (identifiers != null)
                        foreach (Vertex vertex in holeFace.Vertices)
                            vertex.Identifiers.UnionWith(identifiers);
                    shellFace.Holes.Add(holeFace);
                }
            }
            return shellFace;
        }

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <param name="position">The position of the vertex to remove.</param>
        /// <param name="mode">The mode of the removal.</param>
        /// <returns><c>true</c> when the coordinate to remove exists in the graph; otherwise <c>false</c>.</returns>
        /// <remarks>The algorithm may be forced by the <see cref="mode" /> parameter to remove the adjacent faces of the vertex.</remarks>
        public Boolean RemoveVertex(Coordinate position, RemoveMode mode = RemoveMode.Normal)
        {
            if (!_vertices.ContainsKey(position)) return false;
            RemoveVertex(_vertices[position], mode);
            return true;
        }

        /// <summary>
        /// Removes a face from the graph.
        /// </summary>
        /// <param name="face">The face to remove.</param>
        /// <param name="mode">The mode of the removal.</param>
        /// <exception cref="System.ArgumentException">Face is inconvertible to actual representation.</exception>
        /// <remarks>The forced removal mode makes no difference for this method in contrast to the normal mode.</remarks>
        public void RemoveFace(IFace face, RemoveMode mode = RemoveMode.Clean)
        {
            try
            {
                RemoveFace(face as Face, mode);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Face is inconvertible to the actual representation.", "face", ex);
            }
        }

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
        public ICollection<IFace> MergeFace(IBasicPolygon polygon, ISet<Int32> identifiers = null)
        {
            return MergeFace(polygon.Shell, polygon.Holes, identifiers);
        }

        /// <summary>
        /// Merges a face into the graph, resolving face overlapping.
        /// </summary>
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
        /// <remarks>Please note, that for this method the vertices of the shell must be given in counter-clockwise order, while the vertices of the holes in clockwise order.</remarks>
        public ICollection<IFace> MergeFace(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes = null, ISet<Int32> identifiers = null)
        {
            return MergeFace(shell, holes, identifiers, null);
        }

        /// <summary>
        /// Merges another graph into the current instance.
        /// </summary>
        /// <param name="other">The other graph.</param>
        /// <exception cref="System.ArgumentNullException">The other graph is null.</exception>
        public void MergeGraph(IHalfedgeGraph other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other graph is null.");

            foreach (IFace oFace in other.Faces)
            {
                List<Coordinate> shellPositions = oFace.Vertices.Select(vertex => vertex.Position).ToList();
                shellPositions.Add(shellPositions.First());

                IEnumerable<List<Coordinate>> holesPositions = oFace.Holes.Select(
                    face => face.Vertices.Select(vertex => vertex.Position).ToList());

                foreach (List<Coordinate> positions in holesPositions)
                {
                    positions.Add(positions.First());
                    positions.Reverse();
                }

                MergeFace(shellPositions, holesPositions, oFace.Identifiers);
            }
        }

        #endregion

        #region IGeometry support methods

        /// <summary>
        /// Adds a (supported type of) geometry to the graph. The new geometry must appropriately fit to the existing topology graph without any overlap.
        /// </summary>
        /// <remarks>
        /// The supported types are <see cref="IPoint"/>, <see cref="ILinearRing"/>, <see cref="IPolygon"/>, <see cref="IMultiPoint"/>, <see cref="IMultiPolygon"/> and <see cref="IGeometryCollection{T}"/>.
        /// </remarks>
        /// <param name="geometry">The geometry to add.</param>
        public void AddGeometry(IGeometry geometry)
        {
            if (geometry is IPoint)
                AddPoint(geometry as IPoint);
            else if (geometry is ILinearRing)
                AddLinearRing(geometry as ILinearRing);
            else if (geometry is IPolygon)
                AddPolygon(geometry as IPolygon);
            else if (geometry is IMultiPoint)
                AddMultiPoint(geometry as IMultiPoint);
            else if (geometry is IMultiPolygon)
                AddMultiPolygon(geometry as IMultiPolygon);
            else if (geometry is IGeometryCollection<IGeometry>)
                foreach (IGeometry subGeometry in (IGeometryCollection<IGeometry>)geometry)
                    AddGeometry(subGeometry);
            else
                throw new NotSupportedException("The specified geometry type is not supported.");
        }

        /// <summary>
        /// Adds an (isolated) point to the graph.
        /// </summary>
        /// <remarks>
        /// When a point already exists at the given position, it will be returned instead of creating a new one.
        /// </remarks>
        /// <param name="point">The point to add.</param>
        /// <returns>The vertex created by this method.</returns>
        public IVertex AddPoint(IPoint point)
        {
            return AddVertex(point.Coordinate, _identifierProvider.GetIdentifiers(point));
        }

        /// <summary>
        /// Adds a linear ring to the graph.
        /// </summary>
        /// <param name="linearRing">The linear ring to add.</param>
        /// <returns>The face created by this method.</returns>
        public IFace AddLinearRing(ILinearRing linearRing)
        {
            return AddFace(linearRing, null, _identifierProvider.GetIdentifiers(linearRing));
        }

        /// <summary>
        /// Adds a polygon to the graph.
        /// </summary>
        /// <param name="polygon">The polygon to add.</param>
        /// <returns>The face created by this method.</returns>
        public IFace AddPolygon(IPolygon polygon)
        {
            return AddFace(polygon, _identifierProvider.GetIdentifiers(polygon));
        }

        /// <summary>
        /// Adds multiple (isolated) points to the graph.
        /// </summary>
        /// <param name="multiPoint">The points to add.</param>
        /// <returns>The vertices created by this method.</returns>
        public IVertex[] AddMultiPoint(IMultiPoint multiPoint)
        {
            var vertices = new IVertex[multiPoint.Count];
            for (Int32 i = 0; i < multiPoint.Count; ++i)
                vertices[i] = AddPoint(multiPoint[i]);
            return vertices;
        }

        /// <summary>
        /// Adds multiple polygons to the graph.
        /// </summary>
        /// <param name="multiPolygon">The polygons to add.</param>
        /// <returns>The faces created by this method.</returns>
        public IFace[] AddMultiPolygon(IMultiPolygon multiPolygon)
        {
            var faces = new IFace[multiPolygon.Count];
            for (Int32 i = 0; i < multiPolygon.Count; ++i)
                faces[i] = AddPolygon(multiPolygon[i]);
            return faces;
        }

        /// <summary>
        /// Merges a (supported type of) geometry into the graph, resolving geometry overlapping.
        /// </summary>
        /// <param name="geometry">The geometry to merge.</param>
        /// <exception cref="System.ArgumentException">The specified geometry type is not supported.</exception>
        /// <remarks>The supported types are <see cref="IPoint" />, <see cref="ILinearRing" />, <see cref="IPolygon" />, <see cref="IMultiPoint" />,  <see cref="IMultiPolygon"/> and <see cref="IGeometryCollection{T}"/>.</remarks>
        public void MergeGeometry(IGeometry geometry)
        {
            if (geometry is IPoint)
                AddPoint(geometry as IPoint);
            else if (geometry is ILinearRing)
                MergeLinearRing(geometry as ILinearRing);
            else if (geometry is IPolygon)
                MergePolygon(geometry as IPolygon);
            else if (geometry is IMultiPoint)
                AddMultiPoint(geometry as IMultiPoint);
            else if (geometry is IMultiPolygon)
                MergeMultiPolygon(geometry as IMultiPolygon);
            else if (geometry is IGeometryCollection<IGeometry>)
                foreach (IGeometry subGeometry in (IGeometryCollection<IGeometry>)geometry)
                    MergeGeometry(subGeometry);
            else
                throw new NotSupportedException("The specified geometry type is not supported.");
        }

        /// <summary>
        /// Merges a linear ring into the graph.
        /// </summary>
        /// <param name="linearRing">The linear ring to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        public ICollection<IFace> MergeLinearRing(ILinearRing linearRing)
        {
            return MergeFace(linearRing, null, _identifierProvider.GetIdentifiers(linearRing));
        }

        /// <summary>
        /// Merges a polygon into the graph.
        /// </summary>
        /// <param name="polygon">The polygon to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        public ICollection<IFace> MergePolygon(IPolygon polygon)
        {
            return MergeFace(polygon, _identifierProvider.GetIdentifiers(polygon));
        }

        /// <summary>
        /// Merges multiple polygons into the graph.
        /// </summary>
        /// <param name="multiPolygon">The polygons to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        public ICollection<IFace> MergeMultiPolygon(IMultiPolygon multiPolygon)
        {
            var faces = new List<IFace>(multiPolygon.Count);
            foreach (var polygon in multiPolygon)
                faces.AddRange(MergePolygon(polygon));
            return faces;
        }

        /// <summary>
        /// Converts the graph to a collection of geometries.
        /// </summary>
        /// <param name="factory">The geometry factory used to produce the polygon.</param>
        /// <returns>A collection of geometries representing the topology graph.</returns>
        public IGeometry ToGeometry(IGeometryFactory factory = null)
        {
            if (factory == null)
                factory = new GeometryFactory();

            List<IGeometry> resultCollection = _faces.Where(face => face.Type.HasFlag(FaceType.Shell))
                                                     .Select(face => face.ToGeometry(factory) as IGeometry)
                                                     .ToList();

            if (resultCollection.Count == 0)
                return null;

            if (resultCollection.Count == 1)
                return resultCollection[0];

            return factory.CreateGeometryCollection(resultCollection);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HalfedgeGraph"/> class.
        /// </summary>
        public HalfedgeGraph()
            : this(null, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HalfedgeGraph"/> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        public HalfedgeGraph(PrecisionModel precisionModel)
            : this(null, precisionModel)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HalfedgeGraph"/> class.
        /// </summary>
        /// <param name="identifierProvider">The geometry identifier provider.</param>
        public HalfedgeGraph(IIdentifierProvider identifierProvider)
            : this(identifierProvider, null)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="HalfedgeGraph"/> class.
        /// </summary>
        /// <param name="identifierProvider">The geometry identifier provider.</param>
        /// <param name="precisionModel">The precision model.</param>
        public HalfedgeGraph(IIdentifierProvider identifierProvider, PrecisionModel precisionModel)
        {
            _identifierProvider = identifierProvider ?? new NullIdentifierProvider();
            _precisionModel = precisionModel ?? PrecisionModel.Default;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns a string representing the connections of vertices for each face in the graph.
        /// </summary>
        /// <returns>The position for each vertex of each face on a line of the string.</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var face in _faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    sb.Append(vertex.Position);
                    sb.Append(" -> ");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds a halfedge to the halfedge list.
        /// </summary>
        /// <param name="halfedge">The halfedge to add.</param>
        private void AppendToHalfedgeList(Halfedge halfedge)
        {
            halfedge.Index = _halfedges.Count;
            _halfedges.Add(halfedge);
        }

        /// <summary>
        /// Adds a vertex to the vertex list.
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        private void AppendToVertexList(Vertex vertex)
        {
            vertex.Index = _vertices.Count;
            vertex.Graph = this;
            _vertices.Add(vertex.Position, vertex);
        }

        /// <summary>
        /// Adds an edge to the edge list.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        private void AppendToEdgeList(Edge edge)
        {
            edge.Index = _edges.Count;
            _edges.Add(edge);
        }

        /// <summary>
        /// Adds a face to the face list.
        /// </summary>
        /// <param name="face">The face to add.</param>
        private void AppendToFaceList(Face face)
        {
            face.Index = _faces.Count;
            _faces.Add(face);
        }

        /// <summary>
        /// Removes a halfedge from the halfedge list.
        /// </summary>
        /// <param name="halfedge">The halfedge to remove.</param>
        private void RemoveFromHalfedgeList(Halfedge halfedge)
        {
            foreach (Halfedge item in _halfedges.Where(item => item.Index > halfedge.Index))
                item.Index -= 1;
            _halfedges.Remove(halfedge);
        }

        /// <summary>
        /// Removes a vertex from the vertex list.
        /// </summary>
        /// <param name="vertex">The vertex to remove.</param>
        private void RemoveFromVertexList(Vertex vertex)
        {
            foreach (Vertex item in _vertices.Where(pair => pair.Value.Index > vertex.Index).Select(pair => pair.Value))
                item.Index -= 1;
            _vertices.Remove(vertex.Position);
        }

        /// <summary>
        /// Removes an edge from the edge list.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        private void RemoveFromEdgeList(Edge edge)
        {
            foreach (Edge item in _edges.Where(item => item.Index > edge.Index))
                item.Index -= 1;
            _edges.Remove(edge);
        }

        /// <summary>
        /// Removes a face from the face list.
        /// </summary>
        /// <param name="face">The face to remove.</param>
        private void RemoveFromFaceList(Face face)
        {
            foreach (var item in _faces.Where(item => item.Index > face.Index))
                item.Index -= 1;
            _faces.Remove(face);
        }

        /// <summary>
        /// Gets an existing vertex by position or creates a new vertex in the graph.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <returns>The vertex at the given position.</returns>
        private Vertex GetVertex(Coordinate position)
        {
            Vertex vertex;
            if (_vertices.TryGetValue(position, out vertex))
                return vertex;
            else
                return CreateVertex(position);
        }

        /// <summary>
        /// Creates a new, isolated vertex in the graph.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <returns>The vertex created by this method.</returns>
        private Vertex CreateVertex(Coordinate position)
        {
            var vertex = new Vertex(position);
            AppendToVertexList(vertex);
            return vertex;
        }

        /// <summary>
        /// Gets an existing matching face by its coordinates or creates a new face in the graph.
        /// </summary>
        /// <param name="positions">The positions of the vertices.</param>
        /// <param name="addType">The face type (shell, hole or both) to add to the retrieved face.</param>
        /// <returns>The face wit the given coordinates.</returns>
        private Face GetFace(IEnumerable<Coordinate> positions, FaceType addType = 0)
        {
            Vertex[] vertices = positions.Select(GetVertex).ToArray();
            Int32 n = vertices.Length;

            for (Int32 i = 0; i < n; ++i)
            {
                // Calculate the index of the following vertex.
                Int32 j = (i + 1) % n;

                // Find existing halfedges for this face.
                if (vertices[i].FindHalfedgeTo(vertices[j]) == null)
                {
                    try
                    {
                        return CreateFace(vertices, addType);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("Face creation error occurred with the given positions.", "positions", ex);
                    }
                }
            }

            // All necessary halfedges exist, check for the face itself.
            CoordinateRing ring = new CoordinateRing(positions);
            Face face = _faces.Find(f => ring.Equals(new CoordinateRing(f.Vertices.Select(v => v.Position))));
            if (face == null)
            {
                try
                {
                    return CreateFace(vertices, addType);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Face creation error occurred with the given positions.", "positions", ex);
                }
            }

            face.Type |= addType;
            return face;
        }

        /// <summary>
        /// Creates a new face in the graph.
        /// </summary>
        /// <param name="vertices">The vertices of the face in counter-clockwise order.</param>
        /// <param name="addType">The face type (shell, hole or both) to assign to the new face.</param>
        /// <returns>The face created by this method.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
        /// <exception cref="ArgumentException">Thrown when fewer than three vertices are given or an inconvenient vertex is given.</exception>
        /// <exception cref="InvalidOperationException">Thrown when cannot form a valid face with the given vertices and the existing topology.</exception>
        private Face CreateFace(Vertex[] vertices, FaceType addType)
        {
            #region Initialization

            Int32 n = vertices.Length;

            // Require at least 3 vertices.
            if (n < 3)
                throw new ArgumentException("Cannot create a polygon with fewer than three vertices.", "vertices");

            Halfedge[] halfedges = new Halfedge[n];
            Boolean[] isNewEdge = new Boolean[n];
            Boolean[] isIsolatedVertex = new Boolean[n];

            #endregion

            #region Input validation

            // Make sure input is (mostly) acceptable before making any changes to the graph.
            for (Int32 i = 0; i < n; ++i)
            {
                if (vertices[i] == null)
                    throw new ArgumentNullException("vertices", "Can't add a null vertex to a face.");
                if (!vertices[i].OnBoundary)
                    throw new ArgumentException("Can't add an edge to a vertex on the interior of a graph.", "vertices");

                // Calculate the index of the following vertex.
                Int32 j = (i + 1) % n;

                // Find existing halfedges for this face.
                halfedges[i] = vertices[i].FindHalfedgeTo(vertices[j]);
                isNewEdge[i] = halfedges[i] == null;
                isIsolatedVertex[i] = vertices[i].Halfedge == null;

                if (!isNewEdge[i] && !halfedges[i].OnBoundary)
                    throw new InvalidOperationException("Can't add more than two faces to an edge.");
            }

            #endregion

            #region Create the face and the new edges

            // Create face.
            Face face = new Face { Type = addType };
            AppendToFaceList(face);

            // Create new edges.
            for (Int32 i = 0; i < n; ++i)
            {
                if (isNewEdge[i])
                {
                    // Calculate the index of the following vertex.
                    Int32 j = (i + 1) % n;

                    // Create new edge.
                    Edge edge = new Edge();
                    AppendToEdgeList(edge);

                    // Create new halfedges.
                    halfedges[i] = new Halfedge();
                    AppendToHalfedgeList(halfedges[i]);

                    halfedges[i].Opposite = new Halfedge();
                    AppendToHalfedgeList(halfedges[i].Opposite);

                    // Connect opposite halfedge to inner halfedge.
                    halfedges[i].Opposite.Opposite = halfedges[i];

                    // Connect edge to halfedges.
                    edge.HalfedgeA = halfedges[i];

                    // Connect half edges to edge.
                    halfedges[i].Edge = edge;
                    halfedges[i].Opposite.Edge = edge;

                    // Connect halfedges to vertices.
                    halfedges[i].ToVertex = vertices[j];
                    halfedges[i].Opposite.ToVertex = vertices[i];

                    // Connect vertex to outgoing halfedge if it doesn't have one yet.
                    if (isIsolatedVertex[i])
                        vertices[i].Halfedge = halfedges[i];
                }

                if (halfedges[i].Face != null)
                    throw new InvalidOperationException("An inner halfedge already has a face assigned to it.");

                // Connect inner halfedge to face.
                halfedges[i].Face = face;
            }

            #endregion

            #region Adjust halfedge connections

            // Connect next/previous halfedges.
            for (Int32 i = 0; i < n; ++i)
            {
                // Calculate the index of the following vertex.
                Int32 j = (i + 1) % n;

                // Outer halfedges
                if (isNewEdge[i] && isNewEdge[j] && !isIsolatedVertex[j]) // Both edges are new and vertex has faces connected already.
                {
                    // Find the closing halfedge of the first available opening.
                    Halfedge closeHalfedge = vertices[j].Halfedges.First(h => h.Face == null);
                    Halfedge openHalfedge = closeHalfedge.Previous;

                    // Link new outer halfedges into this opening.
                    halfedges[i].Opposite.Previous = openHalfedge;
                    openHalfedge.Next = halfedges[i].Opposite;
                    halfedges[j].Opposite.Next = closeHalfedge;
                    closeHalfedge.Previous = halfedges[j].Opposite;
                }
                else if (isNewEdge[i] && isNewEdge[j]) // Both edges are new.
                {
                    halfedges[i].Opposite.Previous = halfedges[j].Opposite;
                    halfedges[j].Opposite.Next = halfedges[i].Opposite;
                }
                else if (isNewEdge[i] && !isNewEdge[j]) // This is new, next is old.
                {
                    halfedges[i].Opposite.Previous = halfedges[j].Previous;
                    halfedges[j].Previous.Next = halfedges[i].Opposite;
                }
                else if (!isNewEdge[i] && isNewEdge[j]) // This is old, next is new.
                {
                    halfedges[i].Next.Previous = halfedges[j].Opposite;
                    halfedges[j].Opposite.Next = halfedges[i].Next;
                }
                else if (!isNewEdge[i] && !isNewEdge[j] && halfedges[i].Next != halfedges[j]) // Relink faces before adding new edges if they are in the way of a new face.
                {
                    Halfedge closeHalfedge = halfedges[i].Opposite;

                    // Find the closing halfedge of the opening opposite the opening halfedge i is on.
                    do
                    {
                        closeHalfedge = closeHalfedge.Previous.Opposite;
                    } while (closeHalfedge.Face != null &&
                        closeHalfedge != halfedges[j] && closeHalfedge != halfedges[i].Opposite);

                    if (closeHalfedge == halfedges[j] || closeHalfedge == halfedges[i].Opposite)
                        throw new InvalidOperationException("Unable to find an opening to relink an existing face.");

                    Halfedge openHalfedge = closeHalfedge.Previous;

                    // Remove group of faces between two openings, close up gap to form one opening.
                    openHalfedge.Next = halfedges[i].Next;
                    halfedges[i].Next.Previous = openHalfedge;

                    // Insert group of faces into target opening.
                    halfedges[j].Previous.Next = closeHalfedge;
                    closeHalfedge.Previous = halfedges[j].Previous;
                }

                // Inner halfedges.
                halfedges[i].Next = halfedges[j];
                halfedges[j].Previous = halfedges[i];
            }

            #endregion

            #region Finalization

            // Connect face to an inner halfedge.
            face.Halfedge = halfedges[0];
            return face;

            #endregion
        }

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <param name="vertex">The vertex to remove from the graph.</param>
        /// <param name="mode">The mode of the removal.</param>
        /// <exception cref="System.ArgumentException">The given vertex is not located in the current graph.</exception>
        /// <exception cref="System.InvalidOperationException">The selected vertex is not isolated, force is required to remove entire face.</exception>
        /// <remarks>The algorithm may be forced by the <see cref="mode" /> parameter to remove the adjacent faces of the vertex.</remarks>
        private void RemoveVertex(Vertex vertex, RemoveMode mode)
        {
            if (vertex.Graph != this)
                throw new ArgumentException("The given vertex is not located in the current graph.", "vertex");

            if (vertex.Halfedge == null)
                RemoveFromVertexList(vertex);
            else if (mode == RemoveMode.Forced || mode == RemoveMode.Clean)
            {
                foreach (var face in vertex.Faces.ToList())
                    RemoveFace(face, mode);
                RemoveFromVertexList(vertex);
            }
            else
                throw new InvalidOperationException("The selected vertex is not isolated, force is required to remove entire face.");
        }

        /// <summary>
        /// Removes a face from the graph.
        /// </summary>
        /// <param name="face">The face to remove.</param>
        /// <param name="mode">The mode of the removal.</param>
        /// <remarks>The forced removal mode makes no difference for this method in contrast to the normal mode.</remarks>
        /// <exception cref="System.ArgumentNullException">The face is null.</exception>
        private void RemoveFace(Face face, RemoveMode mode = RemoveMode.Clean)
        {
            if (face == null)
                throw new ArgumentNullException("face", "The face is null.");
            if (face.Graph != this)
                throw new ArgumentException("The given face is not located in the current graph.", "face");

            // Vertices to become isolated.
            IEnumerable<Vertex> removeVertices = face.Vertices.Where(vertex => vertex.Faces.Count() == 1)
                                                     .ToList(); // only to prevent deferring query execution

            // Remove face.
            foreach (Halfedge halfedge in face.Halfedges.ToList())
            {
                halfedge.Face = null;
                if (halfedge.Opposite.Face == null)
                {
                    if (halfedge.FromVertex.Halfedge == halfedge)
                        halfedge.FromVertex.Halfedge = halfedge.FromVertex.Halfedges.FirstOrDefault(h => h.Face != face && h.Face != null);

                    if (halfedge.ToVertex.Halfedge == halfedge.Opposite)
                        halfedge.ToVertex.Halfedge = halfedge.ToVertex.Halfedges.FirstOrDefault(h => h.Face != face && h.Face != null);

                    halfedge.Previous.Next = halfedge.Opposite.Next;
                    halfedge.Opposite.Next.Previous = halfedge.Previous;

                    halfedge.Next.Previous = halfedge.Opposite.Previous;
                    halfedge.Opposite.Previous.Next = halfedge.Next;

                    RemoveFromEdgeList(halfedge.Edge);
                    RemoveFromHalfedgeList(halfedge);
                    RemoveFromHalfedgeList(halfedge.Opposite);
                }
            }
            // Remove isolated vertices when requested.
            foreach (Vertex vertex in removeVertices)
            {
                vertex.Halfedge = null;
                if (mode == RemoveMode.Clean)
                    RemoveVertex(vertex, RemoveMode.Normal);
            }
            RemoveFromFaceList(face);

            // Remove holes.
            foreach (Face hole in face.Holes)
            {
                switch (hole.Type)
                {
                    case FaceType.Hole:
                        RemoveFace(hole, mode);
                        break;
                    case FaceType.Both:
                        hole.Type = FaceType.Shell;
                        break;
                }
            }
        }

        /// <summary>
        /// Merges a face into the graph, resolving face intersections.
        /// </summary>
        /// <param name="shell">The vertices of the shell in counter-clockwise order.</param>
        /// <param name="holes">The vertices of the holes in clockwise order.</param>
        /// <param name="identifiers">The identifiers of the face.</param>
        /// <param name="excluded">Faces to exclude from further collision detection.</param>
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
        /// <remarks>Please note, that for this method the vertices of the shell must be given in counter-clockwise order, while the vertices of the holes in clockwise order.</remarks>
        private ICollection<IFace> MergeFace(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes, ISet<Int32> identifiers, List<IFace> excluded)
        {
            // Shell validation
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (shell.Count < 4)
                throw new ArgumentException("The shell does not contain at least 3 different coordinates.", "shell");
            if (!shell[0].Equals(shell[shell.Count - 1]))
                throw new ArgumentException("The first and the last coordinates of the shell are not equal.", "shell");

            // Holes validation
            if (holes != null)
            {
                foreach (IReadOnlyList<Coordinate> hole in holes)
                {
                    if (hole == null)
                        throw new ArgumentNullException("holes", "A hole is null.");
                    if (hole.Count < 4)
                        throw new ArgumentException("A hole does not contain at least 3 different coordinates.", "holes");
                    if (!hole[0].Equals(hole[hole.Count - 1]))
                        throw new ArgumentException("The first and the last coordinates of a hole are not equal.", "holes");
                }
            }

            if(excluded == null)
                excluded = new List<IFace>();

            // Retrieve the vertex positions for the possible collision faces.
            IReadOnlyList<Face> otherFaces = _faces.Where(face => face.Type.HasFlag(FaceType.Shell))
                                           .Where(face => !excluded.Contains(face))
                                           .ToList();
            IDictionary<Int32, IReadOnlyList<Coordinate>> otherPositions = new Dictionary<Int32, IReadOnlyList<Coordinate>>(otherFaces.Count);
            foreach (Face face in otherFaces)
            {
                List<Coordinate> positions = face.Vertices.Select(vertex => vertex.Position).ToList();
                positions.Add(positions.First());
                otherPositions.Add(face.Index, positions);
            }

            // Determine the real colliding faces.
            Envelope shellEnvelope = Envelope.FromCoordinates(shell);
            IReadOnlyList<Face> collisionFaces = (from face in otherFaces
                                          where Envelope.FromCoordinates(otherPositions[face.Index]).Intersects(shellEnvelope)
                                          where ShamosHoeyAlgorithm.Intersects(new[] { otherPositions[face.Index], shell }, _precisionModel) ||
                                              shell.All(coordinate => WindingNumberAlgorithm.InInterior(otherPositions[face.Index], coordinate, _precisionModel)) ||
                                              otherPositions[face.Index].All(coordinate => WindingNumberAlgorithm.InInterior(shell, coordinate, _precisionModel))
                                          orderby face.Type
                                          select face).ToList();

            // Result faces.
            List<IFace> result = new List<IFace>();

            // If there were any colliding faces, process the first one.
            Face collisionFace = collisionFaces.FirstOrDefault();
            if (collisionFace != null)
            {
                // Calculate the internal and external clips with the colliding the faces.
                IReadOnlyList<Coordinate> collisionShellPositions = otherPositions[collisionFace.Index];
                IReadOnlyList<List<Coordinate>> collisionHolesPositions =
                    collisionFace.Holes.Select(face => face.Vertices.Select(vertex => vertex.Position).ToList()).ToList();

                foreach (List<Coordinate> positions in collisionHolesPositions)
                {
                    positions.Add(positions.First());
                    positions.Reverse();
                }

                var algorithm = new GreinerHormannAlgorithm(collisionShellPositions, collisionHolesPositions, shell, holes, true, _precisionModel);
                algorithm.Compute();

                // Determine the tag of the collided and the parameter face.
                ISet<Int32> oldIds = collisionFace.Identifiers;
                ISet<Int32> newIds = identifiers ?? new HashSet<Int32>();

                // Remove the collided face from the graph, because the new clips will be added.
                switch (collisionFace.Type)
                {
                    case FaceType.Shell:
                        RemoveFace(collisionFace);
                        break;
                    case FaceType.Both:
                        collisionFace.Type = FaceType.Hole;
                        foreach (Face hole in collisionFace.Holes)
                            RemoveFace(hole);
                        collisionFace.Holes.Clear();
                        break;
                }

                // Internal clips (requiring re-process).
                foreach (IBasicPolygon polygon in algorithm.InternalPolygons)
                    result.AddRange(MergeFace(polygon.Shell, polygon.Holes, new HashSet<Int32>(oldIds.Union(newIds)), excluded));

                // External clips of the parameter face that are required to be re-processed.
                foreach (IBasicPolygon polygon in algorithm.ExternalPolygonsB)
                    result.AddRange(MergeFace(polygon.Shell, polygon.Holes, newIds, excluded));

                // External clips of the already existing topology graph.
                foreach (Face face in algorithm.ExternalPolygonsA.Select(polygon => (Face)AddFace(polygon)))
                {
                    foreach (Vertex vertex in face.Vertices)
                        vertex.Identifiers.UnionWith(oldIds);
                    result.Add(face);
                }

            }
            else
            {
                // If there were none colliding faces, the whole face can be added to the graph.
                Face face = (Face)AddFace(new BasicPolygon(shell, holes));
                if(identifiers != null)
                    foreach (Vertex vertex in face.Vertices)
                        vertex.Identifiers.UnionWith(identifiers);
                result.Add(face);
                excluded.Add(face);
            }
            return result;
        }

        #endregion

        #region DEBUG methods

        /// <summary>
        /// Prints debug information about this instance.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public void Debug()
        {
            System.Diagnostics.Debug.WriteLine("====================");
            System.Diagnostics.Debug.WriteLine("GRAPH DEBUG START");

            System.Diagnostics.Debug.WriteLine("--------------------");
            System.Diagnostics.Debug.WriteLine("Vertices count: {0}", _vertices.Count);
            foreach (var vertex in _vertices.Values)
            {
                System.Diagnostics.Debug.WriteLine("Vertex #{0}: {1}, halfedge #{2}", vertex.Index, vertex.Position,
                                                   vertex.Halfedge.Index);
            }

            System.Diagnostics.Debug.WriteLine("--------------------");
            System.Diagnostics.Debug.WriteLine("Halfedges count: {0}", _halfedges.Count);
            foreach (var halfedge in _halfedges)
            {
                System.Diagnostics.Debug.WriteLine("Halfedge #{0}: #{1}<-#{0}->#{2}, vertex #{3}->#{4}, face #{5}",
                                                   halfedge.Index, halfedge.Previous.Index, halfedge.Next.Index,
                                                   halfedge.FromVertex.Index, halfedge.ToVertex.Index,
                                                   halfedge.Face != null ? halfedge.Face.Index.ToString() : "null");
            }

            System.Diagnostics.Debug.WriteLine("--------------------");
            System.Diagnostics.Debug.WriteLine("Edges count: {0}", _edges.Count);
            foreach (var edge in _edges)
            {
                System.Diagnostics.Debug.WriteLine("Edge #{0}: halfedges #{1}<->#{2}", edge.Index, edge.HalfedgeA.Index,
                                                   edge.HalfedgeB.Index);
            }

            System.Diagnostics.Debug.WriteLine("--------------------");
            System.Diagnostics.Debug.WriteLine("Faces count: {0}", _faces.Count);
            foreach (var face in _faces)
            {
                System.Diagnostics.Debug.WriteLine("Face #{0}: halfedge #{1}", face.Index, face.Halfedge.Index);
            }
            System.Diagnostics.Debug.WriteLine("GRAPH DEBUG END");
            System.Diagnostics.Debug.WriteLine("====================");
        }

        /// <summary>
        /// Writes debug SVG file about this instance.
        /// </summary>
        /// <param name="filename">The output filename.</param>
        /// <param name="hasLabels">Sets whether to label incrementally the vertices.</param>
        /// <param name="imageSize">Size of the image.</param>
        /// <param name="vertexSize">Size of the radius of a vertex.</param>
        [System.Diagnostics.Conditional("DEBUG")]
        public void DebugSvg(String filename, 
            Boolean hasLabels = true, Double imageSize = 800d, Single vertexSize = 0.5f)
        {
            Double minX, minY, maxX, maxY;
            if (_vertices.Count > 0)
            {
                minX = _vertices.Values.Min(vertex => vertex.Position.X);
                minY = _vertices.Values.Min(vertex => vertex.Position.Y);

                maxX = _vertices.Values.Max(vertex => vertex.Position.X);
                maxY = _vertices.Values.Max(vertex => vertex.Position.Y);
            }
            else
                minX = minY = maxX = maxY = 0d;

            Double sizeX = maxX - minX;
            Double sizeY = maxY - minY;

            Double widthX = imageSize;
            Double widthY = imageSize;

            Double ratioX = widthX / sizeX;
            Double ratioY = widthY / sizeY;

            if (ratioX > ratioY)
                widthX = widthX / ratioX * ratioY;
            else if (ratioY > ratioX)
                widthY = widthY / ratioY * ratioX;

            var coordYTransform = new Func<Double, Double>(y => -y + 2 * minY + sizeY);

            XNamespace ns = "http://www.w3.org/2000/svg";
            XDocument doc = new XDocument(
                new XElement(ns + "svg",
                    new XAttribute("width", (Int32) widthX),
                    new XAttribute("height", (Int32) widthY),
                    new XAttribute("viewBox", String.Format(System.Globalization.CultureInfo.InvariantCulture,
                        "{0} {1} {2} {3}", minX - 1, minY - 1, sizeX + 2, sizeY + 2)),

                    _faces.Select(face => new XElement(ns + "polygon",
                        new XAttribute("points", face.Vertices
                                                     .Select(
                                                         vertex =>
                                                             String.Format(System.Globalization.CultureInfo.InvariantCulture,
                                                                 "{0},{1}",
                                                                 vertex.Position.X,
                                                                 coordYTransform(vertex.Position.Y)))
                                                     .Aggregate(
                                                         (a, b) =>
                                                             String.Format("{0} {1}", a, b))),
                        new XAttribute("style", String.Format("fill: {0}", face.Type.HasFlag(FaceType.Hole) ? "green" : "purple")))),

                    _vertices.Values.Select(vertex => new XElement(ns + "circle",
                        new XAttribute("cx", vertex.Position.X),
                        new XAttribute("cy", coordYTransform(vertex.Position.Y)),
                        new XAttribute("r", vertexSize),
                        new XAttribute("fill", vertex.OnBoundary ? "black" : "red"))),

                    _edges.Select(edge => new XElement(ns + "line",
                        new XAttribute("x1", edge.VertexA.Position.X),
                        new XAttribute("y1", coordYTransform(edge.VertexA.Position.Y)),
                        new XAttribute("x2", edge.VertexB.Position.X),
                        new XAttribute("y2", coordYTransform(edge.VertexB.Position.Y)),
                        new XAttribute("style",
                            String.Format(System.Globalization.CultureInfo.InvariantCulture,
                                "stroke: {0}; stroke-width: {1}",
                                edge.HalfedgeA.OnBoundary
                                    ? "green"
                                    : edge.HalfedgeB.OnBoundary
                                        ? "yellow"
                                        : "blue",
                                vertexSize))))
                    )
                );

            if (hasLabels)
                doc.Descendants(ns + "svg").First().Add(
                    _vertices.Values.Select(vertex => new XElement(ns + "text", vertex.Index,
                        new XAttribute("x", vertex.Position.X - vertexSize / 2),
                        new XAttribute("y", coordYTransform(vertex.Position.Y) + vertexSize / 4),
                        new XAttribute("fill", vertex.OnBoundary ? "red" : "black"),
                        new XAttribute("style", String.Format(System.Globalization.CultureInfo.InvariantCulture,
                            "font-size: {0}", vertexSize))))
                    );

            doc.AddFirst(new XDocumentType("svg", "-//W3//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null));
            doc.Save(new FileStream(filename, FileMode.OpenOrCreate));
        }

        #endregion
    }
}
