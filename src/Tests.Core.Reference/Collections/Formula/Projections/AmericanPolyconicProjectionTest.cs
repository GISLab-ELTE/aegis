// <copyright file="AmericanPolyconicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="AmericanPolyconicProjection" /> class.
    /// </summary>
    [TestFixture]
    public class AmericanPolyconicProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private AmericanPolyconicProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-54));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(5000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(10000000));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7019", "GRS 1980", 6378137.0, 298.257222101);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1053"];

            this.projection = new AmericanPolyconicProjection("EPSG::19941", "Brazil Polyconic", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void AmericanPolyconicProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-24), Angle.FromDegree(-37));
            Coordinate expected = new Coordinate(6725584.49172815, 7240461.99578364);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void AmericanPolyconicProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-24), Angle.FromDegree(-37));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.01);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.01);
        }
    }
}
