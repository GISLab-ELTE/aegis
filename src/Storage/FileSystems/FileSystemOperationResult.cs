// <copyright file="FileSystemOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Organization.FileSystems
{
    /// <summary>
    /// Represent the result of a file system operation.
    /// </summary>
    public class FileSystemOperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemOperationResult" /> class.
        /// </summary>
        public FileSystemOperationResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemOperationResult" /> class.
        /// </summary>
        /// <param name="code">The result code.</param>
        public FileSystemOperationResult(FileSystemOperationResultCode code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Gets or sets the result code of the operation.
        /// </summary>
        /// <value>The result code of the operation.</value>
        public FileSystemOperationResultCode Code { get; set; }
    }
}
