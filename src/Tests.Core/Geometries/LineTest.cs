// <copyright file="LineTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Geometries;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Line"/> class.
    /// </summary>
    [TestFixture]
    public class LineTest
    {
        private Mock<IReferenceSystem> mockReferenceSystem;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.mockReferenceSystem = new Mock<IReferenceSystem>();
            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(3);
        }

        /// <summary>
        /// Tests the constructor of the <see cref="Line"/> class.
        /// </summary>
        [Test]
        public void LineConstructorTest()
        {
            Coordinate start = new Coordinate(1, 1, 1);
            Coordinate end = new Coordinate(2, 2, 2);

            Line line = new Line(null, this.mockReferenceSystem.Object, start, end);

            line.PrecisionModel.ShouldBe(PrecisionModel.Default);
            line.StartCoordinate.ShouldBe(start);
            line.EndCoordinate.ShouldBe(end);
            line.Count.ShouldBe(2);
            line.Dimension.ShouldBe(1);
            line.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            line.IsClosed.ShouldBeFalse();
            line.IsEmpty.ShouldBeFalse();
            line.IsSimple.ShouldBeTrue();
            line.IsValid.ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="Line.Length" /> property.
        /// </summary>
        [Test]
        public void LineLengthTest()
        {
            Coordinate start = new Coordinate(1, 1);
            Coordinate end = new Coordinate(4, 5);
            Line line = new Line(null, null, start, end);
            line.Length.ShouldBe(5);

            start = new Coordinate(1, 1);
            end = new Coordinate(1, 1);
            line = new Line(null, null, start, end);
            line.Length.ShouldBe(0);

            start = new Coordinate(1, 4, 5);
            end = new Coordinate(1, 1, 1);
            line = new Line(null, null, start, end);
            line.Length.ShouldBe(5);
        }

        /// <summary>
        /// Tests the <see cref="Line.Boundary" /> property.
        /// </summary>
        [Test]
        public void LineBoundaryTest()
        {
            Coordinate start = new Coordinate(1, 1);
            Coordinate end = new Coordinate(4, 5);
            Line line = new Line(null, null, start, end);

            line.Boundary.ShouldBeAssignableTo<IMultiPoint>();
            IMultiPoint boundary = line.Boundary as IMultiPoint;
            boundary.Count.ShouldBe(2);
            boundary[0].Coordinate.ShouldBe(start);
            boundary[1].Coordinate.ShouldBe(end);

            line = new Line(null, null, new Coordinate(0, 0), new Coordinate(0, 0));
            line.Boundary.ShouldBeNull();
        }

        /// <summary>
        /// Tests unsupported methods of the <see cref="Line" /> class.
        /// </summary>
        [Test]
        public void LineUnsupportedMethodsTest()
        {
            Line line = new Line(null, null, new Coordinate(0, 0), new Coordinate(0, 0));

            Should.Throw<NotSupportedException>(() => line.Add(new Coordinate(0, 0)));
            Should.Throw<NotSupportedException>(() => line.Insert(0, new Coordinate(0, 0)));
            Should.Throw<NotSupportedException>(() => line.Remove(new Coordinate(0, 0)));
            Should.Throw<NotSupportedException>(() => line.RemoveAt(0));
        }
    }
}
