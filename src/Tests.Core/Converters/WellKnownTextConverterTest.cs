// <copyright file="WellKnownTextConverterTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Converters
{
    using System;
    using System.Linq;
    using AEGIS.Converters;
    using AEGIS.Geometries;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="WellKnownTextConverter" /> class.
    /// </summary>
    [TestFixture]
    public class WellKnownTextConverterTest
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
        /// The array of geometries in WKT format.
        /// </summary>
        private String[] geometriesText;

        /// <summary>
        /// The geometry comparer.
        /// </summary>
        private GeometryComparer geometryComparer;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.factory = new GeometryFactory();

            this.geometries = new IGeometry[]
            {
                this.factory.CreatePoint(1, 1),
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

            this.geometriesText = new String[]
            {
                "POINT (1 1)",
                "LINESTRING (-0.5 0.5, -1 1, -1.5 1.5, -2 2)",
                "POLYGON ((1 1, 2 2, 3 3, 4 4, 1 1), (1 1, 2 2, 3 3, 4 4, 1 1), (1 1, 2 2, 3 3, 4 4, 1 1))",
                "MULTIPOINT (0.5 0.5, 1 1, 1.5 1.5, 2 2)",
                "MULTILINESTRING ((1 1, 2 2, 3 3, 4 4), (0.5 0.5, 1 1, 1.5 1.5, 2 2))",
                "MULTIPOLYGON (((1 1, 2 2, 3 3, 4 4, 1 1), (1 1, 2 2, 3 3, 4 4, 1 1), (1 1, 2 2, 3 3, 4 4, 1 1)), ((0.5 0.5, 1 1, 1.5 1.5, 2 2, 0.5 0.5)))"
            };

            this.geometryComparer = new GeometryComparer();
        }

        /// <summary>
        /// Tests the <see cref="ToGeometry" /> method.
        /// </summary>
        [Test]
        public void WellKnownTextConverterToGeometryTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                IGeometry geometry = WellKnownTextConverter.ToGeometry(this.geometriesText[index], this.factory);
                Assert.AreEqual(0, this.geometryComparer.Compare(geometry, this.geometries[index]));
            }

            IPoint point = WellKnownTextConverter.ToGeometry("POINT(1 1)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(1, 1), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT (1 1)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(1, 1), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT (1 1 1)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(1, 1), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT Z (1 1 1)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(1, 1, 1), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT M (1 1 1)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(1, 1), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT ZM (1 1 1 1)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(1, 1, 1), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT ZM(1 1 1 1)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(1, 1, 1), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT EMPTY", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(0, 0), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT (0 0)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(0, 0), point.Coordinate);

            point = WellKnownTextConverter.ToGeometry("POINT Z (0 0 0)", this.factory) as IPoint;
            Assert.AreEqual(new Coordinate(0, 0), point.Coordinate);

            Assert.Throws<ArgumentNullException>(() => WellKnownTextConverter.ToGeometry(null, this.factory));
            Assert.Throws<ArgumentException>(() => WellKnownTextConverter.ToGeometry(String.Empty, this.factory));
            Assert.Throws<ArgumentException>(() => WellKnownTextConverter.ToGeometry("UNDEFINED (0 0 0)", this.factory));
        }

        /// <summary>
        /// Tests the <see cref="ToWellKnownText" /> method.
        /// </summary>
        [Test]
        public void WellKnownTextConverterToWellKnownTextTest()
        {
            for (Int32 index = 0; index < this.geometries.Length; index++)
            {
                String text = WellKnownTextConverter.ToWellKnownText(this.geometries[index]);
                Assert.AreEqual(this.geometriesText[index], text);
            }

            Assert.Throws<ArgumentNullException>(() => WellKnownTextConverter.ToWellKnownText(null));
            Assert.Throws<ArgumentException>(() => WellKnownTextConverter.ToWellKnownText(this.factory.CreatePoint(new Coordinate(0, 0)), 1));
            Assert.Throws<ArgumentException>(() => WellKnownTextConverter.ToWellKnownText(this.factory.CreateGeometryCollection<IGeometry>()));
        }
    }
}
