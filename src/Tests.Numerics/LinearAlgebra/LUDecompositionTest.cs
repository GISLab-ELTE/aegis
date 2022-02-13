// <copyright file="LUDecompositionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics.LinearAlgebra
{
    using System;
    using AEGIS.Numerics;
    using AEGIS.Numerics.LinearAlgebra;
    using AEGIS.Tests.Numerics;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="LUDecomposition" /> class.
    /// </summary>
    [TestFixture]
    public class LUDecompositionTest
    {
        /// <summary>
        /// The array of matrices.
        /// </summary>
        private Matrix[] matrices;

        /// <summary>
        /// The array of expected L matrices.
        /// </summary>
        private Matrix[] expectedL;

        /// <summary>
        /// The array of expected U matrices.
        /// </summary>
        private Matrix[] expectedU;

        /// <summary>
        /// The array of expected P matrices.
        /// </summary>
        private Matrix[] expectedP;

        /// <summary>
        /// The array of expected determinants.
        /// </summary>
        private Double[] expectedDeterminant;

        /// <summary>
        /// The array of expected inverted matrices.
        /// </summary>
        private Matrix[] expectedInverse;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.matrices = new Matrix[]
            {
                MatrixFactory.CreateSquare(1, 3, 5,
                                           2, 4, 7,
                                           1, 1, 0),
                MatrixFactory.CreateSquare(11, 9, 24, 2,
                                           1, 5, 2, 6,
                                           3, 17, 18, 1,
                                           2, 5, 7, 1),
            };

            this.expectedL = new Matrix[]
            {
                MatrixFactory.CreateSquare(1, 0, 0,
                                           0.5, 1, 0,
                                           0.5, -1, 1),
                MatrixFactory.CreateSquare(1, 0, 0, 0,
                                           0.272727272727273, 1, 0, 0,
                                           0.0909090909090909, 0.28750, 1, 0,
                                           0.181818181818182, 0.23125, 0.00359712230215807, 1),
            };

            this.expectedU = new Matrix[]
            {
                MatrixFactory.CreateSquare(2, 4, 7,
                                           0, 1, 1.5,
                                           0, 0, -2),
                MatrixFactory.CreateSquare(11, 9, 24, 2,
                                           0, 14.54545, 11.45455, 0.45455,
                                           0, 0, -3.47500, 5.68750,
                                           0, 0, 0, 0.51079),
            };

            this.expectedP = new Matrix[]
            {
                MatrixFactory.CreateSquare(0, 1, 0,
                                           1, 0, 0,
                                           0, 0, 1),
                MatrixFactory.CreateSquare(1, 0, 0, 0,
                                           0, 0, 1, 0,
                                           0, 1, 0, 0,
                                           0, 0, 0, 1),
            };

            this.expectedDeterminant = new Double[] { 4, 284 };

            this.expectedInverse = new Matrix[]
            {
                MatrixFactory.CreateSquare(-1.75, 1.25, 0.25,
                                           1.75, -1.25, 0.75,
                                           -0.5, 0.5, -0.5),
                MatrixFactory.CreateSquare(0.72, 0.46, 1.02, -5.23,
                                           0.29, 0.24, 0.60, -2.58,
                                           -0.38, -0.30, -0.65, 3.20,
                                           -0.23, 0.00, -0.45, 1.96),
            };
        }

        /// <summary>
        /// Tests the <see cref="LUDecomposition.Compute()" /> method.
        /// </summary>
        [Test]
        public void LUDecompositionComputeTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                LUDecomposition decomposition = new LUDecomposition(this.matrices[matrixIndex]);
                decomposition.Compute();

                decomposition.L.ShouldBe(this.expectedL[matrixIndex], 0.01);
                decomposition.U.ShouldBe(this.expectedU[matrixIndex], 0.01);
                decomposition.P.ShouldBe(this.expectedP[matrixIndex], 0.01);
            }
        }

        /// <summary>
        /// Tests the <see cref="LUDecomposition.ComputeDeterminant(Matrix)" /> method.
        /// </summary>
        [Test]
        public void LUDecompositionComputeDeterminantTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Double determinant = LUDecomposition.ComputeDeterminant(this.matrices[matrixIndex]);

                determinant.ShouldBe(this.expectedDeterminant[matrixIndex], 0.01);
            }

            Should.Throw<ArgumentNullException>(() => LUDecomposition.ComputeDeterminant(null));
        }

        /// <summary>
        /// Tests the <see cref="LUDecomposition.Invert(Matrix)" /> method.
        /// </summary>
        [Test]
        public void LUDecompositionInvertTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Matrix inverse = LUDecomposition.Invert(this.matrices[matrixIndex]);

                inverse.ShouldBe(this.expectedInverse[matrixIndex], 0.01);
            }

            Should.Throw<ArgumentNullException>(() => LUDecomposition.ComputeDeterminant(null));
        }
    }
}
