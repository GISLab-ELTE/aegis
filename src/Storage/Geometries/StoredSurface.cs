// <copyright file="StoredSurface.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a surface geometry located in a store.
    /// </summary>
    public abstract class StoredSurface : StoredGeometry, ISurface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoredSurface" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        protected StoredSurface(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, referenceSystem, driver, identifier, indexes)
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
