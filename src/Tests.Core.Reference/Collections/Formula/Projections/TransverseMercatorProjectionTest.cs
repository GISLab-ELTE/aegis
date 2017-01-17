// <copyright file="TransverseMercatorProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="MercatorAProjection" /> class.
    /// </summary>
    [TestFixture]
    public class TransverseMercatorProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private TransverseMercatorProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(49));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-2));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(400000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(-100000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996012717);

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7001", "Airy 1830", 6377563.396, 6356256.910);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1264"];

            this.projection = new TransverseMercatorProjection("EPSG::19916", "British National Grid", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void TransverseMercatorProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 30), Angle.FromDegree(0, 30));
            Coordinate expected = new Coordinate(577274.99, 69740.5);
            Coordinate transformed = this.projection.Forward(coordinate);

            expected.X.ShouldBe(transformed.X, 0.01);
            expected.Y.ShouldBe(transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void TransverseMercatorProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(50, 30), Angle.FromDegree(0, 30));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            expected.Latitude.BaseValue.ShouldBe(transformed.Latitude.BaseValue, 0.0001);
            expected.Longitude.BaseValue.ShouldBe(transformed.Longitude.BaseValue, 0.0001);
        }
    }
}
