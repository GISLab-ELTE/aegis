// <copyright file="LineStringTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Geometries
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using AEGIS.Geometries;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="LineString"/> class.
    /// </summary>
    [TestFixture]
    public class LineStringTest
    {
        private List<Coordinate> straight;
        private List<Coordinate> zigzag;
        private List<Coordinate> rectangle;
        private List<Coordinate> crossing;
        private Mock<IReferenceSystem> mockReferenceSystem;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.mockReferenceSystem = new Mock<IReferenceSystem>();
            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(3);

            this.straight = new List<Coordinate>
            {
                new Coordinate(1, 1),
                new Coordinate(5, 5),
                new Coordinate(9, 9)
            };

            this.zigzag = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(5, 5),
                new Coordinate(-1, 10),
                new Coordinate(4, 15),
            };

            this.rectangle = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0)
            };

            this.crossing = new List<Coordinate>
            {
                new Coordinate(0, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(10, 0)
            };
        }

        /// <summary>
        /// Tests the <see cref="LineString.LineString(PrecisionModel, IReferenceSystem)"/> method.
        /// </summary>
        [Test]
        public void LineStringConstructorEmptyTest()
        {
            LineString lineString = new LineString(null, this.mockReferenceSystem.Object);

            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBeNull();
            lineString.EndCoordinate.ShouldBeNull();
            lineString.StartPoint.ShouldBeNull();
            lineString.EndPoint.ShouldBeNull();
            lineString.Count.ShouldBe(0);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            lineString.Envelope.ShouldBeNull();
            lineString.IsClosed.ShouldBeFalse();
            lineString.IsEmpty.ShouldBeTrue();
            lineString.IsSimple.ShouldBeTrue();
            lineString.IsValid.ShouldBeTrue();
            lineString.Centroid.IsValid.ShouldBeFalse();
            lineString.Envelope.ShouldBeNull();
            lineString.Length.ShouldBe(0);
            lineString.Boundary.ShouldBeNull();
            lineString.ToString().ShouldBe("LINESTRING EMPTY");

            // null coordinates
            Should.Throw<ArgumentNullException>(() => lineString = new LineString(null, null, null));
        }

        /// <summary>
        /// Tests the <see cref="LineString.LineString(PrecisionModel, IReferenceSystem, IEnumerable{Coordinate})"/> method a straight line string.
        /// </summary>
        [Test]
        public void LineStringConstructorStraightTest()
        {
            LineString lineString = new LineString(null, null, this.straight);
            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBe(this.straight[0]);
            lineString.EndCoordinate.ShouldBe(this.straight[this.straight.Count - 1]);
            lineString.StartPoint.Coordinate.ShouldBe(this.straight[0]);
            lineString.EndPoint.Coordinate.ShouldBe(this.straight[this.straight.Count - 1]);
            lineString.Count.ShouldBe(this.straight.Count);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(null);
            lineString.Envelope.ShouldBe(new Envelope(1, 9, 1, 9));
            lineString.IsClosed.ShouldBeFalse();
            lineString.IsEmpty.ShouldBeFalse();
            lineString.IsSimple.ShouldBeTrue();
            lineString.IsValid.ShouldBeTrue();
            lineString.Centroid.ShouldBe(new Coordinate(5, 5));
            lineString.Length.ShouldBe(Math.Sqrt(128), 0.00001);
            lineString.ToString().ShouldBe("LINESTRING (1 1 0,5 5 0,9 9 0)");

            lineString.Boundary.ShouldBeAssignableTo<IMultiPoint>();
            IMultiPoint boundary = lineString.Boundary as IMultiPoint;
            boundary.Count.ShouldBe(2);
            boundary[0].Coordinate.ShouldBe(this.straight[0]);
            boundary[1].Coordinate.ShouldBe(this.straight[this.straight.Count - 1]);
        }

        /// <summary>
        /// Tests the <see cref="LineString.LineString(PrecisionModel, IReferenceSystem, IEnumerable{Coordinate})"/> method for a rectangular line string.
        /// </summary>
        [Test]
        public void LineStringConstructorRectangularTest()
        {
            LineString lineString = new LineString(null, this.mockReferenceSystem.Object, this.rectangle);
            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBe(this.rectangle[0]);
            lineString.EndCoordinate.ShouldBe(this.rectangle[this.rectangle.Count - 1]);
            lineString.StartPoint.Coordinate.ShouldBe(this.rectangle[0]);
            lineString.EndPoint.Coordinate.ShouldBe(this.rectangle[this.rectangle.Count - 1]);
            lineString.Count.ShouldBe(this.rectangle.Count);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            lineString.Envelope.ShouldBe(new Envelope(0, 10, 0, 10));
            lineString.IsClosed.ShouldBeTrue();
            lineString.IsEmpty.ShouldBeFalse();
            lineString.IsSimple.ShouldBeTrue();
            lineString.IsValid.ShouldBeTrue();
            lineString.Envelope.ShouldBe(new Envelope(0, 10, 0, 10));
            lineString.Length.ShouldBe(40);
            lineString.Boundary.ShouldBeNull();
        }

        /// <summary>
        /// Tests the <see cref="LineString.LineString(PrecisionModel, IReferenceSystem, IEnumerable{Coordinate})"/> method for a crossing line string.
        /// </summary>
        [Test]
        public void LineStringConstructorCrossingTest()
        {
            LineString lineString = new LineString(null, this.mockReferenceSystem.Object, this.crossing);
            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBe(this.crossing[0]);
            lineString.EndCoordinate.ShouldBe(this.crossing[this.crossing.Count - 1]);
            lineString.StartPoint.Coordinate.ShouldBe(this.crossing[0]);
            lineString.EndPoint.Coordinate.ShouldBe(this.crossing[this.crossing.Count - 1]);
            lineString.Count.ShouldBe(this.crossing.Count);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            lineString.IsClosed.ShouldBeFalse();
            lineString.IsEmpty.ShouldBeFalse();
            lineString.IsSimple.ShouldBeFalse();
            lineString.IsValid.ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="LineString.LineString(PrecisionModel, IReferenceSystem, IEnumerable{Coordinate})"/> method for a zigzag line string.
        /// </summary>
        [Test]
        public void LineStringConstructorZigzagTest()
        {
            LineString lineString = new LineString(null, null, this.zigzag);
            lineString.Centroid.X.ShouldBe(2, 0.00001);
            lineString.Centroid.Y.ShouldBe(7.5, 0.00001);
            lineString.Envelope.ShouldBe(new Envelope(-1, 5, 0, 15));
            lineString.Length.ShouldBe(Math.Sqrt(50) + Math.Sqrt(61) + Math.Sqrt(50), 0.00001);
        }

        /// <summary>
        /// Tests the <see cref="LineString.Contains(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LineStringContainsTest()
        {
            LineString lineString = new LineString(null, null, this.straight);

            foreach (Coordinate coordinate in this.straight)
            {
                lineString.Contains(coordinate).ShouldBeTrue();
            }

            foreach (Coordinate coordinate in this.rectangle)
            {
                lineString.Contains(coordinate).ShouldBeFalse();
            }
        }

        /// <summary>
        /// Tests the <see cref="LineString.GetCoordinate(Int32)" /> method.
        /// </summary>
        [Test]
        public void LineStringGetCoordinateTest()
        {
            LineString lineString = new LineString(null, null, this.zigzag);

            for (Int32 i = 0; i < lineString.Count; i++)
            {
                lineString.GetCoordinate(i).ShouldBe(this.zigzag[i]);
                lineString[i].ShouldBe(this.zigzag[i]);
            }

            Should.Throw<ArgumentOutOfRangeException>(() => lineString.GetCoordinate(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.GetCoordinate(lineString.Count));
        }

        /// <summary>
        /// Tests the <see cref="LineString.SetCoordinate(Int32, Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LineStringSetCoordinateTest()
        {
            LineString lineString = new LineString(null, null, this.zigzag);

            for (Int32 i = 0; i < lineString.Count; i++)
            {
                lineString.SetCoordinate(i, this.crossing[i]);

                lineString.GetCoordinate(i).ShouldBe(this.crossing[i]);
            }

            Should.Throw<ArgumentNullException>(() => lineString.SetCoordinate(0, null));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.SetCoordinate(-1, Coordinate.Empty));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.SetCoordinate(lineString.Count, Coordinate.Empty));
        }

        /// <summary>
        /// Tests the <see cref="LineString.Add(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LineStringAddTest()
        {
            LineString lineString = new LineString(null, null, this.zigzag);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.Add(this.rectangle[i]);
            }

            lineString.Count.ShouldBe(this.zigzag.Count + this.rectangle.Count);
            for (Int32 i = 0; i < this.zigzag.Count; i++)
            {
                lineString.GetCoordinate(i).ShouldBe(this.zigzag[i]);
            }

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.GetCoordinate(i + this.zigzag.Count).ShouldBe(this.rectangle[i]);
            }

            Should.Throw<ArgumentNullException>(() => lineString.Add(null));
        }

        /// <summary>
        /// Tests the <see cref="LineString.Insert(Int32, Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LineStringInsertTest()
        {
            LineString lineString = new LineString(null, null, this.zigzag);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.Insert(i, this.rectangle[i]);
            }

            lineString.Count.ShouldBe(this.zigzag.Count + this.rectangle.Count);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.GetCoordinate(i).ShouldBe(this.rectangle[i]);
            }

            for (Int32 i = 0; i < this.zigzag.Count; i++)
            {
                lineString.GetCoordinate(i + this.rectangle.Count).ShouldBe(this.zigzag[i]);
            }

            Should.Throw<ArgumentOutOfRangeException>(() => lineString.Insert(-1, Coordinate.Empty));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.Insert(lineString.Count, Coordinate.Empty));
            Should.Throw<ArgumentNullException>(() => lineString.Insert(0, null));
        }

        /// <summary>
        /// Tests the <see cref="LineString.Remove(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LineStringRemoveTest()
        {
            LineString lineString = new LineString(null, null, this.rectangle);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.Remove(this.rectangle[i]);
            }

            lineString.Count.ShouldBe(0);

            Should.Throw<ArgumentNullException>(() => lineString.Remove(null));
        }

        /// <summary>
        /// Tests the <see cref="LineString.RemoveAt(Int32)" /> method.
        /// </summary>
        [Test]
        public void LineStringRemoveAtTest()
        {
            LineString lineString = new LineString(null, null, this.rectangle);

            for (Int32 i = this.rectangle.Count - 1; i >= 0; i--)
            {
                lineString.RemoveAt(i);
            }

            lineString.Count.ShouldBe(0);

            Should.Throw<ArgumentOutOfRangeException>(() => lineString.RemoveAt(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.RemoveAt(lineString.Count));
        }

        /// <summary>
        /// Tests the <see cref="LineString.Clear(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LineStringClearTest()
        {
            LineString lineString = new LineString(null, null, this.rectangle);
            lineString.Clear();

            lineString.Count.ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="LineString.GetEnumerator()" /> method.
        /// </summary>
        [Test]
        public void LineStringGetEnumeratorTest()
        {
            LineString lineString = new LineString(null, null, this.rectangle);

            IEnumerator<Coordinate> genericEnumerator = lineString.GetEnumerator();
            IEnumerator enumerator = ((IEnumerable)lineString).GetEnumerator();

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                genericEnumerator.MoveNext().ShouldBeTrue();
                genericEnumerator.Current.ShouldBe(this.rectangle[i]);

                enumerator.MoveNext().ShouldBeTrue();
                enumerator.Current.ShouldBe(this.rectangle[i]);
            }
        }
    }
}
