// <copyright file="HilbertRTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Indexes.Rectangle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AEGIS.Geometries;
    using AEGIS.Indexes.Rectangle;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Tests for the <see cref="HilbertRTree"/> class.
    /// </summary>
    [TestFixture]
    public class HilbertRTreeTest
    {
        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory factory;

        /// <summary>
        /// The list of geometries.
        /// </summary>
        private List<IPoint> geometries;

        /// <summary>
        /// The tree under test.
        /// </summary>
        private HilbertRTree tree;

        /// <summary>
        /// Sets up the test class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.factory = new GeometryFactory();
            this.geometries = new List<IPoint>(Enumerable.Range(1, 1000).Select(value => this.factory.CreatePoint(value, value, value)));
            this.tree = new HilbertRTree();
            this.tree.Add(this.geometries);
        }

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void HilbertRTreeConstructorTest()
        {
            HilbertRTree tree = new HilbertRTree();
            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(8);
            tree.MaxChildren.ShouldBe(12);
            tree.IsReadOnly.ShouldBeFalse();

            tree = new HilbertRTree(9);
            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(6);
            tree.MaxChildren.ShouldBe(9);
            tree.IsReadOnly.ShouldBeFalse();

            Should.Throw<ArgumentOutOfRangeException>(() => new HilbertRTree(-3));
            Should.Throw<ArgumentOutOfRangeException>(() => new HilbertRTree(0));
            Should.Throw<ArgumentOutOfRangeException>(() => new HilbertRTree(20));
            Should.Throw<ArgumentNullException>(() => new HilbertRTree(3, null));
        }

        /// <summary>
        /// Tests the <see cref="Add" /> method.
        /// </summary>
        [Test]
        public void HilbertRTreeAddTest()
        {
            HilbertRTree tree = new HilbertRTree(3);

            // the maxChildren in this case is the minimum allowed 3
            // the minChildren in this case is 3 * 2 / 3 = 2
            // the root node has a maxChildren count of 2 * minChildren, so in this case it's 4
            // therefore the height shall increase once we insert the fifth geometry
            tree.Add(this.geometries[0]);
            tree.Height.ShouldBe(1);
            tree.NumberOfGeometries.ShouldBe(1);

            tree.Add(this.geometries[1]);
            tree.Height.ShouldBe(1);
            tree.NumberOfGeometries.ShouldBe(2);

            tree.Add(this.geometries[2]);
            tree.Height.ShouldBe(1);
            tree.NumberOfGeometries.ShouldBe(3);

            tree.Add(this.geometries[3]);
            tree.Height.ShouldBe(1);
            tree.NumberOfGeometries.ShouldBe(4);

            tree.Add(this.geometries[4]);
            tree.Height.ShouldBe(2);
            tree.NumberOfGeometries.ShouldBe(5);

            // insert the remaining geometries
            for (Int32 geometryIndex = 5; geometryIndex < this.geometries.Count; geometryIndex++)
                tree.Add(this.geometries[geometryIndex]);

            // check that the tree has a correct height
            tree.NumberOfGeometries.ShouldBe(this.geometries.Count);
            tree.Height.ShouldBeGreaterThanOrEqualTo((Int32)Math.Floor(Math.Log(this.geometries.Count, 3)));
            tree.Height.ShouldBeLessThanOrEqualTo((Int32)Math.Ceiling(Math.Log(this.geometries.Count, 2)));

            // add a complete collection
            tree = new HilbertRTree();
            tree.Add(this.geometries);
            tree.NumberOfGeometries.ShouldBe(this.geometries.Count);

            // errors
            Should.Throw<ArgumentNullException>(() => tree.Add((IGeometry)null));
            Should.Throw<ArgumentNullException>(() => tree.Add((IEnumerable<IGeometry>)null));
        }

        /// <summary>
        /// Tests the <see cref="Search" /> method.
        /// </summary>
        [Test]
        public void HilbertRTreeSearchTest()
        {
            // empty results
            for (Int32 value = 0; value < 1000; value++)
                this.tree.Search(new Envelope(value, value, value, value, 0, 0)).ShouldBeEmpty();

            // all results
            this.tree.Search(Envelope.Infinity).Count().ShouldBe(this.geometries.Count);
            this.tree.Search(Envelope.FromEnvelopes(this.geometries.Select(geometry => geometry.Envelope))).Count().ShouldBe(this.geometries.Count);

            // exact results
            HilbertRTree tree = new HilbertRTree(3);

            Coordinate[] firstShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 10, 0),
                new Coordinate(10, 10, 0),
                new Coordinate(10, 0, 0),
                new Coordinate(0, 0, 0),
            };

            Coordinate[] secondShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 15, 0),
                new Coordinate(15, 15, 0),
                new Coordinate(15, 0, 0),
                new Coordinate(0, 0, 0),
            };

            Coordinate[] thirdShell = new Coordinate[]
            {
                new Coordinate(30, 30, 0),
                new Coordinate(30, 40, 0),
                new Coordinate(40, 40, 0),
                new Coordinate(40, 30, 0),
                new Coordinate(30, 30, 0),
            };

            tree.Add(this.factory.CreatePolygon(firstShell));
            tree.Add(this.factory.CreatePolygon(secondShell));
            tree.Add(this.factory.CreatePolygon(thirdShell));

            tree.Search(Envelope.FromCoordinates(firstShell)).Count().ShouldBe(1);
            tree.Search(Envelope.FromCoordinates(secondShell)).Count().ShouldBe(2);
            tree.Search(Envelope.FromCoordinates(thirdShell)).Count().ShouldBe(1);
            tree.Search(Envelope.FromCoordinates(secondShell.Union(thirdShell))).Count().ShouldBe(3);

            // errors
            Should.Throw<ArgumentNullException>(() => this.tree.Search(null));
        }

        /// <summary>
        /// Tests the <see cref="Contains" /> method.
        /// </summary>
        [Test]
        public void HilbertRTreeContainsTest()
        {
            this.tree.Contains(null).ShouldBeFalse();
            this.tree.Contains(this.factory.CreatePoint(1000, 1000, 1000)).ShouldBeFalse();

            HilbertRTree tree = new HilbertRTree(3);
            tree.Add(this.geometries);
            Int32 contained = this.geometries.Count(geometry => tree.Contains(geometry));
            contained.ShouldBe(this.geometries.Count);
        }

        /// <summary>
        /// Tests the <see cref="Remove" /> method.
        /// </summary>
        [Test]
        public void HilbertRTreeRemoveTest()
        {
            // remove geometry
            this.tree.Remove(this.factory.CreatePoint(1000, 1000, 1000)).ShouldBeFalse();

            foreach (IPoint geometry in this.geometries)
                this.tree.Remove(geometry).ShouldBeTrue();

            // remove envelope
            this.tree.Add(this.geometries);
            this.tree.Remove(Envelope.Infinity).ShouldBeTrue();

            // remove envelope with results
            List<IBasicGeometry> geometries;

            this.tree.Remove(Envelope.Infinity, out geometries).ShouldBeFalse();
            geometries.Count.ShouldBe(0);

            this.tree.Add(this.geometries);
            this.tree.Remove(Envelope.Infinity, out geometries).ShouldBeTrue();
            geometries.Count.ShouldBe(this.geometries.Count);

            // errors
            Should.Throw<ArgumentNullException>(() => this.tree.Remove((Envelope)null));
            Should.Throw<ArgumentNullException>(() => this.tree.Remove((IGeometry)null));
        }

        /// <summary>
        /// Tests the <see cref="Clear" /> method.
        /// </summary>
        [Test]
        public void HilbertRTreeClearTest()
        {
            this.tree.Clear();

            this.tree.Height.ShouldBe(0);
            this.tree.NumberOfGeometries.ShouldBe(0);
        }
    }
}
