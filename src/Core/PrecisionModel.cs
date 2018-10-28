// <copyright file="PrecisionModel.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AEGIS.Collections;
    using AEGIS.Numerics;
    using AEGIS.Resources;

    /// <summary>
    /// Represents a type defining precision of <see cref="IGeometry" /> instances.
    /// </summary>
    public class PrecisionModel : IComparable<PrecisionModel>, IEquatable<PrecisionModel>
    {
        /// <summary>
        /// The string for fixed precision models. This field is constant.
        /// </summary>
        private const String FixedPrecisionModelString = "FIXED ({0})";

        /// <summary>
        /// The string for floating precision models. This field is constant.
        /// </summary>
        private const String FloatingPrecisionModelString = "FLOATING";

        /// <summary>
        /// The string for floating single precision models. This field is constant.
        /// </summary>
        private const String FloatingSinglePrecisionModelString = "FLOATING (SINGLE)";

        /// <summary>
        /// The base tolerance value.
        /// </summary>
        private Double baseTolerance;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecisionModel" /> class.
        /// </summary>
        public PrecisionModel()
        {
            this.ModelType = PrecisionModelType.Floating;
            this.baseTolerance = 1 / Math.Pow(10, this.MaximumSignificantDigits - 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecisionModel" /> class.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        public PrecisionModel(PrecisionModelType modelType)
        {
            this.ModelType = modelType;
            this.baseTolerance = 1 / Math.Pow(10, this.MaximumSignificantDigits - 1);

            if (modelType == PrecisionModelType.Fixed)
            {
                this.Scale = 1.0;
                this.baseTolerance = 0.5;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrecisionModel" /> class.
        /// </summary>
        /// <param name="scale">The scale of the model.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The scale is equal to or less than 0.</exception>
        public PrecisionModel(Double scale)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale), CoreMessages.ScaleIsEqualToOrLessThan0);

            this.ModelType = PrecisionModelType.Fixed;
            this.Scale = scale;
            this.baseTolerance = 0.5 / this.Scale;
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>The type of the precision model.</value>
        public PrecisionModelType ModelType { get; private set; }

        /// <summary>
        /// Gets the maximum number of significant digits in the model.
        /// </summary>
        /// <value>The maximum number of significant digits in the precision model.</value>
        public Int32 MaximumSignificantDigits
        {
            get
            {
                switch (this.ModelType)
                {
                    case PrecisionModelType.FloatingSingle:
                        return 6;
                    case PrecisionModelType.Floating:
                        return 16;
                    default: // PrecisionModelType.Fixed
                        return 1 + Math.Max(0, (Int32)Math.Ceiling(Math.Log(this.Scale) / Math.Log(10)));
                }
            }
        }

        /// <summary>
        /// Gets the maximum precise value in the model.
        /// </summary>
        /// <value>The greatest precise value in the precision model.</value>
        public Double MaximumPreciseValue
        {
            get
            {
                switch (this.ModelType)
                {
                    case PrecisionModelType.FloatingSingle:
                        return 8388607.0;
                    case PrecisionModelType.Floating:
                        return 9007199254740992.0;
                    default: // PrecisionModelType.Fixed
                        return Math.Floor((9007199254740992.5 * this.Scale) / this.Scale);
                }
            }
        }

        /// <summary>
        /// Gets the scale of the fixed precision model.
        /// </summary>
        /// <value>The scale of the precision model if the type is fixed, otherwise <c>0</c>.</value>
        public Double Scale { get; private set; }

        /// <summary>
        /// Gets the smallest positive value representable by the specified precision.
        /// </summary>
        /// <value>The smallest positive value greater than 0, which is representable by the specified precision.</value>
        public Double Epsilon
        {
            get
            {
                switch (this.ModelType)
                {
                    case PrecisionModelType.FloatingSingle:
                        return Single.Epsilon;
                    case PrecisionModelType.Floating:
                        return Double.Epsilon;
                    default:
                        return this.Scale;
                }
            }
        }

        /// <summary>
        /// Rounds the specified value to match the precision model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The precise value.</returns>
        public Double MakePrecise(Double value)
        {
            if (Double.IsNaN(value))
                return value;

            switch (this.ModelType)
            {
                case PrecisionModelType.FloatingSingle:
                    return (Single)value;
                case PrecisionModelType.Fixed:
                    return Math.Floor((value * this.Scale) + 0.5) / this.Scale;
                default:
                    return value;
            }
        }

        /// <summary>
        /// Rounds the specified coordinate to match the precision model.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The precise coordinate.</returns>
        public Coordinate MakePrecise(Coordinate coordinate)
        {
            if (coordinate == null)
                return null;

            switch (this.ModelType)
            {
                case PrecisionModelType.FloatingSingle:
                    return new Coordinate((Single)coordinate.X, (Single)coordinate.Y, (Single)coordinate.Z);
                case PrecisionModelType.Fixed:
                    return new Coordinate(Math.Floor((coordinate.X * this.Scale) + 0.5) / this.Scale,
                                          Math.Floor((coordinate.Y * this.Scale) + 0.5) / this.Scale,
                                          Math.Floor((coordinate.Z * this.Scale) + 0.5) / this.Scale);
                default:
                    return coordinate;
            }
        }

        /// <summary>
        /// Rounds the specified coordinate vector to match the precision model.
        /// </summary>
        /// <param name="vector">The coordinate vector.</param>
        /// <returns>The precise coordinate vector.</returns>
        public CoordinateVector MakePrecise(CoordinateVector vector)
        {
            if (vector == null)
                return null;

            switch (this.ModelType)
            {
                case PrecisionModelType.FloatingSingle:
                    return new CoordinateVector((Single)vector.X, (Single)vector.Y, (Single)vector.Z);
                case PrecisionModelType.Fixed:
                    return new CoordinateVector(Math.Floor((vector.X * this.Scale) + 0.5) / this.Scale,
                                                Math.Floor((vector.Y * this.Scale) + 0.5) / this.Scale,
                                                Math.Floor((vector.Z * this.Scale) + 0.5) / this.Scale);
                default:
                    return vector;
            }
        }

        /// <summary>
        /// Rounds the specified list of coordinates to match the precision model.
        /// </summary>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <returns>The precise of precise coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of coordinates is null.</exception>
        public IReadOnlyList<Coordinate> MakePrecise(IReadOnlyList<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            if (this.ModelType == PrecisionModelType.Floating)
                return coordinates;

            Coordinate[] preciseCoordinates = new Coordinate[coordinates.Count];

            if (this.ModelType == PrecisionModelType.Fixed)
            {
                for (Int32 index = 0; index < coordinates.Count; index++)
                {
                    if (coordinates[index] == null)
                        continue;

                    preciseCoordinates[index] = new Coordinate(Math.Floor((coordinates[index].X * this.Scale) + 0.5) / this.Scale,
                                                               Math.Floor((coordinates[index].Y * this.Scale) + 0.5) / this.Scale,
                                                               Math.Floor((coordinates[index].Z * this.Scale) + 0.5) / this.Scale);
                }
            }
            else
            {
                for (Int32 index = 0; index < coordinates.Count; index++)
                {
                    if (coordinates[index] == null)
                        continue;

                    preciseCoordinates[index] = new Coordinate((Single)coordinates[index].X,
                                                               (Single)coordinates[index].Y,
                                                               (Single)coordinates[index].Z);
                }
            }

            return preciseCoordinates;
        }

        /// <summary>
        /// Rounds the specified collection of coordinates to match the precision model.
        /// </summary>
        /// <param name="coordinates">The collection of coordinates.</param>
        /// <returns>The collection of precise coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of coordinates is null.</exception>
        public IEnumerable<Coordinate> MakePrecise(IEnumerable<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException(nameof(coordinates));

            if (this.ModelType == PrecisionModelType.Floating)
                return coordinates;

            if (this.ModelType == PrecisionModelType.Fixed)
            {
                return coordinates.Select(coordinate => new Coordinate(Math.Floor((coordinate.X * this.Scale) + 0.5) / this.Scale,
                                                                       Math.Floor((coordinate.Y * this.Scale) + 0.5) / this.Scale,
                                                                       Math.Floor((coordinate.Z * this.Scale) + 0.5) / this.Scale));
            }
            else
            {
                return coordinates.Select(coordinate => new Coordinate((Single)coordinate.X,
                                                                       (Single)coordinate.Y,
                                                                       (Single)coordinate.Z));
            }
        }

        /// <summary>
        /// Rounds the specified list of coordinate vectors to match the precision model.
        /// </summary>
        /// <param name="vectors">The list of vectors.</param>
        /// <returns>The list of precise coordinate vectors.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of vectors is null.</exception>
        public IReadOnlyList<CoordinateVector> MakePrecise(IReadOnlyList<CoordinateVector> vectors)
        {
            if (vectors == null)
                throw new ArgumentNullException(nameof(vectors));

            if (this.ModelType == PrecisionModelType.Floating)
                return vectors;

            CoordinateVector[] preciseVectors = new CoordinateVector[vectors.Count];

            if (this.ModelType == PrecisionModelType.Fixed)
            {
                for (Int32 index = 0; index < vectors.Count; index++)
                {
                    if (vectors[index] == null)
                        continue;

                    preciseVectors[index] = new CoordinateVector(Math.Floor((vectors[index].X * this.Scale) + 0.5) / this.Scale,
                                                                 Math.Floor((vectors[index].Y * this.Scale) + 0.5) / this.Scale,
                                                                 Math.Floor((vectors[index].Z * this.Scale) + 0.5) / this.Scale);
                }
            }
            else
            {
                for (Int32 index = 0; index < vectors.Count; index++)
                {
                    if (vectors[index] == null)
                        continue;

                    preciseVectors[index] = new CoordinateVector((Single)vectors[index].X,
                                                                 (Single)vectors[index].Y,
                                                                 (Single)vectors[index].Z);
                }
            }

            return preciseVectors;
        }

        /// <summary>
        /// Rounds the specified collection of coordinate vectors to match the precision model.
        /// </summary>
        /// <param name="vectors">The collection of coordinate vectors.</param>
        /// <returns>The collection of precise coordinate vectors.</returns>
        /// <exception cref="System.ArgumentNullException">The collection of vectors is null.</exception>
        public IEnumerable<CoordinateVector> MakePrecise(IEnumerable<CoordinateVector> vectors)
        {
            if (vectors == null)
                throw new ArgumentNullException(nameof(vectors));

            if (this.ModelType == PrecisionModelType.Floating)
                return vectors;

            if (this.ModelType == PrecisionModelType.Fixed)
            {
                return vectors.Select(vector => new CoordinateVector(Math.Floor((vector.X * this.Scale) + 0.5) / this.Scale,
                                                                     Math.Floor((vector.Y * this.Scale) + 0.5) / this.Scale,
                                                                     Math.Floor((vector.Z * this.Scale) + 0.5) / this.Scale));
            }
            else
            {
                return vectors.Select(vector => new CoordinateVector((Single)vector.X,
                                                                     (Single)vector.Y,
                                                                     (Single)vector.Z));
            }
        }

        /// <summary>
        /// Determines whether the specified values are equal.
        /// </summary>
        /// <param name="first">The first value.</param>
        /// <param name="second">The second value.</param>
        /// <returns><c>true</c> if the two values are considered equal at the specified precision; otherwise, <c>false</c>.</returns>
        public Boolean AreEqual(Double first, Double second)
        {
            if (first == second)
                return true;

            return Math.Abs(first - second) < this.baseTolerance;
        }

        /// <summary>
        /// Determines whether the specified coordinates are equal.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <returns><c>true</c> if the two coordinates are considered equal at the specified precision; otherwise, <c>false</c>.</returns>
        public Boolean AreEqual(Coordinate first, Coordinate second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            return this.AreEqual(first.X, second.X) && this.AreEqual(first.Y, second.Y) && this.AreEqual(first.Z, second.Z);
        }

        /// <summary>
        /// Determines whether the specified coordinate vectors are equal.
        /// </summary>
        /// <param name="first">The first coordinate vector.</param>
        /// <param name="second">The second coordinate vector.</param>
        /// <returns><c>true</c> if the two coordinate vectors are considered equal at the specified precision; otherwise, <c>false</c>.</returns>
        public Boolean AreEqual(CoordinateVector first, CoordinateVector second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;

            return this.AreEqual(first.X, second.X) && this.AreEqual(first.Y, second.Y) && this.AreEqual(first.Z, second.Z);
        }

        /// <summary>
        /// Returns the tolerance for the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The tolerance for the values at the specified precision.</returns>
        public Double Tolerance(params Double[] values)
        {
            if (this.ModelType == PrecisionModelType.Fixed || values == null || !values.AnyElement())
                return this.baseTolerance;

            return values.Max(value => Math.Abs(value)) * this.baseTolerance;
        }

        /// <summary>
        /// Returns the tolerance for the specified values.
        /// </summary>
        /// <param name="values">The collection of values.</param>
        /// <returns>The tolerance for the values at the specified precision.</returns>
        public Double Tolerance(IEnumerable<Double> values)
        {
            if (this.ModelType == PrecisionModelType.Fixed || values == null || !values.AnyElement())
                return this.baseTolerance;

            return values.Max(value => Math.Abs(value)) * this.baseTolerance;
        }

        /// <summary>
        /// Returns the tolerance for the specified coordinates.
        /// </summary>
        /// <param name="values">The coordinates.</param>
        /// <returns>The tolerance for the coordinates at the specified precision.</returns>
        public Double Tolerance(params Coordinate[] values)
        {
            if (this.ModelType == PrecisionModelType.Fixed || values == null || !values.AnyElement())
                return this.baseTolerance;

            return values.Max(coordinate => Calculator.Max(Math.Abs(coordinate.X), Math.Abs(coordinate.Y), Math.Abs(coordinate.Z))) * this.baseTolerance;
        }

        /// <summary>
        /// Returns the tolerance for the specified coordinates.
        /// </summary>
        /// <param name="values">The collection of coordinates.</param>
        /// <returns>The tolerance for the coordinates at the specified precision.</returns>
        public Double Tolerance(IEnumerable<Coordinate> values)
        {
            if (this.ModelType == PrecisionModelType.Fixed || values == null || !values.AnyElement())
                return this.baseTolerance;

            return values.Max(coordinate => Calculator.Max(Math.Abs(coordinate.X), Math.Abs(coordinate.Y), Math.Abs(coordinate.Z))) * this.baseTolerance;
        }

        /// <summary>
        /// Returns the tolerance for the specified coordinate vectors.
        /// </summary>
        /// <param name="values">The coordinate vectors.</param>
        /// <returns>The tolerance for the coordinate vectors at the specified precision.</returns>
        public Double Tolerance(params CoordinateVector[] values)
        {
            if (this.ModelType == PrecisionModelType.Fixed || values == null || !values.AnyElement())
                return this.baseTolerance;

            return values.Max(vector => Calculator.Max(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z))) * this.baseTolerance;
        }

        /// <summary>
        /// Returns the tolerance for the specified coordinate vectors.
        /// </summary>
        /// <param name="values">The collection of coordinate vectors.</param>
        /// <returns>The tolerance for the coordinate vectors at the specified precision.</returns>
        public Double Tolerance(IEnumerable<CoordinateVector> values)
        {
            if (this.ModelType == PrecisionModelType.Fixed || values == null || !values.AnyElement())
                return this.baseTolerance;

            return values.Max(vector => Calculator.Max(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z))) * this.baseTolerance;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public Int32 CompareTo(PrecisionModel other)
        {
            if (ReferenceEquals(other, null))
                return 0;
            if (ReferenceEquals(this, other))
                return 0;

            return this.MaximumSignificantDigits.CompareTo(other.MaximumSignificantDigits);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another precision model.
        /// </summary>
        /// <param name="other">A precision model to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
        public Boolean Equals(PrecisionModel other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return this.ModelType == other.ModelType && this.Scale == other.Scale;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object" /> is equal to the current precision model.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            PrecisionModel procisionModelObj = obj as PrecisionModel;

            return procisionModelObj != null && this.ModelType == procisionModelObj.ModelType && this.Scale == procisionModelObj.Scale;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current precision model.</returns>
        public override Int32 GetHashCode()
        {
            return this.ModelType.GetHashCode() ^ this.Scale.GetHashCode() ^ 75632723;
        }

        /// <summary>
        /// Returns a string that represents the current precision model.
        /// </summary>
        /// <returns>A string that represents the current precision model.</returns>
        public override String ToString()
        {
            switch (this.ModelType)
            {
                case PrecisionModelType.FloatingSingle:
                    return FloatingSinglePrecisionModelString;
                case PrecisionModelType.Fixed:
                    return String.Format(CultureInfo.InvariantCulture, FixedPrecisionModelString, this.Scale);
                default:
                    return FloatingPrecisionModelString;
            }
        }

        /// <summary>
        /// The lazily initialized default precision model.
        /// </summary>
        private static readonly Lazy<PrecisionModel> DefaultModel = new Lazy<PrecisionModel>(() => new PrecisionModel(PrecisionModelType.Floating));

        /// <summary>
        /// Gets the default precision model.
        /// </summary>
        /// <value>The default floating precision model.</value>
        public static PrecisionModel Default { get { return DefaultModel.Value; } }

        /// <summary>
        /// Returns the least precise precision model.
        /// </summary>
        /// <param name="models">The precision models.</param>
        /// <returns>The least precise model.</returns>
        /// <exception cref="System.ArgumentNullException">The array of models is null.</exception>
        /// <exception cref="System.ArgumentException">No models are specified.</exception>
        public static PrecisionModel LeastPrecise(params PrecisionModel[] models)
        {
            if (models == null)
                throw new ArgumentNullException(nameof(models));
            if (models.Length == 0)
                throw new ArgumentException(CoreMessages.NoModelsSpecified, nameof(models));

            Int32 preciceIndex = 0;

            for (Int32 index = 1; index < models.Length; index++)
            {
                if (models[preciceIndex].CompareTo(models[index]) < 0)
                    preciceIndex = index;
            }

            return models[preciceIndex];
        }

        /// <summary>
        /// Returns the most precise precision model.
        /// </summary>
        /// <param name="models">The precision models.</param>
        /// <returns>The most precise model.</returns>
        /// <exception cref="System.ArgumentNullException">The array of models is null.</exception>
        /// <exception cref="System.ArgumentException">No models are specified.</exception>
        public static PrecisionModel MostPrecise(params PrecisionModel[] models)
        {
            if (models == null)
                throw new ArgumentNullException(nameof(models));
            if (models.Length == 0)
                throw new ArgumentException(CoreMessages.NoModelsSpecified, nameof(models));

            Int32 preciceIndex = 0;

            for (Int32 index = 1; index < models.Length; index++)
            {
                if (models[preciceIndex].CompareTo(models[index]) > 0)
                    preciceIndex = index;
            }

            return models[preciceIndex];
        }
    }
}
