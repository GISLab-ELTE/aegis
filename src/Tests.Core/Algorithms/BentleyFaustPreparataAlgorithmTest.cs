// <copyright file="BentleyFaustPreparataAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="BentleyFaustPreparataAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class BentleyFaustPreparataAlgorithmTest
    {
        /// <summary>
        /// Tests the <see cref="BentleyFaustPreparataAlgorithm.ApproximateConvexHull(IBasicPolygon)" /> method.
        /// </summary>
        [Test]
        public void BentleyFaustPreparataAlgorithmComputeConvexHullTest()
        {
            // convex polygon
            List<Coordinate> shell = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10),
                new Coordinate(0, 0)
            };

            IReadOnlyList<Coordinate> convexHull = BentleyFaustPreparataAlgorithm.ApproximateConvexHull(shell);
            convexHull.Count.ShouldBe(shell.Count);

            for (Int32 shellIndex = 0; shellIndex < shell.Count; shellIndex++)
                convexHull[shellIndex].ShouldBe(shell[shellIndex]);

            // concave polygon
            shell = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(2, 1), new Coordinate(8, 1),
                new Coordinate(10, 0), new Coordinate(9, 2), new Coordinate(9, 8),
                new Coordinate(10, 10), new Coordinate(8, 9), new Coordinate(2, 9),
                new Coordinate(0, 10), new Coordinate(1, 8), new Coordinate(1, 2),
                new Coordinate(0, 0)
            };

            List<Coordinate> expected = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(10, 0),
                new Coordinate(10, 10), new Coordinate(0, 10),
                new Coordinate(0, 0)
            };

            convexHull = BentleyFaustPreparataAlgorithm.ApproximateConvexHull(shell);
            convexHull.Count.ShouldBe(expected.Count);

            for (Int32 index = 0; index < expected.Count; index++)
                convexHull[index].ShouldBe(expected[index]);
        }
    }
}
