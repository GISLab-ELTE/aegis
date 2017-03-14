// <copyright file="LocalEllipsoidCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="Ellipsoid" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalEllipsoidCollection : LocalReferenceCollection<Ellipsoid>
    {
        /// <summary>
        /// The collection of <see cref="UnitOfMeasurement" /> instances.
        /// </summary>
        private readonly IReferenceCollection<UnitOfMeasurement> unitCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalEllipsoidCollection" /> class.
        /// </summary>
        /// <param name="unitCollection">The unit of measurement collection.</param>
        /// <exception cref="System.ArgumentNullException">The unit of measurement collection is null.</exception>
        public LocalEllipsoidCollection(IReferenceCollection<UnitOfMeasurement> unitCollection)
        {
            if (unitCollection == null)
                throw new ArgumentNullException(nameof(unitCollection), ReferenceMessages.UnitOfMeasurementCollectionIsNull);

            this.unitCollection = unitCollection;
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override Ellipsoid Convert(String[] content)
        {
            if (!String.IsNullOrEmpty(content[4]))
            {
                // inverse flattening is available
                return Ellipsoid.FromInverseFlattening(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                       content[7], this.GetAliases(Int32.Parse(content[0])),
                                                       new Length(Double.Parse(content[2]), this.unitCollection[Authority, Int32.Parse(content[3])]),
                                                       Double.Parse(content[4]));
            }
            else
            {
                // semi-major axis is available
                return Ellipsoid.FromSemiMinorAxis(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                   content[7], this.GetAliases(Int32.Parse(content[0])),
                                                   new Length(Double.Parse(content[2]), this.unitCollection[Authority, Int32.Parse(content[3])]),
                                                   new Length(Double.Parse(content[5]), this.unitCollection[Authority, Int32.Parse(content[3])]));
            }
        }
    }
}
