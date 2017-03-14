// <copyright file="StoredLineString.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Geometries
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AEGIS.Algorithms;
    using AEGIS.Collections.Resources;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a line string geometry located in a store.
    /// </summary>
    public class StoredLineString : StoredCurve, ILineString
    {
        /// <summary>
        /// The string format for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringFormat = "{0} {1} {2}";

        /// <summary>
        /// The divider for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringDivider = ",";

        /// <summary>
        /// The string for empty collection of coordinates. This field is constant.
        /// </summary>
        private const String CollectionEmptyString = " EMPTY";

        /// <summary>
        /// The string format for the collection of coordinates. This field is constant.
        /// </summary>
        private const String CollectionStringFormat = " ({0})";

        /// <summary>
        /// The name of the line string. This field is constant.
        /// </summary>
        private const String LineStringName = "LINESTRING";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredLineString" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredLineString(PrecisionModel precisionModel, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, driver, identifier, indexes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredLineString" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredLineString(StoredGeometryFactory factory, String identifier, IEnumerable<Int32> indexes)
            : base(factory, identifier, indexes)
        {
        }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public override IGeometry Boundary
        {
            get
            {
                if (this.IsClosed)
                {
                    return null;
                }
                else
                {
                    return this.Factory.CreateMultiPoint(new IPoint[]
                    {
                        this.Factory.CreatePoint(this.StartCoordinate),
                        this.Factory.CreatePoint(this.EndCoordinate)
                    });
                }
            }
        }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public override Coordinate Centroid { get { return this.PrecisionModel.MakePrecise(LineAlgorithms.Centroid(this.Coordinates)); } }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty { get { return this.Count == 0; } }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple { get { return !ShamosHoeyAlgorithm.Intersects(this.Coordinates); } }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid { get { return this.Coordinates.All(coordinate => coordinate.IsValid); } }

        /// <summary>
        /// Gets a value indicating whether the curve is closed.
        /// </summary>
        /// <value><c>true</c> if the starting and ending coordinates are equal; otherwise, <c>false</c>.</value>
        public override Boolean IsClosed { get { return this.Count > 0 && this.StartCoordinate == this.EndCoordinate; } }

        /// <summary>
        /// Gets the length of the line string.
        /// </summary>
        /// <value>The length of the line string.</value>
        public override Double Length
        {
            get
            {
                Double length = 0;
                Coordinate current = this.ReadCoordinate(0);
                for (Int32 index = 1; index < this.Count; index++)
                {
                    Coordinate next = this.ReadCoordinate(index + 1);
                    length += Coordinate.Distance(current, next);
                    current = next;
                }

                return length;
            }
        }

        /// <summary>
        /// Gets the staring point.
        /// </summary>
        /// <value>The first point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        public override IPoint StartPoint
        {
            get
            {
                if (this.Count == 0)
                    return null;
                return this.Factory.CreatePoint(this.StartCoordinate);
            }
        }

        /// <summary>
        /// Gets the ending point.
        /// </summary>
        /// <value>The last point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        public override IPoint EndPoint
        {
            get
            {
                if (this.Count == 0)
                    return null;
                return this.Factory.CreatePoint(this.EndCoordinate);
            }
        }

        /// <summary>
        /// Gets the number of coordinates in the line string.
        /// </summary>
        /// <value>The number of coordinates in the line string.</value>
        public Int32 Count { get { return this.ReadCoordinateCount(); } }

        /// <summary>
        /// Gets the coordinates in the line string.
        /// </summary>
        /// <value>The list of coordinates of the line string.</value>
        public IReadOnlyList<Coordinate> Coordinates { get { return this.ReadCoordinates(); } }

        /// <summary>
        /// Gets the staring coordinate.
        /// </summary>
        /// <value>The first coordinate of the line string.</value>
        public Coordinate StartCoordinate
        {
            get
            {
                if (this.Count == 0)
                    return Coordinate.Undefined;

                return this.ReadCoordinate(0);
            }
        }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the line string.</value>
        public Coordinate EndCoordinate
        {
            get
            {
                if (this.Count == 0)
                    return Coordinate.Undefined;

                return this.ReadCoordinate(this.Count - 1);
            }
        }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <value>The coordinate located at the specified index.</value>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        public Coordinate this[Int32 index]
        {
            get
            {
                return this.GetCoordinate(index);
            }
        }

        /// <summary>
        /// Determines whether the line string contains the specified coordinate within its coordinates.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line string contains the specified coordinate within its coordinates; otherwise, <c>false</c>.</returns>
        public Boolean Contains(Coordinate coordinate)
        {
            coordinate = this.PrecisionModel.MakePrecise(coordinate);

            Int32 count = this.ReadCoordinateCount();
            for (Int32 index = 0; index < count; index++)
            {
                if (coordinate == this.ReadCoordinate(index))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public Coordinate GetCoordinate(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            return this.ReadCoordinate(index);
        }

        /// <summary>
        /// Sets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to set.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual void SetCoordinate(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            this.UpdateCoordinate(this.PrecisionModel.MakePrecise(coordinate), index);
        }

        /// <summary>
        /// Adds a coordinate to the end of the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        public virtual void Add(Coordinate coordinate)
        {
            this.CreateCoordinate(this.PrecisionModel.MakePrecise(coordinate));
        }

        /// <summary>
        /// Adds a collection of coordinates to the end of the line string.
        /// </summary>
        /// <param name="collection">The collection of coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public virtual void Add(IEnumerable<Coordinate> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            this.CreateCoordinates(this.PrecisionModel.MakePrecise(collection).ToList());
        }

        /// <summary>
        /// Inserts a coordinate into the line string at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual void Insert(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            this.CreateCoordinate(coordinate, index);
        }

        /// <summary>
        /// Inserts a collection of coordinates into the line string at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="collection">The collection of coordinates.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public virtual void Insert(Int32 index, IEnumerable<Coordinate> collection)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            // TODO: enhance performance with insert options in the driver
            List<Coordinate> coordinates = this.ReadCoordinates().ToList();
            coordinates.InsertRange(index, collection);
            this.UpdateCoordinates(coordinates);
        }

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        public virtual Boolean Remove(Coordinate coordinate)
        {
            coordinate = this.PrecisionModel.MakePrecise(coordinate);

            Int32 count = this.ReadCoordinateCount();
            for (Int32 index = 0; index < count; index++)
            {
                if (coordinate == this.ReadCoordinate(index))
                {
                    this.DeleteCoordinate(index);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the coordinate at the specified index from the line string.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual void RemoveAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            this.DeleteCoordinate(index);
        }

        /// <summary>
        /// Removes all coordinates from the line string.
        /// </summary>
        public void Clear()
        {
            this.DeleteCoordinates();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{Coordinate}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            foreach (Coordinate coordinate in this.ReadCoordinates())
                yield return coordinate;
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
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, LineStringName);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="name">The name of the line string.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        protected String ToString(IFormatProvider provider, String name)
        {
            if (!this.Driver.SupportedOperations.Contains(DriverOperation.Read))
                return name;

            if (this.IsEmpty)
                return name + CollectionEmptyString;

            StringBuilder builder = new StringBuilder();
            for (Int32 index = 0; index < this.ReadCoordinateCount(); index++)
            {
                if (index > 0)
                    builder.Append(CoordinateStringDivider);

                Coordinate coordinate = this.Driver.ReadCoordinate(this.Identifier, index);
                builder.Append(String.Format(provider, CoordinateStringFormat, coordinate.X, coordinate.Y, coordinate.Z));
            }

            return name + String.Format(provider, CollectionStringFormat, builder.ToString());
        }
    }
}
