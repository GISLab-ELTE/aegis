// <copyright file="WorldMillerCylindricalProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS;
    using AEGIS.Reference;
    using AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="WorldMillerCylindricalProjection" /> class.
    /// </summary>
    [TestFixture]
    public class WorldMillerCylindricalProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private WorldMillerCylindricalProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-90));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);

            this.projection = new WorldMillerCylindricalProjection(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, parameters, ellipsoid, AreaOfUse.Undefined);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void WorldMillerCylindricalProjectionForwardTest()
        {
            // expected values were generated with OSGeo Proj.4

            GeoCoordinate input = new GeoCoordinate(Angle.FromDegree(30), Angle.FromDegree(-110));
            Coordinate expected = new Coordinate(Length.FromMetre(-2226389.81587).Value, Length.FromMetre(3441760.13671).Value);
            Coordinate actual = this.projection.Forward(input);

            actual.X.ShouldBe(expected.X, 0.00001);
            actual.Y.ShouldBe(expected.Y, 0.00001);
        }

        /// <summary>
        /// Tests the <see cref="Reverse" /> method.
        /// </summary>
        [Test]
        public void WorldMillerCylindricalProjectionReverseTest()
        {
            // expected values were generated with OSGeo Proj.4

            Coordinate input = new Coordinate(Length.FromMetre(600300).Value, Length.FromMetre(-295675).Value);
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-2.65548507092), Angle.FromDegree(-84.6074133494));
            GeoCoordinate actual = this.projection.Reverse(input);

            actual.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000000001);
            actual.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000000001);
        }
    }
}
