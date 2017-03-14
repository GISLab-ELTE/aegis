// <copyright file="P6SeismicBinGridTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Numerics;

    /// <summary>
    /// Represents a P6 seismic bin grid transformation.
    /// </summary>
    public abstract class P6SeismicBinGridTransformation : CoordinateTransformation<Coordinate>
    {
        /// <summary>
        /// Defines the possible orientations of the grid.
        /// </summary>
        protected enum Orientation
        {
            /// <summary>
            /// Indicates a left handed grid.
            /// </summary>
            LeftHanded,

            /// <summary>
            /// Indicates a right handed grid.
            /// </summary>
            RightHanded
        }

        /// <summary>
        /// The orientation of the transformation.
        /// </summary>
        protected Orientation orientation;

        /// <summary>
        /// Bin grid origin I.
        /// </summary>
        private readonly Double binGridOriginI;

        /// <summary>
        /// Bin grid origin J.
        /// </summary>
        private readonly Double binGridOriginJ;

        /// <summary>
        /// Bin grid origin Easting.
        /// </summary>
        private readonly Double binGridOriginEasting;

        /// <summary>
        /// Bin grid origin Northing.
        /// </summary>
        private readonly Double binGridOriginNorthing;

        /// <summary>
        /// Scale factor of bin grid.
        /// </summary>
        private readonly Double scaleFactorOfBinGrid;

        /// <summary>
        /// Bin width on I axis.
        /// </summary>
        private readonly Double binWidthOnIAxis;

        /// <summary>
        /// Bin width on J axis.
        /// </summary>
        private readonly Double binWidthOnJAxis;

        /// <summary>
        /// Map grid bearing of bin grid J axis.
        /// </summary>
        private readonly Double mapGridBearingOfBinGridJAxis;

        /// <summary>
        /// Bin node increment on I axis.
        /// </summary>
        private readonly Double binNodeIncOnIAxis;

        /// <summary>
        /// Bin node increment on J axis.
        /// </summary>
        private readonly Double binNodeIncOnJAxis;

        /// <summary>
        /// Initializes a new instance of the <see cref="P6SeismicBinGridTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected P6SeismicBinGridTransformation(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters,
                                                 CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, source, target, areaOfUse)
        {
            this.binGridOriginI = this.GetParameterValue(CoordinateOperationParameters.BinGridOriginI);
            this.binGridOriginJ = this.GetParameterValue(CoordinateOperationParameters.BinGridOriginJ);
            this.binGridOriginEasting = this.GetParameterValue(CoordinateOperationParameters.BinGridOriginEasting);
            this.binGridOriginNorthing = this.GetParameterValue(CoordinateOperationParameters.BinGridOriginNorthing);
            this.scaleFactorOfBinGrid = this.GetParameterValue(CoordinateOperationParameters.ScaleFactorOfBinGrid);
            this.binWidthOnIAxis = this.GetParameterValue(CoordinateOperationParameters.BinWidthOnIAxis);
            this.binWidthOnJAxis = this.GetParameterValue(CoordinateOperationParameters.BinWidthOnJAxis);
            this.mapGridBearingOfBinGridJAxis = this.GetParameterBaseValue(CoordinateOperationParameters.MapGridBearingOfBinGridJAxis);
            this.binNodeIncOnIAxis = this.GetParameterValue(CoordinateOperationParameters.BinNodeIncrementOnIAxis);
            this.binNodeIncOnJAxis = this.GetParameterValue(CoordinateOperationParameters.BinNodeIncrementOnJAxis);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            Double x = 0, y = 0;

            switch (this.orientation)
            {
                case Orientation.LeftHanded:
                    x = this.binGridOriginEasting - ((coordinate.X - this.binGridOriginI) * Math.Cos(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnIAxis / this.binNodeIncOnIAxis)
                                              + ((coordinate.Y - this.binGridOriginJ) * Math.Sin(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnJAxis / this.binNodeIncOnJAxis);
                    y = this.binGridOriginNorthing + ((coordinate.X - this.binGridOriginI) * Math.Sin(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnIAxis / this.binNodeIncOnIAxis)
                                               + ((coordinate.Y - this.binGridOriginJ) * Math.Cos(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnJAxis / this.binNodeIncOnJAxis);
                    break;
                case Orientation.RightHanded:
                    x = this.binGridOriginEasting + ((coordinate.X - this.binGridOriginI) * Math.Cos(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnIAxis / this.binNodeIncOnIAxis)
                                              + ((coordinate.Y - this.binGridOriginJ) * Math.Sin(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnJAxis / this.binNodeIncOnJAxis);
                    y = this.binGridOriginNorthing - ((coordinate.X - this.binGridOriginI) * Math.Sin(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnIAxis / this.binNodeIncOnIAxis)
                                               + ((coordinate.Y - this.binGridOriginJ) * Math.Cos(this.mapGridBearingOfBinGridJAxis) * this.scaleFactorOfBinGrid * this.binWidthOnJAxis / this.binNodeIncOnJAxis);
                    break;
            }

            return new Coordinate(x, y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = 0, y = 0;

            switch (this.orientation)
            {
                case Orientation.LeftHanded:
                    x = this.binGridOriginI - (((coordinate.X - this.binGridOriginEasting) * Math.Cos(this.mapGridBearingOfBinGridJAxis) - (coordinate.Y - this.binGridOriginNorthing) * Math.Sin(this.mapGridBearingOfBinGridJAxis)) * (this.binNodeIncOnIAxis / (this.scaleFactorOfBinGrid * this.binWidthOnIAxis)));
                    y = this.binGridOriginJ + (((coordinate.X - this.binGridOriginEasting) * Math.Sin(this.mapGridBearingOfBinGridJAxis) + (coordinate.Y - this.binGridOriginNorthing) * Math.Cos(this.mapGridBearingOfBinGridJAxis)) * (this.binNodeIncOnJAxis / (this.scaleFactorOfBinGrid * this.binWidthOnJAxis)));
                    break;
                case Orientation.RightHanded:
                    x = this.binGridOriginI + (((coordinate.X - this.binGridOriginEasting) * Math.Cos(this.mapGridBearingOfBinGridJAxis) - (coordinate.Y - this.binGridOriginNorthing) * Math.Sin(this.mapGridBearingOfBinGridJAxis)) * (this.binNodeIncOnIAxis / (this.scaleFactorOfBinGrid * this.binWidthOnIAxis)));
                    y = this.binGridOriginJ + (((coordinate.X - this.binGridOriginEasting) * Math.Sin(this.mapGridBearingOfBinGridJAxis) + (coordinate.Y - this.binGridOriginNorthing) * Math.Cos(this.mapGridBearingOfBinGridJAxis)) * (this.binNodeIncOnJAxis / (this.scaleFactorOfBinGrid * this.binWidthOnJAxis)));
                    break;
            }

            return new Coordinate(x, y);
        }
    }
}
