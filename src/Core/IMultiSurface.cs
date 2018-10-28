// <copyright file="IMultiSurface.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;

    /// <summary>
    /// Defines behavior for multi surface geometries.
    /// </summary>
    /// <typeparam name="T">The type of the surface.</typeparam>
    public interface IMultiSurface<out T> : IGeometryCollection<T>
        where T : ISurface
    {
        /// <summary>
        /// Gets the area of the multi surface.
        /// </summary>
        /// <value>The sum of areas within the multi surface.</value>
        Double Area { get; }
    }
}
