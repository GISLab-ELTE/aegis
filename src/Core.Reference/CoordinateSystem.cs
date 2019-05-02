// <copyright file="CoordinateSystem.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright 2016-2019 Roberto Giachetta. Licensed under the
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

namespace AEGIS.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a coordinate system.
    /// </summary>
    /// <remarks>
    /// A coordinate system (CS) is the non-repeating sequence of coordinate system axes that spans a given coordinate space.
    /// A CS is derived from a set of mathematical rules for specifying how coordinates in a given space are to be assigned to points.
    /// The coordinate values in a coordinate tuple shall be recorded in the order in which the coordinate system axes associations are recorded.
    /// </remarks>
    public class CoordinateSystem : IdentifiedObject
    {
        /// <summary>
        /// The array of coordinate system axes.
        /// </summary>
        private readonly CoordinateSystemAxis[] axes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="axes">The axes of the coordinate system.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">No axis is specified for the coordinate system.</exception>
        public CoordinateSystem(String identifier, String name, CoordinateSystemType type, params CoordinateSystemAxis[] axes)
            : base(identifier, name)
        {
            this.axes = axes ?? Array.Empty<CoordinateSystemAxis>();
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="type">The type.</param>
        /// <param name="axes">The axes of the coordinate system.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">No axis is specified for the coordinate system.</exception>
        public CoordinateSystem(String identifier, String name, String remarks, String[] aliases, CoordinateSystemType type, params CoordinateSystemAxis[] axes)
            : base(identifier, name)
        {
            this.axes = axes ?? Array.Empty<CoordinateSystemAxis>();
            this.Type = type;
        }

        /// <summary>
        /// Gets the axes of the coordinate system.
        /// </summary>
        /// <value>The read-only list of coordinate system axes.</value>
        public IReadOnlyList<CoordinateSystemAxis> Axes { get { return this.axes; } }

        /// <summary>
        /// Gets the dimension of the coordinate system.
        /// </summary>
        /// <value>The dimension of the coordinate system.</value>
        public Int32 Dimension { get { return this.axes.Length; } }

        /// <summary>
        /// Gets the type of the coordinate system.
        /// </summary>
        /// <value>The type of the coordinate system.</value>
        public CoordinateSystemType Type { get; }

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> at the specified index.
        /// </summary>
        /// <value>The <see cref="CoordinateSystemAxis" /> at the specified index.</value>
        /// <param name="index">The index of the axis.</param>
        /// <returns>The axis at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is greater than or equal to the number of axis.
        /// </exception>
        public CoordinateSystemAxis this[Int32 index] { get { return this.GetAxis(index); } }

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> with the specified name.
        /// </summary>
        /// <value>The <see cref="CoordinateSystemAxis" /> with the specified name.</value>
        /// <param name="name">The name of the axis.</param>
        /// <returns>The axis with the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The axis name is null.</exception>
        /// <exception cref="System.ArgumentException">The coordinate system does not contain an axis with the specified name.</exception>
        public CoordinateSystemAxis this[String name] { get { return this.GetAxis(name); } }

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> at the specified index.
        /// </summary>
        /// <param name="index">The index of the axis.</param>
        /// <returns>The axis at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is equal to or greater than the number of axis.
        /// </exception>
        public CoordinateSystemAxis GetAxis(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ReferenceMessages.IndexIsLessThan0);
            if (index >= this.axes.Length)
                throw new ArgumentOutOfRangeException(nameof(index), ReferenceMessages.IndexEqualToOrGreaterThanAxisCount);

            return this.axes[index];
        }

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> with the specified name.
        /// </summary>
        /// <param name="name">The name of the axis.</param>
        /// <returns>The axis with the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The axis name is null.</exception>
        /// <exception cref="System.ArgumentException">The coordinate system does not contain an axis with the specified name.</exception>
        public CoordinateSystemAxis GetAxis(String name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            CoordinateSystemAxis axis = this.axes.Where(ax => ax.Name == name).FirstOrDefault();
            if (axis != null)
                return axis;
            else
                throw new ArgumentException(ReferenceMessages.AxisNameIsInvalid, nameof(name));
        }
    }
}
