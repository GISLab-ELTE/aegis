// <copyright file="KrovakProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="KrovakProjection" /> class.
    /// </summary>
    [TestFixture]
    public class KrovakProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private KrovakProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(49, 30, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(24, 50, 0));
            parameters.Add(CoordinateOperationParameters.CoLatitudeOfConeAxis, Angle.FromDegree(30, 17, 17.303));
            parameters.Add(CoordinateOperationParameters.LatitudeOfPseudoStandardParallel, Angle.FromDegree(78, 30, 0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel, 0.9999);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(0));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(0));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7004", "Bessel 1841", 6377397.155, 299.1528128);
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1306"];

            this.projection = new KrovakProjection("EPSG::2065", "S-JTSK (Ferro) / Krovak", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void KrovakProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442), Angle.FromDegree(16, 50, 59.179));
            Coordinate expected = new Coordinate(568991, 1050538.63);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void KrovakProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442), Angle.FromDegree(16, 50, 59.179));
            GeoCoordinate transformer = this.projection.Reverse(this.projection.Forward(expected));

            transformer.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformer.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }
    }
}
