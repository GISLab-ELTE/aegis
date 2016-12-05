// <copyright file="OrthographicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="OrthographicProjection" /> class.
    /// </summary>
    [TestFixture]
    public class OrthographicProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private OrthographicProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(55, 0, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(5, 0, 0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::4326", "WGS 84", 6378137.0, 298.2572236);
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1306"];

            this.projection = new OrthographicProjection("EPSG::4326", "WGS 84 / Orthographic", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void OrthographicProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38));
            Coordinate expected = new Coordinate(-189011.711, -128640.567);
            Coordinate transformed = this.projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void OrthographicProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38));
            GeoCoordinate transformer = this.projection.Reverse(this.projection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformer.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformer.Longitude.BaseValue, 0.00000001);
        }
    }
}
