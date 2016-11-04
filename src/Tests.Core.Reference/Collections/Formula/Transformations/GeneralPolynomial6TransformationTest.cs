// <copyright file="GeneralPolynomial6TransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="GeneralPolynomial6Transformation" /> class.
    /// </summary>
    [TestFixture]
    public class GeneralPolynomial6TransformationTest
    {
        #region Private fields

        /// <summary>
        /// The transformation.
        /// </summary>
        private GeneralPolynomial6Transformation transformation;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource, Angle.FromDegree(53.5));
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource, Angle.FromDegree(-7.7));
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget, Angle.FromDegree(53.5));
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget, Angle.FromDegree(-7.7));
            parameters.Add(CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences, 0.1);
            parameters.Add(CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences, 3600);
            parameters.Add(CoordinateOperationParameters.A0, 0.763);
            parameters.Add(CoordinateOperationParameters.Au1v0, -4.487);
            parameters.Add(CoordinateOperationParameters.Au0v1, 0.123);
            parameters.Add(CoordinateOperationParameters.Au2v0, 0.215);
            parameters.Add(CoordinateOperationParameters.Au1v1, -0.515);
            parameters.Add(CoordinateOperationParameters.Au0v2, 0.183);
            parameters.Add(CoordinateOperationParameters.Au3v0, -0.265);
            parameters.Add(CoordinateOperationParameters.Au2v1, -0.57);
            parameters.Add(CoordinateOperationParameters.Au1v2, 0.414);
            parameters.Add(CoordinateOperationParameters.Au0v3, 2.852);
            parameters.Add(CoordinateOperationParameters.Au3v1, 2.852);
            parameters.Add(CoordinateOperationParameters.Au2v2, 5.703);
            parameters.Add(CoordinateOperationParameters.Au1v3, 13.11);
            parameters.Add(CoordinateOperationParameters.Au2v3, 113.743);
            parameters.Add(CoordinateOperationParameters.Au3v3, -265.898);
            parameters.Add(CoordinateOperationParameters.B0, -2.81);
            parameters.Add(CoordinateOperationParameters.Bu1v0, -0.341);
            parameters.Add(CoordinateOperationParameters.Bu0v1, -4.68);
            parameters.Add(CoordinateOperationParameters.Bu2v0, 1.196);
            parameters.Add(CoordinateOperationParameters.Bu1v1, -0.119);
            parameters.Add(CoordinateOperationParameters.Bu0v2, 0.17);
            parameters.Add(CoordinateOperationParameters.Bu3v0, -0.887);
            parameters.Add(CoordinateOperationParameters.Bu2v1, 4.877);
            parameters.Add(CoordinateOperationParameters.Bu1v2, 3.913);
            parameters.Add(CoordinateOperationParameters.Bu0v3, 2.163);
            parameters.Add(CoordinateOperationParameters.Bu3v1, -46.666);
            parameters.Add(CoordinateOperationParameters.Bu2v2, -27.795);
            parameters.Add(CoordinateOperationParameters.Bu1v3, 18.867);
            parameters.Add(CoordinateOperationParameters.Bu3v2, -95.377);
            parameters.Add(CoordinateOperationParameters.Bu2v3, -284.294);
            parameters.Add(CoordinateOperationParameters.Bu3v3, -853.95);

            CoordinateReferenceSystem source = TestUtilities.ReferenceCollection.GeographicCoordinateReferenceSystems["EPSG::4300"];
            CoordinateReferenceSystem target = TestUtilities.ReferenceCollection.GeographicCoordinateReferenceSystems["EPSG::4258"];
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1305"];

            this.transformation = new GeneralPolynomial6Transformation("ESPG::1041", "TM75 to ETRS89 (1)", parameters, source, target, areaOfUse);
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void GeneralPolynomial6TransformationForwardTest()
        {
            Coordinate coordinate = new Coordinate(55, -6.5);
            Coordinate expected = new Coordinate(55.00002972, -6.50094913);
            Coordinate transformed = this.transformation.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.00001);
            transformed.Y.ShouldBe(expected.Y, 0.00001);
        }

        #endregion
    }
}
