// <copyright file="CholeskyDecompositionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="CholeskyDecomposition" /> class.
    /// </summary>
    [TestFixture]
    public class CholeskyDecompositionTest
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
        /// The array of expected transposed L matrices.
        /// </summary>
        private Matrix[] expectedLT;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.matrices = new Matrix[]
            {
                new Matrix(new[,] { { 18.0, 22, 54, 42 }, { 22, 70, 86, 62 }, { 54, 86, 174, 134 }, { 42, 62, 134, 106 } }),

                new Matrix(new[,] { { 25.0, 15, -5 }, { 15, 18, 0 }, { -5, 0, 11 } })
            };

            this.expectedL = new Matrix[]
            {
                new Matrix(new[,] { { 4.24264, 0, 0, 0 }, { 5.18545, 6.56591, 0, 0 }, { 12.72792, 3.04604, 1.64974, 0 }, { 9.89949, 1.62455, 1.84971, 1.39262 } }),
                new Matrix(new[,] { { 5.0, 0, 0 }, { 3, 3, 0 }, { -1, 1, 3 } })
            };

            this.expectedLT = new Matrix[]
            {
                new Matrix(new[,] { { 4.24264, 5.18545, 12.72792, 9.89949 }, { 0, 6.56591, 3.04604, 1.62455 }, { 0, 0, 1.64974, 1.84971 }, { 0, 0, 0, 1.39262 } }),
                new Matrix(new[,] { { 5.0, 3, -1 }, { 0, 3, 1 }, { 0, 0, 3 } })
            };
        }

        /// <summary>
        /// Tests the constructor of the <see cref="CholeskyDecomposition" /> class.
        /// </summary>
        [Test]
        public void CholeskyDecompositionConstructorTest()
        {
            Should.Throw<ArgumentNullException>(() => new CholeskyDecomposition(null));
            Should.Throw<ArgumentException>(() => new CholeskyDecomposition(new Matrix(2, 3)));
        }

        /// <summary>
        /// Tests the <see cref="CholeskyDecomposition.Compute()" /> method.
        /// </summary>
        [Test]
        public void CholeskyDecompositionComputeTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                CholeskyDecomposition decomposition = new CholeskyDecomposition(this.matrices[matrixIndex]);
                decomposition.Compute();

                decomposition.L.ShouldBe(this.expectedL[matrixIndex], 0.001);
                decomposition.LT.ShouldBe(this.expectedLT[matrixIndex], 0.001);
            }
        }
    }
}
