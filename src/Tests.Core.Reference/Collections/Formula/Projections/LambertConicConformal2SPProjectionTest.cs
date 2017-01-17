// <copyright file="LambertConicConformal2SPProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="LambertConicConformal2SPProjection" /> class.
    /// </summary>
    [TestFixture]
    public class LambertConicConformal2SPProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private LambertConicConformal2SPProjection projection2SP;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(27, 50, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(-99, 00, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(28, 23, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(30, 17, 00));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(2000000.00), UnitsOfMeasurement.Metre));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(0.00), UnitsOfMeasurement.Metre));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7008", "Clarke 1866", 6378206.4, 294.9786982);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::2256"];

            this.projection2SP = new LambertConicConformal2SPProjection("EPSG::14204", " Texas CS27 South Central zone", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void LambertConicConformal2SPProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(28, 30, 00.00), Angle.FromDegree(-96, 00, 00.00));
            Coordinate expected = new Coordinate(Length.FromUSSurveyFoot(2963503.91).BaseValue, Length.FromUSSurveyFoot(254759.80).BaseValue);
            Coordinate transformed = this.projection2SP.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void LambertConicConformal2SPProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(28, 30, 00.00), Angle.FromDegree(-96, 00, 00.00));
            GeoCoordinate transformed = this.projection2SP.Reverse(this.projection2SP.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }
    }
}
