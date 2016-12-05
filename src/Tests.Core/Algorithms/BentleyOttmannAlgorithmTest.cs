// <copyright file="BentleyOttmannAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using ELTE.AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="BentleyOttmannAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class BentleyOttmannAlgorithmTest
    {
        /// <summary>
        /// Tests the <see cref="BentleyOttmannAlgorithm.Intersection(IEnumerable{Coordinate})" /> method.
        /// </summary>
        [Test]
        public void BentleyOttmannAlgorithmIntersectionTest()
        {
            // single line, no intersection
            IReadOnlyList<Coordinate> intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new Coordinate(10, 10),
                new Coordinate(20, 20)
            });

            intersections.ShouldBeEmpty();

            // single line string, one intersection
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new Coordinate(10, 10),
                new Coordinate(20, 20),
                new Coordinate(15, 20),
                new Coordinate(15, 10)
            });

            intersections.ShouldBe(new[] { new Coordinate(15, 15) });

            // multiple lines, no intersection
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 20)
                },
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0)
                }
            });

            intersections.ShouldBeEmpty();

            // multiple lines, one intersection
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 20)
                },
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 10)
                }
            });

            intersections.ShouldBe(new[] { new Coordinate(10, 10) });

            // multiple lines, one intersection
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 20)
                },
                new[]
                {
                    new Coordinate(15, 20),
                    new Coordinate(15, 10)
                }
            });

            intersections.ShouldBe(new[] { new Coordinate(15, 15) });

            // multiple lines, multiple intersections
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(-10, 0),
                    new Coordinate(10, 0)
                },
                new[]
                {
                    new Coordinate(-10, -10),
                    new Coordinate(10, 10)
                },
                new[]
                {
                    new Coordinate(3, 5),
                    new Coordinate(10, 5)
                },
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(10, 8)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(0, 0),
                new Coordinate(5, 5),
                new Coordinate(8, 8),
            });

            // multiple lines, multiple intersections
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(-5, 0),
                    new Coordinate(5, 0)
                },
                new[]
                {
                    new Coordinate(0, -2),
                    new Coordinate(8, 2)
                },
                new[]
                {
                    new Coordinate(1, -3),
                    new Coordinate(3, 3)
                }
            });

            intersections.Count.ShouldBe(3);
            intersections[0].X.ShouldBe(1.6, 0.0001);
            intersections[0].Y.ShouldBe(-1.2, 0.0001);
            intersections[1].X.ShouldBe(2, 0.0001);
            intersections[1].Y.ShouldBe(0, 0.0001);
            intersections[2].X.ShouldBe(4, 0.0001);
            intersections[2].Y.ShouldBe(0, 0.0001);

            // multiple lines, multiple intersections in the same coordinate
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(-5, 0),
                    new Coordinate(5, 0)
                },
                new[]
                {
                    new Coordinate(0, 5),
                    new Coordinate(5, 0)
                },
                new[]
                {
                    new Coordinate(4, -1),
                    new Coordinate(5, 0)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(5, 0),
                new Coordinate(5, 0),
                new Coordinate(5, 0),
            });

            // multiple lines, multiple intersections in the same coordinate
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10)
                },
                new[]
                {
                    new Coordinate(10, 20),
                    new Coordinate(10, 10),
                    new Coordinate(20, 10)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
            });

            // multiple lines, multiple intersections (even in the same coordinate)
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                },
                new[]
                {
                    new Coordinate(20, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(20, 10),
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(10, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
            });

            // single polygon, no intersection
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0)
            });

            intersections.ShouldBeEmpty();

            // single polygon, multiple intersections
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new Coordinate(-1, 1),
                new Coordinate(1, -1),
                new Coordinate(11, -1),
                new Coordinate(13, 1),
                new Coordinate(13, 6),
                new Coordinate(11, 8),
                new Coordinate(8, 8),
                new Coordinate(6, 6),
                new Coordinate(7, 3),
                new Coordinate(9, 1),
                new Coordinate(11, 3),
                new Coordinate(9, 5),
                new Coordinate(3, 5),
                new Coordinate(1, 3),
                new Coordinate(3, 1),
                new Coordinate(5, 3),
                new Coordinate(6, 6),
                new Coordinate(4, 8),
                new Coordinate(1, 8),
                new Coordinate(-1, 6),
                new Coordinate(-1, 1)
            });

            intersections.Select(this.RoundCoordinate).ShouldBe(new[]
            {
                new Coordinate(5.66667, 5),
                new Coordinate(6, 6),
                new Coordinate(6, 6),
                new Coordinate(6, 6),
                new Coordinate(6, 6),
                new Coordinate(6.33333, 5)
            });

            // multiple polygons, no intersection
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(15, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 5),
                    new Coordinate(15, 5),
                    new Coordinate(15, 0)
                }
            });

            intersections.ShouldBeEmpty();

            // multiple polygons, multiple intersections in the same coordinate
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 10)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
                new Coordinate(10, 10),
            });

            // multiple polygons, multiple intersections
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(5, 5),
                    new Coordinate(15, 5),
                    new Coordinate(15, 15),
                    new Coordinate(5, 15),
                    new Coordinate(5, 5)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(5, 10),
                new Coordinate(10, 5),
            });

            // multiple polygons, multiple intersections
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(-10, -5),
                    new Coordinate(10, -5),
                    new Coordinate(0, 5),
                    new Coordinate(-10, -5)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(0, 5),
                new Coordinate(0, 5),
                new Coordinate(5, 0),
            });

            // multiple polygons, multiple intersections (even in the same coordinate)

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(8, 0),
                    new Coordinate(14, 0),
                    new Coordinate(14, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 0)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
            });

            // the previous scenario with one less coordinate
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(8, 0),
                    new Coordinate(14, 0),
                    new Coordinate(8, 8),
                    new Coordinate(8, 0)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
            });

            // the previous scenario with an additional polygon
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(8, 0),
                    new Coordinate(14, 0),
                    new Coordinate(8, 8),
                    new Coordinate(8, 0)
                },
                new[]
                {
                    new Coordinate(-2, 6),
                    new Coordinate(14, 6),
                    new Coordinate(4, 14),
                    new Coordinate(-2, 6)
                }
            });

            intersections.ShouldBe(new[]
            {
                new Coordinate(0, 6),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 6),
                new Coordinate(8, 6),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(9.5, 6),
            });

            // the previous scenario with additional polygons
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0)
                },
                new[]
                {
                    new Coordinate(8, 0),
                    new Coordinate(14, 0),
                    new Coordinate(8, 8),
                    new Coordinate(8, 0)
                },
                new[]
                {
                    new Coordinate(-4, -6),
                    new Coordinate(4, -6),
                    new Coordinate(4, 2),
                    new Coordinate(-4, 2),
                    new Coordinate(-4, -6)
                },
                new[]
                {
                    new Coordinate(-2, 6),
                    new Coordinate(14, 6),
                    new Coordinate(4, 14),
                    new Coordinate(-2, 6)
                },
                new[]
                {
                    new Coordinate(2, 1),
                    new Coordinate(6, 1),
                    new Coordinate(6, 13),
                    new Coordinate(2, 13),
                    new Coordinate(2, 1)
                }
            });

            intersections.Select(this.RoundCoordinate).ShouldBe(new[]
            {
                new Coordinate(0, 2),
                new Coordinate(0, 6),
                new Coordinate(2, 2),
                new Coordinate(2, 6),
                new Coordinate(2, 8),
                new Coordinate(2, 11.33333),
                new Coordinate(3.25, 13),
                new Coordinate(4, 0),
                new Coordinate(4, 1),
                new Coordinate(5.25, 13),
                new Coordinate(6, 6),
                new Coordinate(6, 8),
                new Coordinate(6, 12.4),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 6),
                new Coordinate(8, 6),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(8, 8),
                new Coordinate(9.5, 6),
            });
        }

        /// <summary>
        /// Rounds the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The rounded coordinate.</returns>
        private Coordinate RoundCoordinate(Coordinate coordinate)
        {
            return new Coordinate(Math.Round(coordinate.X, 5), Math.Round(coordinate.Y, 5), Math.Round(coordinate.Z, 5));
        }
    }
}
