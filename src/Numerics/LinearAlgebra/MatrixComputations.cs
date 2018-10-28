// <copyright file="MatrixComputations.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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

namespace AEGIS.Numerics.LinearAlgebra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Numerics.Resources;

    /// <summary>
    /// Defines additional computation methods for <see cref="Matrix" /> instances.
    /// </summary>
    public static class MatrixComputations
    {
        /// <summary>
        /// Determines whether the specified matrix is a zero matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values of the matrix are zeros; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsZero(this Matrix matrix)
        {
            return Matrix.IsZero(matrix);
        }

        /// <summary>
        /// Determines whether the specified matrix is valid.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values of the matrix are numbers; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsValid(this Matrix matrix)
        {
            return Matrix.IsValid(matrix);
        }

        /// <summary>
        /// Determines whether the matrix is symmetric.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if the matrix is square and symmetric; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsSymmetric(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            if (!matrix.IsSquare)
                return false;

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    if (matrix[rowIndex, columnIndex] != matrix[columnIndex, rowIndex])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the matrix is upper triangular.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values in the lower triangle are zero; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsUpperTriangular(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            if (matrix.NumberOfColumns != matrix.NumberOfRows)
                return false;

            for (Int32 rowIndex = 1; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < rowIndex; columnIndex++)
                {
                    if (matrix[rowIndex, columnIndex] != 0)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the matrix is lower triangular.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values in the upper triangle are zero; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsLowerTriangular(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            if (matrix.NumberOfColumns != matrix.NumberOfRows)
                return false;

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows - 1; rowIndex++)
            {
                for (Int32 columnIndex = rowIndex + 1; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    if (matrix[rowIndex, columnIndex] != 0)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the matrix is triangular.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if the matrix is either lower or upper triangular; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsTriangular(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            return MatrixComputations.IsUpperTriangular(matrix) || MatrixComputations.IsLowerTriangular(matrix);
        }

        /// <summary>
        /// Determines whether the matrix is diagonal.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if the matrix is both lower and upper triangular; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsDiagonal(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            return MatrixComputations.IsUpperTriangular(matrix) && MatrixComputations.IsLowerTriangular(matrix);
        }

        /// <summary>
        /// Computes the determinant of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The determinant of the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static Double Determinant(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (matrix.NumberOfRows != matrix.NumberOfColumns)
                throw new ArgumentException(NumericsMessages.MatrixIsNotSquare, nameof(matrix));

            if (matrix.NumberOfRows == 0)
                return 0;

            if (matrix.NumberOfRows == 1)
                return matrix[0, 0];

            if (matrix.NumberOfRows == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            return LUDecomposition.ComputeDeterminant(matrix);
        }

        /// <summary>
        /// Computes the definiteness of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The definiteness of the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The matrix is empty.
        /// or
        /// The matrix is not symmetric.
        /// </exception>
        public static MatrixDefiniteness Definiteness(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            if (matrix.NumberOfRows == 0 && matrix.NumberOfColumns == 0)
                throw new ArgumentException(NumericsMessages.MatrixIsEmpty, nameof(matrix));

            if (!IsSymmetric(matrix))
                throw new ArgumentException(NumericsMessages.MatrixIsNotSymmetric, nameof(matrix));

            if (matrix.All(value => value == 0))
                return MatrixDefiniteness.PositiveSemidefinite;

            Double[] eigenvalues = QRAlgorithm.ComputeEigenvalues(matrix);
            Int32 posValues = 0;
            Int32 negValues = 0;
            Int32 zeroValues = 0;
            for (Int32 index = 0; index < eigenvalues.Length; ++index)
            {
                if (eigenvalues[index] > 0)
                    posValues++;
                else if (eigenvalues[index] < 0)
                    negValues++;
                else
                    zeroValues++;
            }

            if (posValues > 0 && negValues == 0 && zeroValues == 0)
                return MatrixDefiniteness.PositiveDefinite;
            else if (negValues > 0 && posValues == 0 && zeroValues == 0)
                return MatrixDefiniteness.NegativeDefinite;
            else if (posValues > 0 && zeroValues > 0 && negValues == 0)
                return MatrixDefiniteness.PositiveSemidefinite;
            else if (negValues > 0 && zeroValues > 0 && posValues == 0)
                return MatrixDefiniteness.NegativeSemiDefinite;
            else
                return MatrixDefiniteness.Indefinite;
        }

        /// <summary>
        /// Computes the eigenvalues of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The eigenvalues of the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Double[] Eigenvalues(this Matrix matrix)
        {
            return QRAlgorithm.ComputeEigenvalues(matrix);
        }

        /// <summary>
        /// Computes the eigenvectors of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The eigenvectors of the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Vector[] Eigenvectors(this Matrix matrix)
        {
            return QRAlgorithm.ComputeEigenvectors(matrix);
        }

        /// <summary>
        /// Inverts the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The inverse matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not invertible.</exception>
        public static Matrix Invert(this Matrix matrix)
        {
            return LUDecomposition.Invert(matrix);
        }

        /// <summary>
        /// Transposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The transposed matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix Transpose(this Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            Matrix result = new Matrix(matrix.NumberOfColumns, matrix.NumberOfRows);

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    result[columnIndex, rowIndex] = matrix[rowIndex, columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Transposes the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The transposed vector as a matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Matrix Transpose(this Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            Matrix matrix = new Matrix(1, vector.Size);
            for (Int32 index = 0; index < vector.Size; index++)
            {
                matrix[0, index] = vector[index];
            }

            return matrix;
        }
    }
}
