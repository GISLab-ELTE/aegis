// <copyright file="StoredGeometry.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ELTE.AEGIS.Collections;
    using ELTE.AEGIS.Resources;
    using ELTE.AEGIS.Storage.Resources;

    /// <summary>
    /// Represents a geometry located in a store.
    /// </summary>
    public abstract class StoredGeometry : IStoredGeometry
    {
        /// <summary>
        /// The empty array of indexes. This field is read-only.
        /// </summary>
        private static readonly Int32[] EmptyIndexes = new Int32[0];

        /// <summary>
        /// The array of indexes.
        /// </summary>
        private Int32[] indexes;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometry" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        protected StoredGeometry(StoredGeometryFactory factory, String identifier, IEnumerable<Int32> indexes)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            this.Factory = factory;
            this.Identifier = identifier;
            this.indexes = indexes == null ? EmptyIndexes : indexes.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometry" /> class.
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
        protected StoredGeometry(PrecisionModel precisionModel, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver), StorageMessages.DriverIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            this.Factory = new StoredGeometryFactory(driver, precisionModel);
            this.Identifier = identifier;
            this.indexes = indexes == null ? EmptyIndexes : indexes.ToArray();
        }

        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        IGeometryFactory IGeometry.Factory { get { return this.Factory; } }

        /// <summary>
        /// Gets the precision model of the geometry.
        /// </summary>
        /// <value>The precision model of the geometry.</value>
        public PrecisionModel PrecisionModel { get { return this.Factory.PrecisionModel; } }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>The inherent dimension of the geometry.</value>
        public abstract Int32 Dimension { get; }

        /// <summary>
        /// Gets the coordinate dimension of the geometry.
        /// </summary>
        /// <value>The coordinate dimension of the geometry. The coordinate dimension is equal to the dimension of the reference system, if provided.</value>
        public virtual Int32 CoordinateDimension { get { return (this.ReferenceSystem != null) ? this.ReferenceSystem.Dimension : this.SpatialDimension; } }

        /// <summary>
        /// Gets the spatial dimension of the geometry.
        /// </summary>
        /// <value>The spatial dimension of the geometry. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        public virtual Int32 SpatialDimension { get { return (this.Envelope.Minimum.Z != 0 || this.Envelope.Maximum.Z != 0) ? 3 : 2; } }

        /// <summary>
        /// Gets the reference system of the geometry.
        /// </summary>
        /// <value>The reference system of the geometry.</value>
        public IReferenceSystem ReferenceSystem { get { return this.Factory.ReferenceSystem; } }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public Envelope Envelope
        {
            get
            {
                return this.indexes.Length == 0 ? this.Driver.ReadEnvelope(this.Identifier) : this.Driver.ReadEnvelope(this.Identifier, this.indexes);
            }
        }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public abstract IGeometry Boundary { get; }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        public abstract Coordinate Centroid { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public abstract Boolean IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public abstract Boolean IsSimple { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public abstract Boolean IsValid { get; }

        /// <summary>
        /// Gets the feature identifier.
        /// </summary>
        /// <value>The unique feature identifier within the store.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the collection of indexes within the feature.
        /// </summary>
        /// <value>The collection of indexes which determine the location of the geometry within the feature.</value>
        public IEnumerable<Int32> Indexes { get { return this.indexes; } }

        /// <summary>
        /// Gets the driver of the geometry.
        /// </summary>
        /// <value>The driver of the geometry.</value>
        public IGeometryDriver Driver { get { return this.Factory.Driver; } }

        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        public IStoredGeometryFactory Factory { get; private set; }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override sealed String ToString()
        {
            return this.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public abstract String ToString(IFormatProvider provider);

        /// <summary>
        /// Creates a coordinate for the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        protected void CreateCoordinate(Coordinate coordinate)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinate(coordinate, this.Identifier);
            else
                this.Driver.CreateCoordinate(coordinate, this.Identifier, this.indexes);
        }

        /// <summary>
        /// Creates a coordinate for the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="index">The index of the coordinate within the geometry.</param>
        protected void CreateCoordinate(Coordinate coordinate, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinate(coordinate, this.Identifier, index);
            else
                this.Driver.CreateCoordinate(coordinate, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Creates a collection of coordinates for the geometry.
        /// </summary>
        /// <param name="collection">The list of coordinates.</param>
        protected void CreateCoordinates(IReadOnlyList<Coordinate> collection)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinates(collection, this.Identifier);
            else
                this.Driver.CreateCoordinates(collection, this.Identifier, this.indexes);
        }

        /// <summary>
        /// Creates a collection of coordinates for the geometry.
        /// </summary>
        /// <param name="collection">The list of coordinates.</param>
        /// <param name="index">The starting index of the coordinates within the geometry.</param>
        protected void CreateCoordinates(IReadOnlyList<Coordinate> collection, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.CreateCoordinates(collection, this.Identifier, index);
            else
                this.Driver.CreateCoordinates(collection, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the number of coordinate collections of the geometry.
        /// </summary>
        /// <returns>The number of coordinate collections within the geometry.</returns>
        protected Int32 ReadCollectionCount()
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCollectionCount(this.Identifier);
            else
                return this.Driver.ReadCollectionCount(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Reads the number of coordinate collections of the geometry.
        /// </summary>
        /// <param name="index">The starting index of the coordinates within the geometry.</param>
        /// <returns>The number of coordinate collections within the geometry.</returns>
        protected Int32 ReadCollectionCount(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCollectionCount(this.Identifier, index);

            return this.Driver.ReadCollectionCount(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads a coordinate of the specified geometry.
        /// </summary>
        /// <param name="index">The index of the coordinate.</param>
        /// <returns>The coordinate.</returns>
        protected Coordinate ReadCoordinate(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinate(this.Identifier, index);

            return this.Driver.ReadCoordinate(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the number of coordinate of the geometry.
        /// </summary>
        /// <returns>The number of coordinates within the geometry.</returns>
        protected Int32 ReadCoordinateCount()
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinateCount(this.Identifier);

            return this.Driver.ReadCoordinateCount(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Reads the number of coordinate of the geometry.
        /// </summary>
        /// <param name="index">The index of the coordinates within the geometry.</param>
        /// <returns>The number of coordinates within the geometry.</returns>
        protected Int32 ReadCoordinateCount(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinateCount(this.Identifier, index);

            return this.Driver.ReadCoordinateCount(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the coordinates of the geometry.
        /// </summary>
        /// <returns>The list of coordinates.</returns>
        protected IReadOnlyList<Coordinate> ReadCoordinates()
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinates(this.Identifier);

            return this.Driver.ReadCoordinates(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Reads the coordinates of the geometry.
        /// </summary>
        /// <param name="index">The index of the coordinates within the geometry.</param>
        /// <returns>The list of coordinates.</returns>
        protected IReadOnlyList<Coordinate> ReadCoordinates(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadCoordinates(this.Identifier, index);

            return this.Driver.ReadCoordinates(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Reads the specified inner geometry.
        /// </summary>
        /// <param name="index">The index of the inner geometry within the geometry.</param>
        /// <returns>The geometry read by the driver.</returns>
        protected IGeometry ReadGeometry(Int32 index)
        {
            if (this.indexes.Length == 0)
                return this.Driver.ReadGeometry(this.Identifier, index);
            else
                return this.Driver.ReadGeometry(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Updates a coordinate of the geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="index">The index of the coordinate within the geometry.</param>
        protected void UpdateCoordinate(Coordinate coordinate, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.UpdateCoordinate(coordinate, this.Identifier, index);
            else
                this.Driver.UpdateCoordinate(coordinate, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Updates the coordinates of the geometry.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        protected void UpdateCoordinates(IReadOnlyList<Coordinate> coordinates)
        {
            if (this.indexes.Length == 0)
                this.Driver.UpdateCoordinates(coordinates, this.Identifier);
            else
                this.Driver.UpdateCoordinates(coordinates, this.Identifier, this.indexes);
        }

        /// <summary>
        /// Deletes the coordinates of the geometry.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="index">The index of the coordinate collection within the geometry.</param>
        protected void UpdateCoordinates(IReadOnlyList<Coordinate> coordinates, Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.UpdateCoordinates(coordinates, this.Identifier, index);
            else
                this.Driver.UpdateCoordinates(coordinates, this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Deletes a coordinate of the specified geometry.
        /// </summary>
        /// <param name="index">The index of the coordinate.</param>
        protected void DeleteCoordinate(Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.DeleteCoordinate(this.Identifier, index);
            else
                this.Driver.DeleteCoordinate(this.Identifier, this.indexes.Append(index));
        }

        /// <summary>
        /// Deletes the coordinates of the geometry.
        /// </summary>
        protected void DeleteCoordinates()
        {
            if (this.indexes.Length == 0)
                this.Driver.DeleteCoordinates(this.Identifier);
            else
                this.Driver.DeleteCoordinates(this.Identifier, this.indexes);
        }

        /// <summary>
        /// Deletes the coordinates of the geometry.
        /// </summary>
        /// <param name="index">The index of the coordinate collection within the geometry.</param>
        protected void DeleteCoordinates(Int32 index)
        {
            if (this.indexes.Length == 0)
                this.Driver.DeleteCoordinates(this.Identifier, index);
            else
                this.Driver.DeleteCoordinates(this.Identifier, this.indexes.Append(index));
        }
    }
}
