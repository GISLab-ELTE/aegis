// <copyright file="HadoopFileSystemOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a Hadoop file system operation result.
    /// </summary>
    public abstract class HadoopFileSystemOperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperationResult" /> class.
        /// </summary>
        protected HadoopFileSystemOperationResult() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperationResult" /> class.
        /// </summary>
        /// <param name="request">The request of the operation.</param>
        protected HadoopFileSystemOperationResult(String request)
        {
            this.Request = request;
        }

        /// <summary>
        /// Gets or sets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        public String Request { get; set; }
    }
}
