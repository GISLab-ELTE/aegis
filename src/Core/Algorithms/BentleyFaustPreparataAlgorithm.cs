// <copyright file="BentleyFaustPreparataAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Collections;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a type for running the Bentley-Faust-Preparata algorithm.
    /// </summary>
    /// <remarks>
    /// The Bentley-Faust-Preparata algorithm is used for computing the approximate convex hull of a planar coordinate
    /// set in O(n) runtime. In many applications, an approximate hull suffices, and the gain in speed can be
    /// significant for very large coordinate sets.
    /// The algorithm assumes that the specified coordinates are valid, distinct and in the same plane.
    /// </remarks>
    public class BentleyFaustPreparataAlgorithm
    {
        /// <summary>
        /// Represents a range bin.
        /// </summary>
        private struct RangeBin
        {
            /// <summary>
            /// Gets or sets the minimum of the bin.
            /// </summary>
            public Int32? Min { get; set; }

            /// <summary>
            /// Gets or sets the maximum of the bin.
            /// </summary>
            public Int32? Max { get; set; }
        }

        /// <summary>
        /// The approximate convex hull of the source.
        /// </summary>
        private IReadOnlyList<Coordinate> result;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="BentleyFaustPreparataAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BentleyFaustPreparataAlgorithm(IReadOnlyList<Coordinate> source, PrecisionModel precisionModel)
        {
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
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
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The list of approximate convex hull coordinates.</value>
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
        /// Computes the approximate convex hull.
        /// </summary>
        public void Compute()
        {
            if (!this.Source.AnyElement())
            {
                this.result = new Coordinate[0];
                this.hasResult = true;
                return;
            }

            if (this.Source.Count < 4)
            {
                this.result = this.Source.Distinct().ToArray();
                this.hasResult = true;
                return;
            }

            Int32 minMin = 0, minMax = 0;
            Int32 maxMin = 0, maxMax = 0;
            Coordinate first = this.Source.FirstOrDefaultElement();

            Double minX = first.X, maxX = first.X;
            Int32 numberOfBins = this.Source.Count / 2;
            Int32 bottomOfStack = 0, topOfStack = -1; // indexes for bottom and top of the stack
            Coordinate currentCoordinate; // the current coordinate being considered
            Coordinate[] hullStack = new Coordinate[this.Source.Count];

            // get the coordinates with min-max X, and min-max Y
            for (Int32 sourceIndex = 1; sourceIndex < this.Source.Count; sourceIndex++)
            {
                if (this.Source[sourceIndex] == null)
                    continue;

                if (this.Source[sourceIndex].X <= minX)
                {
                    if (this.Source[sourceIndex].X < minX)
                    {
                        minX = this.Source[sourceIndex].X;
                        minMin = minMax = sourceIndex;
                    }
                    else
                    {
                        if (this.Source[sourceIndex].Y < this.Source[minMin].Y)
                            minMin = sourceIndex;
                        else if (this.Source[sourceIndex].Y > this.Source[minMax].Y)
                            minMax = sourceIndex;
                    }
                }

                if (this.Source[sourceIndex].X >= maxX)
                {
                    if (this.Source[sourceIndex].X > maxX)
                    {
                        maxX = this.Source[sourceIndex].X;
                        maxMin = maxMax = sourceIndex;
                    }
                    else
                    {
                        if (this.Source[sourceIndex].Y < this.Source[maxMin].Y)
                            maxMin = sourceIndex;
                        else if (this.Source[sourceIndex].Y > this.Source[maxMax].Y)
                            maxMax = sourceIndex;
                    }
                }
            }

            // degenerate case: all x coordinates are equal to minX
            if (minX == maxX)
            {
                if (minMax != minMin)
                    this.result = new Coordinate[] { this.Source[minMin], this.Source[minMax] };
                else
                    this.result = new Coordinate[] { this.Source[minMin] };
            }

            // get the max and min coordinates in the range bins
            RangeBin[] binArray = new RangeBin[numberOfBins + 2];

            binArray[0].Min = minMin;
            binArray[0].Max = minMax;
            binArray[numberOfBins + 1].Min = maxMin;
            binArray[numberOfBins + 1].Max = maxMax;

            for (Int32 binIndex = 1; binIndex <= numberOfBins; binIndex++)
            {
                binArray[binIndex].Min = binArray[binIndex].Max = null;
            }

            for (Int32 sourceIndex = 0; sourceIndex < this.Source.Count; sourceIndex++)
            {
                if (this.Source[sourceIndex] == null)
                    continue;

                if (this.Source[sourceIndex].X == minX || this.Source[sourceIndex].X == maxX)
                    continue;

                Int32 binIndex;
                if (Coordinate.Orientation(this.Source[minMin], this.Source[maxMin], this.Source[sourceIndex], this.PrecisionModel) == Orientation.Clockwise)
                {
                    // below lower line
                    binIndex = Convert.ToInt32(numberOfBins * (this.Source[sourceIndex].X - minX) / (maxX - minX)) + 1;
                    if (binArray[binIndex].Min == null || this.Source[sourceIndex].Y < this.Source[binArray[binIndex].Min.Value].Y)
                        binArray[binIndex].Min = sourceIndex;
                }
                else if (Coordinate.Orientation(this.Source[minMin], this.Source[maxMin], this.Source[sourceIndex], this.PrecisionModel) == Orientation.Counterclockwise)
                {
                    // above upper line
                    binIndex = Convert.ToInt32(numberOfBins * (this.Source[sourceIndex].X - minX) / (maxX - minX)) + 1;
                    if (binArray[binIndex].Max == null || this.Source[sourceIndex].Y > this.Source[binArray[binIndex].Max.Value].Y)
                        binArray[binIndex].Max = sourceIndex;
                }
            }

            // use the chain algorithm to get the lower and upper hulls

            // compute the lower hull on the stack
            for (Int32 binIndex = 0; binIndex <= numberOfBins + 1; ++binIndex)
            {
                if (binArray[binIndex].Min == null)
                    continue;

                currentCoordinate = this.Source[binArray[binIndex].Min.Value];

                while (topOfStack > 0)
                {
                    // there are at least 2 points on the stack
                    if (Coordinate.Orientation(hullStack[topOfStack - 1], hullStack[topOfStack], currentCoordinate, this.PrecisionModel) == Orientation.Counterclockwise)
                        break;
                    else
                        --topOfStack;
                }

                topOfStack++;
                hullStack[topOfStack] = currentCoordinate;
            }

            // compute the upper hull on the stack above the bottom hull
            if (maxMax != maxMin)
            {
                topOfStack++;
                hullStack[topOfStack] = this.Source[maxMax];
            }

            bottomOfStack = topOfStack;

            for (Int32 binIndex = numberOfBins; binIndex >= 0; --binIndex)
            {
                if (binArray[binIndex].Max == null)
                    continue;

                currentCoordinate = this.Source[binArray[binIndex].Max.Value];

                while (topOfStack > bottomOfStack)
                {
                    // there are at least 2 points on the upper stack
                    if (Coordinate.Orientation(hullStack[topOfStack - 1], hullStack[topOfStack], currentCoordinate, this.PrecisionModel) == Orientation.Counterclockwise)
                        break;
                    else
                        topOfStack--;
                }

                topOfStack++;
                hullStack[topOfStack] = currentCoordinate;
            }

            // push joining endpoint onto stack
            if (minMax != minMin)
            {
                topOfStack++;
                hullStack[topOfStack] = this.Source[minMin];
            }

            // generate result from stack
            hullStack[topOfStack] = hullStack[0];

            this.result = hullStack.GetRange(topOfStack + 1);
            this.hasResult = true;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <returns>The approximate convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ApproximateConvexHull(IBasicPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new BentleyFaustPreparataAlgorithm(source.Shell, PrecisionModel.Default).Result;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The approximate convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ApproximateConvexHull(IBasicPolygon source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new BentleyFaustPreparataAlgorithm(source.Shell, precisionModel).Result;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <returns>The approximate convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ApproximateConvexHull(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new BentleyFaustPreparataAlgorithm(new List<Coordinate>(source), PrecisionModel.Default).Result;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The approximate convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ApproximateConvexHull(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new BentleyFaustPreparataAlgorithm(new List<Coordinate>(source), precisionModel).Result;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <returns>The approximate convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ApproximateConvexHull(IReadOnlyList<Coordinate> source)
        {
            return new BentleyFaustPreparataAlgorithm(source, PrecisionModel.Default).Result;
        }

        /// <summary>
        /// Computes the approximate convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The approximate convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ApproximateConvexHull(IReadOnlyList<Coordinate> source, PrecisionModel precisionModel)
        {
            return new BentleyFaustPreparataAlgorithm(source, precisionModel).Result;
        }
    }
}
