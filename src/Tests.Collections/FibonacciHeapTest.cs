// <copyright file="FibonacciHeapTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="FibonacciHeap{TKey, TValue}" /> class.
    /// </summary>
    [TestFixture]
    public class FibonacciHeapTest
    {
        

        /// <summary>
        /// The source array.
        /// </summary>
        private KeyValuePair<Int32, String>[] values;

        

        

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.values = Collection.GenerateNumbers(-1000, 1000, 200).Distinct().Select(value => new KeyValuePair<Int32, String>(value, value.ToString())).ToArray();
        }

        

        

        /// <summary>
        /// Tests the constructor of the <see cref="FibonacciHeap{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void HeapConstructorTest()
        {
            // no parameters

            FibonacciHeap<Int32, String> heap = new FibonacciHeap<Int32, String>();

            heap.Count.ShouldBe(0);
            heap.Comparer.ShouldBe(Comparer<Int32>.Default);

            // comparer parameter

            heap = new FibonacciHeap<Int32, String>(Comparer<Int32>.Default);

            heap.Count.ShouldBe(0);
            heap.Comparer.ShouldBe(Comparer<Int32>.Default);

            heap = new FibonacciHeap<Int32, String>((IComparer<Int32>)null);

            heap.Comparer.ShouldBe(Comparer<Int32>.Default);

            // source parameter

            heap = new FibonacciHeap<Int32, String>(this.values);

            heap.Count.ShouldBe(this.values.Length);
            heap.Comparer.ShouldBe(Comparer<Int32>.Default);

            // source and comparer parameter

            heap = new FibonacciHeap<Int32, String>(this.values, Comparer<Int32>.Default);

            heap.Count.ShouldBe(this.values.Length);
            heap.Comparer.ShouldBe(Comparer<Int32>.Default);

            heap = new FibonacciHeap<Int32, String>(this.values, null);

            heap.Comparer.ShouldBe(Comparer<Int32>.Default);

            // exceptions

            Should.Throw<ArgumentNullException>(() => heap = new FibonacciHeap<Int32, String>((IEnumerable<KeyValuePair<Int32, String>>)null));
            Should.Throw<ArgumentNullException>(() => heap = new FibonacciHeap<Int32, String>(null, Comparer<Int32>.Default));
        }

        /// <summary>
        /// Tests the <see cref="FibonacciHeap{TKey, TValue}.Peek" /> property.
        /// </summary>
        [Test]
        public void FibonacciHeapPeekTest()
        {
            // small heap

            FibonacciHeap<Int32, String> heap = new FibonacciHeap<Int32, String>();

            heap.Insert(0, "0");
            heap.Peek.ShouldBe("0");

            heap.Insert(1, "1");
            heap.Peek.ShouldBe("0");

            heap.Insert(-1, "-1");
            heap.Peek.ShouldBe("-1");

            // large heap

            heap = new FibonacciHeap<Int32, String>();

            for (Int32 index = 0; index < this.values.Length; index++)
            {
                heap.Insert(this.values[index].Key, this.values[index].Value);

                heap.Peek.ShouldBe(heap.Min(item => item.Key).ToString());
                heap.Peek.ShouldBe(this.values.Take(index + 1).Min(item => item.Key).ToString());
            }

            // exceptions

            heap = new FibonacciHeap<Int32, String>();
            String peek;
            Should.Throw<InvalidOperationException>(() => peek = heap.Peek);
        }

        /// <summary>
        /// Tests the <see cref="FibonacciHeap{TKey, TValue}.Insert(TKey, TValue)" /> method.
        /// </summary>
        [Test]
        public void FibonacciHeapInsertTest()
        {
            FibonacciHeap<Int32, String> heap = new FibonacciHeap<Int32, String>();

            heap.Insert(this.values[0].Key, this.values[0].Value);

            heap.Peek.ShouldBe(this.values[0].Value);
            heap.Count.ShouldBe(1);

            heap.Insert(this.values[1].Key, this.values[1].Value);
            heap.Insert(this.values[2].Key, this.values[2].Value);
            heap.Insert(this.values[3].Key, this.values[3].Value);

            heap.Count.ShouldBe(4);

            heap.Insert(this.values[4].Key, this.values[4].Value);
            heap.Count.ShouldBe(5);

            // exceptions

            Should.Throw<ArgumentNullException>(() => new FibonacciHeap<String, String>().Insert(null, null));
        }

        /// <summary>
        /// Tests the <see cref="FibonacciHeap{TKey, TValue}.RemovePeek()" /> method.
        /// </summary>
        [Test]
        public void FibonacciHeapRemovePeekTest()
        {
            FibonacciHeap<Int32, String> heap = new FibonacciHeap<Int32, String>();

            for (Int32 index = 0; index < this.values.Length; index++)
            {
                heap.Insert(this.values[index].Key, this.values[index].Value);

                heap.Peek.ShouldBe(this.values.Take(index + 1).Select(item => item.Key).Min().ToString());
            }

            while (heap.Count > 0)
            {
                String peek = heap.Peek;
                heap.RemovePeek().ShouldBe(peek);
                this.values.Select(item => item.Value).Contains(peek).ShouldBeTrue();
            }

            // exceptions

            Should.Throw<InvalidOperationException>(() => new FibonacciHeap<String, String>().RemovePeek());
        }

        /// <summary>
        /// Tests the <see cref="FibonacciHeap{TKey, TValue}.Contains(TKey)" /> method.
        /// </summary>
        [Test]
        public void FibonacciHeapContainsTest()
        {
            FibonacciHeap<Int32, String> heap = new FibonacciHeap<Int32, String>(this.values);

            for (Int32 number = -1000; number < 1000; number++)
            {
                heap.Contains(number).ShouldBe(this.values.Contains(new KeyValuePair<Int32, String>(number, number.ToString())));
            }
        }

        /// <summary>
        /// Tests the <see cref="FibonacciHeap{TKey, TValue}.Clear()" /> method.
        /// </summary>
        [Test]
        public void FibonacciHeapClearTest()
        {
            // empty heap

            FibonacciHeap<Int32, String> heap = new FibonacciHeap<Int32, String>();
            heap.Clear();

            heap.Count.ShouldBe(0);

            // filled heap

            heap = new FibonacciHeap<Int32, String>(this.values);
            heap.Clear();

            heap.Count.ShouldBe(0);
        }

        /// <summary>
        /// Tests the enumerator of the <see cref="FibonacciHeap{TKey, TValue}" /> class.
        /// </summary>
        [Test]
        public void FibonacciHeapEnumeratorTest()
        {
            FibonacciHeap<Int32, String> heap = new FibonacciHeap<Int32, String>(this.values.OrderBy(value => value.Key));

            IEnumerator enumerator = this.values.OrderBy(value => value.Key).GetEnumerator();
            IEnumerator<KeyValuePair<Int32, String>> heapEnumerator = heap.GetEnumerator();
            IEnumerator heapCollectionEnumerator = (heap as IEnumerable).GetEnumerator();

            while (enumerator.MoveNext())
            {
                heapEnumerator.MoveNext().ShouldBeTrue();
                heapCollectionEnumerator.MoveNext().ShouldBeTrue();

                heapEnumerator.Current.ShouldBe(enumerator.Current);
                heapEnumerator.Current.ShouldBe(heapCollectionEnumerator.Current);
            }

            // exceptions

            heap.Insert(0, "0");

            Should.Throw<InvalidOperationException>(() => heapEnumerator.MoveNext());
        }

        
    }
}
