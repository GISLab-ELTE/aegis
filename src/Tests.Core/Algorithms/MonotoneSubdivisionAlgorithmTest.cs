// <copyright file="MonotoneSubdivisionAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test class for the <see cref="MonotoneSubdivisionAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class MonotoneSubdivisionAlgorithmTest
    {
        /// <summary>
        /// The source used for testing.
        /// </summary>
        private List<Coordinate> source;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.source = new List<Coordinate>
            {
                new Coordinate(0, 0), new Coordinate(-10, 5), new Coordinate(10, 10),
                new Coordinate(5, 20), new Coordinate(5, -10), new Coordinate(-10, -10)
            };
        }

        /// <summary>
        /// Tests the constructor of the <see cref="MonotoneSubdivisionAlgorithm" /> class.
        /// </summary>
        [Test]
        public void MonotoneSubdivisionAlgorithmConstructorTest()
        {
            // predefined polygon (which is extended)

            MonotoneSubdivisionAlgorithm algorithm = new MonotoneSubdivisionAlgorithm(this.source, null);
            algorithm.Source.Count().ShouldBe(this.source.Count + 1);
            algorithm.Source.Take(this.source.Count).ShouldBe(this.source);
            algorithm.Source.Last().ShouldBe(this.source[0]);

            // random polygon

            IBasicPolygon polygon = RandomPolygonGenerator.CreateRandomPolygon(100, new Coordinate(10, 10), new Coordinate(50, 50));
            algorithm = new MonotoneSubdivisionAlgorithm(polygon.Shell, null);

            algorithm.Source.ShouldBeSameAs(polygon.Shell);

            // exceptions

            Should.Throw<ArgumentNullException>(() => algorithm = new MonotoneSubdivisionAlgorithm(null, null));
        }

        /// <summary>
        /// Tests the <see cref="MonotoneSubdivisionAlgorithm.Compute()" /> method.
        /// </summary>
        [Test]
        public void MonotoneSubdivisionAlgorithmComputeTest()
        {
            MonotoneSubdivisionAlgorithm algorithm = new MonotoneSubdivisionAlgorithm(this.source, null);

            algorithm.Compute();

            algorithm.Result.Count.ShouldBe(4);
            algorithm.Result[0].ShouldBe(new[] { new Coordinate(10, 10), new Coordinate(5, 20), new Coordinate(5, -10) });
            algorithm.Result[1].ShouldBe(new[] { new Coordinate(0, 0), new Coordinate(-10, 5), new Coordinate(10, 10) });
            algorithm.Result[2].ShouldBe(new[] { new Coordinate(0, 0), new Coordinate(10, 10), new Coordinate(5, -10) });
            algorithm.Result[3].ShouldBe(new[] { new Coordinate(0, 0), new Coordinate(5, -10), new Coordinate(-10, -10) });
        }

        /// <summary>
        /// Tests the <see cref="MonotoneSubdivisionAlgorithm.Triangulate(IReadOnlyList{Coordinate})" /> method.
        /// </summary>
        [Test]
        public void MonotoneSubdivisionTriangulateTest()
        {
            for (Int32 polygonNumber = 1; polygonNumber < 10; polygonNumber++)
            {
                IBasicPolygon polygon = RandomPolygonGenerator.CreateRandomPolygon(10 * polygonNumber, new Coordinate(10, 10), new Coordinate(50, 50));
                IReadOnlyList<Coordinate[]> triangles = MonotoneSubdivisionAlgorithm.Triangulate(polygon.Shell);

                triangles.Count.ShouldBe(polygon.Shell.Count - 3);

                foreach (Coordinate[] triangle in triangles)
                {
                    triangle.Length.ShouldBe(3);
                    triangle.ShouldBeSubsetOf(polygon.Shell);
                }
            }
        }
    }
}
