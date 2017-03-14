// <copyright file="VectorFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Numerics.LinearAlgebra
{
    using System;
    using AEGIS.Numerics.Resources;

    /// <summary>
    /// Represents a factory type for the production of <see cref="Vector" /> instances.
    /// </summary>
    public static class VectorFactory
    {
        /// <summary>
        /// Creates a vector.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The produced vector.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The size of the vector is less than 0.</exception>
        public static Vector Create(Int32 size, Double defaultValue)
        {
            Vector vector = new Vector(size);

            for (Int32 index = 0; index < vector.Size; index++)
                vector[index] = defaultValue;

            return vector;
        }

        /// <summary>
        /// Creates a unit vector.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        /// <param name="unitIndex">The index of the unit value.</param>
        /// <returns>The produced unit vector.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The size is less than 1.
        /// or
        /// The unit index is less than 0.
        /// or
        /// The unit index is greater than or equal to the size.
        /// </exception>
        public static Vector CreateUnitVector(Int32 size, Int32 unitIndex)
        {
            if (size < 1)
                throw new ArgumentOutOfRangeException(nameof(size), NumericsMessages.SizeIsLessThan0);
            if (unitIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(unitIndex), NumericsMessages.UnitIndexIsLessThan0);
            if (unitIndex >= size)
                throw new ArgumentOutOfRangeException(nameof(unitIndex), NumericsMessages.UnitIndexIsGeaterThanSize);

            Vector unitVector = new Vector(size);
            unitVector[unitIndex] = 1;

            return unitVector;
        }
    }
}
