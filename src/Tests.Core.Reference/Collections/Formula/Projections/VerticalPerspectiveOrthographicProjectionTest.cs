// <copyright file="VerticalPerspectiveOrthographicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Test fixture for the <see cref="VerticalPerspectiveOrthographicProjection" /> class.
    /// </summary>
    [TestFixture]
    public class VerticalPerspectiveOrthographicProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private VerticalPerspectiveOrthographicProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfTopocentricOrigin, Angle.FromDegree(55));
            parameters.Add(CoordinateOperationParameters.LongitudeOfTopocentricOrigin, Angle.FromDegree(5));
            parameters.Add(CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin, Length.FromMetre(200));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1262"];

            this.projection = new VerticalPerspectiveOrthographicProjection(String.Empty, String.Empty, parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void VerticalPerspectiveOrthographicProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53, 48, 33.82), Angle.FromDegree(2, 7, 46.38), Length.FromMetre(73));
            Coordinate expected = new Coordinate(-189013.869, -128642.04);
            Coordinate transformed = this.projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }
    }
}
