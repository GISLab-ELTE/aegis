// <copyright file="TransverseMercatorSouthProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="MercatorAProjection" /> class.
    /// </summary>
    [TestFixture]
    public class TransverseMercatorSouthProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private TransverseMercatorSouthProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(29));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 1);

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::4326", "WGS 84", 6378137, 6356752.31424);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1264"];

            this.projection = new TransverseMercatorSouthProjection("EPSG::4148", "Hartebeesthoek94", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the <see cref="Forward" /> method.
        /// </summary>
        [Test]
        public void TransverseMercatorSouthProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-25, 43, 55.302), Angle.FromDegree(28, 16, 57.479));
            Coordinate expected = new Coordinate(71984.49, 2847342.74);
            Coordinate transformed = this.projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.02);
            Assert.AreEqual(expected.Y, transformed.Y, 0.02);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void TransverseMercatorSouthProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-25, 43, 55.302), Angle.FromDegree(28, 16, 57.479));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.0001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.0001);
        }
    }
}
