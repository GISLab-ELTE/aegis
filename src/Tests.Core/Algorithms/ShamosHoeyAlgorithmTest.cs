// <copyright file="ShamosHoeyAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="ShamosHoeyAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class ShamosHoeyAlgorithmTest
    {
        /// <summary>
        /// Tests intersection evaluation of the <see cref="ShamosHoeyAlgorithm" /> method.
        /// </summary>
        [Test]
        public void ShamosHoeyAlgorithmIntersectsTest()
        {
            // simple polygon, intersection
            Coordinate[] coordinates = new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(1, 0), new Coordinate(0, 1) };
            ShamosHoeyAlgorithm.Intersects(coordinates).ShouldBeTrue();

            // simple polygon, no intersection
            coordinates = new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 0), new Coordinate(3, 1) };
            ShamosHoeyAlgorithm.Intersects(coordinates).ShouldBeFalse();

            // simple line, no intersection
            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, n)).ToArray();
            ShamosHoeyAlgorithm.Intersects(coordinates).ShouldBeFalse();

            // line string, no intersection
            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n, n % 2)).ToArray();
            ShamosHoeyAlgorithm.Intersects(coordinates).ShouldBeFalse();

            // line string, no intersection
            coordinates = Enumerable.Range(0, 1000).Select(n => new Coordinate(n % 2, n)).ToArray();
            ShamosHoeyAlgorithm.Intersects(coordinates).ShouldBeFalse();

            // multiple line strings, intersection
            List<Coordinate[]> coordinateLists = new List<Coordinate[]>
            {
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1) },
                new Coordinate[] { new Coordinate(0, 1), new Coordinate(1, 0) },
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeTrue();

            // multiple line strings, intersection
            coordinateLists = new List<Coordinate[]>
            {
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2) },
                new Coordinate[] { new Coordinate(2, 0), new Coordinate(0, 2) },
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeTrue();

            // multiple line strings, intersection
            coordinateLists = new List<Coordinate[]>
            {
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, 1), new Coordinate(1, 0) },
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeTrue();

            // multiple line strings, intersection
            coordinateLists = new List<Coordinate[]>
            {
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(1, 2), new Coordinate(2, 1) },
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeTrue();

            // multiple line strings, intersection
            coordinateLists = new List<Coordinate[]>
            {
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(2, 3), new Coordinate(3, 2) },
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeTrue();

            // multiple line strings, no intersection
            coordinateLists = new List<Coordinate[]>
            {
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, -1), new Coordinate(-1, 0) },
                new Coordinate[] { new Coordinate(-1, -2), new Coordinate(-2, -1) },
                new Coordinate[] { new Coordinate(-2, -3), new Coordinate(-3, -2) },
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeFalse();

            // multiple line strings, intersection
            coordinateLists = new List<Coordinate[]>
            {
                new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) },
                new Coordinate[] { new Coordinate(0, -1), new Coordinate(-1, 0) },
                new Coordinate[] { new Coordinate(-1, -2), new Coordinate(-2, -1) },
                new Coordinate[] { new Coordinate(-2, -3), new Coordinate(-3, -2) },
                new Coordinate[] { new Coordinate(1, 2), new Coordinate(2, 1) },
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeTrue();

            // multiple line strings, intersection
            coordinateLists = new List<Coordinate[]>
            {
                Enumerable.Range(1, 1000).Select(n => new Coordinate(n, n)).ToArray(),
                Enumerable.Range(1, 1000).Select(n => new Coordinate(1000 - n, n)).ToArray(),
            };
            ShamosHoeyAlgorithm.Intersects(coordinateLists).ShouldBeTrue();
        }
    }
}
