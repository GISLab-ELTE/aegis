// <copyright file="Angles.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of the known geographic angles.
    /// </summary>
    public static class Angles
    {
        #region Private static fields

        /// <summary>
        /// The equator.
        /// </summary>
        private static Angle? equator;

        /// <summary>
        /// The north pole.
        /// </summary>
        private static Angle? northPole;

        /// <summary>
        /// The south pole.
        /// </summary>
        private static Angle? southPole;

        /// <summary>
        /// The arctic circle.
        /// </summary>
        private static Angle? arcticCircle;

        /// <summary>
        /// The antarctic circle.
        /// </summary>
        private static Angle? antarcticCircle;

        /// <summary>
        /// The tropic of cancer.
        /// </summary>
        private static Angle? tropicOfCancer;

        /// <summary>
        /// The tropic of Capricorn.
        /// </summary>
        private static Angle? tropicOfCapricorn;

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets the equator.
        /// </summary>
        public static Angle Equator { get { return (equator ?? (equator = Angle.FromDegree(0))).Value; } }

        /// <summary>
        /// Gets the north pole.
        /// </summary>
        public static Angle NorthPole { get { return (northPole ?? (northPole = Angle.FromDegree(90))).Value; } }

        /// <summary>
        /// Gets the south pole.
        /// </summary>
        public static Angle SouthPole { get { return (southPole ?? (southPole = Angle.FromDegree(-90))).Value; } }

        /// <summary>
        /// Gets the arctic circle.
        /// </summary>
        public static Angle ArcticCircle { get { return (arcticCircle ?? (arcticCircle = Angle.FromDegree(66.5622))).Value; } }

        /// <summary>
        /// Gets the antarctic circle.
        /// </summary>
        public static Angle AntarcticCircle { get { return (antarcticCircle ?? (antarcticCircle = Angle.FromDegree(-66.5622))).Value; } }

        /// <summary>
        /// Gets the tropic of cancer.
        /// </summary>
        public static Angle TropicOfCancer { get { return (tropicOfCancer ?? (tropicOfCancer = Angle.FromDegree(23.43777778))).Value; } }

        /// <summary>
        /// Gets the tropic of Capricorn.
        /// </summary>
        public static Angle TropicOfCapricorn { get { return (tropicOfCapricorn ?? (tropicOfCapricorn = Angle.FromDegree(-23.43777778))).Value; } }

        #endregion
    }
}
