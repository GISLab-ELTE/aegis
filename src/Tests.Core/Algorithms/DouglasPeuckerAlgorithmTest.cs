// <copyright file="DouglasPeuckerAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="DouglasPeuckerAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class DouglasPeuckerAlgorithmTest
    {
        /// <summary>
        /// Tests the <see cref="DouglasPeuckerAlgorithm.Simplify(IReadOnlyList{Coordinate}, Double)" /> method.
        /// </summary>
        [Test]
        public void DouglasPeuckerAlgorithmSimplifyTest()
        {
            // first line string

            Double delta = 5.0;

            List<Coordinate> coordinates = new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(1.0, 1.0, 0.0),
                new Coordinate(2.0, 5.0, 0.0),
                new Coordinate(5.0, 2.0, 0.0)
            });

            IReadOnlyList<Coordinate> result = DouglasPeuckerAlgorithm.Simplify(coordinates, delta);

            result.Count.ShouldBe(2);
            result[0].X.ShouldBe(1.0);
            result[0].Y.ShouldBe(1.0);
            result[1].X.ShouldBe(5.0);
            result[1].Y.ShouldBe(2.0);

            // third line string

            delta = 2.0;

            coordinates = new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(0.0, 3.0, 0.0),
                new Coordinate(0.5, 3.0, 1.0),
                new Coordinate(1.5, 2.0, 0.0),
                new Coordinate(2.5, 1.0, 0.0),
                new Coordinate(3.7, 3.9, 0.0),
                new Coordinate(5.0, 3.0, 0.0)
            });

            result = DouglasPeuckerAlgorithm.Simplify(coordinates, delta);

            result.Count.ShouldBe(2);
            result[0].X.ShouldBe(0.0);
            result[0].Y.ShouldBe(3.0);
            result[0].Z.ShouldBe(0.0);
            result[1].X.ShouldBe(5.0);
            result[1].Y.ShouldBe(3.0);
            result[1].Z.ShouldBe(0.0);

            // third line string

            delta = 2.0;

            coordinates = new List<Coordinate>(new Coordinate[]
            {
                new Coordinate(0.0, 3.0, 0.0),
                new Coordinate(0.0, 3.0, 3.0),
                new Coordinate(1.5, 2.0, 0.0),
                new Coordinate(2.5, 1.0, 0.0),
                new Coordinate(3.7, 3.9, 0.0),
                new Coordinate(5.0, 3.0, 0.0)
            });

            result = DouglasPeuckerAlgorithm.Simplify(coordinates, delta);

            result.Count.ShouldBe(3);
            result[0].X.ShouldBe(0.0);
            result[0].Y.ShouldBe(3.0);
            result[0].Z.ShouldBe(0.0);
            result[1].X.ShouldBe(0.0);
            result[1].Y.ShouldBe(3.0);
            result[1].Z.ShouldBe(3.0);
            result[2].X.ShouldBe(5.0);
            result[2].Y.ShouldBe(3.0);
            result[2].Z.ShouldBe(0.0);

            // exceptions

            Should.Throw<ArgumentNullException>(() => DouglasPeuckerAlgorithm.Simplify((List<Coordinate>)null, 1.0));
            Should.Throw<ArgumentOutOfRangeException>(() => DouglasPeuckerAlgorithm.Simplify(coordinates, 0));
        }
    }
}
