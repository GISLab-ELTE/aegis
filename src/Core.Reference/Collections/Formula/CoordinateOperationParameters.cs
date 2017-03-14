// <copyright file="CoordinateOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of known <see cref="CoordinateOperationParameter" /> instances.
    /// </summary>
    public static class CoordinateOperationParameters
    {
        private static Lazy<CoordinateOperationParameter> a0;
        private static Lazy<CoordinateOperationParameter> a1;
        private static Lazy<CoordinateOperationParameter> a2;
        private static Lazy<CoordinateOperationParameter> a3;
        private static Lazy<CoordinateOperationParameter> a4;
        private static Lazy<CoordinateOperationParameter> a5;
        private static Lazy<CoordinateOperationParameter> a6;
        private static Lazy<CoordinateOperationParameter> a7;
        private static Lazy<CoordinateOperationParameter> a8;
        private static Lazy<CoordinateOperationParameter> angleFromRectifiedToSkewGrid;
        private static Lazy<CoordinateOperationParameter> au0v1;
        private static Lazy<CoordinateOperationParameter> au0v2;
        private static Lazy<CoordinateOperationParameter> au0v3;
        private static Lazy<CoordinateOperationParameter> au0v4;
        private static Lazy<CoordinateOperationParameter> au0v5;
        private static Lazy<CoordinateOperationParameter> au0v6;
        private static Lazy<CoordinateOperationParameter> au0v8;
        private static Lazy<CoordinateOperationParameter> au1v0;
        private static Lazy<CoordinateOperationParameter> au1v1;
        private static Lazy<CoordinateOperationParameter> au1v2;
        private static Lazy<CoordinateOperationParameter> au1v3;
        private static Lazy<CoordinateOperationParameter> au1v4;
        private static Lazy<CoordinateOperationParameter> au1v5;
        private static Lazy<CoordinateOperationParameter> au1v9;
        private static Lazy<CoordinateOperationParameter> au2v0;
        private static Lazy<CoordinateOperationParameter> au2v1;
        private static Lazy<CoordinateOperationParameter> au2v2;
        private static Lazy<CoordinateOperationParameter> au2v3;
        private static Lazy<CoordinateOperationParameter> au2v4;
        private static Lazy<CoordinateOperationParameter> au2v7;
        private static Lazy<CoordinateOperationParameter> au3v0;
        private static Lazy<CoordinateOperationParameter> au3v1;
        private static Lazy<CoordinateOperationParameter> au3v2;
        private static Lazy<CoordinateOperationParameter> au3v3;
        private static Lazy<CoordinateOperationParameter> au3v9;
        private static Lazy<CoordinateOperationParameter> au4v0;
        private static Lazy<CoordinateOperationParameter> au4v1;
        private static Lazy<CoordinateOperationParameter> au4v2;
        private static Lazy<CoordinateOperationParameter> au5v0;
        private static Lazy<CoordinateOperationParameter> au5v1;
        private static Lazy<CoordinateOperationParameter> au5v2;
        private static Lazy<CoordinateOperationParameter> au6v0;
        private static Lazy<CoordinateOperationParameter> au9v0;
        private static Lazy<CoordinateOperationParameter> azimuthOfInitialLine;
        private static Lazy<CoordinateOperationParameter> b0;
        private static Lazy<CoordinateOperationParameter> b00;
        private static Lazy<CoordinateOperationParameter> b1;
        private static Lazy<CoordinateOperationParameter> b2;
        private static Lazy<CoordinateOperationParameter> b3;
        private static Lazy<CoordinateOperationParameter> binGridOriginEasting;
        private static Lazy<CoordinateOperationParameter> binGridOriginI;
        private static Lazy<CoordinateOperationParameter> binGridOriginJ;
        private static Lazy<CoordinateOperationParameter> binGridOriginNorthing;
        private static Lazy<CoordinateOperationParameter> binNodeIncrementOnIAxis;
        private static Lazy<CoordinateOperationParameter> binNodeIncrementOnJAxis;
        private static Lazy<CoordinateOperationParameter> binWidthOnIAxis;
        private static Lazy<CoordinateOperationParameter> binWidthOnJAxis;
        private static Lazy<CoordinateOperationParameter> bu0v1;
        private static Lazy<CoordinateOperationParameter> bu0v2;
        private static Lazy<CoordinateOperationParameter> bu0v3;
        private static Lazy<CoordinateOperationParameter> bu0v4;
        private static Lazy<CoordinateOperationParameter> bu0v5;
        private static Lazy<CoordinateOperationParameter> bu0v6;
        private static Lazy<CoordinateOperationParameter> bu0v9;
        private static Lazy<CoordinateOperationParameter> bu1v0;
        private static Lazy<CoordinateOperationParameter> bu1v1;
        private static Lazy<CoordinateOperationParameter> bu1v2;
        private static Lazy<CoordinateOperationParameter> bu1v3;
        private static Lazy<CoordinateOperationParameter> bu1v4;
        private static Lazy<CoordinateOperationParameter> bu1v5;
        private static Lazy<CoordinateOperationParameter> bu2v0;
        private static Lazy<CoordinateOperationParameter> bu2v1;
        private static Lazy<CoordinateOperationParameter> bu2v2;
        private static Lazy<CoordinateOperationParameter> bu2v3;
        private static Lazy<CoordinateOperationParameter> bu2v4;
        private static Lazy<CoordinateOperationParameter> bu2v7;
        private static Lazy<CoordinateOperationParameter> bu3v0;
        private static Lazy<CoordinateOperationParameter> bu3v1;
        private static Lazy<CoordinateOperationParameter> bu3v2;
        private static Lazy<CoordinateOperationParameter> bu3v3;
        private static Lazy<CoordinateOperationParameter> bu4v0;
        private static Lazy<CoordinateOperationParameter> bu4v1;
        private static Lazy<CoordinateOperationParameter> bu4v2;
        private static Lazy<CoordinateOperationParameter> bu4v4;
        private static Lazy<CoordinateOperationParameter> bu4v6;
        private static Lazy<CoordinateOperationParameter> bu4v9;
        private static Lazy<CoordinateOperationParameter> bu5v0;
        private static Lazy<CoordinateOperationParameter> bu5v1;
        private static Lazy<CoordinateOperationParameter> bu5v7;
        private static Lazy<CoordinateOperationParameter> bu6v0;
        private static Lazy<CoordinateOperationParameter> bu6v1;
        private static Lazy<CoordinateOperationParameter> bu7v0;
        private static Lazy<CoordinateOperationParameter> bu7v2;
        private static Lazy<CoordinateOperationParameter> bu8v1;
        private static Lazy<CoordinateOperationParameter> bu8v3;
        private static Lazy<CoordinateOperationParameter> bu9v2;
        private static Lazy<CoordinateOperationParameter> bu9v4;
        private static Lazy<CoordinateOperationParameter> c1;
        private static Lazy<CoordinateOperationParameter> c10;
        private static Lazy<CoordinateOperationParameter> c2;
        private static Lazy<CoordinateOperationParameter> c3;
        private static Lazy<CoordinateOperationParameter> c4;
        private static Lazy<CoordinateOperationParameter> c5;
        private static Lazy<CoordinateOperationParameter> c6;
        private static Lazy<CoordinateOperationParameter> c7;
        private static Lazy<CoordinateOperationParameter> c8;
        private static Lazy<CoordinateOperationParameter> c9;
        private static Lazy<CoordinateOperationParameter> coLatitudeOfConeAxis;
        private static Lazy<CoordinateOperationParameter> eastingAtFalseOrigin;
        private static Lazy<CoordinateOperationParameter> eastingAtProjectionCentre;
        private static Lazy<CoordinateOperationParameter> eastingOffset;
        private static Lazy<CoordinateOperationParameter> ellipsoidalHeightOfTopocentricOrigin;
        private static Lazy<CoordinateOperationParameter> ellipsoidScalingFactor;
        private static Lazy<CoordinateOperationParameter> falseEasting;
        private static Lazy<CoordinateOperationParameter> falseNorthing;
        private static Lazy<CoordinateOperationParameter> flatteningDifference;
        private static Lazy<CoordinateOperationParameter> geocenticXOfTopocentricOrigin;
        private static Lazy<CoordinateOperationParameter> geocenticYOfTopocentricOrigin;
        private static Lazy<CoordinateOperationParameter> geocenticZOfTopocentricOrigin;
        private static Lazy<CoordinateOperationParameter> initialLongitude;
        private static Lazy<CoordinateOperationParameter> latitudeOf1stStandardParallel;
        private static Lazy<CoordinateOperationParameter> latitudeOf2ndStandardParallel;
        private static Lazy<CoordinateOperationParameter> latitudeOfFalseOrigin;
        private static Lazy<CoordinateOperationParameter> latitudeOffset;
        private static Lazy<CoordinateOperationParameter> latitudeOfNaturalOrigin;
        private static Lazy<CoordinateOperationParameter> latitudeOfProjectionCentre;
        private static Lazy<CoordinateOperationParameter> latitudeOfPseudoStandardParallel;
        private static Lazy<CoordinateOperationParameter> latitudeOfStandardParallel;
        private static Lazy<CoordinateOperationParameter> latitudeOfTopocentricOrigin;
        private static Lazy<CoordinateOperationParameter> longitudeOfFalseOrigin;
        private static Lazy<CoordinateOperationParameter> longitudeOffset;
        private static Lazy<CoordinateOperationParameter> longitudeOfNaturalOrigin;
        private static Lazy<CoordinateOperationParameter> longitudeOfOrigin;
        private static Lazy<CoordinateOperationParameter> longitudeOfProjectionCentre;
        private static Lazy<CoordinateOperationParameter> longitudeOfTopocentricOrigin;
        private static Lazy<CoordinateOperationParameter> mapGridBearingOfBinGridJAxis;
        private static Lazy<CoordinateOperationParameter> northingAtFalseOrigin;
        private static Lazy<CoordinateOperationParameter> northingAtProjectionCentre;
        private static Lazy<CoordinateOperationParameter> northingOffset;
        private static Lazy<CoordinateOperationParameter> ordinate1OfEvaluationPoint;
        private static Lazy<CoordinateOperationParameter> ordinate1OfEvaluationPointInSource;
        private static Lazy<CoordinateOperationParameter> ordinate1OfEvaluationPointInTarget;
        private static Lazy<CoordinateOperationParameter> ordinate2OfEvaluationPoint;
        private static Lazy<CoordinateOperationParameter> ordinate2OfEvaluationPointInSource;
        private static Lazy<CoordinateOperationParameter> ordinate2OfEvaluationPointInTarget;
        private static Lazy<CoordinateOperationParameter> ordinate3OfEvaluationPoint;
        private static Lazy<CoordinateOperationParameter> pointScaleFactor;
        private static Lazy<CoordinateOperationParameter> projectionPlaneOriginHeight;
        private static Lazy<CoordinateOperationParameter> rotationAngleOfSourceCoordReferenceSystemAxes;
        private static Lazy<CoordinateOperationParameter> scaleDifference;
        private static Lazy<CoordinateOperationParameter> scaleFactorAtNaturalOrigin;
        private static Lazy<CoordinateOperationParameter> scaleFactorForSourceCoordReferenceSystem1stAxis;
        private static Lazy<CoordinateOperationParameter> scaleFactorForSourceCoordReferenceSystem2ndAxis;
        private static Lazy<CoordinateOperationParameter> scaleFactorOfBinGrid;
        private static Lazy<CoordinateOperationParameter> scaleFactorOnInitialLine;
        private static Lazy<CoordinateOperationParameter> scaleFactorOnPseudoStandardParallel;
        private static Lazy<CoordinateOperationParameter> scalingFactorForCoordinateDifferences;
        private static Lazy<CoordinateOperationParameter> scalingFactorForSourceCoordinateDifferences;
        private static Lazy<CoordinateOperationParameter> scalingFactorForTargetCoordinateDifferences;
        private static Lazy<CoordinateOperationParameter> semiMajorAxisLengthDifference;
        private static Lazy<CoordinateOperationParameter> verticalOffset;
        private static Lazy<CoordinateOperationParameter> viewpointHeight;
        private static Lazy<CoordinateOperationParameter> xAxisRotation;
        private static Lazy<CoordinateOperationParameter> xAxisTranslation;
        private static Lazy<CoordinateOperationParameter> yAxisRotation;
        private static Lazy<CoordinateOperationParameter> yAxisTranslation;
        private static Lazy<CoordinateOperationParameter> zAxisRotation;
        private static Lazy<CoordinateOperationParameter> zAxisTranslation;
        private static Lazy<CoordinateOperationParameter> zoneWidth;

        /// <summary>
        /// Initializes static members of the <see cref="CoordinateOperationParameters" /> class.
        /// </summary>
        static CoordinateOperationParameters()
        {
            a0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8623", "A0", "Coefficient used in affine (general parametric) and polynomial transformations."));
            a1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8624", "A1", "Coefficient used in affine (general parametric) and polynomial transformations."));
            a2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8625", "A2", "Coefficient used in affine (general parametric) and polynomial transformations."));
            a3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8626", "A3", "Coefficient used in affine (general parametric) and polynomial transformations."));
            a4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8627", "A4", "Coefficient used in polynomial transformations."));
            a5 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8628", "A5", "Coefficient used in polynomial transformations."));
            a6 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8629", "A6", "Coefficient used in polynomial transformations."));
            a7 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8630", "A7", "Coefficient used in polynomial transformations."));
            a8 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8631", "A8", "Coefficient used in polynomial transformations."));
            angleFromRectifiedToSkewGrid = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8814", "Angle from Rectified to Skew Grid", "The angle at the natural origin of an oblique projection through which the natural coordinate reference system is rotated to make the projection north axis parallel with true north."));
            au0v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8717", "Au0v1", "Coefficient used in polynomial transformations."));
            au0v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8720", "Au0v2", "Coefficient used in polynomial transformations."));
            au0v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8632", "Au0v3", "Coefficient used in polynomial transformations."));
            au0v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8637", "Au0v4", "Coefficient used in polynomial transformations."));
            au0v5 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8673", "Au0v5", "Coefficient used in polynomial transformations."));
            au0v6 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8680", "Au0v6", "Coefficient used in polynomial transformations."));
            au0v8 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8698", "Au0v8", "Coefficient used in polynomial transformations."));
            au1v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8716", "Au1v0", "Coefficient used in polynomial transformations."));
            au1v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8719", "Au1v1", "Coefficient used in polynomial transformations."));
            au1v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8723", "Au1v2", "Coefficient used in polynomial transformations."));
            au1v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8636", "Au1v3", "Coefficient used in polynomial transformations."));
            au1v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8672", "Au1v4", "Coefficient used in polynomial transformations."));
            au1v5 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8679", "Au1v5", "Coefficient used in polynomial transformations."));
            au1v9 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8701", "Au1v9", "Coefficient used in polynomial transformations."));
            au2v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8718", "Au2v0", "Coefficient used in polynomial transformations."));
            au2v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8722", "Au2v1", "Coefficient used in polynomial transformations."));
            au2v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8635", "Au2v2", "Coefficient used in polynomial transformations."));
            au2v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8671", "Au2v3", "Coefficient used in polynomial transformations."));
            au2v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8678", "Au2v4", "Coefficient used in polynomial transformations."));
            au2v7 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8700", "Au2v7", "Coefficient used in polynomial transformations."));
            au3v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8721", "Au3v0", "Coefficient used in polynomial transformations."));
            au3v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8634", "Au3v1", "Coefficient used in polynomial transformations."));
            au3v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8670", "Au3v2", "Coefficient used in polynomial transformations."));
            au3v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8677", "Au3v3", "Coefficient used in polynomial transformations."));
            au3v9 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8702", "Au3v9", "Coefficient used in polynomial transformations."));
            au4v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8633", "Au4v0", "Coefficient used in polynomial transformations."));
            au4v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8669", "Au4v1", "Coefficient used in polynomial transformations."));
            au4v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8676", "Au4v2", "Coefficient used in polynomial transformations."));
            au5v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8668", "Au5v0", "Coefficient used in polynomial transformations."));
            au5v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8675", "Au5v1", "Coefficient used in polynomial transformations."));
            au5v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8697", "Au5v2", "Coefficient used in polynomial transformations."));
            au6v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8674", "Au6v0", "Coefficient used in polynomial transformations."));
            au9v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8699", "Au9v0", "Coefficient used in polynomial transformations."));
            azimuthOfInitialLine = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8813", "Azimuth of initial line", "The azimuthal direction (north zero, east of north being positive) of the great circle which is the centre line of an oblique projection. The azimuth is given at the projection centre."));
            b0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8639", "B0", "Coefficient used in affine (general parametric) and polynomial transformations."));
            b00 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8638", "B00", "Coefficient used only in the Madrid to ED50 polynomial transformation method."));
            b1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8640", "B1", "Coefficient used in affine (general parametric) and polynomial transformations."));
            b2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8641", "B2", "Coefficient used in affine (general parametric) and polynomial transformations."));
            b3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8642", "B3", "Coefficient used in affine (general parametric) and polynomial transformations."));
            binGridOriginEasting = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8735", "Bin grid origin Easting", "The value of the map grid Easting at the bin grid definition point."));
            binGridOriginI = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8733", "Bin grid origin I", "The value of the I-axis coordinate at the bin grid definition point. The I-axis is rotated 90 degrees clockwise from the J-axis."));
            binGridOriginJ = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8734", "Bin grid origin J", "The value of the J-axis coordinate at the bin grid definition point."));
            binGridOriginNorthing = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8736", "Bin grid origin Northing", "The value of the map grid Northing at the bin grid definition point."));
            binNodeIncrementOnIAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8741", "Bin node increment on I-axis", "The numerical increment between successive bin nodes on the I-axis."));
            binNodeIncrementOnJAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8742", "Bin node increment on J-axis", "The numerical increment between successive bin nodes on the J-axis."));
            binWidthOnIAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8738", "Bin width on I-axis", "The nominal separation of bin nodes on the bin grid I-axis. (Note: the actual bin node separation is the product of the nominal separation and the scale factor of the bin grid)."));
            binWidthOnJAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8739", "Bin width on J-axis", "The nominal separation of bin nodes on the bin grid J-axis. (Note: the actual bin node separation is the product of the nominal separation and the scale factor of the bin grid)."));
            bu0v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8725", "Bu0v1", "Coefficient used in polynomial transformations."));
            bu0v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8644", "Bu0v2", "Coefficient used in polynomial transformations."));
            bu0v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8648", "Bu0v3", "Coefficient used in polynomial transformations."));
            bu0v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8653", "Bu0v4", "Coefficient used in polynomial transformations."));
            bu0v5 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8686", "Bu0v5", "Coefficient used in polynomial transformations."));
            bu0v6 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8693", "Bu0v6", "Coefficient used in polynomial transformations."));
            bu0v9 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8709", "Bu0v9", "Coefficient used in polynomial transformations."));
            bu1v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8724", "Bu1v0", "Coefficient used in polynomial transformations."));
            bu1v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8643", "Bu1v1", "Coefficient used in polynomial transformations."));
            bu1v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8647", "Bu1v2", "Coefficient used in polynomial transformations."));
            bu1v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8652", "Bu1v3", "Coefficient used in polynomial transformations."));
            bu1v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8685", "Bu1v4", "Coefficient used in polynomial transformations."));
            bu1v5 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8692", "Bu1v5", "Coefficient used in polynomial transformations."));
            bu2v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8726", "Bu2v0", "Coefficient used in polynomial transformations."));
            bu2v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8646", "Bu2v1", "Coefficient used in polynomial transformations."));
            bu2v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8651", "Bu2v2", "Coefficient used in polynomial transformations."));
            bu2v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8684", "Bu2v3", "Coefficient used in polynomial transformations."));
            bu2v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8691", "Bu2v4", "Coefficient used in polynomial transformations."));
            bu2v7 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8708", "Bu2v7", "Coefficient used in polynomial transformations."));
            bu3v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8645", "Bu3v0", "Coefficient used in polynomial transformations."));
            bu3v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8650", "Bu3v1", "Coefficient used in polynomial transformations."));
            bu3v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8683", "Bu3v2", "Coefficient used in polynomial transformations."));
            bu3v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8690", "Bu3v3", "Coefficient used in polynomial transformations."));
            bu4v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8649", "Bu4v0", "Coefficient used in polynomial transformations."));
            bu4v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8682", "Bu4v1", "Coefficient used in polynomial transformations."));
            bu4v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8689", "Bu4v2", "Coefficient used in polynomial transformations."));
            bu4v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8705", "Bu4v4", "Coefficient used in polynomial transformations."));
            bu4v6 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8710", "Bu4v6", "Coefficient used in polynomial transformations."));
            bu4v9 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8715", "Bu4v9", "Coefficient used in polynomial transformations."));
            bu5v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8681", "Bu5v0", "Coefficient used in polynomial transformations."));
            bu5v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8688", "Bu5v1", "Coefficient used in polynomial transformations."));
            bu5v7 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8713", "Bu5v7", "Coefficient used in polynomial transformations."));
            bu6v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8687", "Bu6v0", "Coefficient used in polynomial transformations."));
            bu6v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8704", "Bu6v1", "Coefficient used in polynomial transformations."));
            bu7v0 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8703", "Bu7v0", "Coefficient used in polynomial transformations."));
            bu7v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8707", "Bu7v2", "Coefficient used in polynomial transformations."));
            bu8v1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8706", "Bu8v1", "Coefficient used in polynomial transformations."));
            bu8v3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8712", "Bu8v3", "Coefficient used in polynomial transformations."));
            bu9v2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8711", "Bu9v2", "Coefficient used in polynomial transformations."));
            bu9v4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8714", "Bu9v4", "Coefficient used in polynomial transformations."));
            c1 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1026", "C1", "Coefficient used in polynomial transformations."));
            c10 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1035", "C10", "Coefficient used in polynomial transformations."));
            c2 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1027", "C2", "Coefficient used in polynomial transformations."));
            c3 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1028", "C3", "Coefficient used in polynomial transformations."));
            c4 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1029", "C4", "Coefficient used in polynomial transformations."));
            c5 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1030", "C5", "Coefficient used in polynomial transformations."));
            c6 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1031", "C6", "Coefficient used in polynomial transformations."));
            c7 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1032", "C7", "Coefficient used in polynomial transformations."));
            c8 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1033", "C8", "Coefficient used in polynomial transformations."));
            c9 = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1034", "C9", "Coefficient used in polynomial transformations."));
            coLatitudeOfConeAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1036", "Co-latitude of cone axis.", "The rotation applied to spherical coordinates for the oblique projection, measured on the conformal sphere in the plane of the meridian of origin."));
            eastingAtFalseOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8826", "Easting at false origin", "The easting value assigned to the false origin."));
            eastingAtProjectionCentre = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8816", "Easting at projection centre", "The easting value assigned to the projection centre."));
            eastingOffset = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8728", "Easting offset", "The difference between the easting values of a point in the target and source coordinate reference systems."));
            ellipsoidalHeightOfTopocentricOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8836", "Ellipsoidal height of topocentric origin", "For topocentric CSs, the ellipsoidal height of the topocentric origin."));
            ellipsoidScalingFactor = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1038", "Ellipsoid scaling factor", "Ratio by which the ellipsoid is enlarged so that survey observations are reduced to a surface above the ellipsoid surface."));
            falseEasting = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8806", "False easting", "Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Easting, FE, is the value assigned to the abscissa (east or west) axis of the projection grid at the natural origin."));
            falseNorthing = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8807", "False northing", "Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Northing, FN, is the value assigned to the ordinate (north or south) axis of the projection grid at the natural origin."));
            flatteningDifference = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8655", "Flattening difference", "The difference between the flattening values of the ellipsoids used in the target and source coordinate reference systems."));
            geocenticXOfTopocentricOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8837", "Geocentric X of topocentric origin", "For topocentric CSs, the geocentric Cartesian X coordinate of the topocentric origin."));
            geocenticYOfTopocentricOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8838", "Geocentric Y of topocentric origin", "For topocentric CSs, the geocentric Cartesian Y coordinate of the topocentric origin."));
            geocenticZOfTopocentricOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8839", "Geocentric Z of topocentric origin", "For topocentric CSs, the geocentric Cartesian Z coordinate of the topocentric origin."));
            initialLongitude = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8830", "Initial longitude", "The longitude of the western limit of the first zone of a Transverse Mercator zoned grid system."));
            latitudeOf1stStandardParallel = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8823", "Latitude of 1st standard parallel", "For a conic projection with two standard parallels, this is the latitude of one of the parallels of intersection of the cone with the ellipsoid. It is normally but not necessarily that nearest to the pole. Scale is true along this parallel."));
            latitudeOf2ndStandardParallel = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8824", "Latitude of 2nd standard parallel", "For a conic projection with two standard parallels, this is the latitude of one of the parallels at which the cone intersects with the ellipsoid. It is normally but not necessarily that nearest to the equator. Scale is true along this parallel."));
            latitudeOfFalseOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8821", "Latitude of false origin", "The latitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined."));
            latitudeOffset = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8601", "Latitude offset", "The difference between the latitude values of a point in the target and source coordinate reference systems."));
            latitudeOfNaturalOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8801", "Latitude of natural origin", null, new String[] { "Latitude of origin" }, "The latitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the latitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0)."));
            latitudeOfProjectionCentre = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8811", "Latitude of projection centre", "For an oblique projection, this is the latitude of the point at which the azimuth of the central line is defined."));
            latitudeOfPseudoStandardParallel = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8818", "Latitude of pseudo standard parallel", "Latitude of the parallel on which the conic or cylindrical projection is based. This latitude is not geographic, but is defined on the conformal sphere AFTER its rotation to obtain the oblique aspect of the projection."));
            latitudeOfStandardParallel = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8832", "Latitude of standard parallel", "For polar aspect azimuthal projections, the parallel on which the scale factor is defined to be unity."));
            latitudeOfTopocentricOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8834", "Latitude of topocentric origin", "For topocentric CSs, the latitude of the topocentric origin."));
            longitudeOfFalseOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8822", "Longitude of false origin", "The longitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined."));
            longitudeOffset = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8602", "Longitude offset", "The difference between the longitude values of a point in the target and source coordinate reference systems."));
            longitudeOfNaturalOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8802", "Longitude of natural origin", null, new String[] { "Central Meridian", "CM" }, "The longitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the longitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0). Sometimes known as \"central meridian (CM)\"."));
            longitudeOfOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8833", "Longitude of origin", "For polar aspect azimuthal projections, the meridian along which the northing axis increments and also across which parallels of latitude increment towards the north pole."));
            longitudeOfProjectionCentre = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8812", "Longitude of projection centre", "For an oblique projection, this is the longitude of the point at which the azimuth of the central line is defined."));
            longitudeOfTopocentricOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8835", "Longitude of topocentric origin", "For topocentric CSs, the longitude of the topocentric origin."));
            mapGridBearingOfBinGridJAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8740", "Map grid bearing of bin grid J-axis", "The orientation of the bin grid J-axis measured clockwise from map grid north."));
            northingAtFalseOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8827", "Northing at false origin", "The northing value assigned to the false origin."));
            northingAtProjectionCentre = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8817", "Northing at projection centre", "The northing value assigned to the projection centre."));
            northingOffset = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8829", "Northing offset", "The difference between the northing values of a point in the target and source coordinate reference systems."));
            ordinate1OfEvaluationPoint = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8617", "Ordinate 1 of evaluation point", "The value of the first ordinate of the evaluation point."));
            ordinate1OfEvaluationPointInSource = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8619", "Ordinate 1 of evaluation point in source CRS", "The value of the first ordinate of the evaluation point expressed in the source coordinate reference system."));
            ordinate1OfEvaluationPointInTarget = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8621", "Ordinate 1 of evaluation point in target CRS", "The value of the first ordinate of the evaluation point expressed in the target coordinate reference system. In the case of an affine transformation the evaluation point is the origin of the source coordinate reference system."));
            ordinate2OfEvaluationPoint = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8618", "Ordinate 2 of evaluation point", "The value of the second ordinate of the evaluation point."));
            ordinate2OfEvaluationPointInSource = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8620", "Ordinate 2 of evaluation point in source CRS", "The value of the second ordinate of the evaluation point expressed in the source coordinate reference system."));
            ordinate2OfEvaluationPointInTarget = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8622", "Ordinate 2 of evaluation point in target CRS", "The value of the second ordinate of the evaluation point expressed in the target coordinate reference system. In the case of an affine transformation the evaluation point is the origin of the source coordinate reference system."));
            ordinate3OfEvaluationPoint = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8667", "Ordinate 3 of evaluation point", "The value of the third ordinate of the evaluation point."));
            pointScaleFactor = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8663", "Point scale factor", "The point scale factor in a selected point of the target coordinate reference system. to be used as representative figure of the scale of the target coordinate reference system in a the area to which a coordinate transformation is defined."));
            projectionPlaneOriginHeight = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::1039", "Projection plane origin height", "For Colombia urban grids, the height of the projection plane at its origin."));
            rotationAngleOfSourceCoordReferenceSystemAxes = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8614", "Rotation angle of source coordinate reference system axes", "Angle (counter-clockwise positive) through which both of the source coordinate reference system axes need to rotated to coincide with the corresponding target coordinate reference system axes. Alternatively, the bearing (clockwise positive) of the source coordinate reference system Y-axis measured relative to target coordinate reference system north."));
            scaleDifference = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8611", "Scale difference", "The scale difference increased by unity equals the ratio of an the length of an arbitrary distance between two points in target and source coordinate reference systems. This is usually averaged for the intersection area of the two coordinate reference systems. If a distance of 100 km in the source coordinate reference system translates into a distance of 100.001 km in the target coordinate reference system, the scale difference is 1 ppm (the ratio being 1.000001)."));
            scaleFactorAtNaturalOrigin = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8805", "Scale factor at natural origin", "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the natural origin."));
            scaleFactorForSourceCoordReferenceSystem1stAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8612", "Scale factor for source coordinate reference system first axis", "The unit of measure of the source coordinate reference system first axis, expressed in the unit of measure of the target coordinate reference system."));
            scaleFactorForSourceCoordReferenceSystem2ndAxis = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8613", "Scale factor for source coordinate reference system second axis", "The unit of measure of the source coordinate reference system second axis, expressed in the unit of measure of the target coordinate reference system."));
            scaleFactorOfBinGrid = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8737", "Scale factor of bin grid", "The point scale factor of the map grid coordinate reference system at a point within the bin grid. Generally either the bin grid origin or the centre of the bin grid will be the chosen point."));
            scaleFactorOnInitialLine = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8815", "Scale factor on initial line", "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the projection center."));
            scaleFactorOnPseudoStandardParallel = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8819", "Scale factor on pseudo standard parallel", "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the pseudo-standard parallel."));
            scalingFactorForCoordinateDifferences = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8696", "Scaling factor for coord differences", "Used in reversible polynomial transformations to normalise coordinate differences to an acceptable numerical range. For the reverse transformation the forward target CRS becomes the reverse source CRS and forward source CRS becomes the reverse target CRS."));
            scalingFactorForSourceCoordinateDifferences = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8694", "Scaling factor for source CRS coord differences", "Used in general polynomial transformations to normalise coordinate differences to an acceptable numerical range."));
            scalingFactorForTargetCoordinateDifferences = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8695", "Scaling factor for target CRS coord differences", "Used in general polynomial transformations to normalise coordinate differences to an acceptable numerical range."));
            semiMajorAxisLengthDifference = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8654", "Semi-major axis length difference", "The difference between the semi-major axis values of the ellipsoids used in the target and source coordinate reference systems."));
            verticalOffset = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8603", "Vertical offset", "The difference between the height or depth values of a point in the target and source coordinate reference systems."));
            viewpointHeight = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8840", "Viewpoint height", "For vertical perspective projections, the height of viewpoint above the topocentric origin."));
            xAxisRotation = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8608", "X-axis rotation", "The angular difference between the Y and Z axes directions of target and source coordinate reference systems. This is a rotation about the X axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis)."));
            xAxisTranslation = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8605", "X-axis translation", "The difference between the X values of a point in the target and source coordinate reference systems."));
            yAxisRotation = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8609", "Y-axis rotation", "The angular difference between the X and Z axes directions of target and source coordinate reference systems. This is a rotation about the Y axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis)."));
            yAxisTranslation = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8606", "Y-axis translation", "The difference between the Y values of a point in the target and source coordinate reference systems."));
            zAxisRotation = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8610", "Z-axis rotation", "The angular difference between the X and Y axes directions of target and source coordinate reference systems. This is a rotation about the Z axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis)."));
            zAxisTranslation = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8607", "Z-axis translation", "The difference between the Z values of a point in the target and source coordinate reference systems."));
            zoneWidth = new Lazy<CoordinateOperationParameter>(() => new CoordinateOperationParameter("EPSG::8831", "Zone width", "The longitude width of a zone of a Transverse Mercator zoned grid system."));
        }

        /// <summary>
        /// Gets A0.
        /// </summary>
        public static CoordinateOperationParameter A0 { get { return a0.Value; } }

        /// <summary>
        /// Gets A1.
        /// </summary>
        public static CoordinateOperationParameter A1 { get { return a1.Value; } }

        /// <summary>
        /// Gets A2.
        /// </summary>
        public static CoordinateOperationParameter A2 { get { return a2.Value; } }

        /// <summary>
        /// Gets A3.
        /// </summary>
        public static CoordinateOperationParameter A3 { get { return a3.Value; } }

        /// <summary>
        /// Gets A4.
        /// </summary>
        public static CoordinateOperationParameter A4 { get { return a4.Value; } }

        /// <summary>
        /// Gets A5.
        /// </summary>
        public static CoordinateOperationParameter A5 { get { return a5.Value; } }

        /// <summary>
        /// Gets A6.
        /// </summary>
        public static CoordinateOperationParameter A6 { get { return a6.Value; } }

        /// <summary>
        /// Gets A7.
        /// </summary>
        public static CoordinateOperationParameter A7 { get { return a7.Value; } }

        /// <summary>
        /// Gets A8.
        /// </summary>
        public static CoordinateOperationParameter A8 { get { return a8.Value; } }

        /// <summary>
        /// Gets the angle from Rectified to Skew Grid.
        /// </summary>
        public static CoordinateOperationParameter AngleFromRectifiedToSkewGrid { get { return angleFromRectifiedToSkewGrid.Value; } }

        /// <summary>
        /// Gets Au0v1.
        /// </summary>
        public static CoordinateOperationParameter Au0v1 { get { return au0v1.Value; } }

        /// <summary>
        /// Gets Au0v2.
        /// </summary>
        public static CoordinateOperationParameter Au0v2 { get { return au0v2.Value; } }

        /// <summary>
        /// Gets Au0v3.
        /// </summary>
        public static CoordinateOperationParameter Au0v3 { get { return au0v3.Value; } }

        /// <summary>
        /// Gets Au0v4.
        /// </summary>
        public static CoordinateOperationParameter Au0v4 { get { return au0v4.Value; } }

        /// <summary>
        /// Gets Au0v5.
        /// </summary>
        public static CoordinateOperationParameter Au0v5 { get { return au0v5.Value; } }

        /// <summary>
        /// Gets Au0v6.
        /// </summary>
        public static CoordinateOperationParameter Au0v6 { get { return au0v6.Value; } }

        /// <summary>
        /// Gets Au0v8.
        /// </summary>
        public static CoordinateOperationParameter Au0v8 { get { return au0v8.Value; } }

        /// <summary>
        /// Gets Au1v0.
        /// </summary>
        public static CoordinateOperationParameter Au1v0 { get { return au1v0.Value; } }

        /// <summary>
        /// Gets Au1v1.
        /// </summary>
        public static CoordinateOperationParameter Au1v1 { get { return au1v1.Value; } }

        /// <summary>
        /// Gets Au1v2.
        /// </summary>
        public static CoordinateOperationParameter Au1v2 { get { return au1v2.Value; } }

        /// <summary>
        /// Gets Au1v3.
        /// </summary>
        public static CoordinateOperationParameter Au1v3 { get { return au1v3.Value; } }

        /// <summary>
        /// Gets Au1v4.
        /// </summary>
        public static CoordinateOperationParameter Au1v4 { get { return au1v4.Value; } }

        /// <summary>
        /// Gets Au1v5.
        /// </summary>
        public static CoordinateOperationParameter Au1v5 { get { return au1v5.Value; } }

        /// <summary>
        /// Gets Au1v9.
        /// </summary>
        public static CoordinateOperationParameter Au1v9 { get { return au1v9.Value; } }

        /// <summary>
        /// Gets Au2v0.
        /// </summary>
        public static CoordinateOperationParameter Au2v0 { get { return au2v0.Value; } }

        /// <summary>
        /// Gets Au2v1.
        /// </summary>
        public static CoordinateOperationParameter Au2v1 { get { return au2v1.Value; } }

        /// <summary>
        /// Gets Au2v2.
        /// </summary>
        public static CoordinateOperationParameter Au2v2 { get { return au2v2.Value; } }

        /// <summary>
        /// Gets Au2v3.
        /// </summary>
        public static CoordinateOperationParameter Au2v3 { get { return au2v3.Value; } }

        /// <summary>
        /// Gets Au2v4.
        /// </summary>
        public static CoordinateOperationParameter Au2v4 { get { return au2v4.Value; } }

        /// <summary>
        /// Gets Au2v7.
        /// </summary>
        public static CoordinateOperationParameter Au2v7 { get { return au2v7.Value; } }

        /// <summary>
        /// Gets Au3v0.
        /// </summary>
        public static CoordinateOperationParameter Au3v0 { get { return au3v0.Value; } }

        /// <summary>
        /// Gets Au3v1.
        /// </summary>
        public static CoordinateOperationParameter Au3v1 { get { return au3v1.Value; } }

        /// <summary>
        /// Gets Au3v2.
        /// </summary>
        public static CoordinateOperationParameter Au3v2 { get { return au3v2.Value; } }

        /// <summary>
        /// Gets Au3v3.
        /// </summary>
        public static CoordinateOperationParameter Au3v3 { get { return au3v3.Value; } }

        /// <summary>
        /// Gets Au3v9.
        /// </summary>
        public static CoordinateOperationParameter Au3v9 { get { return au3v9.Value; } }

        /// <summary>
        /// Gets Au4v0.
        /// </summary>
        public static CoordinateOperationParameter Au4v0 { get { return au4v0.Value; } }

        /// <summary>
        /// Gets Au4v1.
        /// </summary>
        public static CoordinateOperationParameter Au4v1 { get { return au4v1.Value; } }

        /// <summary>
        /// Gets Au4v2.
        /// </summary>
        public static CoordinateOperationParameter Au4v2 { get { return au4v2.Value; } }

        /// <summary>
        /// Gets Au5v0.
        /// </summary>
        public static CoordinateOperationParameter Au5v0 { get { return au5v0.Value; } }

        /// <summary>
        /// Gets Au5v1.
        /// </summary>
        public static CoordinateOperationParameter Au5v1 { get { return au5v1.Value; } }

        /// <summary>
        /// Gets Au5v2.
        /// </summary>
        public static CoordinateOperationParameter Au5v2 { get { return au5v2.Value; } }

        /// <summary>
        /// Gets Au6v0.
        /// </summary>
        public static CoordinateOperationParameter Au6v0 { get { return au6v0.Value; } }

        /// <summary>
        /// Gets Au9v0.
        /// </summary>
        public static CoordinateOperationParameter Au9v0 { get { return au9v0.Value; } }

        /// <summary>
        /// Gets azimuth of initial line.
        /// </summary>
        public static CoordinateOperationParameter AzimuthOfInitialLine { get { return azimuthOfInitialLine.Value; } }

        /// <summary>
        /// Gets B00.
        /// </summary>
        public static CoordinateOperationParameter B00 { get { return b00.Value; } }

        /// <summary>
        /// Gets B0.
        /// </summary>
        public static CoordinateOperationParameter B0 { get { return b0.Value; } }

        /// <summary>
        /// Gets B1.
        /// </summary>
        public static CoordinateOperationParameter B1 { get { return b1.Value; } }

        /// <summary>
        /// Gets B2.
        /// </summary>
        public static CoordinateOperationParameter B2 { get { return b2.Value; } }

        /// <summary>
        /// Gets B3.
        /// </summary>
        public static CoordinateOperationParameter B3 { get { return b3.Value; } }

        /// <summary>
        /// Gets Bin grid origin Easting.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginEasting { get { return binGridOriginEasting.Value; } }

        /// <summary>
        /// Gets Bin grid origin I.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginI { get { return binGridOriginI.Value; } }

        /// <summary>
        /// Gets Bin grid origin J.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginJ { get { return binGridOriginJ.Value; } }

        /// <summary>
        /// Gets Bin grid origin Northing.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginNorthing { get { return binGridOriginNorthing.Value; } }

        /// <summary>
        /// Gets Bin node increment on I-axis.
        /// </summary>
        public static CoordinateOperationParameter BinNodeIncrementOnIAxis { get { return binNodeIncrementOnIAxis.Value; } }

        /// <summary>
        /// Gets Bin node increment on J-axis.
        /// </summary>
        public static CoordinateOperationParameter BinNodeIncrementOnJAxis { get { return binNodeIncrementOnJAxis.Value; } }

        /// <summary>
        /// Gets Bin width on I-axis.
        /// </summary>
        public static CoordinateOperationParameter BinWidthOnIAxis { get { return binWidthOnIAxis.Value; } }

        /// <summary>
        /// Gets Bin width on J-axis.
        /// </summary>
        public static CoordinateOperationParameter BinWidthOnJAxis { get { return binWidthOnJAxis.Value; } }

        /// <summary>
        /// Gets Bu0v1.
        /// </summary>
        public static CoordinateOperationParameter Bu0v1 { get { return bu0v1.Value; } }

        /// <summary>
        /// Gets Bu0v2.
        /// </summary>
        public static CoordinateOperationParameter Bu0v2 { get { return bu0v2.Value; } }

        /// <summary>
        /// Gets Bu1v0.
        /// </summary>
        public static CoordinateOperationParameter Bu1v0 { get { return bu1v0.Value; } }

        /// <summary>
        /// Gets Bu1v4.
        /// </summary>
        public static CoordinateOperationParameter Bu1v4 { get { return bu1v4.Value; } }

        /// <summary>
        /// Gets Bu0v3.
        /// </summary>
        public static CoordinateOperationParameter Bu0v3 { get { return bu0v3.Value; } }

        /// <summary>
        /// Gets Bu0v4.
        /// </summary>
        public static CoordinateOperationParameter Bu0v4 { get { return bu0v4.Value; } }

        /// <summary>
        /// Gets Bu0v5.
        /// </summary>
        public static CoordinateOperationParameter Bu0v5 { get { return bu0v5.Value; } }

        /// <summary>
        /// Gets Bu0v6.
        /// </summary>
        public static CoordinateOperationParameter Bu0v6 { get { return bu0v6.Value; } }

        /// <summary>
        /// Gets Bu0v9.
        /// </summary>
        public static CoordinateOperationParameter Bu0v9 { get { return bu0v9.Value; } }

        /// <summary>
        /// Gets Bu1v1.
        /// </summary>
        public static CoordinateOperationParameter Bu1v1 { get { return bu1v1.Value; } }

        /// <summary>
        /// Gets Bu1v2.
        /// </summary>
        public static CoordinateOperationParameter Bu1v2 { get { return bu1v2.Value; } }

        /// <summary>
        /// Gets Bu1v3.
        /// </summary>
        public static CoordinateOperationParameter Bu1v3 { get { return bu1v3.Value; } }

        /// <summary>
        /// Gets Bu1v5.
        /// </summary>
        public static CoordinateOperationParameter Bu1v5 { get { return bu1v5.Value; } }

        /// <summary>
        /// Gets Bu2v0.
        /// </summary>
        public static CoordinateOperationParameter Bu2v0 { get { return bu2v0.Value; } }

        /// <summary>
        /// Gets Bu2v1.
        /// </summary>
        public static CoordinateOperationParameter Bu2v1 { get { return bu2v1.Value; } }

        /// <summary>
        /// Gets Bu2v2.
        /// </summary>
        public static CoordinateOperationParameter Bu2v2 { get { return bu2v2.Value; } }

        /// <summary>
        /// Gets Bu2v3.
        /// </summary>
        public static CoordinateOperationParameter Bu2v3 { get { return bu2v3.Value; } }

        /// <summary>
        /// Gets Bu2v4.
        /// </summary>
        public static CoordinateOperationParameter Bu2v4 { get { return bu2v4.Value; } }

        /// <summary>
        /// Gets Bu2v7
        /// </summary>
        public static CoordinateOperationParameter Bu2v7 { get { return bu2v7.Value; } }

        /// <summary>
        /// Gets Bu3v0.
        /// </summary>
        public static CoordinateOperationParameter Bu3v0 { get { return bu3v0.Value; } }

        /// <summary>
        /// Gets Bu3v1.
        /// </summary>
        public static CoordinateOperationParameter Bu3v1 { get { return bu3v1.Value; } }

        /// <summary>
        /// Gets Bu3v2.
        /// </summary>
        public static CoordinateOperationParameter Bu3v2 { get { return bu3v2.Value; } }

        /// <summary>
        /// Gets Bu3v3.
        /// </summary>
        public static CoordinateOperationParameter Bu3v3 { get { return bu3v3.Value; } }

        /// <summary>
        /// Gets Bu4v0.
        /// </summary>
        public static CoordinateOperationParameter Bu4v0 { get { return bu4v0.Value; } }

        /// <summary>
        /// Gets Bu4v1.
        /// </summary>
        public static CoordinateOperationParameter Bu4v1 { get { return bu4v1.Value; } }

        /// <summary>
        /// Gets Bu4v2.
        /// </summary>
        public static CoordinateOperationParameter Bu4v2 { get { return bu4v2.Value; } }

        /// <summary>
        /// Gets Bu4v4.
        /// </summary>
        public static CoordinateOperationParameter Bu4v4 { get { return bu4v4.Value; } }

        /// <summary>
        /// Gets Bu4v6.
        /// </summary>
        public static CoordinateOperationParameter Bu4v6 { get { return bu4v6.Value; } }

        /// <summary>
        /// Gets Bu4v9.
        /// </summary>
        public static CoordinateOperationParameter Bu4v9 { get { return bu4v9.Value; } }

        /// <summary>
        /// Gets Bu5v0.
        /// </summary>
        public static CoordinateOperationParameter Bu5v0 { get { return bu5v0.Value; } }

        /// <summary>
        /// Gets Bu5v1.
        /// </summary>
        public static CoordinateOperationParameter Bu5v1 { get { return bu5v1.Value; } }

        /// <summary>
        /// Gets Bu5v7.
        /// </summary>
        public static CoordinateOperationParameter Bu5v7 { get { return bu5v7.Value; } }

        /// <summary>
        /// Gets Bu6v0.
        /// </summary>
        public static CoordinateOperationParameter Bu6v0 { get { return bu6v0.Value; } }

        /// <summary>
        /// Gets Bu6v1.
        /// </summary>
        public static CoordinateOperationParameter Bu6v1 { get { return bu6v1.Value; } }

        /// <summary>
        /// Gets Bu7v0.
        /// </summary>
        public static CoordinateOperationParameter Bu7v0 { get { return bu7v0.Value; } }

        /// <summary>
        /// Gets Bu7v2.
        /// </summary>
        public static CoordinateOperationParameter Bu7v2 { get { return bu7v2.Value; } }

        /// <summary>
        /// Gets Bu8v1.
        /// </summary>
        public static CoordinateOperationParameter Bu8v1 { get { return bu8v1.Value; } }

        /// <summary>
        /// Gets Bu8v3.
        /// </summary>
        public static CoordinateOperationParameter Bu8v3 { get { return bu8v3.Value; } }

        /// <summary>
        /// Gets Bu9v2.
        /// </summary>
        public static CoordinateOperationParameter Bu9v2 { get { return bu9v2.Value; } }

        /// <summary>
        /// Gets Bu9v4.
        /// </summary>
        public static CoordinateOperationParameter Bu9v4 { get { return bu9v4.Value; } }

        /// <summary>
        /// Gets C1.
        /// </summary>
        public static CoordinateOperationParameter C1 { get { return c1.Value; } }

        /// <summary>
        /// Gets C10.
        /// </summary>
        public static CoordinateOperationParameter C10 { get { return c10.Value; } }

        /// <summary>
        /// Gets C2.
        /// </summary>
        public static CoordinateOperationParameter C2 { get { return c2.Value; } }

        /// <summary>
        /// Gets C3.
        /// </summary>
        public static CoordinateOperationParameter C3 { get { return c3.Value; } }

        /// <summary>
        /// Gets C4.
        /// </summary>
        public static CoordinateOperationParameter C4 { get { return c4.Value; } }

        /// <summary>
        /// Gets C5.
        /// </summary>
        public static CoordinateOperationParameter C5 { get { return c5.Value; } }

        /// <summary>
        /// Gets C6.
        /// </summary>
        public static CoordinateOperationParameter C6 { get { return c6.Value; } }

        /// <summary>
        /// Gets C7.
        /// </summary>
        public static CoordinateOperationParameter C7 { get { return c7.Value; } }

        /// <summary>
        /// Gets C8.
        /// </summary>
        public static CoordinateOperationParameter C8 { get { return c8.Value; } }

        /// <summary>
        /// Gets C9.
        /// </summary>
        public static CoordinateOperationParameter C9 { get { return c9.Value; } }

        /// <summary>
        /// Gets Co-latitude of cone axis.
        /// </summary>
        public static CoordinateOperationParameter CoLatitudeOfConeAxis { get { return coLatitudeOfConeAxis.Value; } }

        /// <summary>
        /// Gets easting at false origin.
        /// </summary>
        public static CoordinateOperationParameter EastingAtFalseOrigin { get { return eastingAtFalseOrigin.Value; } }

        /// <summary>
        /// Gets easting at projection centre.
        /// </summary>
        public static CoordinateOperationParameter EastingAtProjectionCentre { get { return eastingAtProjectionCentre.Value; } }

        /// <summary>
        /// Gets ellipsoidal height of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter EllipsoidalHeightOfTopocentricOrigin { get { return ellipsoidalHeightOfTopocentricOrigin.Value; } }

        /// <summary>
        /// Gets ellipsoid scaling factor.
        /// </summary>
        public static CoordinateOperationParameter EllipsoidScalingFactor { get { return ellipsoidScalingFactor.Value; } }

        /// <summary>
        /// Gets easting offset.
        /// </summary>
        public static CoordinateOperationParameter EastingOffset { get { return eastingOffset.Value; } }

        /// <summary>
        /// Gets false easting.
        /// </summary>
        public static CoordinateOperationParameter FalseEasting { get { return falseEasting.Value; } }

        /// <summary>
        /// Gets false northing.
        /// </summary>
        public static CoordinateOperationParameter FalseNorthing { get { return falseNorthing.Value; } }

        /// <summary>
        /// Gets flattening difference.
        /// </summary>
        public static CoordinateOperationParameter FlatteningDifference { get { return flatteningDifference.Value; } }

        /// <summary>
        /// Gets geocentric X of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticXOfTopocentricOrigin { get { return geocenticXOfTopocentricOrigin.Value; } }

        /// <summary>
        /// Gets geocentric Y of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticYOfTopocentricOrigin { get { return geocenticYOfTopocentricOrigin.Value; } }

        /// <summary>
        /// Gets geocentric Z of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticZOfTopocentricOrigin { get { return geocenticZOfTopocentricOrigin.Value; } }

        /// <summary>
        /// Gets initial longitude.
        /// </summary>
        public static CoordinateOperationParameter InitialLongitude { get { return initialLongitude.Value; } }

        /// <summary>
        /// Gets latitude of 1st standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOf1stStandardParallel { get { return latitudeOf1stStandardParallel.Value; } }

        /// <summary>
        /// Gets latitude of 2nd standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOf2ndStandardParallel { get { return latitudeOf2ndStandardParallel.Value; } }

        /// <summary>
        /// Gets latitude of false origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfFalseOrigin { get { return latitudeOfFalseOrigin.Value; } }

        /// <summary>
        /// Gets latitude offset.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOffset { get { return latitudeOffset.Value; } }

        /// <summary>
        /// Gets latitude of natural origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfNaturalOrigin { get { return latitudeOfNaturalOrigin.Value; } }

        /// <summary>
        /// Gets latitude of projection centre.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfProjectionCentre { get { return latitudeOfProjectionCentre.Value; } }

        /// <summary>
        /// Gets latitude of pseudo standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfPseudoStandardParallel { get { return latitudeOfPseudoStandardParallel.Value; } }

        /// <summary>
        /// Gets latitude of standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfStandardParallel { get { return latitudeOfStandardParallel.Value; } }

        /// <summary>
        /// Gets latitude of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfTopocentricOrigin { get { return latitudeOfTopocentricOrigin.Value; } }

        /// <summary>
        /// Gets longitude of false origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfFalseOrigin { get { return longitudeOfFalseOrigin.Value; } }

        /// <summary>
        /// Gets longitude offset.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOffset { get { return longitudeOffset.Value; } }

        /// <summary>
        /// Gets longitude of natural origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfNaturalOrigin { get { return longitudeOfNaturalOrigin.Value; } }

        /// <summary>
        /// Gets longitude of origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfOrigin { get { return longitudeOfOrigin.Value; } }

        /// <summary>
        /// Gets longitude of projection centre.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfProjectionCentre { get { return longitudeOfProjectionCentre.Value; } }

        /// <summary>
        /// Gets longitude of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfTopocentricOrigin { get { return longitudeOfTopocentricOrigin.Value; } }

        /// <summary>
        /// Gets map grid bearing of bin grid J-axis.
        /// </summary>
        public static CoordinateOperationParameter MapGridBearingOfBinGridJAxis { get { return mapGridBearingOfBinGridJAxis.Value; } }

        /// <summary>
        /// Gets northing at false origin.
        /// </summary>
        public static CoordinateOperationParameter NorthingAtFalseOrigin { get { return northingAtFalseOrigin.Value; } }

        /// <summary>
        /// Gets northing at projection centre.
        /// </summary>
        public static CoordinateOperationParameter NorthingAtProjectionCentre { get { return northingAtProjectionCentre.Value; } }

        /// <summary>
        /// Gets northing offset.
        /// </summary>
        public static CoordinateOperationParameter NorthingOffset { get { return northingOffset.Value; } }

        /// <summary>
        /// Gets ordinate 1 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPoint { get { return ordinate1OfEvaluationPoint.Value; } }

        /// <summary>
        /// Gets ordinate 1 of evaluation point in source CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPointInSource { get { return ordinate1OfEvaluationPointInSource.Value; } }

        /// <summary>
        /// Gets ordinate 1 of evaluation point in target CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPointInTarget { get { return ordinate1OfEvaluationPointInTarget.Value; } }

        /// <summary>
        /// Gets ordinate 2 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPoint { get { return ordinate2OfEvaluationPoint.Value; } }

        /// <summary>
        /// Gets ordinate 2 of evaluation point in source CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPointInSource { get { return ordinate2OfEvaluationPointInSource.Value; } }

        /// <summary>
        /// Gets ordinate 2 of evaluation point in target CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPointInTarget { get { return ordinate2OfEvaluationPointInTarget.Value; } }

        /// <summary>
        /// Gets ordinate 3 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate3OfEvaluationPoint { get { return ordinate3OfEvaluationPoint.Value; } }

        /// <summary>
        /// Gets rotation angle of source coordinate reference system axes.
        /// </summary>
        public static CoordinateOperationParameter RotationAngleOfSourceCoordReferenceSystemAxes { get { return rotationAngleOfSourceCoordReferenceSystemAxes.Value; } }

        /// <summary>
        /// Gets point scale factor.
        /// </summary>
        public static CoordinateOperationParameter PointScaleFactor { get { return pointScaleFactor.Value; } }

        /// <summary>
        /// Gets projection plane origin height.
        /// </summary>
        public static CoordinateOperationParameter ProjectionPlaneOriginHeight { get { return projectionPlaneOriginHeight.Value; } }

        /// <summary>
        /// Gets scale difference.
        /// </summary>
        public static CoordinateOperationParameter ScaleDifference { get { return scaleDifference.Value; } }

        /// <summary>
        /// Gets scale factor at natural origin.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorAtNaturalOrigin { get { return scaleFactorAtNaturalOrigin.Value; } }

        /// <summary>
        /// Gets scale factor on initial line.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOnInitialLine { get { return scaleFactorOnInitialLine.Value; } }

        /// <summary>
        /// Gets scale factor on pseudo standard parallel.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOnPseudoStandardParallel { get { return scaleFactorOnPseudoStandardParallel.Value; } }

        /// <summary>
        /// Gets scale factor of bin grid.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOfBinGrid { get { return scaleFactorOfBinGrid.Value; } }

        /// <summary>
        /// Gets scaling factor for coordinate differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForCoordinateDifferences { get { return scalingFactorForCoordinateDifferences.Value; } }

        /// <summary>
        /// Gets scaling factor for source CRS coordinate differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForSourceCoordinateDifferences { get { return scalingFactorForSourceCoordinateDifferences.Value; } }

        /// <summary>
        /// Gets scale factor for source coordinate reference system first axis.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorForSourceCoordReferenceSystem1stAxis { get { return scaleFactorForSourceCoordReferenceSystem1stAxis.Value; } }

        /// <summary>
        /// Gets scale factor for source coordinate reference system second axis.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorForSourceCoordReferenceSystem2ndAxis { get { return scaleFactorForSourceCoordReferenceSystem2ndAxis.Value; } }

        /// <summary>
        /// Gets scaling factor for target CRS coordinate differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForTargetCoordinateDifferences { get { return scalingFactorForTargetCoordinateDifferences.Value; } }

        /// <summary>
        /// Gets semi-major axis length difference.
        /// </summary>
        public static CoordinateOperationParameter SemiMajorAxisLengthDifference { get { return semiMajorAxisLengthDifference.Value; } }

        /// <summary>
        /// Gets vertical offset.
        /// </summary>
        public static CoordinateOperationParameter VerticalOffset { get { return verticalOffset.Value; } }

        /// <summary>
        /// Gets viewpoint height.
        /// </summary>
        public static CoordinateOperationParameter ViewpointHeight { get { return viewpointHeight.Value; } }

        /// <summary>
        /// Gets x-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter XAxisRotation { get { return xAxisRotation.Value; } }

        /// <summary>
        /// Gets x-axis translation.
        /// </summary>
        public static CoordinateOperationParameter XAxisTranslation { get { return xAxisTranslation.Value; } }

        /// <summary>
        /// Gets y-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter YAxisRotation { get { return yAxisRotation.Value; } }

        /// <summary>
        /// Gets y-axis translation.
        /// </summary>
        public static CoordinateOperationParameter YAxisTranslation { get { return yAxisTranslation.Value; } }

        /// <summary>
        /// Gets z-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter ZAxisRotation { get { return zAxisRotation.Value; } }

        /// <summary>
        /// Gets z-axis translation.
        /// </summary>
        public static CoordinateOperationParameter ZAxisTranslation { get { return zAxisTranslation.Value; } }

        /// <summary>
        /// Gets zone width.
        /// </summary>
        public static CoordinateOperationParameter ZoneWidth { get { return zoneWidth.Value; } }
    }
}
