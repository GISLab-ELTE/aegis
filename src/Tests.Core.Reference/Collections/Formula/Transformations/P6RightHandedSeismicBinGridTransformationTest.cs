// <copyright file="P6RightHandedSeismicBinGridTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="P6RightHandedSeismicBinGridTransformation" /> class.
    /// </summary>
    [TestFixture]
    public class P6RightHandedSeismicBinGridTransformationTest
    {
        /// <summary>
        /// The transformation.
        /// </summary>
        private P6RightHandedSeismicBinGridTransformation transformation;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.BinGridOriginI, 1);
            parameters.Add(CoordinateOperationParameters.BinGridOriginJ, 1);
            parameters.Add(CoordinateOperationParameters.BinGridOriginEasting, Length.FromMetre(456781));
            parameters.Add(CoordinateOperationParameters.BinGridOriginNorthing, Length.FromMetre(5836723));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOfBinGrid, 0.99984);
            parameters.Add(CoordinateOperationParameters.BinWidthOnIAxis, Length.FromMetre(25));
            parameters.Add(CoordinateOperationParameters.BinWidthOnJAxis, Length.FromMetre(12.5));
            parameters.Add(CoordinateOperationParameters.MapGridBearingOfBinGridJAxis, Angle.FromDegree(20));
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnIAxis, 1);
            parameters.Add(CoordinateOperationParameters.BinNodeIncrementOnJAxis, 1);

            CoordinateReferenceSystem source = TestUtilities.ReferenceCollection.ProjectedCoordinateReferenceSystems["EPSG::32631"];
            CoordinateReferenceSystem target = TestUtilities.ReferenceCollection.ProjectedCoordinateReferenceSystems["EPSG::32631"];
            AreaOfUse areaOfUse = TestUtilities.ReferenceCollection.AreasOfUse["EPSG::1933"];

            this.transformation = new P6RightHandedSeismicBinGridTransformation("EPSG::6918", "[enter here name of (I = J+90°) bin grid]", parameters, source, target, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void P6RightHandedSeismicBinGridTransformationForwardTest()
        {
            Coordinate coordinate = new Coordinate(300, 247);
            Coordinate expected = new Coordinate(464855.62, 5837055.90);
            Coordinate transformed = this.transformation.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void P6RightHandedSeismicBinGridTransformationReverseTest()
        {
            Coordinate expected = new Coordinate(300, 247);
            Coordinate transformed = this.transformation.Reverse(this.transformation.Forward(expected));

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }
    }
}
