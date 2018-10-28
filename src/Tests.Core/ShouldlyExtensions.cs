// <copyright file="ShouldlyExtensions.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Shouldly;

    /// <summary>
    /// Provides extensions to assertion.
    /// </summary>
    public static class ShouldlyExtensions
    {
        /// <summary>
        /// Asserts whether a coordinate collection matches the expected collection within the specified tolerance.
        /// </summary>
        /// <param name="actual">The actual collection of coordinates.</param>
        /// <param name="expected">The expected collection of coordinates.</param>
        /// <param name="tolerance">The tolerance.</param>
        public static void ShouldBe(this IEnumerable<Coordinate> actual, IEnumerable<Coordinate> expected, Double tolerance)
        {
            if (actual == null && expected == null)
                return;

            if (actual == null)
            {
                expected.ShouldBeEmpty();
                return;
            }

            if (expected == null)
            {
                actual.ShouldBeEmpty();
                return;
            }

            actual.Count().ShouldBe(expected.Count());

            IEnumerator<Coordinate> actualEnumerator = actual.GetEnumerator();
            IEnumerator<Coordinate> expectedEnumerator = expected.GetEnumerator();

            while (actualEnumerator.MoveNext() && expectedEnumerator.MoveNext())
            {
                actualEnumerator.Current.X.ShouldBe(expectedEnumerator.Current.X, tolerance);
                actualEnumerator.Current.Y.ShouldBe(expectedEnumerator.Current.Y, tolerance);
                actualEnumerator.Current.Z.ShouldBe(expectedEnumerator.Current.Z, tolerance);
            }
        }

        /// <summary>
        /// Asserts whether a collection matches the expected collection within the specified tolerance.
        /// </summary>
        /// <param name="actual">The actual collection of coordinate collections.</param>
        /// <param name="expected">The expected collection of coordinate collections.</param>
        /// <param name="tolerance">The tolerance.</param>
        public static void ShouldBe(this IEnumerable<IEnumerable<Coordinate>> actual, IEnumerable<IEnumerable<Coordinate>> expected, Double tolerance)
        {
            if (actual == null && expected == null)
                return;

            if (actual == null)
            {
                expected.ShouldBeEmpty();
                return;
            }

            if (expected == null)
            {
                actual.ShouldBeEmpty();
                return;
            }

            actual.Count().ShouldBe(expected.Count());

            IEnumerator<IEnumerable<Coordinate>> actualEnumerator = actual.GetEnumerator();
            IEnumerator<IEnumerable<Coordinate>> expectedEnumerator = expected.GetEnumerator();

            while (actualEnumerator.MoveNext() && expectedEnumerator.MoveNext())
            {
                actualEnumerator.Current.ShouldBe(expectedEnumerator.Current, tolerance);
            }
        }
    }
}
