// <copyright file="LineAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Collections;
    using AEGIS.Numerics;
    using AEGIS.Resources;

    /// <summary>
    /// Contains algorithms for computing line properties.
    /// </summary>
    public static class LineAlgorithms
    {
        /// <summary>
        /// Computes the centroid of a line.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <returns>The centroid of the line. The centroid is <c>Undefined</c> if either coordinates are invalid.</returns>
        public static Coordinate Centroid(Coordinate lineStart, Coordinate lineEnd)
        {
            if (lineStart == null || lineEnd == null)
                return Coordinate.Undefined;

            return new Coordinate((lineStart.X + lineEnd.X) / 2, (lineStart.Y + lineEnd.Y) / 2, (lineStart.Z + lineEnd.Z) / 2);
        }

        /// <summary>
        /// Computes the centroid of a line.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The centroid of the line. The centroid is <c>Undefined</c> if either coordinates are invalid.</returns>
        public static Coordinate Centroid(Coordinate lineStart, Coordinate lineEnd, PrecisionModel precisionModel)
        {
            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            return precisionModel.MakePrecise(Centroid(lineStart, lineEnd));
        }

        /// <summary>
        /// Computes the centroid of a line string.
        /// </summary>
        /// <param name="coordinates">The list of coordinates representing the line string.</param>
        /// <returns>The centroid of the line string. The centroid is <c>Undefined</c> if either coordinates are invalid or there are no coordinates specified.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of coordinates is null.</exception>
        public static Coordinate Centroid(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            // simple cases
            IEnumerator<Coordinate> coordinateEnumerator = coordinates.Elements().GetEnumerator();

            if (!coordinateEnumerator.MoveNext())
                return Coordinate.Undefined;

            Coordinate current = coordinateEnumerator.Current;

            // take the centroids of the edges weighted with the length of the edge
            Double centroidX = 0, centroidY = 0, centroidZ = 0;
            Double totalLength = 0;

            while (coordinateEnumerator.MoveNext())
            {
                if (!coordinateEnumerator.Current.IsValid)
                    return Coordinate.Undefined;

                Double length = Coordinate.Distance(current, coordinateEnumerator.Current);
                totalLength += length;

                centroidX += length * (current.X + coordinateEnumerator.Current.X) / 2;
                centroidY += length * (current.Y + coordinateEnumerator.Current.Y) / 2;
                centroidZ += length * (current.Z + coordinateEnumerator.Current.Z) / 2;

                current = coordinateEnumerator.Current;
            }

            if (totalLength == 0)
                return current;

            return new Coordinate(centroidX / totalLength, centroidY / totalLength, centroidZ / totalLength);
        }

        /// <summary>
        /// Computes the centroid of a line string.
        /// </summary>
        /// <param name="coordinates">The coordinates of the line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The centroid of the line string. The centroid is <c>Undefined</c> if either coordinates are invalid or there are no coordinates specified.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of coordinates is null.</exception>
        public static Coordinate Centroid(IEnumerable<Coordinate> coordinates, PrecisionModel precisionModel)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            return precisionModel.MakePrecise(Centroid(coordinates));
        }

        /// <summary>
        /// Determines whether two infinite lines coincide.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns><c>true</c> if the two lines coincide; otherwise, <c>false</c>.</returns>
        public static Boolean Coincides(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            return Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two infinite lines coincide.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the two lines coincide; otherwise, <c>false</c>.</returns>
        public static Boolean Coincides(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precisionModel)
        {
            if (firstCoordinate == null || firstVector == null || secondCoordinate == null || secondVector == null)
                return false;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            return CoordinateVector.IsParallel(firstVector, secondVector, precisionModel) &&
                   Distance(firstCoordinate, firstVector, secondCoordinate) <= Math.Max(precisionModel.Tolerance(firstCoordinate, secondCoordinate), precisionModel.Tolerance(firstVector, secondVector));
        }

        /// <summary>
        /// Determines whether a line contains a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate)
        {
            return Contains(lineStart, lineEnd, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether a line contains a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (lineStart == null || lineEnd == null || coordinate == null)
                return false;

            if (!Envelope.Contains(lineStart, lineEnd, coordinate))
                return false;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            return Distance(lineStart, lineEnd, coordinate, precisionModel) <= precisionModel.Tolerance(lineStart, lineEnd, coordinate);
        }

        /// <summary>
        /// Determines whether an infinite line contains a specified coordinate.
        /// </summary>
        /// <param name="lineCoordinate">The coordinate of the line.</param>
        /// <param name="lineVector">The direction vector of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineCoordinate, CoordinateVector lineVector, Coordinate coordinate)
        {
            return Contains(lineCoordinate, lineVector, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether an infinite line contains a specified coordinate.
        /// </summary>
        /// <param name="lineCoordinate">The coordinate of the line.</param>
        /// <param name="lineVector">The direction vector of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineCoordinate, CoordinateVector lineVector, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (lineCoordinate == null || lineVector == null || coordinate == null)
                return false;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            return Distance(lineCoordinate, lineVector, coordinate) <= precisionModel.Tolerance(lineCoordinate, coordinate);
        }

        /// <summary>
        /// Computes the distance of a line to a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The distance of <paramref name="coordinate" /> from the line.</returns>
        public static Double Distance(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate)
        {
            return Distance(lineStart, lineEnd, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the distance of a line to a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance of <paramref name="coordinate" /> from the line.</returns>
        public static Double Distance(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (lineStart == null || lineEnd == null || coordinate == null)
                return Double.NaN;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            // check for empty line
            if (precisionModel.AreEqual(lineStart, lineEnd))
                return Coordinate.Distance(coordinate, lineStart);

            // compute directional vector
            CoordinateVector lineVector = lineEnd - lineStart;

            // check for the starting and ending coordinates
            Double start = (coordinate - lineStart) * lineVector;
            if (start <= 0)
                return Coordinate.Distance(coordinate, lineStart);
            Double end = lineVector * lineVector;
            if (end <= start)
                return Coordinate.Distance(coordinate, lineEnd);

            // compute distance to the nearest coordinate
            return Coordinate.Distance(coordinate, lineStart + (start / end) * lineVector);
        }

        /// <summary>
        /// Computes the distance of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns>The distance of the two lines.</returns>
        public static Double Distance(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            return Distance(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the distance of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The distance of the two lines.</returns>
        public static Double Distance(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precisionModel)
        {
            if (firstLineStart == null || firstLineEnd == null || secondLineStart == null || secondLineEnd == null)
                return Double.NaN;

            CoordinateVector firstVector = firstLineEnd - firstLineStart;
            CoordinateVector secondVector = secondLineEnd - secondLineStart;
            CoordinateVector startVector = firstLineStart - secondLineStart;
            Double firstDotProduct = firstVector * firstVector;
            Double firstToSecondDotProduct = firstVector * secondVector;
            Double secondDotProduct = secondVector * secondVector;
            Double firstToStartDotProduct = firstVector * startVector;
            Double secondToStartDotProduct = secondVector * startVector;
            Double differenceScalar = firstDotProduct * secondDotProduct - firstToSecondDotProduct * firstToSecondDotProduct;
            Double startNominator, startDenominator = differenceScalar;
            Double endNominator, endDenominator = differenceScalar;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            Double tolerance = precisionModel.Tolerance(firstLineStart, firstLineStart, secondLineStart, secondLineEnd);

            if (differenceScalar <= tolerance)
            {
                // the lines are collinear
                startNominator = 0.0;
                startDenominator = 1.0;
                endNominator = secondToStartDotProduct;
                endDenominator = secondDotProduct;
            }
            else
            {
                startNominator = firstToSecondDotProduct * secondToStartDotProduct - secondDotProduct * firstToStartDotProduct;
                endNominator = firstDotProduct * secondToStartDotProduct - firstToSecondDotProduct * firstToStartDotProduct;
                if (startNominator < 0.0)
                {
                    startNominator = 0.0;
                    endNominator = secondToStartDotProduct;
                    endDenominator = secondDotProduct;
                }
                else if (startNominator > startDenominator)
                {
                    startNominator = startDenominator;
                    endNominator = secondToStartDotProduct + firstToSecondDotProduct;
                    endDenominator = secondDotProduct;
                }
            }

            if (endNominator < 0.0)
            {
                endNominator = 0.0;
                if (-firstToStartDotProduct < 0.0)
                {
                    startNominator = 0.0;
                }
                else if (-firstToStartDotProduct > firstDotProduct)
                {
                    startNominator = startDenominator;
                }
                else
                {
                    startNominator = -firstToStartDotProduct;
                    startDenominator = firstDotProduct;
                }
            }
            else if (endNominator > endDenominator)
            {
                endNominator = endDenominator;
                if ((-firstToStartDotProduct + firstToSecondDotProduct) < 0.0)
                {
                    startNominator = 0;
                }
                else if ((-firstToStartDotProduct + firstToSecondDotProduct) > firstDotProduct)
                {
                    startNominator = startDenominator;
                }
                else
                {
                    startNominator = -firstToStartDotProduct + firstToSecondDotProduct;
                    startDenominator = firstDotProduct;
                }
            }

            Double start = Math.Abs(startNominator) <= tolerance ? 0.0 : startNominator / startDenominator;
            Double end = Math.Abs(endNominator) <= tolerance ? 0.0 : endNominator / endDenominator;
            CoordinateVector distanceVector = startVector + (start * firstVector) - (end * secondVector);

            if (distanceVector.IsNull)
                return 0;

            return Math.Sqrt(distanceVector * distanceVector);
        }

        /// <summary>
        /// Computes the distance of an infinite line to a specified coordinate.
        /// </summary>
        /// <param name="lineCoordinate">The line coordinate.</param>
        /// <param name="lineVector">The line vector.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The distance of <paramref name="coordinate" /> to the line. The distance is <c>NaN</c> if not all parameters are valid.</returns>
        public static Double Distance(Coordinate lineCoordinate, CoordinateVector lineVector, Coordinate coordinate)
        {
            if (lineCoordinate == null || lineVector == null || coordinate == null)
                return Double.NaN;

            Double x = Math.Abs(lineVector.Y * coordinate.X + (-lineVector.X) * coordinate.Y - (lineVector.Y * lineCoordinate.X + ((-lineVector.X) * lineCoordinate.Y)));
            Double y = Math.Sqrt(Math.Pow(lineVector.Y, 2) + Math.Pow(lineVector.X, 2));
            return x / y;
        }

        /// <summary>
        /// Returns the length of the specified coordinate collection.
        /// </summary>
        /// <param name="coordinates">The collection of coordinates.</param>
        /// <returns>The length of the collection.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of coordinates is null.</exception>
        public static Double Length(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            IEnumerator<Coordinate> enumerator = coordinates.Elements().GetEnumerator();
            if (!enumerator.MoveNext())
                return 0;

            Double length = 0;
            Coordinate current = enumerator.Current;
            while (enumerator.MoveNext())
            {
                length += Coordinate.Distance(current, enumerator.Current);
                current = enumerator.Current;
            }

            return length;
        }

        /// <summary>
        /// Determines whether two lines intersect internally.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean InternalIntersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            Intersection intersection = ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, PrecisionModel.Default);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Determines whether two lines intersect internally.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean InternalIntersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precisionModel)
        {
            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            Intersection intersection = ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, precisionModel);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Determines whether two lines intersect.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            Intersection intersection = ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, PrecisionModel.Default);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Determines whether two lines intersect.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precisionModel)
        {
            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            Intersection intersection = ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, precisionModel);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Determines whether two infinite lines intersect.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            Intersection intersection = ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Determines whether two infinite lines intersect.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precisionModel)
        {
            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            Intersection intersection = ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, precisionModel);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Determines whether a line intersects with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static Boolean IntersectsWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector)
        {
            Intersection intersection = ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, PrecisionModel.Default);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Determines whether a line intersects with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static Boolean IntersectsWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector, PrecisionModel precisionModel)
        {
            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            Intersection intersection = ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, precisionModel);

            return intersection != null && intersection.Type != IntersectionType.None;
        }

        /// <summary>
        /// Computes the internal intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection InternalIntersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the internal intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection InternalIntersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precisionModel)
        {
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, precisionModel);
        }

        /// <summary>
        /// Computes the intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection Intersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection Intersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precisionModel)
        {
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, precisionModel);
        }

        /// <summary>
        /// Computes the intersection of two infinite lines.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection Intersection(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            return ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the intersection of two infinite lines.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection Intersection(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precisionModel)
        {
            return ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, precisionModel);
        }

        /// <summary>
        /// Computes the intersection of a line with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection IntersectionWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector)
        {
            return ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the intersection of a line with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The computed intersection.</returns>
        public static Intersection IntersectionWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector, PrecisionModel precisionModel)
        {
            return ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, precisionModel);
        }

        /// <summary>
        /// Determines whether two lines are collinear.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines are collinear; otherwise, <c>false</c>.</returns>
        public static Boolean IsCollinear(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            return IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two lines are collinear.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the two lines are collinear; otherwise, <c>false</c>.</returns>
        public static Boolean IsCollinear(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precisionModel)
        {
            if (firstLineStart == null || firstLineEnd == null || secondLineStart == null || secondLineEnd == null)
                return false;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            // compute the directional vectors
            CoordinateVector firstVector = (firstLineEnd - firstLineStart).Normalize();
            CoordinateVector secondVector = (secondLineEnd - secondLineStart).Normalize();

            return CoordinateVector.IsParallel(firstVector, secondVector, precisionModel) &&
                   Coordinate.Distance(secondLineStart, firstLineStart + (secondLineStart - firstLineStart) * firstVector * firstVector) <= precisionModel.Tolerance(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
        }

        /// <summary>
        /// Determines whether two lines are parallel.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            return IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two lines are parallel.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precisionModel)
        {
            if (firstLineStart == null || firstLineEnd == null || secondLineStart == null || secondLineEnd == null)
                return false;

            // check if the directional vectors are parallel
            return CoordinateVector.IsParallel(firstLineEnd - firstLineStart, secondLineEnd - secondLineStart, precisionModel);
        }

        /// <summary>
        /// Determines whether two infinite lines are parallel.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            return IsParallel(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two infinite lines are parallel.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precisionModel)
        {
            if (firstCoordinate == null || firstVector == null || secondCoordinate == null || secondVector == null)
                return false;

            return CoordinateVector.IsParallel(firstVector, secondVector, precisionModel);
        }

        /// <summary>
        /// Computes the intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="includeBoundary">A value indicating whether to include intersection on the boundary.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The intersection.</returns>
        private static Intersection ComputeIntersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, Boolean includeBoundary, PrecisionModel precisionModel)
        {
            // check validity
            if (firstLineStart == null || firstLineEnd == null || secondLineStart == null || secondLineEnd == null)
                return null;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            // check for empty lines
            if (firstLineStart.Equals(firstLineEnd))
            {
                if (firstLineStart.Equals(secondLineStart) || firstLineStart.Equals(secondLineEnd))
                {
                    return new Intersection(precisionModel.MakePrecise(firstLineStart));
                }

                if (Contains(secondLineStart, secondLineEnd, firstLineStart, precisionModel))
                {
                    return new Intersection(precisionModel.MakePrecise(firstLineStart));
                }
                else
                {
                    return null;
                }
            }

            if (secondLineStart.Equals(secondLineEnd))
            {
                if (secondLineStart.Equals(firstLineStart) || secondLineStart.Equals(firstLineEnd))
                {
                    return new Intersection(precisionModel.MakePrecise(secondLineStart));
                }

                if (Contains(firstLineStart, firstLineEnd, secondLineStart, precisionModel))
                {
                    return new Intersection(precisionModel.MakePrecise(secondLineStart));
                }
                else
                {
                    return null;
                }
            }

            // check for equal lines
            if ((firstLineStart.Equals(secondLineStart) && firstLineEnd.Equals(secondLineEnd)) ||
                (firstLineEnd.Equals(secondLineStart) && firstLineStart.Equals(secondLineEnd)))
            {
                return new Intersection(precisionModel.MakePrecise(secondLineStart), precisionModel.MakePrecise(secondLineEnd));
            }

            // check for the envelope
            if (!Envelope.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd))
            {
                return null;
            }

            // compute the direction vectors
            CoordinateVector firstVector = firstLineEnd - firstLineStart;
            CoordinateVector secondVector = secondLineEnd - secondLineStart;
            CoordinateVector startVector = firstLineStart - secondLineStart;

            // check for parallel lines
            if (CoordinateVector.IsParallel(firstVector, secondVector, precisionModel))
            {
                // the starting or ending coordinate of the second line must be on the first line or vice versa
                Double tolerance = precisionModel.Tolerance(secondLineStart, firstLineStart);

                if (LineAlgorithms.Distance(firstLineStart, firstLineEnd, secondLineStart) > tolerance &&
                    LineAlgorithms.Distance(firstLineStart, firstLineEnd, secondLineEnd) > tolerance &&
                    LineAlgorithms.Distance(secondLineStart, secondLineEnd, firstLineStart) > tolerance &&
                    LineAlgorithms.Distance(secondLineStart, secondLineEnd, firstLineEnd) > tolerance)
                {
                    return null;
                }

                Double start, end;
                CoordinateVector differenceVector = firstLineEnd - secondLineStart;
                if (secondVector.X != 0)
                {
                    start = Math.Min(startVector.X / secondVector.X, differenceVector.X / secondVector.X);
                    end = Math.Max(startVector.X / secondVector.X, differenceVector.X / secondVector.X);
                }
                else
                {
                    start = Math.Min(startVector.Y / secondVector.Y, differenceVector.Y / secondVector.Y);
                    end = Math.Max(startVector.Y / secondVector.Y, differenceVector.Y / secondVector.Y);
                }

                if (start > 1 || end < 0)
                {
                    return null;
                }

                return new Intersection(precisionModel.MakePrecise(secondLineStart + Math.Max(start, 0) * secondVector), precisionModel.MakePrecise(secondLineStart + Math.Min(end, 1) * secondVector));
            }

            // the lines are projected into a plane
            Double differencePerp = CoordinateVector.PerpDotProduct(firstVector, secondVector);
            if (differencePerp == 0)
            {
                // this should not happen (case of parallel lines)
                return null;
            }

            Double intervalStart = CoordinateVector.PerpDotProduct(secondVector, startVector) / differencePerp;
            Double intervalEnd = CoordinateVector.PerpDotProduct(firstVector, startVector) / differencePerp;

            if (includeBoundary)
            {
                if (intervalStart < 0 || intervalStart > 1 || intervalEnd < 0 || intervalEnd > 1)
                {
                    // if the intersection is beyond the line
                    return null;
                }
            }
            else
            {
                if (intervalStart <= 0 || intervalStart >= 1 || intervalEnd <= 0 || intervalEnd >= 1)
                {
                    // if the intersection is beyond the line
                    return null;
                }
            }

            // finally, we check if we computed one coordinate
            if (Coordinate.Distance(firstLineStart + intervalStart * firstVector, secondLineStart + intervalEnd * secondVector) > precisionModel.Tolerance(secondLineStart, firstLineStart))
            {
                return null;
            }

            return new Intersection(precisionModel.MakePrecise(firstLineStart + intervalStart * firstVector));
        }

        /// <summary>
        /// Computes the intersection of two infinite lines.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The intersection.</returns>
        private static Intersection ComputeIntersection(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precisionModel)
        {
            if (firstCoordinate == null || firstVector == null || secondCoordinate == null || secondVector == null)
                return null;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            Double tolerance = precisionModel.Tolerance(firstCoordinate, firstCoordinate + firstVector, secondCoordinate, secondCoordinate + secondVector);

            // if they are parallel, they must also be collinear
            if (CoordinateVector.IsParallel(firstVector, secondVector))
            {
                if (Distance(firstCoordinate, firstVector, secondCoordinate) <= tolerance)
                    return new Intersection(precisionModel.MakePrecise(firstCoordinate));

                return null;
            }

            CoordinateVector differenceVector = firstCoordinate - secondCoordinate;

            Double differencePerp = CoordinateVector.PerpDotProduct(firstVector, secondVector);
            if (differencePerp == 0)
            {
                return null;
            }

            Double intervalStart = CoordinateVector.PerpDotProduct(secondVector, differenceVector) / differencePerp;
            Double intervalEnd = CoordinateVector.PerpDotProduct(firstVector, differenceVector) / differencePerp;

            if (Coordinate.Distance(firstCoordinate + intervalStart * firstVector, secondCoordinate + intervalEnd * secondVector) > tolerance)
            {
                return null;
            }

            return new Intersection(precisionModel.MakePrecise(firstCoordinate + intervalStart * firstVector));
        }

        /// <summary>
        /// Computes the intersection of a line with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The intersection.</returns>
        private static Intersection ComputeIntersectionWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector, PrecisionModel precisionModel)
        {
            if (lineStart == null || lineEnd == null || planeCoordinate == null || planeNormalVector == null)
                return null;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            CoordinateVector lineVector = lineEnd - lineStart;
            CoordinateVector coordinateVector = lineStart - planeCoordinate;

            Double lineDot = CoordinateVector.DotProduct(planeNormalVector, lineVector);
            Double coordinateDot = -CoordinateVector.DotProduct(planeNormalVector, coordinateVector);

            if (Math.Abs(lineDot) <= precisionModel.Tolerance(lineStart, lineEnd, planeCoordinate))
            {
                // line is parallel to plane
                if (coordinateDot == 0)
                {
                    // the line lies on the plane
                    return new Intersection(precisionModel.MakePrecise(lineStart), precisionModel.MakePrecise(lineEnd));
                }
                else
                {
                    return null;
                }
            }

            Double ratio = coordinateDot / lineDot;
            if (ratio < 0 || ratio > 1)
                return null;

            // compute segment intersection coordinate
            return new Intersection(precisionModel.MakePrecise(lineStart + ratio * lineVector));
        }
    }
}
