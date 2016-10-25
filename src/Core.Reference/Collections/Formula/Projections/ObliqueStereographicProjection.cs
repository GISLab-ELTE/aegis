// <copyright file="ObliqueStereographicProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using ELTE.AEGIS.Numerics;

    /// <summary>
    /// Represents an Oblique Stereographic Projection.
    /// </summary>
    [IdentifiedObject("EPSG::9809", "Oblique Stereographic")]
    public class ObliqueStereographicProjection : CoordinateProjection
    {
        #region Private fields

        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        private readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        private readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// False easting.
        /// </summary>
        private readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        private readonly Double falseNorthing;

        /// <summary>
        /// Scale factor at natural origin.
        /// </summary>
        private readonly Double scaleFactorAtNaturalOrigin;

        /// <summary>
        /// Radius of conformal sphere at natural origin.
        /// </summary>
        private readonly Double radiusOfConformalSphere;

        /// <summary>
        /// Latitude of natural origin of conformal sphere.
        /// </summary>
        private readonly Double conformalLatitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin of conformal sphere.
        /// </summary>
        private readonly Double conformalLongitudeOfNaturalOrigin;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double n;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double c;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double g;

        /// <summary>
        /// Operation constant.
        /// </summary>
        private readonly Double h;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObliqueStereographicProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public ObliqueStereographicProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObliqueStereographicProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public ObliqueStereographicProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.ObliqueStereographicProjection, parameters, ellipsoid, areaOfUse)
        {
            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);
            this.scaleFactorAtNaturalOrigin = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin);

            if (!this.Ellipsoid.IsSphere)
            {
                // ellipsoid values have to be projected to the sphere
                this.n = Math.Sqrt(1 + (this.Ellipsoid.EccentricitySquare * Calculator.Cos4(this.latitudeOfNaturalOrigin)) / (1 - this.Ellipsoid.EccentricitySquare));

                Double s1 = (1 + Math.Sin(this.latitudeOfNaturalOrigin)) / (1 - Math.Sin(this.latitudeOfNaturalOrigin));
                Double s2 = (1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfNaturalOrigin)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfNaturalOrigin));
                Double w1 = Math.Pow(s1 * Math.Pow(s2, this.Ellipsoid.Eccentricity), this.n);
                Double sinXi1 = (w1 - 1) / (w1 + 1);

                this.radiusOfConformalSphere = this.Ellipsoid.RadiusOfConformalSphere(this.latitudeOfNaturalOrigin);
                this.c = (this.n + Math.Sin(this.latitudeOfNaturalOrigin)) * (1 - sinXi1) / ((this.n - Math.Sin(this.latitudeOfNaturalOrigin)) * (1 + sinXi1));

                Double w2 = this.c * w1;

                this.conformalLatitudeOfNaturalOrigin = Math.Asin((w2 - 1) / (w2 + 1));
                this.conformalLongitudeOfNaturalOrigin = this.longitudeOfNaturalOrigin;
            }
            else
            {
                this.radiusOfConformalSphere = this.Ellipsoid.SemiMajorAxis.Value;

                this.conformalLatitudeOfNaturalOrigin = this.latitudeOfNaturalOrigin;
                this.conformalLongitudeOfNaturalOrigin = this.longitudeOfNaturalOrigin;
            }

            this.g = 2 * this.radiusOfConformalSphere * this.scaleFactorAtNaturalOrigin * Math.Tan(Math.PI / 4 - this.conformalLatitudeOfNaturalOrigin / 2);
            this.h = 4 * this.radiusOfConformalSphere * this.scaleFactorAtNaturalOrigin * Math.Tan(this.conformalLatitudeOfNaturalOrigin) + this.g;
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double conformalLongitude, conformalLatitude;
            if (!this.Ellipsoid.IsSphere)
            {
                conformalLongitude = this.n * (coordinate.Longitude.BaseValue - this.conformalLongitudeOfNaturalOrigin) + this.conformalLongitudeOfNaturalOrigin;

                Double bA = (1 + Math.Sin(coordinate.Latitude.BaseValue)) / (1 - Math.Sin(coordinate.Latitude.BaseValue));
                Double bB = (1 - this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue));
                Double w = this.c * Math.Pow(bA * Math.Pow(bB, this.Ellipsoid.Eccentricity), this.n);

                conformalLatitude = Math.Asin((w - 1) / (w + 1));
            }
            else
            {
                conformalLatitude = coordinate.Latitude.BaseValue;
                conformalLongitude = coordinate.Longitude.BaseValue;
            }

            Double b = 1 + Math.Sin(conformalLatitude) * Math.Sin(this.conformalLatitudeOfNaturalOrigin) + Math.Cos(conformalLatitude) * Math.Cos(this.conformalLatitudeOfNaturalOrigin) * Math.Cos(conformalLongitude - this.conformalLongitudeOfNaturalOrigin);
            Double easting = this.falseEasting + 2 * this.radiusOfConformalSphere * this.scaleFactorAtNaturalOrigin * Math.Cos(conformalLatitude) * Math.Sin(conformalLongitude - this.conformalLongitudeOfNaturalOrigin) / b;
            Double northing = this.falseNorthing + 2 * this.radiusOfConformalSphere * this.scaleFactorAtNaturalOrigin * (Math.Sin(conformalLatitude) * Math.Cos(this.conformalLatitudeOfNaturalOrigin) - Math.Cos(conformalLatitude) * Math.Sin(this.conformalLatitudeOfNaturalOrigin) * Math.Cos(conformalLongitude - this.conformalLongitudeOfNaturalOrigin)) / b;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double i = Math.Atan((coordinate.X - this.falseEasting) / (this.h + coordinate.Y - this.falseNorthing));
            Double j = Math.Atan((coordinate.X - this.falseEasting) / (this.g - coordinate.Y + this.falseNorthing)) - i;

            Double conformalLongitude = j + 2 * i + this.conformalLongitudeOfNaturalOrigin;
            Double conformalLatitude = this.conformalLatitudeOfNaturalOrigin + 2 * Math.Tanh((coordinate.Y - this.falseNorthing - (coordinate.X - this.falseEasting) * Math.Tan(j / 2)) / (2 * this.radiusOfConformalSphere * this.scaleFactorAtNaturalOrigin));

            Double latitude, longitude;
            if (!this.Ellipsoid.IsSphere)
            {
                longitude = (conformalLongitude - this.conformalLongitudeOfNaturalOrigin) / this.n + this.conformalLongitudeOfNaturalOrigin;

                Double psi1 = Math.Log((1 + Math.Sin(conformalLatitude)) / (this.c * (1 - Math.Sin(conformalLatitude)))) / this.n / 2;
                Double latitude1 = 2 * Math.Atan(Math.Exp(psi1)) - Math.PI / 2;
                Double latitude2, psi;

                for (Int32 k = 0; k < 100; k++)
                {
                    psi = Math.Log(Math.Tan(latitude1 / 2 + Math.PI / 4) * Math.Pow((1 - this.Ellipsoid.Eccentricity * Math.Sin(latitude1)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(latitude1)), this.Ellipsoid.Eccentricity / 2));
                    latitude2 = latitude1 - (psi - psi1) * Math.Cos(latitude1) * (1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(latitude1) / (1 - this.Ellipsoid.EccentricitySquare));
                    if (Math.Abs(latitude1 - latitude2) <= 1E-4)
                        break;

                    latitude1 = latitude2;
                }

                latitude = latitude1;
            }
            else
            {
                longitude = conformalLongitude;
                latitude = conformalLatitude;
            }

            return new GeoCoordinate(latitude, longitude);
        }

        #endregion
    }
}
