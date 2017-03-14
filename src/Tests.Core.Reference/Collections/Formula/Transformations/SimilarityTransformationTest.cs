// <copyright file="SimilarityTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Reference;
    using AEGIS.Reference.Collections.Formula;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="SimilarityTransformation" /> class.
    /// </summary>
    [TestFixture]
    public class SimilarityTransformationTest
    {
        /// <summary>
        /// The transformation.
        /// </summary>
        private SimilarityTransformation transformation;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget, Length.FromMetre(-129.549));
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget, Length.FromMetre(-208.185));
            parameters.Add(CoordinateOperationParameters.XAxisRotation, Angle.FromArcSecond(1.56504));
            parameters.Add(CoordinateOperationParameters.ScaleDifference, 1.0000015504);

            CoordinateReferenceSystem source = TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::23031"];
            CoordinateReferenceSystem target = TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::25831"];
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::3732"];

            this.transformation = new SimilarityTransformation("EPSG::5166", "ED50 / UTM zone 31N to ETRS89 / UTM zone 31N (1)", parameters, source, target, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void SimilarityTransformationForwardTest()
        {
            Coordinate coordinate = new Coordinate(300000, 4500000);
            Coordinate expected = new Coordinate(299905.060, 4499796.515);
            Coordinate transformed = this.transformation.Forward(coordinate);

            expected.X.ShouldBe(transformed.X, 1000);
            expected.Y.ShouldBe(transformed.Y, 1000);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void SimilarityTransformationReverseTest()
        {
            Coordinate expected = new Coordinate(300000, 4500000);
            Coordinate transformed = this.transformation.Reverse(this.transformation.Forward(expected));

            expected.X.ShouldBe(transformed.X, 1000);
            expected.Y.ShouldBe(transformed.Y, 1000);
        }
    }
}
