// <copyright file="IStoredGeometryFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines behavior for factories producing <see cref="IGeometry" /> instances located in stores.
    /// </summary>
    public interface IStoredGeometryFactory : IGeometryFactory
    {
        /// <summary>
        /// Gets the geometry driver of the factory.
        /// </summary>
        /// <value>The geometry driver of the factory.</value>
        IGeometryDriver Driver { get; }

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IPoint CreatePoint(String identifier);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IPoint CreatePoint(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IPoint CreatePoint(String identifier, IEnumerable<Int32> indexes);

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
        IPoint CreatePoint(String identifier, IPoint other);

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
        IPoint CreatePoint(String identifier, IPoint other, params Int32[] indexes);

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
        IPoint CreatePoint(String identifier, IPoint other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        IPoint CreatePoint(Coordinate coordinate, params Int32[] indexes);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="coordinate">The coordinate of the point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        IPoint CreatePoint(Coordinate coordinate, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other point is null.</exception>
        IPoint CreatePoint(IPoint other, params Int32[] indexes);

        /// <summary>
        /// Creates a point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other point is null.</exception>
        IPoint CreatePoint(IPoint other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILineString CreateLineString(String identifier);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILineString CreateLineString(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILineString CreateLineString(String identifier, IEnumerable<Int32> indexes);

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
        ILineString CreateLineString(String identifier, ILineString other);

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
        ILineString CreateLineString(String identifier, ILineString other, params Int32[] indexes);

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
        ILineString CreateLineString(String identifier, ILineString other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other line string is null.</exception>
        ILineString CreateLineString(ILineString other, params Int32[] indexes);

        /// <summary>
        /// Creates a line string.
        /// </summary>
        /// <param name="other">The other line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other line string is null.</exception>
        ILineString CreateLineString(ILineString other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILine CreateLine(String identifier);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILine CreateLine(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILine CreateLine(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other line.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILine CreateLine(String identifier, ILine other);

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
        ILine CreateLine(String identifier, ILine other, params Int32[] indexes);

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
        ILine CreateLine(String identifier, ILine other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        ILine CreateLine(ILine other, params Int32[] indexes);

        /// <summary>
        /// Creates a line.
        /// </summary>
        /// <param name="other">The other line.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The line created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other line is null.</exception>
        ILine CreateLine(ILine other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILinearRing CreateLinearRing(String identifier);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILinearRing CreateLinearRing(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILinearRing CreateLinearRing(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other linear ring.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ILinearRing CreateLinearRing(String identifier, ILinearRing other);

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
        ILinearRing CreateLinearRing(String identifier, ILinearRing other, params Int32[] indexes);

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
        ILinearRing CreateLinearRing(String identifier, ILinearRing other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other linear ring is null.</exception>
        ILinearRing CreateLinearRing(ILinearRing other, params Int32[] indexes);

        /// <summary>
        /// Creates a linear ring.
        /// </summary>
        /// <param name="other">The other linear ring.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The linear ring created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other linear ring is null.</exception>
        ILinearRing CreateLinearRing(ILinearRing other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IPolygon CreatePolygon(String identifier);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IPolygon CreatePolygon(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IPolygon CreatePolygon(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other polygon.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IPolygon CreatePolygon(String identifier, IPolygon other);

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
        IPolygon CreatePolygon(String identifier, IPolygon other, params Int32[] indexes);

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
        IPolygon CreatePolygon(String identifier, IPolygon other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        IPolygon CreatePolygon(IPolygon other, params Int32[] indexes);

        /// <summary>
        /// Creates a polygon.
        /// </summary>
        /// <param name="other">The other polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other polygon is null.</exception>
        IPolygon CreatePolygon(IPolygon other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ITriangle CreateTriangle(String identifier);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ITriangle CreateTriangle(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ITriangle CreateTriangle(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other triangle.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        ITriangle CreateTriangle(String identifier, ITriangle other);

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
        ITriangle CreateTriangle(String identifier, ITriangle other, params Int32[] indexes);

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
        ITriangle CreateTriangle(String identifier, ITriangle other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        ITriangle CreateTriangle(ITriangle other, params Int32[] indexes);

        /// <summary>
        /// Creates a triangle.
        /// </summary>
        /// <param name="other">The other triangle.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The triangle created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other triangle is null.</exception>
        ITriangle CreateTriangle(ITriangle other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection CreateGeometryCollection(String identifier);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection CreateGeometryCollection(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection CreateGeometryCollection(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection CreateGeometryCollection(String identifier, IGeometryCollection other);

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
        IGeometryCollection CreateGeometryCollection(String identifier, IGeometryCollection other, params Int32[] indexes);

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
        IGeometryCollection CreateGeometryCollection(String identifier, IGeometryCollection other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        IGeometryCollection CreateGeometryCollection(IGeometryCollection other, params Int32[] indexes);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        IGeometryCollection CreateGeometryCollection(IGeometryCollection other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, params Int32[] indexes)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IEnumerable<Int32> indexes)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other geometry collection.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IGeometryCollection<GeometryType> other)
            where GeometryType : IGeometry;

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
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IGeometryCollection<GeometryType> other, params Int32[] indexes)
            where GeometryType : IGeometry;

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
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(String identifier, IGeometryCollection<GeometryType> other, IEnumerable<Int32> indexes)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IGeometryCollection<GeometryType> other, params Int32[] indexes)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry collection.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="other">The other geometry collection.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The geometry collection created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry collection is null.</exception>
        IGeometryCollection<GeometryType> CreateGeometryCollection<GeometryType>(IGeometryCollection<GeometryType> other, IEnumerable<Int32> indexes)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPoint CreateMultiPoint(String identifier);

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPoint CreateMultiPoint(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPoint CreateMultiPoint(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi point.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPoint CreateMultiPoint(String identifier, IMultiPoint other);

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
        IMultiPoint CreateMultiPoint(String identifier, IMultiPoint other, params Int32[] indexes);

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
        IMultiPoint CreateMultiPoint(String identifier, IMultiPoint other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        IMultiPoint CreateMultiPoint(IMultiPoint other, params Int32[] indexes);

        /// <summary>
        /// Creates a multi point.
        /// </summary>
        /// <param name="other">The other multi point.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi point created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi point is null.</exception>
        IMultiPoint CreateMultiPoint(IMultiPoint other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiLineString CreateMultiLineString(String identifier);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiLineString CreateMultiLineString(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiLineString CreateMultiLineString(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi line string.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiLineString CreateMultiLineString(String identifier, IMultiLineString other);

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
        IMultiLineString CreateMultiLineString(String identifier, IMultiLineString other, params Int32[] indexes);

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
        IMultiLineString CreateMultiLineString(String identifier, IMultiLineString other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        IMultiLineString CreateMultiLineString(IMultiLineString other, params Int32[] indexes);

        /// <summary>
        /// Creates a multi line string.
        /// </summary>
        /// <param name="other">The other multi line string.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi line string created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi line string is null.</exception>
        IMultiLineString CreateMultiLineString(IMultiLineString other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPolygon CreateMultiPolygon(String identifier);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPolygon CreateMultiPolygon(String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPolygon CreateMultiPolygon(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="other">The other multi polygon.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        IMultiPolygon CreateMultiPolygon(String identifier, IMultiPolygon other);

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
        IMultiPolygon CreateMultiPolygon(String identifier, IMultiPolygon other, params Int32[] indexes);

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
        IMultiPolygon CreateMultiPolygon(String identifier, IMultiPolygon other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        IMultiPolygon CreateMultiPolygon(IMultiPolygon other, params Int32[] indexes);

        /// <summary>
        /// Creates a multi polygon.
        /// </summary>
        /// <param name="other">The other multi polygon.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The multi polygon created by the factory.</returns>
        /// <exception cref="System.ArgumentNullException">The other multi polygon is null.</exception>
        IMultiPolygon CreateMultiPolygon(IMultiPolygon other, IEnumerable<Int32> indexes);

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
        IGeometry CreateGeometry(String identifier, IGeometry other);

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
        IGeometry CreateGeometry(String identifier, IGeometry other, params Int32[] indexes);

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
        IGeometry CreateGeometry(String identifier, IGeometry other, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        IGeometry CreateGeometry(IGeometry other, params Int32[] indexes);

        /// <summary>
        /// Creates a geometry matching another geometry.
        /// </summary>
        /// <param name="other">The other geometry.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The produced geometry matching <paramref name="other" />.</returns>
        /// <exception cref="System.ArgumentNullException">The other geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The type of the other geometry is not supported.</exception>
        IGeometry CreateGeometry(IGeometry other, IEnumerable<Int32> indexes);
    }
}
