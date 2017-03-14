// <copyright file="LocalVerticalCoordinateReferenceSystemCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of <see cref="VerticalCoordinateReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalVerticalCoordinateReferenceSystemCollection : LocalReferenceCollection<VerticalCoordinateReferenceSystem>
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
        private IReferenceCollection<AreaOfUse> areaOfUserCollection;

        /// <summary>
        /// The collection of  <see cref="CoordinateSystem" /> instances.
        /// </summary>
        private IReferenceCollection<CoordinateSystem> coordinateSystemCollection;

        /// <summary>
        /// The collection of  <see cref="VerticalDatum" /> instances.
        /// </summary>
        private IReferenceCollection<VerticalDatum> verticalDatumCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalVerticalCoordinateReferenceSystemCollection" /> class.
        /// </summary>
        /// <param name="areaOfUseCollection">The area of use collection.</param>
        /// <param name="coordinateSystemCollection">The coordinate system collection.</param>
        /// <param name="verticalDatumCollection">The vertical datum collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The area of use collection is null.
        /// or
        /// The coordinate system collection is null.
        /// or
        /// The datum collection is null.
        /// </exception>
        public LocalVerticalCoordinateReferenceSystemCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<CoordinateSystem> coordinateSystemCollection, IReferenceCollection<VerticalDatum> verticalDatumCollection)
            : base(ResourceName, AliasTypeName)
        {
            if (areaOfUseCollection == null)
                throw new ArgumentNullException(nameof(areaOfUseCollection), ReferenceMessages.AreaOfUseCollectionIsNull);
            if (coordinateSystemCollection == null)
                throw new ArgumentNullException(nameof(coordinateSystemCollection), ReferenceMessages.CoordinateSystemCollectionIsNull);
            if (verticalDatumCollection == null)
                throw new ArgumentNullException(nameof(verticalDatumCollection), ReferenceMessages.DatumCollectionIsNull);

            this.areaOfUserCollection = areaOfUseCollection;
            this.coordinateSystemCollection = coordinateSystemCollection;
            this.verticalDatumCollection = verticalDatumCollection;
        }

        /// <summary>
        /// Returns a collection with items with the specified area of use.
        /// </summary>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are with the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public IEnumerable<VerticalCoordinateReferenceSystem> WithArea(AreaOfUse area)
        {
            return this.GetReferences().Where(referenceSystem => referenceSystem.AreaOfUse.Equals(area));
        }

        /// <summary>
        /// Returns a collection with items within the specified area of use.
        /// </summary>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are within the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        /// <exception cref="System.ArgumentNullException">The coordinate system is null.</exception>
        public IEnumerable<VerticalCoordinateReferenceSystem> WithinArea(AreaOfUse area)
        {
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return this.GetReferences().Where(referenceSystem => referenceSystem.AreaOfUse.Within(area));
        }

        /// <summary>
        /// Returns a collection with items matching a specified coordinate system.
        /// </summary>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <returns>A collection containing the items that match the specified coordinate system.</returns>
        public IEnumerable<VerticalCoordinateReferenceSystem> WithCoordinateSystem(CoordinateSystem coordinateSystem)
        {
            return this.GetReferences().Where(referenceSystem => referenceSystem.CoordinateSystem.Equals(coordinateSystem));
        }

        /// <summary>
        /// Returns a collection with items matching a specified datum.
        /// </summary>
        /// <param name="datum">The geodetic datum.</param>
        /// <returns>A collection containing the items that match the specified datum.</returns>
        /// <exception cref="System.ArgumentNullException">The datum is null.</exception>
        public IEnumerable<VerticalCoordinateReferenceSystem> WithDatum(GeodeticDatum datum)
        {
            return this.GetReferences().Where(referenceSystem => referenceSystem.Datum.Equals(datum));
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override VerticalCoordinateReferenceSystem Convert(String[] content)
        {
            switch (content[3])
            {
                case "vertical":
                    return new VerticalCoordinateReferenceSystem(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                                 content[11], this.GetAliases(Int32.Parse(content[0])), content[10],
                                                                 this.coordinateSystemCollection[Authority, Int32.Parse(content[4])],
                                                                 this.verticalDatumCollection[Authority, Int32.Parse(content[5])],
                                                                 this.areaOfUserCollection[Authority, Int32.Parse(content[2])]);
                default:
                    return null;
            }
        }
    }
}
