// <copyright file="LambertConicConformal1SPWestOrientatedProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="LambertConicConformal1SPWestOrientatedProjection" /> class.
    /// </summary>
    [TestFixture]
    public class LambertConicConformal1SPWestOrientatedProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private LambertConicConformal1SPWestOrientatedProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(18, 00, 00));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(77, 00, 00));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(250000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(150000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 1.000000);

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7008", "Clarke 1866", 6378206.4, 294.9786982);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::3342"];

            this.projection = new LambertConicConformal1SPWestOrientatedProjection("EPSG::19910", "Jamaica National Grid", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        public void LambertConicConformal1SPWestOrientatedProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(17, 55, 55.80), Angle.FromDegree(76, 56, 37.26));
            Coordinate expected = new Coordinate(255966.58, 142493.51);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void LambertConicConformal1SPWestOrientatedProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(17, 55, 55.80), Angle.FromDegree(76, 56, 37.26));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }
    }
}
