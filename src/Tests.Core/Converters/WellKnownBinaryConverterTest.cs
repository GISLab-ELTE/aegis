// <copyright file="WellKnownBinaryConverterTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Converters
{
    using System;
    using System.Linq;
    using ELTE.AEGIS.Converters;
    using ELTE.AEGIS.Geometries;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="WellKnownBinaryConverter" /> class.
    /// </summary>
    [TestFixture]
    public class WellKnownBinaryConverterTest
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
        }

        /// <summary>
        /// Tests the conversion for geometries.
        /// </summary>
        [Test]
        public void WellKnownBinaryConverterConversionTest()
        {
            foreach (IGeometry geometry in this.geometries)
            {
                Byte[] binary = WellKnownBinaryConverter.ToWellKnownBinary(geometry);
                IGeometry converted = WellKnownBinaryConverter.ToGeometry(binary, this.factory);

                Assert.AreEqual(0, new GeometryComparer().Compare(geometry, converted));
            }
        }
    }
}
