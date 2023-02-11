// <copyright file="RStarTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Geometries;
    using AEGIS.Indexes.Rectangle;
    using AEGIS.Numerics;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Text fixture for the <see cref="RStarTree" /> class.
    /// </summary>
    /// <author>Tamás Nagy</author>
    [TestFixture]
    public class RStarTreeTest
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
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.factory = new GeometryFactory();
            this.geometries = new List<IPoint>(Enumerable.Range(1, 1000).Select(value => this.factory.CreatePoint(value, value, value)));
        }

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void RStarTreeConstructorTest()
        {
            // default values
            RStarTree tree = new RStarTree();

            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(8);
            tree.MaxChildren.ShouldBe(12);

            // min. 2, max. 4 children
            tree = new RStarTree(2, 4);

            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(2);
            tree.MaxChildren.ShouldBe(4);

            // min. 10, max. 100 children
            tree = new RStarTree(10, 100);

            tree.Height.ShouldBe(0);
            tree.NumberOfGeometries.ShouldBe(0);
            tree.MinChildren.ShouldBe(10);
            tree.MaxChildren.ShouldBe(100);

            // exceptions
            Should.Throw<ArgumentOutOfRangeException>(() => new RStarTree(0, 1));
            Should.Throw<ArgumentOutOfRangeException>(() => new RStarTree(1, 1));
        }

        /// <summary>
        /// Tests the <see cref="Add" /> method.
        /// </summary>
        [Test]
        public void RStarTreeAddTest()
        {
            RStarTree tree = new RStarTree(2, 3);

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
            tree = new RStarTree(2, 3);
            tree.Add(this.geometries);

            tree.NumberOfGeometries.ShouldBe(this.geometries.Count);

            // exceptions
            Should.Throw<ArgumentNullException>(() => tree.Add((IGeometry)null));
            Should.Throw<ArgumentNullException>(() => tree.Add((IEnumerable<IGeometry>)null));
        }
    }
}