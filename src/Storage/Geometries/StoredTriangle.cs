// <copyright file="StoredTriangle.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Algorithms;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a triangle located in a store.
    /// </summary>
    public class StoredTriangle : StoredPolygon, ITriangle
    {
        /// <summary>
        /// The name of the triangle. This field is constant.
        /// </summary>
        private const String TiangleName = "TRIANGLE";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredTriangle" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredTriangle(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, referenceSystem, driver, identifier, indexes)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the triangle is valid.
        /// </summary>
        /// <value><c>true</c> if the coordinates form a legitimate triangle; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get
            {
                IReadOnlyList<Coordinate> coordinates = this.Shell;

                if (coordinates.Count != 3)
                    return false;

                Double zValue = coordinates[0].Z;
                if (coordinates.Any(coordinate => coordinate.Z != zValue))
                    return false;

                if (Coordinate.Distance(coordinates[0], coordinates[1]) >= Coordinate.Distance(coordinates[0], coordinates[2]) + Coordinate.Distance(coordinates[1], coordinates[2]) &&
                    Coordinate.Distance(coordinates[0], coordinates[2]) < Coordinate.Distance(coordinates[0], coordinates[1]) + Coordinate.Distance(coordinates[1], coordinates[2]) &&
                    Coordinate.Distance(coordinates[1], coordinates[2]) < Coordinate.Distance(coordinates[0], coordinates[1]) + Coordinate.Distance(coordinates[0], coordinates[2]))
                    return false;

                if (PolygonAlgorithms.Orientation(coordinates) != Orientation.Counterclockwise)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the triangle is convex.
        /// </summary>
        /// <value><c>true</c>, as a triangle is always convex.</value>
        public override sealed Boolean IsConvex { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the triangle is whole.
        /// </summary>
        /// <value><c>true</c>, as a triangle is always whole.</value>
        public override sealed Boolean IsWhole { get { return true; } }

        /// <summary>
        /// Add a hole to the triangle.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override void AddHole(ILinearRing hole)
        {
            throw new NotSupportedException(CoreMessages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override ILinearRing GetHole(Int32 index)
        {
            throw new NotSupportedException(CoreMessages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Removes a hole from the triangle.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the triangle contains the <paramref name="hole" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override Boolean RemoveHole(ILinearRing hole)
        {
            throw new NotSupportedException(CoreMessages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Removes the hole at the specified index of the triangle.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to remove.</param>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override void RemoveHoleAt(Int32 index)
        {
            throw new NotSupportedException(CoreMessages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Removes all holes from the triangle.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override void ClearHoles()
        {
            throw new NotSupportedException(CoreMessages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, TiangleName);
        }
    }
}
