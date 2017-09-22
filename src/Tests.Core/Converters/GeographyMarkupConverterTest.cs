// <copyright file="GeographyMarkupConverterTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Converters
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using AEGIS.Converters;
    using AEGIS.Geometries;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class GeographyMarkupConverterTest
    {
        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory factory;

        /// <summary>
        /// The array of geometries.
        /// </summary>
        private IGeometry[] geometries;

        /// <summary>
        /// The array of geometries with reference system.
        /// </summary>
        private IGeometry[] geometriesWithReference;

        /// <summary>
        /// The array of GML string reprensentations of geometries.
        /// </summary>
        private String[] geometriesMarkup;

        /// <summary>
        /// The feature identifier.
        /// </summary>
        private String identifier;

        /// <summary>
        /// The array of GML string reprensentations of geometries with feature identifiers.
        /// </summary>
        private String[] geometriesMarkupWithIdentifier;

        /// <summary>
        /// The array of GML string reprensentations of geometries with reference system.
        /// </summary>
        private String[] geometriesMarkupWithReference;

        /// <summary>
        /// The geometry comparer.
        /// </summary>
        private GeometryComparer geometryComparer;

        /// <summary>
        /// The mock reference system factory.
        /// </summary>
        private Mock<IReferenceSystemFactory> mockReferenceSystemFactory;

        /// <summary>
        /// The mock reference system.
        /// </summary>
        private Mock<IReferenceSystem> mockReferenceSystem;

        [SetUp]
        public void SetUp()
        {
            this.factory = new GeometryFactory();

            this.geometries = new IGeometry[]
            {
                this.factory.CreatePoint(1.5, 1.5),
                this.factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(-i / 2.0, i / 2.0))),
                this.factory.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)),
                    new Coordinate[][]
                    {
                        Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray(),
                        Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray()
                    }),
                this.factory.CreateMultiPoint(Enumerable.Range(1, 4).Select(i => this.factory.CreatePoint(i / 2.0, i / 2.0))),
                this.factory.CreateMultiLineString(new ILineString[]
                    {
                        this.factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i))),
                        this.factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i / 2.0, i / 2.0)))
                    }),
                this.factory.CreateMultiPolygon(new IPolygon[]
                {
                    this.factory.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)),
                        new Coordinate[][]
                        {
                            Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray(),
                            Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray()
                        }),
                    this.factory.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i / 2.0, i / 2.0)))
                })
            };

            this.mockReferenceSystem = new Mock<IReferenceSystem>(MockBehavior.Strict);
            this.mockReferenceSystem.Setup(referenceSystem => referenceSystem.Code).Returns(1000);
            this.mockReferenceSystem.Setup(referenceSystem => referenceSystem.Dimension).Returns(2);

            this.mockReferenceSystemFactory = new Mock<IReferenceSystemFactory>(MockBehavior.Strict);
            this.mockReferenceSystemFactory.Setup(factory => factory.CreateReferenceSystemFromIdentifier("EPSG::1000")).Returns(this.mockReferenceSystem.Object);

            GeometryFactory factoryWithReference = new GeometryFactory(this.mockReferenceSystem.Object);

            this.geometriesWithReference = new IGeometry[]
            {
                factoryWithReference.CreatePoint(1.5, 1.5),
                factoryWithReference.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(-i / 2.0, i / 2.0))),
                factoryWithReference.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)),
                    new Coordinate[][]
                    {
                        Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray(),
                        Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray()
                    }),
                factoryWithReference.CreateMultiPoint(Enumerable.Range(1, 4).Select(i => this.factory.CreatePoint(i / 2.0, i / 2.0))),
                factoryWithReference.CreateMultiLineString(new ILineString[]
                    {
                        this.factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i))),
                        this.factory.CreateLineString(Enumerable.Range(1, 4).Select(i => new Coordinate(i / 2.0, i / 2.0)))
                    }),
                factoryWithReference.CreateMultiPolygon(new IPolygon[]
                {
                    factoryWithReference.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)),
                        new Coordinate[][]
                        {
                            Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray(),
                            Enumerable.Range(1, 4).Select(i => new Coordinate(i, i)).ToArray()
                        }),
                    factoryWithReference.CreatePolygon(Enumerable.Range(1, 4).Select(i => new Coordinate(i / 2.0, i / 2.0)))
                })
            };

            this.geometriesMarkup = new String[]
            {
                "<gml:Point xmlns:gml=\"http://www.opengis.net/gml/\"><gml:pos>1.5 1.5</gml:pos></gml:Point>",
                "<gml:LineString xmlns:gml=\"http://www.opengis.net/gml/\"><gml:posList>-0.5 0.5 -1 1 -1.5 1.5 -2 2</gml:posList></gml:LineString>",
                "<gml:Polygon xmlns:gml=\"http://www.opengis.net/gml/\"><gml:outerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:outerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs></gml:Polygon>",
                "<gml:MultiPoint xmlns:gml=\"http://www.opengis.net/gml/\"><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2</gml:posList></gml:MultiPoint>",
                "<gml:MultiLineString xmlns:gml=\"http://www.opengis.net/gml/\"><gml:LineStringMember><gml:LineString><gml:posList>1 1 2 2 3 3 4 4</gml:posList></gml:LineString></gml:LineStringMember><gml:LineStringMember><gml:LineString><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2</gml:posList></gml:LineString></gml:LineStringMember></gml:MultiLineString>",
                "<gml:MultiPolygon xmlns:gml=\"http://www.opengis.net/gml/\"><gml:PolygonMember><gml:Polygon><gml:outerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:outerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs></gml:Polygon></gml:PolygonMember><gml:PolygonMember><gml:Polygon><gml:outerBoundaryIs><gml:LinearRing><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2 0.5 0.5</gml:posList></gml:LinearRing></gml:outerBoundaryIs></gml:Polygon></gml:PolygonMember></gml:MultiPolygon>"
            };

            this.identifier = "12345678";

            this.geometriesMarkupWithIdentifier = new String[]
            {
                "<gml:Point gml:id=\"12345678\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:pos>1.5 1.5</gml:pos></gml:Point>",
                "<gml:LineString gml:id=\"12345678\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:posList>-0.5 0.5 -1 1 -1.5 1.5 -2 2</gml:posList></gml:LineString>",
                "<gml:Polygon gml:id=\"12345678\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:outerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:outerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs></gml:Polygon>",
                "<gml:MultiPoint gml:id=\"12345678\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2</gml:posList></gml:MultiPoint>",
                "<gml:MultiLineString gml:id=\"12345678\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:LineStringMember><gml:LineString><gml:posList>1 1 2 2 3 3 4 4</gml:posList></gml:LineString></gml:LineStringMember><gml:LineStringMember><gml:LineString><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2</gml:posList></gml:LineString></gml:LineStringMember></gml:MultiLineString>",
                "<gml:MultiPolygon gml:id=\"12345678\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:PolygonMember><gml:Polygon><gml:outerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:outerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs></gml:Polygon></gml:PolygonMember><gml:PolygonMember><gml:Polygon><gml:outerBoundaryIs><gml:LinearRing><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2 0.5 0.5</gml:posList></gml:LinearRing></gml:outerBoundaryIs></gml:Polygon></gml:PolygonMember></gml:MultiPolygon>"
            };

            this.geometriesMarkupWithReference = new String[]
            {
                "<gml:Point srsName=\"http://www.opengis.net/def/crs/EPSG/0/1000\" srsDimension=\"2\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:pos>1.5 1.5</gml:pos></gml:Point>",
                "<gml:LineString srsName=\"http://www.opengis.net/def/crs/EPSG/0/1000\" srsDimension=\"2\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:posList>-0.5 0.5 -1 1 -1.5 1.5 -2 2</gml:posList></gml:LineString>",
                "<gml:Polygon srsName=\"http://www.opengis.net/def/crs/EPSG/0/1000\" srsDimension=\"2\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:outerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:outerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs></gml:Polygon>",
                "<gml:MultiPoint srsName=\"http://www.opengis.net/def/crs/EPSG/0/1000\" srsDimension=\"2\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2</gml:posList></gml:MultiPoint>",
                "<gml:MultiLineString srsName=\"http://www.opengis.net/def/crs/EPSG/0/1000\" srsDimension=\"2\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:LineStringMember><gml:LineString><gml:posList>1 1 2 2 3 3 4 4</gml:posList></gml:LineString></gml:LineStringMember><gml:LineStringMember><gml:LineString><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2</gml:posList></gml:LineString></gml:LineStringMember></gml:MultiLineString>",
                "<gml:MultiPolygon srsName=\"http://www.opengis.net/def/crs/EPSG/0/1000\" srsDimension=\"2\" xmlns:gml=\"http://www.opengis.net/gml/\"><gml:PolygonMember><gml:Polygon><gml:outerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:outerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs><gml:innerBoundaryIs><gml:LinearRing><gml:posList>1 1 2 2 3 3 4 4 1 1</gml:posList></gml:LinearRing></gml:innerBoundaryIs></gml:Polygon></gml:PolygonMember><gml:PolygonMember><gml:Polygon><gml:outerBoundaryIs><gml:LinearRing><gml:posList>0.5 0.5 1 1 1.5 1.5 2 2 0.5 0.5</gml:posList></gml:LinearRing></gml:outerBoundaryIs></gml:Polygon></gml:PolygonMember></gml:MultiPolygon>"
            };

            this.geometryComparer = new GeometryComparer();
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToMarkup(IGeometry)" /> method.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToMarkupTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                String actual = GeographyMarkupConverter.ToMarkup(this.geometries[index]);
                actual.ShouldBe(this.geometriesMarkup[index]);
            }
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToMarkup(IGeometry, String)" /> method.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToMarkupWithIdentifierTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                String actual = GeographyMarkupConverter.ToMarkup(this.geometries[index], this.identifier);
                actual.ShouldBe(this.geometriesMarkupWithIdentifier[index]);
            }
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToMarkup(IGeometry, String)" /> method with reference systems.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToMarkupWithReferenceTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                String actual = GeographyMarkupConverter.ToMarkup(this.geometriesWithReference[index]);
                actual.ShouldBe(this.geometriesMarkupWithReference[index]);
            }
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToMarkupElement(IGeometry)" /> method.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToMarkupElementTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                XElement element = GeographyMarkupConverter.ToMarkupElement(this.geometries[index]);
                element.Add(new XAttribute(XNamespace.Xmlns + "gml", "http://www.opengis.net/gml/"));
                String elementString = element.ToString(SaveOptions.DisableFormatting);

                elementString.ShouldBe(this.geometriesMarkup[index]);
            }
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToMarkupElement(IGeometry, String)" /> method.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToMarkupElementWithIdentifierTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                XElement element = GeographyMarkupConverter.ToMarkupElement(this.geometries[index], this.identifier);
                element.Add(new XAttribute(XNamespace.Xmlns + "gml", "http://www.opengis.net/gml/"));
                String elementString = element.ToString(SaveOptions.DisableFormatting);

                elementString.ShouldBe(this.geometriesMarkupWithIdentifier[index]);
            }
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToGeometry(string, IGeometryFactory)" /> method.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToGeometryFromStringTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                IGeometry geometry = GeographyMarkupConverter.ToGeometry(this.geometriesMarkup[index], this.factory);
                this.geometryComparer.Compare(geometry, this.geometries[index]).ShouldBe(0);
            }
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToGeometry(XElement, IGeometryFactory)" /> method.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToGeometryFromXElementTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                IGeometry geometry = GeographyMarkupConverter.ToGeometry(XElement.Parse(this.geometriesMarkup[index]), this.factory);
                this.geometryComparer.Compare(geometry, this.geometries[index]).ShouldBe(0);
            }
        }

        /// <summary>
        /// Tests the <see cref="GeographyMarkupConverter.ToGeometry(XElement, IGeometryFactory, IReferenceSystemFactory)" /> method.
        /// </summary>
        [Test]
        public void GeographyMarkupConverterToGeometryWithReferenceSystemTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                IGeometry geometry = GeographyMarkupConverter.ToGeometry(XElement.Parse(this.geometriesMarkupWithReference[index]), this.factory, this.mockReferenceSystemFactory.Object);
                this.geometryComparer.Compare(geometry, this.geometries[index]).ShouldBe(0);
                geometry.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            }
        }
    }
}
