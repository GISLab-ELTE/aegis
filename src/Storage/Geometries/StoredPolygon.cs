// <copyright file="StoredPolygon.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Storage.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AEGIS.Algorithms;
    using AEGIS.Collections;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a polygon geometry in Cartesian coordinate space.
    /// </summary>
    public class StoredPolygon : StoredSurface, IPolygon
    {
        /// <summary>
        /// The string format for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringFormat = "{0} {1} {2}";

        /// <summary>
        /// The divider for coordinates. This field is constant.
        /// </summary>
        private const String CoordinateStringDivider = ",";

        /// <summary>
        /// The string format for line strings. This field is constant.
        /// </summary>
        private const String LineStringStringFormat = "({0})";

        /// <summary>
        /// The divider for line strings. This field is constant.
        /// </summary>
        private const String LineStringStringDivider = ",";

        /// <summary>
        /// The string format for polygons. This field is constant.
        /// </summary>
        private const String PolygonStringFormat = " ({0})";

        /// <summary>
        /// The name of the polygon. This field is constant.
        /// </summary>
        private const String PolygonName = "POLYGON";

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredPolygon" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="driver">The geometry driver.</param>
        /// <param name="identifier">The feature identifier.</param>
        /// <param name="indexes">The indexes of the geometry within the feature.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The driver is null.
        /// or
        /// The identifier is null.
        /// </exception>
        public StoredPolygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IGeometryDriver driver, String identifier, IEnumerable<Int32> indexes)
            : base(precisionModel, referenceSystem, driver, identifier, indexes)
        {
        }

        /// <summary>
        /// Gets the bounding geometry.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public override IGeometry Boundary
        {
            get
            {
                return new StoredMultiLineString(this.PrecisionModel, this.ReferenceSystem, this.Driver, this.Identifier, this.Indexes);
            }
        }

        /// <summary>
        /// Gets the centroid of the polygon.
        /// </summary>
        /// <value>The centroid of the polygon.</value>
        public override Coordinate Centroid
        {
            get
            {
                if (this.HoleCount > 0)
                    return this.PrecisionModel.MakePrecise(PolygonCentroidAlgorithm.ComputeCentroid(this.ReadCoordinates(0), Enumerable.Range(1, this.HoleCount).Select(holeIndex => this.ReadCoordinates(holeIndex))));
                else
                    return this.PrecisionModel.MakePrecise(PolygonCentroidAlgorithm.ComputeCentroid(this.ReadCoordinates(0)));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the polygon is empty.
        /// </summary>
        /// <value><c>true</c> if the polygon is considered to be empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty { get { return this.Shell.Count == 0 && (this.Holes == null || this.Holes.Count == 0); } }

        /// <summary>
        /// Gets a value indicating whether the polygon is simple.
        /// </summary>
        /// <value><c>true</c>, as a polygon is always considered to be simple.</value>
        public override Boolean IsSimple { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the polygon is valid.
        /// </summary>
        /// <value><c>true</c> if the polygon is considered to be valid; otherwise, <c>false</c>.</value>
        public override Boolean IsValid { get { return PolygonAlgorithms.IsValid(this); } }

        /// <summary>
        /// Gets a value indicating whether the polygon is convex.
        /// </summary>
        /// <value><c>true</c> if the polygon is convex; otherwise, <c>false</c>.</value>
        public override Boolean IsConvex { get { return this.HoleCount == 0 && PolygonAlgorithms.IsConvex(this.ReadCoordinates(0)); } }

        /// <summary>
        /// Gets a value indicating whether the polygon is divided.
        /// </summary>
        /// <value><c>true</c>, as a polygon is never divided.</value>
        public override sealed Boolean IsDivided { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether the polygon is whole.
        /// </summary>
        /// <value><c>true</c> if the polygon contains no holes; otherwise, <c>false</c>.</value>
        public override Boolean IsWhole { get { return this.HoleCount == 0; } }

        /// <summary>
        /// Gets the area of the polygon.
        /// </summary>
        /// <value>The area of the surface.</value>
        public override Double Area
        {
            get
            {
                return PolygonAlgorithms.Area(this.ReadCoordinates(0)) + Enumerable.Range(1, this.HoleCount).Sum(holeIndex => -PolygonAlgorithms.Area(this.ReadCoordinates(holeIndex)));
            }
        }

        /// <summary>
        /// Gets the perimeter of the polygon.
        /// </summary>
        /// <value>The perimeter of the surface.</value>
        public override Double Perimeter
        {
            get
            {
                return LineAlgorithms.Length(this.ReadCoordinates(0)) + Enumerable.Range(1, this.HoleCount).Sum(holeIndex => LineAlgorithms.Length(this.ReadCoordinates(holeIndex)));
            }
        }

        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The <see cref="IBasicLineString" /> representing the shell of the polygon.</value>
        IBasicLineString IBasicPolygon.Shell
        {
            get { return this.Shell; }
        }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The <see cref="IList{IBasicCurve}" /> containing the holes of the polygon.</value>
        IReadOnlyList<IBasicLineString> IBasicPolygon.Holes
        {
            get { return this.Holes; }
        }

        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The <see cref="ILinearRing" /> representing the shell of the polygon.</value>
        public ILinearRing Shell { get { return new StoredLinearRing(this.PrecisionModel, this.ReferenceSystem, this.Driver, this.Identifier, this.Indexes.Take()); } }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the polygon.</value>
        public Int32 HoleCount { get { return this.ReadCollectionCount() - 1; } }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The <see cref="IList{LinearRing}" /> containing the holes of the polygon.</value>
        public IReadOnlyList<ILinearRing> Holes { get { return Enumerable.Range(1, this.HoleCount).Select(holeIndex => (this.Factory as StoredGeometryFactory).CreateLinearRing(this.Identifier, holeIndex)).ToList(); } }

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        IBasicLineString IBasicPolygon.GetHole(Int32 index)
        {
            return this.GetHole(index);
        }

        /// <summary>
        /// Add a hole to the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the hole does not match the reference system of the polygon.</exception>
        public virtual void AddHole(ILinearRing hole)
        {
            if (hole == null)
                throw new ArgumentNullException(nameof(hole), CoreMessages.HoleIsNull);

            this.CreateCoordinates(this.PrecisionModel.MakePrecise(hole), this.HoleCount + 1);
        }

        /// <summary>
        /// Add a hole to the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        public virtual void AddHole(IEnumerable<Coordinate> hole)
        {
            if (hole == null)
                throw new ArgumentNullException(nameof(hole), CoreMessages.HoleIsNull);

            this.CreateCoordinates(this.PrecisionModel.MakePrecise(hole).ToArray(), this.HoleCount + 1);
        }

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">There are no holes in the polygon.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// Index is equal to or greater than the number of holes.
        /// </exception>
        public virtual ILinearRing GetHole(Int32 index)
        {
            if (this.HoleCount == 0)
                throw new InvalidOperationException(CoreMessages.NoHolesInPolygon);
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsLessThan0);
            if (index >= this.HoleCount)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanHoleCount);

            return (this.Factory as StoredGeometryFactory).CreateLinearRing(this.Identifier, this.Indexes.Append(index + 1));
        }

        /// <summary>
        /// Removes a hole from the polygon.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the polygon contains the <paramref name="hole" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The hole is null.</exception>
        /// <exception cref="System.ArgumentException">The reference system of the hole does not match the reference system of the polygon.</exception>
        public virtual Boolean RemoveHole(ILinearRing hole)
        {
            if (hole == null)
                throw new ArgumentNullException(nameof(hole), CoreMessages.HoleIsNull);

            if (this.HoleCount == 0)
                return false;

            for (Int32 holeIndex = 0; holeIndex < this.HoleCount; holeIndex++)
            {
                if (this.Holes[holeIndex].Equals(hole))
                {
                    this.DeleteCoordinates(holeIndex + 1);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the hole at the specified index of the polygon.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Index is less than 0.
        /// or
        /// The index is greater than or equal to the number of holes.
        /// </exception>
        public virtual void RemoveHoleAt(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsLessThan0);
            if (index >= this.HoleCount)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanHoleCount);

            this.DeleteCoordinates(index + 1);
        }

        /// <summary>
        /// Removes all holes from the polygon.
        /// </summary>
        public virtual void ClearHoles()
        {
            for (Int32 holeIndex = this.HoleCount; holeIndex >= 0; holeIndex--)
                this.DeleteCoordinates(holeIndex + 1);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString(IFormatProvider provider)
        {
            return this.ToString(provider, PolygonName);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="name">The name of the polygon.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        protected String ToString(IFormatProvider provider, String name)
        {
            if (!this.Driver.SupportedOperations.Contains(DriverOperation.Read))
                return name;

            StringBuilder builder = new StringBuilder();

            StringBuilder partBuilder = new StringBuilder();
            for (Int32 index = 0; index < this.Shell.Count; index++)
            {
                if (index > 0)
                    partBuilder.Append(CoordinateStringDivider);

                partBuilder.Append(String.Format(provider, CoordinateStringFormat, this.Shell[index].X, this.Shell[index].Y, this.Shell[index].Z));
            }

            builder.Append(String.Format(provider, LineStringStringFormat, partBuilder.ToString()));

            for (Int32 holeIndex = 0; holeIndex < this.HoleCount; holeIndex++)
            {
                builder.Append(LineStringStringDivider);

                partBuilder.Clear();
                for (Int32 index = 0; index < this.Holes[holeIndex].Count; index++)
                {
                    if (index > 0)
                        partBuilder.Append(CoordinateStringDivider);

                    partBuilder.Append(String.Format(provider, CoordinateStringFormat, this.Holes[holeIndex][index].X, this.Holes[holeIndex][index].Y, this.Holes[holeIndex][index].Z));
                }

                builder.Append(String.Format(provider, LineStringStringFormat, partBuilder.ToString()));
            }

            return name + String.Format(provider, PolygonStringFormat, builder.ToString());
        }
    }
}
