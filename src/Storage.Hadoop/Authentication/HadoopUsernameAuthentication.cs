// <copyright file="HadoopUsernameAuthentication.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Authentication
{
    using System;
    using AEGIS.Storage.Resources;

    /// <summary>
    /// Represents a Hadoop file system authentication based on user name.
    /// </summary>
    public class HadoopUsernameAuthentication : IHadoopAuthentication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopUsernameAuthentication" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <exception cref="System.ArgumentNullException">The username is null.</exception>
        /// <exception cref="System.ArgumentException">The username is empty.</exception>
        public HadoopUsernameAuthentication(String username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (String.IsNullOrEmpty(username))
                throw new ArgumentException(StorageMessages.UsernameIsEmpty, nameof(username));

            this.Username = username;
        }

        /// <summary>
        /// Gets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        public StorageAuthenticationType AutenticationType { get { return StorageAuthenticationType.UserCredentials; } }

        /// <summary>
        /// Gets the request of the authentication.
        /// </summary>
        /// <value>The request form of the authentication.</value>
        public String Request { get { return "user.name=" + this.Username; } }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The Hadoop username used for executing operations.</value>
        public String Username { get; private set; }
    }
}
