// <copyright file="MatrixTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Numerics
{
    using System;
    using ELTE.AEGIS.Numerics;
    using ELTE.AEGIS.Numerics.LinearAlgebra;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Matrix" /> class.
    /// </summary>
    [TestFixture]
    public class MatrixTest
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
                new Matrix(new[,] { { 1.0, 3, 5 }, { 2, 4, 7 }, { 1, 1, 0 } }),
                new Matrix(new[,] { { 11.0, 9, 24, 2 }, { 1, 5, 2, 6 }, { 3, 17, 18, 1 }, { 2, 5, 7, 1 } })
            };
        }

        /// <summary>
        /// Tests properties of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixPropertiesTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                this.matrices[matrixIndex].NumberOfColumns.ShouldBe(matrixIndex);
                this.matrices[matrixIndex].NumberOfRows.ShouldBe(matrixIndex);
            }

            this.matrices[0].Trace.ShouldBe(0);
            this.matrices[1].Trace.ShouldBe(0);
            this.matrices[2].Trace.ShouldBe(0);
            this.matrices[3].Trace.ShouldBe(5);
            this.matrices[4].Trace.ShouldBe(35);

            new Matrix(2, 3).Trace.ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Matrix.GetColumn(Int32)" /> method.
        /// </summary>
        [Test]
        public void MatrixGetColumnTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < this.matrices[matrixIndex].NumberOfColumns; columnIndex++)
                {
                    Double[] column = this.matrices[matrixIndex].GetColumn(columnIndex);

                    for (Int32 rowIndex = 0; rowIndex < this.matrices[matrixIndex].NumberOfRows; rowIndex++)
                    {
                        column[rowIndex].ShouldBe(this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }

            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[0].GetColumn(0));
            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[0].GetColumn(-1));
        }

        /// <summary>
        /// Tests the <see cref="Matrix.GetRow(Int32)" /> method.
        /// </summary>
        [Test]
        public void MatrixGetRowTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                for (Int32 rowIndex = 0; rowIndex < this.matrices[matrixIndex].NumberOfRows; rowIndex++)
                {
                    Double[] row = this.matrices[matrixIndex].GetRow(rowIndex);

                    for (Int32 columnIndex = 0; columnIndex < this.matrices[matrixIndex].NumberOfColumns; columnIndex++)
                    {
                        row[columnIndex].ShouldBe(this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }

            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[0].GetRow(0));
            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[0].GetRow(-1));
        }

        /// <summary>
        /// Tests the <see cref="Matrix.AreEqual(Matrix, Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixAreEqualTest()
        {
            Matrix.AreEqual(null, null).ShouldBeTrue();

            for (Int32 firstIndex = 0; firstIndex < this.matrices.Length; firstIndex++)
            {
                for (Int32 secondIndex = 0; secondIndex < this.matrices.Length; secondIndex++)
                {
                    Matrix.AreEqual(this.matrices[firstIndex], null).ShouldBeFalse();
                    Matrix.AreEqual(this.matrices[firstIndex], new Matrix(this.matrices[firstIndex])).ShouldBeTrue();

                    if (firstIndex == secondIndex)
                        Matrix.AreEqual(this.matrices[firstIndex], this.matrices[secondIndex]).ShouldBeTrue();
                    else
                        Matrix.AreEqual(this.matrices[firstIndex], this.matrices[secondIndex]).ShouldBeFalse();
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="Matrix.IsValid(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixIsValidTest()
        {
            for (Int32 index = 0; index < this.matrices.Length; index++)
            {
                Matrix.IsValid(this.matrices[index]).ShouldBeTrue();
            }

            Matrix.IsValid(new Matrix(new[,] { { 1, Double.NaN }, { 0, 1 } })).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => Matrix.IsValid(null));
        }

        /// <summary>
        /// Tests the negation operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixUnaryNegationTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Matrix result = -this.matrices[matrixIndex];

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        result[rowIndex, columnIndex].ShouldBe(-this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the addition operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixAdditionTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Matrix result = this.matrices[matrixIndex] + this.matrices[matrixIndex];

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        result[rowIndex, columnIndex].ShouldBe(2 * this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the subtraction operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixSubtractionTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Matrix result = this.matrices[matrixIndex] - this.matrices[matrixIndex];
                result.ShouldAllBe(value => value == 0);

                result = new Matrix(this.matrices[matrixIndex].NumberOfRows, this.matrices[matrixIndex].NumberOfColumns) - this.matrices[matrixIndex];

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        result[rowIndex, columnIndex].ShouldBe(-this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the multiplication operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixMultiplicationTest()
        {
            // multiply with scalar
            Double scalar = Math.PI;
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Matrix product = scalar * this.matrices[matrixIndex];

                for (Int32 rowIndex = 0; rowIndex < product.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < product.NumberOfColumns; columnIndex++)
                    {
                        product[rowIndex, columnIndex].ShouldBe(scalar * this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }

                product = this.matrices[matrixIndex] * scalar;

                for (Int32 rowIndex = 0; rowIndex < product.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < product.NumberOfColumns; columnIndex++)
                    {
                        product[rowIndex, columnIndex].ShouldBe(scalar * this.matrices[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }

            // multiply with matrix
            Matrix[] expectedMultiple = new Matrix[]
            {
                MatrixFactory.CreateSquare(),
                MatrixFactory.CreateSquare(0),
                MatrixFactory.CreateSquare(0, 0, 0, 0),
                MatrixFactory.CreateSquare(12, 20, 26,
                                           17, 29, 38,
                                           3, 7, 12),
                MatrixFactory.CreateSquare(206, 562, 728, 102,
                                           34, 98, 112, 40,
                                           106, 423, 437, 127,
                                           50, 167, 191, 42)
            };

            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Matrix mulMatrix = this.matrices[matrixIndex] * this.matrices[matrixIndex];

                for (Int32 rowIndex = 0; rowIndex < mulMatrix.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < mulMatrix.NumberOfColumns; columnIndex++)
                    {
                        mulMatrix[rowIndex, columnIndex].ShouldBe(expectedMultiple[matrixIndex][rowIndex, columnIndex]);
                    }
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="Vector" /> cast operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixCastToVectorTest()
        {
            // first matrix
            Matrix matrix = new Matrix(new Double[,] { { 1, 5, 8, 4 } });
            Vector expected = new Vector(new Double[] { 1, 5, 8, 4 });
            Vector result = (Vector)matrix;

            result.Size.ShouldBe(4);
            for (Int32 index = 0; index < result.Size; index++)
            {
                result[index].ShouldBe(expected[index]);
            }

            // second matrix
            matrix = new Matrix(new Double[,] { { 1 }, { 5 }, { 7 }, { 3 } });
            expected = new Vector(new Double[] { 1, 5, 7, 3 });
            result = (Vector)matrix;

            result.Size.ShouldBe(4);
            for (Int32 index = 0; index < result.Size; index++)
            {
                result[index].ShouldBe(expected[index]);
            }

            Should.Throw<ArgumentNullException>(() => result = (Vector)((Matrix)null));
            Should.Throw<ArgumentException>(() => result = (Vector)this.matrices[0]);
            Should.Throw<ArgumentException>(() => result = (Vector)this.matrices[2]);
        }
    }
}
