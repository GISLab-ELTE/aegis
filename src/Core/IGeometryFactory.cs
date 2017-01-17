// <copyright file="IGeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for factories producing <see cref="IGeometry" /> instances.
    /// </summary>
    public interface IGeometryFactory : IFactory
    {
        /// <summary>
        /// Gets the precision model used by the factory.
        /// </summary>
        /// <value>The precision model used by the factory.</value>
        PrecisionModel PrecisionModel { get; }

        /// <summary>
        /// Gets the reference system.
        /// </summary>
        /// <value>The reference system.</value>
        IReferenceSystem ReferenceSystem { get; }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The point with the specified X, Y coordinates.</returns>
        IPoint CreatePoint(Double x, Double y);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The point with the specified X, Y, Z coordinates.</returns>
        IPoint CreatePoint(Double x, Double y, Double z);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <returns>The point with the specified coordinate.</returns>
        IPoint CreatePoint(Coordinate coordinate);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>A point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other point is null.</exception>
        IPoint CreatePoint(IPoint other);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <returns>An empty line string.</returns>
        ILineString CreateLineString();

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        ILineString CreateLineString(params Coordinate[] source);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        ILineString CreateLineString(params IPoint[] source);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        ILineString CreateLineString(IEnumerable<Coordinate> source);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        ILineString CreateLineString(IEnumerable<IPoint> source);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <returns>A line string that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other line string is null.</exception>
        ILineString CreateLineString(ILineString other);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <returns>A line containing the specified coordinates.</returns>
        ILine CreateLine(Coordinate start, Coordinate end);

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
        ILine CreateLine(IPoint start, IPoint end);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>A line that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        ILine CreateLine(ILine other);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <returns>An empty linear ring.</returns>
        ILinearRing CreateLinearRing();

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        ILinearRing CreateLinearRing(params Coordinate[] source);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        ILinearRing CreateLinearRing(params IPoint[] source);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        ILinearRing CreateLinearRing(IEnumerable<Coordinate> source);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        ILinearRing CreateLinearRing(IEnumerable<IPoint> source);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <returns>A linear ring that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other linear ring is null.</exception>
        ILinearRing CreateLinearRing(ILinearRing other);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        IPolygon CreatePolygon(params Coordinate[] shell);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The points of the shell.</param>
        /// <returns>A polygon containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        IPolygon CreatePolygon(params IPoint[] shell);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        IPolygon CreatePolygon(IEnumerable<Coordinate> shell);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        IPolygon CreatePolygon(IPolygon other);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <returns>The triangle containing the specified coordinates.</returns>
        ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third);

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
        ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <returns>A triangle that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        ITriangle CreateTriangle(ITriangle other);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <returns>The empty geometry collection.</returns>
        IGeometryCollection CreateGeometryCollection();

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="geometries">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        IGeometryCollection CreateGeometryCollection(IEnumerable<IGeometry> geometries);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        IGeometryCollection CreateGeometryCollection(IGeometryCollection other);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <returns>The empty geometry collection.</returns>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>()
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="geometries">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IEnumerable<GeometryType> geometries)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IGeometryCollection<GeometryType> other)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <returns>The empty multi point.</returns>
        IMultiPoint CreateMultiPoint();

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source coordinates.</param>
        /// <returns>The multi point containing the specified points.</returns>
        IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> points);

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="points">The source points.</param>
        /// <returns>The multi point containing the specified points.</returns>
        IMultiPoint CreateMultiPoint(IEnumerable<IPoint> points);

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <returns>A multi point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        IMultiPoint CreateMultiPoint(IMultiPoint other);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <returns>The empty multi line string.</returns>
        IMultiLineString CreateMultiLineString();

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="lineStrings">The source line strings.</param>
        /// <returns>The multi line string containing the specified line strings.</returns>
        IMultiLineString CreateMultiLineString(IEnumerable<ILineString> lineStrings);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <returns>A multi line string that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        IMultiLineString CreateMultiLineString(IMultiLineString other);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <returns>The empty multi polygon.</returns>
        IMultiPolygon CreateMultiPolygon();

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="polygons">The source polygons.</param>
        /// <returns>The multi polygon containing the specified polygons.</returns>
        IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> polygons);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <returns>A multi polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        IMultiPolygon CreateMultiPolygon(IMultiPolygon other);

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <returns>The produced geometry matching <see cref="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the geometry is not supported.</exception>
        IGeometry CreateGeometry(IGeometry other);

        /// <summary>
        /// Returns a geometry factory with the specified precision model.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>A geometry factory with the specified precision model.</returns>
        IGeometryFactory WithPrecisionModel(PrecisionModel precisionModel);

        /// <summary>
        /// Returns a geometry factory with the specified reference system.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A geometry factory with the specified reference system.</returns>
        IGeometryFactory WithReferenceSystem(IReferenceSystem referenceSystem);
    }
}
