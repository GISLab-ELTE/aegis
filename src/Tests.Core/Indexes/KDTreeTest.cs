// <copyright file="KDTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Geometries;
    using AEGIS.Indexes;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="KDTree" /> class.
    /// </summary>
    [TestFixture]
    public class KDTreeTest
    {
        /// <summary>
        /// A list of 2-dimensional coordinates.
        /// </summary>
        private List<Coordinate> coords2D;

        /// <summary>
        /// A list of 3-dimensional coordinates.
        /// </summary>
        private List<Coordinate> coords3D;

        /// <summary>
        /// A 2 dimensional k-d tree.
        /// </summary>
        private KDTree tree2D;

        /// <summary>
        /// A 3 dimensional k-d tree.
        /// </summary>
        private KDTree tree3D;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.coords2D = new List<Coordinate>(Enumerable.Range(1, 100).Select(value => new Coordinate(value, value)));
            this.coords3D = new List<Coordinate>(Enumerable.Range(1, 100).Select(value => new Coordinate(value, value, value)));
        }

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void KDTreeConstructorTest()
        {
            // Regular cases
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree2D.NumberOfGeometries.ShouldBe(100);
            this.tree2D.IsReadOnly.ShouldBeFalse();

            this.tree3D = new KDTree(this.coords3D, 3);
            this.tree3D.NumberOfGeometries.ShouldBe(100);
            this.tree3D.IsReadOnly.ShouldBeFalse();

            // Empty tree case
            KDTree tree = new KDTree(new List<Coordinate>(), 2);
            tree.NumberOfGeometries.ShouldBe(0);

            // Exception cases
            Should.Throw<ArgumentOutOfRangeException>(() => new KDTree(new List<Coordinate>(), 1));
            Should.Throw<ArgumentOutOfRangeException>(() => new KDTree(new List<Coordinate>(), 4));
            Should.Throw<ArgumentNullException>(() => new KDTree(null, 3));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Add(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void KDTreeAddCoordinateTest()
        {
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree2D.Add(new Coordinate(101, 101));
            this.tree2D.NumberOfGeometries.ShouldBe(101);

            this.tree3D = new KDTree(this.coords3D, 3);
            this.tree3D.Add(new Coordinate(101, 101, 101));
            this.tree3D.Add(new Coordinate(101, 101));
            this.tree3D.NumberOfGeometries.ShouldBe(102);

            Should.Throw<ArgumentNullException>(() => this.tree2D.Add((Coordinate)null));
            Should.Throw<ArgumentNullException>(() => this.tree3D.Add((Coordinate)null));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Add(IEnumerable{Coordinate})" /> method.
        /// </summary>
        [Test]
        public void KDTreeAddCollectionTest()
        {
            List<Coordinate> moreCoords2D = new List<Coordinate>() { new Coordinate(102, 102), new Coordinate(103, 103) };
            List<Coordinate> moreCoords3D = new List<Coordinate>() { new Coordinate(102, 102, 102), new Coordinate(103, 103, 103) };

            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree2D.Add(moreCoords2D);
            this.tree2D.NumberOfGeometries.ShouldBe(102);

            this.tree3D = new KDTree(this.coords3D, 3);
            this.tree3D.Add(moreCoords3D);
            this.tree3D.NumberOfGeometries.ShouldBe(102);

            Should.Throw<ArgumentNullException>(() => this.tree2D.Add((List<Coordinate>)null));
            Should.Throw<ArgumentNullException>(() => this.tree3D.Add((List<Coordinate>)null));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Contains(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void KDTreeContainsTest()
        {
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree3D = new KDTree(this.coords3D, 3);

            // Null cases
            this.tree2D.Contains(null).ShouldBeFalse();
            this.tree3D.Contains(null).ShouldBeFalse();

            // Regular not contains cases
            this.tree2D.Contains(new Coordinate(1, 2)).ShouldBeFalse();
            this.tree3D.Contains(new Coordinate(1, 2, 3)).ShouldBeFalse();

            // Different dimension coordinate cases
            this.tree3D.Contains(new Coordinate(1, 1)).ShouldBeFalse();
            this.tree2D.Contains(new Coordinate(1, 1, 1)).ShouldBeFalse();

            // Regular contains cases
            foreach (Coordinate point in this.coords2D)
                this.tree2D.Contains(point).ShouldBeTrue();
            foreach (Coordinate point in this.coords3D)
                this.tree3D.Contains(point).ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Search(Envelope)" /> method.
        /// </summary>
        [Test]
        public void KDTreeSearchTest()
        {
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree3D = new KDTree(this.coords3D, 3);

            // Should be empty for envelopes that do not contain any of the coordinates
            this.tree2D.Search(new Envelope(0.5, 0.6, 0.5, 0.6)).ShouldBeEmpty();
            this.tree2D.Search(new Envelope(200, 210, 200, 210)).ShouldBeEmpty();
            this.tree3D.Search(new Envelope(0.5, 0.6, 0.5, 0.6, 0.5, 0.6)).ShouldBeEmpty();
            this.tree3D.Search(new Envelope(200, 210, 200, 210, 200, 210)).ShouldBeEmpty();

            // Should find all geometries as results for these searches
            this.tree2D.Search(Envelope.Infinity).Count().ShouldBe(this.tree2D.NumberOfGeometries);
            this.tree2D.Search(Envelope.FromCoordinates(this.coords2D)).Count().ShouldBe(this.tree2D.NumberOfGeometries);
            this.tree3D.Search(Envelope.Infinity).Count().ShouldBe(this.tree3D.NumberOfGeometries);
            this.tree3D.Search(Envelope.FromCoordinates(this.coords3D)).Count().ShouldBe(this.tree3D.NumberOfGeometries);

            // Should find correct results for some concrete examples
            this.tree2D.Search(Envelope.FromCoordinates(new Coordinate(0, 0), new Coordinate(10, 10))).Count().ShouldBe(10);
            this.tree3D.Search(Envelope.FromCoordinates(new Coordinate(0, 0, 0), new Coordinate(10, 10, 10))).Count().ShouldBe(10);

            // Should throw exception for null
            Should.Throw<ArgumentNullException>(() => this.tree2D.Search(null));
            Should.Throw<ArgumentNullException>(() => this.tree3D.Search(null));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Remove(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void KDTreeRemoveCoordinateTest()
        {
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree3D = new KDTree(this.coords3D, 3);

            // Should not remove when coordinate is not in the tree
            this.tree2D.Remove(new Coordinate(101, 101)).ShouldBeFalse();
            this.tree3D.Remove(new Coordinate(101, 101, 101)).ShouldBeFalse();

            // Should remove geometries that are in the tree
            foreach (Coordinate point in this.coords2D)
                this.tree2D.Remove(point).ShouldBeTrue();

            this.tree2D.NumberOfGeometries.ShouldBe(0);

            foreach (Coordinate point in this.coords3D)
                this.tree3D.Remove(point).ShouldBeTrue();

            this.tree3D.NumberOfGeometries.ShouldBe(0);

            // Should throw exception when removing with null
            Should.Throw<ArgumentNullException>(() => this.tree2D.Remove((Coordinate)null));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Remove(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void KDTreeRemoveEnvelopeTest()
        {
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree3D = new KDTree(this.coords3D, 3);

            // Should not remove when envelope does not contain any point from tree
            this.tree2D.Remove(new Envelope(101, 102, 101, 102)).ShouldBeFalse();
            this.tree3D.Remove(new Envelope(101, 102, 101, 102, 101, 102)).ShouldBeFalse();

            // Should remove correctly based on envelope
            this.tree2D.Add(this.coords2D);
            this.tree2D.Remove(new Envelope(0, 49, 0, 49)).ShouldBeTrue();
            this.tree2D.NumberOfGeometries.ShouldBe(152);
            this.tree2D.Remove(new Envelope(0, 100, 0, 100)).ShouldBeTrue();
            this.tree2D.NumberOfGeometries.ShouldBe(0);

            this.tree3D.Add(this.coords3D);
            this.tree3D.Remove(new Envelope(0, 49, 0, 49, 0, 49)).ShouldBeTrue();
            this.tree3D.NumberOfGeometries.ShouldBe(152);
            this.tree3D.Remove(new Envelope(0, 100, 0, 100, 0, 100)).ShouldBeTrue();
            this.tree3D.NumberOfGeometries.ShouldBe(0);

            // Should throw exception when removing with null
            Should.Throw<ArgumentNullException>(() => this.tree2D.Remove((Envelope)null));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Remove(Envelope, out List{Coordinate})" /> method.
        /// </summary>
        [Test]
        public void KDTreeRemoveEnvelopeWithResultTest()
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            // Should remove based on envelope with results
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree2D.Remove(new Envelope(0, 100, 0, 100), out coordinates).ShouldBeTrue();
            coordinates.Count.ShouldBe(this.coords2D.Count);

            this.tree2D.Remove(new Envelope(0, 100, 0, 100), out coordinates).ShouldBeFalse();
            coordinates.Count.ShouldBe(0);

            // Should throw exception when removing with null
            Should.Throw<ArgumentNullException>(() => this.tree2D.Remove(null, out coordinates));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.SearchNearest(Coordinate)" /> method.
        /// </summary>
        [Test]
        public void KDTreeSearchNearestTest()
        {
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree3D = new KDTree(this.coords3D, 3);

            // Regular cases
            this.tree2D.SearchNearest(new Coordinate(0, 0)).ShouldBe(new Coordinate(1, 1));
            this.tree2D.SearchNearest(new Coordinate(1, 1)).ShouldBe(new Coordinate(1, 1));
            this.tree2D.SearchNearest(new Coordinate(50.6, 50.6)).ShouldBe(new Coordinate(51, 51));
            this.tree2D.SearchNearest(new Coordinate(50.4, 50.4)).ShouldBe(new Coordinate(50, 50));

            this.tree3D.SearchNearest(new Coordinate(0, 0, 0)).ShouldBe(new Coordinate(1, 1, 1));
            this.tree3D.SearchNearest(new Coordinate(1, 1, 1)).ShouldBe(new Coordinate(1, 1, 1));
            this.tree3D.SearchNearest(new Coordinate(50.6, 50.6, 50.6)).ShouldBe(new Coordinate(51, 51, 51));
            this.tree3D.SearchNearest(new Coordinate(50.4, 50.4, 50.4)).ShouldBe(new Coordinate(50, 50, 50));

            // Error cases
            Should.Throw<ArgumentNullException>(() => this.tree2D.SearchNearest(null));
        }

        /// <summary>
        /// Tests the <see cref="KDTree.Clear" /> method.
        /// </summary>
        [Test]
        public void KDTreeClearTest()
        {
            this.tree2D = new KDTree(this.coords2D, 2);
            this.tree2D.Clear();
            this.tree2D.NumberOfGeometries.ShouldBe(0);
        }
    }
}