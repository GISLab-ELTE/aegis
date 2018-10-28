// <copyright file="FileSystemOperationResultCode.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Defines the result codes for file system operations.
    /// </summary>
    public enum FileSystemOperationResultCode
    {
        /// <summary>
        /// Indicates that the operation successfully completed.
        /// </summary>
        Completed,

        /// <summary>
        /// Indicates that access is denied for the specified authentication.
        /// </summary>
        AccessDenied,

        /// <summary>
        /// Indicates that the operation is invalid.
        /// </summary>
        InvalidOperation,

        /// <summary>
        /// Indicates that the path is invalid.
        /// </summary>
        InvalidPath,

        /// <summary>
        /// Indicates that the path is unavailable.
        /// </summary>
        UnavailablePath,

        /// <summary>
        /// Indicates that the operation is unsupported.
        /// </summary>
        UnsupportedOperation,

        /// <summary>
        /// Indicates that the connection is not available.
        /// </summary>
        ConnectionNotAvailable,

        /// <summary>
        /// Indicates that the result is unknown.
        /// </summary>
        Unknown
    }
}
