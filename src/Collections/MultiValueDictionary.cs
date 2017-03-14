// <copyright file="MultiValueDictionary.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2017 Roberto Giachetta. Licensed under the
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
    using System.Linq;
    using AEGIS.Collections.Resources;

    /// <summary>
    /// Represents a multi-value dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public sealed class MultiValueDictionary<TKey, TValue> : IDictionary<TKey, ICollection<TValue>>
    {
        /// <summary>
        /// The underlying dictionary.
        /// </summary>
        private Dictionary<TKey, List<TValue>> dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public MultiValueDictionary()
        {
            this.dictionary = new Dictionary<TKey, List<TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that is empty, has the default initial capacity, and uses the specified <see cref="IComparer{TKey}" />.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        public MultiValueDictionary(IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, List<TValue>>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="MultiValueDictionary{TKey, TValue}" /> and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The <see cref="MultiValueDictionary{TKey, TValue}" /> whose elements are copied to the new <see cref="MultiValueDictionary{TKey, TValue}" />.</param>
        /// <exception cref="System.ArgumentNullException">The dictionary is null.</exception>
        public MultiValueDictionary(MultiValueDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), CollectionMessages.DictionaryIsNull);

            this.dictionary = new Dictionary<TKey, List<TValue>>(dictionary.dictionary);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiValueDictionary{TKey, TValue}" /> class that contains elements copied from the specified <see cref="MultiValueDictionary{TKey, TValue}" /> and uses the specified <see cref="IComparer{TKey}" />.
        /// </summary>
        /// <param name="dictionary">The <see cref="MultiValueDictionary{TKey, TValue}" /> whose elements are copied to the new <see cref="MultiValueDictionary{TKey, TValue}" />.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{TKey}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentNullException">The dictionary is null.</exception>
        public MultiValueDictionary(MultiValueDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary), CollectionMessages.DictionaryIsNull);

            this.dictionary = new Dictionary<TKey, List<TValue>>(dictionary.dictionary, comparer);
        }

        /// <summary>
        /// Gets the number of elements contained in the dictionary.
        /// </summary>
        public Int32 Count { get { return this.dictionary.Count; } }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public Boolean IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets the keys currently present in the dictionary.
        /// </summary>
        public ICollection<TKey> Keys { get { return this.dictionary.Keys; } }

        /// <summary>
        /// Gets the values currently present in the dictionary.
        /// </summary>
        public ICollection<ICollection<TValue>> Values { get { return this.dictionary.Values.ToList<ICollection<TValue>>(); } }

        /// <summary>
        /// Gets or sets the element at the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value located at <paramref name="key" />.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public ICollection<TValue> this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

                return this.dictionary[key];
            }

            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

                this.dictionary[key] = value.ToList<TValue>();
            }
        }

        /// <summary>
        /// Adds a key-value pair to the dictionary.
        /// </summary>
        /// <param name="item">The key-value pair to add to the dictionary.</param>
        void ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Add(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        void ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Clear()
        {
            this.dictionary.Clear();
        }

        /// <summary>
        /// Determines whether the dictionary contains a collection of values.
        /// </summary>
        /// <returns><c>true</c> if all values of <paramref name="item" /> are found in the dictionary; otherwise, <c>false</c>.</returns>
        /// <param name="item">The collection of values to locate in the dictionary.</param>
        Boolean ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Contains(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            if (!this.ContainsKey(item.Key))
                return false;

            foreach (TValue it in item.Value)
            {
                if (!this.dictionary[item.Key].Contains(it))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Copies the elements of the dictionary to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from dictionary. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The array index is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
        void ICollection<KeyValuePair<TKey, ICollection<TValue>>>.CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), CollectionMessages.ArrayIsNull);
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), CollectionMessages.ArrayIndexIsLessThan0);
            if (array.Length - arrayIndex < this.Count)
                throw new ArgumentException(CollectionMessages.ArrayIndexIsGreaterThanSpace, nameof(arrayIndex));

            KeyValuePair<TKey, List<TValue>>[] tempArray = new KeyValuePair<TKey, List<TValue>>[this.dictionary.Count];
            (this.dictionary as IDictionary<TKey, List<TValue>>).CopyTo(tempArray, 0);

            for (Int32 index = arrayIndex; index < tempArray.Length; ++index)
            {
                array[index - arrayIndex] = new KeyValuePair<TKey, ICollection<TValue>>(tempArray[index].Key, tempArray[index].Value as ICollection<TValue>);
            }
        }

        /// <summary>
        /// Removes a specific key-value pair from the dictionary.
        /// </summary>
        /// <param name="item">The key-value pair to remove.</param>
        /// <returns><c>true</c> if all values of <paramref name="item" /> were successfully removed from the dictionary; otherwise, <c>false</c>.</returns>
        Boolean ICollection<KeyValuePair<TKey, ICollection<TValue>>>.Remove(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            if (!this.ContainsKey(item.Key))
                return false;

            foreach (TValue it in item.Value)
            {
                if (!this[item.Key].Contains(it))
                    return false;
            }

            this.Remove(item.Key);

            return true;
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public void Add(TKey key, ICollection<TValue> value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            if (this.dictionary.ContainsKey(key))
            {
                this.dictionary[key].AddRange(value);
            }
            else
            {
                this.dictionary[key] = value.ToList<TValue>();
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><c>true</c> if <paramref name="key" /> is found in the dictionary; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>. This method returns <c>false</c> if key is not found.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            return this.dictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the dictionary contains an element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean TryGetValue(TKey key, out ICollection<TValue> value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            List<TValue> outValue;
            Boolean retValue = this.dictionary.TryGetValue(key, out outValue);

            value = outValue;

            return retValue;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{KeyValuePair{TKey, TValue}}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, ICollection<TValue>> item in this.dictionary as IDictionary<TKey, ICollection<TValue>>)
                yield return item;
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
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            if (!this.dictionary.ContainsKey(key))
                this.dictionary[key] = new List<TValue>();

            this.dictionary[key].Add(value);
        }

        /// <summary>
        /// Removes the value with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="value">The value of the element to remove.</param>
        /// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>. This method returns <c>false</c> if key is not found.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean Remove(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            if (!this.ContainsKey(key))
                return false;

            if (!this.dictionary[key].Contains(value))
                return false;

            this.dictionary[key].Remove(value);

            if (this.dictionary[key].Count == 0)
                this.dictionary.Remove(key);

            return true;
        }
    }
}
