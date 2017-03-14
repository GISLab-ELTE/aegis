// <copyright file="HadoopFileSystemTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Tests.IO.Storage
{
    using System;
    using System.IO;
    using System.Net.Http;
    using AEGIS.Storage;
    using AEGIS.Storage.Authentication;
    using AEGIS.Storage.FileSystems;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="HadoopFileSystem" /> class.
    /// </summary>
    [TestFixture]
    public class HadoopFileSystemTest
    {
        /// <summary>
        /// The hostname.
        /// </summary>
        private String hostname;

        /// <summary>
        /// The port number.
        /// </summary>
        private Int32 portNumber;

        /// <summary>
        /// The authentication.
        /// </summary>
        private IHadoopAuthentication authentication;

        /// <summary>
        /// The path of the working directory.
        /// </summary>
        private String directoryPath;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.hostname = "TESTSERVER";
            this.portNumber = 14000;
            this.authentication = new HadoopUsernameAuthentication("hduser");
            this.directoryPath = "/UnitTest" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// Test tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            // remove the test directory
            if (fileSystem.IsConnected && fileSystem.Exists(this.directoryPath))
                fileSystem.Delete(this.directoryPath);
        }

        /// <summary>
        /// Tests the constructor of the <see cref="HadoopFileSystem" /> class.
        /// </summary>
        [Test]
        public void HadoopFileSystemConstructorTest()
        {
            HadoopFileSystem fileSystem;
            Uri location = new Uri(String.Format("http://{0}:{1}", this.hostname, this.portNumber));

            // host name and port parameters

            fileSystem = new HadoopFileSystem(this.hostname, this.portNumber);

            fileSystem.IsConnected.ShouldBeFalse();
            fileSystem.DirectorySeparator.ShouldBe('/');
            fileSystem.VolumeSeparator.ShouldBe('/');
            fileSystem.PathSeparator.ShouldBe(';');
            fileSystem.VolumeSeparator.ShouldBe('/');
            fileSystem.UriScheme.ShouldBe("hdfs");
            fileSystem.IsContentBrowsingSupported.ShouldBeTrue();
            fileSystem.IsContentWritingSupported.ShouldBeTrue();
            fileSystem.IsStreamingSupported.ShouldBeTrue();
            fileSystem.Location.ShouldBe(location);
            fileSystem.Authentication.AutenticationType.ShouldBe(StorageAuthenticationType.Anonymous);

            // host, port and authentication parameters

            fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            fileSystem.Authentication.ShouldBe(this.authentication);

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(null, 1));
            Should.Throw<ArgumentException>(() => fileSystem = new HadoopFileSystem(String.Empty, 1));
            Should.Throw<ArgumentException>(() => fileSystem = new HadoopFileSystem(":", 1));
            Should.Throw<ArgumentOutOfRangeException>(() => fileSystem = new HadoopFileSystem(this.hostname, 0));

            Should.Throw<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(null));
            Should.Throw<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(null, this.authentication));
            Should.Throw<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(location, this.authentication, null));
            Should.Throw<ArgumentNullException>(() => fileSystem = new HadoopFileSystem(location, (HttpClient)null));
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.CreateDirectory(String)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemCreateDirectoryTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();

            // new directory

            fileSystem.Exists(this.directoryPath).ShouldBeFalse();

            fileSystem.CreateDirectory(this.directoryPath);

            fileSystem.Exists(this.directoryPath).ShouldBeTrue();
            fileSystem.IsDirectory(this.directoryPath).ShouldBeTrue();

            fileSystem.CreateDirectory(this.directoryPath + "/InternalDirectory");

            fileSystem.Exists(this.directoryPath + "/InternalDirectory").ShouldBeTrue();
            fileSystem.IsDirectory(this.directoryPath + "/InternalDirectory").ShouldBeTrue();

            // existing directory

            fileSystem.CreateDirectory("/");
            fileSystem.CreateDirectory(this.directoryPath);

            fileSystem.Exists(this.directoryPath).ShouldBeTrue();

            fileSystem.Delete(this.directoryPath);

            // multiple new directories

            fileSystem.CreateDirectory(this.directoryPath + "/InternalDirectory");

            fileSystem.Exists(this.directoryPath).ShouldBeTrue();
            fileSystem.Exists(this.directoryPath + "/InternalDirectory").ShouldBeTrue();

            fileSystem.Delete(this.directoryPath);

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem.CreateDirectory(null));
            Should.Throw<ArgumentException>(() => fileSystem.CreateDirectory(String.Empty));
            Should.Throw<ArgumentException>(() => fileSystem.CreateDirectory("InvalidPath"));
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.OpenFile(String, FileMode, FileAccess)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemOpenFileTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();
            if (!fileSystem.Exists(this.directoryPath + "TestFile.txt"))
                Assert.Inconclusive();

            // read existing file

            Stream fileStream = fileSystem.OpenFile(this.directoryPath + "TestFile.txt", FileMode.Open, FileAccess.Read);

            fileSystem.ShouldNotBeNull();

            StreamReader reader = new StreamReader(fileStream);

            reader.ReadToEnd().Length.ShouldBeGreaterThan(0);
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.Delete(String)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemDeleteTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();

            // existing path

            fileSystem.CreateDirectory(this.directoryPath);

            fileSystem.Exists(this.directoryPath).ShouldBeTrue();
            fileSystem.IsDirectory(this.directoryPath).ShouldBeTrue();

            fileSystem.Delete(this.directoryPath);

            fileSystem.Exists(this.directoryPath).ShouldBeFalse();
            fileSystem.IsDirectory(this.directoryPath).ShouldBeFalse();

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem.Delete(null));
            Should.Throw<ArgumentException>(() => fileSystem.Delete(String.Empty));
            Should.Throw<ArgumentException>(() => fileSystem.Delete("InvalidPath"));
            Should.Throw<ArgumentException>(() => fileSystem.Delete("/NotExistingPath"));
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.GetDirectoryRoot(String)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemGetDirectoryRootTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            // valid parameters

            fileSystem.GetDirectoryRoot("/").ShouldBe("/");
            fileSystem.GetDirectoryRoot(this.directoryPath).ShouldBe("/");

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem.GetDirectoryRoot(null));
            Should.Throw<ArgumentException>(() => fileSystem.GetDirectoryRoot(String.Empty));
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.GetParent(String)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemGetParentTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();

            // valid parameters

            fileSystem.CreateDirectory(this.directoryPath);
            fileSystem.CreateDirectory(this.directoryPath + "/InternalDirectory");

            fileSystem.GetParent("/").ShouldBeNull();
            fileSystem.GetParent(this.directoryPath).ShouldBe("/");
            fileSystem.GetParent(this.directoryPath + "/").ShouldBe("/");
            fileSystem.GetParent(this.directoryPath + "/InternalDirectory").ShouldBe(this.directoryPath);
            fileSystem.GetParent(this.directoryPath + "/InternalDirectory/").ShouldBe(this.directoryPath);

            fileSystem.Delete(this.directoryPath);

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem.GetParent(null));
            Should.Throw<ArgumentException>(() => fileSystem.GetParent(String.Empty));
            Should.Throw<ArgumentException>(() => fileSystem.GetParent(":"));
            Should.Throw<ArgumentException>(() => fileSystem.GetParent("/NotExistingPath/"));
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.GetDirectory(String)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemGetDirectoryTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();

            // valid parameters

            fileSystem.CreateDirectory(this.directoryPath);
            fileSystem.CreateDirectory(this.directoryPath + "/InternalDirectory");

            fileSystem.GetDirectory("/").ShouldBe("/");
            fileSystem.GetDirectory(this.directoryPath).ShouldBe(this.directoryPath);
            fileSystem.GetDirectory(this.directoryPath + "/").ShouldBe(this.directoryPath);
            fileSystem.GetDirectory(this.directoryPath + "/InternalDirectory").ShouldBe(this.directoryPath + "/InternalDirectory");

            fileSystem.Delete(this.directoryPath);

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem.GetDirectory(null));
            Should.Throw<ArgumentException>(() => fileSystem.GetDirectory(String.Empty));
            Should.Throw<ArgumentException>(() => fileSystem.GetDirectory(":"));
            Should.Throw<ArgumentException>(() => fileSystem.GetDirectory("/NotExistingPath/"));
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.GetDirectories(String, String, Boolean)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemGetDirectoriesTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();

            // empty directory

            fileSystem.CreateDirectory(this.directoryPath);
            fileSystem.GetDirectories(this.directoryPath).ShouldBeEmpty();

            // only directories

            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory1");
            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory2");

            String[] directories = fileSystem.GetDirectories(this.directoryPath);

            directories.Length.ShouldBe(2);
            directories[0].ShouldBe(this.directoryPath + "/InnerDirectory1");
            directories[1].ShouldBe(this.directoryPath + "/InnerDirectory2");

            // with search pattern

            directories = fileSystem.GetDirectories(this.directoryPath, "*1", false);

            directories.Length.ShouldBe(1);
            directories[0].ShouldBe(this.directoryPath + "/InnerDirectory1");

            // recursive

            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory1");
            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory2");

            directories = fileSystem.GetDirectories(this.directoryPath, "*", true);

            directories.Length.ShouldBe(4);
            directories[0].ShouldBe(this.directoryPath + "/InnerDirectory1");
            directories[1].ShouldBe(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory1");
            directories[2].ShouldBe(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory2");
            directories[3].ShouldBe(this.directoryPath + "/InnerDirectory2");

            fileSystem.Delete(this.directoryPath);

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem.GetDirectories(null));
            Should.Throw<ArgumentException>(() => fileSystem.GetDirectories(String.Empty));
            Should.Throw<ArgumentException>(() => fileSystem.GetDirectories(":"));
            Should.Throw<ArgumentException>(() => fileSystem.GetDirectories("/NotExistingPath/"));
        }

        /// <summary>
        /// Tests the <see cref="HadoopFileSystem.GetFileSystemEntries(String, String, Boolean)" /> method.
        /// </summary>
        [Test]
        public void HadoopFileSystemGetFileSystemEntriesTest()
        {
            HadoopFileSystem fileSystem = new HadoopFileSystem(this.hostname, this.portNumber, this.authentication);

            if (!fileSystem.IsConnected)
                Assert.Inconclusive();

            // empty directory

            fileSystem.CreateDirectory(this.directoryPath);
            fileSystem.GetFileSystemEntries(this.directoryPath).ShouldBeEmpty();

            // only directories

            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory1");
            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory2");

            FileSystemEntry[] directories = fileSystem.GetFileSystemEntries(this.directoryPath);

            directories.Length.ShouldBe(2);
            directories[0].Path.ShouldBe(this.directoryPath + "/InnerDirectory1");
            directories[1].Path.ShouldBe(this.directoryPath + "/InnerDirectory2");

            // with search pattern

            directories = fileSystem.GetFileSystemEntries(this.directoryPath, "*1", false);

            directories.Length.ShouldBe(1);
            directories[0].Path.ShouldBe(this.directoryPath + "/InnerDirectory1");

            // recursive

            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory1");
            fileSystem.CreateDirectory(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory2");

            directories = fileSystem.GetFileSystemEntries(this.directoryPath, "*", true);

            directories.Length.ShouldBe(4);
            directories[0].Path.ShouldBe(this.directoryPath + "/InnerDirectory1");
            directories[1].Path.ShouldBe(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory1");
            directories[2].Path.ShouldBe(this.directoryPath + "/InnerDirectory1/DeepInnerDirectory2");
            directories[3].Path.ShouldBe(this.directoryPath + "/InnerDirectory2");

            fileSystem.Delete(this.directoryPath);

            // exceptions

            Should.Throw<ArgumentNullException>(() => fileSystem.GetFileSystemEntries(null));
            Should.Throw<ArgumentException>(() => fileSystem.GetFileSystemEntries(String.Empty));
            Should.Throw<ArgumentException>(() => fileSystem.GetFileSystemEntries(":"));
            Should.Throw<ArgumentException>(() => fileSystem.GetFileSystemEntries("/NotExistingPath/"));
        }
    }
}
