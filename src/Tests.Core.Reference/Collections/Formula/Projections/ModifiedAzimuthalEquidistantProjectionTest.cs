// <copyright file="ModifiedAzimuthalEquidistantProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Reference;
    using AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="ModifiedAzimuthalEquidistantProjection" /> class.
    /// </summary>
    [TestFixture]
    public class ModifiedAzimuthalEquidistantProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private ModifiedAzimuthalEquidistantProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(9, 32, 48.15));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(138, 10, 07.48));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(40000.00));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(60000.00));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7008", "Clarke 1866", 6378206.4, 294.9786982);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::3108"];

            this.projection = new ModifiedAzimuthalEquidistantProjection("EPSG::9832", "Yap Islands", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void ModifiedAzimuthalEquidistantProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(9, 35, 47.493), Angle.FromDegree(138, 11, 34.908));
            Coordinate expected = new Coordinate(42665.90, 65509.82);
            Coordinate transformed = this.projection.Forward(coordinate);

            expected.X.ShouldBe(transformed.X, 0.01);
            expected.Y.ShouldBe(transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void ModifiedAzimuthalEquidistantProjectionProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(9, 35, 47.493), Angle.FromDegree(138, 11, 34.908));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            expected.Latitude.BaseValue.ShouldBe(transformed.Latitude.BaseValue, 0.0001);
            expected.Longitude.BaseValue.ShouldBe(transformed.Longitude.BaseValue, 0.0001);
        }
    }
}
