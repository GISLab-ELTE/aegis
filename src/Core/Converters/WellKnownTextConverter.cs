// <copyright file="WellKnownTextConverter.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using AEGIS.Geometries;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a converter for Well-known Text (WKT) representation.
    /// </summary>
    public static class WellKnownTextConverter
    {
        /// <summary>
        /// The regular expression pattern for geometries. This field is constant.
        /// </summary>
        private const String PatternGeometry = @"^(?<type>(\w+((\s*Z)|(\s*M)|(\s*ZM))?))\s*((?<empty>(EMPTY))|[(](?<content>.+)[)])$";

        /// <summary>
        /// The regular expression pattern for floating point numbers. This field is constant.
        /// </summary>
        private const String PatternNumber = @"[-]?(\d*\.\d+|\d+)";

        /// <summary>
        /// The regular expression pattern for coordinates. This field is constant.
        /// </summary>
        private const String PatternCoordinate = @"(?<x>" + PatternNumber + @")\s+(?<y>" + PatternNumber + @")(\s+(?<z>" + PatternNumber + @"))?(\s+" + PatternNumber + @")?";

        /// <summary>
        /// The regular expression pattern for coordinate collections. This field is constant.
        /// </summary>
        private const String PatternCoordinateCollection = PatternCoordinate + @"(,\s*" + PatternCoordinate + @")*";

        /// <summary>
        /// The regular expression pattern for polygons. This field is constant.
        /// </summary>
        private const String PatternPolygon = @"[(](?<shell>[0-9\., ]+)[)](,\s*[(](?<hole>[0-9\., ]+)[)])*";

        /// <summary>
        /// The regular expression pattern for multi points. This field is constant.
        /// </summary>
        private const String PatternMultiPoint = @"[(]?" + PatternCoordinate + @"[)]?(,\s*[(]?" + PatternCoordinate + @"[)]?)*";

        /// <summary>
        /// The regular expression pattern for multi line strings. This field is constant.
        /// </summary>
        private const String PatternMultiLineString = @"[(](?<lineString>[0-9\., ]+)[)](,\s*[(](?<lineString>[0-9\., ]+)[)])*";

        /// <summary>
        /// The regular expression pattern for multi polygons. This field is constant.
        /// </summary>
        private const String PatternMultiPolygon = @"[(](?<polygon>([(][0-9\., ]+[)](,\s*[(][0-9\., ]+[)])*))[)](,\s*[(](?<polygon>([(][0-9\., ]+[)](,\s*[(][0-9\., ]+[)])*))[)])*";

        /// <summary>
        /// Converts a geometry to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The specified geometry is not supported.</exception>
        public static String ToWellKnownText(this IGeometry geometry)
        {
            return ToWellKnownText(geometry, 2);
        }

        /// <summary>
        /// Converts a geometry to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The dimension is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static String ToWellKnownText(this IGeometry geometry, Int32 dimension)
        {
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry));

            if (dimension < 2 || dimension > 3)
                throw new ArgumentException(CoreMessages.DimensionIsInvalid, nameof(dimension));

            if (geometry is IPoint)
                return ToWellKnownText(geometry as IPoint, dimension);
            if (geometry is ILineString)
                return ToWellKnownText(geometry as ILineString, dimension);
            if (geometry is IPolygon)
                return ToWellKnownText(geometry as IPolygon, dimension);
            if (geometry is IMultiPoint)
                return ToWellKnownText(geometry as IMultiPoint, dimension);
            if (geometry is IMultiLineString)
                return ToWellKnownText(geometry as IMultiLineString, dimension);
            if (geometry is IMultiPolygon)
                return ToWellKnownText(geometry as IMultiPolygon, dimension);

            throw new ArgumentException(CoreMessages.GeometryIsNotSupported, nameof(geometry));
        }

        /// <summary>
        /// Convert Well-known Text representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="source">The source.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source is invalid.
        /// or
        /// The specified source is not supported.
        /// </exception>
        public static IGeometry ToGeometry(this IGeometryFactory factory, String source)
        {
            return ToGeometry(source, factory);
        }

        /// <summary>
        /// Convert Well-known Text representation to <see cref="IGeometry" /> representation.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The <see cref="IGeometry" /> representation of the geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source is invalid.
        /// or
        /// The specified source is not supported.
        /// </exception>
        public static IGeometry ToGeometry(String source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (String.IsNullOrEmpty(source))
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            IGeometry resultGeometry = null;

            try
            {
                Match match = Regex.Match(source, PatternGeometry, RegexOptions.IgnoreCase);

                String geometryType = match.Groups["type"].Value;
                String geometryContent = match.Groups["content"].Value;
                Boolean isEmpty = match.Groups["empty"].Success;

                switch (geometryType)
                {
                    case "POINT":
                    case "POINT M":
                        resultGeometry = isEmpty ? factory.CreatePoint(Coordinate.Empty) : ToPoint(geometryContent, 2, factory);
                        break;
                    case "POINT Z":
                    case "POINT ZM":
                        resultGeometry = isEmpty ? factory.CreatePoint(Coordinate.Empty) : ToPoint(geometryContent, 3, factory);
                        break;
                    case "LINESTRING":
                    case "LINESTRING M":
                        resultGeometry = isEmpty ? factory.CreateLineString() : ToLineString(geometryContent, 2, factory);
                        break;
                    case "LINESTRING Z":
                    case "LINESTRING ZM":
                        resultGeometry = isEmpty ? factory.CreateLineString() : ToLineString(geometryContent, 3, factory);
                        break;
                    case "POLYGON":
                    case "POLYGON M":
                        resultGeometry = isEmpty ? factory.CreatePolygon(new Coordinate[0]) : ToPolygon(geometryContent, 2, factory);
                        break;
                    case "POLYGON Z":
                    case "POLYGON ZM":
                        resultGeometry = isEmpty ? factory.CreatePolygon(new Coordinate[0]) : ToPolygon(geometryContent, 3, factory);
                        break;
                    case "MULTIPOINT":
                    case "MULTIPOINT M":
                        resultGeometry = isEmpty ? factory.CreateMultiPoint() : ToMultiPoint(geometryContent, 2, factory);
                        break;
                    case "MULTIPOINT Z":
                    case "MULTIPOINT ZM":
                        resultGeometry = isEmpty ? factory.CreateMultiPoint() : ToMultiPoint(geometryContent, 3, factory);
                        break;
                    case "MULTILINESTRING":
                    case "MULTILINESTRING M":
                        resultGeometry = isEmpty ? factory.CreateMultiLineString() : ToMultiLineString(geometryContent, 2, factory);
                        break;
                    case "MULTILINESTRING Z":
                    case "MULTILINESTRING ZM":
                        resultGeometry = isEmpty ? factory.CreateMultiLineString() : ToMultiLineString(geometryContent, 3, factory);
                        break;
                    case "MULTIPOLYGON":
                    case "MULTIPOLYGON M":
                        resultGeometry = isEmpty ? factory.CreateMultiPolygon() : ToMultiPolygon(geometryContent, 2, factory);
                        break;
                    case "MULTIPOLYGON Z":
                    case "MULTIPOLYGON ZM":
                        resultGeometry = isEmpty ? factory.CreateMultiPolygon() : ToMultiPolygon(geometryContent, 3, factory);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }

            if (resultGeometry == null)
                throw new ArgumentException(CoreMessages.SourceIsNotSupported, nameof(source));

            return resultGeometry;
        }

        /// <summary>
        /// Converts the point to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted point.</returns>
        private static String ToWellKnownText(IPoint point, Int32 dimension)
        {
            if (dimension == 3)
            {
                return String.Format("POINT Z ({0} {1} {2})",
                                     point.X.ToString("G", CultureInfo.InvariantCulture),
                                     point.Y.ToString("G", CultureInfo.InvariantCulture),
                                     point.Z.ToString("G", CultureInfo.InvariantCulture));
            }
            else
            {
                return String.Format("POINT ({0} {1})",
                                     point.X.ToString("G", CultureInfo.InvariantCulture),
                                     point.Y.ToString("G", CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Converts the line string to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted point.</returns>
        private static String ToWellKnownText(ILineString lineString, Int32 dimension)
        {
            StringBuilder builder = new StringBuilder(20 + (dimension == 2 ? 20 : 30) * lineString.Count);
            if (dimension == 3)
                builder.Append("LINESTRING Z (");
            else
                builder.Append("LINESTRING (");

            ConvertCoordinates(builder, lineString, dimension);
            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the polygon to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted polygon.</returns>
        private static String ToWellKnownText(IPolygon polygon, Int32 dimension)
        {
            StringBuilder builder = new StringBuilder(15 + (dimension == 2 ? 20 : 30) * (polygon.Shell.Count + (polygon.Holes != null ? polygon.Holes.Sum(hole => hole.Count) : 0)));
            if (dimension == 3)
                builder.Append("POLYGON Z ((");
            else
                builder.Append("POLYGON ((");

            ConvertCoordinates(builder, polygon.Shell, dimension);
            builder.Append(")");

            for (Int32 holeIndex = 0; holeIndex < polygon.HoleCount; holeIndex++)
            {
                builder.Append(", (");
                ConvertCoordinates(builder, polygon.Holes[holeIndex], dimension);
                builder.Append(")");
            }

            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the multi point to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="multiPoint">The multi point.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted multi point.</returns>
        private static String ToWellKnownText(IMultiPoint multiPoint, Int32 dimension)
        {
            StringBuilder builder = new StringBuilder(15 + (dimension == 2 ? 20 : 30) * multiPoint.Count);
            if (dimension == 3)
                builder.Append("MULTIPOINT Z (");
            else
                builder.Append("MULTIPOINT (");

            ConvertCoordinates(builder, multiPoint.Select(point => point.Coordinate), dimension);

            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the multi line string to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="multiLineString">The multi line string.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted multi line string.</returns>
        private static String ToWellKnownText(IMultiLineString multiLineString, Int32 dimension)
        {
            StringBuilder builder = new StringBuilder(20 + (dimension == 2 ? 20 : 30) * multiLineString.Sum(lineString => lineString.Count));
            if (dimension == 3)
                builder.Append("MULTILINESTRING Z (");
            else
                builder.Append("MULTILINESTRING (");

            for (Int32 lineStringIndex = 0; lineStringIndex < multiLineString.Count; lineStringIndex++)
            {
                if (lineStringIndex > 0)
                    builder.Append(", ");

                builder.Append("(");
                ConvertCoordinates(builder, multiLineString[lineStringIndex], dimension);
                builder.Append(")");
            }

            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the multi polygon to Well-known Text (WKT) representation.
        /// </summary>
        /// <param name="multiPolygon">The multi polygon.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted multi polygon.</returns>
        private static String ToWellKnownText(IMultiPolygon multiPolygon, Int32 dimension)
        {
            StringBuilder builder = new StringBuilder(20 + (dimension == 2 ? 20 : 30) * multiPolygon.Sum(polygon => (polygon.Shell.Count + (polygon.Holes != null ? polygon.Holes.Sum(hole => hole.Count) : 0))));
            if (dimension == 3)
                builder.Append("MULTIPOLYGON Z (");
            else
                builder.Append("MULTIPOLYGON (");

            for (Int32 polygonIndex = 0; polygonIndex < multiPolygon.Count; polygonIndex++)
            {
                if (polygonIndex > 0)
                    builder.Append(", ");

                builder.Append("((");
                ConvertCoordinates(builder, multiPolygon[polygonIndex].Shell, dimension);
                builder.Append(")");

                for (Int32 holeIndex = 0; holeIndex < multiPolygon[polygonIndex].HoleCount; holeIndex++)
                {
                    builder.Append(", (");
                    ConvertCoordinates(builder, multiPolygon[polygonIndex].Holes[holeIndex], dimension);
                    builder.Append(")");
                }

                builder.Append(")");
            }

            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a point.
        /// </summary>
        /// <param name="text">The WKT representation of the point.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted point.</returns>
        private static IPoint ToPoint(String text, Int32 dimension, IGeometryFactory factory)
        {
            Match match = Regex.Match(text, PatternCoordinate);

            if (dimension == 3)
            {
                return factory.CreatePoint(Double.Parse(match.Groups["x"].Value, CultureInfo.InvariantCulture),
                                           Double.Parse(match.Groups["y"].Value, CultureInfo.InvariantCulture),
                                           Double.Parse(match.Groups["z"].Value, CultureInfo.InvariantCulture));
            }
            else
            {
                return factory.CreatePoint(Double.Parse(match.Groups["x"].Value, CultureInfo.InvariantCulture),
                                           Double.Parse(match.Groups["y"].Value, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a line string.
        /// </summary>
        /// <param name="text">The WKT representation of the line string.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted line string.</returns>
        private static ILineString ToLineString(String text, Int32 dimension, IGeometryFactory factory)
        {
            return factory.CreateLineString(ConvertCoordinates(text, dimension));
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a polygon.
        /// </summary>
        /// <param name="text">The WKT representation of the polygon.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted polygon.</returns>
        private static IPolygon ToPolygon(String text, Int32 dimension, IGeometryFactory factory)
        {
            Match match = Regex.Match(text, PatternPolygon);

            IReadOnlyList<Coordinate> shell = ConvertCoordinates(match.Groups["shell"].Value, dimension);
            IReadOnlyList<Coordinate>[] holes = new IReadOnlyList<Coordinate>[match.Groups["hole"].Captures.Count];

            for (Int32 holeIndex = 0; holeIndex < holes.Length; holeIndex++)
            {
                holes[holeIndex] = ConvertCoordinates(match.Groups["hole"].Captures[holeIndex].Value, dimension);
            }

            return factory.CreatePolygon(shell, holes);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a multi point.
        /// </summary>
        /// <param name="text">The WKT representation of the multi point.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi point.</returns>
        private static IMultiPoint ToMultiPoint(String text, Int32 dimension, IGeometryFactory factory)
        {
            Match match = Regex.Match(text, PatternMultiPoint);

            Coordinate[] coordinates = new Coordinate[match.Groups["x"].Captures.Count];

            for (Int32 coordIndex = 0; coordIndex < coordinates.Length; coordIndex++)
            {
                if (dimension == 3)
                {
                    coordinates[coordIndex] = new Coordinate(Double.Parse(match.Groups["x"].Captures[coordIndex].Value, CultureInfo.InvariantCulture),
                                                             Double.Parse(match.Groups["y"].Captures[coordIndex].Value, CultureInfo.InvariantCulture),
                                                             Double.Parse(match.Groups["z"].Captures[coordIndex].Value, CultureInfo.InvariantCulture));
                }
                else
                {
                    coordinates[coordIndex] = new Coordinate(Double.Parse(match.Groups["x"].Captures[coordIndex].Value, CultureInfo.InvariantCulture),
                                                             Double.Parse(match.Groups["y"].Captures[coordIndex].Value, CultureInfo.InvariantCulture));
                }
            }

            return factory.CreateMultiPoint(coordinates);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a multi line string.
        /// </summary>
        /// <param name="text">The WKT representation of the multi line string.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi line string.</returns>
        private static IMultiLineString ToMultiLineString(String text, Int32 dimension, IGeometryFactory factory)
        {
            Match match = Regex.Match(text, PatternMultiLineString);
            ILineString[] lineStrings = new ILineString[match.Groups["lineString"].Captures.Count];

            for (Int32 lineStringIndex = 0; lineStringIndex < lineStrings.Length; lineStringIndex++)
            {
                lineStrings[lineStringIndex] = ToLineString(match.Groups["lineString"].Captures[lineStringIndex].Value, dimension, factory);
            }

            return factory.CreateMultiLineString(lineStrings);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a multi polygon.
        /// </summary>
        /// <param name="text">The WKT representation of the multi polygon.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi polygon.</returns>
        private static IMultiPolygon ToMultiPolygon(String text, Int32 dimension, IGeometryFactory factory)
        {
            Match match = Regex.Match(text, PatternMultiPolygon);
            IPolygon[] polygons = new IPolygon[match.Groups["polygon"].Captures.Count];

            for (Int32 polygonIndex = 0; polygonIndex < polygons.Length; polygonIndex++)
            {
                polygons[polygonIndex] = ToPolygon(match.Groups["polygon"].Captures[polygonIndex].Value, dimension, factory);
            }

            return factory.CreateMultiPolygon(polygons);
        }

        /// <summary>
        /// Converts to specified coordinate list to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        private static void ConvertCoordinates(StringBuilder builder, IReadOnlyList<Coordinate> coordinates, Int32 dimension)
        {
            for (Int32 coordIndex = 0; coordIndex < coordinates.Count; coordIndex++)
            {
                if (coordIndex > 0)
                    builder.Append(", ");

                if (dimension == 3)
                {
                    builder.AppendFormat("{0} {1} {2}",
                                         coordinates[coordIndex].X.ToString("G", CultureInfo.InvariantCulture),
                                         coordinates[coordIndex].Y.ToString("G", CultureInfo.InvariantCulture),
                                         coordinates[coordIndex].Z.ToString("G", CultureInfo.InvariantCulture));
                }
                else
                {
                    builder.AppendFormat("{0} {1}",
                                         coordinates[coordIndex].X.ToString("G", CultureInfo.InvariantCulture),
                                         coordinates[coordIndex].Y.ToString("G", CultureInfo.InvariantCulture));
                }
            }
        }

        /// <summary>
        /// Converts to specified coordinate list to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        private static void ConvertCoordinates(StringBuilder builder, IEnumerable<Coordinate> coordinates, Int32 dimension)
        {
            Boolean firstCoordinate = true;
            foreach (Coordinate coordinate in coordinates)
            {
                if (!firstCoordinate)
                    builder.Append(", ");

                if (dimension == 3)
                {
                    builder.AppendFormat("{0} {1} {2}",
                                         coordinate.X.ToString("G", CultureInfo.InvariantCulture),
                                         coordinate.Y.ToString("G", CultureInfo.InvariantCulture),
                                         coordinate.Z.ToString("G", CultureInfo.InvariantCulture));
                }
                else
                {
                    builder.AppendFormat("{0} {1}",
                                         coordinate.X.ToString("G", CultureInfo.InvariantCulture),
                                         coordinate.Y.ToString("G", CultureInfo.InvariantCulture));
                }

                firstCoordinate = false;
            }
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a coordinate list.
        /// </summary>
        /// <param name="text">The WKT representation of the coordinate list.</param>
        /// <param name="dimension">The dimension of the coordinates.</param>
        /// <returns>The converted coordinate list.</returns>
        private static IReadOnlyList<Coordinate> ConvertCoordinates(String text, Int32 dimension)
        {
            Match match = Regex.Match(text, PatternCoordinateCollection);

            Coordinate[] coordinates = new Coordinate[match.Groups["x"].Captures.Count];

            for (Int32 coordIndex = 0; coordIndex < coordinates.Length; coordIndex++)
            {
                if (dimension == 3)
                {
                    coordinates[coordIndex] = new Coordinate(Double.Parse(match.Groups["x"].Captures[coordIndex].Value, CultureInfo.InvariantCulture),
                                                             Double.Parse(match.Groups["y"].Captures[coordIndex].Value, CultureInfo.InvariantCulture),
                                                             Double.Parse(match.Groups["z"].Captures[coordIndex].Value, CultureInfo.InvariantCulture));
                }
                else
                {
                    coordinates[coordIndex] = new Coordinate(Double.Parse(match.Groups["x"].Captures[coordIndex].Value, CultureInfo.InvariantCulture),
                                                             Double.Parse(match.Groups["y"].Captures[coordIndex].Value, CultureInfo.InvariantCulture));
                }
            }

            return coordinates;
        }
    }
}
