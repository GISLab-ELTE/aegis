// <copyright file="DriverParameter.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ELTE.AEGIS.Storage.Resources;

    /// <summary>
    /// Represents a parameter of a geometry stream format.
    /// </summary>
    public class DriverParameter : IdentifiedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverParameter" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="isOptional">A value indicating whether the parameter is optional.</param>
        /// <param name="defaultValue">The default value of the parameter (if optional).</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public DriverParameter(String identifier, String name, String remarks, String[] aliases, Type type, Boolean isOptional, Object defaultValue, params Predicate<Object>[] conditions)
            : base(identifier, name, remarks, aliases)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), StorageMessages.TypeIsNull);

            this.Type = type;
            this.IsOptional = isOptional;
            this.DefaultValue = defaultValue;
            this.Conditions = conditions;
        }

        /// <summary>
        /// Gets the type declaration of the parameter.
        /// </summary>
        /// <value>The type declaration of the parameter.</value>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the parameter is optional.
        /// </summary>
        /// <value><c>true</c> if the parameter is optional; otherwise, <c>false</c>.</value>
        public Boolean IsOptional { get; private set; }

        /// <summary>
        /// Gets the default value of the parameter.
        /// </summary>
        /// <value>The default value of the parameter.</value>
        public Object DefaultValue { get; private set; }

        /// <summary>
        /// Gets the conditions the parameter value must satisfy.
        /// </summary>
        /// <value>The conditions the parameter value must satisfy.</value>
        public IReadOnlyList<Predicate<Object>> Conditions { get; private set; }

        /// <summary>
        /// Determines whether the specified value is valid for the parameter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if <paramref name="value" /> meets all conditions; otherwise, <c>false</c>.</returns>
        public Boolean IsValid(Object value)
        {
            if (this.Conditions == null || this.Conditions.Count == 0)
                return true;

            for (Int32 condIndex = 0; condIndex < this.Conditions.Count; condIndex++)
            {
                if (!this.Conditions[condIndex](value))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), true, null, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks, T defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), true, defaultValue, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks, String[] aliases)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), true, null, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks, String[] aliases, T defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), true, defaultValue, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), true, null, conditions);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks, T defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), true, defaultValue, conditions);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks, String[] aliases, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), true, null, conditions);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter<T>(String identifier, String name, String remarks, String[] aliases, T defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), true, defaultValue, conditions);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, Type type)
        {
            return new DriverParameter(identifier, name, remarks, null, type, true, null, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, Type type, Object defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, null, type, true, defaultValue, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, String[] aliases, Type type)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, true, null, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, String[] aliases, Type type, Object defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, true, defaultValue, null);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, Type type, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, type, true, null, conditions);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, Type type, Object defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, type, true, defaultValue, conditions);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, String[] aliases, Type type, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, true, null, conditions);
        }

        /// <summary>
        /// Creates an optional <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateOptionalParameter(String identifier, String name, String remarks, String[] aliases, Type type, Object defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, true, defaultValue, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), false, null, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks, T defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), false, defaultValue, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks, String[] aliases)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), false, null, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks, String[] aliases, T defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), false, defaultValue, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), false, null, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks, T defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, typeof(T), false, defaultValue, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks, String[] aliases, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), false, null, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter<T>(String identifier, String name, String remarks, String[] aliases, T defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, typeof(T), false, defaultValue, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, Type type)
        {
            return new DriverParameter(identifier, name, remarks, null, type, false, null, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, Type type, Object defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, null, type, false, defaultValue, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, String[] aliases, Type type)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, false, null, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, String[] aliases, Type type, Object defaultValue)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, false, defaultValue, null);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, Type type, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, type, false, null, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, Type type, Object defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, null, type, false, defaultValue, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, String[] aliases, Type type, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, false, null, conditions);
        }

        /// <summary>
        /// Creates a required <see cref="DriverParameter" />.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type declaration of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="conditions">The conditions the parameter value must satisfy.</param>
        /// <returns>The produced operation parameter.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The type is null.
        /// </exception>
        public static DriverParameter CreateRequiredParameter(String identifier, String name, String remarks, String[] aliases, Type type, Object defaultValue, params Predicate<Object>[] conditions)
        {
            return new DriverParameter(identifier, name, remarks, aliases, type, false, defaultValue, conditions);
        }
    }
}
