// <copyright file="WindingNumberAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ELTE.AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="WindingNumberAlgorithm" /> class.
    /// </summary>
    public class WindingNumberAlgorithmTest
    {
        /// <summary>
        /// Tests the <see cref="WindingNumberAlgorithm.Location(IBasicPolygon, Coordinate)" /> method.
        /// </summary>
        [Test]
        public void WindingNumberAlgorithmLocationTest()
        {
            // simple convex polygon
            Coordinate[] polygon = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };

            WindingNumberAlgorithm.Location(polygon, new Coordinate(0, 5)).ShouldBe(RelativeLocation.Boundary);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(10, 5)).ShouldBe(RelativeLocation.Boundary);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(5, 0)).ShouldBe(RelativeLocation.Boundary);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(10, 10)).ShouldBe(RelativeLocation.Boundary);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(5, 5)).ShouldBe(RelativeLocation.Interior);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(15, 5)).ShouldBe(RelativeLocation.Exterior);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(0, 5)).ShouldBe(RelativeLocation.Boundary);

            // simple convex polygon with negative coordinates
            polygon = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, -5),
                new Coordinate(10, 15),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };

            WindingNumberAlgorithm.Location(polygon, new Coordinate(5, 12.5)).ShouldBe(RelativeLocation.Boundary);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(5, 10)).ShouldBe(RelativeLocation.Interior);
            WindingNumberAlgorithm.Location(polygon, new Coordinate(-5, 0)).ShouldBe(RelativeLocation.Exterior);
        }

        /// <summary>
        /// Tests the location computation methods of the <see cref="WindingNumberAlgorithm" /> class.
        /// </summary>
        [Test]
        public void WindingNumberAlgorithmLocationCheckingTest()
        {
            // simple polygon

            Coordinate[] shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(20, 0),
                new Coordinate(15, 5),
                new Coordinate(20, 10),
                new Coordinate(0, 20),
                new Coordinate(0, 0),
            };

            Coordinate coordinate = new Coordinate(-1, 7);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(0, 0);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(0, 5);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(1, 1);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(3, 3);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(5, 8);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(6, 0);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(8, 4);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(8, 22);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(12, -2);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(12, 8);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(15, 5);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(15, 9);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(16, 1);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(19, 3);
            WindingNumberAlgorithm.InInterior(shell, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, coordinate).ShouldBeFalse();

            // polygon with holes

            shell = new Coordinate[]
            {
                new Coordinate(0, 0),
                new Coordinate(20, 0),
                new Coordinate(15, 5),
                new Coordinate(20, 10),
                new Coordinate(0, 20),
                new Coordinate(0, 0),
            };

            Coordinate[] hole1 = new Coordinate[]
            {
                new Coordinate(2, 2),
                new Coordinate(2, 4),
                new Coordinate(4, 4),
                new Coordinate(4, 2),
                new Coordinate(2, 2),
            };
            Coordinate[] hole2 = new Coordinate[]
            {
                new Coordinate(10, 7),
                new Coordinate(10, 9),
                new Coordinate(16, 9),
                new Coordinate(16, 7),
                new Coordinate(10, 7),
            };

            coordinate = new Coordinate(-1, 7);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(0, 0);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(0, 5);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(1, 1);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(3, 3);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(5, 8);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(6, 0);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(8, 4);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(8, 22);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(12, -2);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(12, 8);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(15, 5);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(15, 9);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();

            coordinate = new Coordinate(16, 1);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();

            coordinate = new Coordinate(19, 3);
            WindingNumberAlgorithm.InInterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
            WindingNumberAlgorithm.InExterior(shell, new[] { hole1, hole2 }, coordinate).ShouldBeTrue();
            WindingNumberAlgorithm.OnBoundary(shell, new[] { hole1, hole2 }, coordinate).ShouldBeFalse();
        }
    }
}
