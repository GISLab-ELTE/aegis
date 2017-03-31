// <copyright file="StoredMultiPoint.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a multi point located in a store.
    /// </summary>
    public class StoredMultiPoint : StoredGeometryCollection<IPoint>, IMultiPoint
    {
        /// <summary>
        /// The name of the multi point. This field is constant.
        /// </summary>
        private const String MultiPointName = "MULTIPOINT";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredMultiPoint" /> class.
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
        public StoredMultiPoint(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, referenceSystem, driver, identifier, indexes)
        {
        }

        /// <summary>
        /// Gets the inherent dimension of the multi point.
        /// </summary>
        /// <value><c>0</c>, which is the defined dimension of a multi point.</value>
        public override sealed Int32 Dimension { get { return 0; } }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple
        {
            get
            {
                HashSet<Coordinate> hashSet = new HashSet<Coordinate>();
                for (Int32 geometryIndex = 0; geometryIndex < this.Count; geometryIndex++)
                {
                    if (hashSet.Contains(this[geometryIndex].Coordinate))
                        return false;
                    hashSet.Add(this[geometryIndex].Coordinate);
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the coordinates of the multi point.
        /// </summary>
        /// <value>The read-only list of coordinates.</value>
        public IReadOnlyList<Coordinate> Coordinates
        {
            get { return this.Driver.ReadCoordinates(this.Identifier); }
        }

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
