// <copyright file="HadoopBooleanOperationResultTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.Storage.FileSystems.Operation
{
    using AEGIS.Storage.FileSystems.Operations;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="HadoopBooleanOperationResult" /> class.
    /// </summary>
    [TestFixture]
    public class HadoopBooleanOperationResultTest
    {
        /// <summary>
        /// Tests the constructor of the <see cref="HadoopBooleanOperationResult" /> class.
        /// </summary>
        [Test]
        public void HadoopBooleanOperationResultConstructorTest()
        {
            // no parameters

            HadoopBooleanOperationResult result = new HadoopBooleanOperationResult();

            result.Request.ShouldBeNull();
            result.Success.ShouldBe(false);

            // request and success parameters

            result = new HadoopBooleanOperationResult("request", true);

            result.Request.ShouldBe("request");
            result.Success.ShouldBe(true);
        }
    }
}
