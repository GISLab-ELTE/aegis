// <copyright file="StoredGeometryCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Storage.Geometries
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a geometry collection located in a store.
    /// </summary>
    /// <typeparam name="GeometryType">The type of the geometry.</typeparam>
    public class StoredGeometryCollection<GeometryType> : StoredGeometry, IGeometryCollection<GeometryType>
        where GeometryType : IGeometry
    {
        /// <summary>
        /// The divider for items. This field is constant.
        /// </summary>
        private const String ItemStringDivider = ",";

        /// <summary>
        /// The string for empty collections. This field is constant.
        /// </summary>
        private const String CollectionEmptyString = " EMPTY";

        /// <summary>
        /// The string format for collections. This field is constant.
        /// </summary>
        private const String CollectionStringFormat = " ({0})";

        /// <summary>
        /// The name of the geometry collections. This field is constant.
        /// </summary>
        private const String GeometryCollectionName = "GEOMETRYCOLLECTION";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometryCollection{GeometryType}" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredGeometryCollection(PrecisionModel precisionModel, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, driver, identifier, indexes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredGeometryCollection{GeometryType}" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredGeometryCollection(StoredGeometryFactory factory, String identifier, IEnumerable<Int32> indexes)
            : base(factory, identifier, indexes)
        {
        }

        /// <summary>
        /// Gets the inherent dimension of the geometry list.
        /// </summary>
        /// <value>The maximum inherent dimension of all geometries within the collection.</value>
        public override Int32 Dimension
        {
            get
            {
                Int32 count = this.ReadCollectionCount();
                return count == 0 ? 0 : Enumerable.Range(0, count).Max(index => this.ReadGeometry(index).Dimension);
            }
        }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public override IGeometry Boundary
        {
            get
            {
                Int32 count = this.ReadCollectionCount();
                if (count == 0)
                    return null;

                if (count == 1)
                    return this.ReadGeometry(0).Boundary;

                List<IGeometry> boundaryList = new List<IGeometry>();

                for (Int32 index = 0; index < count; index++)
                {
                    IGeometry geometry = this.ReadGeometry(index);

                    if (geometry != null && !(geometry is IPoint))
                    {
                        // check whether the boundary contains multiple parts (e.g. polygon)
                        IGeometry boundary = geometry.Boundary;
                        if (boundary is IEnumerable<IGeometry>)
                        {
                            // only the parts should be added to the boundary (so that it is not recursive)
                            foreach (IGeometry boundaryPart in boundary as IEnumerable<IGeometry>)
                                boundaryList.Add(boundaryPart);
                        }
                        else
                        {
                            boundaryList.Add(boundary);
                        }
                    }
                }

                if (boundaryList.Count == 0)
                    return null;
                if (boundaryList.Count == 1)
                    return boundaryList[0];

                return this.Factory.CreateGeometryCollection(boundaryList);
            }
        }

        /// <summary>
        /// Gets the centroid of the geometry list.
        /// </summary>
        /// <value>The centroid of the geometry list.</value>
        public override Coordinate Centroid
        {
            get
            {
                Int32 count = this.ReadCollectionCount();
                if (count == 0)
                    return Coordinate.Undefined;

                if (count == 1)
                    return this.ReadGeometry(0).Centroid;

                return this.PrecisionModel.MakePrecise(Coordinate.Centroid(Enumerable.Range(0, count).Select(index => this.ReadGeometry(index).Centroid)));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty
        {
            get
            {
                Int32 count = this.ReadCollectionCount();
                return count == 0 || Enumerable.Range(0, count).All(index => this.ReadGeometry(index).IsEmpty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple
        {
            get
            {
                Int32 count = this.ReadCollectionCount();
                return count == 0 || Enumerable.Range(0, count).All(index => this.ReadGeometry(index).IsSimple);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get
            {
                Int32 count = this.ReadCollectionCount();
                return count == 0 || Enumerable.Range(0, count).All(index => this.ReadGeometry(index).IsValid);
            }
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <returns>The number of elements in the collection.</returns>
        public virtual Int32 Count { get { return this.ReadCollectionCount(); } }

        /// <summary>
        /// Gets or sets the geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to get.</param>
        /// <returns>The geometry located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        IGeometry IGeometryCollection.this[Int32 index]
        {
            get
            {
                return this.ReadGeometry(index);
            }
        }

        /// <summary>
        /// Gets or sets the geometry at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the geometry to get or set.</param>
        /// <returns>The geometry located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of geometries.
        /// </exception>
        public virtual GeometryType this[Int32 index]
        {
            get
            {
                return (GeometryType)this.ReadGeometry(index);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the geometry list.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{T}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<GeometryType> GetEnumerator()
        {
            Int32 count = this.ReadCollectionCount();
            for (Int32 index = 0; index < count; index++)
                yield return (GeometryType)this.ReadGeometry(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the geometry list.
        /// </summary>
        /// <returns>A <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, GeometryCollectionName);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="name">The name of the geometry.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        protected String ToString(IFormatProvider provider, String name)
        {
            if (!this.Driver.SupportedOperations.Contains(DriverOperation.Read))
                return name;

            if (this.IsEmpty)
                return name + CollectionEmptyString;

            StringBuilder builder = new StringBuilder();
            for (Int32 index = 0; index < this.Count; index++)
            {
                if (index > 0)
                    builder.Append(ItemStringDivider);

                builder.Append(this.ReadCoordinate(index).ToString(provider));
            }

            return name + String.Format(provider, CollectionStringFormat, builder.ToString());
        }
    }
}
