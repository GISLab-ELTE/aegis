// <copyright file="PolarStereographicAProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="PolarStereographicAProjection" /> class.
    /// </summary>
    [TestFixture]
    public class PolarStereographicAProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private PolarStereographicAProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(90));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.994);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(2000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(2000000));

            Ellipsoid ellipsoid = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1996"];

            this.projection = new PolarStereographicAProjection("EPSG::16061", "Universal Polar Stereographic North", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void PolarStereographicAProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(73), Angle.FromDegree(44));
            Coordinate expected = new Coordinate(3320416.75, 632668.43);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void PolarStereographicAProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(73), Angle.FromDegree(44));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            transformed.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformed.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }
    }
}
