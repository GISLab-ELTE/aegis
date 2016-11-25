// <copyright file="HadoopBooleanOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.FileSystems.Operations
{
    using System;

    /// <summary>
    /// Represents a Hadoop file system operation result containing a boolean.
    /// </summary>
    public class HadoopBooleanOperationResult : HadoopFileSystemOperationResult
    {
        #region Public properties

        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        /// <value><c>true</c> if the operation was successful; otherwise <c>false</c>.</value>
        public Boolean Success { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopBooleanOperationResult" /> class.
        /// </summary>
        public HadoopBooleanOperationResult() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopBooleanOperationResult" /> class.
        /// </summary>
        /// <param name="request">The request of the operation.</param>
        /// <param name="success">A value indicating the success of the operation.</param>
        public HadoopBooleanOperationResult(String request, Boolean success)
            : base(request)
        {
            this.Success = success;
        }

        #endregion
    }
}
