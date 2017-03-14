// <copyright file="Feature.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Features
{
    using System;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a geographic feature.
    /// </summary>
    public class Feature : IFeature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Feature" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="attributes">The attributes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The geometry is null.
        /// or
        /// The attribute collection is null.
        /// </exception>
        public Feature(String identifier, IGeometry geometry, IAttributeCollection attributes)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry), CoreMessages.GeometryIsNull);
            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes), CoreMessages.AttributeCollectionIsNull);

            this.Factory = new FeatureFactory(geometry.Factory, attributes.Factory);
            this.Identifier = identifier;
            this.Geometry = geometry;
            this.Attributes = attributes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature" /> class.
        /// </summary>
        /// <param name="factory">The factory is null.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="geometry">The geometry.</param>
        /// <param name="attributes">The attributes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// or
        /// The geometry is null.
        /// </exception>
        public Feature(FeatureFactory factory, String identifier, IGeometry geometry, IAttributeCollection attributes)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryIsNull);
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), CoreMessages.IdentifierIsNull);

            this.Factory = factory;
            this.Identifier = identifier;
            this.Geometry = (geometry == null) ? null : geometry.Factory.Equals(this.Factory.GeometryFactory) ? geometry : this.Factory.GeometryFactory.CreateGeometry(geometry);
            this.Attributes = (attributes == null) ? null : attributes.Factory.Equals(this.Factory.AttributeCollectionFactory) ? attributes : this.Factory.AttributeCollectionFactory.CreateCollection(attributes);
        }

        /// <summary>
        /// Gets the attribute collection of the feature.
        /// </summary>
        /// <value>The collection of attribute.</value>
        public IAttributeCollection Attributes { get; private set; }

        /// <summary>
        /// Gets the factory of the feature.
        /// </summary>
        /// <value>The factory implementation the feature was constructed by.</value>
        public IFeatureFactory Factory { get; private set; }

        /// <summary>
        /// Gets the geometry of the feature.
        /// </summary>
        /// <value>The geometry of the feature.</value>
        public IGeometry Geometry { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the feature.
        /// </summary>
        /// <value>The identifier of the feature.</value>
        public String Identifier { get; private set; }
    }
}