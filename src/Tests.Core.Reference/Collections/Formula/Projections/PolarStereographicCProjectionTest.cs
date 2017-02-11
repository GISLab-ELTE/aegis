// <copyright file="PolarStereographicCProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Reference;
    using ELTE.AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="PolarStereographicCProjection" /> class.
    /// </summary>
    [TestFixture]
    public class PolarStereographicCProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private PolarStereographicCProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfStandardParallel, Angle.FromDegree(-67));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(140));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.FromMetre(300000));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.FromMetre(200000));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7022", "International 1924", 6378388, 297);
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::2818"];

            this.projection = new PolarStereographicCProjection("EPSG::19983", "Terre Adelie Polar Stereographic ", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the <see cref="Forward" /> method.
        /// </summary>
        [Test]
        public void PolarStereographicCProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(-66, 36, 18.820), Angle.FromDegree(140, 04, 17.04));
            Coordinate expected = new Coordinate(303169.52, 244055.72);
            Coordinate transformed = this.projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void PolarStereographicCProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromDegree(-66, 36, 18.820), Angle.FromDegree(140, 04, 17.04));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
