// <copyright file="LocalCoordinateSystemCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="CoordinateSystem" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalCoordinateSystemCollection : LocalReferenceCollection<CoordinateSystem>
    {
        /// <summary>
        /// The resource path to the axis mappings. This field is constant.
        /// </summary>
        private const String ResourcePathMapping = "AEGIS.Reference.Resources.CoordinateSystemAxisMapping.txt";

        /// <summary>
        /// The collection of <see cref="CoordinateSystemAxis" /> instances.
        /// </summary>
        private readonly ICoordinateSystemAxisCollection axisCollection;

        /// <summary>
        /// The collection of <see cref="UnitOfMeasurement" /> instances.
        /// </summary>
        private readonly IReferenceCollection<UnitOfMeasurement> unitCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCoordinateSystemCollection" /> class.
        /// </summary>
        /// <param name="axisCollection">The axis collection.</param>
        /// <param name="unitCollection">The unit of measurement collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The coordinate system axis collection is null.
        /// or
        /// The unit of measurement collection is null.
        /// </exception>
        public LocalCoordinateSystemCollection(ICoordinateSystemAxisCollection axisCollection, IReferenceCollection<UnitOfMeasurement> unitCollection)
        {
            this.axisCollection = axisCollection ?? throw new ArgumentNullException(nameof(axisCollection));
            this.unitCollection = unitCollection ?? throw new ArgumentNullException(nameof(unitCollection));
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override CoordinateSystem Convert(String[] content)
        {
            CoordinateSystemType type = (CoordinateSystemType)Enum.Parse(typeof(CoordinateSystemType), content[2], true);

            return new CoordinateSystem(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                        content[4], this.GetAliases(Int32.Parse(content[0])),
                                        type, this.GetAxes(content[0]));
        }

        /// <summary>
        /// Returns the axes for the specified coordinate system.
        /// </summary>
        /// <param name="identifier">The identifier of the coordinate system.</param>
        /// <returns>The array of coordinate system axes.</returns>
        private CoordinateSystemAxis[] GetAxes(String identifier)
        {
            List<CoordinateSystemAxis> axes = new List<CoordinateSystemAxis>();

            foreach (String[] axisMapping in this.GetAxisMapping(identifier))
            {
                if (!Enum.TryParse<AxisDirection>(axisMapping[3], true, out AxisDirection direction) &&
                    !Enum.TryParse<AxisDirection>(axisMapping[3].Replace("-", String.Empty), true, out direction) &&
                    !Enum.TryParse<AxisDirection>(axisMapping[3].Substring(0, axisMapping[3].IndexOf(' ')), true, out direction))
                {
                    direction = AxisDirection.Undefined;
                }

                axes.Add(this.axisCollection[axisMapping[2], direction, this.unitCollection[Authority, Int32.Parse(axisMapping[5])]]);
            }

            return axes.ToArray();
        }

        /// <summary>
        /// Returns the axis mapping for the specified coordinate system.
        /// </summary>
        /// <param name="identifier">The identifier of the coordinate system.</param>
        /// <returns>The collection of  axis mappings.</returns>
        private IEnumerable<String[]> GetAxisMapping(String identifier)
        {
            using (StreamReader reader = new StreamReader(this.GetType().GetTypeInfo().Assembly.GetManifestResourceStream(ResourcePathMapping)))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] content = line.Split(Divider);

                    if (content[1] == identifier)
                        yield return content;
                }
            }
        }
    }
}
