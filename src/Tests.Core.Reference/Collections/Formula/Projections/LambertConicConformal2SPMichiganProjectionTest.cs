// <copyright file="LambertConicConformal2SPMichiganProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="LambertConicConformal2SPMichiganProjection" /> class.
    /// </summary>
    [TestFixture]
    public class LambertConicConformal2SPMichiganProjectionTest
    {
        #region Private fields

        /// <summary>
        /// The projection.
        /// </summary>
        private LambertConicConformal2SPMichiganProjection projection2SPMichigan;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(43, 19, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(-84, 20, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(44, 11, 00));
            parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(45, 42, 00));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(2000000.00), UnitsOfMeasurement.Metre));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(0.00), UnitsOfMeasurement.Metre));
            parameters.Add(CoordinateOperationParameters.EllipsoidScalingFactor, 1.0000382);

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7008", "Clarke 1866", 6378206.4, 294.9786982);
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::2256"];

            this.projection2SPMichigan = new LambertConicConformal2SPMichiganProjection("EPSG::4267", "NAD 27 Michigan Central", parameters, ellipsoid, areaOfUse);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void LambertConicConformal2SPMichiganProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(43, 45), Angle.FromDegree(-83, 10));
            Coordinate expected = new Coordinate(Length.FromUSSurveyFoot(2308335.75).BaseValue, Length.FromUSSurveyFoot(160210.48).BaseValue);
            Coordinate transformed = this.projection2SPMichigan.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void LambertConicConformal2SPMichiganProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(43, 45), Angle.FromDegree(-83, 10));
            GeoCoordinate transformed = this.projection2SPMichigan.Reverse(this.projection2SPMichigan.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }

        #endregion
    }
}
