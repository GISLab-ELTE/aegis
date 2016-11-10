// <copyright file="StorageAuthenticationType.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage
{
    /// <summary>
    /// Defines possible types for storage authentication.
    /// </summary>
    public enum StorageAuthenticationType
    {
        /// <summary>
        /// Indicates that the authentication is anonymous.
        /// </summary>
        Anonymous,

        /// <summary>
        /// Indicates that the authentication is performed using user credentials.
        /// </summary>
        UserCredentials,

        /// <summary>
        /// Indicates that the authentication is performed using system credentials.
        /// </summary>
        SystemCredentials
    }
}
