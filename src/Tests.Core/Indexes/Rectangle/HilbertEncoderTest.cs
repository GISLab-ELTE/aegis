// <copyright file="HilbertEncoderTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Indexes.Rectangle
{
    using System;
    using System.Numerics;
    using System.Text;
    using NUnit.Framework;
    using Shouldly;
    using static AEGIS.Indexes.Rectangle.HilbertRTree;

    [TestFixture]
    public class HilbertEncoderTest
    {
        [Test]
        public void EncodeInteger2DPositivePointOnHilbertCurveTest()
        {
            HilbertEncoder encoder = new HilbertEncoder(false, false);

            BigInteger hilbertCode = encoder.Encode(new Coordinate(0, 0));
            hilbertCode.ShouldBe(new BigInteger(0));

            hilbertCode = encoder.Encode(new Coordinate(1, 0));
            hilbertCode.ShouldBe(new BigInteger(1));

            hilbertCode = encoder.Encode(new Coordinate(0, 1));
            hilbertCode.ShouldBe(new BigInteger(3));

            hilbertCode = encoder.Encode(new Coordinate(3, 1));
            hilbertCode.ShouldBe(new BigInteger(12));

            hilbertCode = encoder.Encode(new Coordinate(5, 5));
            hilbertCode.ShouldBe(new BigInteger(34));
        }

        [Test]
        public void EncodeFractional2DPositivePointOnHilbertCurveTest()
        {
            HilbertEncoder encoder = new HilbertEncoder(false, false);

            BigInteger hilbertCode = encoder.Encode(new Coordinate(0.1D, 0.1D));
            hilbertCode.ShouldBe(new BigInteger(0));

            hilbertCode = encoder.Encode(new Coordinate(0.8D, 0D));
            hilbertCode.ShouldBe(new BigInteger(1));

            hilbertCode = encoder.Encode(new Coordinate(0.5D, 0D));
            hilbertCode.ShouldBe(new BigInteger(0));

            hilbertCode = encoder.Encode(new Coordinate(0, 0.8D));
            hilbertCode.ShouldBe(new BigInteger(3));
        }

        [Test]
        public void EncodeInteger2DNegativePointOnHilbertCurveTest()
        {
            HilbertEncoder encoder = new HilbertEncoder(false, true);

            // correct for offset to search for zero point on the grid
            UInt32 correction = UInt32.MaxValue / 2;
            Coordinate zeroPointOnNegativeGrid = new Coordinate(-correction, -correction);
            BigInteger hilbertCode = encoder.Encode(zeroPointOnNegativeGrid);
            hilbertCode.ShouldBe(new BigInteger(0));
        }

        [Test]
        public void NegativeCoordinateShouldThrowAnExceptionOnPositiveHilbertGrid()
        {
            HilbertEncoder encoder = new HilbertEncoder(false, false);
            Should.Throw<ArgumentException>(() => encoder.Encode(new Coordinate(-1, 1)));
            Should.Throw<ArgumentException>(() => encoder.Encode(new Coordinate(1, -1)));
            Should.Throw<ArgumentException>(() => encoder.Encode(new Coordinate(-1, -1)));
        }

        [Test]
        public void EncodeInteger3DPositivePointOnHilbertCurveTest()
        {
            HilbertEncoder encoder = new HilbertEncoder(true, false);

            BigInteger hilbertCode = encoder.Encode(new Coordinate(0, 0, 0));
            hilbertCode.ShouldBe(new BigInteger(0));

            hilbertCode = encoder.Encode(new Coordinate(1, 0, 0));
            hilbertCode.ShouldBe(new BigInteger(1));

            hilbertCode = encoder.Encode(new Coordinate(0, 1, 0));
            hilbertCode.ShouldBe(new BigInteger(7));

            hilbertCode = encoder.Encode(new Coordinate(0, 0, 1));
            hilbertCode.ShouldBe(new BigInteger(3));

            hilbertCode = encoder.Encode(new Coordinate(5, 5, 5));
            hilbertCode.ShouldBe(new BigInteger(359));
        }
    }
}
