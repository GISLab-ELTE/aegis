// <copyright file="GnomonicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS;
    using AEGIS.Reference;
    using AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="GnomonicProjection" /> class.
    /// </summary>
    /// <author>Péter Rónai</author>
    [TestFixture]
    public class GnomonicProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private GnomonicProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(40));
            parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromDegree(-100));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);

            this.projection = new GnomonicProjection(String.Empty, String.Empty, parameters, ellipsoid, null);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void GnomonicProjectionForwardTest()
        {
            // expected values were generated with OSGeo Proj.4

            GeoCoordinate input = new GeoCoordinate(Angle.FromDegree(30), Angle.FromDegree(-110));
            Coordinate expected = new Coordinate(Length.FromMetre(-984035.612371).Value, Length.FromMetre(-1080927.60583).Value);
            Coordinate actual = this.projection.Forward(input);

            actual.X.ShouldBe(expected.X, 0.000001);
            actual.Y.ShouldBe(expected.Y, 0.000001);
        }

        /// <summary>
        /// Tests the <see cref="Reverse" /> method.
        /// </summary>
        [Test]
        public void GnomonicProjectionReverseTest()
        {
            // expected values were generated with OSGeo Proj.4

            Coordinate coordinate = new Coordinate(Length.FromMetre(600300).Value, Length.FromMetre(-295675).Value);
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(37.1540954053), Angle.FromDegree(-93.2553802038));
            GeoCoordinate actual = this.projection.Reverse(coordinate);

            actual.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.0000000001);
            actual.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.0000000001);

            // this is for testing a corner case of the reverse transformation:
            // when the point given is exactly on the projection centre, the reverse transformation is calculated differently
            coordinate = new Coordinate(Length.FromMetre(0).Value, Length.FromMetre(0).Value);
            expected = new GeoCoordinate(Angle.FromDegree(40), Angle.FromDegree(-100));
            actual = this.projection.Reverse(coordinate);

            actual.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.0000000001);
            actual.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.0000000001);
        }
    }
}
