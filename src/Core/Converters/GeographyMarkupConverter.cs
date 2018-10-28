// <copyright file="GeographyMarkupConverter.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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
    using System.Xml.Linq;
    using AEGIS.Geometries;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a converter for Geographic Markup Language (GML).
    /// </summary>
    public static class GeographyMarkupConverter
    {
        /// <summary>
        /// The reference system URI. This field is constant.
        /// </summary>
        private const String ReferenceSystemUri = "http://www.opengis.net/def/crs/EPSG/0/";

        /// <summary>
        /// The GML namespace. This field is read-only.
        /// </summary>
        private static readonly XNamespace Namespace = "http://www.opengis.net/gml/";

        /// <summary>
        /// Converts the geometry to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static String ToMarkup(this IGeometry geometry)
        {
            XElement element = ToMarkupElement(geometry);
            element.Add(new XAttribute(XNamespace.Xmlns + "gml", Namespace));

            return element.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces);
        }

        /// <summary>
        /// Converts the geometry to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static String ToMarkup(this IGeometry geometry, String identifier)
        {
            XElement element = ToMarkupElement(geometry, identifier);
            element.Add(new XAttribute(XNamespace.Xmlns + "gml", Namespace));

            return element.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces);
        }

        /// <summary>
        /// Converts the geometry to <see cref="XElement"/> in Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static XElement ToMarkupElement(this IGeometry geometry)
        {
            return ToMarkupElement(geometry, null);
        }

        /// <summary>
        /// Converts the geometry to <see cref="XElement"/> in Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted geometry.</returns>
        /// <remarks>
        /// The <see cref="XElement" /> does not contain the namespace attribute for <code>gml</code>
        /// (<code>xmlns:gml="http://www.opengis.net/gml/"</code>), which should be added to a parent node before usage.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified geometry is invalid.
        /// or
        /// The specified geometry is not supported.
        /// </exception>
        public static XElement ToMarkupElement(this IGeometry geometry, String identifier)
        {
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry));

            try
            {
                if (geometry is IPoint)
                    return ToMarkupInternal(geometry as IPoint, false, identifier);
                if (geometry is ILinearRing)
                    return ToMarkupInternal(geometry as ILinearRing, false, identifier);
                if (geometry is ILineString)
                    return ToMarkupInternal(geometry as ILineString, false, identifier);
                if (geometry is IPolygon)
                    return ToMarkupInternal(geometry as IPolygon, false, identifier);
                if (geometry is IMultiPoint)
                    return ToMarkupInternal(geometry as IMultiPoint, false, identifier);
                if (geometry is IMultiLineString)
                    return ToMarkupInternal(geometry as IMultiLineString, false, identifier);
                if (geometry is IMultiPolygon)
                    return ToMarkupInternal(geometry as IMultiPolygon, false, identifier);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.GeometryIsInvalid, nameof(geometry), ex);
            }

            throw new ArgumentException(CoreMessages.GeometryIsNotSupported, nameof(geometry));
        }

        /// <summary>
        /// Converts the geometry from Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="source">The source XML element.</param>
        /// <param name="geometryFactory">The geometry factory.</param>
        /// <param name="referenceSystemFactory">The reference system factory.</param>
        /// <returns>The converted geometry.</returns>
        /// <remarks>
        /// The <see cref="XElement" /> does not contain the namespace attribute for <code>gml</code>
        /// (<code>xmlns:gml="http://www.opengis.net/gml/"</code>), which should be added to a parent node before usage.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// or
        /// The reference system factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IGeometry ToGeometry(this XElement source, IGeometryFactory geometryFactory, IReferenceSystemFactory referenceSystemFactory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (geometryFactory == null)
                throw new ArgumentNullException(nameof(geometryFactory));
            if (referenceSystemFactory == null)
                throw new ArgumentNullException(nameof(referenceSystemFactory));

            IReferenceSystem referenceSystem = GetReferenceSystem(source.Attribute("srsName"), referenceSystemFactory);

            return ToGeometry(source, geometryFactory.WithReferenceSystem(referenceSystem));
        }

        /// <summary>
        /// Converts the geometry from Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="source">The source XML element.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IGeometry ToGeometry(this XElement source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            IGeometry resultGeometry = null;

            try
            {
                switch (source.Name.LocalName)
                {
                    case "Point":
                        resultGeometry = ToPoint(source, factory);
                        break;
                    case "LineString":
                        resultGeometry = ToLineString(source, factory);
                        break;
                    case "LinearRing":
                        resultGeometry = ToLinearRing(source, factory);
                        break;
                    case "Polygon":
                        resultGeometry = ToPolygon(source, factory);
                        break;
                    case "MultiPoint":
                        resultGeometry = ToMultiPoint(source, factory);
                        break;
                    case "MultiLineString":
                        resultGeometry = ToMultiLineString(source, factory);
                        break;
                    case "MultiPolygon":
                        resultGeometry = ToMultiPolygon(source, factory);
                        break;
                    case "GeomCollection":
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }

            return resultGeometry;
        }

        /// <summary>
        /// Converts the geometry from Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="source">The source collection of XML elements.</param>
        /// <param name="geometryFactory">The geometry factory.</param>
        /// <param name="referenceSystemFactory">The reference system factory.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// or
        /// The reference system factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IGeometry ToGeometry(this IEnumerable<XElement> source, IGeometryFactory geometryFactory, IReferenceSystemFactory referenceSystemFactory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (geometryFactory == null)
                throw new ArgumentNullException(nameof(geometryFactory));
            if (referenceSystemFactory == null)
                throw new ArgumentNullException(nameof(referenceSystemFactory));

            try
            {
                List<IGeometry> geometries = new List<IGeometry>();

                foreach (XElement element in source)
                {
                    geometries.Add(ToGeometry(element, geometryFactory, referenceSystemFactory));
                }

                if (geometries.Count == 0)
                    return null;

                if (geometries.Count == 1)
                    return geometries[0];

                if (geometries.All(geometry => geometry is IPoint))
                    return geometryFactory.CreateMultiPoint(geometries.Cast<IPoint>());

                if (geometries.All(geometry => geometry is ILineString))
                    return geometryFactory.CreateMultiLineString(geometries.Cast<ILineString>());

                if (geometries.All(geometry => geometry is IPolygon))
                    return geometryFactory.CreateMultiPolygon(geometries.Cast<IPolygon>());

                return geometryFactory.CreateGeometryCollection(geometries);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }
        }

        /// <summary>
        /// Converts the geometry from Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="source">The source collection of XML elements.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IGeometry ToGeometry(this IEnumerable<XElement> source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            try
            {
                List<IGeometry> geometries = new List<IGeometry>();

                foreach (XElement element in source)
                {
                    geometries.Add(ToGeometry(element, factory));
                }

                if (geometries.Count == 0)
                    return null;

                if (geometries.Count == 1)
                    return geometries[0];

                if (geometries.All(geometry => geometry is IPoint))
                    return factory.CreateMultiPoint(geometries.Cast<IPoint>());

                if (geometries.All(geometry => geometry is ILineString))
                    return factory.CreateMultiLineString(geometries.Cast<ILineString>());

                if (geometries.All(geometry => geometry is IPolygon))
                    return factory.CreateMultiPolygon(geometries.Cast<IPolygon>());

                return factory.CreateGeometryCollection(geometries);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }
        }

        /// <summary>
        /// Converts the geometry from Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="geometryFactory">The geometry factory.</param>
        /// <param name="referenceSystemFactory">The reference system factory.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// or
        /// The reference system factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IGeometry ToGeometry(this String source, IGeometryFactory geometryFactory, IReferenceSystemFactory referenceSystemFactory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (geometryFactory == null)
                throw new ArgumentNullException(nameof(geometryFactory));
            if (referenceSystemFactory == null)
                throw new ArgumentNullException(nameof(referenceSystemFactory));

            try
            {
                XDocument doc = XDocument.Parse(source);

                return ToGeometry(doc.Elements(), geometryFactory, referenceSystemFactory);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }
        }

        /// <summary>
        /// Converts the geometry from Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted geometry.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The geometry factory is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IGeometry ToGeometry(this String source, IGeometryFactory factory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            try
            {
                XDocument doc = XDocument.Parse(source);

                return ToGeometry(doc.Elements(), factory);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }
        }

        /// <summary>
        /// Converts the point to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="partial">Indicates that the geometry is part of another.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted point.</returns>
        private static XElement ToMarkupInternal(IPoint point, Boolean partial, String identifier = null)
        {
            XElement element = new XElement(Namespace + "Point");

            ConvertIdentifier(element, identifier);

            if (!partial)
                ConvertReferenceSystem(element, point.ReferenceSystem, point.CoordinateDimension);

            if (point.CoordinateDimension == 3)
                element.Add(new XElement(Namespace + "pos", point.X.ToString("G", CultureInfo.InvariantCulture) + " " + point.Y.ToString("G", CultureInfo.InvariantCulture) + " " + point.Z.ToString("G", CultureInfo.InvariantCulture)));
            else
                element.Add(new XElement(Namespace + "pos", point.X.ToString("G", CultureInfo.InvariantCulture) + " " + point.Y.ToString("G", CultureInfo.InvariantCulture)));

            return element;
        }

        /// <summary>
        /// Converts the line string to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <param name="partial">Indicates that the geometry is part of another.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted line string.</returns>
        private static XElement ToMarkupInternal(ILineString lineString, Boolean partial, String identifier = null)
        {
            XElement element = new XElement(Namespace + "LineString");

            ConvertIdentifier(element, identifier);

            if (!partial)
                ConvertReferenceSystem(element, lineString.ReferenceSystem, lineString.CoordinateDimension);

            ConvertCoordinates(element, lineString);

            return element;
        }

        /// <summary>
        /// Converts the linear ring to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="geometry">The linear ring.</param>
        /// <param name="partial">Indicates that the geometry is part of another.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted linear ring.</returns>
        private static XElement ToMarkupInternal(ILinearRing geometry, Boolean partial, String identifier = null)
        {
            XElement element = new XElement(Namespace + "LinearRing");

            ConvertIdentifier(element, identifier);

            if (!partial)
                ConvertReferenceSystem(element, geometry.ReferenceSystem, geometry.CoordinateDimension);

            ConvertCoordinates(element, geometry);

            return element;
        }

        /// <summary>
        /// Converts the polygon to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="partial">Indicates that the geometry is part of another.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted polygon.</returns>
        private static XElement ToMarkupInternal(IPolygon polygon, Boolean partial, String identifier = null)
        {
            XElement element = new XElement(Namespace + "Polygon");

            ConvertIdentifier(element, identifier);

            if (!partial)
                ConvertReferenceSystem(element, polygon.ReferenceSystem, polygon.CoordinateDimension);

            element.Add(new XElement(Namespace + "outerBoundaryIs", ToMarkupInternal(polygon.Shell, true)));

            foreach (ILinearRing hole in polygon.Holes)
            {
                element.Add(new XElement(Namespace + "innerBoundaryIs", ToMarkupInternal(hole, true)));
            }

            return element;
        }

        /// <summary>
        /// Converts the multi point to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="multiPoint">The multi point.</param>
        /// <param name="partial">Indicates that the geometry is part of another.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted multi point.</returns>
        private static XElement ToMarkupInternal(IMultiPoint multiPoint, Boolean partial, String identifier = null)
        {
            XElement element = new XElement(Namespace + "MultiPoint");

            ConvertIdentifier(element, identifier);

            if (!partial)
                ConvertReferenceSystem(element, multiPoint.ReferenceSystem, multiPoint.CoordinateDimension);

            ConvertCoordinates(element, multiPoint.Select(point => point.Coordinate));

            return element;
        }

        /// <summary>
        /// Converts the multi line string to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="multiLineString">The multi line string.</param>
        /// <param name="partial">Indicates that the geometry is part of another.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted multi line string.</returns>
        private static XElement ToMarkupInternal(IMultiLineString multiLineString, Boolean partial, String identifier = null)
        {
            XElement element = new XElement(Namespace + "MultiLineString");

            ConvertIdentifier(element, identifier);

            if (!partial)
                ConvertReferenceSystem(element, multiLineString.ReferenceSystem, multiLineString.CoordinateDimension);

            foreach (ILineString lineString in multiLineString)
            {
                element.Add(new XElement(Namespace + "LineStringMember", ToMarkupInternal(lineString, true)));
            }

            return element;
        }

        /// <summary>
        /// Converts the multi polygon to Geography Markup Language (GML) representation.
        /// </summary>
        /// <param name="multiPolygon">The multi polygon.</param>
        /// <param name="partial">Indicates that the geometry is part of another.</param>
        /// <param name="identifier">The geometry identifier.</param>
        /// <returns>The converted multi polygon.</returns>
        private static XElement ToMarkupInternal(IMultiPolygon multiPolygon, Boolean partial, String identifier = null)
        {
            XElement element = new XElement(Namespace + "MultiPolygon");

            ConvertIdentifier(element, identifier);

            if (!partial)
                ConvertReferenceSystem(element, multiPolygon.ReferenceSystem, multiPolygon.CoordinateDimension);

            foreach (IPolygon polygon in multiPolygon)
            {
                element.Add(new XElement(Namespace + "PolygonMember", ToMarkupInternal(polygon, true)));
            }

            return element;
        }

        /// <summary>
        /// Converts the GML element to point.
        /// </summary>
        /// <param name="element">The GML element of the point.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted point.</returns>
        private static IPoint ToPoint(XElement element, IGeometryFactory factory)
        {
            if (element.Elements() == null)
                return factory.CreatePoint(Coordinate.Undefined);

            foreach (XElement innerElement in element.Elements())
            {
                if (innerElement.Name.LocalName == "coordinates")
                    factory.CreatePoint(ConvertCoordinate(innerElement.Value.Split(',')));

                if (innerElement.Name.LocalName == "coord" ||
                    innerElement.Name.LocalName == "pos")
                    factory.CreatePoint(ConvertCoordinate(innerElement.Value.Split(' ')));
            }

            return factory.CreatePoint(Coordinate.Undefined);
        }

        /// <summary>
        /// Converts the GML element to line string.
        /// </summary>
        /// <param name="element">The GML element of the line string.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted line string.</returns>
        private static ILineString ToLineString(XElement element, IGeometryFactory factory)
        {
            if (element.Elements() == null)
                return factory.CreateLineString();

            List<Coordinate> coordinates = new List<Coordinate>();

            foreach (XElement innerElement in element.Elements())
            {
                switch (innerElement.Name.LocalName)
                {
                    case "coordinates":
                        return factory.CreateLineString(ConvertCoordinates(innerElement));
                    case "posList":
                        return factory.CreateLineString(ConvertPosList(innerElement, GetDimension(element, innerElement)));
                    case "coord":
                        coordinates.Add(ConvertCoordinate(innerElement.Value.Split(',')));
                        break;
                }
            }

            return factory.CreateLineString(coordinates);
        }

        /// <summary>
        /// Converts the GML element to linear ring.
        /// </summary>
        /// <param name="element">The GML element of the linear ring.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted linear ring.</returns>
        private static ILinearRing ToLinearRing(XElement element, IGeometryFactory factory)
        {
            if (element.Elements() == null)
                return factory.CreateLinearRing();

            List<Coordinate> coordinates = new List<Coordinate>();

            foreach (XElement innerElement in element.Elements())
            {
                switch (innerElement.Name.LocalName)
                {
                    case "coordinates":
                        return factory.CreateLinearRing(ConvertCoordinates(innerElement));
                    case "posList":
                        return factory.CreateLinearRing(ConvertPosList(innerElement, GetDimension(element, innerElement)));
                    case "coord":
                        coordinates.Add(ConvertCoordinate(innerElement.Value.Split(',')));
                        break;
                }
            }

            return factory.CreateLinearRing(coordinates);
        }

        /// <summary>
        /// Converts the GML element to polygon.
        /// </summary>
        /// <param name="element">The GML element of the polygon.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted polygon.</returns>
        private static IPolygon ToPolygon(XElement element, IGeometryFactory factory)
        {
            if (element.Elements() == null)
                return factory.CreatePolygon(new Coordinate[0]);

            ILinearRing shell = null;
            List<ILinearRing> holes = new List<ILinearRing>();

            foreach (XElement innerElement in element.Elements())
            {
                switch (innerElement.Name.LocalName)
                {
                    case "outerBoundaryIs":
                        shell = ToLinearRing(innerElement.Elements().FirstOrDefault(), factory);
                        break;
                    case "innerBoundaryIs":
                        holes.Add(ToLinearRing(innerElement.Elements().FirstOrDefault(), factory));
                        break;
                }
            }

            return factory.CreatePolygon(shell, holes);
        }

        /// <summary>
        /// Converts the GML element to multi point.
        /// </summary>
        /// <param name="element">The GML element of the multi point.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi point.</returns>
        private static IMultiPoint ToMultiPoint(XElement element, IGeometryFactory factory)
        {
            if (element.Elements() == null)
                return factory.CreateMultiPoint();

            List<Coordinate> coordinates = new List<Coordinate>();

            foreach (XElement innerElement in element.Elements())
            {
                switch (innerElement.Name.LocalName)
                {
                    case "coordinates":
                        return factory.CreateMultiPoint(ConvertCoordinates(innerElement));
                    case "posList":
                        return factory.CreateMultiPoint(ConvertPosList(innerElement, GetDimension(element, innerElement)));
                    case "coord":
                        coordinates.Add(ConvertCoordinate(innerElement.Value.Split(',')));
                        break;
                }
            }

            return factory.CreateMultiPoint(coordinates);
        }

        /// <summary>
        /// Converts the GML element to multi line string.
        /// </summary>
        /// <param name="element">The GML element of the multi line string.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi line string.</returns>
        private static IMultiLineString ToMultiLineString(XElement element, IGeometryFactory factory)
        {
            if (element.Elements() == null)
                return factory.CreateMultiLineString();

            List<ILineString> lineStrings = new List<ILineString>();

            return factory.CreateMultiLineString(element.Elements()
                                                        .Where(innerElement => innerElement.Name.LocalName == "LineStringMember")
                                                        .Select(innerElement => ToLineString(innerElement.Elements().FirstOrDefault(), factory)));
        }

        /// <summary>
        /// Converts the GML element to multi polygon.
        /// </summary>
        /// <param name="element">The GML element of the multi polygon.</param>
        /// <param name="factory">The geometry factory.</param>
        /// <returns>The converted multi polygon.</returns>
        private static IMultiPolygon ToMultiPolygon(XElement element, IGeometryFactory factory)
        {
            if (element.Elements() == null)
                return factory.CreateMultiPolygon();

            List<ILineString> lineStrings = new List<ILineString>();

            return factory.CreateMultiPolygon(element.Elements()
                                                     .Where(innerElement => innerElement.Name.LocalName == "PolygonMember")
                                                     .Select(innerElement => ToPolygon(innerElement.Elements().FirstOrDefault(), factory)));
        }

        /// <summary>
        /// Converts the geometry identifier to GML representation.
        /// </summary>
        /// <param name="element">The XML element.</param>
        /// <param name="identifier">The geometry identifier.</param>
        private static void ConvertIdentifier(XElement element, String identifier)
        {
            if (String.IsNullOrEmpty(identifier))
                return;

            element.Add(new XAttribute(Namespace + "id", identifier));
        }

        /// <summary>
        /// Converts the reference system to GML representation.
        /// </summary>
        /// <param name="element">The XML element.</param>
        /// <param name="referenceSystem">The reference system of the geometry.</param>
        /// <param name="dimension">The coordinate dimension of the geometry.</param>
        private static void ConvertReferenceSystem(XElement element, IReferenceSystem referenceSystem, Int32 dimension)
        {
            if (referenceSystem == null)
                return;

            element.Add(new XAttribute("srsName", ReferenceSystemUri + referenceSystem.Code));
            element.Add(new XAttribute("srsDimension", dimension));
        }

        /// <summary>
        /// Converts the coordinates to GML representation.
        /// </summary>
        /// <param name="element">The XML element.</param>
        /// <param name="coordinates">The list of coordinates.</param>
        private static void ConvertCoordinates(XElement element, IEnumerable<Coordinate> coordinates)
        {
            StringBuilder builder = new StringBuilder();
            Boolean firstCoordinate = true;

            foreach (Coordinate coordinate in coordinates)
            {
                if (!firstCoordinate)
                    builder.Append(" ");

                builder.Append(coordinate.X.ToString("G", CultureInfo.InvariantCulture));
                builder.Append(" ");
                builder.Append(coordinate.Y.ToString("G", CultureInfo.InvariantCulture));

                firstCoordinate = false;
            }

            element.Add(new XElement(Namespace + "posList", builder.ToString()));
        }

        /// <summary>
        /// Converts the coordinate from the specified array.
        /// </summary>
        /// <param name="array">The array representation of the coordinate.</param>
        /// <returns>The coordinate.</returns>
        private static Coordinate ConvertCoordinate(String[] array)
        {
            if (array.Length == 2)
            {
                return new Coordinate(Double.Parse(array[0].TrimStart(), CultureInfo.InvariantCulture),
                                      Double.Parse(array[1].TrimStart(), CultureInfo.InvariantCulture));
            }
            else
            {
                return new Coordinate(Double.Parse(array[0].TrimStart(), CultureInfo.InvariantCulture),
                                      Double.Parse(array[1].TrimStart(), CultureInfo.InvariantCulture),
                                      Double.Parse(array[2].TrimStart(), CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Converts the coordinate from GML representation.
        /// </summary>
        /// <param name="coordinateElement">The element to read the coordinate from.</param>
        /// <returns>The coordinate.</returns>
        private static Coordinate ConvertCoordinate(XElement coordinateElement)
        {
            String coordX = null, coordY = null, coordZ = null;

            foreach (XElement element in coordinateElement.Elements())
            {
                if (element.Name.LocalName == "X")
                    coordX = element.Value;

                if (element.Name.LocalName == "Y")
                    coordY = element.Value;

                if (element.Name.LocalName == "Z")
                    coordZ = element.Value;
            }

            if (coordX == null || coordY == null)
                return Coordinate.Undefined;

            if (coordZ == null)
            {
                return new Coordinate(Double.Parse(coordX, CultureInfo.InvariantCulture),
                                      Double.Parse(coordY, CultureInfo.InvariantCulture));
            }

            return new Coordinate(Double.Parse(coordX, CultureInfo.InvariantCulture),
                                  Double.Parse(coordY, CultureInfo.InvariantCulture),
                                  Double.Parse(coordZ, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts the coordinates from GML representation.
        /// </summary>
        /// <param name="element">The element to read the coordinates from.</param>
        /// <returns>The collection of coordinates.</returns>
        private static IEnumerable<Coordinate> ConvertCoordinates(XElement element)
        {
            return element.Value.Replace(", ", ",").Split(' ').Select(text => text.Split(',')).Select(array => ConvertCoordinate(array));
        }

        /// <summary>
        /// Converts the coordinates from GML representation.
        /// </summary>
        /// <param name="element">The element to read the coordinates from.</param>
        /// <param name="dimension">The dimension of the coordinates.</param>
        /// <returns>The collection of coordinates.</returns>
        private static IEnumerable<Coordinate> ConvertPosList(XElement element, Int32 dimension)
        {
            String[] coordinates = element.Value.Split(' ', ',');

            for (Int32 coordIndex = 0; coordIndex < coordinates.Length; coordIndex += dimension)
            {
                if (dimension == 2)
                {
                    yield return new Coordinate(Double.Parse(coordinates[coordIndex], CultureInfo.InvariantCulture),
                                                Double.Parse(coordinates[coordIndex + 1], CultureInfo.InvariantCulture));
                }
                else
                {
                    yield return new Coordinate(Double.Parse(coordinates[coordIndex], CultureInfo.InvariantCulture),
                                                Double.Parse(coordinates[coordIndex + 1], CultureInfo.InvariantCulture),
                                                Double.Parse(coordinates[coordIndex + 2], CultureInfo.InvariantCulture));
                }
            }
        }

        /// <summary>
        /// Returns the reference system specified by the attribute.
        /// </summary>
        /// <param name="attribute">The XML attribute.</param>
        /// <param name="referenceSystemFactory">The reference system factory.</param>
        /// <returns>The reference system.</returns>
        private static IReferenceSystem GetReferenceSystem(XAttribute attribute, IReferenceSystemFactory referenceSystemFactory)
        {
            if (attribute == null || referenceSystemFactory == null)
                return null;

            if (!attribute.Value.Contains(ReferenceSystemUri))
                return null;

            return referenceSystemFactory.CreateReferenceSystemFromIdentifier("EPSG::" + attribute.Value.Remove(0, ReferenceSystemUri.Length));
        }

        /// <summary>
        /// Returns the dimensions stored in the specified elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns>The last dimension value within the specified elements; if no dimension is specified, <c>2</c>.</returns>
        private static Int32 GetDimension(params XElement[] elements)
        {
            XAttribute dimensionAttribute = elements.Select(element => element.Attribute("srsDimension")).LastOrDefault(attrbiute => attrbiute != null);

            if (dimensionAttribute == null)
                return 2;

            return Int32.Parse(dimensionAttribute.Value);
        }
    }
}
