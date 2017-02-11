// <copyright file="Matrix.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a matrix in Euclidean space.
    /// </summary>
    public class Matrix : IEnumerable<Double>
    {
        /// <summary>
        /// The version of the matrix.
        /// </summary>
        private Int32 version;

        /// <summary>
        /// The values stored in row major order.
        /// </summary>
        private Double[] values;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public Matrix(Int32 numberOfRows, Int32 numberOfColumns)
        {
            if (numberOfRows < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfRows), NumericsMessages.NumberOfRowsIsLessThan0);
            if (numberOfColumns < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfColumns), NumericsMessages.NumberOfColumnsIsLessThan0);

            this.NumberOfRows = numberOfRows;
            this.NumberOfColumns = numberOfColumns;
            this.values = new Double[this.NumberOfRows * this.NumberOfColumns];
            this.version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" /> class.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public Matrix(Double[,] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), NumericsMessages.SourceIsNull);

            this.NumberOfRows = source.GetLength(0);
            this.NumberOfColumns = source.GetLength(1);
            this.values = new Double[this.NumberOfRows * this.NumberOfColumns];
            this.version = 0;

            for (Int32 rowIndex = 0; rowIndex < source.GetLength(0); rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < source.GetLength(1); columnIndex++)
                {
                    this.values[rowIndex * this.NumberOfColumns + columnIndex] = source[rowIndex, columnIndex];
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix" /> class based on the other matrix.
        /// </summary>
        /// <param name="other">The other matrix.</param>
        /// <exception cref="System.ArgumentNullException">The other matrix is null.</exception>
        public Matrix(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), NumericsMessages.OtherMatrixIsNull);

            this.NumberOfRows = other.NumberOfRows;
            this.NumberOfColumns = other.NumberOfColumns;
            this.values = new Double[this.NumberOfRows * this.NumberOfColumns];
            this.version = 0;

            Array.Copy(other.values, this.values, this.values.Length);
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        public Int32 NumberOfRows { get; private set; }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The number of columns.</value>
        public Int32 NumberOfColumns { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the matrix is square.
        /// </summary>
        /// <value><c>true</c> if the matrix is square; otherwise, <c>false</c>.</value>
        public Boolean IsSquare { get { return this.NumberOfRows == this.NumberOfColumns; } }

        /// <summary>
        /// Gets the trace of the matrix.
        /// </summary>
        /// <value>The sum of elements on the main diagonal of the matrix.</value>
        /// <exception cref="System.InvalidOperationException">The matrix must be square to have a trace.</exception>
        public Double Trace
        {
            get
            {
                if (!this.IsSquare)
                    return Double.NaN;

                Double trace = 0;
                for (Int32 rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
                {
                    trace += this.values[rowIndex * this.NumberOfColumns + rowIndex];
                }

                return trace;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Double" /> value located at the specified row and column indexes.
        /// </summary>
        /// <value>The <see cref="Double" /> value located at the specified row and column indexes.</value>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The <see cref="Double" /> value at the specified index.</returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Row index was outside the bounds of the matrix.
        /// or
        /// Column index was outside the bounds of the matrix.
        /// </exception>
        public Double this[Int32 rowIndex, Int32 columnIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= this.NumberOfRows)
                    throw new ArgumentOutOfRangeException(nameof(rowIndex), NumericsMessages.RowIndexOutsideMatrixBounds);
                if (columnIndex < 0 || columnIndex >= this.NumberOfColumns)
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), NumericsMessages.ColumnIndexOutsideMatrixBounds);

                return this.values[rowIndex * this.NumberOfColumns + columnIndex];
            }

            set
            {
                if (rowIndex < 0 || rowIndex >= this.NumberOfRows)
                    throw new ArgumentOutOfRangeException(nameof(rowIndex), NumericsMessages.RowIndexOutsideMatrixBounds);
                if (columnIndex < 0 || columnIndex >= this.NumberOfColumns)
                    throw new ArgumentOutOfRangeException(nameof(columnIndex), NumericsMessages.ColumnIndexOutsideMatrixBounds);

                if (this.values[rowIndex * this.NumberOfColumns + columnIndex] == value)
                    return;

                this.values[rowIndex * this.NumberOfColumns + columnIndex] = value;
                this.version++;
            }
        }

        /// <summary>
        /// Sums the specified <see cref="Matrix" /> instances.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns>The sum of the two <see cref="Matrix" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first matrix is null.
        /// or
        /// The second matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of rows does not match.
        /// or
        /// The number of columns does not match.
        /// </exception>
        public static Matrix operator +(Matrix first, Matrix second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstMatrixIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondMatrixIsNull);

            if (first.NumberOfRows != second.NumberOfRows)
                throw new ArgumentException(NumericsMessages.NumberOfRowsDoesNotMatch, nameof(second));
            if (first.NumberOfColumns != second.NumberOfColumns)
                throw new ArgumentException(NumericsMessages.NumberOfColumnsDoesNotMatch, nameof(second));

            Matrix result = new Matrix(first.NumberOfRows, first.NumberOfColumns);

            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = first[rowIndex, columnIndex] + second[rowIndex, columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the specified <see cref="Matrix" /> instances.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns>The extract of the two <see cref="Matrix" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first matrix is null.
        /// or
        /// The second matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The number of rows does not match.
        /// or
        /// The number of columns does not match.
        /// </exception>
        public static Matrix operator -(Matrix first, Matrix second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstMatrixIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondMatrixIsNull);

            if (first.NumberOfRows != second.NumberOfRows)
                throw new ArgumentException(NumericsMessages.NumberOfRowsDoesNotMatch, nameof(second));
            if (first.NumberOfColumns != second.NumberOfColumns)
                throw new ArgumentException(NumericsMessages.NumberOfColumnsDoesNotMatch, nameof(second));

            Matrix result = new Matrix(first.NumberOfRows, first.NumberOfColumns);

            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = first[rowIndex, columnIndex] - second[rowIndex, columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Inverts all values of the specified <see cref="Matrix" />.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The <see cref="Matrix" /> with the inverted values.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix operator -(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            Matrix result = new Matrix(matrix.NumberOfRows, matrix.NumberOfColumns);
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = -matrix[rowIndex, columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplies the <see cref="System.Double" /> scalar with the specified <see cref="Matrix" />.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The scalar multiplication of the <see cref="Matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix operator *(Double scalar, Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            Matrix result = new Matrix(matrix.NumberOfRows, matrix.NumberOfColumns);
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = scalar * matrix[rowIndex, columnIndex];
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplies the specified <see cref="Matrix" /> with the <see cref="System.Double" /> scalar.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar multiplication of the <see cref="Matrix" />.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Matrix operator *(Matrix matrix, Double scalar)
        {
            return scalar * matrix; // the operation is commutative
        }

        /// <summary>
        /// Multiplies the specified <see cref="Matrix" /> instances.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns>The product of the specified <see cref="Matrix" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first matrix is null.
        /// or
        /// The second matrix is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of columns in the first matrix does not match the number of rows in the second matrix.</exception>
        public static Matrix operator *(Matrix first, Matrix second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first), NumericsMessages.FirstMatrixIsNull);
            if (second == null)
                throw new ArgumentNullException(nameof(second), NumericsMessages.SecondMatrixIsNull);

            if (first.NumberOfColumns != second.NumberOfRows)
                throw new ArgumentException(NumericsMessages.NumberOfRowsDoesNotMatchColumnsMatrix, nameof(second));

            Matrix result = new Matrix(first.NumberOfRows, second.NumberOfColumns);
            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    for (Int32 k = 0; k < first.NumberOfColumns; k++)
                    {
                        result[rowIndex, columnIndex] += first[rowIndex, k] * second[k, columnIndex];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Performs an explicit conversion of a single column or single row <see cref="Matrix" /> to a <see cref="Vector" />.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The result of the conversion.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        /// <exception cref="System.ArgumentException">The matrix is neither single column, nor single row.</exception>
        public static explicit operator Vector(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            if (matrix.NumberOfColumns != 1 && matrix.NumberOfRows != 1)
                throw new ArgumentException(NumericsMessages.MatrixNotSingleColumnOrRow, nameof(matrix));

            Vector result;
            if (matrix.NumberOfColumns == 1)
            {
                result = new Vector(matrix.NumberOfRows);
                for (Int32 index = 0; index < matrix.NumberOfRows; index++)
                    result[index] = matrix[index, 0];
            }
            else
            {
                result = new Vector(matrix.NumberOfColumns);
                for (Int32 index = 0; index < matrix.NumberOfColumns; index++)
                    result[index] = matrix[0, index];
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified matrix instances are equal.
        /// </summary>
        /// <param name="first">The first matrix.</param>
        /// <param name="second">The second matrix.</param>
        /// <returns><c>true</c> if the matrices are the same size, and all values are equal; otherwise, <c>false</c>.</returns>
        public static Boolean AreEqual(Matrix first, Matrix second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;
            if (ReferenceEquals(first, second))
                return true;

            if (first.NumberOfRows != second.NumberOfRows || first.NumberOfColumns != second.NumberOfColumns)
                return false;

            return first.values.SequenceEqual(second.values);
        }

        /// <summary>
        /// Determines whether the specified matrix is a zero matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values of the matrix are zeros; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsZero(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            if (matrix.NumberOfColumns == 0 && matrix.NumberOfRows == 0)
                return true;

            return matrix.All(value => value == 0);
        }

        /// <summary>
        /// Determines whether the specified matrix is valid.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns><c>true</c> if all values of the matrix are numbers; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The matrix is null.</exception>
        public static Boolean IsValid(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix), NumericsMessages.MatrixIsNull);

            if (matrix.NumberOfColumns == 0 && matrix.NumberOfRows == 0)
                return true;

            return matrix.All(value => !Double.IsNaN(value));
        }

        /// <summary>
        /// Returns the specified row of the matrix.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>The array of values in the specified row.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Row index was outside the bounds of the matrix.</exception>
        public Double[] GetRow(Int32 rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= this.NumberOfRows)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), NumericsMessages.RowIndexOutsideMatrixBounds);

            Double[] row = new Double[this.NumberOfColumns];

            Array.Copy(this.values, rowIndex * this.NumberOfColumns, row, 0, this.NumberOfColumns);

            return row;
        }

        /// <summary>
        /// Returns the specified column of the matrix.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The array of values in the specified column.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Column index was outside the bounds of the matrix.</exception>
        public Double[] GetColumn(Int32 columnIndex)
        {
            if (columnIndex < 0 || this.NumberOfRows == 0 || columnIndex >= this.NumberOfColumns)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), NumericsMessages.ColumnIndexOutsideMatrixBounds);

            Double[] column = new Double[this.NumberOfRows];

            for (Int32 rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
                column[rowIndex] = this.values[rowIndex * this.NumberOfColumns + columnIndex];

            return column;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{Double}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Double> GetEnumerator()
        {
            for (Int32 rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
                {
                    yield return this.values[rowIndex * this.NumberOfColumns + columnIndex];
                }
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

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the values of the instance.</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder(this.NumberOfColumns * this.NumberOfRows * 8);
            builder.Append("(");
            for (Int32 rowIndex = 0; rowIndex < this.NumberOfRows; rowIndex++)
            {
                if (rowIndex > 0)
                    builder.Append("; ");
                for (Int32 columnIndex = 0; columnIndex < this.NumberOfColumns; columnIndex++)
                {
                    if (columnIndex > 0)
                        builder.Append(' ');
                    builder.Append(this.values[rowIndex * this.NumberOfColumns + columnIndex].ToString(CultureInfo.InvariantCulture));
                }
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
