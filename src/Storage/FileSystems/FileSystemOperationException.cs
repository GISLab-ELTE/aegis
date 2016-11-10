// <copyright file="FileSystemOperationException.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Organization.FileSystems
{
    using System;
    using System.Net;

    /// <summary>
    /// Represents the exception that is thrown in case of FTP operation error.
    /// </summary>
    public class FileSystemOperationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemOperationException" /> class.
        /// </summary>
        public FileSystemOperationException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemOperationException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FileSystemOperationException(String message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemOperationException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="path">The path where the exception occurred.</param>
        /// <param name="code">The result code of the exception.</param>
        public FileSystemOperationException(String message, Uri path, FileSystemOperationResultCode code)
            : base(message)
        {
            this.Path = path;
            this.Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemOperationException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public FileSystemOperationException(String message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemOperationException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="path">The path where the exception occurred.</param>
        /// <param name="code">The result code of the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public FileSystemOperationException(String message, Uri path, FileSystemOperationResultCode code, Exception innerException)
            : base(message, innerException)
        {
            this.Path = path;
            this.Code = code;
        }

        /// <summary>
        /// Gets the path where the exception occurred.
        /// </summary>
        /// <value>The absolute path where the exception occurred.</value>
        public Uri Path { get; private set; }

        /// <summary>
        /// Gets the result code of the exception.
        /// </summary>
        /// <value>The result code of the exception.</value>
        public FileSystemOperationResultCode Code { get; private set; }
    }
}
