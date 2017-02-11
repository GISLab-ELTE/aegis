// <copyright file="MonotoneSubdivisionAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Collections;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a type for executing the Monotone Subdivision algorithm.
    /// </summary>
    /// <remarks>
    /// Monotone subdivision is an algorithm for creating the triangulation of the polygon by partitioning to trapezoids, converting them to monotone subdivisions, and triangulating each monotone piece.
    /// </remarks>
    public class MonotoneSubdivisionAlgorithm
    {
        /// <summary>
        /// The coordinates of the resulting triangles.
        /// </summary>
        private List<Coordinate[]> result;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonotoneSubdivisionAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public MonotoneSubdivisionAlgorithm(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            this.Source = source;
            if (this.Source.First() != this.Source.Last())
                this.Source = this.Source.Append(this.Source.First());

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.hasResult = false;
        }

        /// <summary>
        /// Gets the precision model used by the algorithm.
        /// </summary>
        /// <value>The precision model used by the algorithm.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the source coordinates.
        /// </summary>
        /// <value>The read-only list of coordinates representing the polygon shell.</value>
        public IEnumerable<Coordinate> Source { get; private set; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The list of coordinates of the triangulation.</value>
        public IReadOnlyList<Coordinate[]> Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();

                return this.result;
            }
        }

        /// <summary>
        /// Partitions a polygon by triangles.
        /// </summary>
        public void Compute()
        {
            this.result = new List<Coordinate[]>();

            List<Coordinate> shell = new List<Coordinate>(this.Source);
            List<Coordinate> nextShell = new List<Coordinate>(this.Source);
            Coordinate[] triangle;

            Int32 coordinateCount = shell.Count - 1;
            Int32 coordinateIndex = 1;
            while (shell.Count != 4)
            {
                Coordinate centroid = LineAlgorithms.Centroid(shell[(shell.Count + coordinateIndex - 1) % coordinateCount], shell[(shell.Count + coordinateIndex + 1) % coordinateCount], this.PrecisionModel);
                nextShell.Remove(nextShell[coordinateIndex]);
                if (!WindingNumberAlgorithm.InExterior(shell, centroid, this.PrecisionModel) && !ShamosHoeyAlgorithm.Intersects(nextShell, this.PrecisionModel))
                {
                    triangle = new Coordinate[3]
                    {
                        shell[(coordinateCount + coordinateIndex - 1) % coordinateCount],
                        shell[coordinateIndex],
                        shell[(coordinateCount + coordinateIndex + 1) % coordinateCount]
                    };
                    this.result.Add(triangle);
                    shell.Remove(shell[coordinateIndex]);
                    coordinateIndex = 1;
                    coordinateCount--;
                }
                else
                {
                    coordinateIndex = (coordinateIndex + 1) % (shell.Count - 1);
                    nextShell = new List<Coordinate>(shell);
                }
            }

            triangle = new Coordinate[3] { shell[0], shell[1], shell[2] };
            this.result.Add(triangle);
            this.hasResult = true;
        }

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The coordinates of the simplified polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IReadOnlyList<Coordinate[]> Triangulate(IEnumerable<Coordinate> shell)
        {
            return new MonotoneSubdivisionAlgorithm(shell, null).Result;
        }

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The coordinates of the simplified polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IReadOnlyList<Coordinate[]> Triangulate(IEnumerable<Coordinate> shell, PrecisionModel precisionModel)
        {
            return new MonotoneSubdivisionAlgorithm(shell, precisionModel).Result;
        }

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The coordinates of the simplified polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IReadOnlyList<Coordinate[]> Triangulate(IReadOnlyList<Coordinate> shell)
        {
            return new MonotoneSubdivisionAlgorithm(shell, null).Result;
        }

        /// <summary>
        /// Determines the triangles of the polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The coordinates of the simplified polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IReadOnlyList<Coordinate[]> Triangulate(IReadOnlyList<Coordinate> shell, PrecisionModel precisionModel)
        {
            return new MonotoneSubdivisionAlgorithm(shell, precisionModel).Result;
        }
    }
}
