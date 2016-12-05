// <copyright file="HadoopRenameOperation.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Net.Http;
    using System.Threading.Tasks;
    using ELTE.AEGIS.Storage.Authentication;
    using ELTE.AEGIS.Storage.Resources;
    using Newtonsoft.Json.Linq;

    /// <summary>
    ///  Represents a Hadoop file system operation for renaming.
    /// </summary>
    public class HadoopRenameOperation : HadoopFileSystemOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation" /> class.
        /// </summary>
        public HadoopRenameOperation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="destination">The destination path.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The destination path is empty.
        /// </exception>
        public HadoopRenameOperation(String path, IHadoopAuthentication authentication, String destination)
            : base(path, authentication)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination), StorageMessages.DestinationPathIsNull);
            if (String.IsNullOrEmpty(destination))
                throw new ArgumentException(StorageMessages.DestinationPathIsEmpty, nameof(destination));

            this.Destination = destination;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        public HadoopRenameOperation(HttpClient client)
            : base(client, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopRenameOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="destination">The destination path.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// or
        /// The destination path is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The destination path is empty.
        /// </exception>
        public HadoopRenameOperation(HttpClient client, String path, IHadoopAuthentication authentication, String destination)
            : base(client, null, path, authentication)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination), StorageMessages.DestinationPathIsNull);
            if (String.IsNullOrEmpty(destination))
                throw new ArgumentException(StorageMessages.DestinationPathIsEmpty, nameof(destination));

            this.Destination = destination;
        }

        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>The destination path.</value>
        public String Destination { get; set; }

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The HTTP type of the request used for execution.</value>
        protected override HttpRequestType RequestType
        {
            get { return HttpRequestType.Put; }
        }

        /// <summary>
        /// Gets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        protected override String OperationRequest
        {
            get { return "op=RENAME&destination=" + this.Destination; }
        }

        /// <summary>
        /// Creates the result for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced operation result.</returns>
        protected async override Task<HadoopFileSystemOperationResult> CreateResultAsync(HttpContent content)
        {
            return new HadoopBooleanOperationResult
            {
                Request = this.CompleteRequest,
                Success = JObject.Parse(await content.ReadAsStringAsync()).Value<Boolean>("boolean")
            };
        }
    }
}
