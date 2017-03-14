// <copyright file="LocalVerticalDatumCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="VerticalDatum" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalVerticalDatumCollection : LocalReferenceCollection<VerticalDatum>, IEnumerable<VerticalDatum>
    {
        /// <summary>
        /// The name of the resource. This field is constant.
        /// </summary>
        private const String ResourceName = "Datum";

        /// <summary>
        /// The collection of  <see cref="AreaOfUse" /> instances.
        /// </summary>
        private IReferenceCollection<AreaOfUse> areaOfUseCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalVerticalDatumCollection" /> class.
        /// </summary>
        /// <param name="areaOfUseCollection">The area of use collection.</param>
        /// <exception cref="System.ArgumentNullException">The area of use collection is null.</exception>
        public LocalVerticalDatumCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection)
            : base(ResourceName, ResourceName)
        {
            if (areaOfUseCollection == null)
                throw new ArgumentNullException(nameof(areaOfUseCollection), ReferenceMessages.AreaOfUseCollectionIsNull);

            this.areaOfUseCollection = areaOfUseCollection;
        }

        /// <summary>
        /// Returns a collection with items with the specified area of use.
        /// </summary>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are with the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public IEnumerable<VerticalDatum> WithArea(AreaOfUse area)
        {
            return this.GetReferences().Where(datum => datum.AreaOfUse.Equals(area));
        }

        /// <summary>
        /// Returns a collection with items within the specified area of use.
        /// </summary>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are within the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public IEnumerable<VerticalDatum> WithinArea(AreaOfUse area)
        {
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return this.GetReferences().Where(datum => datum.AreaOfUse.Within(area));
        }

        /// <summary>
        /// Returns a collection with items with the specified type.
        /// </summary>
        /// <param name="type">The type of the datum.</param>
        /// <returns>A collection containing the items that are with the specified type.</returns>
        public IEnumerable<VerticalDatum> WithType(VerticalDatumType type)
        {
            return this.GetReferences().Where(datum => datum.Type.Equals(type));
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override VerticalDatum Convert(String[] content)
        {
            switch (content[2])
            {
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
