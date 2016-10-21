// <copyright file="GeometryDistanceAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Tests.Algorithms
{
    using System;
    using ELTE.AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="GeometryDistanceAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class GeometryDistanceAlgorithmTest
    {
        /// <summary>
        /// Tests the <see cref="GeometryDistanceAlgorithm.Distance(IBasicPoint, IBasicPoint)" /> method.
        /// </summary>
        [Test]
        public void GeometryDistanceAlgorithmDistancePointsTest()
        {
            BasicPoint first = new BasicPoint(new Coordinate(0, 10));
            BasicPoint second = new BasicPoint(new Coordinate(10, 10));
            GeometryDistanceAlgorithm.Distance(first, second).ShouldBe(10);

            first = new BasicPoint(new Coordinate(10, 10));
            second = new BasicPoint(new Coordinate(10, 0));
            GeometryDistanceAlgorithm.Distance(first, second).ShouldBe(10);

            first = new BasicPoint(new Coordinate(67, 75));
            second = new BasicPoint(new Coordinate(17, 89));
            GeometryDistanceAlgorithm.Distance(first, second).ShouldBe(51.92, 0.01);

            // same point
            GeometryDistanceAlgorithm.Distance(first, first).ShouldBe(0);

            // same coordinate
            second = new BasicPoint(first.Coordinate);
            GeometryDistanceAlgorithm.Distance(first, second).ShouldBe(0);

            // undefined coordinate
            second = new BasicPoint(Coordinate.Undefined);
            GeometryDistanceAlgorithm.Distance(first, second).ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="GeometryDistanceAlgorithm.Distance(IBasicPoint, IBasicPoint)" /> method.
        /// </summary>
        [Test]
        public void GeometryDistanceAlgorithmDistanceLineStringsTest()
        {
            // parallel line strings
            BasicLineString firstLineString = new BasicLineString(new[] { new Coordinate(2, 2), new Coordinate(2, -2) });
            BasicLineString secondLineString = new BasicLineString(new[] { new Coordinate(4, 2), new Coordinate(4, -2) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(2);

            firstLineString = new BasicLineString(new[] { new Coordinate(0, 0), new Coordinate(2, 2) });
            secondLineString = new BasicLineString(new[] { new Coordinate(0, 2), new Coordinate(2, 4) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(Math.Sqrt(2), 0.001);

            firstLineString = new BasicLineString(new[] { new Coordinate(2, 2), new Coordinate(2, -2), new Coordinate(10, -2), new Coordinate(10, 2) });
            secondLineString = new BasicLineString(new[] { new Coordinate(2, 24), new Coordinate(2, 22), new Coordinate(10, 22), new Coordinate(10, 24) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(20);

            // one line is skewed towards the other
            firstLineString = new BasicLineString(new[] { new Coordinate(2, 2), new Coordinate(2, -2) });
            secondLineString = new BasicLineString(new Coordinate[] { new Coordinate(4, 2), new Coordinate(3, -2) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(1);

            // a line and a line string, where the line string's middle point is the closest to the middle of the line
            firstLineString = new BasicLineString(new[] { new Coordinate(2, 2), new Coordinate(2, -2) });
            secondLineString = new BasicLineString(new[] { new Coordinate(4, 2), new Coordinate(3, 0), new Coordinate(7, -2) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(1);

            // intersecting lines
            firstLineString = new BasicLineString(new[] { new Coordinate(0, 0), new Coordinate(2, 2) });
            secondLineString = new BasicLineString(new[] { new Coordinate(0, 2), new Coordinate(2, 0) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(0);

            firstLineString = new BasicLineString(new[] { new Coordinate(0, 0), new Coordinate(1, 1) });
            secondLineString = new BasicLineString(new[] { new Coordinate(0, 1), new Coordinate(1, 0) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(0);

            firstLineString = new BasicLineString(new[] { new Coordinate(0, 0), new Coordinate(1, 1) });
            secondLineString = new BasicLineString(new[] { new Coordinate(1, 1), new Coordinate(2, 2) });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(0);

            // point on line
            BasicPoint point = new BasicPoint(new Coordinate(1, 1));
            secondLineString = new BasicLineString(new[] { new Coordinate(0, 0), new Coordinate(2, 2) });
            GeometryDistanceAlgorithm.Distance(point, secondLineString).ShouldBe(0);

            point = new BasicPoint(new Coordinate(1.5, 1.5));
            secondLineString = new BasicLineString(new[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2) });
            GeometryDistanceAlgorithm.Distance(point, secondLineString).ShouldBe(0);

            // empty line strings
            secondLineString = new BasicLineString(new Coordinate[] { });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(Double.MaxValue);

            firstLineString = new BasicLineString(new Coordinate[] { });
            secondLineString = new BasicLineString(new Coordinate[] { });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(Double.MaxValue);

            // invalid line
            secondLineString = new BasicLineString(new Coordinate[] { new Coordinate(0, 0), Coordinate.Undefined });
            GeometryDistanceAlgorithm.Distance(firstLineString, secondLineString).ShouldBe(Double.MaxValue);
        }

        /// <summary>
        /// Tests the <see cref="GeometryDistanceAlgorithm.Distance(IPoint, IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void GeometryDistanceAlgorithmDistancePolygonsTest()
        {
            // point outside polygon
            BasicPoint point = new BasicPoint(new Coordinate(1, 4));
            BasicPolygon polygon = new BasicPolygon(new[] { new Coordinate(0, 0), new Coordinate(2, 0), new Coordinate(2, 2), new Coordinate(0, 2) });

            GeometryDistanceAlgorithm.Distance(point, polygon).ShouldBe(2);

            // point on polygon shell
            point = new BasicPoint(new Coordinate(2, 1));
            polygon = new BasicPolygon(new[] { new Coordinate(0, 0), new Coordinate(2, 0), new Coordinate(2, 2), new Coordinate(0, 2) });

            GeometryDistanceAlgorithm.Distance(point, polygon).ShouldBe(0);

            // point in polygon
            point = new BasicPoint(new Coordinate(1, 1));
            polygon = new BasicPolygon(new[] { new Coordinate(0, 0), new Coordinate(2, 0), new Coordinate(2, 2), new Coordinate(0, 2) });

            GeometryDistanceAlgorithm.Distance(point, polygon).ShouldBe(0);
        }
    }
}
