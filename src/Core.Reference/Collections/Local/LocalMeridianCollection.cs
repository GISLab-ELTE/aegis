// <copyright file="LocalMeridianCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="Meridian" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalMeridianCollection : LocalReferenceCollection<Meridian>
    {
        /// <summary>
        /// The name of the resource. This field is constant.
        /// </summary>
        private const String ResourceName = "PrimeMeridian";

        /// <summary>
        /// The name of the alias type. This field is constant.
        /// </summary>
        private const String AliasName = "Prime Meridian";

        /// <summary>
        /// The collection of <see cref="UnitOfMeasurement" /> instances.
        /// </summary>
        private readonly IReferenceCollection<UnitOfMeasurement> unitCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalMeridianCollection" /> class.
        /// </summary>
        /// <param name="unitCollection">The unit of measurement collection.</param>
        /// <exception cref="System.ArgumentNullException">The unit of measurement collection is null.</exception>
        public LocalMeridianCollection(IReferenceCollection<UnitOfMeasurement> unitCollection)
            : base(ResourceName, AliasName)
        {
            if (unitCollection == null)
                throw new ArgumentNullException(nameof(unitCollection), ReferenceMessages.UnitOfMeasurementCollectionIsNull);

            this.unitCollection = unitCollection;
        }

        /// <summary>
        /// Returns a collection with items matching a specified longitude.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <returns>A collection containing the items that match the specified longitude.</returns>
        public IEnumerable<Meridian> WithLongitude(Angle longitude)
        {
            return this.GetReferences().Where(meridian => meridian.Longitude == longitude);
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override Meridian Convert(String[] content)
        {
            return new Meridian(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                content[4], this.GetAliases(Int32.Parse(content[0])),
                                new Angle(Double.Parse(content[2]), this.unitCollection[Authority, Int32.Parse(content[3])]));
        }
    }
}
