// <copyright file="StoredLinearRing.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;
    using Collections.Resources;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a linear ring geometry located in a store.
    /// </summary>
    public class StoredLinearRing : StoredLineString, ILinearRing
    {
        /// <summary>
        /// The name of the linear ring. This field is constant.
        /// </summary>
        private const String LinearRingName = "LINEARRING";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredLinearRing" /> class.
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
        public StoredLinearRing(PrecisionModel precisionModel, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, driver, identifier, indexes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredLinearRing" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredLinearRing(StoredGeometryFactory factory, String identifier, IEnumerable<Int32> indexes)
            : base(factory, identifier, indexes)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the linear ring is closed.
        /// </summary>
        /// <value><c>true</c>, as linear ring is always considered to be closed.</value>
        public override Boolean IsClosed { get { return true; } }

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
        public override void SetCoordinate(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            if (index == 0 || index == this.Count - 1)
            {
                base.SetCoordinate(0, coordinate);
                base.SetCoordinate(this.Count - 1, coordinate);
            }
            else
            {
                base.SetCoordinate(index, coordinate);
            }
        }

        /// <summary>
        /// Adds a coordinate to the end of the linear ring.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        public override void Add(Coordinate coordinate)
        {
            if (this.Count == 0)
            {
                base.Add(coordinate);
                base.Add(coordinate);
            }
            else
            {
                base.Insert(this.Count - 1, coordinate);
            }
        }

        /// <summary>
        /// Adds a collection of coordinates to the end of the line string.
        /// </summary>
        /// <param name="collection">The collection of coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public override void Add(IEnumerable<Coordinate> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), CollectionMessages.CollectionIsNull);

            if (this.Count == 0)
            {
                base.Add(collection);
                base.Add(collection);
            }
            else
            {
                this.Insert(this.Count - 1, collection);
            }
        }

        /// <summary>
        /// Inserts a coordinate into the linear ring at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is greater than the number of coordinates.
        /// </exception>
        public override void Insert(Int32 index, Coordinate coordinate)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            if (index == 0)
            {
                base.Insert(0, coordinate);
                base.SetCoordinate(this.Count - 1, coordinate);
            }
            else
            {
                base.Insert(index, coordinate);
            }
        }

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the linear ring.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.InvalidOperationException">A linear ring must contain at least two coordinates.</exception>
        public override Boolean Remove(Coordinate coordinate)
        {
            coordinate = this.PrecisionModel.MakePrecise(coordinate);

            if (coordinate == this.StartCoordinate)
            {
                if (this.Count == 2)
                {
                    this.Clear();
                }
                else
                {
                    base.RemoveAt(0);
                    base.SetCoordinate(this.Count - 1, this.StartCoordinate);
                }

                return true;
            }
            else
            {
                return base.Remove(coordinate);
            }
        }

        /// <summary>
        /// Removes the coordinate at the specified index from the linear ring.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is equal to or greater than the number of coordinates.
        /// </exception>
        public override void RemoveAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanNumberOfCoordinates);

            if (this.Count == 2)
            {
                this.Clear();
            }
            else if (index == 0 || index == this.Count - 1)
            {
                base.RemoveAt(0);
                base.SetCoordinate(this.Count - 1, this.StartCoordinate);
            }
            else
            {
                base.RemoveAt(index);
            }
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, LinearRingName);
        }
    }
}
