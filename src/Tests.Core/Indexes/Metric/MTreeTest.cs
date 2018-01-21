// <copyright file="MTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Indexes.Metric
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AEGIS.Algorithms;
    using AEGIS.Geometries;
    using AEGIS.Indexes.Metric;
    using AEGIS.Indexes.Metric.SplitPolicy;
    using NUnit.Framework;
    using Shouldly;
    using static AEGIS.Indexes.Metric.MTree<IPoint>;

    [TestFixture]
    public class MTreeTest
    {
        /// <summary>
        /// The factory used to create geometries (test data).
        /// </summary>
        private IGeometryFactory factory;

        /// <summary>
        /// The geometries (test data).
        /// </summary>
        private List<IPoint> geometries;

        /// <summary>
        /// The M-tree under test.
        /// </summary>
        private MTree<IPoint> tree;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.factory = new GeometryFactory();
            this.geometries = new List<IPoint>(Enumerable.Range(1, 1000).Select(value => this.factory.CreatePoint(value, value, value)));

            this.tree = new MTree<IPoint>((x, y) => GeometryDistanceAlgorithm.Distance(x, y));
            this.tree.Add(this.geometries);
        }

        /// <summary>
        /// Tests the constructors of <see cref="MTree{T}"/>.
        /// </summary>
        [Test]
        public void MTreeConstructorTest()
        {
            MTree<IPoint> tree = new MTree<IPoint>((x, y) => GeometryDistanceAlgorithm.Distance(x, y));
            tree.NumberOfDataItems.ShouldBe(0);
            tree.MinChildren.ShouldBe(50);
            tree.MaxChildren.ShouldBe(101);

            tree = new MTree<IPoint>(2, 4, (x, y) => GeometryDistanceAlgorithm.Distance(x, y));
            tree.NumberOfDataItems.ShouldBe(0);
            tree.MinChildren.ShouldBe(2);
            tree.MaxChildren.ShouldBe(4);

            tree = new MTree<IPoint>(10, 100, (x, y) => GeometryDistanceAlgorithm.Distance(x, y));
            tree.NumberOfDataItems.ShouldBe(0);
            tree.MinChildren.ShouldBe(10);
            tree.MaxChildren.ShouldBe(100);

            Should.Throw<ArgumentOutOfRangeException>(() => new MTree<IPoint>(-10, 1, (x, y) => GeometryDistanceAlgorithm.Distance(x, y)));
            Should.Throw<ArgumentOutOfRangeException>(() => new MTree<IPoint>(0, 1, (x, y) => GeometryDistanceAlgorithm.Distance(x, y)));
            Should.Throw<ArgumentOutOfRangeException>(() => new MTree<IPoint>(1, 1, (x, y) => GeometryDistanceAlgorithm.Distance(x, y)));

            Should.Throw<ArgumentNullException>(() => new MTree<IPoint>(3, 5, null));
        }

        /// <summary>
        /// Tests the <see cref="MTree{T}.Add(T)"/> and <see cref="MTree{T}.Add(IEnumerable{T})"/> methods.
        /// </summary>
        [Test]
        public void MTreeAddTest()
        {
            MTree<IPoint> tree = new MTree<IPoint>(2, 3, (x, y) => GeometryDistanceAlgorithm.Distance(x, y));

            tree.Add(this.geometries[0]);
            tree.NumberOfDataItems.ShouldBe(1);

            tree.Add(this.geometries[1]);
            tree.NumberOfDataItems.ShouldBe(2);

            tree.Add(this.geometries[2]);
            tree.NumberOfDataItems.ShouldBe(3);

            tree.Add(this.geometries[3]);
            tree.NumberOfDataItems.ShouldBe(4);

            for (int i = 4; i < this.geometries.Count; i++)
                tree.Add(this.geometries[i]);

            tree.NumberOfDataItems.ShouldBe(this.geometries.Count);

            tree = new MTree<IPoint>(2, 3, (x, y) => GeometryDistanceAlgorithm.Distance(x, y));

            tree.Add(this.geometries);
            tree.NumberOfDataItems.ShouldBe(this.geometries.Count);

            Should.Throw<ArgumentNullException>(() => tree.Add((IPoint)null));
            Should.Throw<ArgumentNullException>(() => tree.Add((IEnumerable<IPoint>)null));

            // try to insert item already in the tree
            Should.Throw<ArgumentException>(() => tree.Add(this.geometries[0]));
        }

        /// <summary>
        /// Tests the <see cref="MTree{T}.Contains(T)"/> method.
        /// </summary>
        [Test]
        public void MTreeContainsTest()
        {
            this.geometries.Count(x => this.tree.Contains(x)).ShouldBe(this.geometries.Count);

            IPoint pointNotPresentInTree = this.factory.CreatePoint(5000D, 5000D, 5000D);
            this.tree.Contains(pointNotPresentInTree).ShouldBeFalse();

            pointNotPresentInTree = this.factory.CreatePoint(1.000001D, 1.000001D, 1.000001D);
            this.tree.Contains(pointNotPresentInTree).ShouldBeFalse();

            this.tree.Contains(this.factory.CreatePoint(Coordinate.Undefined)).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => this.tree.Contains(null));
        }

        /// <summary>
        /// Tests the <see cref="MTree{T}.Search(T, int)"/> and <see cref="MTree{T}.Search(T, double, int)"/> methods.
        /// </summary>
        [Test]
        public void MTreeSearchTest()
        {
            MTree<IPoint> tree = new MTree<IPoint>(2, 3, (x, y) => GeometryDistanceAlgorithm.Distance(x, y));

            tree.Search(this.geometries[0]).ShouldBeEmpty();

            IPoint[] points = new IPoint[9];

            for (Int32 i = 0; i < 9; i++)
            {
                points[i] = this.factory.CreatePoint(i, 0);
                tree.Add(points[i]);
            }

            // search without limits should return all items in the tree
            tree.Search(this.factory.CreatePoint(2, 0)).Count().ShouldBe(9);

            // search returns the correct data items in the correct order with correct distance information
            List<ResultItem<IPoint>> results = new List<ResultItem<IPoint>>(tree.Search(this.factory.CreatePoint(8, 0)));
            List<IPoint> expectedResults = new List<IPoint>(points.Reverse());

            for (Int32 i = 0; i < points.Length; i++)
            {
                results[i].Item.ShouldBe(expectedResults[i]);
                results[i].Distance.ShouldBe(i);
            }

            // search with a given radius should return only the elements within that radius
            tree.Search(this.factory.CreatePoint(8, 0), 1.0D).Count().ShouldBe(2);
            tree.Search(this.factory.CreatePoint(7, 0), 1.0D).Count().ShouldBe(3);
            tree.Search(this.factory.CreatePoint(9, 0), 1.0D).Count().ShouldBe(1);
            tree.Search(this.factory.CreatePoint(10, 0), 1.0D).Count().ShouldBe(0);

            // search with a given limit should return only that amount of elements
            tree.Search(this.factory.CreatePoint(2, 0), 0).Count().ShouldBe(0);
            tree.Search(this.factory.CreatePoint(2, 0), 1).Count().ShouldBe(1);
            tree.Search(this.factory.CreatePoint(2, 0), 5).Count().ShouldBe(5);
            tree.Search(this.factory.CreatePoint(2, 0), 10).Count().ShouldBe(9);

            // search with both radius and limit where radius contains less elements than limit
            tree.Search(this.factory.CreatePoint(2, 0), 1.1D, 5).Count().ShouldBe(3);

            // search with both radius and limit where radius contains more elements than limit
            tree.Search(this.factory.CreatePoint(2, 0), 1.1D, 2).Count().ShouldBe(2);

            // errors
            Should.Throw<ArgumentNullException>(() => tree.Search(null));
        }

        /// <summary>
        /// Tests the <see cref="MTree{T}.Remove(T)"/> method.
        /// </summary>
        [Test]
        public void MTreeRemoveTest()
        {
            this.tree.Remove(this.factory.CreatePoint(1000, 1000, 1000)).ShouldBeFalse();
            this.tree.Remove(this.factory.CreatePoint(Coordinate.Undefined)).ShouldBeFalse();

            this.geometries.Count(p => this.tree.Remove(p)).ShouldBe(this.geometries.Count);

            // errors
            Should.Throw<ArgumentNullException>(() => this.tree.Remove(null));
        }

        /// <summary>
        /// Tests the <see cref="MTree{T}.Clear"/> method.
        /// </summary>
        [Test]
        public void MTreeClearTest()
        {
            this.tree.Clear();
            this.tree.NumberOfDataItems.ShouldBe(0);
        }
    }
}
