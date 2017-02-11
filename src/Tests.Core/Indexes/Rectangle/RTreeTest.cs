// <copyright file="RTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Indexes.Rectangle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Geometries;
    using ELTE.AEGIS.Indexes.Rectangle;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="RTree" /> class.
    /// </summary>
    [TestFixture]
    public class RTreeTest
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
        /// The R-tree.
        /// </summary>
        private RTree tree;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.factory = new GeometryFactory();
            this.geometries = new List<IPoint>(Enumerable.Range(1, 1000).Select(value => this.factory.CreatePoint(value, value, value)));

            this.tree = new RTree();
            this.tree.Add(this.geometries);
        }

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void RTreeConstructorTest()
        {
            // default values

            RTree tree = new RTree();

            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(8);
            tree.MaxChildren.ShouldBe(12);
            tree.IsReadOnly.ShouldBeFalse();

            // min. 2, max. 4 children

            tree = new RTree(2, 4);

            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(2);
            tree.MaxChildren.ShouldBe(4);
            tree.IsReadOnly.ShouldBeFalse();

            // min. 10, max. 100 children

            tree = new RTree(10, 100);

            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(10);
            tree.MaxChildren.ShouldBe(100);
            tree.IsReadOnly.ShouldBeFalse();

            // errors

            Should.Throw<ArgumentOutOfRangeException>(() => new RStarTree(0, 1));
            Should.Throw<ArgumentOutOfRangeException>(() => new RStarTree(1, 1));
        }

        /// <summary>
        /// Tests the <see cref="Add" /> method.
        /// </summary>
        [Test]
        public void RTreeAddTest()
        {
            RTree tree = new RTree(2, 3);

            // add a single geometry

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
            tree.Height.ShouldBe(2);
            tree.NumberOfGeometries.ShouldBe(4);

            for (Int32 geometryIndex = 4; geometryIndex < this.geometries.Count; geometryIndex++)
                tree.Add(this.geometries[geometryIndex]);

            tree.NumberOfGeometries.ShouldBe(this.geometries.Count);
            tree.Height.ShouldBeGreaterThanOrEqualTo((Int32)Math.Floor(Math.Log(this.geometries.Count, 3)));
            tree.Height.ShouldBeLessThanOrEqualTo((Int32)Math.Ceiling(Math.Log(this.geometries.Count, 2)));

            // add a complete collection

            tree = new RTree(2, 12);
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
        public void RTreeSearchTest()
        {
            // empty results

            this.tree.Search(Envelope.Undefined).ShouldBeEmpty();

            for (Int32 value = 0; value < 1000; value++)
                this.tree.Search(new Envelope(value, value, value, value, 0, 0)).ShouldBeEmpty();

            // all results
            this.tree.Search(Envelope.Infinity).Count().ShouldBe(this.tree.NumberOfGeometries);
            this.tree.Search(Envelope.FromEnvelopes(this.geometries.Select(geometry => geometry.Envelope))).Count().ShouldBe(this.tree.NumberOfGeometries);

            // exact results

            RTree tree = new RTree();

            Coordinate[] firstShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 10, 0),
                new Coordinate(10, 10, 0),
                new Coordinate(10, 0, 0),
                new Coordinate(0, 0, 0)
            };

            Coordinate[] secondShell = new Coordinate[]
            {
                new Coordinate(0, 0, 0),
                new Coordinate(0, 15, 0),
                new Coordinate(15, 15, 0),
                new Coordinate(15, 0, 0),
                new Coordinate(0, 0, 0)
            };

            Coordinate[] thirdShell = new Coordinate[]
            {
                new Coordinate(30, 30, 0),
                new Coordinate(30, 40, 0),
                new Coordinate(40, 40, 0),
                new Coordinate(40, 30, 0),
                new Coordinate(30, 30, 0)
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
        public void RTreeContainsTest()
        {
            this.tree.Contains(null).ShouldBeFalse();
            this.tree.Contains(this.factory.CreatePoint(1000, 1000, 1000)).ShouldBeFalse();
            this.tree.Contains(this.factory.CreatePoint(Coordinate.Undefined)).ShouldBeFalse();

            foreach (IPoint geometry in this.geometries)
                this.tree.Contains(geometry).ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="Remove" /> method.
        /// </summary>
        [Test]
        public void RTreeRemoveTest()
        {
            // remove geometry
            this.tree.Remove(this.factory.CreatePoint(1000, 1000, 1000)).ShouldBeFalse();
            this.tree.Remove(this.factory.CreatePoint(Coordinate.Undefined)).ShouldBeFalse();

            foreach (IPoint geometry in this.geometries)
                this.tree.Remove(geometry).ShouldBeTrue();

            // remove envelope
            this.tree.Add(this.geometries);

            this.tree.Remove(Envelope.Infinity).ShouldBeTrue();

            // remove envelope with results
            List<IGeometry> geometries;

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
        public void RTreeClearTest()
        {
            this.tree.Clear();

            this.tree.Height.ShouldBe(0);
            this.tree.NumberOfGeometries.ShouldBe(0);
        }
    }
}