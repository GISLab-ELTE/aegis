// <copyright file="WellKnownBinaryConverter.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using AEGIS.Geometries;
    using AEGIS.Resources;
    using AEGIS.Utilities;

    /// <summary>
    /// Represents a converter for Well-known Binary (WKB) representation.
    /// </summary>
    public static class WellKnownBinaryConverter
    {
        /// <summary>
        /// Defines Well-known Binary types.
        /// </summary>
        private enum WellKnownBinaryTypes
        {
            /// <summary>
            /// Indicates a generic geometry.
            /// </summary>
            Geometry = 0,

            /// <summary>
            /// Indicates a point.
            /// </summary>
            Point = 1,

            /// <summary>
            /// Indicates a line string.
            /// </summary>
            LineString = 2,

            /// <summary>
            /// Indicates a polygon.
            /// </summary>
            Polygon = 3,

            /// <summary>
            /// Indicates a multi point.
            /// </summary>
            MultiPoint = 4,

            /// <summary>
            /// Indicates a multi line string.
            /// </summary>
            MultiLineString = 5,

            /// <summary>
            /// Indicates a multi polygon.
            /// </summary>
            MultiPolygon = 6,

            /// <summary>
            /// Indicates a geometry collection.
            /// </summary>
            GeometryCollection = 7,

            /// <summary>
            /// Indicates a circular string.
            /// </summary>
            CircularString = 8,

            /// <summary>
            /// Indicates a compound curve.
            /// </summary>
            CompoundCurve = 9,

            /// <summary>
            /// Indicates a curve polygon.
            /// </summary>
            CurvePolygon = 10,

            /// <summary>
            /// Indicates a multi curve.
            /// </summary>
            MultiCurve = 11,

            /// <summary>
            /// Indicates a multi surface.
            /// </summary>
            MultiSurface = 12,

            /// <summary>
            /// Indicates a curve.
            /// </summary>
            Curve = 13,

            /// <summary>
            /// Indicates a surface.
            /// </summary>
            Surface = 14,

            /// <summary>
            /// Indicates a polyhedral surface.
            /// </summary>
            PolyhedralSurface = 15,

            /// <summary>
            /// Indicates a TIN.
            /// </summary>
            TIN = 16,

            /// <summary>
            /// Indicates a triangle.
            /// </summary>
            Triangle = 17,

            /// <summary>
            /// Indicates a 3D geometry.
            /// </summary>
            GeometryZ = 1000,

            /// <summary>
            /// Indicates a 3D point.
            /// </summary>
            PointZ = 1001,

            /// <summary>
            /// Indicates a 3D line string.
            /// </summary>
            LineStringZ = 1002,

            /// <summary>
            /// Indicates a 3D polygon.
            /// </summary>
            PolygonZ = 1003,

            /// <summary>
            /// Indicates a 3D multi point.
            /// </summary>
            MultiPointZ = 1004,

            /// <summary>
            /// Indicates a 3D multi line string.
            /// </summary>
            MultiLineStringZ = 1005,

            /// <summary>
            /// Indicates a 3D multi polygon.
            /// </summary>
            MultiPolygonZ = 1006,

            /// <summary>
            /// Indicates a 3D geometry collection.
            /// </summary>
            GeometryCollectionZ = 1007,

            /// <summary>
            /// Indicates a 3D circular string.
            /// </summary>
            CircularStringZ = 1008,

            /// <summary>
            /// Indicates a 3D compound curve.
            /// </summary>
            CompoundCurveZ = 1009,

            /// <summary>
            /// Indicates a 3D curve polygon.
            /// </summary>
            CurvePolygonZ = 1010,

            /// <summary>
            /// Indicates a 3D multi curve.
            /// </summary>
            MultiCurveZ = 1011,

            /// <summary>
            /// Indicates a 3D multi surface.
            /// </summary>
            MultiSurfaceZ = 1012,

            /// <summary>
            /// Indicates a 3D curve.
            /// </summary>
            CurveZ = 1013,

            /// <summary>
            /// Indicates a 3D surface.
            /// </summary>
            SurfaceZ = 1014,

            /// <summary>
            /// Indicates a 3D polyhedral surface.
            /// </summary>
            PolyhedralSurfaceZ = 1015,

            /// <summary>
            /// Indicates a 3D TIN.
            /// </summary>
            TINZ = 1016,

            /// <summary>
            /// Indicates a 3D triangle.
            /// </summary>
            TriangleZ = 1017,

            /// <summary>
            /// Indicates a measured geometry.
            /// </summary>
            GeometryM = 2000,

            /// <summary>
            /// Indicates a measured point.
            /// </summary>
            PointM = 2001,

            /// <summary>
            /// Indicates a measured line string.
            /// </summary>
            LineStringM = 2002,

            /// <summary>
            /// Indicates a measured polygon.
            /// </summary>
            PolygonM = 2003,

            /// <summary>
            /// Indicates a measured multi point.
            /// </summary>
            MultiPointM = 2004,

            /// <summary>
            /// Indicates a measured multi line string.
            /// </summary>
            MultiLineStringM = 2005,

            /// <summary>
            /// Indicates a measured multi polygon.
            /// </summary>
            MultiPolygonM = 2006,

            /// <summary>
            /// Indicates a measured geometry collection.
            /// </summary>
            GeometryCollectionM = 2007,

            /// <summary>
            /// Indicates a measured circular string.
            /// </summary>
            CircularStringM = 2008,

            /// <summary>
            /// Indicates a measured compound curve.
            /// </summary>
            CompoundCurveM = 2009,

            /// <summary>
            /// Indicates a measured curve polygon.
            /// </summary>
            CurvePolygonM = 2010,

            /// <summary>
            /// Indicates a measured multi curve.
            /// </summary>
            MultiCurveM = 2011,

            /// <summary>
            /// Indicates a measured multi surface.
            /// </summary>
            MultiSurfaceM = 2012,

            /// <summary>
            /// Indicates a measured curve.
            /// </summary>
            CurveM = 2013,

            /// <summary>
            /// Indicates a measured surface.
            /// </summary>
            SurfaceM = 2014,

            /// <summary>
            /// Indicates a measured polyhedral surface.
            /// </summary>
            PolyhedralSurfaceM = 2015,

            /// <summary>
            /// Indicates a measured TIN.
            /// </summary>
            TINM = 2016,

            /// <summary>
            /// Indicates a measured triangle.
            /// </summary>
            TriangleM = 2017,

            /// <summary>
            /// Indicates a measured 3D geometry.
            /// </summary>
            GeometryZM = 3000,

            /// <summary>
            /// Indicates a measured 3D point.
            /// </summary>
            PointZM = 3001,

            /// <summary>
            /// Indicates a measured 3D line string.
            /// </summary>
            LineStringZM = 3002,

            /// <summary>
            /// Indicates a measured 3D polygon.
            /// </summary>
            PolygonZM = 3003,

            /// <summary>
            /// Indicates a measured 3D multi point.
            /// </summary>
            MultiPointZM = 3004,

            /// <summary>
            /// Indicates a measured 3D multi line string.
            /// </summary>
            MultiLineStringZM = 3005,

            /// <summary>
            /// Indicates a measured 3D multi polygon.
            /// </summary>
            MultiPolygonZM = 3006,

            /// <summary>
            /// Indicates a measured 3D geometry collection.
            /// </summary>
            GeometryCollectionZM = 3007,

            /// <summary>
            /// Indicates a measured 3D circular string.
            /// </summary>
            CircularStringZM = 3008,

            /// <summary>
            /// Indicates a measured 3D compound curve.
            /// </summary>
            CompoundCurveZM = 3009,

            /// <summary>
            /// Indicates a measured 3D curve polygon.
            /// </summary>
            CurvePolygonZM = 3010,

            /// <summary>
            /// Indicates a measured 3D multi curve.
            /// </summary>
            MultiCurveZM = 3011,

            /// <summary>
            /// Indicates a measured 3D multi surface.
            /// </summary>
            MultiSurfaceZM = 3012,

            /// <summary>
            /// Indicates a measured 3D curve.
            /// </summary>
            CurveZM = 3013,

            /// <summary>
            /// Indicates a measured 3D surface.
            /// </summary>
            SurfaceZM = 3014,

            /// <summary>
            /// Indicates a measured 3D polyhedral surface.
            /// </summary>
            PolyhedralSurfaceZM = 3015,

            /// <summary>
            /// Indicates a measured 3D TIN.
            /// </summary>
            TINZM = 3016,

            /// <summary>
            /// Indicates a measured 3D triangle.
            /// </summary>
            TriangleZM = 3017
        }

        /// <summary>
        /// Converts a geometry to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static Byte[] ToWellKnownBinary(this IGeometry geometry)
        {
            return ToWellKnownBinary(geometry, ByteOrder.LittleEndian, geometry.CoordinateDimension);
        }

        /// <summary>
        /// Converts a geometry to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static Byte[] ToWellKnownBinary(this IGeometry geometry, ByteOrder byteOrder)
        {
            return ToWellKnownBinary(geometry, byteOrder, geometry.CoordinateDimension);
        }

        /// <summary>
        /// Converts a geometry to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified dimension is invalid.
        /// or
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static Byte[] ToWellKnownBinary(this IGeometry geometry, Int32 dimension)
        {
            return ToWellKnownBinary(geometry, ByteOrder.LittleEndian, geometry.CoordinateDimension);
        }

        /// <summary>
        /// Converts a geometry to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified dimension is invalid.
        /// or
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static Byte[] ToWellKnownBinary(this IGeometry geometry, ByteOrder byteOrder, Int32 dimension)
        {
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry), CoreMessages.GeometryIsNull);

            if (dimension < 2 || dimension > 3)
                throw new ArgumentException(CoreMessages.DimensionIsInvalid, nameof(dimension));

            try
            {
                if (geometry is IPoint)
                    return ToWellKnownBinary(geometry as IPoint, byteOrder, dimension);
                if (geometry is ILineString)
                    return ToWellKnownBinary(geometry as ILineString, byteOrder, dimension);
                if (geometry is IPolygon)
                    return ToWellKnownBinary(geometry as IPolygon, byteOrder, dimension);
                if (geometry is IMultiPoint)
                    return ToWellKnownBinary(geometry as IMultiPoint, byteOrder, dimension);
                if (geometry is IMultiLineString)
                    return ToWellKnownBinary(geometry as IMultiLineString, byteOrder, dimension);
                if (geometry is IMultiPolygon)
                    return ToWellKnownBinary(geometry as IMultiPolygon, byteOrder, dimension);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.GeometryIsInvalid, nameof(geometry), ex);
            }

            throw new ArgumentException(CoreMessages.GeometryIsNotSupported, nameof(geometry));
        }

        /// <summary>
        /// Converts a Well-known Binary (WKB) to geometry.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        /// <param name="source">The source byte array.</param>
        /// <returns>The converted geometry.</returns>
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
        public static IGeometry ToGeometry(this IGeometryFactory factory, Byte[] source)
        {
            return ToGeometry(source, factory);
        }

        /// <summary>
        /// Converts a Well-known Binary (WKB) to geometry.
        /// </summary>
        /// <param name="source">The source byte array.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted geometry.</returns>
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
        public static IGeometry ToGeometry(Byte[] source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            if (source.Length == 0)
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.GeometryFactoryIsNull);

            IGeometry resultGeometry = null;

            try
            {
                ByteOrder byteOrder = (source[0] == 1) ? ByteOrder.LittleEndian : ByteOrder.BigEndian;

                switch ((WellKnownBinaryTypes)EndianBitConverter.ToInt32(source, 1, byteOrder))
                {
                    case WellKnownBinaryTypes.Point:
                    case WellKnownBinaryTypes.PointM:
                        resultGeometry = ToPoint(source, byteOrder, 2, factory);
                        break;
                    case WellKnownBinaryTypes.PointZ:
                    case WellKnownBinaryTypes.PointZM:
                        resultGeometry = ToPoint(source, byteOrder, 3, factory);
                        break;
                    case WellKnownBinaryTypes.LineString:
                    case WellKnownBinaryTypes.LineStringM:
                        resultGeometry = ToLineString(source, byteOrder, 2, factory);
                        break;
                    case WellKnownBinaryTypes.LineStringZ:
                    case WellKnownBinaryTypes.LineStringZM:
                        resultGeometry = ToLineString(source, byteOrder, 3, factory);
                        break;
                    case WellKnownBinaryTypes.Polygon:
                    case WellKnownBinaryTypes.PolygonM:
                        resultGeometry = ToPolygon(source, byteOrder, 2, factory);
                        break;
                    case WellKnownBinaryTypes.PolygonZ:
                    case WellKnownBinaryTypes.PolygonZM:
                        resultGeometry = ToPolygon(source, byteOrder, 3, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPoint:
                    case WellKnownBinaryTypes.MultiPointM:
                        resultGeometry = ToMultiPoint(source, byteOrder, 2, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPointZ:
                    case WellKnownBinaryTypes.MultiPointZM:
                        resultGeometry = ToMultiPoint(source, byteOrder, 3, factory);
                        break;
                    case WellKnownBinaryTypes.MultiLineString:
                    case WellKnownBinaryTypes.MultiLineStringM:
                        resultGeometry = ToMultiLineString(source, byteOrder, 2, factory);
                        break;
                    case WellKnownBinaryTypes.MultiLineStringZ:
                    case WellKnownBinaryTypes.MultiLineStringZM:
                        resultGeometry = ToMultiLineString(source, byteOrder, 3, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPolygon:
                    case WellKnownBinaryTypes.MultiPolygonM:
                        resultGeometry = ToMultiPolygon(source, byteOrder, 2, factory);
                        break;
                    case WellKnownBinaryTypes.MultiPolygonZ:
                    case WellKnownBinaryTypes.MultiPolygonZM:
                        resultGeometry = ToMultiPolygon(source, byteOrder, 3, factory);
                        break;
                }
            }
            catch
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source));
            }

            if (resultGeometry == null)
                throw new ArgumentException(CoreMessages.SourceIsNotSupported, nameof(source));

            return resultGeometry;
        }

        /// <summary>
        /// Converts the point to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ToWellKnownBinary(IPoint geometry, ByteOrder byteOrder, Int32 dimension)
        {
            Byte[] geometryBytes = new Byte[dimension == 3 ? 29 : 21];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (dimension == 3)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.PointZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.Point, geometryBytes, 1, byteOrder);

            EndianBitConverter.CopyBytes(geometry.X, geometryBytes, 5, byteOrder);
            EndianBitConverter.CopyBytes(geometry.Y, geometryBytes, 13, byteOrder);

            if (dimension == 3)
                EndianBitConverter.CopyBytes(geometry.Z, geometryBytes, 21, byteOrder);

            return geometryBytes;
        }

        /// <summary>
        /// Converts the line string to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ToWellKnownBinary(ILineString geometry, ByteOrder byteOrder, Int32 dimension)
        {
            Byte[] geometryBytes = new Byte[9 + ((dimension == 3) ? 24 : 16) * geometry.Count];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (dimension == 3)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.LineStringZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.LineString, geometryBytes, 1, byteOrder);

            Int32 byteIndex = 5;
            ConvertCoordinates(geometryBytes, ref byteIndex, byteOrder, geometry, dimension);

            return geometryBytes;
        }

        /// <summary>
        /// Converts the polygon to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ToWellKnownBinary(IPolygon geometry, ByteOrder byteOrder, Int32 dimension)
        {
            Byte[] geometryBytes = new Byte[9 + 4 * (geometry.HoleCount + 1) + ((dimension == 3) ? 24 : 16) * (geometry.Shell.Count + geometry.Holes.Sum(hole => hole.Count))];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (dimension == 3)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.PolygonZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.Polygon, geometryBytes, 1, byteOrder);

            // number of rings
            EndianBitConverter.CopyBytes(geometry.HoleCount + 1, geometryBytes, 5, byteOrder);
            Int32 byteIndex = 9;

            // shell
            ConvertCoordinates(geometryBytes, ref byteIndex, byteOrder, geometry.Shell, dimension);

            // holes
            for (Int32 holeIndex = 0; holeIndex < geometry.Holes.Count; holeIndex++)
            {
                ConvertCoordinates(geometryBytes, ref byteIndex, byteOrder, geometry.Holes[holeIndex], dimension);
            }

            return geometryBytes;
        }

        /// <summary>
        /// Converts the multi point to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ToWellKnownBinary(IMultiPoint geometry, ByteOrder byteOrder, Int32 dimension)
        {
            Byte[] geometryBytes = new Byte[9 + ((dimension == 3) ? 24 : 16) * geometry.Count];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (dimension == 3)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPointZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPoint, geometryBytes, 1, byteOrder);

            // number of points
            Int32 byteIndex = 5;
            EndianBitConverter.CopyBytes(geometry.Count, geometryBytes, byteIndex, byteOrder);
            byteIndex += 4;

            for (Int32 geometryIndex = 0; geometryIndex < geometry.Count; geometryIndex++)
            {
                EndianBitConverter.CopyBytes(geometry[geometryIndex].X, geometryBytes, byteIndex, byteOrder);
                EndianBitConverter.CopyBytes(geometry[geometryIndex].Y, geometryBytes, byteIndex + 8, byteOrder);

                if (dimension == 3)
                {
                    EndianBitConverter.CopyBytes(geometry[geometryIndex].Z, geometryBytes, byteIndex + 16, byteOrder);
                    byteIndex += 24;
                }
                else
                {
                    byteIndex += 16;
                }
            }

            return geometryBytes;
        }

        /// <summary>
        /// Converts the multi line string to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ToWellKnownBinary(IMultiLineString geometry, ByteOrder byteOrder, Int32 dimension)
        {
            Byte[] geometryBytes = new Byte[9 + 4 * geometry.Count + ((dimension == 3) ? 24 : 16) * geometry.Sum(lineString => lineString.Count)];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (dimension == 3)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiLineStringZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiLineString, geometryBytes, 1, byteOrder);

            // number of line strings
            EndianBitConverter.CopyBytes(geometry.Count, geometryBytes, 5, byteOrder);
            Int32 byteIndex = 9;

            for (Int32 geometryIndex = 0; geometryIndex < geometry.Count; geometryIndex++)
            {
                ConvertCoordinates(geometryBytes, ref byteIndex, byteOrder, geometry[geometryIndex], dimension);
            }

            return geometryBytes;
        }

        /// <summary>
        /// Converts the multi polygon to Well-known Binary (WKB) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <returns>The WKB representation of the <paramref name="geometry" />.</returns>
        private static Byte[] ToWellKnownBinary(IMultiPolygon geometry, ByteOrder byteOrder, Int32 dimension)
        {
            Byte[] geometryBytes = new Byte[9 + 4 * geometry.Count +
                                            4 * geometry.Sum(polygon => polygon.HoleCount + 1) +
                                            ((dimension == 3) ? 24 : 16) * geometry.Sum(polygon => polygon.Shell.Count + polygon.Holes.Sum(hole => hole.Count))];

            if (byteOrder == ByteOrder.LittleEndian)
                geometryBytes[0] = 1;

            if (dimension == 3)
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPolygonZ, geometryBytes, 1, byteOrder);
            else
                EndianBitConverter.CopyBytes((Int32)WellKnownBinaryTypes.MultiPolygon, geometryBytes, 1, byteOrder);

            // number of polygons
            EndianBitConverter.CopyBytes(geometry.Count, geometryBytes, 5, byteOrder);
            Int32 byteIndex = 9;

            for (Int32 geometryIndex = 0; geometryIndex < geometry.Count; geometryIndex++)
            {
                // number of rings
                EndianBitConverter.CopyBytes(geometry[geometryIndex].HoleCount + 1, geometryBytes, byteIndex, byteOrder);
                byteIndex += 4;

                // shell
                ConvertCoordinates(geometryBytes, ref byteIndex, byteOrder, geometry[geometryIndex].Shell, dimension);

                // holes
                for (Int32 j = 0; j < geometry[geometryIndex].Holes.Count; j++)
                {
                    ConvertCoordinates(geometryBytes, ref byteIndex, byteOrder, geometry[geometryIndex].Holes[j], dimension);
                }
            }

            return geometryBytes;
        }

        /// <summary>
        /// Converts the Well-known Binary (WKB) to a point.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the point.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted point.</returns>
        private static IPoint ToPoint(Byte[] geometryBytes, ByteOrder byteOrder, Int32 dimension, IGeometryFactory factory)
        {
            return factory.CreatePoint(EndianBitConverter.ToDouble(geometryBytes, 5, byteOrder),
                                       EndianBitConverter.ToDouble(geometryBytes, 13, byteOrder),
                                       dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, 21, byteOrder) : 0);
        }

        /// <summary>
        /// Converts the Well-known Binary (WKB) to a line string.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the line string.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted line string.</returns>
        private static ILineString ToLineString(Byte[] geometryBytes, ByteOrder byteOrder, Int32 dimension, IGeometryFactory factory)
        {
            Int32 coordinateSize = (dimension == 3) ? 24 : 16;
            Int32 coordinateCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            Coordinate[] coordinates = new Coordinate[coordinateCount];

            for (Int32 byteIndex = 9, coordinateIndex = 0; coordinateIndex < coordinateCount; byteIndex += coordinateSize, coordinateIndex++)
            {
                coordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                              EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                              dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
            }

            return factory.CreateLineString(coordinates);
        }

        /// <summary>
        /// Converts the Well-known Binary (WKB) to a polygon.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the polygon.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted polygon.</returns>
        private static IPolygon ToPolygon(Byte[] geometryBytes, ByteOrder byteOrder, Int32 dimension, IGeometryFactory factory)
        {
            Int32 coordinateSize = (dimension == 3) ? 24 : 16;
            Int32 ringCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);
            Int32 shellCoordinateCount = EndianBitConverter.ToInt32(geometryBytes, 9, byteOrder);

            Coordinate[] shellCoordinates = new Coordinate[shellCoordinateCount];

            for (Int32 byteIndex = 13, coordinateIndex = 0; coordinateIndex < shellCoordinateCount; byteIndex += coordinateSize, coordinateIndex++)
            {
                shellCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                   EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                   dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
            }

            if (ringCount > 1)
            {
                Coordinate[][] holes = new Coordinate[ringCount - 1][];

                Int32 holeStartIndex = 13 + shellCoordinateCount * coordinateSize;

                for (Int32 ringIndex = 1; ringIndex < ringCount; ringIndex++)
                {
                    Int32 holeCoordianteCount = EndianBitConverter.ToInt32(geometryBytes, holeStartIndex, byteOrder);
                    holeStartIndex += 4;
                    Coordinate[] holeCoordinates = new Coordinate[holeCoordianteCount];

                    for (Int32 byteIndex = holeStartIndex, coordinateIndex = 0; coordinateIndex < holeCoordianteCount; byteIndex += coordinateSize, coordinateIndex++)
                    {
                        holeCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                          EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                          dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
                    }

                    holes[ringIndex - 1] = holeCoordinates;

                    holeStartIndex += holeCoordianteCount * coordinateSize;
                }

                return factory.CreatePolygon(shellCoordinates, holes);
            }
            else
            {
                return factory.CreatePolygon(shellCoordinates);
            }
        }

        /// <summary>
        /// Converts the Well-known Binary (WKB) to a multi point.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the multi point.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi point.</returns>
        private static IMultiPoint ToMultiPoint(Byte[] geometryBytes, ByteOrder byteOrder, Int32 dimension, IGeometryFactory factory)
        {
            Int32 pointSize = (dimension == 3) ? 24 : 16;
            Int32 pointCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            IPoint[] points = new IPoint[pointCount];

            for (Int32 byteIndex = 9, pointIndex = 0; pointIndex < pointCount; byteIndex += pointSize, pointIndex++)
            {
                points[pointIndex] = factory.CreatePoint(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                         EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                         dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
            }

            return factory.CreateMultiPoint(points);
        }

        /// <summary>
        /// Converts the Well-known Binary (WKB) to a multi line string.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the multi line string.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi line string.</returns>
        private static IMultiLineString ToMultiLineString(Byte[] geometryBytes, ByteOrder byteOrder, Int32 dimension, IGeometryFactory factory)
        {
            Int32 coordinateSize = (dimension == 3) ? 24 : 16;
            Int32 lineStringCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            ILineString[] lineStrings = new ILineString[lineStringCount];

            Int32 lineStringStartIndex = 9;
            for (Int32 lineStringIndex = 0; lineStringIndex < lineStringCount; lineStringIndex++)
            {
                Int32 coordinateCount = EndianBitConverter.ToInt32(geometryBytes, lineStringStartIndex, byteOrder);
                lineStringStartIndex += 4;

                Coordinate[] coordinates = new Coordinate[coordinateCount];

                for (Int32 byteIndex = lineStringStartIndex, coordinateIndex = 0; coordinateIndex < coordinateCount; byteIndex += coordinateSize, coordinateIndex++)
                {
                    coordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                  EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                  dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
                }

                lineStrings[lineStringIndex] = factory.CreateLineString(coordinates);

                lineStringStartIndex += coordinates.Length * coordinateSize;
            }

            return factory.CreateMultiLineString(lineStrings);
        }

        /// <summary>
        /// Converts the Well-known Binary (WKB) to a multi polygon.
        /// </summary>
        /// <param name="geometryBytes">The WKB representation of the multi polygon.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi polygon.</returns>
        private static IMultiPolygon ToMultiPolygon(Byte[] geometryBytes, ByteOrder byteOrder, Int32 dimension, IGeometryFactory factory)
        {
            Int32 coordinateSize = (dimension == 3) ? 24 : 16;
            Int32 polygonCount = EndianBitConverter.ToInt32(geometryBytes, 5, byteOrder);

            IPolygon[] polygons = new Polygon[polygonCount];

            Int32 startIndex = 9;
            for (Int32 polygonIndex = 0; polygonIndex < polygonCount; polygonIndex++)
            {
                // number of rings
                Int32 ringCount = EndianBitConverter.ToInt32(geometryBytes, startIndex, byteOrder);

                // shells
                Int32 shellCoordinateCount = EndianBitConverter.ToInt32(geometryBytes, startIndex + 4, byteOrder);

                startIndex += 8;

                Coordinate[] shellCoordinates = new Coordinate[shellCoordinateCount];

                for (Int32 byteIndex = startIndex, coordinateIndex = 0; coordinateIndex < shellCoordinateCount; byteIndex += coordinateSize, coordinateIndex++)
                {
                    shellCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                       EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                       dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
                }

                startIndex += shellCoordinateCount * coordinateSize;

                if (ringCount > 1)
                {
                    // holes
                    Coordinate[][] holes = new Coordinate[ringCount - 1][];

                    for (Int32 ringIndex = 1; ringIndex < ringCount; ringIndex++)
                    {
                        Int32 holeCoordianteCount = EndianBitConverter.ToInt32(geometryBytes, startIndex, byteOrder);
                        startIndex += 4;

                        Coordinate[] holeCoordinates = new Coordinate[holeCoordianteCount];

                        for (Int32 byteIndex = startIndex, coordinateIndex = 0; coordinateIndex < holeCoordianteCount; byteIndex += coordinateSize, coordinateIndex++)
                        {
                            holeCoordinates[coordinateIndex] = new Coordinate(EndianBitConverter.ToDouble(geometryBytes, byteIndex, byteOrder),
                                                                              EndianBitConverter.ToDouble(geometryBytes, byteIndex + 8, byteOrder),
                                                                              dimension == 3 ? EndianBitConverter.ToDouble(geometryBytes, byteIndex + 16, byteOrder) : 0);
                        }

                        holes[ringIndex - 1] = holeCoordinates;

                        startIndex += holeCoordianteCount * coordinateSize;
                    }

                    polygons[polygonIndex] = factory.CreatePolygon(shellCoordinates, holes);
                }
                else
                {
                    polygons[polygonIndex] = factory.CreatePolygon(shellCoordinates);
                }
            }

            return factory.CreateMultiPolygon(polygons);
        }

        /// <summary>
        /// Converts the coordinates to Well-known Binary representation.
        /// </summary>
        /// <param name="byteArray">The byte-array.</param>
        /// <param name="byteIndex">The starting index.</param>
        /// <param name="byteOrder">The byte-order of the conversion.</param>
        /// <param name="coordinateList">The coordinate list.</param>
        /// <param name="dimension">The dimension of the coordinates.</param>
        private static void ConvertCoordinates(Byte[] byteArray, ref Int32 byteIndex, ByteOrder byteOrder, IReadOnlyList<Coordinate> coordinateList, Int32 dimension)
        {
            // number of coordinates
            EndianBitConverter.CopyBytes(coordinateList.Count, byteArray, byteIndex, byteOrder);
            byteIndex += 4;

            // coordinates
            for (Int32 coordIndex = 0; coordIndex < coordinateList.Count; coordIndex++)
            {
                EndianBitConverter.CopyBytes(coordinateList[coordIndex].X, byteArray, byteIndex, byteOrder);
                EndianBitConverter.CopyBytes(coordinateList[coordIndex].Y, byteArray, byteIndex + 8, byteOrder);

                if (dimension == 3)
                {
                    EndianBitConverter.CopyBytes(coordinateList[coordIndex].Z, byteArray, byteIndex + 16, byteOrder);
                    byteIndex += 24;
                }
                else
                {
                    byteIndex += 16;
                }
            }
        }
    }
}
