// <copyright file="FeatureTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Features
{
    using System;
    using AEGIS.Features;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Feature" /> class.
    /// </summary>
    [TestFixture]
    public class FeatureTest
    {
        /// <summary>
        /// The feature identifier.
        /// </summary>
        private String identifier;

        /// <summary>
        /// The mocked attribute collection.
        /// </summary>
        private IAttributeCollection mockAttributes;

        /// <summary>
        /// The mocked geometry.
        /// </summary>
        private IGeometry mockGeometry;

        /// <summary>
        /// Tests the constructor of the <see cref="Feature" /> class.
        /// </summary>
        [Test]
        public void FeatureConstructorTest()
        {
            this.identifier = Guid.NewGuid().ToString();
            this.mockAttributes = new Mock<IAttributeCollection>().Object;
            this.mockGeometry = new Mock<IGeometry>().Object;

            // without geometry and attributes
            Feature feature = new Feature(this.identifier, null, null);
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBeNull();
            feature.Attributes.ShouldBeEmpty();

            // with attributes
            feature = new Feature(this.identifier, null, this.mockAttributes);
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBeNull();
            feature.Attributes.ShouldBe(this.mockAttributes);

            // with geometry
            feature = new Feature(this.identifier, this.mockGeometry, null);
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBe(this.mockGeometry);
            feature.Attributes.ShouldBeEmpty();

            // with geometry and attributes
            feature = new Feature(this.identifier, this.mockGeometry, this.mockAttributes);
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBe(this.mockGeometry);
            feature.Attributes.ShouldBe(this.mockAttributes);
        }
    }
}
