// <copyright file="CoordinateSystemAxis.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a coordinate system axis.
    /// </summary>
    public class CoordinateSystemAxis : IdentifiedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemAxis" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        public CoordinateSystemAxis(String identifier, String name, AxisDirection direction, UnitOfMeasurement unit)
            : this(identifier, name, direction, unit, Double.NegativeInfinity, Double.PositiveInfinity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemAxis" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <param name="minimum">The minimum value of the axis.</param>
        /// <param name="maximum">The maximum value of the axis.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The maximum value is equal to or less than the minimum value.</exception>
        public CoordinateSystemAxis(String identifier, String name, AxisDirection direction, UnitOfMeasurement unit, Double minimum, Double maximum)
            : this(identifier, name, null, null, null, direction, unit, minimum, maximum)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemAxis" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="description">The description.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        public CoordinateSystemAxis(String identifier, String name, String remarks, String[] aliases, String description, AxisDirection direction, UnitOfMeasurement unit)
            : this(identifier, name, direction, unit, Double.NegativeInfinity, Double.PositiveInfinity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemAxis" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="description">The description.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <param name="minimum">The minimum value of the axis.</param>
        /// <param name="maximum">The maximum value of the axis.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The maximum value is equal to or less than the minimum value.</exception>
        public CoordinateSystemAxis(String identifier, String name, String remarks, String[] aliases, String description, AxisDirection direction, UnitOfMeasurement unit, Double minimum, Double maximum)
            : base(identifier, name, remarks, aliases)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitIsNull);
            if (maximum <= minimum)
                throw new ArgumentException(ReferenceMessages.MaximumIsEqualToOrLessThanMinimum, nameof(maximum));

            this.Description = description ?? String.Empty;
            this.Direction = direction;
            this.Unit = unit;
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        /// <summary>
        /// Gets the description of the axis.
        /// </summary>
        /// <value>The description of the axis.</value>
        public String Description { get; private set; }

        /// <summary>
        /// Gets the direction of the axis.
        /// </summary>
        /// <value>Direction of this coordinate system axis (or in the case of Cartesian projected coordinates, the direction of this coordinate system axis locally).</value>
        public AxisDirection Direction { get; private set; }

        /// <summary>
        /// Gets the unit of measurement.
        /// </summary>
        /// <value>The unit of measurement.</value>
        public UnitOfMeasurement Unit { get; private set; }

        /// <summary>
        /// Gets the minimum value of the axis.
        /// </summary>
        /// <value>The minimum value normally allowed for this axis, in the unit for the axis.</value>
        public Double Minimum { get; private set; }

        /// <summary>
        /// Gets the maximum value of the axis.
        /// </summary>
        /// <value>The maximum value normally allowed for this axis, in the unit for the axis.</value>
        public Double Maximum { get; private set; }
    }
}
