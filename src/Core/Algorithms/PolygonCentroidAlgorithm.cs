// <copyright file="PolygonCentroidAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a type for computing polygon centroid.
    /// </summary>
    /// <remarks>
    /// The algorithm assumes that the polygon is valid.
    /// </remarks>
    public class PolygonCentroidAlgorithm
    {
        /// <summary>
        /// The centroid of the polygon.
        /// </summary>
        private Coordinate result;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// The area of the polygon.
        /// </summary>
        private Double area;

        /// <summary>
        /// The base coordinate.
        /// </summary>
        private Coordinate baseCoordinate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCentroidAlgorithm" /> class.
        /// </summary>
        /// <param name="polygon">The source polygon.</param>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public PolygonCentroidAlgorithm(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon));

            this.Shell = polygon.Shell;
            this.Holes = polygon.Holes;
            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCentroidAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public PolygonCentroidAlgorithm(IEnumerable<Coordinate> shell)
        {
            this.Shell = shell ?? throw new ArgumentNullException(nameof(shell));
            this.Holes = null;
            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCentroidAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public PolygonCentroidAlgorithm(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            this.Shell = shell ?? throw new ArgumentNullException(nameof(shell));
            this.Holes = holes;
            this.hasResult = false;
        }

        /// <summary>
        /// Gets the coordinates of the polygon shell.
        /// </summary>
        /// <value>The read-only list of coordinates representing the polygon shell.</value>
        public IEnumerable<Coordinate> Shell { get; private set; }

        /// <summary>
        /// Gets collection of coordinates representing the polygon holes.
        /// </summary>
        /// <value>The collection of coordinates representing the polygon holes.</value>
        public IEnumerable<IEnumerable<Coordinate>> Holes { get; private set; }

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The centroid of the polygon.</value>
        public Coordinate Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();
                return this.result;
            }
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IBasicPolygon source)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(source);
            algorithm.Compute();

            return algorithm.Result;
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IReadOnlyList<Coordinate> shell)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(shell);
            algorithm.Compute();

            return algorithm.Result;
        }

        /// <summary>
        /// Compute the centroid of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        /// <returns>The centroid of the polygon.</returns>
        public static Coordinate ComputeCentroid(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes)
        {
            PolygonCentroidAlgorithm algorithm = new PolygonCentroidAlgorithm(shell, holes);
            algorithm.Compute();

            return algorithm.Result;
        }

        /// <summary>
        /// Computes the centroid of the polygon.
        /// </summary>
        public void Compute()
        {
            if (this.Shell == null || !this.Shell.Any())
                return;

            Double resultX = 0, resultY = 0;
            this.area = 0;

            this.baseCoordinate = this.Shell.FirstOrDefault();
            this.AddCoordinates(this.Shell, 1, ref resultX, ref resultY);

            if (this.Holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in this.Holes)
                {
                    this.AddCoordinates(hole, -1, ref resultX, ref resultY);
                }
            }

            resultX = resultX / (3 * this.area);
            resultY = resultY / (3 * this.area);

            this.result = new Coordinate(resultX, resultY, this.baseCoordinate.Z);
            this.hasResult = true;
        }

        /// <summary>
        /// Add a list of coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="sign">The sign of the coordinates (positive for shell, negative for hole).</param>
        /// <param name="resultX">The X coordinate of the result.</param>
        /// <param name="resultY">The Y coordinate of the result.</param>
        /// <exception cref="System.InvalidOperationException">
        /// The number of coordinates in less than 3.
        /// or
        /// The coordinates do not represent a surface.
        /// </exception>
        private void AddCoordinates(IEnumerable<Coordinate> coordinates, Int32 sign, ref Double resultX, ref Double resultY)
        {
            IEnumerator<Coordinate> enumerator = coordinates.GetEnumerator();

            if (!enumerator.MoveNext())
                return;

            Coordinate current = enumerator.Current, next;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == null)
                    continue;

                next = enumerator.Current;
                this.AddTriangle(this.baseCoordinate, current, next, sign, ref resultX, ref resultY);
                current = next;
            }
        }

        /// <summary>
        /// Adds a triangle.
        /// </summary>
        /// <param name="first">The first coordinate of the triangle.</param>
        /// <param name="second">The second coordinate of the triangle.</param>
        /// <param name="third">The third coordinate of the triangle.</param>
        /// <param name="sign">The sign of the triangle (positive for shell, negative for hole).</param>
        /// <param name="resultX">The X coordinate of the result.</param>
        /// <param name="resultY">The Y coordinate of the result.</param>
        private void AddTriangle(Coordinate first, Coordinate second, Coordinate third, Int32 sign, ref Double resultX, ref Double resultY)
        {
            Double triArea = (second.X - first.X) * (third.Y - first.Y) - (third.X - first.X) * (second.Y - first.Y);
            this.area += sign * triArea;

            resultX += sign * triArea * (first.X + second.X + third.X);
            resultY += sign * triArea * (first.Y + second.Y + third.Y);
        }
    }
}
