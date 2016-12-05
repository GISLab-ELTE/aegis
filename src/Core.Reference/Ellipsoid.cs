// <copyright file="Ellipsoid.cs" company="Eötvös Loránd University (ELTE)">
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
    using ELTE.AEGIS.Numerics;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents an ellipsoid used in planetary surface modeling.
    /// </summary>
    public sealed class Ellipsoid : IdentifiedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="semiMinorAxis">The semi-minor axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <param name="flattening">The flattening.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        private Ellipsoid(String identifier, String name, String remarks, String[] aliases, Double semiMajorAxis, Double semiMinorAxis, Double inverseFlattening, Double flattening, Double eccentricity)
            : this(identifier, name, remarks, aliases, new Length(semiMajorAxis, UnitsOfMeasurement.Metre), new Length(semiMinorAxis, UnitsOfMeasurement.Metre), inverseFlattening, flattening, eccentricity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="semiMinorAxis">The semi-minor axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <param name="flattening">The flattening.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        private Ellipsoid(String identifier, String name, String remarks, String[] aliases, Length semiMajorAxis, Length semiMinorAxis, Double inverseFlattening, Double flattening, Double eccentricity)
            : base(identifier, name, remarks, aliases)
        {
            this.SemiMajorAxis = semiMajorAxis;
            this.SemiMinorAxis = semiMinorAxis;
            this.InverseFattening = inverseFlattening;
            this.Flattening = 1 / inverseFlattening;
            this.IsSphere = inverseFlattening == 1;

            this.Eccentricity = eccentricity;
            this.EccentricitySquare = eccentricity * eccentricity;
            this.SecondEccentricity = Math.Sqrt(this.EccentricitySquare / (1 - this.EccentricitySquare));
            this.SecondEccentricitySquare = this.SecondEccentricity * this.SecondEccentricity;
            this.RadiusOfAuthalicSphere = new Length(this.SemiMajorAxis.Value * Math.Sqrt((1 - (1 - this.EccentricitySquare) / (2 * this.Eccentricity) * Math.Log((1 - this.Eccentricity) / (1 + this.Eccentricity))) * 0.5), this.SemiMajorAxis.Unit);
        }

        /// <summary>
        /// Gets the semi-major axis.
        /// </summary>
        public Length SemiMajorAxis { get; private set; }

        /// <summary>
        /// Gets the semi-minor axis.
        /// </summary>
        public Length SemiMinorAxis { get; private set; }

        /// <summary>
        /// Gets the radius of the authalic sphere.
        /// </summary>
        public Length RadiusOfAuthalicSphere { get; private set; }

        /// <summary>
        /// Gets the inverse fattening.
        /// </summary>
        public Double InverseFattening { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the ellipsoid is a sphere.
        /// </summary>
        public Boolean IsSphere { get; private set; }

        /// <summary>
        /// Gets the flattening.
        /// </summary>
        public Double Flattening { get; private set; }

        /// <summary>
        /// Gets the eccentricity.
        /// </summary>
        public Double Eccentricity { get; private set; }

        /// <summary>
        /// Gets the square of the eccentricity.
        /// </summary>
        public Double EccentricitySquare { get; private set; }

        /// <summary>
        /// Gets the second eccentricity.
        /// </summary>
        public Double SecondEccentricity { get; private set; }

        /// <summary>
        /// Gets the square of the second eccentricity.
        /// </summary>
        public Double SecondEccentricitySquare { get; private set; }

        /// <summary>
        /// Determines the radius of meridian curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of meridian curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Length RadiusOfMeridianCurvature(Angle latitude)
        {
            if (latitude.BaseValue > Math.PI / 2 || latitude.BaseValue < -Math.PI / 2)
                throw new ArgumentException(ReferenceMessages.LatitudeInvalid, nameof(latitude));

            if (this.IsSphere)
                return this.SemiMajorAxis;
            else
                return new Length(this.SemiMajorAxis.Value * (1 - this.EccentricitySquare) / Math.Pow(1 - this.EccentricitySquare * Math.Sin(latitude.BaseValue) * Math.Sin(latitude.BaseValue), 1.5), this.SemiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the radius of meridian curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of meridian curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Double RadiusOfMeridianCurvature(Double latitude)
        {
            if (latitude > Math.PI / 2 || latitude < -Math.PI / 2)
                throw new ArgumentException(ReferenceMessages.LatitudeInvalid, nameof(latitude));

            if (this.IsSphere)
                return this.SemiMajorAxis.Value;
            else
                return this.SemiMajorAxis.Value * (1 - this.EccentricitySquare) / Math.Pow(1 - this.EccentricitySquare * Math.Sin(latitude) * Math.Sin(latitude), 1.5);
        }

        /// <summary>
        /// Determines the radius of prime vertical curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of prime vertical curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Length RadiusOfPrimeVerticalCurvature(Angle latitude)
        {
            if (latitude.BaseValue > Math.PI / 2 || latitude.BaseValue < -Math.PI / 2)
                throw new ArgumentException(ReferenceMessages.LatitudeInvalid, nameof(latitude));

            if (this.IsSphere)
                return this.SemiMajorAxis;
            else
                return new Length(this.SemiMajorAxis.Value / Math.Sqrt(1 - this.EccentricitySquare * Math.Sin(latitude.BaseValue) * Math.Sin(latitude.BaseValue)), this.SemiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the radius of prime vertical curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of prime vertical curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Double RadiusOfPrimeVerticalCurvature(Double latitude)
        {
            if (latitude > Math.PI / 2 || latitude < -Math.PI / 2)
                throw new ArgumentException(ReferenceMessages.LatitudeInvalid, nameof(latitude));

            if (this.IsSphere)
                return this.SemiMajorAxis.Value;
            else
                return this.SemiMajorAxis.Value / Math.Sqrt(1 - this.EccentricitySquare * Math.Sin(latitude) * Math.Sin(latitude));
        }

        /// <summary>
        /// Determines the radius of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of parallel curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Length RadiusOfParalellCurvature(Angle latitude)
        {
            if (latitude.BaseValue > Math.PI / 2 || latitude.BaseValue < -Math.PI / 2)
                throw new ArgumentException(ReferenceMessages.LatitudeInvalid, nameof(latitude));

            if (this.IsSphere)
                return this.SemiMajorAxis * Math.Cos(latitude.BaseValue);
            else
                return new Length(this.SemiMajorAxis.Value * Math.Cos(latitude.BaseValue) / Math.Sqrt(1 - this.EccentricitySquare * Math.Sin(latitude.BaseValue) * Math.Sin(latitude.BaseValue)), this.SemiMajorAxis.Unit);
        }

        /// <summary>
        /// Determines the radius of parallel curvature at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of parallel curvature at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Double RadiusOfParalellCurvature(Double latitude)
        {
            if (latitude > Math.PI / 2 || latitude < -Math.PI / 2)
                throw new ArgumentException(ReferenceMessages.LatitudeInvalid, nameof(latitude));

            if (this.IsSphere)
                return this.SemiMajorAxis.Value * Math.Cos(latitude);
            else
                return this.SemiMajorAxis.Value * Math.Cos(latitude) / Math.Sqrt(1 - this.EccentricitySquare * Math.Sin(latitude) * Math.Sin(latitude));
        }

        /// <summary>
        /// Determines the radius of conformal sphere at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The radius of conformal sphere at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Length RadiusOfConformalSphere(Angle latitude)
        {
            Length rho = this.RadiusOfMeridianCurvature(latitude);
            Length nu = this.RadiusOfPrimeVerticalCurvature(latitude);
            return new Length(Math.Sqrt(rho.Value * nu.Value), rho.Unit);
        }

        /// <summary>
        /// Determines the radius of conformal sphere at a specified latitude.
        /// </summary>
        /// <param name="latitude">The latitude (defined in <see cref="UnitsOfMeasurement.Radian" />).</param>
        /// <returns>The radius of conformal sphere at the specified latitude.</returns>
        /// <exception cref="System.ArgumentException">The latitude is invalid.</exception>
        public Double RadiusOfConformalSphere(Double latitude)
        {
            return Math.Sqrt(this.RadiusOfMeridianCurvature(latitude) * this.RadiusOfPrimeVerticalCurvature(latitude));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="semiMinorAxis">The semi-minor axis.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">The semi-minor axis must be measured in the same measurement unit as the semi-major axis.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is grater than the semi-major axis.
        /// </exception>
        public static Ellipsoid FromSemiMinorAxis(String identifier, String name, Length semiMajorAxis, Length semiMinorAxis)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (semiMinorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisIsEqualToOrLessThan0);
            if (semiMajorAxis < semiMinorAxis)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisGreaterThaneSemiMajorAxis);
            if (semiMajorAxis.Unit != semiMinorAxis.Unit)
                throw new ArgumentException(ReferenceMessages.SemiMajorAxisUnitNotEqualToSemiMinorAxisUnit, nameof(semiMinorAxis));

            Double inverseFlattening = (semiMajorAxis == semiMinorAxis) ? 1 : semiMajorAxis.Value / (semiMajorAxis.Value - semiMinorAxis.Value);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="semiMinorAxis">The semi-minor axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is grater than the semi-major axis.
        /// </exception>
        public static Ellipsoid FromSemiMinorAxis(String identifier, String name, Double semiMajorAxis, Double semiMinorAxis)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (semiMinorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisIsEqualToOrLessThan0);
            if (semiMajorAxis < semiMinorAxis)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisGreaterThaneSemiMajorAxis);

            Double inverseFlattening = (semiMajorAxis == semiMinorAxis) ? 1 : semiMajorAxis / (semiMajorAxis - semiMinorAxis);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="semiMinorAxis">The semi-minor axis.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">The semi-minor axis must be measured in the same measurement unit as the semi-major axis.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is grater than the semi-major axis.
        /// </exception>
        public static Ellipsoid FromSemiMinorAxis(String identifier, String name, String remarks, String[] aliases, Length semiMajorAxis, Length semiMinorAxis)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (semiMinorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisIsEqualToOrLessThan0);
            if (semiMajorAxis < semiMinorAxis)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisGreaterThaneSemiMajorAxis);
            if (semiMajorAxis.Unit != semiMinorAxis.Unit)
                throw new ArgumentException(ReferenceMessages.SemiMajorAxisUnitNotEqualToSemiMinorAxisUnit, nameof(semiMinorAxis));

            Double inverseFlattening = (semiMajorAxis == semiMinorAxis) ? 1 : semiMajorAxis.Value / (semiMajorAxis.Value - semiMinorAxis.Value);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="semiMinorAxis">The semi-minor axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the semi-minor axis.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is equal to or less than 0.
        /// or
        /// The semi-minor axis is grater than the semi-major axis.
        /// </exception>
        public static Ellipsoid FromSemiMinorAxis(String identifier, String name, String remarks, String[] aliases, Double semiMajorAxis, Double semiMinorAxis)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (semiMinorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisIsEqualToOrLessThan0);
            if (semiMajorAxis < semiMinorAxis)
                throw new ArgumentOutOfRangeException(nameof(semiMinorAxis), ReferenceMessages.SemiMinorAxisGreaterThaneSemiMajorAxis);

            Double inverseFlattening = (semiMajorAxis == semiMinorAxis) ? 1 : semiMajorAxis / (semiMajorAxis - semiMinorAxis);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The inverse flattening is less than 1.
        /// </exception>
        public static Ellipsoid FromInverseFlattening(String identifier, String name, Length semiMajorAxis, Double inverseFlattening)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (inverseFlattening < 1)
                throw new ArgumentOutOfRangeException(nameof(inverseFlattening), ReferenceMessages.InverseFlatteningLessThan1);

            Length semiMinorAxis = new Length((inverseFlattening != 0) ? semiMajorAxis.Value * (1 - 1 / inverseFlattening) : semiMajorAxis.Value, semiMajorAxis.Unit);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// inverseThe inverse flattening is less than 1.
        /// </exception>
        public static Ellipsoid FromInverseFlattening(String identifier, String name, Double semiMajorAxis, Double inverseFlattening)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (inverseFlattening < 1)
                throw new ArgumentOutOfRangeException(nameof(inverseFlattening), ReferenceMessages.InverseFlatteningLessThan1);

            Double semiMinorAxis = (inverseFlattening != 0) ? semiMajorAxis * (1 - 1 / inverseFlattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The inverse flattening is less than 1.
        /// </exception>
        public static Ellipsoid FromInverseFlattening(String identifier, String name, String remarks, String[] aliases, Length semiMajorAxis, Double inverseFlattening)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (inverseFlattening < 1)
                throw new ArgumentOutOfRangeException(nameof(inverseFlattening), ReferenceMessages.InverseFlatteningLessThan1);

            Length semiMinorAxis = new Length((inverseFlattening != 0) ? semiMajorAxis.Value * (1 - 1 / inverseFlattening) : semiMajorAxis.Value, semiMajorAxis.Unit);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="inverseFlattening">The inverse flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the inverse flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// inverseThe inverse flattening is less than 1.
        /// </exception>
        public static Ellipsoid FromInverseFlattening(String identifier, String name, String remarks, String[] aliases, Double semiMajorAxis, Double inverseFlattening)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (inverseFlattening < 1)
                throw new ArgumentOutOfRangeException(nameof(inverseFlattening), ReferenceMessages.InverseFlatteningLessThan1);

            Double semiMinorAxis = (inverseFlattening != 0) ? semiMajorAxis * (1 - 1 / inverseFlattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, inverseFlattening, inverseFlattening == 1 ? 0 : 1 / inverseFlattening, Math.Sqrt(2 * 1 / inverseFlattening - 1 / (inverseFlattening * inverseFlattening)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="flattening">The flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The flattening is equal to or less than 0.
        /// or
        /// The flattening greater than 1.
        /// </exception>
        public static Ellipsoid FromFlattening(String identifier, String name, Length semiMajorAxis, Double flattening)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (flattening <= 0)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsEqualToOrLessThan0);
            if (flattening > 1)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsGreaterThan1);

            Length semiMinorAxis = new Length((flattening != 1) ? semiMajorAxis.Value * (1 - flattening) : semiMajorAxis.Value, UnitsOfMeasurement.Metre);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 / flattening, flattening, Math.Sqrt(2 * flattening - flattening * flattening));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="flattening">The flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The flattening is equal to or less than 0.
        /// or
        /// The flattening greater than 1.
        /// </exception>
        public static Ellipsoid FromFlattening(String identifier, String name, Double semiMajorAxis, Double flattening)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (flattening <= 0)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsEqualToOrLessThan0);
            if (flattening > 1)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsGreaterThan1);

            Double semiMinorAxis = (flattening != 1) ? semiMajorAxis * (1 - flattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 / flattening, flattening, Math.Sqrt(2 * flattening - flattening * flattening));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="flattening">The flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The flattening is equal to or less than 0.
        /// or
        /// The flattening greater than 1.
        /// </exception>
        public static Ellipsoid FromFlattening(String identifier, String name, String remarks, String[] aliases, Length semiMajorAxis, Double flattening)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (flattening <= 0)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsEqualToOrLessThan0);
            if (flattening > 1)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsGreaterThan1);

            Length semiMinorAxis = new Length((flattening != 1) ? semiMajorAxis.Value * (1 - flattening) : semiMajorAxis.Value, UnitsOfMeasurement.Metre);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 / flattening, flattening, Math.Sqrt(2 * flattening - flattening * flattening));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the flattening.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="flattening">The flattening.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the flattening.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The flattening is equal to or less than 0.
        /// or
        /// The flattening greater than 1.
        /// </exception>
        public static Ellipsoid FromFlattening(String identifier, String name, String remarks, String[] aliases, Double semiMajorAxis, Double flattening)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (flattening <= 0)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsEqualToOrLessThan0);
            if (flattening > 1)
                throw new ArgumentOutOfRangeException(nameof(flattening), ReferenceMessages.FlatteningIsGreaterThan1);

            Double semiMinorAxis = (flattening != 1) ? semiMajorAxis * (1 - flattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 / flattening, flattening, Math.Sqrt(2 * flattening - flattening * flattening));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The eccentricity is equal to or less than 0.
        /// or
        /// The eccentricity greater than 1.
        /// </exception>
        public static Ellipsoid FromEccentricity(String identifier, String name, Length semiMajorAxis, Double eccentricity)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (eccentricity <= 0)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsEqualToOrLessThan0);
            if (eccentricity > 1)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsGreaterThan1);

            Double flattening = 1 - Math.Sqrt(1 - eccentricity * eccentricity);
            Length semiMinorAxis = new Length((flattening != 1) ? semiMajorAxis.Value * (1 - flattening) : semiMajorAxis.Value, UnitsOfMeasurement.Metre);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 - Math.Sqrt(1 - eccentricity * eccentricity), flattening, eccentricity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The eccentricity is equal to or less than 0.
        /// or
        /// The eccentricity greater than 1.
        /// </exception>
        public static Ellipsoid FromEccentricity(String identifier, String name, Double semiMajorAxis, Double eccentricity)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (eccentricity <= 0)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsEqualToOrLessThan0);
            if (eccentricity > 1)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsGreaterThan1);

            Double flattening = 1 - Math.Sqrt(1 - eccentricity * eccentricity);
            Double semiMinorAxis = (flattening != 1) ? semiMajorAxis * (1 - flattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 - Math.Sqrt(1 - eccentricity * eccentricity), flattening, eccentricity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The eccentricity is equal to or less than 0.
        /// or
        /// The eccentricity greater than 1.
        /// </exception>
        public static Ellipsoid FromEccentricity(String identifier, String name, String remarks, String[] aliases, Length semiMajorAxis, Double eccentricity)
        {
            if (semiMajorAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (eccentricity <= 0)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsEqualToOrLessThan0);
            if (eccentricity > 1)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsGreaterThan1);

            Double flattening = 1 - Math.Sqrt(1 - eccentricity * eccentricity);
            Length semiMinorAxis = new Length((flattening != 1) ? semiMajorAxis.Value * (1 - flattening) : semiMajorAxis.Value, UnitsOfMeasurement.Metre);

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 - Math.Sqrt(1 - eccentricity * eccentricity), flattening, eccentricity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiMajorAxis">The semi-major axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class based on the eccentricity.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The semi-major axis is equal to or less than 0.
        /// or
        /// The eccentricity is equal to or less than 0.
        /// or
        /// The eccentricity greater than 1.
        /// </exception>
        public static Ellipsoid FromEccentricity(String identifier, String name, String remarks, String[] aliases, Double semiMajorAxis, Double eccentricity)
        {
            if (semiMajorAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiMajorAxis), ReferenceMessages.SemiMajorAxisIsEqualToOrLessThan0);
            if (eccentricity <= 0)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsEqualToOrLessThan0);
            if (eccentricity > 1)
                throw new ArgumentOutOfRangeException(nameof(eccentricity), ReferenceMessages.EccentricityIsGreaterThan1);

            Double flattening = 1 - Math.Sqrt(1 - eccentricity * eccentricity);
            Double semiMinorAxis = (flattening != 1) ? semiMajorAxis * (1 - flattening) : semiMajorAxis;

            return new Ellipsoid(identifier, name, null, null, semiMajorAxis, semiMinorAxis, 1 - Math.Sqrt(1 - eccentricity * eccentricity), flattening, eccentricity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class as a sphere.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiAxis">The semi axis.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class as a sphere.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The semi axis is equal to or less than 0.</exception>
        public static Ellipsoid FromSphere(String identifier, String name, Length semiAxis)
        {
            if (semiAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiAxis), ReferenceMessages.SemiAxisIsEqualToOrLessThan0);

            return new Ellipsoid(identifier, name, null, null, semiAxis, semiAxis, 1, 0, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class as a sphere.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="semiAxis">The semi axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class as a sphere.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The semi axis is equal to or less than 0.</exception>
        public static Ellipsoid FromSphere(String identifier, String name, Double semiAxis)
        {
            if (semiAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiAxis), ReferenceMessages.SemiAxisIsEqualToOrLessThan0);

            return new Ellipsoid(identifier, name, null, null, semiAxis, semiAxis, 1, 0, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class as a sphere.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiAxis">The semi axis.</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class as a sphere.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The semi axis is equal to or less than 0.</exception>
        public static Ellipsoid FromSphere(String identifier, String name, String remarks, String[] aliases, Length semiAxis)
        {
            if (semiAxis.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiAxis), ReferenceMessages.SemiAxisIsEqualToOrLessThan0);

            return new Ellipsoid(identifier, name, null, null, semiAxis, semiAxis, 1, 0, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ellipsoid" /> class as a sphere.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="semiAxis">The semi axis (defined in <see cref="UnitsOfMeasurement.Metre" />).</param>
        /// <returns>A new instance of the <see cref="Ellipsoid" /> class as a sphere.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The semi axis is equal to or less than 0.</exception>
        public static Ellipsoid FromSphere(String identifier, String name, String remarks, String[] aliases, Double semiAxis)
        {
            if (semiAxis <= 0)
                throw new ArgumentOutOfRangeException(nameof(semiAxis), ReferenceMessages.SemiAxisIsEqualToOrLessThan0);

            return new Ellipsoid(identifier, name, null, null, semiAxis, semiAxis, 1, 0, 0);
        }
    }
}
