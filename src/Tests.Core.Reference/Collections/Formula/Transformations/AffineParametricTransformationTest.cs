// <copyright file="AffineParametricTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Reference.Collections.Local;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="AffineParametricTransformation" /> class.
    /// </summary>
    [TestFixture]
    public class AffineParametricTransformationTest
    {
        /// <summary>
        /// The transformation.
        /// </summary>
        private AffineParametricTransformation transformation;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.A0, 82357.457);
            parameters.Add(CoordinateOperationParameters.A1, 0.304794369);
            parameters.Add(CoordinateOperationParameters.A2, 0.000015417425);
            parameters.Add(CoordinateOperationParameters.B0, 28091.324);
            parameters.Add(CoordinateOperationParameters.B1, -0.000015417425);
            parameters.Add(CoordinateOperationParameters.B2, 0.304794369);

            CoordinateReferenceSystem source = TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::24100"];
            CoordinateReferenceSystem target = TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::24200"];
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::3342"];

            this.transformation = new AffineParametricTransformation("ESPG::10087", "Jamaica 1875 / Jamaica (Old Grid) to JAD69 / Jamaica National Grid (1)", parameters, source, target, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void AffineParametricTransformationForwardTest()
        {
            Coordinate coordinate = new Coordinate(553900, 482500);
            Coordinate expected = new Coordinate(251190.497, 175146.067);
            Coordinate transformed = this.transformation.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void AffineParametricTransformationReverseTest()
        {
            Coordinate expected = new Coordinate(553900, 482500);
            Coordinate transformed = this.transformation.Reverse(this.transformation.Forward(expected));

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
        }
    }
}
