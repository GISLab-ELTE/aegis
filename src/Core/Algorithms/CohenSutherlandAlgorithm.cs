// <copyright file="CohenSutherlandAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a type for performing the Cohen–Sutherland algorithm.
    /// </summary>
    /// <remarks>
    /// The Cohen–Sutherland algorithm is a computational geometry algorithm used for line clipping using a rectangular
    /// clipping window. The algorithm divides a two-dimensional space into 9 regions, and then efficiently determines
    /// the lines and portions of lines that are visible in the center region of interest (the viewport).
    /// </remarks>
    public class CohenSutherlandAlgorithm
    {
        /// <summary>
        /// The bits represent the location of the point in relation to the viewport.
        /// </summary>
        private enum OutCode
        {
            /// <summary>
            /// Indicates that the coordinate is inside the envelope.
            /// </summary>
            Inside = 0,

            /// <summary>
            /// Indicates that the coordinate is left to the envelope.
            /// </summary>
            Left = 1,

            /// <summary>
            /// Indicates that the coordinate is right to the envelope.
            /// </summary>
            Right = 2,

            /// <summary>
            /// Indicates that the coordinate is below the envelope.
            /// </summary>
            Bottom = 4,

            /// <summary>
            /// Indicates that the coordinate is above the envelope.
            /// </summary>
            Top = 8
        }

        /// <summary>
        /// The collection of source line strings.
        /// </summary>
        private IEnumerable<IReadOnlyList<Coordinate>> source;

        /// <summary>
        /// The clipping window.
        /// </summary>
        private Envelope window;

        /// <summary>
        /// The list of clipped line strings.
        /// </summary>
        private List<List<Coordinate>> result;

        /// <summary>
        /// A values indicating whether the result was already computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="CohenSutherlandAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of coordinates to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CohenSutherlandAlgorithm(IReadOnlyList<Coordinate> source, Envelope window, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);
            if (window == null)
                throw new ArgumentNullException(nameof(window), CoreMessages.ClippingWindowIsNull);

            this.source = new List<IReadOnlyList<Coordinate>>();
            (this.source as List<IReadOnlyList<Coordinate>>).Add(source);
            this.window = window;
            this.hasResult = false;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CohenSutherlandAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The list of line strings to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public CohenSutherlandAlgorithm(IEnumerable<IReadOnlyList<Coordinate>> source, Envelope window, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);
            if (window == null)
                throw new ArgumentNullException(nameof(window), CoreMessages.ClippingWindowIsNull);

            this.source = source;
            this.window = window;
            this.hasResult = false;
            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
        }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the collection of source line strings.
        /// </summary>
        public IEnumerable<IReadOnlyList<Coordinate>> Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the clipping window.
        /// </summary>
        public Envelope Window
        {
            get { return this.window; }
        }

        /// <summary>
        /// Gets the collection of clipped line strings.
        /// </summary>
        public IReadOnlyList<IReadOnlyList<Coordinate>> Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();

                return this.result;
            }
        }

        /// <summary>
        /// Clips a line string with an envelope.
        /// </summary>
        /// <param name="source">The line string to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IReadOnlyList<IReadOnlyList<Coordinate>> Clip(IBasicLineString source, Envelope window)
        {
            return Clip(source, window, null);
        }

        /// <summary>
        /// Clips a line string with an envelope.
        /// </summary>
        /// <param name="source">The line string to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IReadOnlyList<IReadOnlyList<Coordinate>> Clip(IBasicLineString source, Envelope window, PrecisionModel precisionModel)
        {
            return new CohenSutherlandAlgorithm(source, window, precisionModel).Result;
        }

        /// <summary>
        /// Clips a polygons with an envelope.
        /// </summary>
        /// <param name="source">The polygon to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IReadOnlyList<IReadOnlyList<Coordinate>> Clip(IBasicPolygon source, Envelope window)
        {
            return Clip(source, window, null);
        }

        /// <summary>
        /// Clips a polygons with an envelope.
        /// </summary>
        /// <param name="source">The polygon to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IReadOnlyList<IReadOnlyList<Coordinate>> Clip(IBasicPolygon source, Envelope window, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            List<IReadOnlyList<Coordinate>> coordinates = new List<IReadOnlyList<Coordinate>>();

            coordinates.Add(source.Shell);

            foreach (IBasicLineString hole in source.Holes)
            {
                if (hole == null)
                    continue;

                coordinates.Add(hole);
            }

            return new CohenSutherlandAlgorithm(coordinates, window, precisionModel).Result;
        }

        /// <summary>
        /// Clips a collection of polygons with an envelope.
        /// </summary>
        /// <param name="source">The collection of polygons to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IReadOnlyList<IReadOnlyList<Coordinate>> Clip(IEnumerable<IBasicPolygon> source, Envelope window)
        {
            return Clip(source, window, null);
        }

        /// <summary>
        /// Clips a collection of polygons with an envelope.
        /// </summary>
        /// <param name="source">The collection of polygons to clip.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The clipped coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The clipping window is null.
        /// </exception>
        public static IReadOnlyList<IReadOnlyList<Coordinate>> Clip(IEnumerable<IBasicPolygon> source, Envelope window, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);
            if (window == null)
                throw new ArgumentNullException(nameof(window), CoreMessages.ClippingWindowIsNull);

            List<IReadOnlyList<Coordinate>> coordinates = new List<IReadOnlyList<Coordinate>>();

            foreach (IBasicPolygon basicPolygon in source)
            {
                if (basicPolygon == null || basicPolygon.Shell == null)
                    continue;

                coordinates.Add(basicPolygon.Shell);

                foreach (IBasicLineString hole in basicPolygon.Holes)
                {
                    if (hole == null)
                        continue;

                    coordinates.Add(hole);
                }
            }

            return new CohenSutherlandAlgorithm(coordinates, window, precisionModel).Result;
        }

        /// <summary>
        /// Computes the result of the algorithm.
        /// </summary>
        public void Compute()
        {
            this.result = new List<List<Coordinate>>();

            foreach (IReadOnlyList<Coordinate> lineString in this.source)
            {
                List<Coordinate> clippedLineString = new List<Coordinate>();

                for (Int32 index = 0; index < lineString.Count - 1; ++index)
                {
                    Coordinate[] calculatedCoordinates = Clip(lineString[index], lineString[index + 1], this.window);

                    if (calculatedCoordinates == null)
                        continue;

                    if (clippedLineString.Count != 0 && this.PrecisionModel.AreEqual(clippedLineString[clippedLineString.Count - 1], calculatedCoordinates[0]))
                    {
                        clippedLineString.Add(calculatedCoordinates[calculatedCoordinates.Length - 1]);
                    }
                    else
                    {
                        if (clippedLineString.Count != 0)
                            this.result.Add(clippedLineString);

                        clippedLineString = new List<Coordinate>(calculatedCoordinates);
                    }
                }

                this.result.Add(clippedLineString);
            }

            this.hasResult = true;
        }

        /// <summary>
        /// Compute the bit code of a coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The computed bitcode.</returns>
        private static OutCode ComputeOutCode(Coordinate coordinate, Envelope window)
        {
            return ComputeOutCode(coordinate.X, coordinate.Y, window);
        }

        /// <summary>
        /// Compute the bit code of a coordinate.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The computed bitcode.</returns>
        private static OutCode ComputeOutCode(Double x, Double y, Envelope window)
        {
            OutCode code = OutCode.Inside;

            if (x < window.MinX)
                code |= OutCode.Left;
            if (x > window.MaxX)
                code |= OutCode.Right;
            if (y < window.MinY)
                code |= OutCode.Top;
            if (y > window.MaxY)
                code |= OutCode.Bottom;

            return code;
        }

        /// <summary>
        /// Clips a line.
        /// </summary>
        /// <param name="first">The first coordinate of the line.</param>
        /// <param name="second">The second coordinate of the line.</param>
        /// <param name="window">The clipping window.</param>
        /// <returns>The clipped line.</returns>
        private static Coordinate[] Clip(Coordinate first, Coordinate second, Envelope window)
        {
            if (first == null || second == null)
                return null;

            Boolean accept = false;

            OutCode outCodeFirst = ComputeOutCode(first, window);
            OutCode outCodeSecond = ComputeOutCode(second, window);

            while (!accept)
            {
                if ((outCodeFirst | outCodeSecond) == OutCode.Inside)
                {
                    accept = true;
                    break;
                }

                if ((outCodeFirst & outCodeSecond) != 0)
                    break;

                OutCode outCode = outCodeFirst != OutCode.Inside ? outCodeFirst : outCodeSecond;

                Coordinate intersectionCoordinate = ComputeIntersection(first, second, window, outCode);

                if (outCode == outCodeFirst)
                {
                    first = intersectionCoordinate;
                    outCodeFirst = ComputeOutCode(first, window);
                }

                if (outCode == outCodeSecond)
                {
                    second = intersectionCoordinate;
                    outCodeSecond = ComputeOutCode(second, window);
                }
            }

            if (accept)
                return new Coordinate[] { first, second };

            return null;
        }

        /// <summary>
        /// Computes the intersection of a line.
        /// </summary>
        /// <param name="first">The first coordinate of the line.</param>
        /// <param name="second">The second coordinate of the line.</param>
        /// <param name="window">The clipping window.</param>
        /// <param name="clipTo">Location of first coordinate in relation to the region of interest.</param>
        /// <returns>The intersection coordinate.</returns>
        /// <exception cref="System.ArgumentException">The code is invalid.</exception>
        private static Coordinate ComputeIntersection(Coordinate first, Coordinate second, Envelope window, OutCode clipTo)
        {
            Double dx = second.X - first.X;
            Double dy = second.Y - first.Y;

            Double slopeY = dx / dy;
            Double slopeX = dy / dx;

            if (clipTo.HasFlag(OutCode.Top))
            {
                return new Coordinate(first.X + slopeY * (window.MaxY - first.Y), window.MaxY);
            }

            if (clipTo.HasFlag(OutCode.Bottom))
            {
                return new Coordinate(first.X + slopeY * (window.MinY - first.Y), window.MinY);
            }

            if (clipTo.HasFlag(OutCode.Right))
            {
                return new Coordinate(window.MaxX, first.Y + slopeX * (window.MaxX - first.X));
            }

            if (clipTo.HasFlag(OutCode.Left))
            {
                return new Coordinate(window.MinX, first.Y + slopeX * (window.MinX - first.X));
            }

            return Coordinate.Undefined;
        }
    }
}
