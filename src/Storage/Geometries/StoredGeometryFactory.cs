// <copyright file="StoredGeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Collections;
    using ELTE.AEGIS.Resources;
    using ELTE.AEGIS.Storage.Resources;

    /// <summary>
    /// Represents a factory producing <see cref="IGeometry" /> instances located in stores.
    /// </summary>
    public class StoredGeometryFactory : Factory, IStoredGeometryFactory
    {
        /// <summary>
        /// The internal reference system.
        /// </summary>
        private IReferenceSystem referenceSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometryFactory" /> class.
        /// </summary>
        /// <param name="driver">The geometry driver.</param>
        /// <exception cref="System.ArgumentNullException">The driver is null.</exception>
        public StoredGeometryFactory(IGeometryDriver driver)
            : this(driver, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometryFactory" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <exception cref="System.ArgumentNullException">The driver is null.</exception>
        public StoredGeometryFactory(IGeometryDriver driver, PrecisionModel precisionModel)
            : this(driver, precisionModel, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometryFactory" /> class.
        /// </summary>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <exception cref="System.ArgumentNullException">The driver is null.</exception>
        public StoredGeometryFactory(IGeometryDriver driver, PrecisionModel precisionModel, IReferenceSystem referenceSystem)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver), StorageMessages.DriverIsNull);

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.Driver = driver;
            this.referenceSystem = referenceSystem;
        }

        /// <summary>
        /// Gets the precision model used by the factory.
        /// </summary>
        /// <value>The precision model used by the factory.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the reference system used by the factory.
        /// </summary>
        /// <value>The reference system used by the factory.</value>
        public IReferenceSystem ReferenceSystem
        {
            get
            {
                if (this.referenceSystem != null)
                    return this.referenceSystem;
                return this.Driver.ReferenceSystemDriver.ReadReferenceSystem();
            }
        }

        /// <summary>
        /// Gets the geometry driver of the factory.
        /// </summary>
        /// <value>The geometry driver of the factory.</value>
        public IGeometryDriver Driver { get; private set; }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The point with the specified X, Y coordinates.</returns>
        public IPoint CreatePoint(Double x, Double y)
        {
            StoredPoint point = new StoredPoint(this, this.Driver.CreateIdentifier(), null);
            point.Coordinate = new Coordinate(x, y);

            return point;
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <returns>The point with the specified X, Y, Z coordinates.</returns>
        public IPoint CreatePoint(Double x, Double y, Double z)
        {
            StoredPoint point = new StoredPoint(this, this.Driver.CreateIdentifier(), null);
            point.Coordinate = new Coordinate(x, y, z);

            return point;
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <returns>The point with the specified coordinate.</returns>
        public IPoint CreatePoint(Coordinate coordinate)
        {
            StoredPoint point = new StoredPoint(this, this.Driver.CreateIdentifier(), null);
            point.Coordinate = coordinate;

            return point;
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>A point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentException">The other point is null.</exception>
        public IPoint CreatePoint(IPoint other)
        {
            return this.CreatePoint(other, null);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <returns>An empty line string.</returns>
        public ILineString CreateLineString()
        {
            return new StoredLineString(this, this.Driver.CreateIdentifier(), null);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILineString CreateLineString(params Coordinate[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLineString lineString = new StoredLineString(this, this.Driver.CreateIdentifier(), null);
            if (source != null)
                lineString.Add(source);

            return lineString;
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILineString CreateLineString(params IPoint[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLineString lineString = new StoredLineString(this, this.Driver.CreateIdentifier(), null);
            if (source != null)
                lineString.Add(source.Elements().Select(point => point.Coordinate));

            return lineString;
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A line string containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILineString CreateLineString(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLineString lineString = new StoredLineString(this, this.Driver.CreateIdentifier(), null);
            if (source != null)
                lineString.Add(source);

            return lineString;
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A line string containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILineString CreateLineString(IEnumerable<IPoint> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLineString lineString = new StoredLineString(this, this.Driver.CreateIdentifier(), null);
            if (source != null)
                lineString.Add(source.Where(point => point != null).Select(point => point.Coordinate));

            return lineString;
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <returns>A line string that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentException">The other line string is null.</exception>
        public ILineString CreateLineString(ILineString other)
        {
            return this.CreateLineString(other, null);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="start">The starting coordinate.</param>
        /// <param name="end">The ending coordinate.</param>
        /// <returns>A line containing the specified coordinates.</returns>
        public ILine CreateLine(Coordinate start, Coordinate end)
        {
            StoredLine line = new StoredLine(this, this.Driver.CreateIdentifier(), null);
            line.SetCoordinate(0, start);
            line.SetCoordinate(1, end);

            return line;
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
        public ILine CreateLine(IPoint start, IPoint end)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start), CoreMessages.StartPointIsNull);
            if (end == null)
                throw new ArgumentNullException(nameof(end), CoreMessages.EndPointIsNull);

            StoredLine line = new StoredLine(this, this.Driver.CreateIdentifier(), null);
            line.SetCoordinate(0, start.Coordinate);
            line.SetCoordinate(1, end.Coordinate);

            return line;
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <returns>A line that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        public ILine CreateLine(ILine other)
        {
            return this.CreateLine(other, null);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <returns>An empty linear ring.</returns>
        public ILinearRing CreateLinearRing()
        {
            return new StoredLinearRing(this, this.Driver.CreateIdentifier(), null);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILinearRing CreateLinearRing(params Coordinate[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLinearRing ring = new StoredLinearRing(this, this.Driver.CreateIdentifier(), null);
            ring.Add(source);

            return ring;
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILinearRing CreateLinearRing(params IPoint[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLinearRing ring = new StoredLinearRing(this, this.Driver.CreateIdentifier(), null);
            ring.Add(source.Where(point => point != null).Select(point => point.Coordinate));

            return ring;
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>A linear ring containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILinearRing CreateLinearRing(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLinearRing ring = new StoredLinearRing(this, this.Driver.CreateIdentifier(), null);
            ring.Add(source);

            return ring;
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>A linear ring containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public ILinearRing CreateLinearRing(IEnumerable<IPoint> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredLinearRing ring = new StoredLinearRing(this, this.Driver.CreateIdentifier(), null);
            ring.Add(source.Where(point => point != null).Select(point => point.Coordinate));

            return ring;
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <returns>A linear ring that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other linear ring is null.</exception>
        public ILinearRing CreateLinearRing(ILinearRing other)
        {
            return this.CreateLinearRing(other, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <returns>An empty polygon.</returns>
        public virtual IPolygon CreatePolygon()
        {
            return new StoredPolygon(this, this.Driver.CreateIdentifier(), null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public IPolygon CreatePolygon(params Coordinate[] shell)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            StoredPolygon polygon = new StoredPolygon(this, this.Driver.CreateIdentifier(), null);
            (polygon.Shell as StoredLineString).Add(shell);

            return polygon;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public IPolygon CreatePolygon(params IPoint[] shell)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            StoredPolygon polygon = new StoredPolygon(this, this.Driver.CreateIdentifier(), null);
            (polygon.Shell as StoredLineString).Add(shell.Where(point => point != null).Select(point => point.Coordinate));

            return polygon;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public IPolygon CreatePolygon(IEnumerable<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            StoredPolygon polygon = new StoredPolygon(this, this.Driver.CreateIdentifier(), null);
            (polygon.Shell as StoredLinearRing).Add(shell);

            return polygon;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public IPolygon CreatePolygon(IEnumerable<IPoint> shell)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            StoredPolygon polygon = new StoredPolygon(this, this.Driver.CreateIdentifier(), null);
            (polygon.Shell as StoredLinearRing).Add(shell.Elements().Select(point => point.Coordinate));

            return polygon;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public IPolygon CreatePolygon(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            StoredPolygon polygon = new StoredPolygon(this, this.Driver.CreateIdentifier(), null);
            (polygon.Shell as StoredLinearRing).Add(shell);

            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in holes)
                {
                    if (hole != null)
                        polygon.AddHole(hole);
                }
            }

            return polygon;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <returns>A polygon containing the specified coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public IPolygon CreatePolygon(IEnumerable<IPoint> shell, IEnumerable<IEnumerable<IPoint>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            StoredPolygon polygon = new StoredPolygon(this, this.Driver.CreateIdentifier(), null);
            (polygon.Shell as StoredLinearRing).Add(shell.Elements().Select(point => point.Coordinate));

            if (holes != null)
            {
                foreach (IEnumerable<IPoint> hole in holes)
                {
                    if (hole != null)
                        polygon.AddHole(hole.Where(point => point != null).Select(point => point.Coordinate));
                }
            }

            return polygon;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <returns>A polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        public IPolygon CreatePolygon(IPolygon other)
        {
            return this.CreatePolygon(other, null);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <returns>The triangle containing the specified coordinates.</returns>
        public ITriangle CreateTriangle(Coordinate first, Coordinate second, Coordinate third)
        {
            StoredTriangle triangle = new StoredTriangle(this, this.Driver.CreateIdentifier(), null);
            triangle.Shell.Add(first);
            triangle.Shell.Add(second);
            triangle.Shell.Add(third);

            return triangle;
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
        public ITriangle CreateTriangle(IPoint first, IPoint second, IPoint third)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstPointIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondPointIsNull);
            if (third == null)
                throw new ArgumentNullException(nameof(third), CoreMessages.ThirdPointIsNull);

            StoredTriangle triangle = new StoredTriangle(this, this.Driver.CreateIdentifier(), null);
            triangle.Shell.Add(first.Coordinate);
            triangle.Shell.Add(second.Coordinate);
            triangle.Shell.Add(third.Coordinate);

            return triangle;
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <returns>A triangle that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        public ITriangle CreateTriangle(ITriangle other)
        {
            return this.CreateTriangle(other, null);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <returns>The empty geometry collection.</returns>
        public IGeometryCollection CreateGeometryCollection()
        {
            return this.CreateGeometryCollection();
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="source">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IGeometryCollection CreateGeometryCollection(IEnumerable<IGeometry> source)
        {
            return this.CreateGeometryCollection(source);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public IGeometryCollection CreateGeometryCollection(IGeometryCollection other)
        {
            return this.CreateGeometryCollection(other);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <returns>The empty geometry collection.</returns>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>()
            where GeometryType : IGeometry
        {
            return new StoredGeometryCollection<GeometryType>(this, this.Driver.CreateIdentifier(), null);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="source">The source geometries.</param>
        /// <returns>The geometry collection containing the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IEnumerable<GeometryType> source)
            where GeometryType : IGeometry
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredGeometryCollection<GeometryType> collection = new StoredGeometryCollection<GeometryType>(this, this.Driver.CreateIdentifier(), null);

            Int32 index = 0;
            foreach (GeometryType geometry in source)
            {
                this.CreateGeometry(geometry, index);
                index++;
            }

            return collection;
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>A geometry collection that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IGeometryCollection<GeometryType> other)
            where GeometryType : IGeometry
        {
            return this.CreateGeometryCollection<GeometryType>(other, null);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <returns>The empty multi point.</returns>
        public IMultiPoint CreateMultiPoint()
        {
            return new StoredMultiPoint(this, this.Driver.CreateIdentifier(), null);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <returns>The multi point containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IMultiPoint CreateMultiPoint(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredMultiPoint multiPoint = new StoredMultiPoint(this, this.Driver.CreateIdentifier(), null);

            Int32 index = 0;
            foreach (Coordinate coordinate in source)
            {
                this.CreatePoint(coordinate, index);
                index++;
            }

            return multiPoint;
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="source">The source points.</param>
        /// <returns>The multi point containing the specified points.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IMultiPoint CreateMultiPoint(IEnumerable<IPoint> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredMultiPoint multiPoint = new StoredMultiPoint(this, this.Driver.CreateIdentifier(), null);

            Int32 index = 0;
            foreach (IPoint point in source)
            {
                this.CreatePoint(point, index);
                index++;
            }

            return multiPoint;
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <returns>A multi point that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        public IMultiPoint CreateMultiPoint(IMultiPoint other)
        {
            return this.CreateMultiPoint(other, null);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <returns>The empty multi line string.</returns>
        public IMultiLineString CreateMultiLineString()
        {
            return new StoredMultiLineString(this, this.Driver.CreateIdentifier(), null);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="source">The source line strings.</param>
        /// <returns>The multi line string containing the specified line strings.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IMultiLineString CreateMultiLineString(IEnumerable<ILineString> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredMultiLineString multiLineString = new StoredMultiLineString(this, this.Driver.CreateIdentifier(), null);

            Int32 index = 0;
            foreach (ILineString lineString in source)
            {
                this.CreateLineString(lineString, index);
                index++;
            }

            return multiLineString;
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <returns>A multi line string that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        public IMultiLineString CreateMultiLineString(IMultiLineString other)
        {
            return this.CreateMultiLineString(other, null);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <returns>The empty multi polygon.</returns>
        public IMultiPolygon CreateMultiPolygon()
        {
            return new StoredMultiPolygon(this, this.Driver.CreateIdentifier(), null);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="source">The source polygons.</param>
        /// <returns>The multi polygon containing the specified polygons.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public IMultiPolygon CreateMultiPolygon(IEnumerable<IPolygon> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            StoredMultiPolygon multiPolygon = new StoredMultiPolygon(this, this.Driver.CreateIdentifier(), null);

            Int32 index = 0;
            foreach (IPolygon polygon in source)
            {
                this.CreatePolygon(polygon, index);
                index++;
            }

            return multiPolygon;
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <returns>A multi polygon that matches <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        public IMultiPolygon CreateMultiPolygon(IMultiPolygon other)
        {
            return this.CreateMultiPolygon(other, null);
        }

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        public IGeometry CreateGeometry(IGeometry other)
        {
            return this.CreateGeometry(other, null);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IPoint CreatePoint(String identifier)
        {
            return this.CreatePoint(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IPoint CreatePoint(String identifier, params Int32[] indexes)
        {
            return this.CreatePoint(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IPoint CreatePoint(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredPoint(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other point.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other point is null.
        /// </exception>
        public IPoint CreatePoint(String identifier, IPoint other)
        {
            return this.CreatePoint(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other point is null.
        /// </exception>
        public IPoint CreatePoint(String identifier, IPoint other, params Int32[] indexes)
        {
            return this.CreatePoint(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other point is null.
        /// </exception>
        public IPoint CreatePoint(String identifier, IPoint other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherPointIsNull);

            StoredPoint point = new StoredPoint(this, identifier, indexes);
            point.Coordinate = other.Coordinate;

            return point;
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        public IPoint CreatePoint(Coordinate coordinate, params Int32[] indexes)
        {
            return this.CreatePoint(coordinate, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        public IPoint CreatePoint(Coordinate coordinate, IEnumerable<Int32> indexes)
        {
            StoredPoint point = new StoredPoint(this, this.Driver.CreateIdentifier(), indexes);
            point.Coordinate = coordinate;

            return point;
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other point is null.</exception>
        public IPoint CreatePoint(IPoint other, params Int32[] indexes)
        {
            return this.CreatePoint(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other point is null.</exception>
        public IPoint CreatePoint(IPoint other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherPointIsNull);

            StoredPoint point;
            if (other is IStoredGeometry)
            {
                point = new StoredPoint(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                point = new StoredPoint(this, this.Driver.CreateIdentifier(), indexes);
                point.Coordinate = other.Coordinate;
            }

            return point;
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILineString CreateLineString(String identifier)
        {
            return this.CreateLineString(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILineString CreateLineString(String identifier, params Int32[] indexes)
        {
            return this.CreateLineString(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILineString CreateLineString(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredLineString(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other line string.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other line string is null.
        /// </exception>
        public ILineString CreateLineString(String identifier, ILineString other)
        {
            return this.CreateLineString(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other line string is null.
        /// </exception>
        public ILineString CreateLineString(String identifier, ILineString other, params Int32[] indexes)
        {
            return this.CreateLineString(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other line string is null.
        /// </exception>
        public ILineString CreateLineString(String identifier, ILineString other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentException(nameof(other), CoreMessages.OtherLineStringIsNull);

            StoredLineString lineString = new StoredLineString(this, identifier, indexes);
            lineString.Add(other);

            return lineString;
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILineString CreateLineString(ILineString other, params Int32[] indexes)
        {
            return this.CreateLineString(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other line string is null.</exception>
        public ILineString CreateLineString(ILineString other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentException(nameof(other), CoreMessages.OtherLineStringIsNull);

            StoredLineString lineString;
            if (other is IStoredGeometry)
            {
                lineString = new StoredLineString(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                lineString = new StoredLineString(this, this.Driver.CreateIdentifier(), indexes);
                lineString.Add(other);
            }

            return lineString;
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILine CreateLine(String identifier)
        {
            return this.CreateLine(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILine CreateLine(String identifier, params Int32[] indexes)
        {
            return this.CreateLine(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILine CreateLine(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredLine(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other line.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILine CreateLine(String identifier, ILine other)
        {
            return this.CreateLine(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other line.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other line is null.
        /// </exception>
        public ILine CreateLine(String identifier, ILine other, params Int32[] indexes)
        {
            return this.CreateLine(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other line.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other line is null.
        /// </exception>
        public ILine CreateLine(String identifier, ILine other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherLineIsNull);

            StoredLine line = new StoredLine(this, identifier, indexes);
            line.SetCoordinate(0, other.StartCoordinate);
            line.SetCoordinate(1, other.EndCoordinate);

            return line;
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        public ILine CreateLine(ILine other, params Int32[] indexes)
        {
            return this.CreateLine(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        public ILine CreateLine(ILine other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherLineIsNull);

            StoredLine line;
            if (other is IStoredGeometry)
            {
                line = new StoredLine(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                line = new StoredLine(this, this.Driver.CreateIdentifier(), indexes);
                line.SetCoordinate(0, other.StartCoordinate);
                line.SetCoordinate(1, other.EndCoordinate);
            }

            return line;
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILinearRing CreateLinearRing(String identifier)
        {
            return this.CreateLinearRing(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILinearRing CreateLinearRing(String identifier, params Int32[] indexes)
        {
            return this.CreateLinearRing(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILinearRing CreateLinearRing(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredLinearRing(this, identifier, null);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other linear ring.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ILinearRing CreateLinearRing(String identifier, ILinearRing other)
        {
            return this.CreateLinearRing(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other linear ring.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other linear ring is null.
        /// </exception>
        public ILinearRing CreateLinearRing(String identifier, ILinearRing other, params Int32[] indexes)
        {
            return this.CreateLinearRing(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other linear ring.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other linear ring is null.
        /// </exception>
        public ILinearRing CreateLinearRing(String identifier, ILinearRing other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherLinearRingIsNull);

            StoredLinearRing linearRing = new StoredLinearRing(this, identifier, indexes);
            linearRing.Add(other);

            return linearRing;
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other linear ring is null.</exception>
        public ILinearRing CreateLinearRing(ILinearRing other, params Int32[] indexes)
        {
            return this.CreateLinearRing(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The linear ring is null.</exception>
        public ILinearRing CreateLinearRing(ILinearRing other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherLinearRingIsNull);

            StoredLinearRing linearRing;
            if (other is IStoredGeometry)
            {
                linearRing = new StoredLinearRing(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                linearRing = new StoredLinearRing(this, this.Driver.CreateIdentifier(), indexes);
                linearRing.Add(other);
            }

            return linearRing;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IPolygon CreatePolygon(String identifier)
        {
            return this.CreatePolygon(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IPolygon CreatePolygon(String identifier, params Int32[] indexes)
        {
            return this.CreatePolygon(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IPolygon CreatePolygon(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredPolygon(this, identifier, null);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other polygon.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IPolygon CreatePolygon(String identifier, IPolygon other)
        {
            return this.CreatePolygon(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other polygon is null.
        /// </exception>
        public IPolygon CreatePolygon(String identifier, IPolygon other, params Int32[] indexes)
        {
            return this.CreatePolygon(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other polygon is null.
        /// </exception>
        public IPolygon CreatePolygon(String identifier, IPolygon other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherPolygonIsNull);

            StoredPolygon polygon = new StoredPolygon(this, identifier, indexes);
            (polygon.Shell as StoredLinearRing).Add(other.Shell);

            return other;
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        public IPolygon CreatePolygon(IPolygon other, params Int32[] indexes)
        {
            return this.CreatePolygon(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        public IPolygon CreatePolygon(IPolygon other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherPolygonIsNull);

            StoredPolygon polygon;
            if (other is IStoredGeometry)
            {
                polygon = new StoredPolygon(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                polygon = new StoredPolygon(this, this.Driver.CreateIdentifier(), indexes);
                (polygon.Shell as StoredLinearRing).Add(other.Shell);

                foreach (ILinearRing hole in other.Holes)
                {
                    if (hole != null)
                        polygon.AddHole(hole);
                }
            }

            return polygon;
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ITriangle CreateTriangle(String identifier)
        {
            return this.CreateTriangle(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ITriangle CreateTriangle(String identifier, params Int32[] indexes)
        {
            return this.CreateTriangle(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ITriangle CreateTriangle(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredTriangle(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other triangle.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public ITriangle CreateTriangle(String identifier, ITriangle other)
        {
            return this.CreateTriangle(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other triangle.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other triangle is null.
        /// </exception>
        public ITriangle CreateTriangle(String identifier, ITriangle other, params Int32[] indexes)
        {
            return this.CreateTriangle(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other triangle.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other triangle is null.
        /// </exception>
        public ITriangle CreateTriangle(String identifier, ITriangle other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherTriangleIsNull);

            StoredTriangle triangle = new StoredTriangle(this, identifier, indexes);

            IReadOnlyList<Coordinate> coordinates = other.Shell;
            triangle.Shell.Add(coordinates.Count > 0 ? coordinates[0] : Coordinate.Undefined);
            triangle.Shell.Add(coordinates.Count > 1 ? coordinates[1] : Coordinate.Undefined);
            triangle.Shell.Add(coordinates.Count > 2 ? coordinates[2] : Coordinate.Undefined);

            return triangle;
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        public ITriangle CreateTriangle(ITriangle other, params Int32[] indexes)
        {
            return this.CreateTriangle(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        public ITriangle CreateTriangle(ITriangle other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherTriangleIsNull);

            IReadOnlyList<Coordinate> coordinates = other.Shell;

            StoredTriangle triangle;
            if (other is IStoredGeometry)
            {
                triangle = new StoredTriangle(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                triangle = new StoredTriangle(this, this.Driver.CreateIdentifier(), indexes);
                triangle.Shell.Add(coordinates.Count > 0 ? coordinates[0] : Coordinate.Undefined);
                triangle.Shell.Add(coordinates.Count > 1 ? coordinates[1] : Coordinate.Undefined);
                triangle.Shell.Add(coordinates.Count > 2 ? coordinates[2] : Coordinate.Undefined);
            }

            return triangle;
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IGeometryCollection CreateGeometryCollection(String identifier)
        {
            return this.CreateGeometryCollection(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IGeometryCollection CreateGeometryCollection(String identifier, params Int32[] indexes)
        {
            return this.CreateGeometryCollection(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IGeometryCollection CreateGeometryCollection(String identifier, IEnumerable<Int32> indexes)
        {
            return this.CreateGeometryCollection(identifier, indexes);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IGeometryCollection CreateGeometryCollection(String identifier, IGeometryCollection other)
        {
            return this.CreateGeometryCollection(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other geometry collection is null.
        /// </exception>
        public IGeometryCollection CreateGeometryCollection(String identifier, IGeometryCollection other, params Int32[] indexes)
        {
            return this.CreateGeometryCollection(identifier, other, indexes);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other geometry collection is null.
        /// </exception>
        public IGeometryCollection CreateGeometryCollection(String identifier, IGeometryCollection other, IEnumerable<Int32> indexes)
        {
            return this.CreateGeometryCollection(identifier, other, indexes);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public IGeometryCollection CreateGeometryCollection(IGeometryCollection other, params Int32[] indexes)
        {
            return this.CreateGeometryCollection(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public IGeometryCollection CreateGeometryCollection(IGeometryCollection other, IEnumerable<Int32> indexes)
        {
            return this.CreateGeometryCollection(other, indexes);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier)
            where GeometryType : IGeometry
        {
            return this.CreateGeometryCollection<GeometryType>(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, params Int32[] indexes)
            where GeometryType : IGeometry
        {
            return this.CreateGeometryCollection<GeometryType>(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IEnumerable<Int32> indexes)
            where GeometryType : IGeometry
        {
            return new StoredGeometryCollection<GeometryType>(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IGeometryCollection<GeometryType> other)
            where GeometryType : IGeometry
        {
            return this.CreateGeometryCollection<GeometryType>(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other geometry collection is null.
        /// </exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IGeometryCollection<GeometryType> other, params Int32[] indexes)
            where GeometryType : IGeometry
        {
            return this.CreateGeometryCollection<GeometryType>(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other geometry collection is null.
        /// </exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IGeometryCollection<GeometryType> other, IEnumerable<Int32> indexes)
            where GeometryType : IGeometry
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherGeometryCollectionIsNull);

            StoredGeometryCollection<GeometryType> collection = new StoredGeometryCollection<GeometryType>(this, identifier, indexes);
            Int32 index = 0;
            foreach (GeometryType geometry in other)
            {
                this.CreateGeometry(geometry, index);
                index++;
            }

            return collection;
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IGeometryCollection<GeometryType> other, params Int32[] indexes)
            where GeometryType : IGeometry
        {
            return this.CreateGeometryCollection<GeometryType>(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        public IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IGeometryCollection<GeometryType> other, IEnumerable<Int32> indexes)
            where GeometryType : IGeometry
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherGeometryCollectionIsNull);

            StoredGeometryCollection<GeometryType> collection;
            if (other is IStoredGeometry)
            {
                collection = new StoredGeometryCollection<GeometryType>(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                collection = new StoredGeometryCollection<GeometryType>(this, this.Driver.CreateIdentifier(), indexes);
                Int32 index = 0;
                foreach (GeometryType geometry in other)
                {
                    this.CreateGeometry(geometry, index);
                    index++;
                }
            }

            return collection;
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPoint CreateMultiPoint(String identifier)
        {
            return this.CreateMultiPoint(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPoint CreateMultiPoint(String identifier, params Int32[] indexes)
        {
            return this.CreateMultiPoint(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPoint CreateMultiPoint(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredMultiPoint(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi point.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPoint CreateMultiPoint(String identifier, IMultiPoint other)
        {
            return this.CreateMultiPoint(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other multi point is null.
        /// </exception>
        public IMultiPoint CreateMultiPoint(String identifier, IMultiPoint other, params Int32[] indexes)
        {
            return this.CreateMultiPoint(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other multi point is null.
        /// </exception>
        public IMultiPoint CreateMultiPoint(String identifier, IMultiPoint other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiPointIsNull);

            StoredMultiPoint collection = new StoredMultiPoint(this, identifier, indexes);
            Int32 index = 0;
            foreach (IPoint geometry in other)
            {
                this.CreateGeometry(geometry, index);
                index++;
            }

            return collection;
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        public IMultiPoint CreateMultiPoint(IMultiPoint other, params Int32[] indexes)
        {
            return this.CreateMultiPoint(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        public IMultiPoint CreateMultiPoint(IMultiPoint other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiPointIsNull);

            StoredMultiPoint collection;
            if (other is IStoredGeometry)
            {
                collection = new StoredMultiPoint(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                collection = new StoredMultiPoint(this, this.Driver.CreateIdentifier(), indexes);
                Int32 index = 0;
                foreach (IPoint geometry in other)
                {
                    this.CreateGeometry(geometry, index);
                    index++;
                }
            }

            return collection;
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiLineString CreateMultiLineString(String identifier)
        {
            return this.CreateMultiLineString(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiLineString CreateMultiLineString(String identifier, params Int32[] indexes)
        {
            return this.CreateMultiLineString(identifier, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiLineString CreateMultiLineString(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredMultiLineString(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi line string.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiLineString CreateMultiLineString(String identifier, IMultiLineString other)
        {
            return this.CreateMultiLineString(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other multi line string is null.
        /// </exception>
        public IMultiLineString CreateMultiLineString(String identifier, IMultiLineString other, params Int32[] indexes)
        {
            return this.CreateMultiLineString(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other multi line string is null.
        /// </exception>
        public IMultiLineString CreateMultiLineString(String identifier, IMultiLineString other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiLineStringIsNull);

            StoredMultiLineString collection = new StoredMultiLineString(this, identifier, indexes);
            Int32 index = 0;
            foreach (ILineString geometry in other)
            {
                this.CreateGeometry(geometry, index);
                index++;
            }

            return collection;
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        public IMultiLineString CreateMultiLineString(IMultiLineString other, params Int32[] indexes)
        {
            return this.CreateMultiLineString(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        public IMultiLineString CreateMultiLineString(IMultiLineString other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiLineStringIsNull);

            StoredMultiLineString collection;
            if (other is IStoredGeometry)
            {
                collection = new StoredMultiLineString(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                collection = new StoredMultiLineString(this, this.Driver.CreateIdentifier(), indexes);
                Int32 index = 0;
                foreach (ILineString geometry in other)
                {
                    this.CreateGeometry(geometry, index);
                    index++;
                }
            }

            return collection;
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPolygon CreateMultiPolygon(String identifier)
        {
            return this.CreateMultiPolygon(identifier, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPolygon CreateMultiPolygon(String identifier, params Int32[] indexes)
        {
            return this.CreateMultiPolygon(identifier, indexes);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPolygon CreateMultiPolygon(String identifier, IEnumerable<Int32> indexes)
        {
            return new StoredMultiPolygon(this, identifier, indexes);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi polygon.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IMultiPolygon CreateMultiPolygon(String identifier, IMultiPolygon other)
        {
            return this.CreateMultiPolygon(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other multi polygon is null.
        /// </exception>
        public IMultiPolygon CreateMultiPolygon(String identifier, IMultiPolygon other, params Int32[] indexes)
        {
            return this.CreateMultiPolygon(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other multi polygon is null.
        /// </exception>
        public IMultiPolygon CreateMultiPolygon(String identifier, IMultiPolygon other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiPolygonIsNull);

            StoredMultiPolygon collection = new StoredMultiPolygon(this, this.Driver.CreateIdentifier(), indexes);
            Int32 index = 0;
            foreach (IPolygon geometry in other)
            {
                this.CreateGeometry(geometry, index);
                index++;
            }

            return collection;
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        public IMultiPolygon CreateMultiPolygon(IMultiPolygon other, params Int32[] indexes)
        {
            return this.CreateMultiPolygon(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        public IMultiPolygon CreateMultiPolygon(IMultiPolygon other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherMultiPolygonIsNull);

            StoredMultiPolygon collection;
            if (other is IStoredGeometry)
            {
                collection = new StoredMultiPolygon(this, (other as IStoredGeometry).Identifier, ConcatIndexes(other, indexes));
            }
            else
            {
                collection = new StoredMultiPolygon(this, this.Driver.CreateIdentifier(), indexes);
                Int32 index = 0;
                foreach (IPolygon geometry in other)
                {
                    this.CreateGeometry(geometry, index);
                    index++;
                }
            }

            return collection;
        }

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        public IGeometry CreateGeometry(String identifier, IGeometry other)
        {
            return this.CreateGeometry(identifier, other, null as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        public IGeometry CreateGeometry(String identifier, IGeometry other, params Int32[] indexes)
        {
            return this.CreateGeometry(identifier, other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        public IGeometry CreateGeometry(String identifier, IGeometry other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherGeometryIsNull);

            if (other is IPoint)
                return this.CreatePoint(identifier, other as IPoint, indexes);
            if (other is ILine)
                return this.CreateLine(identifier, other as ILine, indexes);
            if (other is ILinearRing)
                return this.CreateLinearRing(identifier, other as ILinearRing, indexes);
            if (other is ILineString)
                return this.CreateLineString(identifier, other as ILineString, indexes);
            if (other is ITriangle)
                return this.CreateTriangle(identifier, other as ITriangle, indexes);
            if (other is IPolygon)
                return this.CreatePolygon(identifier, other as IPolygon, indexes);
            if (other is IMultiPoint)
                return this.CreateMultiPoint(identifier, other as IMultiPoint, indexes);
            if (other is IMultiLineString)
                return this.CreateMultiLineString(identifier, other as IMultiLineString, indexes);
            if (other is IMultiPolygon)
                return this.CreateMultiPolygon(identifier, other as IMultiPolygon, indexes);
            if (other is IGeometryCollection)
                return this.CreateGeometryCollection(identifier, other as IGeometryCollection, indexes);

            throw new ArgumentException(nameof(other), CoreMessages.OtherGeometryNotSupported);
        }

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        public IGeometry CreateGeometry(IGeometry other, params Int32[] indexes)
        {
            return this.CreateGeometry(other, indexes as IEnumerable<Int32>);
        }

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        public IGeometry CreateGeometry(IGeometry other, IEnumerable<Int32> indexes)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), CoreMessages.OtherGeometryIsNull);

            if (other is IPoint)
                return this.CreatePoint(other as IPoint, indexes);
            if (other is ILine)
                return this.CreateLine(other as ILine, indexes);
            if (other is ILinearRing)
                return this.CreateLinearRing(other as ILinearRing, indexes);
            if (other is ILineString)
                return this.CreateLineString(other as ILineString, indexes);
            if (other is IPolygon)
                return this.CreatePolygon(other as IPolygon, indexes);
            if (other is ITriangle)
                return this.CreateTriangle(other as ITriangle, indexes);
            if (other is IMultiPoint)
                return this.CreateMultiPoint(other as IMultiPoint, indexes);
            if (other is IMultiLineString)
                return this.CreateMultiLineString(other as IMultiLineString, indexes);
            if (other is IMultiPolygon)
                return this.CreateMultiPolygon(other as IMultiPolygon, indexes);
            if (other is IGeometryCollection)
                return this.CreateGeometryCollection(other as IGeometryCollection, indexes);

            throw new ArgumentException(nameof(other), CoreMessages.OtherGeometryNotSupported);
        }

        /// <summary>
        /// Returns a geometry factory with the specified precision model.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>A geometry factory with the specified precision model.</returns>
        public IGeometryFactory WithPrecisionModel(PrecisionModel precisionModel)
        {
            return new StoredGeometryFactory(this.Driver, precisionModel, this.referenceSystem);
        }

        /// <summary>
        /// Returns a geometry factory with the specified reference system.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <returns>A geometry factory with the specified reference system.</returns>
        public IGeometryFactory WithReferenceSystem(IReferenceSystem referenceSystem)
        {
            return new StoredGeometryFactory(this.Driver, this.PrecisionModel, this.referenceSystem);
        }

        /// <summary>
        /// Concatenates the indexes of the geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The concatenated indexes.</returns>
        private static IEnumerable<Int32> ConcatIndexes(IGeometry geometry, IEnumerable<Int32> indexes)
        {
            if (!(geometry is IStoredGeometry))
                return indexes;
            if (indexes == null)
                return (geometry as IStoredGeometry).Indexes;
            return (geometry as IStoredGeometry).Indexes.Concat(indexes);
        }
    }
}
