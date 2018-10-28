// <copyright file="FeatureFactoryTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS;
    using AEGIS.Features;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="FeatureFactory" /> class.
    /// </summary>
    [TestFixture]
    public class FeatureFactoryTest
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
        /// The array of mocked features.
        /// </summary>
        private IFeature[] mockFeatures;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.identifier = Guid.NewGuid().ToString();
            this.mockAttributes = new Mock<IAttributeCollection>().Object;
            this.mockGeometry = new Mock<IGeometry>().Object;

            Mock<IFeature> mockFeature = new Mock<IFeature>();
            mockFeature.Setup(feature => feature.Identifier).Returns(this.identifier);
            mockFeature.Setup(feature => feature.Geometry).Returns(this.mockGeometry);
            mockFeature.Setup(feature => feature.Attributes).Returns(this.mockAttributes);

            this.mockFeatures = new IFeature[] { mockFeature.Object };
        }

        /// <summary>
        /// Tests creating a feature.
        /// </summary>
        [Test]
        public void FeatureFactoryCreateFeatureTest()
        {
            FeatureFactory factory = new FeatureFactory();

            // without geometry and attributes
            IFeature feature = factory.CreateFeature(this.identifier);
            feature.ShouldBeOfType<Feature>();
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBeNull();
            feature.Attributes.ShouldBeEmpty();

            // with attributes
            feature = factory.CreateFeature(this.identifier, this.mockAttributes);
            feature.ShouldBeOfType<Feature>();
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBeNull();
            feature.Attributes.ShouldBe(this.mockAttributes);

            // with geometry
            feature = factory.CreateFeature(this.identifier, this.mockGeometry);
            feature.ShouldBeOfType<Feature>();
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBe(this.mockGeometry);
            feature.Attributes.ShouldBeEmpty();

            // with geometry and attributes
            feature = factory.CreateFeature(this.identifier, this.mockGeometry, this.mockAttributes);
            feature.ShouldBeOfType<Feature>();
            feature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBe(this.mockGeometry);
            feature.Attributes.ShouldBe(this.mockAttributes);

            // from another feature
            IFeature otherFeature = factory.CreateFeature(feature);
            otherFeature.ShouldBeOfType<Feature>();
            otherFeature.Identifier.ShouldBe(this.identifier);
            feature.Geometry.ShouldBe(this.mockGeometry);
            feature.Attributes.ShouldBe(this.mockAttributes);

            Should.Throw<ArgumentNullException>(() => factory.CreateFeature((String)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature((IFeature)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature(null, this.mockAttributes));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature(String.Empty, (IAttributeCollection)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature(null, this.mockGeometry));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature(String.Empty, (IGeometry)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature(null, this.mockGeometry, this.mockAttributes));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature(String.Empty, this.mockGeometry, null));
            Should.Throw<ArgumentNullException>(() => factory.CreateFeature(String.Empty, null, this.mockAttributes));
        }

        /// <summary>
        /// Tests creating a feature collection.
        /// </summary>
        [Test]
        public void FeatureFactoryCreateFeatureCollectionTest()
        {
            FeatureFactory factory = new FeatureFactory();

            // without features and attributes
            IFeatureCollection collection = factory.CreateCollection(this.identifier);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(0);
            collection.Identifiers.ShouldBeEmpty();
            collection.IsReadOnly.ShouldBeFalse();
            collection.Geometry.ShouldBeNull();
            collection.Attributes.ShouldBeEmpty();

            // with attributes
            collection = factory.CreateCollection(this.identifier, this.mockAttributes);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(0);
            collection.Identifiers.ShouldBeEmpty();
            collection.IsReadOnly.ShouldBeFalse();
            collection.Geometry.ShouldBeNull();
            collection.Attributes.ShouldBe(this.mockAttributes);

            // with features
            collection = factory.CreateCollection(this.identifier, this.mockFeatures);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(this.mockFeatures.Length);
            collection.Identifiers.First().ShouldBe(this.identifier);
            collection.IsReadOnly.ShouldBeFalse();
            (collection.Geometry as IGeometryCollection<IGeometry>).First().ShouldBe(this.mockGeometry);
            collection.Attributes.ShouldBeEmpty();

            // with attribute and features
            collection = factory.CreateCollection(this.identifier, this.mockAttributes, this.mockFeatures);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(this.mockFeatures.Length);
            collection.Identifiers.First().ShouldBe(this.identifier);
            collection.IsReadOnly.ShouldBeFalse();
            (collection.Geometry as IGeometryCollection<IGeometry>).First().ShouldBe(this.mockGeometry);
            collection.Attributes.ShouldBe(this.mockAttributes);

            // from another feature collection
            IFeatureCollection otherCollection = factory.CreateCollection(collection);
            otherCollection.ShouldBeOfType<FeatureCollection>();
            otherCollection.Identifier.ShouldBe(this.identifier);
            otherCollection.Count.ShouldBe(this.mockFeatures.Length);
            otherCollection.Identifiers.First().ShouldBe(this.identifier);
            otherCollection.IsReadOnly.ShouldBeFalse();
            (otherCollection.Geometry as IGeometryCollection<IGeometry>).First().ShouldBe(this.mockGeometry);
            otherCollection.Attributes.ShouldBe(this.mockAttributes);

            Should.Throw<ArgumentNullException>(() => factory.CreateCollection((String)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection((IFeatureCollection)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection(null, this.mockAttributes));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection(String.Empty, (IAttributeCollection)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection(null, this.mockFeatures));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection(String.Empty, (IEnumerable<IFeature>)null));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection(null, this.mockAttributes, this.mockFeatures));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection(String.Empty, this.mockAttributes, null));
            Should.Throw<ArgumentNullException>(() => factory.CreateCollection(String.Empty, null, this.mockFeatures));
        }
    }
}