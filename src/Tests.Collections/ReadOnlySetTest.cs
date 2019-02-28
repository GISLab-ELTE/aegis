// <copyright file="ReadOnlySetTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="ReadOnlySet{T}" /> class.
    /// </summary>
    [TestFixture]
    public class ReadOnlySetTest
    {
        /// <summary>
        /// The array of inner sets that are wrapped.
        /// </summary>
        private ISet<Int32>[] innerSets;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            IEnumerable<Int32> values = Collection.GenerateNumbers(1, 1000, 100);

            this.innerSets = new ISet<Int32>[]
            {
                new SortedSet<Int32>(),
                new SortedSet<Int32>(Enumerable.Range(1, 1000)),
                new SortedSet<Int32>(Enumerable.Repeat(1, 10)),
                new SortedSet<Int32>(values),
                new HashSet<Int32>(),
                new HashSet<Int32>(Enumerable.Range(1, 1000)),
                new HashSet<Int32>(Enumerable.Range(1, 10)),
                new HashSet<Int32>(values)
            };
        }

        /// <summary>
        /// Tests the constructor of the <see cref="ReadOnlySet{T}" /> class.
        /// </summary>
        [Test]
        public void ReadOnlySetConstructorTest()
        {
            foreach (ISet<Int32> set in this.innerSets)
            {
                ReadOnlySet<Int32> readOnlySet = new ReadOnlySet<Int32>(set);

                readOnlySet.IsReadOnly.ShouldBeTrue();
                readOnlySet.Count.ShouldBe(set.Count);
            }

            // exceptions

            Should.Throw<ArgumentNullException>(() => new ReadOnlySet<Int32>(null));
        }

        /// <summary>
        /// Tests supported interface methods of the <see cref="ReadOnlySet{T}" /> class.
        /// </summary>
        [Test]
        public void ReadOnlySetSupportedMethodsTest()
        {
            foreach (ISet<Int32> set in this.innerSets)
            {
                ReadOnlySet<Int32> readOnlySet = new ReadOnlySet<Int32>(set);

                readOnlySet.IsSubsetOf(this.innerSets[0]).ShouldBe(set.IsSubsetOf(this.innerSets[0]));
                readOnlySet.IsSupersetOf(this.innerSets[0]).ShouldBe(set.IsSupersetOf(this.innerSets[0]));
                readOnlySet.IsProperSupersetOf(this.innerSets[0]).ShouldBe(set.IsProperSupersetOf(this.innerSets[0]));
                readOnlySet.IsProperSubsetOf(this.innerSets[0]).ShouldBe(set.IsProperSubsetOf(this.innerSets[0]));
                readOnlySet.Overlaps(this.innerSets[0]).ShouldBe(set.Overlaps(this.innerSets[0]));
                readOnlySet.SetEquals(this.innerSets[0]).ShouldBe(set.SetEquals(this.innerSets[0]));

                readOnlySet.Contains(0).ShouldBe(set.Contains(0));

                Int32[] setValues = new Int32[set.Count], readOnlySetValues = new Int32[readOnlySet.Count];
                set.CopyTo(setValues, 0);
                readOnlySet.CopyTo(readOnlySetValues, 0);

                readOnlySetValues.ShouldBe(setValues);
            }
        }

        /// <summary>
        /// Tests the enumerator of the <see cref="ReadOnlySet{T}" /> class.
        /// </summary>
        [Test]
        public void ReadOnlySetEnumeratorTest()
        {
            // traversal

            foreach (ISet<Int32> set in this.innerSets)
            {
                ReadOnlySet<Int32> readOnlySet = new ReadOnlySet<Int32>(set);

                IEnumerator<Int32> enumerator = set.GetEnumerator();
                IEnumerator<Int32> readOnlyEnumerator = readOnlySet.GetEnumerator();
                IEnumerator readOnlyCollectionEnumerator = (readOnlySet as IEnumerable).GetEnumerator();

                while (enumerator.MoveNext())
                {
                    readOnlyEnumerator.MoveNext().ShouldBeTrue();
                    readOnlyCollectionEnumerator.MoveNext().ShouldBeTrue();

                    readOnlyEnumerator.Current.ShouldBe(enumerator.Current);
                    readOnlyEnumerator.Current.ShouldBe(readOnlyCollectionEnumerator.Current);
                }
            }
        }
    }
}
