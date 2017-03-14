// <copyright file="LUDecomposition.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Numerics.LinearAlgebra
{
    using System;
    using System.Linq;
    using AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a type performing the LU decomposition of <see cref="Matrix" /> instances.
    /// </summary>
    public class LUDecomposition
    {
        /// <summary>
        /// The original matrix.
        /// </summary>
        private Matrix matrix;

        /// <summary>
        /// The determinant of the matrix.
        /// </summary>
        private Double? determinant;

        /// <summary>
        /// The permutation array for generating the pivot matrix.
        /// </summary>
        private Int32[] permutationArray;

        /// <summary>
        /// The number of permutations for computing the determinant.
        /// </summary>
        private Int32 numberOfPermutations;

        /// <summary>
        /// The generation pivot permutation matrix.
        /// </summary>
        private Matrix p;

        /// <summary>
        /// The generated L matrix.
        /// </summary>
        private Matrix l;

        /// <summary>
        /// The generated U matrix.
        /// </summary>
        private Matrix u;

        /// <summary>
        /// Initializes a new instance of the <see cref="LUDecomposition" /> class.
        /// </summary>
        /// <param name="matrix">The matrix of which the decomposition is computed.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public LUDecomposition(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            this.matrix = matrix;
            this.determinant = null;
        }

        /// <summary>
        /// Gets the L (lower triangular) matrix.
        /// </summary>
        /// <value>The L matrix.</value>
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
        /// Gets the U (upper triangular) matrix.
        /// </summary>
        /// <value>The U matrix.</value>
        public Matrix U
        {
            get
            {
                if (this.u == null)
                    this.Compute();
                return this.u;
            }
        }

        /// <summary>
        /// Gets the LU (the lower and upper triangles integrated) matrix.
        /// </summary>
        /// <value>The LU matrix.</value>
        public Matrix LU
        {
            get
            {
                if (this.l == null || this.u == null)
                    this.Compute();
                return this.l * this.u;
            }
        }

        /// <summary>
        /// Gets the P (pivot permutation) matrix.
        /// </summary>
        /// <value>The P matrix.</value>
        public Matrix P
        {
            get
            {
                if (this.p == null)
                    this.ComputePivotPermutationMatrix();

                return this.p;
            }
        }

        /// <summary>
        /// Gets the determinant of the matrix.
        /// </summary>
        /// <value>The determinant of the matrix.</value>
        public Double Determinant
        {
            get
            {
                if (this.matrix.NumberOfRows != this.matrix.NumberOfColumns)
                    return Double.NaN;

                if (this.determinant == null)
                    this.ComputeDeterminant();

                return this.determinant.Value;
            }
        }

        /// <summary>
        /// Decomposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The decomposed (LU) matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static Matrix Decompose(Matrix matrix)
        {
            LUDecomposition decomposition = new LUDecomposition(matrix);
            decomposition.Compute();
            return decomposition.LU;
        }

        /// <summary>
        /// Decomposes the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix to decompose.</param>
        /// <param name="l">The L (lower triangular) matrix.</param>
        /// <param name="u">The U (upper triangular) matrix.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static void Decompose(Matrix matrix, out Matrix l, out Matrix u)
        {
            LUDecomposition decomposition = new LUDecomposition(matrix);
            decomposition.Compute();
            l = decomposition.L;
            u = decomposition.U;
        }

        /// <summary>
        /// Computes the determinant of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The determinant of the specified matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not square.</exception>
        public static Double ComputeDeterminant(Matrix matrix)
        {
            LUDecomposition decomposition = new LUDecomposition(matrix);
            decomposition.Compute();
            return decomposition.Determinant;
        }

        /// <summary>
        /// Inverts the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The inverted matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is not invertible.</exception>
        public static Matrix Invert(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            if (matrix.All(value => value == 0))
                throw new ArgumentException(NumericsMessages.MatrixIsNotInvertible, nameof(matrix));

            LUDecomposition decomposition = new LUDecomposition(matrix);
            decomposition.Compute();

            if (decomposition.Determinant == 0)
                throw new ArgumentException(NumericsMessages.MatrixIsNotInvertible, nameof(matrix));

            Matrix inverse = new Matrix(matrix.NumberOfRows, matrix.NumberOfColumns);

            for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
            {
                Vector b = VectorFactory.CreateUnitVector(matrix.NumberOfRows, columnIndex);
                Vector y = SolveEquation(decomposition, b);

                for (Int32 rowIndex = 0; rowIndex < inverse.NumberOfRows; ++rowIndex)
                {
                    inverse[rowIndex, columnIndex] = y[rowIndex];
                }
            }

            return inverse;
        }

        /// <summary>
        /// Solves a linear equation system.
        /// </summary>
        /// <param name="a">The left side of the equation represented by a matrix.</param>
        /// <param name="b">The right side of the equation represented by a vector.</param>
        /// <returns>The vector containing the unknown variables of the equation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The matrix is null.
        /// or
        /// The vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The matrix is not square.
        /// or
        /// The size of the matrix does not match the size of the vector.
        /// </exception>
        public static Vector SolveEquation(Matrix a, Vector b)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), NumericsMessages.MatrixIsNull);
            if (b == null)
                throw new ArgumentNullException(nameof(b), NumericsMessages.VectorIsNull);
            if (!a.IsSquare)
                throw new ArgumentException(NumericsMessages.MatrixIsNotSquare, nameof(a));
            if (a.NumberOfRows != b.Size)
                throw new ArgumentException(NumericsMessages.MatrixSizeDoesNotMatchVector, nameof(b));

            LUDecomposition decomposition = new LUDecomposition(a);
            decomposition.Compute();
            return SolveEquation(decomposition, b);
        }

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            Matrix pivotMatrix = this.ComputePivot();

            this.l = MatrixFactory.CreateIdentity(this.matrix.NumberOfRows, Math.Min(this.matrix.NumberOfRows, this.matrix.NumberOfColumns));
            this.u = new Matrix(Math.Min(this.matrix.NumberOfRows, this.matrix.NumberOfColumns), this.matrix.NumberOfColumns);

            for (Int32 rowIndex = 0; rowIndex < this.matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < this.matrix.NumberOfColumns; columnIndex++)
                {
                    Double sum;
                    if (columnIndex <= rowIndex)
                    {
                        sum = 0;
                        for (Int32 index = 0; index < columnIndex; index++)
                        {
                            sum += this.l[columnIndex, index] * this.u[index, rowIndex];
                        }

                        this.u[columnIndex, rowIndex] = pivotMatrix[columnIndex, rowIndex] - sum;
                    }

                    if (columnIndex >= rowIndex)
                    {
                        sum = 0;
                        for (Int32 index = 0; index < rowIndex; index++)
                        {
                            sum += this.l[columnIndex, index] * this.u[index, rowIndex];
                        }

                        if (this.u[rowIndex, rowIndex] == 0)
                            this.l[columnIndex, rowIndex] = pivotMatrix[columnIndex, rowIndex] - sum;
                        else
                            this.l[columnIndex, rowIndex] = (pivotMatrix[columnIndex, rowIndex] - sum) / this.u[rowIndex, rowIndex];
                    }
                }
            }
        }

        /// <summary>
        /// Solves a linear equation system.
        /// </summary>
        /// <param name="decomposition">The LU decomposition.</param>
        /// <param name="b">The vector.</param>
        /// <returns>The vector containing the unknown variables of the equation.</returns>
        private static Vector SolveEquation(LUDecomposition decomposition, Vector b)
        {
            // P*b
            Vector pb = decomposition.P * b;

            // L*y = P*b with forward substitution
            Vector y = new Vector(pb.Size);
            for (Int32 rowIndex = 0; rowIndex < decomposition.L.NumberOfRows; ++rowIndex)
            {
                y[rowIndex] = pb[rowIndex];
                for (Int32 columnIndex = 0; columnIndex < rowIndex; ++columnIndex)
                {
                    y[rowIndex] -= decomposition.L[rowIndex, columnIndex] * y[columnIndex];
                }

                y[rowIndex] /= decomposition.L[rowIndex, rowIndex] == 0 ? 1 : decomposition.L[rowIndex, rowIndex];
            }

            // U*x = y with back substitution
            Vector x = new Vector(y.Size);
            for (Int32 rowIndex = x.Size - 1; rowIndex >= 0; --rowIndex)
            {
                x[rowIndex] = y[rowIndex];
                for (Int32 columnIndex = rowIndex + 1; columnIndex < decomposition.U.NumberOfRows; ++columnIndex)
                {
                    x[rowIndex] -= decomposition.U[rowIndex, columnIndex] * x[columnIndex];
                }

                x[rowIndex] /= decomposition.U[rowIndex, rowIndex] == 0 ? 1 : decomposition.U[rowIndex, rowIndex];
            }

            return x;
        }

        /// <summary>
        /// Computes the pivot matrix.
        /// </summary>
        /// <returns>The pivot matrix.</returns>
        private Matrix ComputePivot()
        {
            Matrix pivotMatrix = MatrixFactory.CreateIdentity(this.matrix.NumberOfRows, this.matrix.NumberOfColumns);

            this.ComputePivotPermutationArray();

            for (Int32 rowIndex = 0; rowIndex < this.matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < this.matrix.NumberOfColumns; columnIndex++)
                    pivotMatrix[rowIndex, columnIndex] = this.matrix[this.permutationArray[rowIndex], columnIndex];
            }

            return pivotMatrix;
        }

        /// <summary>
        /// Computes the pivot permutation array for the matrix.
        /// </summary>
        private void ComputePivotPermutationArray()
        {
            if (this.permutationArray != null)
                return;

            // first we assume that all rows are in current order
            this.permutationArray = Enumerable.Range(0, this.matrix.NumberOfRows).ToArray();
            this.numberOfPermutations = 0;

            for (Int32 columnIndex = 0; columnIndex < this.matrix.NumberOfColumns; columnIndex++)
            {
                Int32 maxJ = columnIndex; // max search in the (partial) row
                for (Int32 rowIndex = columnIndex; rowIndex < this.matrix.NumberOfRows; rowIndex++)
                {
                    if (Math.Abs(this.matrix[rowIndex, columnIndex]) > Math.Abs(this.matrix[maxJ, columnIndex]))
                        maxJ = rowIndex;
                }

                // if the row is not in the correct order
                if (maxJ != columnIndex)
                {
                    this.numberOfPermutations++;
                    Int32 temp = this.permutationArray[columnIndex];
                    this.permutationArray[columnIndex] = this.permutationArray[maxJ];
                    this.permutationArray[maxJ] = temp;
                }
            }
        }

        /// <summary>
        /// Computes the pivot permutation matrix.
        /// </summary>
        private void ComputePivotPermutationMatrix()
        {
            if (this.p != null)
                return;

            this.ComputePivotPermutationArray();

            this.p = new Matrix(this.permutationArray.Length, this.permutationArray.Length);

            for (Int32 rowIndex = 0; rowIndex < this.matrix.NumberOfRows; rowIndex++)
                this.p[rowIndex, this.permutationArray[rowIndex]] = 1;
        }

        /// <summary>
        /// Computes the determinant of the matrix.
        /// </summary>
        private void ComputeDeterminant()
        {
            if (this.determinant != null)
                return;

            this.determinant = (this.numberOfPermutations % 2 == 0) ? 1 : (-1);
            for (Int32 rowIndex = 0; rowIndex < this.matrix.NumberOfRows; rowIndex++)
                this.determinant *= this.u[rowIndex, rowIndex];
        }
    }
}
