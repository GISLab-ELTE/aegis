// <copyright file="LiangBarskyAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="LiangBarskyAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class LiangBarskyAlgorithmTest
    {
        /// <summary>
        /// The clipping window used for testing.
        /// </summary>
        private Envelope clippingWindow;

        /// <summary>
        /// The array of coordinate arrays used for testing.
        /// </summary>
        private Coordinate[][] coordinates;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.clippingWindow = new Envelope(10, 20, 10, 20);
            this.coordinates = new[]
            {
                new[]
                {
                    new Coordinate(5, 15),
                    new Coordinate(15, 15),
                    new Coordinate(25, 15),
                },
                new[]
                {
                    new Coordinate(5, 15),
                    new Coordinate(14, 15),
                    new Coordinate(18, 15),
                    new Coordinate(25, 15),
                },
                new[]
                {
                    new Coordinate(5, 15),
                    new Coordinate(25, 15),
                },
                new[]
                {
                    new Coordinate(5, 12),
                    new Coordinate(25, 12),
                    new Coordinate(25, 18),
                    new Coordinate(5, 18),
                },
            };
        }

        /// <summary>
        /// Tests the <see cref="LiangBarskyAlgorithm.Compute()" /> method.
        /// </summary>
        [Test]
        public void LiangBarskyAlgorithmComputeTest()
        {
            List<IReadOnlyList<Coordinate>> totalExpected = new List<IReadOnlyList<Coordinate>>();
            Coordinate[][] expected = new[] { new[] { new Coordinate(10, 15), new Coordinate(15, 15), new Coordinate(20, 15) } };
            totalExpected.AddRange(expected);

            LiangBarskyAlgorithm algorithm = new LiangBarskyAlgorithm(this.coordinates[0], this.clippingWindow);
            algorithm.Compute();
            algorithm.Result.ShouldBe(expected);

            expected = new[] { new[] { new Coordinate(10, 15), new Coordinate(14, 15), new Coordinate(18, 15), new Coordinate(20, 15) } };
            totalExpected.AddRange(expected);

            algorithm = new LiangBarskyAlgorithm(this.coordinates[1], this.clippingWindow);
            algorithm.Compute();
            algorithm.Result.ShouldBe(expected);

            expected = new[] { new[] { new Coordinate(10, 15), new Coordinate(20, 15) } };
            totalExpected.AddRange(expected);

            algorithm = new LiangBarskyAlgorithm(this.coordinates[2], this.clippingWindow);
            algorithm.Compute();
            algorithm.Result.ShouldBe(expected);

            expected = new[] { new[] { new Coordinate(10, 12), new Coordinate(20, 12) }, new[] { new Coordinate(20, 18), new Coordinate(10, 18) } };
            totalExpected.AddRange(expected);

            algorithm = new LiangBarskyAlgorithm(this.coordinates[3], this.clippingWindow);
            algorithm.Compute();
            algorithm.Result.ShouldBe(expected);

            algorithm = new LiangBarskyAlgorithm(this.coordinates, this.clippingWindow);
            algorithm.Compute();
            IEnumerable<IReadOnlyList<Coordinate>> totalResult = algorithm.Result;

            Int32 index = 0;
            foreach (IReadOnlyList<Coordinate> result in totalResult)
            {
                result.ShouldBe(totalExpected[index]);
                index++;
            }
        }

        /// <summary>
        /// Tests the <see cref="LiangBarskyAlgorithm.Clip(IBasicLineString, Envelope)" /> method.
        /// </summary>
        [Test]
        public void LiangBarskyAlgorithmClipBasicLineStringTest()
        {
            BasicLineString source = new BasicLineString(new[] { new Coordinate(5, 15), new Coordinate(15, 15), new Coordinate(25, 15) });
            IReadOnlyList<IReadOnlyList<Coordinate>> actual = LiangBarskyAlgorithm.Clip(source, this.clippingWindow);
            Coordinate[][] expected = new[] { new[] { new Coordinate(10, 15), new Coordinate(15, 15), new Coordinate(20, 15) } };
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests the <see cref="LiangBarskyAlgorithm.Clip(IBasicPolygon, Envelope)" /> method.
        /// </summary>
        [Test]
        public void LiangBarskyAlgorithmClipBasicPolygonTest()
        {
            BasicPolygon source = new BasicPolygon(new[] { new Coordinate(5, 12), new Coordinate(25, 12), new Coordinate(25, 18), new Coordinate(5, 18) });
            IReadOnlyList<IReadOnlyList<Coordinate>> actual = LiangBarskyAlgorithm.Clip(source, this.clippingWindow);
            Coordinate[][] expected = new[] { new[] { new Coordinate(10, 12), new Coordinate(20, 12) }, new[] { new Coordinate(20, 18), new Coordinate(10, 18) } };
            actual.ShouldBe(expected);
        }
    }
}
