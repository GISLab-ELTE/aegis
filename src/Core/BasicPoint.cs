// <copyright file="BasicPoint.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Globalization;
    using Resources;

    /// <summary>
    /// Represents a basic point geometry in spatial coordinate space.
    /// </summary>
    public class BasicPoint : IBasicPoint
    {
        /// <summary>
        /// The string format for points. This field is constant.
        /// </summary>
        private const String PointStringFormat = "POINT ({0} {1} {2})";

        /// <summary>
        /// The envelope of the point.
        /// </summary>
        private Envelope envelope;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicPoint" /> class.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public BasicPoint(Coordinate coordinate)
        {
            this.Coordinate = coordinate ?? throw new ArgumentNullException(nameof(coordinate));
        }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>The inherent dimension of the geometry.</value>
        public Int32 Dimension { get { return 0; } }

        /// <summary>
        /// Gets the minimum bounding <see cref="Envelope" /> of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public Envelope Envelope
        {
            get { return this.envelope ?? (this.envelope = Envelope.FromCoordinates(this.Coordinate)); }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty { get { return this.Coordinate.IsEmpty; } }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return this.Coordinate.IsValid; } }

        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        public Double X { get { return this.Coordinate.X; } }

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        public Double Y { get { return this.Coordinate.Y; } }

        /// <summary>
        /// Gets the Z coordinate.
        /// </summary>
        /// <value>The Z coordinate.</value>
        public Double Z { get { return this.Coordinate.Z; } }

        /// <summary>
        /// Gets the coordinate associated with the point.
        /// </summary>
        /// <value>The coordinate associated with the point.</value>
        public Coordinate Coordinate { get; private set; }

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
            return String.Format(provider, PointStringFormat, this.Coordinate.X, this.Coordinate.Y, this.Coordinate.Z);
        }
    }
}
