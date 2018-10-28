// <copyright file="ColombiaUrbanProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using AEGIS.Reference;
    using AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="ColombiaUrbanProjection" /> class.
    /// </summary>
    [TestFixture]
    public class ColombiaUrbanProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private ColombiaUrbanProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(4, 40, 49.75));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-74, 8, 47.73));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(92334.879));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(109320.965));
            parameters.Add(CoordinateOperationParameters.ProjectionPlaneOriginHeight, 2550.000);

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::4019", "GRS 1980", 6378137.0, 298.2572221);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1264"];

            this.projection = new ColombiaUrbanProjection("EPSG::3116", "MAGNA-SIRGAS / Bogota urban grid", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the <see cref="Forward" /> method.
        /// </summary>
        [Test]
        public void ColombiaUrbanProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(4, 48), Angle.FromDegree(-74, 15));
            Coordinate expected = new Coordinate(80859.033, 122543.174);
            Coordinate transformed = this.projection.Forward(coordinate);

            Assert.AreEqual(transformed.X, expected.X, 1);
            Assert.AreEqual(transformed.Y, expected.Y, 1);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void ColombiaUrbanProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(4, 48), Angle.FromDegree(-74, 15));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            Assert.AreEqual(transformed.Latitude.BaseValue, expected.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(transformed.Longitude.BaseValue, expected.Longitude.BaseValue, 0.0001);
        }
    }
}
