// <copyright file="MersenneTwisterRandomGenerator.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics.Randomizers
{
    using System;
    using ELTE.AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents the Mersenne Twister pseudorandom number generator.
    /// </summary>
    /// <remarks>
    /// The Mersenne Twister is by far the most widely used general-purpose pseudorandom number generator. Its name derives from the fact that its period length is chosen to be a Mersenne prime.
    /// This implementation of the Mersenne Twister algorithm is based on the Mersenne prime 2^19937−1, called MT19937.
    /// </remarks>
    public class MersenneTwisterRandomGenerator : Random
    {
        /// <summary>
        /// The word size (in number of bits).
        /// </summary>
        private const Int16 W = 32;

        /// <summary>
        /// Degree of recurrence.
        /// </summary>
        private const Int16 N = 624;

        /// <summary>
        /// The middle word, an offset used in the recurrence relation defining the series x.
        /// </summary>
        private const Int16 M = 397;

        /// <summary>
        /// The separation point of one word, or the number of bits of the lower bitmask.
        /// </summary>
        private const Int16 R = 31;

        /// <summary>
        /// Coefficients of the rational normal form twist matrix.
        /// </summary>
        private const Int64 A = 0x9908B0DF16;

        /// <summary>
        /// Tempering bit shift 1.
        /// </summary>
        private const Int16 S = 7;

        /// <summary>
        /// Tempering bitmask 1.
        /// </summary>
        private const Int64 B = 0x9D2C5680;

        /// <summary>
        /// Tempering bit shift 2.
        /// </summary>
        private const Int16 T = 15;

        /// <summary>
        /// Tempering bitmask 2.
        /// </summary>
        private const Int64 C = 0xEFC60000;

        /// <summary>
        /// Tempering bit shift 3.
        /// </summary>
        private const Int16 U = 11;

        /// <summary>
        /// Tempering bitmask 3.
        /// </summary>
        private const Int64 D = 0xFFFFFFFF;

        /// <summary>
        /// Tempering bit shift 4.
        /// </summary>
        private const Int16 L = 18;

        /// <summary>
        /// Another parameter to the generator, though not part of the algorithm proper.
        /// </summary>
        private const Int32 F = 1812433253;

        /// <summary>
        /// Lower mask.
        /// </summary>
        private const Int32 LowerMask = unchecked((1 << R) - 1);

        /// <summary>
        /// Upper mask.
        /// </summary>
        private const Int32 UpperMask = ~LowerMask;

        /// <summary>
        /// Array to store the state of the generator.
        /// </summary>
        private readonly Int32[] mT;

        /// <summary>
        /// The index of the generated numbers.
        /// </summary>
        private Int16 index;

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwisterRandomGenerator" /> class.
        /// </summary>
        public MersenneTwisterRandomGenerator()
            : this(Environment.TickCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MersenneTwisterRandomGenerator" /> class.
        /// </summary>
        /// <param name="seed">The seed of the generator.</param>
        public MersenneTwisterRandomGenerator(Int32 seed)
        {
            this.mT = new Int32[N];
            this.index = N;
            this.mT[0] = Math.Abs(seed);
            for (UInt32 i = 1; i < N; ++i)
            {
                this.mT[i] = (Int32)(F * (this.mT[i - 1] ^ (this.mT[i - 1] >> (W - 2))) + i);
            }
        }

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="Int32.MaxValue" />.</returns>
        public override Int32 Next()
        {
            if (this.index == N)
                this.Twist();

            Int32 y = this.mT[this.index];
            y = (Int32)(y ^ ((y >> U) & D));
            y = (Int32)(y ^ ((y << S) & B));
            y = (Int32)(y ^ ((y << T) & C));
            y = y ^ (y >> L);

            ++this.index;

            return y;
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue" />; that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue" />. However, if <paramref name="maxValue" /> equals 0, <paramref name="maxValue" /> is returned.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The maximum value is less than 0.</exception>
        public override Int32 Next(Int32 maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException(nameof(maxValue), NumericsMessages.MaxValueLessThan0);

            if (maxValue == 0)
                return 0;

            return (Int32)(this.Next() / ((Int64)Int32.MaxValue + 1) * maxValue);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minValue" /> and less than <paramref name="maxValue" />; that is, the range of return values includes <paramref name="minValue" /> but not <paramref name="maxValue" />. If <paramref name="minValue" /> equals <paramref name="maxValue" />, <paramref name="minValue" /> is returned.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The minimum value is greater than the maximum value.</exception>
        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException(nameof(minValue), NumericsMessages.MinValueGreaterThanMaxValue);

            if (maxValue == minValue)
                return minValue;

            return (Int32)(this.Next() / ((Int64)Int32.MaxValue + 1) * ((Int64)maxValue - minValue) + minValue);
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="System.ArgumentNullException">The buffer is null.</exception>
        public override void NextBytes(Byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), NumericsMessages.BufferIsNull);

            for (Int32 i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (Byte)this.Next(0, 256);
            }
        }

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        public override Double NextDouble()
        {
            return (Double)this.Next() / Int32.MaxValue;
        }

        /// <summary>
        /// Generates the next n values.
        /// </summary>
        private void Twist()
        {
            for (Int32 i = 0; i < N; ++i)
            {
                Int32 x = (this.mT[i] & UpperMask) + (this.mT[(i + 1) % N] & LowerMask);
                Int32 xA = x >> 1;
                if ((x % 2) != 0)
                    xA = (Int32)(xA ^ A);
                this.mT[i] = this.mT[(i + M) % N] ^ xA;
            }

            this.index = 0;
        }
    }
}
