// <copyright file="VectorTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Numerics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Numerics;
    using AEGIS.Numerics.LinearAlgebra;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="Vector" /> class.
    /// </summary>
    [TestFixture]
    public class VectorTest
    {
        /// <summary>
        /// An array of vector array.
        /// </summary>
        private Vector[] vectors;

        /// <summary>
        /// An array of test values.
        /// </summary>
        private Double[] testValues;

        /// <summary>
        /// The null vector.
        /// </summary>
        private Vector nullVector;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testValues = new[] { 1.2, 5.8, 7.456, 15 };

            this.vectors = new Vector[]
            {
                new Vector(4),
                new Vector(1, 1, 1, 1),
                new Vector(3, 8, 54, 32),
                new Vector(this.testValues)
            };

            this.nullVector = null;
        }

        /// <summary>
        /// Tests the constructors of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorConstructorTest()
        {
            Vector vector = new Vector(0);
            vector.Size.ShouldBe(0);

            vector = new Vector(10);
            vector.Size.ShouldBe(10);
            vector.ShouldAllBe(value => value == 0);

            vector = new Vector(1, 1, 1, 1);
            vector.Size.ShouldBe(4);
            vector.ShouldAllBe(value => value == 1);

            vector = new Vector(this.testValues);
            vector.ShouldBe(this.testValues);

            vector = new Vector((IEnumerable<Double>)this.testValues);
            vector.ShouldBe(this.testValues);

            Vector copy = new Vector(vector);
            copy.ShouldBe(vector);

            Should.Throw<ArgumentOutOfRangeException>(() => new Vector(-3));
            Should.Throw<ArgumentNullException>(() => new Vector((Double[])null));
            Should.Throw<ArgumentNullException>(() => new Vector((IEnumerable<Double>)null));
            Should.Throw<ArgumentNullException>(() => new Vector((Vector)null));
        }

        /// <summary>
        /// Tests item properties of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorItemTest()
        {
            this.vectors[2][0].ShouldBe(3);
            this.vectors[2][3].ShouldBe(32);

            this.vectors[2][0] = Math.PI;
            this.vectors[2][0].ShouldBe(Math.PI);
            this.vectors[2][3] = 2 * Math.PI;
            this.vectors[2][3].ShouldBe(2 * Math.PI);

            Double value = 0;
            Should.Throw<ArgumentOutOfRangeException>(() => value = this.vectors[0][-1]);
            Should.Throw<ArgumentOutOfRangeException>(() => this.vectors[0][-1] = value);
        }

        /// <summary>
        /// Tests properties of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorPropertiesTest()
        {
            foreach (Vector vector in this.vectors)
                vector.Size.ShouldBe(4);

            this.vectors[0].Length.ShouldBe(0);
            this.vectors[1].Length.ShouldBe(2);
            this.vectors[2].Length.ShouldBe(63.35, 0.01);
            this.vectors[3].Length.ShouldBe(17.77, 0.01);
        }

        /// <summary>
        /// Tests the <see cref="Vector.ToString()" /> method.
        /// </summary>
        [Test]
        public void VectorToStringTest()
        {
            this.vectors[0].ToString().ShouldBe("(0 0 0 0)");
            this.vectors[1].ToString().ShouldBe("(1 1 1 1)");
            this.vectors[2].ToString().ShouldBe("(3 8 54 32)");
            this.vectors[3].ToString().ShouldBe("(1.2 5.8 7.456 15)");
        }

        /// <summary>
        /// Tests the <see cref="Vector.AreEqual(Vector, Vector)" /> method.
        /// </summary>
        [Test]
        public void VectorAreEqualTest()
        {
            Vector.AreEqual(this.vectors[0], new Vector(4)).ShouldBeTrue();
            Vector.AreEqual(new Vector(), new Vector()).ShouldBeTrue();
            Vector.AreEqual(this.vectors[3], new Vector(this.testValues)).ShouldBeTrue();
            Vector.AreEqual(this.vectors[3], new Vector(4)).ShouldBeFalse();
            Vector.AreEqual(new Vector(), new Vector(4)).ShouldBeFalse();
            Vector.AreEqual(this.nullVector, new Vector(4)).ShouldBeFalse();
            Vector.AreEqual(this.nullVector, null).ShouldBeTrue();
            Vector.AreEqual(this.vectors[0], this.vectors[0]).ShouldBeTrue();
        }

        /// <summary>
        /// Tests the <see cref="Vector.IsZero(Vector)" /> method.
        /// </summary>
        [Test]
        public void VectorIsZeroTest()
        {
            for (Int32 i = 1; i < this.vectors.Length; i++)
            {
                Vector.IsZero(this.vectors[i]).ShouldBeFalse();
            }

            Vector.IsZero(this.vectors[0]).ShouldBeTrue();

            Should.Throw<ArgumentNullException>(() => Vector.IsZero(this.nullVector));
        }

        /// <summary>
        /// Tests the <see cref="Vector.IsValid(Vector)" /> method.
        /// </summary>
        [Test]
        public void VectorIsValidTest()
        {
            for (Int32 i = 1; i < this.vectors.Length; i++)
            {
                Vector.IsValid(this.vectors[i]).ShouldBeTrue();
            }

            Vector.IsValid(new Vector(3, Double.NaN)).ShouldBeFalse();

            Should.Throw<ArgumentNullException>(() => Vector.IsValid(this.nullVector));
        }

        /// <summary>
        /// Tests the <see cref="Vector.InnerProduct(Vector, Vector)" /> method.
        /// </summary>
        [Test]
        public void VectorInnerProductTest()
        {
            Double resultScalar = Vector.InnerProduct(this.vectors[0], this.vectors[1]);
            resultScalar.ShouldBe(0);
            resultScalar = Vector.InnerProduct(this.vectors[3], this.vectors[0]);
            resultScalar.ShouldBe(0);
            resultScalar = Vector.InnerProduct(this.vectors[2], this.vectors[3]);
            resultScalar.ShouldBe(932.624);

            Should.Throw<ArgumentNullException>(() => resultScalar = this.vectors[1] * this.nullVector);
            Should.Throw<ArgumentNullException>(() => resultScalar = this.nullVector * this.vectors[3]);
            Should.Throw<ArgumentException>(() => resultScalar = new Vector(7) * this.vectors[3]);
        }

        /// <summary>
        /// Tests the <see cref="Vector.OuterProduct(Vector, Vector)" /> method.
        /// </summary>
        [Test]
        public void VectorOuterProductTest()
        {
            Vector vector = new Vector(new Double[] { 1, 5, 3.2 });
            Matrix result;

            result = Vector.OuterProduct(this.vectors[0], vector);
            result.NumberOfRows.ShouldBe(4);
            result.NumberOfColumns.ShouldBe(3);
            result.ShouldAllBe(value => value == 0);

            result = Vector.OuterProduct(vector, this.vectors[0]);
            result.NumberOfRows.ShouldBe(3);
            result.NumberOfColumns.ShouldBe(4);
            result.ShouldAllBe(value => value == 0);

            result = Vector.OuterProduct(this.vectors[3], vector);
            result.NumberOfRows.ShouldBe(4);
            result.NumberOfColumns.ShouldBe(3);
            result.ShouldBe(new Double[] { 1.2, 6, 3.84, 5.8, 29, 18.56, 7.456, 37.28, 23.8592, 15, 75, 48 });

            result = Vector.OuterProduct(vector, new Vector());
            result.ShouldBeEmpty();

            Should.Throw<ArgumentNullException>(() => result = Vector.OuterProduct(vector, this.nullVector));
            Should.Throw<ArgumentNullException>(() => result = Vector.OuterProduct(this.nullVector, vector));
        }

        /// <summary>
        /// Tests the negation operator of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorNegationTest()
        {
            for (Int32 i = 0; i < this.vectors.Length; i++)
            {
                Vector result = -this.vectors[i];

                for (Int32 j = 0; j < 4; j++)
                {
                    result[j].ShouldBe(-this.vectors[i][j]);
                }
            }

            // exceptions
            Vector v = null;
            Should.Throw<ArgumentNullException>(() => v = -v);
        }

        /// <summary>
        /// Tests the addition operator of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorAdditionTest()
        {
            // addition of 2 vectors
            Vector result = this.vectors[0] + this.vectors[1];
            result.ShouldAllBe(value => value == 1);

            result = this.vectors[2] + this.vectors[3];
            result[0].ShouldBe(4.2);
            result[1].ShouldBe(13.8);
            result[2].ShouldBe(61.456);
            result[3].ShouldBe(47);

            // addition of a vector and an array
            result = this.vectors[2] + this.testValues;
            result[0].ShouldBe(4.2);
            result[1].ShouldBe(13.8);
            result[2].ShouldBe(61.456);
            result[3].ShouldBe(47);

            result = this.testValues + this.vectors[2];
            result[0].ShouldBe(4.2);
            result[1].ShouldBe(13.8);
            result[2].ShouldBe(61.456);
            result[3].ShouldBe(47);

            // exceptions
            Should.Throw<ArgumentNullException>(() => result = this.vectors[1] + this.nullVector);
            Should.Throw<ArgumentNullException>(() => result = this.nullVector + this.vectors[1]);
            Should.Throw<ArgumentException>(() => result = this.vectors[1] + new Vector(3));
            Should.Throw<ArgumentNullException>(() => result = this.vectors[1] + (Double[])null);
            Should.Throw<ArgumentNullException>(() => result = (Double[])null + this.vectors[1]);
            Should.Throw<ArgumentNullException>(() => result = this.nullVector + new Double[] { 1, 2, 3 });
            Should.Throw<ArgumentNullException>(() => result = new Double[] { 1, 2, 3 } + this.nullVector);
            Should.Throw<ArgumentException>(() => result = this.vectors[1] + new Double[] { 1, 2, 3 });
            Should.Throw<ArgumentException>(() => result = new Double[] { 1, 2, 3 } + this.vectors[1]);
        }

        /// <summary>
        /// Tests the subtraction operator of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorSubtractionTest()
        {
            Vector result = this.vectors[0] - this.vectors[1];
            result.ShouldAllBe(value => value == -1);

            result = this.vectors[3] - this.vectors[2];
            result[0].ShouldBe(-1.8);
            result[1].ShouldBe(-2.2);
            result[2].ShouldBe(-46.544);
            result[3].ShouldBe(-17);

            Should.Throw<ArgumentNullException>(() => result = this.vectors[1] - this.nullVector);
            Should.Throw<ArgumentNullException>(() => result = this.nullVector - this.vectors[1]);
            Should.Throw<ArgumentException>(() => result = new Vector(5) - this.vectors[2]);
        }

        /// <summary>
        /// Tests the multiplication operator of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorMultiplicationTest()
        {
            // scalar
            for (Int32 i = 0; i < this.vectors.Length; i++)
            {
                Vector[] results = new Vector[]
                {
                    -5 * this.vectors[i],
                    3.567 * this.vectors[i],
                    this.vectors[i] * -5,
                    this.vectors[i] * 3.567
                };

                for (Int32 j = 0; j < this.vectors[i].Size; j++)
                {
                    results[0][j].ShouldBe(-5 * this.vectors[i][j]);
                    results[1][j].ShouldBe(3.567 * this.vectors[i][j]);
                    results[2][j].ShouldBe(-5 * this.vectors[i][j]);
                    results[3][j].ShouldBe(3.567 * this.vectors[i][j]);
                }
            }

            // vector
            Double resultScalar = this.vectors[0] * this.vectors[1];
            resultScalar.ShouldBe(0);
            resultScalar = this.vectors[3] * this.vectors[0];
            resultScalar.ShouldBe(0);
            resultScalar = this.vectors[2] * this.vectors[3];
            resultScalar.ShouldBe(932.624);

            // matrix (left side)
            Matrix matrix = new Matrix(new[,] { { 11, 9, 24, 2 }, { 1.75, 5, 2.33, 6 }, { 3, 17, 18, 1 }, { 2, 5, 7.4, 1 } });
            Vector resultVector = matrix * this.vectors[0];
            resultVector.ShouldBe(new[] { 0.0, 0, 0, 0 });
            resultVector = matrix * this.vectors[1];
            resultVector.ShouldBe(new[] { 46, 15.08, 39, 15.4 });
            resultVector = matrix * this.vectors[3];
            resultVector.ShouldBe(new[] { 274.344, 138.472, 251.408, 101.574 }, 0.001);

            matrix = new Matrix(new[,] { { 1, 2, 18, 3.45 }, { 7.5, 4, 13.4, 7.2 } });
            resultVector = matrix * this.vectors[2];
            resultVector.ShouldBe(new[] { 1101.4, 1008.5 }, 0.001);
            resultVector = matrix * this.vectors[3];
            resultVector.ShouldBe(new[] { 198.758, 240.11 }, 0.001);

            // matrix (right side)
            matrix = new Matrix(new[,] { { 1, 18, 7.5, 13.4 } });
            Matrix resultMatrix = this.vectors[0] * matrix;
            resultMatrix.NumberOfColumns.ShouldBe(4);
            resultMatrix.NumberOfRows.ShouldBe(4);
            resultMatrix.ShouldAllBe(value => value == 0);

            resultMatrix = this.vectors[1] * matrix;
            resultMatrix.ShouldBe(new[] { 1, 18, 7.5, 13.4, 1, 18, 7.5, 13.4, 1, 18, 7.5, 13.4, 1, 18, 7.5, 13.4 });

            // exceptions
            Should.Throw<ArgumentNullException>(() => resultVector = 1 * this.nullVector);
            Should.Throw<ArgumentNullException>(() => resultVector = this.nullVector * 1);
            Should.Throw<ArgumentNullException>(() => resultScalar = this.vectors[1] * this.nullVector);
            Should.Throw<ArgumentNullException>(() => resultScalar = this.nullVector * this.vectors[3]);
            Should.Throw<ArgumentException>(() => resultScalar = new Vector(7) * this.vectors[3]);
            Should.Throw<ArgumentNullException>(() => resultVector = matrix * this.nullVector);
            Should.Throw<ArgumentNullException>(() => resultVector = (Matrix)null * this.vectors[2]);
            Should.Throw<ArgumentException>(() => resultVector = new Matrix(new Double[2, 2]) * this.vectors[2]);
            Should.Throw<ArgumentNullException>(() => matrix = this.nullVector * matrix);
            Should.Throw<ArgumentNullException>(() => matrix = this.vectors[2] * (Matrix)null);
            Should.Throw<ArgumentException>(() => matrix = this.vectors[2] * new Matrix(new Double[2, 2]));
        }

        /// <summary>
        /// Tests the division operator of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorDivisionTest()
        {
            // scalar
            for (Int32 i = 0; i < this.vectors.Length; i++)
            {
                for (Int32 j = 0; j < this.vectors[i].Size; j++)
                {
                    Vector result = this.vectors[i] / -5;
                    result[j].ShouldBe(this.vectors[i][j] / -5, 0.001);
                    result = this.vectors[i] / 3.456;
                    result[j].ShouldBe(this.vectors[i][j] / 3.456, 0.001);
                }
            }

            // exceptions
            Vector vector;
            Should.Throw<ArgumentNullException>(() => vector = this.nullVector / 1);
        }

        /// <summary>
        /// Tests the <see cref="Vector.Normalize(Vector)"/> method.
        /// </summary>
        [Test]
        public void VectorNormalizeTest()
        {
            Vector[] normalizedVectors = new Vector[]
            {
                new Vector(4),
                new Vector(0.5, 0.5, 0.5, 0.5),
                new Vector(3 / Math.Sqrt(4013), 8 / Math.Sqrt(4013), 54 / Math.Sqrt(4013), 32 / Math.Sqrt(4013)),
                new Vector(0.0675403, 0.326445, 0.419651, 0.844254)
            };

            for (Int32 i = 0; i < normalizedVectors.Length; i++)
            {
                Vector.Normalize(this.vectors[i]).ShouldBe(normalizedVectors[i], 0.000001);
            }
        }

        /// <summary>
        /// Tests the <see cref="Matrix" /> cast operator of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorOperatorCastToMatrixTest()
        {
            Vector vector = new Vector(this.testValues);
            Matrix result = (Matrix)vector;

            result.NumberOfColumns.ShouldBe(1);
            result.NumberOfRows.ShouldBe(4);

            for (Int32 i = 0; i < vector.Size; i++)
            {
                result[i, 0].ShouldBe(vector[i]);
            }

            // exceptions
            Should.Throw<ArgumentNullException>(() => result = (Matrix)this.nullVector);
        }

        /// <summary>
        /// Tests enumeration of the <see cref="Vector" /> class.
        /// </summary>
        [Test]
        public void VectorEnumeratorTest()
        {
            Double[] expected = new[] { 1.2, 5.8, 7.456, 15 };
            IEnumerator<Double> genericEnumerator = this.vectors[3].GetEnumerator();
            IEnumerator enumerator = (this.vectors[3] as IEnumerable).GetEnumerator();

            foreach (Double exp in expected)
            {
                enumerator.MoveNext().ShouldBeTrue();
                genericEnumerator.MoveNext().ShouldBeTrue();
                enumerator.Current.ShouldBe(exp);
                genericEnumerator.Current.ShouldBe(exp);
            }
        }
    }
}
