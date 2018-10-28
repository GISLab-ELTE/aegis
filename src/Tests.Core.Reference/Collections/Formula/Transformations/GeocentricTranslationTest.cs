// <copyright file="GeocentricTranslationTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="GeocentricTranslation" /> class.
    /// </summary>
    [TestFixture]
    public class GeocentricTranslationTest
    {
        /// <summary>
        /// The transformation.
        /// </summary>
        private GeocentricTranslation transformation;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.XAxisTranslation, Length.FromMetre(84.87));
            parameters.Add(CoordinateOperationParameters.YAxisTranslation, Length.FromMetre(96.49));
            parameters.Add(CoordinateOperationParameters.ZAxisTranslation, Length.FromMetre(116.95));

            CoordinateReferenceSystem source = TestUtilities.ReferenceProvider.GeographicCoordinateReferenceSystems["EPSG::4326"];
            CoordinateReferenceSystem target = TestUtilities.ReferenceProvider.GeographicCoordinateReferenceSystems["EPSG::4230"];
            AreaOfUse areaOfUse = TestUtilities.ReferenceProvider.AreasOfUse["EPSG::1262"];

            this.transformation = new GeocentricTranslation(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, parameters, source, target, areaOfUse);
        }

        /// <summary>
        /// Tests the forward computation.
        /// </summary>
        [Test]
        public void GeocentricTranslationForwardTest()
         {
            Coordinate coordinate = new Coordinate(3771793.96, 140253.34, 5124304.35);
            Coordinate expected = new Coordinate(3771878.84, 140349.83, 5124421.3);
            Coordinate transformed = this.transformation.Forward(coordinate);

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
            transformed.Z.ShouldBe(expected.Z, 0.01);
        }

        /// <summary>
        /// Tests the reverse computation.
        /// </summary>
        [Test]
        public void GeocentricTranslationReverseTest()
        {
            Coordinate expected = new Coordinate(3771793.96, 140253.34, 5124304.35);
            Coordinate transformed = this.transformation.Reverse(this.transformation.Forward(expected));

            transformed.X.ShouldBe(expected.X, 0.01);
            transformed.Y.ShouldBe(expected.Y, 0.01);
            transformed.Z.ShouldBe(expected.Z, 0.01);
        }
    }
}
