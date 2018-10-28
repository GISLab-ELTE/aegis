// <copyright file="DisjointSetForestTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Collections;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="DisjointSetForest{T}" /> class.
    /// </summary>
    [TestFixture]
    public class DisjointSetForestTest
    {
        /// <summary>
        /// The array of values that are inserted into the DisjointSetForest.
        /// </summary>
        private Int32[] values;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.values = Collection.GenerateNumbers(1000, -1000, 20).ToArray();
        }

        /// <summary>
        /// Tests the constructor of the <see cref="DisjointSetForest{T}" /> class.
        /// </summary>
        [Test]
        public void DisjointSetForestConstructorTest()
        {
            // no parameters
            DisjointSetForest<Int32> disjointSet = new DisjointSetForest<Int32>();

            disjointSet.Count.ShouldBe(0);
            disjointSet.SetCount.ShouldBe(0);

            // capacity parameter

            disjointSet = new DisjointSetForest<Int32>(100);

            disjointSet.Count.ShouldBe(0);
            disjointSet.SetCount.ShouldBe(0);

            // source parameter

            disjointSet = new DisjointSetForest<Int32>(this.values);

            disjointSet.Count.ShouldBe(this.values.Length);
            disjointSet.SetCount.ShouldBe(this.values.Length);

            // exceptions

            Should.Throw<ArgumentOutOfRangeException>(() => disjointSet = new DisjointSetForest<Int32>(-1));
            Should.Throw<ArgumentNullException>(() => disjointSet = new DisjointSetForest<Int32>(null));
        }

        /// <summary>
        /// Tests the <see cref="DisjointSetForest{T}.MakeSet(T)" /> method.
        /// </summary>
        [Test]
        public void DisjointSetForestMakeSetTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();
            DisjointSetForest<String> disjointSetString = new DisjointSetForest<String>();

            foreach (Int32 value in this.values)
            {
                disjointSetInt.MakeSet(value);
                disjointSetString.MakeSet(value.ToString());
            }

            // testing if added correctly

            disjointSetInt.Count.ShouldBe(this.values.Length);
            disjointSetInt.SetCount.ShouldBe(this.values.Length);

            disjointSetString.Count.ShouldBe(this.values.Length);
            disjointSetString.SetCount.ShouldBe(this.values.Length);

            foreach (Int32 value in this.values)
            {
                value.ShouldBe(disjointSetInt.FindSet(value));
                disjointSetString.FindSet(value.ToString()).ShouldBe(value.ToString());
            }

            // testing element uniqueness
            for (Int32 i = 0; i < 20; i++)
            {
                disjointSetInt.MakeSet(this.values[0]);
                disjointSetString.MakeSet(this.values[0].ToString());
            }

            disjointSetInt.Count.ShouldBe(this.values.Length);
            disjointSetInt.SetCount.ShouldBe(this.values.Length);

            disjointSetString.Count.ShouldBe(this.values.Length);
            disjointSetString.SetCount.ShouldBe(this.values.Length);

            // exceptions

            Should.Throw<ArgumentNullException>(() => new DisjointSetForest<String>().MakeSet(null));
        }

        /// <summary>
        /// Tests the <see cref="DisjointSetForest{T}.FindSet(T)" /> method.
        /// </summary>
        [Test]
        public void DisjointSetForestFindTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();
            DisjointSetForest<String> disjointSetString = new DisjointSetForest<String>();

            foreach (Int32 value in this.values)
            {
                disjointSetInt.MakeSet(value);
                disjointSetString.MakeSet(value.ToString());
            }

            foreach (Int32 value in this.values)
            {
                value.ShouldBe(disjointSetInt.FindSet(value));
                disjointSetString.FindSet(value.ToString()).ShouldBe(value.ToString());
            }

            // exceptions

            Should.Throw<ArgumentNullException>(() => disjointSetString.FindSet(null));
            Should.Throw<ArgumentException>(() => disjointSetInt.FindSet(10000));
            Should.Throw<ArgumentException>(() => disjointSetString.FindSet(10000.ToString()));
        }

        /// <summary>
        /// Tests the <see cref="DisjointSetForest{T}.JoinSets(T, T)" /> method.
        /// </summary>
        [Test]
        public void DisjointSetForestJoinSetsTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();
            DisjointSetForest<String> disjointSetString = new DisjointSetForest<String>();

            foreach (Int32 value in this.values)
            {
                disjointSetInt.MakeSet(value);
                disjointSetString.MakeSet(value.ToString());
            }

            for (Int32 i = 0; i < this.values.Length; i = i + 2)
            {
                disjointSetInt.JoinSets(this.values[i], this.values[i + 1]);
                disjointSetString.JoinSets(this.values[i].ToString(), this.values[i + 1].ToString());
            }

            // testing representatives

            for (Int32 i = 0; i < this.values.Length; i = i + 2)
            {
                disjointSetInt.FindSet(this.values[i + 1]).ShouldBe(disjointSetInt.FindSet(this.values[i]));
                disjointSetString.FindSet(this.values[i + 1].ToString()).ShouldBe(disjointSetString.FindSet(this.values[i].ToString()));
            }

            // testing setCounts

            disjointSetInt.SetCount.ShouldBe(this.values.Length / 2);
            disjointSetString.SetCount.ShouldBe(this.values.Length / 2);

            disjointSetInt.Count.ShouldBe(this.values.Length);
            disjointSetString.Count.ShouldBe(this.values.Length);

            // exceptions

            Should.Throw<ArgumentException>(() => disjointSetInt.JoinSets(this.values[0], 1000));
            Should.Throw<ArgumentException>(() => disjointSetInt.JoinSets(1000, this.values[0]));
            Should.Throw<ArgumentException>(() => disjointSetInt.JoinSets(1000, 101));

            Should.Throw<ArgumentException>(() => disjointSetString.JoinSets(this.values[0].ToString(), 1000.ToString()));
            Should.Throw<ArgumentException>(() => disjointSetString.JoinSets(1000.ToString(), this.values[0].ToString()));
            Should.Throw<ArgumentException>(() => disjointSetString.JoinSets(1000.ToString(), 101.ToString()));

            Should.Throw<ArgumentNullException>(() => disjointSetString.JoinSets(this.values[0].ToString(), null));
            Should.Throw<ArgumentNullException>(() => disjointSetString.JoinSets(null, this.values[0].ToString()));
            Should.Throw<ArgumentNullException>(() => disjointSetString.JoinSets(null, null));
        }

        /// <summary>
        /// Tests the <see cref="DisjointSetForest{T}.Clear()" /> method.
        /// </summary>
        [Test]
        public void DisjointSetForestClearTest()
        {
            // empty
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>();

            disjointSetInt.Clear();

            disjointSetInt.Count.ShouldBe(0);
            disjointSetInt.SetCount.ShouldBe(0);

            // filled
            disjointSetInt = new DisjointSetForest<Int32>(this.values);

            disjointSetInt.Clear();

            disjointSetInt.Count.ShouldBe(0);
            disjointSetInt.SetCount.ShouldBe(0);
        }

        /// <summary>
        /// Tests the enumerator of the <see cref="DisjointSetForest{T}" /> class.
        /// </summary>
        [Test]
        public void DisjointSetForestEnumeratorTest()
        {
            DisjointSetForest<Int32> disjointSetInt = new DisjointSetForest<Int32>(this.values);
            IEnumerator enumerator = this.values.GetEnumerator();
            IEnumerator<Int32> disjointSetEnumerator = disjointSetInt.GetEnumerator();
            while (enumerator.MoveNext())
            {
                disjointSetEnumerator.MoveNext().ShouldBeTrue();
                disjointSetEnumerator.Current.ShouldBe(enumerator.Current);
            }
        }
    }
}
