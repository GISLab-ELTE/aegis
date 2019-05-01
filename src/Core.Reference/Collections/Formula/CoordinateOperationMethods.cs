// <copyright file="CoordinateOperationMethods.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;

    /// <summary>
    /// Represents a collection of available <see cref="CoordinateOperationMethod" /> instances, as defined by the <see cref="http://www.epsg.org/">EPSG Geodetic Parameter Dataset</see>.
    /// </summary>
    public static class CoordinateOperationMethods
    {
        /// <summary>
        /// Affine parametric transformation.
        /// </summary>
        public static CoordinateOperationMethod AffineParametricTransformation = new CoordinateOperationMethod("EPSG::9624", "Affine parametric transformation", true,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.A1,
                                              CoordinateOperationParameters.A2,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.B1,
                                              CoordinateOperationParameters.B2);

        /// <summary>
        /// Albers Equal Area.
        /// </summary>
        public static CoordinateOperationMethod AlbersEqualAreaProjection = new CoordinateOperationMethod("EPSG::9822", "Albers Equal Area", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin);

        /// <summary>
        /// American Polyconic.
        /// </summary>
        public static CoordinateOperationMethod AmericanPolyconicProjection = new CoordinateOperationMethod("EPSG::9818", "American Polyconic", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Bonne.
        /// </summary>
        public static CoordinateOperationMethod Bonne = new CoordinateOperationMethod("EPSG::9827", "Bonne", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Bonne South Orientated.
        /// </summary>
        public static CoordinateOperationMethod BonneSouthOrientated = new CoordinateOperationMethod("EPSG::9828", "Bonne South Orientated", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Cassini-Soldner.
        /// </summary>
        public static CoordinateOperationMethod CassiniSoldnerProjection = new CoordinateOperationMethod("EPSG::9806", "Cassini-Soldner", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Colombia Urban.
        /// </summary>
        public static CoordinateOperationMethod ColombiaUrbanProjection = new CoordinateOperationMethod("EPSG::1052", "Colombia Urban", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing,
                                              CoordinateOperationParameters.ProjectionPlaneOriginHeight);

        /// <summary>
        /// Complex polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod ComplexPolynomial3 = new CoordinateOperationMethod("EPSG::9652", "Complex polynomial of degree 3", false,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                              CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                              CoordinateOperationParameters.A1,
                                              CoordinateOperationParameters.A2,
                                              CoordinateOperationParameters.A3,
                                              CoordinateOperationParameters.A4,
                                              CoordinateOperationParameters.A5,
                                              CoordinateOperationParameters.A6);

        /// <summary>
        /// Complex polynomial of degree 4.
        /// </summary>
        public static CoordinateOperationMethod ComplexPolynomial4 = new CoordinateOperationMethod("EPSG::9653", "Complex polynomial of degree 4", false,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                              CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                              CoordinateOperationParameters.A1,
                                              CoordinateOperationParameters.A2,
                                              CoordinateOperationParameters.A3,
                                              CoordinateOperationParameters.A4,
                                              CoordinateOperationParameters.A5,
                                              CoordinateOperationParameters.A6,
                                              CoordinateOperationParameters.A7,
                                              CoordinateOperationParameters.A8);

        /// <summary>
        /// Coordinate Frame Rotation (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod CoordinateFrameRotationGeocentricDomain = new CoordinateOperationMethod("EPSG::1032", "Coordinate Frame Rotation (geocentric domain)", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.XAxisRotation,
                                              CoordinateOperationParameters.YAxisRotation,
                                              CoordinateOperationParameters.ZAxisRotation,
                                              CoordinateOperationParameters.ScaleDifference);

        /// <summary>
        /// Coordinate Frame Rotation (geog2D domain).
        /// </summary>
        public static CoordinateOperationMethod CoordinateFrameRotationGeographic2DDomain = new CompoundCoordinateOperationMethod("EPSG::9607", "Coordinate Frame Rotation (geog2D domain)", true,
                                                      Geographic3DTo2DConversion,
                                                      GeographicToGeocentricConversion,
                                                      CoordinateFrameRotationGeocentricDomain,
                                                      GeographicToGeocentricConversion,
                                                      Geographic3DTo2DConversion);

        /// <summary>
        /// Coordinate Frame Rotation (geog3D domain).
        /// </summary>
        public static CoordinateOperationMethod CoordinateFrameRotationGeographic3DDomain = new CompoundCoordinateOperationMethod("EPSG::1038", "Coordinate Frame Rotation (geog3D domain)", true,
                                                      GeographicToGeocentricConversion,
                                                      CoordinateFrameRotationGeocentricDomain,
                                                      GeographicToGeocentricConversion);

        /// <summary>
        /// Equidistant Cylindrical.
        /// </summary>
        public static CoordinateOperationMethod EquidistantCylindricalProjection = new CoordinateOperationMethod("EPSG::1028", "Equidistant Cylindrical", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Equidistant Cylindrical (Spherical).
        /// </summary>
        public static CoordinateOperationMethod EquidistantCylindricalSphericalProjection = new CoordinateOperationMethod("EPSG::1029", "Equidistant Cylindrical (Spherical)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// General polynomial of degree 2.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial2 = new CoordinateOperationMethod("EPSG::9645", "General polynomial of degree 2", false,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                              CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au0v2,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2);

        /// <summary>
        /// General polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial3 = new CoordinateOperationMethod("EPSG::9646", "General polynomial of degree 3", false,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                              CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au0v2,
                                              CoordinateOperationParameters.Au3v0,
                                              CoordinateOperationParameters.Au2v1,
                                              CoordinateOperationParameters.Au1v2,
                                              CoordinateOperationParameters.Au0v3,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2,
                                              CoordinateOperationParameters.Bu3v0,
                                              CoordinateOperationParameters.Bu2v1,
                                              CoordinateOperationParameters.Bu1v2,
                                              CoordinateOperationParameters.Bu0v3);

        /// <summary>
        /// General polynomial of degree 4.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial4 = new CoordinateOperationMethod("EPSG::9647", "General polynomial of degree 4", false,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                              CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au0v2,
                                              CoordinateOperationParameters.Au3v0,
                                              CoordinateOperationParameters.Au2v1,
                                              CoordinateOperationParameters.Au1v2,
                                              CoordinateOperationParameters.Au0v3,
                                              CoordinateOperationParameters.Au4v0,
                                              CoordinateOperationParameters.Au3v1,
                                              CoordinateOperationParameters.Au2v2,
                                              CoordinateOperationParameters.Au1v3,
                                              CoordinateOperationParameters.Au0v4,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2,
                                              CoordinateOperationParameters.Bu3v0,
                                              CoordinateOperationParameters.Bu2v1,
                                              CoordinateOperationParameters.Bu1v2,
                                              CoordinateOperationParameters.Bu0v3,
                                              CoordinateOperationParameters.Bu4v0,
                                              CoordinateOperationParameters.Bu3v1,
                                              CoordinateOperationParameters.Bu2v2,
                                              CoordinateOperationParameters.Bu1v3,
                                              CoordinateOperationParameters.Bu0v4);

        /// <summary>
        /// General polynomial of degree 6.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial6 = new CoordinateOperationMethod("EPSG::9648", "General polynomial of degree 6", false,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences,
                                              CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au0v2,
                                              CoordinateOperationParameters.Au3v0,
                                              CoordinateOperationParameters.Au2v1,
                                              CoordinateOperationParameters.Au1v2,
                                              CoordinateOperationParameters.Au0v3,
                                              CoordinateOperationParameters.Au4v0,
                                              CoordinateOperationParameters.Au3v1,
                                              CoordinateOperationParameters.Au2v2,
                                              CoordinateOperationParameters.Au1v3,
                                              CoordinateOperationParameters.Au0v4,
                                              CoordinateOperationParameters.Au5v0,
                                              CoordinateOperationParameters.Au4v1,
                                              CoordinateOperationParameters.Au3v2,
                                              CoordinateOperationParameters.Au2v3,
                                              CoordinateOperationParameters.Au1v4,
                                              CoordinateOperationParameters.Au0v5,
                                              CoordinateOperationParameters.Au6v0,
                                              CoordinateOperationParameters.Au5v1,
                                              CoordinateOperationParameters.Au4v2,
                                              CoordinateOperationParameters.Au3v3,
                                              CoordinateOperationParameters.Au2v4,
                                              CoordinateOperationParameters.Au1v5,
                                              CoordinateOperationParameters.Au0v6,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2,
                                              CoordinateOperationParameters.Bu3v0,
                                              CoordinateOperationParameters.Bu2v1,
                                              CoordinateOperationParameters.Bu1v2,
                                              CoordinateOperationParameters.Bu0v3,
                                              CoordinateOperationParameters.Bu4v0,
                                              CoordinateOperationParameters.Bu3v1,
                                              CoordinateOperationParameters.Bu2v2,
                                              CoordinateOperationParameters.Bu1v3,
                                              CoordinateOperationParameters.Bu0v4,
                                              CoordinateOperationParameters.Bu5v0,
                                              CoordinateOperationParameters.Bu4v1,
                                              CoordinateOperationParameters.Bu3v2,
                                              CoordinateOperationParameters.Bu2v3,
                                              CoordinateOperationParameters.Bu1v4,
                                              CoordinateOperationParameters.Bu0v5,
                                              CoordinateOperationParameters.Bu6v0,
                                              CoordinateOperationParameters.Bu5v1,
                                              CoordinateOperationParameters.Bu4v2,
                                              CoordinateOperationParameters.Bu3v3,
                                              CoordinateOperationParameters.Bu2v4,
                                              CoordinateOperationParameters.Bu1v5,
                                              CoordinateOperationParameters.Bu0v6);

        /// <summary>
        /// Geographic 2D offsets.
        /// </summary>
        public static CoordinateOperationMethod Geographic2DOffsets = new CoordinateOperationMethod("EPSG::9619", "Geographic 2D offsets", true,
                                              CoordinateOperationParameters.LatitudeOffset,
                                              CoordinateOperationParameters.LongitudeOffset);

        /// <summary>
        /// Geographic 3D to 2D conversion.
        /// </summary>
        public static CoordinateOperationMethod Geographic3DTo2DConversion = new CoordinateOperationMethod("EPSG::9659", "Geographic 3D to 2D conversion", true);

        /// <summary>
        /// Geographic/geocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeographicToGeocentricConversion = new CoordinateOperationMethod("EPSG::9602", "Geographic/geocentric conversion", true);

        /// <summary>
        /// Geocentric/topocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeocentricToTopocentricConversion = new CoordinateOperationMethod("EPSG::9836", "Geocentric/topocentric conversion", true,
                                              CoordinateOperationParameters.GeocenticXOfTopocentricOrigin,
                                              CoordinateOperationParameters.GeocenticYOfTopocentricOrigin,
                                              CoordinateOperationParameters.GeocenticZOfTopocentricOrigin);

        /// <summary>
        /// Geographic/topocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeographicToTopocentricConversion = new CoordinateOperationMethod("EPSG::9837", "Geographic/topocentric conversion", true,
                                              CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.LongitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin);

        /// <summary>
        /// Geocentric translations (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod GeocentricTranslationGeocentricDomain = new CoordinateOperationMethod("EPSG::1031", "Geocentric translations (geocentric domain)", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation);

        /// <summary>
        /// Geocentric translations (geog2D domain).
        /// </summary>
        public static CoordinateOperationMethod GeocentricTranslationGeographic2DDomain = new CompoundCoordinateOperationMethod("EPSG::9603", "Geocentric translations (geog2D domain)", true,
                                                      Geographic3DTo2DConversion,
                                                      GeographicToGeocentricConversion,
                                                      GeocentricTranslationGeocentricDomain,
                                                      GeographicToGeocentricConversion,
                                                      Geographic3DTo2DConversion);

        /// <summary>
        /// Geocentric translations (geog3D domain).
        /// </summary>
        public static CoordinateOperationMethod GeocentricTranslationGeographic3DDomain = new CompoundCoordinateOperationMethod("EPSG::1035", "Geocentric translations (geog3D domain)", true,
                                                      GeographicToGeocentricConversion,
                                                      GeocentricTranslationGeocentricDomain,
                                                      GeographicToGeocentricConversion);

        /// <summary>
        /// Gnomonic Projection.
        /// </summary>
        public static CoordinateOperationMethod GnomonicProjection = new CoordinateOperationMethod("AEGIS::735137", "Gnomonic Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Guam Projection.
        /// </summary>
        public static CoordinateOperationMethod GuamProjection = new CoordinateOperationMethod("EPSG::9831", "Guam Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Hotine Oblique Mercator (variant A).
        /// </summary>
        public static CoordinateOperationMethod HotineObliqueMercatorAProjection = new CoordinateOperationMethod("EPSG::9812", "Hotine Oblique Mercator (variant A)", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.AzimuthOfInitialLine,
                                              CoordinateOperationParameters.AngleFromRectifiedToSkewGrid,
                                              CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Hotine Oblique Mercator (variant B).
        /// </summary>
        public static CoordinateOperationMethod HotineObliqueMercatorBProjection = new CoordinateOperationMethod("EPSG::9815", "Hotine Oblique Mercator (variant B)", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.AzimuthOfInitialLine,
                                              CoordinateOperationParameters.AngleFromRectifiedToSkewGrid,
                                              CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                              CoordinateOperationParameters.EastingAtProjectionCentre,
                                              CoordinateOperationParameters.NorthingAtProjectionCentre);

        /// <summary>
        /// Laborde Oblique Mercator.
        /// </summary>
        public static CoordinateOperationMethod LabordeObliqueMercatorProjection = new CoordinateOperationMethod("EPSG::9813", "Laborde Oblique Mercator", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.AzimuthOfInitialLine,
                                              CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Hyperbolic Cassini-Soldner.
        /// </summary>
        public static CoordinateOperationMethod HyperbolicCassiniSoldnerProjection = new CoordinateOperationMethod("EPSG::9833", "Hyperbolic Cassini-Soldner", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Krovak Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakProjection = new CoordinateOperationMethod("EPSG::9819", "Krovak Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                              CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                              CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Krovak North Oriented Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakNorthOrientedProjection = new CoordinateOperationMethod("EPSG::1041", "Krovak North Oriented Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                              CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                              CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Krovak Modified Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakModifiedProjection = new CoordinateOperationMethod("EPSG::1042", "Krovak Modified Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                              CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                              CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.C1,
                                              CoordinateOperationParameters.C2,
                                              CoordinateOperationParameters.C3,
                                              CoordinateOperationParameters.C4,
                                              CoordinateOperationParameters.C5,
                                              CoordinateOperationParameters.C6,
                                              CoordinateOperationParameters.C7,
                                              CoordinateOperationParameters.C8,
                                              CoordinateOperationParameters.C9,
                                              CoordinateOperationParameters.C10);

        /// <summary>
        /// Krovak Modified North Oriented Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakModifiedNorthOrientedProjection = new CoordinateOperationMethod("EPSG::1043", "Krovak Modified North Oriented Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                              CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                              CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.C1,
                                              CoordinateOperationParameters.C2,
                                              CoordinateOperationParameters.C3,
                                              CoordinateOperationParameters.C4,
                                              CoordinateOperationParameters.C5,
                                              CoordinateOperationParameters.C6,
                                              CoordinateOperationParameters.C7,
                                              CoordinateOperationParameters.C8,
                                              CoordinateOperationParameters.C9,
                                              CoordinateOperationParameters.C10);

        /// <summary>
        /// Lambert Azimuthal Equal Area Projection.
        /// </summary>
        public static CoordinateOperationMethod LambertAzimuthalEqualAreaProjection = new CoordinateOperationMethod("EPSG::9820", "Lambert Azimuthal Equal Area Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Lambert Azimuthal Equal Area (Spherical) Projection.
        /// </summary>
        public static CoordinateOperationMethod LambertAzimuthalEqualAreaSphericalProjection = new CoordinateOperationMethod("EPSG::1027", "Lambert Azimuthal Equal Area (Spherical) Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Lambert Conic Conformal (West Orientated).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal1SPProjection = new CoordinateOperationMethod("EPSG::9826", "Lambert Conic Conformal (West Orientated)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Lambert Conic Conformal (1SP).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal1SPWestOrientatedProjection = new CoordinateOperationMethod("EPSG::9801", "Lambert Conic Conformal (1SP)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Lambert Conic Conformal (2SP).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPProjection = new CoordinateOperationMethod("EPSG::9802", "Lambert Conic Conformal (2SP)", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin);

        /// <summary>
        /// Lambert Conic Conformal (2SP Belgium).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPBelgiumProjection = new CoordinateOperationMethod("EPSG::9803", "Lambert Conic Conformal (2SP Belgium)", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin);

        /// <summary>
        /// Lambert Conic Conformal (2SP Michigan).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPMichiganProjection = new CoordinateOperationMethod("EPSG::1051", "Lambert Conic Conformal (2SP Michigan)", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin,
                                              CoordinateOperationParameters.EllipsoidScalingFactor);

        /// <summary>
        /// Lambert Conic Near-Conformal.
        /// </summary>
        public static CoordinateOperationMethod LambertConicNearConformalProjection = new CoordinateOperationMethod("EPSG::9817", "Lambert Conic Near-Conformal", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Lambert Cylindrical Equal Area (ellipsoidal case).
        /// </summary>
        public static CoordinateOperationMethod LambertCylindricalEqualAreaEllipsoidalProjection = new CoordinateOperationMethod("EPSG::9835", "Lambert Cylindrical Equal Area (ellipsoidal case)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Lambert Cylindrical Equal Area (spherical case).
        /// </summary>
        public static CoordinateOperationMethod LambertCylindricalEqualAreaSphericalProjection = new CoordinateOperationMethod("EPSG::9834", "Lambert Cylindrical Equal Area (spherical case)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Mercator (variant A).
        /// </summary>
        public static CoordinateOperationMethod MercatorAProjection = new CoordinateOperationMethod("EPSG::9804", "Mercator (variant A)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Mercator (variant B).
        /// </summary>
        public static CoordinateOperationMethod MercatorBProjection = new CoordinateOperationMethod("EPSG::9805", "Mercator (variant B)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Mercator (variant C).
        /// </summary>
        public static CoordinateOperationMethod MercatorCProjection = new CoordinateOperationMethod("EPSG::1044", "Mercator (variant C)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin);

        /// <summary>
        /// Mercator (Spherical).
        /// </summary>
        public static CoordinateOperationMethod MercatorSphericalProjection = new CoordinateOperationMethod("EPSG::1026", "Mercator (Spherical)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Miller Cylindrical Projection.
        /// </summary>
        public static CoordinateOperationMethod MillerCylindricalProjection = new CoordinateOperationMethod("ESRI::54002", "Miller Cylindrical Projection", true,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Modified Azimuthal Equidistant Projection.
        /// </summary>
        public static CoordinateOperationMethod ModifiedAzimuthalEquidistantProjection = new CoordinateOperationMethod("EPSG::9832", "Modified Azimuthal Equidistant Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Molodensky-Badekas (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod MolodenskyBadekasTransformation = new CoordinateOperationMethod("EPSG::1034", "Molodensky-Badekas (geocentric domain)", false,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.XAxisRotation,
                                              CoordinateOperationParameters.YAxisRotation,
                                              CoordinateOperationParameters.ZAxisRotation,
                                              CoordinateOperationParameters.ScaleDifference,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate3OfEvaluationPoint);

        /// <summary>
        /// Molodensky.
        /// </summary>
        public static CoordinateOperationMethod MolodenskyTransformation = new CoordinateOperationMethod("EPSG::9604", "Molodensky", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.SemiMajorAxisLengthDifference,
                                              CoordinateOperationParameters.FlatteningDifference);

        /// <summary>
        /// Oblique Stereographic.
        /// </summary>
        public static CoordinateOperationMethod ObliqueStereographicProjection = new CoordinateOperationMethod("EPSG::9809", "Oblique Stereographic", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Orthographic.
        /// </summary>
        public static CoordinateOperationMethod OrthographicProjection = new CoordinateOperationMethod("EPSG::9840", "Orthographic", true,
                                             CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                             CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                             CoordinateOperationParameters.FalseEasting,
                                             CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// P6 (I = J-90°) seismic bin grid transformation.
        /// </summary>
        public static CoordinateOperationMethod P6LeftHandedSeismicBinGridTransformation = new CoordinateOperationMethod("EPSG::1049", "P6 (I = J-90°) seismic bin grid transformation", true,
                                              CoordinateOperationParameters.BinGridOriginI,
                                              CoordinateOperationParameters.BinGridOriginJ,
                                              CoordinateOperationParameters.BinGridOriginEasting,
                                              CoordinateOperationParameters.BinGridOriginNorthing,
                                              CoordinateOperationParameters.ScaleFactorOfBinGrid,
                                              CoordinateOperationParameters.BinWidthOnIAxis,
                                              CoordinateOperationParameters.BinWidthOnJAxis,
                                              CoordinateOperationParameters.MapGridBearingOfBinGridJAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnIAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnJAxis);

        /// <summary>
        /// P6 (I = J+90°) seismic bin grid transformation.
        /// </summary>
        public static CoordinateOperationMethod P6RightHandedSeismicBinGridTransformation = new CoordinateOperationMethod("EPSG::9666", "P6 (I = J+90°) seismic bin grid transformation", true,
                                              CoordinateOperationParameters.BinGridOriginI,
                                              CoordinateOperationParameters.BinGridOriginJ,
                                              CoordinateOperationParameters.BinGridOriginEasting,
                                              CoordinateOperationParameters.BinGridOriginNorthing,
                                              CoordinateOperationParameters.ScaleFactorOfBinGrid,
                                              CoordinateOperationParameters.BinWidthOnIAxis,
                                              CoordinateOperationParameters.BinWidthOnJAxis,
                                              CoordinateOperationParameters.MapGridBearingOfBinGridJAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnIAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnJAxis);

        /// <summary>
        /// Polar Stereographic (variant A).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicAProjection = new CoordinateOperationMethod("EPSG::9810", "Polar Stereographic (variant A)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Polar Stereographic (variant B).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicBProjection = new CoordinateOperationMethod("EPSG::9829", "Polar Stereographic (variant B)", true,
                                              CoordinateOperationParameters.LatitudeOfStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Polar Stereographic (variant C).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicCProjection = new CoordinateOperationMethod("EPSG::9830", "Polar Stereographic (variant C)", true,
                                              CoordinateOperationParameters.LatitudeOfStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin);

        /// <summary>
        /// Popular Visualisation Pseudo Mercator.
        /// </summary>
        public static CoordinateOperationMethod PopularVisualisationPseudoMercatorProjection = new CoordinateOperationMethod("EPSG::1024", "Popular Visualisation Pseudo Mercator", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Position Vector transformation (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod PositionVectorTransformation = new CoordinateOperationMethod("EPSG::1033", "Position Vector transformation (geocentric domain)", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.XAxisRotation,
                                              CoordinateOperationParameters.YAxisRotation,
                                              CoordinateOperationParameters.ZAxisRotation,
                                              CoordinateOperationParameters.ScaleDifference);

        /// <summary>
        /// Pseudo Plate Carrée.
        /// </summary>
        public static CoordinateOperationMethod PseudoPlateCareeProjection = new CoordinateOperationMethod("EPSG::9825", "Pseudo Plate Carrée", true);

        /// <summary>
        /// Reversible polynomial of degree 2.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial2 = new CoordinateOperationMethod("EPSG::9649", "Reversible polynomial of degree 2", true,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au0v2,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2);

        /// <summary>
        /// Reversible polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial3 = new CoordinateOperationMethod("EPSG::9650", "Reversible polynomial of degree 3", true,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au0v2,
                                              CoordinateOperationParameters.Au3v0,
                                              CoordinateOperationParameters.Au2v1,
                                              CoordinateOperationParameters.Au1v2,
                                              CoordinateOperationParameters.Au0v3,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2,
                                              CoordinateOperationParameters.Bu3v0,
                                              CoordinateOperationParameters.Bu2v1,
                                              CoordinateOperationParameters.Bu1v2,
                                              CoordinateOperationParameters.Bu0v3);

        /// <summary>
        /// Reversible polynomial of degree 4.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial4 = new CoordinateOperationMethod("EPSG::9651", "Reversible polynomial of degree 4", true,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au0v2,
                                              CoordinateOperationParameters.Au3v0,
                                              CoordinateOperationParameters.Au2v1,
                                              CoordinateOperationParameters.Au1v2,
                                              CoordinateOperationParameters.Au0v3,
                                              CoordinateOperationParameters.Au4v0,
                                              CoordinateOperationParameters.Au3v1,
                                              CoordinateOperationParameters.Au2v2,
                                              CoordinateOperationParameters.Au1v3,
                                              CoordinateOperationParameters.Au0v4,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2,
                                              CoordinateOperationParameters.Bu3v0,
                                              CoordinateOperationParameters.Bu2v1,
                                              CoordinateOperationParameters.Bu1v2,
                                              CoordinateOperationParameters.Bu0v3,
                                              CoordinateOperationParameters.Bu4v0,
                                              CoordinateOperationParameters.Bu3v1,
                                              CoordinateOperationParameters.Bu2v2,
                                              CoordinateOperationParameters.Bu1v3,
                                              CoordinateOperationParameters.Bu0v4);

        /// <summary>
        /// Reversible polynomial of degree 13.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial13 = new CoordinateOperationMethod("EPSG::9654", "Reversible polynomial of degree 13", true,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.ScalingFactorForCoordinateDifferences,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.Au1v0,
                                              CoordinateOperationParameters.Au0v1,
                                              CoordinateOperationParameters.Au2v0,
                                              CoordinateOperationParameters.Au1v1,
                                              CoordinateOperationParameters.Au1v9,
                                              CoordinateOperationParameters.Au3v0,
                                              CoordinateOperationParameters.Au3v9,
                                              CoordinateOperationParameters.Au2v1,
                                              CoordinateOperationParameters.Au2v7,
                                              CoordinateOperationParameters.Au4v0,
                                              CoordinateOperationParameters.Au4v1,
                                              CoordinateOperationParameters.Au5v2,
                                              CoordinateOperationParameters.Au0v8,
                                              CoordinateOperationParameters.Au9v0,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.Bu1v0,
                                              CoordinateOperationParameters.Bu0v1,
                                              CoordinateOperationParameters.Bu2v0,
                                              CoordinateOperationParameters.Bu1v1,
                                              CoordinateOperationParameters.Bu0v2,
                                              CoordinateOperationParameters.Bu3v0,
                                              CoordinateOperationParameters.Bu4v0,
                                              CoordinateOperationParameters.Bu1v3,
                                              CoordinateOperationParameters.Bu5v0,
                                              CoordinateOperationParameters.Bu2v3,
                                              CoordinateOperationParameters.Bu1v4,
                                              CoordinateOperationParameters.Bu0v5,
                                              CoordinateOperationParameters.Bu6v0,
                                              CoordinateOperationParameters.Bu3v3,
                                              CoordinateOperationParameters.Bu2v4,
                                              CoordinateOperationParameters.Bu1v5,
                                              CoordinateOperationParameters.Bu7v0,
                                              CoordinateOperationParameters.Bu6v1,
                                              CoordinateOperationParameters.Bu4v4,
                                              CoordinateOperationParameters.Bu8v1,
                                              CoordinateOperationParameters.Bu7v2,
                                              CoordinateOperationParameters.Bu2v7,
                                              CoordinateOperationParameters.Bu0v9,
                                              CoordinateOperationParameters.Bu4v6,
                                              CoordinateOperationParameters.Bu9v2,
                                              CoordinateOperationParameters.Bu8v3,
                                              CoordinateOperationParameters.Bu5v7,
                                              CoordinateOperationParameters.Bu9v4,
                                              CoordinateOperationParameters.Bu4v9);

        /// <summary>
        /// Similarity Transformation.
        /// </summary>
        public static CoordinateOperationMethod SimilarityTransformation = new CoordinateOperationMethod("EPSG::9621", "Similarity Transformation", true,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScaleDifference,
                                              CoordinateOperationParameters.XAxisRotation);

        /// <summary>
        /// Sinusoidal Projection.
        /// </summary>
        public static CoordinateOperationMethod SinusoidalProjection = new CoordinateOperationMethod("ESRI::53008", "Sinusoidal Projection", true,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Transverse Mercator.
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorProjection = new CoordinateOperationMethod("EPSG::9807", "Transverse Mercator", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Transverse Mercator (South Orientated).
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorSouthProjection = new CoordinateOperationMethod("EPSG::9808", "Transverse Mercator (South Orientated)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Transverse Mercator Zoned Grid System.
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorZonedProjection = new CoordinateOperationMethod("EPSG::9824", "Transverse Mercator Zoned Grid System", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.InitialLongitude,
                                              CoordinateOperationParameters.ZoneWidth,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing);

        /// <summary>
        /// Vertical Perspective (Orthographic case).
        /// </summary>
        public static CoordinateOperationMethod VerticalPerspectiveOrthographicProjection = new CoordinateOperationMethod("EPSG::9839", "Vertical Perspective (Orthographic case)", false,
                                              CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.LongitudeOfTopocentricOrigin);

        /// <summary>
        /// Vertical Perspective.
        /// </summary>
        public static CoordinateOperationMethod VerticalPerspectiveProjection = new CoordinateOperationMethod("EPSG::9838", "Vertical Perspective", false,
                                              CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.LongitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin,
                                              CoordinateOperationParameters.ViewpointHeight);
    }
}
