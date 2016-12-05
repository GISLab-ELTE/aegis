// <copyright file="LambertAzimuthalEqualAreaProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Azimuthal Equal Area Projection.
    /// </summary>
    [IdentifiedObject("AEGIS::9820", "Lambert Azimuthal Equal Area Projection")]
    public class LambertAzimuthalEqualAreaProjection : CoordinateProjection
    {
        /// <summary>
        /// Latitude of natural origin.
        /// </summary>
        protected readonly Double latitudeOfNaturalOrigin;

        /// <summary>
        /// Longitude of natural origin.
        /// </summary>
        protected readonly Double longitudeOfNaturalOrigin;

        /// <summary>
        /// False easting.
        /// </summary>
        protected readonly Double falseEasting;

        /// <summary>
        /// False northing.
        /// </summary>
        protected readonly Double falseNorthing;

        /// <summary>
        /// Operation constants.
        /// </summary>
        protected readonly Double D;

        /// <summary>
        /// Operation constants.
        /// </summary>
        protected readonly Double RQ;

        /// <summary>
        /// Operation constants.
        /// </summary>
        protected readonly Double betaO;

        /// <summary>
        /// Operation constants.
        /// </summary>
        protected readonly Double qP;

        /// <summary>
        /// Operation constants.
        /// </summary>
        protected readonly Double qO;

        /// <summary>
        /// Operation constants.
        /// </summary>
        protected readonly OperationAspect operationAspect;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertAzimuthalEqualAreaProjection" /> class.
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
        public LambertAzimuthalEqualAreaProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertAzimuthalEqualAreaProjection" /> class.
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
        public LambertAzimuthalEqualAreaProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, remarks, aliases, CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertAzimuthalEqualAreaProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected LambertAzimuthalEqualAreaProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            // EPSG Guidance Note number 7, part 2

            this.latitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LatitudeOfNaturalOrigin);
            this.longitudeOfNaturalOrigin = this.GetParameterBaseValue(CoordinateOperationParameters.LongitudeOfNaturalOrigin);
            this.falseEasting = this.GetParameterValue(CoordinateOperationParameters.FalseEasting);
            this.falseNorthing = this.GetParameterValue(CoordinateOperationParameters.FalseNorthing);

            this.qP = (1 - this.Ellipsoid.EccentricitySquare) * ((1 / (1 - this.Ellipsoid.EccentricitySquare)) - ((1 / 2 * this.Ellipsoid.Eccentricity) * Math.Log((1 - this.Ellipsoid.Eccentricity) / (1 + this.Ellipsoid.Eccentricity), Math.E)));
            this.qO = (1 - this.Ellipsoid.EccentricitySquare) * ((Math.Sin(this.latitudeOfNaturalOrigin) / (1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfNaturalOrigin))) - ((1 / (2 * this.Ellipsoid.Eccentricity)) * Math.Log((1 - this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfNaturalOrigin)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(this.latitudeOfNaturalOrigin)), Math.E)));
            this.betaO = Math.Asin(this.qO / this.qP);
            this.RQ = this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Sqrt(this.qP / 2);
            this.D = this.Ellipsoid.SemiMajorAxis.BaseValue * (Math.Cos(this.latitudeOfNaturalOrigin) / Math.Sqrt(1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(this.latitudeOfNaturalOrigin))) / (this.RQ * Math.Cos(this.betaO));

            if (Math.Abs(this.latitudeOfNaturalOrigin) - Math.Abs(Angles.NorthPole.BaseValue) <= 1E-10)
                this.operationAspect = OperationAspect.NorthPolar;
            else if (Math.Abs(this.latitudeOfNaturalOrigin) - Math.Abs(Angles.SouthPole.BaseValue) <= 1E-10)
                this.operationAspect = OperationAspect.SouthPolar;
            else if (this.Ellipsoid.IsSphere && Math.Abs(this.latitudeOfNaturalOrigin) - Math.Abs(Angles.Equator.BaseValue) <= 1E-10)
                this.operationAspect = OperationAspect.Equatorial;
            else
                this.operationAspect = OperationAspect.Oblique;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double easting = 0, northing = 0;

            if (this.Ellipsoid.IsSphere)
            {
                // source: Snyder, J. P.: Map Projections - A Working Manual, page 185
                Double k;

                switch (this.operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        easting = 2 * this.Ellipsoid.RadiusOfAuthalicSphere.Value * Math.Sin(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                        northing = -2 * this.Ellipsoid.RadiusOfAuthalicSphere.Value * Math.Sin(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                        break;
                    case OperationAspect.SouthPolar:
                        easting = 2 * this.Ellipsoid.RadiusOfAuthalicSphere.Value * Math.Cos(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                        northing = 2 * this.Ellipsoid.RadiusOfAuthalicSphere.Value * Math.Cos(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                        break;
                    case OperationAspect.Equatorial:
                        k = Math.Sqrt(2 / (1 + Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin)));
                        easting = this.Ellipsoid.RadiusOfAuthalicSphere.Value * k * Math.Cos(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                        northing = this.Ellipsoid.RadiusOfAuthalicSphere.Value * k * Math.Sin(coordinate.Latitude.BaseValue);
                        break;
                    case OperationAspect.Oblique:
                        k = Math.Sqrt(2 / (1 + Math.Sin(this.latitudeOfNaturalOrigin) * Math.Sin(coordinate.Latitude.BaseValue) + Math.Cos(this.latitudeOfNaturalOrigin) * Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin)));
                        easting = this.Ellipsoid.RadiusOfAuthalicSphere.Value * k * Math.Cos(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin);
                        northing = this.Ellipsoid.RadiusOfAuthalicSphere.Value * k * (Math.Cos(this.latitudeOfNaturalOrigin) * Math.Sin(coordinate.Latitude.BaseValue) - Math.Sin(this.latitudeOfNaturalOrigin) * Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin));
                        break;
                }
            }
            else
            {
                Double q = (1 - this.Ellipsoid.EccentricitySquare) * ((Math.Sin(coordinate.Latitude.BaseValue) / (1 - this.Ellipsoid.EccentricitySquare * Calculator.Sin2(coordinate.Latitude.BaseValue))) - ((1 / (2 * this.Ellipsoid.Eccentricity)) * Math.Log((1 - this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + this.Ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)), Math.E)));

                switch (this.operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        Double rho = this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Sqrt(this.qP - q);

                        easting = this.falseEasting + (rho * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin));
                        northing = this.falseNorthing - (rho * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin));
                        break;
                    case OperationAspect.SouthPolar:
                        Double ro = this.Ellipsoid.SemiMajorAxis.BaseValue * Math.Sqrt(this.qP + q);

                        easting = this.falseEasting + (ro * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin));
                        northing = this.falseNorthing + (ro * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin));
                        break;
                    case OperationAspect.Oblique:
                        Double beta = Math.Asin(q / this.qP);
                        Double b = this.RQ * Math.Sqrt(2 / (1 + Math.Sin(this.betaO) * Math.Sin(beta) + (Math.Cos(this.betaO) * Math.Cos(beta) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin))));

                        easting = this.falseEasting + ((b * this.D) * (Math.Cos(beta) * Math.Sin(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin)));
                        northing = this.falseNorthing + ((b / this.D) * ((Math.Cos(this.betaO) * Math.Sin(beta)) - (Math.Sin(this.betaO) * Math.Cos(beta) * Math.Cos(coordinate.Longitude.BaseValue - this.longitudeOfNaturalOrigin))));
                        break;
                }
            }

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double phi = 0, lambda = 0;

            if (this.Ellipsoid.IsSphere)
            {
                // source: Snyder, J. P.: Map Projections - A Working Manual, page 185

                Double rho = Math.Pow(Math.Pow(coordinate.X, 2) + Math.Pow(coordinate.Y, 2), 1 / 2);
                Double c = 2 * Math.Asin(rho / (2 * this.Ellipsoid.RadiusOfAuthalicSphere.Value));
                phi = Math.Asin(Math.Cos(c) * Math.Sin(this.latitudeOfNaturalOrigin) + (coordinate.Y * Math.Sin(c) * Math.Cos(this.latitudeOfNaturalOrigin / rho)));

                switch (this.operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        lambda = this.longitudeOfNaturalOrigin + Math.Atan(coordinate.X / (-coordinate.Y));
                        break;
                    case OperationAspect.SouthPolar:
                        lambda = this.longitudeOfNaturalOrigin + Math.Atan(coordinate.X / coordinate.Y);
                        break;
                    case OperationAspect.Oblique:
                        lambda = this.longitudeOfNaturalOrigin + Math.Atan(coordinate.X * Math.Sin(c) / (rho * Math.Cos(this.latitudeOfNaturalOrigin) * Math.Cos(c) - coordinate.Y * Math.Sin(this.latitudeOfNaturalOrigin) * Math.Sin(c)));
                        break;
                }
            }
            else
            {
                Double rho = Math.Pow(Math.Pow((coordinate.X - this.falseEasting) / this.D, 2) + Math.Pow(this.D * (coordinate.Y - this.falseNorthing), 2), 0.5);
                Double c = 2 * Math.Asin(rho / 2 * this.RQ);
                Double beta = 0;

                switch (this.operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        beta = Math.Asin(1 - Math.Pow(rho, 2) / (Math.Pow(this.Ellipsoid.SemiMajorAxis.BaseValue, 2) * (1 - ((1 - this.Ellipsoid.EccentricitySquare) / 2 * this.Ellipsoid.Eccentricity) * Math.Log((1 - this.Ellipsoid.Eccentricity) / (1 + this.Ellipsoid.Eccentricity), Math.E))));
                        lambda = this.longitudeOfNaturalOrigin + Math.Atan((coordinate.X - this.falseEasting) / (this.falseNorthing - coordinate.Y));
                        break;
                    case OperationAspect.SouthPolar:
                        beta = -Math.Asin(1 - Math.Pow(rho, 2) / (Math.Pow(this.Ellipsoid.SemiMajorAxis.BaseValue, 2) * (1 - ((1 - this.Ellipsoid.EccentricitySquare) / 2 * this.Ellipsoid.Eccentricity) * Math.Log((1 - this.Ellipsoid.Eccentricity) / (1 + this.Ellipsoid.Eccentricity), Math.E))));
                        lambda = this.longitudeOfNaturalOrigin + Math.Atan((coordinate.X - this.falseEasting) / (coordinate.Y - this.falseNorthing));
                        break;
                    case OperationAspect.Oblique:
                        beta = Math.Asin((Math.Cos(c) * Math.Sin(this.betaO)) + ((this.D * (coordinate.Y - this.falseNorthing) * Math.Sin(c) * Math.Cos(this.betaO)) / rho));
                        lambda = this.longitudeOfNaturalOrigin + Math.Atan((coordinate.X - this.falseEasting) * Math.Sin(c) / (this.D * rho * Math.Cos(this.betaO) * Math.Cos(c) - Math.Pow(this.D, 2) * (coordinate.Y - this.falseNorthing) * Math.Sin(this.betaO) * Math.Sin(c)));
                        break;
                }

                phi = beta + ((this.Ellipsoid.EccentricitySquare / 3 + 31 * Math.Pow(this.Ellipsoid.EccentricitySquare, 2) / 180 + 517 * Math.Pow(this.Ellipsoid.EccentricitySquare, 3) / 5040) * Math.Sin(2 * beta)) + ((23 * Math.Pow(this.Ellipsoid.EccentricitySquare, 2) / 360 + 251 * Math.Pow(this.Ellipsoid.EccentricitySquare, 3) / 3780) * Math.Sin(4 * beta)) + ((761 * Math.Pow(this.Ellipsoid.EccentricitySquare, 3) / 45360) * Math.Sin(6 * beta));
            }

            return new GeoCoordinate(phi, lambda);
        }
    }
}
