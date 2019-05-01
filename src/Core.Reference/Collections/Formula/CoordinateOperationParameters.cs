// <copyright file="CoordinateOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of available <see cref="CoordinateOperationParameter" /> instances, as defined by the <see cref="http://www.epsg.org/">EPSG Geodetic Parameter Dataset</see>.
    /// </summary>
    public static class CoordinateOperationParameters
    {
        /// <summary>
        /// A0.
        /// </summary>
        public static CoordinateOperationParameter A0 = new CoordinateOperationParameter("EPSG::8623", "A0", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// A1.
        /// </summary>
        public static CoordinateOperationParameter A1 = new CoordinateOperationParameter("EPSG::8624", "A1", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// A2.
        /// </summary>
        public static CoordinateOperationParameter A2 = new CoordinateOperationParameter("EPSG::8625", "A2", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// A3.
        /// </summary>
        public static CoordinateOperationParameter A3 = new CoordinateOperationParameter("EPSG::8626", "A3", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// A4.
        /// </summary>
        public static CoordinateOperationParameter A4 = new CoordinateOperationParameter("EPSG::8627", "A4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// A5.
        /// </summary>
        public static CoordinateOperationParameter A5 = new CoordinateOperationParameter("EPSG::8628", "A5", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// A6.
        /// </summary>
        public static CoordinateOperationParameter A6 = new CoordinateOperationParameter("EPSG::8629", "A6", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// A7.
        /// </summary>
        public static CoordinateOperationParameter A7 = new CoordinateOperationParameter("EPSG::8630", "A7", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// A8.
        /// </summary>
        public static CoordinateOperationParameter A8 = new CoordinateOperationParameter("EPSG::8631", "A8", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Angle from Rectified to Skew Grid.
        /// </summary>
        public static CoordinateOperationParameter AngleFromRectifiedToSkewGrid = new CoordinateOperationParameter("EPSG::8814", "Angle from Rectified to Skew Grid", "The angle at the natural origin of an oblique projection through which the natural coordinate reference system is rotated to make the projection north axis parallel with true north.");

        /// <summary>
        /// Au0v1.
        /// </summary>
        public static CoordinateOperationParameter Au0v1 = new CoordinateOperationParameter("EPSG::8717", "Au0v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au0v2.
        /// </summary>
        public static CoordinateOperationParameter Au0v2 = new CoordinateOperationParameter("EPSG::8720", "Au0v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au0v3.
        /// </summary>
        public static CoordinateOperationParameter Au0v3 = new CoordinateOperationParameter("EPSG::8632", "Au0v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au0v4.
        /// </summary>
        public static CoordinateOperationParameter Au0v4 = new CoordinateOperationParameter("EPSG::8637", "Au0v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au0v5.
        /// </summary>
        public static CoordinateOperationParameter Au0v5 = new CoordinateOperationParameter("EPSG::8673", "Au0v5", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au0v6.
        /// </summary>
        public static CoordinateOperationParameter Au0v6 = new CoordinateOperationParameter("EPSG::8680", "Au0v6", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au0v8.
        /// </summary>
        public static CoordinateOperationParameter Au0v8 = new CoordinateOperationParameter("EPSG::8698", "Au0v8", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au1v0.
        /// </summary>
        public static CoordinateOperationParameter Au1v0 = new CoordinateOperationParameter("EPSG::8716", "Au1v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au1v1.
        /// </summary>
        public static CoordinateOperationParameter Au1v1 = new CoordinateOperationParameter("EPSG::8719", "Au1v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au1v2.
        /// </summary>
        public static CoordinateOperationParameter Au1v2 = new CoordinateOperationParameter("EPSG::8723", "Au1v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au1v3.
        /// </summary>
        public static CoordinateOperationParameter Au1v3 = new CoordinateOperationParameter("EPSG::8636", "Au1v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au1v4.
        /// </summary>
        public static CoordinateOperationParameter Au1v4 = new CoordinateOperationParameter("EPSG::8672", "Au1v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au1v5.
        /// </summary>
        public static CoordinateOperationParameter Au1v5 = new CoordinateOperationParameter("EPSG::8679", "Au1v5", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au1v9.
        /// </summary>
        public static CoordinateOperationParameter Au1v9 = new CoordinateOperationParameter("EPSG::8701", "Au1v9", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au2v0.
        /// </summary>
        public static CoordinateOperationParameter Au2v0 = new CoordinateOperationParameter("EPSG::8718", "Au2v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au2v1.
        /// </summary>
        public static CoordinateOperationParameter Au2v1 = new CoordinateOperationParameter("EPSG::8722", "Au2v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au2v2.
        /// </summary>
        public static CoordinateOperationParameter Au2v2 = new CoordinateOperationParameter("EPSG::8635", "Au2v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au2v3.
        /// </summary>
        public static CoordinateOperationParameter Au2v3 = new CoordinateOperationParameter("EPSG::8671", "Au2v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au2v4.
        /// </summary>
        public static CoordinateOperationParameter Au2v4 = new CoordinateOperationParameter("EPSG::8678", "Au2v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au2v7.
        /// </summary>
        public static CoordinateOperationParameter Au2v7 = new CoordinateOperationParameter("EPSG::8700", "Au2v7", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au3v0.
        /// </summary>
        public static CoordinateOperationParameter Au3v0 = new CoordinateOperationParameter("EPSG::8721", "Au3v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au3v1.
        /// </summary>
        public static CoordinateOperationParameter Au3v1 = new CoordinateOperationParameter("EPSG::8634", "Au3v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au3v2.
        /// </summary>
        public static CoordinateOperationParameter Au3v2 = new CoordinateOperationParameter("EPSG::8670", "Au3v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au3v3.
        /// </summary>
        public static CoordinateOperationParameter Au3v3 = new CoordinateOperationParameter("EPSG::8677", "Au3v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au3v9.
        /// </summary>
        public static CoordinateOperationParameter Au3v9 = new CoordinateOperationParameter("EPSG::8702", "Au3v9", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au4v0.
        /// </summary>
        public static CoordinateOperationParameter Au4v0 = new CoordinateOperationParameter("EPSG::8633", "Au4v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au4v1.
        /// </summary>
        public static CoordinateOperationParameter Au4v1 = new CoordinateOperationParameter("EPSG::8669", "Au4v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au4v2.
        /// </summary>
        public static CoordinateOperationParameter Au4v2 = new CoordinateOperationParameter("EPSG::8676", "Au4v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au5v0.
        /// </summary>
        public static CoordinateOperationParameter Au5v0 = new CoordinateOperationParameter("EPSG::8668", "Au5v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au5v1.
        /// </summary>
        public static CoordinateOperationParameter Au5v1 = new CoordinateOperationParameter("EPSG::8675", "Au5v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au5v2.
        /// </summary>
        public static CoordinateOperationParameter Au5v2 = new CoordinateOperationParameter("EPSG::8697", "Au5v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au6v0.
        /// </summary>
        public static CoordinateOperationParameter Au6v0 = new CoordinateOperationParameter("EPSG::8674", "Au6v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Au9v0.
        /// </summary>
        public static CoordinateOperationParameter Au9v0 = new CoordinateOperationParameter("EPSG::8699", "Au9v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Azimuth of initial line.
        /// </summary>
        public static CoordinateOperationParameter AzimuthOfInitialLine = new CoordinateOperationParameter("EPSG::8813", "Azimuth of initial line", "The azimuthal direction (north zero, east of north being positive) of the great circle which is the centre line of an oblique projection. The azimuth is given at the projection centre.");

        /// <summary>
        /// B0.
        /// </summary>
        public static CoordinateOperationParameter B0 = new CoordinateOperationParameter("EPSG::8639", "B0", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// B00.
        /// </summary>
        public static CoordinateOperationParameter B00 = new CoordinateOperationParameter("EPSG::8638", "B00", "Coefficient used only in the Madrid to ED50 polynomial transformation method.");

        /// <summary>
        /// B1.
        /// </summary>
        public static CoordinateOperationParameter B1 = new CoordinateOperationParameter("EPSG::8640", "B1", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// B2.
        /// </summary>
        public static CoordinateOperationParameter B2 = new CoordinateOperationParameter("EPSG::8641", "B2", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// B3.
        /// </summary>
        public static CoordinateOperationParameter B3 = new CoordinateOperationParameter("EPSG::8642", "B3", "Coefficient used in affine (general parametric) and polynomial transformations.");

        /// <summary>
        /// Bin grid origin Easting.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginEasting = new CoordinateOperationParameter("EPSG::8735", "Bin grid origin Easting", "The value of the map grid Easting at the bin grid definition point.");

        /// <summary>
        /// Bin grid origin I.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginI = new CoordinateOperationParameter("EPSG::8733", "Bin grid origin I", "The value of the I-axis coordinate at the bin grid definition point. The I-axis is rotated 90 degrees clockwise from the J-axis.");

        /// <summary>
        /// Bin grid origin J.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginJ = new CoordinateOperationParameter("EPSG::8734", "Bin grid origin J", "The value of the J-axis coordinate at the bin grid definition point.");

        /// <summary>
        /// Bin grid origin Northing.
        /// </summary>
        public static CoordinateOperationParameter BinGridOriginNorthing = new CoordinateOperationParameter("EPSG::8736", "Bin grid origin Northing", "The value of the map grid Northing at the bin grid definition point.");

        /// <summary>
        /// Bin node increment on I-axis.
        /// </summary>
        public static CoordinateOperationParameter BinNodeIncrementOnIAxis = new CoordinateOperationParameter("EPSG::8741", "Bin node increment on I-axis", "The numerical increment between successive bin nodes on the I-axis.");

        /// <summary>
        /// Bin node increment on J-axis.
        /// </summary>
        public static CoordinateOperationParameter BinNodeIncrementOnJAxis = new CoordinateOperationParameter("EPSG::8742", "Bin node increment on J-axis", "The numerical increment between successive bin nodes on the J-axis.");

        /// <summary>
        /// Bin width on I-axis.
        /// </summary>
        public static CoordinateOperationParameter BinWidthOnIAxis = new CoordinateOperationParameter("EPSG::8738", "Bin width on I-axis", "The nominal separation of bin nodes on the bin grid I-axis. (Note: the actual bin node separation is the product of the nominal separation and the scale factor of the bin grid).");

        /// <summary>
        /// Bin width on J-axis.
        /// </summary>
        public static CoordinateOperationParameter BinWidthOnJAxis = new CoordinateOperationParameter("EPSG::8739", "Bin width on J-axis", "The nominal separation of bin nodes on the bin grid J-axis. (Note: the actual bin node separation is the product of the nominal separation and the scale factor of the bin grid).");

        /// <summary>
        /// Bu0v1.
        /// </summary>
        public static CoordinateOperationParameter Bu0v1 = new CoordinateOperationParameter("EPSG::8725", "Bu0v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu0v2.
        /// </summary>
        public static CoordinateOperationParameter Bu0v2 = new CoordinateOperationParameter("EPSG::8644", "Bu0v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu0v3.
        /// </summary>
        public static CoordinateOperationParameter Bu0v3 = new CoordinateOperationParameter("EPSG::8648", "Bu0v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu0v4.
        /// </summary>
        public static CoordinateOperationParameter Bu0v4 = new CoordinateOperationParameter("EPSG::8653", "Bu0v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu0v5.
        /// </summary>
        public static CoordinateOperationParameter Bu0v5 = new CoordinateOperationParameter("EPSG::8686", "Bu0v5", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu0v6.
        /// </summary>
        public static CoordinateOperationParameter Bu0v6 = new CoordinateOperationParameter("EPSG::8693", "Bu0v6", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu0v9.
        /// </summary>
        public static CoordinateOperationParameter Bu0v9 = new CoordinateOperationParameter("EPSG::8709", "Bu0v9", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu1v0.
        /// </summary>
        public static CoordinateOperationParameter Bu1v0 = new CoordinateOperationParameter("EPSG::8724", "Bu1v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu1v1.
        /// </summary>
        public static CoordinateOperationParameter Bu1v1 = new CoordinateOperationParameter("EPSG::8643", "Bu1v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu1v2.
        /// </summary>
        public static CoordinateOperationParameter Bu1v2 = new CoordinateOperationParameter("EPSG::8647", "Bu1v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu1v3.
        /// </summary>
        public static CoordinateOperationParameter Bu1v3 = new CoordinateOperationParameter("EPSG::8652", "Bu1v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu1v4.
        /// </summary>
        public static CoordinateOperationParameter Bu1v4 = new CoordinateOperationParameter("EPSG::8685", "Bu1v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu1v5.
        /// </summary>
        public static CoordinateOperationParameter Bu1v5 = new CoordinateOperationParameter("EPSG::8692", "Bu1v5", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu2v0.
        /// </summary>
        public static CoordinateOperationParameter Bu2v0 = new CoordinateOperationParameter("EPSG::8726", "Bu2v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu2v1.
        /// </summary>
        public static CoordinateOperationParameter Bu2v1 = new CoordinateOperationParameter("EPSG::8646", "Bu2v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu2v2.
        /// </summary>
        public static CoordinateOperationParameter Bu2v2 = new CoordinateOperationParameter("EPSG::8651", "Bu2v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu2v3.
        /// </summary>
        public static CoordinateOperationParameter Bu2v3 = new CoordinateOperationParameter("EPSG::8684", "Bu2v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu2v4.
        /// </summary>
        public static CoordinateOperationParameter Bu2v4 = new CoordinateOperationParameter("EPSG::8691", "Bu2v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu2v7.
        /// </summary>
        public static CoordinateOperationParameter Bu2v7 = new CoordinateOperationParameter("EPSG::8708", "Bu2v7", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu3v0.
        /// </summary>
        public static CoordinateOperationParameter Bu3v0 = new CoordinateOperationParameter("EPSG::8645", "Bu3v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu3v1.
        /// </summary>
        public static CoordinateOperationParameter Bu3v1 = new CoordinateOperationParameter("EPSG::8650", "Bu3v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu3v2.
        /// </summary>
        public static CoordinateOperationParameter Bu3v2 = new CoordinateOperationParameter("EPSG::8683", "Bu3v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu3v3.
        /// </summary>
        public static CoordinateOperationParameter Bu3v3 = new CoordinateOperationParameter("EPSG::8690", "Bu3v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu4v0.
        /// </summary>
        public static CoordinateOperationParameter Bu4v0 = new CoordinateOperationParameter("EPSG::8649", "Bu4v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu4v1.
        /// </summary>
        public static CoordinateOperationParameter Bu4v1 = new CoordinateOperationParameter("EPSG::8682", "Bu4v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu4v2.
        /// </summary>
        public static CoordinateOperationParameter Bu4v2 = new CoordinateOperationParameter("EPSG::8689", "Bu4v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu4v4.
        /// </summary>
        public static CoordinateOperationParameter Bu4v4 = new CoordinateOperationParameter("EPSG::8705", "Bu4v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu4v6.
        /// </summary>
        public static CoordinateOperationParameter Bu4v6 = new CoordinateOperationParameter("EPSG::8710", "Bu4v6", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu4v9.
        /// </summary>
        public static CoordinateOperationParameter Bu4v9 = new CoordinateOperationParameter("EPSG::8715", "Bu4v9", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu5v0.
        /// </summary>
        public static CoordinateOperationParameter Bu5v0 = new CoordinateOperationParameter("EPSG::8681", "Bu5v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu5v1.
        /// </summary>
        public static CoordinateOperationParameter Bu5v1 = new CoordinateOperationParameter("EPSG::8688", "Bu5v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu5v7.
        /// </summary>
        public static CoordinateOperationParameter Bu5v7 = new CoordinateOperationParameter("EPSG::8713", "Bu5v7", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu6v0.
        /// </summary>
        public static CoordinateOperationParameter Bu6v0 = new CoordinateOperationParameter("EPSG::8687", "Bu6v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu6v1.
        /// </summary>
        public static CoordinateOperationParameter Bu6v1 = new CoordinateOperationParameter("EPSG::8704", "Bu6v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu7v0.
        /// </summary>
        public static CoordinateOperationParameter Bu7v0 = new CoordinateOperationParameter("EPSG::8703", "Bu7v0", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu7v2.
        /// </summary>
        public static CoordinateOperationParameter Bu7v2 = new CoordinateOperationParameter("EPSG::8707", "Bu7v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu8v1.
        /// </summary>
        public static CoordinateOperationParameter Bu8v1 = new CoordinateOperationParameter("EPSG::8706", "Bu8v1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu8v3.
        /// </summary>
        public static CoordinateOperationParameter Bu8v3 = new CoordinateOperationParameter("EPSG::8712", "Bu8v3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu9v2.
        /// </summary>
        public static CoordinateOperationParameter Bu9v2 = new CoordinateOperationParameter("EPSG::8711", "Bu9v2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Bu9v4.
        /// </summary>
        public static CoordinateOperationParameter Bu9v4 = new CoordinateOperationParameter("EPSG::8714", "Bu9v4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C1.
        /// </summary>
        public static CoordinateOperationParameter C1 = new CoordinateOperationParameter("EPSG::1026", "C1", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C10.
        /// </summary>
        public static CoordinateOperationParameter C10 = new CoordinateOperationParameter("EPSG::1035", "C10", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C2.
        /// </summary>
        public static CoordinateOperationParameter C2 = new CoordinateOperationParameter("EPSG::1027", "C2", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C3.
        /// </summary>
        public static CoordinateOperationParameter C3 = new CoordinateOperationParameter("EPSG::1028", "C3", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C4.
        /// </summary>
        public static CoordinateOperationParameter C4 = new CoordinateOperationParameter("EPSG::1029", "C4", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C5.
        /// </summary>
        public static CoordinateOperationParameter C5 = new CoordinateOperationParameter("EPSG::1030", "C5", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C6.
        /// </summary>
        public static CoordinateOperationParameter C6 = new CoordinateOperationParameter("EPSG::1031", "C6", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C7.
        /// </summary>
        public static CoordinateOperationParameter C7 = new CoordinateOperationParameter("EPSG::1032", "C7", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C8.
        /// </summary>
        public static CoordinateOperationParameter C8 = new CoordinateOperationParameter("EPSG::1033", "C8", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// C9.
        /// </summary>
        public static CoordinateOperationParameter C9 = new CoordinateOperationParameter("EPSG::1034", "C9", "Coefficient used in polynomial transformations.");

        /// <summary>
        /// Cartesian coordinate programme file.
        /// </summary>
        public static CoordinateOperationParameter CartesianCoordinateProgrammeFile = new CoordinateOperationParameter("EPSG::1024", "Cartesian coordinate programme file", "The name of the programme file containing Cartesian coordinates in both source and target CRSs.");

        /// <summary>
        /// Co-latitude of cone axis.
        /// </summary>
        public static CoordinateOperationParameter CoLatitudeOfConeAxis = new CoordinateOperationParameter("EPSG::1036", "Co-latitude of cone axis", "The rotation applied to spherical coordinates for the oblique projection, measured on the conformal sphere in the plane of the meridian of origin.");

        /// <summary>
        /// Coord. op. code for northern boundary.
        /// </summary>
        public static CoordinateOperationParameter CoordOpCodeForNorthernBoundary = new CoordinateOperationParameter("EPSG::8659", "Coord. op. code for northern boundary", "The EPSG code for the coordinate transformation applied at the northern boundary of the interpolation area. Applies to Norwegian offshore interpolation method.");

        /// <summary>
        /// Coord. op. code for southern boundary.
        /// </summary>
        public static CoordinateOperationParameter CoordOpCodeForSouthernBoundary = new CoordinateOperationParameter("EPSG::8660", "Coord. op. code for southern boundary", "The EPSG code for the coordinate transformation applied at the southern boundary of the interpolation area. Applies to Norwegian offshore interpolation method.");

        /// <summary>
        /// Coord. op. name for northern boundary.
        /// </summary>
        public static CoordinateOperationParameter CoordOpNameForNorthernBoundary = new CoordinateOperationParameter("EPSG::8661", "Coord. op. name for northern boundary", "The EPSG name for the coordinate transformation applied at the northern boundary of the interpolation area. Applies to Norwegian offshore interpolation method.");

        /// <summary>
        /// Coord. op. name for southern boundary.
        /// </summary>
        public static CoordinateOperationParameter CoordOpNameForSouthernBoundary = new CoordinateOperationParameter("EPSG::8662", "Coord. op. name for southern boundary", "The EPSG name for the coordinate transformation applied at the southern boundary of the interpolation area. Applies to Norwegian offshore interpolation method.");

        /// <summary>
        /// Easting and northing difference file.
        /// </summary>
        public static CoordinateOperationParameter EastingAndNordingDifferenceFile = new CoordinateOperationParameter("EPSG::8664", "Easting and northing difference file", "The name of the [path and] file containing easting and northing differences.");

        /// <summary>
        /// Easting at false origin.
        /// </summary>
        public static CoordinateOperationParameter EastingAtFalseOrigin = new CoordinateOperationParameter("EPSG::8826", "Easting at false origin", "The easting value assigned to the false origin.");

        /// <summary>
        /// Easting at projection centre.
        /// </summary>
        public static CoordinateOperationParameter EastingAtProjectionCentre = new CoordinateOperationParameter("EPSG::8816", "Easting at projection centre", "The easting value assigned to the projection centre.");

        /// <summary>
        /// Easting offset.
        /// </summary>
        public static CoordinateOperationParameter EastingOffset = new CoordinateOperationParameter("EPSG::8728", "Easting offset", "The difference between the easting values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Ellipsoidal height difference file.
        /// </summary>
        public static CoordinateOperationParameter EllipsoidalHeightDifferenceFile = new CoordinateOperationParameter("EPSG::1058", "Ellipsoidal height difference file", "The name of the [path and] file containing ellipsoidal height differences. ");

        /// <summary>
        /// Ellipsoidal height of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter EllipsoidalHeightOfTopocentricOrigin = new CoordinateOperationParameter("EPSG::8836", "Ellipsoidal height of topocentric origin", "For topocentric CSs, the ellipsoidal height of the topocentric origin.");

        /// <summary>
        /// Ellipsoid scaling factor.
        /// </summary>
        public static CoordinateOperationParameter EllipsoidScalingFactor = new CoordinateOperationParameter("EPSG::1038", "Ellipsoid scaling factor", "Ratio by which the ellipsoid is enlarged so that survey observations are reduced to a surface above the ellipsoid surface.");

        /// <summary>
        /// False easting.
        /// </summary>
        public static CoordinateOperationParameter FalseEasting = new CoordinateOperationParameter("EPSG::8806", "False easting", "Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Easting, FE, is the value assigned to the abscissa (east or west) axis of the projection grid at the natural origin.");

        /// <summary>
        /// False northing.
        /// </summary>
        public static CoordinateOperationParameter FalseNorthing = new CoordinateOperationParameter("EPSG::8807", "False northing", "Since the natural origin may be at or near the centre of the projection and under normal coordinate circumstances would thus give rise to negative coordinates over parts of the mapped area, this origin is usually given false coordinates which are large enough to avoid this inconvenience. The False Northing, FN, is the value assigned to the ordinate (north or south) axis of the projection grid at the natural origin.");

        /// <summary>
        /// Flattening difference.
        /// </summary>
        public static CoordinateOperationParameter FlatteningDifference = new CoordinateOperationParameter("EPSG::8655", "Flattening difference", "The difference between the flattening values of the ellipsoids used in the target and source coordinate reference systems.");

        /// <summary>
        /// Geocentric translation file.
        /// </summary>
        public static CoordinateOperationParameter GeocentricTranslationFile = new CoordinateOperationParameter("EPSG::8727", "Geocentric translation file", "The name of the [path and] file containing a grid of geocentric translations.");

        /// <summary>
        /// Geocentric X of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticXOfTopocentricOrigin = new CoordinateOperationParameter("EPSG::8837", "Geocentric X of topocentric origin", "For topocentric CSs, the geocentric Cartesian X coordinate of the topocentric origin.");

        /// <summary>
        /// Geocentric Y of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticYOfTopocentricOrigin = new CoordinateOperationParameter("EPSG::8838", "Geocentric Y of topocentric origin", "For topocentric CSs, the geocentric Cartesian Y coordinate of the topocentric origin.");

        /// <summary>
        /// Geocentric Z of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter GeocenticZOfTopocentricOrigin = new CoordinateOperationParameter("EPSG::8839", "Geocentric Z of topocentric origin", "For topocentric CSs, the geocentric Cartesian Z coordinate of the topocentric origin.");

        /// <summary>
        /// Geoid (height correction) model file.
        /// </summary>
        public static CoordinateOperationParameter GeoidHeightCorrectionModelFile = new CoordinateOperationParameter("EPSG::8666", "Geoid (height correction) model file", "The name of the [path and] file containing height offsets. 'Geoid model' is used loosely in the parameter name. If the offset is between the ellipsoid and the geoid then the offset will be the geoid separation N. When the offset is between the ellipsoid and a realisation of the geoid through a vertical datum then the offsets C will form a height correction surface. A height correction surface differs from a geoid model by small amounts due to the difference between the geoid and the actual datum surface as well as some other assumptions regarding the gravity field.");

        /// <summary>
        /// Geoid undulation.
        /// </summary>
        public static CoordinateOperationParameter GeoidUndulation = new CoordinateOperationParameter("EPSG::8604", "Geoid undulation", "The height offset. 'Geoid undulation' is used loosely in the parameter name. If the offset is between the ellipsoid and the geoid then the offset will be the geoid separation N. When the offset is between the ellipsoid and a realisation of the geoid through a vertical datum then the offsets C will form a height correction surface. A height correction surface differs from a geoid model by small amounts due to the difference between the geoid and the actual datum surface as well as some other assumptions regarding the gravity field.");

        /// <summary>
        /// GNTRANS transformation identifier.
        /// </summary>
        public static CoordinateOperationParameter GntransTransformationIdentifier = new CoordinateOperationParameter("EPSG::1025", "GNTRANS transformation identifier", " Identifier for the subset of parameters stored inside the transformation programme.");

        /// <summary>
        /// Horizontal CRS code.
        /// </summary>
        public static CoordinateOperationParameter HorizontalCRSCode = new CoordinateOperationParameter("EPSG::1037", "Horizontal CRS code", "The EPSG code for the CRS that should be used to reference horizontal coordinates used as arguments within the method.");

        /// <summary>
        /// Inclination in latitude.
        /// </summary>
        public static CoordinateOperationParameter InclinationInLatitude = new CoordinateOperationParameter("EPSG::8730", "Inclination in latitude", "The value of the inclination parameter in the latitude domain, i.e. in the plane of the meridian, derived at a specified evaluation point.");

        /// <summary>
        /// Inclination in longitude.
        /// </summary>
        public static CoordinateOperationParameter InclinationInLongitude = new CoordinateOperationParameter("EPSG::8731", "Inclination in longitude", "The value of the inclination parameter in the the longitude domain, i.e. perpendicular to the plane of the meridian, derived at a specified evaluation point.");

        /// <summary>
        /// Initial longitude.
        /// </summary>
        public static CoordinateOperationParameter InitialLongitude = new CoordinateOperationParameter("EPSG::8830", "Initial longitude", "The longitude of the western limit of the first zone of a Transverse Mercator zoned grid system.");

        /// <summary>
        /// Interpolation CRS code.
        /// </summary>
        public static CoordinateOperationParameter InterpolationCRSCode = new CoordinateOperationParameter("EPSG::1048", " Interpolation CRS code", "The EPSG code for the CRS that should be used to interpolate gridded data.");

        /// <summary>
        /// Latitude difference file.
        /// </summary>
        public static CoordinateOperationParameter LatitudeDifferenceFile = new CoordinateOperationParameter("EPSG::8657", "Latitude difference file", "The name of the [path and] file containing latitude differences.");

        /// <summary>
        /// Latitude and longitude difference file.
        /// </summary>
        public static CoordinateOperationParameter LatitudeAndLongitudeDifferenceFile = new CoordinateOperationParameter("EPSG::8656", "Latitude and longitude difference file", "The name of the [path and] file containing latitude differences.");

        /// <summary>
        /// Latitude of 1st standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOf1stStandardParallel = new CoordinateOperationParameter("EPSG::8823", "Latitude of 1st standard parallel", "For a conic projection with two standard parallels, this is the latitude of one of the parallels of intersection of the cone with the ellipsoid. It is normally but not necessarily that nearest to the pole. Scale is true along this parallel.");

        /// <summary>
        /// Latitude of 2nd standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOf2ndStandardParallel = new CoordinateOperationParameter("EPSG::8824", "Latitude of 2nd standard parallel", "For a conic projection with two standard parallels, this is the latitude of one of the parallels at which the cone intersects with the ellipsoid. It is normally but not necessarily that nearest to the equator. Scale is true along this parallel.");

        /// <summary>
        /// Latitude of false origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfFalseOrigin = new CoordinateOperationParameter("EPSG::8821", "Latitude of false origin", "The latitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.");

        /// <summary>
        /// Latitude offset.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOffset = new CoordinateOperationParameter("EPSG::8601", "Latitude offset", "The difference between the latitude values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfNaturalOrigin = new CoordinateOperationParameter("EPSG::8801", "Latitude of natural origin", null, new String[] { "Latitude of origin" }, "The latitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the latitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0).");

        /// <summary>
        /// Latitude of projection centre.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfProjectionCentre = new CoordinateOperationParameter("EPSG::8811", "Latitude of projection centre", "For an oblique projection, this is the latitude of the point at which the azimuth of the central line is defined.");

        /// <summary>
        /// Latitude of pseudo standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfPseudoStandardParallel = new CoordinateOperationParameter("EPSG::8818", "Latitude of pseudo standard parallel", "Latitude of the parallel on which the conic or cylindrical projection is based. This latitude is not geographic, but is defined on the conformal sphere AFTER its rotation to obtain the oblique aspect of the projection.");

        /// <summary>
        /// Latitude of standard parallel.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfStandardParallel = new CoordinateOperationParameter("EPSG::8832", "Latitude of standard parallel", "For polar aspect azimuthal projections, the parallel on which the scale factor is defined to be unity.");

        /// <summary>
        /// Latitude of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter LatitudeOfTopocentricOrigin = new CoordinateOperationParameter("EPSG::8834", "Latitude of topocentric origin", "For topocentric CSs, the latitude of the topocentric origin.");

        /// <summary>
        /// Longitude difference file.
        /// </summary>
        public static CoordinateOperationParameter LongitudeDifferenceFile = new CoordinateOperationParameter("EPSG::8658", "Longitude difference file", "The name of the [path and] file containing longitude differences.");

        /// <summary>
        /// Longitude of false origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfFalseOrigin = new CoordinateOperationParameter("EPSG::8822", "Longitude of false origin", "The longitude of the point which is not the natural origin and at which grid coordinate values false easting and false northing are defined.");

        /// <summary>
        /// Longitude offset.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOffset = new CoordinateOperationParameter("EPSG::8602", "Longitude offset", "The difference between the longitude values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfNaturalOrigin = new CoordinateOperationParameter("EPSG::8802", "Longitude of natural origin", null, new String[] { "Central Meridian", "CM" }, "The longitude of the point from which the values of both the geographical coordinates on the ellipsoid and the grid coordinates on the projection are deemed to increment or decrement for computational purposes. Alternatively it may be considered as the longitude of the point which in the absence of application of false coordinates has grid coordinates of (0,0). Sometimes known as \"central meridian (CM)\".");

        /// <summary>
        /// Longitude of origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfOrigin = new CoordinateOperationParameter("EPSG::8833", "Longitude of origin", "For polar aspect azimuthal projections, the meridian along which the northing axis increments and also across which parallels of latitude increment towards the north pole.");

        /// <summary>
        /// Longitude of projection centre.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfProjectionCentre = new CoordinateOperationParameter("EPSG::8812", "Longitude of projection centre", "For an oblique projection, this is the longitude of the point at which the azimuth of the central line is defined.");

        /// <summary>
        /// Longitude of topocentric origin.
        /// </summary>
        public static CoordinateOperationParameter LongitudeOfTopocentricOrigin = new CoordinateOperationParameter("EPSG::8835", "Longitude of topocentric origin", "For topocentric CSs, the longitude of the topocentric origin.");

        /// <summary>
        /// Map grid bearing of bin grid J-axis.
        /// </summary>
        public static CoordinateOperationParameter MapGridBearingOfBinGridJAxis = new CoordinateOperationParameter("EPSG::8740", "Map grid bearing of bin grid J-axis", "The orientation of the bin grid J-axis measured clockwise from map grid north.");

        /// <summary>
        /// Maritime Province residual file.
        /// </summary>
        public static CoordinateOperationParameter MaritimeProvinceResidualFile = new CoordinateOperationParameter("EPSG::8665", "Maritime Province residual file", "Coordinate differences at control points.");

        /// <summary>
        /// Northing at false origin.
        /// </summary>
        public static CoordinateOperationParameter NorthingAtFalseOrigin = new CoordinateOperationParameter("EPSG::8827", "Northing at false origin", "The northing value assigned to the false origin.");

        /// <summary>
        /// Northing at projection centre.
        /// </summary>
        public static CoordinateOperationParameter NorthingAtProjectionCentre = new CoordinateOperationParameter("EPSG::8817", "Northing at projection centre", "The northing value assigned to the projection centre.");

        /// <summary>
        /// Northing offset.
        /// </summary>
        public static CoordinateOperationParameter NorthingOffset = new CoordinateOperationParameter("EPSG::8729", "Northing offset", "The difference between the northing values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Ordinate 1 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPoint = new CoordinateOperationParameter("EPSG::8617", "Ordinate 1 of evaluation point", "The value of the first ordinate of the evaluation point.");

        /// <summary>
        /// Ordinate 1 of evaluation point in source CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPointInSource = new CoordinateOperationParameter("EPSG::8619", "Ordinate 1 of evaluation point in source CRS", "The value of the first ordinate of the evaluation point expressed in the source coordinate reference system.");

        /// <summary>
        /// Ordinate 1 of evaluation point in target CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate1OfEvaluationPointInTarget = new CoordinateOperationParameter("EPSG::8621", "Ordinate 1 of evaluation point in target CRS", "The value of the first ordinate of the evaluation point expressed in the target coordinate reference system. In the case of an affine transformation the evaluation point is the origin of the source coordinate reference system.");

        /// <summary>
        /// Ordinate 2 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPoint = new CoordinateOperationParameter("EPSG::8618", "Ordinate 2 of evaluation point", "The value of the second ordinate of the evaluation point.");

        /// <summary>
        /// Ordinate 2 of evaluation point in source CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPointInSource = new CoordinateOperationParameter("EPSG::8620", "Ordinate 2 of evaluation point in source CRS", "The value of the second ordinate of the evaluation point expressed in the source coordinate reference system.");

        /// <summary>
        /// Ordinate 2 of evaluation point in target CRS.
        /// </summary>
        public static CoordinateOperationParameter Ordinate2OfEvaluationPointInTarget = new CoordinateOperationParameter("EPSG::8622", "Ordinate 2 of evaluation point in target CRS", "The value of the second ordinate of the evaluation point expressed in the target coordinate reference system. In the case of an affine transformation the evaluation point is the origin of the source coordinate reference system.");

        /// <summary>
        /// Ordinate 3 of evaluation point.
        /// </summary>
        public static CoordinateOperationParameter Ordinate3OfEvaluationPoint = new CoordinateOperationParameter("EPSG::8667", "Ordinate 3 of evaluation point", "The value of the third ordinate of the evaluation point.");

        /// <summary>
        /// Parameter reference epoch.
        /// </summary>
        public static CoordinateOperationParameter ParameterReferenceEpoch = new CoordinateOperationParameter("EPSG::1047", "Parameter reference epoch", "The reference epoch for the parameters of a time-dependent transformation.");

        /// <summary>
        /// Point motion velocity grid file.
        /// </summary>
        public static CoordinateOperationParameter PointMotionVelocityGridFile = new CoordinateOperationParameter("EPSG::1050", "Point motion velocity grid file ", "The name of the [path and] grid file containing velocities.");

        /// <summary>
        /// Point scale factor.
        /// </summary>
        public static CoordinateOperationParameter PointScaleFactor = new CoordinateOperationParameter("EPSG::8663", "Point scale factor", "The point scale factor in a selected point of the target coordinate reference system. to be used as representative figure of the scale of the target coordinate reference system in a the area to which a coordinate transformation is defined.");

        /// <summary>
        /// Projection plane origin height.
        /// </summary>
        public static CoordinateOperationParameter ProjectionPlaneOriginHeight = new CoordinateOperationParameter("EPSG::1039", "Projection plane origin height", "For Colombia urban grids, the height of the projection plane at its origin.");

        /// <summary>
        /// Rate of change of scale difference.
        /// </summary>
        public static CoordinateOperationParameter RateOfChangeOfScaleDifference = new CoordinateOperationParameter("EPSG::1046", "Rate of change of scale difference", null, new[] { "d(dS)" }, "Time first derivative of scale difference.");

        /// <summary>
        /// Rate of change of X-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter RateOfChangeOfXAxisRotation = new CoordinateOperationParameter("EPSG::1043", "Rate of change of X-axis rotation", null, new[] { "d(rX)" }, "Time first derivative of X-axis rotation.");

        /// <summary>
        /// Rate of change of X-axis translation.
        /// </summary>
        public static CoordinateOperationParameter RateOfChangeOfXAxisTranslation = new CoordinateOperationParameter("EPSG::1040", "Rate of change of X-axis translation", null, new[] { "d(tX)" }, "Time first derivative of X-axis translation.");

        /// <summary>
        /// Rate of change of Y-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter RateOfChangeOfYAxisRotation = new CoordinateOperationParameter("EPSG::1044", "Rate of change of Y-axis rotation", null, new[] { "d(rY)" }, "Time first derivative of Y-axis rotation.");

        /// <summary>
        /// Rate of change of Y-axis translation.
        /// </summary>
        public static CoordinateOperationParameter RateOfChangeOfYAxisTranslation = new CoordinateOperationParameter("EPSG::1041", "Rate of change of Y-axis translation", null, new[] { "d(tY)" }, "Time first derivative of Y-axis translation.");

        /// <summary>
        /// Rate of change of Z-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter RateOfChangeOfZAxisRotation = new CoordinateOperationParameter("EPSG::1045", "Rate of change of Z-axis rotation", null, new[] { "d(rZ)" }, "Time first derivative of Z-axis rotation.");

        /// <summary>
        /// Rate of change of Z-axis translation.
        /// </summary>
        public static CoordinateOperationParameter RateOfChangeOfZAxisTranslation = new CoordinateOperationParameter("EPSG::1042", "Rate of change of Z-axis translation", null, new[] { "d(tZ)" }, "Time first derivative of Z-axis translation.");

        /// <summary>
        /// Rotation angle of source coordinate reference system axes.
        /// </summary>
        public static CoordinateOperationParameter RotationAngleOfSourceCoordReferenceSystemAxes = new CoordinateOperationParameter("EPSG::8614", "Rotation angle of source coordinate reference system axes", "Angle (counter-clockwise positive) through which both of the source coordinate reference system axes need to rotated to coincide with the corresponding target coordinate reference system axes. Alternatively, the bearing (clockwise positive) of the source coordinate reference system Y-axis measured relative to target coordinate reference system north.");

        /// <summary>
        /// Scale difference.
        /// </summary>
        public static CoordinateOperationParameter ScaleDifference = new CoordinateOperationParameter("EPSG::8611", "Scale difference", "The scale difference increased by unity equals the ratio of an the length of an arbitrary distance between two points in target and source coordinate reference systems. This is usually averaged for the intersection area of the two coordinate reference systems. If a distance of 100 km in the source coordinate reference system translates into a distance of 100.001 km in the target coordinate reference system, the scale difference is 1 ppm (the ratio being 1.000001).");

        /// <summary>
        /// Scale factor at natural origin.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorAtNaturalOrigin = new CoordinateOperationParameter("EPSG::8805", "Scale factor at natural origin", "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the natural origin.");

        /// <summary>
        /// Scale factor for source coordinate reference system first axis.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorForSourceCoordReferenceSystem1stAxis = new CoordinateOperationParameter("EPSG::8612", "Scale factor for source coordinate reference system first axis", "The unit of measure of the source coordinate reference system first axis, expressed in the unit of measure of the target coordinate reference system.");

        /// <summary>
        /// Scale factor for source coordinate reference system second axis.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorForSourceCoordReferenceSystem2ndAxis = new CoordinateOperationParameter("EPSG::8613", "Scale factor for source coordinate reference system second axis", "The unit of measure of the source coordinate reference system second axis, expressed in the unit of measure of the target coordinate reference system.");

        /// <summary>
        /// Scale factor of bin grid.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOfBinGrid = new CoordinateOperationParameter("EPSG::8737", "Scale factor of bin grid", "The point scale factor of the map grid coordinate reference system at a point within the bin grid. Generally either the bin grid origin or the centre of the bin grid will be the chosen point.");

        /// <summary>
        /// Scale factor on initial line.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOnInitialLine = new CoordinateOperationParameter("EPSG::8815", "Scale factor on initial line", "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the projection center.");

        /// <summary>
        /// Scale factor on pseudo standard parallel.
        /// </summary>
        public static CoordinateOperationParameter ScaleFactorOnPseudoStandardParallel = new CoordinateOperationParameter("EPSG::8819", "Scale factor on pseudo standard parallel", "The factor by which the map grid is reduced or enlarged during the projection process, defined by its value at the pseudo-standard parallel.");

        /// <summary>
        /// Scaling factor for coord differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForCoordinateDifferences = new CoordinateOperationParameter("EPSG::8696", "Scaling factor for coord differences", "Used in reversible polynomial transformations to normalise coordinate differences to an acceptable numerical range. For the reverse transformation the forward target CRS becomes the reverse source CRS and forward source CRS becomes the reverse target CRS.");

        /// <summary>
        /// Scaling factor for source CRS coord differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForSourceCoordinateDifferences = new CoordinateOperationParameter("EPSG::8694", "Scaling factor for source CRS coord differences", "Used in general polynomial transformations to normalise coordinate differences to an acceptable numerical range.");

        /// <summary>
        /// Scaling factor for target CRS coord differences.
        /// </summary>
        public static CoordinateOperationParameter ScalingFactorForTargetCoordinateDifferences = new CoordinateOperationParameter("EPSG::8695", "Scaling factor for target CRS coord differences", "Used in general polynomial transformations to normalise coordinate differences to an acceptable numerical range.");

        /// <summary>
        /// Semi-major axis length difference.
        /// </summary>
        public static CoordinateOperationParameter SemiMajorAxisLengthDifference = new CoordinateOperationParameter("EPSG::8654", "Semi-major axis length difference", "The difference between the semi-major axis values of the ellipsoids used in the target and source coordinate reference systems.");

        /// <summary>
        /// Spherical latitude of origin.
        /// </summary>
        public static CoordinateOperationParameter SphericalLatitudeOfOrigin = new CoordinateOperationParameter("EPSG::8828", "Spherical latitude of origin", "The latitude of the point from which the values of both the geographical coordinates on the sphere.");

        /// <summary>
        /// Spherical longitude of origin.
        /// </summary>
        public static CoordinateOperationParameter SphericalLongitudeOfOrigin = new CoordinateOperationParameter("EPSG::8829", "Spherical longitude of origin", "The longitude of the point from which the values of both the geographical coordinates on the sphere.");

        /// <summary>
        /// Unit conversion scalar.
        /// </summary>
        public static CoordinateOperationParameter UnitConversionScalar = new CoordinateOperationParameter("EPSG::1051", "Unit conversion scalar", "Ratio of [(factor b) / (factor c)] from the EPSG Dataset Unit of Measure table, populated with respect to the SI base unit, e.g. ft/m.");

        /// <summary>
        /// Vertical offset.
        /// </summary>
        public static CoordinateOperationParameter VerticalOffset = new CoordinateOperationParameter("EPSG::8603", "Vertical offset", "The difference between the height or depth values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Vertical offset file.
        /// </summary>
        public static CoordinateOperationParameter VerticalOffsetFile = new CoordinateOperationParameter("EPSG::8732", "Vertical offset file", "The name of the [path and] file containing vertical offsets, the differences in gravity-related height.");

        /// <summary>
        /// Viewpoint height.
        /// </summary>
        public static CoordinateOperationParameter ViewpointHeight = new CoordinateOperationParameter("EPSG::8840", "Viewpoint height", "For vertical perspective projections, the height of viewpoint above the topocentric origin.");

        /// <summary>
        /// X-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter XAxisRotation = new CoordinateOperationParameter("EPSG::8608", "X-axis rotation", "The angular difference between the Y and Z axes directions of target and source coordinate reference systems. This is a rotation about the X axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis).");

        /// <summary>
        /// X-axis translation.
        /// </summary>
        public static CoordinateOperationParameter XAxisTranslation = new CoordinateOperationParameter("EPSG::8605", "X-axis translation", "The difference between the X values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Y-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter YAxisRotation = new CoordinateOperationParameter("EPSG::8609", "Y-axis rotation", "The angular difference between the X and Z axes directions of target and source coordinate reference systems. This is a rotation about the Y axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis).");

        /// <summary>
        /// Y-axis translation.
        /// </summary>
        public static CoordinateOperationParameter YAxisTranslation = new CoordinateOperationParameter("EPSG::8606", "Y-axis translation", "The difference between the Y values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Z-axis rotation.
        /// </summary>
        public static CoordinateOperationParameter ZAxisRotation = new CoordinateOperationParameter("EPSG::8610", "Z-axis rotation", "The angular difference between the X and Y axes directions of target and source coordinate reference systems. This is a rotation about the Z axis as viewed from the origin looking along that axis. The particular method defines which direction is positive, and what is being rotated (point or axis).");

        /// <summary>
        /// Z-axis translation.
        /// </summary>
        public static CoordinateOperationParameter ZAxisTranslation = new CoordinateOperationParameter("EPSG::8607", "Z-axis translation", "The difference between the Z values of a point in the target and source coordinate reference systems.");

        /// <summary>
        /// Zone width.
        /// </summary>
        public static CoordinateOperationParameter ZoneWidth = new CoordinateOperationParameter("EPSG::8831", "Zone width", "The longitude width of a zone of a Transverse Mercator zoned grid system.");
    }
}
