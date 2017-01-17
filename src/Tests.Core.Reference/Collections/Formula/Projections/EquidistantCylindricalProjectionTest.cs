// <copyright file="EquidistantCylindricalProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="EquidistantCylindricalProjection" /> class.
    /// </summary>
    [TestFixture]
    public class EquidistantCylindricalProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private EquidistantCylindricalProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1262"];

            this.projection = new EquidistantCylindricalProjection("EPSG::4085", "World Equidistant Cylindrical", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void EquidistantCylindricalProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(55), Angle.FromDegree(10));
            Coordinate expected = new Coordinate(Length.FromMetre(1113194.91).BaseValue, Length.FromMetre(6097230.31).BaseValue);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void EquidistantCylindricalProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(10), Angle.FromDegree(-62));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }
    }
}
