// <copyright file="SparseArrayTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Collections;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="SparseArray{T}" /> class.
    /// </summary>
    [TestFixture]
    public class SparseArrayTest
    {
        

        /// <summary>
        /// The array of values that are inserted into the sparse array.
        /// </summary>
        private Int32[] values;

        

        

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.values = Collection.GenerateNumbers(0, 1000, 200).ToArray();
        }

        

        

        /// <summary>
        /// Tests the constructor of the <see cref="SparseArray{T}" /> class.
        /// </summary>
        [Test]
        public void SparseArrayConstructorTest()
        {
            // no parameters
            SparseArray<Int32> array = new SparseArray<Int32>(0);

            array.IsReadOnly.ShouldBeFalse();
            array.Length.ShouldBe(0);
            array.LongLength.ShouldBe(0);
            array.Count.ShouldBe(0);

            // capacity parameter
            array = new SparseArray<Int32>(1000);

            array.Length.ShouldBe(1000);
            array.LongLength.ShouldBe(1000);
            array.ActualCount.ShouldBe(0);

            // source parameter without zeros
            array = new SparseArray<Int32>(Enumerable.Range(1, 1000));

            array.Length.ShouldBe(1000);
            array.LongLength.ShouldBe(1000);
            array.ActualCount.ShouldBe(1000);

            for (Int32 index = 0; index < array.Length; index++)
            {
                array[index].ShouldBe(index + 1);
            }

            // source parameter with zeros
            array = new SparseArray<Int32>(this.values);

            array.ActualCount.ShouldBeLessThan(this.values.Length);
            array.Length.ShouldBe(this.values.Length);

            for (Int32 index = 0; index < array.Length; index++)
            {
                array[index].ShouldBe(this.values[index]);
            }

            // source parameter with only zeros
            array = new SparseArray<Int32>(Enumerable.Repeat(0, 10));

            array.Length.ShouldBe(10);
            array.ActualCount.ShouldBe(0);

            for (Int32 index = 0; index < array.Count; index++)
            {
                array[index].ShouldBe(0);
            }

            // exceptions
            Should.Throw<ArgumentOutOfRangeException>(() => array = new SparseArray<Int32>(-1));
            Should.Throw<ArgumentNullException>(() => array = new SparseArray<Int32>(null));
        }

        /// <summary>
        /// Tests the indexer property of the <see cref="SparseArray{T}" /> class.
        /// </summary>
        [Test]
        public void SparseArrayItemTest()
        {
            // get
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);

            for (Int32 index = 0; index < array.Count; index++)
            {
                array[index].ShouldBe(this.values[index]);
                array[(Int64)index].ShouldBe(this.values[index]);
            }

            // set
            array = new SparseArray<Int32>(Enumerable.Repeat(0, this.values.Length));

            for (Int32 index = 0; index < this.values.Length; index++)
                array[index] = this.values[index];

            for (Int32 index = 0; index < array.Count; index++)
                array[index].ShouldBe(this.values[index]);

            for (Int64 index = 0; index < this.values.Length; index++)
                array[index] = this.values[index];

            for (Int32 index = 0; index < array.Count; index++)
                array[index].ShouldBe(this.values[index]);

            // exceptions
            Should.Throw<ArgumentOutOfRangeException>(() => array[-1] = this.values[0]);
            Should.Throw<ArgumentOutOfRangeException>(() => this.values[0] = array[-1]);
            Should.Throw<ArgumentOutOfRangeException>(() => array[this.values.Length] = this.values[0]);
            Should.Throw<ArgumentOutOfRangeException>(() => this.values[0] = array[this.values.Length]);
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.Add(T)" /> method.
        /// </summary>
        [Test]
        public void SparseArrayAddTest()
        {
            // empty array
            SparseArray<Int32> array = new SparseArray<Int32>(0);

            for (Int32 index = 0; index < this.values.Length; index++)
                array.Add(this.values[index]);

            array.Length.ShouldBe(this.values.Length);

            for (Int32 index = 0; index < array.Count; index++)
                array[index].ShouldBe(this.values[index]);

            // filled array
            array = new SparseArray<Int32>(Enumerable.Repeat(0, 10));

            for (Int32 index = 0; index < this.values.Length; index++)
                array.Add(this.values[index]);

            array.Length.ShouldBe(this.values.Length + 10);

            for (Int32 index = 10; index < array.Count; index++)
                array[index].ShouldBe(this.values[index - 10]);
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.Clear()" /> method.
        /// </summary>
        [Test]
        public void SparseArrayClearTest()
        {
            // empty array
            SparseArray<Int32> array = new SparseArray<Int32>(0);
            array.Clear();

            array.ActualCount.ShouldBe(0);
            array.Count.ShouldBe(0);
            array.Length.ShouldBe(0);

            // filled array
            array = new SparseArray<Int32>(this.values);
            array.Clear();

            array.ActualCount.ShouldBe(0);
            array.Count.ShouldBe(this.values.Length);
            array.Length.ShouldBe(this.values.Length);
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.Contains(T)" /> method.
        /// </summary>
        [Test]
        public void SparseArrayContainsTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);

            array.Contains(0).ShouldBeTrue();
            array.Contains(5).ShouldBeTrue();
            array.Contains(10).ShouldBeTrue();
            array.Contains(100).ShouldBeTrue();
            array.Contains(-10).ShouldBeFalse();
            array.Contains(1000).ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.CopyTo(T[], Int32)" /> method.
        /// </summary>
        [Test]
        public void SparseArrayCopyToTest()
        {
            Int32[] result = new Int32[this.values.Length * 2];

            // zero start index
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);
            array.CopyTo(result, 0);

            for (Int32 index = 0; index < array.Count; index++)
                array[index].ShouldBe(result[index]);

            // greater start index
            array.CopyTo(result, this.values.Length);

            for (Int32 index = 0; index < array.Count; index++)
                array[index].ShouldBe(result[this.values.Length + index]);

            // exceptions
            Should.Throw<ArgumentNullException>(() => array.CopyTo(null, 0));
            Should.Throw<ArgumentOutOfRangeException>(() => array.CopyTo(result, -1));
            Should.Throw<ArgumentException>(() => array.CopyTo(result, this.values.Length * 2));
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.Remove(T)" /> method.
        /// </summary>
        [Test]
        public void SparseArrayRemoveTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);
            array.Length.ShouldBe(this.values.Length);

            array.Remove(-1).ShouldBeFalse();
            array.Length.ShouldBe(this.values.Length);

            array.Remove(0).ShouldBeTrue();
            array.Length.ShouldBe(this.values.Length - 1);

            array.Remove(0).ShouldBeFalse();
            array.Length.ShouldBe(this.values.Length - 1);

            array.Remove(5).ShouldBeTrue();
            array.Length.ShouldBe(this.values.Length - 2);

            for (Int32 index = 0; index < array.Count; index++)
                array[index].ShouldBe(this.values[2 + index]);
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.IndexOf(T)" /> method.
        /// </summary>
        [Test]
        public void SparseArrayIndexOfTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);

            array.IndexOf(0).ShouldBe(0);
            array.IndexOf(5).ShouldBe(1);
            array.IndexOf(100).ShouldBe(20);
            array.IndexOf(-1).ShouldBe(-1);
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.Insert(Int64, T)" /> method.
        /// </summary>
        [Test]
        public void SparseArrayInsertTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);
            array.Insert(1, -1);
            array.Insert(3, -2);

            array.Length.ShouldBe(this.values.Length + 2);

            array[0].ShouldBe(0);
            array[1].ShouldBe(-1);
            array[2].ShouldBe(5);
            array[3].ShouldBe(-2);
            array[4].ShouldBe(10);
            array[5].ShouldBe(15);

            // exceptions
            Should.Throw<ArgumentOutOfRangeException>(() => array.Insert(-1, 1));
            Should.Throw<ArgumentOutOfRangeException>(() => array.Insert(array.Length, 1));
        }

        /// <summary>
        /// Tests the <see cref="SparseArray{T}.RemoveAt(Int64)" /> method.
        /// </summary>
        [Test]
        public void SparseArrayRemoveAtTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);
            array.RemoveAt(1);
            array.RemoveAt(1);
            array.RemoveAt(3);

            array.Length.ShouldBe(this.values.Length - 3);

            array[0].ShouldBe(0);
            array[1].ShouldBe(15);
            array[2].ShouldBe(20);
            array[3].ShouldBe(30);

            // exceptions
            Should.Throw<ArgumentOutOfRangeException>(() => array.RemoveAt(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => array.RemoveAt(array.Length));
        }

        /// <summary>
        /// Tests the enumerator of the <see cref="SparseArray{T}" /> class.
        /// </summary>
        [Test]
        public void SparseArrayEnumeratorTest()
        {
            SparseArray<Int32> array = new SparseArray<Int32>(this.values);

            IEnumerator enumerator = this.values.GetEnumerator();
            IEnumerator<Int32> arrayEnumerator = array.GetEnumerator();
            IEnumerator arrayCollectionEnumerator = (array as IEnumerable).GetEnumerator();

            while (enumerator.MoveNext())
            {
                arrayEnumerator.MoveNext().ShouldBeTrue();
                arrayCollectionEnumerator.MoveNext().ShouldBeTrue();

                arrayEnumerator.Current.ShouldBe(enumerator.Current);
                arrayEnumerator.Current.ShouldBe(arrayCollectionEnumerator.Current);
            }

            arrayEnumerator.Reset();
            arrayCollectionEnumerator.Reset();

            while (arrayEnumerator.MoveNext())
            {
                arrayCollectionEnumerator.MoveNext().ShouldBeTrue();
                arrayEnumerator.Current.ShouldBe(arrayCollectionEnumerator.Current);
            }

            // exceptions
            array.Add(0);

            Should.Throw<InvalidOperationException>(() => arrayEnumerator.MoveNext());
            Should.Throw<InvalidOperationException>(() => arrayEnumerator.Reset());
        }

        
    }
}
