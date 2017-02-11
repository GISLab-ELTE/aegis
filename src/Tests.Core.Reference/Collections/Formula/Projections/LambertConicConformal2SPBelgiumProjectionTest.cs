// <copyright file="LambertConicConformal2SPBelgiumProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Reference;
    using ELTE.AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="LambertConicConformal2SPBelgiumProjection" /> class.
    /// </summary>
    [TestFixture]
    public class LambertConicConformal2SPBelgiumProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private LambertConicConformal2SPBelgiumProjection projection2SPBelgium;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(90, 00, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(4, 21, 24.983));
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(49, 50, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(51, 10, 00));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(150000.01));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(5400088.44));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7022", "International 1924", 6378388, 297);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1347"];

            this.projection2SPBelgium = new LambertConicConformal2SPBelgiumProjection("EPSG::19902 ", "Belge Lambert 72", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        public void LambertConicConformal2SPBelgiumProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 40, 46.461), Angle.FromDegree(5, 48, 26.533));
            Coordinate expected = new Coordinate(251763.20, 153034.13);
            Coordinate transformed = this.projection2SPBelgium.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void LambertConicConformal2SPBelgiumProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(50, 40, 46.461), Angle.FromDegree(5, 48, 26.533));
            GeoCoordinate transformed = this.projection2SPBelgium.Reverse(this.projection2SPBelgium.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }
    }
}
