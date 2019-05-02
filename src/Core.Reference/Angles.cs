// <copyright file="Angles.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of the known geographic angles.
    /// </summary>
    public static class Angles
    {
        /// <summary>
        /// The equator.
        /// </summary>
        public static readonly Angle Equator = Angle.FromDegree(0);

        /// <summary>
        /// The north pole.
        /// </summary>
        public static readonly Angle NorthPole = Angle.FromDegree(90);

        /// <summary>
        /// The south pole.
        /// </summary>
        public static readonly Angle SouthPole = Angle.FromDegree(-90);

        /// <summary>
        /// The Arctic Circle.
        /// </summary>
        /// <remarks>
        /// The position of the Arctic Circle is not fixed; as of 25 February 2019, it runs 66°33'47.5" north of the Equator.
        /// </remarks>
        public static readonly Angle ArcticCircle = Angle.FromDegree(66, 33, 47.5);

        /// <summary>
        /// The Antarctic Circle.
        /// </summary>
        /// <remarks>
        /// The position of the Antarctic Circle is not fixed; as of 24 February 2019, it runs 66°33'47.5" south of the Equator.
        /// </remarks>
        public static readonly Angle AntarcticCircle = Angle.FromDegree(66, 33, 47.5);

        /// <summary>
        /// The Tropic of Cancer.
        /// </summary>
        /// <remarks>
        /// The position of the Tropic of Cancer is not fixed; as of 26 February 2019, it runs 23°26'12.5" north of the Equator.
        /// </remarks>
        public static readonly Angle TropicOfCancer = Angle.FromDegree(23, 26, 12.5);

        /// <summary>
        /// The Tropic of Capricorn.
        /// </summary>
        /// <remarks>
        /// The position of the Tropic of Capricorn is not fixed; as of 26 February 2019, it runs 23°26'12.5" south of the Equator.
        /// </remarks>
        public static readonly Angle TropicOfCapricorn = Angle.FromDegree(23, 26, 12.5);
    }
}
