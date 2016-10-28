// <copyright file="LocalAreaOfUseCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a collection of <see cref="AreaOfUse" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG gedetic dataset format.
    /// </remarks>
    public class LocalAreaOfUseCollection : LocalReferenceCollection<AreaOfUse>
    {
        #region Private constants

        /// <summary>
        /// The name of the resource. This field is constant.
        /// </summary>
        private const String ResourceName = "Area";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAreaOfUseCollection" /> class.
        /// </summary>
        public LocalAreaOfUseCollection()
            : base(ResourceName, ResourceName)
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected override AreaOfUse Convert(String[] content)
        {
            Double south = Double.NaN, west = Double.NaN, north = Double.NaN, east = Double.NaN;
            Double.TryParse(content[3], out south);
            Double.TryParse(content[4], out west);
            Double.TryParse(content[5], out north);
            Double.TryParse(content[6], out east);

            return AreaOfUse.FromDegrees(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                         content[2], null, this.GetAliases(Int32.Parse(content[0])),
                                         south, west, north, east);
        }

        #endregion
    }
}
