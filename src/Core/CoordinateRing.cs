// <copyright file="CoordinateRing.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a coordinate ring of a shell with a consistent representation for equal rings.
    /// </summary>
    /// <author>Máté Cserép</author>
    public class CoordinateRing : IEnumerable<Coordinate>, IEquatable<CoordinateRing>
    {
        #region Private fields

        /// <summary>
        /// Stored the coordinates of the ring.
        /// </summary>
        private readonly List<Coordinate> _elements;

        /// <summary>
        /// Stores the index of the starting coordinate.
        /// </summary>
        private readonly Int32 _shift;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the number of elements contained in the <see cref="CoordinateRing"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="CoordinateRing"/>.
        /// </returns>
        public Int32 Count
        {
            get { return _elements.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CoordinateRing"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="CoordinateRing"/> is read-only; otherwise, false.
        /// </returns>
        public Boolean IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Return the coordinates in their original order.
        /// </summary>
        /// <value>The original input.</value>
        public IReadOnlyList<Coordinate> Origin
        {
            get { return _elements; }
        }

        #endregion

        #region Public indexer

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="CoordinateRing"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="CoordinateRing"/> is read-only.</exception>
        public Coordinate this[Int32 index]
        {
            get { return _elements[Shift(index)]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateRing"/> class.
        /// </summary>
        /// <param name="elements">The input coordinates of the ring.</param>
        /// <exception cref="System.NotImplementedException">CoordinateRing is not implemented for lists with no unique coordinate.</exception>
        public CoordinateRing(IEnumerable<Coordinate> elements)
        {
            _elements = new List<Coordinate>(elements);
            if (_elements.Count > 0 && _elements.First() == _elements.Last())
                _elements.RemoveAt(Count - 1);

            _shift = 0;
            if (_elements.Count > 1)
            {
                var sortedElements = _elements
                    .OrderBy(item => item.X)
                    .ThenBy(item => item.Y)
                    .ThenBy(item => item.Z).ToList();

                Int32 count;
                while (_shift < _elements.Count &&
                       (count = _elements.Count(item => item.Equals(sortedElements[_shift % Count]))) > 1)
                    _shift += count;

                if (_shift == _elements.Count)
                    throw new NotImplementedException("CoordinateRing is not implemented for inputs with no unique coordinate.");

                _shift = _elements.IndexOf(sortedElements[_shift]);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the <see cref="CoordinateRing"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="CoordinateRing"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="CoordinateRing"/>.</param>
        public Boolean Contains(Coordinate item)
        {
            return _elements.Contains(item);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="CoordinateRing"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="CoordinateRing"/>.</param>
        public Int32 IndexOf(Coordinate item)
        {
            return Shift(_elements.IndexOf(item));
        }

        /// <summary>
        /// Copies the elements of the <see cref="CoordinateRing"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="CoordinateRing"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="CoordinateRing"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(Coordinate[] array, Int32 arrayIndex)
        {
            for (Int32 index = 0; index < Count; ++index)
                array[arrayIndex + index] = _elements[Shift(index)];
        }

        #endregion

        #region Public IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            for (Int32 index = 0; index < Count; ++index)
                yield return _elements[Shift(index)];
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Public IEquatable methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public Boolean Equals(CoordinateRing other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (ReferenceEquals(other, null))
                return false;
            if (Count != other.Count)
                return false;

            Boolean match = true;
            IEnumerator<Coordinate> enumerator = GetEnumerator();
            IEnumerator<Coordinate> otherEnumerator = other.GetEnumerator();
            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                if (!enumerator.Current.Equals(otherEnumerator.Current))
                {
                    match = false;
                    break;
                }
            }
            if (match) return true;

            enumerator = GetReverseEnumerator();
            otherEnumerator = other.GetEnumerator();
            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                if (!enumerator.Current.Equals(otherEnumerator.Current))
                    return false;
            }
            return true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Return the properly shifted index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The shifted index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The index exceeds the size of the collection.</exception>
        private Int32 Shift(Int32 index)
        {
            if(index >= Count)
                throw new ArgumentOutOfRangeException("index", "The index exceeds the size of the collection.");
            return (_shift + index) % Count;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection in reversed order.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        private IEnumerator<Coordinate> GetReverseEnumerator()
        {
            if (Count == 0)
                yield break;

            yield return _elements[Shift(0)];
            for (Int32 index = Count - 1; index > 0; --index)
                yield return _elements[Shift(index)];
        }

        #endregion
    }
}
