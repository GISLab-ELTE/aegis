// <copyright file="FileSystemEntry.cs" company="Eötvös Loránd University (ELTE)">
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
    using System;
    using ELTE.AEGIS.Storage.Resources;

    /// <summary>
    /// Represents a file system entry.
    /// </summary>
    public class FileSystemEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntry" /> class.
        /// </summary>
        /// <param name="path">The path of the entry.</param>
        /// <param name="name">The name of the entry.</param>
        /// <param name="entryType">The type of the entry.</param>
        /// <param name="creationTime">The time of creation.</param>
        /// <param name="lastAccessTime">The time of the last access.</param>
        /// <param name="lastModificationTime">The time of the last modification.</param>
        /// <param name="length">The length of the file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The name is null.
        /// </exception>
        public FileSystemEntry(String path, String name, FileSystemEntryType entryType, DateTime creationTime, DateTime lastAccessTime, DateTime lastModificationTime, Int64 length)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path), StorageMessages.PathIsNull);
            if (name == null)
                throw new ArgumentNullException(nameof(name), StorageMessages.NameIsNull);

            this.Path = path;
            this.Name = name;
            this.Type = entryType;
            this.CreationTime = creationTime;
            this.LastAccessTime = lastAccessTime;
            this.LastModificationTime = lastModificationTime;
            this.Length = length;
        }

        /// <summary>
        /// Gets the path of the entry.
        /// </summary>
        /// <value>The full path of the entry.</value>
        public String Path { get; private set; }

        /// <summary>
        /// Gets the name of the entry.
        /// </summary>
        /// <value>The name of the entry.</value>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the full path of the parent directory.
        /// </summary>
        /// <value>The full path of the parent directory.</value>
        public String Parent { get { return this.Path.Substring(0, this.Path.LastIndexOf(this.Name, StringComparison.OrdinalIgnoreCase)); } }

        /// <summary>
        /// Gets the type of the entry.
        /// </summary>
        /// <value>The type of the entry.</value>
        public FileSystemEntryType Type { get; private set; }

        /// <summary>
        /// Gets the time of creation.
        /// </summary>
        /// <value>The time of creation. If the query is not supported by the file system, the minimum <see cref="DateTime" /> value is returned.</value>
        public DateTime CreationTime { get; private set; }

        /// <summary>
        /// Gets the time of the last access.
        /// </summary>
        /// <value>The time of the last access. If the query is not supported by the file system, the minimum <see cref="DateTime" /> value is returned.</value>
        public DateTime LastAccessTime { get; private set; }

        /// <summary>
        /// Gets the time of the last modification.
        /// </summary>
        /// <value>The time of the last modification. If the query is not supported by the file system, the minimum <see cref="DateTime" /> value is returned.</value>
        public DateTime LastModificationTime { get; private set; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file in bytes. The value is <c>0</c> for directories.</value>
        public Int64 Length { get; private set; }
    }
}
