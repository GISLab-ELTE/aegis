// <copyright file="LocalCoordinateSystemAxisCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="CoordinateSystemAxis" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalCoordinateSystemAxisCollection : ICoordinateSystemAxisCollection
    {
        /// <summary>
        /// The name of the resource. This field is constant.
        /// </summary>
        private const String ResourceName = "CoordinateSystemAxis";

        /// <summary>
        /// Represents raw data of coordinate system axes.
        /// </summary>
        private class CoordinateSystemAxisData : IdentifiedObject
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CoordinateSystemAxisData" /> class.
            /// </summary>
            /// <param name="identifier">The identifier.</param>
            /// <param name="name">The name.</param>
            /// <param name="remarks">The remarks.</param>
            /// <param name="aliases">The aliases.</param>
            /// <param name="description">The description.</param>
            public CoordinateSystemAxisData(String identifier, String name, String remarks, String[] aliases, String description)
                : base(identifier, name, remarks, aliases)
            {
                this.Description = description;
            }

            /// <summary>
            /// Gets the description of the axis.
            /// </summary>
            /// <value>The description of the axis.</value>
            public String Description { get; private set; }
        }

        /// <summary>
        /// Represents a collection of <see cref="CoordinateSystemAxisData" /> instances.
        /// </summary>
        private class CoordinateSystemAxisDataCollection : LocalReferenceCollection<CoordinateSystemAxisData>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CoordinateSystemAxisDataCollection" /> class.
            /// </summary>
            public CoordinateSystemAxisDataCollection()
                : base(ResourceName, ResourceName)
            {
            }

            /// <summary>
            /// Converts the specified content.
            /// </summary>
            /// <param name="content">The content.</param>
            /// <returns>The converted reference.</returns>
            protected override CoordinateSystemAxisData Convert(String[] content)
            {
                return new CoordinateSystemAxisData(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                    content[2], this.GetAliases(Int32.Parse(content[0])),
                                                    content[3]);
            }
        }

        /// <summary>
        /// The collection of  raw data.
        /// </summary>
        private CoordinateSystemAxisDataCollection dataCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCoordinateSystemAxisCollection" /> class.
        /// </summary>
        public LocalCoordinateSystemAxisCollection()
        {
            this.dataCollection = new CoordinateSystemAxisDataCollection();
        }

        /// <summary>
        /// Gets the instance with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The instance with the specified authority and code.</returns>
        public CoordinateSystemAxis this[String authority, Int32 code]
        {
            get
            {
                return this[authority, code, AxisDirection.Undefined, UnitsOfMeasurement.Unity];
            }
        }

        /// <summary>
        /// Gets the instance with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The instance with the specified authority and code.</returns>
        public CoordinateSystemAxis this[String authority, Int32 code, AxisDirection direction, UnitOfMeasurement unit]
        {
            get
            {
                CoordinateSystemAxisData data = this.dataCollection[authority, code];

                if (data == null)
                    return null;

                return this.Convert(data, direction, unit);
            }
        }

        /// <summary>
        /// Gets the instance with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The instance with the specified identifier.</returns>
        public CoordinateSystemAxis this[String identifier]
        {
            get { return this[identifier, AxisDirection.Undefined, UnitsOfMeasurement.Unity]; }
        }

        /// <summary>
        /// Gets the instance with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The instance with the specified identifier.</returns>
        public CoordinateSystemAxis this[String identifier, AxisDirection direction, UnitOfMeasurement unit]
        {
            get
            {
                if (unit == null)
                    unit = UnitsOfMeasurement.Unity;

                CoordinateSystemAxisData data = this.dataCollection[identifier];

                if (data == null)
                    return null;

                return this.Convert(data, direction, unit);
            }
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateSystemAxis> WithIdentifier(String identifier)
        {
            return this.WithIdentifier(identifier, AxisDirection.Undefined, UnitsOfMeasurement.Unity);
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified identifier with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        public IEnumerable<CoordinateSystemAxis> WithIdentifier(String identifier, AxisDirection direction, UnitOfMeasurement unit)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitIsNull);

            return this.dataCollection.WithIdentifier(identifier).Select(data => this.Convert(data, direction, unit));
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateSystemAxis> WithName(String name)
        {
            return this.WithName(name, AxisDirection.Undefined, UnitsOfMeasurement.Unity);
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified name with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        public IEnumerable<CoordinateSystemAxis> WithName(String name, AxisDirection direction, UnitOfMeasurement unit)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitIsNull);

            return this.dataCollection.WithName(name).Select(data => this.Convert(data, direction, unit));
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateSystemAxis> WithMatchingIdentifier(String identifier)
        {
            return this.WithMatchingIdentifier(identifier, AxisDirection.Undefined, UnitsOfMeasurement.Unity);
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified identifier expression with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        public IEnumerable<CoordinateSystemAxis> WithMatchingIdentifier(String identifier, AxisDirection direction, UnitOfMeasurement unit)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitIsNull);

            return this.dataCollection.WithMatchingIdentifier(identifier).Select(data => this.Convert(data, direction, unit));
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateSystemAxis> WithMatchingName(String name)
        {
            return this.WithMatchingName(name, AxisDirection.Undefined, UnitsOfMeasurement.Unity);
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A collection containing the items that match the specified name expression with the specified direction and unit.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        public IEnumerable<CoordinateSystemAxis> WithMatchingName(String name, AxisDirection direction, UnitOfMeasurement unit)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitIsNull);

            return this.dataCollection.WithMatchingName(name).Select(data => this.Convert(data, direction, unit));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{CoordinateSystemAxis}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<CoordinateSystemAxis> GetEnumerator()
        {
            foreach (CoordinateSystemAxisData data in this.dataCollection)
                yield return this.Convert(data, AxisDirection.Undefined, UnitsOfMeasurement.Unity);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (CoordinateSystemAxisData data in this.dataCollection)
                yield return this.Convert(data, AxisDirection.Undefined, UnitsOfMeasurement.Unity);
        }

        /// <summary>
        /// Converts the coordinate system axis data to an axis.
        /// </summary>
        /// <param name="data">The axis data.</param>
        /// <param name="direction">The axis direction.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The converted coordinate system axis.</returns>
        private CoordinateSystemAxis Convert(CoordinateSystemAxisData data, AxisDirection direction, UnitOfMeasurement unit)
        {
            return new CoordinateSystemAxis(data.Identifier, data.Name, data.Remarks, data.Aliases.ToArray(), data.Description, direction, unit);
        }
    }
}
