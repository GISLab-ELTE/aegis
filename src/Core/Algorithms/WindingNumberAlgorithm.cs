// <copyright file="WindingNumberAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a type for performing the Winding Number algorithm.
    /// </summary>
    /// <remarks>
    /// The Winding Number algorithm counts the number of times the polygon winds around a coordinate.
    /// When the coordinate is outside, the value of the winding number is zero; otherwise, the coordinate is inside.
    /// However, the winding number is not defined for coordinates on the boundary of the polygon, it might be both a non-zero or a zero value.
    /// For an input consisting of n line segments, the Winding Number algorithm has a linear complexity of O(2n).
    /// The algorithm assumes that the specified coordinates are valid, ordered, distinct and in the same plane.
    /// </remarks>
    public class WindingNumberAlgorithm
    {
        /// <summary>
        /// The coordinate.
        /// </summary>
        private Coordinate coordinate;

        /// <summary>
        /// A value indicating whether the result is computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// The result of the algorithm.
        /// </summary>
        private Int32 result;

        /// <summary>
        /// A value indicating whether the coordinate is on an edge.
        /// </summary>
        private Boolean isCoordinateOnEdge;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindingNumberAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate for which the Winding Number is calculated.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public WindingNumberAlgorithm(IEnumerable<Coordinate> shell, Coordinate coordinate, PrecisionModel precisionModel)
        {
            this.Shell = shell ?? throw new ArgumentNullException(nameof(shell));
            this.PrecisionModel = precisionModel;
            this.coordinate = coordinate;
            this.hasResult = false;
            this.isCoordinateOnEdge = false;
        }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the coordinates of the polygon shell.
        /// </summary>
        /// <value>The coordinates of the polygon shell.</value>
        public IEnumerable<Coordinate> Shell { get; private set; }

        /// <summary>
        /// Gets or sets the coordinate for which the Winding Number is computed.
        /// </summary>
        /// <value>The coordinate.</value>
        public Coordinate Coordinate
        {
            get
            {
                return this.coordinate;
            }

            set
            {
                if (this.coordinate != value)
                {
                    this.coordinate = value;
                    this.hasResult = false;
                }
            }
        }

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The Winding Number of the specified coordinate.</value>
        public Int32 Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();
                return this.result;
            }
        }

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IReadOnlyList<Coordinate> shell, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            return ComputeLocation(shell, null, coordinate, null);
        }

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IReadOnlyList<Coordinate> shell, Coordinate coordinate, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            return ComputeLocation(shell, null, coordinate, precision);
        }

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IBasicPolygon polygon, Coordinate coordinate)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon));

            return ComputeLocation(polygon.Shell, polygon.Holes, coordinate, null);
        }

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IBasicPolygon polygon, Coordinate coordinate, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon));

            return ComputeLocation(polygon.Shell, polygon.Holes, coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean OnBoundary(IReadOnlyList<Coordinate> shell, Coordinate coordinate)
        {
            return ComputeOnBoundary(shell, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean OnBoundary(IReadOnlyList<Coordinate> shell, Coordinate coordinate, PrecisionModel precisionModel)
        {
            return ComputeOnBoundary(shell, coordinate, precisionModel);
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        public static Boolean OnBoundary(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            if (ComputeOnBoundary(shell, coordinate, PrecisionModel.Default))
                return true;

            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in holes)
                {
                    if (ComputeOnBoundary(hole, coordinate, PrecisionModel.Default))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        public static Boolean OnBoundary(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            if (ComputeOnBoundary(shell, coordinate, precisionModel))
                return true;

            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in holes)
                {
                    if (ComputeOnBoundary(hole, coordinate, precisionModel))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Computes whether the given coordinate is in the interior of the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is in the interior of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IBasicPolygon polygon, Coordinate coordinate)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon));

            return InInterior(polygon.Shell, polygon.Holes, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is in the interior of the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is in the interior of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IBasicPolygon polygon, Coordinate coordinate, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon));

            return InInterior(polygon.Shell, polygon.Holes, coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IReadOnlyList<Coordinate> shell, Coordinate coordinate)
        {
            return ComputeLocation(shell, null, coordinate, PrecisionModel.Default) == RelativeLocation.Interior;
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IReadOnlyList<Coordinate> shell, Coordinate coordinate, PrecisionModel precision)
        {
            return ComputeLocation(shell, null, coordinate, PrecisionModel.Default) == RelativeLocation.Interior;
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            return ComputeLocation(shell, holes, coordinate, PrecisionModel.Default) == RelativeLocation.Interior;
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            return ComputeLocation(shell, holes, coordinate, precisionModel) == RelativeLocation.Interior;
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IBasicPolygon polygon, Coordinate coordinate)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon));

            return InExterior(polygon.Shell, polygon.Holes, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IBasicPolygon polygon, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon));

            return InExterior(polygon.Shell, polygon.Holes, coordinate, precisionModel);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IReadOnlyList<Coordinate> shell, Coordinate coordinate)
        {
            return InExterior(shell, null, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IReadOnlyList<Coordinate> shell, Coordinate coordinate, PrecisionModel precision)
        {
            return InExterior(shell, null, coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            return ComputeLocation(shell, holes, coordinate, PrecisionModel.Default) == RelativeLocation.Exterior;
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            return ComputeLocation(shell, holes, coordinate, precision) == RelativeLocation.Exterior;
        }

        /// <summary>
        /// Computes the Winding Number.
        /// </summary>
        public void Compute()
        {
            this.result = 0;

            // loop through all edges of the polygon
            IEnumerator<Coordinate> enumerator = this.Shell.GetEnumerator();
            if (!enumerator.MoveNext())
                return;

            Coordinate current = enumerator.Current, next = null;

            while (enumerator.MoveNext())
            {
                next = enumerator.Current;
                if (next == null)
                    continue;

                Orientation orientation;

                if (current.Y <= this.coordinate.Y && next.Y > this.coordinate.Y)
                {
                    // an upward crossing
                    orientation = Coordinate.Orientation(this.coordinate, current, next, this.PrecisionModel);
                    switch (orientation)
                    {
                        case Orientation.Counterclockwise:
                            // has a valid up intersect
                            ++this.result;
                            break;
                        case Orientation.Collinear:
                            // the winding number is not defined for coordinates on the boundary
                            this.isCoordinateOnEdge = true;
                            break;
                    }
                }
                else if (current.Y > this.coordinate.Y && next.Y <= this.coordinate.Y)
                {
                    // a downward crossing
                    orientation = Coordinate.Orientation(this.coordinate, current, next, this.PrecisionModel);
                    switch (orientation)
                    {
                        case Orientation.Clockwise:
                            // has a valid down intersect
                            --this.result;
                            break;
                        case Orientation.Collinear:
                            // the winding number is not defined for coordinates on the boundary
                            this.isCoordinateOnEdge = true;
                            break;
                    }
                }

                current = next;
            }

            this.hasResult = true;
        }

        /// <summary>
        /// Computes the location of the specified coordinate within a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The examined coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The location of the coordinate.</returns>
        private static RelativeLocation ComputeLocation(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (!coordinate.IsValid)
                return RelativeLocation.Undefined;

            if (shell.Any(coord => coord == null || !coord.IsValid))
                return RelativeLocation.Undefined;

            if (!Envelope.Contains(shell, coordinate))
                return RelativeLocation.Exterior;

            WindingNumberAlgorithm algorithm = new WindingNumberAlgorithm(shell, coordinate, precisionModel);
            algorithm.Compute();

            // probably the Winding Number algorithm already detected that the coordinate is on the boundary of the polygon shell
            if (algorithm.isCoordinateOnEdge)
                return RelativeLocation.Boundary;

            // additionally check the boundary
            if (ComputeOnBoundary(shell, coordinate, precisionModel))
                return RelativeLocation.Boundary;

            if (algorithm.Result == 0)
                return RelativeLocation.Exterior;

            // if the coordinate is not outside, the holes are also required to be checked
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in holes)
                {
                    if (hole == null)
                        continue;

                    if (hole.Any(coord => coord == null || !coord.IsValid))
                        return RelativeLocation.Undefined;

                    algorithm = new WindingNumberAlgorithm(hole, coordinate, precisionModel);
                    algorithm.Compute();

                    if (algorithm.isCoordinateOnEdge)
                        return RelativeLocation.Boundary;

                    // additionally check the boundary
                    if (ComputeOnBoundary(hole, coordinate, precisionModel))
                        return RelativeLocation.Boundary;

                    if (algorithm.Result != 0)
                        return RelativeLocation.Exterior;
                }
            }

            return RelativeLocation.Interior;
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        private static Boolean ComputeOnBoundary(IEnumerable<Coordinate> shell, Coordinate coordinate, PrecisionModel precisionModel)
        {
            IEnumerator<Coordinate> enumerator = shell.GetEnumerator();
            if (!enumerator.MoveNext())
                return false;

            Coordinate current = enumerator.Current, next;

            while (enumerator.MoveNext())
            {
                next = enumerator.Current;

                if (LineAlgorithms.Contains(current, next, coordinate, precisionModel))
                    return true;

                current = next;
            }

            return false;
        }
    }
}
