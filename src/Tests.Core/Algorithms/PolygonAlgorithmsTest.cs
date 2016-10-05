// <copyright file="PolygonAlgorithmsTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using ELTE.AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="PolygonAlgorithms" /> class.
    /// </summary>
    [TestFixture]
    public class PolygonAlgorithmsTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="PolygonAlgorithms.Area(IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsAreaTest()
        {
            // area of a rectangle in counterclockwise orientation
            Coordinate[] shell = new[]
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100),
                new Coordinate(5, 5)
            };
            PolygonAlgorithms.Area(shell).ShouldBe(11875);

            // area of a rectangle in counterclockwise orientation and double precision coordinates
            shell = new[]
            {
                new Coordinate(5.5, 5.5), new Coordinate(130.5, 5.5),
                new Coordinate(130.5, 100.5), new Coordinate(5.5, 100.5)
            };
            IBasicPolygon polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.Area(polygon).ShouldBe(11875);

            // area of a square with a hole
            shell = new[]
            {
                new Coordinate(-5, 5), new Coordinate(5, 5),
                new Coordinate(5, 15), new Coordinate(-5, 15)
            };
            Coordinate[] hole = new[]
            {
                new Coordinate(0, 0), new Coordinate(0, 2),
                new Coordinate(2, 2), new Coordinate(2, 0),
            };

            polygon = new BasicPolygon(shell, new[] { hole });
            PolygonAlgorithms.Area(polygon).ShouldBe(96);

            // area of a regular hexagon in counterclockwise orientation
            shell = new[]
            {
                new Coordinate(-3, -5), new Coordinate(3, -5),
                new Coordinate(6, 0), new Coordinate(3, 5),
                new Coordinate(-3, 5), new Coordinate(-6, 0)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.Area(polygon).ShouldBe(90);

            // area of a concave polygon
            shell = new[]
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(5, 3), new Coordinate(7, 5),
                new Coordinate(7, 1)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.Area(polygon).ShouldBe(14);

            // area of a non-simple polygon
            shell = new[]
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(5, 3), new Coordinate(7, 5),
                new Coordinate(7, 3), new Coordinate(5, 3),
                new Coordinate(5, 1)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.Area(polygon).ShouldBe(10);

            // exceptions
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.Area((IBasicPolygon)null));
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.Area((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="PolygonAlgorithms.IsConvex(IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsIsConvexTest()
        {
            // rectangle with counterclockwise orientation
            Coordinate[] shell = new[]
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100)
            };
            IBasicPolygon polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsConvex(polygon).ShouldBeTrue();

            // rectangle with a hole
            shell = new[]
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100)
            };
            Coordinate[] hole = new[]
            {
                new Coordinate(0, 0), new Coordinate(0, 2),
                new Coordinate(2, 2), new Coordinate(2, 0),
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            PolygonAlgorithms.IsConvex(polygon).ShouldBeFalse();

            // non-convex polygon with counterclockwise orientation
            shell = new[]
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(70, 70),
                new Coordinate(5, 100)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsConvex(polygon).ShouldBeFalse();

            // convex polygon with counterclockwise orientation
            shell = new[]
            {
                new Coordinate(6, 7), new Coordinate(7, 6),
                new Coordinate(9, 6), new Coordinate(9, 8),
                new Coordinate(8, 10), new Coordinate(7, 9)
            };
            PolygonAlgorithms.IsConvex(shell).ShouldBeTrue();

            // convex polygon with clockwise orientation
            shell = new[]
            {
                new Coordinate(0, 2), new Coordinate(2, 0),
                new Coordinate(3, -2), new Coordinate(1, -4),
                new Coordinate(-1, -4), new Coordinate(-3, -2),
                new Coordinate(-2, 0)
            };
            PolygonAlgorithms.IsConvex(shell).ShouldBeTrue();

            // convex polygon that contains collinear edges
            shell = new[]
            {
                new Coordinate(0, 2), new Coordinate(2, 0),
                new Coordinate(3, -2), new Coordinate(1, -4),
                new Coordinate(0, -4),
                new Coordinate(-1, -4), new Coordinate(-3, -2),
                new Coordinate(-2, 0)
            };
            PolygonAlgorithms.IsConvex(shell).ShouldBeTrue();

            // non-simple polygon
            shell = new[]
            {
                new Coordinate(1, 1), new Coordinate(1, 3),
                new Coordinate(5, 3), new Coordinate(7, 5),
                new Coordinate(7, 3), new Coordinate(5, 3),
                new Coordinate(5, 1)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsConvex(polygon).ShouldBeFalse();

            // exceptions
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.IsConvex((IBasicPolygon)null));
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.IsConvex((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="PolygonAlgorithms.IsValid(IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsIsValidTest()
        {
            // simple convex polygon without holes in clockwise orientation, containing collinear edges
            Coordinate[] shell = new[]
            {
                new Coordinate(0, 2), new Coordinate(2, 0),
                new Coordinate(3, -2), new Coordinate(1, -4),
                new Coordinate(0, -4), new Coordinate(-1, -4),
                new Coordinate(-3, -2), new Coordinate(-2, 0)
            };
            IBasicPolygon polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsValid(polygon).ShouldBeTrue();

            // rectangle in counterclockwise orientation
            shell = new[]
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(5, 100),
                new Coordinate(5, 5)
            };
            PolygonAlgorithms.IsValid(shell).ShouldBeTrue();

            // square with a hole
            shell = new[]
            {
                new Coordinate(-5, 5), new Coordinate(5, 5),
                new Coordinate(5, 15), new Coordinate(-5, 15)
            };
            Coordinate[] hole = new[]
            {
                new Coordinate(0, 0), new Coordinate(0, 2),
                new Coordinate(2, 2), new Coordinate(2, 0),
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            PolygonAlgorithms.IsValid(polygon).ShouldBeTrue();

            // two coordinates
            shell = new[]
            {
                new Coordinate(-5, 5), new Coordinate(5, 5)
            };
            PolygonAlgorithms.IsValid(shell).ShouldBeFalse();

            // simple concave polygon in counterclockwise orientation
            shell = new[]
            {
                new Coordinate(5, 5), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(70, 70),
                new Coordinate(5, 100)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsValid(polygon).ShouldBeTrue();

            // simple concave polygon in clockwise orientation
            shell = new[]
            {
                new Coordinate(2, 2), new Coordinate(2, 5),
                new Coordinate(4, 5), new Coordinate(3, 3),
                new Coordinate(6, 3), new Coordinate(5, 5),
                new Coordinate(7, 5), new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsValid(polygon).ShouldBeTrue();

            // triangle with a line inside
            shell = new[]
            {
                new Coordinate(1, 1), new Coordinate(6, 1),
                new Coordinate(2, 6)
            };
            hole = new[]
            {
                new Coordinate(2, 2), new Coordinate(4, 2)
            };
            polygon = new BasicPolygon(shell, new[] { hole });
            PolygonAlgorithms.IsValid(polygon).ShouldBeFalse();

            // non-simple polygon
            shell = new[]
            {
                new Coordinate(2, 2), new Coordinate(7, 5),
                new Coordinate(2, 5), new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsValid(polygon).ShouldBeFalse();

            // coordinate present more than once
            shell = new[]
            {
                new Coordinate(2, 2), new Coordinate(2, 5),
                new Coordinate(4, 5), new Coordinate(3, 3),
                new Coordinate(6, 3), new Coordinate(5, 5),
                new Coordinate(7, 5), new Coordinate(7, 2),
                new Coordinate(7, 2)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsValid(polygon).ShouldBeFalse();

            // not ordered coordinates
            shell = new[]
            {
                new Coordinate(2, 2), new Coordinate(2, 5),
                new Coordinate(3, 3), new Coordinate(7, 5),
                new Coordinate(6, 3), new Coordinate(5, 5),
                new Coordinate(7, 2), new Coordinate(4, 5),
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsValid(polygon).ShouldBeFalse();

            // different Z coordinates
            shell = new[]
            {
                new Coordinate(5, 5, 1), new Coordinate(130, 5),
                new Coordinate(130, 100), new Coordinate(70, 70),
                new Coordinate(5, 100)
            };
            polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.IsValid(polygon).ShouldBeFalse();

            // exceptions
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.IsValid((IBasicPolygon)null));
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.IsValid((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="PolygonAlgorithms.Orientation(IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsOrientationTest()
        {
            // counter clockwise orientation convex polygon
            Coordinate[] coordinates = new[]
            {
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10),
                new Coordinate(0, 0)
            };

            PolygonAlgorithms.Orientation(coordinates).ShouldBe(Orientation.Counterclockwise);

            coordinates = new[] { new Coordinate(2, 1, 0), new Coordinate(4, 1, 0), new Coordinate(3, 3, 0), new Coordinate(2, 1, 0) };

            PolygonAlgorithms.Orientation(coordinates).ShouldBe(Orientation.Counterclockwise);

            coordinates = new[] { new Coordinate(2, 1, 0), new Coordinate(4, 1, 0), new Coordinate(3, 3, 0) };

            PolygonAlgorithms.Orientation(coordinates).ShouldBe(Orientation.Counterclockwise);

            // clockwise orientation convex polygon
            coordinates = new[]
            {
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(10, 10), new Coordinate(10, 0),
                new Coordinate(0, 0)
            };

            PolygonAlgorithms.Orientation(coordinates).ShouldBe(Orientation.Clockwise);

            // collinear orientation convex polygon
            coordinates = new[]
            {
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(0, 20), new Coordinate(0, 30),
                new Coordinate(0, 0)
            };

            PolygonAlgorithms.Orientation(coordinates).ShouldBe(Orientation.Collinear);

            // clockwise concave polygon
            coordinates = new[]
            {
                new Coordinate(0, 0), new Coordinate(0, 10),
                new Coordinate(10, 10), new Coordinate(10, 0),
                new Coordinate(5, 5), new Coordinate(0, 0)
            };

            PolygonAlgorithms.Orientation(coordinates).ShouldBe(Orientation.Clockwise);

            // counter clockwise concave polygon
            coordinates = new[]
            {
                new Coordinate(0, 0), new Coordinate(2, 1), new Coordinate(8, 1),
                new Coordinate(10, 0), new Coordinate(9, 2), new Coordinate(9, 8),
                new Coordinate(10, 10), new Coordinate(8, 9), new Coordinate(2, 9),
                new Coordinate(0, 10), new Coordinate(1, 8), new Coordinate(1, 2),
                new Coordinate(0, 0)
            };

            PolygonAlgorithms.Orientation(coordinates).ShouldBe(Orientation.Counterclockwise);
        }

        /// <summary>
        /// Tests the <see cref="PolygonAlgorithms.SignedArea(IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void PolygonAlgorithmsSingedAreaTest()
        {
            // signed area of a rectangle with coordinates given in counterclockwise order
            Coordinate[] shell = new[]
            {
                new Coordinate(5.5, 5.5), new Coordinate(130.5, 5.5),
                new Coordinate(130.5, 100.5), new Coordinate(5.5, 100.5)
            };
            BasicPolygon polygon = new BasicPolygon(shell, null);
            PolygonAlgorithms.SignedArea(polygon).ShouldBe(-11875);

            // signed area of a triangle with coordinates given in clockwise order
            shell = new[]
            {
                new Coordinate(3, 3), new Coordinate(6, 1),
                new Coordinate(2, 1), new Coordinate(3, 3)
            };
            PolygonAlgorithms.SignedArea(shell).ShouldBe(4);

            // exceptions
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.SignedArea((List<Coordinate>)null));
            Should.Throw<ArgumentNullException>(() => PolygonAlgorithms.SignedArea((IBasicPolygon)null));
        }

        #endregion
    }
}
