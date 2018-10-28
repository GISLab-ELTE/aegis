// <copyright file="LineAlgorithmsTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Algorithms
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="LineAlgorithms" /> class.
    /// </summary>
    [TestFixture]
    public class LineAlgorithmsTest
    {
        /// <summary>
        /// Tests centroid computation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsCentroidTest()
        {
            // valid lines
            LineAlgorithms.Centroid(new Coordinate(2, 5), new Coordinate(7, 8)).ShouldBe(new Coordinate(4.5, 6.5));
            LineAlgorithms.Centroid(new BasicLineString(new[] { new Coordinate(5, 2), new Coordinate(5, 100) })).ShouldBe(new Coordinate(5, 51));

            // invalid line
            LineAlgorithms.Centroid(new Coordinate(Double.NaN, 5), new Coordinate(7, 8)).IsValid.ShouldBeFalse();

            // valid line string
            List<Coordinate> lineString = new List<Coordinate>
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(1, 7)
            };
            LineAlgorithms.Centroid(lineString).ShouldBe(new Coordinate(1, 4));

            // valid lines with fixed precision
            PrecisionModel precision = new PrecisionModel(0.001);

            LineAlgorithms.Centroid(new Coordinate(23000, 16000), new Coordinate(20000, 22000), precision).ShouldBe(new Coordinate(22000, 19000));
            LineAlgorithms.Centroid(new Coordinate(864325000, 96041000), new Coordinate(5470000, 827611000), precision).ShouldBe(new Coordinate(434898000, 461826000));

            // valid lines at given fixed precision
            LineAlgorithms.Centroid(new Coordinate(23654, 16412), new Coordinate(19530, 22009), precision).ShouldBe(new Coordinate(22000, 19000));
            LineAlgorithms.Centroid(new Coordinate(864325203, 96041202), new Coordinate(5470432, 827611534), precision).ShouldBe(new Coordinate(434898000, 461826000));
        }

        /// <summary>
        /// Tests coincidence evaluation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsCoincidesTest()
        {
            // coinciding lines
            Coordinate firstCoordinate = new Coordinate(1, 1);
            CoordinateVector firstVector = new CoordinateVector(0, 2);
            Coordinate secondCoordinate = new Coordinate(1, 1);
            CoordinateVector secondVector = new CoordinateVector(0, 4);
            LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector).ShouldBeTrue();

            firstCoordinate = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondCoordinate = new Coordinate(2, 4);
            secondVector = new CoordinateVector(0, 4);
            LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector).ShouldBeTrue();

            firstCoordinate = new Coordinate(15, 7);
            firstVector = new CoordinateVector(9, 5);
            secondCoordinate = new Coordinate(15, 7);
            secondVector = new CoordinateVector(9, 5);
            LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector).ShouldBeTrue();

            firstCoordinate = new Coordinate(2, 4);
            firstVector = new CoordinateVector(3, -1);
            secondCoordinate = new Coordinate(14, 0);
            secondVector = new CoordinateVector(3, -1);
            LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector).ShouldBeTrue();

            // not coinciding lines
            firstCoordinate = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondCoordinate = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector).ShouldBeFalse();

            firstCoordinate = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondCoordinate = new Coordinate(2, 4);
            secondVector = new CoordinateVector(4, 0);
            LineAlgorithms.Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector).ShouldBeFalse();
        }

        /// <summary>
        /// Tests containment evaluation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsContainsTest()
        {
            // line contains point
            Coordinate lineStart = new Coordinate(0, 1);
            Coordinate lineEnd = new Coordinate(100, 1);
            Coordinate coordinate = new Coordinate(1, 1);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate).ShouldBeTrue();

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2, 2);
            coordinate = new Coordinate(1, 1);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate).ShouldBeTrue();

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(-4, 2);
            coordinate = new Coordinate(-2, 1);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(-1, 0.5);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate).ShouldBeTrue();

            // line does not contain point
            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2, 2);
            coordinate = new Coordinate(1, 1.2);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate).ShouldBeFalse();

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(0, 200);
            coordinate = new Coordinate(0, 201);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate).ShouldBeFalse();

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(0, 200);
            coordinate = new Coordinate(0, 201);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate).ShouldBeFalse();

            // infinite line contains point
            lineStart = new Coordinate(2, 5);
            CoordinateVector lineVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(2, 4);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(2, 400);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate).ShouldBeTrue();

            lineStart = new Coordinate(2, 4);
            lineVector = new CoordinateVector(-3, 1);
            coordinate = new Coordinate(14, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate).ShouldBeTrue();

            // infinite line does not contain point
            coordinate = new Coordinate(2.001, 400);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate).ShouldBeFalse();

            lineStart = new Coordinate(1, 1);
            lineVector = new CoordinateVector(0, 2);
            coordinate = new Coordinate(0, 101);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate).ShouldBeFalse();

            // finite line with fixed precision
            PrecisionModel precision = new PrecisionModel(0.001);

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2000, 2000);
            coordinate = new Coordinate(1000, 1000);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision).ShouldBeTrue();

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(-4000, 2000);
            coordinate = new Coordinate(-3000, 2000);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision).ShouldBeTrue();

            coordinate = new Coordinate(-3000, 1000);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision).ShouldBeTrue();

            // finite line at given fixed precision
            lineStart = new Coordinate(12, 201);
            lineEnd = new Coordinate(1870, 2390);
            coordinate = new Coordinate(980, 1110);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision).ShouldBeTrue();

            lineStart = new Coordinate(-20, 50);
            lineEnd = new Coordinate(-4102, 1960);
            coordinate = new Coordinate(-2111, 983);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision).ShouldBeTrue();

            lineStart = new Coordinate(-20, 50);
            lineEnd = new Coordinate(-4102, 1960);
            coordinate = new Coordinate(-2111, 983);
            LineAlgorithms.Contains(lineStart, lineEnd, coordinate, precision).ShouldBeTrue();

            // infinite line with fixed precision
            lineStart = new Coordinate(1000, 1000);
            lineVector = new CoordinateVector(0, 2000);
            coordinate = new Coordinate(1000, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeTrue();

            coordinate = new Coordinate(0, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeFalse();

            coordinate = new Coordinate(2000, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeFalse();

            // infinite at given fixed precision
            lineStart = new Coordinate(1260, 1440);
            lineVector = new CoordinateVector(0, 2035);
            coordinate = new Coordinate(1230, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeTrue();

            coordinate = new Coordinate(1759, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeTrue();

            coordinate = new Coordinate(761, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeTrue();

            coordinate = new Coordinate(759, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeFalse();

            coordinate = new Coordinate(1761, 0);
            LineAlgorithms.Contains(lineStart, lineVector, coordinate, precision).ShouldBeFalse();
        }

        /// <summary>
        /// Tests distance computation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsDistanceTest()
        {
            // distance of a line and a coordinate
            Coordinate lineStart = new Coordinate(0, 1);
            Coordinate lineEnd = new Coordinate(100, 1);
            Coordinate coordinate = new Coordinate(1, 1);
            LineAlgorithms.Distance(lineStart, lineEnd, coordinate).ShouldBe(0);

            lineStart = new Coordinate(0, 1);
            lineEnd = new Coordinate(100, 1);
            coordinate = new Coordinate(1, 7);
            LineAlgorithms.Distance(lineStart, lineEnd, coordinate).ShouldBe(6);

            lineStart = new Coordinate(5, 7);
            lineEnd = new Coordinate(5, 70);
            coordinate = new Coordinate(5, 6);
            LineAlgorithms.Distance(lineStart, lineEnd, coordinate).ShouldBe(1);

            lineStart = new Coordinate(5, 7);
            lineEnd = new Coordinate(5, 70);
            coordinate = new Coordinate(4, 7);
            LineAlgorithms.Distance(lineStart, lineEnd, coordinate).ShouldBe(1);

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(2, -3);
            coordinate = new Coordinate(2, 1);
            LineAlgorithms.Distance(lineStart, lineEnd, coordinate).ShouldBe(2.22, 0.01);

            // distance of two lines
            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(100, 0);
            Coordinate secondLineStart = new Coordinate(0, 0);
            Coordinate secondLineEnd = new Coordinate(0, 100);
            LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd).ShouldBe(0);

            lineStart = new Coordinate(0, 0);
            lineEnd = new Coordinate(100, 0);
            secondLineStart = new Coordinate(0, 1);
            secondLineEnd = new Coordinate(100, 1);
            LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd).ShouldBe(1);

            lineStart = new Coordinate(2, 4);
            lineEnd = new Coordinate(5, 3);
            secondLineStart = new Coordinate(6, 2);
            secondLineEnd = new Coordinate(6, 6);
            LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd).ShouldBe(1);

            lineStart = new Coordinate(1, 1);
            lineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(4, 2);
            secondLineEnd = new Coordinate(0, 2);
            LineAlgorithms.Distance(lineStart, lineEnd, secondLineStart, secondLineEnd).ShouldBe(0);

            // distance of an infinite line and a point
            lineStart = new Coordinate(1, 1);
            CoordinateVector coordinateVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(2, 1500);
            LineAlgorithms.Distance(lineStart, coordinateVector, coordinate).ShouldBe(1);

            lineStart = new Coordinate(1, 1);
            coordinateVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(1, 1500);
            LineAlgorithms.Distance(lineStart, coordinateVector, coordinate).ShouldBe(0);

            lineStart = new Coordinate(1, 1);
            coordinateVector = new CoordinateVector(0, 1);
            coordinate = new Coordinate(0, -100);
            LineAlgorithms.Distance(lineStart, coordinateVector, coordinate).ShouldBe(1);

            lineStart = new Coordinate(2, 4);
            coordinateVector = new CoordinateVector(-3, 1);
            coordinate = new Coordinate(14, 0);
            LineAlgorithms.Distance(lineStart, coordinateVector, coordinate).ShouldBe(0);

            lineStart = new Coordinate(2, 4);
            coordinateVector = new CoordinateVector(-3, 1);
            coordinate = new Coordinate(5, 3);
            LineAlgorithms.Distance(lineStart, coordinateVector, coordinate).ShouldBe(0);
        }

        /// <summary>
        /// Tests intersection evaluation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsIntersectsTest()
        {
            // coinciding lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(1, 1);
            Coordinate secondLineEnd = new Coordinate(4, 1);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            // parallel lines
            firstLineStart = new Coordinate(1.3, 1.3);
            firstLineEnd = new Coordinate(1.3, 4.3);
            secondLineStart = new Coordinate(2.3, 1.3);
            secondLineEnd = new Coordinate(2.3, 4.3);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            // intersecting lines
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(4, 2);
            secondLineEnd = new Coordinate(0, 2);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 4);
            secondLineStart = new Coordinate(1, 4);
            secondLineEnd = new Coordinate(4, 1);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2, 5);
            secondLineEnd = new Coordinate(2, 10);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2, 1);
            secondLineEnd = new Coordinate(2, 8);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(7, 7);
            firstLineEnd = new Coordinate(7, 7);
            secondLineStart = new Coordinate(7, 7);
            secondLineEnd = new Coordinate(7, 7);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            // not intersecting lines
            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2.1, 1);
            secondLineEnd = new Coordinate(10, 8);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(1, 8.001);
            secondLineEnd = new Coordinate(10, 8.001);
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            // internally intersecting lines
            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(2, 3);
            LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(3, 2);
            LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 2);
            secondLineEnd = new Coordinate(200, 3);
            LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 2);
            secondLineEnd = new Coordinate(200, 3);
            LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            // not internally intersecting lines
            firstLineStart = new Coordinate(0, 0);
            firstLineEnd = new Coordinate(0, 1000.33);
            secondLineStart = new Coordinate(1000.33, 200);
            secondLineEnd = new Coordinate(0, 0);
            LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(100, 1);
            secondLineStart = new Coordinate(1, 4);
            secondLineEnd = new Coordinate(1, 1);
            LineAlgorithms.InternalIntersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            // intersecting infinite lines
            firstLineStart = new Coordinate(2, 5);
            CoordinateVector firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 4);
            CoordinateVector secondVector = new CoordinateVector(4, 0);
            LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();

            firstLineStart = new Coordinate(2, 4);
            firstVector = new CoordinateVector(-3, 1);
            secondLineStart = new Coordinate(5, 3);
            secondVector = new CoordinateVector(-3, 1);
            LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 2);
            secondVector = new CoordinateVector(0, 1);
            LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();
            LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeFalse();
        }

        /// <summary>
        /// Tests intersection computation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsIntersectionTest()
        {
            // coinciding lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(1, 1);
            Coordinate secondLineEnd = new Coordinate(4, 1);
            Intersection intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Interval);
            intersection.Start.ShouldBe(new Coordinate(1, 1));
            intersection.End.ShouldBe(new Coordinate(4, 1));

            // lines intersecting in one point
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(4, 2);
            secondLineEnd = new Coordinate(0, 2);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(1, 2));

            firstLineStart = new Coordinate(0, 0);
            firstLineEnd = new Coordinate(10, 0);
            secondLineStart = new Coordinate(0, 0);
            secondLineEnd = new Coordinate(0, 3);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(0, 0));

            firstLineStart = new Coordinate(-1, 6);
            firstLineEnd = new Coordinate(1, 2);
            secondLineStart = new Coordinate(4, 0);
            secondLineEnd = new Coordinate(0, 4);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(0, 4));

            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 2);
            secondLineStart = new Coordinate(2, 2);
            secondLineEnd = new Coordinate(2, 2);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(2, 2));

            // lines intersecting in more than one point
            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2, 5);
            secondLineEnd = new Coordinate(2, 10);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Interval);
            intersection.Start.ShouldBe(new Coordinate(2, 5));
            intersection.End.ShouldBe(new Coordinate(2, 8));

            firstLineStart = new Coordinate(4, 1);
            firstLineEnd = new Coordinate(1, 4);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(2, 3);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Interval);
            intersection.Start.ShouldBe(new Coordinate(3, 2));
            intersection.End.ShouldBe(new Coordinate(2, 3));

            firstLineStart = new Coordinate(2, 8);
            firstLineEnd = new Coordinate(6, 8);
            secondLineStart = new Coordinate(0, 8);
            secondLineEnd = new Coordinate(8, 8);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Interval);
            intersection.Start.ShouldBe(new Coordinate(2, 8));
            intersection.End.ShouldBe(new Coordinate(6, 8));

            // parallel lines
            firstLineStart = new Coordinate(1.3, 1.3);
            firstLineEnd = new Coordinate(1.3, 4.3);
            secondLineStart = new Coordinate(2.3, 1.3);
            secondLineEnd = new Coordinate(2.3, 4.3);
            LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeNull();

            // not intersecting lines
            firstLineStart = new Coordinate(2, 2);
            firstLineEnd = new Coordinate(2, 8);
            secondLineStart = new Coordinate(2.1, 1);
            secondLineEnd = new Coordinate(10, 8);
            LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeNull();

            // coinciding infinite lines
            firstLineStart = new Coordinate(2, 4);
            CoordinateVector firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 5);
            CoordinateVector secondVector = new CoordinateVector(0, 4);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(2, 4));

            // intersecting infinite lines
            firstLineStart = new Coordinate(1, 2);
            firstVector = new CoordinateVector(1, -2);
            secondLineStart = new Coordinate(4, 0);
            secondVector = new CoordinateVector(1, -1);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(0, 4));

            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(1, 1));

            // not intersecting infinite lines
            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 2);
            secondVector = new CoordinateVector(0, 1);
            LineAlgorithms.Intersection(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeNull();

            // internally intersecting lines
            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(3, 2);
            secondLineEnd = new Coordinate(2, 3);
            intersection = LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Interval);
            intersection.Start.ShouldBe(new Coordinate(3, 2));
            intersection.End.ShouldBe(new Coordinate(2, 3));

            firstLineStart = new Coordinate(1, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 2);
            secondLineEnd = new Coordinate(200, 2);
            intersection = LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(3, 2));

            firstLineStart = new Coordinate(10.58, 4);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(10.58, 4);
            secondLineEnd = new Coordinate(4, 1);
            intersection = LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
            intersection.Type.ShouldBe(IntersectionType.Interval);
            intersection.Start.ShouldBe(new Coordinate(10.58, 4));
            intersection.End.ShouldBe(new Coordinate(4, 1));

            // not internally intersecting lines
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(10, 1);
            secondLineStart = new Coordinate(1, 4);
            secondLineEnd = new Coordinate(1, 1);
            LineAlgorithms.InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeNull();

            // intersecting lines with fixed precision
            PrecisionModel precisionModel = new PrecisionModel(0.001);

            firstLineStart = new Coordinate(1000, 1000);
            firstLineEnd = new Coordinate(1000, 4000);
            secondLineStart = new Coordinate(4000, 2000);
            secondLineEnd = new Coordinate(-2000, 1000);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, precisionModel);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(1000, 2000));

            secondLineEnd = new Coordinate(-2000, 0);
            intersection = LineAlgorithms.Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, precisionModel);
            intersection.Type.ShouldBe(IntersectionType.Coordinate);
            intersection.Coordinate.ShouldBe(new Coordinate(1000, 1000));
        }

        /// <summary>
        /// Tests collinearity evaluation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsIsCollinearTest()
        {
            // collinear lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(1, 1);
            Coordinate secondLineEnd = new Coordinate(4, 1);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(1, 1);
            secondLineEnd = new Coordinate(15, 1);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(16, 1);
            secondLineStart = new Coordinate(15, 1);
            secondLineEnd = new Coordinate(2, 1);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(15, 1);
            secondLineEnd = new Coordinate(1, 1);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(4, 1);
            secondLineStart = new Coordinate(4, 1);
            secondLineEnd = new Coordinate(-5, 1);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineEnd = new Coordinate(1000, 1);
            secondLineStart = new Coordinate(250, 1);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineStart = new Coordinate(259, 1);
            secondLineEnd = new Coordinate(1000, 1);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            // not collinear lines
            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineStart = new Coordinate(1, 100);
            secondLineEnd = new Coordinate(1, 1200);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            firstLineStart = new Coordinate(20, 1);
            firstLineEnd = new Coordinate(250, 1);
            secondLineStart = new Coordinate(250, 1);
            secondLineEnd = new Coordinate(1000, 2);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            firstLineStart = new Coordinate(1.2, 1.2);
            firstLineEnd = new Coordinate(1.2, 7.2);
            secondLineStart = new Coordinate(2.2, 8.2);
            secondLineEnd = new Coordinate(2.2, 15.2);
            LineAlgorithms.IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();
        }

        /// <summary>
        /// Tests parallelism evaluation of the <see cref="LineAlgorithms" /> class.
        /// </summary>
        [Test]
        public void LineAlgorithmsIsParallelTest()
        {
            // parallel lines
            Coordinate firstLineStart = new Coordinate(1, 1);
            Coordinate firstLineEnd = new Coordinate(4, 1);
            Coordinate secondLineStart = new Coordinate(2, 2);
            Coordinate secondLineEnd = new Coordinate(4, 2);
            LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            firstLineStart = new Coordinate(1.2, 1.2);
            firstLineEnd = new Coordinate(1.2, 7.2);
            secondLineStart = new Coordinate(2.2, 8.2);
            secondLineEnd = new Coordinate(2.2, 15.2);
            LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 7);
            secondLineStart = new Coordinate(1, 1);
            secondLineEnd = new Coordinate(1, 7);
            LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();
            LineAlgorithms.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeTrue();

            // non parallel lines
            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 7);
            secondLineStart = new Coordinate(2, 1);
            secondLineEnd = new Coordinate(2.2, 8);
            LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            firstLineStart = new Coordinate(1, 1);
            firstLineEnd = new Coordinate(1, 7);
            secondLineStart = new Coordinate(0, 2);
            secondLineEnd = new Coordinate(10, 2);
            LineAlgorithms.IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd).ShouldBeFalse();

            // parallel infinite lines
            firstLineStart = new Coordinate(1, 1);
            CoordinateVector firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 2);
            CoordinateVector secondVector = new CoordinateVector(0, 1);
            LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();

            firstLineStart = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 4);
            secondVector = new CoordinateVector(0, 4);
            LineAlgorithms.Coincides(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();
            LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();

            firstLineStart = new Coordinate(2, 4);
            firstVector = new CoordinateVector(-3, 1);
            secondLineStart = new Coordinate(5, 3);
            secondVector = new CoordinateVector(-3, 1);
            LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();

            // non parallel infinite lines
            firstLineStart = new Coordinate(1, 1);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(1, 1);
            secondVector = new CoordinateVector(1, 0);
            LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();
            LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeFalse();

            firstLineStart = new Coordinate(2, 5);
            firstVector = new CoordinateVector(0, 1);
            secondLineStart = new Coordinate(2, 4);
            secondVector = new CoordinateVector(4, 0);
            LineAlgorithms.Intersects(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeTrue();
            LineAlgorithms.IsParallel(firstLineStart, firstVector, secondLineStart, secondVector).ShouldBeFalse();
        }
    }
}
