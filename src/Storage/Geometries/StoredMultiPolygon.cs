// <copyright file="StoredMultiPolygon.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;

    /// <summary>
    /// Represents a multi polygon located in a store.
    /// </summary>
    public class StoredMultiPolygon : StoredGeometryCollection<IPolygon>, IMultiPolygon
    {
        /// <summary>
        /// The name of the multi point. This field is constant.
        /// </summary>
        private const String MultiPointName = "MULTIPOLYGON";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredMultiPolygon" /> class.
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
        public StoredMultiPolygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, referenceSystem, driver, identifier, indexes)
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
        public Double Area { get { return this.Sum(polygon => polygon.Area); } }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, MultiPointName);
        }
    }
}
