// <copyright file="ReferenceCollectionQueries.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Defines methods for querying reference collections.
    /// </summary>
    internal static class ReferenceCollectionQueries
    {
        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <typeparam name="ReferenceType">The type of the reference.</typeparam>
        /// <param name="collection">The reference collection.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public static IEnumerable<ReferenceType> WithIdentifier<ReferenceType>(this IEnumerable<ReferenceType> collection, String identifier)
            where ReferenceType : IdentifiedObject
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            return collection.Where(item => item.Identifier.IndexOf(identifier, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <typeparam name="ReferenceType">The type of the reference.</typeparam>
        /// <param name="collection">The reference collection.</param>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The name is null.
        /// </exception>
        public static IEnumerable<ReferenceType> WithName<ReferenceType>(this IEnumerable<ReferenceType> collection, String name)
            where ReferenceType : IdentifiedObject
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            return collection.Where(item => item.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 || item.Aliases.Any(alias => alias.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <typeparam name="ReferenceType">The type of the reference.</typeparam>
        /// <param name="collection">The reference collection.</param>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public static IEnumerable<ReferenceType> WithMatchingIdentifier<ReferenceType>(this IEnumerable<ReferenceType> collection, String identifier)
            where ReferenceType : IdentifiedObject
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            Regex identifierRegex = new Regex(identifier, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            return collection.Where(item => identifierRegex.IsMatch(item.Identifier));
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <typeparam name="ReferenceType">The type of the reference.</typeparam>
        /// <param name="collection">The reference collection.</param>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The name is null.
        /// </exception>
        public static IEnumerable<ReferenceType> WithMatchingName<ReferenceType>(this IEnumerable<ReferenceType> collection, String name)
            where ReferenceType : IdentifiedObject
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            Regex nameRegex = new Regex(name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            return collection.Where(item => nameRegex.IsMatch(item.Name) || item.Aliases.Any(alias => nameRegex.IsMatch(alias)));
        }

        /// <summary>
        /// Returns a collection with items with the specified area.
        /// </summary>
        /// <param name="collection">The area of use collection.</param>
        /// <param name="south">The south angle of the area.</param>
        /// <param name="west">The west angle of the area.</param>
        /// <param name="north">The north angle of the area.</param>
        /// <param name="east">The east angle of the area.</param>
        /// <returns>A collection containing the items with the specified area.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<AreaOfUse> WithArea(this IEnumerable<AreaOfUse> collection, Angle south, Angle west, Angle north, Angle east)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.South.Equals(south) && item.East.Equals(east) && item.North.Equals(north) && item.West.Equals(west));
        }

        /// <summary>
        /// Returns a collection with items within the specified area.
        /// </summary>
        /// <param name="collection">The area of use collection.</param>
        /// <param name="south">The south angle of the area.</param>
        /// <param name="west">The west angle of the area.</param>
        /// <param name="north">The north angle of the area.</param>
        /// <param name="east">The east angle of the area.</param>
        /// <returns>A collection containing the items that are within the specified area.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<AreaOfUse> WithinArea(this IEnumerable<AreaOfUse> collection, Angle south, Angle west, Angle north, Angle east)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Within(south, west, north, east));
        }

        /// <summary>
        /// Returns a collection with items with the specified area of use.
        /// </summary>
        /// <param name="collection">The compound reference system collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items with the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<CompoundReferenceSystem> WithAreaOfUse(this IEnumerable<CompoundReferenceSystem> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Equals(area));
        }

        /// <summary>
        /// Returns a collection with items within the specified area of use.
        /// </summary>
        /// <param name="collection">The compound reference system collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are within the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<CompoundReferenceSystem> WithinAreaOfUse(this IEnumerable<CompoundReferenceSystem> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Within(area));
        }

        /// <summary>
        /// Returns a collection with items matching the specified parameters.
        /// </summary>
        /// <param name="collection">The coordinate operation method collection.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <returns>A collection containing the instances that match the specified parameters.</returns>
        public static IEnumerable<CoordinateOperationMethod> WithParameters(this IEnumerable<CoordinateOperationMethod> collection, IEnumerable<CoordinateOperationParameter> parameters)
        {
            return collection.Where(item => IsMatching(item.Parameters, parameters));
        }

        /// <summary>
        /// Returns a collection with items with the specified area of use.
        /// </summary>
        /// <param name="collection">The coordinate reference system collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items with the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<CoordinateReferenceSystem> WithAreaOfUse(this IEnumerable<CoordinateReferenceSystem> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Equals(area));
        }

        /// <summary>
        /// Returns a collection with items within the specified area of use.
        /// </summary>
        /// <param name="collection">The coordinate reference system collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are within the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<CoordinateReferenceSystem> WithinAreaOfUse(this IEnumerable<CoordinateReferenceSystem> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Within(area));
        }

        /// <summary>
        /// Returns a collection with items matching a specified coordinate system.
        /// </summary>
        /// <param name="collection">The coordinate reference system collection.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <returns>A collection containing the items that match the specified coordinate system.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The coordinate system is null.
        /// </exception>
        public static IEnumerable<CoordinateReferenceSystem> WithCoordinateSystem(this IEnumerable<CoordinateReferenceSystem> collection, CoordinateSystem coordinateSystem)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (coordinateSystem == null)
                throw new ArgumentNullException(nameof(coordinateSystem), ReferenceMessages.CoordinateSystemIsNull);

            return collection.Where(item => item.CoordinateSystem.Equals(coordinateSystem));
        }

        /// <summary>
        /// Returns a collection with items matching a specified datum.
        /// </summary>
        /// <param name="collection">The coordinate reference system collection.</param>
        /// <param name="datum">The geodetic datum.</param>
        /// <returns>A collection containing the items that match the specified datum.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The datum is null.
        /// </exception>
        public static IEnumerable<CoordinateReferenceSystem> WithDatum(this IEnumerable<CoordinateReferenceSystem> collection, GeodeticDatum datum)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (datum == null)
                throw new ArgumentNullException(nameof(datum), ReferenceMessages.DatumIsNull);

            return collection.Where(item => item.Datum.Equals(datum));
        }

        /// <summary>
        /// Returns a collection with items with the specified dimension.
        /// </summary>
        /// <param name="collection">The coordinate system collection.</param>
        /// <param name="dimension">The dimension.</param>
        /// <returns>A collection containing the items that are with the specified dimension.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<CoordinateSystem> WithDimension(this IEnumerable<CoordinateSystem> collection, Int32 dimension)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Dimension == dimension);
        }

        /// <summary>
        /// Returns a collection with items with the specified type.
        /// </summary>
        /// <param name="collection">The coordinate system collection.</param>
        /// <param name="type">The type of the coordinate system.</param>
        /// <returns>A collection containing the items that are with the specified type.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<CoordinateSystem> WithType(this IEnumerable<CoordinateSystem> collection, CoordinateSystemType type)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Type == type);
        }

        /// <summary>
        /// Returns a collection with items with the specified area of use.
        /// </summary>
        /// <param name="collection">The geodetic datum collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items with the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<GeodeticDatum> WithArea(this IEnumerable<GeodeticDatum> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Equals(area));
        }

        /// <summary>
        /// Returns a collection with items within the specified area of use.
        /// </summary>
        /// <param name="collection">The geodetic datum collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are within the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<GeodeticDatum> WithinArea(this IEnumerable<GeodeticDatum> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Within(area));
        }

        /// <summary>
        /// Returns a collection with items with the specified ellipsoid.
        /// </summary>
        /// <param name="collection">The geodetic datum collection.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>A collection containing the items that are with the specified ellipsoid.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public static IEnumerable<GeodeticDatum> WithEllipsoid(this IEnumerable<GeodeticDatum> collection, Ellipsoid ellipsoid)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.Ellipsoid.Equals(ellipsoid));
        }

        /// <summary>
        /// Returns a collection with items with the specified prime meridian.
        /// </summary>
        /// <param name="collection">The geodetic datum collection.</param>
        /// <param name="primeMeridian">The prime meridian.</param>
        /// <returns>A collection containing the items that are with the specified prime meridian.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The prime meridian is null.
        /// </exception>
        public static IEnumerable<GeodeticDatum> WithPrimeMeridian(this IEnumerable<GeodeticDatum> collection, Meridian primeMeridian)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (primeMeridian == null)
                throw new ArgumentNullException(nameof(primeMeridian), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.PrimeMeridian.Equals(primeMeridian));
        }

        /// <summary>
        /// Returns a collection with items matching a specified longitude.
        /// </summary>
        /// <param name="collection">The meridian collection.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>A collection containing the items that match the specified longitude.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<Meridian> WithLongitude(this IEnumerable<Meridian> collection, Angle longitude)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Longitude.Equals(longitude));
        }

        /// <summary>
        /// Returns a collection with items matching a specified projection.
        /// </summary>
        /// <param name="collection">The projected coordinate reference system collection.</param>
        /// <param name="projection">The coordinate projection.</param>
        /// <returns>A collection containing the items that match the specified projection.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The projection is null.
        /// </exception>
        public static IEnumerable<ProjectedCoordinateReferenceSystem> WithProjection(this IEnumerable<ProjectedCoordinateReferenceSystem> collection,  CoordinateProjection projection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Projection.Equals(projection));
        }

        /// <summary>
        /// Returns a collection with items with the specified dimension.
        /// </summary>
        /// <param name="collection">The reference system collection.</param>
        /// <param name="dimension">The dimension.</param>
        /// <returns>A collection containing the items that are with the specified dimension.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<ReferenceSystem> WithDimension(this IEnumerable<ReferenceSystem> collection, Int32 dimension)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Dimension == dimension);
        }

        /// <summary>
        /// Returns a collection with items with the specified type.
        /// </summary>
        /// <param name="collection">The reference system collection.</param>
        /// <param name="type">The type of the reference system.</param>
        /// <returns>A collection containing the items that are with the specified type.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<ReferenceSystem> WithType(this IEnumerable<ReferenceSystem> collection, ReferenceSystemType type)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Type == type);
        }

        /// <summary>
        /// Returns a collection with items with the specified area of use.
        /// </summary>
        /// <param name="collection">The vertical datum collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items with the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<VerticalDatum> WithArea(this IEnumerable<VerticalDatum> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Equals(area));
        }

        /// <summary>
        /// Returns a collection with items within the specified area of use.
        /// </summary>
        /// <param name="collection">The vertical datum collection.</param>
        /// <param name="area">The area of use.</param>
        /// <returns>A collection containing the items that are within the specified area of use.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The collection is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public static IEnumerable<VerticalDatum> WithinArea(this IEnumerable<VerticalDatum> collection, AreaOfUse area)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);
            if (area == null)
                throw new ArgumentNullException(nameof(area), ReferenceMessages.AreaOfUseIsNull);

            return collection.Where(item => item.AreaOfUse.Within(area));
        }

        /// <summary>
        /// Returns a collection with items with the specified type.
        /// </summary>
        /// <param name="collection">The vertical datum collection.</param>
        /// <param name="type">The type of the datum.</param>
        /// <returns>A collection containing the items that are with the specified ellipsoid.</returns>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public static IEnumerable<VerticalDatum> WithType(this IEnumerable<VerticalDatum> collection, VerticalDatumType type)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection), ReferenceMessages.CollectionIsNull);

            return collection.Where(item => item.Type.Equals(type));
        }

        /// <summary>
        /// Returns whether the specified parameter collections match.
        /// </summary>
        /// <param name="parameters">The collection of  parameters.</param>
        /// <param name="otherParameters">The other collection of parameters.</param>
        /// <returns><c>true</c> if the collections match; otherwise, <c>false</c>.</returns>
        private static Boolean IsMatching(IEnumerable<CoordinateOperationParameter> parameters, IEnumerable<CoordinateOperationParameter> otherParameters)
        {
            if (parameters == null && otherParameters == null)
                return true;

            if (parameters == null || otherParameters == null)
                return false;

            if (parameters.Count() != otherParameters.Count())
                return false;

            foreach (CoordinateOperationParameter parameter in parameters)
            {
                if (!otherParameters.Contains(parameter))
                    return false;
            }

            return true;
        }
    }
}
