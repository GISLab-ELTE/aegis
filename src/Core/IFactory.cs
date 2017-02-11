// <copyright file="IFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines general behavior of factories.
    /// </summary>
    /// <remarks>
    /// A factory is an object with the sole purpose of creating other objects, known as products. It can be seen as an abstraction of the constructor, allowing more construction possibilities and additional overloads for creating product instances. Factories are also useful in case the concrete implementation of the product needs be hidden, and only the interface can be revealed. Thus the factory can change the product implementation during runtime.
    /// Instantiation of more complex objects, such as features should be performed by using a dedicated factory to control global behavior for a group of objects.
    /// This interface realizes the abstract factory design pattern.
    /// </remarks>
    public interface IFactory
    {
        /// <summary>
        /// Gets the directly underlying factories.
        /// </summary>
        /// <value>The collection of directly underlying factories.</value>
        IEnumerable<IFactory> Factories { get; }

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The factory contract.</typeparam>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        Boolean ContainsFactory<TContract>();

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        Boolean ContainsFactory(Type factoryContract);

        /// <summary>
        /// Ensures the specified underlying factory.
        /// </summary>
        /// <typeparam name="TContract">The factory contract.</typeparam>
        /// <param name="factory">The factory behavior.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory contract is null.
        /// or
        /// The factory behavior is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The factory behavior does not implement the contract.</exception>
        void EnsureFactory<TContract>(TContract factory)
            where TContract : IFactory;

        /// <summary>
        /// Ensures the specified underlying factory.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <param name="factory">The factory behavior.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory contract is null.
        /// or
        /// The factory behavior is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The factory behavior does not implement the contract.</exception>
        void EnsureFactory(Type factoryContract, IFactory factory);

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The factory contract.</typeparam>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        TContract GetFactory<TContract>()
            where TContract : IFactory;

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        IFactory GetFactory(Type factoryContract);
    }
}
