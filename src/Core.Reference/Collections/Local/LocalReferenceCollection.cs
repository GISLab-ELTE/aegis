// <copyright file="LocalReferenceCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of references.
    /// </summary>
    /// <typeparam name="ReferenceType">The type of the reference.</typeparam>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public abstract class LocalReferenceCollection<ReferenceType> : IReferenceCollection<ReferenceType>
        where ReferenceType : IdentifiedObject
    {
        /// <summary>
        /// The divider character. This field is constant.
        /// </summary>
        protected const Char Divider = '\t';

        /// <summary>
        /// The name of the authority. This field is constant.
        /// </summary>
        protected const String Authority = "EPSG";

        /// <summary>
        /// The resource path. This field is constant.
        /// </summary>
        private const String ResourcePath = "ELTE.AEGIS.Reference.Resources.{0}.txt";

        /// <summary>
        /// The resource path to the name aliases. This field is constant.
        /// </summary>
        private const String ResourcePathAlias = "Alias";

        /// <summary>
        /// The name of the resource. This field is read-only.
        /// </summary>
        private readonly String resourceName;

        /// <summary>
        /// The name of the alias type. This field is read-only.
        /// </summary>
        private readonly String aliasName;

        /// <summary>
        /// The dictionary of references.
        /// </summary>
        private Dictionary<Int32, ReferenceType> referenceDictionary;

        /// <summary>
        /// The dictionary of aliases.
        /// </summary>
        private Dictionary<Int32, List<String>> aliasDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalReferenceCollection{ReferenceType}" /> class.
        /// </summary>
        protected LocalReferenceCollection()
            : this(typeof(ReferenceType).Name, typeof(ReferenceType).Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalReferenceCollection{ReferenceType}" /> class.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="sliasName">The name of the alias type.</param>
        protected LocalReferenceCollection(String resourceName, String sliasName)
        {
            this.resourceName = resourceName ?? typeof(ReferenceType).Name;
            this.aliasName = sliasName ?? typeof(ReferenceType).Name;
        }

        /// <summary>
        /// Gets the item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The item with the specified identifier.</returns>
        public ReferenceType this[String identifier]
        {
            get
            {
                if (identifier == null)
                    return null;

                String authority = IdentifiedObject.GetAuthority(identifier);
                Int32 code = IdentifiedObject.GetCode(identifier);

                if (!String.IsNullOrEmpty(authority) && authority != Authority)
                    return null;

                return this.GetReference(code);
            }
        }

        /// <summary>
        /// Gets the item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The item with the specified authority and code.</returns>
        public ReferenceType this[String authority, Int32 code]
        {
            get
            {
                if (authority == null || authority != Authority)
                    return null;

                return this.GetReference(code);
            }
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<ReferenceType> WithIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            this.EnsureAliases();
            this.EnsureReferences();

            // match exact code
            String authority = IdentifiedObject.GetAuthority(identifier);
            Int32 code = IdentifiedObject.GetCode(identifier);

            if (authority == Authority && this.referenceDictionary.ContainsKey(code))
                yield return this.referenceDictionary[code];

            // match contained code
            foreach (ReferenceType reference in this.referenceDictionary.Values.Where(reference => reference.Identifier.IndexOf(identifier, StringComparison.OrdinalIgnoreCase) >= 0))
                yield return reference;
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<ReferenceType> WithName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            this.EnsureAliases();
            this.EnsureReferences();

            // match exact name
            ReferenceType match = this.referenceDictionary.Values.FirstOrDefault(reference => String.Equals(reference.Name, name, StringComparison.OrdinalIgnoreCase) || reference.Aliases.Any(alias => String.Equals(alias, name, StringComparison.OrdinalIgnoreCase)));
            if (match != null)
                yield return match;

            // match contained name
            foreach (ReferenceType reference in this.referenceDictionary.Values.Where(reference => reference.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 || reference.Aliases.Any(alias => alias.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)))
                yield return reference;
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<ReferenceType> WithMatchingIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            this.EnsureAliases();
            this.EnsureReferences();

            Regex identifierRegex = new Regex(identifier, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            identifier = Regex.Escape(identifier);
            foreach (ReferenceType reference in this.referenceDictionary.Values.Where(reference => identifierRegex.IsMatch(reference.Identifier)))
                yield return reference;
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<ReferenceType> WithMatchingName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            this.EnsureAliases();
            this.EnsureReferences();

            Regex nameRegex = new Regex(name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            name = Regex.Escape(name);
            foreach (ReferenceType regexMatch in this.referenceDictionary.Values.Where(reference => nameRegex.IsMatch(reference.Name) || reference.Aliases.Any(alias => nameRegex.IsMatch(alias))))
                yield return regexMatch;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{ReferenceType}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<ReferenceType> GetEnumerator()
        {
            this.EnsureReferences();
            this.EnsureAliases();

            foreach (ReferenceType reference in this.referenceDictionary.Values)
                yield return reference;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.EnsureReferences();
            this.EnsureAliases();

            foreach (ReferenceType reference in this.referenceDictionary.Values)
                yield return reference;
        }

        /// <summary>
        /// Returns a reference.
        /// </summary>
        /// <param name="code">The code of the reference.</param>
        /// <returns>The reference if available; otherwise, <c>null</c>.</returns>
        protected ReferenceType GetReference(Int32 code)
        {
            this.EnsureAliases();
            this.EnsureReferences();

            if (code == 0 || !this.referenceDictionary.ContainsKey(code))
                return null;

            return this.referenceDictionary[code];
        }

        /// <summary>
        /// Returns all references.
        /// </summary>
        /// <returns>The collection of  references.</returns>
        protected IEnumerable<ReferenceType> GetReferences()
        {
            this.EnsureAliases();
            this.EnsureReferences();

            return this.referenceDictionary.Values;
        }

        /// <summary>
        /// Opens the resource stream.
        /// </summary>
        /// <returns>The resource stream.</returns>
        protected Stream OpenResource()
        {
            return this.OpenResource(this.resourceName);
        }

        /// <summary>
        /// Opens the resource stream.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns>The resource stream.</returns>
        protected Stream OpenResource(String resourceName)
        {
            return this.GetType().GetTypeInfo().Assembly.GetManifestResourceStream(String.Format(ResourcePath, resourceName));
        }

        /// <summary>
        /// Returns the aliases for the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>The aliases.</returns>
        protected String[] GetAliases(Int32 code)
        {
            return (this.aliasDictionary != null && this.aliasDictionary.ContainsKey(code)) ? this.aliasDictionary[code].ToArray() : new String[0];
        }

        /// <summary>
        /// Converts the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The converted reference.</returns>
        protected abstract ReferenceType Convert(String[] content);

        /// <summary>
        /// Ensures that the references are available.
        /// </summary>
        private void EnsureReferences()
        {
            if (this.referenceDictionary != null)
                return;

            this.referenceDictionary = new Dictionary<Int32, ReferenceType>();

            using (StreamReader reader = new StreamReader(this.OpenResource()))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    ReferenceType reference = this.Convert(line.Split(Divider));

                    if (reference == null)
                        continue;

                    this.referenceDictionary[reference.Code] = reference;
                }
            }
        }

        /// <summary>
        /// Ensures that the aliases are available.
        /// </summary>
        private void EnsureAliases()
        {
            if (this.aliasDictionary != null)
                return;

            this.aliasDictionary = new Dictionary<Int32, List<String>>();

            using (StreamReader reader = new StreamReader(this.OpenResource(ResourcePathAlias)))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] content = line.Split(Divider);

                    Int32 code = Int32.Parse(content[2]);
                    if (content[1] == this.aliasName)
                    {
                        if (this.aliasDictionary.ContainsKey(code))
                            this.aliasDictionary[code].Add(content[4]);
                        else
                            this.aliasDictionary.Add(code, new List<String> { content[4] });
                    }
                }
            }
        }
    }
}
