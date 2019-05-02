// <copyright file="LinearRingTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections;
    using System.Collections.Generic;
    using AEGIS.Geometries;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="LinearRing"/> class.
    /// </summary>
    [TestFixture]
    public class LinearRingTest
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
        /// Tests the constructor of the <see cref="Line"/> class.
        /// </summary>
        [Test]
        public void LinearRingConstructorTest()
        {
            // empty
            LinearRing lineString = new LinearRing(null, this.mockReferenceSystem.Object);

            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBeNull();
            lineString.EndCoordinate.ShouldBeNull();
            lineString.StartPoint.ShouldBeNull();
            lineString.EndPoint.ShouldBeNull();
            lineString.Count.ShouldBe(0);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            lineString.IsClosed.ShouldBeTrue();
            lineString.IsEmpty.ShouldBeTrue();
            lineString.IsSimple.ShouldBeTrue();
            lineString.IsValid.ShouldBeTrue();
            lineString.ToString().ShouldBe("LINEARRING EMPTY");

            // straight
            lineString = new LinearRing(null, null, this.straight);
            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBe(this.straight[0]);
            lineString.EndCoordinate.ShouldBe(this.straight[0]);
            lineString.StartPoint.Coordinate.ShouldBe(this.straight[0]);
            lineString.EndPoint.Coordinate.ShouldBe(this.straight[0]);
            lineString.Count.ShouldBe(this.straight.Count + 1);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(null);
            lineString.IsClosed.ShouldBeTrue();
            lineString.IsEmpty.ShouldBeFalse();
            lineString.IsSimple.ShouldBeTrue();
            lineString.IsValid.ShouldBeTrue();
            lineString.ToString().ShouldBe("LINEARRING (1 1 0,5 5 0,9 9 0,1 1 0)");

            // rectangle
            lineString = new LinearRing(null, this.mockReferenceSystem.Object, this.rectangle);
            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBe(this.rectangle[0]);
            lineString.EndCoordinate.ShouldBe(this.rectangle[this.rectangle.Count - 1]);
            lineString.StartPoint.Coordinate.ShouldBe(this.rectangle[0]);
            lineString.EndPoint.Coordinate.ShouldBe(this.rectangle[this.rectangle.Count - 1]);
            lineString.Count.ShouldBe(this.rectangle.Count);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            lineString.IsClosed.ShouldBeTrue();
            lineString.IsEmpty.ShouldBeFalse();
            lineString.IsSimple.ShouldBeTrue();
            lineString.IsValid.ShouldBeTrue();

            // crossing
            lineString = new LinearRing(null, this.mockReferenceSystem.Object, this.crossing);
            lineString.PrecisionModel.ShouldBe(PrecisionModel.Default);
            lineString.StartCoordinate.ShouldBe(this.crossing[0]);
            lineString.EndCoordinate.ShouldBe(this.crossing[0]);
            lineString.StartPoint.Coordinate.ShouldBe(this.crossing[0]);
            lineString.EndPoint.Coordinate.ShouldBe(this.crossing[0]);
            lineString.Count.ShouldBe(this.crossing.Count + 1);
            lineString.Dimension.ShouldBe(1);
            lineString.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            lineString.IsClosed.ShouldBeTrue();
            lineString.IsEmpty.ShouldBeFalse();
            lineString.IsSimple.ShouldBeFalse();
            lineString.IsValid.ShouldBeFalse();

            // null coordinates
            Should.Throw<ArgumentNullException>(() => lineString = new LinearRing(null, null, null));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Centroid" /> property.
        /// </summary>
        [Test]
        public void LinearRingCentroidTest()
        {
            // empty
            LinearRing lineString = new LinearRing(null, null);
            lineString.Centroid.ShouldBeNull();

            // zigzag
            lineString = new LinearRing(null, null, this.zigzag);
            lineString.Centroid.X.ShouldBe(2, 0.00001);
            lineString.Centroid.Y.ShouldBe(7.5, 0.00001);

            // rectangle
            lineString = new LinearRing(null, null, this.rectangle);
            lineString.Centroid.ShouldBe(new Coordinate(5, 5));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Envelope" /> property.
        /// </summary>
        [Test]
        public void LinearRingEnvelopeTest()
        {
            // empty
            LinearRing lineString = new LinearRing(null, null);
            lineString.Envelope.ShouldBeNull();

            // zigzag
            lineString = new LinearRing(null, null, this.zigzag);
            lineString.Envelope.ShouldBe(new Envelope(-1, 5, 0, 15));

            // rectangle
            lineString = new LinearRing(null, null, this.rectangle);
            lineString.Envelope.ShouldBe(new Envelope(0, 10, 0, 10));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Length" /> property.
        /// </summary>
        [Test]
        public void LinearRingLengthTest()
        {
            // empty
            LinearRing lineString = new LinearRing(null, null);
            lineString.Length.ShouldBe(0);

            // straight
            lineString = new LinearRing(null, this.mockReferenceSystem.Object, this.straight);
            lineString.Length.ShouldBe(2 * Math.Sqrt(128), 0.00001);

            // zigzag
            lineString = new LinearRing(null, this.mockReferenceSystem.Object, this.zigzag);
            lineString.Length.ShouldBe(Math.Sqrt(50) + Math.Sqrt(61) + Math.Sqrt(50) + Math.Sqrt(241), 0.00001);

            // rectangle
            lineString = new LinearRing(null, null, this.rectangle);
            lineString.Length.ShouldBe(40);
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Boundary" /> property.
        /// </summary>
        [Test]
        public void LinearRingBoundaryTest()
        {
            // straight
            LinearRing lineString = new LinearRing(null, null, this.straight);
            lineString.Boundary.ShouldBeNull();

            // empty
            lineString = new LinearRing(null, null);
            lineString.Boundary.ShouldBeNull();
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Contains(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LinearRingContainsTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.straight);

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
        /// Tests the <see cref="LinearRing.GetCoordinate(Int32)" /> method.
        /// </summary>
        [Test]
        public void LinearRingGetCoordinateTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.zigzag);

            for (Int32 i = 0; i < this.zigzag.Count; i++)
            {
                lineString.GetCoordinate(i).ShouldBe(this.zigzag[i]);
                lineString[i].ShouldBe(this.zigzag[i]);
            }

            lineString.GetCoordinate(this.zigzag.Count).ShouldBe(this.zigzag[0]);
            lineString[this.zigzag.Count].ShouldBe(this.zigzag[0]);

            Should.Throw<ArgumentOutOfRangeException>(() => lineString.GetCoordinate(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.GetCoordinate(lineString.Count));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.SetCoordinate(Int32, Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LinearRingSetCoordinateTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.zigzag);

            for (Int32 i = 1; i < this.zigzag.Count; i++)
            {
                lineString.SetCoordinate(i, this.crossing[i]);
                lineString.GetCoordinate(i).ShouldBe(this.crossing[i]);
            }

            lineString.SetCoordinate(0, this.crossing[0]);
            lineString.GetCoordinate(0).ShouldBe(this.crossing[0]);
            lineString.GetCoordinate(this.crossing.Count).ShouldBe(this.crossing[0]);

            lineString.SetCoordinate(0, this.zigzag[0]);
            lineString.GetCoordinate(0).ShouldBe(this.zigzag[0]);
            lineString.GetCoordinate(this.zigzag.Count).ShouldBe(this.zigzag[0]);

            Should.Throw<ArgumentNullException>(() => lineString.SetCoordinate(0, null));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.SetCoordinate(-1, new Coordinate(0, 0)));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.SetCoordinate(lineString.Count, new Coordinate(0, 0)));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Add(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LinearRingAddTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.zigzag);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.Add(this.rectangle[i]);
            }

            lineString.Count.ShouldBe(this.zigzag.Count + this.rectangle.Count + 1);
            for (Int32 i = 0; i < this.zigzag.Count; i++)
            {
                lineString.GetCoordinate(i).ShouldBe(this.zigzag[i]);
            }

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.GetCoordinate(i + this.zigzag.Count).ShouldBe(this.rectangle[i]);
            }

            lineString.GetCoordinate(this.zigzag.Count + this.rectangle.Count).ShouldBe(this.zigzag[0]);

            Should.Throw<ArgumentNullException>(() => lineString.Add(null));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Insert(Int32, Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LinearRingInsertTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.zigzag);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.Insert(i, this.rectangle[i]);
            }

            lineString.Count.ShouldBe(this.zigzag.Count + this.rectangle.Count + 1);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.GetCoordinate(i).ShouldBe(this.rectangle[i]);
            }

            for (Int32 i = 0; i < this.zigzag.Count; i++)
            {
                lineString.GetCoordinate(i + this.rectangle.Count).ShouldBe(this.zigzag[i]);
            }

            Should.Throw<ArgumentOutOfRangeException>(() => lineString.Insert(-1, new Coordinate(0, 0)));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.Insert(lineString.Count, new Coordinate(0, 0)));
            Should.Throw<ArgumentNullException>(() => lineString.Insert(0, null));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Remove(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LinearRingRemoveTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.rectangle);

            for (Int32 i = 0; i < this.rectangle.Count; i++)
            {
                lineString.Remove(this.rectangle[i]);
            }

            lineString.Count.ShouldBe(0);

            Should.Throw<ArgumentNullException>(() => lineString.Remove(null));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.RemoveAt(Int32)" /> method.
        /// </summary>
        [Test]
        public void LinearRingRemoveAtTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.zigzag);

            for (Int32 i = this.zigzag.Count - 1; i >= 0; i--)
            {
                lineString.RemoveAt(i);
            }

            lineString.Count.ShouldBe(0);

            Should.Throw<ArgumentOutOfRangeException>(() => lineString.RemoveAt(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => lineString.RemoveAt(lineString.Count));
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.Clear(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void LinearRingClearTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.rectangle);
            lineString.Clear();

            lineString.Count.ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="LinearRing.GetEnumerator()" /> method.
        /// </summary>
        [Test]
        public void LinearRingGetEnumeratorTest()
        {
            LinearRing lineString = new LinearRing(null, null, this.rectangle);

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
