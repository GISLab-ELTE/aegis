// <copyright file="IGeometryDriver.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a driver for reading and writing geometries of features.
    /// </summary>
    public interface IGeometryDriver : IDriver
    {
        /// <summary>
        /// Gets the reference system driver.
        /// </summary>
        /// <value>The reference system driver.</value>
        IReferenceSystemDriver ReferenceSystemDriver { get; }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used by the driver.</value>
        PrecisionModel PrecisionModel { get; }

        /// <summary>
        /// Gets the geometry factory.
        /// </summary>
        /// <value>The geometry factory used by the driver.</value>
        IGeometryFactory Factory { get; }

        /// <summary>
        /// Creates a coordinate for the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void CreateCoordinate(Coordinate coordinate, String identifier);

        /// <summary>
        /// Creates a coordinate for the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void CreateCoordinate(Coordinate coordinate, String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a coordinate for the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the geometry.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void CreateCoordinate(Coordinate coordinate, String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a collection of coordinates for the specified geometry.
        /// </summary>
        /// <param name="collection">The list of coordinates.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void CreateCoordinates(IReadOnlyList<Coordinate> collection, String identifier);

        /// <summary>
        /// Creates a collection of coordinates for the specified geometry.
        /// </summary>
        /// <param name="collection">The list of coordinates.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void CreateCoordinates(IReadOnlyList<Coordinate> collection, String identifier, params Int32[] indexes);

        /// <summary>
        /// Creates a collection of coordinates for the specified geometry.
        /// </summary>
        /// <param name="collection">The list of coordinates.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void CreateCoordinates(IReadOnlyList<Coordinate> collection, String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Creates a geometry.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The geometry created by the driver.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The operation is not supported by the driver.
        /// or
        /// The specified geometry is not supported by the driver.
        /// </exception>
        GeometryType CreateGeometry<GeometryType>(String identifier)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <returns>The geometry created by the driver.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The operation is not supported by the driver.
        /// or
        /// The specified geometry is not supported by the driver.
        /// </exception>
        GeometryType CreateGeometry<GeometryType>(String identifier, params Int32[] indexes)
            where GeometryType : IGeometry;

        /// <summary>
        /// Creates a geometry.
        /// </summary>
        /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <returns>The geometry created by the driver.</returns>
        /// <exception cref="System.NotSupportedException">
        /// The operation is not supported by the driver.
        /// or
        /// The specified geometry is not supported by the driver.
        /// </exception>
        GeometryType CreateGeometry<GeometryType>(String identifier, IEnumerable<Int32> indexes)
            where GeometryType : IGeometry;

        /// <summary>
        /// Reads the number of coordinate collections of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The number of coordinate collections within the feature.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Int32 ReadCollectionCount(String identifier);

        /// <summary>
        /// Reads the number of coordinate collections of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <returns>The number of coordinate collections within the feature at the specified indexes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Int32 ReadCollectionCount(String identifier, params Int32[] indexes);

        /// <summary>
        /// Reads the number of coordinate collections of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <returns>The number of coordinate collections within the feature at the specified indexes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Int32 ReadCollectionCount(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Reads a coordinate of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="index">The index of the coordinate.</param>
        /// <returns>The coordinate.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Coordinate ReadCoordinate(String identifier, Int32 index);

        /// <summary>
        /// Reads a coordinate of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate.</param>
        /// <returns>The coordinate.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Coordinate ReadCoordinate(String identifier, params Int32[] indexes);

        /// <summary>
        /// Reads a coordinate of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate.</param>
        /// <returns>The coordinate.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Coordinate ReadCoordinate(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Reads the number of coordinate of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The number of coordinates within the geometry.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Int32 ReadCoordinateCount(String identifier);

        /// <summary>
        /// Reads the number of coordinate of at the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <returns>The number of coordinates within the feature at the specified indexes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Int32 ReadCoordinateCount(String identifier, params Int32[] indexes);

        /// <summary>
        /// Reads the number of coordinate of at the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <returns>The number of coordinates within the feature at the specified indexes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Int32 ReadCoordinateCount(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Reads the coordinates of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The list of coordinates.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IReadOnlyList<Coordinate> ReadCoordinates(String identifier);

        /// <summary>
        /// Reads the coordinates of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <returns>The list of coordinates at the specified indexes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IReadOnlyList<Coordinate> ReadCoordinates(String identifier, params Int32[] indexes);

        /// <summary>
        /// Reads the coordinates of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <returns>The list of coordinates at the specified indexes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IReadOnlyList<Coordinate> ReadCoordinates(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Reads all envelopes.
        /// </summary>
        /// <returns>The list of envelopes.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IReadOnlyList<Envelope> ReadEnvelopes();

        /// <summary>
        /// Reads the envelope of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The envelope of the geometry.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Envelope ReadEnvelope(String identifier);

        /// <summary>
        /// Reads the envelope of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The envelope of the geometry.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Envelope ReadEnvelope(String identifier, params Int32[] indexes);

        /// <summary>
        /// Reads the envelope of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <returns>The envelope of the geometry.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        Envelope ReadEnvelope(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Reads the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <returns>The geometry read by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IGeometry ReadGeometry(String identifier);

        /// <summary>
        /// Reads the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate within the feature.</param>
        /// <returns>The geometry read by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IGeometry ReadGeometry(String identifier, params Int32[] indexes);

        /// <summary>
        /// Reads the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate within the feature.</param>
        /// <returns>The geometry read by the driver.</returns>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        IGeometry ReadGeometry(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Updates a coordinate of the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="index">The index of the coordinate.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateCoordinate(Coordinate coordinate, String identifier, Int32 index);

        /// <summary>
        /// Updates a coordinate of the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateCoordinate(Coordinate coordinate, String identifier, params Int32[] indexes);

        /// <summary>
        /// Updates a coordinate of the specified geometry.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateCoordinate(Coordinate coordinate, String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Updates the coordinates of the specified geometry.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateCoordinates(IReadOnlyList<Coordinate> coordinates, String identifier);

        /// <summary>
        /// Updates the coordinates of the specified geometry.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateCoordinates(IReadOnlyList<Coordinate> coordinates, String identifier, params Int32[] indexes);

        /// <summary>
        /// Updates the coordinates of the specified geometry.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinate collection within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateCoordinates(IReadOnlyList<Coordinate> coordinates, String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Updates the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateGeometry(IGeometry geometry, String identifier);

        /// <summary>
        /// Updates the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateGeometry(IGeometry geometry, String identifier, params Int32[] indexes);

        /// <summary>
        /// Updates the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void UpdateGeometry(IGeometry geometry, String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Deletes a coordinate of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="index">The index of the coordinate.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteCoordinate(String identifier, Int32 index);

        /// <summary>
        /// Deletes a coordinate of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteCoordinate(String identifier, params Int32[] indexes);

        /// <summary>
        /// Deletes a coordinate of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteCoordinate(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Deletes the coordinates of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteCoordinates(String identifier);

        /// <summary>
        /// Deletes the coordinates of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteCoordinates(String identifier, params Int32[] indexes);

        /// <summary>
        /// Deletes the coordinates of the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteCoordinates(String identifier, IEnumerable<Int32> indexes);

        /// <summary>
        /// Deletes the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteGeometry(String identifier);

        /// <summary>
        /// Deletes the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteGeometry(String identifier, params Int32[] indexes);

        /// <summary>
        /// Deletes the specified geometry.
        /// </summary>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the coordinates within the feature.</param>
        /// <exception cref="System.NotSupportedException">The operation is not supported by the driver.</exception>
        void DeleteGeometry(String identifier, IEnumerable<Int32> indexes);
    }
}
