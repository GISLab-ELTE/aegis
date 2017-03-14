// <copyright file="PolygonAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Numerics;
    using AEGIS.Resources;

    /// <summary>
    /// Contains algorithms for computing planar polygon properties.
    /// </summary>
    public static class PolygonAlgorithms
    {
        /// <summary>
        /// Computes the area of the specified polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Double Area(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            return Math.Abs(SignedArea(polygon));
        }

        /// <summary>
        /// Computes the area of the specified polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double Area(IEnumerable<Coordinate> shell)
        {
            return Math.Abs(SignedArea(shell));
        }

        /// <summary>
        /// Computes the area of the specified polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes in reverse order to the shell.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double Area(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            return Math.Abs(SignedArea(shell, holes));
        }

        /// <summary>
        /// Computes the signed area of the specified polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The signed area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Double SignedArea(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            return SignedArea(polygon.Shell, polygon.Holes);
        }

        /// <summary>
        /// Computes the signed area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The area of the polygon. The area is positive if the coordinates are in counterclockwise order; otherwise it is negative. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double SignedArea(IEnumerable<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            IEnumerator<Coordinate> enumerator = shell.Elements().GetEnumerator();
            if (!enumerator.MoveNext())
                return 0;

            Double area = 0;
            Int32 count = 1;
            Coordinate current = enumerator.Current, next;

            while (enumerator.MoveNext())
            {
                next = enumerator.Current;
                area += current.X * next.Y - next.X * current.Y;

                count++;
                current = next;
            }

            if (count < 3)
                return 0;

            return -area / 2;
        }

        /// <summary>
        /// Computes the signed area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes in reverse order to the shell.</param>
        /// <returns>The area of the polygon. The area is positive if the coordinates of the shell are in counterclockwise order; otherwise it is negative. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double SignedArea(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            Double area = SignedArea(shell);
            if (holes != null)
            {
                foreach (IReadOnlyList<Coordinate> hole in holes)
                {
                    if (hole != null)
                        area += SignedArea(hole);
                }
            }

            return area;
        }

        /// <summary>
        /// Determines whether the specified polygon is convex.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns><c>true</c> if the polygon is convex; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Boolean IsConvex(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            if (polygon.HoleCount > 0)
                return false;

            return IsConvex(polygon.Shell);
        }

        /// <summary>
        /// Determines whether the specified polygon is convex.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns><c>true</c> if the polygon is convex; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsConvex(IEnumerable<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            IEnumerator<Coordinate> enumerator = shell.Elements().GetEnumerator();
            if (!enumerator.MoveNext())
                return true;

            Coordinate first = enumerator.Current, previous = first;

            if (!enumerator.MoveNext())
                return true;

            Coordinate second = enumerator.Current, current = second;

            if (!enumerator.MoveNext())
                return true;

            Coordinate next = enumerator.Current;

            Orientation initialOrientation = Coordinate.Orientation(previous, current, next), orientation;

            while (enumerator.MoveNext())
            {
                previous = current;
                current = next;
                next = enumerator.Current;
                orientation = Coordinate.Orientation(previous, current, next);

                if (initialOrientation == AEGIS.Orientation.Collinear)
                    orientation = initialOrientation;
                else if (orientation != initialOrientation && orientation != AEGIS.Orientation.Collinear)
                    return false;
            }

            orientation = Coordinate.Orientation(current, first, second); // the edge around the starting coordinate

            return orientation == initialOrientation || orientation == AEGIS.Orientation.Collinear;
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            return IsValid(polygon.Shell, polygon.Holes, true, null);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IBasicPolygon polygon, PrecisionModel precisionModel)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            return IsValid(polygon.Shell, polygon.Holes, true, precisionModel);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IEnumerable<Coordinate> shell)
        {
            return IsValid(shell, null, true, null);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IEnumerable<Coordinate> shell, PrecisionModel precisionModel)
        {
            return IsValid(shell, null, true, precisionModel);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            return IsValid(shell, holes, true, null);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, PrecisionModel precisionModel)
        {
            return IsValid(shell, holes, true, precisionModel);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <param name="validateIntersections">A value indicating whether to validate for intersections.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Boolean validateIntersections)
        {
            return IsValid(shell, holes, validateIntersections, null);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <param name="validateIntersections">A value indicating whether to validate for intersections.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes, Boolean validateIntersections, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            // shell and holes existence
            IEnumerator<Coordinate> shellEnumerator = shell.GetEnumerator();
            if (!shellEnumerator.MoveNext())
            {
                // shell has no coordinates
                if (holes == null)
                    return true;

                return holes.Any(hole => holes.AnyElement());
            }

            Int32 count = 1;
            Coordinate first = shellEnumerator.Current, previous = first, current = first;
            if (first == null || !first.IsValid)
                return false;

            while (shellEnumerator.MoveNext())
            {
                current = shellEnumerator.Current;

                if (current == null || !current.IsValid || current.Z != first.Z || precisionModel.AreEqual(current, previous))
                    return false;

                previous = current;
                count++;
            }

            if (count < 4 || !precisionModel.AreEqual(first, current))
                return false;

            List<IEnumerable<Coordinate>> ringList = new List<IEnumerable<Coordinate>>();
            ringList.Add(shell);

            // holes
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in holes)
                {
                    if (hole == null)
                        continue;

                    IEnumerator<Coordinate> holeEnumerator = hole.GetEnumerator();
                    if (!holeEnumerator.MoveNext())
                        continue;

                    count = 1;
                    first = holeEnumerator.Current;
                    if (first == null || !first.IsValid)
                        return false;

                    while (holeEnumerator.MoveNext())
                    {
                        current = holeEnumerator.Current;

                        if (current == null || !current.IsValid || current.Z != first.Z || precisionModel.AreEqual(current, previous))
                            return false;

                        previous = current;
                        count++;
                    }

                    if (count < 4 || !precisionModel.AreEqual(first, current))
                        return false;

                    ringList.Add(hole);
                }
            }

            // check for any intersection
            if (validateIntersections && ShamosHoeyAlgorithm.Intersects(ringList, precisionModel))
                return false;

            return true;
        }

        /// <summary>
        /// Computes the planar orientation of a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The orientation of the polygon shell. If the polygon is invalid <c>Undefined</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Orientation Orientation(IBasicPolygon polygon)
        {
            return Orientation(polygon, null);
        }

        /// <summary>
        /// Computes the planar orientation of a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The orientation of the polygon shell. If the polygon is invalid <c>Undefined</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Orientation Orientation(IBasicPolygon polygon, PrecisionModel precisionModel)
        {
            if (polygon == null)
                throw new ArgumentNullException(nameof(polygon), CoreMessages.PolygonIsNull);

            return Orientation(polygon.Shell, precisionModel);
        }

        /// <summary>
        /// Computes the planar orientation of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The orientation of the polygon. If the polygon is invalid <see cref="AEGIS.Orientation.Undefined" /> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Orientation Orientation(IEnumerable<Coordinate> shell)
        {
            return Orientation(shell, null);
        }

        /// <summary>
        /// Computes the planar orientation of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The orientation of the polygon. If the polygon is invalid <see cref="AEGIS.Orientation.Undefined" /> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Orientation Orientation(IEnumerable<Coordinate> shell, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            // check for polygon properties
            IEnumerator<Coordinate> enumerator = shell.GetEnumerator();

            Double sum = 0;
            Int32 count = 1;

            if (!enumerator.MoveNext())
                return AEGIS.Orientation.Undefined;

            Coordinate first = enumerator.Current, previous = first, current = first;

            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                sum += (current.X - previous.X) * (current.Y + previous.Y);
                count++;

                previous = current;
            }

            if (count < 3)
                return AEGIS.Orientation.Undefined;

            if (precisionModel == null)
                precisionModel = PrecisionModel.Default;

            if (!precisionModel.AreEqual(first, current))
                sum += (first.X - current.X) * (first.Y + current.Y);

            if (Math.Abs(sum) <= precisionModel.Tolerance(shell))
                return AEGIS.Orientation.Collinear;

            return (sum > 0) ? AEGIS.Orientation.Clockwise : AEGIS.Orientation.Counterclockwise;
        }
    }
}
