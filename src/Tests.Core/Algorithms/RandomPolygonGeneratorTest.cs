// <copyright file="RandomPolygonGeneratorTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Algorithms;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="RandomPolygonGenerator" /> class.
    /// </summary>
    [TestFixture]
    public class RandomPolygonGeneratorTest
    {
        /// <summary>
        /// Tests the <see cref="CreateRandomPolygon" /> method.
        /// </summary>
        [Test]
        public void RandomPolygonGeneratorCreateRandomPolygonTest()
        {
            // check properties of generated polygons

            Int32 coordinateNumber = 10;
            Coordinate minCoordinate = new Coordinate(10, 10);
            Coordinate maxCoordinate = new Coordinate(20, 20);
            Double convexityRatio = 0.1;

            for (Int32 polygonNumber = 1; polygonNumber < 20; polygonNumber++)
            {
                IBasicPolygon randomPolygon = RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber * polygonNumber, minCoordinate, maxCoordinate);

                randomPolygon.Shell.Count.ShouldBe(coordinateNumber * polygonNumber + 1); // number of coordinates
                PolygonAlgorithms.Orientation(randomPolygon).ShouldBe(Orientation.Counterclockwise); // orientation

                foreach (Coordinate coordinate in randomPolygon.Shell)
                {
                    (coordinate.X > minCoordinate.X && coordinate.X < maxCoordinate.X && coordinate.Y > minCoordinate.Y && coordinate.Y < maxCoordinate.Y).ShouldBeTrue(); // all coordinates are located in the rectangle
                }

                ShamosHoeyAlgorithm.Intersects(randomPolygon.Shell).ShouldBeFalse(); // no intersection
            }

            // check convexity
            for (Int32 polygonNumber = 1; polygonNumber < 20; polygonNumber++)
            {
                PolygonAlgorithms.IsConvex(RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber * polygonNumber, minCoordinate, maxCoordinate, 1)).ShouldBeTrue();
            }

            for (Int32 polygonNumber = 1; polygonNumber < 20; polygonNumber++)
            {
                PolygonAlgorithms.IsConvex(RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber * polygonNumber, minCoordinate, maxCoordinate, 0)).ShouldBeFalse();
            }

            // check exceptions

            Should.Throw<ArgumentOutOfRangeException>(() => RandomPolygonGenerator.CreateRandomPolygon(1, minCoordinate, maxCoordinate, convexityRatio));
            Should.Throw<ArgumentOutOfRangeException>(() => RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber, maxCoordinate, minCoordinate, convexityRatio));
            Should.Throw<ArgumentOutOfRangeException>(() => RandomPolygonGenerator.CreateRandomPolygon(coordinateNumber, minCoordinate, maxCoordinate, -1));
        }
    }
}
