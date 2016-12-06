// <copyright file="GeometryComparer.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a comparer for <see cref="IGeometry" /> instances.
    /// </summary>
    public class GeometryComparer : IComparer<IGeometry>, IComparer<IPoint>, IComparer<ILineString>, IComparer<IPolygon>, IComparer<IEnumerable<IGeometry>>
    {
        /// <summary>
        /// The array specifying the order of geometries.
        /// </summary>
        private static readonly Type[] GeometryOrder;

        /// <summary>
        /// The coordinate comparer.
        /// </summary>
        private CoordinateComparer comparer;

        /// <summary>
        /// Initializes static members of the <see cref="GeometryComparer" /> class.
        /// </summary>
        static GeometryComparer()
        {
            GeometryOrder = new Type[] { typeof(IPoint), typeof(IMultiPoint), typeof(ILinearRing), typeof(ILineString), typeof(IMultiLineString), typeof(IPolygon), typeof(IMultiPolygon), typeof(IGeometryCollection) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryComparer" /> class.
        /// </summary>
        public GeometryComparer() { this.comparer = new CoordinateComparer(); }

        /// <summary>
        /// Compares two <see cref="IGeometry" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="first">The first <see cref="IGeometry" /> to compare.</param>
        /// <param name="second">The second <see cref="IGeometry" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first geometry is null.
        /// or
        /// The second geometry is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Comparison of the specified geometries is not supported.</exception>
        public Int32 Compare(IGeometry first, IGeometry second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstGeometryIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondGeometryIsNull);
            if (ReferenceEquals(first, second))
                return 0;

            // in case the classes are different, the order is specified by the following array

            Int32 xIndex = OrderIndex(first);
            Int32 yIndex = OrderIndex(second);

            if (xIndex == GeometryOrder.Length || yIndex == GeometryOrder.Length)
                throw new NotSupportedException(CoreMessages.GeometryComparisonNotSupported);

            if (xIndex < yIndex)
                return -1;
            if (xIndex > yIndex)
                return -1;

            switch (xIndex)
            {
                case 0:
                    return this.Compare(first as IPoint, second as IPoint);
                case 2:
                case 3:
                    // linear ring should be compared as line string
                    return this.Compare(first as ILineString, second as ILineString);
                case 5:
                    return this.Compare(first as IPolygon, second as IPolygon);
                case 1:
                case 4:
                case 6:
                case 7:
                    // all collection types should be compared in the same manner
                    return this.Compare(first as IEnumerable<IGeometry>, second as IEnumerable<IGeometry>);
            }

            throw new NotSupportedException(CoreMessages.GeometryComparisonNotSupported);
        }

        /// <summary>
        /// Compares two <see cref="IPoint" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="first">The first <see cref="IPoint" /> to compare.</param>
        /// <param name="second">The second <see cref="IPoint" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first geometry is null.
        /// or
        /// The second geometry is null.
        /// </exception>
        public Int32 Compare(IPoint first, IPoint second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstGeometryIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondGeometryIsNull);
            if (first == second)
                return 0;

            return this.comparer.Compare(first.Coordinate, second.Coordinate);
        }

        /// <summary>
        /// Compares two <see cref="ILineString" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="first">The first <see cref="ILineString" /> to compare.</param>
        /// <param name="second">The second <see cref="ILineString" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first geometry is null.
        /// or
        /// The second geometry is null.
        /// </exception>
        public Int32 Compare(ILineString first, ILineString second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstGeometryIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondGeometryIsNull);
            if (first == second)
                return 0;

            Int32 index = 0;

            // look for the first different coordinate in the line string
            while (index < first.Count && index < second.Count)
            {
                Int32 comparison = this.comparer.Compare(first.GetCoordinate(index), second.GetCoordinate(index));
                if (comparison != 0)
                    return comparison;
                index++;
            }

            // check whether there are additional coordinates in either line string
            if (index < first.Count)
                return 1;
            if (index < second.Count)
                return -1;
            return 0;
        }

        /// <summary>
        /// Compares two <see cref="IPolygon" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="first">The first <see cref="IPolygon" /> to compare.</param>
        /// <param name="second">The second <see cref="IPolygon" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first geometry is null.
        /// or
        /// The second geometry is null.
        /// </exception>
        public Int32 Compare(IPolygon first, IPolygon second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstGeometryIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondGeometryIsNull);
            if (first == second)
                return 0;

            // check the shell
            Int32 shellCompare = this.Compare(first.Shell, second.Shell);
            if (shellCompare != 0)
                return shellCompare;

            Int32 index = 0;

            // look for the first different hole in the polygon
            while (index < first.HoleCount && index < second.HoleCount)
            {
                Int32 holeCompare = this.Compare(first.GetHole(index), second.GetHole(index));
                if (holeCompare != 0)
                    return holeCompare;
                index++;
            }

            // check whether there are additional holes in either polygon
            if (index < first.HoleCount)
                return 1;
            if (index < second.HoleCount)
                return -1;
            return 0;
        }

        /// <summary>
        /// Compares two <see cref="IEnumerable{IGeometry}" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="first">The first <see cref="IEnumerable{IGeometry}" /> to compare.</param>
        /// <param name="second">The second <see cref="IEnumerable{IGeometry}" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first geometry is null.
        /// or
        /// The second geometry is null.
        /// </exception>
        public Int32 Compare(IEnumerable<IGeometry> first, IEnumerable<IGeometry> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), CoreMessages.FirstGeometryIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), CoreMessages.SecondGeometryIsNull);
            if (first == second)
                return 0;

            IEnumerator<IGeometry> enumX = first.GetEnumerator();
            IEnumerator<IGeometry> enumY = second.GetEnumerator();

            // look for the first different element in the collection
            while (enumX.MoveNext() && enumY.MoveNext())
            {
                Int32 comparison = this.Compare(enumX.Current, enumY.Current);
                if (comparison != 0)
                    return comparison;
            }

            // check whether there are additional elements in either collection
            if (enumX.MoveNext())
                return 1;
            if (enumY.MoveNext())
                return -1;
            return 0;
        }

        /// <summary>
        /// Returns the index of the specified geometry within the geometry order.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The index of the geometry within the geometry order.</returns>
        private static Int32 OrderIndex(IGeometry geometry)
        {
            return geometry.GetType().GetTypeInfo().ImplementedInterfaces.Min(type =>
            {
                Int32 index = Array.IndexOf(GeometryOrder, type);

                return index >= 0 ? index : GeometryOrder.Length;
            });
        }
    }
}
