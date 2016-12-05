// <copyright file="GeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a factory for producing <see cref="Geometry" /> instances.
    /// </summary>
    /// <remarks>
    /// This implementation of <see cref="IGeometryFactory" /> produces geometries in coordinate space.
    /// </remarks>
    public class GeometryFactory : Factory, IGeometryFactory
    {
        /// <summary>
        /// Gets the precision model used by the factory.
        /// </summary>
        /// <value>The precision model used by the factory.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the reference system used by the factory.
        /// </summary>
        /// <value>The reference system used by the factory.</value>
        public IReferenceSystem ReferenceSystem { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryFactory" /> class.
        /// </summary>
        public GeometryFactory()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryFactory" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        public GeometryFactory(IReferenceSystem referenceSystem)
            : this(null, referenceSystem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryFactory" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        public GeometryFactory(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : base()
        {
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.ReferenceSystem = referenceSystem;
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The point with the specified X, Y coordinates.</returns>
        public virtual IPoint CreatePoint(Double x, Double y)
        {
            return new Point(this, x, y, 0);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The point with the specified X, Y, Z coordinates.</returns>
        public virtual IPoint CreatePoint(Double x, Double y, Double z)
        {
            return new Point(this, x, y, z);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <returns>The point with the specified coordinate.</returns>
        public virtual IPoint CreatePoint(Coordinate coordinate)
        {
            return new Point(this, coordinate.X, coordinate.Y, coordinate.Z);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>A point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other point is null.</exception>
        public virtual IPoint CreatePoint(IPoint other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherPointIsNull);

            return new Point(this, other.X, other.Y, other.Z);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <returns>An empty line string.</returns>
        public virtual ILineString CreateLineString()
        {
            return new LineString(this);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        public virtual ILineString CreateLineString(params Coordinate[] source)
        {
            return new LineString(this, source);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        public virtual ILineString CreateLineString(params IPoint[] source)
        {
            if (source == null)
                return new LineString(this, null);
            else
                return new LineString(this, source.Select(point => point.Coordinate));
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        public virtual ILineString CreateLineString(IEnumerable<Coordinate> source)
        {
            return new LineString(this, source);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        public virtual ILineString CreateLineString(IEnumerable<IPoint> source)
        {
            if (source == null)
                return new LineString(this);
            else
                return new LineString(this, source.Select(point => point.Coordinate));
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <returns>A line string that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other line string is null.</exception>
        public virtual ILineString CreateLineString(ILineString other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherLineStringIsNull);

            return new LineString(this, other);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <returns>A line containing the specified coordinates.</returns>
        public virtual ILine CreateLine(Coordinate start, Coordinate end)
        {
            return new Line(this, start, end);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting point.</param>
        /// <param name="end">The ending point.</param>
        /// <returns>A line containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The start point is null.
        /// or
        /// The end point is null.
        /// </exception>
        public virtual ILine CreateLine(IPoint start, IPoint end)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start), CoreMessages.StartPointIsNull);
            if (end == null)
                throw new ArgumentNullException(nameof(end), CoreMessages.EndPointIsNull);

            return new Line(this, start.Coordinate, end.Coordinate);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>A line that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        public virtual ILine CreateLine(ILine other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherLineIsNull);

            return new Line(this, other.StartCoordinate, other.EndCoordinate);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <returns>An empty linear ring.</returns>
        public virtual ILinearRing CreateLinearRing()
        {
            return new LinearRing(this);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public virtual ILinearRing CreateLinearRing(params Coordinate[] source)
        {
            return new LinearRing(this, source);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public virtual ILinearRing CreateLinearRing(params IPoint[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            return new LinearRing(this, source.Select(point => point.Coordinate));
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public virtual ILinearRing CreateLinearRing(IEnumerable<Coordinate> source)
        {
            return new LinearRing(this, source);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public virtual ILinearRing CreateLinearRing(IEnumerable<IPoint> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            return new LinearRing(this, source.Select(point => point.Coordinate));
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <returns>A linear ring that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other linear ring is null.</exception>
        public virtual ILinearRing CreateLinearRing(ILinearRing other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherLinearRingIsNull);

            return new LinearRing(this, other);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public virtual IPolygon CreatePolygon(params Coordinate[] shell)
        {
            return new Polygon(this, shell, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The points of the shell.</param>
        /// <returns>A polygon containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public virtual IPolygon CreatePolygon(params IPoint[] shell)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            return new Polygon(this, shell.Select(point => point.Coordinate), null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public virtual IPolygon CreatePolygon(IEnumerable<Coordinate> shell)
        {
            return new Polygon(this, shell, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public virtual IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            return new Polygon(this, shell, holes);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        public virtual IPolygon CreatePolygon(IPolygon other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherPolygonIsNull);

            return new Polygon(this, other.Shell, other.Holes);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <returns>The triangle containing the specified coordinates.</returns>
        public virtual ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third)
        {
            return new Triangle(this, first, second, third);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="third">The third point.</param>
        /// <returns>The triangle containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first point is null.
        /// or
        /// The second point is null.
        /// or
        /// The third point is null.
        /// </exception>
        public virtual ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstPointIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondPointIsNull);
            if (third == null)
                throw new ArgumentNullException(nameof(third), CoreMessages.ThirdPointIsNull);

            return new Triangle(this, first.Coordinate, second.Coordinate, third.Coordinate);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <returns>A triangle that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        public virtual ITriangle CreateTriangle(ITriangle other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherTriangleIsNull);

            return new Triangle(this, other.Shell[0], other.Shell[1], other.Shell[2]);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <returns>The empty geometry collection.</returns>
        public virtual IGeometryCollection CreateGeometryCollection()
        {
            return new GeometryList<IGeometry>(this);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        public virtual IGeometryCollection CreateGeometryCollection(IEnumerable<IGeometry> geometries)
        {
            return new GeometryList<IGeometry>(this, geometries);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public virtual IGeometryCollection CreateGeometryCollection(IGeometryCollection other)
        {
            return new GeometryList<IGeometry>(this, other);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <returns>The empty geometry collection.</returns>
        public virtual IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>()
            where GeometryType : IGeometry
        {
            return new GeometryList<GeometryType>(this);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="geometries">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        public virtual IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IEnumerable<GeometryType> geometries)
            where GeometryType : IGeometry
        {
            return new GeometryList<GeometryType>(this, geometries);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public virtual IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IGeometryCollection<GeometryType> other)
            where GeometryType : IGeometry
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherGeometryCollectionIsNull);

            return new GeometryList<GeometryType>(this, (IEnumerable<GeometryType>)other);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <returns>The empty multi point.</returns>
        public virtual IMultiPoint CreateMultiPoint()
        {
            return new MultiPoint(this);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="coordinates">The source coordinates.</param>
        /// <returns>The multi point containing the specified coordinates.</returns>
        public virtual IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> coordinates)
        {
            return new MultiPoint(this, coordinates.Select(coordinate => this.CreatePoint(coordinate.X, coordinate.Y, coordinate.Z)));
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <returns>The multi point containing the specified points.</returns>
        public virtual IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points)
        {
            return new MultiPoint(this, points);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <returns>A multi point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        public virtual IMultiPoint CreateMultiPoint(IMultiPoint other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiPointIsNull);

            return new MultiPoint(this, other);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <returns>The empty multi line string.</returns>
        public virtual IMultiLineString CreateMultiLineString()
        {
            return new MultiLineString(this);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="lineStrings">The source line strings.</param>
        /// <returns>The multi line string containing the specified line strings.</returns>
        public virtual IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings)
        {
            return new MultiLineString(this, lineStrings);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <returns>A multi line string that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        public virtual IMultiLineString CreateMultiLineString(IMultiLineString other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiLineStringIsNull);

            return new MultiLineString(this, other);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <returns>The empty multi polygon.</returns>
        public virtual IMultiPolygon CreateMultiPolygon()
        {
            return new MultiPolygon(this);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <returns>The multi polygon containing the specified polygons.</returns>
        public virtual IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons)
        {
            return new MultiPolygon(this, polygons);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <returns>A multi polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        public virtual IMultiPolygon CreateMultiPolygon(IMultiPolygon other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiPolygonIsNull);

            return new MultiPolygon(this, other);
        }

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <returns>The produced geometry matching <see cref="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the geometry is not supported.</exception>
        public IGeometry CreateGeometry(IGeometry other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherGeometryIsNull);

            if (other is IPoint)
                return this.CreatePoint(other as IPoint);
            if (other is ILine)
                return this.CreateLine(other as ILine);
            if (other is ILinearRing)
                return this.CreateLinearRing(other as ILinearRing);
            if (other is ILineString)
                return this.CreateLineString(other as ILineString);
            if (other is ITriangle)
                return this.CreateTriangle(other as ITriangle);
            if (other is IPolygon)
                return this.CreatePolygon(other as IPolygon);
            if (other is IMultiPoint)
                return this.CreateMultiPoint(other as IMultiPoint);
            if (other is IMultiLineString)
                return this.CreateMultiLineString(other as IMultiLineString);
            if (other is IMultiPolygon)
                return this.CreateMultiPolygon(other as IMultiPolygon);
            if (other is IGeometryCollection<IGeometry>)
                return this.CreateGeometryCollection(other as IGeometryCollection<IGeometry>);

            throw new ArgumentException(CoreMessages.GeometryTypeNotSupported, nameof(other));
        }
    }
}
