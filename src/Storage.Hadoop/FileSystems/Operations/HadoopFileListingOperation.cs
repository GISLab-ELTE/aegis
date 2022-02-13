// <copyright file="HadoopFileListingOperation.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AEGIS.Storage.Authentication;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a Hadoop file system operation returning a file system entry list.
    /// </summary>
    public class HadoopFileListingOperation : HadoopFileSystemOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperation" /> class.
        /// </summary>
        public HadoopFileListingOperation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="authentication">The authentication.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The authentication is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The path is empty.</exception>
        public HadoopFileListingOperation(String path, IHadoopAuthentication authentication)
            : base(path, authentication)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperation" /> class.
        /// </summary>
        /// <param name="client">The HTTP client.</param>
        /// <exception cref="System.ArgumentNullException">The client is null.</exception>
        public HadoopFileListingOperation(HttpClient client)
            : base(client, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperation" /> class.
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
        public HadoopFileListingOperation(HttpClient client, String path, IHadoopAuthentication authentication)
            : base(client, null, path, authentication)
        {
        }

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
            get { return "op=LISTSTATUS"; }
        }

        /// <summary>
        /// Creates the result for the specified content asynchronously.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        /// <returns>The produced operation result.</returns>
        protected async override Task<HadoopFileSystemOperationResult> CreateResultAsync(HttpContent content)
        {
            return new HadoopFileListingOperationResult
            {
                Request = this.CompleteRequest,
                StatusList = JObject.Parse(await content.ReadAsStringAsync()).GetValue("FileStatuses")["FileStatus"].Children<JObject>().Select(fileObject =>
                {
                    HadoopFileStatusOperationResult fileResult = new HadoopFileStatusOperationResult
                    {
                        Name = fileObject.Value<String>("pathSuffix"),
                        AccessTime = new DateTime(1970, 1, 1) + TimeSpan.FromTicks(fileObject.Value<Int64>("accessTime")),
                        ModificationTime = new DateTime(1970, 1, 1) + TimeSpan.FromTicks(fileObject.Value<Int64>("modificationTime")),
                        Length = fileObject.Value<Int64>("length"),
                        BlockSize = fileObject.Value<Int64>("blockSize"),
                    };

                    switch (fileObject.Value<String>("type"))
                    {
                        case "FILE":
                            fileResult.EntryType = FileSystemEntryType.File;
                            break;
                        case "DIRECTORY":
                            fileResult.EntryType = FileSystemEntryType.Directory;
                            break;
                        case "SYMLINK":
                            fileResult.EntryType = FileSystemEntryType.Link;
                            break;
                    }

                    return fileResult;
                }).ToArray(),
            };
        }
    }
}
