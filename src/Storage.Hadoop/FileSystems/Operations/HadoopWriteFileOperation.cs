﻿// <copyright file="HadoopWriteFileOperation.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Net.Http;
    using System.Threading.Tasks;
    using AEGIS.Storage.Authentication;

    /// <summary>
    ///  Represents a Hadoop file system operation for writing files.
    /// </summary>
    public class HadoopWriteFileOperation : HadoopFileSystemOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopWriteFileOperation" /> class.
        /// </summary>
        public HadoopWriteFileOperation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopWriteFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopWriteFileOperation(String path, IHadoopAuthentication authentication)
            : base(path, authentication)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopWriteFileOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="content">The HTTP content.</param>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        public HadoopWriteFileOperation(HttpClient client, HttpContent content)
            : base(client, content)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopWriteFileOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="content">The HTTP content.</param>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopWriteFileOperation(HttpClient client, HttpContent content, String path, IHadoopAuthentication authentication)
            : base(client, content, path, authentication)
        {
        }

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The HTTP type of the request used for execution.</value>
        protected override HttpRequestType RequestType
        {
            get { return HttpRequestType.Post; }
        }

        /// <summary>
        /// Gets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        protected override String OperationRequest
        {
            get { return "op=APPEND"; }
        }

        /// <summary>
        /// Creates the result for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced operation result.</returns>
        protected async override Task<HadoopFileSystemOperationResult> CreateResultAsync(HttpContent content)
        {
            return new HadoopFileStreamingOperationResult
            {
                Request = this.CompleteRequest,
                FileStream = await content.ReadAsStreamAsync(),
            };
        }
    }
}
