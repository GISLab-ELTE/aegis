// <copyright file="KrovakModifiedProjectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="KrovakModifiedProjection" /> class.
    /// </summary>
    [TestFixture]
    public class KrovakModifiedProjectionTest
    {
        #region Private fields

        /// <summary>
        /// The projection.
        /// </summary>
        private KrovakModifiedProjection projection;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(49, 30, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfOrigin, Angle.FromDegree(24, 50, 0));
            parameters.Add(CoordinateOperationParameters.CoLatitudeOfConeAxis, Angle.FromDegree(30, 17, 17.3031));
            parameters.Add(CoordinateOperationParameters.LatitudeOfPseudoStandardParallel, Angle.FromDegree(78, 30, 0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel, 0.9999);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(5000000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(5000000));
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPoint, Length.FromMetre(1089000));
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPoint, Length.FromMetre(654000));
            parameters.Add(CoordinateOperationParameters.C1, 2.946529277 * Math.Pow(10, -2));
            parameters.Add(CoordinateOperationParameters.C2, 2.515965696 * Math.Pow(10, -2));
            parameters.Add(CoordinateOperationParameters.C3, 1.193845912 * Math.Pow(10, -7));
            parameters.Add(CoordinateOperationParameters.C4, -4.668270147 * Math.Pow(10, -7));
            parameters.Add(CoordinateOperationParameters.C5, 9.233980362 * Math.Pow(10, -12));
            parameters.Add(CoordinateOperationParameters.C6, 1.523735715 * Math.Pow(10, -12));
            parameters.Add(CoordinateOperationParameters.C7, 1.696780024 * Math.Pow(10, -18));
            parameters.Add(CoordinateOperationParameters.C8, 4.408314235 * Math.Pow(10, -18));
            parameters.Add(CoordinateOperationParameters.C9, -8.331083518 * Math.Pow(10, -24));
            parameters.Add(CoordinateOperationParameters.C10, -3.689471323 * Math.Pow(10, -24));

            Ellipsoid ellipsoid = Ellipsoid.FromInverseFlattening("EPSG::7004", "Bessel 1841", 6377397.155, 299.1528128);
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1079"];

            this.projection = new KrovakModifiedProjection("EPSG::5224", "S-JTSK/05 (Ferro) / Modified Krovak", parameters, ellipsoid, areaOfUse);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void KrovakModifiedProjectionForwardTest()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Angle.FromDegree(50, 12, 32.442).BaseValue, Angle.FromDegree(34, 30, 59.179).BaseValue - Angle.FromDegree(17.66666667).BaseValue);
            Coordinate expected = new Coordinate(Length.FromMetre(5568990.91).BaseValue, Length.FromMetre(6050538.71).BaseValue);
            Coordinate transformed = this.projection.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 1);
            transformed.Y.ShouldBe(expected.Y, 1);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void KrovakModifiedProjectionReverseTest()
        {
            GeoCoordinate expected = new GeoCoordinate(Angle.FromRadian(0.876312568), Angle.FromRadian(0.602425500));
            GeoCoordinate transformer = this.projection.Reverse(this.projection.Forward(expected));

            transformer.Latitude.BaseValue.ShouldBe(expected.Latitude.BaseValue, 0.00000001);
            transformer.Longitude.BaseValue.ShouldBe(expected.Longitude.BaseValue, 0.00000001);
        }

        #endregion
    }
}
