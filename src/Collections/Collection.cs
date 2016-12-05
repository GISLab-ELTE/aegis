// <copyright file="Collection.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.ObjectModel;
    using System.Linq;
    using ELTE.AEGIS.Collections.Resources;

    /// <summary>
    /// Provides extensions to collections.
    /// </summary>
    public static class Collection
    {
        

        /// <summary>
        /// Represents a proxy list for querying a range within a list.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the list.</typeparam>
        private class ProxyRangeReadOnlyList<T> : IReadOnlyList<T>
        {
            

            /// <summary>
            /// The source list.
            /// </summary>
            private IReadOnlyList<T> source;

            /// <summary>
            /// The starting index for taking elements in the source.
            /// </summary>
            private Int32 startIndex;

            /// <summary>
            /// The number of elements taken from the source.
            /// </summary>
            private Int32 count;

            

            

            /// <summary>
            /// Initializes a new instance of the <see cref="ProxyRangeReadOnlyList{T}" /> class.
            /// </summary>
            /// <param name="source">The source list.</param>
            /// <param name="startIndex">The starting index.</param>
            /// <param name="count">The number of elements taken from the source.</param>
            public ProxyRangeReadOnlyList(IReadOnlyList<T> source, Int32 startIndex, Int32 count)
            {
                this.source = source;
                this.startIndex = startIndex;
                this.count = Math.Min(count, source.Count - startIndex);
            }

            

            

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <value>The element at the specified index.</value>
            /// <param name="index">The zero-based index of the element to get.</param>
            /// <returns>The element located at the specified index.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The index is less than 0.
            /// or
            /// The index is equal to or greater than the count.
            /// </exception>
            public T this[int index]
            {
                get
                {
                    if (index < 0)
                        throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
                    if (index >= this.count)
                        throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsEqualToOrGreaterThanCount);

                    return this.source[index - this.startIndex];
                }
            }

            /// <summary>
            /// Gets the number of elements contained in the list.
            /// </summary>
            /// <value>The number of elements contained in the list.</value>
            public Int32 Count
            {
                get
                {
                    return this.count;
                }
            }

            

            

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (Int32 index = 0; index < this.count; index++)
                    yield return this.source[this.startIndex + index];
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
        /// Represents a proxy list for wrapping the specified number of elements in a list in reverse order.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the list.</typeparam>
        private class ProxyReverseReadOnlyList<T> : IReadOnlyList<T>
        {
            

            /// <summary>
            /// The source list.
            /// </summary>
            private IReadOnlyList<T> source;

            

            

            /// <summary>
            /// Initializes a new instance of the <see cref="ProxyReverseReadOnlyList{T}" /> class.
            /// </summary>
            /// <param name="source">The source list.</param>
            public ProxyReverseReadOnlyList(IReadOnlyList<T> source)
            {
                this.source = source;
            }

            

            

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <value>The element at the specified index.</value>
            /// <param name="index">The zero-based index of the element to get.</param>
            /// <returns>The element located at the specified index.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">
            /// The index is less than 0.
            /// or
            /// The index is equal to or greater than the count.
            /// </exception>
            public T this[int index]
            {
                get
                {
                    return this.source[this.source.Count - 1 - index];
                }
            }

            /// <summary>
            /// Gets the number of elements contained in the list.
            /// </summary>
            /// <value>The number of elements contained in the list.</value>
            public Int32 Count
            {
                get
                {
                    return this.source.Count;
                }
            }

            

            

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (Int32 sourceIndex = this.source.Count - 1; sourceIndex >= 0; sourceIndex--)
                    yield return this.source[sourceIndex];
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
        /// Generates a collection of numbers.
        /// </summary>
        /// <param name="firstBoundary">The first boundary.</param>
        /// <param name="secondBoundary">The second boundary.</param>
        /// <param name="count">The cont of the numbers.</param>
        /// <returns>The collection of generated numbers.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The count is less than 1.</exception>
        public static IEnumerable<Int32> GenerateNumbers(Int32 firstBoundary, Int32 secondBoundary, Int32 count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), CollectionMessages.CountIsLessThan1);

            if (firstBoundary > secondBoundary)
            {
                Int32 temp = firstBoundary;
                firstBoundary = secondBoundary;
                secondBoundary = temp;
            }

            Double delta = (Double)Math.Abs(secondBoundary - firstBoundary) / count;

            for (Int32 index = 0; index < count; index++)
                yield return Math.Min((Int32)Math.Round(firstBoundary + index * delta), secondBoundary);
        }

        

        

        /// <summary>
        /// Determines whether a sequence contains any non-null elements.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns><c>true</c> if there are any non-null items in the collection; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static Boolean AnyElement<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The first non-null element from the collection.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.ArgumentException">The collection is empty.</exception>
        public static T FirstElement<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null)
                        return enumerator.Current;
                }
            }

            throw new ArgumentException(CollectionMessages.CollectionIsEmpty, nameof(collection));
        }

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The first non-null element from the collection; or the default value if such an item does not exist.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static T FirstOrDefaultElement<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null)
                        return enumerator.Current;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The last non-null element from the collection.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.ArgumentException">The collection is empty.</exception>
        public static T LastElement<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            T last = default(T);
            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null)
                        last = enumerator.Current;
                }
            }

            if (last != null)
                return last;

            throw new ArgumentException(CollectionMessages.CollectionIsEmpty, nameof(collection));
        }

        /// <summary>
        /// Returns the non-null elements of the collection.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The collection containing the non-null elements of the source collection.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<T> Elements<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null)
                        yield return enumerator.Current;
                }
            }
        }

        /// <summary>
        /// Returns the number of non-null elements within the collection.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>The number of non-null elements within the collection.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static Int32 ElementCount<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            Int32 count = 0;
            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Appends an item to the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item which is appended to the collection.</param>
        /// <returns>The collection with contains the additional item.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> collection, T item)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            using (IEnumerator<T> enumerator = collection.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }

            yield return item;
        }

        /// <summary>
        /// Returns a read-only list wrapper for the current collection.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The list.</param>
        /// <returns>A <see cref="ReadOnlyCollection{T}" /> that acts as a read-only wrapper around the current <see cref="IList{T}" />.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            IReadOnlyList<T> readOnlyCollection = collection as IReadOnlyList<T>;
            if (readOnlyCollection != null)
                return readOnlyCollection;

            return new ReadOnlyCollection<T>(collection);
        }

        /// <summary>
        /// Returns a read-only dictionary wrapper for the current collection.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="collection">The dictionary.</param>
        /// <returns>A <see cref="ReadOnlyDictionary{TKey, TValue}" /> that acts as a read-only wrapper around the current <see cref="IDictionary{TKey, TValue}" />.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            IReadOnlyDictionary<TKey, TValue> readOnlyDictionary = collection as IReadOnlyDictionary<TKey, TValue>;
            if (readOnlyDictionary != null)
                return readOnlyDictionary;

            return new ReadOnlyDictionary<TKey, TValue>(collection);
        }

        /// <summary>
        /// Returns a read-only set wrapper for the current collection.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="set">The set.</param>
        /// <returns>A <see cref="ReadOnlySet{T}" /> that acts as a read-only wrapper around the current <see cref="ISet{T}" />.</returns>
        /// <exception cref="System.ArgumentNullException">The set is null.</exception>
        public static ISet<T> AsReadOnly<T>(this ISet<T> set)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set), CollectionMessages.SetIsNull);

            if (set.IsReadOnly)
                return set;

            return new ReadOnlySet<T>(set);
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of the element that matches the defined predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="match">The condition of the element to search for.</param>
        /// <returns>The zero-based index of the first element that matches the defined predicate.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The predicate is null.
        /// </exception>
        public static Int32 IndexOf<T>(this IEnumerable<T> collection, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);
            if (match == null)
                throw new ArgumentNullException(nameof(match), CollectionMessages.PredicateIsNull);

            Int32 index = 0;
            foreach (T item in collection)
            {
                if (match(item))
                    return index;
                index++;
            }

            return -1;
        }

        /// <summary>
        /// Returns the zero-based indexes of the elements that match the defined predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="match">The condition of the element to search for.</param>
        /// <returns>The zero-based indexes of the elements that match the defined predicate.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The predicate is null.
        /// </exception>
        public static IEnumerable<Int32> IndexesOf<T>(this IEnumerable<T> collection, Predicate<T> match)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);
            if (match == null)
                throw new ArgumentNullException(nameof(match), CollectionMessages.PredicateIsNull);

            Int32 index = 0;
            foreach (T item in collection)
            {
                if (match(item))
                    yield return index;
                index++;
            }
        }

        /// <summary>
        /// Returns the index of the smallest item within the collection specified by the selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector function.</param>
        /// <returns>The index of the smallest item within the collection specified by the selector</returns>
        /// <exception cref="ArgumentNullException">
        /// The collection is null.
        /// or
        /// The selector is null.
        /// </exception>
        /// <exception cref="ArgumentException">The collection is empty.</exception>
        public static Int32 MaxIndex<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector)
            where TResult : IComparable<TResult>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);
            if (selector == null)
                throw new ArgumentNullException(nameof(selector), CollectionMessages.SelectorIsNull);

            IEnumerator<TSource> enumerator = collection.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new ArgumentException(CollectionMessages.CollectionIsEmpty, nameof(collection));

            Int32 index = 0, maxIndex = 0;
            TResult max = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                TResult currentResult = selector(enumerator.Current);
                if (max.CompareTo(currentResult) > 0)
                {
                    max = currentResult;
                    maxIndex = index;
                }

                index++;
            }

            return maxIndex;
        }

        /// <summary>
        /// Returns the index of the smallest item within the collection specified by the selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="selector">The selector function.</param>
        /// <returns>The index of the smallest item within the collection specified by the selector</returns>
        /// <exception cref="ArgumentNullException">
        /// The collection is null.
        /// or
        /// The selector is null.
        /// </exception>
        /// <exception cref="ArgumentException">The collection is empty.</exception>
        public static Int32 MinIndex<TSource, TResult>(this IEnumerable<TSource> collection, Func<TSource, TResult> selector)
            where TResult : IComparable<TResult>
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);
            if (selector == null)
                throw new ArgumentNullException(nameof(selector), CollectionMessages.SelectorIsNull);

            IEnumerator<TSource> enumerator = collection.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new ArgumentException(CollectionMessages.CollectionIsEmpty, nameof(collection));

            Int32 index = 0, minIndex = 0;
            TResult min = selector(enumerator.Current);

            while (enumerator.MoveNext())
            {
                TResult currentResult = selector(enumerator.Current);
                if (min.CompareTo(currentResult) < 0)
                {
                    min = currentResult;
                    minIndex = index;
                }

                index++;
            }

            return minIndex;
        }

        /// <summary>
        /// Inverts the order of the elements in a list.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>A list whose elements correspond to those of the input sequence in reverse order.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IReadOnlyList<T> Reverse<T>(this IReadOnlyList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);
            return new ProxyReverseReadOnlyList<T>(collection);
        }

        /// <summary>
        /// Takes the specified number of elements from a list.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="count">The number of elements to take from the collection.</param>
        /// <returns>The list containing the specified number of elements.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The count is less than 1.</exception>
        public static IReadOnlyList<T> GetRange<T>(this IReadOnlyList<T> collection, Int32 count)
        {
            return GetRange<T>(collection, 0, count);
        }

        /// <summary>
        /// Takes the specified number of elements from a list.
        /// </summary>
        /// <typeparam name="T">The type of the objects within the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="startIndex">The starting index.</param>
        /// <param name="count">The number of elements to take from the collection.</param>
        /// <returns>The list containing the specified number of elements.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is equal to or greater than the count.
        /// or
        /// The count is less than 1.
        /// </exception>
        public static IReadOnlyList<T> GetRange<T>(this IReadOnlyList<T> collection, Int32 startIndex, Int32 count)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);
            if (startIndex > collection.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex), CollectionMessages.IndexIsEqualToOrGreaterThanCount);
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), CollectionMessages.CountIsLessThan1);

            return new ProxyRangeReadOnlyList<T>(collection, startIndex, count);
        }

        
    }
}
