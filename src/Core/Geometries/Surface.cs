// <copyright file="Surface.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Geometries
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a surface geometry in Cartesian coordinate space.
    /// </summary>
    public abstract class Surface : Geometry, ISurface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Surface" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        protected Surface(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : base(precisionModel, referenceSystem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Surface" /> class.
        /// </summary>
        /// <param name="factory">The factory of the surface.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        protected Surface(IGeometryFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Gets the inherent dimension of the surface.
        /// </summary>
        /// <value><c>2</c>, which is the defined dimension of a surface.</value>
        public override sealed Int32 Dimension { get { return 2; } }

        /// <summary>
        /// Gets a value indicating whether the surface is convex.
        /// </summary>
        /// <value><c>true</c> if the surface is convex; otherwise, <c>false</c>.</value>
        public abstract Boolean IsConvex { get; }

        /// <summary>
        /// Gets a value indicating whether the surface is divided.
        /// </summary>
        /// <value><c>true</c> if the surface is divided; otherwise, <c>false</c>.</value>
        public abstract Boolean IsDivided { get; }

        /// <summary>
        /// Gets a value indicating whether the surface is whole.
        /// </summary>
        /// <value><c>true</c> if the surface is whole; otherwise, <c>false</c>.</value>
        public abstract Boolean IsWhole { get; }

        /// <summary>
        /// Gets the area of the surface.
        /// </summary>
        /// <value>The area of the surface.</value>
        public abstract Double Area { get; }

        /// <summary>
        /// Gets the perimeter of the surface.
        /// </summary>
        /// <value>The perimeter of the surface.</value>
        public abstract Double Perimeter { get; }
    }
}
