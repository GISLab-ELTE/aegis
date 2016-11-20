// <copyright file="SparseArray.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Collections.Resources;

    /// <summary>
    /// Represents a sparse array.
    /// </summary>
    /// <typeparam name="T">The type of the value in the sparse array.</typeparam>
    /// <remarks>
    /// A sparse array is an array in which most of the elements have the same value (known as the default value—usually 0 or null).
    /// The occurrence of zero elements in a large array is inefficient for both computation and storage.
    /// An array in which there is a large number of zero elements is referred to as being sparse.
    /// </remarks>
    public class SparseArray<T> : IList<T>, IReadOnlyList<T>
    {
        #region Private fields

        /// <summary>
        /// The items of the sparse array.
        /// </summary>
        private Dictionary<Int64, T> items;

        /// <summary>
        /// The version of the array.
        /// </summary>
        private Int32 version;

        /// <summary>
        /// The number of elements in the sparse array.
        /// </summary>
        private Int64 length;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseArray{T}" /> class.
        /// </summary>
        /// <param name="length">The number of elements that the array can initially store.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The length is less than 0.</exception>
        public SparseArray(Int64 length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), Messages.LengthIsLessThan0);

            this.items = new Dictionary<Int64, T>();
            this.version = 0;
            this.length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseArray{T}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public SparseArray(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), Messages.CollectionIsNull);

            this.items = new Dictionary<Int64, T>();
            this.version = 0;

            IList<T> collectionList = collection as IList<T>;

            if (collectionList != null)
            {
                this.length = collectionList.Count;

                for (Int32 i = 0; i < collectionList.Count; i++)
                {
                    if (!AreEqual(collectionList[i], default(T)))
                        this.items.Add(i, collectionList[i]);
                }
            }
            else
            {
                Int32 count = 0;

                foreach (T item in collection)
                {
                    if (!AreEqual(item, default(T)))
                        this.items.Add(count, item);

                    count++;
                }

                this.length = count;
            }
        }

        #endregion

        #region IList properties

        /// <summary>
        /// Gets the number of elements contained in the array.
        /// </summary>
        /// <returns>The number of elements contained in the array.</returns>
        public Int32 Count { get { return this.Length; } }

        /// <summary>
        /// Gets a value indicating whether the array is read-only.
        /// </summary>
        /// <returns><c>true</c> if the array is read-only; otherwise, false.</returns>
        public Boolean IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value located at <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of items in the array.
        /// </exception>
        public T this[Int32 index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsLessThan0);
                if (index >= this.length)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsEqualToOrGreaterThanArraySize);

                T value;
                if (!this.items.TryGetValue(index, out value))
                    return default(T);

                return value;
            }

            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsLessThan0);
                if (index >= this.length)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsEqualToOrGreaterThanArraySize);

                if (this.items.ContainsKey(index))
                    this.items[index] = value;
                else
                    this.items.Add(index, value);

                this.version++;
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the actual number of elements contained in the array.
        /// </summary>
        /// <returns>The actual number of elements contained in the array.</returns>
        public Int32 ActualCount { get { return this.items.Count; } }

        /// <summary>
        /// Gets a 32-bit integer that represents the total number of elements in all the dimensions of the array.
        /// </summary>
        /// <value>A 32-bit integer that represents the total number of elements in all the dimensions of the array; <c>0</c> if there are no elements in the array.</value>
        /// <exception cref="System.OverflowException">The array contains more elements than the maximum value.</exception>
        public Int32 Length
        {
            get
            {
                if (this.length > Int32.MaxValue)
                    throw new OverflowException(Messages.TooManyElementsInArray);

                return (Int32)this.length;
            }
        }

        /// <summary>
        /// Gets a 64-bit integer that represents the total number of elements in all the dimensions of the array.
        /// </summary>
        /// <value>A 64-bit integer that represents the total number of elements in all the dimensions of the array; <c>0</c> if there are no elements in the array.</value>
        public Int64 LongLength { get { return this.length; } }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value located at <paramref name="index" />.</returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of items in the array.
        /// </exception>
        public T this[Int64 index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsLessThan0);
                if (index >= this.length)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsEqualToOrGreaterThanArraySize);

                T value;
                if (!this.items.TryGetValue(index, out value))
                    return default(T);

                return value;
            }

            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsLessThan0);
                if (index >= this.length)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsEqualToOrGreaterThanArraySize);

                if (this.items.ContainsKey(index))
                    this.items[index] = value;
                else
                    this.items.Add(index, value);

                this.version++;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines the index of a specific item in the array.
        /// </summary>
        /// <param name="item">The object to locate in the array.</param>
        /// <returns>The index of <paramref name="item" /> if found in the array; otherwise, <c>-1</c>.</returns>
        public Int64 IndexOf(T item)
        {
            if (AreEqual(item, default(T)))
            {
                Int32 index = 0;

                foreach (Int64 key in this.items.Keys)
                {
                    if (index != key)
                        return index;

                    index++;
                }
            }
            else
            {
                foreach (KeyValuePair<Int64, T> pair in this.items)
                {
                    if (AreEqual(pair.Value, item))
                        return pair.Key;
                }
            }

            return -1;
        }

        /// <summary>
        /// Inserts an item to the array at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the array.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        public void Insert(Int64 index, T item)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsLessThan0);
            if (index >= this.length)
                throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsEqualToOrGreaterThanArraySize);

            this.UpdateIndexes(index, 1);

            if (!AreEqual(item, default(T)))
            {
                this.items.Add(index, item);
            }

            this.length++;
            this.version++;
        }

        /// <summary>
        /// Removes the array item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        public void RemoveAt(Int64 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsLessThan0);
            if (index >= this.length)
                throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsEqualToOrGreaterThanArraySize);

            if (this.items.ContainsKey(index))
                this.items.Remove(index);

            this.UpdateIndexes(index, -1);

            this.length--;
            this.version++;
        }

        #endregion

        #region IList methods

        /// <summary>
        /// Adds an item to the array.
        /// </summary>
        /// <param name="item">The object to add to the array.</param>
        public void Add(T item)
        {
            if (!AreEqual(item, default(T)))
                this.items.Add(this.length, item);

            this.length++;
            this.version++;
        }

        /// <summary>
        /// Removes all items from the array.
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Determines whether the array contains a specific value.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="item" /> is found in the array; otherwise, <c>false</c>.</returns>
        /// <param name="item">The object to locate in the array.</param>
        public Boolean Contains(T item)
        {
            if (AreEqual(item, default(T)))
                return this.length > this.items.Count;

            return this.items.Values.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the current collection to an <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements copied from the current collection. The <see cref="System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The array index is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), Messages.ArrayIsNull);
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), Messages.ArrayIndexIsLessThan0);
            if (array.Length - arrayIndex < this.length)
                throw new ArgumentException(Messages.ArrayIndexIsGreaterThanSpace, nameof(arrayIndex));

            Int64 arrayLongIndex = arrayIndex;

            foreach (T item in this)
            {
                array[arrayLongIndex] = item;
                arrayLongIndex++;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the array.
        /// </summary>
        /// <param name="item">The object to remove from the array.</param>
        /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the array; otherwise, <c>false</c>.
        /// </returns>
        public Boolean Remove(T item)
        {
            Int64 index = this.IndexOf(item);

            if (index != -1)
            {
                if (!AreEqual(item, default(T)))
                    this.items.Remove(index);

                this.UpdateIndexes(index, -1);

                this.length--;
                this.version++;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines the index of a specific item in the array.
        /// </summary>
        /// <param name="item">The object to locate in the array.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, <c>-1</c>.</returns>
        Int32 IList<T>.IndexOf(T item)
        {
            Int64 index = this.IndexOf(item);

            if (index > Int32.MaxValue)
                return Int32.MaxValue;

            return (Int32)index;
        }

        /// <summary>
        /// Inserts an item to the array at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the array.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        void IList<T>.Insert(Int32 index, T item)
        {
            this.Insert(index, item);
        }

        /// <summary>
        /// Removes the array item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of elements in the array.
        /// </exception>
        void IList<T>.RemoveAt(Int32 index)
        {
            this.RemoveAt(index);
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{TValue}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Updates the indexes of the array.
        /// </summary>
        /// <param name="index">The staring index.</param>
        /// <param name="offset">The offset.</param>
        private void UpdateIndexes(Int64 index, Int32 offset)
        {
            this.items = this.items.ToDictionary(pair => pair.Key < index ? pair.Key : pair.Key + offset, pair => pair.Value);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Determines whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value.</param>
        /// <param name="second">The second value.</param>
        /// <returns><c>true</c> if the values are equal; otherwise, <c>false</c>.</returns>
        private static Boolean AreEqual(T first, T second)
        {
            return EqualityComparer<T>.Default.Equals(first, second);
        }

        #endregion

        #region Public types

        /// <summary>
        /// Supports a simple iteration over a <see cref="SparseArray{T}" /> collection.
        /// </summary>
        public sealed class Enumerator : IEnumerator<T>
        {
            #region Private fields

            /// <summary>
            /// The array that is enumerated.
            /// </summary>
            private SparseArray<T> localArray;

            /// <summary>
            /// The version at which the enumerator was instantiated.
            /// </summary>
            private Int32 localVersion;

            /// <summary>
            /// The position of the enumerator.
            /// </summary>
            private Int64 position;

            /// <summary>
            /// The current item.
            /// </summary>
            private T current;

            /// <summary>
            /// The inner enumerator used for enumerating the items.
            /// </summary>
            private IEnumerator<KeyValuePair<Int64, T>> innerEnumerator;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator" /> class.
            /// </summary>
            /// <param name="array">The array.</param>
            /// <exception cref="System.ArgumentNullException">The array is null.</exception>
            internal Enumerator(SparseArray<T> array)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array), Messages.ArrayIsNull);

                this.localVersion = array.version;
                this.localArray = array;
                this.position = -1;
                this.innerEnumerator = array.items.GetEnumerator();
                this.current = default(T);
            }

            #endregion

            #region IEnumerator properties

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            public T Current { get { return this.current; } }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            Object IEnumerator.Current { get { return this.current; } }

            #endregion

            #region IEnumerable methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (this.localVersion != this.localArray.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                this.position++;

                if (this.position >= this.localArray.length)
                {
                    this.position = this.localArray.length;
                    this.current = default(T);
                    return false;
                }

                // advance the inner enumerator to the next position if any
                while (this.position > this.innerEnumerator.Current.Key && this.innerEnumerator.MoveNext())
                {
                }

                if (this.position == this.innerEnumerator.Current.Key)
                    this.current = this.innerEnumerator.Current.Value;
                else
                    this.current = default(T);

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (this.localVersion != this.localArray.version)
                    throw new InvalidOperationException(Messages.CollectionWasModifiedAfterEnumerator);

                this.innerEnumerator.Reset();
                this.position = -1;

                this.current = default(T);
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }

            #endregion
        }

        #endregion
    }
}
