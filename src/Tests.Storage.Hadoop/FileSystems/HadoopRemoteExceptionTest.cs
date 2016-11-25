// <copyright file="HadoopRemoteExceptionTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests.IO.Storage
{
    using ELTE.AEGIS.Storage.FileSystems;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="HadoopRemoteException" /> class.
    /// </summary>
    [TestFixture]
    public class HadoopRemoteExceptionTest
    {
        #region Test methods

        /// <summary>
        /// Tests the constructor of the <see cref="HadoopRemoteException" /> class.
        /// </summary>
        [Test]
        public void HadoopRemoteExceptionConstructorTest()
        {
            // no parameters

            HadoopRemoteException exception = new HadoopRemoteException();
            exception.ExceptionName.ShouldBeNull();
            exception.JavaClassName.ShouldBeNull();

            // message parameter

            exception = new HadoopRemoteException("Exception message.");

            exception.Message.ShouldBe("Exception message.");
            exception.ExceptionName.ShouldBeNull();
            exception.JavaClassName.ShouldBeNull();

            // message, name and class parameters

            exception = new HadoopRemoteException("Exception message.", "Exception name.", "Class name.");

            exception.Message.ShouldBe("Exception message.");
            exception.ExceptionName.ShouldBe("Exception name.");
            exception.JavaClassName.ShouldBe("Class name.");
        }

        #endregion
    }
}
