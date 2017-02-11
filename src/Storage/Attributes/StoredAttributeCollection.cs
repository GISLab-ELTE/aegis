// <copyright file="StoredAttributeCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Attributes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ELTE.AEGIS.Collections.Resources;

    /// <summary>
    /// Represents an attribute collection located in a store.
    /// </summary>
    public class StoredAttributeCollection : IStoredAttributeCollection
    {
        /// <summary>
        /// Represents the collection of attribute keys.
        /// </summary>
        private class KeyCollection : ICollection<String>
        {
            /// <summary>
            /// The underlying attribute collection.
            /// </summary>
            private StoredAttributeCollection collection;

            /// <summary>
            /// Initializes a new instance of the <see cref="KeyCollection" /> class.
            /// </summary>
            /// <param name="collection">The collection.</param>
            public KeyCollection(StoredAttributeCollection collection)
            {
                this.collection = collection;
            }

            /// <summary>
            /// Gets the number of elements contained in the collection.
            /// </summary>
            public Int32 Count { get { return this.collection.Driver.ReadAttributeCount(this.collection.Identifier); } }

            /// <summary>
            /// Gets a value indicating whether the collection is read-only.
            /// </summary>
            public Boolean IsReadOnly { get { return true; } }

            /// <summary>
            /// Adds an item to the collection.
            /// </summary>
            /// <param name="item">The object to add to the collection.</param>
            /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
            public void Add(String item)
            {
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the collection.
            /// </summary>
            /// <param name="item">The object to remove from the collection.</param>
            /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the collection; otherwise, <c>false</c>.</returns>
            /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
            public Boolean Remove(String item)
            {
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);
            }

            /// <summary>
            /// Removes all items from the collection.
            /// </summary>
            /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
            public void Clear()
            {
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);
            }

            /// <summary>
            /// Determines whether the collection contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the collection.</param>
            /// <returns><c>true</c> if <paramref name="item" /> is found in the collection; otherwise, <c>false</c>.</returns>
            public Boolean Contains(String item)
            {
                return this.collection.Driver.ContainsAttribute(this.collection.Identifier, item);
            }

            /// <summary>
            /// Copies the geometries of the geometry list to an array, starting at a particular array index.
            /// </summary>
            /// <param name="array">The one-dimensional array that is the destination of the elements copied from geometry list.</param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <exception cref="System.ArgumentNullException">The array is null.</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">Index is less than 0.</exception>
            /// <exception cref="System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
            public void CopyTo(String[] array, Int32 arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array), CollectionMessages.ArrayIsNull);
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex), CollectionMessages.IndexIsLessThan0);
                if (arrayIndex + this.collection.Driver.ReadAttributeCount(this.collection.Identifier) > array.Length)
                    throw new ArgumentException(CollectionMessages.ArrayIndexIsGreaterThanSpace, nameof(array));

                foreach (String key in this.collection.Driver.ReadAttributeKeys(this.collection.Identifier))
                {
                    array[arrayIndex] = key;
                    arrayIndex++;
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An <see cref="IEnumerator{String}" /> that can be used to iterate through the collection.</returns>
            public IEnumerator<String> GetEnumerator()
            {
                foreach (String key in this.collection.Driver.ReadAttributeKeys(this.collection.Identifier))
                    yield return key;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Represents the collection of attribute values.
        /// </summary>
        private class ValueCollection : ICollection<Object>
        {
            /// <summary>
            /// The underlying attribute collection.
            /// </summary>
            private StoredAttributeCollection collection;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValueCollection" /> class.
            /// </summary>
            /// <param name="collection">The collection.</param>
            public ValueCollection(StoredAttributeCollection collection)
            {
                this.collection = collection;
            }

            /// <summary>
            /// Gets the number of elements contained in the collection.
            /// </summary>
            public Int32 Count { get { return this.collection.Driver.ReadAttributeCount(this.collection.Identifier); } }

            /// <summary>
            /// Gets a value indicating whether the collection is read-only.
            /// </summary>
            public Boolean IsReadOnly { get { return true; } }

            /// <summary>
            /// Adds an item to the collection.
            /// </summary>
            /// <param name="item">The object to add to the collection.</param>
            /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
            public void Add(Object item)
            {
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the collection.
            /// </summary>
            /// <param name="item">The object to remove from the collection.</param>
            /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the collection; otherwise, <c>false</c>.</returns>
            /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
            public Boolean Remove(Object item)
            {
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);
            }

            /// <summary>
            /// Removes all items from the collection.
            /// </summary>
            /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
            public void Clear()
            {
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);
            }

            /// <summary>
            /// Determines whether the collection contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the collection.</param>
            /// <returns><c>true</c> if <paramref name="item" /> is found in the collection; otherwise, <c>false</c>.</returns>
            public Boolean Contains(Object item)
            {
                foreach (String key in this.collection.Driver.ReadAttributeKeys(this.collection.Identifier))
                {
                    if (this.collection.Driver.ReadAttribute(this.collection.Identifier, key) == item)
                        return true;
                }

                return false;
            }

            /// <summary>
            /// Copies the geometries of the geometry list to an array, starting at a particular array index.
            /// </summary>
            /// <param name="array">The one-dimensional array that is the destination of the elements copied from geometry list.</param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <exception cref="System.ArgumentNullException">The array is null.</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">Index is less than 0.</exception>
            /// <exception cref="System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
            public void CopyTo(Object[] array, Int32 arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array), CollectionMessages.ArrayIsNull);
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex), CollectionMessages.IndexIsLessThan0);
                if (arrayIndex + this.collection.Driver.ReadAttributeCount(this.collection.Identifier) > array.Length)
                    throw new ArgumentException(CollectionMessages.ArrayIndexIsGreaterThanSpace, nameof(array));

                foreach (String key in this.collection.Driver.ReadAttributeKeys(this.collection.Identifier))
                {
                    array[arrayIndex] = key;
                    arrayIndex++;
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An <see cref="IEnumerator{Object}" /> that can be used to iterate through the collection.</returns>
            public IEnumerator<Object> GetEnumerator()
            {
                foreach (String key in this.collection.Driver.ReadAttributeKeys(this.collection.Identifier))
                    yield return this.collection.Driver.ReadAttribute(this.collection.Identifier, key);
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredAttributeCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredAttributeCollection(IStoredAttributeCollectionFactory factory, String identifier)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), ELTE.AEGIS.Resources.CoreMessages.FactoryIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ELTE.AEGIS.Resources.CoreMessages.IdentifierIsNull);

            this.Factory = factory;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets the feature identifier.
        /// </summary>
        /// <value>The feature identifier.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the driver of the attribute collection.
        /// </summary>
        /// <value>The driver the attribute collection.</value>
        public IAttributeDriver Driver { get { return this.Factory.Driver; } }

        /// <summary>
        /// Gets the factory of the attribute collection.
        /// </summary>
        /// <value>The factory the attribute collection was constructed by.</value>
        public IStoredAttributeCollectionFactory Factory { get; private set; }

        /// <summary>
        /// Gets the factory of the attribute collection.
        /// </summary>
        /// <value>The factory the attribute collection was constructed by.</value>
        IAttributeCollectionFactory IAttributeCollection.Factory { get { return this.Factory; } }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        /// <value>The number of elements contained in the collection.</value>
        public Int32 Count { get { return this.Driver.ReadAttributeCount(this.Identifier); } }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <value><c>true</c> if the collection is read-only; otherwise, <c>false</c>.</value>
        public Boolean IsReadOnly { get { return this.Driver.SupportedOperations.Length == 1 && this.Driver.SupportedOperations[0] == DriverOperation.Read; } }

        /// <summary>
        /// Gets the collection of keys.
        /// </summary>
        /// <value>The collection of keys.</value>
        public ICollection<String> Keys { get { return new KeyCollection(this); } }

        /// <summary>
        /// Gets the collection of values.
        /// </summary>
        /// <value>The collection of values.</value>
        public ICollection<Object> Values { get { return new ValueCollection(this); } }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for the specified key.</returns>
        /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
        public Object this[String key]
        {
            get
            {
                return this.Driver.ReadAttribute(this.Identifier, key);
            }

            set
            {
                if (this.IsReadOnly)
                    throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);

                this.Driver.UpdateAttribute(this.Identifier, key, value);
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the collection.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the collection.</exception>
        public void Add(String key, Object value)
        {
            if (this.IsReadOnly)
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);

            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            if (this.Driver.ContainsAttribute(this.Identifier, key))
                throw new ArgumentException(CollectionMessages.KeyAlreadyExistsInTheCollection);

            this.Driver.UpdateAttribute(this.Identifier, key, value);
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The object to add to the collection.</param>
        /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
        /// <exception cref="System.ArgumentException">
        /// The key is null.
        /// or
        /// An element with the same key already exists in the collection.
        /// </exception>
        void ICollection<KeyValuePair<String, Object>>.Add(KeyValuePair<String, Object> item)
        {
            if (this.IsReadOnly)
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);

            if (item.Key == null)
                throw new ArgumentException(CollectionMessages.KeyIsNull, nameof(item));

            if (this.Driver.ContainsAttribute(this.Identifier, item.Key))
                throw new ArgumentException(CollectionMessages.KeyAlreadyExistsInTheCollection);

            this.Driver.UpdateAttribute(this.Identifier, item.Key, item.Value);
        }

        /// <summary>
        /// Determines whether the collection contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the collection.</param>
        /// <returns><c>true</c> if the v contains an element with the key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean ContainsKey(String key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            return this.Driver.ContainsAttribute(this.Identifier, key);
        }

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns><c>true</c> if <paramref name="item" /> is found in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The key is null.</exception>
        Boolean ICollection<KeyValuePair<String, Object>>.Contains(KeyValuePair<String, Object> item)
        {
            if (item.Key == null)
                throw new ArgumentException(CollectionMessages.KeyIsNull, nameof(item));

            if (!this.Driver.ContainsAttribute(this.Identifier, item.Key))
                return false;

            Object value = this.Driver.ReadAttribute(this.Identifier, item.Key);

            return (value == null && item.Value == null) || value.Equals(item.Value);
        }

        /// <summary>
        /// Removes the element with the specified key from the collection.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the element is successfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean Remove(String key)
        {
            if (this.IsReadOnly)
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);

            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            if (!this.Driver.ContainsAttribute(this.Identifier, key))
                return false;

            this.Driver.DeleteAttribute(this.Identifier, key);
            return true;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns><c>true</c> if <paramref name="item" /> was successfully removed from the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
        /// <exception cref="System.ArgumentException">The key is null.</exception>
        Boolean ICollection<KeyValuePair<String, Object>>.Remove(KeyValuePair<String, Object> item)
        {
            if (this.IsReadOnly)
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);

            if (item.Key == null)
                throw new ArgumentException(CollectionMessages.KeyIsNull, nameof(item));

            if (!this.Driver.ContainsAttribute(this.Identifier, item.Key))
                return false;

            Object value = this.Driver.ReadAttribute(this.Identifier, item.Key);

            if (!(value == null && item.Value == null) && !value.Equals(item.Value))
                return false;

            this.Driver.DeleteAttribute(this.Identifier, item.Key);
            return true;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the collection contains an element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        public Boolean TryGetValue(String key, out Object value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), CollectionMessages.KeyIsNull);

            value = null;
            if (!this.Driver.ContainsAttribute(this.Identifier, key))
                return false;

            value = this.Driver.ReadAttribute(this.Identifier, key);
            return true;
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
        public void Clear()
        {
            if (this.IsReadOnly)
                throw new NotSupportedException(CollectionMessages.CollectionIsReadOnly);

            this.Driver.DeleteAttributes(this.Identifier);
        }

        /// <summary>
        /// Copies the geometries of the geometry list to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from geometry list.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Index is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
        public void CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), CollectionMessages.ArrayIsNull);
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), CollectionMessages.IndexIsLessThan0);
            if (arrayIndex + this.Driver.ReadAttributeCount(this.Identifier) > array.Length)
                throw new ArgumentException(CollectionMessages.ArrayIndexIsGreaterThanSpace, nameof(array));

            foreach (String key in this.Driver.ReadAttributeKeys(this.Identifier))
            {
                array[arrayIndex] = new KeyValuePair<String, Object>(key, this.Driver.ReadAttribute(this.Identifier, key));
                arrayIndex++;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            foreach (String key in this.Driver.ReadAttributeKeys(this.Identifier))
            {
                yield return new KeyValuePair<String, Object>(key, this.Driver.ReadAttribute(this.Identifier, key));
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
