// <copyright file="ObliqueStereographicProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="MercatorAProjection" /> class.
    /// </summary>
    [TestFixture]
    public class ObliqueStereographicProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private ObliqueStereographicProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(52, 9, 22.178));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(5, 23, 15.5));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(155000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(463000));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9999079);

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7004", "Bessel 1841", 6377397.155, 299.1528128);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1275"];

            this.projection = new ObliqueStereographicProjection("EPSG::19914", "RD New", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void ObliqueStereographicProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(53), Angle.FromDegree(6));
            Coordinate expected = new Coordinate(196105.283, 557057.739);
            Coordinate transformed = this.projection.Forward(coordinate);

            expected.X.ShouldBe(transformed.X, 0.01);
            expected.Y.ShouldBe(transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void ObliqueStereographicProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(53), Angle.FromDegree(6));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.0001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.0001);
        }
    }
}
