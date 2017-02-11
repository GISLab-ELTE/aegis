// <copyright file="HadoopFileSystemOperation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.FileSystems.Operations
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ELTE.AEGIS.Storage.Authentication;
    using ELTE.AEGIS.Storage.Resources;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a Hadoop file system operation.
    /// </summary>
    public abstract class HadoopFileSystemOperation : IDisposable
    {
        /// <summary>
        /// Defines the possible HTTP request types.
        /// </summary>
        protected enum HttpRequestType
        {
            /// <summary>
            /// Indicates a PUT request.
            /// </summary>
            Put,

            /// <summary>
            /// Indicates a POST request.
            /// </summary>
            Post,

            /// <summary>
            /// Indicates a GET request.
            /// </summary>
            Get,

            /// <summary>
            /// Indicates a DELETE request.
            /// </summary>
            Delete
        }

        /// <summary>
        /// The HTTP content.
        /// </summary>
        private HttpContent content;

        /// <summary>
        /// The HTTP client performing the operations.
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// A value indicating whether this instance is disposed.
        /// </summary>
        private Boolean disposed;

        /// <summary>
        /// A value indicating whether the HTTP client should be disposed.
        /// </summary>
        private Boolean disposeClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        protected HadoopFileSystemOperation()
        {
            this.client = new HttpClient();
            this.disposeClient = false;
            this.disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        protected HadoopFileSystemOperation(String path, IHadoopAuthentication authentication)
            : this()
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path), StorageMessages.PathIsNull);
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException(StorageMessages.PathIsEmpty, nameof(path));
            if (authentication == null)
                throw new ArgumentNullException(nameof(authentication), StorageMessages.AuthenticationIsNull);

            this.Path = path;
            this.Authentication = authentication;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="content">The HTTP content.</param>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        protected HadoopFileSystemOperation(HttpClient client, HttpContent content)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), StorageMessages.ClientIsNull);

            this.client = client;
            this.content = content;
            this.disposeClient = false;
            this.disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="content">The HTTP content.</param>
        /// <param name="path">The path of the operation.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        protected HadoopFileSystemOperation(HttpClient client, HttpContent content, String path, IHadoopAuthentication authentication)
            : this(client, content)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path), StorageMessages.PathIsNull);
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException(StorageMessages.PathIsEmpty, nameof(path));
            if (authentication == null)
                throw new ArgumentNullException(nameof(authentication), StorageMessages.AuthenticationIsNull);

            this.Path = path;
            this.Authentication = authentication;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="HadoopFileSystemOperation" /> class.
        /// </summary>
        ~HadoopFileSystemOperation()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets or sets the timeout of the client.
        /// </summary>
        /// <value>The timeout applied to the operation.</value>
        public TimeSpan Timeout { get { return this.client.Timeout; } set { this.client.Timeout = value; } }

        /// <summary>
        /// Gets or sets the authentication used for the operation.
        /// </summary>
        /// <value>The Hadoop authentication.</value>
        public IHadoopAuthentication Authentication { get; set; }

        /// <summary>
        /// Gets the path of the operation.
        /// </summary>
        /// <value>The absolute path to the operation.</value>
        public String Path { get; private set; }

        /// <summary>
        /// Executes the operation asynchronously.
        /// </summary>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">The remote address returned with an invalid response.</exception>
        /// <exception cref="HadoopRemoteException">The remote address returned with an exception.</exception>
        public async Task<HadoopFileSystemOperationResult> ExecuteAsync()
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.GetType().FullName);

            HttpResponseMessage message = null;

            // execute operation (without content)
            switch (this.RequestType)
            {
                case HttpRequestType.Put:
                    message = await this.client.PutAsync(this.CompleteRequest, null);
                    break;
                case HttpRequestType.Post:
                    message = await this.client.PostAsync(this.CompleteRequest, null);
                    break;
                case HttpRequestType.Get:
                    message = await this.client.GetAsync(this.CompleteRequest, HttpCompletionOption.ResponseHeadersRead);
                    break;
                case HttpRequestType.Delete:
                    message = await this.client.DeleteAsync(this.CompleteRequest);
                    break;
            }

            // some operations result in redirect to data node
            if (message.StatusCode == HttpStatusCode.TemporaryRedirect)
            {
                // execute the operation on the data node (with content)
                switch (this.RequestType)
                {
                    case HttpRequestType.Put:
                        message = await this.client.PutAsync(message.Headers.Location, this.content);
                        break;
                    case HttpRequestType.Post:
                        message = await this.client.PutAsync(message.Headers.Location, this.content);
                        break;
                    case HttpRequestType.Get:
                        message = await this.client.GetAsync(message.Headers.Location, HttpCompletionOption.ResponseHeadersRead);
                        break;
                    case HttpRequestType.Delete:
                        message = await this.client.DeleteAsync(message.Headers.Location);
                        break;
                }
            }

            // handle successful request
            if (message.IsSuccessStatusCode)
            {
                return await this.CreateResultAsync(message.Content);
            }

            // handle unsuccessful request
            switch (message.StatusCode)
            {
                case HttpStatusCode.BadRequest: // excepted error cases
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.InternalServerError:
                    throw await this.CreateRemoteExceptionAsync(message.Content);
                default: // unexpected error cases
                    throw new HttpRequestException(StorageMessages.InvalidResponseFromRemote);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The HTTP type of the request used for execution.</value>
        protected abstract HttpRequestType RequestType { get; }

        /// <summary>
        /// Gets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        protected abstract String OperationRequest { get; }

        /// <summary>
        /// Gets the complete request of the operation.
        /// </summary>
        /// <value>The complete request including the path, operation and authentication.</value>
        protected String CompleteRequest
        {
            get
            {
                switch (this.Authentication.AutenticationType)
                {
                    case StorageAuthenticationType.UserCredentials:
                        return String.Format("{0}?{1}&{2}", this.Path, this.OperationRequest, this.Authentication.Request);
                    default:
                        return String.Format("{0}?{1}", this.Path, this.OperationRequest);
                }
            }
        }

        /// <summary>
        /// Creates the result for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced operation result.</returns>
        protected abstract Task<HadoopFileSystemOperationResult> CreateResultAsync(HttpContent content);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            this.disposed = true;

            if (disposing)
            {
                if (this.disposeClient)
                    this.client.Dispose();
            }
        }

        /// <summary>
        /// Creates the remote exception for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced remote exception.</returns>
        private async Task<HadoopRemoteException> CreateRemoteExceptionAsync(HttpContent content)
        {
            JObject contentObject = JObject.Parse(await content.ReadAsStringAsync()).Value<JObject>("RemoteException");

            return new HadoopRemoteException(contentObject.Value<String>("message"), contentObject.Value<String>("exception"), contentObject.Value<String>("javaClassName"));
        }
    }
}
