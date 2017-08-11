// <copyright file="GeometryList.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Geometries
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AEGIS.Collections.Resources;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a generic list of geometries in Cartesian coordinate space.
    /// </summary>
    /// <typeparam name="GeometryType">The type of geometries in the list.</typeparam>
    public class GeometryList<GeometryType> : Geometry, IGeometryCollection<GeometryType>, IList<GeometryType>
        where GeometryType : IGeometry
    {
        /// <summary>
        /// The divider for items. This field is constant.
        /// </summary>
        private const String ItemStringDivider = ",";

        /// <summary>
        /// The string for empty collections. This field is constant.
        /// </summary>
        private const String CollectionEmptyString = " EMPTY";

        /// <summary>
        /// The string format for collections. This field is constant.
        /// </summary>
        private const String CollectionStringFormat = " ({0})";

        /// <summary>
        /// The name of the geometry collections. This field is constant.
        /// </summary>
        private const String GeometryCollectionName = "GEOMETRYCOLLECTION";

        /// <summary>
        /// The list of items.
        /// </summary>
        private List<GeometryType> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{GeometryType}" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        public GeometryList(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : base(precisionModel, referenceSystem)
        {
            this.items = new List<GeometryType>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{GeometryType}" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="capacity">The number of elements that the list can initially store.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public GeometryList(PrecisionModel precisionModel, IReferenceSystem referenceSystem, Int32 capacity)
            : base(precisionModel, referenceSystem)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), AEGIS.Collections.Resources.CollectionMessages.CapacityLessThan0);

            this.items = new List<GeometryType>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryList{GeometryType}" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="source">The source of geometries.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public GeometryList(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IEnumerable<GeometryType> source)
            : base(precisionModel, referenceSystem)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // FIXME: source geometries may have different precision and reference system, thus this breaks consistency
            this.items = new List<GeometryType>(source);
        }

        /// <summary>
        /// Gets the inherent dimension of the geometry list.
        /// </summary>
        /// <value>The maximum inherent dimension of all geometries within the collection.</value>
        public override Int32 Dimension { get { return (this.items.Count == 0) ? 0 : this.items.Max(geometry => geometry.Dimension); } }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding envelope of the geometry.</value>
        public override Envelope Envelope
        {
            get
            {
                if (this.items.Count == 0)
                    return Envelope.Undefined;
                else if (this.items.Count == 1)
                    return this.items[0].Envelope;
                else
                    return Envelope.FromEnvelopes(this.items.Select(geometry => geometry.Envelope));
            }
        }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public override IGeometry Boundary
        {
            get
            {
                if (this.items.Count == 0)
                    return null;

                if (this.items.Count == 1)
                    return this.items[0].Boundary as Geometry;

                List<IGeometry> boundaryList = new List<IGeometry>();
                foreach (GeometryType geometry in this.items)
                {
                    if (geometry != null && !(geometry is IPoint))
                    {
                        // check whether the boundary contains multiple parts (e.g. polygon)

                        if (geometry.Boundary is IEnumerable<IGeometry> boundaryCollection)
                        {
                            // only the parts should be added to the boundary (so that it is not recursive)
                            foreach (IGeometry boundaryPart in boundaryCollection)
                                boundaryList.Add(boundaryPart);
                        }
                        else
                        {
                            boundaryList.Add(geometry.Boundary);
                        }
                    }
                }

                if (boundaryList.Count == 0)
                    return null;
                if (boundaryList.Count == 1)
                    return boundaryList[0];

                return new GeometryList<IGeometry>(this.PrecisionModel, this.ReferenceSystem, boundaryList);
            }
        }

        /// <summary>
        /// Gets the centroid of the geometry list.
        /// </summary>
        /// <value>The centroid of the geometry list.</value>
        public override Coordinate Centroid
        {
            get
            {
                if (this.items.Count == 0)
                    return Coordinate.Undefined;
                if (this.items.Count == 1)
                    return this.items[0].Centroid;

                return this.PrecisionModel.MakePrecise(Coordinate.Centroid(this.items.Select(x => x.Centroid)));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry list is empty.
        /// </summary>
        /// <value><c>true</c> if all geometries in the geometry list are empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty
        {
            get { return this.items.Count == 0 || this.items.All(x => x.IsEmpty); }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry list is simple.
        /// </summary>
        /// <value><c>true</c> if all geometries in the geometry list are simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple
        {
            get { return this.items.Count == 0 || this.items.All(geometry => geometry.IsSimple); }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry list is valid.
        /// </summary>
        /// <value><c>true</c> if all geometries in the geometry list are valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get { return this.items.Count == 0 || this.items.All(geometry => geometry.IsValid); }
        }

        /// <summary>
        /// Gets the number of geometries contained in the geometry list.
        /// </summary>
        /// <value>
        /// The number of geometries contained in the geometry list.
        /// </value>
        public virtual Int32 Count { get { return this.items.Count; } }

        /// <summary>
        /// Gets or sets the geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to get.</param>
        /// <returns>The geometry located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        IGeometry IGeometryCollection.this[Int32 index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
                if (index >= this.items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

                return this.items[index];
            }
        }

        /// <summary>
        /// Gets or sets the geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to get or set.</param>
        /// <returns>The geometry located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        public virtual GeometryType this[Int32 index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
                if (index >= this.items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);
                return this.items[index];
            }

            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
                if (index >= this.items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

                if (!ReferenceEquals(this.items[index], value))
                {
                    this.items[index] = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry list is read-only.
        /// </summary>
        /// <value><c>true</c> if the geometry list is read-only; otherwise, <c>false</c>.</value>
        public virtual Boolean IsReadOnly { get { return false; } }

        /// <summary>
        /// Determines the index of a specific geometry in the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The zero-based index of geometry if found in the list; otherwise, -1.</returns>
        public virtual Int32 IndexOf(GeometryType geometry)
        {
            return this.items.IndexOf(geometry);
        }

        /// <summary>
        /// Inserts a geometry to the geometry list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the <paramref name="geometry" /> should be inserted.</param>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        /// <exception cref="System.ArgumentException">The reference system of the geometry does not match the reference system of the collection.</exception>
        public virtual void Insert(Int32 index, GeometryType geometry)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            this.items.Insert(index, geometry);
        }

        /// <summary>
        /// Removes the geometry list geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        public virtual void RemoveAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            this.items.RemoveAt(index);
        }

        /// <summary>
        /// Determines whether a geometry is in the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if item is found in the geometry list; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public virtual Boolean Contains(GeometryType geometry)
        {
            return this.items.Contains(geometry);
        }

        /// <summary>
        /// Adds a geometry to the end of the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the geometry does not match the reference system of the collection.</exception>
        public virtual void Add(GeometryType geometry)
        {
            this.items.Add(geometry);
        }

        /// <summary>
        /// Removes the first occurrence of a specified geometry from the geometry list.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the <paramref name="geometry" /> is successfully removed; otherwise, <c>false</c>.</returns>
        public virtual Boolean Remove(GeometryType geometry)
        {
            return this.items.Remove(geometry);
        }

        /// <summary>
        /// Removes all geometries from the geometry list.
        /// </summary>
        public virtual void Clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Copies the geometries of the geometry list to a <see cref="System.Array" />, starting at a particular <see cref="System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array" /> that is the destination of the elements copied from geometry list.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Index is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source collection is greater than the available space from the array index to the end of the destination array.</exception>
        public void CopyTo(GeometryType[] array, Int32 arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), CollectionMessages.IndexIsLessThan0);
            if (arrayIndex + this.items.Count > array.Length)
                throw new ArgumentException(AEGIS.Collections.Resources.CollectionMessages.ArrayIndexIsGreaterThanSpace, nameof(array));

            this.items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the geometry list.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{T}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<GeometryType> GetEnumerator()
        {
            foreach (GeometryType item in this.items)
                yield return item;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the geometry list.
        /// </summary>
        /// <returns>A <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (GeometryType item in this.items)
                yield return item;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, GeometryCollectionName);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="name">The name of the geometry.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        protected String ToString(IFormatProvider provider, String name)
        {
            if (this.IsEmpty)
                return name + CollectionEmptyString;

            StringBuilder builder = new StringBuilder();
            for (Int32 index = 0; index < this.items.Count; index++)
            {
                if (index > 0)
                    builder.Append(ItemStringDivider);

                builder.Append(this.items[index].ToString(provider));
            }

            return name + String.Format(provider, CollectionStringFormat, builder.ToString());
        }
    }
}
