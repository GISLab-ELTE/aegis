// <copyright file="KrovakModifiedProjection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Reference.Collections.Formula
{
    using System;
    using System.Collections.Generic;
    using AEGIS.Numerics;

    /// <summary>
    /// Represents a Krovak Modified Projection.
    /// </summary>
    [IdentifiedObject("AEGIS::1042", "Krovak Modified Projection")]
    public class KrovakModifiedProjection : KrovakProjection
    {
        /// <summary>
        /// Ordinate 1 of evaluation point.
        /// </summary>
        protected readonly Double ordinate1OfEvaluationPoint;

        /// <summary>
        /// Ordinate 2 of evaluation point.
        /// </summary>
        protected readonly Double ordinate2OfEvaluationPoint;

        /// <summary>
        /// C1 parameter.
        /// </summary>
        protected readonly Double C1;

        /// <summary>
        /// C2 parameter.
        /// </summary>
        protected readonly Double C2;

        /// <summary>
        /// C3 parameter.
        /// </summary>
        protected readonly Double C3;

        /// <summary>
        /// C4 parameter.
        /// </summary>
        protected readonly Double C4;

        /// <summary>
        /// C5 parameter.
        /// </summary>
        protected readonly Double C5;

        /// <summary>
        /// C6 parameter.
        /// </summary>
        protected readonly Double C6;

        /// <summary>
        /// C7 parameter.
        /// </summary>
        protected readonly Double C7;

        /// <summary>
        /// C8 parameter.
        /// </summary>
        protected readonly Double C8;

        /// <summary>
        /// C9 parameter.
        /// </summary>
        protected readonly Double C9;

        /// <summary>
        /// C10 parameter.
        /// </summary>
        protected readonly Double C10;

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakModifiedProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public KrovakModifiedProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, null, null, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakModifiedProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        public KrovakModifiedProjection(String identifier, String name, String remarks, String[] aliases, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, CoordinateOperationMethods.KrovakModifiedProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakModifiedProjection" /> class.
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
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        protected KrovakModifiedProjection(String identifier, String name, String remarks, String[] aliases, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, remarks, aliases, method, parameters, ellipsoid, areaOfUse)
        {
            this.ordinate1OfEvaluationPoint = this.GetParameterValue(CoordinateOperationParameters.Ordinate1OfEvaluationPoint);
            this.ordinate2OfEvaluationPoint = this.GetParameterValue(CoordinateOperationParameters.Ordinate2OfEvaluationPoint);
            this.C1 = this.GetParameterValue(CoordinateOperationParameters.C1);
            this.C2 = this.GetParameterValue(CoordinateOperationParameters.C2);
            this.C3 = this.GetParameterValue(CoordinateOperationParameters.C3);
            this.C4 = this.GetParameterValue(CoordinateOperationParameters.C4);
            this.C5 = this.GetParameterValue(CoordinateOperationParameters.C5);
            this.C6 = this.GetParameterValue(CoordinateOperationParameters.C6);
            this.C7 = this.GetParameterValue(CoordinateOperationParameters.C7);
            this.C8 = this.GetParameterValue(CoordinateOperationParameters.C8);
            this.C9 = this.GetParameterValue(CoordinateOperationParameters.C9);
            this.C10 = this.GetParameterValue(CoordinateOperationParameters.C10);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Coordinate baseCoord = base.ComputeForward(coordinate);

            Double xP = baseCoord.Y - this.falseNorthing;
            Double yP = baseCoord.X - this.falseEasting;

            Double xR = xP - this.ordinate1OfEvaluationPoint;
            Double yR = yP - this.ordinate2OfEvaluationPoint;

            Double dX = this.C1 + this.C3 * xR - this.C4 * yR - 2 * this.C6 * xR * yR + this.C5 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C7 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) - this.C8 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) + 4 * this.C9 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C10 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));
            Double dY = this.C2 + this.C3 * yR + this.C4 * xR - 2 * this.C5 * xR * yR + this.C6 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C8 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) + this.C7 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) - 4 * this.C10 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C9 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));

            return new Coordinate(yP - dY + this.falseEasting, xP - dX + this.falseNorthing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double xR = (coordinate.Y - this.falseNorthing) - this.ordinate1OfEvaluationPoint;
            Double yR = (coordinate.X - this.falseEasting) - this.ordinate2OfEvaluationPoint;

            Double dX = this.C1 + this.C3 * xR - this.C4 * yR - 2 * this.C6 * xR * yR + this.C5 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C7 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) - this.C8 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) + 4 * this.C9 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C10 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));
            Double dY = this.C2 + this.C3 * yR + this.C4 * xR - 2 * this.C5 * xR * yR + this.C6 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C8 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) + this.C7 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) - 4 * this.C10 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + this.C9 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));

            return base.ComputeReverse(new Coordinate(coordinate.X + dY, coordinate.Y + dX));
        }
    }
}
