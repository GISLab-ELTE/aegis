// <copyright file="Norm.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    using System;
    using ELTE.AEGIS.Numerics.Resources;

    /// <summary>
    /// Defines methods for computing norms of vectors and matrices.
    /// </summary>
    public static class Norm
    {
        /// <summary>
        /// Computes the p-norm of the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="p">The power of the norm.</param>
        /// <returns>The p-norm of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value of p is less than 1.</exception>
        public static Double ComputePNorm(Double[] vector, Int32 p)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), NumericsMessages.VectorIsNull);
            if (p < 1)
                throw new ArgumentOutOfRangeException(nameof(p), NumericsMessages.PIsLessThan1);

            Double sum = 0;
            for (Int32 valueIndex = 0; valueIndex < vector.Length; valueIndex++)
                sum += Math.Pow(Math.Abs(vector[valueIndex]), p);
            return Math.Pow(sum, 1.0 / p);
        }

        /// <summary>
        /// Computes the p-norm of the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="p">The power of the norm.</param>
        /// <returns>The p-norm of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value of p is less than 1.</exception>
        public static Double ComputePNorm(this Vector vector, Int32 p)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), NumericsMessages.VectorIsNull);
            if (p < 1)
                throw new ArgumentOutOfRangeException(nameof(p), NumericsMessages.PIsLessThan1);

            Double sum = 0;
            for (Int32 valueIndex = 0; valueIndex < vector.Size; valueIndex++)
                sum += Math.Pow(Math.Abs(vector[valueIndex]), p);
            return Math.Pow(sum, 1.0 / p);
        }

        /// <summary>
        /// Computes the p-norm of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="p">The power of the norm.</param>
        /// <returns>The p-norm of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value of p is less than 1.</exception>
        public static Double ComputePNorm(this Matrix matrix, Int32 p)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.VectorIsNull);
            if (p < 1)
                throw new ArgumentOutOfRangeException(nameof(p), NumericsMessages.PIsLessThan1);

            Double sum = 0;
            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                    sum += Math.Pow(Math.Abs(matrix[rowIndex, columnIndex]), p);
            }

            return Math.Pow(sum, 1.0 / p);
        }
    }
}
