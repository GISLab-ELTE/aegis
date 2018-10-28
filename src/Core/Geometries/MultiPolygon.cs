// <copyright file="MultiPolygon.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a set of polygons in Cartesian coordinate space.
    /// </summary>
    public class MultiPolygon : GeometryList<IPolygon>, IMultiPolygon
    {
        /// <summary>
        /// The name of the multi polygon. This field is constant.
        /// </summary>
        private const String MultiPolygonName = "MULTIPOLYGON";

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygon" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        public MultiPolygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : base(precisionModel, referenceSystem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygon" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="source">The source of polygons.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public MultiPolygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IEnumerable<IPolygon> source)
            : base(precisionModel, referenceSystem, source)
        {
        }

        /// <summary>
        /// Gets the inherent dimension of the multi polygon.
        /// </summary>
        /// <value><c>2</c>, which is the defined dimension of a multi polygon.</value>
        public override sealed Int32 Dimension { get { return 2; } }

        /// <summary>
        /// Gets the area of the multi polygon.
        /// </summary>
        /// <value>The sum of polygon areas within the multi polygon.</value>
        public Double Area
        {
            get { return this.Sum(polygon => polygon.Area); }
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, MultiPolygonName);
        }
    }
}
