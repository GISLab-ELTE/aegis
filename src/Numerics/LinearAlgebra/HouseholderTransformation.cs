// <copyright file="HouseholderTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using ELTE.AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a type performing the Householder Transformation of <see cref="Vector" /> instances.
    /// </summary>
    public class HouseholderTransformation
    {
        /// <summary>
        /// The original vector.
        /// </summary>
        private Double[] vector;

        /// <summary>
        /// The Householder transformed matrix.
        /// </summary>
        private Matrix h;

        /// <summary>
        /// Initializes a new instance of the <see cref="HouseholderTransformation" /> class.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public HouseholderTransformation(Double[] vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), NumericsMessages.VectorIsNull);

            this.vector = vector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HouseholderTransformation" /> class.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public HouseholderTransformation(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), NumericsMessages.VectorIsNull);

            this.vector = vector.ToArray();
        }

        /// <summary>
        /// Gets the Householder transform.
        /// </summary>
        /// <value>The Householder transform matrix.</value>
        public Matrix H
        {
            get
            {
                if (this.h == null)
                    this.Compute();
                return this.h;
            }
        }

        /// <summary>
        /// Transforms the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Householder matrix of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Matrix Transform(Vector vector)
        {
            HouseholderTransformation transformation = new HouseholderTransformation(vector);
            transformation.Compute();
            return transformation.H;
        }

        /// <summary>
        /// Transforms the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The Householder matrix of the <paramref name="vector" />.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Matrix Transform(Double[] vector)
        {
            HouseholderTransformation transformation = new HouseholderTransformation(vector);
            transformation.Compute();
            return transformation.H;
        }

        /// <summary>
        /// Tridiagonalizes the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The tridiagonalization of the <paramref name="matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The matrix is not square.
        /// or
        /// The matrix is not symmetric.
        /// </exception>
        public static Matrix Tridiagonalize(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);
            if (!matrix.IsSquare)
                throw new ArgumentException(NumericsMessages.MatrixIsNotSquare, nameof(matrix));
            if (!MatrixComputations.IsSymmetric(matrix))
                throw new ArgumentException(NumericsMessages.MatrixIsNotSymmetric, nameof(matrix));

            Matrix tridiagonalMatrix = new Matrix(matrix);

            for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns - 2; columnIndex++)
            {
                Double[] column = tridiagonalMatrix.GetColumn(columnIndex);

                Double sum = 0;
                for (Int32 index = columnIndex + 1; index < column.Length; index++)
                    sum += column[index] * column[index];

                Double alpha = -Math.Sign(column[columnIndex + 1]) * Math.Sqrt(sum);
                Double r = Math.Sqrt(0.5 * (alpha * alpha - column[columnIndex + 1] * alpha));

                Vector v = new Vector(column.Length);
                v[columnIndex + 1] = (column[columnIndex + 1] - alpha) / 2 / r;
                for (Int32 j = columnIndex + 2; j < column.Length; j++)
                    v[j] = column[j] / 2 / r;

                Matrix p = MatrixFactory.CreateIdentity(column.Length) - 2 * v * v.Transpose();

                tridiagonalMatrix = p * tridiagonalMatrix * p;
            }

            return tridiagonalMatrix;
        }

        /// <summary>
        /// Perform computation.
        /// </summary>
        public void Compute()
        {
            Vector u = new Vector(this.vector);
            if (u[0] != 0)
                u[0] -= Math.Sign(this.vector[0]) * Norm.ComputePNorm(this.vector, 2);
            else
                u[0] -= Norm.ComputePNorm(this.vector, 2);

            Vector v;
            if (Norm.ComputePNorm(u, 2) != 0)
                v = u / Norm.ComputePNorm(u, 2);
            else
                v = u;
            Matrix vTranspone = v.Transpose();

            this.h = MatrixFactory.CreateIdentity(this.vector.Length) - 2 / (vTranspone * v)[0] * (v * vTranspone);
        }
    }
}
