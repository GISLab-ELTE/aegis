// <copyright file="QRAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using AEGIS.Numerics;
    using AEGIS.Numerics.LinearAlgebra;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="QRAlgorithm" /> class.
    /// </summary>
    public class QRAlgorithmTest
    {
        /// <summary>
        /// The array of matrices.
        /// </summary>
        private Matrix[] matrices;

        /// <summary>
        /// The array of expected eigenvalues.
        /// </summary>
        private Double[][] expectedEigenvalues;

        /// <summary>
        /// The array of expected eigenvectors.
        /// </summary>
        private Vector[][] expectedEigenvectors;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            this.matrices = new Matrix[]
            {
                new Matrix(new[,] { { 6.0, -1 }, { 2, 3 } }),
                new Matrix(new[,] { { 12.0, -51, 4 }, { 6, 167, -68 }, { -4, 24, -41 } }),
            };

            this.expectedEigenvalues = new Double[][]
            {
                new[] { 5.0, 4 },
                new[] { 156.137, 16.06, -34.1967 },
            };

            this.expectedEigenvectors = new Vector[][]
            {
                new[] { new Vector(-0.7071, -0.7071), new Vector(0.7071, -0.7071) },
                new[] { new Vector(0.3281, -0.9369, -0.1207), new Vector(0.8668, 0.3494, -0.356), new Vector(-0.375, -0.0121, -0.9268) },
            };
        }

        /// <summary>
        /// Tests the <see cref="QRAlgorithm.ComputeEigenvalues(Matrix)" /> method.
        /// </summary>
        [Test]
        public void QRAlgorithmComputeEigenvaluesTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Double[] result = QRAlgorithm.ComputeEigenvalues(this.matrices[matrixIndex]).ToArray();

                result.ShouldBe(this.expectedEigenvalues[matrixIndex], 0.001);
            }
        }

        /// <summary>
        /// Tests the <see cref="QRAlgorithm.ComputeEigenvectors(Matrix)" /> method.
        /// </summary>
        [Test]
        public void QRAlgorithmComputeEigenvectorsTest()
        {
            for (Int32 matrixIndex = 0; matrixIndex < this.matrices.Length; matrixIndex++)
            {
                Vector[] result = QRAlgorithm.ComputeEigenvectors(this.matrices[matrixIndex]).ToArray();

                for (Int32 vectorIndex = 0; vectorIndex < result.Length; vectorIndex++)
                {
                    for (Int32 coordinateIndex = 0; coordinateIndex < result[vectorIndex].Size; coordinateIndex++)
                    {
                        result[vectorIndex][coordinateIndex].ShouldBe(this.expectedEigenvectors[matrixIndex][vectorIndex][coordinateIndex], 0.001);
                    }
                }
            }
        }
    }
}
