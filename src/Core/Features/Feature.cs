// <copyright file="Feature.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Features
{
    using System;
    using AEGIS.Attributes;
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
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            this.Geometry = geometry;
            this.Attributes = attributes ?? new AttributeCollection();
        }

        /// <summary>
        /// Gets the attribute collection of the feature.
        /// </summary>
        /// <value>The collection of attribute.</value>
        public IAttributeCollection Attributes { get; private set; }

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