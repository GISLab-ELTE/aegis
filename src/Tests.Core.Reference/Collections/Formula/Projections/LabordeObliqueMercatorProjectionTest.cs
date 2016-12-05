// <copyright file="LabordeObliqueMercatorProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using ELTE.AEGIS;
    using ELTE.AEGIS.Reference;
    using ELTE.AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="LabordeObliqueMercatorProjection" /> class.
    /// </summary>
    [TestFixture]
    public class LabordeObliqueMercatorProjectionTest
    {
        /// <summary>
        /// The projection.
        /// </summary>
        private LabordeObliqueMercatorProjection projection;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromGrad(-21));
            parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromGrad(49));
            parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, Angle.FromGrad(21));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, 0.9995);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(400000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(800000));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7022", "International 1924", 6378388, 297);
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1149"];

            this.projection = new LabordeObliqueMercatorProjection("EPSG::19861", "Laborde Grid", parameters, ellipsoid, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void LabordeObliqueMercatorProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromGrad(-17.988666667), Angle.FromGrad(46.800381173));
            Coordinate expected = new Coordinate(188333.848, 1098841.091);
            Coordinate transformed = this.projection.Forward(coordinate);

            Assert.AreEqual(expected.X, transformed.X, 0.01);
            Assert.AreEqual(expected.Y, transformed.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void LabordeObliqueMercatorProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromGrad(-17.988666667), Angle.FromGrad(46.800381173));
            GeoCoordinate transformed = this.projection.Reverse(this.projection.Forward(expected));

            Assert.AreEqual(expected.Latitude.BaseValue, transformed.Latitude.BaseValue, 0.00000001);
            Assert.AreEqual(expected.Longitude.BaseValue, transformed.Longitude.BaseValue, 0.00000001);
        }
    }
}
