// <copyright file="HadoopUsernameAuthenticationTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.IO.Storage.Authentication
{
    using System;
    using ELTE.AEGIS.Storage;
    using ELTE.AEGIS.Storage.Authentication;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="HadoopUsernameAuthentication" /> class.
    /// </summary>
    [TestFixture]
    public class HadoopUsernameAuthenticationTest
    {
        /// <summary>
        /// Tests the constructor of the <see cref="HadoopUsernameAuthentication" /> class.
        /// </summary>
        [Test]
        public void HadoopUserNameAuthenticationConstructorTest()
        {
            // accepted user name

            HadoopUsernameAuthentication authentication = new HadoopUsernameAuthentication("user");

            authentication.Username.ShouldBe("user");
            authentication.Request.ShouldBe("user.name=user");
            authentication.AutenticationType.ShouldBe(StorageAuthenticationType.UserCredentials);

            // exceptions

            Should.Throw<ArgumentNullException>(() => new HadoopUsernameAuthentication(null));
            Should.Throw<ArgumentException>(() => new HadoopUsernameAuthentication(String.Empty));
        }
    }
}
