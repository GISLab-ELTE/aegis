// <copyright file="CoordinateOperationMethods.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;

    /// <summary>
    /// Represents a collection of known <see cref="CoordinateOperationMethod" /> instances.
    /// </summary>
    public static class CoordinateOperationMethods
    {
        private static Lazy<CoordinateOperationMethod> affineParametricTransformation;
        private static Lazy<CoordinateOperationMethod> albersEqualAreaProjection;
        private static Lazy<CoordinateOperationMethod> americanPolyconicProjection;
        private static Lazy<CoordinateOperationMethod> bonne;
        private static Lazy<CoordinateOperationMethod> bonneSouthOrientated;
        private static Lazy<CoordinateOperationMethod> cassiniSoldnerProjection;
        private static Lazy<CoordinateOperationMethod> colombiaUrbanProjection;
        private static Lazy<CoordinateOperationMethod> coordinateFrameRotationGeocentricDomain;
        private static Lazy<CompoundCoordinateOperationMethod> coordinateFrameRotationGeographic2DDomain;
        private static Lazy<CompoundCoordinateOperationMethod> coordinateFrameRotationGeographic3DDomain;
        private static Lazy<CoordinateOperationMethod> complexPolynomial3;
        private static Lazy<CoordinateOperationMethod> complexPolynomial4;
        private static Lazy<CoordinateOperationMethod> equidistantCylindricalProjection;
        private static Lazy<CoordinateOperationMethod> equidistantCylindricalSphericalProjection;
        private static Lazy<CoordinateOperationMethod> generalPolynomial2;
        private static Lazy<CoordinateOperationMethod> generalPolynomial3;
        private static Lazy<CoordinateOperationMethod> generalPolynomial4;
        private static Lazy<CoordinateOperationMethod> generalPolynomial6;
        private static Lazy<CoordinateOperationMethod> geographic2DOffsets;
        private static Lazy<CoordinateOperationMethod> geographic3DTo2DConversion;
        private static Lazy<CoordinateOperationMethod> geographicToGeocentricConversion;
        private static Lazy<CoordinateOperationMethod> geocentricToTopocentricConversion;
        private static Lazy<CoordinateOperationMethod> geographicToTopocentricConversion;
        private static Lazy<CoordinateOperationMethod> geocentricTranslationGeocentricDomain;
        private static Lazy<CompoundCoordinateOperationMethod> geocentricTranslationGeographic2DDomain;
        private static Lazy<CompoundCoordinateOperationMethod> geocentricTranslationGeographic3DDomain;
        private static Lazy<CoordinateOperationMethod> gnomonicProjection;
        private static Lazy<CoordinateOperationMethod> guamProjection;
        private static Lazy<CoordinateOperationMethod> hotineObliqueMercatorAProjection;
        private static Lazy<CoordinateOperationMethod> hotineObliqueMercatorBProjection;
        private static Lazy<CoordinateOperationMethod> labordeObliqueMercatorProjection;
        private static Lazy<CoordinateOperationMethod> hyperbolicCassiniSoldnerProjection;
        private static Lazy<CoordinateOperationMethod> krovakProjection;
        private static Lazy<CoordinateOperationMethod> krovakNorthOrientedProjection;
        private static Lazy<CoordinateOperationMethod> krovakModifiedProjection;
        private static Lazy<CoordinateOperationMethod> krovakModifiedNorthOrientedProjection;
        private static Lazy<CoordinateOperationMethod> lambertAzimuthalEqualAreaProjection;
        private static Lazy<CoordinateOperationMethod> lambertAzimuthalEqualAreaSphericalProjection;
        private static Lazy<CoordinateOperationMethod> lambertConicConformal1SPProjection;
        private static Lazy<CoordinateOperationMethod> lambertConicConformal1SPWestOrientatedProjection;
        private static Lazy<CoordinateOperationMethod> lambertConicConformal2SPProjection;
        private static Lazy<CoordinateOperationMethod> lambertConicConformal2SPBelgiumProjection;
        private static Lazy<CoordinateOperationMethod> lambertConicConformal2SPMichiganProjection;
        private static Lazy<CoordinateOperationMethod> lambertConicNearConformalProjection;
        private static Lazy<CoordinateOperationMethod> lambertCylindricalEqualAreaEllipsoidalProjection;
        private static Lazy<CoordinateOperationMethod> lambertCylindricalEqualAreaSphericalProjection;
        private static Lazy<CoordinateOperationMethod> mercatorAProjection;
        private static Lazy<CoordinateOperationMethod> mercatorBProjection;
        private static Lazy<CoordinateOperationMethod> mercatorCProjection;
        private static Lazy<CoordinateOperationMethod> mercatorSphericalProjection;
        private static Lazy<CoordinateOperationMethod> modifiedAzimuthalEquidistantProjection;
        private static Lazy<CoordinateOperationMethod> molodenskyBadekasTransformation;
        private static Lazy<CoordinateOperationMethod> molodenskyTransformation;
        private static Lazy<CoordinateOperationMethod> obliqueStereographicProjection;
        private static Lazy<CoordinateOperationMethod> orthographicProjection;
        private static Lazy<CoordinateOperationMethod> p6LeftHandedSeismicBinGridTransformation;
        private static Lazy<CoordinateOperationMethod> p6RightHandedSeismicBinGridTransformation;
        private static Lazy<CoordinateOperationMethod> polarStereographicAProjection;
        private static Lazy<CoordinateOperationMethod> polarStereographicBProjection;
        private static Lazy<CoordinateOperationMethod> polarStereographicCProjection;
        private static Lazy<CoordinateOperationMethod> popularVisualisationPseudoMercatorProjection;
        private static Lazy<CoordinateOperationMethod> positionVectorTransformation;
        private static Lazy<CoordinateOperationMethod> pseudoPlateCareeProjection;
        private static Lazy<CoordinateOperationMethod> reversiblePolynomial2;
        private static Lazy<CoordinateOperationMethod> reversiblePolynomial3;
        private static Lazy<CoordinateOperationMethod> reversiblePolynomial4;
        private static Lazy<CoordinateOperationMethod> reversiblePolynomial13;
        private static Lazy<CoordinateOperationMethod> similarityTransformation;
        private static Lazy<CoordinateOperationMethod> sinusoidalProjection;
        private static Lazy<CoordinateOperationMethod> transverseMercatorProjection;
        private static Lazy<CoordinateOperationMethod> transverseMercatorSouthProjection;
        private static Lazy<CoordinateOperationMethod> transverseMercatorZonedProjection;
        private static Lazy<CoordinateOperationMethod> verticalPerspectiveOrthographicProjection;
        private static Lazy<CoordinateOperationMethod> verticalPerspectiveProjection;
        private static Lazy<CoordinateOperationMethod> worldMillerCylindricalProjection;

        /// <summary>
        /// Initializes static members of the <see cref="CoordinateOperationMethods" /> class.
        /// </summary>
        static CoordinateOperationMethods()
        {
            affineParametricTransformation = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9624", "Affine parametric transformation", true,
                                              CoordinateOperationParameters.A0,
                                              CoordinateOperationParameters.A1,
                                              CoordinateOperationParameters.A2,
                                              CoordinateOperationParameters.B0,
                                              CoordinateOperationParameters.B1,
                                              CoordinateOperationParameters.B2));
            albersEqualAreaProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9822", "Albers Equal Area", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin));
            americanPolyconicProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9818", "American Polyconic", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            bonne = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9827", "Bonne", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            bonneSouthOrientated = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9828", "Bonne South Orientated", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            cassiniSoldnerProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9806", "Cassini-Soldner", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            colombiaUrbanProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1052", "Colombia Urban", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing,
                                              CoordinateOperationParameters.ProjectionPlaneOriginHeight));
            complexPolynomial3 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9652", "Complex polynomial of degree 3 ", false,
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
                                              CoordinateOperationParameters.A6));
            complexPolynomial4 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9653", "Complex polynomial of degree 4 ", false,
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
                                              CoordinateOperationParameters.A8));
            coordinateFrameRotationGeocentricDomain = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1032", "Coordinate Frame Rotation (geocentric domain)", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.XAxisRotation,
                                              CoordinateOperationParameters.YAxisRotation,
                                              CoordinateOperationParameters.ZAxisRotation,
                                              CoordinateOperationParameters.ScaleDifference));
            coordinateFrameRotationGeographic2DDomain = new Lazy<CompoundCoordinateOperationMethod>(() =>
                new CompoundCoordinateOperationMethod("EPSG::9607", "Coordinate Frame Rotation (geog2D domain)", true,
                                                      Geographic3DTo2DConversion,
                                                      GeographicToGeocentricConversion,
                                                      CoordinateFrameRotationGeocentricDomain,
                                                      GeographicToGeocentricConversion,
                                                      Geographic3DTo2DConversion));
            coordinateFrameRotationGeographic3DDomain = new Lazy<CompoundCoordinateOperationMethod>(() =>
                new CompoundCoordinateOperationMethod("EPSG::1038", "Coordinate Frame Rotation (geog3D domain)", true,
                                                      GeographicToGeocentricConversion,
                                                      CoordinateFrameRotationGeocentricDomain,
                                                      GeographicToGeocentricConversion));
            equidistantCylindricalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1028", "Equidistant Cylindrical", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            equidistantCylindricalSphericalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1029", "Equidistant Cylindrical (Spherical)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            generalPolynomial2 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9645", "General polynomial of degree 2", false,
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
                                              CoordinateOperationParameters.Bu0v2));
            generalPolynomial3 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9646", "General polynomial of degree 3", false,
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
                                              CoordinateOperationParameters.Bu0v3));
            generalPolynomial4 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9647", "General polynomial of degree 4", false,
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
                                              CoordinateOperationParameters.Bu0v4));
            generalPolynomial6 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9648", "General polynomial of degree 6", false,
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
                                              CoordinateOperationParameters.Bu0v6));
            geographic2DOffsets = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9619", "Geographic2D offsets", true,
                                              CoordinateOperationParameters.LatitudeOffset,
                                              CoordinateOperationParameters.LongitudeOffset));
            geographic3DTo2DConversion = new Lazy<CoordinateOperationMethod>(() => new CoordinateOperationMethod("EPSG::9659", "Geographic3D to 2D conversion", true));
            geographicToGeocentricConversion = new Lazy<CoordinateOperationMethod>(() => new CoordinateOperationMethod("EPSG::9602", "Geographic/geocentric conversion", true));
            geocentricToTopocentricConversion = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9836", "Geocentric/topocentric conversion", true,
                                              CoordinateOperationParameters.GeocenticXOfTopocentricOrigin,
                                              CoordinateOperationParameters.GeocenticYOfTopocentricOrigin,
                                              CoordinateOperationParameters.GeocenticZOfTopocentricOrigin));
            geographicToTopocentricConversion = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9837", "Geographic/topocentric conversion", true,
                                              CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.LongitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin));
            geocentricTranslationGeocentricDomain = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1031", "Geocentric translations (geocentric domain)", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation));
            geocentricTranslationGeographic2DDomain = new Lazy<CompoundCoordinateOperationMethod>(() =>
                new CompoundCoordinateOperationMethod("EPSG::9603", "Geocentric translations (geog2D domain)", true,
                                                      Geographic3DTo2DConversion,
                                                      GeographicToGeocentricConversion,
                                                      GeocentricTranslationGeocentricDomain,
                                                      GeographicToGeocentricConversion,
                                                      Geographic3DTo2DConversion));
            geocentricTranslationGeographic3DDomain = new Lazy<CompoundCoordinateOperationMethod>(() =>
                new CompoundCoordinateOperationMethod("EPSG::1035", "Geocentric translations (geog3D domain)", true,
                                                      GeographicToGeocentricConversion,
                                                      GeocentricTranslationGeocentricDomain,
                                                      GeographicToGeocentricConversion));
            gnomonicProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("AEGIS::735137", "Gnomonic Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            guamProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9831", "Guam Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            hotineObliqueMercatorAProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9812", "Hotine Oblique Mercator (variant A)", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.AzimuthOfInitialLine,
                                              CoordinateOperationParameters.AngleFromRectifiedToSkewGrid,
                                              CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            hotineObliqueMercatorBProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9815", "Hotine Oblique Mercator (variant B)", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.AzimuthOfInitialLine,
                                              CoordinateOperationParameters.AngleFromRectifiedToSkewGrid,
                                              CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                              CoordinateOperationParameters.EastingAtProjectionCentre,
                                              CoordinateOperationParameters.NorthingAtProjectionCentre));
            labordeObliqueMercatorProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9813", "Laborde Oblique Mercator", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfProjectionCentre,
                                              CoordinateOperationParameters.AzimuthOfInitialLine,
                                              CoordinateOperationParameters.ScaleFactorOnInitialLine,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            hyperbolicCassiniSoldnerProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9833", "Hyperbolic Cassini-Soldner", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            krovakProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9819", "Krovak Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                              CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                              CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            krovakNorthOrientedProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1041", "Krovak North Oriented Projection", true,
                                              CoordinateOperationParameters.LatitudeOfProjectionCentre,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.CoLatitudeOfConeAxis,
                                              CoordinateOperationParameters.LatitudeOfPseudoStandardParallel,
                                              CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            krovakModifiedProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1042", "Krovak Modified Projection", true,
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
                                              CoordinateOperationParameters.C10));
            krovakModifiedNorthOrientedProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1043", "Krovak Modified North Oriented Projection", true,
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
                                              CoordinateOperationParameters.C10));
            lambertAzimuthalEqualAreaProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9820", "Lambert Azimuthal Equal Area Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            lambertAzimuthalEqualAreaSphericalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1027", "Lambert Azimuthal Equal Area (Spherical) Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            lambertConicConformal1SPProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9826", "Lambert Conic Conformal (West Orientated)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            lambertConicConformal1SPWestOrientatedProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9801", "Lambert Conic Conformal (1SP)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            lambertConicConformal2SPProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9802", "Lambert Conic Conformal (2SP)", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin));
            lambertConicConformal2SPBelgiumProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9803", "Lambert Conic Conformal (2SP Belgium)", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin));
            lambertConicConformal2SPMichiganProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1051", "Lambert Conic Conformal (2SP Michigan)", true,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LongitudeOfFalseOrigin,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LatitudeOf2ndStandardParallel,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin,
                                              CoordinateOperationParameters.EllipsoidScalingFactor));
            lambertConicNearConformalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9817", "Lambert Conic Near-Conformal", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            lambertCylindricalEqualAreaEllipsoidalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9835", "Lambert Cylindrical Equal Area (ellipsoidal case)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            lambertCylindricalEqualAreaSphericalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9834", "Lambert Cylindrical Equal Area (spherical case)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            mercatorAProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9804", "Mercator (variant A)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            mercatorBProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9805", "Mercator (variant B)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            mercatorCProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1044", "Mercator (variant C)", true,
                                              CoordinateOperationParameters.LatitudeOf1stStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LatitudeOfFalseOrigin,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin));
            mercatorSphericalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1026", "Mercator (Spherical)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            modifiedAzimuthalEquidistantProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9832", "Modified Azimuthal Equidistant Projection", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            molodenskyBadekasTransformation = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1034", "Molodensky-Badekas (geocentric domain)", false,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.XAxisRotation,
                                              CoordinateOperationParameters.YAxisRotation,
                                              CoordinateOperationParameters.ZAxisRotation,
                                              CoordinateOperationParameters.ScaleDifference,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPoint,
                                              CoordinateOperationParameters.Ordinate3OfEvaluationPoint));
            molodenskyTransformation = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9604", "Molodensky", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.SemiMajorAxisLengthDifference,
                                              CoordinateOperationParameters.FlatteningDifference));
            obliqueStereographicProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9809", "Oblique Stereographic", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            orthographicProjection = new Lazy<CoordinateOperationMethod>(() =>
               new CoordinateOperationMethod("EPSG::9840", "Orthographic", true,
                                             CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                             CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                             CoordinateOperationParameters.FalseEasting,
                                             CoordinateOperationParameters.FalseNorthing));
            p6LeftHandedSeismicBinGridTransformation = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1049", "P6 (I = J-90°) seismic bin grid transformation", true,
                                              CoordinateOperationParameters.BinGridOriginI,
                                              CoordinateOperationParameters.BinGridOriginJ,
                                              CoordinateOperationParameters.BinGridOriginEasting,
                                              CoordinateOperationParameters.BinGridOriginNorthing,
                                              CoordinateOperationParameters.ScaleFactorOfBinGrid,
                                              CoordinateOperationParameters.BinWidthOnIAxis,
                                              CoordinateOperationParameters.BinWidthOnJAxis,
                                              CoordinateOperationParameters.MapGridBearingOfBinGridJAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnIAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnJAxis));
            p6RightHandedSeismicBinGridTransformation = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9666", "P6 (I = J+90°) seismic bin grid transformation", true,
                                              CoordinateOperationParameters.BinGridOriginI,
                                              CoordinateOperationParameters.BinGridOriginJ,
                                              CoordinateOperationParameters.BinGridOriginEasting,
                                              CoordinateOperationParameters.BinGridOriginNorthing,
                                              CoordinateOperationParameters.ScaleFactorOfBinGrid,
                                              CoordinateOperationParameters.BinWidthOnIAxis,
                                              CoordinateOperationParameters.BinWidthOnJAxis,
                                              CoordinateOperationParameters.MapGridBearingOfBinGridJAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnIAxis,
                                              CoordinateOperationParameters.BinNodeIncrementOnJAxis));
            polarStereographicAProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9810", "Polar Stereographic (variant A)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            polarStereographicBProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9829", "Polar Stereographic (variant B)", true,
                                              CoordinateOperationParameters.LatitudeOfStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            polarStereographicCProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9830", "Polar Stereographic (variant C)", true,
                                              CoordinateOperationParameters.LatitudeOfStandardParallel,
                                              CoordinateOperationParameters.LongitudeOfOrigin,
                                              CoordinateOperationParameters.EastingAtFalseOrigin,
                                              CoordinateOperationParameters.NorthingAtFalseOrigin));
            popularVisualisationPseudoMercatorProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1024", "Popular Visualisation Pseudo Mercator", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            positionVectorTransformation = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::1033", "Position Vector transformation (geocentric domain)", true,
                                              CoordinateOperationParameters.XAxisTranslation,
                                              CoordinateOperationParameters.YAxisTranslation,
                                              CoordinateOperationParameters.ZAxisTranslation,
                                              CoordinateOperationParameters.XAxisRotation,
                                              CoordinateOperationParameters.YAxisRotation,
                                              CoordinateOperationParameters.ZAxisRotation,
                                              CoordinateOperationParameters.ScaleDifference));
            pseudoPlateCareeProjection = new Lazy<CoordinateOperationMethod>(() => new CoordinateOperationMethod("EPSG::9825", "Pseudo Plate Carrée", true));
            reversiblePolynomial2 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9649", "Reversible polynomial of degree 2", true,
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
                                              CoordinateOperationParameters.Bu0v2));
            reversiblePolynomial3 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9650", "Reversible polynomial of degree 3", true,
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
                                              CoordinateOperationParameters.Bu0v3));
            reversiblePolynomial4 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9651", "Reversible polynomial of degree 4", true,
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
                                              CoordinateOperationParameters.Bu0v4));
            reversiblePolynomial13 = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9654", "Reversible polynomial of degree 13", true,
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
                                              CoordinateOperationParameters.Bu4v9));
            similarityTransformation = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9621", "Similarity Transformation", true,
                                              CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget,
                                              CoordinateOperationParameters.ScaleDifference,
                                              CoordinateOperationParameters.XAxisRotation));
            sinusoidalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("ESRI::53008", "Sinusoidal Projection", true,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            transverseMercatorProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9807", "Transverse Mercator", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            transverseMercatorSouthProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9808", "Transverse Mercator (South Orientated)", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            transverseMercatorZonedProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9824", "Transverse Mercator Zoned Grid System", true,
                                              CoordinateOperationParameters.LatitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.InitialLongitude,
                                              CoordinateOperationParameters.ZoneWidth,
                                              CoordinateOperationParameters.ScaleFactorAtNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
            verticalPerspectiveOrthographicProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9839", "Vertical Perspective (Orthographic case)", false,
                                              CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.LongitudeOfTopocentricOrigin));
            verticalPerspectiveProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("EPSG::9838", "Vertical Perspective", false,
                                              CoordinateOperationParameters.LatitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.LongitudeOfTopocentricOrigin,
                                              CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin,
                                              CoordinateOperationParameters.ViewpointHeight));
            worldMillerCylindricalProjection = new Lazy<CoordinateOperationMethod>(() =>
                new CoordinateOperationMethod("ESRI::54002", "Miller Cylindrical Projection", true,
                                              CoordinateOperationParameters.LongitudeOfNaturalOrigin,
                                              CoordinateOperationParameters.FalseEasting,
                                              CoordinateOperationParameters.FalseNorthing));
        }

        /// <summary>
        /// Gets the operation method Affine parametric transformation.
        /// </summary>
        public static CoordinateOperationMethod AffineParametricTransformation { get { return affineParametricTransformation.Value; } }

        /// <summary>
        /// Gets the operation method Albers Equal Area.
        /// </summary>
        public static CoordinateOperationMethod AlbersEqualAreaProjection { get { return albersEqualAreaProjection.Value; } }

        /// <summary>
        /// Gets the operation method American Polyconic.
        /// </summary>
        public static CoordinateOperationMethod AmericanPolyconicProjection { get { return americanPolyconicProjection.Value; } }

        /// <summary>
        /// Gets the operation method Bonne.
        /// </summary>
        public static CoordinateOperationMethod Bonne { get { return bonne.Value; } }

        /// <summary>
        /// Gets the operation method Bonne South Orientated.
        /// </summary>
        public static CoordinateOperationMethod BonneSouthOrientated { get { return bonneSouthOrientated.Value; } }

        /// <summary>
        /// Gets the operation method Cassini-Soldner.
        /// </summary>
        public static CoordinateOperationMethod CassiniSoldnerProjection { get { return cassiniSoldnerProjection.Value; } }

        /// <summary>
        /// Gets the operation method Colombia Urban.
        /// </summary>
        public static CoordinateOperationMethod ColombiaUrbanProjection { get { return colombiaUrbanProjection.Value; } }

        /// <summary>
        /// Gets the operation method Complex polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod ComplexPolynomial3 { get { return complexPolynomial3.Value; } }

        /// <summary>
        /// Gets the operation method Complex polynomial of degree 4 .
        /// </summary>
        public static CoordinateOperationMethod ComplexPolynomial4 { get { return complexPolynomial4.Value; } }

        /// <summary>
        /// Gets the operation method Coordinate Frame Rotation (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod CoordinateFrameRotationGeocentricDomain { get { return coordinateFrameRotationGeocentricDomain.Value; } }

        /// <summary>
        /// Gets the operation method Coordinate Frame Rotation (geog2D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod CoordinateFrameRotationGeographic2DDomain { get { return coordinateFrameRotationGeographic2DDomain.Value; } }

        /// <summary>
        /// Gets the operation method Coordinate Frame Rotation (geog3D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod CoordinateFrameRotationGeographic3DDomain { get { return coordinateFrameRotationGeographic3DDomain.Value; } }

        /// <summary>
        /// Gets the operation method Equidistant Cylindrical.
        /// </summary>
        public static CoordinateOperationMethod EquidistantCylindricalProjection { get { return equidistantCylindricalProjection.Value; } }

        /// <summary>
        /// Gets the operation method Equidistant Cylindrical (Spherical).
        /// </summary>
        public static CoordinateOperationMethod EquidistantCylindricalSphericalProjection { get { return equidistantCylindricalSphericalProjection.Value; } }

        /// <summary>
        /// Gets the operation method General polynomial of degree 2.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial2 { get { return generalPolynomial2.Value; } }

        /// <summary>
        /// Gets the operation method General polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial3 { get { return generalPolynomial3.Value; } }

        /// <summary>
        /// Gets the operation method General polynomial of degree 4.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial4 { get { return generalPolynomial4.Value; } }

        /// <summary>
        /// Gets the operation method General polynomial of degree 6.
        /// </summary>
        public static CoordinateOperationMethod GeneralPolynomial6 { get { return generalPolynomial6.Value; } }

        /// <summary>
        /// Gets the operation method Geocentric translations (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod GeocentricTranslationGeocentricDomain { get { return geocentricTranslationGeocentricDomain.Value; } }

        /// <summary>
        /// Gets the operation method Geocentric translations (geog2D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod GeocentricTranslationGeographic2DDomain { get { return geocentricTranslationGeographic2DDomain.Value; } }

        /// <summary>
        /// Gets the operation method Geocentric translations (geog3D domain).
        /// </summary>
        public static CompoundCoordinateOperationMethod GeocentricTranslationGeographic3DDomain { get { return geocentricTranslationGeographic3DDomain.Value; } }

        /// <summary>
        /// Gets the operation method Geographic2D offsets.
        /// </summary>
        public static CoordinateOperationMethod Geographic2DOffsets { get { return geographic2DOffsets.Value; } }

        /// <summary>
        /// Gets the operation method Geographic3D to 2D conversion.
        /// </summary>
        public static CoordinateOperationMethod Geographic3DTo2DConversion { get { return geographic3DTo2DConversion.Value; } }

        /// <summary>
        /// Gets the operation method Geographic/geocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeographicToGeocentricConversion { get { return geographicToGeocentricConversion.Value; } }

        /// <summary>
        /// Gets the operation method Geocentric/topocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeocentricToTopocentricConversion { get { return geocentricToTopocentricConversion.Value; } }

        /// <summary>
        /// Gets the operation method Geographic/topocentric conversion.
        /// </summary>
        public static CoordinateOperationMethod GeographicToTopocentricConversion { get { return geographicToTopocentricConversion.Value; } }

        /// <summary>
        /// Gets the operation method Gnomonic Projection.
        /// </summary>
        public static CoordinateOperationMethod GnomonicProjection { get { return gnomonicProjection.Value; } }

        /// <summary>
        /// Gets the operation method Guam Projection.
        /// </summary>
        public static CoordinateOperationMethod GuamProjection { get { return guamProjection.Value; } }

        /// <summary>
        /// Gets the operation method Hotine Oblique Mercator (variant A).
        /// </summary>
        public static CoordinateOperationMethod HotineObliqueMercatorAProjection { get { return hotineObliqueMercatorAProjection.Value; } }

        /// <summary>
        /// Gets the operation method Hotine Oblique Mercator (variant B).
        /// </summary>
        public static CoordinateOperationMethod HotineObliqueMercatorBProjection { get { return hotineObliqueMercatorBProjection.Value; } }

        /// <summary>
        /// Gets the operation method Laborde Oblique Mercator.
        /// </summary>
        public static CoordinateOperationMethod LabordeObliqueMercatorProjection { get { return labordeObliqueMercatorProjection.Value; } }

        /// <summary>
        /// Gets the operation method Hyperbolic Cassini-Soldner.
        /// </summary>
        public static CoordinateOperationMethod HyperbolicCassiniSoldnerProjection { get { return hyperbolicCassiniSoldnerProjection.Value; } }

        /// <summary>
        /// Gets the operation method Krovak Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakProjection { get { return krovakProjection.Value; } }

        /// <summary>
        /// Gets the operation method Krovak North Oriented Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakNorthOrientedProjection { get { return krovakNorthOrientedProjection.Value; } }

        /// <summary>
        /// Gets the operation method Krovak Modified Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakModifiedProjection { get { return krovakModifiedProjection.Value; } }

        /// <summary>
        /// Gets the operation method Krovak Modified North Oriented Projection.
        /// </summary>
        public static CoordinateOperationMethod KrovakModifiedNorthOrientedProjection { get { return krovakModifiedNorthOrientedProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Azimuthal Equal Area Projection.
        /// </summary>
        public static CoordinateOperationMethod LambertAzimuthalEqualAreaProjection { get { return lambertAzimuthalEqualAreaProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Azimuthal Equal Area (Spherical) Projection.
        /// </summary>
        public static CoordinateOperationMethod LambertAzimuthalEqualAreaSphericalProjection { get { return lambertAzimuthalEqualAreaSphericalProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Conic Conformal (West Orientated).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal1SPWestOrientatedProjection { get { return lambertConicConformal1SPWestOrientatedProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Conic Conformal (1SP).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal1SPProjection { get { return lambertConicConformal1SPProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Conic Conformal (2SP).
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPProjection { get { return lambertConicConformal2SPProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Conic Conformal (2SP Belgium)
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPBelgiumProjection { get { return lambertConicConformal2SPBelgiumProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Conic Conformal (2SP Michigan)
        /// </summary>
        public static CoordinateOperationMethod LambertConicConformal2SPMichiganProjection { get { return lambertConicConformal2SPMichiganProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Conic Near-Conformal.
        /// </summary>
        public static CoordinateOperationMethod LambertConicNearConformalProjection { get { return lambertConicNearConformalProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Cylindrical Equal Area (ellipsoidal case).
        /// </summary>
        public static CoordinateOperationMethod LambertCylindricalEqualAreaEllipsoidalProjection { get { return lambertCylindricalEqualAreaEllipsoidalProjection.Value; } }

        /// <summary>
        /// Gets the operation method Lambert Cylindrical Equal Area (spherical case).
        /// </summary>
        public static CoordinateOperationMethod LambertCylindricalEqualAreaSphericalProjection { get { return lambertCylindricalEqualAreaSphericalProjection.Value; } }

        /// <summary>
        /// Gets the operation method Mercator (variant A).
        /// </summary>
        public static CoordinateOperationMethod MercatorAProjection { get { return mercatorAProjection.Value; } }

        /// <summary>
        /// Gets the operation method Mercator (variant B).
        /// </summary>
        public static CoordinateOperationMethod MercatorBProjection { get { return mercatorBProjection.Value; } }

        /// <summary>
        /// Gets the operation method Mercator (variant C).
        /// </summary>
        public static CoordinateOperationMethod MercatorCProjection { get { return mercatorCProjection.Value; } }

        /// <summary>
        /// Gets the operation method Mercator (Spherical).
        /// </summary>
        public static CoordinateOperationMethod MercatorSphericalProjection { get { return mercatorSphericalProjection.Value; } }

        /// <summary>
        /// Gets the operation method Modified Azimuthal Equidistant Projection.
        /// </summary>
        public static CoordinateOperationMethod ModifiedAzimuthalEquidistantProjection { get { return modifiedAzimuthalEquidistantProjection.Value; } }

        /// <summary>
        /// Gets the operation method Molodensky-Badekas (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod MolodenskyBadekasTransformation { get { return molodenskyBadekasTransformation.Value; } }

        /// <summary>
        /// Gets the operation method Molodensky.
        /// </summary>
        public static CoordinateOperationMethod MolodenskyTransformation { get { return molodenskyTransformation.Value; } }

        /// <summary>
        /// Gets the operation method Oblique Stereographic.
        /// </summary>
        public static CoordinateOperationMethod ObliqueStereographicProjection { get { return obliqueStereographicProjection.Value; } }

        /// <summary>
        /// Gets the operation method Orthographic.
        /// </summary>
        public static CoordinateOperationMethod OrthographicProjection { get { return orthographicProjection.Value; } }

        /// <summary>
        /// Gets the operation method P6 (I = J-90°) seismic bin grid transformation.
        /// </summary>
        public static CoordinateOperationMethod P6LeftHandedSeismicBinGridTransformation { get { return p6LeftHandedSeismicBinGridTransformation.Value; } }

        /// <summary>
        /// Gets the operation method P6 (I = J+90°) seismic bin grid transformation.
        /// </summary>
        public static CoordinateOperationMethod P6RightHandedSeismicBinGridTransformation { get { return p6RightHandedSeismicBinGridTransformation.Value; } }

        /// <summary>
        /// Gets the operation method Polar Stereographic (variant A).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicAProjection { get { return polarStereographicAProjection.Value; } }

        /// <summary>
        /// Gets the operation method Polar Stereographic (variant B).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicBProjection { get { return polarStereographicBProjection.Value; } }

        /// <summary>
        /// Gets the operation method Polar Stereographic (variant C).
        /// </summary>
        public static CoordinateOperationMethod PolarStereographicCProjection { get { return polarStereographicCProjection.Value; } }

        /// <summary>
        /// Gets the operation method Popular Visualisation Pseudo Mercator.
        /// </summary>
        public static CoordinateOperationMethod PopularVisualisationPseudoMercatorProjection { get { return popularVisualisationPseudoMercatorProjection.Value; } }

        /// <summary>
        /// Gets the operation method Position Vector transformation (geocentric domain).
        /// </summary>
        public static CoordinateOperationMethod PositionVectorTransformation { get { return positionVectorTransformation.Value; } }

        /// <summary>
        /// Gets the operation method Pseudo Plate Carrée.
        /// </summary>
        public static CoordinateOperationMethod PseudoPlateCareeProjection { get { return pseudoPlateCareeProjection.Value; } }

        /// <summary>
        /// Gets the operation method Reversible polynomial of degree 2.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial2 { get { return reversiblePolynomial2.Value; } }

        /// <summary>
        /// Gets the operation method Reversible polynomial of degree 3.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial3 { get { return reversiblePolynomial3.Value; } }

        /// <summary>
        /// Gets the operation method Reversible polynomial of degree 4.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial4 { get { return reversiblePolynomial4.Value; } }

        /// <summary>
        /// Gets the operation method Reversible polynomial of degree 13.
        /// </summary>
        public static CoordinateOperationMethod ReversiblePolynomial13 { get { return reversiblePolynomial13.Value; } }

        /// <summary>
        /// Gets the operation method Similarity Transformation.
        /// </summary>
        public static CoordinateOperationMethod SimilarityTransformation { get { return similarityTransformation.Value; } }

        /// <summary>
        /// Gets the operation method Sinusoidal Projection.
        /// </summary>
        public static CoordinateOperationMethod SinusoidalProjection { get { return sinusoidalProjection.Value; } }

        /// <summary>
        /// Gets the operation method Transverse Mercator.
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorProjection { get { return transverseMercatorProjection.Value; } }

        /// <summary>
        /// Gets the operation method Transverse Mercator (South Orientated).
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorSouthProjection { get { return transverseMercatorSouthProjection.Value; } }

        /// <summary>
        /// Gets the operation method Transverse Mercator Zoned Grid System.
        /// </summary>
        public static CoordinateOperationMethod TransverseMercatorZonedProjection { get { return transverseMercatorZonedProjection.Value; } }

        /// <summary>
        /// Gets the operation method Vertical Perspective (Orthographic case).
        /// </summary>
        public static CoordinateOperationMethod VerticalPerspectiveOrthographicProjection { get { return verticalPerspectiveOrthographicProjection.Value; } }

        /// <summary>
        /// Gets the operation method Vertical Perspective.
        /// </summary>
        public static CoordinateOperationMethod VerticalPerspectiveProjection { get { return verticalPerspectiveProjection.Value; } }

        /// <summary>
        /// Gets the operation World Miller Cylindrical Projection.
        /// </summary>
        public static CoordinateOperationMethod WorldMillerCylindricalProjection { get { return worldMillerCylindricalProjection.Value; } }
    }
}
