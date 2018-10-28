// <copyright file="P6LeftHandedSeismicBinGridTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="P6LeftHandedSeismicBinGridTransformation" /> class.
    /// </summary>
    [TestFixture]
    public class P6LeftHandedSeismicBinGridTransformationTest
    {
        /// <summary>
        /// The transformation.
        /// </summary>
        private P6LeftHandedSeismicBinGridTransformation transformation;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.BinGridOriginI, 5000);
            parameters.Add(CoordinateOperationParameters.BinGridOriginJ, 0);
            parameters.Add(CoordinateOperationParameters.BinGridOriginEasting, Length.FromUSSurveyFoot(871200));
            parameters.Add(CoordinateOperationParameters.BinGridOriginNorthing, Length.FromUSSurveyFoot(10280160));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOfBinGrid, 1);
            parameters.Add(CoordinateOperationParameters.MapGridBearingOfBinGridJAxis, Angle.FromDegree(340));
            parameters.Add(CoordinateOperationParameters.BinWidthOnIAxis, Length.FromUSSurveyFoot(82.5));
            parameters.Add(CoordinateOperationParameters.BinWidthOnJAxis, Length.FromUSSurveyFoot(41.25));
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnIAxis, 1);
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnJAxis, 1);

            CoordinateReferenceSystem source = TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::32066"];
            CoordinateReferenceSystem target = TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::32066"];
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::3641"];

            this.transformation = new P6LeftHandedSeismicBinGridTransformation("EPSG::6919", "[enter here name of (I = J-90°) bin grid] ", parameters, source, target, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void P6LeftHandedSeismicBinGridTransformationForwardTest()
        {
            Coordinate coordinate = new Coordinate(4700, 247);
            Coordinate expected = new Coordinate(890972.63, 10298199.29);
            Coordinate transformed = this.transformation.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void P6LeftHandedSeismicBinGridTransformationReverseTest()
        {
            Coordinate expected = new Coordinate(4700, 247);
            Coordinate transformed = this.transformation.Reverse(this.transformation.Forward(expected));

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }
    }
}
