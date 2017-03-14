// <copyright file="WellKnownTextConverter.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using AEGIS.Geometries;
    using AEGIS.Reference;
    using AEGIS.Reference.Collections;
    using AEGIS.Reference.Resources;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a converter for Well-known Text (WKT) representation.
    /// </summary>
    public static class WellKnownTextConverter
    {
        /// <summary>
        /// The regular expression pattern for floating point numbers. This field is constant.
        /// </summary>
        private const String PatternNumber = @"[-]?(\d*\.\d+|\d+)";

        /// <summary>
        /// The regular expression pattern for left delimiters. This field is constant.
        /// </summary>
        private const String PatternLeftDelim = @"[(\[]";

        /// <summary>
        /// The regular expression pattern for right delimiters. This field is constant.
        /// </summary>
        private const String PatternRightDelim = @"[)\]]";

        /// <summary>
        /// The regular expression pattern for names. This field is constant.
        /// </summary>
        private const String PatternName = @"""?[A-Za-z0-9_ ()/]+""?";

        /// <summary>
        /// The regular expression pattern for authorities. This field is constant.
        /// </summary>
        private const String PatternAuthority = PatternName + @",\s*""?[0-9]+""?";

        /// <summary>
        /// The regular expression pattern for coordinate system axes. This field is constant.
        /// </summary>
        private const String PatternAxis = PatternName + @",\s*[A-Z]+";

        /// <summary>
        /// The regular expression pattern for ellipsoids. This field is constant.
        /// </summary>
        private const String PatternEllipsoid = PatternName + @",\s*" + PatternNumber + @",\s*" + PatternNumber + @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The regular expression pattern for meridians. This field is constant.
        /// </summary>
        private const String PatternMeridian = PatternName + @",\s*" + PatternNumber + @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The regular expression pattern for units of measurement. This field is constant.
        /// </summary>
        private const String PatternUnitOfMeasurement = PatternName + @",\s*" + PatternNumber + @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The regular expression pattern for datums. This field is constant.
        /// </summary>
        private const String PatternDatum = PatternName +
                                            @",\s*(ELLIPSOID|SPHEROID)\s*" + PatternLeftDelim + PatternEllipsoid + PatternRightDelim +
                                            @"(,\s*TOWGS84\s*" + PatternLeftDelim + PatternNumber + @"(,\s*" + PatternNumber + @"){6}" + PatternRightDelim + @")?" +
                                            @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The regular expression pattern for geodetic coordinate reference systems. This field is constant.
        /// </summary>
        private const String PatternGeodeticReferenceSystem = PatternName +
                                                              @",\s*DATUM\s*" + PatternLeftDelim + PatternDatum + PatternRightDelim +
                                                              @",\s*PRIMEM\s*" + PatternLeftDelim + PatternMeridian + PatternRightDelim +
                                                              @",\s*UNIT\s*" + PatternLeftDelim + PatternUnitOfMeasurement + PatternRightDelim +
                                                              @"(,\s*AXIS\s*" + PatternLeftDelim + PatternAxis + PatternRightDelim + @"){0,3}" +
                                                              @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The regular expression pattern for coordinate operation parameters. This field is constant.
        /// </summary>
        private const String PatternCoordinateOperationParameter = PatternName + @",\s*" + PatternNumber;

        /// <summary>
        /// The regular expression pattern for coordinate operation methods. This field is constant.
        /// </summary>
        private const String PatternCoordinateOperationMethod = PatternName + @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The regular expression pattern for projected coordinate reference systems. This field is constant.
        /// </summary>
        private const String PatternProjectedReferenceSystem = PatternName +
                                                               @",\s*GEOGCS\s*" + PatternLeftDelim + PatternGeodeticReferenceSystem + PatternRightDelim +
                                                               @",\s*PROJECTION\s*" + PatternLeftDelim + PatternCoordinateOperationMethod + PatternRightDelim +
                                                               @"(,\s*PARAMETER\s*" + PatternLeftDelim + PatternCoordinateOperationParameter + PatternRightDelim + @")*" +
                                                               @",\s*UNIT\s*" + PatternLeftDelim + PatternUnitOfMeasurement + PatternRightDelim +
                                                               @"(,\s*AXIS\s*" + PatternLeftDelim + PatternAxis + PatternRightDelim + @"){0,3}" +
                                                               @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The grouped regular expression pattern for names. This field is constant.
        /// </summary>
        private const String PatternGroupedName = @"""?(?<name>[A-Za-z0-9_ ()/]+)""?";

        /// <summary>
        /// The grouped regular expression pattern for authorities. This field is constant.
        /// </summary>
        private const String PatternGroupedAuthority = @"""?(?<authorityName>[A-Z0-9_ ]+)""?,\s*""?(?<authorityCode>[0-9]+)""?";

        /// <summary>
        /// The grouped regular expression pattern for coordinate system axes. This field is constant.
        /// </summary>
        private const String PatternGroupedAxis = @"^" + PatternGroupedName + @",(?<direction>[A-Z]+)$";

        /// <summary>
        /// The grouped regular expression pattern for ellipsoids. This field is constant.
        /// </summary>
        private const String PatternGroupedEllipsoid = @"^" + PatternGroupedName +
                                                       @",\s*(?<semiMajorAxis>" + PatternNumber + @")" +
                                                       @",\s*(?<inverseFlattening>" + PatternNumber + @")" +
                                                       @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternGroupedAuthority + PatternRightDelim + ")?$";

        /// <summary>
        /// The grouped regular expression pattern for meridians. This field is constant.
        /// </summary>
        private const String PatternGroupedMeridian = @"^" + PatternGroupedName +
                                                      @",\s*(?<longitude>" + PatternNumber + @")" +
                                                      @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternGroupedAuthority + PatternRightDelim + ")?$";

        /// <summary>
        /// The grouped regular expression pattern for units of measurement. This field is constant.
        /// </summary>
        private const String PatternGroupedUnitOfMeasurement = @"^" + PatternGroupedName +
                                                               @",\s*(?<baseMultiple>" + PatternNumber + @")" +
                                                               @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternGroupedAuthority + PatternRightDelim + ")?$";

        /// <summary>
        /// The grouped regular expression pattern for datums. This field is constant.
        /// </summary>
        private const String PatternGroupedDatum = @"^" + PatternGroupedName +
                                                   @",\s*(ELLIPSOID|SPHEROID)\s*" + PatternLeftDelim + @"(?<ellipsoid>" + PatternEllipsoid + @")" + PatternRightDelim +
                                                   @"(,\s*TOWGS84\s*" + PatternLeftDelim + @"(?<towgs84>" + PatternNumber + @"(,\s*" + PatternNumber + @"){6})" + PatternRightDelim + @")?" +
                                                   @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternGroupedAuthority + PatternRightDelim + ")?$";

        /// <summary>
        /// The grouped regular expression pattern for geodetic coordinate reference systems. This field is constant.
        /// </summary>
        private const String PatternGroupedGeodeticReferenceSystem = @"^" + PatternGroupedName +
                                                                     @",\s*DATUM\s*" + PatternLeftDelim + @"(?<datum>" + PatternDatum + @")" + PatternRightDelim +
                                                                     @",\s*PRIMEM\s*" + PatternLeftDelim + @"(?<meridian>" + PatternMeridian + @")" + PatternRightDelim +
                                                                     @",\s*UNIT\s*" + PatternLeftDelim + @"(?<unit>" + PatternUnitOfMeasurement + @")" + PatternRightDelim +
                                                                     @"(,\s*AXIS\s*" + PatternLeftDelim + @"(?<axis>" + PatternAxis + @")" + PatternRightDelim + @"){0,3}" +
                                                                     @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternGroupedAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The grouped regular expression pattern for coordinate operation parameters. This field is constant.
        /// </summary>
        private const String PatternGroupedCoordinateOperationParameter = @"^" + PatternGroupedName + @",\s*(?<value>" + PatternNumber + @")$";

        /// <summary>
        /// The grouped regular expression pattern for coordinate operation methods. This field is constant.
        /// </summary>
        private const String PatternGroupedCoordinateOperationMethod = PatternGroupedName + @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternGroupedAuthority + PatternRightDelim + ")?";

        /// <summary>
        /// The grouped regular expression pattern for projected coordinate reference systems. This field is constant.
        /// </summary>
        private const String PatternGroupedProjectedReferenceSystem = @"^" + PatternGroupedName +
                                                                      @",\s*GEOGCS\s*" + PatternLeftDelim + @"(?<geographicReferenceSystem>" + PatternGeodeticReferenceSystem + @")" + PatternRightDelim +
                                                                      @",\s*PROJECTION\s*" + PatternLeftDelim + @"(?<projection>" + PatternCoordinateOperationMethod + @")" + PatternRightDelim +
                                                                      @"(,\s*PARAMETER\s*" + PatternLeftDelim + @"(?<parameter>" + PatternCoordinateOperationParameter + @")" + PatternRightDelim + @")*" +
                                                                      @",\s*UNIT\s*" + PatternLeftDelim + @"(?<unit>" + PatternUnitOfMeasurement + @")" + PatternRightDelim +
                                                                      @"(,\s*AXIS\s*" + PatternLeftDelim + @"(?<axis>" + PatternAxis + @")" + PatternRightDelim + @"){0,2}" +
                                                                      @"(,\s*AUTHORITY\s*" + PatternLeftDelim + PatternGroupedAuthority + PatternRightDelim + ")?$";

        /// <summary>
        /// The grouped regular expression pattern for identified objects. This field is constant.
        /// </summary>
        private const String PatternGroupedIdentifiedObject = @"^(?<type>[A-Z]+)\s*" + PatternLeftDelim + @"(?<content>.+)" + PatternRightDelim + @"$";

        /// <summary>
        /// Converts an identified object to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="source">The source identified object.</param>
        /// <returns>The WKT representation of the identified object.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source is invalid.
        /// or
        /// The specified source is not supported.</exception>
        public static String ToWellKnownText(IdentifiedObject source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            try
            {
                if (source is ProjectedCoordinateReferenceSystem)
                    return ToWellKnownText(source as ProjectedCoordinateReferenceSystem);
                if (source is GeographicCoordinateReferenceSystem)
                    return ToWellKnownText(source as GeographicCoordinateReferenceSystem);
                if (source is GeocentricCoordinateReferenceSystem)
                    return ToWellKnownText(source as GeocentricCoordinateReferenceSystem);
                if (source is UnitOfMeasurement)
                    return ToWellKnownText(source as UnitOfMeasurement);
                if (source is GeodeticDatum)
                    return ToWellKnownText(source as GeodeticDatum);
                if (source is Ellipsoid)
                    return ToWellKnownText(source as Ellipsoid);
                if (source is Meridian)
                    return ToWellKnownText(source as Meridian);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }

            throw new ArgumentException(CoreMessages.SourceIsNotSupported, nameof(source));
        }

        /// <summary>
        /// Converts a reference system to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="source">The source reference system.</param>
        /// <returns>The WKT representation of the reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source is invalid.
        /// or
        /// The specified source is not supported.</exception>
        public static String ToWellKnownText(IReferenceSystem source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);

            try
            {
                if (source is ProjectedCoordinateReferenceSystem)
                    return ToWellKnownText(source as ProjectedCoordinateReferenceSystem);
                if (source is GeographicCoordinateReferenceSystem)
                    return ToWellKnownText(source as GeographicCoordinateReferenceSystem);
                if (source is GeocentricCoordinateReferenceSystem)
                    return ToWellKnownText(source as GeocentricCoordinateReferenceSystem);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }

            throw new ArgumentException(CoreMessages.SourceIsNotSupported, nameof(source));
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to identified object.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted identified object.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The provider is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IdentifiedObject ToIdentifiedObject(String source, IReferenceProvider provider)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);
            if (String.IsNullOrEmpty(source))
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider), ReferenceMessages.ProviderIsNull);

            try
            {
                Match match = Regex.Match(source, PatternGroupedIdentifiedObject, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                String type = match.Groups["type"].Value;
                String content = match.Groups["content"].Value;

                switch (type)
                {
                    case "GEOCCS":
                        return ToGeocentricCoordinateReferenceSystem(content, provider);
                    case "GEOGCS":
                        return ToGeographicCoordinateReferenceSystem(content, provider);
                    case "PROJCS":
                        return ToProjectedReferenceSystem(content, provider);
                    case "DATUM":
                        return ToGeodeticDatum(content, provider.Meridians.FirstOrDefault(), provider);
                    case "SPHEROID":
                    case "ELLIPSOID":
                        return ToEllipsoid(content, provider);
                    case "PRIMEM":
                        return ToMeridian(content, UnitsOfMeasurement.Metre, provider);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }

            throw new ArgumentException(CoreMessages.SourceIsNotSupported, nameof(source));
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a reference system.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted reference system.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The specified source is invalid.</exception>
        public static IReferenceSystem ToReferenceSystem(String source, IReferenceProvider provider)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), CoreMessages.SourceIsNull);
            if (String.IsNullOrEmpty(source))
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source));

            try
            {
                Match match = Regex.Match(source, PatternGroupedIdentifiedObject, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                String type = match.Groups["type"].Value;
                String content = match.Groups["content"].Value;

                IReferenceSystem result = null;

                switch (type)
                {
                    case "GEOCCS":
                        result = ToGeocentricCoordinateReferenceSystem(content, provider);
                        break;
                    case "GEOGCS":
                        result = ToGeographicCoordinateReferenceSystem(content, provider);
                        break;
                    case "PROJCS":
                        result = ToProjectedReferenceSystem(content, provider);
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(CoreMessages.SourceIsInvalid, nameof(source), ex);
            }
        }

        /// <summary>
        /// Converts the specified projected coordinate reference system to Well-known Text (WKT).
        /// </summary>
        /// <param name="referenceSystem">The projected coordinate reference system.</param>
        /// <returns>The converted projected coordinate reference system.</returns>
        private static String ToWellKnownText(ProjectedCoordinateReferenceSystem referenceSystem)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat(@"PROJCS[""{0}"", {1}, PROJECTION[""{2}""]",
                                referenceSystem.Name,
                                ToWellKnownText(referenceSystem.BaseReferenceSystem),
                                referenceSystem.Projection.Method.Name);

            foreach (KeyValuePair<CoordinateOperationParameter, Object> param in referenceSystem.Projection.Parameters)
            {
                if (param.Value is String)
                {
                    result.AppendFormat(@", PARAMETER[""{0}"", {1}]", param.Key.Name, param.Value);
                    continue;
                }

                if (param.Value is Angle)
                {
                    result.AppendFormat(@", PARAMETER[""{0}"", {1}]", param.Key.Name, ((Angle)param.Value).Value.ToString("G", CultureInfo.InvariantCulture));
                    continue;
                }

                if (param.Value is Length)
                {
                    result.AppendFormat(@", PARAMETER[""{0}"", {1}]", param.Key.Name, ((Length)param.Value).Value.ToString("G", CultureInfo.InvariantCulture));
                    continue;
                }

                if (param.Value is IConvertible)
                {
                    result.AppendFormat(@", PARAMETER[""{0}"", {1}]", param.Key.Name, Convert.ToDouble(param.Value).ToString("G", CultureInfo.InvariantCulture));
                    continue;
                }
            }

            result.AppendFormat(@", {0}", ToWellKnownText(referenceSystem.CoordinateSystem[0].Unit));
            result.AppendFormat(@", AUTHORITY[""{0}"", ""{1}""]]", referenceSystem.Authority, referenceSystem.Code);

            return result.ToString();
        }

        /// <summary>
        /// Converts the specified geographic coordinate reference system to Well-known Text (WKT).
        /// </summary>
        /// <param name="referenceSystem">The geographic coordinate reference system.</param>
        /// <returns>The converted geographic coordinate reference system.</returns>
        private static String ToWellKnownText(GeographicCoordinateReferenceSystem referenceSystem)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat(@"GEOGCS[""{0}"", {1}, {2}",
                                referenceSystem.Name,
                                ToWellKnownText(referenceSystem.Datum as GeodeticDatum),
                                ToWellKnownText((referenceSystem.Datum as GeodeticDatum).PrimeMeridian));

            UnitOfMeasurement linearUnit = null;
            UnitOfMeasurement angularUnit = null;

            foreach (CoordinateSystemAxis axis in referenceSystem.CoordinateSystem.Axes)
            {
                if (axis.Unit.Type == UnitQuantityType.Angle)
                    angularUnit = axis.Unit;
                else if (axis.Unit.Type == UnitQuantityType.Length)
                    linearUnit = axis.Unit;
            }

            if (angularUnit != null)
                result.AppendFormat(@", {0}", ToWellKnownText(angularUnit));

            if (linearUnit != null)
                result.AppendFormat(@", {0}", ToWellKnownText(linearUnit));

            result.AppendFormat(@", AUTHORITY[""{0}"", ""{1}""]]", referenceSystem.Authority, referenceSystem.Code);

            return result.ToString();
        }

        /// <summary>
        /// Converts the specified geocentric coordinate reference system to Well-known Text (WKT).
        /// </summary>
        /// <param name="referenceSystem">The geocentric coordinate reference system.</param>
        /// <returns>The converted geocentric coordinate reference system.</returns>
        private static String ToWellKnownText(GeocentricCoordinateReferenceSystem referenceSystem)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat(@"GEOCCS[""{0}"", {1}, {2}",
                                referenceSystem.Name,
                                ToWellKnownText(referenceSystem.Datum as GeodeticDatum),
                                ToWellKnownText((referenceSystem.Datum as GeodeticDatum).PrimeMeridian));

            UnitOfMeasurement linearUnit = null;

            foreach (CoordinateSystemAxis axis in referenceSystem.CoordinateSystem.Axes)
            {
                if (axis.Unit.Type == UnitQuantityType.Length)
                    linearUnit = axis.Unit;
            }

            if (linearUnit != null)
                result.AppendFormat(@", {0}", ToWellKnownText(linearUnit));

            result.AppendFormat(@", AUTHORITY[""{0}"", ""{1}""]]", referenceSystem.Authority, referenceSystem.Code);

            return result.ToString();
        }

        /// <summary>
        /// Converts the specified unit of measurement to Well-known Text (WKT).
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The converted unit of measurement.</returns>
        private static String ToWellKnownText(UnitOfMeasurement unit)
        {
            return String.Format(@"UNIT[""{0}"", {1}, AUTHORITY[""{2}"", ""{3}""]]",
                                 unit.Name,
                                 unit.BaseMultiple.ToString("G", CultureInfo.InvariantCulture),
                                 unit.Authority,
                                 unit.Code);
        }

        /// <summary>
        /// Converts the specified geodetic datum to Well-known Text (WKT).
        /// </summary>
        /// <param name="datum">The geodetic datum.</param>
        /// <returns>The converted geodetic datum.</returns>
        private static String ToWellKnownText(GeodeticDatum datum)
        {
            return String.Format(@"DATUM[""{0}"", {1}, AUTHORITY[""{2}"", ""{3}""]]",
                                 datum.Name,
                                 ToWellKnownText(datum.Ellipsoid),
                                 datum.Authority,
                                 datum.Code);
        }

        /// <summary>
        /// Converts the specified ellipsoid to Well-known Text (WKT).
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The converted ellipsoid.</returns>
        private static String ToWellKnownText(Ellipsoid ellipsoid)
        {
            return String.Format(@"SPHEROID[""{0}"", {1}, {2}, AUTHORITY[""{3}"", ""{4}""]]",
                                 ellipsoid.Name,
                                 ellipsoid.SemiMajorAxis.Value.ToString("G", CultureInfo.InvariantCulture),
                                 ellipsoid.InverseFattening.ToString("G", CultureInfo.InvariantCulture),
                                 ellipsoid.Authority,
                                 ellipsoid.Code);
        }

        /// <summary>
        /// Converts the specified meridian to Well-known Text (WKT).
        /// </summary>
        /// <param name="meridian">The meridian.</param>
        /// <returns>The converted meridian.</returns>
        private static String ToWellKnownText(Meridian meridian)
        {
            return String.Format(@"PRIMEM[""{0}"", {1}, AUTHORITY[""{2}"", ""{3}""]]",
                                 meridian.Name,
                                 meridian.Longitude.Value.ToString("G", CultureInfo.InvariantCulture),
                                 meridian.Authority,
                                 meridian.Code);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a geocentric coordinate reference system.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted geocentric coordinate reference system.</returns>
        private static GeocentricCoordinateReferenceSystem ToGeocentricCoordinateReferenceSystem(String source, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedGeodeticReferenceSystem);

            String name = match.Groups["name"].Value.Replace('_', ' ').Replace("GCS_", String.Empty);
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                GeocentricCoordinateReferenceSystem referenceSystem = provider.GeocentricCoordinateReferenceSystems[authorityName, code];
                if (referenceSystem != null)
                    return referenceSystem;
            }

            // convert content
            UnitOfMeasurement unit = ToAngularUnit(match.Groups["unit"].Value, provider);
            Meridian meridian = ToMeridian(match.Groups["meridian"].Value, unit, provider);
            GeodeticDatum datum = ToGeodeticDatum(match.Groups["datum"].Value, meridian, provider);

            // search by name
            foreach (GeocentricCoordinateReferenceSystem referenceSystem in provider.GeocentricCoordinateReferenceSystems.WithName(name))
            {
                if (referenceSystem.Datum.Equals(datum))
                    return referenceSystem;
            }

            // convert coordinate system
            CoordinateSystemAxis[] axes = new CoordinateSystemAxis[match.Groups["axis"].Captures.Count];
            foreach (Capture capture in match.Groups["axis"].Captures)
                axes[capture.Index] = ToCoordinateSystemAxis(capture.Value, unit);

            CoordinateSystem coordinateSystem = null;
            if (axes.Length == 0)
            {
                coordinateSystem = provider.CoordinateSystems["EPSG", 6500];
            }
            else
            {
                coordinateSystem = new CoordinateSystem(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, CoordinateSystemType.Ellipsoidal, axes);
            }

            // user defined
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode))
                return new GeocentricCoordinateReferenceSystem(IdentifiedObject.GetIdentifier(authorityName, authorityCode), name, coordinateSystem, datum, AreaOfUse.Undefined);
            else
                return new GeocentricCoordinateReferenceSystem(UnitOfMeasurement.UserDefinedIdentifier, name, coordinateSystem, datum, AreaOfUse.Undefined);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a geographic coordinate reference system.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted geographic coordinate reference system.</returns>
        private static GeographicCoordinateReferenceSystem ToGeographicCoordinateReferenceSystem(String source, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedGeodeticReferenceSystem);

            String name = match.Groups["name"].Value.Replace('_', ' ').Replace("GCS_", String.Empty);
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                GeographicCoordinateReferenceSystem referenceSystem = provider.GeographicCoordinateReferenceSystems[authorityName, code];
                if (referenceSystem != null)
                    return referenceSystem;
            }

            // trim the "GCS " from the name
            if (name.Length > 4 && name.Substring(0, 4) == "GCS ")
                name = name.Substring(4);

            // convert content
            UnitOfMeasurement unit = ToAngularUnit(match.Groups["unit"].Value, provider);
            Meridian meridian = ToMeridian(match.Groups["meridian"].Value, unit, provider);
            GeodeticDatum datum = ToGeodeticDatum(match.Groups["datum"].Value, meridian, provider);

            // search by name
            foreach (GeographicCoordinateReferenceSystem referenceSystem in provider.GeographicCoordinateReferenceSystems.WithName(name))
            {
                if (referenceSystem.Datum.Equals(datum))
                    return referenceSystem;
            }

            // convert coordinate system
            CoordinateSystemAxis[] axes = new CoordinateSystemAxis[match.Groups["axis"].Captures.Count];
            foreach (Capture capture in match.Groups["axis"].Captures)
                axes[capture.Index] = ToCoordinateSystemAxis(capture.Value, unit);

            CoordinateSystem coordinateSystem = null;
            if (axes.Length == 0)
            {
                // no axes are specified, querying default coordinate systems
                if (unit.Authority != "EPSG")
                    return null;

                switch (unit.Code)
                {
                    case 9102: // degree
                        coordinateSystem = provider.CoordinateSystems["EPSG", 6422];
                        break;
                    case 9105: // grad
                        coordinateSystem = provider.CoordinateSystems["EPSG", 6403];
                        break;
                    case 9101: // radian
                        coordinateSystem = provider.CoordinateSystems["EPSG", 6428];
                        break;
                }
            }
            else
            {
                coordinateSystem = new CoordinateSystem(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, CoordinateSystemType.Ellipsoidal, axes);
            }

            // user defined
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode))
                return new GeographicCoordinateReferenceSystem(IdentifiedObject.GetIdentifier(authorityName, authorityCode), name, coordinateSystem, datum, AreaOfUse.Undefined);
            else
                return new GeographicCoordinateReferenceSystem(UnitOfMeasurement.UserDefinedIdentifier, name, coordinateSystem, datum, AreaOfUse.Undefined);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a projected coordinate reference system.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted projected coordinate reference system.</returns>
        private static ProjectedCoordinateReferenceSystem ToProjectedReferenceSystem(String source, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedProjectedReferenceSystem);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                ProjectedCoordinateReferenceSystem referenceSystem = provider.ProjectedCoordinateReferenceSystems[authorityName, code];
                if (referenceSystem != null)
                    return referenceSystem;
            }

            // convert content
            GeographicCoordinateReferenceSystem geographicReferenceSystem = ToGeographicCoordinateReferenceSystem(match.Groups["geographicReferenceSystem"].Value, provider);

            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>(match.Groups["parameter"].Captures.Count);
            foreach (Capture capture in match.Groups["parameter"].Captures)
            {
                KeyValuePair<CoordinateOperationParameter, Double> parameter = ToCoordinateOperationParameter(capture.Value, provider);
                parameters.Add(parameter.Key, parameter.Value);
            }

            CoordinateOperationMethod method = ToCoordinateOperationMethod(match.Groups["projection"].Value, provider);

            // in case the method is not supported
            if (method == null)
                return null;

            CoordinateProjection projection = provider.CoordinateProjections.WithProperties(method, parameters, AreaOfUse.Undefined, geographicReferenceSystem.Datum.Ellipsoid).FirstOrDefault();

            // in case the parameters are not matched
            if (projection == null)
                return null;

            // search by name
            foreach (ProjectedCoordinateReferenceSystem referenceSystem in provider.ProjectedCoordinateReferenceSystems.WithName(name))
            {
                if (referenceSystem.BaseReferenceSystem.Equals(geographicReferenceSystem) &&
                    referenceSystem.Projection.Equals(projection))
                    return referenceSystem;
            }

            // convert coordinate system
            UnitOfMeasurement unit = ToLinearUnit(match.Groups["unit"].Value, provider);
            CoordinateSystemAxis[] axes = new CoordinateSystemAxis[match.Groups["axis"].Captures.Count];
            foreach (Capture capture in match.Groups["axis"].Captures)
                axes[capture.Index] = ToCoordinateSystemAxis(capture.Value, unit);

            CoordinateSystem coordinateSystem = null;
            if (axes.Length == 0)
            {
                // no axes are specified, querying default coordinate systems
                if (unit.Authority != "EPSG")
                    return null;

                coordinateSystem = provider.CoordinateSystems["EPSG", 4400];
            }
            else
            {
                coordinateSystem = new CoordinateSystem(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, CoordinateSystemType.Ellipsoidal, axes);
            }

            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode))
                return new ProjectedCoordinateReferenceSystem(IdentifiedObject.GetIdentifier(authorityName, authorityCode), name, geographicReferenceSystem, coordinateSystem, projection.AreaOfUse, projection);
            else
                return new ProjectedCoordinateReferenceSystem(UnitOfMeasurement.UserDefinedIdentifier, name, geographicReferenceSystem, coordinateSystem, projection.AreaOfUse, projection);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a coordinate operation method.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted coordinate operation method.</returns>
        private static CoordinateOperationMethod ToCoordinateOperationMethod(String source, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedCoordinateOperationMethod);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            CoordinateOperationMethod method;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                method = provider.CoordinateOperationMethods[authorityName, code];
                if (method != null)
                    return method;
            }

            // search by name
            method = provider.CoordinateOperationMethods.WithName(name).FirstOrDefault();
            if (method != null)
                return method;

            // not supported method
            return null;
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a coordinate operation parameter.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted coordinate operation parameter and value.</returns>
        private static KeyValuePair<CoordinateOperationParameter, Double> ToCoordinateOperationParameter(String source, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedCoordinateOperationParameter);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String value = match.Groups["value"].Value;

            CoordinateOperationParameter parameter = provider.CoordinateOperationParameters.WithName(name).FirstOrDefault();

            return new KeyValuePair<CoordinateOperationParameter, Double>(parameter, Double.Parse(value, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a coordinate system axis.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The converted coordinate system axis.</returns>
        private static CoordinateSystemAxis ToCoordinateSystemAxis(String source, UnitOfMeasurement unit)
        {
            Match match = Regex.Match(source, PatternGroupedAxis);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String direction = match.Groups["direction"].Value.Replace('_', ' ');

            return new CoordinateSystemAxis(IdentifiedObject.UserDefinedIdentifier, IdentifiedObject.UserDefinedName, (AxisDirection)Enum.Parse(typeof(AxisDirection), direction, true), unit);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to an angular unit of measurement.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted unit of measurement.</returns>
        private static UnitOfMeasurement ToAngularUnit(String source, IReferenceProvider provider)
        {
            return ToUnitOfMeasurement(source, UnitQuantityType.Angle, provider);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a linear unit of measurement.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted unit of measurement.</returns>
        private static UnitOfMeasurement ToLinearUnit(String source, IReferenceProvider provider)
        {
            return ToUnitOfMeasurement(source, UnitQuantityType.Length, provider);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a unit of measurement.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="unitQuantityType">The unit quantity type.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted unit of measurement.</returns>
        private static UnitOfMeasurement ToUnitOfMeasurement(String source, UnitQuantityType unitQuantityType, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedUnitOfMeasurement);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            UnitOfMeasurement unit;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                unit = provider.UnitsOfMeasurement[authorityName, code];
                if (unit != null)
                    return unit;
            }

            // search by name
            unit = provider.UnitsOfMeasurement.WithName(name).FirstOrDefault();
            if (unit != null)
                return unit;

            // user defined
            Double baseMultiple = Double.Parse(match.Groups["baseMultiple"].Value, CultureInfo.InvariantCulture);

            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode))
                unit = new UnitOfMeasurement(IdentifiedObject.GetIdentifier(authorityName, authorityCode), name, null, baseMultiple, unitQuantityType);
            else
                unit = new UnitOfMeasurement(UnitOfMeasurement.UserDefinedIdentifier, name, null, baseMultiple, unitQuantityType);

            return unit;
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a meridian.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted meridian.</returns>
        private static Meridian ToMeridian(String source, UnitOfMeasurement unit, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedMeridian);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            Meridian meridian;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                meridian = provider.Meridians[authorityName, code];
                if (meridian != null)
                    return meridian;
            }

            // search by name
            meridian = provider.Meridians.WithName(name).FirstOrDefault();
            if (meridian != null)
                return meridian;

            // user defined
            Double longitude = Double.Parse(match.Groups["longitude"].Value, CultureInfo.InvariantCulture);

            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode))
                meridian = new Meridian(IdentifiedObject.GetIdentifier(authorityName, authorityCode), name, new Angle(longitude, unit));
            else
                meridian = new Meridian(UnitOfMeasurement.UserDefinedIdentifier, name, new Angle(longitude, unit));

            return meridian;
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to a geodetic datum.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="primeMeridian">The prime meridian.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted geodetic datum.</returns>
        private static GeodeticDatum ToGeodeticDatum(String source, Meridian primeMeridian, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedDatum);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                GeodeticDatum datum = provider.GeodeticDatums[authorityName, code];
                if (datum != null)
                    return datum;
            }

            // trim the "D " from the name
            if (name.Length > 2 && name.Substring(0, 2) == "D ")
                name = name.Substring(2);

            // convert content
            Ellipsoid ellipsoid = ToEllipsoid(match.Groups["ellipsoid"].Value, provider);

            // search by name
            foreach (GeodeticDatum datum in provider.GeodeticDatums.WithName(name))
            {
                if (datum.Ellipsoid.Equals(ellipsoid))
                    return datum;
            }

            // user defined
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode))
                return new GeodeticDatum(IdentifiedObject.GetIdentifier(authorityName, authorityCode), name, null, null, AreaOfUse.Undefined, ellipsoid, primeMeridian);
            else
                return new GeodeticDatum(GeodeticDatum.UserDefinedIdentifier, name, null, null, AreaOfUse.Undefined, ellipsoid, primeMeridian);
        }

        /// <summary>
        /// Converts the specified Well-known Text (WKT) to an ellipsoid.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="provider">The reference provider.</param>
        /// <returns>The converted ellipsoid.</returns>
        private static Ellipsoid ToEllipsoid(String source, IReferenceProvider provider)
        {
            Match match = Regex.Match(source, PatternGroupedEllipsoid);

            String name = match.Groups["name"].Value.Replace('_', ' ');
            String authorityName = match.Groups["authorityName"].Value;
            String authorityCode = match.Groups["authorityCode"].Value;
            Int32 code;

            Ellipsoid ellipsoid = null;

            // search by identifier
            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode) && Int32.TryParse(authorityCode, out code))
            {
                ellipsoid = provider.Ellipsoids[authorityName, code];
                if (ellipsoid != null)
                    return ellipsoid;
            }

            // search by name
            ellipsoid = provider.Ellipsoids.WithName(name).FirstOrDefault();
            if (ellipsoid != null)
                return ellipsoid;

            // user defined
            Double semiMajorAxis = Double.Parse(match.Groups["semiMajorAxis"].Value, CultureInfo.InvariantCulture);
            Double inverseFlattening = Double.Parse(match.Groups["inverseFlattening"].Value, CultureInfo.InvariantCulture);

            if (!String.IsNullOrEmpty(authorityName) && !String.IsNullOrEmpty(authorityCode))
                ellipsoid = Ellipsoid.FromInverseFlattening(IdentifiedObject.GetIdentifier(authorityName, authorityCode), name, semiMajorAxis, inverseFlattening);
            else
                ellipsoid = Ellipsoid.FromInverseFlattening(IdentifiedObject.UserDefinedIdentifier, name, semiMajorAxis, inverseFlattening);

            return ellipsoid;
        }

        /// <summary>
        /// Converts to specified coordinate list to Well-known Text (WKT) format.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="dimension">The dimension of the geometry.</param>
        private static void ConvertCoordinates(StringBuilder builder, IReadOnlyList<Coordinate> coordinates, Int32 dimension)
        {
            for (Int32 coordIndex = 0; coordIndex < coordinates.Count; coordIndex++)
            {
                if (coordIndex > 0)
                    builder.Append(", ");

                if (dimension == 3)
                {
                    builder.AppendFormat("{0} {1} {2}",
                                         coordinates[coordIndex].X.ToString("G", CultureInfo.InvariantCulture),
                                         coordinates[coordIndex].Y.ToString("G", CultureInfo.InvariantCulture),
                                         coordinates[coordIndex].Z.ToString("G", CultureInfo.InvariantCulture));
                }
                else
                {
                    builder.AppendFormat("{0} {1}",
                                         coordinates[coordIndex].X.ToString("G", CultureInfo.InvariantCulture),
                                         coordinates[coordIndex].Y.ToString("G", CultureInfo.InvariantCulture));
                }
            }
        }
    }
}
