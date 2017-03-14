// <copyright file="GrahamScanAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Algorithms
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="GrahamScanAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class GrahamScanAlgorithmTest
    {
        /// <summary>
        /// Tests the <see cref="GrahamScanAlgorithm.ComputeConvexHull(IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void GrahamScanAlgorithmComputeConvexHullTest()
        {
            // convex polygon
            Coordinate[] shell = new[]
            {
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10),
                new Coordinate(0, 0)
            };

            IReadOnlyList<Coordinate> convexHull = GrahamScanAlgorithm.ComputeConvexHull(shell);
            convexHull.ShouldBe(shell);

            // concave polygon
            shell = new[]
            {
                new Coordinate(0, 0), new Coordinate(2, 1), new Coordinate(8, 1),
                new Coordinate(10, 0), new Coordinate(9, 2), new Coordinate(9, 8),
                new Coordinate(10, 10), new Coordinate(8, 9), new Coordinate(2, 9),
                new Coordinate(0, 10), new Coordinate(1, 8), new Coordinate(1, 2),
                new Coordinate(0, 0)
            };

            Coordinate[] expected = new[]
            {
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10),
                new Coordinate(0, 0)
            };

            convexHull = GrahamScanAlgorithm.ComputeConvexHull(shell);
            convexHull.ShouldBe(expected);
        }
    }
}
