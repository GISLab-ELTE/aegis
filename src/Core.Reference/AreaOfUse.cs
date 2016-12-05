// <copyright file="AreaOfUse.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ELTE.AEGIS.Algorithms;
    using ELTE.AEGIS.Numerics;

    /// <summary>
    /// Represents an area of use.
    /// </summary>
    public class AreaOfUse : IdentifiedObject
    {
        /// <summary>
        /// The undefined area of use. This field is read-only.
        /// </summary>
        public static readonly AreaOfUse Undefined = new AreaOfUse(UndefinedIdentifier, UndefinedName, Angle.Undefined, Angle.Undefined, Angle.Undefined, Angle.Undefined);

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="south">The south boundary.</param>
        /// <param name="west">The west boundary.</param>
        /// <param name="north">The north boundary.</param>
        /// <param name="east">The east boundary.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, Angle south, Angle west, Angle north, Angle east)
            : this(identifier, name, null, null, null, west, east, north, south, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="south">The south boundary.</param>
        /// <param name="west">The west boundary.</param>
        /// <param name="north">The north boundary.</param>
        /// <param name="east">The east boundary.</param>
        /// <param name="boundary">The collection of bounding coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, Angle south, Angle west, Angle north, Angle east, IEnumerable<GeoCoordinate> boundary)
            : this(identifier, name, null, null, null, west, east, north, south, boundary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="south">The south boundary.</param>
        /// <param name="west">The west boundary.</param>
        /// <param name="north">The north boundary.</param>
        /// <param name="east">The east boundary.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, String description, String remarks, String[] aliases, Angle south, Angle west, Angle north, Angle east)
            : this(identifier, name, description, remarks, aliases, west, east, north, south, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="south">The south boundary.</param>
        /// <param name="west">The west boundary.</param>
        /// <param name="north">The north boundary.</param>
        /// <param name="east">The east boundary.</param>
        /// <param name="boundary">The collection of bounding coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, String description, String remarks, String[] aliases, Angle south, Angle west, Angle north, Angle east, IEnumerable<GeoCoordinate> boundary)
            : base(identifier, name, remarks, aliases)
        {
            this.Description = description;
            this.West = west;
            this.East = east;
            this.North = north;
            this.South = south;
            this.Boundary = boundary;
        }

        /// <summary>
        /// Gets the south boundary.
        /// </summary>
        /// <value>The angle of the south boundary.</value>
        public Angle South { get; private set; }

        /// <summary>
        /// Gets the west boundary.
        /// </summary>
        /// <value>The angle of the west boundary.</value>
        public Angle West { get; private set; }

        /// <summary>
        /// Gets the north boundary.
        /// </summary>
        /// <value>The angle of the north boundary.</value>
        public Angle North { get; private set; }

        /// <summary>
        /// Gets the east boundary.
        /// </summary>
        /// <value>The angle of the east boundary.</value>
        public Angle East { get; private set; }

        /// <summary>
        /// Gets the collection of bounding coordinates.
        /// </summary>
        /// <value>The collection of bounding coordinates.</value>
        public IEnumerable<GeoCoordinate> Boundary { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public String Description { get; private set; }

        /// <summary>
        /// Creates an area of use from boundaries given in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="south">The south boundary (in degrees).</param>
        /// <param name="west">The west boundary (in degrees).</param>
        /// <param name="north">The north boundary (in degrees).</param>
        /// <param name="east">The east boundary (in degrees).</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromDegrees(String identifier, String name, Double south, Double west, Double north, Double east)
        {
            return new AreaOfUse(identifier, name, Angle.FromDegree(south), Angle.FromDegree(west), Angle.FromDegree(north), Angle.FromDegree(east));
        }

        /// <summary>
        /// Creates an area of use from boundaries given in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="south">The south boundary (in degrees).</param>
        /// <param name="west">The west boundary (in degrees).</param>
        /// <param name="north">The north boundary (in degrees).</param>
        /// <param name="east">The east boundary (in degrees).</param>
        /// <param name="boundary">The collection of bounding coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromDegrees(String identifier, String name, Double south, Double west, Double north, Double east, IEnumerable<GeoCoordinate> boundary)
        {
            return new AreaOfUse(identifier, name, Angle.FromDegree(south), Angle.FromDegree(west), Angle.FromDegree(north), Angle.FromDegree(east), boundary);
        }

        /// <summary>
        /// Creates an area of use from boundaries given in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="south">The south boundary (in degrees).</param>
        /// <param name="west">The west boundary (in degrees).</param>
        /// <param name="north">The north boundary (in degrees).</param>
        /// <param name="east">The east boundary (in degrees).</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromDegrees(String identifier, String name, String description, String remarks, String[] aliases, Double south, Double west, Double north, Double east)
        {
            return new AreaOfUse(identifier, name, description, remarks, aliases, Angle.FromDegree(south), Angle.FromDegree(west), Angle.FromDegree(north), Angle.FromDegree(east));
        }

        /// <summary>
        /// Creates an area of use from boundaries given in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="south">The south boundary (in degrees).</param>
        /// <param name="west">The west boundary (in degrees).</param>
        /// <param name="north">The north boundary (in degrees).</param>
        /// <param name="east">The east boundary (in degrees).</param>
        /// <param name="boundary">The collection of bounding coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromDegrees(String identifier, String name, String description, String remarks, String[] aliases, Double south, Double west, Double north, Double east, IEnumerable<GeoCoordinate> boundary)
        {
            return new AreaOfUse(identifier, name, description, remarks, aliases, Angle.FromDegree(south), Angle.FromDegree(west), Angle.FromDegree(north), Angle.FromDegree(east), boundary);
        }

        /// <summary>
        /// Creates an area of use from boundaries given in radians.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="south">The south boundary (in radians).</param>
        /// <param name="west">The west boundary (in radians).</param>
        /// <param name="north">The north boundary (in radians).</param>
        /// <param name="east">The east boundary (in radians).</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromRadians(String identifier, String name, Double south, Double west, Double north, Double east)
        {
            return new AreaOfUse(identifier, name, Angle.FromRadian(south), Angle.FromRadian(west), Angle.FromRadian(north), Angle.FromRadian(east));
        }

        /// <summary>
        /// Creates an area of use from boundaries given in radians.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="south">The south boundary (in radians).</param>
        /// <param name="west">The west boundary (in radians).</param>
        /// <param name="north">The north boundary (in radians).</param>
        /// <param name="east">The east boundary (in radians).</param>
        /// <param name="boundary">The collection of bounding coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromRadians(String identifier, String name, Double south, Double west, Double north, Double east, IEnumerable<GeoCoordinate> boundary)
        {
            return new AreaOfUse(identifier, name, Angle.FromRadian(south), Angle.FromRadian(west), Angle.FromRadian(north), Angle.FromRadian(east), boundary);
        }

        /// <summary>
        /// Creates an area of use from boundaries given in radians.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="south">The south boundary (in radians).</param>
        /// <param name="west">The west boundary (in radians).</param>
        /// <param name="north">The north boundary (in radians).</param>
        /// <param name="east">The east boundary (in radians).</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromRadians(String identifier, String name, String description, String remarks, String[] aliases, Double south, Double west, Double north, Double east)
        {
            return new AreaOfUse(identifier, name, description, remarks, aliases, Angle.FromRadian(south), Angle.FromRadian(west), Angle.FromRadian(north), Angle.FromRadian(east));
        }

        /// <summary>
        /// Creates an area of use from boundaries given in radians.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="south">The south boundary (in radians).</param>
        /// <param name="west">The west boundary (in radians).</param>
        /// <param name="north">The north boundary (in radians).</param>
        /// <param name="east">The east boundary (in radians).</param>
        /// <param name="boundary">The collection of bounding coordinates.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromRadians(String identifier, String name, String description, String remarks, String[] aliases, Double south, Double west, Double north, Double east, IEnumerable<GeoCoordinate> boundary)
        {
            return new AreaOfUse(identifier, name, description, remarks, aliases, Angle.FromRadian(south), Angle.FromRadian(west), Angle.FromRadian(north), Angle.FromRadian(east), boundary);
        }

        /// <summary>
        /// Determines whether the area contains a geographic coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate is within the area; otherwise, <c>false</c>.</returns>
        public Boolean Contains(GeoCoordinate coordinate)
        {
            if (coordinate == null)
                return false;

            // if the boundary is specified, the contained can be determined more exactly
            if (this.Boundary != null)
                return !WindingNumberAlgorithm.InExterior(this.Boundary.Select(coord => (Coordinate)coord).ToArray(), (Coordinate)coordinate);

            return (this.West <= this.East && this.West <= coordinate.Latitude && coordinate.Latitude <= this.East) ||
                   (this.West > this.East && ((this.West - Angle.FromRadian(2 * Math.PI) <= coordinate.Latitude && coordinate.Latitude <= this.East) ||
                                    (this.West <= coordinate.Latitude && coordinate.Latitude <= this.East + Angle.FromRadian(2 * Math.PI))));
        }

        /// <summary>
        /// Determines whether the area is within the specified boundaries.
        /// </summary>
        /// <param name="south">The south boundary.</param>
        /// <param name="west">The west boundary.</param>
        /// <param name="north">The north boundary.</param>
        /// <param name="east">The east boundary.</param>
        /// <returns><c>true</c> if the area is within the specified boundary; otherwise, <c>false</c>.</returns>
        public Boolean Within(Angle south, Angle west, Angle north, Angle east)
        {
            return this.South.BaseValue >= south.BaseValue && this.West.BaseValue >= west.BaseValue &&
                   this.North.BaseValue <= north.BaseValue && this.East.BaseValue <= east.BaseValue;
        }

        /// <summary>
        /// Determines whether the area is within the other area.
        /// </summary>
        /// <param name="area">The other area of use.</param>
        /// <returns><c>true</c> if the area is within the other area; otherwise, <c>false</c>.</returns>
        public Boolean Within(AreaOfUse area)
        {
            return this.Within(area.South, area.West, area.North, area.East);
        }
    }
}
