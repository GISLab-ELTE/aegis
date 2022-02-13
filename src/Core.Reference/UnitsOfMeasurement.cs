// <copyright file="UnitsOfMeasurement.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    using System;
    using AEGIS.Numerics;

    /// <summary>
    /// Represents a collection of available <see cref="UnitOfMeasurement" /> instances, as defined by the <see cref="http://www.epsg.org/">EPSG Geodetic Parameter Dataset</see>.
    /// </summary>
    public static class UnitsOfMeasurement
    {
        /// <summary>
        /// Arc-minute.
        /// </summary>
        public static UnitOfMeasurement ArcMinute = new UnitOfMeasurement("EPSG::9103", "arc-minute", null, new[] { "min", "mina" }, "'", Math.PI / 10800, UnitQuantityType.Angle);

        /// <summary>
        /// Arc-second.
        /// </summary>
        public static UnitOfMeasurement ArcSecond = new UnitOfMeasurement("EPSG::9104", "arc-second", null, new[] { "sec", "seca" }, "\"", Math.PI / 648000, UnitQuantityType.Angle);

        /// <summary>
        /// Arc-seconds per year.
        /// </summary>
        public static UnitOfMeasurement ArcSecondsPerYear = new UnitOfMeasurement("EPSG::1043", "arc-seconds per year", " =((pi/180) / 3600) radians per year. Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new[] { "sec/yr", "sec/a" }, "sec/a", 1.5363146893207598E-13, UnitQuantityType.Angle);

        /// <summary>
        /// Bin.
        /// </summary>
        public static UnitOfMeasurement Bin = new UnitOfMeasurement("EPSG::1024", "bin", "One of the dimensions of a seismic bin. Its size (in units of the bin grid's base coordinate reference system) is defined through an associated coordinate operation.", null, "bin", 1, UnitQuantityType.Scale);

        /// <summary>
        /// Bin.
        /// </summary>
        public static UnitOfMeasurement BinWidth12Metres = new UnitOfMeasurement("EPSG::9209", "Bin width 12.5 metres", null, null, "bin", 12.5, UnitQuantityType.Length);

        /// <summary>
        /// Bin width 165 US survey feet.
        /// </summary>
        public static UnitOfMeasurement BinWidth165Feet = new UnitOfMeasurement("EPSG::9205", "Bin width 165 US survey feet", null, null, "bin", 1980 / 39.37, UnitQuantityType.Length);

        /// <summary>
        /// Bin width 25 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth25Metres = new UnitOfMeasurement("EPSG::9208", "Bin width 25 metres", null, null, "bin", 25, UnitQuantityType.Length);

        /// <summary>
        /// Bin width 330 US survey feet.
        /// </summary>
        public static UnitOfMeasurement BinWidth330Feet = new UnitOfMeasurement("EPSG::9204", "Bin width 330 US survey feet", null, null, "bin", 3960 / 39.37, UnitQuantityType.Length);

        /// <summary>
        /// Bin width 37.5 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth37Metres = new UnitOfMeasurement("EPSG::9207", "Bin width 37.5 metres", null, null, "bin", 37.5, UnitQuantityType.Length);

        /// <summary>
        /// Bin width 3.125 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth3Metres = new UnitOfMeasurement("EPSG::9211", "Bin width 3.125 metres", null, null, "bin", 3.125, UnitQuantityType.Length);

        /// <summary>
        /// Bin width 6.25 metres.
        /// </summary>
        public static UnitOfMeasurement BinWidth6Metres = new UnitOfMeasurement("EPSG::9210", "Bin width 6.25 metres", null, null, "bin", 3960 / 39.37, UnitQuantityType.Length);

        /// <summary>
        /// Bin width 82.5 US survey feet.
        /// </summary>
        public static UnitOfMeasurement BinWidth82Feet = new UnitOfMeasurement("EPSG::9206", "Bin width 82.5 US survey feet", null, null, "bin", 990 / 39.37, UnitQuantityType.Length);

        /// <summary>
        /// British chain (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishChainBenoit1895A = new UnitOfMeasurement("EPSG::9052", "British chain (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new[] { "chain", "chBnA" }, "ch", 20.1167824, UnitQuantityType.Length);

        /// <summary>
        /// British chain (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishChainBenoit1895B = new UnitOfMeasurement("EPSG::9062", "British chain (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new[] { "chain", "chBnB" }, "ch", 20.11678249437587, UnitQuantityType.Length);

        /// <summary>
        /// British chain (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishChainSears1922 = new UnitOfMeasurement("EPSG::9042", "British chain (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new[] { "chain", "chSe" }, "ch", 20.116765121552632, UnitQuantityType.Length);

        /// <summary>
        /// British chain (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishChainSears1922T = new UnitOfMeasurement("EPSG::9301", "British chain (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 1 chSe(T) = 22 ydSe(T). Used in metrication of Malaya RSO grid.", new[] { "chain", "chSe(T)" }, "ch", 20.116756, UnitQuantityType.Length);

        /// <summary>
        /// British foot (1865).
        /// </summary>
        public static UnitOfMeasurement BritishFoot1865 = new UnitOfMeasurement("EPSG::9070", "British foot (1865)", "Uses Clark's estimate of 1853-1865 British foot-metre ratio of 0.9144025 metres per yard. Used in 1962 and 1975 estimates of Indian foot.", new[] { "foot", "ftBr(65)" }, "ft", 0.30480083333333335, UnitQuantityType.Length);

        /// <summary>
        /// British foot (1936).
        /// </summary>
        public static UnitOfMeasurement BritishFoot1936 = new UnitOfMeasurement("EPSG::9095", "British foot (1936)", "For the 1936 retriangulation OSGB defines the relationship of 10 feet of 1796 to the International metre through the logarithmic relationship (10^0.48401603 exactly). 1 ft = 0.3048007491…m. Also used for metric conversions in Ireland.", new[] { "foot", "ftBr(36)" }, "ft", 0.3048007491, UnitQuantityType.Length);

        /// <summary>
        /// British foot (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishFootBenoit1895A = new UnitOfMeasurement("EPSG::9051", "British foot (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new[] { "foot", "ftBnA" }, "ft", 0.3047997333333333, UnitQuantityType.Length);

        /// <summary>
        /// British foot (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishFootBenoit1895B = new UnitOfMeasurement("EPSG::9061", "British foot (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new[] { "foot", "ftBnB" }, "ft", 0.30479973476327077, UnitQuantityType.Length);

        /// <summary>
        /// British foot (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishFootSears1922 = new UnitOfMeasurement("EPSG::9041", "British foot (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new[] { "foot", "ftSe" }, "ft", 0.3047994715386762, UnitQuantityType.Length);

        /// <summary>
        /// British foot (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishFootSears1922T = new UnitOfMeasurement("EPSG::9300", "British foot (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ", new[] { "foot", "ftSe(T)" }, "ft", 0.3047994715386762, UnitQuantityType.Length);

        /// <summary>
        /// British link (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishLinkBenoit1895A = new UnitOfMeasurement("EPSG::9053", "British link (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new[] { "link", "ftBnA" }, "l", 0.201167824, UnitQuantityType.Length);

        /// <summary>
        /// British link (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishLinkBenoit1895B = new UnitOfMeasurement("EPSG::9063", "British link (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new[] { "link", "ftBnB" }, "l", 0.2011678249437587, UnitQuantityType.Length);

        /// <summary>
        /// British link (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishLinkSears1922 = new UnitOfMeasurement("EPSG::9043", "British link (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new[] { "link", "ftSe" }, "l", 0.2011676512155263, UnitQuantityType.Length);

        /// <summary>
        /// British link (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishLinkSears1922T = new UnitOfMeasurement("EPSG::9302", "British link (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ", new[] { "link", "ftSe(T)" }, "l", 0.20116756, UnitQuantityType.Length);

        /// <summary>
        /// British yard (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishYardBenoit1895A = new UnitOfMeasurement("EPSG::9050", "British yard (Benoit 1895 A)", "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.", new[] { "yard", "ftBnA" }, "yd", 0.9143992, UnitQuantityType.Length);

        /// <summary>
        /// British yard (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishYardBenoit1895B = new UnitOfMeasurement("EPSG::9060", "British yard (Benoit 1895 B)", "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.", new[] { "yard", "ftBnB" }, "yd", 0.9143992042898124, UnitQuantityType.Length);

        /// <summary>
        /// British yard (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishYardSears1922 = new UnitOfMeasurement("EPSG::9040", "British yard (Sears 1922)", "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.", new[] { "yard", "ftSe" }, "yd", 0.9143984146160287, UnitQuantityType.Length);

        /// <summary>
        /// British yard (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishYardSears1922T = new UnitOfMeasurement("EPSG::9099", "British yard (Sears 1922 truncated)", "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ", new[] { "yard", "ftSe(T)" }, "yd", 0.914398, UnitQuantityType.Length);

        /// <summary>
        /// Centesimal minute.
        /// </summary>
        public static UnitOfMeasurement CentesimalMinute = new UnitOfMeasurement("EPSG::9112", "centesimal minute", "/100 of a grad and gon = ((pi/200) / 100) radians.", new[] { "c", "cgr" }, "c", Math.PI / 20000, UnitQuantityType.Angle);

        /// <summary>
        /// Centesimal second.
        /// </summary>
        public static UnitOfMeasurement CentesimalSecond = new UnitOfMeasurement("EPSG::9113", "centesimal second", "1/100 of a centesimal minute or 1/10,000th of a grad and gon = ((pi/200) / 10000) radians.", new[] { "cc", "ccgr" }, "cc", Math.PI / 2000000, UnitQuantityType.Angle);

        /// <summary>
        /// Centimetre.
        /// </summary>
        public static UnitOfMeasurement Centimetre = new UnitOfMeasurement("EPSG::1033", "centimetre", null, new[] { "centimeter", "cm" }, "cm", 0.01, UnitQuantityType.Length);

        /// <summary>
        /// Centimetres per year.
        /// </summary>
        public static UnitOfMeasurement CentimetresPerYear = new UnitOfMeasurement("EPSG::1034", "centimetres per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new[] { "centimeters per year", "cm/yr", "cm/a" }, "cm/a", 3.1688765172731484E-10, UnitQuantityType.Length);

        /// <summary>
        /// Chain.
        /// </summary>
        public static UnitOfMeasurement Chain = new UnitOfMeasurement("EPSG::9097", "chain", "=22 international yards or 66 international feet.", new[] { "international chain", "ch" }, "ch", 20.1168, UnitQuantityType.Length);

        /// <summary>
        /// Clarke's chain.
        /// </summary>
        public static UnitOfMeasurement ClarkesChain = new UnitOfMeasurement("EPSG::9038", "Clarke's chain", "=22 Clarke's yards. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new[] { "chCla" }, "ch", 20.1166195164, UnitQuantityType.Length);

        /// <summary>
        /// Clarke's foot.
        /// </summary>
        public static UnitOfMeasurement ClarkesFoot = new UnitOfMeasurement("EPSG::9005", "Clarke's foot", "Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new[] { "South African geodetic foot", "ftCla" }, "ft", 0.3047972654, UnitQuantityType.Length);

        /// <summary>
        /// Clarke's link.
        /// </summary>
        public static UnitOfMeasurement ClarkesLink = new UnitOfMeasurement("EPSG::9039", "Clarke's link", "=1/100 Clarke's chain. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new[] { "link (Clarke's ratio)", "lkCla" }, "l", 0.201166195164, UnitQuantityType.Length);

        /// <summary>
        /// Clarke's yard.
        /// </summary>
        public static UnitOfMeasurement ClarkesYard = new UnitOfMeasurement("EPSG::9037", "Clarke's yard", "=3 Clarke's feet. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.", new[] { "ydCla" }, "yd", 0.9143917962, UnitQuantityType.Length);

        /// <summary>
        /// Coefficient.
        /// </summary>
        public static UnitOfMeasurement Coefficient = new UnitOfMeasurement("EPSG::9203", "coefficient", "Used when parameters are coefficients. They inherently take the units which depend upon the term to which the coefficient applies.", null, null, 1, UnitQuantityType.Scale);

        /// <summary>
        /// Degree.
        /// </summary>
        public static UnitOfMeasurement Degree = new UnitOfMeasurement("EPSG::9102", "degree", "= pi/180 radians", new[] { "deg", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Degree hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeHemishere = new UnitOfMeasurement("EPSG::9116", "degree hemisphere", "Degree representation. Format: degrees (real, any precision) - hemisphere abbreviation (single character N S E or W). Convert to degrees using algorithm.", new[] { "degH", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Degree minute.
        /// </summary>
        public static UnitOfMeasurement DegreeMinute = new UnitOfMeasurement("EPSG::9115", "degree minute", "Degree representation. Format: signed degrees (integer) - arc-minutes (real, any precision). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.", new[] { "DM", "dega" }, "°'", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Degree minute hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteHemishere = new UnitOfMeasurement("EPSG::9118", "degree minute hemisphere", "Degree representation. Format: degrees (integer) - arc-minutes (real, any precision) - hemisphere abbreviation (single character N S E or W). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.", new[] { "DMH", "dega" }, "°'", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Degree minute second.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteSecond = new UnitOfMeasurement("EPSG::9107", "degree minute second", "Degree representation. Format: signed degrees (integer) - arc-minutes (integer) - arc-seconds (real, any precision). Different symbol sets are in use as field separators, for example º ' \". Convert to degrees using algorithm.", new[] { "DMS", "dega" }, "°'\"", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Degree minute second hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteSecondHemishere = new UnitOfMeasurement("EPSG::9108", "degree minute second hemisphere", "Degree representation. Format: degrees (integer) - arc-minutes (integer) - arc-seconds (real) - hemisphere abbreviation (single character N S E or W). Different symbol sets are in use as field separators for example º ' \". Convert to deg using algorithm.", new[] { "DMSH", "dega" }, "°'\"", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Degree (supplier to define representation).
        /// </summary>
        public static UnitOfMeasurement DegreeSupplierDefined = new UnitOfMeasurement("EPSG::9122", "degree (supplier to define representation)", "= pi/180 radians. The degree representation (e.g. decimal, DMSH, etc.) must be clarified by suppliers of data associated with this code.", new[] { "deg", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Fathom.
        /// </summary>
        public static UnitOfMeasurement Fathom = new UnitOfMeasurement("EPSG::9014", "fathom", null, new[] { "f" }, "f", 1.8288, UnitQuantityType.Length);

        /// <summary>
        /// Foot.
        /// </summary>
        public static UnitOfMeasurement Foot = new UnitOfMeasurement("EPSG::9002", "foot", null, new[] { "ft" }, "ft", 0.3048, UnitQuantityType.Length);

        /// <summary>
        /// German legal metre.
        /// </summary>
        public static UnitOfMeasurement GermanLegalMetre = new UnitOfMeasurement("EPSG::9031", "German legal metre", "Used in Namibia.", new[] { "GLM", "mGer" }, "m", 1.0000135965, UnitQuantityType.Length);

        /// <summary>
        /// Gold Coast foot.
        /// </summary>
        public static UnitOfMeasurement GoldCoastFoot = new UnitOfMeasurement("EPSG::9094", "Gold Coast foot", "Used in Ghana and some adjacent parts of British west Africa prior to metrication, except for the metrication of projection defining parameters when British foot (Sears 1922) used.", new[] { "foot", "ftGC" }, "ft", 0.3047997101815088, UnitQuantityType.Length);

        /// <summary>
        /// Gon.
        /// </summary>
        public static UnitOfMeasurement Gon = new UnitOfMeasurement("EPSG::9106", "gon", "=pi/200 radians", new[] { "g" }, "g", Math.PI / 200, UnitQuantityType.Angle);

        /// <summary>
        /// Grad.
        /// </summary>
        public static UnitOfMeasurement Grad = new UnitOfMeasurement("EPSG::9105", "grad", "=pi/200 radians", new[] { "gr" }, "gr", Math.PI / 200, UnitQuantityType.Angle);

        /// <summary>
        /// Hemisphere degree.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegree = new UnitOfMeasurement("EPSG::9117", "hemisphere degree", "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (real, any precision). Convert to degrees using algorithm.", new[] { "Hdeg", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Hemisphere degree minute.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegreeMinute = new UnitOfMeasurement("EPSG::9119", "hemisphere degree minute", "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (integer) - arc-minutes (real, any precision). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.", new[] { "HDM", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Hemisphere degree minute second.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegreeMinuteSecond = new UnitOfMeasurement("EPSG::9120", "hemisphere degree minute second", "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (integer) - arc-minutes (integer) - arc-seconds (real). Different symbol sets are in use as field separators for example º ' \". Convert to deg using algorithm.", new[] { "HDMS", "dega" }, "°'|\"", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Indian foot.
        /// </summary>
        public static UnitOfMeasurement IndianFoot = new UnitOfMeasurement("EPSG::9080", "Indian foot", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (= 3 British feet) taken to be J.S.Clark's 1865 value of 0.9144025 metres.", new[] { "Indian geodetic foot", "ftInd" }, "ft", 0.30479951024814694, UnitQuantityType.Length);

        /// <summary>
        /// Indian foot (1937).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1937 = new UnitOfMeasurement("EPSG::9081", "Indian foot (1937)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British foot taken to be 1895 Benoit value of 12/39.370113m. Rounded to 8 decimal places as 0.30479841. Used from Bangladesh to Vietnam. Previously used in India and Pakistan but superseded.", new[] { "Indian geodetic foot", "ftInd(37)" }, "ft", 0.30479841, UnitQuantityType.Length);

        /// <summary>
        /// Indian foot (1962).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1962 = new UnitOfMeasurement("EPSG::9082", "Indian foot (1962)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures with a small error as 1 Ind ft=0.3047996m. Used in Pakistan since metrication.", new[] { "ftInd(62)" }, "ft", 0.3047996, UnitQuantityType.Length);

        /// <summary>
        /// Indian foot (1975).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1975 = new UnitOfMeasurement("EPSG::9083", "Indian foot (1975)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures as 1 Ind ft=0.3047995m. Used in India since metrication.", new[] { "ftInd(75)" }, "ft", 0.3047995, UnitQuantityType.Length);

        /// <summary>
        /// Indian yard.
        /// </summary>
        public static UnitOfMeasurement IndianYard = new UnitOfMeasurement("EPSG::9084", "Indian yard", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (= 3 British feet) taken to be J.S.Clark's 1865 value of 0.9144025 metres.", new[] { "yard", "ydInd" }, "yd", 0.9143985307444408, UnitQuantityType.Length);

        /// <summary>
        /// Indian yard (1937).
        /// </summary>
        public static UnitOfMeasurement IndianYard1937 = new UnitOfMeasurement("EPSG::9085", "Indian yard (1937)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be 1895 Benoit value of 12/39.370113m. Rounded to 8 decimal places as 0.30479841. Used from Bangladesh to Vietnam. Previously used in India and Pakistan but superseded.", new[] { "yard", "ydInd(37)" }, "yd", 0.9143952, UnitQuantityType.Length);

        /// <summary>
        /// Indian yard (1962).
        /// </summary>
        public static UnitOfMeasurement IndianYard1962 = new UnitOfMeasurement("EPSG::9086", "Indian yard (1962)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures with a small error as 1 Ind ft=0.3047996m. Used in Pakistan since metrication.", new[] { "ydInd(62)" }, "yd", 0.9143988, UnitQuantityType.Length);

        /// <summary>
        /// Indian yard (1975).
        /// </summary>
        public static UnitOfMeasurement IndianYard1975 = new UnitOfMeasurement("EPSG::9087", "Indian yard (1975)", "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures as 1 Ind ft=0.3047995m. Used in India since metrication.", new[] { "ydInd(75)" }, "yd", 0.9143985, UnitQuantityType.Length);

        /// <summary>
        /// Kilometre.
        /// </summary>
        public static UnitOfMeasurement Kilometre = new UnitOfMeasurement("EPSG::9036", "kilometre", null, new[] { "kilometer", "km" }, "km", 1000, UnitQuantityType.Length);

        /// <summary>
        /// Link.
        /// </summary>
        public static UnitOfMeasurement Link = new UnitOfMeasurement("EPSG::9098", "link", "=1/100 international chain", new[] { "international link", "lk" }, "lk", 0.201168, UnitQuantityType.Length);

        /// <summary>
        /// Metre.
        /// </summary>
        public static UnitOfMeasurement Metre = new UnitOfMeasurement("EPSG::9001", "metre", null, new[] { "meter", "International metre", "m" }, "m", 1, UnitQuantityType.Length);

        /// <summary>
        /// Metres per second.
        /// </summary>
        public static UnitOfMeasurement MetresPerSecond = new UnitOfMeasurement("EPSG::1026", "metres per second", "Length rate.", new[] { "meters per second", "m/s" }, "m/s", 3.1688765172731486E-8, UnitQuantityType.Length);

        /// <summary>
        /// Metres per year.
        /// </summary>
        public static UnitOfMeasurement MetresPerYear = new UnitOfMeasurement("EPSG::1042", "metres per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new[] { "meters per year", "m/yr", "m/a" }, "m/a", 3.1688765172731486E-8, UnitQuantityType.Length);

        /// <summary>
        /// Microradian.
        /// </summary>
        public static UnitOfMeasurement MicroRadian = new UnitOfMeasurement("EPSG::9109", "microradian", "rad * 10E-6", new[] { "µrad", "urad" }, "µrad", 0.000001, UnitQuantityType.Angle);

        /// <summary>
        /// Mil_6400.
        /// </summary>
        public static UnitOfMeasurement Mil6400 = new UnitOfMeasurement("EPSG::9114", "mil_6400", "Angle subtended by 1/6400 part of a circle. Approximates to 1/1000th radian. Note that other approximations (notably 1/6300 circle and 1/6000 circle) also exist.", new[] { "mil", "mila" }, "mil", 0.0009817477042468094, UnitQuantityType.Angle);

        /// <summary>
        /// Milliarc-second.
        /// </summary>
        public static UnitOfMeasurement MilliarcSecond = new UnitOfMeasurement("EPSG::1031", "milliarc-second", "= ((pi/180) / 3600 / 1000) radians", new[] { "mas", "msec" }, "msec", 4.848136811095355E-9, UnitQuantityType.Angle);

        /// <summary>
        /// Milliarc-second per year.
        /// </summary>
        public static UnitOfMeasurement MilliarcSecondPerYear = new UnitOfMeasurement("EPSG::1032 ", "milliarc-second per year", "= ((pi/180) / 3600 / 1000) radians per year. Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new[] { "mas/yr", "msec/yr", "msec/a" }, "msec/a", 1.5363146893207598E-16, UnitQuantityType.Angle);

        /// <summary>
        /// Millimetre.
        /// </summary>
        public static UnitOfMeasurement Millimetre = new UnitOfMeasurement("EPSG::1025", "millimetre", null, new[] { "millimeter", "mm" }, "mm", 0.001, UnitQuantityType.Length);

        /// <summary>
        /// Millimetre per year.
        /// </summary>
        public static UnitOfMeasurement MillimetrePerYear = new UnitOfMeasurement("EPSG::1027", "millimetre per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new[] { "millimeters per year", "mm/yr", "mm/a" }, "mm/a", 3.168876517273149E-11, UnitQuantityType.Length);

        /// <summary>
        /// Mautical mile.
        /// </summary>
        public static UnitOfMeasurement NauticalMile = new UnitOfMeasurement("EPSG::9030", "nautical mile", null, new[] { "NM", "nautmi" }, "NM", 1852, UnitQuantityType.Length);

        /// <summary>
        /// Parts per billion.
        /// </summary>
        public static UnitOfMeasurement PartsPerBillion = new UnitOfMeasurement("EPSG::1028", "parts per billion", "Billion is internationally ambiguous, in different languages being 1E+9 and 1E+12. One billion taken here to be 1E+9.", new[] { "ppb", "nm/m" }, "ppb", 1.0E-9, UnitQuantityType.Scale);

        /// <summary>
        /// Parts per billion per year.
        /// </summary>
        public static UnitOfMeasurement PartsPerBillionPerYear = new UnitOfMeasurement("EPSG::1030", "parts per billion per year", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029. Billion is internationally ambiguous, in different languages being 1E+9 and 1E+12. One billion taken here to be 1E+9.", new[] { "ppb/yr", "ppb/a", "nm/m/a" }, "ppb/a", 3.1688765172731483E-17, UnitQuantityType.Scale);

        /// <summary>
        /// Parts per million.
        /// </summary>
        public static UnitOfMeasurement PartsPerMillion = new UnitOfMeasurement("EPSG::9202", "parts per million", "Year taken to be IUGS definition of 31556925.445 seconds; see UoM code 1029.", new[] { "ppm", "µm/m" }, "ppm", 0.000001, UnitQuantityType.Scale);

        /// <summary>
        /// Parts per million per year.
        /// </summary>
        public static UnitOfMeasurement PartsPerMillionPerYear = new UnitOfMeasurement("EPSG::1041", "parts per million per year", null, new[] { "ppm/yr", "ppm/a", "µm/m/a" }, "ppm/a", 3.1688765172731486E-14, UnitQuantityType.Scale);

        /// <summary>
        /// Radian.
        /// </summary>
        public static UnitOfMeasurement Radian = new UnitOfMeasurement("EPSG::9101", "radian", null, new[] { "rad" }, "rad", 1, UnitQuantityType.Angle);

        /// <summary>
        /// Radians per second.
        /// </summary>
        public static UnitOfMeasurement RadiansPerSecond = new UnitOfMeasurement("EPSG::1035", "radians per second", "Angle rate.", new[] { "rad/s" }, "rad/s", 1, UnitQuantityType.Angle);

        /// <summary>
        /// Second.
        /// </summary>
        public static UnitOfMeasurement Second = new UnitOfMeasurement("EPSG::1040", "second", "Not to be confused with the angle unit arc-second.", null, "s", 1, UnitQuantityType.Time);

        /// <summary>
        /// Sexagesimal DM.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDM = new UnitOfMeasurement("EPSG::9111", "sexagesimal DM", "Pseudo unit. Format: sign - degrees - decimal point - integer minutes (two digits) - fraction of minutes (any precision). Must include leading zero in integer minutes. Must exclude decimal point for minutes. Convert to deg using algorithm.", new[] { "DDD.MMm", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Sexagesimal DMS.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDMS = new UnitOfMeasurement("EPSG::9110", "sexagesimal DMS", "Pseudo unit. Format: signed degrees - period - minutes (2 digits) - integer seconds (2 digits) - fraction of seconds (any precision). Must include leading zero in minutes and seconds and exclude decimal point for seconds. Convert to degree using formula.", new[] { "DDD.MMSSsss", "dega", "sexagesimal degree" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Sexagesimal DMS.s.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDMSs = new UnitOfMeasurement("EPSG::9121", "sexagesimal DMs.s", "Pseudo unit. Format: signed degrees - minutes (two digits) - seconds (real, any precision). Must include leading zero in minutes and seconds where value is under 10 and include decimal separator for seconds. Convert to degree using algorithm.", new[] { "DDDMMSS.sss", "dega" }, "°", Math.PI / 180, UnitQuantityType.Angle);

        /// <summary>
        /// Statute mile.
        /// </summary>
        public static UnitOfMeasurement StatuteMile = new UnitOfMeasurement("EPSG::9093", "Statute mile", "=5280 feet", new[] { "mi" }, "M", 1609.344, UnitQuantityType.Length);

        /// <summary>
        /// Unity.
        /// </summary>
        public static UnitOfMeasurement Unity = new UnitOfMeasurement("EPSG::9201", "unity", null, 1, UnitQuantityType.Scale);

        /// <summary>
        /// Unity per second.
        /// </summary>
        public static UnitOfMeasurement UnityPerSecond = new UnitOfMeasurement("EPSG::1036", "unity per second", "Scale rate.", null, null, 1, UnitQuantityType.Scale);

        /// <summary>
        /// US survey chain.
        /// </summary>
        public static UnitOfMeasurement USSurveyChain = new UnitOfMeasurement("EPSG::9033", "US survey chain", "Used in USA primarily for public lands cadastral work.", new[] { "chUS" }, "ch", 20.11684023368047, UnitQuantityType.Length);

        /// <summary>
        /// US survey foot.
        /// </summary>
        public static UnitOfMeasurement USSurveyFoot = new UnitOfMeasurement("EPSG::9003", "US survey foot", "Used in USA.", new[] { "American foot", "ftUS" }, "ft", 0.30480060960121924, UnitQuantityType.Length);

        /// <summary>
        /// US survey link.
        /// </summary>
        public static UnitOfMeasurement USSurveyLink = new UnitOfMeasurement("EPSG::9034", "US survey link", "Used in USA primarily for public lands cadastral work.", new[] { "lkUS" }, "lk", 0.2011684023368047, UnitQuantityType.Length);

        /// <summary>
        /// US survey mile.
        /// </summary>
        public static UnitOfMeasurement USSurveyMile = new UnitOfMeasurement("EPSG::9035", "US survey mile", "Used in USA primarily for public lands cadastral work.", new[] { "miUS" }, "M", 1609.3472186944375, UnitQuantityType.Length);

        /// <summary>
        /// Yard.
        /// </summary>
        public static UnitOfMeasurement Yard = new UnitOfMeasurement("EPSG::9096", "yard", "=3 international feet.", new[] { "international yard", "yd" }, "yd", 0.9144, UnitQuantityType.Length);

        /// <summary>
        /// Year.
        /// </summary>
        public static UnitOfMeasurement Year = new UnitOfMeasurement("EPSG::1029", "year", null, 3.1556925445E7, UnitQuantityType.Time);
    }
}
