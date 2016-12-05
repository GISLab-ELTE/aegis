// <copyright file="GeometryDistanceAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a type for computing distances between geometries.
    /// </summary>
    /// <remarks>
    /// This implementation computes only planar distance.
    /// </remarks>
    public class GeometryDistanceAlgorithm
    {
        /// <summary>
        /// Contains the computed result (distance).
        /// </summary>
        private Double result;

        /// <summary>
        /// True if, and only if a result has been computed with this instance.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Precision Model used during computation.
        /// </summary>
        private PrecisionModel precisionModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryDistanceAlgorithm" /> class.
        /// </summary>
        /// <param name="first">The first input for the algorithm.</param>
        /// <param name="second">The second input for the algorithm.</param>
        /// <param name="precisionModel">The precision model used during computation.</param>
        /// <remarks>
        /// The collections may represent a point, a line string or polygon.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">
        /// The first collection is null.
        /// or
        /// The second collection is null.
        /// </exception>
        public GeometryDistanceAlgorithm(IEnumerable<Coordinate> first, IEnumerable<Coordinate> second, PrecisionModel precisionModel)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstCollectionIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondCollectionIsNull);

            this.hasResult = false;
            this.First = first;
            this.Second = second;
            this.precisionModel = precisionModel ?? PrecisionModel.Default;
        }

        /// <summary>
        /// Gets the result (the distance between the two 2-dimensional objects).
        /// </summary>
        public Double Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();

                return this.result;
            }
        }

        /// <summary>
        /// Gets the first collection of coordinates.
        /// </summary>
        /// <value>The first collection of coordinates.</value>
        public IEnumerable<Coordinate> First { get; private set; }

        /// <summary>
        /// Gets the second collection of coordinates.
        /// </summary>
        /// <value>The second collection of coordinates.</value>
        public IEnumerable<Coordinate> Second { get; private set; }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first point is null.
        /// or
        /// The second point is null.
        /// </exception>
        public static Double Distance(IBasicPoint first, IBasicPoint second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstPointIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondPointIsNull);

            return Coordinate.Distance(first.Coordinate, second.Coordinate);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The second point.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first point is null.
        /// or
        /// The second point is null.
        /// </exception>
        public static Double Distance(IBasicPoint first, IBasicPoint second, PrecisionModel precisionModel)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstPointIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondPointIsNull);

            if (precisionModel == null)
                return Coordinate.Distance(first.Coordinate, second.Coordinate);

            return precisionModel.MakePrecise(Coordinate.Distance(first.Coordinate, second.Coordinate));
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="lineString">The line string.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The point is null.
        /// or
        /// The line string is null.
        /// </exception>
        public static Double Distance(IBasicPoint point, IBasicLineString lineString)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point), CoreMessages.PointIsNull);
            if (lineString == null)
                throw new ArgumentNullException(nameof(lineString), CoreMessages.LineStringIsNull);

            return new GeometryDistanceAlgorithm(new[] { point.Coordinate }, lineString, PrecisionModel.Default).Result;
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="lineString">The line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The point is null.
        /// or
        /// The line string is null.
        /// </exception>
        public static Double Distance(IBasicPoint point, IBasicLineString lineString, PrecisionModel precisionModel)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point), CoreMessages.PointIsNull);
            if (lineString == null)
                throw new ArgumentNullException(nameof(lineString), CoreMessages.LineStringIsNull);

            return new GeometryDistanceAlgorithm(new[] { point.Coordinate }, lineString, precisionModel).Result;
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The point is null.
        /// or
        /// The polygon is null.
        /// </exception>
        public static Double Distance(IBasicPoint point, IBasicPolygon polygon)
        {
            return Distance(point, polygon, null);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The point is null.
        /// or
        /// The polygon is null.
        /// </exception>
        public static Double Distance(IBasicPoint point, IBasicPolygon polygon, PrecisionModel precisionModel)
        {
            if (point == null)
                throw new ArgumentNullException(nameof(point), CoreMessages.PointIsNull);
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            if (!WindingNumberAlgorithm.InExterior(polygon, point.Coordinate))
                return 0;

            if (polygon.HoleCount == 0)
            {
                return Distance(point, polygon.Shell, precisionModel);
            }

            return Math.Min(Distance(point, polygon.Shell, precisionModel), polygon.Holes.Min(hole => Distance(point, hole, precisionModel)));
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The line string is null.
        /// or
        /// The point is null.
        /// </exception>
        public static Double Distance(IBasicLineString lineString, IBasicPoint point)
        {
            return Distance(point, lineString, null);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <param name="point">The point.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The line string is null.
        /// or
        /// The point is null.
        /// </exception>
        public static Double Distance(IBasicLineString lineString, IBasicPoint point, PrecisionModel precisionModel)
        {
            return Distance(point, lineString, precisionModel);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="first">The first line string.</param>
        /// <param name="second">The second line string.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first line string is null.
        /// or
        /// The second line string is null.
        /// </exception>
        public static Double Distance(IBasicLineString first, IBasicLineString second)
        {
            return Distance(first, second, null);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="first">The first line string.</param>
        /// <param name="second">The second line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first line string is null.
        /// or
        /// The second line string is null.
        /// </exception>
        public static Double Distance(IBasicLineString first, IBasicLineString second, PrecisionModel precisionModel)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstLineStringIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondLineStringIsNull);

            return new GeometryDistanceAlgorithm(first, second, precisionModel).Result;
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The line string is null.
        /// or
        /// The polygon is null.
        /// </exception>
        public static Double Distance(IBasicLineString lineString, IBasicPolygon polygon)
        {
            return Distance(lineString, polygon, null);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The line string is null.
        /// or
        /// The polygon is null.
        /// </exception>
        public static Double Distance(IBasicLineString lineString, IBasicPolygon polygon, PrecisionModel precisionModel)
        {
            if (lineString == null)
                throw new ArgumentNullException(nameof(lineString), CoreMessages.LineStringIsNull);
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            if (WindingNumberAlgorithm.InExterior(polygon, lineString[0]))
                return 0;

            if (polygon.HoleCount == 0)
            {
                return Distance(lineString, polygon.Shell, precisionModel);
            }

            return Math.Min(Distance(lineString, polygon.Shell, precisionModel), polygon.Holes.Min(hole => Distance(lineString, hole, precisionModel)));
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="point">The point.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The polygon is null.
        /// or
        /// The point is null.
        /// </exception>
        public static Double Distance(IBasicPolygon polygon, IBasicPoint point)
        {
            return Distance(point, polygon, null);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="point">The point.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The polygon is null.
        /// or
        /// The point is null.
        /// </exception>
        public static Double Distance(IBasicPolygon polygon, IBasicPoint point, PrecisionModel precisionModel)
        {
            return Distance(point, polygon, precisionModel);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="lineString">The line string.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The polygon is null.
        /// or
        /// The line string is null.
        /// </exception>
        public static Double Distance(IBasicPolygon polygon, IBasicLineString lineString)
        {
            return Distance(lineString, polygon, null);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="lineString">The line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The polygon is null.
        /// or
        /// The line string is null.
        /// </exception>
        public static Double Distance(IBasicPolygon polygon, IBasicLineString lineString, PrecisionModel precisionModel)
        {
            return Distance(lineString, polygon, precisionModel);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="first">The line string.</param>
        /// <param name="second">The polygon.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        public static Double Distance(IBasicPolygon first, IBasicPolygon second)
        {
            return Distance(first, second, null);
        }

        /// <summary>
        /// Computes the distance between the specified geometries.
        /// </summary>
        /// <param name="first">The line string.</param>
        /// <param name="second">The polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance between the specified geometries.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        public static Double Distance(IBasicPolygon first, IBasicPolygon second, PrecisionModel precisionModel)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstPolygonIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondPolygonIsNull);

            if (WindingNumberAlgorithm.InExterior(first, second.Shell[0]))
                return 0;

            if (first.HoleCount == 0)
            {
                return Distance(first.Shell, second, precisionModel);
            }

            return Math.Min(Distance(first.Shell, second, precisionModel), first.Holes.Min(hole => Distance(hole, second, precisionModel)));
        }

        /// <summary>
        /// Computes the result of the algorithm.
        /// </summary>
        public void Compute()
        {
            Double minDistance = Double.MaxValue;
            Coordinate firstPrevious = null, secondPrevious = null;

            foreach (Coordinate first in this.First)
            {
                if (first == null)
                    continue;

                foreach (Coordinate second in this.Second)
                {
                    if (second == null)
                        continue;

                    // distance between a vertex of first, and a vertex of second
                    Double distance = Coordinate.Distance(first, second);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }

                    // distance between a vertex of first, and a segment of second
                    if (secondPrevious != null)
                    {
                        distance = DistanceFromLine(first, secondPrevious, second);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }

                    // distance between a vertex of second, and a segment of first
                    if (firstPrevious != null)
                    {
                        distance = DistanceFromLine(second, firstPrevious, first);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                        }
                    }

                    if (firstPrevious != null && secondPrevious != null && LineAlgorithms.Intersects(firstPrevious, first, secondPrevious, second))
                    {
                        this.result = 0;
                        this.hasResult = true;
                        return;
                    }

                    secondPrevious = second;
                }

                firstPrevious = first;
            }

            this.result = this.precisionModel.MakePrecise(minDistance);
            this.hasResult = true;
        }

        /// <summary>
        /// Computes the square of the distance between a coordinate and a line.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <returns>The square of the distance between the coordinate and the line.</returns>
        private static Double DistanceSquareFromLine(Coordinate coordinate, Coordinate lineStart, Coordinate lineEnd)
        {
            Double lengthSquare = DistanceSquare(lineStart, lineEnd);

            if (lengthSquare == 0)
            {
                return DistanceSquare(coordinate, lineStart);
            }

            Double t = ((coordinate.X - lineStart.X) * (lineEnd.X - lineStart.X) + (coordinate.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y)) / lengthSquare;
            if (t < 0)
            {
                return DistanceSquare(coordinate, lineStart);
            }

            if (t > 1)
            {
                return DistanceSquare(coordinate, lineEnd);
            }

            return DistanceSquare(coordinate, new Coordinate(lineStart.X + t * (lineEnd.X - lineStart.X), lineStart.Y + t * (lineEnd.Y - lineStart.Y)));
        }

        /// <summary>
        /// Computes the distance between a coordinate and a line.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <returns>The distance between the coordinate and the line.</returns>
        private static Double DistanceFromLine(Coordinate coordinate, Coordinate lineStart, Coordinate lineEnd)
        {
            return Math.Sqrt(DistanceSquareFromLine(coordinate, lineStart, lineEnd));
        }

        /// <summary>
        /// Returns the square of the distance of the specified coordinates.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns>The square of the distance of the specified coordinates.</returns>
        private static Double DistanceSquare(Coordinate first, Coordinate second)
        {
            Double x = first.X - second.X, y = first.Y - second.Y, z = first.Z - second.Z;

            return x * x + y * y + z * z;
        }
    }
}
