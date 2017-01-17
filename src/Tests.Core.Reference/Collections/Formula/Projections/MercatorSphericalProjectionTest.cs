// <copyright file="MercatorSphericalProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="MercatorSphericalProjection" /> class.
    /// </summary>
    [TestFixture]
    public class MercatorSphericalProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private MercatorSphericalProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            Ellipsoid ellipsoid = Ellipsoid.FromSphere(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, 6371007);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1262"];

            this.projection = new MercatorSphericalProjection(IdentifiedObject.UserDefinedIdentifier, "World Spherical Mercator", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void MercatorSphericalProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(24, 22, 54.433), Angle.FromDegree(-100, 20, 0));
            Coordinate expected = new Coordinate(-11156569.9, 2796869.94);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 100);
            transformed.Y.ShouldBe(expected.Y, 100);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void MercatorSphericalProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(24, 22, 54.433), Angle.FromDegree(-100, 20, 0));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.0001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.0001);
        }
    }
}
