// <copyright file="HadoopFileListingOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.FileSystems.Operations
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Hadoop file system operation result containing a file status list.
    /// </summary>
    public class HadoopFileListingOperationResult : HadoopFileSystemOperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperationResult" /> class.
        /// </summary>
        public HadoopFileListingOperationResult()
        {
            this.StatusList = new HadoopFileStatusOperationResult[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperationResult" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="statusList">The list of file status results.</param>
        public HadoopFileListingOperationResult(String request, IList<HadoopFileStatusOperationResult> statusList)
            : base(request)
        {
            if (statusList == null)
                this.StatusList = new HadoopFileStatusOperationResult[0];
            else
                this.StatusList = statusList;
        }

        /// <summary>
        /// Gets or sets the status list.
        /// </summary>
        /// <value>The status list.</value>
        public IList<HadoopFileStatusOperationResult> StatusList { get; set; }
    }
}
