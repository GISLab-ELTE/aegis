// <copyright file="CassiniSoldnerProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="CassiniSoldnerProjection" /> class.
    /// </summary>
    [TestFixture]
    public class CassiniSoldnerProjectionTest
    {
        #region Private fields

        /// <summary>
        /// The projection.
        /// </summary>
        private CassiniSoldnerProjection projection;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(10, 26, 30));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-61, 20, 0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromClarkesLink(430000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromClarkesLink(325000));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7007", "Clarke 1858", Length.FromClarkesFoot(20926348), Length.FromClarkesFoot(20855233)).ToUnit(UnitsOfMeasurement.ClarkesLink);
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1339"];

            this.projection = new CassiniSoldnerProjection("EPSG::19925", "Trinidad Grid", parameters, ellipsoid, areaOfUse);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void CassiniSoldnerProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(10), Angle.FromDegree(-62));
            Coordinate expected = new Coordinate(Length.FromClarkesLink(66644.94).Value, Length.FromClarkesLink(82536.22).Value);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void CassiniSoldnerProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(10), Angle.FromDegree(-62));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }

        #endregion
    }
}
