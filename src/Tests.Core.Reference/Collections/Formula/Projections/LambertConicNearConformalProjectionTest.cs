// <copyright file="LambertConicNearConformalProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Reference;
    using AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="LambertConicNearConformalProjection" /> class.
    /// </summary>
    [TestFixture]
    public class LambertConicNearConformalProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private LambertConicNearConformalProjection projectionLambertConicNearConformal;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(34, 39, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(37, 21, 00));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(300000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(300000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.99962560);

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7011", "Clarke 1880 (IGN)", 6378249.2, 6356515);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1623"];

            this.projectionLambertConicNearConformal = new LambertConicNearConformalProjection("EPSG::19940", "Levant Zone", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        public void LambertConicNearConformalProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(37, 31, 17.625), Angle.FromDegree(34, 08, 11.291));
            Coordinate expected = new Coordinate(15707.96, 623165.96);
            Coordinate transformed = this.projectionLambertConicNearConformal.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void LambertConicNearConformalProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(37, 31, 17.625), Angle.FromDegree(34, 08, 11.291));
            GeoCoordinate transformed = this.projectionLambertConicNearConformal.Reverse(this.projectionLambertConicNearConformal.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }
    }
}
