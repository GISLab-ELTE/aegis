// <copyright file="MinkowskiSumAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using ELTE.AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="MinkowskiSumAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class MinkowskiSumAlgorithmTest
    {
        /// <summary>
        /// Tests the <see cref="Buffer" /> method.
        /// </summary>
        [Test]
        public void MinkowskiSumAlgorithmBufferTest()
        {
            // two triangles
            Coordinate[] buffer = new[] { new Coordinate(5, 5), new Coordinate(5, 7), new Coordinate(4, 6) };
            Coordinate[] sourceShell = new[] { new Coordinate(2, 1), new Coordinate(4, 1), new Coordinate(3, 3), new Coordinate(2, 1) };
            Coordinate[] expectedShell = new[]
            {
                new Coordinate(6, 7),
                new Coordinate(7, 6),
                new Coordinate(9, 6),
                new Coordinate(9, 8),
                new Coordinate(8, 10),
                new Coordinate(7, 9),
            };

            IBasicPolygon expected = new BasicPolygon(expectedShell);
            IBasicPolygon actual = MinkowskiSumAlgorithm.Buffer(sourceShell, buffer);

            PolygonAlgorithms.IsConvex(actual).ShouldBeTrue();
            actual.Shell.ShouldBe(expected.Shell);

            // triangle and a square
            buffer = new[] { new Coordinate(8, 6), new Coordinate(9, 6), new Coordinate(9, 7), new Coordinate(8, 7) };

            sourceShell = new[] { new Coordinate(3, 4), new Coordinate(5, 4), new Coordinate(4, 6), new Coordinate(3, 4) };

            expectedShell = new[]
            {
                new Coordinate(11, 11),
                new Coordinate(11, 10),
                new Coordinate(13, 10),
                new Coordinate(14, 10),
                new Coordinate(14, 11),
                new Coordinate(13, 13),
                new Coordinate(12, 13),
                new Coordinate(11, 11)
            };

            expected = new BasicPolygon(expectedShell);
            actual = MinkowskiSumAlgorithm.Buffer(sourceShell, buffer);

            PolygonAlgorithms.IsConvex(actual).ShouldBeTrue();
            actual.Shell.ShouldBe(expected.Shell);

            // square and circle of 4 points (square)
            buffer = new[] { new Coordinate(8, 6), new Coordinate(9, 6), new Coordinate(9, 7), new Coordinate(8, 7), new Coordinate(8, 6) };

            expectedShell = new[]
            {
                new Coordinate(6, 6),
                new Coordinate(8, 4),
                new Coordinate(9, 4),
                new Coordinate(11, 6),
                new Coordinate(11, 7),
                new Coordinate(9, 9),
                new Coordinate(8, 9),
                new Coordinate(6, 7),
                new Coordinate(6, 6)
            };

            expected = new BasicPolygon(expectedShell);
            actual = MinkowskiSumAlgorithm.Buffer(buffer, 2, 4);

            PolygonAlgorithms.IsConvex(actual).ShouldBeTrue();
            actual.Shell.Count.ShouldBe(expected.Shell.Count);
            for (Int32 coordIndex = 0; coordIndex < expected.Shell.Count; coordIndex++)
            {
                actual.Shell[coordIndex].X.ShouldBe(expected.Shell[coordIndex].X, 0.01);
                actual.Shell[coordIndex].Y.ShouldBe(expected.Shell[coordIndex].Y, 0.01);
            }

            // point and circle (of 4 points)
            IBasicPoint point = new BasicPoint(new Coordinate(1, 1, 0));
            actual = MinkowskiSumAlgorithm.Buffer(point, 2, 4);

            expectedShell = new[]
            {
                new Coordinate(3, 1), new Coordinate(1, 3),
                new Coordinate(-1, 1), new Coordinate(1, -1)
            };

            expected = new BasicPolygon(expectedShell);

            PolygonAlgorithms.IsConvex(actual).ShouldBeTrue();
            actual.Shell.Count.ShouldBe(expected.Shell.Count);
            for (Int32 coordIndex = 0; coordIndex < expected.Shell.Count; coordIndex++)
            {
                actual.Shell[coordIndex].X.ShouldBe(expected.Shell[coordIndex].X, 0.01);
                actual.Shell[coordIndex].Y.ShouldBe(expected.Shell[coordIndex].Y, 0.01);
            }

            // square with a square hole and circle (of 8 points)
            sourceShell = new[]
            {
                new Coordinate(1, 1),
                new Coordinate(9, 1),
                new Coordinate(9, 9),
                new Coordinate(1, 9),
                new Coordinate(1, 1)
            };
            Coordinate[][] sourceHoles = new[]
            {
                new[]
                {
                    new Coordinate(3, 3),
                    new Coordinate(3, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 3),
                    new Coordinate(3, 3)
                }
            };

            actual = MinkowskiSumAlgorithm.Buffer(new BasicPolygon(sourceShell, sourceHoles), 1, 8);

            expectedShell = new[]
            {
                new Coordinate(0, 1), new Coordinate(0.3, 0.3),
                new Coordinate(1, 0), new Coordinate(9, 0),
                new Coordinate(9.7, 0.3), new Coordinate(10, 1),
                new Coordinate(10, 9), new Coordinate(9.7, 9.7),
                new Coordinate(9, 10), new Coordinate(1, 10),
                new Coordinate(0.3, 9.7), new Coordinate(0, 9)
            };
            Coordinate[][] expectedHoles = new[]
            {
                new[]
                {
                    new Coordinate(4, 3), new Coordinate(3.7, 3.7),
                    new Coordinate(3, 4), new Coordinate(3, 6),
                    new Coordinate(3.7, 6.3), new Coordinate(4, 7),
                    new Coordinate(6, 7), new Coordinate(6.3, 6.3),
                    new Coordinate(7, 6), new Coordinate(7, 4),
                    new Coordinate(6.3, 3.7), new Coordinate(6, 3)
                }
            };

            expected = new BasicPolygon(expectedShell, expectedHoles);

            actual.Shell.Count.ShouldBe(expected.Shell.Count);
            for (Int32 coordIndex = 0; coordIndex < expected.Shell.Count; coordIndex++)
            {
                actual.Shell[coordIndex].X.ShouldBe(expected.Shell[coordIndex].X, 0.01);
                actual.Shell[coordIndex].Y.ShouldBe(expected.Shell[coordIndex].Y, 0.01);
            }

            actual.HoleCount.ShouldBe(expected.HoleCount);
            for (Int32 holeIndex = 0; holeIndex < expected.HoleCount; holeIndex++)
            {
                actual.Holes[holeIndex].Count.ShouldBe(expected.Holes[holeIndex].Count);
                for (Int32 coordIndex = 0; coordIndex < expected.Holes[holeIndex].Count; coordIndex++)
                {
                    actual.Holes[holeIndex][coordIndex].X.ShouldBe(expected.Holes[holeIndex][coordIndex].X, 0.01);
                    actual.Holes[holeIndex][coordIndex].Y.ShouldBe(expected.Holes[holeIndex][coordIndex].Y, 0.01);
                }
            }

            // line string (2 points) and square
            buffer = new[] { new Coordinate(0, 2), new Coordinate(0, 0), new Coordinate(2, 0), new Coordinate(2, 2) };

            actual = MinkowskiSumAlgorithm.Buffer(new[] { new Coordinate(0, 4), new Coordinate(2, 4) }, buffer);
            expectedShell = new[]
            {
                new Coordinate(0, 6), new Coordinate(0, 4),
                new Coordinate(2, 4), new Coordinate(4, 4),
                new Coordinate(4, 6), new Coordinate(2, 6)
            };
            expected = new BasicPolygon(expectedShell);

            actual.Shell.ShouldBe(expected.Shell);

            // line string (4 points) and square
            buffer = new[] { new Coordinate(0, 2), new Coordinate(0, 0), new Coordinate(2, 0), new Coordinate(2, 2) };

            actual = MinkowskiSumAlgorithm.Buffer(new[] { new Coordinate(1, 1), new Coordinate(3, 3), new Coordinate(5, 6), new Coordinate(3, 8) }, buffer);
            expectedShell = new[]
            {
                new Coordinate(1, 3), new Coordinate(1, 1),
                new Coordinate(3, 1), new Coordinate(5, 3),
                new Coordinate(7, 6), new Coordinate(7, 8),
                new Coordinate(5, 10), new Coordinate(3, 10)
            };
            expected = new BasicPolygon(expectedShell);

            actual.Shell.ShouldBe(expected.Shell);

            // concave and convex with no hole in the result
            buffer = new[] { new Coordinate(13, 2), new Coordinate(14, 3), new Coordinate(13, 4), new Coordinate(12, 3) };

            sourceShell = new[]
            {
                new Coordinate(1, 1), new Coordinate(11, 1),
                new Coordinate(11, 6), new Coordinate(8, 6),
                new Coordinate(9, 3), new Coordinate(3, 3),
                new Coordinate(4, 6), new Coordinate(1, 6)
            };

            actual = MinkowskiSumAlgorithm.Buffer(sourceShell, buffer);

            expectedShell = new[]
            {
                new Coordinate(13, 4), new Coordinate(14, 3),
                new Coordinate(24, 3), new Coordinate(25, 4),
                new Coordinate(25, 9), new Coordinate(24, 10),
                new Coordinate(21, 10), new Coordinate(20, 9),
                new Coordinate(20.67, 7), new Coordinate(17.33, 7),
                new Coordinate(18, 9), new Coordinate(17, 10),
                new Coordinate(14, 10), new Coordinate(13, 9)
            };

            expected = new BasicPolygon(expectedShell);
            actual.Shell.Count.ShouldBe(expected.Shell.Count);
            for (Int32 coordIndex = 0; coordIndex < expected.Shell.Count; coordIndex++)
            {
                actual.Shell[coordIndex].X.ShouldBe(expected.Shell[coordIndex].X, 0.01);
                actual.Shell[coordIndex].Y.ShouldBe(expected.Shell[coordIndex].Y, 0.01);
            }

            // convex and concave with no hole in the result
            buffer = new[] { new Coordinate(0, 3), new Coordinate(-3, 0), new Coordinate(0, -3), new Coordinate(3, 0) };

            sourceShell = new[]
            {
                new Coordinate(1, 1), new Coordinate(11, 1),
                new Coordinate(11, 6), new Coordinate(8, 6),
                new Coordinate(9, 3), new Coordinate(3, 3),
                new Coordinate(4, 6), new Coordinate(1, 6)
            };

            actual = MinkowskiSumAlgorithm.Buffer(sourceShell, buffer);

            expectedShell = new[]
            {
                new Coordinate(-2, 1), new Coordinate(1, -2),
                new Coordinate(11, -2), new Coordinate(14, 1),
                new Coordinate(14, 6), new Coordinate(11, 9),
                new Coordinate(8, 9), new Coordinate(6, 7),
                new Coordinate(4, 9), new Coordinate(1, 9),
                new Coordinate(-2, 6)
            };

            expected = new BasicPolygon(expectedShell);
            actual.Shell.Count.ShouldBe(expected.Shell.Count);
            for (Int32 coordIndex = 0; coordIndex < expected.Shell.Count; coordIndex++)
            {
                actual.Shell[coordIndex].X.ShouldBe(expected.Shell[coordIndex].X, 0.01);
                actual.Shell[coordIndex].Y.ShouldBe(expected.Shell[coordIndex].Y, 0.01);
            }

            // convex and concave with hole in the result
            buffer = new[] { new Coordinate(13, 4), new Coordinate(12, 3), new Coordinate(13, 2), new Coordinate(14, 3) };

            sourceShell = new[]
            {
                new Coordinate(1, 1), new Coordinate(11, 1),
                new Coordinate(11, 6), new Coordinate(7, 6),
                new Coordinate(9, 3), new Coordinate(4, 3),
                new Coordinate(6, 6), new Coordinate(1, 6)
            };

            actual = MinkowskiSumAlgorithm.Buffer(sourceShell, buffer);
            expectedShell = new[]
            {
                new Coordinate(13, 4), new Coordinate(14, 3),
                new Coordinate(24, 3), new Coordinate(25, 4),
                new Coordinate(25, 9), new Coordinate(24, 10),
                new Coordinate(20, 10), new Coordinate(19.5, 9.5),
                new Coordinate(19, 10), new Coordinate(14, 10),
                new Coordinate(13, 9)
            };

            expectedHoles = new[]
            {
                new[]
                {
                    new Coordinate(19.5, 8.25),
                    new Coordinate(20.3, 7),
                    new Coordinate(18.6, 7)
                }
            };

            expected = new BasicPolygon(expectedShell, expectedHoles);
            actual.Shell.Count.ShouldBe(expected.Shell.Count);
            for (Int32 coordIndex = 0; coordIndex < expected.Shell.Count; coordIndex++)
            {
                actual.Shell[coordIndex].X.ShouldBe(expected.Shell[coordIndex].X, 0.01);
                actual.Shell[coordIndex].Y.ShouldBe(expected.Shell[coordIndex].Y, 0.01);
            }

            actual.HoleCount.ShouldBe(expected.HoleCount);
            for (Int32 holeIndex = 0; holeIndex < expected.HoleCount; holeIndex++)
            {
                actual.Holes[holeIndex].Count.ShouldBe(expected.Holes[holeIndex].Count);
                for (Int32 coordIndex = 0; coordIndex < expected.Holes[holeIndex].Count; coordIndex++)
                {
                    actual.Holes[holeIndex][coordIndex].X.ShouldBe(expected.Holes[holeIndex][coordIndex].X, 0.1);
                    actual.Holes[holeIndex][coordIndex].Y.ShouldBe(expected.Holes[holeIndex][coordIndex].Y, 0.1);
                }
            }

            // convex source polygon with hole and a rhombus
            buffer = new[]
            {
               new Coordinate(10, 2),
               new Coordinate(11, 3),
               new Coordinate(10, 4),
               new Coordinate(9, 3)
            };

            sourceShell = new[]
            {
                new Coordinate(1, 1),
                new Coordinate(9, 1),
                new Coordinate(9, 9),
                new Coordinate(1, 9),
                new Coordinate(1, 1)
            };

            sourceHoles = new[]
            {
                new[]
                {
                    new Coordinate(3, 3),
                    new Coordinate(3, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 3),
                    new Coordinate(3, 3)
                }
            };

            actual = MinkowskiSumAlgorithm.Buffer(new BasicPolygon(sourceShell, sourceHoles), new BasicPolygon(buffer));

            expectedShell = new[]
            {
                new Coordinate(10, 4), new Coordinate(11, 3),
                new Coordinate(19, 3), new Coordinate(20, 4),
                new Coordinate(20, 12), new Coordinate(19, 13),
                new Coordinate(11, 13), new Coordinate(10, 12)
            };

            expectedHoles = new[]
            {
                new[]
                {
                    new Coordinate(14, 6), new Coordinate(13, 7),
                    new Coordinate(13, 9), new Coordinate(14, 10),
                    new Coordinate(16, 10), new Coordinate(17, 9),
                    new Coordinate(17, 7), new Coordinate(16, 6)
                }
            };

            expected = new BasicPolygon(expectedShell, expectedHoles);

            actual.Shell.Count.ShouldBe(expected.Shell.Count);
            for (Int32 coordIndex = 0; coordIndex < expected.Shell.Count; coordIndex++)
            {
                actual.Shell[coordIndex].X.ShouldBe(expected.Shell[coordIndex].X, 0.01);
                actual.Shell[coordIndex].Y.ShouldBe(expected.Shell[coordIndex].Y, 0.01);
            }

            actual.HoleCount.ShouldBe(expected.HoleCount);
            for (Int32 holeIndex = 0; holeIndex < expected.HoleCount; holeIndex++)
            {
                actual.Shell.Count.ShouldBe(expected.Holes[holeIndex].Count);
                for (Int32 coordIndex = 0; coordIndex < expected.Holes[holeIndex].Count; coordIndex++)
                {
                    actual.Holes[holeIndex][coordIndex].X.ShouldBe(expected.Holes[holeIndex][coordIndex].X, 0.01);
                    actual.Holes[holeIndex][coordIndex].Y.ShouldBe(expected.Holes[holeIndex][coordIndex].Y, 0.01);
                }
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => MinkowskiSumAlgorithm.Buffer((IBasicPolygon)null, new BasicPolygon(buffer)));
            Should.Throw<ArgumentNullException>(() => MinkowskiSumAlgorithm.Buffer(new BasicPolygon(sourceShell, sourceHoles), null));
        }
    }
}
