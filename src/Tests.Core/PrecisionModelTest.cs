// <copyright file="PrecisionModelTest.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Shouldly;

    /// <summary>
    /// Test fixture for the <see cref="PrecisionModel" /> class.
    /// </summary>
    [TestFixture]
    public class PrecisionModelTest
    {
        #region Private fields

        /// <summary>
        /// The default precision model.
        /// </summary>
        private PrecisionModel defaultModel;

        /// <summary>
        /// The floating point precision model.
        /// </summary>
        private PrecisionModel floatingModel;

        /// <summary>
        /// The single precision floating point precision model.
        /// </summary>
        private PrecisionModel floatingSingleModel;

        /// <summary>
        /// The fixed point precision model for large differences.
        /// </summary>
        private PrecisionModel fixedLargeModel1;

        /// <summary>
        /// The fixed point precision model for large differences.
        /// </summary>
        private PrecisionModel fixedLargeModel2;

        /// <summary>
        /// The fixed point precision model for small differences.
        /// </summary>
        private PrecisionModel fixedSmallModel1;

        /// <summary>
        /// The fixed point precision model for small differences.
        /// </summary>
        private PrecisionModel fixedSmallModel2;

        /// <summary>
        /// The collection of values.
        /// </summary>
        private Double[] values;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.defaultModel = new PrecisionModel();
            this.floatingModel = new PrecisionModel(PrecisionModelType.Floating);
            this.floatingSingleModel = new PrecisionModel(PrecisionModelType.FloatingSingle);
            this.fixedLargeModel1 = new PrecisionModel(1000);
            this.fixedLargeModel2 = new PrecisionModel(1000000000000);
            this.fixedSmallModel1 = new PrecisionModel(0.001);
            this.fixedSmallModel2 = new PrecisionModel(0.000000000001);

            this.values = Enumerable.Range(1, 28).Select(value => Math.Pow((Double)value / 2, (value / 2) * ((value % 2 == 0) ? 1 : -1))).ToArray();
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Tests the constructor of the <see cref="PrecisionModel" /> class.
        /// </summary>
        [Test]
        public void PrecisionModelPropertiesTest()
        {
            // no arguments

            this.defaultModel.ModelType.ShouldBe(PrecisionModelType.Floating);
            this.defaultModel.MaximumSignificantDigits.ShouldBe(16);
            this.defaultModel.Scale.ShouldBe(0);

            // floating model type

            this.floatingModel.ModelType.ShouldBe(PrecisionModelType.Floating);
            this.floatingModel.MaximumSignificantDigits.ShouldBe(16);
            this.floatingModel.Scale.ShouldBe(0);

            // floating single model type

            this.floatingSingleModel.ModelType.ShouldBe(PrecisionModelType.FloatingSingle);
            this.floatingSingleModel.MaximumSignificantDigits.ShouldBe(6);
            this.floatingSingleModel.Scale.ShouldBe(0);

            // fixed type with scale of 1000

            this.fixedLargeModel1.ModelType.ShouldBe(PrecisionModelType.Fixed);
            this.fixedLargeModel1.MaximumSignificantDigits.ShouldBe(4);
            this.fixedLargeModel1.Scale.ShouldBe(1000);

            // fixed type with scale of 1000000000000

            this.fixedLargeModel2.ModelType.ShouldBe(PrecisionModelType.Fixed);
            this.fixedLargeModel2.MaximumSignificantDigits.ShouldBe(13);
            this.fixedLargeModel2.Scale.ShouldBe(1000000000000);

            // fixed type with scale of 0.001

            this.fixedSmallModel1.ModelType.ShouldBe(PrecisionModelType.Fixed);
            this.fixedSmallModel1.MaximumSignificantDigits.ShouldBe(1);
            this.fixedSmallModel1.Scale.ShouldBe(0.001);

            // fixed type with scale of 0.000000000001

            this.fixedSmallModel2.ModelType.ShouldBe(PrecisionModelType.Fixed);
            this.fixedSmallModel2.MaximumSignificantDigits.ShouldBe(1);
            this.fixedSmallModel2.Scale.ShouldBe(0.000000000001);
        }

        /// <summary>
        /// Tests the <see cref="PrecisionModel.MakePrecise(Double)" /> method.
        /// </summary>
        [Test]
        public void PrecisionModelMakePreciseTest()
        {
            for (Int32 i = 0; i < this.values.Length; i++)
            {
                this.defaultModel.MakePrecise(this.values[i]).ShouldBe(this.values[i]);
                this.floatingModel.MakePrecise(this.values[i]).ShouldBe(this.values[i]);
                this.floatingSingleModel.MakePrecise(this.values[i]).ShouldBe((Single)this.values[i]);
                this.fixedLargeModel1.MakePrecise(this.values[i]).ShouldBe(Math.Round(this.values[i], 3));
                this.fixedLargeModel2.MakePrecise(this.values[i]).ShouldBe(Math.Round(this.values[i], 12));
                this.fixedSmallModel1.MakePrecise(this.values[i]).ShouldBe(Math.Round(this.values[i] / 1000) * 1000);
                this.fixedSmallModel2.MakePrecise(this.values[i]).ShouldBe(Math.Round(this.values[i] / 1000000000000) * 1000000000000);
            }
        }

        /// <summary>
        /// Tests the <see cref="PrecisionModel.Tolerance(Double[])" /> method.
        /// </summary>
        [Test]
        public void PrecisionModelToleranceTest()
        {
            for (Int32 i = 0; i < this.values.Length; i++)
            {
                this.defaultModel.Tolerance(this.values[i]).ShouldBeGreaterThan(this.values[i] * Math.Pow(10, -16));
                this.defaultModel.Tolerance(this.values[i]).ShouldBeLessThanOrEqualTo(this.values[i] * Math.Pow(10, -15));
                this.floatingModel.Tolerance(this.values[i]).ShouldBeGreaterThan(this.values[i] * Math.Pow(10, -16));
                this.floatingModel.Tolerance(this.values[i]).ShouldBeLessThanOrEqualTo(this.values[i] * Math.Pow(10, -15));
                this.floatingSingleModel.Tolerance(this.values[i]).ShouldBeGreaterThan(this.values[i] * Math.Pow(10, -6));
                this.floatingSingleModel.Tolerance(this.values[i]).ShouldBeLessThanOrEqualTo(this.values[i] * Math.Pow(10, -5));
                this.fixedLargeModel1.Tolerance(this.values[i]).ShouldBe(0.0005);
                this.fixedLargeModel2.Tolerance(this.values[i]).ShouldBe(0.0000000000005);
                this.fixedSmallModel1.Tolerance(this.values[i]).ShouldBe(500);
                this.fixedSmallModel2.Tolerance(this.values[i]).ShouldBe(500000000000);
            }
        }

        #endregion
    }
}
