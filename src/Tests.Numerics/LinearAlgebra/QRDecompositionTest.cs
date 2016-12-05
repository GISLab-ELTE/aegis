// <copyright file="QRDecompositionTest.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Test fixture for the <see cref="QRDecomposition" /> class.
    /// </summary>
    [TestFixture]
    public class QRDecompositionTest
    {
        /// <summary>
        /// The array of matrices.
        /// </summary>
        private Matrix[] matrices;

        /// <summary>
        /// The array of expected Q matrices.
        /// </summary>
        private Matrix[] expectedQ;

        /// <summary>
        /// The array of expected R matrices.
        /// </summary>
        private Matrix[] expectedR;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            this.matrices = new Matrix[]
            {
                new Matrix(new[,] { { 6.0, -1 }, { 2, 3 } }),
                new Matrix(new[,] { { 12.0, -51, 4 }, { 6, 167, -68 }, { -4, 24, -41 } })
            };

            this.expectedQ = new Matrix[]
            {
                new Matrix(new[,] { { -0.9487, 0.3162 }, { -0.3162, -0.9487 } }),
                new Matrix(new[,] { { -0.8571, 0.3943, 0.3314 }, { -0.4286, -0.9029, -0.0343 }, { 0.2857, -0.1714, 0.9429 } })
            };

            this.expectedR = new Matrix[]
            {
                new Matrix(new[,] { { -6.3246, 0 }, { 0, -3.1623 } }),
                new Matrix(new[,] { { -14.0, -21, 14 }, { 0, -175, 70 }, { 0, 0, -35 } })
            };
        }

        /// <summary>
        /// Tests the <see cref="QRDecomposition.Compute()" /> method.
        /// </summary>
        [Test]
        public void QRDecompositionComputeTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                QRDecomposition decomposition = new QRDecomposition(this.matrices[matrixIndex]);
                decomposition.Compute();

                decomposition.Q.ShouldBe(this.expectedQ[matrixIndex], 0.001);
                decomposition.R.ShouldBe(this.expectedR[matrixIndex], 0.001);

                Matrix product = decomposition.Q * decomposition.R;
                product.ShouldBe(this.matrices[matrixIndex], 0.001);
            }
        }
    }
}
