// <copyright file="DouglasPeuckerAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a type for executing the Douglas-Peucker algorithm.
    /// </summary>
    /// <remarks>
    /// Douglas-Peucker (or Ramer-Douglas-Peucker) algorithm is used to reduce vertexes in a line string, resulting in a similar line string in O(n^2) runtime.
    /// The algorithm assumes that the source is a simple line string without circle.
    /// </remarks>
    /// <author>Bence Molnár, Roberto Giachetta</author>
    public class DouglasPeuckerAlgorithm
    {
        /// <summary>
        /// The tolerance.
        /// </summary>
        private Double delta;

        /// <summary>
        /// The simplified line string.
        /// </summary>
        private Coordinate[] result;

        /// <summary>
        /// The marks on the coordinates.
        /// </summary>
        private Boolean[] marks;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="DouglasPeuckerAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="delta">The tolerance.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public DouglasPeuckerAlgorithm(IReadOnlyList<Coordinate> source, Double delta, PrecisionModel precisionModel)
        {
            if (delta <= 0)
                throw new ArgumentOutOfRangeException(nameof(delta), CoreMessages.DeltaIsEqualToOrLessThan0);

            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.delta = delta;
            this.hasResult = false;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
        }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the source coordinates.
        /// </summary>
        /// <value>The read-only list of source coordinates.</value>
        public IReadOnlyList<Coordinate> Source { get; private set; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The list of coordinates of the simplified line string.</value>
        public IReadOnlyList<Coordinate> Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();

                return this.result;
            }
        }

        /// <summary>
        /// Simplifies the specified line string.
        /// </summary>
        /// <param name="source">The line string.</param>
        /// <param name="delta">The tolerance.</param>
        /// <returns>The simplified line string.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IBasicLineString Simplify(IBasicLineString source, Double delta)
        {
            return Simplify(source, delta, PrecisionModel.Default);
        }

        /// <summary>
        /// Simplifies the specified line string.
        /// </summary>
        /// <param name="source">The line string.</param>
        /// <param name="delta">The tolerance.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The simplified line string.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IBasicLineString Simplify(IBasicLineString source, Double delta, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            DouglasPeuckerAlgorithm algorithm = new DouglasPeuckerAlgorithm(source, delta, precisionModel);
            algorithm.Compute();
            return new BasicProxyLineString(algorithm.Result);
        }

        /// <summary>
        /// Simplifies the specified polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <param name="delta">The tolerance.</param>
        /// <returns>The simplified polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IBasicPolygon Simplify(IBasicPolygon source, Double delta)
        {
            return Simplify(source, delta, PrecisionModel.Default);
        }

        /// <summary>
        /// Simplifies the specified polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <param name="delta">The tolerance.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The simplified polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IBasicPolygon Simplify(IBasicPolygon source, Double delta, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            DouglasPeuckerAlgorithm algorithm = new DouglasPeuckerAlgorithm(source.Shell, delta, precisionModel);
            algorithm.Compute();
            IReadOnlyList<Coordinate> shell = algorithm.Result;
            List<IReadOnlyList<Coordinate>> holes = new List<IReadOnlyList<Coordinate>>(source.HoleCount);

            foreach (IReadOnlyList<Coordinate> hole in source.Holes)
            {
                algorithm = new DouglasPeuckerAlgorithm(hole, delta, precisionModel);
                algorithm.Compute();
                holes.Add(algorithm.Result);
            }

            return new BasicProxyPolygon(shell, holes);
        }

        /// <summary>
        /// Simplifies the specified line string.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="delta">The tolerance.</param>
        /// <returns>The list of coordinates of the simplified line string.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IReadOnlyList<Coordinate> Simplify(IReadOnlyList<Coordinate> source, Double delta)
        {
            return Simplify(source, delta, PrecisionModel.Default);
        }

        /// <summary>
        /// Simplifies the specified line string.
        /// </summary>
        /// <param name="source">The coordinates of the line string.</param>
        /// <param name="delta">The tolerance.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The list of coordinates of the simplified line string.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The delta is less than or equal to 0.</exception>
        public static IReadOnlyList<Coordinate> Simplify(IReadOnlyList<Coordinate> source, Double delta, PrecisionModel precisionModel)
        {
            DouglasPeuckerAlgorithm algorithm = new DouglasPeuckerAlgorithm(source, delta, precisionModel);
            algorithm.Compute();
            return algorithm.Result;
        }

        /// <summary>
        /// Runs the reduction algorithm.
        /// </summary>
        public void Compute()
        {
            this.marks = new Boolean[this.Source.Count];

            // mark the first and last coordinates
            this.marks[0] = this.marks[this.Source.Count - 1] = true;

            this.SimplifySegment(0, this.Source.Count - 1); // recursive simplification of the source

            // create the result based on the marked coordinates
            this.result = new Coordinate[this.marks.Count(value => value)];
            Int32 resultIndex = 0;
            for (Int32 sourceIndex = 0; sourceIndex < this.Source.Count; sourceIndex++)
            {
                if (this.marks[sourceIndex])
                {
                    this.result[resultIndex] = this.Source[sourceIndex];
                    resultIndex++;
                }
            }

            this.hasResult = true;
        }

        /// <summary>
        /// Simplifies a segment of the line string.
        /// </summary>
        /// <param name="startIndex">The starting index of the segment.</param>
        /// <param name="endIndex">The ending index of the segment.</param>
        private void SimplifySegment(Int32 startIndex, Int32 endIndex)
        {
            if (this.Source[startIndex] == null || this.Source[endIndex] == null)
                return;

            if (endIndex <= startIndex + 1) // the segment is a line
                return;

            Double maxDistance = 0;
            Int32 maxIndex = startIndex;

            // find the most distant coordinate from the line between the starting and ending coordinates
            for (Int32 coordinateIndex = startIndex + 1; coordinateIndex < endIndex; coordinateIndex++)
            {
                if (this.Source[coordinateIndex] == null)
                    continue;

                Double distance = LineAlgorithms.Distance(this.Source[startIndex], this.Source[endIndex], this.Source[coordinateIndex], this.PrecisionModel);

                if (distance > maxDistance)
                {
                    maxIndex = coordinateIndex;
                    maxDistance = distance;
                }
            }

            if (maxDistance <= this.delta)
            {
                // the distance is smaller than the delta, all coordinates should be removed
                return;
            }

            // recursively simplify both segments
            this.marks[maxIndex] = true;
            this.SimplifySegment(startIndex, maxIndex);
            this.SimplifySegment(maxIndex, startIndex);
        }
    }
}
