// <copyright file="Factory.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Linq;
    using System.Reflection;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a base type for all factories.
    /// </summary>
    /// <remarks>
    /// This type is the primary implementation of the <see cref="IFactory" /> interface and part of the abstract factory design pattern.
    /// </remarks>
    public abstract class Factory : IFactory
    {
        /// <summary>
        /// The dictionary of factories based on factory contract.
        /// </summary>
        private Dictionary<Type, IFactory> factoryDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class.
        /// </summary>
        protected Factory()
        {
            this.factoryDictionary = new Dictionary<Type, IFactory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class.
        /// </summary>
        /// <param name="factories">The underlying factories based on factory contract.</param>
        protected Factory(params IFactory[] factories)
        {
            if (factories != null)
            {
                this.factoryDictionary = new Dictionary<Type, IFactory>(factories.Length);

                foreach (IFactory factory in factories)
                {
                    foreach (Type factoryContract in factory.GetType().GetTypeInfo().ImplementedInterfaces.Where(inter => inter.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IFactory))))
                    {
                        this.factoryDictionary.Add(factoryContract, factory);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the directly underlying factories.
        /// </summary>
        /// <value>The collection of directly underlying factories.</value>
        public IEnumerable<IFactory> Factories { get { return this.factoryDictionary.Values; } }

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The factory contract.</typeparam>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        public Boolean ContainsFactory<TContract>()
        {
            return this.ContainsFactory(typeof(TContract));
        }

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        public Boolean ContainsFactory(Type factoryContract)
        {
            if (factoryContract == null)
                throw new ArgumentNullException(nameof(factoryContract), CoreMessages.FactoryContractIsNull);

            return this.GetFactoryInternal(factoryContract, new HashSet<Type>()) != null;
        }

        /// <summary>
        /// Ensures the specified underlying factory.
        /// </summary>
        /// <typeparam name="TContract">The factory contract.</typeparam>
        /// <param name="factory">The factory behavior.</param>
        /// <exception cref="System.ArgumentNullException">The factory realization is null.</exception>
        public void EnsureFactory<TContract>(TContract factory)
            where TContract : IFactory
        {
            this.EnsureFactory(typeof(TContract), factory);
        }

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
        /// <exception cref="System.ArgumentException">
        /// The factory behavior does not implement the contract.
        /// or
        /// The factory is invalid.
        /// </exception>
        public void EnsureFactory(Type factoryContract, IFactory factory)
        {
            if (factoryContract == null)
                throw new ArgumentNullException(nameof(factoryContract), CoreMessages.FactoryContractIsNull);
            if (factory == null)
                throw new ArgumentNullException(nameof(factory), CoreMessages.FactoryBehaviorIsNull);
            if (!factory.GetType().GetTypeInfo().ImplementedInterfaces.Contains(factoryContract))
                throw new ArgumentException(CoreMessages.FactoryNotImplementingContract, nameof(factory));

            Factory actualFactory = factory as Factory;
            if (actualFactory == null)
                throw new ArgumentException(CoreMessages.FactoryIsInvalid, nameof(factory));

            if (this.factoryDictionary.ContainsKey(factoryContract))
                return;

            this.factoryDictionary.Add(factoryContract, actualFactory);
        }

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The factory contract.</typeparam>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        public TContract GetFactory<TContract>()
            where TContract : IFactory
        {
            return (TContract)this.GetFactory(typeof(TContract));
        }

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        public IFactory GetFactory(Type factoryContract)
        {
            if (factoryContract == null)
                throw new ArgumentNullException(nameof(factoryContract), CoreMessages.FactoryContractIsNull);

            return this.GetFactoryInternal(factoryContract, new HashSet<Type>());
        }

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <param name="checkedTypes">The set of checked factory types.</param>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        private IFactory GetFactoryInternal(Type factoryContract, HashSet<Type> checkedTypes)
        {
            if (this.factoryDictionary.ContainsKey(factoryContract))
                return this.factoryDictionary[factoryContract];

            checkedTypes.Add(this.GetType());

            // recursively process all underlying factories
            foreach (IFactory factory in this.factoryDictionary.Values)
            {
                if (checkedTypes.Contains(factory.GetType()))
                    continue;

                Factory factoryImpl = factory as Factory;

                if (factoryImpl != null)
                {
                    IFactory factoryImplementation = factoryImpl.GetFactoryInternal(factoryContract, checkedTypes);

                    if (factoryImplementation != null)
                        return factoryImplementation;
                }
                else
                {
                    IFactory factoryImplementation = factoryImpl.GetFactory(factoryContract);

                    if (factoryImplementation != null)
                        return factoryImplementation;
                }
            }

            return null;
        }
    }
}
