// <copyright file="QRAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a type for performing a QR algorithm on <see cref="Matrix" /> instances.
    /// </summary>
    /// <remarks>
    /// The QR algorithm is an eigenvalue computation algorithm based on the QR decomposition (<see cref="QRDecomposition" />). The basic idea is to perform a QR decomposition, writing the matrix as a product of an orthogonal matrix and an upper triangular matrix, multiply the factors in the reverse order, and iterate.
    /// This implementation follows the basic algorithm without any performance improvements.
    /// </remarks>
    public class QRAlgorithm
    {
        /// <summary>
        /// The limit of value change in order for the iteration to stop. This field is constant.
        /// </summary>
        private const Double ConvergenceLimit = 0.00001;

        /// <summary>
        /// The source matrix.
        /// </summary>
        private Matrix source;

        /// <summary>
        /// The array of eigenvalues.
        /// </summary>
        private Double[] eigenvalues;

        /// <summary>
        /// The array of eigenvectors.
        /// </summary>
        private Vector[] eigenvectors;

        /// <summary>
        /// Initializes a new instance of the <see cref="QRAlgorithm" /> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <exception cref="ArgumentNullException">The matrix is null.</exception>
        public QRAlgorithm(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            this.source = matrix;
        }

        /// <summary>
        /// Gets the eigenvalues.
        /// </summary>
        /// <value>The collection containing the eigenvalues.</value>
        public Double[] Eigenvalues
        {
            get
            {
                if (this.eigenvalues == null)
                    this.Compute();

                return this.eigenvalues;
            }
        }

        /// <summary>
        /// Gets the eigenvectors.
        /// </summary>
        /// <value>The collection containing the eigenvectors.</value>
        public Vector[] Eigenvectors
        {
            get
            {
                if (this.eigenvectors == null)
                    this.Compute();

                return this.eigenvectors;
            }
        }

        /// <summary>
        /// Computes the eigenvalues of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The collection containing the eigenvalues of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Double[] ComputeEigenvalues(Matrix matrix)
        {
            QRAlgorithm algorithm = new QRAlgorithm(matrix);
            algorithm.Compute();

            return algorithm.Eigenvalues;
        }

        /// <summary>
        /// Computes the eigenvalues of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="numberOfIterations">The number of iterations.</param>
        /// <returns>The collection containing the eigenvalues of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The number of iterations is less than 1.</exception>
        public static Double[] ComputeEigenvalues(Matrix matrix, Int32 numberOfIterations)
        {
            QRAlgorithm algorithm = new QRAlgorithm(matrix);
            algorithm.Compute(numberOfIterations);

            return algorithm.Eigenvalues;
        }

        /// <summary>
        /// Computes the eigenvectors of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The collection containing the eigenvectors of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Vector[] ComputeEigenvectors(Matrix matrix)
        {
            QRAlgorithm algorithm = new QRAlgorithm(matrix);
            algorithm.Compute();

            return algorithm.Eigenvectors;
        }

        /// <summary>
        /// Computes the eigenvectors of the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="numberOfIterations">The number of iterations.</param>
        /// <returns>The collection containing the eigenvectors of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The number of iterations is less than 1.</exception>
        public static Vector[] ComputeEigenvectors(Matrix matrix, Int32 numberOfIterations)
        {
            QRAlgorithm algorithm = new QRAlgorithm(matrix);
            algorithm.Compute(numberOfIterations);

            return algorithm.Eigenvectors;
        }

        /// <summary>
        /// Computes the result of the algorithm.
        /// </summary>
        public void Compute()
        {
            if (this.source.NumberOfRows == 0 || this.source.NumberOfColumns == 0)
            {
                this.eigenvalues = new Double[0];
                this.eigenvectors = new Vector[0];
                return;
            }

            Matrix resultMatrix = this.source;
            Matrix resultQ = MatrixFactory.CreateIdentity(resultMatrix.NumberOfRows);
            Double value;

            Int32 minimumIterations = (Int32)Math.Pow(resultMatrix.NumberOfRows, 3);
            Int32 iterationNumber = 0;

            QRDecomposition decomposition;

            do
            {
                value = resultMatrix[0, 0];

                decomposition = new QRDecomposition(resultMatrix);
                decomposition.Compute();

                resultMatrix = decomposition.R * decomposition.Q;
                resultQ = resultQ * decomposition.Q;

                iterationNumber++;
            }
            while (iterationNumber < minimumIterations || Math.Abs(value - resultMatrix[0, 0]) > ConvergenceLimit);

            this.eigenvalues = new Double[resultMatrix.NumberOfColumns];
            for (Int32 columnIndex = 0; columnIndex < resultMatrix.NumberOfColumns; columnIndex++)
                this.eigenvalues[columnIndex] = resultMatrix[columnIndex, columnIndex];

            this.eigenvectors = new Vector[resultMatrix.NumberOfColumns];
            for (Int32 columnIndex = 0; columnIndex < resultMatrix.NumberOfColumns; columnIndex++)
                this.eigenvectors[columnIndex] = new Vector(resultQ.GetColumn(columnIndex));

            Int32 count = this.eigenvalues.Length;
            do
            {
                Int32 nextCount = 0;
                for (Int32 swapIndex = 0; swapIndex < count - 1; swapIndex++)
                {
                    if (this.eigenvalues[swapIndex] < this.eigenvalues[swapIndex + 1])
                    {
                        Swap(ref this.eigenvalues[swapIndex], ref this.eigenvalues[swapIndex + 1]);
                        Swap(ref this.eigenvectors[swapIndex], ref this.eigenvectors[swapIndex + 1]);
                        nextCount = swapIndex;
                    }
                }

                count = nextCount;
            }
            while (count > 0);
        }

        /// <summary>
        /// Computes the result of the algorithm.
        /// </summary>
        /// <param name="numberOfIterations">The number of iterations.</param>
        /// <exception cref="ArgumentOutOfRangeException">The number of iterations is less than 1.</exception>
        public void Compute(Int32 numberOfIterations)
        {
            if (numberOfIterations < 1)
                throw new ArgumentOutOfRangeException(nameof(numberOfIterations), NumericsMessages.NumberOfIterationsIsLessThan1);

            if (this.source.NumberOfRows == 0 || this.source.NumberOfColumns == 0)
            {
                this.eigenvalues = new Double[0];
                this.eigenvectors = new Vector[0];
                return;
            }

            Matrix resultMatrix = this.source;
            Matrix resultQ = MatrixFactory.CreateIdentity(resultMatrix.NumberOfRows);

            QRDecomposition decomposition = null;

            for (Int32 iterationNumber = 0; iterationNumber < numberOfIterations; iterationNumber++)
            {
                decomposition = new QRDecomposition(resultMatrix);
                decomposition.Compute();

                resultMatrix = decomposition.R * decomposition.Q;
                resultQ = resultQ * decomposition.Q;
            }

            this.eigenvalues = new Double[resultMatrix.NumberOfRows];
            for (Int32 rowIndex = 0; rowIndex < resultMatrix.NumberOfRows; rowIndex++)
                this.eigenvalues[rowIndex] = resultMatrix[rowIndex, rowIndex];

            this.eigenvectors = new Vector[resultMatrix.NumberOfColumns];
            for (Int32 columnIndex = 0; columnIndex < resultMatrix.NumberOfColumns; columnIndex++)
                this.eigenvectors[columnIndex] = new Vector(resultQ.GetColumn(columnIndex));
        }

        /// <summary>
        /// Swaps the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="first">The first item.</param>
        /// <param name="second">The second item.</param>
        private static void Swap<T>(ref T first, ref T second)
        {
            T temp = first;
            first = second;
            second = temp;
        }
    }
}
