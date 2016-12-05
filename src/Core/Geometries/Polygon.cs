// <copyright file="Polygon.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Geometries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELTE.AEGIS.Algorithms;
    using ELTE.AEGIS.Resources;

    /// <summary>
    /// Represents a polygon geometry in Cartesian coordinate space.
    /// </summary>
    public class Polygon : Surface, IPolygon
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
        /// The holes of the polygon.
        /// </summary>
        private readonly List<ILinearRing> holes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        public Polygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem)
            : base(precisionModel, referenceSystem)
        {
            this.Shell = this.Factory.CreateLinearRing();
            this.holes = new List<ILinearRing>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The collection of holes.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public Polygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem, ILinearRing shell, IEnumerable<ILinearRing> holes)
            : base(precisionModel, referenceSystem)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            // initialize shell
            if (shell.Factory.Equals(this.Factory))
                this.Shell = shell;
            else
                this.Shell = this.Factory.CreateLinearRing(shell);

            // initialize holes
            this.holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (ILinearRing hole in holes)
                {
                    if (hole == null)
                        continue;

                    if (hole.Factory.Equals(this.Factory))
                        this.holes.Add(hole);
                    else
                        this.holes.Add(this.Factory.CreateLinearRing(hole));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">The shell is empty.</exception>
        public Polygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
            : base(precisionModel, referenceSystem)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            // initialize shell
            this.Shell = this.Factory.CreateLinearRing(shell);

            // initialize holes
            this.holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> coordinates in holes)
                {
                    if (coordinates == null)
                        continue;

                    this.holes.Add(this.Factory.CreateLinearRing(coordinates));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="factory">The factory of the polygon.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public Polygon(IGeometryFactory factory)
            : base(factory)
        {
            this.Shell = this.Factory.CreateLinearRing();
            this.holes = new List<ILinearRing>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="factory">The factory of the polygon.</param>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The collection of holes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The factory is null.
        /// </exception>
        public Polygon(IGeometryFactory factory, ILinearRing shell, IEnumerable<ILinearRing> holes)
            : base(factory)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            // initialize shell
            if (shell.Factory.Equals(this.Factory))
                this.Shell = shell;
            else
                this.Shell = this.Factory.CreateLinearRing(shell);

            // initialize holes
            this.holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (ILinearRing hole in holes)
                {
                    if (hole == null)
                        continue;

                    if (hole.Factory.Equals(this.Factory))
                        this.holes.Add(hole);
                    else
                        this.holes.Add(this.Factory.CreateLinearRing(hole));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon" /> class.
        /// </summary>
        /// <param name="factory">The factory of the polygon.</param>
        /// <param name="shell">The coordinates of the shell.</param>
        /// <param name="holes">The coordinates of the holes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell is null.
        /// or
        /// The factory is null.
        /// </exception>
        public Polygon(IGeometryFactory factory, IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
            : base(factory)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell), CoreMessages.ShellIsNull);

            // initialize shell
            this.Shell = this.Factory.CreateLinearRing(shell);

            // initialize holes
            this.holes = new List<ILinearRing>();
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> coordinates in holes)
                {
                    if (coordinates == null)
                        continue;

                    this.holes.Add(this.Factory.CreateLinearRing(coordinates));
                }
            }
        }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding envelope of the geometry.</value>
        public override Envelope Envelope
        {
            get
            {
                return this.Shell.Envelope;
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
                List<ILinearRing> boundary = new List<ILinearRing>() { this.Factory.CreateLinearRing(this.Shell) };
                boundary.AddRange(this.holes.Select(hole => this.Factory.CreateLinearRing(hole)));

                return this.Factory.CreateMultiLineString(boundary);
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
                if (this.holes.Count > 0)
                    return this.PrecisionModel.MakePrecise(PolygonCentroidAlgorithm.ComputeCentroid(this.Shell, this.holes.Select(shell => shell)));
                else
                    return this.PrecisionModel.MakePrecise(PolygonCentroidAlgorithm.ComputeCentroid(this.Shell));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the polygon is empty.
        /// </summary>
        /// <value><c>true</c> if the polygon is considered to be empty; otherwise, <c>false</c>.</value>
        public override Boolean IsEmpty { get { return this.Shell.Count == 0 && (this.holes == null || this.holes.Count == 0); } }

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
        public override Boolean IsConvex { get { return this.holes.Count == 0 && PolygonAlgorithms.IsConvex(this.Shell); } }

        /// <summary>
        /// Gets a value indicating whether the polygon is divided.
        /// </summary>
        /// <value><c>true</c>, as a polygon is never divided.</value>
        public override sealed Boolean IsDivided { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether the polygon is whole.
        /// </summary>
        /// <value><c>true</c> if the polygon contains no holes; otherwise, <c>false</c>.</value>
        public override Boolean IsWhole { get { return this.holes.Count == 0; } }

        /// <summary>
        /// Gets the area of the polygon.
        /// </summary>
        /// <value>The area of the surface.</value>
        public override Double Area
        {
            get
            {
                return PolygonAlgorithms.Area(this.Shell) + this.holes.Sum(hole => -PolygonAlgorithms.Area(hole));
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
                return this.Shell.Length + this.holes.Sum(hole => hole.Length);
            }
        }

        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The linear ring representing the shell of the polygon.</value>
        IBasicLineString IBasicPolygon.Shell
        {
            get { return this.Shell; }
        }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The read-only list containing the holes of the polygon.</value>
        IReadOnlyList<IBasicLineString> IBasicPolygon.Holes
        {
            get { return this.Holes.Cast<IBasicLineString>().ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Gets the shell of the polygon.
        /// </summary>
        /// <value>The line string representing the shell of the polygon.</value>
        public ILinearRing Shell { get; private set; }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the polygon.</value>
        public Int32 HoleCount { get { return this.holes.Count; } }

        /// <summary>
        /// Gets the holes of the polygon.
        /// </summary>
        /// <value>The read-only list containing the holes of the polygon.</value>
        public IReadOnlyList<ILinearRing> Holes { get { return this.holes; } }

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

            if (hole.Factory.Equals(this.Factory))
                this.holes.Add(hole);
            else
                this.holes.Add(this.Factory.CreateLinearRing(hole));
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

            this.holes.Add(this.Factory.CreateLinearRing(hole.Select(coordinate => this.PrecisionModel.MakePrecise(coordinate))));
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
            if (this.holes.Count == 0)
                throw new InvalidOperationException(CoreMessages.NoHolesInPolygon);
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsLessThan0);
            if (index >= this.holes.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanHoleCount);

            return this.holes[index];
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

            if (this.holes.Count == 0)
                return false;

            for (Int32 holeIndex = 0; holeIndex < this.holes.Count; holeIndex++)
            {
                if (this.holes[holeIndex].Equals(hole))
                {
                    this.holes.RemoveAt(holeIndex);

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
            if (index >= this.holes.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanHoleCount);

            this.holes.RemoveAt(index);
        }

        /// <summary>
        /// Removes all holes from the polygon.
        /// </summary>
        public virtual void ClearHoles()
        {
            this.holes.Clear();
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
            StringBuilder builder = new StringBuilder();

            StringBuilder partBuilder = new StringBuilder();
            for (Int32 index = 0; index < this.Shell.Count; index++)
            {
                if (index > 0)
                    partBuilder.Append(CoordinateStringDivider);

                partBuilder.Append(String.Format(provider, CoordinateStringFormat, this.Shell[index].X, this.Shell[index].Y, this.Shell[index].Z));
            }

            builder.Append(String.Format(provider, LineStringStringFormat, partBuilder.ToString()));

            for (Int32 holeIndex = 0; holeIndex < this.holes.Count; holeIndex++)
            {
                builder.Append(LineStringStringDivider);

                partBuilder.Clear();
                for (Int32 index = 0; index < this.holes[holeIndex].Count; index++)
                {
                    if (index > 0)
                        partBuilder.Append(CoordinateStringDivider);

                    partBuilder.Append(String.Format(provider, CoordinateStringFormat, this.holes[holeIndex][index].X, this.holes[holeIndex][index].Y, this.holes[holeIndex][index].Z));
                }

                builder.Append(String.Format(provider, LineStringStringFormat, partBuilder.ToString()));
            }

            return name + String.Format(provider, PolygonStringFormat, builder.ToString());
        }
    }
}
