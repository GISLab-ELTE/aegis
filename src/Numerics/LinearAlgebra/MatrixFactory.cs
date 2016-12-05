// <copyright file="MatrixFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics.LinearAlgebra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a factory type for the production of <see cref="Matrix" /> instances.
    /// </summary>
    public static class MatrixFactory
    {
        /// <summary>
        /// Creates a generic matrix.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The generated matrix.</returns>
        /// <exception cref="ArgumentNullException">The collection of values is null.</exception>
        public static Matrix Create(Double[][] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (values.Length == 0)
                return new Matrix(0, 0);

            if (values.All(array => array == null))
                return new Matrix(values.Length, 0);

            Matrix matrix = new Matrix(values.Length, values.Max(array => array == null ? 0 : array.Length));

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                if (values[rowIndex] == null)
                    continue;

                for (Int32 columnIndex = 0; columnIndex < values[rowIndex].Length; columnIndex++)
                {
                    matrix[rowIndex, columnIndex] = values[rowIndex][columnIndex];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Creates a generic matrix.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The generated matrix.</returns>
        /// <exception cref="ArgumentNullException">The collection of values is null.</exception>
        public static Matrix Create(Int32[][] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (values.Length == 0)
                return new Matrix(0, 0);

            if (values.All(array => array == null))
                return new Matrix(values.Length, 0);

            Matrix matrix = new Matrix(values.Length, values.Max(array => array == null ? 0 : array.Length));

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                if (values[rowIndex] == null)
                    continue;

                for (Int32 columnIndex = 0; columnIndex < values[rowIndex].Length; columnIndex++)
                {
                    matrix[rowIndex, columnIndex] = values[rowIndex][columnIndex];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Creates a generic matrix.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The generated matrix.</returns>
        /// <exception cref="ArgumentNullException">The collection of values is null.</exception>
        public static Matrix Create(Double[,] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (values.Length == 0)
                return new Matrix(0, 0);

            return new Matrix(values);
        }

        /// <summary>
        /// Creates a generic matrix.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The generated matrix.</returns>
        /// <exception cref="ArgumentNullException">The collection of values is null.</exception>
        public static Matrix Create(Int32[,] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (values.Length == 0)
                return new Matrix(0, 0);

            Matrix matrix = new Matrix(values.GetLength(0), values.GetLength(1));

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    matrix[rowIndex, columnIndex] = values[rowIndex, columnIndex];
                }
            }

            return matrix;
        }

        /// <summary>
        /// Creates a generic matrix with the specified default value.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The generated matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static Matrix Create(Int32 numberOfRows, Int32 numberOfColumns, Double defaultValue)
        {
            Matrix matrix = new Matrix(numberOfRows, numberOfColumns);

            for (Int32 rowIndex = 0; rowIndex < matrix.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < matrix.NumberOfColumns; columnIndex++)
                {
                    matrix[rowIndex, columnIndex] = defaultValue;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Creates an identity matrix.
        /// </summary>
        /// <param name="size">The size of the matrix.</param>
        /// <returns>The produced identity matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The size is less than 0.</exception>
        public static Matrix CreateIdentity(Int32 size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), NumericsMessages.SizeIsLessThan0);

            return CreateIdentity(size, size);
        }

        /// <summary>
        /// Creates an identity matrix.
        /// </summary>
        /// <param name="numberOrRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <returns>The produced identity matrix.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        public static Matrix CreateIdentity(Int32 numberOrRows, Int32 numberOfColumns)
        {
            Matrix id = new Matrix(numberOrRows, numberOfColumns);
            for (Int32 valueIndex = 0; valueIndex < numberOrRows && valueIndex < numberOfColumns; valueIndex++)
                id[valueIndex, valueIndex] = 1;

            return id;
        }

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Matrix CreateDiagonal(params Double[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            Matrix diagonal = new Matrix(values.Length, values.Length);
            for (Int32 valueIndex = 0; valueIndex < values.Length; valueIndex++)
                diagonal[valueIndex, valueIndex] = values[valueIndex];

            return diagonal;
        }

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The list of values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Matrix CreateDiagonal(IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            Int32 count = values.Count();
            Matrix diagonal = new Matrix(count, count);

            Int32 valueIndex = 0;
            foreach (Double value in values)
            {
                diagonal[valueIndex, valueIndex] = value;
                valueIndex++;
            }

            return diagonal;
        }

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Matrix CreateDiagonal(params Int32[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            Matrix diagonal = new Matrix(values.Length, values.Length);
            for (Int32 valueIndex = 0; valueIndex < values.Length; valueIndex++)
                diagonal[valueIndex, valueIndex] = values[valueIndex];

            return diagonal;
        }

        /// <summary>
        /// Creates a diagonal matrix.
        /// </summary>
        /// <param name="values">The list of values to be inserted in the diagonal of the matrix.</param>
        /// <returns>The produced diagonal matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        public static Matrix CreateDiagonal(IEnumerable<Int32> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            Int32 count = values.Count();
            Matrix diagonal = new Matrix(count, count);

            Int32 valueIndex = 0;
            foreach (Double value in values)
            {
                diagonal[valueIndex, valueIndex] = value;
                valueIndex++;
            }

            return diagonal;
        }

        /// <summary>
        /// Creates a square matrix.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The produced square matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The number of values is not a square number.</exception>
        public static Matrix CreateSquare(params Double[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            if (Math.Sqrt(values.Length) != Math.Floor(Math.Sqrt(values.Length)))
                throw new ArgumentException(NumericsMessages.NumberOfValuesIsNotSquare, nameof(values));

            Int32 size = Convert.ToInt32(Math.Sqrt(values.Length));
            Matrix matrix = new Matrix(size, size);
            for (Int32 valueIndex = 0; valueIndex < values.Length; valueIndex++)
            {
                matrix[valueIndex / size, valueIndex % size] = values[valueIndex];
            }

            return matrix;
        }

        /// <summary>
        /// Creates a square matrix.
        /// </summary>
        /// <param name="values">The list of values.</param>
        /// <returns>The produced square matrix.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of values is null.</exception>
        /// <exception cref="System.ArgumentException">The number of values is not a square number.</exception>
        public static Matrix CreateSquare(IEnumerable<Double> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), NumericsMessages.ValueCollectionIsNull);

            Int32 count = values.Count();
            if (Math.Sqrt(count) != Math.Floor(Math.Sqrt(count)))
                throw new ArgumentException(NumericsMessages.NumberOfValuesIsNotSquare, nameof(values));

            Int32 size = Convert.ToInt32(Math.Sqrt(count));
            Matrix matrix = new Matrix(size, size);

            Int32 valueIndex = 0;
            foreach (Double value in values)
            {
                matrix[valueIndex / size, valueIndex % size] = value;
                valueIndex++;
            }

            return matrix;
        }
    }
}
