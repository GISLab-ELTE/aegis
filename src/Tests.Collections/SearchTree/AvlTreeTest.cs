// <copyright file="AvlTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Collections.SearchTrees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Collections;
    using AEGIS.Collections.SearchTrees;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="AvlTree{TKey, TValue}" /> class.
    /// </summary>
    [TestFixture]
    public class AvlTreeTest
    {
        /// <summary>
        /// The array of integer values.
        /// </summary>
        private KeyValuePair<Int32, String>[] values;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.values = Collection.GenerateNumbers(1000, -1000, 20).Select(value => new KeyValuePair<Int32, String>(value, value.ToString())).ToArray();
        }

        /// <summary>
        /// Tests the constructor of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void AvlTreeConstructorTest()
        {
            // no parameters

            AvlTree<Int32, String> tree = new AvlTree<Int32, String>();

            tree.Count.ShouldBe(0);
            tree.Height.ShouldBe(-1);
            tree.Comparer.ShouldBe(Comparer<Int32>.Default);

            // comparer parameter

            tree = new AvlTree<Int32, String>(Comparer<Int32>.Default);

            tree.Count.ShouldBe(0);
            tree.Height.ShouldBe(-1);
            tree.Comparer.ShouldBe(Comparer<Int32>.Default);

            tree = new AvlTree<Int32, String>(null);

            tree.Comparer.ShouldBe(Comparer<Int32>.Default);
        }

        /// <summary>
        /// Tests tree operations of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void AvlTreeOperationsTest()
        {
            String value;
            AvlTree<Int32, String> tree = new AvlTree<Int32, String>();

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Insert(keyValuePair.Key, keyValuePair.Value);
            }

            tree.Count.ShouldBe(this.values.Length);

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Contains(keyValuePair.Key).ShouldBeTrue();
                tree.Search(keyValuePair.Key).ShouldBe(keyValuePair.Value);

                tree.TrySearch(keyValuePair.Key, out value).ShouldBeTrue();
                value.ShouldBe(keyValuePair.Value);
            }

            for (Int32 number = -1000; number < 1000; number++)
            {
                tree.Remove(number).ShouldBe(this.values.Select(keyValuePair => keyValuePair.Key).Contains(number));
            }

            tree.Count.ShouldBe(0);

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Insert(keyValuePair.Key, keyValuePair.Value);
            }

            tree.Clear();

            tree.Count.ShouldBe(0);

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Contains(keyValuePair.Key).ShouldBeFalse();
                tree.TrySearch(keyValuePair.Key, out value).ShouldBeFalse();
            }

            // exceptions

            Should.Throw<ArgumentException>(() => tree.Search(0));
            Should.Throw<ArgumentException>(() =>
            {
                tree.Insert(0, String.Empty);
                tree.Insert(0, String.Empty);
            });

            AvlTree<Object, String> objectTree = new AvlTree<Object, String>();

            Should.Throw<ArgumentNullException>(() => objectTree.Insert(null, null));
            Should.Throw<ArgumentNullException>(() => objectTree.Contains(null));
            Should.Throw<ArgumentNullException>(() => objectTree.TrySearch(null, out value));
            Should.Throw<ArgumentNullException>(() => objectTree.Remove(null));
        }

        /// <summary>
        /// Test tree balance of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void AvlTreeBalanceTest()
        {
            // ++,+

            AvlTree<Int32, String> tree = new AvlTree<Int32, String>();
            tree.Insert(1, "1");
            tree.Insert(2, "2");
            tree.Insert(3, "3");

            tree.Select(element => element.Key).ShouldBe(Enumerable.Range(1, 3));
            tree.Height.ShouldBe(1);

            // --,-

            tree = new AvlTree<Int32, String>();
            tree.Insert(3, "1");
            tree.Insert(2, "2");
            tree.Insert(1, "3");

            tree.Select(element => element.Key).ShouldBe(Enumerable.Range(1, 3));
            tree.Height.ShouldBe(1);

            // ++,-

            tree = new AvlTree<Int32, String>();
            tree.Insert(4, "1");
            tree.Insert(2, "2");
            tree.Insert(9, "3");
            tree.Insert(6, "4");
            tree.Insert(3, "5");
            tree.Insert(10, "6");
            tree.Insert(8, "7");
            tree.Insert(11, "8");
            tree.Insert(1, "9");
            tree.Insert(5, "10");
            tree.Insert(7, "11");

            tree.Select(element => element.Key).ShouldBe(Enumerable.Range(1, 11));
            tree.Height.ShouldBe(3);

            // --,+

            tree = new AvlTree<Int32, String>();
            tree.Insert(8, "1");
            tree.Insert(10, "2");
            tree.Insert(3, "3");
            tree.Insert(5, "4");
            tree.Insert(11, "5");
            tree.Insert(9, "6");
            tree.Insert(1, "7");
            tree.Insert(4, "8");
            tree.Insert(2, "9");
            tree.Insert(6, "10");
            tree.Insert(7, "11");

            tree.Select(element => element.Key).ShouldBe(Enumerable.Range(1, 11));
            tree.Height.ShouldBe(3);

            // ++,=

            tree = new AvlTree<Int32, String>();
            tree.Insert(4, "1");
            tree.Insert(2, "2");
            tree.Insert(5, "3");
            tree.Insert(1, "4");
            tree.Insert(3, "5");
            tree.Remove(5);

            tree.Select(element => element.Key).ShouldBe(Enumerable.Range(1, 4));
            tree.Height.ShouldBe(2);

            // --,=

            tree = new AvlTree<Int32, String>();
            tree.Insert(2, "1");
            tree.Insert(1, "2");
            tree.Insert(4, "3");
            tree.Insert(5, "4");
            tree.Insert(3, "5");
            tree.Remove(1);

            tree.Select(element => element.Key).ShouldBe(Enumerable.Range(2, 4));
            tree.Height.ShouldBe(2);
        }

        /// <summary>
        /// Tests tree enumeration of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void AvlTreeEnumeratorTest()
        {
            // enumerator

            AvlTree<Int32, String> tree = new AvlTree<Int32, String>();

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                if (!tree.Contains(keyValuePair.Key))
                    tree.Insert(keyValuePair.Key, keyValuePair.Value);
            }

            Int32 prevKey = this.values.Min(keyValuePair => keyValuePair.Key) - 1;
            Int32 count = 0;
            foreach (KeyValuePair<Int32, String> element in tree)
            {
                element.Key.ShouldBeGreaterThan(prevKey);
                prevKey = element.Key;
                count++;
            }

            count.ShouldBe(tree.Count);

            // search tree enumerator

            tree = new AvlTree<Int32, String>();

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                if (!tree.Contains(keyValuePair.Key))
                    tree.Insert(keyValuePair.Key, keyValuePair.Value);
            }

            ISearchTreeEnumerator<Int32, String> enumerator = tree.GetTreeEnumerator();

            List<Int32> forwardList = new List<Int32>();
            List<Int32> backwardList = new List<Int32>();

            if (enumerator.MoveMin())
            {
                do
                {
                    forwardList.Add(enumerator.Current.Key);
                }
                while (enumerator.MoveNext());
            }

            if (enumerator.MoveMax())
            {
                do
                {
                    backwardList.Add(enumerator.Current.Key);
                }
                while (enumerator.MovePrev());
            }

            backwardList.Reverse();
            forwardList.ShouldBe(backwardList);
        }
    }
}
