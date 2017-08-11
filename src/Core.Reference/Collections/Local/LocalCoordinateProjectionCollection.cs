// <copyright file="LocalCoordinateProjectionCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Local
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a collection of <see cref="CoordinateProjection" /> instances.
    /// </summary>
    /// <remarks>
    /// This type queries references from local resources, which are specified according to the EPSG geodetic dataset format.
    /// </remarks>
    public class LocalCoordinateProjectionCollection : ICoordinateProjectionCollection
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
        /// The identifier of the default ellipsoid.
        /// </summary>
        private const String DefaultEllipsoidIdentifier = "EPSG::7030";

        /// <summary>
        /// Represents raw data of coordinate projections.
        /// </summary>
        protected class CoordinateProjectionData : IdentifiedObject
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CoordinateProjectionData" /> class.
            /// </summary>
            /// <param name="identifier">The identifier.</param>
            /// <param name="name">The name.</param>
            /// <param name="remarks">The remarks.</param>
            /// <param name="aliases">The aliases.</param>
            /// <param name="method">The coordinate operation method.</param>
            /// <param name="parameters">The parameters of the operation.</param>
            /// <param name="areaOfUse">The area of use.</param>
            public CoordinateProjectionData(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, AreaOfUse areaOfUse)
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
        }

        /// <summary>
        /// Represents a collection of <see cref="CoordinateProjectionData" /> instances.
        /// </summary>
        private class CoordinateProjectionDataCollection : LocalReferenceCollection<CoordinateProjectionData>
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
            /// The collection of <see cref="UnitOfMeasurement" /> instances.
            /// </summary>
            private readonly IReferenceCollection<UnitOfMeasurement> unitCollection;

            /// <summary>
            /// The dictionary containing projection parameters.
            /// </summary>
            private Dictionary<Int32, Dictionary<CoordinateOperationParameter, Object>> projectionParameters;

            /// <summary>
            /// Initializes a new instance of the <see cref="CoordinateProjectionDataCollection" /> class.
            /// </summary>
            /// <param name="areaOfUseCollection">The area of use collection.</param>
            /// <param name="methodCollection">The coordinate operation method collection.</param>
            /// <param name="parameterCollection">The coordinate operation parameter collection.</param>
            /// <param name="unitCollection">The unit of measurement collection.</param>
            public CoordinateProjectionDataCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<CoordinateOperationMethod> methodCollection, IReferenceCollection<CoordinateOperationParameter> parameterCollection, IReferenceCollection<UnitOfMeasurement> unitCollection)
                : base(ResourceName, AliasName)
            {
                this.areaOfUseCollection = areaOfUseCollection;
                this.methodCollection = methodCollection;
                this.parameterCollection = parameterCollection;
                this.unitCollection = unitCollection;
            }

            /// <summary>
            /// Converts the specified content.
            /// </summary>
            /// <param name="content">The content.</param>
            /// <returns>The converted reference.</returns>
            protected override CoordinateProjectionData Convert(String[] content)
            {
                this.EnsureProjectionParameters();

                CoordinateOperationMethod method = !String.IsNullOrEmpty(content[10]) ? this.methodCollection.WithIdentifier(IdentifiedObject.GetIdentifier(Authority, content[10])).FirstOrDefault() : null;
                AreaOfUse areaOfUse = this.areaOfUseCollection.WithIdentifier(IdentifiedObject.GetIdentifier(Authority, content[7])).FirstOrDefault();

                // TODO: remove condition, once all projections are implemented
                if (method == null)
                    return null;

                return new CoordinateProjectionData(IdentifiedObject.GetIdentifier(Authority, content[0]), content[1],
                                                    content[13], this.GetAliases(Int32.Parse(content[0])),
                                                    method,
                                                    this.projectionParameters.ContainsKey(Int32.Parse(content[0])) ? this.projectionParameters[Int32.Parse(content[0])] : null,
                                                    areaOfUse);
            }

            /// <summary>
            /// Ensures that all projection parameters are available.
            /// </summary>
            private void EnsureProjectionParameters()
            {
                if (this.projectionParameters != null)
                    return;

                this.projectionParameters = new Dictionary<Int32, Dictionary<CoordinateOperationParameter, Object>>();

                using (StreamReader reader = new StreamReader(this.OpenResource(ResourceNameParameterValue)))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] content = line.Split(Divider);

                        Int32 code = Int32.Parse(content[0]);

                        if (!this.projectionParameters.ContainsKey(code))
                            this.projectionParameters.Add(code, new Dictionary<CoordinateOperationParameter, Object>());

                        CoordinateOperationParameter parameter = this.parameterCollection.WithIdentifier(IdentifiedObject.GetIdentifier(Authority, content[2])).FirstOrDefault();

                        // TODO: remove condition, once all parameters are implemented
                        if (parameter == null)
                            continue;

                        if (!String.IsNullOrEmpty(content[3]))
                        {
                            // the parameter is a value
                            if (!String.IsNullOrEmpty(content[5]))
                            {
                                // the parameter has a measurement unit

                                UnitOfMeasurement unit = this.unitCollection.WithIdentifier(IdentifiedObject.GetIdentifier(Authority, content[5])).FirstOrDefault();

                                switch (unit.Type)
                                {
                                    case UnitQuantityType.Angle:
                                        this.projectionParameters[code].Add(parameter, new Angle(Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat), unit));
                                        break;
                                    case UnitQuantityType.Length:
                                        this.projectionParameters[code].Add(parameter, new Length(Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat), unit));
                                        break;
                                    case UnitQuantityType.Scale:
                                        this.projectionParameters[code].Add(parameter, Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat));
                                        break;
                                }
                            }
                            else
                            {
                                // the parameter is scalar

                                this.projectionParameters[code].Add(parameter, Double.Parse(content[3], CultureInfo.InvariantCulture.NumberFormat));
                            }
                        }

                        if (!String.IsNullOrEmpty(content[4]))
                        {
                            // the parameter is a path
                            this.projectionParameters[code].Add(parameter, content[4]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The collection of  raw data. This field is read-only.
        /// </summary>
        private readonly CoordinateProjectionDataCollection dataCollection;

        /// <summary>
        /// The collection of  <see cref="Ellipsoid" /> instances. This field is read-only.
        /// </summary>
        private readonly IReferenceCollection<Ellipsoid> ellipsoidCollection;

        /// <summary>
        /// The collection of <see cref="UnitOfMeasurement" /> instances. This field is read-only.
        /// </summary>
        private readonly IReferenceCollection<UnitOfMeasurement> unitCollection;

        /// <summary>
        /// The collection of  projection types.
        /// </summary>
        private Dictionary<Int32, Type> projectionTypes;

        /// <summary>
        /// The default ellipsoid.
        /// </summary>
        private Ellipsoid defaultEllipsoid;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCoordinateProjectionCollection" /> class.
        /// </summary>
        /// <param name="areaOfUseCollection">The area of use collection.</param>
        /// <param name="ellpsoidCollection">The ellipsoid collection.</param>
        /// <param name="methodCollection">The coordinate operation method collection.</param>
        /// <param name="parameterCollection">The coordinate operation parameter collection.</param>
        /// <param name="unitCollection">The unit of measurement collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The area of use collection is null.
        /// or
        /// The ellipsoid collection is null.
        /// or
        /// The coordinate operation method collection is null.
        /// or
        /// The coordinate operation parameter collection is null.
        /// or
        /// The unit of measurement collection is null.
        /// </exception>
        public LocalCoordinateProjectionCollection(IReferenceCollection<AreaOfUse> areaOfUseCollection, IReferenceCollection<Ellipsoid> ellpsoidCollection, IReferenceCollection<CoordinateOperationMethod> methodCollection, IReferenceCollection<CoordinateOperationParameter> parameterCollection, IReferenceCollection<UnitOfMeasurement> unitCollection)
        {
            if (areaOfUseCollection == null)
                throw new ArgumentNullException(nameof(areaOfUseCollection));
            if (methodCollection == null)
                throw new ArgumentNullException(nameof(methodCollection));
            if (parameterCollection == null)
                throw new ArgumentNullException(nameof(parameterCollection));
            if (unitCollection == null)
                throw new ArgumentNullException(nameof(unitCollection));

            this.dataCollection = new CoordinateProjectionDataCollection(areaOfUseCollection, methodCollection, parameterCollection, unitCollection);
            this.ellipsoidCollection = ellpsoidCollection ?? throw new ArgumentNullException(nameof(ellpsoidCollection));
            this.unitCollection = unitCollection;
        }

        /// <summary>
        /// Gets the item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <returns>The instance with the specified authority and code.</returns>
        public CoordinateProjection this[String authority, Int32 code]
        {
            get
            {
                return this[authority, code, this.defaultEllipsoid];
            }
        }

        /// <summary>
        /// Gets the item with the specified authority and code.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The instance with the specified authority and code.</returns>
        public CoordinateProjection this[String authority, Int32 code, Ellipsoid ellipsoid]
        {
            get
            {
                if (authority == null || ellipsoid == null)
                    return null;

                CoordinateProjectionData data = this.dataCollection[authority, code];

                this.EnsureOperationTypes();

                // TODO: remove condition, once all projections are implemented
                if (data == null || !this.projectionTypes.ContainsKey(data.Method.Code))
                    return null;

                return this.CreateProjection(data, ellipsoid);
            }
        }

        /// <summary>
        /// Gets the item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The instance with the specified identifier.</returns>
        public CoordinateProjection this[String identifier]
        {
            get { return this[identifier, this.defaultEllipsoid]; }
        }

        /// <summary>
        /// Gets the item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The instance with the specified identifier.</returns>
        public CoordinateProjection this[String identifier, Ellipsoid ellipsoid]
        {
            get
            {
                if (identifier == null || ellipsoid == null)
                    return null;

                CoordinateProjectionData data = this.dataCollection[identifier];

                this.EnsureOperationTypes();

                // TODO: remove condition, once all projections are implemented
                if (data == null || !this.projectionTypes.ContainsKey(data.Method.Code))
                    return null;

                return this.CreateProjection(data, ellipsoid);
            }
        }

        /// <summary>
        /// Gets the item with the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The instance with the specified identifier.</returns>
        public CoordinateProjection this[CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, AreaOfUse areaOfUse, Ellipsoid ellipsoid]
        {
            get
            {
                if (method == null)
                    return null;
                if (areaOfUse == null)
                    return null;
                if (ellipsoid == null)
                    return null;

                CoordinateProjectionData matchingData = this.dataCollection.Where(data => data != null && this.projectionTypes.ContainsKey(data.Method.Code)).FirstOrDefault(data => data.Method.Equals(method) && (areaOfUse.Equals(AreaOfUse.Undefined) || data.AreaOfUse.Equals(areaOfUse)) && this.IsMatching(data.Parameters, parameters));

                // no matching projection is available
                if (matchingData == null)
                    return null;

                return this.CreateProjection(matchingData, ellipsoid);
            }
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateProjection> WithIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            this.EnsureOperationTypes();

            Ellipsoid ellipsoid = this.ellipsoidCollection.WithIdentifier(DefaultEllipsoidIdentifier).FirstOrDefault();

            return this.dataCollection.WithIdentifier(identifier).Where(data => this.projectionTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateProjection(data, ellipsoid));
        }

        /// <summary>
        /// Returns a collection with items matching the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A collection containing the items that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateProjection> WithName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.EnsureOperationTypes();

            return this.dataCollection.WithName(name).Where(data => this.projectionTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateProjection(data, this.defaultEllipsoid));
        }

        /// <summary>
        /// Returns a collection with items matching the specified identifier expression.
        /// </summary>
        /// <param name="identifier">The regular expression of the identifier.</param>
        /// <returns>A collection containing the items that match the specified identifier expression.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public IEnumerable<CoordinateProjection> WithMatchingIdentifier(String identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            this.EnsureOperationTypes();

            return this.dataCollection.WithMatchingIdentifier(identifier).Where(data => this.projectionTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateProjection(data, this.defaultEllipsoid));
        }

        /// <summary>
        /// Returns a collection with items matching the specified name expression.
        /// </summary>
        /// <param name="name">The regular expression of the name.</param>
        /// <returns>A collection containing the items that match the specified name expression.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public IEnumerable<CoordinateProjection> WithMatchingName(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.EnsureOperationTypes();

            return this.dataCollection.WithName(name).Where(data => this.projectionTypes.ContainsKey(data.Method.Code)).Select(data => this.CreateProjection(data, this.defaultEllipsoid));
        }

        /// <summary>
        /// Returns a collection with items matching the specified properties.
        /// </summary>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The instance with the specified properties.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The area of use is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public IEnumerable<CoordinateProjection> WithProperties(CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, AreaOfUse areaOfUse, Ellipsoid ellipsoid)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            if (areaOfUse == null)
                throw new ArgumentNullException(nameof(areaOfUse));
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid));

            this.EnsureOperationTypes();

            return this.dataCollection.Where(data => data != null && this.projectionTypes.ContainsKey(data.Method.Code)).Where(data => data.Method.Equals(method) && (areaOfUse.Equals(AreaOfUse.Undefined) || data.AreaOfUse.Equals(areaOfUse)) && this.IsMatching(data.Parameters, parameters)).Select(data => this.CreateProjection(data, ellipsoid));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator{ReferenceType}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<CoordinateProjection> GetEnumerator()
        {
            this.EnsureOperationTypes();

            foreach (CoordinateProjectionData data in this.dataCollection)
                yield return this.CreateProjection(data, this.defaultEllipsoid);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.EnsureOperationTypes();

            foreach (CoordinateProjectionData data in this.dataCollection)
                yield return this.CreateProjection(data, this.defaultEllipsoid);
        }

        /// <summary>
        /// Ensures that all operation types are available.
        /// </summary>
        private void EnsureOperationTypes()
        {
            if (this.projectionTypes != null)
                return;

            this.projectionTypes = new Dictionary<Int32, Type>();

            foreach (TypeInfo type in this.GetType().GetTypeInfo().Assembly.DefinedTypes.Where(type => type.IsClass && type.CustomAttributes.Any(attribute => attribute.AttributeType.Equals(typeof(IdentifiedObjectAttribute)) && type.IsSubclassOf(typeof(CoordinateProjection)))))
            {
                this.projectionTypes.Add(IdentifiedObject.GetCode(type.GetCustomAttribute<IdentifiedObjectAttribute>().Identifier), type.AsType());
            }

            this.defaultEllipsoid = this.ellipsoidCollection.WithIdentifier(DefaultEllipsoidIdentifier).FirstOrDefault();
        }

        /// <summary>
        /// Creates a coordinate projection.
        /// </summary>
        /// <param name="data">The raw coordinate projection data.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The produced coordinate projection.</returns>
        private CoordinateProjection CreateProjection(CoordinateProjectionData data, Ellipsoid ellipsoid)
        {
            Object[] arguments = new Object[] { data.Identifier, data.Name, data.Remarks, data.Aliases, data.Parameters, ellipsoid, data.AreaOfUse };

            return Activator.CreateInstance(this.projectionTypes[data.Method.Code], arguments) as CoordinateProjection;
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
            }

            foreach (CoordinateOperationParameter parameter in parameters.Keys)
            {
                if (this.GetDouble(parameters[parameter]) != this.GetDouble(otherParameters[parameter]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Converts the specified value to a double precision floating point number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        private Double GetDouble(Object value)
        {
            if (value is Angle)
                return ((Angle)value).Value;
            if (value is Length)
                return ((Length)value).Value;
            if (value is IConvertible)
                return Convert.ToDouble(value);

            return Double.NaN;
        }
    }
}
