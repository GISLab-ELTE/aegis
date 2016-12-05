// <copyright file="BinarySearchTreeTest.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Tests.Collections.SearchTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Collections;
    using ELTE.AEGIS.Collections.SearchTree;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="BinarySearchTree{TKey, TValue}" /> class.
    /// </summary>
    [TestFixture]
    public class BinarySearchTreeTest
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
            this.values = Collection.GenerateNumbers(-1000, 1000, 20).Select(value => new KeyValuePair<Int32, String>(value, value.ToString())).ToArray();
        }

        

        

        /// <summary>
        /// Tests the constructor of the <see cref="BinarySearchTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void BinarySearchTreeConstructorTest()
        {
            // no parameters

            BinarySearchTree<Int32, String> tree = new BinarySearchTree<Int32, String>();

            tree.Count.ShouldBe(0);
            tree.Height.ShouldBe(-1);
            tree.Comparer.ShouldBe(Comparer<Int32>.Default);

            // comparer parameter

            tree = new BinarySearchTree<Int32, String>(Comparer<Int32>.Default);

            tree.Count.ShouldBe(0);
            tree.Height.ShouldBe(-1);
            tree.Comparer.ShouldBe(Comparer<Int32>.Default);

            tree = new BinarySearchTree<Int32, String>(null);

            tree.Comparer.ShouldBe(Comparer<Int32>.Default);
        }

        /// <summary>
        /// Tests tree operations of the <see cref="BinarySearchTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void BinarySearchTreeOperationsTest()
        {
            String value;
            BinarySearchTree<Int32, String> tree = new BinarySearchTree<Int32, String>();

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

            BinarySearchTree<Object, String> objectTree = new BinarySearchTree<Object, String>();

            Should.Throw<ArgumentNullException>(() => objectTree.Insert(null, null));
            Should.Throw<ArgumentNullException>(() => objectTree.Contains(null));
            Should.Throw<ArgumentNullException>(() => objectTree.TrySearch(null, out value));
            Should.Throw<ArgumentNullException>(() => objectTree.Remove(null));
        }

        /// <summary>
        /// Test tree height of the <see cref="BinarySearchTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void BinarySearchTreeHeightTest()
        {
            // one sided tree

            BinarySearchTree<Int32, String> tree = new BinarySearchTree<Int32, String>();

            foreach (KeyValuePair<Int32, String> keyValuePair in this.values)
            {
                tree.Insert(keyValuePair.Key, keyValuePair.Value);
            }

            tree.Height.ShouldBe(19);
            tree.Count.ShouldBe(20);

            // two sided tree

            tree = new BinarySearchTree<Int32, String>();

            tree.Insert(500, "500");
            for (Int32 value = 1; value < 500; value++)
            {
                tree.Insert(value, value.ToString());
                tree.Insert(value + 500, value.ToString());
            }

            tree.Height.ShouldBe(499);
            tree.Count.ShouldBe(999);
        }

        /// <summary>
        /// Test tree enumeration of the <see cref="BinarySearchTree{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void BinarySearchTreeEnumeratorTest()
        {
            // enumerator

            BinarySearchTree<Int32, String> tree = new BinarySearchTree<Int32, String>();

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

            tree.Count.ShouldBe(count);

            // search tree enumerator

            tree = new BinarySearchTree<Int32, String>();

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
