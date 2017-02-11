// <copyright file="LocalCoordinateTransformationCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="CoordinateTransformation{CoordinateType}" /> instances.
    /// </summary>
    /// <typeparam name="CoordinateType">The type of the coordinate.</typeparam>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalCoordinateTransformationCollection<CoordinateType> : ICoordinateTransformationCollection<CoordinateType>
    {
        /// <summary>
        /// The name of the resource. This field is constant.
        /// </summary>
        private const String ResourceName = "CoordinateOperation";

        /// <summary>
        /// The name of the alias type. This field is constant.
        /// </summary>
        private const String AliasName = "Coordinate_Operation";

        /// <summary>
        /// The name of the parameter value resource. This field is constant.
        /// </summary>
        private const String ResourceNameParameterValue = "CoordinateOperationParameterValue";

        /// <summary>
        /// Represents raw data of coordinate transformations.
        /// </summary>
        protected class CoordinateTransformationData : IdentifiedObject
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CoordinateTransformationData" /> class.
            /// </summary>
            /// <param name="identifier">The identifier.</param>
            /// <param name="name">The name.</param>
            /// <param name="remarks">The remarks.</param>
            /// <param name="aliases">The aliases.</param>
            /// <param name="method">The coordinate operation method.</param>
            /// <param name="parameters">The parameters of the operation.</param>
            /// <param name="source">The source coordinate reference system.</param>
            /// <param name="target">The target coordinate reference system.</param>
            /// <param name="areaOfUse">The area of use.</param>
            public CoordinateTransformationData(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
                : base(identifier, name, remarks, aliases)
            {
                this.AreaOfUse = areaOfUse;
                this.Method = method;
                this.Parameters = parameters;
            }

            /// <summary>
            /// Gets the area of use.
            /// </summary>
            /// <value>The area of use.</value>
            public AreaOfUse AreaOfUse { get; private set; }

            /// <summary>
            /// Gets the coordinate operation method.
            /// </summary>
            /// <value>The coordinate operation method.</value>
            public CoordinateOperationMethod Method { get; private set; }

            /// <summary>
            /// Gets the coordinate operation parameters.
            /// </summary>
            /// <value>The coordinate operation parameters.</value>
            public Dictionary<CoordinateOperationParameter, Object> Parameters { get; private set; }

            /// <summary>
            /// Gets the source coordinate reference system.
            /// </summary>
            /// <value>The source coordinate reference system.</value>
            public CoordinateReferenceSystem Collection { get; private set; }

            /// <summary>
            /// Gets the target coordinate reference system.
            /// </summary>
            /// <value>The target coordinate reference system.</value>
            public CoordinateReferenceSystem Target { get; private set; }
        }

        /// <summary>
        /// Represents a collection of <see cref="CoordinateTransformationData" /> instances.
        /// </summary>
        private class CoordinateTransformationDataCollection : LocalReferenceCollection<CoordinateTransformationData>
        {
            /// <summary>
            /// The collection of  <see cref="AreaOfUse" /> instances.
            /// </summary>
            private readonly IReferenceCollection<AreaOfUse> areaOfUseCollection;

            /// <summary>
            /// The collection of  <see cref="CoordinateOperationMethod" /> instances.
            /// </summary>
            private readonly IReferenceCollection<CoordinateOperationMethod> methodCollection;

            /// <summary>
            /// The collection of  <see cref="CoordinateOperationParameter" /> instances.
            /// </summary>
            private readonly IReferenceCollection<CoordinateOperationParameter> parameterCollection;

            /// <summary>
            /// The collection of  <see cref="CoordinateReferenceSystem" /> instances.
            /// </summary>
            private readonly IReferenceCollection<CoordinateReferenceSystem> referenceSystemCollection;

            /// <summary>
            /// The collection of <see cref="UnitOfMeasurement" /> instances.
            /// </summary>
            private readonly IReferenceCollection<UnitOfMeasurement> unitCollection;

            /// <summary>
            /// The dictionary containing projection parameters.
            /// </summary>
            private Dictionary<Int32, Dictionary<CoordinateOperationParameter, Object>> transformationParameters;

            /// <summary>
            /// Initializes a new instance of the <see cref="CoordinateTransformationDataCollection" /> class.
            /// </summary>
            /// <param name="areaOfUseCollection">The area of use collection.</param>
            /// <param name="methodCollection">The coordinate operation method collection.</param>
            /// <param name="parameterCollection">The coordinate operation parameter collection.</param>
            /// <param name="referenceSystemCollection">The coordinate reference system collection.</param>
            /// <param name="unitCollection">The unit of measurement collection.</param>
            public CoordinateTransformationDataCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<CoordinateOperationMethod> methodCollection, IReferenceCollection<CoordinateOperationParameter> parameterCollection, IReferenceCollection<CoordinateReferenceSystem> referenceSystemCollection, IReferenceCollection<UnitOfMeasurement> unitCollection)
                : base(ResourceName, AliasName)
            {
                this.areaOfUseCollection = areaOfUseCollection;
                this.methodCollection = methodCollection;
                this.parameterCollection = parameterCollection;
                this.referenceSystemCollection = referenceSystemCollection;
                this.unitCollection = unitCollection;
            }

            /// <summary>
            /// Converts the specified content.
            /// </summary>
            /// <param name="content">The content.</param>
            /// <returns>The converted reference.</returns>
            protected override CoordinateTransformationData Convert(String[] content)
            {
                this.EnsureTransformationParameters();

                CoordinateOperationMethod method = !String.IsNullOrEmpty(content[10]) ? this.methodCollection[Authority, Int32.Parse(content[10])] : null;
                CoordinateReferenceSystem source = this.referenceSystemCollection[Authority, Int32.Parse(content[3])];
                CoordinateReferenceSystem target = this.referenceSystemCollection[Authority, Int32.Parse(content[4])];

                AreaOfUse areaOfUse = this.areaOfUseCollection[Authority, Int32.Parse(content[7])];

                // TODO: remove condition, once all operations are implemented
                if (method == null)
                    return null;

                return new CoordinateTransformationData(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                        content[13], this.GetAliases(Int32.Parse(content[0])),
                                                        method,
                                                        this.transformationParameters.ContainsKey(Int32.Parse(content[0])) ? this.transformationParameters[Int32.Parse(content[0])] : null,
                                                        source,
                                                        target,
                                                        areaOfUse);
            }

            /// <summary>
            /// Ensures that all transformation parameters are available.
            /// </summary>
            private void EnsureTransformationParameters()
            {
                if (this.transformationParameters != null)
                    return;

                this.transformationParameters = new Dictionary<Int32, Dictionary<CoordinateOperationParameter, Object>>();

                using (StreamReader reader = new StreamReader(this.OpenResource(ResourceNameParameterValue)))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] content = line.Split(Divider);

                        Int32 code = Int32.Parse(content[0]);

                        if (!this.transformationParameters.ContainsKey(code))
                            this.transformationParameters.Add(code, new Dictionary<CoordinateOperationParameter, Object>());

                        CoordinateOperationParameter parameter = this.parameterCollection[Authority, Int32.Parse(content[2])];

                        // TODO: remove condition, once all parameters are implemented
                        if (parameter == null)
                            continue;

                        if (!String.IsNullOrEmpty(content[3]))
                        {
                            // the parameter is a value
                            if (!String.IsNullOrEmpty(content[5]))
                            {
                                // the parameter has a measurement unit

                                UnitOfMeasurement unit = this.unitCollection[Authority, Int32.Parse(content[5])];

                                switch (unit.Type)
                                {
                                    case UnitQuantityType.Angle:
                                        this.transformationParameters[code].Add(parameter, new Angle(Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat), unit));
                                        break;
                                    case UnitQuantityType.Length:
                                        this.transformationParameters[code].Add(parameter, new Length(Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat), unit));
                                        break;
                                    case UnitQuantityType.Scale:
                                        this.transformationParameters[code].Add(parameter, Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat));
                                        break;
                                }
                            }
                            else
                            {
                                // the parameter is scalar

                                this.transformationParameters[code].Add(parameter, Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat));
                            }
                        }

                        if (!String.IsNullOrEmpty(content[4]))
                        {
                            // the parameter is a path
                            this.transformationParameters[code].Add(parameter, content[4]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The collection of  raw data.
        /// </summary>
        private readonly CoordinateTransformationDataCollection dataCollection;

        /// <summary>
        /// The collection of  transformation types.
        /// </summary>
        private Dictionary<Int32, Type> transformationTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCoordinateTransformationCollection{CoordinateType}" /> class.
        /// </summary>
        /// <param name="areaOfUseCollection">The area of use collection.</param>
        /// <param name="methodCollection">The coordinate operation method collection.</param>
        /// <param name="parameterCollection">The coordinate operation parameter collection.</param>
        /// <param name="referenceSystemCollection">The coordinate reference system collection.</param>
        /// <param name="unitCollection">The unit of measurement collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The area of use collection is null.
        /// or
        /// The coordinate operation method collection is null.
        /// or
        /// The coordinate operation parameter collection is null.
        /// or
        /// The reference system collection is null.
        /// or
        /// The unit of measurement collection is null.
        /// </exception>
        public LocalCoordinateTransformationCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<CoordinateOperationMethod> methodCollection, IReferenceCollection<CoordinateOperationParameter> parameterCollection, IReferenceCollection<CoordinateReferenceSystem> referenceSystemCollection, IReferenceCollection<UnitOfMeasurement> unitCollection)
        {
            if (areaOfUseCollection == null)
                throw new ArgumentNullException(nameof(areaOfUseCollection), ReferenceMessages.AreaOfUseCollectionIsNull);
            if (methodCollection == null)
                throw new ArgumentNullException(nameof(methodCollection), ReferenceMessages.MethodCollectionIsNull);
            if (parameterCollection == null)
                throw new ArgumentNullException(nameof(parameterCollection), ReferenceMessages.ParameterCollectionIsNull);
            if (referenceSystemCollection == null)
                throw new ArgumentNullException(nameof(referenceSystemCollection), ReferenceMessages.ReferenceSystemCollectionIsNull);

            this.dataCollection = new CoordinateTransformationDataCollection(areaOfUseCollection, methodCollection, parameterCollection, referenceSystemCollection, unitCollection);
        }

        /// <summary>
        /// Gets the instance with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The instance with the specified authority and code.</returns>
        public CoordinateTransformation<CoordinateType> this[String authority, Int32 code]
        {
            get
            {
                if (authority == null)
                    return null;

                CoordinateTransformationData data = this.dataCollection[authority, code];

                this.EnsureOperationTypes();

                // TODO: remove condition, once all operations are implemented
                if (data == null || !this.transformationTypes.ContainsKey(data.Method.Code))
                    return null;

                return this.CreateTransformation(data);
            }
        }

        /// <summary>
        /// Gets the instance with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The instance with the specified identifier.</returns>
        public CoordinateTransformation<CoordinateType> this[String identifier]
        {
            get
            {
                if (identifier == null)
                    return null;

                CoordinateTransformationData data = this.dataCollection[identifier];

                this.EnsureOperationTypes();

                // TODO: remove condition, once all operations are implemented
                if (data == null || !this.transformationTypes.ContainsKey(data.Method.Code))
                    return null;

                return this.CreateTransformation(data);
            }
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateTransformation<CoordinateType>> WithIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            this.EnsureOperationTypes();

            return this.dataCollection.WithIdentifier(identifier).Where(data => this.transformationTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateTransformation(data));
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateTransformation<CoordinateType>> WithName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            this.EnsureOperationTypes();

            return this.dataCollection.WithName(name).Where(data => this.transformationTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateTransformation(data));
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateTransformation<CoordinateType>> WithMatchingIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier), ReferenceMessages.IdentifierIsNull);

            this.EnsureOperationTypes();

            return this.dataCollection.WithMatchingIdentifier(identifier).Where(data => this.transformationTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateTransformation(data));
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateTransformation<CoordinateType>> WithMatchingName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), ReferenceMessages.NameIsNull);

            this.EnsureOperationTypes();

            return this.dataCollection.WithMatchingName(name).Where(data => this.transformationTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateTransformation(data));
        }

        /// <summary>
        /// Returns the item with the specified properties.
        /// </summary>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <returns>The instance with the specified properties.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// </exception>
        public IEnumerable<CoordinateTransformation<CoordinateType>> WithProperties(CoordinateReferenceSystem source, CoordinateReferenceSystem target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), ReferenceMessages.SourceCoordinateReferenceSystemIsNull);
            if (target == null)
                throw new ArgumentNullException(nameof(target), ReferenceMessages.TargetCoordinateReferenceSystemIsNull);

            this.EnsureOperationTypes();

            return this.dataCollection.Where(data => this.transformationTypes.ContainsKey(data.Method.Code)).Where(data => data.Collection.Equals(source) && data.Target.Equals(target)).Select(data => this.CreateTransformation(data));
        }

        /// <summary>
        /// Returns the item with the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <returns>The instance with the specified method and parameters.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public IEnumerable<CoordinateTransformation<CoordinateType>> WithProperties(CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, AreaOfUse areaOfUse)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method), ReferenceMessages.MethodIsNull);
            if (areaOfUse == null)
                throw new ArgumentNullException(nameof(areaOfUse), ReferenceMessages.AreaOfUseIsNull);

            this.EnsureOperationTypes();

            return this.dataCollection.Where(data => this.transformationTypes.ContainsKey(data.Method.Code)).Where(data => data.Method.Equals(method) && data.AreaOfUse.Equals(areaOfUse) && this.IsMatching(data.Parameters, parameters)).Select(data => this.CreateTransformation(data));
        }

        /// <summary>
        /// Returns the item with the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <returns>The instance with the specified properties.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public IEnumerable<CoordinateTransformation<CoordinateType>> WithProperties(CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method), ReferenceMessages.MethodIsNull);
            if (source == null)
                throw new ArgumentNullException(nameof(source), ReferenceMessages.SourceCoordinateReferenceSystemIsNull);
            if (target == null)
                throw new ArgumentNullException(nameof(target), ReferenceMessages.TargetCoordinateReferenceSystemIsNull);
            if (areaOfUse == null)
                throw new ArgumentNullException(nameof(areaOfUse), ReferenceMessages.AreaOfUseIsNull);

            this.EnsureOperationTypes();

            return this.dataCollection.Where(data => this.transformationTypes.ContainsKey(data.Method.Code)).Where(data => data.Method.Equals(method) && data.Collection.Equals(source) && data.Target.Equals(target) && data.AreaOfUse.Equals(areaOfUse) && this.IsMatching(data.Parameters, parameters)).Select(data => this.CreateTransformation(data));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{ReferenceType}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<CoordinateTransformation<CoordinateType>> GetEnumerator()
        {
            this.EnsureOperationTypes();

            foreach (CoordinateTransformationData data in this.dataCollection)
                yield return this.CreateTransformation(data);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.EnsureOperationTypes();

            foreach (CoordinateTransformationData data in this.dataCollection)
                yield return this.CreateTransformation(data);
        }

        /// <summary>
        /// Ensures that all operation types are available.
        /// </summary>
        private void EnsureOperationTypes()
        {
            if (this.transformationTypes != null)
                return;

            this.transformationTypes = new Dictionary<Int32, Type>();

            foreach (TypeInfo type in this.GetType().GetTypeInfo().Assembly.DefinedTypes.Where(type => type.IsClass && type.CustomAttributes.Any(attribute => attribute.AttributeType.Equals(typeof(IdentifiedObjectAttribute)) && type.IsSubclassOf(typeof(CoordinateTransformation<CoordinateType>)))))
            {
                this.transformationTypes.Add(IdentifiedObject.GetCode(type.GetCustomAttribute<IdentifiedObjectAttribute>().Identifier), type.AsType());
            }
        }

        /// <summary>
        /// Creates a coordinate transformation.
        /// </summary>
        /// <param name="data">The raw coordinate transformation data.</param>
        /// <returns>The produced coordinate projection.</returns>
        private CoordinateTransformation<CoordinateType> CreateTransformation(CoordinateTransformationData data)
        {
            Object[] arguments = new Object[] { data.Identifier, data.Name, data.Remarks, data.Aliases, data.Parameters, data.Collection, data.Target, data.AreaOfUse };

            return Activator.CreateInstance(this.transformationTypes[data.Method.Code], arguments) as CoordinateTransformation<CoordinateType>;
        }

        /// <summary>
        /// Determines whether the specified parameters are matching.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="otherParameters">The other parameters.</param>
        /// <returns><c>true</c> if all parameters match; otherwise, <c>false</c>.</returns>
        private Boolean IsMatching(IDictionary<CoordinateOperationParameter, Object> parameters, IDictionary<CoordinateOperationParameter, Object> otherParameters)
        {
            if (parameters == null && otherParameters == null)
                return true;
            if (parameters == null || otherParameters == null)
                return false;
            if (parameters.Count != otherParameters.Count)
                return false;

            foreach (CoordinateOperationParameter parameter in parameters.Keys)
            {
                if (!otherParameters.ContainsKey(parameter))
                    return false;

                if (parameters[parameter].Equals(otherParameters[parameter]))
                    return false;
            }

            return true;
        }
    }
}
