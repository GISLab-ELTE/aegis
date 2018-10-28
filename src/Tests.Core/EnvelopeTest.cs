// <copyright file="EnvelopeTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Collections;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Envelope" /> class.
    /// </summary>
    [TestFixture]
    public class EnvelopeTest
    {
        /// <summary>
        /// Tests the constructor of the <see cref="Envelope" /> class.
        /// </summary>
        [Test]
        public void EnvelopeConstructorTest()
        {
            // random values
            Int32[] values = Collection.GenerateNumbers(-100000, 100000, 600).ToArray();

            for (Int32 i = 0; i < 100; i++)
            {
                Double firstX = values[i];
                Double firstY = values[100 + i];
                Double firstZ = values[200 + i];
                Double secondX = values[300 + i];
                Double secondY = values[400 + i];
                Double secondZ = values[500 + i];

                Envelope envelope = new Envelope(firstX, secondX, firstY, secondY, firstZ, secondZ);

                envelope.Minimum.X.ShouldBe(Math.Min(firstX, secondX));
                envelope.MinX.ShouldBe(Math.Min(firstX, secondX));
                envelope.Minimum.Y.ShouldBe(Math.Min(firstY, secondY));
                envelope.MinY.ShouldBe(Math.Min(firstY, secondY));
                envelope.Minimum.Z.ShouldBe(Math.Min(firstZ, secondZ));
                envelope.MinZ.ShouldBe(Math.Min(firstZ, secondZ));
                envelope.Maximum.X.ShouldBe(Math.Max(firstX, secondX));
                envelope.MaxX.ShouldBe(Math.Max(firstX, secondX));
                envelope.Maximum.Y.ShouldBe(Math.Max(firstY, secondY));
                envelope.MaxY.ShouldBe(Math.Max(firstY, secondY));
                envelope.Maximum.Z.ShouldBe(Math.Max(firstZ, secondZ));
                envelope.MaxZ.ShouldBe(Math.Max(firstZ, secondZ));
                envelope.ToString().ShouldBe("(" + envelope.MinX + " " + envelope.MinY + " " + envelope.MinZ + ", " + envelope.MaxX + " " + envelope.MaxY + " " + envelope.MaxZ + ")");

                Envelope other = new Envelope(firstX, secondX, firstY, secondY, firstZ, secondZ);

                other.GetHashCode().ShouldBe(envelope.GetHashCode());
                other.ToString().ShouldBe(envelope.ToString());
            }

            // empty envelope
            new Envelope(0, 0, 0, 0, 0, 0).ToString().ShouldBe("EMPTY (0 0 0)");
            new Envelope(0, 0, 10, 10, 100, 100).ToString().ShouldBe("EMPTY (0 10 100)");
        }

        /// <summary>
        /// Tests the properties of the <see cref="Envelope" /> class.
        /// </summary>
        [Test]
        public void EnvelopePropertiesTest()
        {
            // static instances
            Envelope.Infinity.IsEmpty.ShouldBeFalse();
            Envelope.Infinity.IsPlanar.ShouldBeFalse();
            Envelope.Infinity.IsValid.ShouldBeTrue();

            // empty instance
            Envelope envelope = new Envelope(10, 10, 10, 10, 10, 10);

            envelope.IsEmpty.ShouldBeTrue();
            envelope.IsPlanar.ShouldBeTrue();
            envelope.IsValid.ShouldBeTrue();
            envelope.Center.ShouldBe(new Coordinate(10, 10, 10));
            envelope.Surface.ShouldBe(0);
            envelope.Volume.ShouldBe(0);

            // planar instance
            envelope = new Envelope(10, 20, 100, 200, 10, 10);

            envelope.IsEmpty.ShouldBeFalse();
            envelope.IsPlanar.ShouldBeTrue();
            envelope.IsValid.ShouldBeTrue();
            envelope.Center.ShouldBe(new Coordinate(15, 150, 10));
            envelope.Surface.ShouldBe(1000);
            envelope.Volume.ShouldBe(0);

            // general instance
            envelope = new Envelope(10, 20, 100, 200, 1000, 2000);

            envelope.IsEmpty.ShouldBeFalse();
            envelope.IsPlanar.ShouldBeFalse();
            envelope.IsValid.ShouldBeTrue();
            envelope.Center.ShouldBe(new Coordinate(15, 150, 1500));
            envelope.Surface.ShouldBe(222000);
            envelope.Volume.ShouldBe(1000000);
        }

        /// <summary>
        /// Tests equality of <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeEqualsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);
            Envelope second = new Envelope(10, 20, 100, 200, 1000, 2000);

            Envelope[] others = new Envelope[]
            {
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001)
            };

            // IEquatable method
            first.Equals(null).ShouldBeFalse();
            first.Equals(first).ShouldBeTrue();
            first.Equals(second).ShouldBeTrue();

            foreach (Envelope envelope in others)
                first.Equals(envelope).ShouldBeFalse();

            // Object method
            first.Equals((Object)null).ShouldBeFalse();
            first.Equals((Object)first).ShouldBeTrue();
            first.Equals((Object)second).ShouldBeTrue();

            foreach (Envelope envelope in others)
                first.Equals((Object)envelope).ShouldBeFalse();

            // static method
            Envelope.Equals(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeTrue();
            Envelope.Equals(first.Minimum, first.Maximum, second.Minimum, second.Maximum).ShouldBeTrue();

            foreach (Envelope envelope in others)
                Envelope.Equals(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
        }

        /// <summary>
        /// Tests containment of <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeContainsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // coordinate containment, object method
            first.Contains(Coordinate.Empty).ShouldBeFalse();
            first.Contains(Coordinate.Undefined).ShouldBeFalse();
            first.Contains(new Coordinate(15, 150, 1500)).ShouldBeTrue();

            // coordinate containment, static method
            Envelope.Contains(first.Minimum, first.Maximum, Coordinate.Empty).ShouldBeFalse();
            Envelope.Contains(first.Minimum, first.Maximum, Coordinate.Undefined).ShouldBeFalse();
            Envelope.Contains(first.Minimum, first.Maximum, new Coordinate(15, 150, 1500)).ShouldBeTrue();

            // envelope containment, false cases
            Envelope[] others = new Envelope[]
            {
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(9, 20, 100, 200, 1000, 2000),
                new Envelope(10, 20, 99, 200, 1000, 2000),
                new Envelope(10, 20, 100, 200, 999, 2000),
                new Envelope(9, 21, 99, 201, 999, 2001)
            };

            foreach (Envelope envelope in others)
            {
                first.Contains(envelope).ShouldBeFalse();
                Envelope.Contains(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
            }

            // envelope containment, true cases
            first.Contains(first).ShouldBeTrue();
            Envelope.Contains(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeTrue();

            others = new Envelope[]
            {
                new Envelope(11, 19, 101, 199, 1001, 1999),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 19, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 199, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 1999)
            };

            foreach (Envelope envelope in others)
            {
                first.Contains(envelope).ShouldBeTrue();
                Envelope.Contains(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeTrue();
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => first.Contains((Coordinate)null));
            Should.Throw<ArgumentNullException>(() => first.Contains((Envelope)null));
        }

        /// <summary>
        /// Tests crossing of <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeCrossesTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // false cases
            first.Crosses(first).ShouldBeFalse();
            Envelope.Crosses(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeFalse();

            Envelope[] others = new Envelope[]
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Crosses(envelope).ShouldBeFalse();
                Envelope.Crosses(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
            }

            // true cases
            others = new Envelope[]
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001)
            };

            foreach (Envelope envelope in others)
            {
                first.Crosses(envelope).ShouldBeTrue();
                Envelope.Crosses(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeTrue();
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => first.Crosses(null));
        }

        /// <summary>
        /// Tests disjointedness of <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeDisjointTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // false cases
            first.Disjoint(first).ShouldBeFalse();
            Envelope.Disjoint(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeFalse();

            Envelope[] others = new Envelope[]
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(20, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 200, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 1000, 2000),
                new Envelope(10, 20, 200, 300, 2000, 3000),
                new Envelope(20, 30, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 2000, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Disjoint(envelope).ShouldBeFalse();
                Envelope.Disjoint(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
            }

            // true cases
            others = new Envelope[]
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Disjoint(envelope).ShouldBeTrue();
                Envelope.Disjoint(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeTrue();
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => first.Disjoint(null));
        }

        /// <summary>
        /// Tests intersection of <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeIntersectsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // false cases
            Envelope[] others = new Envelope[]
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Intersects(envelope).ShouldBeFalse();
                Envelope.Intersects(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
            }

            // true cases
            first.Intersects(first).ShouldBeTrue();
            Envelope.Intersects(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeTrue();

            others = new Envelope[]
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(20, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 200, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 1000, 2000),
                new Envelope(10, 20, 200, 300, 2000, 3000),
                new Envelope(20, 30, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 2000, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Intersects(envelope).ShouldBeTrue();
                Envelope.Intersects(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeTrue();
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => first.Intersects(null));
        }

        /// <summary>
        /// Tests overlapping of <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeOverlapsTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // false cases
            first.Overlaps(first).ShouldBeFalse();
            Envelope.Overlaps(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeFalse();

            Envelope[] others = new Envelope[]
            {
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Overlaps(envelope).ShouldBeFalse();
                Envelope.Overlaps(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
            }

            // true cases
            others = new Envelope[]
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001)
            };

            foreach (Envelope envelope in others)
            {
                first.Overlaps(envelope).ShouldBeTrue();
                Envelope.Overlaps(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeTrue();
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => first.Overlaps(null));
        }

        /// <summary>
        /// Tests touching of <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeTouchesTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // false cases
            first.Touches(first).ShouldBeFalse();
            Envelope.Touches(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeFalse();

            Envelope[] others = new Envelope[]
            {
                new Envelope(11, 21, 101, 201, 1001, 2001),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Touches(envelope).ShouldBeFalse();
                Envelope.Touches(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
            }

            // true cases
            others = new Envelope[]
            {
                new Envelope(20, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 200, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 1000, 2000),
                new Envelope(10, 20, 200, 300, 2000, 3000),
                new Envelope(20, 30, 100, 200, 2000, 3000),
                new Envelope(20, 30, 200, 300, 2000, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Touches(envelope).ShouldBeTrue();
                Envelope.Touches(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeTrue();
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => first.Touches(null));
        }

        /// <summary>
        /// Tests containment within <see cref="Envelope" /> instances.
        /// </summary>
        [Test]
        public void EnvelopeWithinTest()
        {
            Envelope first = new Envelope(10, 20, 100, 200, 1000, 2000);

            // false cases
            Envelope[] others = new Envelope[]
            {
                new Envelope(11, 19, 101, 199, 1001, 1999),
                new Envelope(11, 20, 100, 200, 1000, 2000),
                new Envelope(10, 19, 100, 200, 1000, 2000),
                new Envelope(10, 20, 101, 200, 1000, 2000),
                new Envelope(10, 20, 100, 199, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1001, 2000),
                new Envelope(10, 20, 100, 200, 1000, 1999),
                new Envelope(21, 30, 100, 200, 1000, 2000),
                new Envelope(10, 20, 201, 300, 1000, 2000),
                new Envelope(10, 20, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 1000, 2000),
                new Envelope(10, 20, 201, 300, 2001, 3000),
                new Envelope(21, 30, 100, 200, 2001, 3000),
                new Envelope(21, 30, 201, 300, 2001, 3000)
            };

            foreach (Envelope envelope in others)
            {
                first.Within(envelope).ShouldBeFalse();
                Envelope.Within(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeFalse();
            }

            // true cases
            first.Within(first).ShouldBeTrue();
            Envelope.Within(first.Minimum, first.Maximum, first.Minimum, first.Maximum).ShouldBeTrue();

            others = new Envelope[]
            {
                new Envelope(10, 21, 100, 200, 1000, 2000),
                new Envelope(10, 20, 100, 201, 1000, 2000),
                new Envelope(10, 20, 100, 200, 1000, 2001),
                new Envelope(9, 20, 100, 200, 1000, 2000),
                new Envelope(10, 20, 99, 200, 1000, 2000),
                new Envelope(10, 20, 100, 200, 999, 2000),
                new Envelope(9, 21, 99, 201, 999, 2001)
            };

            foreach (Envelope envelope in others)
            {
                first.Within(envelope).ShouldBeTrue();
                Envelope.Within(first.Minimum, first.Maximum, envelope.Minimum, envelope.Maximum).ShouldBeTrue();
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => first.Within(null));
        }

        /// <summary>
        /// Tests <see cref="Envelope" /> construction from coordinates.
        /// </summary>
        [Test]
        public void EnvelopeFromCoordinatesTest()
        {
            Envelope.FromCoordinates(null).ShouldBeNull();
            Envelope.FromCoordinates((IEnumerable<Coordinate>)null).ShouldBeNull();
            Envelope.FromCoordinates(new Coordinate[0]).ShouldBeNull();

            Coordinate[] source = Enumerable.Range(0, 11).Select(value => new Coordinate(value, value, value)).ToArray();
            Envelope.FromCoordinates(source).ShouldBe(new Envelope(0, 10, 0, 10, 0, 10));
            Envelope.FromCoordinates(source.ToList()).ShouldBe(new Envelope(0, 10, 0, 10, 0, 10));

            source = Enumerable.Range(0, 11).Select(value => new Coordinate(value, 10 - value, Math.Abs(value - 5))).ToArray();
            Envelope.FromCoordinates(source).ShouldBe(new Envelope(0, 10, 0, 10, 0, 5));
            Envelope.FromCoordinates(source.ToList()).ShouldBe(new Envelope(0, 10, 0, 10, 0, 5));

            source = Enumerable.Repeat(0, 11).Select(value => new Coordinate(value, value, value)).ToArray();
            Envelope.FromCoordinates(source).ShouldBe(new Envelope(0, 0, 0, 0, 0, 0));
            Envelope.FromCoordinates(source.ToList()).ShouldBe(new Envelope(0, 0, 0, 0, 0, 0));
        }

        /// <summary>
        /// Tests <see cref="Envelope" /> construction from envelopes.
        /// </summary>
        [Test]
        public void EnvelopeFromEnvelopesTest()
        {
            Envelope.FromEnvelopes(null).ShouldBeNull();
            Envelope.FromEnvelopes((IEnumerable<Envelope>)null).ShouldBeNull();
            Envelope.FromEnvelopes(new Envelope[0]).ShouldBeNull();

            Envelope[] source = Enumerable.Range(0, 11).Select(value => new Envelope(value, value, value, value, value, value)).ToArray();

            Envelope.FromEnvelopes(source).ShouldBe(new Envelope(0, 10, 0, 10, 0, 10));
            Envelope.FromEnvelopes(source.ToList()).ShouldBe(new Envelope(0, 10, 0, 10, 0, 10));

            source = Enumerable.Range(0, 11).Select(value =>
                new Envelope(value, value, 10 - value, 10 - value, Math.Abs(value - 5), Math.Abs(value - 5))).ToArray();

            Envelope.FromEnvelopes(source).ShouldBe(new Envelope(0, 10, 0, 10, 0, 5));
            Envelope.FromEnvelopes(source.ToList()).ShouldBe(new Envelope(0, 10, 0, 10, 0, 5));

            source = Enumerable.Range(0, 11).Select(value =>
                new Envelope(value, 10 - value, Math.Abs(value - 5), 10 - value, Math.Abs(value - 5), value)).ToArray();

            Envelope.FromEnvelopes(source).ShouldBe(new Envelope(0, 10, 0, 10, 0, 10));
            Envelope.FromEnvelopes(source.ToList()).ShouldBe(new Envelope(0, 10, 0, 10, 0, 10));

            source = Enumerable.Repeat(0, 11).Select(value => new Envelope(value, value, value, value, value, value)).ToArray();
            Envelope.FromEnvelopes(source).ShouldBe(new Envelope(0, 0, 0, 0, 0, 0));
            Envelope.FromEnvelopes(source.ToList()).ShouldBe(new Envelope(0, 0, 0, 0, 0, 0));
        }
    }
}
