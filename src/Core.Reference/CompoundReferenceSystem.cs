// <copyright file="CompoundReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a compound reference system.
    /// </summary>
    public class CompoundReferenceSystem : ReferenceSystem
    {
        /// <summary>
        /// The list of components.
        /// </summary>
        private readonly ReferenceSystem[] components;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="components">The components of the reference system.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The are of use is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">No components are specified.</exception>
        public CompoundReferenceSystem(String identifier, String name, AreaOfUse areaOfUse, params ReferenceSystem[] components)
            : this(identifier, name, null, null, null, areaOfUse, components)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope of the reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="components">The components of the reference system.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The are of use is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">No components are specified.</exception>
        public CompoundReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, AreaOfUse areaOfUse, params ReferenceSystem[] components)
            : base(identifier, name, remarks, aliases, scope)
        {
            if (components == null || components.Length == 0)
                throw new ArgumentException(ReferenceMessages.NoComponentsAreSpecified, nameof(components));
            if (areaOfUse == null)
                throw new ArgumentNullException(nameof(areaOfUse), ReferenceMessages.AreaOfUseIsNull);

            this.components = components;
            this.AreaOfUse = areaOfUse;
        }

        /// <summary>
        /// Gets the number of dimensions.
        /// </summary>
        /// <value>The number of dimensions.</value>
        public override Int32 Dimension { get { return this.components.Sum(referenceSystem => referenceSystem.Dimension); } }

        /// <summary>
        /// Gets the type of the reference system.
        /// </summary>
        /// <value>The type of the reference system.</value>
        public override ReferenceSystemType Type { get { return ReferenceSystemType.Compound; } }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the reference system is applicable.</value>
        public AreaOfUse AreaOfUse { get; private set; }

        /// <summary>
        /// Gets the components of the reference system.
        /// </summary>
        /// <value>The read-only list of components of the compound reference system.</value>
        public IReadOnlyList<ReferenceSystem> Components { get { return this.components; } }
    }
}
