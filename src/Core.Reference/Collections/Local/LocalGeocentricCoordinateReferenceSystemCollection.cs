// <copyright file="LocalGeocentricCoordinateReferenceSystemCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of <see cref="GeocentricCoordinateReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalGeocentricCoordinateReferenceSystemCollection : LocalReferenceCollection<GeocentricCoordinateReferenceSystem>
    {
        #region Private constants

        /// <summary>
        /// The name of the resource. This field is constant.
        /// </summary>
        private const String ResourceName = "CoordinateReferenceSystem";

        /// <summary>
        /// The name of the alias type. This field is constant.
        /// </summary>
        private const String AliasTypeName = "Coordinate Reference System";

        #endregion

        #region Private fields

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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalGeocentricCoordinateReferenceSystemCollection" /> class.
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
        public LocalGeocentricCoordinateReferenceSystemCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<CoordinateSystem> coordinateSystemCollection, IReferenceCollection<GeodeticDatum> geodeticDatumCollection)
            : base(ResourceName, AliasTypeName)
        {
            if (areaOfUseCollection == null)
                throw new ArgumentNullException(nameof(areaOfUseCollection), Messages.AreaOfUseCollectionIsNull);
            if (coordinateSystemCollection == null)
                throw new ArgumentNullException(nameof(coordinateSystemCollection), Messages.CoordinateSystemIsNull);
            if (geodeticDatumCollection == null)
                throw new ArgumentNullException(nameof(geodeticDatumCollection), Messages.DatumCollectionIsNull);

            this.areaOfUseCollection = areaOfUseCollection;
            this.coordinateSystemCollection = coordinateSystemCollection;
            this.geodeticDatumCollection = geodeticDatumCollection;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override GeocentricCoordinateReferenceSystem Convert(String[] content)
        {
            switch (content[3])
            {
                case "geocentric":
                    return new GeocentricCoordinateReferenceSystem(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                                   content[11], this.GetAliases(Int32.Parse(content[0])), content[10],
                                                                   this.coordinateSystemCollection[Authority, Int32.Parse(content[4])],
                                                                   this.geodeticDatumCollection[Authority, Int32.Parse(content[5])],
                                                                   this.areaOfUseCollection[Authority, Int32.Parse(content[2])]);
                default:
                    return null;
            }
        }

        #endregion
    }
}
