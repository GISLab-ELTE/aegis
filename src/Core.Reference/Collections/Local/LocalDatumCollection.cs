// <copyright file="LocalDatumCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="Datum" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalDatumCollection : LocalReferenceCollection<Datum>
    {
        /// <summary>
        /// The collection of  <see cref="AreaOfUse" /> instances.
        /// </summary>
        private IReferenceCollection<AreaOfUse> areaOfUseCollection;

        /// <summary>
        /// The collection of  <see cref="Ellipsoid" /> instances.
        /// </summary>
        private IReferenceCollection<Ellipsoid> ellipsoidCollection;

        /// <summary>
        /// The collection of  <see cref="Meridian" /> instances.
        /// </summary>
        private IReferenceCollection<Meridian> meridianCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDatumCollection" /> class.
        /// </summary>
        /// <param name="areaOfUseCollection">The area of use collection.</param>
        /// <param name="ellipsoidCollection">The ellipsoid collection.</param>
        /// <param name="meridianCollection">The meridian collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The area of use collection is null.
        /// or
        /// The ellipsoid collection is null.
        /// or
        /// The meridian collection is null.
        /// </exception>
        public LocalDatumCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<Ellipsoid> ellipsoidCollection, IReferenceCollection<Meridian> meridianCollection)
        {
            this.areaOfUseCollection = areaOfUseCollection ?? throw new ArgumentNullException(nameof(areaOfUseCollection));
            this.ellipsoidCollection = ellipsoidCollection ?? throw new ArgumentNullException(nameof(ellipsoidCollection));
            this.meridianCollection = meridianCollection ?? throw new ArgumentNullException(nameof(meridianCollection));
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override Datum Convert(String[] content)
        {
            switch (content[2])
            {
                case "geodetic":
                    return new GeodeticDatum(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                             content[9], this.GetAliases(Int32.Parse(content[0])),
                                             content[3], content[4], content[8],
                                             this.areaOfUseCollection[Authority, Int32.Parse(content[7])],
                                             this.ellipsoidCollection[Authority, Int32.Parse(content[5])],
                                             this.meridianCollection[Authority, Int32.Parse(content[6])]);
                case "vertical":
                    return new VerticalDatum(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                             content[9], this.GetAliases(Int32.Parse(content[0])),
                                             content[3], content[4], content[8],
                                             this.areaOfUseCollection[Authority, Int32.Parse(content[7])]);
                default:
                    return null;
            }
        }
    }
}
