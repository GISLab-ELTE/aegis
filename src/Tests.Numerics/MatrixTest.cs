// <copyright file="MatrixTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using AEGIS.Numerics;
    using AEGIS.Numerics.LinearAlgebra;
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
                new Matrix(new[,] { { 0.5, 0 }, { 0, -0.5 } }),
                new Matrix(new[,] { { 1.0, 3, 5 }, { 2, 4, 7 }, { 1, 1, 0 } }),
                new Matrix(new[,] { { 11.0, 9, 24, 2 }, { 1, 5, 2, 6 }, { 3, 17, 18, 1 }, { 2, 5, 7, 1 } }),
                new Matrix(new[,] { { 1, 3 }, { -1, 0.7 }, { 12, 0 } }),
                new Matrix(new[,] { { 1, 3, 7 }, { 1.6, -12, 5 } })
            };
        }

        /// <summary>
        /// Tests constructors of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixConstructorTest()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => new Matrix(-1, 1));
            Should.Throw<ArgumentOutOfRangeException>(() => new Matrix(1, -1));
            Should.Throw<ArgumentNullException>(() => new Matrix((Double[,])null));
            Should.Throw<ArgumentNullException>(() => new Matrix((Matrix)null));
        }

        /// <summary>
        /// Tests item properties of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixItemTest()
        {
            // getters
            this.matrices[3][0, 0].ShouldBe(1.0);
            this.matrices[3][0, 1].ShouldBe(3);
            this.matrices[3][1, 0].ShouldBe(2);
            this.matrices[6][0, 0].ShouldBe(1);
            this.matrices[6][0, 1].ShouldBe(3);
            this.matrices[6][1, 0].ShouldBe(1.6);

            Double value = 0;
            Should.Throw<ArgumentOutOfRangeException>(() => value = this.matrices[3][-1, 0]);
            Should.Throw<ArgumentOutOfRangeException>(() => value = this.matrices[3][0, -1]);
            Should.Throw<ArgumentOutOfRangeException>(() => value = this.matrices[3][3, 0]);
            Should.Throw<ArgumentOutOfRangeException>(() => value = this.matrices[3][0, 3]);

            // setters
            this.matrices[3][0, 0] = Math.PI;
            this.matrices[3][0, 0].ShouldBe(Math.PI);
            this.matrices[3][0, 1] = 2 * Math.PI;
            this.matrices[3][0, 1].ShouldBe(2 * Math.PI);
            this.matrices[3][1, 0] = 3 * Math.PI;
            this.matrices[3][1, 0].ShouldBe(3 * Math.PI);

            this.matrices[6][0, 0] = Math.PI;
            this.matrices[6][0, 0].ShouldBe(Math.PI);
            this.matrices[6][0, 1] = 2 * Math.PI;
            this.matrices[6][0, 1].ShouldBe(2 * Math.PI);
            this.matrices[6][1, 0] = 3 * Math.PI;
            this.matrices[6][1, 0].ShouldBe(3 * Math.PI);

            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[3][-1, 0] = value);
            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[3][0, -1] = value);
            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[3][3, 0] = value);
            Should.Throw<ArgumentOutOfRangeException>(() => this.matrices[3][0, 3] = value);
        }

        /// <summary>
        /// Tests properties of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixPropertiesTest()
        {
            for (Int32 i = 0; i < 5; i++)
            {
                this.matrices[i].NumberOfColumns.ShouldBe(i);
                this.matrices[i].NumberOfRows.ShouldBe(i);
            }

            this.matrices[5].NumberOfColumns.ShouldBe(2);
            this.matrices[5].NumberOfRows.ShouldBe(3);
            this.matrices[6].NumberOfColumns.ShouldBe(3);
            this.matrices[6].NumberOfRows.ShouldBe(2);

            this.matrices[0].Trace.ShouldBe(0);
            this.matrices[1].Trace.ShouldBe(0);
            this.matrices[2].Trace.ShouldBe(0);
            this.matrices[3].Trace.ShouldBe(5);
            this.matrices[4].Trace.ShouldBe(35);

            this.matrices[5].Trace.ShouldBe(Double.NaN);
            this.matrices[6].Trace.ShouldBe(Double.NaN);
        }

        /// <summary>
        /// Tests the <see cref="Matrix.GetColumn(Int32)" /> method.
        /// </summary>
        [Test]
        public void MatrixGetColumnTest()
        {
            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                for (Int32 columnIndex = 0; columnIndex < this.matrices[i].NumberOfColumns; columnIndex++)
                {
                    Double[] column = this.matrices[i].GetColumn(columnIndex);

                    for (Int32 rowIndex = 0; rowIndex < this.matrices[i].NumberOfRows; rowIndex++)
                    {
                        column[rowIndex].ShouldBe(this.matrices[i][rowIndex, columnIndex]);
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
            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                for (Int32 rowIndex = 0; rowIndex < this.matrices[i].NumberOfRows; rowIndex++)
                {
                    Double[] row = this.matrices[i].GetRow(rowIndex);

                    for (Int32 columnIndex = 0; columnIndex < this.matrices[i].NumberOfColumns; columnIndex++)
                    {
                        row[columnIndex].ShouldBe(this.matrices[i][rowIndex, columnIndex]);
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

            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                Matrix.AreEqual(this.matrices[i], null).ShouldBeFalse();
                Matrix.AreEqual(null, this.matrices[i]).ShouldBeFalse();
                Matrix.AreEqual(this.matrices[i], new Matrix(this.matrices[i])).ShouldBeTrue();

                for (Int32 j = 0; j < this.matrices.Length; j++)
                {
                    if (i == j)
                        Matrix.AreEqual(this.matrices[i], this.matrices[j]).ShouldBeTrue();
                    else
                        Matrix.AreEqual(this.matrices[i], this.matrices[j]).ShouldBeFalse();
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="Matrix.IsValid(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixIsValidTest()
        {
            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                Matrix.IsValid(this.matrices[i]).ShouldBeTrue();
            }

            Matrix.IsValid(new Matrix(new[,] { { 1, Double.NaN }, { 0, 1 } })).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => Matrix.IsValid(null));
        }

        /// <summary>
        /// Tests the <see cref="Matrix.IsZero(Matrix)" /> method.
        /// </summary>
        [Test]
        public void MatrixIsZeroTest()
        {
            for (Int32 i = 0; i < 1; i++)
            {
                Matrix.IsZero(this.matrices[i]).ShouldBeTrue();
            }

            for (Int32 i = 2; i < this.matrices.Length; i++)
            {
                Matrix.IsZero(this.matrices[i]).ShouldBeFalse();
            }

            Matrix.IsValid(new Matrix(new[,] { { 1, Double.NaN }, { 0, 1 } })).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => Matrix.IsZero(null));
        }

        /// <summary>
        /// Tests the negation operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixNegationTest()
        {
            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                Matrix result = -this.matrices[i];

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        result[rowIndex, columnIndex].ShouldBe(-this.matrices[i][rowIndex, columnIndex]);
                    }
                }
            }

            Matrix matrix = null;
            Should.Throw<ArgumentNullException>(() => matrix = -matrix);
        }

        /// <summary>
        /// Tests the addition operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixAdditionTest()
        {
            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                Matrix result = this.matrices[i] + this.matrices[i];

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        result[rowIndex, columnIndex].ShouldBe(2 * this.matrices[i][rowIndex, columnIndex]);
                    }
                }
            }

            Matrix matrix = null;
            Should.Throw<ArgumentNullException>(() => matrix = this.matrices[3] + matrix);
            Should.Throw<ArgumentNullException>(() => matrix = matrix + this.matrices[3]);
            Should.Throw<ArgumentException>(() => matrix = this.matrices[3] + this.matrices[6]);
            Should.Throw<ArgumentException>(() => matrix = this.matrices[3] + this.matrices[5]);
        }

        /// <summary>
        /// Tests the subtraction operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixSubtractionTest()
        {
            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                Matrix result = this.matrices[i] - this.matrices[i];
                result.ShouldAllBe(value => value == 0);

                result = new Matrix(this.matrices[i].NumberOfRows, this.matrices[i].NumberOfColumns) - this.matrices[i];

                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                    {
                        result[rowIndex, columnIndex].ShouldBe(-this.matrices[i][rowIndex, columnIndex]);
                    }
                }
            }

            Matrix matrix = null;
            Should.Throw<ArgumentNullException>(() => matrix = this.matrices[3] - matrix);
            Should.Throw<ArgumentNullException>(() => matrix = matrix - this.matrices[3]);
            Should.Throw<ArgumentException>(() => matrix = this.matrices[3] - this.matrices[5]);
            Should.Throw<ArgumentException>(() => matrix = this.matrices[3] - this.matrices[6]);
        }

        /// <summary>
        /// Tests the multiplication operator of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixMultiplicationTest()
        {
            // multiply with scalar
            Double scalar = Math.PI;
            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                Matrix product = scalar * this.matrices[i];

                for (Int32 rowIndex = 0; rowIndex < product.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < product.NumberOfColumns; columnIndex++)
                    {
                        product[rowIndex, columnIndex].ShouldBe(scalar * this.matrices[i][rowIndex, columnIndex]);
                    }
                }

                product = this.matrices[i] * scalar;

                for (Int32 rowIndex = 0; rowIndex < product.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < product.NumberOfColumns; columnIndex++)
                    {
                        product[rowIndex, columnIndex].ShouldBe(scalar * this.matrices[i][rowIndex, columnIndex]);
                    }
                }
            }

            // multiply with matrix
            Matrix[] expectedMultiple = new Matrix[]
            {
                MatrixFactory.CreateSquare(),
                MatrixFactory.CreateSquare(0),
                MatrixFactory.CreateSquare(0.25, 0, 0, 0.25),
                MatrixFactory.CreateSquare(12, 20, 26,
                                           17, 29, 38,
                                           3, 7, 12),
                MatrixFactory.CreateSquare(206, 562, 728, 102,
                                           34, 98, 112, 40,
                                           106, 423, 437, 127,
                                           50, 167, 191, 42)
            };

            for (Int32 i = 0; i < 4; i++)
            {
                Matrix mulMatrix = this.matrices[i] * this.matrices[i];

                for (Int32 rowIndex = 0; rowIndex < mulMatrix.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < mulMatrix.NumberOfColumns; columnIndex++)
                    {
                        mulMatrix[rowIndex, columnIndex].ShouldBe(expectedMultiple[i][rowIndex, columnIndex]);
                    }
                }
            }

            Matrix matrix = null;
            Should.Throw<ArgumentNullException>(() => matrix = 1 * matrix);
            Should.Throw<ArgumentNullException>(() => matrix = matrix * 1);
            Should.Throw<ArgumentNullException>(() => matrix = this.matrices[3] * matrix);
            Should.Throw<ArgumentNullException>(() => matrix = matrix * this.matrices[3]);
            Should.Throw<ArgumentException>(() => matrix = this.matrices[3] * this.matrices[6]);
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
            for (Int32 i = 0; i < result.Size; i++)
            {
                result[i].ShouldBe(expected[i]);
            }

            // second matrix
            matrix = new Matrix(new Double[,] { { 1 }, { 5 }, { 7 }, { 3 } });
            expected = new Vector(new Double[] { 1, 5, 7, 3 });
            result = (Vector)matrix;

            result.Size.ShouldBe(4);
            for (Int32 i = 0; i < result.Size; i++)
            {
                result[i].ShouldBe(expected[i]);
            }

            Should.Throw<ArgumentNullException>(() => result = (Vector)((Matrix)null));
            Should.Throw<ArgumentException>(() => result = (Vector)this.matrices[0]);
            Should.Throw<ArgumentException>(() => result = (Vector)this.matrices[2]);
        }

        /// <summary>
        /// Tests enumeration of the <see cref="Matrix" /> class.
        /// </summary>
        [Test]
        public void MatrixEnumeratorTest()
        {
            Double[] expected = new[] { 1.0, 3, 5, 2, 4, 7, 1, 1, 0 };
            IEnumerator<Double> genericEnumerator = this.matrices[3].GetEnumerator();
            IEnumerator enumerator = (this.matrices[3] as IEnumerable).GetEnumerator();

            foreach (Double exp in expected)
            {
                enumerator.MoveNext().ShouldBeTrue();
                genericEnumerator.MoveNext().ShouldBeTrue();
                enumerator.Current.ShouldBe(exp);
                genericEnumerator.Current.ShouldBe(exp);
            }
        }

        /// <summary>
        /// Tests the <see cref="Matrix.ToString()"/> method.
        /// </summary>
        [Test]
        public void MatrixToStringTest()
        {
            String[] expectedStrings = new String[]
            {
                "()",
                "(0)",
                "(0.5 0; 0 -0.5)",
                "(1 3 5; 2 4 7; 1 1 0)",
                "(11 9 24 2; 1 5 2 6; 3 17 18 1; 2 5 7 1)",
                "(1 3; -1 0.7; 12 0)",
                "(1 3 7; 1.6 -12 5)"
            };

            for (Int32 i = 0; i < this.matrices.Length; i++)
            {
                this.matrices[i].ToString().ShouldBe(expectedStrings[i]);
            }
        }
    }
}
