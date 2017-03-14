// <copyright file="ConnectionException.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage
{
    using System;

    /// <summary>
    /// Represents the exception that is thrown in case of connection error.
    /// </summary>
    public class ConnectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        public ConnectionException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConnectionException(String message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="path">The path where the exception occurred.</param>
        public ConnectionException(String message, String path)
            : base(message)
        {
            this.Path = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ConnectionException(String message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="path">The path where the exception occurred.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ConnectionException(String message, String path, Exception innerException)
            : base(message, innerException)
        {
            this.Path = path;
        }

        /// <summary>
        /// Gets the path where the exception occurred.
        /// </summary>
        /// <value>The absolute path where the exception occurred.</value>
        public String Path { get; private set; }
    }
}
