// <copyright file="Line.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Geometries
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a line geometry in Cartesian coordinate space.
    /// </summary>
    public class Line : LineString, ILine
    {
        /// <summary>
        /// The name of the line. This field is constant.
        /// </summary>
        private const String LineName = "LINE";

        /// <summary>
        /// Initializes a new instance of the <see cref="Line" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="startCoordinate">The starting coordinate.</param>
        /// <param name="endCoordinate">The ending coordinate.</param>
        public Line(PrecisionModel precisionModel, IReferenceSystem referenceSystem, Coordinate startCoordinate, Coordinate endCoordinate)
            : base(precisionModel, referenceSystem, new List<Coordinate> { startCoordinate, endCoordinate })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line" /> class.
        /// </summary>
        /// <param name="factory">The factory of the line.</param>
        /// <param name="startCoordinate">The starting coordinate.</param>
        /// <param name="endCoordinate">The ending coordinate.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public Line(IGeometryFactory factory, Coordinate startCoordinate, Coordinate endCoordinate)
            : base(factory, new List<Coordinate> { startCoordinate, endCoordinate })
        {
        }

        /// <summary>
        /// Gets a value indicating whether the current geometry is simple.
        /// </summary>
        /// <value><c>true</c>, as a line is always considered to be simple.</value>
        public override Boolean IsSimple { get { return true; } }

        /// <summary>
        /// Gets the length of the line.
        /// </summary>
        /// <value>The length of the line.</value>
        public override Double Length { get { return Coordinate.Distance(this.StartCoordinate, this.EndCoordinate); } }

        /// <summary>
        /// Adds a coordinate to the end of the line.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.NotSupportedException">Extension of lines is not supported.</exception>
        public override void Add(Coordinate coordinate)
        {
            throw new NotSupportedException(CoreMessages.LineExtensionNotSupported);
        }

        /// <summary>
        /// Inserts a coordinate into the line at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.NotSupportedException">Extension of lines is not supported.</exception>
        public override void Insert(Int32 index, Coordinate coordinate)
        {
            throw new NotSupportedException(CoreMessages.LineExtensionNotSupported);
        }

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the line.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">Reduction of lines is not supported.</exception>
        public override Boolean Remove(Coordinate coordinate)
        {
            throw new NotSupportedException(CoreMessages.LineReductionNotSupported);
        }

        /// <summary>
        /// Removes the coordinate at the specified index from the line.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to remove.</param>
        /// <exception cref="System.NotSupportedException">Reduction of lines is not supported.</exception>
        public override void RemoveAt(Int32 index)
        {
            throw new NotSupportedException(CoreMessages.LineReductionNotSupported);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, LineName);
        }
    }
}
