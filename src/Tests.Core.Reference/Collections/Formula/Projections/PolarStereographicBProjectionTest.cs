// <copyright file="PolarStereographicBProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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
    using ELTE.AEGIS.Reference;
    using ELTE.AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="PolarStereographicBProjection" /> class.
    /// </summary>
    [TestFixture]
    public class PolarStereographicBProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private PolarStereographicBProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfStandardParallel, Angle.FromDegree(-71));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(70));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(6000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(6000000));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1278"];

            this.projection = new PolarStereographicBProjection("EPSG::19993", "Australian Antarctic Polar Stereographic", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the <see cref="Forward" /> method.
        /// </summary>
        [Test]
        public void PolarStereographicBProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-75), Angle.FromDegree(120));
            Coordinate expected = new Coordinate(7255380.79, 7053389.56);
            Coordinate transformed = this.projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void PolarStereographicBProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-75), Angle.FromDegree(120));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
