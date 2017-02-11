// <copyright file="HadoopRemoteException.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.FileSystems
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the exception that is thrown remotely by the Hadoop distributed file system.
    /// </summary>
    public class HadoopRemoteException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRemoteException" /> class.
        /// </summary>
        public HadoopRemoteException()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRemoteException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HadoopRemoteException(String message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRemoteException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="exceptionName">The name of the remote exception.</param>
        /// <param name="javaClassName">The name of the JAVA exception class.</param>
        public HadoopRemoteException(String message, String exceptionName, String javaClassName)
            : base(message)
        {
            this.ExceptionName = exceptionName;
            this.JavaClassName = javaClassName;
        }

        /// <summary>
        /// Gets the name of the remote exception.
        /// </summary>
        /// <value>The name of the remote exception.</value>
        public String ExceptionName { get; private set; }

        /// <summary>
        /// Gets the name of the JAVA exception class.
        /// </summary>
        /// <value>The full name of the JAVA exception class.</value>
        public String JavaClassName { get; private set; }
    }
}
