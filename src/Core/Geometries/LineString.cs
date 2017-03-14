// <copyright file="LineString.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Algorithms;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a line string geometry in Cartesian coordinate space.
    /// </summary>
    public class LineString : Curve, ILineString
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
        /// The list of coordinates.
        /// </summary>
        private readonly List<Coordinate> coordinates;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        public LineString(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : base(precisionModel, referenceSystem)
        {
            this.coordinates = new List<Coordinate>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="source">The source coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public LineString(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IEnumerable<Coordinate> source)
            : base(precisionModel, referenceSystem)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            this.coordinates = new List<Coordinate>(source.Select(coordinate => this.PrecisionModel.MakePrecise(coordinate)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="factory">The factory of the line string.</param>
        public LineString(IGeometryFactory factory)
            : base(factory)
        {
            this.coordinates = new List<Coordinate>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString" /> class.
        /// </summary>
        /// <param name="factory">The factory of the line string.</param>
        /// <param name="source">The source coordinates.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The factory is null.
        /// </exception>
        public LineString(IGeometryFactory factory, IEnumerable<Coordinate> source)
            : base(factory)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            this.coordinates = new List<Coordinate>(source.Select(coordinate => this.PrecisionModel.MakePrecise(coordinate)));
        }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding envelope of the geometry.</value>
        public override Envelope Envelope
        {
            get { return Envelope.FromCoordinates(this.coordinates); }
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
        /// Gets the centroid of the line string.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public override sealed Coordinate Centroid { get { return this.PrecisionModel.MakePrecise(LineAlgorithms.Centroid(this.coordinates)); } }

        /// <summary>
        /// Gets a value indicating whether the curve is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public override sealed Boolean IsEmpty { get { return this.coordinates.Count == 0; } }

        /// <summary>
        /// Gets a value indicating whether the curve is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple
        {
            get
            {
                return !ShamosHoeyAlgorithm.Intersects(this.coordinates);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the curve is valid.
        /// </summary>
        /// <value><c>true</c> if all coordinates of the curve are valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get { return this.coordinates.All(coordinate => coordinate.IsValid); }
        }

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
                for (Int32 coordinateIndex = 0; coordinateIndex < this.coordinates.Count - 1; coordinateIndex++)
                    length += Coordinate.Distance(this.coordinates[coordinateIndex], this.coordinates[coordinateIndex + 1]);
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
                return this.Factory.CreatePoint(this.coordinates[this.coordinates.Count - 1]);
            }
        }

        /// <summary>
        /// Gets the number of coordinates in the line string.
        /// </summary>
        /// <value>The number of coordinates in the line string.</value>
        public Int32 Count { get { return this.coordinates.Count; } }

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
                return this.coordinates[0];
            }
        }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the curve.</value>
        public Coordinate EndCoordinate
        {
            get
            {
                if (this.Count == 0)
                    return Coordinate.Undefined;
                return this.coordinates[this.coordinates.Count - 1];
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
        public virtual Boolean Contains(Coordinate coordinate)
        {
            return this.coordinates.Contains(coordinate);
        }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual Coordinate GetCoordinate(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            return this.coordinates[index];
        }

        /// <summary>
        /// Sets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to set.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual void SetCoordinate(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            coordinate = this.PrecisionModel.MakePrecise(coordinate);

            this.coordinates[index] = coordinate;
        }

        /// <summary>
        /// Adds a coordinate to the end of the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        public virtual void Add(Coordinate coordinate)
        {
            this.coordinates.Add(this.PrecisionModel.MakePrecise(coordinate));
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
        /// <exception cref="System.ArgumentException">The coordinate is not valid.</exception>
        public virtual void Insert(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            this.coordinates.Insert(index, this.PrecisionModel.MakePrecise(coordinate));
        }

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the line string.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        public virtual Boolean Remove(Coordinate coordinate)
        {
            coordinate = this.PrecisionModel.MakePrecise(coordinate);

            return this.coordinates.Remove(coordinate);
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
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            this.coordinates.RemoveAt(index);
        }

        /// <summary>
        /// Removes all coordinates from the line string.
        /// </summary>
        public virtual void Clear()
        {
            this.coordinates.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{Coordinate}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            foreach (Coordinate coordinate in this.coordinates)
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
            if (this.IsEmpty)
                return name + CollectionEmptyString;

            StringBuilder builder = new StringBuilder();
            for (Int32 index = 0; index < this.coordinates.Count; index++)
            {
                if (index > 0)
                    builder.Append(CoordinateStringDivider);

                builder.Append(String.Format(provider, CoordinateStringFormat, this.coordinates[index].X, this.coordinates[index].Y, this.coordinates[index].Z));
            }

            return name + String.Format(provider, CollectionStringFormat, builder.ToString());
        }
    }
}
