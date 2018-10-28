// <copyright file="FeatureCollectionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Features;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="FeatureCollection" /> class.
    /// </summary>
    [TestFixture]
    public class FeatureCollectionTest
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

            this.mockFeatures = new IFeature[10];
            for (Int32 i = 0; i < this.mockFeatures.Length; i++)
            {
                Mock<IFeature> mockFeature = new Mock<IFeature>();
                mockFeature.Setup(feature => feature.Identifier).Returns(Guid.NewGuid().ToString());
                mockFeature.Setup(feature => feature.Geometry).Returns(this.mockGeometry);
                mockFeature.Setup(feature => feature.Attributes).Returns(this.mockAttributes);
                this.mockFeatures[i] = mockFeature.Object;
            }
        }

        /// <summary>
        /// Tests constructors of the <see cref="FeatureCollection" /> class.
        /// </summary>
        [Test]
        public void FeatureCollectionConstructorTest()
        {
            // without features and attributes
            FeatureCollection collection = new FeatureCollection(this.identifier, null);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(0);
            collection.Identifiers.ShouldBeEmpty();
            collection.IsReadOnly.ShouldBeFalse();
            collection.Geometry.ShouldBeNull();
            collection.Attributes.ShouldBeEmpty();

            // with attributes
            collection = new FeatureCollection(this.identifier, this.mockAttributes);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(0);
            collection.Identifiers.ShouldBeEmpty();
            collection.IsReadOnly.ShouldBeFalse();
            collection.Geometry.ShouldBeNull();
            collection.Attributes.ShouldBe(this.mockAttributes);

            // with features
            collection = new FeatureCollection(this.identifier, null, this.mockFeatures);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(this.mockFeatures.Length);
            collection.IsReadOnly.ShouldBeFalse();
            (collection.Geometry as IGeometryCollection<IGeometry>).First().ShouldBe(this.mockGeometry);
            collection.Attributes.ShouldBeEmpty();

            // with attribute and features
            collection = new FeatureCollection(this.identifier, this.mockAttributes, this.mockFeatures);
            collection.ShouldBeOfType<FeatureCollection>();
            collection.Identifier.ShouldBe(this.identifier);
            collection.Count.ShouldBe(this.mockFeatures.Length);
            collection.IsReadOnly.ShouldBeFalse();
            (collection.Geometry as IGeometryCollection<IGeometry>).First().ShouldBe(this.mockGeometry);
            collection.Attributes.ShouldBe(this.mockAttributes);

            Should.Throw<ArgumentNullException>(() => new FeatureCollection(null, null));
            Should.Throw<ArgumentNullException>(() => new FeatureCollection(this.identifier, null, null));
        }

        /// <summary>
        /// Tests the <see cref="FeatureCollection.Add(IFeature)" /> method.
        /// </summary>
        [Test]
        public void FeatureCollectionAddTest()
        {
            FeatureCollection collection = new FeatureCollection(this.identifier, null);

            collection.Add(this.mockFeatures[0]);
            collection.Count.ShouldBe(1);
            collection[this.mockFeatures[0].Identifier].ShouldBe(this.mockFeatures[0]);

            collection.Add(this.mockFeatures[1]);
            collection.Count.ShouldBe(2);
            collection[this.mockFeatures[0].Identifier].ShouldBe(this.mockFeatures[0]);
            collection[this.mockFeatures[1].Identifier].ShouldBe(this.mockFeatures[1]);

            Should.Throw<ArgumentNullException>(() => collection.Add(null));
            Should.Throw<ArgumentException>(() => collection.Add(this.mockFeatures[0]));
        }

        /// <summary>
        /// Tests the <see cref="FeatureCollection.Clear" /> method.
        /// </summary>
        [Test]
        public void FeatureCollectionClearTest()
        {
            FeatureCollection collection = new FeatureCollection(this.identifier, this.mockAttributes, this.mockFeatures);
            collection.Clear();
            collection.Count.ShouldBe(0);
        }

        /// <summary>
        /// Tests the <see cref="FeatureCollection.Contains(IFeature)" /> method.
        /// </summary>
        [Test]
        public void FeatureCollectionContainsTest()
        {
            FeatureCollection collection = new FeatureCollection(this.identifier, this.mockAttributes, this.mockFeatures.Take(1));
            collection.Contains(this.mockFeatures[0]).ShouldBeTrue();
            collection.Contains(this.mockFeatures[1]).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => collection.Contains(null));
        }

        /// <summary>
        /// Tests the <see cref="FeatureCollection.CopyTo(IFeature[], Int32)" /> method.
        /// </summary>
        [Test]
        public void FeatureCollectionCopyToTest()
        {
            FeatureCollection collection = new FeatureCollection(this.identifier, this.mockAttributes, this.mockFeatures);

            IFeature[] array = new IFeature[collection.Count];
            collection.CopyTo(array, 0);

            foreach (IFeature feature in this.mockFeatures)
            {
                array.Contains(feature).ShouldBeTrue();
            }

            Should.Throw<ArgumentNullException>(() => collection.CopyTo(null, 0));
            Should.Throw<ArgumentOutOfRangeException>(() => collection.CopyTo(array, -1));
            Should.Throw<ArgumentException>(() => collection.CopyTo(array, 1));
        }

        /// <summary>
        /// Tests the <see cref="FeatureCollection.Remove(IFeature)" /> method.
        /// </summary>
        [Test]
        public void FeatureCollectionRemoveTest()
        {
            FeatureCollection collection = new FeatureCollection(this.identifier, this.mockAttributes, this.mockFeatures.Take(1));
            collection.Remove(this.mockFeatures[0]).ShouldBeTrue();
            collection.Count.ShouldBe(0);
            collection.Contains(this.mockFeatures[0]).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => collection.Remove(null));
        }

        /// <summary>
        /// Tests collection enumeration.
        /// </summary>
        [Test]
        public void FeatureCollectionEnumerateTest()
        {
            FeatureCollection collection = new FeatureCollection(this.identifier, this.mockAttributes, this.mockFeatures);

            foreach (IFeature feature in collection)
            {
                this.mockFeatures.Contains(feature).ShouldBeTrue();
            }

            foreach (IFeature feature in (IEnumerable)collection)
            {
                this.mockFeatures.Contains(feature).ShouldBeTrue();
            }
        }
    }
}
