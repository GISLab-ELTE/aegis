// <copyright file="QRDecomposition.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a type for QR decomposition of <see cref="Matrix" /> instances.
    /// </summary>
    /// <remarks>
    /// A QR decomposition of a matrix is a decomposition of a matrix A into a product A = QR of an orthogonal matrix Q and an upper triangular matrix R.
    /// QR decomposition is often used to solve the linear least squares problem, and is the basis for a particular eigenvalue algorithm, the QR algorithm.
    /// This implementation uses the Householder reflection transformation, and also computed the Householder vectors in matrix form (H).
    /// </remarks>
    public class QRDecomposition
    {
        /// <summary>
        /// The source matrix.
        /// </summary>
        private Matrix source;

        /// <summary>
        /// The decomposition matrix.
        /// </summary>
        private Double[][] decomposition;

        /// <summary>
        /// The diagonal of the R matrix.
        /// </summary>
        private Double[] rDiagonal;

        /// <summary>
        /// The generated H matrix.
        /// </summary>
        private Matrix h;

        /// <summary>
        /// The generated Q matrix.
        /// </summary>
        private Matrix q;

        /// <summary>
        /// The generated QR matrix.
        /// </summary>
        private Matrix qr;

        /// <summary>
        /// The generated R matrix.
        /// </summary>
        private Matrix r;

        /// <summary>
        /// Initializes a new instance of the <see cref="QRDecomposition" /> class.
        /// </summary>
        /// <param name="matrix">The matrix of which the decomposition is computed.</param>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public QRDecomposition(Matrix matrix)
        {
            this.source = matrix ?? throw new ArgumentNullException(nameof(matrix));
        }

        /// <summary>
        /// Gets the H (Householder) matrix.
        /// </summary>
        /// <value>The H matrix.</value>
        public Matrix H
        {
            get
            {
                if (this.h != null)
                    return this.h;

                if (this.decomposition == null)
                    this.Compute();

                return this.ComputeH();
            }
        }

        /// <summary>
        /// Gets the Q (orthogonal) matrix.
        /// </summary>
        /// <value>The Q matrix.</value>
        public Matrix Q
        {
            get
            {
                if (this.q != null)
                    return this.q;

                if (this.decomposition == null)
                    this.Compute();

                return this.ComputeQ();
            }
        }

        /// <summary>
        /// Gets the R (upper triangular) matrix.
        /// </summary>
        /// <value>The R matrix.</value>
        public Matrix R
        {
            get
            {
                if (this.r != null)
                    return this.r;

                if (this.decomposition == null)
                    this.Compute();

                return this.ComputeR();
            }
        }

        /// <summary>
        /// Gets the QR matrix.
        /// </summary>
        /// <value>The QR matrix.</value>
        public Matrix QR
        {
            get
            {
                if (this.qr != null)
                    return this.qr;

                if (this.decomposition == null)
                    this.Compute();

                this.qr = MatrixFactory.Create(this.decomposition);

                return this.qr;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the source matrix is full rank.
        /// </summary>
        /// <value><c>true</c> if this source matrix is full rank; otherwise, <c>false</c>.</value>
        public Boolean IsFullRank
        {
            get
            {
                foreach (Double value in this.rDiagonal)
                {
                    if (value == 0)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            this.Initialize();

            this.ComputeIteration();
        }

        /// <summary>
        /// Perform computation.
        /// </summary>
        /// <param name="numberOfIterations">The number of iterations.</param>
        public void Compute(Int32 numberOfIterations)
        {
            this.Initialize();

            for (Int32 iteration = 0; iteration < numberOfIterations; iteration++)
            {
                this.ComputeIteration();
            }
        }

        /// <summary>
        /// Initializes the computation.
        /// </summary>
        private void Initialize()
        {
            this.decomposition = new Double[this.source.NumberOfRows][];
            for (Int32 rowIndex = 0; rowIndex < this.decomposition.Length; rowIndex++)
            {
                this.decomposition[rowIndex] = new Double[this.source.NumberOfColumns];
                for (Int32 columnIndex = 0; columnIndex < this.decomposition[rowIndex].Length; columnIndex++)
                {
                    this.decomposition[rowIndex][columnIndex] = this.source[rowIndex, columnIndex];
                }
            }

            this.rDiagonal = new Double[this.source.NumberOfColumns];
        }

        /// <summary>
        /// Computes one iteration of the QR decomposition.
        /// </summary>
        private void ComputeIteration()
        {
            for (Int32 columnIndex = 0; columnIndex < this.source.NumberOfColumns; columnIndex++)
            {
                Double norm = 0;
                for (Int32 rowIndex = columnIndex; rowIndex < this.source.NumberOfRows; rowIndex++)
                {
                    norm = Calculator.Hypot(norm, this.decomposition[rowIndex][columnIndex]);
                }

                if (norm == 0)
                    continue;

                if (this.decomposition[columnIndex][columnIndex] < 0)
                {
                    norm = -norm;
                }

                for (Int32 rowIndex = columnIndex; rowIndex < this.source.NumberOfRows; rowIndex++)
                {
                    this.decomposition[rowIndex][columnIndex] /= norm;
                }

                this.decomposition[columnIndex][columnIndex] += 1;

                for (Int32 firstIndex = columnIndex + 1; firstIndex < this.source.NumberOfColumns; firstIndex++)
                {
                    Double sum = 0;
                    for (Int32 secondIndex = columnIndex; secondIndex < this.source.NumberOfRows; secondIndex++)
                    {
                        sum += this.decomposition[secondIndex][columnIndex] * this.decomposition[secondIndex][firstIndex];
                    }

                    sum = -sum / this.decomposition[columnIndex][columnIndex];
                    for (Int32 secondIndex = columnIndex; secondIndex < this.source.NumberOfRows; secondIndex++)
                    {
                        this.decomposition[secondIndex][firstIndex] += sum * this.decomposition[secondIndex][columnIndex];
                    }
                }

                this.rDiagonal[columnIndex] = -norm;
            }
        }

        /// <summary>
        /// Computes the H matrix.
        /// </summary>
        /// <returns>The H matrix.</returns>
        private Matrix ComputeH()
        {
            this.h = new Matrix(this.source.NumberOfRows, this.source.NumberOfColumns);
            for (Int32 rowIndex = 0; rowIndex < this.source.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex <= rowIndex; columnIndex++)
                {
                    this.h[rowIndex, columnIndex] = this.decomposition[rowIndex][columnIndex];
                }
            }

            return this.h;
        }

        /// <summary>
        /// Computes the Q matrix.
        /// </summary>
        /// <returns>The Q matrix.</returns>
        private Matrix ComputeQ()
        {
            this.q = new Matrix(this.source.NumberOfRows, this.source.NumberOfColumns);
            for (Int32 columnIndex = this.q.NumberOfColumns - 1; columnIndex >= 0; columnIndex--)
            {
                this.q[columnIndex, columnIndex] = 1;

                for (Int32 secondColumnIndex = columnIndex; secondColumnIndex < this.q.NumberOfColumns; secondColumnIndex++)
                {
                    if (this.decomposition[columnIndex][columnIndex] == 0)
                        continue;

                    Double sum = 0;
                    for (Int32 rowIndex = columnIndex; rowIndex < this.source.NumberOfRows; rowIndex++)
                    {
                        sum += this.decomposition[rowIndex][columnIndex] * this.q[rowIndex, secondColumnIndex];
                    }

                    sum = -sum / this.decomposition[columnIndex][columnIndex];

                    for (Int32 rowIndex = columnIndex; rowIndex < this.source.NumberOfRows; rowIndex++)
                    {
                        this.q[rowIndex, secondColumnIndex] += sum * this.decomposition[rowIndex][columnIndex];
                    }
                }
            }

            return this.q;
        }

        /// <summary>
        /// Computes the R matrix.
        /// </summary>
        /// <returns>The R matrix.</returns>
        private Matrix ComputeR()
        {
            this.r = new Matrix(this.source.NumberOfColumns, this.source.NumberOfColumns);

            for (Int32 rowIndex = 0; rowIndex < this.r.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = rowIndex; columnIndex < this.r.NumberOfColumns; columnIndex++)
                {
                    if (rowIndex == columnIndex)
                    {
                        this.r[rowIndex, columnIndex] = this.rDiagonal[rowIndex];
                    }
                    else
                    {
                        this.r[rowIndex, columnIndex] = this.decomposition[rowIndex][columnIndex];
                    }
                }
            }

            return this.r;
        }
    }
}
