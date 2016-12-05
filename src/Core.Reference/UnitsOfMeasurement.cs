// <copyright file="UnitsOfMeasurement.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using ELTE.AEGIS.Numerics;

    /// <summary>
    /// Represents a collection of known <see cref="UnitOfMeasurement" /> instances.
    /// </summary>
    public static class UnitsOfMeasurement
    {
        private static Lazy<UnitOfMeasurement> arcMinute = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9103", "arc-minute", null, new String[] { "min", "mina" }, "'", Math.PI / 10800, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> arcSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9104", "arc-second", null, new String[] { "sec", "seca" }, "\"", Math.PI / 648000, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> arcSecondsPerYear = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1043", "arc-seconds per year", " =((pi/180) / 3600) radians per year. Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new String[] { "sec/yr", "sec/a" }, "sec/a", 1.5363146893207598E-13, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> bin = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1024", "bin", "One of the dimensions of a seismic bin. Its size (in units of the bin grid's base coordinate reference system) is defined through an associated coordinate operation.", null, "bin", 1, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> binWidth12Metres = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9209", "Bin width 12.5 metres", null, null, "bin", 12.5, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> binWidth165Feet = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9205", "Bin width 165 US survey feet", null, null, "bin", 1980 / 39.37, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> binWidth25Metres = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9208", "Bin width 25 metres", null, null, "bin", 25, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> binWidth330Feet = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9204", "Bin width 330 US survey feet", null, null, "bin", 3960 / 39.37, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> binWidth37Metres = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9207", "Bin width 37.5 metres", null, null, "bin", 37.5, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> binWidth3Metres = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9211", "Bin width 3.125 metres", null, null, "bin", 3.125, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> binWidth6Metres = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9210", "Bin width 6.25 metres", null, null, "bin", 3960 / 39.37, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> binWidth82Feet = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9206", "Bin width 82.5 US survey feet", null, null, "bin", 990 / 39.37, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishChainBenoit1895A = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9052", "British chain (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new String[] { "chain", "chBnA" }, "ch", 20.1167824, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishChainBenoit1895B = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9062", "British chain (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new String[] { "chain", "chBnB" }, "ch", 20.11678249437587, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishChainSears1922 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9042", "British chain (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new String[] { "chain", "chSe" }, "ch", 20.116765121552632, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishChainSears1922T = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9301", "British chain (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 1 chSe(T) = 22 ydSe(T). Used in metrication of Malaya RSO grid.", new String[] { "chain", "chSe(T)" }, "ch", 20.116756, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishFoot1865 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9070", "British foot (1865)", "Uses Clark's estimate of 1853-1865 British foot-metre ratio of 0.9144025 metres per yard. Used in 1962 and 1975 estimates of Indian foot.", new String[] { "foot", "ftBr(65)" }, "ft", 0.30480083333333335, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishFoot1936 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9095", "British foot (1936)", "For the 1936 retriangulation OSGB defines the relationship of 10 feet of 1796 to the International metre through the logarithmic relationship (10^0.48401603 exactly). 1 ft = 0.3048007491…m. Also used for metric conversions in Ireland.", new String[] { "foot", "ftBr(36)" }, "ft", 0.3048007491, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishFootBenoit1895A = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9051", "British foot (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new String[] { "foot", "ftBnA" }, "ft", 0.3047997333333333, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishFootBenoit1895B = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9061", "British foot (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new String[] { "foot", "ftBnB" }, "ft", 0.30479973476327077, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishFootSears1922 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9041", "British foot (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new String[] { "foot", "ftSe" }, "ft", 0.3047994715386762, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishFootSears1922T = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9300", "British foot (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ", new String[] { "foot", "ftSe(T)" }, "ft", 0.3047994715386762, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishLinkBenoit1895A = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9053", "British link (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new String[] { "link", "ftBnA" }, "l", 0.201167824, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishLinkBenoit1895B = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9063", "British link (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new String[] { "link", "ftBnB" }, "l", 0.2011678249437587, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishLinkSears1922 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9043", "British link (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new String[] { "link", "ftSe" }, "l", 0.2011676512155263, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishLinkSears1922T = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9302", "British link (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ", new String[] { "link", "ftSe(T)" }, "l", 0.20116756, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishYardBenoit1895A = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9050", "British yard (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new String[] { "yard", "ftBnA" }, "yd", 0.9143992, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishYardBenoit1895B = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9060", "British yard (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new String[] { "yard", "ftBnB" }, "yd", 0.9143992042898124, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishYardSears1922 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9040", "British yard (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new String[] { "yard", "ftSe" }, "yd", 0.9143984146160287, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> britishYardSears1922T = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9099", "British yard (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ", new String[] { "yard", "ftSe(T)" }, "yd", 0.914398, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> centesimalMinute = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9112", "centesimal minute", "/100 of a grad and gon = ((pi/200) / 100) radians.", new String[] { "c", "cgr" }, "c", Math.PI / 20000, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> centesimalSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9113", "centesimal second", "1/100 of a centesimal minute or 1/10,000th of a grad and gon = ((pi/200) / 10000) radians.", new String[] { "cc", "ccgr" }, "cc", Math.PI / 2000000, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> centimetre = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1033", "centimetre", null, new String[] { "centimeter", "cm" }, "cm", 0.01, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> centimetresPerYear = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1034", "centimetres per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new String[] { "centimeters per year", "cm/yr", "cm/a" }, "cm/a", 3.1688765172731484E-10, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> chain = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9097", "chain", "=22 international yards or 66 international feet.", new String[] { "international chain", "ch" }, "ch", 20.1168, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> clarkesChain = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9038", "Clarke's chain", "=22 Clarke's yards. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new String[] { "chCla" }, "ch", 20.1166195164, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> clarkesFoot = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9005", "Clarke's foot", "Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new String[] { "South African geodetic foot", "ftCla" }, "ft", 0.3047972654, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> clarkesLink = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9039", "Clarke's link", "=1/100 Clarke's chain. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new String[] { "link (Clarke's ratio)", "lkCla" }, "l", 0.201166195164, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> clarkesYard = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9037", "Clarke's yard", "=3 Clarke's feet. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new String[] { "ydCla" }, "yd", 0.9143917962, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> coefficient = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9203", "coefficient", "Used when parameters are coefficients. They inherently take the units which depend upon the term to which the coefficient applies.", null, null, 1, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> degree = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9102", "degree", "= pi/180 radians", new String[] { "deg", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> degreeHemishere = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9116", "degree hemisphere", "Degree representation. Format: degrees (real, any precision) - hemisphere abbreviation (single character N S E or W). Convert to degrees using algorithm.", new String[] { "degH", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> degreeMinute = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9115", "degree minute", "Degree representation. Format: signed degrees (integer) - arc-minutes (real, any precision). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.", new String[] { "DM", "dega" }, "°'", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> degreeMinuteHemishere = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9118", "degree minute hemisphere", "Degree representation. Format: degrees (integer) - arc-minutes (real, any precision) - hemisphere abbreviation (single character N S E or W). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.", new String[] { "DMH", "dega" }, "°'", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> degreeMinuteSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9107", "degree minute second", "Degree representation. Format: signed degrees (integer) - arc-minutes (integer) - arc-seconds (real, any precision). Different symbol sets are in use as field separators, for example º ' \". Convert to degrees using algorithm.", new String[] { "DMS", "dega" }, "°'\"", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> degreeMinuteSecondHemishere = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9108", "degree minute second hemisphere", "Degree representation. Format: degrees (integer) - arc-minutes (integer) - arc-seconds (real) - hemisphere abbreviation (single character N S E or W). Different symbol sets are in use as field separators for example º ' \". Convert to deg using algorithm.", new String[] { "DMSH", "dega" }, "°'\"", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> degreeSupplierDefined = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9122", "degree (supplier to define representation)", "= pi/180 radians. The degree representation (e.g. decimal, DMSH, etc.) must be clarified by suppliers of data associated with this code.", new String[] { "deg", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> fathom = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9014", "fathom", null, new String[] { "f" }, "f", 1.8288, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> foot = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9002", "foot", null, new String[] { "ft" }, "ft", 0.3048, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> germanLegalMetre = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9031", "German legal metre", "Used in Namibia.", new String[] { "GLM", "mGer" }, "m", 1.0000135965, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> goldCoastFoot = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9094", "Gold Coast foot", "Used in Ghana and some adjacent parts of British west Africa prior to metrication, except for the metrication of projection defining parameters when British foot (Sears 1922) used.", new String[] { "foot", "ftGC" }, "ft", 0.3047997101815088, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> gon = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9106", "gon", "=pi/200 radians", new String[] { "g" }, "g", Math.PI / 200, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> grad = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9105", "grad", "=pi/200 radians", new String[] { "gr" }, "gr", Math.PI / 200, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> hemisphereDegree = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9117", "hemisphere degree", "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (real, any precision). Convert to degrees using algorithm.", new String[] { "Hdeg", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> hemisphereDegreeMinute = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9119", "hemisphere degree minute", "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (integer) - arc-minutes (real, any precision). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.", new String[] { "HDM", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> hemisphereDegreeMinuteSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9120", "hemisphere degree minute second", "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (integer) - arc-minutes (integer) - arc-seconds (real). Different symbol sets are in use as field separators for example º ' \". Convert to deg using algorithm.", new String[] { "HDMS", "dega" }, "°'|\"", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> indianFoot = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9080", "Indian foot", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (= 3 British feet) taken to be J.S.Clark's 1865 value of 0.9144025 metres.", new String[] { "Indian geodetic foot", "ftInd" }, "ft", 0.30479951024814694, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> indianFoot1937 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9081", "Indian foot (1937)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British foot taken to be 1895 Benoit value of 12/39.370113m. Rounded to 8 decimal places as 0.30479841. Used from Bangladesh to Vietnam. Previously used in India and Pakistan but superseded.", new String[] { "Indian geodetic foot", "ftInd(37)" }, "ft", 0.30479841, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> indianFoot1962 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9082", "Indian foot (1962)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures with a small error as 1 Ind ft=0.3047996m. Used in Pakistan since metrication.", new String[] { "ftInd(62)" }, "ft", 0.3047996, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> indianFoot1975 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9083", "Indian foot (1975)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures as 1 Ind ft=0.3047995m. Used in India since metrication.", new String[] { "ftInd(75)" }, "ft", 0.3047995, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> indianYard = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9084", "Indian yard", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (= 3 British feet) taken to be J.S.Clark's 1865 value of 0.9144025 metres.", new String[] { "yard", "ydInd" }, "yd", 0.9143985307444408, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> indianYard1937 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9085", "Indian yard (1937)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be 1895 Benoit value of 12/39.370113m. Rounded to 8 decimal places as 0.30479841. Used from Bangladesh to Vietnam. Previously used in India and Pakistan but superseded.", new String[] { "yard", "ydInd(37)" }, "yd", 0.9143952, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> indianYard1962 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9086", "Indian yard (1962)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures with a small error as 1 Ind ft=0.3047996m. Used in Pakistan since metrication.", new String[] { "ydInd(62)" }, "yd", 0.9143988, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> indianYard1975 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9087", "Indian yard (1975)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures as 1 Ind ft=0.3047995m. Used in India since metrication.", new String[] { "ydInd(75)" }, "yd", 0.9143985, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> kilometre = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9036", "kilometre", null, new String[] { "kilometer", "km" }, "km", 1000, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> link = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9098", "link", "=1/100 international chain", new String[] { "international link", "lk" }, "lk", 0.201168, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> metre = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9001", "metre", null, new String[] { "meter", "International metre", "m" }, "m", 1, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> metresPerSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1026", "metres per second", "Length rate.", new String[] { "meters per second", "m/s", }, "m/s", 3.1688765172731486E-8, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> metresPerYear = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1042", "metres per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new String[] { "meters per year", "m/yr", "m/a" }, "m/a", 3.1688765172731486E-8, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> microRadian = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9109", "microradian", "rad * 10E-6", new String[] { "µrad", "urad" }, "µrad", 0.000001, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> mil6400 = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9114", "mil_6400", "Angle subtended by 1/6400 part of a circle. Approximates to 1/1000th radian. Note that other approximations (notably 1/6300 circle and 1/6000 circle) also exist.", new String[] { "mil", "mila" }, "mil", 0.0009817477042468094, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> milliarcSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1031", "milliarc-second", "= ((pi/180) / 3600 / 1000) radians", new String[] { "mas", "msec" }, "msec", 4.848136811095355E-9, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> milliarcSecondPerYear = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1032 ", "milliarc-second per year", "= ((pi/180) / 3600 / 1000) radians per year. Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new String[] { "mas/yr", "msec/yr", "msec/a" }, "msec/a", 1.5363146893207598E-16, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> millimetre = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1025", "millimetre", null, new String[] { "millimeter", "mm" }, "mm", 0.001, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> millimetrePerYear = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1027", "millimetre per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new String[] { "millimeters per year", "mm/yr", "mm/a" }, "mm/a", 3.168876517273149E-11, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> nauticalMile = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9030", "nautical mile", null, new String[] { "NM", "nautmi" }, "NM", 1852, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> partsPerBillion = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1028", "parts per billion", "Billion is internationally ambiguous, in different languages being 1E+9 and 1E+12. One billion taken here to be 1E+9.", new String[] { "ppb", "nm/m" }, "ppb", 1.0E-9, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> partsPerBillionPerYear = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1030", "parts per billion per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029. Billion is internationally ambiguous, in different languages being 1E+9 and 1E+12. One billion taken here to be 1E+9.", new String[] { "ppb/yr", "ppb/a", "nm/m/a" }, "ppb/a", 3.1688765172731483E-17, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> partsPerMillion = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9202", "parts per million", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new String[] { "ppm", "µm/m" }, "ppm", 0.000001, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> partsPerMillionPerYear = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1041", "parts per million", null, new String[] { "ppm/yr", "ppm/a", "µm/m/a" }, "ppm/a", 3.1688765172731486E-14, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> radian = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9101", "radian", null, new String[] { "rad" }, "rad", 1, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> radiansPerSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1035", "radians per second", "Angle rate.", new String[] { "rad/s" }, "rad/s", 1, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> second = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1040", "second", "Not to be confused with the angle unit arc-second.", null, "s", 1, UnitQuantityType.Time));

        private static Lazy<UnitOfMeasurement> sexagesimalDM = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9111", "sexagesimal DM", "Pseudo unit. Format: sign - degrees - decimal point - integer minutes (two digits) - fraction of minutes (any precision). Must include leading zero in integer minutes. Must exclude decimal point for minutes. Convert to deg using algorithm.", new String[] { "DDD.MMm", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> sexagesimalDMS = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9110", "sexagesimal DMS", "Pseudo unit. Format: signed degrees - period - minutes (2 digits) - integer seconds (2 digits) - fraction of seconds (any precision). Must include leading zero in minutes and seconds and exclude decimal point for seconds. Convert to degree using formula.", new String[] { "DDD.MMSSsss", "dega", "sexagesimal degree" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> sexagesimalDMSs = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9121", "sexagesimal DM", "Pseudo unit. Format: signed degrees - minutes (two digits) - seconds (real, any precision). Must include leading zero in minutes and seconds where value is under 10 and include decimal separator for seconds. Convert to degree using algorithm.", new String[] { "DDDMMSS.sss", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle));

        private static Lazy<UnitOfMeasurement> statuteMile = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9093", "Statute mile", "=5280 feet", new String[] { "mi" }, "M", 1609.344, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> unity = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9201", "unity", null, 1, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> unityPerSecond = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1036", "unity per second", "Scale rate.", null, null, 1, UnitQuantityType.Scale));

        private static Lazy<UnitOfMeasurement> usSurveyChain = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9033", "US survey chain", "Used in USA primarily for public lands cadastral work.", new String[] { "chUS" }, "ch", 20.11684023368047, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> usSurveyFoot = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9003", "US survey foot", "Used in USA.", new String[] { "American foot", "ftUS" }, "ft", 0.30480060960121924, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> usSurveyLink = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9034", "US survey link", "Used in USA primarily for public lands cadastral work.", new String[] { "lkUS" }, "lk", 0.2011684023368047, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> usSurveyMile = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9035", "US survey mile", "Used in USA primarily for public lands cadastral work.", new String[] { "miUS" }, "M", 1609.3472186944375, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> yard = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::9096", "yard", "=3 international feet.", new String[] { "international yard", "yd" }, "yd", 0.9144, UnitQuantityType.Length));

        private static Lazy<UnitOfMeasurement> year = new Lazy<UnitOfMeasurement>(() => new UnitOfMeasurement("EPSG::1029", "year", null, 3.1556925445E7, UnitQuantityType.Time));

        /// <summary>
        /// Gets arc-minute.
        /// </summary>
        public static UnitOfMeasurement ArcMinute { get { return arcMinute.Value; } }

        /// <summary>
        /// Gets arc-second.
        /// </summary>
        public static UnitOfMeasurement ArcSecond { get { return arcSecond.Value; } }

        /// <summary>
        /// Gets centesimal minute.
        /// </summary>
        public static UnitOfMeasurement CentesimalMinute { get { return centesimalMinute.Value; } }

        /// <summary>
        /// Gets centesimal second.
        /// </summary>
        public static UnitOfMeasurement CentesimalSecond { get { return centesimalSecond.Value; } }

        /// <summary>
        /// Gets degree.
        /// </summary>
        public static UnitOfMeasurement Degree { get { return degree.Value; } }

        /// <summary>
        /// Gets degree hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeHemisphere { get { return degreeHemishere.Value; } }

        /// <summary>
        /// Gets degree minute.
        /// </summary>
        public static UnitOfMeasurement DegreeMinute { get { return degreeMinute.Value; } }

        /// <summary>
        /// Gets degree minute hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteHemisphere { get { return degreeMinuteHemishere.Value; } }

        /// <summary>
        /// Gets degree minute second.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteSecond { get { return degreeMinuteSecond.Value; } }

        /// <summary>
        /// Gets degree minute second hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteSecondHemisphere { get { return degreeMinuteSecondHemishere.Value; } }

        /// <summary>
        /// Gets degree (supplier to define representation).
        /// </summary>
        public static UnitOfMeasurement DegreeSupplierDefined { get { return degreeSupplierDefined.Value; } }

        /// <summary>
        /// Gets gon.
        /// </summary>
        public static UnitOfMeasurement Gon { get { return gon.Value; } }

        /// <summary>
        /// Gets grad.
        /// </summary>
        public static UnitOfMeasurement Grad { get { return grad.Value; } }

        /// <summary>
        /// Gets hemisphere degree.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegree { get { return hemisphereDegree.Value; } }

        /// <summary>
        /// Gets hemisphere degree minute.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegreeMinute { get { return hemisphereDegreeMinute.Value; } }

        /// <summary>
        /// Gets hemisphere degree minute second.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegreeMinuteSecond { get { return hemisphereDegreeMinuteSecond.Value; } }

        /// <summary>
        /// Gets microradian.
        /// </summary>
        public static UnitOfMeasurement MicroRadian { get { return microRadian.Value; } }

        /// <summary>
        /// Gets mil_6400.
        /// </summary>
        public static UnitOfMeasurement Mil6400 { get { return mil6400.Value; } }

        /// <summary>
        /// Gets milliarc-second.
        /// </summary>
        public static UnitOfMeasurement MilliarcSecond { get { return milliarcSecond.Value; } }

        /// <summary>
        /// Gets radian.
        /// </summary>
        public static UnitOfMeasurement Radian { get { return radian.Value; } }

        /// <summary>
        /// Gets sexagesimal DM.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDM { get { return sexagesimalDM.Value; } }

        /// <summary>
        /// Gets sexagesimal DMS.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDMS { get { return sexagesimalDMS.Value; } }

        /// <summary>
        /// Gets sexagesimal DMS.s.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDMSs { get { return sexagesimalDMSs.Value; } }

        /// <summary>
        /// Gets arc-seconds per year.
        /// </summary>
        public static UnitOfMeasurement ArcSecondsPerYear { get { return arcSecondsPerYear.Value; } }

        /// <summary>
        /// Gets milliarc-second per year.
        /// </summary>
        public static UnitOfMeasurement MilliarcSecondPerYear { get { return milliarcSecondPerYear.Value; } }

        /// <summary>
        /// Gets radians per second.
        /// </summary>
        public static UnitOfMeasurement RadiansPerSecond { get { return radiansPerSecond.Value; } }

        /// <summary>
        /// Gets bin width 12.5 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth12Metres { get { return binWidth12Metres.Value; } }

        /// <summary>
        /// Gets bin width 165 US survey feet.
        /// </summary>
        public static UnitOfMeasurement BinWidth165Feet { get { return binWidth165Feet.Value; } }

        /// <summary>
        /// Gets bin width 25 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth25Metres { get { return binWidth25Metres.Value; } }

        /// <summary>
        /// Gets bin width 330 US survey feet.
        /// </summary>
        public static UnitOfMeasurement BinWidth330Feet { get { return binWidth330Feet.Value; } }

        /// <summary>
        /// Gets bin width 37.5 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth37Metres { get { return binWidth37Metres.Value; } }

        /// <summary>
        /// Gets bin width 3.125 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth3Metres { get { return binWidth3Metres.Value; } }

        /// <summary>
        /// Gets bin width 6.25 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth6Metres { get { return binWidth6Metres.Value; } }

        /// <summary>
        /// Gets bin width 82.5 US survey feet.
        /// </summary>
        public static UnitOfMeasurement BinWidth82Feet { get { return binWidth82Feet.Value; } }

        /// <summary>
        /// Gets British chain (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishChainBenoit1895A { get { return britishChainBenoit1895A.Value; } }

        /// <summary>
        /// Gets British chain (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishChainBenoit1895B { get { return britishChainBenoit1895B.Value; } }

        /// <summary>
        /// Gets British chain (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishChainSears1922 { get { return britishChainSears1922.Value; } }

        /// <summary>
        /// Gets British chain (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishChainSears1922T { get { return britishChainSears1922T.Value; } }

        /// <summary>
        /// Gets British foot (1865).
        /// </summary>
        public static UnitOfMeasurement BritishFoot1865 { get { return britishFoot1865.Value; } }

        /// <summary>
        /// Gets British foot (1936).
        /// </summary>
        public static UnitOfMeasurement BritishFoot1936 { get { return britishFoot1936.Value; } }

        /// <summary>
        /// Gets British foot (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishFootBenoit1895A { get { return britishFootBenoit1895A.Value; } }

        /// <summary>
        /// Gets British foot (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishFootBenoit1895B { get { return britishFootBenoit1895B.Value; } }

        /// <summary>
        /// Gets British foot (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishFootSears1922 { get { return britishFootSears1922.Value; } }

        /// <summary>
        /// Gets British foot (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishFootSears1922T { get { return britishFootSears1922T.Value; } }

        /// <summary>
        /// Gets British link (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishLinkBenoit1895A { get { return britishLinkBenoit1895A.Value; } }

        /// <summary>
        /// Gets British link (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishLinkBenoit1895B { get { return britishLinkBenoit1895B.Value; } }

        /// <summary>
        /// Gets British link (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishLinkSears1922 { get { return britishLinkSears1922.Value; } }

        /// <summary>
        /// Gets British link (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishLinkSears1922T { get { return britishLinkSears1922T.Value; } }

        /// <summary>
        /// Gets British yard (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishYardBenoit1895A { get { return britishYardBenoit1895A.Value; } }

        /// <summary>
        /// Gets British yard (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishYardBenoit1895B { get { return britishYardBenoit1895B.Value; } }

        /// <summary>
        /// Gets British yard (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishYardSears1922 { get { return britishYardSears1922.Value; } }

        /// <summary>
        /// Gets British yard (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishYardSears1922T { get { return britishYardSears1922T.Value; } }

        /// <summary>
        /// Gets centimetre.
        /// </summary>
        public static UnitOfMeasurement Centimetre { get { return centimetre.Value; } }

        /// <summary>
        /// Gets chain.
        /// </summary>
        public static UnitOfMeasurement Chain { get { return chain.Value; } }

        /// <summary>
        /// Gets Clarke's chain.
        /// </summary>
        public static UnitOfMeasurement ClarkesChain { get { return clarkesChain.Value; } }

        /// <summary>
        /// Gets Clarke's foot.
        /// </summary>
        public static UnitOfMeasurement ClarkesFoot { get { return clarkesFoot.Value; } }

        /// <summary>
        /// Gets Clarke's link.
        /// </summary>
        public static UnitOfMeasurement ClarkesLink { get { return clarkesLink.Value; } }

        /// <summary>
        /// Gets Clarke's yard.
        /// </summary>
        public static UnitOfMeasurement ClarkesYard { get { return clarkesYard.Value; } }

        /// <summary>
        /// Gets fathom.
        /// </summary>
        public static UnitOfMeasurement Fathom { get { return fathom.Value; } }

        /// <summary>
        /// Gets foot.
        /// </summary>
        public static UnitOfMeasurement Foot { get { return foot.Value; } }

        /// <summary>
        /// Gets German legal metre.
        /// </summary>
        public static UnitOfMeasurement GermanLegalMetre { get { return germanLegalMetre.Value; } }

        /// <summary>
        /// Gets gold Coast foot.
        /// </summary>
        public static UnitOfMeasurement GoldCoastFoot { get { return goldCoastFoot.Value; } }

        /// <summary>
        /// Gets Indian foot.
        /// </summary>
        public static UnitOfMeasurement IndianFoot { get { return indianFoot.Value; } }

        /// <summary>
        /// Gets Indian foot (1937).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1937 { get { return indianFoot1937.Value; } }

        /// <summary>
        /// Gets Indian foot (1962).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1962 { get { return indianFoot1962.Value; } }

        /// <summary>
        /// Gets Indian foot (1975).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1975 { get { return indianFoot1975.Value; } }

        /// <summary>
        /// Gets Indian yard.
        /// </summary>
        public static UnitOfMeasurement IndianYard { get { return indianYard.Value; } }

        /// <summary>
        /// Gets Indian yard (1937).
        /// </summary>
        public static UnitOfMeasurement IndianYard1937 { get { return indianYard1937.Value; } }

        /// <summary>
        /// Gets Indian yard (1962).
        /// </summary>
        public static UnitOfMeasurement IndianYard1962 { get { return indianYard1962.Value; } }

        /// <summary>
        /// Gets Indian yard (1975).
        /// </summary>
        public static UnitOfMeasurement IndianYard1975 { get { return indianYard1975.Value; } }

        /// <summary>
        /// Gets kilometre.
        /// </summary>
        public static UnitOfMeasurement Kilometre { get { return kilometre.Value; } }

        /// <summary>
        /// Gets link.
        /// </summary>
        public static UnitOfMeasurement Link { get { return link.Value; } }

        /// <summary>
        /// Gets metre.
        /// </summary>
        public static UnitOfMeasurement Metre { get { return metre.Value; } }

        /// <summary>
        /// Gets millimetre.
        /// </summary>
        public static UnitOfMeasurement Millimetre { get { return millimetre.Value; } }

        /// <summary>
        /// Gets nautical mile.
        /// </summary>
        public static UnitOfMeasurement NauticalMile { get { return nauticalMile.Value; } }

        /// <summary>
        /// Gets statute mile.
        /// </summary>
        public static UnitOfMeasurement StatuteMile { get { return statuteMile.Value; } }

        /// <summary>
        /// Gets uS survey chain.
        /// </summary>
        public static UnitOfMeasurement USSurveyChain { get { return usSurveyChain.Value; } }

        /// <summary>
        /// Gets uS survey foot.
        /// </summary>
        public static UnitOfMeasurement USSurveyFoot { get { return usSurveyFoot.Value; } }

        /// <summary>
        /// Gets uS survey link.
        /// </summary>
        public static UnitOfMeasurement USSurveyLink { get { return usSurveyLink.Value; } }

        /// <summary>
        /// Gets uS survey mile.
        /// </summary>
        public static UnitOfMeasurement USSurveyMile { get { return usSurveyMile.Value; } }

        /// <summary>
        /// Gets yard.
        /// </summary>
        public static UnitOfMeasurement Yard { get { return yard.Value; } }

        /// <summary>
        /// Gets centimetres per year.
        /// </summary>
        public static UnitOfMeasurement CentimetresPerYear { get { return centimetresPerYear.Value; } }

        /// <summary>
        /// Gets metres per second.
        /// </summary>
        public static UnitOfMeasurement MetresPerSecond { get { return metresPerSecond.Value; } }

        /// <summary>
        /// Gets metres per year.
        /// </summary>
        public static UnitOfMeasurement MetresPerYear { get { return metresPerYear.Value; } }

        /// <summary>
        /// Gets millimetre per year.
        /// </summary>
        public static UnitOfMeasurement MillimetrePerYear { get { return millimetrePerYear.Value; } }

        /// <summary>
        /// Gets bin.
        /// </summary>
        public static UnitOfMeasurement Bin { get { return bin.Value; } }

        /// <summary>
        /// Gets coefficient.
        /// </summary>
        public static UnitOfMeasurement Coefficient { get { return coefficient.Value; } }

        /// <summary>
        /// Gets parts per billion.
        /// </summary>
        public static UnitOfMeasurement PartsPerBillion { get { return partsPerBillion.Value; } }

        /// <summary>
        /// Gets parts per million.
        /// </summary>
        public static UnitOfMeasurement PartsPerMillion { get { return partsPerMillion.Value; } }

        /// <summary>
        /// Gets unity.
        /// </summary>
        public static UnitOfMeasurement Unity { get { return unity.Value; } }

        /// <summary>
        /// Gets parts per billion per year.
        /// </summary>
        public static UnitOfMeasurement PartsPerBillionPerYear { get { return partsPerBillionPerYear.Value; } }

        /// <summary>
        /// Gets parts per million.
        /// </summary>
        public static UnitOfMeasurement PartsPerMillionPerYear { get { return partsPerMillionPerYear.Value; } }

        /// <summary>
        /// Gets unity per second.
        /// </summary>
        public static UnitOfMeasurement UnityPerSecond { get { return unityPerSecond.Value; } }

        /// <summary>
        /// Gets second.
        /// </summary>
        public static UnitOfMeasurement Second { get { return second.Value; } }

        /// <summary>
        /// Gets year.
        /// </summary>
        public static UnitOfMeasurement Year { get { return year.Value; } }
    }
}
