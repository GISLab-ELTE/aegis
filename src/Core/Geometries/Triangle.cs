// <copyright file="Triangle.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using ELTE.AEGIS.Algorithms;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a triangle geometry in Cartesian coordinate space.
    /// </summary>
    /// <remarks>
    /// A Triangle is a polygon with 3 distinct, non-collinear vertexes and no interior boundary.
    /// </remarks>
    public class Triangle : Polygon, ITriangle
    {
        #region Private fields

        /// <summary>
        /// The name of the triangle. This field is constant.
        /// </summary>
        private const String TiangleName = "TRIANGLE";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        public Triangle(PrecisionModel precisionModel, IReferenceSystem referenceSystem, Coordinate first, Coordinate second, Coordinate third)
            : base(precisionModel, referenceSystem, new Coordinate[] { first, second, third }, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle" /> class.
        /// </summary>
        /// <param name="factory">The factory of the triangle.</param>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public Triangle(IGeometryFactory factory, Coordinate first, Coordinate second, Coordinate third)
            : base(factory, new Coordinate[] { first, second, third }, null)
        {
        }

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets a value indicating whether the triangle is valid.
        /// </summary>
        /// <value><c>true</c> if the coordinates form a legitimate triangle; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get
            {
                Double zValue = this.Shell.StartCoordinate.Z;
                if (this.Shell.Any(coordinate => coordinate.Z != zValue))
                    return false;

                if (Coordinate.Distance(this.Shell[0], this.Shell[1]) >= Coordinate.Distance(this.Shell[0], this.Shell[2]) + Coordinate.Distance(this.Shell[1], this.Shell[2]) &&
                    Coordinate.Distance(this.Shell[0], this.Shell[2]) < Coordinate.Distance(this.Shell[0], this.Shell[1]) + Coordinate.Distance(this.Shell[1], this.Shell[2]) &&
                    Coordinate.Distance(this.Shell[1], this.Shell[2]) < Coordinate.Distance(this.Shell[0], this.Shell[1]) + Coordinate.Distance(this.Shell[0], this.Shell[2]))
                    return false;

                if (PolygonAlgorithms.Orientation(this.Shell, this.PrecisionModel) != Orientation.Counterclockwise)
                    return false;

                return true;
            }
        }

        #endregion

        #region ISurface properties

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

        #endregion

        #region IPolygon methods

        /// <summary>
        /// Add a hole to the triangle.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override void AddHole(ILinearRing hole)
        {
            throw new NotSupportedException(Messages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override ILinearRing GetHole(Int32 index)
        {
            throw new NotSupportedException(Messages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Removes a hole from the triangle.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the triangle contains the <paramref name="hole" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override Boolean RemoveHole(ILinearRing hole)
        {
            throw new NotSupportedException(Messages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Removes the hole at the specified index of the triangle.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to remove.</param>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override void RemoveHoleAt(Int32 index)
        {
            throw new NotSupportedException(Messages.HolesNotSupportedInTriangle);
        }

        /// <summary>
        /// Removes all holes from the triangle.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Holes are not supported in the triangle.</exception>
        public override void ClearHoles()
        {
            throw new NotSupportedException(Messages.HolesNotSupportedInTriangle);
        }

        #endregion

        #region IGeometry methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, TiangleName);
        }

        #endregion
    }
}
