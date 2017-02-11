// <copyright file="HadoopReadFileOperation.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using ELTE.AEGIS.Storage.Authentication;
    using ELTE.AEGIS.Storage.Resources;

    /// <summary>
    ///  Represents a Hadoop file system operation for reading files.
    /// </summary>
    public class HadoopReadFileOperation : HadoopFileSystemOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        public HadoopReadFileOperation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopReadFileOperation(String path, IHadoopAuthentication authentication)
            : base(path, authentication)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="offset">The zero based byte offset in the file.</param>
        /// <param name="length">The number of bytes to be read.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The offset is less than 0.
        /// or
        /// The length is less than 0.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopReadFileOperation(String path, IHadoopAuthentication authentication, Int64 offset, Int64 length)
            : base(path, authentication)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), StorageMessages.OffsetIsLessThan0);
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), StorageMessages.LengthIsLessThan0);

            this.Offset = offset;
            this.Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        public HadoopReadFileOperation(HttpClient client)
            : base(client, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
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
        public HadoopReadFileOperation(HttpClient client, String path, IHadoopAuthentication authentication)
            : base(client, null, path, authentication)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopReadFileOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <param name="offset">The zero based byte offset in the file.</param>
        /// <param name="length">The number of bytes to be read.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The offset is less than 0.
        /// or
        /// The length is less than 0.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The client is null.
        /// or
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopReadFileOperation(HttpClient client, String path, IHadoopAuthentication authentication, Int64 offset, Int64 length)
            : base(client, null, path, authentication)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), StorageMessages.OffsetIsLessThan0);
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), StorageMessages.LengthIsLessThan0);

            this.Offset = offset;
            this.Length = length;
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>The zero-based byte offset at which the reading from the file begins.</value>
        public Int64 Offset { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The number of bytes read from the file. If the length is zero, the entire content of the file will be read.</value>
        public Int64 Length { get; set; }

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The HTTP type of the request used for execution.</value>
        protected override HttpRequestType RequestType
        {
            get { return HttpRequestType.Get; }
        }

        /// <summary>
        /// Gets the request of the operation.
        /// </summary>
        /// <value>The request of the operation.</value>
        protected override String OperationRequest
        {
            get
            {
                StringBuilder requestBuilder = new StringBuilder("op=OPEN");

                if (this.Offset > 0)
                    requestBuilder.Append("&offset=" + this.Offset);

                if (this.Length > 0)
                    requestBuilder.Append("&length=" + this.Length);

                return requestBuilder.ToString();
            }
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
                FileStream = await content.ReadAsStreamAsync()
            };
        }
    }
}
