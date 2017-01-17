// <copyright file="WellKnownTextConverterTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.Reference.Converters
{
    using System;
    using System.Linq;
    using AEGIS.Reference.Converters;
    using AEGIS.Tests.Reference;
    using ELTE.AEGIS.Geometries;
    using ELTE.AEGIS.Reference;
    using NUnit.Framework;

    /// <summary>
    /// Test fixture for the <see cref="WellKnownTextConverter" /> class.
    /// </summary>
    [TestFixture]
    public class WellKnownTextConverterTest
    {
        /// <summary>
        /// The array of identified objects.
        /// </summary>
        private IdentifiedObject[] identifiedObjects;

        /// <summary>
        /// The array of identified objects in WKT format.
        /// </summary>
        private String[] identifiedObjectsText;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.identifiedObjects = new IdentifiedObject[]
            {
                TestUtilities.ReferenceProvider.Meridians["EPSG::8901"],
                TestUtilities.ReferenceProvider.Meridians["EPSG::8901"],
                TestUtilities.ReferenceProvider.Ellipsoids["EPSG::7030"],
                TestUtilities.ReferenceProvider.Ellipsoids["EPSG::7030"],
                TestUtilities.ReferenceProvider.Datums["EPSG::6326"],
                TestUtilities.ReferenceProvider.Datums["EPSG::6326"],
                TestUtilities.ReferenceProvider.GeographicCoordinateReferenceSystems["EPSG::4326"],
                TestUtilities.ReferenceProvider.GeographicCoordinateReferenceSystems["EPSG::4326"],
                TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::32633"],
                TestUtilities.ReferenceProvider.ProjectedCoordinateReferenceSystems["EPSG::32633"],
                new GeographicCoordinateReferenceSystem(IdentifiedObject.UserDefinedIdentifier, "Unknown", CoordinateSystem.Undefined, new GeodeticDatum(IdentifiedObject.UserDefinedIdentifier, "Unknown", null, null, AreaOfUse.Undefined, TestUtilities.ReferenceProvider.Ellipsoids["EPSG::7008"], TestUtilities.ReferenceProvider.Meridians["EPSG::8901"]), AreaOfUse.Undefined)
            };

            this.identifiedObjectsText = new String[]
            {
                "PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\",8901]]",
                "PRIMEM[\"Greenwich\", 0]",
                "SPHEROID[\"WGS 84\",6378137,298.257223560493,AUTHORITY[\"EPSG\",7030]]",
                "SPHEROID[\"WGS 84\",6378137,298.257223560493]",
                "DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137,298.257223560493],AUTHORITY[\"EPSG\",6326]]",
                "DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137,298.257223560493]]",
                "GEOGCS[\"GCS_WGS_84\",DATUM[\"World_Geodetic_System_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295],AUTHORITY[\"EPSG\",4326]]",
                "GEOGCS[\"GCS_WGS_84\",DATUM[\"World_Geodetic_System_1984\",SPHEROID[\"WGS_84\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]]",
                "PROJCS[\"UTM Zone 33N\",GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137,298.257223560493]],PRIMEM[\"Greenwich\",0],UNIT[\"degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",15],PARAMETER[\"scale_factor_at_natural_origin\",0.9996],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1],AUTHORITY[\"EPSG\",32633]]",
                "PROJCS[\"UTM Zone 33N\",GEOGCS[\"WGS 84\",DATUM[\"World Geodetic System 1984\",SPHEROID[\"WGS 84\",6378137,298.257223560493]],PRIMEM[\"Greenwich\",0],UNIT[\"degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",15],PARAMETER[\"scale_factor_at_natural_origin\",0.9996],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]",
                "GEOGCS[\"Unknown\",DATUM[\"D_Unknown\",SPHEROID[\"Clarke_1866\",6378206.4,294.978698213901]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]"
            };
        }

        /// <summary>
        /// Tests the <see cref="ToWellKnownText" /> method.
        /// </summary>
        [Test]
        public void WellKnownTextConverterToWellKnownTextTest()
        {
            for (Int32 index = 0; index < this.identifiedObjects.Length - 1; index++)
            {
                String text = WellKnownTextConverter.ToWellKnownText(this.identifiedObjects[index]);
                IdentifiedObject identifiedObject = WellKnownTextConverter.ToIdentifiedObject(text, TestUtilities.ReferenceProvider);

                Assert.AreEqual(this.identifiedObjects[index], identifiedObject);
            }

            Assert.Throws<ArgumentNullException>(() => WellKnownTextConverter.ToWellKnownText((IdentifiedObject)null));
            Assert.Throws<ArgumentNullException>(() => WellKnownTextConverter.ToWellKnownText((IReferenceSystem)null));
        }

        /// <summary>
        /// Tests the <see cref="ToIdentifiedObject" /> method.
        /// </summary>
        [Test]
        public void WellKnownTextConverterToIdentifiedObjectTest()
        {
            for (Int32 index = 0; index < this.identifiedObjects.Length; index++)
            {
                IdentifiedObject identifiedObject = WellKnownTextConverter.ToIdentifiedObject(this.identifiedObjectsText[index], TestUtilities.ReferenceProvider);
                Assert.AreEqual(this.identifiedObjects[index], identifiedObject);
            }

            Meridian meridian = WellKnownTextConverter.ToIdentifiedObject("PRIMEM[\"Greenwich\",0.0]", TestUtilities.ReferenceProvider) as Meridian;
            Assert.AreEqual(meridian, TestUtilities.ReferenceProvider.Meridians["EPSG", 8901]);

            meridian = WellKnownTextConverter.ToIdentifiedObject("PRIMEM(\"Greenwich\",0.0)", TestUtilities.ReferenceProvider) as Meridian;
            Assert.AreEqual(meridian, TestUtilities.ReferenceProvider.Meridians["EPSG", 8901]);

            meridian = WellKnownTextConverter.ToIdentifiedObject("PRIMEM(Greenwich,0.0)", TestUtilities.ReferenceProvider) as Meridian;
            Assert.AreEqual(meridian, TestUtilities.ReferenceProvider.Meridians["EPSG", 8901]);

            meridian = WellKnownTextConverter.ToIdentifiedObject("PRIMEM[\"Greenwich\", 0.0, AUTHORITY[\"EPSG\", 8901]]", TestUtilities.ReferenceProvider) as Meridian;
            Assert.AreEqual(meridian, TestUtilities.ReferenceProvider.Meridians["EPSG", 8901]);

            meridian = WellKnownTextConverter.ToIdentifiedObject("PRIMEM [\"Greenwich\", 0.0, AUTHORITY [\"EPSG\", 8901]])", TestUtilities.ReferenceProvider) as Meridian;
            Assert.AreEqual(meridian, TestUtilities.ReferenceProvider.Meridians["EPSG", 8901]);

            meridian = WellKnownTextConverter.ToIdentifiedObject("PRIMEM[Greenwich, 0.0, AUTHORITY[EPSG, 8901]]", TestUtilities.ReferenceProvider) as Meridian;
            Assert.AreEqual(meridian, TestUtilities.ReferenceProvider.Meridians["EPSG", 8901]);

            Assert.Throws<ArgumentNullException>(() => WellKnownTextConverter.ToIdentifiedObject(null, null));
            Assert.Throws<ArgumentException>(() => WellKnownTextConverter.ToIdentifiedObject(String.Empty, TestUtilities.ReferenceProvider));
            Assert.Throws<ArgumentException>(() => WellKnownTextConverter.ToIdentifiedObject("UNDEFINED[\"something\"]", TestUtilities.ReferenceProvider));
        }
    }
}
