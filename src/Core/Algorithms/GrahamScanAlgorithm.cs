// <copyright file="GrahamScanAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a type for running the Graham scan algorithm.
    /// </summary>
    /// <remarks>
    /// The Graham scan algorithm is used to compute the convex hull of a planar polygon in O(n log n) runtime.
    /// The algorithm assumes that the specified polygon is valid.
    /// </remarks>
    public class GrahamScanAlgorithm
    {
        /// <summary>
        /// Represents a comparer used by the Graham scan algorithm.
        /// </summary>
        private class GrahamComparer : IComparer<Coordinate>
        {
            /// <summary>
            /// The origin coordinate.
            /// </summary>
            private Coordinate origin;

            /// <summary>
            /// The precision model.
            /// </summary>
            private PrecisionModel precisionModel;

            /// <summary>
            /// Initializes a new instance of the <see cref="GrahamComparer" /> class.
            /// </summary>
            /// <param name="origin">The origin.</param>
            /// <param name="precisionModel">The precision model.</param>
            public GrahamComparer(Coordinate origin, PrecisionModel precisionModel)
            {
                this.origin = origin;
                this.precisionModel = precisionModel;
            }

            /// <summary>
            /// Compares the specified coordinates.
            /// </summary>
            /// <param name="x">The first coordinate to compare.</param>
            /// <param name="y">The second coordinate to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
            public Int32 Compare(Coordinate x, Coordinate y)
            {
                if (x.Equals(y))
                    return 0;

                Orientation orientation = Coordinate.Orientation(this.origin, x, y, this.precisionModel);

                switch (orientation)
                {
                    case Orientation.Counterclockwise:
                        return -1;
                    case Orientation.Clockwise:
                        return 1;
                    default:
                        return Coordinate.Distance(this.origin, x) < Coordinate.Distance(this.origin, y) ? -1 : 1;
                }
            }
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
        /// Initializes a new instance of the <see cref="GrahamScanAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        protected GrahamScanAlgorithm(IEnumerable<Coordinate> source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            this.Source = source;
            this.hasResult = false;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
        }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the collection of source coordinates.
        /// </summary>
        /// <value>The collection of source coordinates.</value>
        public IEnumerable<Coordinate> Source { get; private set; }

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
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <returns>The convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ComputeConvexHull(IBasicPolygon source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            return new GrahamScanAlgorithm(source.Shell, PrecisionModel.Default).Result;
        }

        /// <summary>
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The source polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ComputeConvexHull(IBasicPolygon source, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            return new GrahamScanAlgorithm(source.Shell, precisionModel).Result;
        }

        /// <summary>
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <returns>The convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ComputeConvexHull(IReadOnlyList<Coordinate> source)
        {
            return new GrahamScanAlgorithm(source, PrecisionModel.Default).Result;
        }

        /// <summary>
        /// Computes the convex hull of the specified polygon.
        /// </summary>
        /// <param name="source">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The convex hull of <paramref name="source" />.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IReadOnlyList<Coordinate> ComputeConvexHull(IReadOnlyList<Coordinate> source, PrecisionModel precisionModel)
        {
            return new GrahamScanAlgorithm(source, precisionModel).Result;
        }

        /// <summary>
        /// Computes the convex hull.
        /// </summary>
        public void Compute()
        {
            Coordinate[] coordinates = new Coordinate[this.Source.ElementCount()];

            Int32 coordinateIndex = 0;
            foreach (Coordinate coordinate in this.Source.Elements())
            {
                coordinates[coordinateIndex] = coordinate;
                coordinateIndex++;
            }

            // search for the minimal coordinate

            Int32 min = 0;
            for (coordinateIndex = 1; coordinateIndex < coordinates.Length; coordinateIndex++)
            {
                if ((coordinates[coordinateIndex].Y < coordinates[min].Y) || ((coordinates[coordinateIndex].Y == coordinates[min].Y) && (coordinates[coordinateIndex].X < coordinates[min].X)))
                {
                    min = coordinateIndex;
                }
            }

            Coordinate temp = coordinates[min];
            coordinates[min] = coordinates[0];
            coordinates[0] = temp;

            // sort coordinates
            Array.Sort(coordinates, 1, coordinates.Length - 2, new GrahamComparer(coordinates[0], this.PrecisionModel));

            // select convex hull candidate coordinates
            Coordinate[] convexHullCoordinates = new Coordinate[coordinates.Length];

            // first 3 elements are definitely candidates
            Int32 convexHullCount = 3;
            Array.Copy(coordinates, convexHullCoordinates, 3);

            for (coordinateIndex = 3; coordinateIndex < coordinates.Length; coordinateIndex++)
            {
                // further coordinates should be checked and removed if not counter clockwise
                while (convexHullCount > 2 && Coordinate.Orientation(convexHullCoordinates[convexHullCount - 2], convexHullCoordinates[convexHullCount - 1], coordinates[coordinateIndex], this.PrecisionModel) != Orientation.Counterclockwise)
                {
                    convexHullCount--;
                }

                convexHullCoordinates[convexHullCount] = coordinates[coordinateIndex];
                convexHullCount++;
            }

            // copy to result array
            this.result = convexHullCoordinates.GetRange(convexHullCount);
            this.hasResult = true;
        }
    }
}
