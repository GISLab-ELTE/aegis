// <copyright file="MinkowskiSumAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents the Minkowski addition algorithm for computing the buffer of a geometry.
    /// </summary>
    /// <remarks>
    /// The implementation assumes that the specified coordinates are valid, in counterclockwise order (clockwise order in case of hole coordinates) and in the same plane.
    /// </remarks>
    public class MinkowskiSumAlgorithm
    {
        /// <summary>
        /// The number of points to create a circle. This field is constant.
        /// </summary>
        private const Int32 NumberOfPoints = 128;

        /// <summary>
        /// The list of source shell coordinates.
        /// </summary>
        private readonly IReadOnlyList<Coordinate> sourceShellCoordinates;

        /// <summary>
        /// The list of source hole coordinates.
        /// </summary>
        private readonly IReadOnlyList<IReadOnlyList<Coordinate>> sourceHoleCoordinates;

        /// <summary>
        /// The list of buffer coordinates.
        /// </summary>
        private readonly IReadOnlyList<Coordinate> bufferCoordinates;

        /// <summary>
        /// The coordinates of the result shell.
        /// </summary>
        private List<Coordinate> resultShellCoordinates;

        /// <summary>
        /// The coordinates of the result holes.
        /// </summary>
        private List<List<Coordinate>> resultHoleCoordinates;

        /// <summary>
        /// The result polygon.
        /// </summary>
        private IBasicPolygon result;

        /// <summary>
        /// A values indicating whether the result was already computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinkowskiSumAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public MinkowskiSumAlgorithm(IBasicPoint source, IReadOnlyList<Coordinate> buffer, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.sourceShellCoordinates = new Coordinate[] { source.Coordinate };
            this.sourceHoleCoordinates = null;

            if (buffer.Count > 0 && buffer[0] != buffer[buffer.Count - 1])
                this.bufferCoordinates = new BasicProxyLineString(buffer, true);
            else
                this.bufferCoordinates = buffer;

            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinkowskiSumAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public MinkowskiSumAlgorithm(IBasicPolygon source, IReadOnlyList<Coordinate> buffer, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.sourceShellCoordinates = source.Shell;
            this.sourceHoleCoordinates = source.Holes.Select(hole => hole).ToList();

            if (buffer.Count > 0 && buffer[0] != buffer[buffer.Count - 1])
                this.bufferCoordinates = new BasicProxyLineString(buffer, true);
            else
                this.bufferCoordinates = buffer;

            this.hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinkowskiSumAlgorithm" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public MinkowskiSumAlgorithm(IReadOnlyList<Coordinate> source, IReadOnlyList<Coordinate> buffer, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;

            if (source.Count > 0 && source[0] != source[source.Count - 1])
                this.sourceShellCoordinates = new BasicProxyLineString(source, true);
            else
                this.sourceShellCoordinates = source;

            this.sourceHoleCoordinates = null;

            if (buffer.Count > 0 && buffer[0] != buffer[buffer.Count - 1])
                this.bufferCoordinates = new BasicProxyLineString(buffer, true);
            else
                this.bufferCoordinates = buffer;

            this.hasResult = false;
        }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The buffered polygon.</value>
        public IBasicPolygon Result
        {
            get
            {
                if (!this.hasResult)
                {
                    this.Compute();
                }

                return this.result;
            }
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IBasicPolygon Buffer(IBasicPoint source, Double radius)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, NumberOfPoints), null).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IBasicPolygon Buffer(IBasicPoint source, Double radius, PrecisionModel precisionModel)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, NumberOfPoints), precisionModel).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="numberOfCoordinates">The number of coordinates of the buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon Buffer(IBasicPoint source, Double radius, Int32 numberOfCoordinates)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, numberOfCoordinates), null).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="numberOfCoordinates">The number of coordinates of the buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon Buffer(IBasicPoint source, Double radius, Int32 numberOfCoordinates, PrecisionModel precisionModel)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, numberOfCoordinates), precisionModel).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public static IBasicPolygon Buffer(IBasicPoint source, IBasicPolygon buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return new MinkowskiSumAlgorithm(source, buffer.Shell, null).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public static IBasicPolygon Buffer(IBasicPoint source, IBasicPolygon buffer, PrecisionModel precisionModel)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return new MinkowskiSumAlgorithm(source, buffer.Shell, precisionModel).Result;
        }

        /// <summary>
        ///  Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IBasicPolygon Buffer(IBasicPolygon source, Double radius)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, NumberOfPoints), null).Result;
        }

        /// <summary>
        ///  Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IBasicPolygon Buffer(IBasicPolygon source, Double radius, PrecisionModel precisionModel)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, NumberOfPoints), precisionModel).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="numberOfCoordinates">The number of coordinates of the buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon Buffer(IBasicPolygon source, Double radius, Int32 numberOfCoordinates)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, numberOfCoordinates), null).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="numberOfCoordinates">The number of coordinates of the buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon Buffer(IBasicPolygon source, Double radius, Int32 numberOfCoordinates, PrecisionModel precisionModel)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, numberOfCoordinates), precisionModel).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public static IBasicPolygon Buffer(IBasicPolygon source, IBasicPolygon buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return new MinkowskiSumAlgorithm(source, buffer.Shell, null).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public static IBasicPolygon Buffer(IBasicPolygon source, IBasicPolygon buffer, PrecisionModel precisionModel)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            return new MinkowskiSumAlgorithm(source, buffer.Shell, precisionModel).Result;
        }

        /// <summary>
        ///  Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IBasicPolygon Buffer(IReadOnlyList<Coordinate> source, Double radius)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, NumberOfPoints), null).Result;
        }

        /// <summary>
        ///  Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public static IBasicPolygon Buffer(IReadOnlyList<Coordinate> source, Double radius, PrecisionModel precisionModel)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, NumberOfPoints), precisionModel).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="numberOfCoordinates">The number of coordinates of the buffer.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon Buffer(IReadOnlyList<Coordinate> source, Double radius, Int32 numberOfCoordinates)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, numberOfCoordinates), null).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="radius">The radius of the buffer.</param>
        /// <param name="numberOfCoordinates">The number of coordinates of the buffer.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon Buffer(IReadOnlyList<Coordinate> source, Double radius, Int32 numberOfCoordinates, PrecisionModel precisionModel)
        {
            return new MinkowskiSumAlgorithm(source, CreateCircle(radius, numberOfCoordinates), precisionModel).Result;
        }

        /// <summary>
        ///  Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer polygon.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public static IBasicPolygon Buffer(IReadOnlyList<Coordinate> source, IReadOnlyList<Coordinate> buffer)
        {
            return new MinkowskiSumAlgorithm(source, buffer, null).Result;
        }

        /// <summary>
        ///  Buffers he specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="buffer">The buffer polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The buffered polygon.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The buffer is null.
        /// </exception>
        public static IBasicPolygon Buffer(IReadOnlyList<Coordinate> source, IReadOnlyList<Coordinate> buffer, PrecisionModel precisionModel)
        {
            return new MinkowskiSumAlgorithm(source, buffer, precisionModel).Result;
        }

        /// <summary>
        /// Buffers he specified source.
        /// </summary>
        public void Compute()
        {
            this.resultShellCoordinates = new List<Coordinate>();
            this.resultHoleCoordinates = new List<List<Coordinate>>();

            if (this.sourceShellCoordinates.Count == 1)
            {
                this.ComputeMinkowskiSumOfPoint();
            }
            else
            {
                this.ComputeMinkowskiSumOfCoordinateList(this.sourceShellCoordinates, false);

                if (this.sourceHoleCoordinates != null)
                {
                    for (Int32 holeIndex = 0; holeIndex < this.sourceHoleCoordinates.Count; holeIndex++)
                        this.ComputeMinkowskiSumOfCoordinateList(this.sourceHoleCoordinates[holeIndex], true);
                }
            }

            this.result = new BasicProxyPolygon(this.resultShellCoordinates, this.resultHoleCoordinates);
            this.hasResult = true;
        }

        /// <summary>
        /// Computes the Minkowski sum of a point.
        /// </summary>
        private void ComputeMinkowskiSumOfPoint()
        {
            if (this.sourceShellCoordinates[0] == null)
                return;

            for (Int32 bufferIndex = 0; bufferIndex < this.bufferCoordinates.Count - 1; bufferIndex++)
            {
                this.resultShellCoordinates.Add(this.PrecisionModel.MakePrecise(new Coordinate(this.bufferCoordinates[bufferIndex].X + this.sourceShellCoordinates[0].X, this.bufferCoordinates[bufferIndex].Y + this.sourceShellCoordinates[0].Y)));
            }
        }

        /// <summary>
        /// Computes the Minkowski sum of a polygon.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="isHole">A value indicating whether the source is a hole.</param>
        private void ComputeMinkowskiSumOfCoordinateList(IReadOnlyList<Coordinate> source, Boolean isHole)
        {
            List<Coordinate> resultCoordinates = new List<Coordinate>();
            for (Int32 sourceIndex = 0; sourceIndex < source.Count - 1; sourceIndex++)
            {
                if (source[sourceIndex] == null)
                    continue;

                // find the buffer polygon edges that lie between the corresponding source polygon edges
                List<Tuple<Coordinate, Coordinate>> edges = new List<Tuple<Coordinate, Coordinate>>();
                edges = this.GetConvolutionEdges(source, sourceIndex);

                // sort edges in counterclockwise order and add them to the result distinctly
                edges = SortEdgesInCounterclockwiseOrder(edges);
                for (Int32 edgeIndex = 0; edgeIndex < edges.Count; edgeIndex++)
                    resultCoordinates.Add(this.PrecisionModel.MakePrecise(new Coordinate(edges[edgeIndex].Item1.X, edges[edgeIndex].Item1.Y)));

                if (edges.Count != 0)
                {
                    resultCoordinates.Add(this.PrecisionModel.MakePrecise(new Coordinate(edges[edges.Count - 1].Item2.X, edges[edges.Count - 1].Item2.Y)));
                }
                else if (resultCoordinates.Count >= 2)
                {
                    Int32 count = resultCoordinates.Count - 1;
                    Coordinate coordinate = new Coordinate(source[sourceIndex].X + resultCoordinates[count].X - resultCoordinates[count - 1].X,
                                                           source[sourceIndex].Y + resultCoordinates[count].Y - resultCoordinates[count - 1].Y);
                    resultCoordinates.Add(this.PrecisionModel.MakePrecise(coordinate));
                }
            }

            // compute the resulting polygon
            if (resultCoordinates.Count != 0)
            {
                resultCoordinates.Add(resultCoordinates[0]);
                this.ComputeResultPolygon(resultCoordinates, isHole);
            }
        }

        /// <summary>
        /// Creates the result polygon with separated shell and hole coordinates.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="isHole">A value indicating whether the coordinate list is a hole.</param>
        private void ComputeResultPolygon(IReadOnlyList<Coordinate> coordinates, Boolean isHole)
        {
            BentleyOttmannAlgorithm algorithm = new BentleyOttmannAlgorithm(coordinates, this.PrecisionModel);
            List<Coordinate> intersections = new List<Coordinate>(algorithm.Intersections);
            List<Tuple<Int32, Int32>> edgeIndexes = new List<Tuple<Int32, Int32>>(algorithm.EdgeIndexes);

            // add edge intersection coordinates to the list of coordinates in counterclockwise order
            List<Coordinate> coordinatesWithIntersections = new List<Coordinate>();
            coordinatesWithIntersections = this.GetCoordinatesWithIntersections(coordinates, intersections, edgeIndexes);

            // remove unnecessary internal coordinates and create holes
            List<List<Coordinate>> internalPolygons = new List<List<Coordinate>>();
            List<List<Coordinate>> holes = new List<List<Coordinate>>();
            List<Int32> nonShellIndexes = new List<Int32>();
            Int32 internalPolygonIndex = -1;
            for (Int32 coordinateIndex = 0; coordinateIndex < coordinatesWithIntersections.Count; coordinateIndex++)
            {
                if (internalPolygonIndex != -1)
                {
                    internalPolygons[internalPolygonIndex].Add(coordinatesWithIntersections[coordinateIndex]);
                    nonShellIndexes.Add(coordinateIndex);
                }

                if (intersections.Any(x => x.Equals(coordinatesWithIntersections[coordinateIndex])))
                {
                    Int32 matchingPolygonIndex = internalPolygons.FindIndex(x => x[0].Equals(coordinatesWithIntersections[coordinateIndex]));
                    if (internalPolygonIndex != -1 && internalPolygonIndex < internalPolygons.Count && matchingPolygonIndex != -1)
                    {
                        if (PolygonAlgorithms.Orientation(internalPolygons[matchingPolygonIndex], this.PrecisionModel) == Orientation.Clockwise)
                            holes.Add(internalPolygons[matchingPolygonIndex]);

                        for (Int32 polygonIndex = internalPolygons.Count - 1; polygonIndex >= matchingPolygonIndex; polygonIndex--)
                            internalPolygons.RemoveAt(polygonIndex);

                        internalPolygonIndex = matchingPolygonIndex - 1;
                    }
                    else
                    {
                        internalPolygonIndex++;
                        internalPolygons.Add(new List<Coordinate>() { coordinatesWithIntersections[coordinateIndex] });
                    }
                }
            }

            for (Int32 index = 0; index < nonShellIndexes.Count; index++)
                coordinatesWithIntersections.RemoveAt(nonShellIndexes[nonShellIndexes.Count - 1 - index]);

            // add shell and hole coordinates to the result
            if (isHole)
            {
                this.resultHoleCoordinates.Add(new List<Coordinate>(coordinatesWithIntersections));
            }
            else
            {
                this.resultShellCoordinates = coordinatesWithIntersections;
                foreach (List<Coordinate> hole in holes)
                    this.resultHoleCoordinates.Add(hole);
            }
        }

        /// <summary>
        /// Returns the buffer edges that lie between the corresponding source edges.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="sourceIndex">Index of the source coordinate.</param>
        /// <returns>The list of edges.</returns>
        private List<Tuple<Coordinate, Coordinate>> GetConvolutionEdges(IReadOnlyList<Coordinate> coordinates, Int32 sourceIndex)
        {
            List<Tuple<Coordinate, Coordinate>> edges = new List<Tuple<Coordinate, Coordinate>>();
            Double previousElementX = sourceIndex != 0 ? coordinates[sourceIndex - 1].X : coordinates[coordinates.Count - 2].X;
            Double previousElementY = sourceIndex != 0 ? coordinates[sourceIndex - 1].Y : coordinates[coordinates.Count - 2].Y;

            Double firstAngle = Math.Atan2(coordinates[sourceIndex].Y - previousElementY, coordinates[sourceIndex].X - previousElementX);
            Double secondAngle = Math.Atan2(coordinates[sourceIndex + 1].Y - coordinates[sourceIndex].Y, coordinates[sourceIndex + 1].X - coordinates[sourceIndex].X);
            if (firstAngle < 0 && secondAngle == 0)
                secondAngle = 2 * Math.PI;
            if (firstAngle < 0)
                firstAngle = 2 * Math.PI + firstAngle;
            if (secondAngle < 0)
                secondAngle = 2 * Math.PI + secondAngle;

            for (Int32 bufferIndex = 0; bufferIndex < this.bufferCoordinates.Count - 1; bufferIndex++)
            {
                Double middleAngle = Math.Atan2(this.bufferCoordinates[bufferIndex + 1].Y - this.bufferCoordinates[bufferIndex].Y, this.bufferCoordinates[bufferIndex + 1].X - this.bufferCoordinates[bufferIndex].X);
                if (middleAngle < 0)
                    middleAngle = 2 * Math.PI + middleAngle;

                Boolean addCondition = false;
                if (PolygonAlgorithms.Orientation(coordinates, this.PrecisionModel) == Orientation.Clockwise)
                {
                    if (firstAngle >= secondAngle)
                        addCondition = (middleAngle <= firstAngle) && (middleAngle >= secondAngle);
                    else
                        addCondition = middleAngle <= firstAngle || middleAngle >= secondAngle;
                }
                else
                {
                    if (firstAngle <= secondAngle)
                        addCondition = middleAngle >= firstAngle && middleAngle <= secondAngle;
                    else
                        addCondition = (middleAngle > firstAngle) || (middleAngle < secondAngle);
                }

                if (addCondition)
                {
                    edges.Add(
                        Tuple.Create(
                            this.PrecisionModel.MakePrecise(new Coordinate(coordinates[sourceIndex].X + this.bufferCoordinates[bufferIndex].X, coordinates[sourceIndex].Y + this.bufferCoordinates[bufferIndex].Y)),
                            this.PrecisionModel.MakePrecise(new Coordinate(coordinates[sourceIndex].X + this.bufferCoordinates[bufferIndex + 1].X, coordinates[sourceIndex].Y + this.bufferCoordinates[bufferIndex + 1].Y))));
                }
            }

            return edges;
        }

        /// <summary>
        /// Gets the list of coordinates with intersections in counterclockwise order.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <param name="intersections">The intersections.</param>
        /// <param name="edgeIndexes">The edge indexes.</param>
        /// <returns>
        /// The list of coordinates with intersections.
        /// </returns>
        private List<Coordinate> GetCoordinatesWithIntersections(IReadOnlyList<Coordinate> coordinates, List<Coordinate> intersections, List<Tuple<Int32, Int32>> edgeIndexes)
        {
            List<Coordinate> coordinatesWithIntersections = new List<Coordinate>();

            // add intersection coordinates to the list of coordinates in the right order
            for (Int32 coordinateIndex = 0; coordinateIndex < coordinates.Count - 1; coordinateIndex++)
            {
                List<Coordinate> intersectionCoordinates = new List<Coordinate>();
                coordinatesWithIntersections.Add(coordinates[coordinateIndex]);

                for (Int32 edgeIndex = 0; edgeIndex < edgeIndexes.Count; edgeIndex++)
                {
                    if (edgeIndexes[edgeIndex].Item2.Equals(coordinateIndex))
                        intersectionCoordinates.Add(intersections[edgeIndex]);
                }

                for (Int32 edgeIndex = 0; edgeIndex < edgeIndexes.Count; edgeIndex++)
                {
                    if (edgeIndexes[edgeIndex].Item1.Equals(coordinateIndex))
                        intersectionCoordinates.Add(intersections[edgeIndex]);
                }

                intersectionCoordinates = SortCoordinatesOnALine(intersectionCoordinates, coordinates[coordinateIndex], coordinates[coordinateIndex + 1]);

                for (Int32 index = 0; index < intersectionCoordinates.Count; index++)
                    coordinatesWithIntersections.Add(intersectionCoordinates[index]);
            }

            coordinatesWithIntersections.Add(coordinates[coordinates.Count - 1]);

            // remove duplicate coordinates
            List<Int32> indexesToRemove = new List<Int32>();

            for (Int32 coordinateIndex = 0; coordinateIndex < coordinatesWithIntersections.Count - 1; coordinateIndex++)
            {
                if (coordinatesWithIntersections[coordinateIndex] == coordinatesWithIntersections[coordinateIndex + 1])
                    indexesToRemove.Add(coordinateIndex);
            }

            for (Int32 index = indexesToRemove.Count - 1; index >= 0; index--)
                coordinatesWithIntersections.RemoveAt(indexesToRemove[index]);

            return coordinatesWithIntersections;
        }

        /// <summary>
        /// Sorts the edges in counterclockwise order.
        /// </summary>
        /// <param name="edges">The edges.</param>
        /// <returns>The sorted edges.</returns>
        private static List<Tuple<Coordinate, Coordinate>> SortEdgesInCounterclockwiseOrder(List<Tuple<Coordinate, Coordinate>> edges)
        {
            List<Tuple<Coordinate, Coordinate>> sortedEdges = new List<Tuple<Coordinate, Coordinate>>(edges.Count);
            Int32 startEdgeIndex = edges.FindIndex(edge => !edges.Any(otherEdge => edge.Item1.Equals(otherEdge.Item2)));
            if (startEdgeIndex != -1)
            {
                sortedEdges.Add(edges[startEdgeIndex]);
                for (Int32 edgeIndex = 0; edgeIndex < edges.Count; edgeIndex++)
                {
                    Int32 nextEdgeIndex = edges.FindIndex(x => x.Item1.Equals(sortedEdges[edgeIndex].Item2));
                    if (nextEdgeIndex != -1)
                        sortedEdges.Add(edges[nextEdgeIndex]);
                    else
                        break;
                }
            }

            return sortedEdges;
        }

        /// <summary>
        /// Sorts coordinates laying on a given line.
        /// </summary>
        /// <param name="coordinates">The coordinates to sort.</param>
        /// <param name="lineStart">The line start.</param>
        /// <param name="lineEnd">The line end.</param>
        /// <returns>The sorted coordinates.</returns>
        private static List<Coordinate> SortCoordinatesOnALine(List<Coordinate> coordinates, Coordinate lineStart, Coordinate lineEnd)
        {
            Boolean descendingX = lineStart.X >= lineEnd.X;
            Boolean descendingY = lineStart.Y >= lineEnd.Y;

            if (descendingX && descendingY)
                return coordinates.OrderByDescending(coordinate => coordinate.X).ThenByDescending(coordinate => coordinate.Y).ToList();

            if (descendingX && !descendingY)
                return coordinates.OrderByDescending(coordinate => coordinate.X).ThenBy(coordinate => coordinate.Y).ToList();

            if (!descendingX && descendingY)
                return coordinates.OrderBy(coordinate => coordinate.X).ThenByDescending(coordinate => coordinate.Y).ToList();

            return coordinates.OrderBy(coordinate => coordinate.X).ThenBy(coordinate => coordinate.Y).ToList();
        }

        /// <summary>
        /// Creates a circle as a polygon with the given radius and number of coordinates.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="numberOfCoordinates">The number of coordinates.</param>
        /// <returns>The read-only list of coordinates of the circle.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        private static List<Coordinate> CreateCircle(Double radius, Int32 numberOfCoordinates)
        {
            if (numberOfCoordinates < 3)
                throw new ArgumentOutOfRangeException(nameof(numberOfCoordinates), CoreMessages.NumberOfCoordinatesIsLessThan3);

            List<Coordinate> coordinates = new List<Coordinate>(numberOfCoordinates);
            for (Int32 index = 0; index < numberOfCoordinates; index++)
            {
                Double angle = (Math.PI / 180) * index * (360 / numberOfCoordinates);
                Double x = radius * Math.Cos(angle);
                Double y = radius * Math.Sin(angle);
                coordinates.Add(new Coordinate(x, y));
            }

            return coordinates;
        }
    }
}
