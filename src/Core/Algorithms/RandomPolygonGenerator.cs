// <copyright file="RandomPolygonGenerator.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Resources;

    /// <summary>
    /// Represents a type which performs creation of random polygons.
    /// </summary>
    /// <remarks>
    /// The implementation is based on Rod Stephens's polygon generator algorithm, <see cref="http://csharphelper.com/blog/2012/08/generate-random-polygons-in-c/" />.
    /// </remarks>
    public class RandomPolygonGenerator
    {
        /// <summary>
        /// The resulting polygon.
        /// </summary>
        private BasicProxyPolygon result;

        /// <summary>
        /// A value indicating whether the result has been computed.
        /// </summary>
        private Boolean hasResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomPolygonGenerator"/> class.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="envelopeMin">The lower bound of the generated polygon envelope.</param>
        /// <param name="envelopeMax">The upper bound of the generated polygon envelope.</param>
        /// <param name="convexityRatio">The convexity ratio.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of coordinates is less than 3.
        /// or
        /// The envelope's minimum coordinate is greater than the maximum coordinate.
        /// or
        /// The convexity ratio is less than 0.
        /// or
        /// The convexity ratio is greater than 1.
        /// </exception>
        public RandomPolygonGenerator(Int32 coordinateCount, Coordinate envelopeMin, Coordinate envelopeMax, Double convexityRatio, PrecisionModel precisionModel)
        {
            if (coordinateCount < 3)
                throw new ArgumentOutOfRangeException(nameof(coordinateCount), CoreMessages.CoordinateCountLessThan3);
            if (envelopeMin.X > envelopeMax.X || envelopeMin.Y > envelopeMax.Y)
                throw new ArgumentOutOfRangeException(nameof(envelopeMax), CoreMessages.EnvelopeMinIsGreaterThanMax);
            if (convexityRatio < 0.0)
                throw new ArgumentOutOfRangeException(nameof(convexityRatio), CoreMessages.ConvexityRatioLessThan0);
            if (convexityRatio > 1.0)
                throw new ArgumentOutOfRangeException(nameof(convexityRatio), CoreMessages.ConvexityRatioGreaterThan1);

            this.PrecisionModel = precisionModel ?? PrecisionModel.Default;
            this.CoordinateCount = coordinateCount;
            this.EnvelopeMinimum = envelopeMin;
            this.EnvelopeMaximum = envelopeMax;
            this.ConvexityRatio = convexityRatio;
            this.result = null;
            this.hasResult = false;
        }

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets the number of coordinates.
        /// </summary>
        /// <value>The number of coordinates.</value>
        public Int32 CoordinateCount { get; private set; }

        /// <summary>
        /// Gets the minimum coordinate of the envelope.
        /// </summary>
        /// <value>The minimum coordinate of the envelope.</value>
        public Coordinate EnvelopeMinimum { get; private set; }

        /// <summary>
        /// Gets the maximum coordinate of the envelope.
        /// </summary>
        /// <value>The maximum coordinate of the envelope.</value>
        public Coordinate EnvelopeMaximum { get; private set; }

        /// <summary>
        /// Gets the convexity ratio.
        /// </summary>
        /// <value>The convexity ratio.</value>
        public Double ConvexityRatio { get; private set; }

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The generated random polygon.</value>
        public IBasicPolygon Result
        {
            get
            {
                if (!this.hasResult)
                    this.Compute();
                return this.result;
            }
        }

        /// <summary>
        /// Computes the result of the algorithm.
        /// </summary>
        public void Compute()
        {
            Coordinate[] shell = new Coordinate[this.CoordinateCount + 1];

            Random rand = new Random();

            // random points
            Double[] values = new Double[this.CoordinateCount];
            Double minRadius = this.ConvexityRatio;
            Double maxRadius = 1.0;

            for (Int32 coordIndex = 0; coordIndex < this.CoordinateCount; coordIndex++)
            {
                values[coordIndex] = minRadius + rand.NextDouble() * (maxRadius - minRadius);
            }

            // random angle weights
            Double[] angleWeights = new Double[this.CoordinateCount];
            Double totalWeight = 0;

            for (Int32 i = 0; i < this.CoordinateCount; i++)
            {
                angleWeights[i] = rand.NextDouble();
                totalWeight += angleWeights[i];
            }

            // convert weights into radians
            Double[] angles = new Double[this.CoordinateCount];
            for (Int32 coordIndex = 0; coordIndex < this.CoordinateCount; coordIndex++)
            {
                angles[coordIndex] = angleWeights[coordIndex] * 2 * Math.PI / totalWeight;
            }

            // moving points according to angles
            Double halfWidth = (this.EnvelopeMaximum.X - this.EnvelopeMinimum.X) / 2;
            Double halfHeight = (this.EnvelopeMaximum.Y - this.EnvelopeMinimum.Y) / 2;
            Double midPointX = this.EnvelopeMinimum.X + halfWidth;
            Double midPointY = this.EnvelopeMinimum.Y + halfHeight;
            Double theta = 0;

            for (Int32 coordIndex = 0; coordIndex < this.CoordinateCount; coordIndex++)
            {
                shell[coordIndex] = this.PrecisionModel.MakePrecise(new Coordinate(midPointX + (halfWidth * values[coordIndex] * Math.Cos(theta)), midPointY + (halfHeight * values[coordIndex] * Math.Sin(theta))));
                theta += angles[coordIndex];
            }

            shell[this.CoordinateCount] = shell[0];

            this.result = new BasicProxyPolygon(shell);
            this.hasResult = true;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount)
        {
            return new RandomPolygonGenerator(coordinateCount, new Coordinate(Double.MinValue, Double.MinValue), new Coordinate(Double.MaxValue, Double.MaxValue), 0.1, null).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, PrecisionModel precisionModel)
        {
            return new RandomPolygonGenerator(coordinateCount, new Coordinate(Double.MinValue, Double.MinValue), new Coordinate(Double.MaxValue, Double.MaxValue), 0.1, precisionModel).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="envelope">The bounding envelope of the generated polygon.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Envelope envelope)
        {
            return new RandomPolygonGenerator(coordinateCount, envelope.Minimum, envelope.Maximum, 0.1, null).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="envelope">The bounding envelope of the generated polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The number of coordinates is less than 3.</exception>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Envelope envelope, PrecisionModel precisionModel)
        {
            return new RandomPolygonGenerator(coordinateCount, envelope.Minimum, envelope.Maximum, 0.1, precisionModel).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="envelope">The bounding envelope of the generated polygon.</param>
        /// <param name="convexityRatio">The convexity ratio.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of coordinates is less than 3.
        /// or
        /// The convexity ratio is less than 0.
        /// or
        /// The convexity ratio is greater than 1.
        /// </exception>
        /// <remarks>
        /// Statistics for <paramref name="convexityRatio" /> with respect to chance of convex polygon: 0.1 => 1%, 0.2 => 3%, 0.3 => 6%, 0.4 => 10%, 0.5 => 15%, 0.6 => 25%, 0.7 => 39%, 0.8 => 53%, 0.9 => 72%, 1.0 => 100%.
        /// </remarks>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Envelope envelope, Double convexityRatio)
        {
            return new RandomPolygonGenerator(coordinateCount, envelope.Minimum, envelope.Maximum, convexityRatio, null).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="envelope">The bounding envelope of the generated polygon.</param>
        /// <param name="convexityRatio">The convexity ratio.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of coordinates is less than 3.
        /// or
        /// The convexity ratio is less than 0.
        /// or
        /// The convexity ratio is greater than 1.
        /// </exception>
        /// <remarks>
        /// Statistics for <paramref name="convexityRatio" /> with respect to chance of convex polygon: 0.1 => 1%, 0.2 => 3%, 0.3 => 6%, 0.4 => 10%, 0.5 => 15%, 0.6 => 25%, 0.7 => 39%, 0.8 => 53%, 0.9 => 72%, 1.0 => 100%.
        /// </remarks>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Envelope envelope, Double convexityRatio, PrecisionModel precisionModel)
        {
            return new RandomPolygonGenerator(coordinateCount, envelope.Minimum, envelope.Maximum, convexityRatio, precisionModel).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon vertexes.</param>
        /// <param name="envelopeMin">The lower bound of the generated polygon envelope.</param>
        /// <param name="envelopeMax">The upper bound of the generated polygon envelope.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of coordinates is less than 3.
        /// or
        /// The envelope's minimum coordinate is greater than the maximum coordinate.
        /// </exception>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Coordinate envelopeMin, Coordinate envelopeMax)
        {
            return new RandomPolygonGenerator(coordinateCount, envelopeMin, envelopeMax, 0.1, null).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon vertexes.</param>
        /// <param name="envelopeMin">The lower bound of the generated polygon envelope.</param>
        /// <param name="envelopeMax">The upper bound of the generated polygon envelope.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of coordinates is less than 3.
        /// or
        /// The envelope's minimum coordinate is greater than the maximum coordinate.
        /// </exception>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Coordinate envelopeMin, Coordinate envelopeMax, PrecisionModel precisionModel)
        {
            return new RandomPolygonGenerator(coordinateCount, envelopeMin, envelopeMax, 0.1, precisionModel).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="envelopeMin">The lower bound of the generated polygon envelope.</param>
        /// <param name="envelopeMax">The upper bound of the generated polygon envelope.</param>
        /// <param name="convexityRatio">The convexity ratio.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of coordinates is less than 3.
        /// or
        /// The envelope's minimum coordinate is greater than the maximum coordinate.
        /// or
        /// The convexity ratio is less than 0.
        /// or
        /// The convexity ratio is greater than 1.
        /// </exception>
        /// <remarks>
        /// Statistics for <paramref name="convexityRatio" /> with respect to chance of convex polygon: 0.1 => 1%, 0.2 => 3%, 0.3 => 6%, 0.4 => 10%, 0.5 => 15%, 0.6 => 25%, 0.7 => 39%, 0.8 => 53%, 0.9 => 72%, 1.0 => 100%.
        /// </remarks>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Coordinate envelopeMin, Coordinate envelopeMax, Double convexityRatio)
        {
            return new RandomPolygonGenerator(coordinateCount, envelopeMin, envelopeMax, convexityRatio, null).Result;
        }

        /// <summary>
        /// Generates a random polygon.
        /// </summary>
        /// <param name="coordinateCount">The number of the polygon coordinates.</param>
        /// <param name="envelopeMin">The lower bound of the generated polygon envelope.</param>
        /// <param name="envelopeMax">The upper bound of the generated polygon envelope.</param>
        /// <param name="convexityRatio">The convexity ratio.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns>The generated polygon.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of coordinates is less than 3.
        /// or
        /// The envelope's minimum coordinate is greater than the maximum coordinate.
        /// or
        /// The convexity ratio is less than 0.
        /// or
        /// The convexity ratio is greater than 1.
        /// </exception>
        /// <remarks>
        /// Statistics for <paramref name="convexityRatio" /> with respect to chance of convex polygon: 0.1 => 1%, 0.2 => 3%, 0.3 => 6%, 0.4 => 10%, 0.5 => 15%, 0.6 => 25%, 0.7 => 39%, 0.8 => 53%, 0.9 => 72%, 1.0 => 100%.
        /// </remarks>
        public static IBasicPolygon CreateRandomPolygon(Int32 coordinateCount, Coordinate envelopeMin, Coordinate envelopeMax, Double convexityRatio, PrecisionModel precisionModel)
        {
            return new RandomPolygonGenerator(coordinateCount, envelopeMin, envelopeMax, convexityRatio, null).Result;
        }
    }
}
