// <copyright file="DriverFormat.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a format of geometry stream.
    /// </summary>
    public class DriverFormat : IdentifiedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverFormat" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="extensions">The extensions of the input/output data source.</param>
        /// <param name="internetMediaTypes">The Internet media types.</param>
        /// <param name="supportedGeometries">The supported geometries of the format.</param>
        /// <param name="parameters">The parameters of the format.</param>
        public DriverFormat(String identifier, String name, Version version, String[] extensions, String[] internetMediaTypes, Type[] supportedGeometries, params DriverParameter[] parameters)
            : this(identifier, name, null, null, version, extensions, internetMediaTypes, supportedGeometries, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverFormat" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="extensions">The extensions of the input/output data source.</param>
        /// <param name="internetMediaTypes">The Internet media types.</param>
        /// <param name="supportedGeometries">The supported geometries of the format.</param>
        /// <param name="parameters">The parameters of the format.</param>
        public DriverFormat(String identifier, String name, String remarks, String[] aliases, Version version, String[] extensions, String[] internetMediaTypes, Type[] supportedGeometries, params DriverParameter[] parameters)
            : base(identifier, name, remarks, aliases)
        {
            this.Version = version;
            this.Extensions = extensions;
            this.InternetMediaTypes = internetMediaTypes;
            this.SupportedGeometries = supportedGeometries;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the version of the format.
        /// </summary>
        /// <value>The version of the format.</value>
        public Version Version { get; private set; }

        /// <summary>
        /// Gets the file extensions of the format.
        /// </summary>
        /// <value>The array containing the file extensions of the format.</value>
        public IReadOnlyList<String> Extensions { get; private set; }

        /// <summary>
        /// Gets the Internet media types of the format.
        /// </summary>
        /// <value>The array containing the Internet media types of the format.</value>
        public IReadOnlyList<String> InternetMediaTypes { get; private set; }

        /// <summary>
        /// Gets the supported geometry types of the format.
        /// </summary>
        /// <value>The array containing the supported geometry types of the format.</value>
        public IReadOnlyList<Type> SupportedGeometries { get; private set; }

        /// <summary>
        /// Gets the parameters of the format.
        /// </summary>
        /// <value>The array containing the parameters of the format.</value>
        public IReadOnlyList<DriverParameter> Parameters { get; private set; }
    }
}
