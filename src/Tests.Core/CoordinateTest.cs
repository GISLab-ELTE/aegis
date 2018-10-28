// <copyright file="CoordinateTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Coordinate" /> class.
    /// </summary>
    [TestFixture]
    public class CoordinateTest
    {
        /// <summary>
        /// Test properties of the <see cref="Coordinate" /> method.
        /// </summary>
        [Test]
        public void CoordinatePropertiesTest()
        {
            for (Int32 number = 1; number < 100; number++)
            {
                Coordinate coordinate = new Coordinate(number, number, number);
                coordinate.X.ShouldBe(number);
                coordinate.Y.ShouldBe(number);
                coordinate.Z.ShouldBe(number);
                coordinate.IsEmpty.ShouldBeFalse();
                coordinate.IsValid.ShouldBeTrue();
            }

            new Coordinate(0, 0, 0).IsEmpty.ShouldBeTrue();
            new Coordinate(0, 0, 0).IsValid.ShouldBeTrue();

            new Coordinate(Double.NaN, 0, 0).IsEmpty.ShouldBeFalse();
            new Coordinate(Double.NaN, 0, 0).IsValid.ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="Coordinate.Angle(Coordinate, Coordinate, Coordinate)" /> method.
        /// </summary>
        [Test]
        public void CoordinateAngleTest()
        {
            // 90°
            Coordinate origin = new Coordinate(0, 0);
            Coordinate first = new Coordinate(1, 0);
            Coordinate second = new Coordinate(0, 1);

            Coordinate.Angle(origin, first, second).ShouldBe(Math.PI / 2, 0.00001);

            // 180°
            origin = new Coordinate(0, 0);
            first = new Coordinate(1, 0);
            second = new Coordinate(-1, 0);

            Coordinate.Angle(origin, first, second).ShouldBe(Math.PI, 0.00001);

            // -180°
            origin = new Coordinate(0, 0);
            first = new Coordinate(-1, 0);
            second = new Coordinate(1, 0);

            Coordinate.Angle(origin, first, second).ShouldBe(Math.PI, 0.00001);

            // 45°
            origin = new Coordinate(0, 0);
            first = new Coordinate(1, 0);
            second = new Coordinate(1, 1);

            Coordinate.Angle(origin, first, second).ShouldBe(Math.PI / 4, 0.00001);

            // 60°
            origin = new Coordinate(0, 0);
            first = new Coordinate(1, 0);
            second = new Coordinate(1, Math.Sqrt(3));

            Coordinate.Angle(origin, first, second).ShouldBe(Math.PI / 3, 0.00001);
        }

        /// <summary>
        /// Tests the <see cref="Coordinate.Orientation(Coordinate, Coordinate, Coordinate)" /> method.
        /// </summary>
        [Test]
        public void CoordinateOrientationTest()
        {
            Orientation orientation = Coordinate.Orientation(Coordinate.Empty, new Coordinate(0, 1), new Coordinate(1, 0));
            orientation.ShouldBe(Orientation.Clockwise);

            orientation = Coordinate.Orientation(Coordinate.Empty, new Coordinate(0, 1), new Coordinate(0, 1));
            orientation.ShouldBe(Orientation.Collinear);

            orientation = Coordinate.Orientation(Coordinate.Empty, new Coordinate(1, 1), new Coordinate(10, 10));
            orientation.ShouldBe(Orientation.Collinear);

            orientation = Coordinate.Orientation(Coordinate.Empty, new Coordinate(0, 1), new Coordinate(0, 10));
            orientation.ShouldBe(Orientation.Collinear);

            orientation = Coordinate.Orientation(Coordinate.Empty, new Coordinate(0, 1), new Coordinate(0, -1));
            orientation.ShouldBe(Orientation.Collinear);

            orientation = Coordinate.Orientation(Coordinate.Empty, new Coordinate(0, 1), new Coordinate(-1, 0));
            orientation.ShouldBe(Orientation.Counterclockwise);

            orientation = Coordinate.Orientation(Coordinate.Empty, new Coordinate(0, 1), Coordinate.Undefined);
            orientation.ShouldBe(Orientation.Undefined);
        }
    }
}
