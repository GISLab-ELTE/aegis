// <copyright file="StoredMultiLineString.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a multi line string located in a store.
    /// </summary>
    public class StoredMultiLineString : StoredGeometryCollection<ILineString>, IMultiLineString
    {
        /// <summary>
        /// The name of the multi line strings. This field is constant.
        /// </summary>
        private const String MultiLineStringName = "MULTILINESTRING";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredMultiLineString" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredMultiLineString(PrecisionModel precisionModel, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, driver, identifier, indexes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredMultiLineString" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredMultiLineString(StoredGeometryFactory factory, String identifier, IEnumerable<Int32> indexes)
            : base(factory, identifier, indexes)
        {
        }

        /// <summary>
        /// Gets the inherent dimension of the multi line string.
        /// </summary>
        /// <value><c>1</c>, which is the defined dimension of a multi line string.</value>
        public override sealed Int32 Dimension { get { return 1; } }

        /// <summary>
        /// Gets a value indicating whether the multi line string is simple.
        /// </summary>
        /// <value><c>true</c> if all line string within the multi line string are simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple { get { return this.All(lineString => lineString.IsSimple); } }

        /// <summary>
        /// Gets a value indicating whether the multi line string is closed.
        /// </summary>
        /// <value><c>true</c> if all curves within the multi line string are closed; otherwise, <c>false</c>.</value>
        public virtual Boolean IsClosed { get { return this.All(lineString => lineString.IsClosed); } }

        /// <summary>
        /// Gets the length of the multi line string.
        /// </summary>
        /// <value>The length of the multi line string.</value>
        public virtual Double Length { get { return this.Sum(lineString => lineString.Length); } }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, MultiLineStringName);
        }
    }
}
