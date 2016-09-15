// <copyright file="Vector.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using ELTE.AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a vector in Euclidean space.
    /// </summary>
    public class Vector : IEnumerable<Double>
    {
        #region Private fields

        /// <summary>
        /// The array of values.
        /// </summary>
        private Double[] values;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The size is less than 0.</exception>
        public Vector(Int32 size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), Messages.SizeIsLessThan0);

            this.values = new Double[size];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="values">The values of the vector.</param>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public Vector(params Double[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), Messages.ValueCollectionIsNull);

            this.values = values.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="values">The values of the vector.</param>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public Vector(IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), Messages.ValueCollectionIsNull);

            this.values = values.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> class.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <exception cref="System.ArgumentNullException">The other vector is null.</exception>
        public Vector(Vector other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), Messages.OtherVectorIsNull);

            this.values = new Double[other.values.Length];
            Array.Copy(other.values, this.values, this.values.Length);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the size of the vector.
        /// </summary>
        /// <value>The size of the vector.</value>
        public Int32 Size { get { return this.values.Length; } }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <value>The length of the vector.</value>
        public Double Length { get { return Math.Sqrt(this.values.Sum(value => value * value)); } }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <value>The value.</value>
        /// <param name="index">The index.</param>
        /// <returns>The value at the specified index.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Index is outside the bounds of the vector.</exception>
        public Double this[Int32 index]
        {
            get
            {
                if (index < 0 || index >= this.Size)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsOutsideVectorBounds);

                return this.values[index];
            }

            set
            {
                if (index < 0 || index >= this.Size)
                    throw new ArgumentOutOfRangeException(nameof(index), Messages.IndexIsOutsideVectorBounds);

                this.values[index] = value;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts the specified vector to a <see cref="Matrix" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The matrix equivalent of the vector.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static explicit operator Matrix(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);

            Matrix matrix = new Matrix(vector.Size, 1);
            for (Int32 rowIndex = 0; rowIndex < vector.Size; rowIndex++)
            {
                matrix[rowIndex, 0] = vector[rowIndex];
            }

            return matrix;
        }

        /// <summary>
        /// Inverts all values of the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The vector with the inverted values.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator -(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);

            Vector result = new Vector(vector.Size);
            for (Int32 valueIndex = 0; valueIndex < vector.Size; valueIndex++)
            {
                result.values[valueIndex] = -vector.values[valueIndex];
            }

            return result;
        }

        /// <summary>
        /// Sums the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator +(Vector first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            if (first.Size != second.Size)
                throw new ArgumentException(Messages.VectorDimensionsAreDifferent, nameof(second));

            Vector result = new Vector(first.Size);
            for (Int32 valueIndex = 0; valueIndex < first.Size; valueIndex++)
            {
                result.values[valueIndex] = first.values[valueIndex] + second.values[valueIndex];
            }

            return result;
        }

        /// <summary>
        /// Sums the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator +(Vector first, Double[] second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            if (first.Size != second.Length)
                throw new ArgumentException(Messages.VectorDimensionsAreDifferent, nameof(second));

            Vector result = new Vector(first.Size);
            for (Int32 valueIndex = 0; valueIndex < first.Size; valueIndex++)
            {
                result.values[valueIndex] = first.values[valueIndex] + second[valueIndex];
            }

            return result;
        }

        /// <summary>
        /// Sums the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator +(Double[] first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            if (first.Length != second.Size)
                throw new ArgumentException(Messages.VectorDimensionsAreDifferent, nameof(second));

            Vector result = new Vector(first.Length);
            for (Int32 valueIndex = 0; valueIndex < first.Length; valueIndex++)
            {
                result.values[valueIndex] = first[valueIndex] + second.values[valueIndex];
            }

            return result;
        }

        /// <summary>
        /// Extracts the specified vector instances.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The extract of the two vector instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Vector operator -(Vector first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            if (first.Size != second.Size)
                throw new ArgumentException(Messages.VectorDimensionsAreDifferent, nameof(second));

            Vector result = new Vector(first.Size);
            for (Int32 valueIndex = 0; valueIndex < first.Size; valueIndex++)
            {
                result.values[valueIndex] = first.values[valueIndex] - second.values[valueIndex];
            }

            return result;
        }

        /// <summary>
        /// Multiplies the <see cref="System.Double" /> scalar with a vector.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The scalar multiplication of the vector.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator *(Double scalar, Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);

            Vector result = new Vector(vector.Size);
            for (Int32 valueIndex = 0; valueIndex < result.Size; valueIndex++)
            {
                result.values[valueIndex] = scalar * vector.values[valueIndex];
            }

            return result;
        }

        /// <summary>
        /// Multiplies the vector with a <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar multiplication of the vector.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator *(Vector vector, Double scalar)
        {
            return scalar * vector; // the operation is commutative
        }

        /// <summary>
        /// Calculate the inner product of two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The <see cref="System.Double" /> product of the two vectors.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are not equal.</exception>
        public static Double operator *(Vector first, Vector second)
        {
            return InnerProduct(first, second);
        }

        /// <summary>
        /// Multiplies the vector with a <see cref="Matrix" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The product of the vector and the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The vector is null.
        /// or
        /// The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The number of rows in the matrix is not 1.</exception>
        public static Matrix operator *(Vector vector, Matrix matrix)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), Messages.MatrixIsNull);

            if (matrix.NumberOfRows != 1)
                throw new ArgumentException(Messages.MatrixRowsMoreThan1, nameof(matrix));

            Matrix result = new Matrix(vector.Size, matrix.NumberOfColumns);
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = vector.values[rowIndex] * matrix[0, columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplies the <see cref="Matrix" /> with a vector.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The product of the vector and the matrix.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The vector is null.
        /// or
        /// The matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The size of the matrix does not match the size of the vector.</exception>
        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), Messages.MatrixIsNull);

            if (vector.Size != matrix.NumberOfColumns)
                throw new ArgumentException(Messages.MatrixSizeDoesNotMatchVector, nameof(vector));

            Vector result = new Vector(matrix.NumberOfRows);
            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    result.values[rowIndex] += matrix[rowIndex, columnIndex] * vector.values[columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Divides the vector with a <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The quotient of the vector and the scalar.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Vector operator /(Vector vector, Double scalar)
        {
            return (1 / scalar) * vector;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether the specified vector instances are equal.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns><c>true</c> if the vectors are the same size, and all values are equal; otherwise, <c>false</c>.</returns>
        public static Boolean AreEqual(Vector first, Vector second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;
            if (ReferenceEquals(first, second))
                return true;

            if (first.values.Length != second.values.Length)
                return false;

            return first.values.SequenceEqual(second.values);
        }

        /// <summary>
        /// Determines whether the specified vector is a zero vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns><c>true</c> if all values of the vector are zeros; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Boolean IsZero(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);

            return vector.values.All(value => value == 0);
        }

        /// <summary>
        /// Determines whether the specified vector is valid.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns><c>true</c> if all values of the vector are numbers; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The vector is null.</exception>
        public static Boolean IsValid(Vector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), Messages.VectorIsNull);

            return vector.values.All(value => !Double.IsNaN(value));
        }

        /// <summary>
        /// Calculates the inner product of two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector</param>
        /// <returns>The resulting scalars.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Double InnerProduct(Vector first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            if (first.Size != second.Size)
                throw new ArgumentException(Messages.VectorDimensionsAreDifferent, nameof(second));

            Double result = 0;
            for (Int32 valueIndex = 0; valueIndex < first.Size; valueIndex++)
            {
                result += first.values[valueIndex] * second.values[valueIndex];
            }

            return result;
        }

        /// <summary>
        /// Calculates the outer product (also known as direct product) of two vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector</param>
        /// <returns>The resulting <see cref="Matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first vector is null.
        /// or
        /// The second vector is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The dimensions of the two vectors are different.</exception>
        public static Matrix OuterProduct(Vector first, Vector second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), Messages.FirstVectorIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), Messages.SecondVectorIsNull);

            Matrix result = new Matrix(first.Size, second.Size);
            for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
            {
                for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                {
                    result[rowIndex, columnIndex] = first[rowIndex] * second[columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Normalizes the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector Normalize(Vector vector)
        {
            return new Vector(vector.Select(value => value /= vector.Length).ToArray());
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{Double}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Double> GetEnumerator()
        {
            for (Int32 valueIndex = 0; valueIndex < this.values.Length; valueIndex++)
            {
                yield return this.values[valueIndex];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the values of the instance.</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(this.values.Length * 8);
            builder.Append("(");
            for (Int32 valueIndex = 0; valueIndex < this.values.Length; valueIndex++)
            {
                if (valueIndex > 0)
                    builder.Append(' ');
                builder.Append(this.values[valueIndex].ToString(CultureInfo.InvariantCulture));
            }

            builder.Append(")");

            return builder.ToString();
        }

        #endregion
    }
}
