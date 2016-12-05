// <copyright file="Angle.cs" company="Eötvös Loránd University (ELTE)">
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
    using System.Globalization;
    using System.Linq;
    using ELTE.AEGIS.Reference.Resources;

    /// <summary>
    /// Represents an angle measure.
    /// </summary>
    public struct Angle : IEquatable<Angle>, IComparable<Angle>
    {
        /// <summary>
        /// Represents the zero <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle Zero = Angle.FromRadian(0);

        /// <summary>
        /// Represents the unknown <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle Undefined = Angle.FromRadian(Double.NaN);

        /// <summary>
        /// Represents the negative infinite <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle NegativeInfinity = Angle.FromRadian(Double.PositiveInfinity);

        /// <summary>
        /// Represents the positive infinite <see cref="Angle" /> value. This field is read-only.
        /// </summary>
        public static readonly Angle PositiveInfinity = Angle.FromRadian(Double.NegativeInfinity);

        /// <summary>
        /// Represents the smallest positive <see cref="Angle" /> value that is greater than zero. This field is read-only.
        /// </summary>
        public static readonly Angle Epsilon = Angle.FromRadian(Double.Epsilon);

        /// <summary>
        /// Represents the <see cref="Angle" /> value of a half circle. This field is read-only.
        /// </summary>
        public static readonly Angle HalfCircle = Angle.FromDegree(180);

        /// <summary>
        /// Represents the <see cref="Angle" /> value of a circle. This field is read-only.
        /// </summary>
        public static readonly Angle Circle = Angle.FromDegree(360);

        /// <summary>
        /// The string format for angles. This field is constant.
        /// </summary>
        private const String AngleStringFormat = "{0}{1}";

        /// <summary>
        /// The value.
        /// </summary>
        private readonly Double value;

        /// <summary>
        /// The unit of measurement.
        /// </summary>
        private readonly UnitOfMeasurement unit;

        /// <summary>
        /// Initializes a new instance of the <see cref="Angle" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <exception cref="System.ArgumentException">The unit of measurement is invalid.</exception>
        public Angle(Double value, UnitOfMeasurement unit)
        {
            if (unit != null && unit.Type != UnitQuantityType.Angle)
                throw new ArgumentException(ReferenceMessages.UnitOfMeasurementIsInvalid, nameof(unit));

            this.value = value;
            this.unit = unit ?? UnitsOfMeasurement.Radian;
        }

        /// <summary>
        /// Gets the value of the angle.
        /// </summary>
        public Double Value { get { return this.value; } }

        /// <summary>
        /// Gets the base value (converted to radians) of the angle.
        /// </summary>
        public Double BaseValue { get { return this.value * this.unit.BaseMultiple; } }

        /// <summary>
        /// Gets the unit of measurement of the angle.
        /// </summary>
        public UnitOfMeasurement Unit { get { return this.unit; } }

        /// <summary>
        /// Gets the value using the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The value converted to the specified unit of measurement.</returns>
        /// <exception cref="System.ArgumentNullException">The unit of measurement is null.</exception>
        /// <exception cref="System.ArgumentException">The unit of measurement is invalid.</exception>
        public Double GetValue(UnitOfMeasurement unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitOfMeasurementIsNull);
            if (unit.Type != UnitQuantityType.Angle)
                throw new ArgumentException(ReferenceMessages.UnitOfMeasurementIsInvalid, nameof(unit));

            if (unit == this.unit)
                return this.value;

            return this.BaseValue / unit.BaseMultiple;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>A <see cref="System.String" /> containing the value and unit symbol of the instance.</returns>
        /// <exception cref="System.ArgumentNullException">The unit of measurement is null.</exception>
        /// <exception cref="System.ArgumentException">The unit of measurement is invalid.</exception>
        public String ToString(UnitOfMeasurement unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitOfMeasurementIsNull);
            if (unit.Type != UnitQuantityType.Angle)
                throw new ArgumentException(ReferenceMessages.UnitOfMeasurementIsInvalid, nameof(unit));

            if (this.Unit != unit)
            {
                return String.Format(CultureInfo.InvariantCulture, AngleStringFormat, this.BaseValue / unit.BaseMultiple, unit.Symbol);
            }
            else
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// Returns the equivalent angle in the specified unit of measurement.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The equivalent angle in the specified unit of measurement.</returns>
        /// <exception cref="System.ArgumentNullException">The unit of measurement is null.</exception>
        /// <exception cref="System.ArgumentException">The unit of measurement is invalid.</exception>
        public Angle ToUnit(UnitOfMeasurement unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit), ReferenceMessages.UnitOfMeasurementIsNull);
            if (unit.Type != UnitQuantityType.Angle)
                throw new ArgumentException(ReferenceMessages.UnitOfMeasurementIsInvalid, nameof(unit));

            if (unit == this.unit)
                return this;

            return new Angle(this.BaseValue / unit.BaseMultiple, unit);
        }

        /// <summary>
        /// Indicates whether this instance and a specified other angle are equal.
        /// </summary>
        /// <param name="other">Another angle to compare to.</param>
        /// <returns><c>true</c> if <paramref name="other" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Angle other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.BaseValue.Equals(other.BaseValue);
        }

        /// <summary>
        /// Compares the current instance with another angle.
        /// </summary>
        /// <param name="other">An angle to compare with this angle.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public Int32 CompareTo(Angle other)
        {
            return this.BaseValue.CompareTo(other.BaseValue);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return (obj is Angle) && this.BaseValue.Equals(((Angle)obj).BaseValue);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return this.BaseValue.GetHashCode() ^ 625911703;
        }

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the value and unit symbol of the instance.</returns>
        public override String ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, AngleStringFormat, this.Value, this.Unit.Symbol);
        }

        /// <summary>
        /// Sums the specified angle instances.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns>The sum of the two angle instances.</returns>
        public static Angle operator +(Angle first, Angle second)
        {
            if (first.Unit.Equals(second.Unit))
                return new Angle(first.Value + second.Value, first.Unit);

            return new Angle(first.BaseValue + second.BaseValue, UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Negates the specified angle instance.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The negate of the angle instance.</returns>
        public static Angle operator -(Angle angle)
        {
            return new Angle(-angle.Value, angle.Unit);
        }

        /// <summary>
        /// Extracts the specified angle instances.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns>The extract of the two angle instances.</returns>
        public static Angle operator -(Angle first, Angle second)
        {
            if (first.Unit.Equals(second.Unit))
                return new Angle(first.Value - second.Value, first.Unit);

            return new Angle(first.BaseValue - second.BaseValue, UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Multiplies the scalar with the specified angle.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>The scalar multiplication of the angle.</returns>
        public static Angle operator *(Double scalar, Angle angle)
        {
            return new Angle(scalar * angle.Value, angle.Unit);
        }

        /// <summary>
        /// Multiplies the specified angle with the scalar.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar multiplication of the angle.</returns>
        public static Angle operator *(Angle angle, Double scalar)
        {
            return new Angle(angle.Value * scalar, angle.Unit);
        }

        /// <summary>
        /// Divides the specified angle with the scalar.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The scalar division of the angle.</returns>
        public static Angle operator /(Angle angle, Double scalar)
        {
            return new Angle(angle.Value / scalar, angle.Unit);
        }

        /// <summary>
        /// Indicates whether the specified angle instances are equal.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the two angle instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Angle first, Angle second)
        {
            return first.BaseValue == second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the specified angle instances are not equal.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if two angle instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Angle first, Angle second)
        {
            return first.BaseValue != second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified angle instance is less than the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first angle instance is less than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Angle first, Angle second)
        {
            return first.BaseValue < second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified angle instance is greater than the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first angle instance is greater than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Angle first, Angle second)
        {
            return first.BaseValue > second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified angle instance is smaller or equal to the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first angle instance is smaller or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Angle first, Angle second)
        {
            return first.BaseValue <= second.BaseValue;
        }

        /// <summary>
        /// Indicates whether the first specified angle instance is greater or equal to the second.
        /// </summary>
        /// <param name="first">The first angle.</param>
        /// <param name="second">The second angle.</param>
        /// <returns><c>true</c> if the first angle instance is greater or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Angle first, Angle second)
        {
            return first.BaseValue >= second.BaseValue;
        }

        /// <summary>
        /// Converts the specified angle instance to a <see cref="System.Double" /> value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="System.Double" /> value of the specified angle instance.</returns>
        public static explicit operator Double(Angle value)
        {
            return value.BaseValue;
        }

        /// <summary>
        /// Converts the specified <see cref="System.Double" /> instance to a angle value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The angle value of the specified <see cref="System.Double" /> instance.</returns>
        public static explicit operator Angle(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.ArcMinute" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.ArcMinute" />.</returns>
        public static Angle FromArcMinute(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.ArcMinute);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.ArcSecond" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.ArcSecond" />.</returns>
        public static Angle FromArcSecond(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.ArcSecond);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.CentesimalMinute" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.CentesimalMinute" />.</returns>
        public static Angle FromCentesimalMinute(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.CentesimalMinute);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.CentesimalSecond" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.CentesimalSecond" />.</returns>
        public static Angle FromCentesimalSecond(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.CentesimalSecond);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.Degree" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.Degree" />.</returns>
        public static Angle FromDegree(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Degree);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in degrees and minutes.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="arcMinute">The arc minutes.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.Degree" />.</returns>
        public static Angle FromDegree(Int32 degree, Double arcMinute)
        {
            if (degree < 0)
                return new Angle(degree - arcMinute / 60.0, UnitsOfMeasurement.Degree);
            else
                return new Angle(degree + arcMinute / 60.0, UnitsOfMeasurement.Degree);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in degrees, minutes and seconds.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <param name="arcMinute">The arc minutes.</param>
        /// <param name="arcSecond">The arc seconds.</param>
        /// <returns>A angle instance with the value specified in degrees, minutes and seconds.</returns>
        public static Angle FromDegree(Int32 degree, Int32 arcMinute, Double arcSecond)
        {
            if (degree < 0)
                return new Angle(degree - arcMinute / 60.0 - arcSecond / 3600, UnitsOfMeasurement.Degree);
            else
                return new Angle(degree + arcMinute / 60.0 + arcSecond / 3600, UnitsOfMeasurement.Degree);
        }

        /// <summary>
        /// Creates an angle from value specified in <see cref="UnitsOfMeasurement.Gon" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.Gon" />.</returns>
        public static Angle FromGon(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Gon);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.Grad" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.Grad" />.</returns>
        public static Angle FromGrad(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Grad);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.MicroRadian" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.MicroRadian" />.</returns>
        public static Angle FromMicroRadian(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.MicroRadian);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.MilliarcSecond" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.MilliarcSecond" />.</returns>
        public static Angle FromMilliarcSecond(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.MilliarcSecond);
        }

        /// <summary>
        /// Initializes a new instance of the angle struct from the value specified in <see cref="UnitsOfMeasurement.Radian" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A angle instance with the value specified in <see cref="UnitsOfMeasurement.Radian" />.</returns>
        public static Angle FromRadian(Double value)
        {
            return new Angle(value, UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Determines whether the specified angle instance is valid.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns><c>true</c> if <paramref name="angle" /> is not an unknown value; otherwise, <c>false</c>.</returns>
        public static Boolean IsValid(Angle angle)
        {
            return !Double.IsNaN(angle.value);
        }

        /// <summary>
        /// Determines the maximum of the specified angle instances.
        /// </summary>
        /// <param name="angles">The angle values.</param>
        /// <returns>The angle instance representing the maximum of <paramref name="angles" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array of angles is null.</exception>
        public static Angle Max(params Angle[] angles)
        {
            if (angles == null)
                throw new ArgumentNullException(nameof(angles), ReferenceMessages.AngleArrayIsNull);
            if (angles.Length == 0)
                return Angle.Undefined;

            return new Angle(angles.Max(angle => angle.BaseValue), UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Determines the maximum of the specified angle instances.
        /// </summary>
        /// <param name="angles">The angle values.</param>
        /// <returns>The angle instance representing the maximum of <paramref name="angles" />.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of angles is null.</exception>
        public static Angle Max(IEnumerable<Angle> angles)
        {
            if (angles == null)
                throw new ArgumentNullException(nameof(angles), ReferenceMessages.AngleCollectionIsNull);
            if (!angles.Any())
                return Angle.Undefined;

            return new Angle(angles.Max(angle => angle.BaseValue), UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Determines the minimum of the specified angle instances.
        /// </summary>
        /// <param name="angles">The angle values.</param>
        /// <returns>The angle instance representing the minimum of <paramref name="angles" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array of angles is null.</exception>
        public static Angle Min(params Angle[] angles)
        {
            if (angles == null)
                throw new ArgumentNullException(nameof(angles), ReferenceMessages.AngleArrayIsNull);
            if (angles.Length == 0)
                return Angle.Undefined;

            return new Angle(angles.Min(angle => angle.BaseValue), UnitsOfMeasurement.Radian);
        }

        /// <summary>
        /// Determines the minimum of the specified angle instances.
        /// </summary>
        /// <param name="angles">The angle values.</param>
        /// <returns>The angle instance representing the minimum of <paramref name="angles" />.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of angles is null.</exception>
        public static Angle Min(IEnumerable<Angle> angles)
        {
            if (angles == null)
                throw new ArgumentNullException(nameof(angles), ReferenceMessages.AngleArrayIsNull);
            if (!angles.Any())
                return Angle.Undefined;

            return new Angle(angles.Min(angle => angle.BaseValue), UnitsOfMeasurement.Radian);
        }
    }
}
