// <copyright file="PolygonTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Geometries
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Geometries;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Polygon" /> class.
    /// </summary>
    [TestFixture]
    public class PolygonTest
    {
        private Mock<IReferenceSystem> mockReferenceSystem;
        private Coordinate[] shellCoordinates;
        private Coordinate[][] holeCoordinates;
        private LinearRing shellRing;
        private LinearRing[] holeRings;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.mockReferenceSystem = new Mock<IReferenceSystem>();
            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(3);

            this.shellCoordinates = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0)
            };
            this.holeCoordinates = new Coordinate[][]
            {
                new Coordinate[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 2),
                    new Coordinate(2, 2)
                },
                new Coordinate[0],
                null
            };

            this.shellRing = new LinearRing(null, this.mockReferenceSystem.Object, this.shellCoordinates);
            this.holeRings = new LinearRing[]
            {
                new LinearRing(null, this.mockReferenceSystem.Object, this.holeCoordinates[0]),
                new LinearRing(null, this.mockReferenceSystem.Object),
                null
            };
        }

        /// <summary>
        /// Tests the <see cref="Polygon.Polygon(PrecisionModel, IReferenceSystem)"/> method.
        /// </summary>
        [Test]
        public void PolygonConstructorEmptyTest()
        {
            Polygon polygon = new Polygon(null, null);
            polygon.Dimension.ShouldBe(2);
            polygon.ReferenceSystem.ShouldBe(null);
            polygon.Area.ShouldBe(0);
            polygon.Perimeter.ShouldBe(0);
            polygon.Shell.IsEmpty.ShouldBeTrue();
            polygon.Envelope.ShouldBeNull();
            polygon.Boundary.IsEmpty.ShouldBeTrue();
            polygon.Centroid.ShouldBeNull();
            polygon.HoleCount.ShouldBe(0);
            polygon.IsEmpty.ShouldBeTrue();
            polygon.IsConvex.ShouldBeTrue();
            polygon.IsDivided.ShouldBeFalse();
            polygon.IsWhole.ShouldBeTrue();
            polygon.IsSimple.ShouldBeTrue();
            polygon.IsValid.ShouldBeTrue();
            polygon.ToString().ShouldBe("POLYGON EMPTY");
        }

        /// <summary>
        /// Tests the <see cref="Polygon.Polygon(PrecisionModel, IReferenceSystem, LinearRing, IEnumerable{LinearRing})"/> method with only shell.
        /// </summary>
        [Test]
        public void PolygonConstructorLinearRingShellTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellRing, null);
            polygon.Shell.ShouldBe(this.shellRing);
            polygon.HoleCount.ShouldBe(0);
            polygon.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);

            polygon.Area.ShouldBe(100);
            polygon.Perimeter.ShouldBe(40);
            polygon.Envelope.ShouldBe(new Envelope(0, 10, 0, 10));

            polygon.Boundary.ShouldBeOfType(typeof(MultiLineString));
            (polygon.Boundary as MultiLineString).Count.ShouldBe(1);
            (polygon.Boundary as MultiLineString)[0].ShouldBe(this.shellCoordinates);

            polygon.Centroid.ShouldBe(new Coordinate(5, 5));
            polygon.IsEmpty.ShouldBeFalse();
            polygon.IsConvex.ShouldBeTrue();
            polygon.IsDivided.ShouldBeFalse();
            polygon.IsWhole.ShouldBeTrue();
            polygon.IsSimple.ShouldBeTrue();
            polygon.IsValid.ShouldBeTrue();
            polygon.ToString().ShouldBe("POLYGON ((0 0 0,10 0 0,10 10 0,0 10 0,0 0 0))");
        }

        /// <summary>
        /// Tests the <see cref="Polygon.Polygon(PrecisionModel, IReferenceSystem, LinearRing, IEnumerable{LinearRing})"/> method with shell and holes.
        /// </summary>
        [Test]
        public void PolygonConstructorLinearRingShellWithHolesTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellRing, this.holeRings);
            polygon.Shell.ShouldBe(this.shellRing);
            polygon.HoleCount.ShouldBe(1);
            polygon.Holes[0].ShouldBe(this.holeRings[0]);
            polygon.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);

            polygon.Area.ShouldBe(64);
            polygon.Perimeter.ShouldBe(64);
            polygon.Envelope.ShouldBe(new Envelope(0, 10, 0, 10));

            polygon.Boundary.ShouldBeOfType(typeof(MultiLineString));
            (polygon.Boundary as MultiLineString).Count.ShouldBe(2);
            (polygon.Boundary as MultiLineString)[0].ShouldBe(this.shellCoordinates);
            (polygon.Boundary as MultiLineString)[1].ShouldBe(this.holeCoordinates[0]);

            polygon.Centroid.ShouldBe(new Coordinate(5, 5));
            polygon.IsEmpty.ShouldBeFalse();
            polygon.IsConvex.ShouldBeFalse();
            polygon.IsDivided.ShouldBeFalse();
            polygon.IsWhole.ShouldBeFalse();
            polygon.IsSimple.ShouldBeTrue();
            polygon.IsValid.ShouldBeTrue();
            polygon.ToString().ShouldBe("POLYGON ((0 0 0,10 0 0,10 10 0,0 10 0,0 0 0),(2 2 0,2 8 0,8 8 0,8 2 0,2 2 0))");
        }

        /// <summary>
        /// Tests the <see cref="Polygon.Polygon(PrecisionModel, IReferenceSystem, LinearRing, IEnumerable{LinearRing})"/> method with a null shell.
        /// </summary>
        [Test]
        public void PolygonConstructorLinearRingNullTest()
        {
            Polygon polygon;
            Should.Throw<ArgumentNullException>(() => polygon = new Polygon(null, null, null, null));
        }

        /// <summary>
        /// Tests the <see cref="Polygon.Polygon(PrecisionModel, IReferenceSystem, IEnumerable{Coordinate}, IEnumerable{IEnumerable{Coordinate}})"/> method with shell and holes.
        /// </summary>
        [Test]
        public void PolygonConstructorEnumerableShellWithHolesTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.Shell.ShouldBe(this.shellRing);
            polygon.HoleCount.ShouldBe(1);
            polygon.Holes[0].ShouldBe(this.holeRings[0]);
            polygon.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);

            polygon.Area.ShouldBe(64);
            polygon.Perimeter.ShouldBe(64);
            polygon.Envelope.ShouldBe(new Envelope(0, 10, 0, 10));

            polygon.Boundary.ShouldBeOfType(typeof(MultiLineString));
            (polygon.Boundary as MultiLineString).Count.ShouldBe(2);
            (polygon.Boundary as MultiLineString)[0].ShouldBe(this.shellCoordinates);
            (polygon.Boundary as MultiLineString)[1].ShouldBe(this.holeCoordinates[0]);

            polygon.Centroid.ShouldBe(new Coordinate(5, 5));
            polygon.IsEmpty.ShouldBeFalse();
            polygon.IsConvex.ShouldBeFalse();
            polygon.IsDivided.ShouldBeFalse();
            polygon.IsWhole.ShouldBeFalse();
            polygon.IsSimple.ShouldBeTrue();
            polygon.IsValid.ShouldBeTrue();
            polygon.ToString().ShouldBe("POLYGON ((0 0 0,10 0 0,10 10 0,0 10 0,0 0 0),(2 2 0,2 8 0,8 8 0,8 2 0,2 2 0))");
        }

        /// <summary>
        /// Tests the <see cref="Polygon.Polygon(PrecisionModel, IReferenceSystem, IEnumerable{Coordinate}, IEnumerable{IEnumerable{Coordinate}})"/> method with a null shell.
        /// </summary>
        [Test]
        public void PolygonConstructorEnumerableNullTest()
        {
            Polygon polygon;
            Should.Throw<ArgumentNullException>(() => polygon = new Polygon(null, null, (IEnumerable<Coordinate>)null, null));
        }

        /// <summary>
        /// Tests the <see cref="Polygon.AddHole(ILinearRing)"/> method with a basic ring.
        /// </summary>
        [Test]
        public void PolygonAddHoleLinearRingBasicTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.AddHole(this.holeRings[0]);

            polygon.HoleCount.ShouldBe(2);
            polygon.Holes[0].ShouldBe(this.holeRings[0]);
            polygon.Holes[1].ShouldBe(this.holeRings[0]);
        }

        /// <summary>
        /// Tests the <see cref="Polygon.AddHole(ILinearRing)"/> method with an emtpy ring.
        /// </summary>
        [Test]
        public void PolygonAddHoleLinearRingEmptyTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.AddHole(this.holeRings[1]);

            polygon.HoleCount.ShouldBe(1);
            polygon.Holes[0].ShouldBe(this.holeRings[0]);
        }

        /// <summary>
        /// Tests the <see cref="Polygon.AddHole(ILinearRing)"/> method with a null ring.
        /// </summary>
        [Test]
        public void PolygonAddHoleLinearRingNullTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            Should.Throw<ArgumentNullException>(() => polygon.AddHole(this.holeRings[2]));
        }

        /// <summary>
        /// Tests the <see cref="Polygon.AddHole(ILinearRing)"/> method with a basic ring.
        /// </summary>
        [Test]
        public void PolygonAddHoleEnumerableBasicTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.AddHole(this.holeCoordinates[0]);

            polygon.HoleCount.ShouldBe(2);
            polygon.Holes[0].ShouldBe(this.holeCoordinates[0]);
            polygon.Holes[1].ShouldBe(this.holeCoordinates[0]);
        }

        /// <summary>
        /// Tests the <see cref="Polygon.AddHole(IEnumerable{Coordinate})"/> method with an empty ring.
        /// </summary>
        [Test]
        public void PolygonAddHoleEnumerableEmptyTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.AddHole(this.holeCoordinates[1]);

            polygon.HoleCount.ShouldBe(1);
            polygon.Holes[0].ShouldBe(this.holeCoordinates[0]);
        }

        /// <summary>
        /// Tests the <see cref="Polygon.AddHole(IEnumerable{Coordinate})"/> method with a null ring.
        /// </summary>
        [Test]
        public void PolygonAddHoleEnumerableNullTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            Should.Throw<ArgumentNullException>(() => polygon.AddHole(this.holeCoordinates[2]));
        }

        /// <summary>
        /// Tests the <see cref="Polygon.GetHole(Int32)"/> method with an existing hole.
        /// </summary>
        [Test]
        public void PolygonGetHoleExistsTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.GetHole(0).ShouldBe(this.holeRings[0]);
        }

        /// <summary>
        /// Tests the <see cref="Polygon.GetHole(Int32)"/> method with a non-existing hole.
        /// </summary>
        [Test]
        public void PolygonGetHoleNotExistsTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            Should.Throw<ArgumentOutOfRangeException>(() => polygon.GetHole(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => polygon.GetHole(1));
        }

        /// <summary>
        /// Tests the <see cref="Polygon.RemoveHole(ILinearRing)"/> method with an existing hole.
        /// </summary>
        [Test]
        public void PolygonRemoveHoleExistsTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellRing, this.holeRings);
            polygon.RemoveHole(this.holeRings[0]).ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="Polygon.RemoveHole(ILinearRing)"/> method with a non-existing hole.
        /// </summary>
        [Test]
        public void PolygonRemoveHoleNotExistsTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, null);
            polygon.RemoveHole(this.holeRings[0]).ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="Polygon.RemoveHole(ILinearRing)"/> method with a null hole.
        /// </summary>
        [Test]
        public void PolygonRemoveHoleNullTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            Should.Throw<ArgumentNullException>(() => polygon.AddHole(this.holeCoordinates[2]));
        }

        /// <summary>
        /// Tests the <see cref="Polygon.RemoveHoleAt(Int32)"/> method with an existing hole.
        /// </summary>
        [Test]
        public void PolygonRemoveHoleAtExistsTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.RemoveHoleAt(0);

            polygon.HoleCount.ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="Polygon.RemoveHoleAt(Int32)"/> method with a non-existing hole.
        /// </summary>
        [Test]
        public void PolygonRemoveHoleAtNotExistsTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            Should.Throw<ArgumentOutOfRangeException>(() => polygon.RemoveHoleAt(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => polygon.RemoveHoleAt(1));
        }

        /// <summary>
        /// Tests the <see cref="Polygon.ClearHoles()"/> method.
        /// </summary>
        [Test]
        public void PolygonClearHolesTest()
        {
            Polygon polygon = new Polygon(null, this.mockReferenceSystem.Object, this.shellCoordinates, this.holeCoordinates);
            polygon.ClearHoles();

            polygon.HoleCount.ShouldBe(0);
        }
    }
}
