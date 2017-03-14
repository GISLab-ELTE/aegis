// <copyright file="MatrixComputationsTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics.LinearAlgebra
{
    using System;
    using AEGIS.Numerics;
    using AEGIS.Numerics.LinearAlgebra;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="MatrixComputations" /> class.
    /// </summary>
    [TestFixture]
    public class MatrixComputationsTest
    {
        /// <summary>
        /// The array of matrices.
        /// </summary>
        private Matrix[] matrices;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.matrices = new Matrix[]
            {
                new Matrix(new Double[0, 0]),
                new Matrix(new Double[1, 1]),
                new Matrix(new Double[2, 2]),
                new Matrix(new Double[,] { { 1, 3, 5 }, { 2, 4, 7 }, { 1, 1, 0 } }),
                new Matrix(new Double[,] { { 11, 9, 24, 2 }, { 1, 5, 2, 6 }, { 3, 17, 18, 1 }, { 2, 5, 7, 1 } }),
                new Matrix(new Double[,] { { 1, 2, 18 }, { 7.5, 4, 13.4 } })
            };
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.IsZero(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsIsZeroTest()
        {
            MatrixComputations.IsZero(this.matrices[0]).ShouldBeTrue();
            MatrixComputations.IsZero(this.matrices[1]).ShouldBeTrue();
            MatrixComputations.IsZero(this.matrices[2]).ShouldBeTrue();
            MatrixComputations.IsZero(this.matrices[3]).ShouldBeFalse();
            MatrixComputations.IsZero(this.matrices[4]).ShouldBeFalse();
            MatrixComputations.IsZero(this.matrices[5]).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => MatrixComputations.IsZero(null));
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.IsSymmetric(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsIsSymmetricTest()
        {
            Double[,] testValues = new Double[,] { { 5, 4, 1.3 }, { 4, 8.7, 2 }, { 1.3, 2, 0 } };
            Matrix testMatrix = new Matrix(testValues);

            MatrixComputations.IsSymmetric(this.matrices[0]).ShouldBeTrue();
            MatrixComputations.IsSymmetric(this.matrices[1]).ShouldBeTrue();
            MatrixComputations.IsSymmetric(this.matrices[2]).ShouldBeTrue();
            MatrixComputations.IsSymmetric(testMatrix).ShouldBeTrue();
            MatrixComputations.IsSymmetric(this.matrices[3]).ShouldBeFalse();
            MatrixComputations.IsSymmetric(this.matrices[4]).ShouldBeFalse();
            MatrixComputations.IsSymmetric(this.matrices[5]).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => MatrixComputations.IsSymmetric(null));
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.Determinant(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsComputeDeterminantTest()
        {
            MatrixComputations.Determinant(this.matrices[0]).ShouldBe(0);
            MatrixComputations.Determinant(this.matrices[1]).ShouldBe(0);
            MatrixComputations.Determinant(this.matrices[2]).ShouldBe(0);
            MatrixComputations.Determinant(this.matrices[3]).ShouldBe(4);
            MatrixComputations.Determinant(this.matrices[4]).ShouldBe(284, 0.00001);

            Should.Throw<ArgumentException>(() => MatrixComputations.Determinant(this.matrices[5]));
            Should.Throw<ArgumentNullException>(() => MatrixComputations.Determinant(null));
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.Definiteness(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsComputeDefinitenessTest()
        {
            Matrix matrix = new Matrix(new Double[,] { { 1, -1 }, { -1, 4 } });
            MatrixComputations.Definiteness(matrix).ShouldBe(MatrixDefiniteness.PositiveDefinite);

            matrix = new Matrix(new Double[,] { { 1, 0, 0 }, { 0, 3, 0 }, { 0, 0, 2 } });
            MatrixComputations.Definiteness(matrix).ShouldBe(MatrixDefiniteness.PositiveDefinite);

            matrix = new Matrix(new Double[,] { { 2, -1, 0 }, { -1, 2, -1 }, { 0, -1, 2 } });
            MatrixComputations.Definiteness(matrix).ShouldBe(MatrixDefiniteness.PositiveDefinite);

            matrix = new Matrix(new Double[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 0.5 } });
            MatrixComputations.Definiteness(matrix).ShouldBe(MatrixDefiniteness.Indefinite);

            MatrixComputations.Definiteness(this.matrices[1]).ShouldBe(MatrixDefiniteness.PositiveSemidefinite);
            MatrixComputations.Definiteness(this.matrices[2]).ShouldBe(MatrixDefiniteness.PositiveSemidefinite);

            Should.Throw<ArgumentException>(() => MatrixComputations.Definiteness(this.matrices[0]));
            Should.Throw<ArgumentException>(() => MatrixComputations.Definiteness(this.matrices[3]));
            Should.Throw<ArgumentException>(() => MatrixComputations.Definiteness(this.matrices[4]));
            Should.Throw<ArgumentException>(() => MatrixComputations.Definiteness(this.matrices[5]));
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.Eigenvalues(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsEigenvaluesTest()
        {
            // first matrix
            Matrix matrix = new Matrix(new[,] { { 5, 4, 1.3 }, { 4, 8.7, 2 }, { 1.3, 2, 0 } });
            Double[] expected = new Double[3] { 11.7417, 2.44301, -0.484678 };

            Double[] result = MatrixComputations.Eigenvalues(matrix);

            Array.Sort(expected);
            Array.Sort(result);

            result.Length.ShouldBe(expected.Length);
            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index].ShouldBe(expected[index], 0.001);
            }

            // second matrix
            matrix = new Matrix(new[,] { { 5, 8, 7.6, 2 }, { 8, 42, 0, 1 }, { 7.6, 0, 3, 5 }, { 2, 1, 5, 22.4 } });
            expected = new Double[4] { 43.8386, 24.094, 8.97766, -4.51021 };

            result = MatrixComputations.Eigenvalues(matrix);

            Array.Sort(expected);
            Array.Sort(result);

            result.Length.ShouldBe(expected.Length);
            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index].ShouldBe(expected[index], 0.001);
            }

            // identity matrix
            matrix = MatrixFactory.CreateIdentity(4);
            expected = new Double[4] { 1, 1, 1, 1 };

            result = MatrixComputations.Eigenvalues(matrix);

            Array.Sort(expected);
            Array.Sort(result);

            result.Length.ShouldBe(expected.Length);
            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index].ShouldBe(expected[index], 0.001);
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => MatrixComputations.Eigenvalues(null));
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.Transpose(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsTransposeTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Matrix result = MatrixComputations.Transpose(this.matrices[matrixIndex]);

                for (Int32 rowIndex = 0; rowIndex < this.matrices[matrixIndex].NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < this.matrices[matrixIndex].NumberOfColumns; columnIndex++)
                    {
                        result[columnIndex, rowIndex].ShouldBe(this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.IsUpperTriangular(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsIsUpperTriangleTest()
        {
            Double[][,] values = new Double[][,]
            {
                new Double[,] { { 5, 4, 1.3 }, { 0, 8.7, 2 }, { 0, 0, 0 } },
                new Double[,] { { 5, 8, 7.6, 2 }, { 0, 42, 0, 1 }, { 0, 0, 3, 5 }, { 0, 0, 5, 22.4 } },
                new Double[,] { { 5, 4, 1.3 }, { 0, 8.7, 2 }, { 0, 0, 4 } }
            };

            MatrixComputations.IsUpperTriangular(new Matrix(values[0])).ShouldBeTrue();
            MatrixComputations.IsUpperTriangular(new Matrix(values[2])).ShouldBeTrue();
            MatrixComputations.IsUpperTriangular(this.matrices[0]).ShouldBeTrue();
            MatrixComputations.IsUpperTriangular(this.matrices[1]).ShouldBeTrue();
            MatrixComputations.IsUpperTriangular(this.matrices[2]).ShouldBeTrue();

            MatrixComputations.IsUpperTriangular(new Matrix(values[1])).ShouldBeFalse();
            MatrixComputations.IsUpperTriangular(this.matrices[3]).ShouldBeFalse();
            MatrixComputations.IsUpperTriangular(this.matrices[4]).ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.IsLowerTriangular(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationsIsLowerTriangleTest()
        {
            Double[][,] values = new Double[][,]
            {
                new Double[,] { { 5, 0, 0 }, { 7.6, 8.7, 0 }, { 1.4, 3, 0 } },
                new Double[,] { { 5, 0, 0, 0 }, { 0, 42, 0, 0 }, { 0, 0, 3, 5 }, { 0, 0, 5, 22.4 } },
                new Double[,] { { 5, 0, 0 }, { 0, 8.7, 0 }, { 0, 0, 4 } }
            };

            MatrixComputations.IsLowerTriangular(new Matrix(values[0])).ShouldBeTrue();
            MatrixComputations.IsLowerTriangular(new Matrix(values[2])).ShouldBeTrue();
            MatrixComputations.IsLowerTriangular(this.matrices[0]).ShouldBeTrue();
            MatrixComputations.IsLowerTriangular(this.matrices[1]).ShouldBeTrue();
            MatrixComputations.IsLowerTriangular(this.matrices[2]).ShouldBeTrue();

            MatrixComputations.IsLowerTriangular(new Matrix(values[1])).ShouldBeFalse();
            MatrixComputations.IsLowerTriangular(this.matrices[3]).ShouldBeFalse();
            MatrixComputations.IsLowerTriangular(this.matrices[4]).ShouldBeFalse();
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.Invert(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixComputationInvertTest()
        {
            Matrix result = MatrixComputations.Invert(this.matrices[3]);
            Matrix identity = this.matrices[3] * result;

            for (Int32 rowIndex = 0; rowIndex < identity.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < identity.NumberOfColumns; columnIndex++)
                {
                    if (rowIndex == columnIndex)
                        identity[rowIndex, columnIndex].ShouldBe(1, 0.001);
                    else
                        identity[rowIndex, columnIndex].ShouldBe(0, 0.001);
                }
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => MatrixComputations.Invert(null));
            Should.Throw<ArgumentException>(() => MatrixComputations.Invert(this.matrices[2]));
            Should.Throw<ArgumentException>(() => MatrixComputations.Invert(this.matrices[5]));
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.Transpose(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixTransposeMatrixTest()
        {
            for (Int32 matricesIndex = 0; matricesIndex < this.matrices.Length; matricesIndex++)
            {
                Matrix transponedMatrix = MatrixComputations.Transpose(this.matrices[matricesIndex]);
                transponedMatrix.NumberOfColumns.ShouldBe(this.matrices[matricesIndex].NumberOfRows);
                transponedMatrix.NumberOfRows.ShouldBe(this.matrices[matricesIndex].NumberOfColumns);

                for (Int32 rowIndex = 0; rowIndex < transponedMatrix.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < transponedMatrix.NumberOfColumns; columnIndex++)
                    {
                        transponedMatrix[rowIndex, columnIndex].ShouldBe(this.matrices[matricesIndex][columnIndex, rowIndex]);
                    }
                }
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => MatrixComputations.Transpose((Matrix)null));
        }

        /// <summary>
        /// Tests the <see cref="MatrixComputations.Transpose(Vector)" /> method.
        /// </summary>
        [Test]
        public void MatrixTransposeVectorTest()
        {
            // first vector
            Vector vector = new Vector(0);
            Matrix result = MatrixComputations.Transpose(vector);

            result.NumberOfColumns.ShouldBe(0);
            result.NumberOfRows.ShouldBe(1);

            // second vector
            vector = new Vector(1, 1, 1, 1);
            result = MatrixComputations.Transpose(vector);

            result.NumberOfColumns.ShouldBe(4);
            result.NumberOfRows.ShouldBe(1);
            result.ShouldAllBe(value => value == 1);

            // third vector
            vector = new Vector(3, 8, 54, 32);
            result = MatrixComputations.Transpose(vector);
            result.NumberOfColumns.ShouldBe(4);
            result.NumberOfRows.ShouldBe(1);

            result.NumberOfColumns.ShouldBe(4);
            result.NumberOfRows.ShouldBe(1);
            for (Int32 index = 0; index < vector.Size; index++)
            {
                result[0, index].ShouldBe(vector[index]);
            }

            // forth vector
            vector = new Vector(1.2, 5.8, 7.456, 15);
            result = MatrixComputations.Transpose(vector);
            result.NumberOfColumns.ShouldBe(4);
            result.NumberOfRows.ShouldBe(1);

            result.NumberOfColumns.ShouldBe(4);
            result.NumberOfRows.ShouldBe(1);
            for (Int32 index = 0; index < vector.Size; index++)
            {
                result[0, index].ShouldBe(vector[index]);
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => MatrixComputations.Transpose((Vector)null));
        }
    }
}