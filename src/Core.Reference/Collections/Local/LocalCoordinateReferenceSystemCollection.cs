// <copyright file="LocalCoordinateReferenceSystemCollection.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of <see cref="CoordinateReferenceSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalCoordinateReferenceSystemCollection : IReferenceCollection<CoordinateReferenceSystem>
    {
        /// <summary>
        /// The geocentric reference system collection. This field is read-only.
        /// </summary>
        private readonly IReferenceCollection<GeocentricCoordinateReferenceSystem> geocentricCollection;

        /// <summary>
        /// The geographic reference system collection. This field is read-only.
        /// </summary>
        private readonly IReferenceCollection<GeographicCoordinateReferenceSystem> geographicCollection;

        /// <summary>
        /// The projected reference system collection. This field is read-only.
        /// </summary>
        private readonly IReferenceCollection<ProjectedCoordinateReferenceSystem> projectedCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCoordinateReferenceSystemCollection" /> class.
        /// </summary>
        /// <param name="geocentricCollection">The geocentric reference system collection.</param>
        /// <param name="geographicCollection">The geographic reference system collection.</param>
        /// <param name="projectedCollection">The projected reference system collection.</param>
        public LocalCoordinateReferenceSystemCollection(IReferenceCollection<GeocentricCoordinateReferenceSystem> geocentricCollection, IReferenceCollection<GeographicCoordinateReferenceSystem> geographicCollection, IReferenceCollection<ProjectedCoordinateReferenceSystem> projectedCollection)
        {
            this.geocentricCollection = geocentricCollection;
            this.geographicCollection = geographicCollection;
            this.projectedCollection = projectedCollection;
        }

        /// <summary>
        /// Gets the item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The item with the specified authority and code; or <c>null</c> if no item exists with the specified authority and code.</returns>
        public CoordinateReferenceSystem this[String authority, Int32 code]
        {
            get
            {
                CoordinateReferenceSystem system = this.geocentricCollection[authority, code];
                if (system != null)
                    return system;

                system = this.geographicCollection[authority, code];
                if (system != null)
                    return system;

                return this.projectedCollection[authority, code];
            }
        }

        /// <summary>
        /// Gets the item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The item with the specified identifier; or <c>null</c> if no item exists with the specified identifier.</returns>
        public CoordinateReferenceSystem this[String identifier]
        {
            get
            {
                CoordinateReferenceSystem system = this.geocentricCollection[identifier];
                if (system != null)
                    return system;

                system = this.geographicCollection[identifier];
                if (system != null)
                    return system;

                return this.projectedCollection[identifier];
            }
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateReferenceSystem> WithIdentifier(String identifier)
        {
            foreach (GeocentricCoordinateReferenceSystem item in this.geocentricCollection.WithIdentifier(identifier))
                yield return item;
            foreach (GeographicCoordinateReferenceSystem item in this.geographicCollection.WithIdentifier(identifier))
                yield return item;
            foreach (ProjectedCoordinateReferenceSystem item in this.projectedCollection.WithIdentifier(identifier))
                yield return item;
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateReferenceSystem> WithName(String name)
        {
            foreach (GeocentricCoordinateReferenceSystem item in this.geocentricCollection.WithName(name))
                yield return item;
            foreach (GeographicCoordinateReferenceSystem item in this.geographicCollection.WithName(name))
                yield return item;
            foreach (ProjectedCoordinateReferenceSystem item in this.projectedCollection.WithName(name))
                yield return item;
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateReferenceSystem> WithMatchingIdentifier(String identifier)
        {
            foreach (GeocentricCoordinateReferenceSystem item in this.geocentricCollection.WithMatchingIdentifier(identifier))
                yield return item;
            foreach (GeographicCoordinateReferenceSystem item in this.geographicCollection.WithMatchingIdentifier(identifier))
                yield return item;
            foreach (ProjectedCoordinateReferenceSystem item in this.projectedCollection.WithMatchingIdentifier(identifier))
                yield return item;
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateReferenceSystem> WithMatchingName(String name)
        {
            foreach (GeocentricCoordinateReferenceSystem item in this.geocentricCollection.WithMatchingName(name))
                yield return item;
            foreach (GeographicCoordinateReferenceSystem item in this.geographicCollection.WithMatchingName(name))
                yield return item;
            foreach (ProjectedCoordinateReferenceSystem item in this.projectedCollection.WithMatchingName(name))
                yield return item;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<CoordinateReferenceSystem> GetEnumerator()
        {
            foreach (GeocentricCoordinateReferenceSystem item in this.geocentricCollection)
                yield return item;
            foreach (GeographicCoordinateReferenceSystem item in this.geographicCollection)
                yield return item;
            foreach (ProjectedCoordinateReferenceSystem item in this.projectedCollection)
                yield return item;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
