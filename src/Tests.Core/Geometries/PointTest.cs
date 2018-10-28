// <copyright file="PointTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Geometries;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Point"/> class.
    /// </summary>
    [TestFixture]
    public class PointTest
    {
        private Mock<IReferenceSystem> mockReferenceSystem;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.mockReferenceSystem = new Mock<IReferenceSystem>();
            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(2);
        }

        /// <summary>
        /// Tests the constructor of the <see cref="Point"/> class.
        /// </summary>
        [Test]
        public void PointConstructorTest()
        {
            Point point = new Point(null, this.mockReferenceSystem.Object, Coordinate.Empty);

            point.PrecisionModel.ShouldBe(PrecisionModel.Default);
            point.Coordinate.ShouldBe(Coordinate.Empty);
            point.Dimension.ShouldBe(0);
            point.ReferenceSystem.ShouldBe(this.mockReferenceSystem.Object);
            point.Boundary.ShouldBeNull();
            point.Centroid.ShouldBe(Coordinate.Empty);
            point.IsEmpty.ShouldBeTrue();
            point.IsSimple.ShouldBeTrue();
            point.ToString().ShouldBe("POINT (0 0 0)");

            PrecisionModel model = new PrecisionModel(PrecisionModelType.Fixed);
            Coordinate coordinate = new Coordinate(1, 2, 3);
            point = new Point(model, null, coordinate);

            point.PrecisionModel.ShouldBe(model);
            point.Coordinate.ShouldBe(coordinate);
            point.Dimension.ShouldBe(0);
            point.ReferenceSystem.ShouldBe(null);
            point.Boundary.ShouldBeNull();
            point.Centroid.ShouldBe(coordinate);
            point.IsEmpty.ShouldBeFalse();
            point.IsSimple.ShouldBeTrue();
            point.IsValid.ShouldBeTrue();
            point.ToString().ShouldBe("POINT (1 2 3)");

            Should.Throw<ArgumentNullException>(() => new Point(PrecisionModel.Default, null, null));
        }

        /// <summary>
        /// Tests coordinate properties of the <see cref="Point" /> class.
        /// </summary>
        [Test]
        public void PointCoordinatePropertiesTest()
        {
            Point point = new Point(null, null, Coordinate.Empty);
            point.X.ShouldBe(Coordinate.Empty.X);
            point.Y.ShouldBe(Coordinate.Empty.Y);
            point.Z.ShouldBe(Coordinate.Empty.Z);

            Coordinate coordinate = new Coordinate(1, 2, 3);
            point = new Point(null, null, coordinate);
            point.X.ShouldBe(coordinate.X);
            point.Y.ShouldBe(coordinate.Y);
            point.Z.ShouldBe(coordinate.Z);

            point.X = 1.5;
            point.Y = 2.5;
            point.Z = 3.5;
            point.X.ShouldBe(1.5);
            point.Y.ShouldBe(2.5);
            point.Z.ShouldBe(3.5);
            point.Coordinate.ShouldBe(new Coordinate(1.5, 2.5, 3.5));

            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(2);
            point = new Point(null, this.mockReferenceSystem.Object, Coordinate.Empty);

            point.X = 1.5;
            point.Y = 2.5;
            point.Z = 3.5;
            point.X.ShouldBe(1.5);
            point.Y.ShouldBe(2.5);
            point.Z.ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="Point.CoordinateDimension" /> property.
        /// </summary>
        [Test]
        public void PointCoordinateDimensionTest()
        {
            Point point = new Point(null, this.mockReferenceSystem.Object, Coordinate.Empty);

            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(3);
            point.CoordinateDimension.ShouldBe(3);

            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(1);
            point.CoordinateDimension.ShouldBe(1);

            point = new Point(null, null, new Coordinate(1, 2));
            point.CoordinateDimension.ShouldBe(2);

            point = new Point(null, null, new Coordinate(1, 2, 3));
            point.CoordinateDimension.ShouldBe(3);

            point = new Point(null, null, new Coordinate(Double.NaN, Double.NaN, Double.NaN));
            point.CoordinateDimension.ShouldBe(3);
        }

        /// <summary>
        /// Tests the <see cref="Point.CoordinateDimension" /> property.
        /// </summary>
        [Test]
        public void PointSpatialDimensionTest()
        {
            Point point = new Point(null, null, Coordinate.Empty);
            point.SpatialDimension.ShouldBe(2);

            point = new Point(null, null, new Coordinate(1, 2, 3));
            point.SpatialDimension.ShouldBe(3);

            point = new Point(null, null, new Coordinate(Double.NaN, Double.NaN, Double.NaN));
            point.SpatialDimension.ShouldBe(3);

            this.mockReferenceSystem.Setup(mock => mock.Dimension).Returns(2);
            point = new Point(null, this.mockReferenceSystem.Object, new Coordinate(1, 2, 3));
            point.SpatialDimension.ShouldBe(2);
        }

        /// <summary>
        /// Tests the <see cref="Point.Envelope" /> property.
        /// </summary>
        [Test]
        public void PointCoordinateEnvelopeTest()
        {
            Point point = new Point(null, this.mockReferenceSystem.Object, Coordinate.Empty);
            point.Envelope.Minimum.ShouldBe(Coordinate.Empty);
            point.Envelope.Maximum.ShouldBe(Coordinate.Empty);

            Coordinate coordinate = new Coordinate(1, 2);
            point = new Point(null, null, coordinate);
            point.Envelope.Minimum.ShouldBe(coordinate);
            point.Envelope.Maximum.ShouldBe(coordinate);
        }

        /// <summary>
        /// Tests the <see cref="Point.IsValid" /> property.
        /// </summary>
        [Test]
        public void PointCoordinateIsValidTest()
        {
            Point point = new Point(null, this.mockReferenceSystem.Object, Coordinate.Empty);
            point.IsValid.ShouldBeTrue();

            point = new Point(null, null, new Coordinate(1, 2));
            point.IsValid.ShouldBeTrue();

            point = new Point(null, null, new Coordinate(Double.NaN, Double.NaN));
            point.IsValid.ShouldBeFalse();
        }
    }
}
