// <copyright file="HouseholderTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Numerics.LinearAlgebra
{
    using System;
    using ELTE.AEGIS.Numerics;
    using ELTE.AEGIS.Numerics.LinearAlgebra;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="HouseholderTransformation" /> class.
    /// </summary>
    [TestFixture]
    public class HouseholderTransformationTest
    {
        #region Test methods

        /// <summary>
        /// Tests the <see cref="HouseholderTransformation.Compute()" /> method.
        /// </summary>
        [Test]
        public void HouseholderTransformationComputeTest()
        {
            // first vector
            HouseholderTransformation transformation = new HouseholderTransformation(new Vector(new Double[] { 1, 1, 1, 1 }));
            transformation.Compute();

            Matrix resultH = transformation.H;
            Matrix expectedH = new Matrix(new[,] { { 0.5, 0.5, 0.5, 0.5 }, { 0.5, 0.5, -0.5, -0.5 }, { 0.5, -0.5, 0.5, -0.5 }, { 0.5, -0.5, -0.5, 0.5 } });

            resultH.NumberOfColumns.ShouldBe(expectedH.NumberOfColumns);
            resultH.NumberOfRows.ShouldBe(expectedH.NumberOfRows);
            for (Int32 rowIndex = 0; rowIndex < resultH.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < resultH.NumberOfColumns; columnIndex++)
                {
                    resultH[rowIndex, columnIndex].ShouldBe(expectedH[rowIndex, columnIndex], 0.01);
                }
            }

            // second vector
            transformation = new HouseholderTransformation(new Double[] { 1, -1 });
            transformation.Compute();

            resultH = transformation.H;
            expectedH = new Matrix(new[,] { { 1 / Math.Sqrt(2), -1 / Math.Sqrt(2) }, { -1 / Math.Sqrt(2), -1 / Math.Sqrt(2) } });

            resultH.NumberOfColumns.ShouldBe(expectedH.NumberOfColumns);
            resultH.NumberOfRows.ShouldBe(expectedH.NumberOfRows);
            for (Int32 rowIndex = 0; rowIndex < resultH.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < resultH.NumberOfColumns; columnIndex++)
                {
                    resultH[rowIndex, columnIndex].ShouldBe(expectedH[rowIndex, columnIndex], 0.01);
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="HouseholderTransformation.Tridiagonalize(Matrix)" /> method.
        /// </summary>
        [Test]
        public void HouseholderTransformationTridiagonalizeTest()
        {
            // first matrix
            Matrix matrix = new Matrix(new[,] { { 4.0, 1, -2, 2 }, { 1, 2, 0, 1 }, { -2, 0, 3, -2 }, { 2, 1, -2, -1 } });
            Matrix expected = new Matrix(new[,] { { 4, -3, 0, 0 }, { -3, 10.0 / 3, -5.0 / 3, 0 }, { 0, -5.0 / 3, -33.0 / 25, 68.0 / 75 }, { 0, 0, 68.0 / 75, 149.0 / 75 } });

            Matrix result = HouseholderTransformation.Tridiagonalize(matrix);

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex].ShouldBe(expected[rowIndex, columnIndex], 0.01);
                }
            }

            // second matrix
            matrix = new Matrix(new[,] { { 4.0, 2, 2, 1 }, { 2, -3, 1, 1 }, { 2, 1, 3, 1 }, { 1, 1, 1, 2 } });
            expected = new Matrix(new[,] { { 4, -3, 0, 0 }, { -3, 2, 3.1623, 0 }, { 0, 3.1623, -1.4, -0.2 }, { 0, 0, -0.2, 1.4 } });

            result = HouseholderTransformation.Tridiagonalize(matrix);

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex].ShouldBe(expected[rowIndex, columnIndex], 0.01);
                }
            }

            // exceptions

            Should.Throw<ArgumentNullException>(() => HouseholderTransformation.Tridiagonalize(null));
            Should.Throw<ArgumentException>(() => HouseholderTransformation.Tridiagonalize(new Matrix(2, 3)));
            Should.Throw<ArgumentException>(() => HouseholderTransformation.Tridiagonalize(MatrixFactory.CreateSquare(1, 2, 3, 4)));
        }

        #endregion
    }
}
