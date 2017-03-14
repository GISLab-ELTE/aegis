// <copyright file="ForwardLonLatHiCoordinateInterpretationStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Strategies
{
    /// <summary>
    /// Represents a forward coordinate interpretation for (longitude, latitude, height) representation.
    /// </summary>
    public class ForwardLonLatHiCoordinateInterpretationStrategy : ForwardLatLonCoordinateInterpretationStrategy
    {
        /// <summary>
        /// The unit of measurement of the height. This field is read-only.
        /// </summary>
        private readonly UnitOfMeasurement heightUnit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForwardLonLatHiCoordinateInterpretationStrategy" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <exception cref="System.ArgumentNullException">The reference system is null.</exception>
        /// <exception cref="System.ArgumentException">The dimension of the coordinate system is less than expected.</exception>
        public ForwardLonLatHiCoordinateInterpretationStrategy(CoordinateReferenceSystem referenceSystem)
            : base(referenceSystem)
        {
            this.heightUnit = this.referenceSystem.CoordinateSystem.GetAxis(2).Unit;
        }

        /// <summary>
        /// Applies the strategy on the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public override Coordinate Apply(GeoCoordinate coordinate)
        {
            return new Coordinate(coordinate.Longitude.GetValue(this.longitudeUnit), coordinate.Latitude.GetValue(this.latitudeUnit), coordinate.Height.GetValue(this.heightUnit));
        }
    }
}
