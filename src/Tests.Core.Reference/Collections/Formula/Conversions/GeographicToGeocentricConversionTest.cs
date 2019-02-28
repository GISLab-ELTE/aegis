// <copyright file="GeographicToGeocentricConversionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Reference.Collections.Formula
{
    using System;
    using AEGIS;
    using AEGIS.Reference;
    using AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="GeographicToGeocentricConversion" /> class.
    /// </summary>
    [TestFixture]
    public class GeographicToGeocentricConversionTest
    {
        /// <summary>
        /// The conversion.
        /// </summary>
        private GeographicToGeocentricConversion conversion;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);

            this.conversion = new GeographicToGeocentricConversion(String.Empty, String.Empty, ellipsoid);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void GeographicToGeocentricConversionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            Coordinate expected = new Coordinate(3771793.968, 140253.342, 5124304.349);
            Coordinate transformed = this.conversion.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.001);
            transformed.Y.ShouldBe(expected.Y, 0.001);
            transformed.Z.ShouldBe(expected.Z, 0.001);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void GeographicToGeocentricConversionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            GeoCoordinate transformed = this.conversion.Reverse(this.conversion.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.0001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.0001);
            transformed.Height.BaseValue.ShouldBe(expected.Height.BaseValue, 0.0001);
        }
    }
}
