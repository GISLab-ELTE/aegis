// <copyright file="ContinuedFractionApproximator.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Numerics.Approximation
{
    using System;
    using AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a type used for approximating floating point numbers with rational numbers using continued fractions.
    /// </summary>
    /// <remarks>
    /// A continued fraction is an expression obtained through an iterative process of representing a number as the
    /// sum of its integer part and the reciprocal of another number, then writing this other number as the sum of
    /// its integer part and another reciprocal, and so on.
    /// The representation may be infinite (for irrational numbers), thus there is a limitation on the number of
    /// iterations made.
    /// <see cref="http://en.wikipedia.org/wiki/Continued_fraction"/>
    /// </remarks>
    public class ContinuedFractionApproximator
    {
        /// <summary>
        /// The number of iterations performed in the current recursion.
        /// </summary>
        private Int32 iterations;

        /// <summary>
        /// The maximum number of iterations for the current recursion.
        /// </summary>
        private Int32 maxIterations;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuedFractionApproximator"/> class.
        /// </summary>
        /// <param name="iterationLimit">The iteration limit for the approximator.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The iteration limit is less than 1.</exception>
        public ContinuedFractionApproximator(Int32 iterationLimit)
        {
            if (iterationLimit < 1)
                throw new ArgumentOutOfRangeException(nameof(iterationLimit), NumericsMessages.IterationLimitIsLessThan1);

            this.IterationLimit = iterationLimit;
        }

        /// <summary>
        /// Gets the iteration limit for approximation.
        /// </summary>
        /// <value>The maximum number of iterations that can be performed for approximation.</value>
        public Int32 IterationLimit { get; private set; }

        /// <summary>
        /// Returns the nearest rational number to the specified double-precision floating point number.
        /// </summary>
        /// <param name="value">The number to approximate.</param>
        /// <param name="precision">The precision of the approximation.</param>
        /// <returns>The nearest rational number to <paramref name="value"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The value is greater than 4503599627370496 or less than -4503599627370496.
        /// or
        /// The precision is less than or equal to 0.
        /// </exception>
        /// <exception cref="System.ArgumentException">The value cannot be approximated.</exception>
        public Rational GetNearestRational(Double value, Double precision)
        {
            if (value < -1L << 52)
                throw new ArgumentOutOfRangeException(nameof(value), String.Format(NumericsMessages.ValueIsLessThanArg, -1L << 52));
            if (value > 1L << 52)
                throw new ArgumentOutOfRangeException(nameof(value), String.Format(NumericsMessages.ValueIsGreaterThanArg, 1L << 52));
            if (precision <= 0)
                throw new ArgumentOutOfRangeException(nameof(precision), NumericsMessages.PrecisionIsLessThanOrEqualTo0);

            Rational result = Rational.Zero;

            this.maxIterations = 1;
            while (Math.Abs(value - (Double)result.Numerator / result.Denominator) > precision)
            {
                if (this.maxIterations > this.IterationLimit)
                    throw new ArgumentException(NumericsMessages.CannotApproximateValue);

                this.iterations = 0;
                this.maxIterations++;

                result = this.ApproximateRecursive(value);
            }

            return result;
        }

        /// <summary>
        /// Performs approximation recursively.
        /// </summary>
        /// <param name="value">The number to approximate.</param>
        /// <returns>The nearest rational number to <paramref name="value"/> given the number of iterations.</returns>
        private Rational ApproximateRecursive(Double value)
        {
            Int64 integerPart = (Int64)value;
            Double fractionPart = value - integerPart;

            if (this.iterations >= this.maxIterations || fractionPart == 0)
                return new Rational(integerPart);

            this.iterations++;
            return integerPart + new Rational(1, this.ApproximateRecursive(1 / fractionPart));
        }
    }
}
