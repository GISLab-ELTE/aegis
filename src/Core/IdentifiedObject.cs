// <copyright file="IdentifiedObject.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents an identified object.
    /// </summary>
    public abstract class IdentifiedObject : IEquatable<IdentifiedObject>
    {
        #region Public constants

        /// <summary>
        /// The user-defined identifier. This field is constant.
        /// </summary>
        public const String UserDefinedIdentifier = DefaultAuthority + Separator + UserDefinedCode;

        /// <summary>
        /// The user-defined name. This field is constant.
        /// </summary>
        public const String UserDefinedName = "User-defined";

        #endregion

        #region Protected constants

        /// <summary>
        /// The undefined identifier. This field is constant.
        /// </summary>
        protected const String UndefinedIdentifier = DefaultAuthority + Separator + UndefinedCode;

        /// <summary>
        /// The undefined name. This field is constant.
        /// </summary>
        protected const String UndefinedName = "Undefined";

        #endregion

        #region Private constants

        /// <summary>
        /// The default authority. This field is constant.
        /// </summary>
        private const String DefaultAuthority = "AEGIS";

        /// <summary>
        /// The code for the user-defined identifier. This field is constant.
        /// </summary>
        private const String UserDefinedCode = "999999";

        /// <summary>
        /// The code for the undefined identifier. This field is constant.
        /// </summary>
        private const String UndefinedCode = "000000";

        /// <summary>
        /// The separator between authority and code. This field is constant.
        /// </summary>
        private const String Separator = "::";

        /// <summary>
        /// The string format of the identified object. This field is constant.
        /// </summary>
        private const String StringFormat = "[{0}] {1}";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedObject" /> class.
        /// </summary>
        protected IdentifiedObject()
            : this(UndefinedIdentifier, UndefinedName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedObject" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected IdentifiedObject(String identifier, String name)
            : this(identifier, name, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedObject" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected IdentifiedObject(String identifier, String name, String remarks, String[] aliases)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);

            this.Identifier = identifier;
            this.Name = name ?? String.Empty;
            this.Remarks = remarks ?? String.Empty;
            this.Aliases = aliases ?? new String[0];
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>An identifier which references elsewhere the object's defining information.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the authority.
        /// </summary>
        /// <value>The authority responsible for the object if provided; otherwise, <c>Empty</c>.</value>
        public String Authority { get { return GetAuthority(this.Identifier); } }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code by which the object is identified in the authority's domain if provided; otherwise, <c>0</c>.</value>
        public Int32 Code { get { return GetCode(this.Identifier); } }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The primary name by which this object is identified.</value>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the remarks.
        /// </summary>
        /// <value>Comments on or information about this object, including data source information.</value>
        public String Remarks { get; private set; }

        /// <summary>
        /// Gets the aliases that can also be used for naming purposes.
        /// </summary>
        /// <value>The collection of aliases containing alternative names by which this object is identified.</value>
        public IEnumerable<String> Aliases { get; private set; }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Determines whether the specified identifier object is equal to the current instance.
        /// </summary>
        /// <param name="other">The identifier object to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public virtual Boolean Equals(IdentifiedObject other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.Identifier.Equals(other.Identifier) && this.Name.Equals(other.Name);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified object is equal to the current identifier object.
        /// </summary>
        /// <param name="obj">The object to compare with the current identifier object.</param>
        /// <returns><c>true</c> if the object is equal to the current identifier object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            IdentifiedObject identifiedObj = obj as IdentifiedObject;

            return identifiedObj != null && this.Identifier.Equals(identifiedObj.Identifier) && this.Name.Equals(identifiedObj.Name);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current identifier object.</returns>
        public override Int32 GetHashCode()
        {
            return this.Identifier.GetHashCode() ^ this.Name.GetHashCode() ^ 925409699;
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents the current identifier object.
        /// </summary>
        /// <returns>A <see cref="String" /> that contains both identifier and name.</returns>
        public override String ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, StringFormat, this.Identifier, this.Name);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Returns the authority for the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The authority responsible for the identifier if provided; otherwise, <c>Empty</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public static String GetAuthority(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);

            return identifier.Contains(Separator) ? identifier.Substring(0, identifier.IndexOf(Separator, StringComparison.Ordinal)) : String.Empty;
        }

        /// <summary>
        /// Returns the code for the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The code in the authority's domain if provided; otherwise, <c>0</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public static Int32 GetCode(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), Messages.IdentifierIsNull);

            String codeString = identifier.Contains(Separator) ? identifier.Substring(identifier.LastIndexOf(Separator, StringComparison.Ordinal) + Separator.Length) : identifier;
            Int32 code;

            return Int32.TryParse(codeString, out code) ? code : 0;
        }

        /// <summary>
        /// Returns the identifier for the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The identifier.</returns>
        public static String GetIdentifier(String authority, Int32 code)
        {
            if (authority == null)
                authority = DefaultAuthority;

            return authority + Separator + code;
        }

        /// <summary>
        /// Returns the identifier for the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The identifier.</returns>
        public static String GetIdentifier(String authority, String code)
        {
            if (authority == null)
                authority = DefaultAuthority;
            if (code == null)
                code = UndefinedCode;

            return authority + Separator + code;
        }

        #endregion
    }
}
