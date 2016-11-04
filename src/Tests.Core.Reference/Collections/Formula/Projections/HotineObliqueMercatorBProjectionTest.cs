// <copyright file="HotineObliqueMercatorBProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="HotineObliqueMercatorBProjection" /> class.
    /// </summary>
    [TestFixture]
    public class HotineObliqueMercatorBProjectionTest
    {
        #region Private fields

        /// <summary>
        /// The projection.
        /// </summary>
        private HotineObliqueMercatorBProjection projection;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(4));
            parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromDegree(115));
            parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, Angle.FromDegree(53, 18, 56.9537));
            parameters.Add(CoordinateOperationParameters.AngleFromRectifiedToSkewGrid, Angle.FromDegree(53, 7, 48.3685));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, 0.99984);
            parameters.Add(CoordinateOperationParameters.EastingAtProjectionCentre, Length.FromMetre(590476.87));
            parameters.Add(CoordinateOperationParameters.NorthingAtProjectionCentre, Length.FromMetre(442857.65));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7016", "Everest 1830 (1967 Definition)", 6377298.556, 300.8017);
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1362"];

            this.projection = new HotineObliqueMercatorBProjection("EPSG::19958", "Rectified Skew Orthomorphic Borneo Grid (metres)", parameters, ellipsoid, areaOfUse);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void HotineObliqueMercatorBProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(5, 23, 14.1129), Angle.FromDegree(115, 48, 19.8196));
            Coordinate expected = new Coordinate(679245.73, 596562.78);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void HotineObliqueMercatorBProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(5, 23, 14.1129), Angle.FromDegree(115, 48, 19.8196));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }

        #endregion
    }
}
