// <copyright file="GuamProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="GuamProjection" /> class.
    /// </summary>
    [TestFixture]
    public class GuamProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private GuamProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(13, 28, 20.87887));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(144, 44, 55.50254));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(50000.00));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(50000.00));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7008", "Clarke 1866", 6378206.4, 294.9786982);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::3255"];

            this.projection = new GuamProjection("EPSG::15400", "Guam SPCS", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void GuamProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(13, 20, 20.53846), Angle.FromDegree(144, 38, 07.19265));
            Coordinate expected = new Coordinate(37712.48, 35242.00);
            Coordinate transformed = this.projection.Forward(coordinate);

            expected.X.ShouldBe(transformed.X, 0.01);
            expected.Y.ShouldBe(transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void GuamProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(13, 20, 20.53846), Angle.FromDegree(144, 38, 07.19265));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.001);
        }
    }
}
