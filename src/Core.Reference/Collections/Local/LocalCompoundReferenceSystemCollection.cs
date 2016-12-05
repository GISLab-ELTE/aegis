// <copyright file="LocalCompoundReferenceSystemCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of <see cref="CompoundReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalCompoundReferenceSystemCollection : LocalReferenceCollection<CompoundReferenceSystem>
    {
        /// <summary>
        /// The name of the resource. This field is constant.
        /// </summary>
        private const String ResourceName = "CoordinateReferenceSystem";

        /// <summary>
        /// The name of the alias type. This field is constant.
        /// </summary>
        private const String AliasTypeName = "Coordinate Reference System";

        /// <summary>
        /// The collection of  <see cref="AreaOfUse" /> instances.
        /// </summary>
        private IReferenceCollection<AreaOfUse> areaOfUseCollection;

        /// <summary>
        /// The collection of  <see cref="ReferenceSystem" /> instances.
        /// </summary>
        private IReferenceCollection<ReferenceSystem> referenceSystemCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCompoundReferenceSystemCollection" /> class.
        /// </summary>
        /// <param name="areaOfUseCollection">The area of use collection.</param>
        /// <param name="referenceSystemCollection">The reference system collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The area of use collection is null.
        /// or
        /// The reference system collection is null.
        /// </exception>
        public LocalCompoundReferenceSystemCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<ReferenceSystem> referenceSystemCollection)
            : base(ResourceName, AliasTypeName)
        {
            if (areaOfUseCollection == null)
                throw new ArgumentNullException(nameof(areaOfUseCollection), ReferenceMessages.AreaOfUseCollectionIsNull);
            if (referenceSystemCollection == null)
                throw new ArgumentNullException(nameof(referenceSystemCollection), ReferenceMessages.ReferenceSystemCollectionIsNull);

            this.areaOfUseCollection = areaOfUseCollection;
            this.referenceSystemCollection = referenceSystemCollection;
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override CompoundReferenceSystem Convert(String[] content)
        {
            switch (content[3])
            {
                case "compound":
                    return new CompoundReferenceSystem(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                       content[11], this.GetAliases(Int32.Parse(content[0])), content[10],
                                                       this.areaOfUseCollection[Authority, Int32.Parse(content[2])],
                                                       this.referenceSystemCollection[Authority, Int32.Parse(content[8])],
                                                       this.referenceSystemCollection[Authority, Int32.Parse(content[9])]);
                default:
                    return null;
            }
        }
    }
}
