// <copyright file="EllipsoidComputations.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference
{
    using System;
    using System.Linq;
    using AEGIS.Numerics;
    using AEGIS.Reference.Resources;

    /// <summary>
    /// Represents a type for specific ellipsoidal computations.
    /// </summary>
    public static class EllipsoidComputations
    {
        /// <summary>
        /// Transforms the ellipsoid to the specified unit of measurement.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="unitOfMeasurement">The unit of measurement.</param>
        /// <returns>The ellipsoid measured in the specified unit of measurement.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The unit of measurement is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The unit of measurement is not a length measure.</exception>
        public static Ellipsoid ToUnit(this Ellipsoid ellipsoid, UnitOfMeasurement unitOfMeasurement)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);
            if (unitOfMeasurement == null)
                throw new ArgumentNullException(nameof(unitOfMeasurement), ReferenceMessages.UnitIsNull);
            if (unitOfMeasurement.Type != UnitQuantityType.Length)
                throw new ArgumentException(ReferenceMessages.UnitIsNotLength, nameof(unitOfMeasurement));

            if (ellipsoid.SemiMajorAxis.Unit.Equals(unitOfMeasurement))
                return ellipsoid;

            if (ellipsoid.IsSphere)
            {
                return Ellipsoid.FromSphere(ellipsoid.Identifier, ellipsoid.Name, ellipsoid.Remarks, ellipsoid.Aliases.ToArray(),
                                            new Length(ellipsoid.SemiMajorAxis.GetValue(unitOfMeasurement), unitOfMeasurement));
            }
            else
            {
                return Ellipsoid.FromInverseFlattening(ellipsoid.Identifier, ellipsoid.Name, ellipsoid.Remarks, ellipsoid.Aliases.ToArray(),
                                                       new Length(ellipsoid.SemiMajorAxis.GetValue(unitOfMeasurement), unitOfMeasurement), ellipsoid.InverseFattening);
            }
        }

        /// <summary>
        /// Determines the geocentric latitude at a specified geodetic latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="geodeticLatitude">The geodetic latitude.</param>
        /// <returns>The geocentric latitude at the specified geodetic latitude.</returns>
        public static Angle GetGeocentricLatitude(this Ellipsoid ellipsoid, Angle geodeticLatitude)
        {
            return Angle.FromRadian(GetGeocentricLatitude(ellipsoid, geodeticLatitude.BaseValue));
        }

        /// <summary>
        /// Determines the geocentric latitude at a specified geodetic latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="geodeticLatitude">The geodetic latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The geocentric latitude (defined in <see cref="UnitsOfMeasurement.Radian" />) at the specified geodetic latitude.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public static Double GetGeocentricLatitude(this Ellipsoid ellipsoid, Double geodeticLatitude)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);

            if (geodeticLatitude > Math.PI / 2 || geodeticLatitude < -Math.PI / 2)
                return Double.NaN;

            if (ellipsoid.IsSphere)
                return geodeticLatitude;
            else
                return ellipsoid.SemiMinorAxis.Value * ellipsoid.SemiMinorAxis.Value / (ellipsoid.SemiMajorAxis.Value * ellipsoid.SemiMajorAxis.Value) * Math.Tan(geodeticLatitude);
        }

        /// <summary>
        /// Determines the reduced latitude at a specified geodetic latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="geodeticLatitude">The geodetic latitude.</param>
        /// <returns>The reduced latitude at the specified geodetic latitude.</returns>
        public static Angle GetReducedLatitude(this Ellipsoid ellipsoid, Angle geodeticLatitude)
        {
            return Angle.FromRadian(GetReducedLatitude(ellipsoid, geodeticLatitude.BaseValue));
        }

        /// <summary>
        /// Determines the reduced latitude at a specified geodetic latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="geodeticLatitude">The geodetic latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The reduced latitude (defined in <see cref="UnitsOfMeasurement.Radian" />) at the specified geodetic latitude.</returns>
        public static Double GetReducedLatitude(this Ellipsoid ellipsoid, Double geodeticLatitude)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);

            if (geodeticLatitude > Math.PI / 2 || geodeticLatitude < -Math.PI / 2)
                return Double.NaN;

            if (ellipsoid.IsSphere)
                return geodeticLatitude;
            else
                return ellipsoid.SemiMinorAxis.Value / ellipsoid.SemiMajorAxis.Value * Math.Tan(geodeticLatitude);
        }

        /// <summary>
        /// Determines the distances the of parallel curvature at a specified geodetic latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The distances the of parallel curvature.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        /// <exception cref="System.ArgumentException">The latitude value is not valid.</exception>
        public static Length DistanceOfParalellCurvature(this Ellipsoid ellipsoid, Angle latitude)
        {
            return new Length(DistanceOfParalellCurvature(ellipsoid, latitude.BaseValue), ellipsoid.SemiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the distances the of parallel curvature at a specified geodetic latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The distances the of parallel curvature.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public static Double DistanceOfParalellCurvature(this Ellipsoid ellipsoid, Double latitude)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);

            if (latitude > Math.PI / 2 || latitude < -Math.PI / 2)
                return Double.NaN;

            if (ellipsoid.IsSphere)
                return ellipsoid.SemiMajorAxis.Value * Math.Sin(latitude);
            else
                return ellipsoid.SemiMinorAxis.Value * ellipsoid.SemiMinorAxis.Value / ellipsoid.SemiMajorAxis.Value * Math.Sin(latitude) / Math.Sqrt(1 - ellipsoid.EccentricitySquare * Math.Sin(latitude) * Math.Sin(latitude));
        }

        /// <summary>
        /// Determines the length of vertical curvature between specified latitudes.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="startingLatitude">The starting latitude.</param>
        /// <param name="endingLatitude">The ending latitude.</param>
        /// <returns>The length of vertical curvature between the specified latitudes.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The starting latitude value is not valid.
        /// or
        /// The ending latitude value is not valid.
        /// </exception>
        public static Length LengthOfVerticalCurvature(this Ellipsoid ellipsoid, Angle startingLatitude, Angle endingLatitude)
        {
             return new Length(LengthOfVerticalCurvature(ellipsoid, startingLatitude.BaseValue, endingLatitude.BaseValue), ellipsoid.SemiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the length of vertical curvature between specified latitudes.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="startingLatitude">The starting latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <param name="endingLatitude">The ending latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The length of vertical curvature between the specified latitudes.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public static Double LengthOfVerticalCurvature(this Ellipsoid ellipsoid, Double startingLatitude, Double endingLatitude)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);

            if (startingLatitude > Math.PI / 2 || startingLatitude < -Math.PI / 2 || endingLatitude > Math.PI / 2 || endingLatitude < -Math.PI / 2)
                return Double.NaN;

            if (ellipsoid.IsSphere)
                return ellipsoid.SemiMajorAxis.Value * Math.Abs(endingLatitude - startingLatitude);
            else
                return Numerics.Integral.SimpsonsMethod.ComputeIntegral(latitude => ellipsoid.RadiusOfMeridianCurvature(latitude), startingLatitude, endingLatitude);
        }

        /// <summary>
        /// Determines the length of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="startingLongitude">The starting longitude.</param>
        /// <param name="endingLongitude">The ending longitude.</param>
        /// <returns>The length of parallel curvature between the specified longitudes at the specified latitude.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public static Length LengthOfParalellCurvature(this Ellipsoid ellipsoid, Angle latitude, Angle startingLongitude, Angle endingLongitude)
        {
            return LengthOfParalellCurvature(ellipsoid, latitude, endingLongitude - startingLongitude);
        }

        /// <summary>
        /// Determines the length of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitudeDifference">The longitude difference.</param>
        /// <returns>The length of parallel curvature for the specified longitude difference at the specified latitude.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public static Length LengthOfParalellCurvature(this Ellipsoid ellipsoid, Angle latitude, Angle longitudeDifference)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);

            return ellipsoid.RadiusOfParalellCurvature(latitude) * longitudeDifference.BaseValue;
        }

        /// <summary>
        /// Determines the length of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <param name="startingLongitude">The starting longitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <param name="endingLongitude">The ending longitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The length of parallel curvature for the specified longitude difference at the specified latitude.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public static Double LengthOfParalellCurvature(this Ellipsoid ellipsoid, Double latitude, Double startingLongitude, Double endingLongitude)
        {
            return LengthOfParalellCurvature(ellipsoid, latitude, endingLongitude - startingLongitude);
        }

        /// <summary>
        /// Determines the length of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <param name="longitudeDifference">The longitude difference (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The length of parallel curvature for the specified longitude difference at the specified latitude.</returns>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public static Double LengthOfParalellCurvature(this Ellipsoid ellipsoid, Double latitude, Double longitudeDifference)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);

            return ellipsoid.RadiusOfParalellCurvature(latitude) * longitudeDifference;
        }

        /// <summary>
        /// Determines a destination geodetic coordinate based on starting coordinate and vector.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="sourceCoordinate">The source coordinate.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The destination geodetic coordinate based on starting coordinate and vector.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The coordinate is null.
        /// or
        /// The vector is null.
        /// </exception>
        public static GeoCoordinate GetCoordinate(this Ellipsoid ellipsoid, GeoCoordinate sourceCoordinate, GeoVector vector)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);
            if (sourceCoordinate == null)
                throw new ArgumentNullException(nameof(sourceCoordinate), ReferenceMessages.CoordinateIsNull);
            if (vector == null)
                throw new ArgumentNullException(nameof(vector), ReferenceMessages.VectorIsNull);

            if (!vector.IsValid)
                return GeoCoordinate.Undefined;

            if (vector.IsNull)
                return sourceCoordinate;

            if (ellipsoid.IsSphere)
            {
                Double phi, lambda = 0, deltaLambda = 0;

                phi = Math.Asin(Math.Sin(sourceCoordinate.Latitude.BaseValue) * Math.Cos(vector.Distance.BaseValue / ellipsoid.SemiMajorAxis.Value) + Math.Cos(sourceCoordinate.Latitude.BaseValue) * Math.Sin(vector.Distance.BaseValue / ellipsoid.SemiMajorAxis.Value) * Math.Cos(vector.Azimuth.BaseValue));

                // the destination is a pole
                if (Math.Abs(phi - Math.PI / 2) <= 1E-10)
                    return new GeoCoordinate(Angles.NorthPole, Angle.Zero);
                if (Math.Abs(phi - 3 * Math.PI / 2) <= 1E-10)
                    return new GeoCoordinate(Angles.SouthPole, Angle.Zero);

                // the vector is north or south oriented
                if (Math.Abs(Math.Sin(vector.Azimuth.BaseValue)) <= 1E-10)
                {
                    if (Math.Abs(vector.Distance.BaseValue - LengthOfVerticalCurvature(ellipsoid, sourceCoordinate.Latitude.BaseValue, phi)) <= 1E-10)
                        deltaLambda = 0;
                    else
                        deltaLambda = Math.PI;
                }
                else if (Math.Cos(sourceCoordinate.Latitude.BaseValue) <= 1E-10)
                {
                    // we start from a pole
                    deltaLambda = -vector.Azimuth.BaseValue;
                }
                else
                {
                    deltaLambda = Math.Asin(Math.Sin(vector.Azimuth.BaseValue) * Math.Sin(vector.Distance.BaseValue / ellipsoid.SemiMajorAxis.Value) / Math.Cos(phi));
                    deltaLambda = Math.Acos((Math.Cos(vector.Distance.BaseValue / ellipsoid.SemiMajorAxis.Value) - Math.Sin(sourceCoordinate.Latitude.BaseValue) * Math.Sin(phi)) / Math.Cos(sourceCoordinate.Latitude.BaseValue) / Math.Cos(phi)) * Math.Sin(deltaLambda) / Math.Abs(Math.Sin(deltaLambda));
                }

                lambda = (sourceCoordinate.Longitude.BaseValue + deltaLambda) % Math.PI;

                return new GeoCoordinate(phi, lambda);
            }
            else
            {
                // damn complex equation, you don't wanna know

                Double phi0 = sourceCoordinate.Latitude.BaseValue;
                Double lambda0 = sourceCoordinate.Longitude.BaseValue;
                Double alpha0 = vector.Azimuth.BaseValue;

                Double sinAlpha0 = Math.Sin(alpha0);
                Double sin2Alpha0 = Calculator.Sin2(alpha0);
                Double cosAlpha0 = Math.Cos(alpha0);
                Double cos2Alpha0 = Calculator.Cos2(alpha0);

                Double sinPhi0 = Math.Sin(phi0);
                Double sin2Phi0 = Calculator.Sin2(phi0);
                Double cosPhi0 = Math.Cos(phi0);
                Double cos2Phi0 = Calculator.Cos2(phi0);

                Double mPhi0 = ellipsoid.RadiusOfMeridianCurvature(phi0);
                Double nPhi0 = ellipsoid.RadiusOfPrimeVerticalCurvature(phi0);
                Double n2Phi0Cos2phi0 = Math.Pow(ellipsoid.RadiusOfPrimeVerticalCurvature(phi0) * Math.Cos(phi0), 2);
                Double n3Phi0Cos3phi0 = Math.Pow(ellipsoid.RadiusOfPrimeVerticalCurvature(phi0) * Math.Cos(phi0), 3);
                Double oneMinusE2 = 1 - ellipsoid.EccentricitySquare;
                Double e4 = ellipsoid.EccentricitySquare * ellipsoid.EccentricitySquare;

                Double dPhi = cosAlpha0 / mPhi0;
                Double dLambda = sinAlpha0 / nPhi0;
                Double dAlpha = sinAlpha0 / nPhi0 * Math.Tan(phi0);

                Double dPhi2 = -sin2Alpha0 * Math.Tan(phi0) / mPhi0 / nPhi0 - cosAlpha0 * 3 * ellipsoid.EccentricitySquare * sinPhi0 * cosPhi0 / (mPhi0 * mPhi0) / (1 - ellipsoid.EccentricitySquare * sin2Phi0);
                Double dLambda2 = 2 * sinAlpha0 * cosAlpha0 * sinPhi0 / n2Phi0Cos2phi0 / oneMinusE2;
                Double dAlpha2 = sinAlpha0 * cosAlpha0 * (1 + sin2Phi0 - 2 * ellipsoid.EccentricitySquare * sin2Phi0 - ellipsoid.EccentricitySquare * sin2Phi0 * cos2Phi0) / (n2Phi0Cos2phi0 * oneMinusE2);

                Double dPhi3 = sin2Alpha0 * cosAlpha0 * (-1 - 2 * sin2Phi0 + 3 * ellipsoid.EccentricitySquare * sin2Phi0 + 10 * ellipsoid.EccentricitySquare * sin2Phi0 * cos2Phi0) / (mPhi0 * n2Phi0Cos2phi0 * oneMinusE2) - 3 * ellipsoid.EccentricitySquare * Calculator.Cos3(alpha0) * ((cos2Phi0 - sin2Phi0) * (1 - ellipsoid.EccentricitySquare * sin2Phi0) - 4 * ellipsoid.EccentricitySquare * Calculator.Sin3(phi0) * cosPhi0) / (mPhi0 * n2Phi0Cos2phi0 * oneMinusE2);
                Double dLambda3 = 2 * sinAlpha0 * cos2Alpha0 * ((1 - ellipsoid.EccentricitySquare * sin2Phi0) * (1 + 2 * sin2Phi0) - 3 * ellipsoid.EccentricitySquare * sin2Phi0 * cos2Phi0) / (Math.Pow(nPhi0 * cosPhi0, 3) * oneMinusE2) - 2 * Calculator.Sin3(alpha0) * sin2Phi0 / n3Phi0Cos3phi0;
                Double dAlpha3 = sinAlpha0 * cos2Alpha0 * sinPhi0 * (3 - 3 * ellipsoid.EccentricitySquare * sin2Phi0 * (3 - 9 * ellipsoid.EccentricitySquare + 6 * e4) + cos2Phi0 * (2 - 4 * ellipsoid.EccentricitySquare) - 2 * ellipsoid.EccentricitySquare * Calculator.Cos4(phi0)) / (n3Phi0Cos3phi0 * oneMinusE2) + sinAlpha0 * cos2Alpha0 * sin2Phi0 * (1 + sin2Phi0 * cos2Phi0 * (-3 * ellipsoid.EccentricitySquare + 7 * e4) - 2 * e4 * sin2Phi0 * sin2Phi0 * cos2Phi0 + 2 * e4 * sin2Phi0 * cos2Phi0 * cos2Phi0) / (n3Phi0Cos3phi0 * oneMinusE2) - Calculator.Sin3(alpha0) * sinPhi0 * (1 + sin2Phi0 * (1 - 2 * ellipsoid.EccentricitySquare) - ellipsoid.EccentricitySquare * sin2Phi0 * cos2Phi0) / n3Phi0Cos3phi0;

                Double distanceSquare = vector.Distance.BaseValue;
                Double phi = phi0 + dPhi * vector.Distance.BaseValue + dPhi2 * distanceSquare / 2 + dPhi3 * Math.Pow(vector.Distance.BaseValue, 3) / 6;
                Double lambda = lambda0 + dLambda * vector.Distance.BaseValue + dLambda2 * distanceSquare / 2 + dLambda3 * Math.Pow(vector.Distance.BaseValue, 3) / 6;
                Double alpha = alpha0 + dAlpha * vector.Distance.BaseValue + dAlpha2 * distanceSquare / 2 + dAlpha3 * Math.Pow(vector.Distance.BaseValue, 3) / 6;

                return new GeoCoordinate(phi, lambda);
            }
        }

        /// <summary>
        /// Determines a geodetic vector between source and destination coordinates.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="source">The source coordinate.</param>
        /// <param name="destination">The destination coordinate.</param>
        /// <returns>The geodetic vector between source and destination coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The source coordinate is null.
        /// or
        /// The destination coordinate is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Vector computation is only supported on sphere.</exception>
        public static GeoVector GetVector(this Ellipsoid ellipsoid, GeoCoordinate source, GeoCoordinate destination)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException(nameof(ellipsoid), ReferenceMessages.EllipsoidIsNull);
            if (source == null)
                throw new ArgumentNullException(nameof(source), ReferenceMessages.SourceCoordinateIsNull);
            if (destination == null)
                throw new ArgumentNullException(nameof(destination), ReferenceMessages.DestinationCoordinateIsNull);

            if (ellipsoid.IsSphere)
                throw new NotSupportedException(ReferenceMessages.VectorComputationOnlySupportedOnSphere);

            if (!source.IsValid || !destination.IsValid)
                return GeoVector.Undefined;

            if (source.Equals(destination))
                return GeoVector.ZeroVector;

            Double deltaLambda, omega, dist, azimuth;

            // case the destination is a pole
            if (Math.Abs(source.Latitude.BaseValue - Math.PI / 2) <= 1E-10)
            {
                azimuth = Math.PI;
                dist = LengthOfVerticalCurvature(ellipsoid, source.Latitude.BaseValue, destination.Latitude.BaseValue);
            }

            if (Math.Abs(source.Latitude.BaseValue - 3 * Math.PI / 2) <= 1E-10)
            {
                azimuth = 0;
                dist = LengthOfVerticalCurvature(ellipsoid, source.Latitude.BaseValue, destination.Latitude.BaseValue);
            }
            else
            {
                deltaLambda = destination.Longitude.BaseValue - source.Longitude.BaseValue;
                dist = Math.Acos(Math.Sin(source.Latitude.BaseValue) * Math.Sin(destination.Latitude.BaseValue) + Math.Cos(source.Latitude.BaseValue) * Math.Cos(destination.Latitude.BaseValue) * Math.Cos(deltaLambda)) * ellipsoid.SemiMajorAxis.BaseValue;

                // case the vector is north or south oriented
                if (Math.Sin(deltaLambda) == 0)
                {
                    if (Math.Abs(dist - LengthOfVerticalCurvature(ellipsoid, source.Latitude.BaseValue, destination.Latitude.BaseValue)) <= 1E-10)
                        deltaLambda = 0;
                    else
                        deltaLambda = Math.PI;
                }

                omega = Math.Acos((Math.Sin(destination.Latitude.BaseValue) - Math.Sin(source.Latitude.BaseValue) * Math.Cos(dist / ellipsoid.SemiMajorAxis.BaseValue)) / Math.Cos(source.Latitude.BaseValue) / Math.Sin(dist / ellipsoid.SemiMajorAxis.BaseValue)) * Math.Sin(deltaLambda) / Math.Abs(Math.Sin(deltaLambda));

                azimuth = deltaLambda > 0 ? omega : 2 * Math.PI - omega;
            }

            return new GeoVector(azimuth, dist);
        }
    }
}
