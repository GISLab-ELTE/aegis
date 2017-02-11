// <copyright file="CholeskyDecomposition.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    using System;
    using ELTE.AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a type for Cholesky decomposition of <see cref="Matrix" /> instances.
    /// </summary>
    /// <remarks>
    /// The Cholesky decomposition is a decomposition of a Hermitian, positive-definite matrix into the product of a lower triangular matrix (L) and its conjugate transpose (LT).
    /// </remarks>
    public class CholeskyDecomposition
    {
        /// <summary>
        /// The original matrix.
        /// </summary>
        private readonly Matrix matrix;

        /// <summary>
        /// The generated L matrix.
        /// </summary>
        private Matrix l;

        /// <summary>
        /// The generated transposed L matrix.
        /// </summary>
        private Matrix transposedL;

        /// <summary>
        /// Initializes a new instance of the <see cref="CholeskyDecomposition" /> class.
        /// </summary>
        /// <param name="matrix">The matrix of which the decomposition is computed.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not symmetric.</exception>
        public CholeskyDecomposition(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);
            if (!MatrixComputations.IsSymmetric(matrix))
                throw new ArgumentException(NumericsMessages.MatrixIsNotSymmetric, nameof(matrix));

            this.matrix = matrix;
        }

        /// <summary>
        /// Gets the L (lower triangular) matrix.
        /// </summary>
        public Matrix L
        {
            get
            {
                if (this.l == null)
                    this.Compute();
                return this.l;
            }
        }

        /// <summary>
        /// Gets the transposed L (lower triangular) matrix.
        /// </summary>
        public Matrix LT
        {
            get
            {
                if (this.transposedL == null)
                    this.Compute();
                return this.transposedL;
            }
        }

        /// <summary>
        /// Gets the LLt matrix.
        /// </summary>
        public Matrix LLT
        {
            get
            {
                if (this.l == null || this.transposedL == null)
                    this.Compute();
                return this.l * this.transposedL;
            }
        }

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            Compute(this.matrix, out this.l, out this.transposedL);
        }

        /// <summary>
        /// Computes the Cholesky decomposition.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="l">The L matrix.</param>
        /// <param name="lt">The transposed L matrix.</param>
        private static void Compute(Matrix matrix, out Matrix l, out Matrix lt)
        {
            l = new Matrix(matrix.NumberOfRows, matrix.NumberOfRows);
            Double sum;

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex <= rowIndex; columnIndex++)
                {
                    sum = 0;

                    for (Int32 remainderIndex = 0; remainderIndex < columnIndex; remainderIndex++)
                    {
                        sum += l[rowIndex, remainderIndex] * l[columnIndex, remainderIndex];
                    }

                    if (rowIndex == columnIndex)
                    {
                        l[rowIndex, rowIndex] = Math.Sqrt(matrix[rowIndex, rowIndex] - sum);
                    }
                    else
                    {
                        l[rowIndex, columnIndex] = (1 / l[columnIndex, columnIndex]) * (matrix[rowIndex, columnIndex] - sum);
                    }
                }
            }

            lt = l.Transpose();
        }
    }
}
