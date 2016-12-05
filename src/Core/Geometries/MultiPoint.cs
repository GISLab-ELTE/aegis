// <copyright file="MultiPoint.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;

    /// <summary>
    /// Represents a set of points in Cartesian coordinate space.
    /// </summary>
    public class MultiPoint : GeometryList<IPoint>, IMultiPoint
    {
        /// <summary>
        /// The name of the multi point. This field is constant.
        /// </summary>
        private const String MultiPointName = "MULTIPOINT";

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        public MultiPoint(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : base(precisionModel, referenceSystem)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="source">The source of points.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public MultiPoint(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IEnumerable<IPoint> source)
            : base(precisionModel, referenceSystem, source)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="factory">The factory of the multi point.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public MultiPoint(IGeometryFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="factory">The factory of the multi point.</param>
        /// <param name="source">The source of points.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The source is null.
        /// </exception>
        public MultiPoint(IGeometryFactory factory, IEnumerable<IPoint> source)
            : base(factory, source)
        {
        }

        /// <summary>
        /// Gets the inherent dimension of the multi point.
        /// </summary>
        /// <value><c>0</c>, which is the defined dimension of a multi point.</value>
        public override sealed Int32 Dimension { get { return 0; } }

        /// <summary>
        /// Gets a value indicating whether the multi point is simple.
        /// </summary>
        /// <value><c>true</c> if all points are distinct; otherwise, <c>false</c>.</value>
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