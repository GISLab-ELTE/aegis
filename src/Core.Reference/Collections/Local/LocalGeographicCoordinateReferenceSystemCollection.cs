// <copyright file="LocalGeographicCoordinateReferenceSystemCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of <see cref="GeographicCoordinateReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalGeographicCoordinateReferenceSystemCollection : LocalReferenceCollection<GeographicCoordinateReferenceSystem>
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
        /// The collection of  <see cref="CoordinateSystem" /> instances.
        /// </summary>
        private IReferenceCollection<CoordinateSystem> coordinateSystemCollection;

        /// <summary>
        /// The collection of  <see cref="GeodeticDatum" /> instances.
        /// </summary>
        private IReferenceCollection<GeodeticDatum> geodeticDatumCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalGeographicCoordinateReferenceSystemCollection" /> class.
        /// </summary>
        /// <param name="areaOfUseCollection">The area of use collection.</param>
        /// <param name="coordinateSystemCollection">The coordinate system collection.</param>
        /// <param name="geodeticDatumCollection">The geodetic datum collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The area of use collection is null.
        /// or
        /// The coordinate system collection is null.
        /// or
        /// The datum collection is null.
        /// </exception>
        public LocalGeographicCoordinateReferenceSystemCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<CoordinateSystem> coordinateSystemCollection, IReferenceCollection<GeodeticDatum> geodeticDatumCollection)
            : base(ResourceName, AliasTypeName)
        {
            if (areaOfUseCollection == null)
                throw new ArgumentNullException(nameof(areaOfUseCollection), ReferenceMessages.AreaOfUseCollectionIsNull);
            if (coordinateSystemCollection == null)
                throw new ArgumentNullException(nameof(coordinateSystemCollection), ReferenceMessages.CoordinateSystemCollectionIsNull);
            if (geodeticDatumCollection == null)
                throw new ArgumentNullException(nameof(geodeticDatumCollection), ReferenceMessages.DatumCollectionIsNull);

            this.areaOfUseCollection = areaOfUseCollection;
            this.coordinateSystemCollection = coordinateSystemCollection;
            this.geodeticDatumCollection = geodeticDatumCollection;
        }

        /// <summary>
        /// Returns a collection with items with the specified area of use.
        /// </summary>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are with the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public IEnumerable<GeographicCoordinateReferenceSystem> WithArea(AreaOfUse area)
        {
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return this.GetReferences().Where(referenceSystem => referenceSystem.AreaOfUse.Equals(area));
        }

        /// <summary>
        /// Returns a collection with items within the specified area of use.
        /// </summary>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are within the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public IEnumerable<GeographicCoordinateReferenceSystem> WithinArea(AreaOfUse area)
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
        /// <exception cref="System.ArgumentNullException">The coordinate system is null.</exception>
        public IEnumerable<GeographicCoordinateReferenceSystem> WithCoordinateSystem(CoordinateSystem coordinateSystem)
        {
            if (coordinateSystem == null)
                throw new ArgumentNullException(nameof(coordinateSystem), ReferenceMessages.CoordinateSystemIsNull);

            return this.GetReferences().Where(referenceSystem => referenceSystem.CoordinateSystem.Equals(coordinateSystem));
        }

        /// <summary>
        /// Returns a collection with items matching a specified datum.
        /// </summary>
        /// <param name="datum">The geodetic datum.</param>
        /// <returns>A collection containing the items that match the specified datum.</returns>
        /// <exception cref="System.ArgumentNullException">The datum is null.</exception>
        public IEnumerable<GeographicCoordinateReferenceSystem> WithDatum(GeodeticDatum datum)
        {
            if (datum == null)
                throw new ArgumentNullException(nameof(datum), ReferenceMessages.DatumIsNull);

            return this.GetReferences().Where(referenceSystem => referenceSystem.Datum.Equals(datum));
        }

        /// <summary>
        /// Returns a collection with items matching a specified dimension.
        /// </summary>
        /// <param name="dimension">The dimension of the coordinate system.</param>
        /// <returns>A collection containing the items that match the specified dimension.</returns>
        public IEnumerable<GeographicCoordinateReferenceSystem> WithDimension(Int32 dimension)
        {
            return this.GetReferences().Where(referenceSystem => referenceSystem.Dimension == dimension);
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override GeographicCoordinateReferenceSystem Convert(String[] content)
        {
            switch (content[3])
            {
                case "geographic 2D":
                case "geographic 3D":
                    return new GeographicCoordinateReferenceSystem(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                                   content[11], this.GetAliases(Int32.Parse(content[0])), content[10],
                                                                   this.coordinateSystemCollection[Authority, Int32.Parse(content[4])],
                                                                   this.geodeticDatumCollection[Authority, Int32.Parse(content[5])],
                                                                   this.areaOfUseCollection[Authority, Int32.Parse(content[2])]);
                default:
                    return null;
            }
        }
    }
}
