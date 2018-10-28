// <copyright file="RedBlackTreeTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="RedBlackTree{TKey, TValue}" /> class.
    /// </summary>
    [TestFixture]
    public class RedBlackTreeTest
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
        /// Tests the constructor of the <see cref="RedBlackTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void RedBlackTreeConstructorTest()
        {
            // no parameters
            RedBlackTree<Int32, String> tree = new RedBlackTree<Int32, String>();
            tree.Count.ShouldBe(0);
            tree.Height.ShouldBe(-1);
            tree.Comparer.ShouldBe(Comparer<Int32>.Default);

            // comparer parameter
            tree = new RedBlackTree<Int32, String>(Comparer<Int32>.Default);
            tree.Count.ShouldBe(0);
            tree.Height.ShouldBe(-1);
            tree.Comparer.ShouldBe(Comparer<Int32>.Default);

            tree = new RedBlackTree<Int32, String>(null);
            tree.Comparer.ShouldBe(Comparer<Int32>.Default);
        }

        /// <summary>
        /// Tests the <see cref="RedBlackTree{TKey, TValue}.Insert(TKey, TValue)" /> method.
        /// </summary>
        [Test]
        public void RedBlackTreeInsertTest()
        {
            RedBlackTree<Int32, String> tree = new RedBlackTree<Int32, String>();
            String value;

            // insert elements
            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Insert(keyValuePair.Key, keyValuePair.Value);
                tree.Height.ShouldBeLessThanOrEqualTo(5); // the maximum height of a Red Black tree is 2*lg(n+1)
            }

            tree.Count.ShouldBe(this.values.Length);
            tree.Height.ShouldBeLessThanOrEqualTo(5);

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Contains(keyValuePair.Key).ShouldBeTrue();
                tree.Search(keyValuePair.Key).ShouldBe(keyValuePair.Value);

                tree.TrySearch(keyValuePair.Key, out value).ShouldBeTrue();
                value.ShouldBe(keyValuePair.Value);
            }

            // exceptions
            Should.Throw<ArgumentException>(() => tree.Search(-10));
            Should.Throw<ArgumentException>(() =>
            {
                tree.Insert(0, String.Empty);
                tree.Insert(0, String.Empty);
            });

            RedBlackTree<Object, String> objectTree = new RedBlackTree<Object, String>();
            Should.Throw<ArgumentNullException>(() => objectTree.Insert(null, null));
            Should.Throw<ArgumentNullException>(() => objectTree.Contains(null));
            Should.Throw<ArgumentNullException>(() => objectTree.TrySearch(null, out value));
        }

        /// <summary>
        /// Tests the <see cref="RedBlackTree{TKey, TValue}.Remove(TKey)" /> method.
        /// </summary>
        [Test]
        public void RedBlackTreeRemoveTest()
        {
            // inserting and then removing elements
            RedBlackTree<Int32, String> tree = new RedBlackTree<Int32, String>();
            String value;

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Insert(keyValuePair.Key, keyValuePair.Value);
                tree.Height.ShouldBeLessThanOrEqualTo(20);
            }

            for (Int32 number = -1000; number < 1000; number++)
            {
                tree.Remove(number).ShouldBe(this.values.Select(keyValuePair => keyValuePair.Key).Contains(number));
                tree.Height.ShouldBeLessThanOrEqualTo(20);
            }

            tree.Height.ShouldBe(-1);
            tree.Count.ShouldBe(0);

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Insert(keyValuePair.Key, keyValuePair.Value);
                tree.Height.ShouldBeLessThanOrEqualTo(20);
            }

            tree.Clear();

            tree.Height.ShouldBe(-1);
            tree.Count.ShouldBe(0);

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Contains(keyValuePair.Key).ShouldBeFalse();
                tree.TrySearch(keyValuePair.Key, out value).ShouldBeFalse();
            }

            // exceptions
            RedBlackTree<Object, String> objectTree = new RedBlackTree<Object, String>();
            Should.Throw<ArgumentNullException>(() => objectTree.Remove(null));
        }

        /// <summary>
        /// Test tree enumeration of the <see cref="RedBlackTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void RedBlackTreeEnumeratorTest()
        {
            // enumerator

            RedBlackTree<Int32, String> tree = new RedBlackTree<Int32, String>();

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

            tree = new RedBlackTree<Int32, String>();

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