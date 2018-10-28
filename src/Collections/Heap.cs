// <copyright file="Heap.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using AEGIS.Collections.Resources;

    /// <summary>
    /// Represents a heap data structure containing key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the heap.</typeparam>
    /// <typeparam name="TValue">The type of the values in the heap.</typeparam>
    /// <remarks>
    /// A heap is a specialized tree-based data structure that satisfies the heap property:
    /// If A is a parent node of B then key(A) is ordered with respect to key(B) with the same ordering applying across the heap.
    /// This implementation of the <see cref="IHeap{TKey, TValue}" /> interface uses the default ordering scheme of the key type if not
    /// specified differently by providing an instance of the  <see cref="IComparer{T}" /> interface.
    /// For example, the default ordering provides a min heap in case of number types.
    /// The storage of the key/value pairs is array based with O(log n) insertion and removal time.
    /// </remarks>
    public class Heap<TKey, TValue> : IHeap<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        /// <summary>
        /// The default capacity. This field is constant.
        /// </summary>
        private const Int32 DefaultCapacity = 4;

        /// <summary>
        /// The empty array. This field is read-only.
        /// </summary>
        private static readonly KeyValuePair<TKey, TValue>[] EmptyArray = new KeyValuePair<TKey, TValue>[0];

        /// <summary>
        /// The comparer that is used to determine order of keys for the heap..
        /// </summary>
        private readonly IComparer<TKey> comparer;

        /// <summary>
        /// The items of the heap stored in an array.
        /// </summary>
        private KeyValuePair<TKey, TValue>[] items;

        /// <summary>
        /// The number of items stored in the heap.
        /// </summary>
        private Int32 size;

        /// <summary>
        /// The version of the heap.
        /// </summary>
        private Int32 version;

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the default comparer for the key type.
        /// </summary>
        public Heap()
        {
            this.items = EmptyArray;
            this.size = 0;
            this.version = 0;
            this.comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the default comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="Heap{TKey, TValue}" /> can contain.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public Heap(Int32 capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(CollectionMessages.CapacityLessThan0);

            this.items = new KeyValuePair<TKey, TValue>[capacity];
            this.size = 0;
            this.version = 0;
            this.comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="IComparer{T}" />.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        public Heap(IComparer<TKey> comparer)
        {
            this.size = 0;
            this.version = 0;
            this.items = EmptyArray;

            this.comparer = comparer ?? Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that contains elements copied from the specified <see cref="Heap{TKey, TValue}" /> and uses the default comparer for the key type.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{KeyValuePair{TKey, TValue}}" /> whose elements are copied to the new <see cref="Heap{TKey, TValue}" />.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public Heap(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            this.size = 0;
            this.version = 0;
            this.items = new KeyValuePair<TKey, TValue>[DefaultCapacity];
            this.comparer = Comparer<TKey>.Default;

            foreach (KeyValuePair<TKey, TValue> element in source)
            {
                this.Insert(element.Key, element.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="IComparer{T}" />.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{KeyValuePair{TKey, TValue}}" /> whose elements are copied to the new <see cref="Heap{TKey, TValue}" />.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public Heap(IEnumerable<KeyValuePair<TKey, TValue>> source, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            this.items = EmptyArray;
            this.size = 0;
            this.version = 0;

            this.comparer = comparer ?? Comparer<TKey>.Default;

            foreach (KeyValuePair<TKey, TValue> element in source)
            {
                this.Insert(element.Key, element.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap{TKey, TValue}" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="IComparer{T}" />.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="Heap{TKey, TValue}" /> can contain.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public Heap(Int32 capacity, IComparer<TKey> comparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), CollectionMessages.CapacityLessThan0);

            this.items = new KeyValuePair<TKey, TValue>[capacity];
            this.size = 0;
            this.version = 0;

            this.comparer = comparer ?? Comparer<TKey>.Default;
        }

        /// <summary>
        /// Gets the number of elements actually contained in the heap.
        /// </summary>
        /// <value>The number of elements actually contained in the heap.</value>
        public Int32 Count { get { return this.size; } }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <value>The number of elements that the heap can contain before resizing is required.</value>
        /// <exception cref="System.ArgumentOutOfRangeException">Capacity is set to a value that is less than <see cref="Count" />.</exception>
        public Int32 Capacity
        {
            get
            {
                return this.items.Length;
            }

            set
            {
                if (value != this.items.Length)
                {
                    if (value < this.size)
                        throw new InvalidOperationException(CollectionMessages.CapacityLessThanCount);

                    if (value > 0)
                    {
                        KeyValuePair<TKey, TValue>[] newItems = new KeyValuePair<TKey, TValue>[value];
                        if (this.size > 0)
                        {
                            Array.Copy(this.items, 0, newItems, 0, this.size);
                        }

                        this.items = newItems;
                    }
                    else
                    {
                        this.items = EmptyArray;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value at the top of the heap without removing it.
        /// </summary>
        /// <value>The value at the beginning of the heap.</value>
        /// <exception cref="System.InvalidOperationException">The heap is empty.</exception>
        public TValue Peek
        {
            get
            {
                if (this.size == 0)
                    throw new InvalidOperationException(CollectionMessages.HeapIsEmpty);

                return this.items[0].Value;
            }
        }

        /// <summary>
        /// Gets the <see cref="IComparer{T}" /> that is used to determine order of keys for the heap.
        /// </summary>
        /// <value>The <see cref="IComparer{T}" /> generic interface implementation that is used to determine order of keys for the current heap and to provide hash values for the keys.</value>
        public IComparer<TKey> Comparer { get { return this.comparer; } }

        /// <summary>
        /// Inserts the specified key and value to the heap.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (this.size == this.items.Length)
                this.EnsureCapacity(this.size + 1);

            this.items[this.size] = new KeyValuePair<TKey, TValue>(key, value);
            this.size++;
            this.version++;

            Int32 current = this.size - 1;
            while (current > 0 && this.comparer.Compare(this.items[(current - 1) / 2].Key, this.items[current].Key) > 0)
            {
                KeyValuePair<TKey, TValue> h = this.items[current];
                this.items[current] = this.items[(current - 1) / 2];
                this.items[(current - 1) / 2] = h;

                current = (current - 1) / 2;
            }
        }

        /// <summary>
        /// Removes and returns the value at the top of the heap.
        /// </summary>
        /// <returns>The value that is removed from the top of the heap.</returns>
        /// <exception cref="System.InvalidOperationException">The heap is empty.</exception>
        public TValue RemovePeek()
        {
            if (this.size == 0)
                throw new InvalidOperationException(CollectionMessages.HeapIsEmpty);

            KeyValuePair<TKey, TValue> result = this.items[0];
            Int32 current = 0, leftChild, rightChild, parent;

            this.items[0] = this.items[this.size - 1];
            this.size--;
            this.version++;

            while (current < this.size)
            {
                parent = current;
                leftChild = 2 * current + 1;
                rightChild = 2 * current + 2;
                if (this.size > leftChild && this.comparer.Compare(this.items[current].Key, this.items[leftChild].Key) > 0)
                {
                    current = leftChild;
                }

                if (this.size > rightChild && this.comparer.Compare(this.items[current].Key, this.items[rightChild].Key) > 0)
                {
                    current = rightChild;
                }

                if (current == parent)
                    break;

                KeyValuePair<TKey, TValue> h = this.items[current];
                this.items[current] = this.items[parent];
                this.items[parent] = h;
            }

            return result.Value;
        }

        /// <summary>
        /// Determines whether the heap contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the heap.</param>
        /// <returns><c>true</c> if the heap contains an element with the specified key; otherwise, <c>false</c>.</returns>
        public Boolean Contains(TKey key)
        {
            for (Int32 index = 0; index < this.size; index++)
            {
                if (this.comparer.Compare(this.items[index].Key, key) == 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all keys and values from the heap.
        /// </summary>
        public void Clear()
        {
            this.size = 0;
            this.version++;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{KeyValuePair{TKey, TValue}}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
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

        /// <summary>
        /// Ensures the capacity of the heap is at least the given minimum value.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        protected void EnsureCapacity(Int32 min)
        {
            if (this.items.Length < min)
            {
                Int32 newCapacity = this.items.Length == 0 ? DefaultCapacity : this.items.Length * 2;

                if (newCapacity < min)
                    newCapacity = min;

                if (newCapacity > 0)
                {
                    KeyValuePair<TKey, TValue>[] newItems = new KeyValuePair<TKey, TValue>[newCapacity];
                    if (this.size > 0)
                    {
                        Array.Copy(this.items, 0, newItems, 0, this.size);
                    }

                    this.items = newItems;
                }
                else
                {
                    this.items = EmptyArray;
                }
            }
        }

        /// <summary>
        /// Enumerates the elements of a heap.
        /// </summary>
        /// <remarks>
        /// The enumerator performs a level order traversal of the specified heap.
        /// </remarks>
        public sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
        {
            /// <summary>
            /// The heap that is enumerated.
            /// </summary>
            private Heap<TKey, TValue> localHeap;

            /// <summary>
            /// The version at which the enumerator was instantiated.
            /// </summary>
            private Int32 localVersion;

            /// <summary>
            /// The position of the enumerator.
            /// </summary>
            private Int32 position;

            /// <summary>
            /// The current item.
            /// </summary>
            private KeyValuePair<TKey, TValue> current;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator" /> class.
            /// </summary>
            /// <param name="heap">The heap.</param>
            /// <exception cref="System.ArgumentNullException">The heap is null.</exception>
            internal Enumerator(Heap<TKey, TValue> heap)
            {
                this.localHeap = heap ?? throw new ArgumentNullException(nameof(heap));
                this.localVersion = heap.version;

                this.position = -1;
                this.current = default(KeyValuePair<TKey, TValue>);
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.</value>
            public KeyValuePair<TKey, TValue> Current
            {
                get { return this.current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.-</value>
            Object IEnumerator.Current
            {
                get { return this.current; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (this.localVersion != this.localHeap.version)
                    throw new InvalidOperationException(CollectionMessages.CollectionWasModifiedAfterEnumerator);

                this.position++;

                if (this.position >= this.localHeap.size)
                {
                    this.position = this.localHeap.size;
                    this.current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }
                else
                {
                    this.current = this.localHeap.items[this.position];
                    return true;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (this.localVersion != this.localHeap.version)
                    throw new InvalidOperationException(CollectionMessages.CollectionWasModifiedAfterEnumerator);

                this.position = -1;
                this.current = default(KeyValuePair<TKey, TValue>);
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}