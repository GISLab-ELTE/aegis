// <copyright file="BasicProxyPolygon.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using AEGIS.Algorithms;
    using AEGIS.Collections.Resources;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a basic polygon in spatial coordinate space that proxies existing coordinate collections.
    /// </summary>
    public class BasicProxyPolygon : IBasicPolygon
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
        private const String PolygonStringFormat = "POLYGON ({0})";

        /// <summary>
        /// The list of holes.
        /// </summary>
        private List<IBasicLineString> holes;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicProxyPolygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates representing the polygon shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public BasicProxyPolygon(IReadOnlyList<Coordinate> shell)
            : this(shell, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicProxyPolygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates representing the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public BasicProxyPolygon(IReadOnlyList<Coordinate> shell, IEnumerable<IReadOnlyList<Coordinate>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException(nameof(shell));

            this.Shell = new BasicProxyLineString(shell, true);

            this.holes = new List<IBasicLineString>();
            if (holes != null)
            {
                foreach (IReadOnlyList<Coordinate> hole in holes)
                {
                    if (hole != null)
                    {
                        this.holes.Add(new BasicProxyLineString(hole, true));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the inherent dimension of the surface.
        /// </summary>
        /// <value><c>2</c>, which is the defined dimension of a surface.</value>
        public Int32 Dimension { get { return 2; } }

        /// <summary>
        /// Gets the minimum bounding <see cref="Envelope" /> of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public Envelope Envelope { get { return this.Shell.Envelope; } }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return this.Shell.IsEmpty && (this.Holes.Count == 0 || this.Holes.All(hole => hole.IsEmpty)); } }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return PolygonAlgorithms.IsValid(this); } }

        /// <summary>
        /// Gets the shell of the clip.
        /// </summary>
        /// <value>The line string representing the shell of the polygon.</value>
        public IBasicLineString Shell { get; private set; }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the polygon.</value>
        public Int32 HoleCount { get { return this.Holes.Count; } }

        /// <summary>
        /// Gets the holes of the clip.
        /// </summary>
        /// <value>The read-only list containing the holes of the polygon.</value>
        public IReadOnlyList<IBasicLineString> Holes { get { return this.holes; } }

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
        public IBasicLineString GetHole(Int32 index)
        {
            if (this.holes.Count == 0)
                throw new InvalidOperationException(CoreMessages.NoHolesInPolygon);
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), CollectionMessages.IndexIsLessThan0);
            if (index >= this.holes.Count)
                throw new ArgumentOutOfRangeException(nameof(index), CoreMessages.IndexIsEqualToOrGreaterThanHoleCount);

            return this.holes[index];
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return this.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public String ToString(IFormatProvider provider)
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
                for (Int32 index = 0; index < this.Shell.Count; index++)
                {
                    if (index > 0)
                        partBuilder.Append(CoordinateStringDivider);

                    partBuilder.Append(String.Format(provider, CoordinateStringFormat, this.Holes[holeIndex][index].X, this.Holes[holeIndex][index].Y, this.Holes[holeIndex][index].Z));
                }

                builder.Append(String.Format(provider, LineStringStringFormat, partBuilder.ToString()));
            }

            return String.Format(provider, PolygonStringFormat, builder.ToString());
        }
    }
}
