// <copyright file="HalfedgeGraph.IdentifierProvider.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Topology
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    public partial class HalfedgeGraph
    {
        /// <summary>
        /// Represents a fixed, <c>null</c> resulting geometry identifier provider.
        /// </summary>
        /// <seealso cref="AEGIS.Topology.IIdentifierProvider" />
        /// <author>Máté Cserép</author>
        public class NullIdentifierProvider : IIdentifierProvider
        {
            #region Implementation of IIdentifierProvider

            /// <summary>
            /// Retrieves the identifier from an <see cref="IGeometry"/>.
            /// </summary>
            /// <param name="geometry">The geometry.</param>
            /// <returns>The identifier.</returns>
            public ISet<int> GetIdentifiers(IGeometry geometry)
            {
                return null;
            }

            #endregion
        }

        /// <summary>
        /// Represents a fixed resulting geometry identifier provider.
        /// </summary>
        /// <seealso cref="AEGIS.Topology.IIdentifierProvider" />
        /// <author>Máté Cserép</author>
        public class FixedIdentifierProvider : IIdentifierProvider
        {
            #region Private fields

            /// <summary>
            /// The fixed identifier to return.
            /// </summary>
            private readonly ISet<Int32> _identifier;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="FixedIdentifierProvider"/> class.
            /// </summary>
            /// <param name="identifier">The identifier.</param>
            public FixedIdentifierProvider(Int32 identifier)
            {
                _identifier = new HashSet<Int32>() { identifier };
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FixedIdentifierProvider"/> class.
            /// </summary>
            /// <param name="identifiers">The identifiers.</param>
            public FixedIdentifierProvider(ISet<Int32> identifiers)
            {
                _identifier = identifiers;
            }

            #endregion

            #region Implementation of IIdentifierProvider

            /// <summary>
            /// Retrieves the identifier from an <see cref="IGeometry"/>.
            /// </summary>
            /// <param name="geometry">The geometry.</param>
            /// <returns>The identifier.</returns>
            public ISet<Int32> GetIdentifiers(IGeometry geometry)
            {
                return _identifier;
            }

            #endregion
        }
    }
}
