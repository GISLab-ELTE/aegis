// <copyright file="GeographicToTopocentricConversionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS;
    using ELTE.AEGIS.Reference;
    using ELTE.AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="GeographicToTopocentricConversion" /> class.
    /// </summary>
    [TestFixture]
    public class GeographicToTopocentricConversionTest
    {
        /// <summary>
        /// The conversion.
        /// </summary>
        private GeographicToTopocentricConversion conversion;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfTopocentricOrigin, Angle.FromDegree(55));
            parameters.Add(CoordinateOperationParameters.LongitudeOfTopocentricOrigin, Angle.FromDegree(5));
            parameters.Add(CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin, Length.FromMetre(200));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);

            this.conversion = new GeographicToTopocentricConversion(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, parameters, ellipsoid);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void GeographicToTopocentricConversionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            Coordinate expected = new Coordinate(-189013.869, -128642.04, -4220.171);
            Coordinate transformed = this.conversion.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.001);
            Assert.AreEqual(expected.Y, transformed.Y, 0.001);
            Assert.AreEqual(expected.Z, transformed.Z, 0.001);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void GeographicToTopocentricConversionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            GeoCoordinate transformed = this.conversion.Reverse(this.conversion.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Height.BaseValue, transformed.Height.BaseValue, 0.0001);
        }
    }
}
