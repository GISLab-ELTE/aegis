﻿// <copyright file="GreinerHormannAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="GreinerHormannAlgorithm" /> class.
    /// </summary>
    /// <author>Máté Cserép</author>
    [TestFixture]
    public class GreinerHormannAlgorithmTest
    {
        /// <summary>
        /// Test cases for non-intersecting polygon shells without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmNoIntersectionTest()
        {
            // distinct polygons
            GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(20, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 0),
                },
                true, null);

            algorithm.InternalPolygons.ShouldBeEmpty();

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(20, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 0),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);

            // containing polygons
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 10),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 10),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 10),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(30, 0),
                    new Coordinate(30, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                });
            algorithm.ExternalPolygonsA[0].Holes.ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(10, 20),
                    new Coordinate(20, 20),
                    new Coordinate(20, 10),
                    new Coordinate(10, 10),
                },
            }, 0.0001);

            algorithm.ExternalPolygonsB.ShouldBeEmpty();
        }

        /// <summary>
        /// Test cases for tangential, but non-intersecting polygon shells without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmTangentialTest()
        {
            // tangent polygons without any mid-intersection point on edge
            GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 0),
                },
                true, null);

            algorithm.InternalPolygons.ShouldBeEmpty();

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 0),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);

            // tangent polygons with mid-intersection point on edge
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 0),
                },
                true, null);

            algorithm.InternalPolygons.ShouldBeEmpty();

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(20, 0),
                    new Coordinate(20, 20),
                    new Coordinate(10, 20),
                    new Coordinate(10, 10),
                    new Coordinate(10, 0),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);

            // tangent and containing polygons (touch from the inner boundary): case 1
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(6, 6),
                    new Coordinate(10, 6),
                    new Coordinate(10, 10),
                    new Coordinate(6, 10),
                    new Coordinate(6, 6),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 6),
                    new Coordinate(10, 10),
                    new Coordinate(6, 10),
                    new Coordinate(6, 6),
                    new Coordinate(10, 6),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(6, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 6),
                    new Coordinate(6, 6),
                    new Coordinate(6, 10),
                }, true);
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.ShouldBeEmpty();

            // tangent and containing polygons (touch from the inner boundary): case 2
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(7, 10),
                    new Coordinate(3, 10),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(3, 10),
                    new Coordinate(5, 5),
                    new Coordinate(7, 10),
                    new Coordinate(3, 10),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(7, 10),
                    new Coordinate(3, 10),
                    new Coordinate(5, 5),
                    new Coordinate(7, 10),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(3, 10),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(7, 10),
                    new Coordinate(5, 5),
                    new Coordinate(3, 10),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.ShouldBeEmpty();
        }

        /// <summary>
        /// Tests cases for intersecting convex polygons without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmConvexTest()
        {
            // single intersection of convex polygons
            GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(5, 5),
                    new Coordinate(15, 5),
                    new Coordinate(15, 15),
                    new Coordinate(5, 15),
                    new Coordinate(5, 5),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 5),
                    new Coordinate(10, 10),
                    new Coordinate(5, 10),
                    new Coordinate(5, 5),
                    new Coordinate(10, 5),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(5, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 5),
                    new Coordinate(5, 5),
                    new Coordinate(5, 10),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(10, 5),
                    new Coordinate(15, 5),
                    new Coordinate(15, 15),
                    new Coordinate(5, 15),
                    new Coordinate(5, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 5),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);
        }

        /// <summary>
        /// Tests cases for intersecting concave polygons without holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmConcaveTest()
        {
            // intersection of a convex and a concave polygon, single intersection
            GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(50, 0),
                    new Coordinate(50, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(10, -10),
                    new Coordinate(20, 10),
                    new Coordinate(30, 10),
                    new Coordinate(40, -10),
                    new Coordinate(60, 20),
                    new Coordinate(20, 40),
                    new Coordinate(10, -10),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(12, 0),
                    new Coordinate(15, 0),
                    new Coordinate(20, 10),
                    new Coordinate(30, 10),
                    new Coordinate(35, 0),
                    new Coordinate(46.66667, 0),
                    new Coordinate(50, 5),
                    new Coordinate(50, 25),
                    new Coordinate(40, 30),
                    new Coordinate(18, 30),
                    new Coordinate(12, 0),
                }, 0.001);
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(4);
            algorithm.ExternalPolygonsA.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsA.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(15, 0),
                    new Coordinate(35, 0),
                    new Coordinate(30, 10),
                    new Coordinate(20, 10),
                    new Coordinate(15, 0),
                },
                new[]
                {
                    new Coordinate(18, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                    new Coordinate(12, 0),
                    new Coordinate(18, 30),
                },
                new[]
                {
                    new Coordinate(46.66667, 0),
                    new Coordinate(50, 0),
                    new Coordinate(50, 5),
                    new Coordinate(46.66667, 0),
                },
                new[]
                {
                    new Coordinate(50, 25),
                    new Coordinate(50, 30),
                    new Coordinate(40, 30),
                    new Coordinate(50, 25),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsA.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            algorithm.ExternalPolygonsB.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsB.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(12, 0),
                    new Coordinate(10, -10),
                    new Coordinate(15, 0),
                    new Coordinate(12, 0),
                },
                new[]
                {
                    new Coordinate(35, 0),
                    new Coordinate(40, -10),
                    new Coordinate(46.66667, 0),
                    new Coordinate(35, 0),
                },
                new[]
                {
                    new Coordinate(40, 30),
                    new Coordinate(20, 40),
                    new Coordinate(18, 30),
                    new Coordinate(40, 30),
                },
                new[]
                {
                    new Coordinate(50, 5),
                    new Coordinate(60, 20),
                    new Coordinate(50, 25),
                    new Coordinate(50, 5),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsB.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            // intersection of a convex and a concave polygon, multiple intersections
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(60, 0),
                    new Coordinate(60, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(10, 40),
                    new Coordinate(10, -10),
                    new Coordinate(20, 40),
                    new Coordinate(30, -10),
                    new Coordinate(40, 40),
                    new Coordinate(50, -10),
                    new Coordinate(50, 50),
                    new Coordinate(20, 50),
                    new Coordinate(10, 40),
                },
                true, null);

            algorithm.InternalPolygons.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.InternalPolygons.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(12, 0),
                    new Coordinate(18, 30),
                    new Coordinate(10, 30),
                    new Coordinate(10, 0),
                },
                new[]
                {
                    new Coordinate(28, 0),
                    new Coordinate(32, 0),
                    new Coordinate(38, 30),
                    new Coordinate(22, 30),
                    new Coordinate(28, 0),
                },
                new[]
                {
                    new Coordinate(48, 0),
                    new Coordinate(50, 0),
                    new Coordinate(50, 30),
                    new Coordinate(42, 30),
                    new Coordinate(48, 0),
                },
            }, 0.0001);
            algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            algorithm.ExternalPolygonsA.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsA.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(10, 30),
                    new Coordinate(0, 30),
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 30),
                },
                new[]
                {
                    new Coordinate(12, 0),
                    new Coordinate(28, 0),
                    new Coordinate(22, 30),
                    new Coordinate(18, 30),
                    new Coordinate(12, 0),
                },
                new[]
                {
                    new Coordinate(32, 0),
                    new Coordinate(48, 0),
                    new Coordinate(42, 30),
                    new Coordinate(38, 30),
                    new Coordinate(32, 0),
                },
                new[]
                {
                    new Coordinate(50, 0),
                    new Coordinate(60, 0),
                    new Coordinate(60, 30),
                    new Coordinate(50, 30),
                    new Coordinate(50, 0),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsA.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            algorithm.ExternalPolygonsB.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsB.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(10, 0),
                    new Coordinate(10, -10),
                    new Coordinate(12, 0),
                    new Coordinate(10, 0),
                },
                new[]
                {
                    new Coordinate(18, 30),
                    new Coordinate(20, 40),
                    new Coordinate(22, 30),
                    new Coordinate(38, 30),
                    new Coordinate(40, 40),
                    new Coordinate(42, 30),
                    new Coordinate(50, 30),
                    new Coordinate(50, 50),
                    new Coordinate(20, 50),
                    new Coordinate(10, 40),
                    new Coordinate(10, 30),
                    new Coordinate(18, 30),
                },
                new[]
                {
                    new Coordinate(28, 0),
                    new Coordinate(30, -10),
                    new Coordinate(32, 0),
                    new Coordinate(28, 0),
                },
                new[]
                {
                    new Coordinate(48, 0),
                    new Coordinate(50, -10),
                    new Coordinate(50, 0),
                    new Coordinate(48, 0),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsB.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();
        }

        /// <summary>
        /// Tests for polygon intersection with holes.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmHoleTest()
        {
            // hole contained in internal part
            Coordinate[] shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0),
            };
            Coordinate[][] holesA = new[]
            {
                new[]
                {
                    new Coordinate(5, 5),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 5),
                    new Coordinate(5, 5),
                },
            };
            Coordinate[] shellB = new[]
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4),
            };

            GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null, true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 4),
                    new Coordinate(8, 4),
                });
            algorithm.InternalPolygons[0].Holes[0].ShouldBe(
                new[]
                {
                    new Coordinate(5, 5),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 5),
                    new Coordinate(5, 5),
                }, 0.0001);

            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 8),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsB.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 4),
                },
                new[]
                {
                    new Coordinate(5, 5),
                    new Coordinate(7, 5),
                    new Coordinate(7, 7),
                    new Coordinate(5, 7),
                    new Coordinate(5, 5),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);

            // hole overlapping internal and external part
            shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0),
            };
            holesA = new[]
            {
                new[]
                {
                    new Coordinate(5, 1),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 1),
                    new Coordinate(5, 1),
                },
            };
            shellB = new[]
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4),
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null, true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(7, 4),
                    new Coordinate(8, 4),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 4),
                    new Coordinate(5, 4),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 4),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(5, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(7, 4),
                    new Coordinate(7, 1),
                    new Coordinate(5, 1),
                    new Coordinate(5, 4),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsB.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 4),
                },
                new[]
                {
                    new Coordinate(7, 4),
                    new Coordinate(7, 7),
                    new Coordinate(5, 7),
                    new Coordinate(5, 4),
                    new Coordinate(7, 4),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);

            // hole contained in internal part touching side of an external part
            shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0),
            };
            holesA = new[]
            {
                new[]
                {
                    new Coordinate(5, 4),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 4),
                    new Coordinate(5, 4),
                },
            };
            shellB = new[]
            {
                new Coordinate(4, 4),
                new Coordinate(12, 4),
                new Coordinate(12, 12),
                new Coordinate(4, 12),
                new Coordinate(4, 4),
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null, true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(7, 4),
                    new Coordinate(8, 4),
                    new Coordinate(8, 8),
                    new Coordinate(4, 8),
                    new Coordinate(4, 4),
                    new Coordinate(5, 4),
                    new Coordinate(5, 7),
                    new Coordinate(7, 7),
                    new Coordinate(7, 4),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 4),
                    new Coordinate(7, 4),
                    new Coordinate(5, 4),
                    new Coordinate(4, 4),
                    new Coordinate(4, 8),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsB.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(8, 4),
                    new Coordinate(12, 4),
                    new Coordinate(12, 12),
                    new Coordinate(4, 12),
                    new Coordinate(4, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 4),
                },
                new[]
                {
                    new Coordinate(7, 4),
                    new Coordinate(7, 7),
                    new Coordinate(5, 7),
                    new Coordinate(5, 4),
                    new Coordinate(7, 4),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsB.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            // hole is completely filled with the other subject polygon
            shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };
            holesA = new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 2),
                    new Coordinate(2, 2),
                },
            };

            shellB = new[]
            {
                new Coordinate(2, 2),
                new Coordinate(8, 2),
                new Coordinate(8, 8),
                new Coordinate(2, 8),
                new Coordinate(2, 2),
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null, true, null);
            algorithm.InternalPolygons.ShouldBeEmpty();
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                });
            algorithm.ExternalPolygonsA[0].Holes[0].ShouldBe(
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 2),
                    new Coordinate(2, 2),
                });
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(8, 2),
                    new Coordinate(8, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 2),
                });
            algorithm.ExternalPolygonsB.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            // hole is partially filled with the other subject polygon
            shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };
            holesA = new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 2),
                    new Coordinate(2, 2),
                },
            };
            shellB = new[]
            {
                new Coordinate(2, 2),
                new Coordinate(8, 2),
                new Coordinate(8, 8),
                new Coordinate(2, 2),
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null, true, null);
            algorithm.Compute();
            algorithm.InternalPolygons.ShouldBeEmpty();
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                });

            algorithm.ExternalPolygonsA[0].Holes.ShouldBe(new[]
            {
                new[]
                {
                        new Coordinate(2, 2),
                        new Coordinate(2, 8),
                        new Coordinate(8, 8),
                        new Coordinate(8, 2),
                        new Coordinate(2, 2),
                },
            }, 0.0001);

            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(8, 2),
                    new Coordinate(8, 8),
                    new Coordinate(2, 2),
                });
            algorithm.ExternalPolygonsB.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            // shell of the first polygon contains the second and the holes also contain each other
            shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(10, 0),
                new Coordinate(10, 10),
                new Coordinate(0, 10),
                new Coordinate(0, 0),
            };
            holesA = new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 2),
                    new Coordinate(2, 2),
                },
            };
            shellB = new[]
            {
                new Coordinate(-2, -2),
                new Coordinate(12, -2),
                new Coordinate(12, 12),
                new Coordinate(-2, 12),
                new Coordinate(-2, -2),
            };
            Coordinate[][] holesB = new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 2),
                    new Coordinate(2, 2),
                },
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, holesB, true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(10, 0),
                    new Coordinate(10, 10),
                    new Coordinate(0, 10),
                    new Coordinate(0, 0),
                });
            algorithm.InternalPolygons[0].Holes.ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                    new Coordinate(8, 8),
                    new Coordinate(8, 2),
                    new Coordinate(2, 2),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsA.ShouldBeEmpty();
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(-2, -2),
                    new Coordinate(12, -2),
                    new Coordinate(12, 12),
                    new Coordinate(-2, 12),
                    new Coordinate(-2, -2),
                });
            algorithm.ExternalPolygonsB[0].Holes.ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(0, 10),
                    new Coordinate(10, 10),
                    new Coordinate(10, 0),
                    new Coordinate(0, 0),
                },
            }, 0.0001);

            // first polygon contains second polygon, which in reverse contains the hole of the first polygon
            shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0),
            };
            holesA = new[]
            {
                new[]
                {
                    new Coordinate(4, 4),
                    new Coordinate(5, 4),
                    new Coordinate(5, 5),
                    new Coordinate(4, 5),
                    new Coordinate(4, 4),
                },
            };

            shellB = new[]
            {
                new Coordinate(3, 3),
                new Coordinate(3, 6),
                new Coordinate(6, 6),
                new Coordinate(6, 3),
                new Coordinate(3, 3),
            };

            algorithm = new GreinerHormannAlgorithm(shellA, holesA, shellB, null, true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(3, 3),
                    new Coordinate(3, 6),
                    new Coordinate(6, 6),
                    new Coordinate(6, 3),
                    new Coordinate(3, 3),
                });
            algorithm.InternalPolygons[0].Holes.ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(4, 4),
                    new Coordinate(4, 5),
                    new Coordinate(5, 5),
                    new Coordinate(5, 4),
                    new Coordinate(4, 4),
                },
            }, 0.0001);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                });
            algorithm.ExternalPolygonsA[0].Holes.ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(3, 3),
                    new Coordinate(3, 6),
                    new Coordinate(6, 6),
                    new Coordinate(6, 3),
                    new Coordinate(3, 3),
                },
            }, 0.0001);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(4, 4),
                    new Coordinate(4, 5),
                    new Coordinate(5, 5),
                    new Coordinate(5, 4),
                    new Coordinate(4, 4),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);
        }

        /// <summary>
        /// Tests for degenerate intersection of polygons.
        /// </summary>
        [Test]
        public void GreinerHormannAlgorithmDegenerateTest()
        {
            // degenerate test: common edges and multiple exit points in a row
            GreinerHormannAlgorithm algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 2),
                    new Coordinate(4, 2),
                    new Coordinate(4, 0),
                    new Coordinate(8, 0),
                },
                new[]
                {
                    new Coordinate(4, 1),
                    new Coordinate(6, 1),
                    new Coordinate(6, 13),
                    new Coordinate(2, 13),
                    new Coordinate(2, 2),
                    new Coordinate(4, 2),
                    new Coordinate(4, 1),
                },
                true, null);

            algorithm.InternalPolygons.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.InternalPolygons.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(2, 2),
                    new Coordinate(4, 2),
                    new Coordinate(4, 1),
                    new Coordinate(6, 1),
                    new Coordinate(6, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 2),
                },
            }, 0.0001);
            algorithm.InternalPolygons.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            algorithm.ExternalPolygonsA.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsA.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 2),
                    new Coordinate(2, 2),
                    new Coordinate(2, 8),
                },
                new[]
                {
                    new Coordinate(4, 1),
                    new Coordinate(4, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 1),
                    new Coordinate(4, 1),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsA.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            algorithm.ExternalPolygonsB.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(6, 8),
                    new Coordinate(6, 13),
                    new Coordinate(2, 13),
                    new Coordinate(2, 8),
                    new Coordinate(6, 8),
                });
            algorithm.ExternalPolygonsB.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            // degenerate test: common edges and multiple entry points in a row
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 6),
                    new Coordinate(2, 6),
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(-2, 6),
                    new Coordinate(2, 6),
                    new Coordinate(2, 12),
                    new Coordinate(-2, 6),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 6),
                    new Coordinate(2, 6),
                    new Coordinate(2, 8),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(0, 6),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 6),
                    new Coordinate(2, 6),
                    new Coordinate(0, 6),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(2, 12),
                    new Coordinate(-2, 6),
                    new Coordinate(0, 6),
                    new Coordinate(0, 8),
                    new Coordinate(2, 8),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);

            // degenerate test: containment with common edges
            Coordinate[] shellA = new[]
            {
                new Coordinate(0, 0),
                new Coordinate(8, 0),
                new Coordinate(8, 8),
                new Coordinate(0, 8),
                new Coordinate(0, 0),
            };
            Coordinate[] shellB = new[]
            {
                new Coordinate(2, 0),
                new Coordinate(6, 0),
                new Coordinate(6, 8),
                new Coordinate(2, 8),
                new Coordinate(2, 0),
            };

            algorithm = new GreinerHormannAlgorithm(shellA, shellB, true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(2, 0),
                    new Coordinate(6, 0),
                    new Coordinate(6, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 0),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(2);
            algorithm.ExternalPolygonsA.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsA.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(2, 0),
                    new Coordinate(2, 8),
                },
                new[]
                {
                    new Coordinate(6, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 0),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsA.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            algorithm.ExternalPolygonsB.ShouldBeEmpty();

            // the previous test case, only the order of the parameters are swapped
            algorithm = new GreinerHormannAlgorithm(shellB, shellA, true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(2, 0),
                    new Coordinate(6, 0),
                    new Coordinate(6, 8),
                    new Coordinate(2, 8),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.ShouldBeEmpty();

            algorithm.ExternalPolygonsB.Count.ShouldBe(2);
            algorithm.ExternalPolygonsB.ShouldAllBe(polygon => polygon.IsValid);
            algorithm.ExternalPolygonsB.Select(polygon => polygon.Shell).ShouldBe(new[]
            {
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(2, 0),
                    new Coordinate(2, 8),
                },
                new[]
                {
                    new Coordinate(6, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 8),
                    new Coordinate(6, 8),
                    new Coordinate(6, 0),
                },
            }, 0.0001);
            algorithm.ExternalPolygonsB.SelectMany(polygon => polygon.Holes).ShouldBeEmpty();

            // intersecting polygons with tangential internal part
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(6, 0),
                    new Coordinate(6, 5),
                    new Coordinate(3, 5),
                    new Coordinate(3, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(2, 3),
                    new Coordinate(8, 3),
                    new Coordinate(8, 5),
                    new Coordinate(3, 5),
                    new Coordinate(3, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 3),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(6, 3),
                    new Coordinate(6, 5),
                    new Coordinate(3, 5),
                    new Coordinate(3, 8),
                    new Coordinate(2, 8),
                    new Coordinate(2, 3),
                    new Coordinate(6, 3),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(2, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(6, 0),
                    new Coordinate(6, 3),
                    new Coordinate(2, 3),
                    new Coordinate(2, 8),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(6, 3),
                    new Coordinate(8, 3),
                    new Coordinate(8, 5),
                    new Coordinate(6, 5),
                    new Coordinate(6, 3),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);

            // intersecting polygons with common border attached to internal part
            algorithm = new GreinerHormannAlgorithm(
                new[]
                {
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 10),
                    new Coordinate(6, 12),
                    new Coordinate(4, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                },
                new[]
                {
                    new Coordinate(3, 6),
                    new Coordinate(6, 12),
                    new Coordinate(6, 13),
                    new Coordinate(3, 13),
                    new Coordinate(3, 6),
                },
                true, null);

            algorithm.InternalPolygons.Count.ShouldBe(1);
            algorithm.InternalPolygons[0].IsValid.ShouldBeTrue();
            algorithm.InternalPolygons[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(4, 8),
                    new Coordinate(3, 8),
                    new Coordinate(3, 6),
                    new Coordinate(4, 8),
                });
            algorithm.InternalPolygons[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsA.Count.ShouldBe(1);
            algorithm.ExternalPolygonsA[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsA[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(3, 8),
                    new Coordinate(0, 8),
                    new Coordinate(0, 0),
                    new Coordinate(8, 0),
                    new Coordinate(8, 10),
                    new Coordinate(6, 12),
                    new Coordinate(4, 8),
                    new Coordinate(3, 6),
                    new Coordinate(3, 8),
                });
            algorithm.ExternalPolygonsA[0].HoleCount.ShouldBe(0);

            algorithm.ExternalPolygonsB.Count.ShouldBe(1);
            algorithm.ExternalPolygonsB[0].IsValid.ShouldBeTrue();
            algorithm.ExternalPolygonsB[0].Shell.ShouldBe(
                new[]
                {
                    new Coordinate(6, 12),
                    new Coordinate(6, 13),
                    new Coordinate(3, 13),
                    new Coordinate(3, 8),
                    new Coordinate(4, 8),
                    new Coordinate(6, 12),
                });
            algorithm.ExternalPolygonsB[0].HoleCount.ShouldBe(0);
        }
    }
}
