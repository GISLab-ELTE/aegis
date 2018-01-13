// <copyright file="HilbertRTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes.Rectangle
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Text;
    using AEGIS.Numerics;
    using AEGIS.Utilities;

    /// <summary>
    /// Represents a 3D Hilbert R-Tree, which contains a collection of <see cref="IBasicGeometry"/> instances.
    /// </summary>
    public class HilbertRTree : RTree
    {
        /// <summary>
        /// Represents an encoder which can take a 2D or 3D <see cref="Coordinate"/> and encode it
        /// using a space filling curve into a one dimensional integer.
        /// </summary>
        public interface ISpaceFillingCurveEncoder
        {
            /// <summary>
            /// Encode the <see cref="Coordinate"/> using a space filling curve.
            /// </summary>
            /// <param name="coordinate">the 2D or 3D coordinate to encode</param>
            /// <returns>the sequence number of the curve in the point represented by <code>coordinate</code></returns>
            BigInteger Encode(Coordinate coordinate);
        }

        /// <summary>
        /// The standard implementation of <see cref="ISpaceFillingCurveEncoder"/>, it uses the Hilbert Curve to encode coordinates.
        /// It supports both 2D and 3D data.
        /// </summary>
        /// <remarks>
        /// The <see cref="HilbertEncoder"/> operates on a fixed order of 32 which limits the maximum and minimum values of coordinates.
        /// The code can be ported to work on a higher order, but this port is non-trivial. Care should be taken with the <see cref="Pack(uint[])"/> method
        /// as it is implemented for 32 bit result chunks. The size of the result chunks varies depending on the order.
        /// Also, <see cref="UInt32"/> is used as coordinate components, which supports orders up to 32.
        ///
        /// The Hilbert Curve only supports positive coordinate values, but this can be overcame using offset during transition.
        /// This can be requested using parameters of the constructor.
        /// </remarks>
        public class HilbertEncoder : ISpaceFillingCurveEncoder
        {
            private const Int32 HilbertCurveOrder = 32;

            private readonly UInt32 gridTransitionOffset;
            private readonly Boolean is3D;

            /// <summary>
            /// Initializes a new instance of the <see cref="HilbertEncoder"/> class.
            /// It can convert 3 dimensional coordinates of both negative and positive values.
            /// </summary>
            public HilbertEncoder()
                : this(true, true) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="HilbertEncoder"/> class with the attributes specified by constructor parameters.
            /// </summary>
            /// <param name="is3D">Set to <code>true</code> if you want to convert 3D data, or <code>false</code> if you want to convert 2D data.</param>
            /// <param name="negativeValuesAllowed">Set to <code>true</code> if you want to allow negative values. In this case an offset will be used to map values to non-negative integers.</param>
            public HilbertEncoder(Boolean is3D, Boolean negativeValuesAllowed)
            {
                this.gridTransitionOffset = negativeValuesAllowed ? (UInt32.MaxValue / 2) : 0;
                this.is3D = is3D;
            }

            /// <summary>
            /// Encode the <see cref="Coordinate" /> using a space filling curve.
            /// </summary>
            /// <param name="coordinate">the 2D or 3D coordinate to encode</param>
            /// <returns>
            /// the sequence number of the curve in the point represented by <code>coordinate</code>
            /// </returns>
            public BigInteger Encode(Coordinate coordinate)
            {
                UInt32[] hilbertCode = is3D ?
                    GetHilbertCode(Gridify(coordinate.X), Gridify(coordinate.Y), Gridify(coordinate.Z)) :
                    GetHilbertCode(Gridify(coordinate.X), Gridify(coordinate.Y));
                return Pack(hilbertCode);
            }

            private UInt32 Gridify(Double a)
            {
                if(gridTransitionOffset == 0 && a < 0.0D)
                {
                    throw new ArgumentException("This HilbertEncoder can only handle positive coordinates.");
                }
                return Convert.ToUInt32(a + gridTransitionOffset);
            }

            private BigInteger Pack(UInt32[] hilbertCode)
            {
                Byte[] bytes = new Byte[32 * hilbertCode.Length];

                for (Int32 i = 0; i < hilbertCode.Length; i++)
                {
                    EndianBitConverter.GetBytes(hilbertCode[i], ByteOrder.LittleEndian)
                                      .CopyTo(bytes, i * 32);
                }

                // the resulting "bytes" array is in Little Endian too, as defined by BigInteger constructor
                return new BigInteger(bytes);
            }

            private UInt32[] GetHilbertCode(UInt32 x, UInt32 y)
            {
                return this.GetHilbertCode(new UInt32[] { x, y }, new UInt32[] { 2U, 1U });
            }

            private UInt32[] GetHilbertCode(UInt32 x, UInt32 y, UInt32 z)
            {
                return this.GetHilbertCode(new UInt32[] { x, y, z }, new UInt32[] { 4U, 2U, 1U });
            }

            private UInt32[] GetHilbertCode(UInt32[] indexes, UInt32[] g_mask)
            {
                // implementation based on the following article:
                // J. K. Lawder: Calculation of Mappings Between One and n-dimensional Values Using the Hilbert Space-filling Curve
                if (indexes.Length < 2 || indexes.Length > 3)
                {
                    throw new ArgumentException("Hilbert Code calculation only works on geometries with 2 or 3 dimension coordinates.");
                }

                Int32 dim = indexes.Length;
                UInt32 mask = 1U << (HilbertCurveOrder - 1);
                UInt32 element, A = 0U, W = 0U, S, tS, T, tT, J, P = 0U, xJ;
                UInt32[] h = new UInt32[dim];
                Int32 i = HilbertCurveOrder * dim - dim, j;
                for (j = 0; j < dim; j++)
                {
                    if ((indexes[j] & mask) != 0U)
                    {
                        A |= g_mask[j];
                    }
                }

                S = tS = A;
                P = Calc_P2(S, dim, g_mask);

                // add in DIM bits to hilbert code
                element = (UInt32)(i / HilbertCurveOrder);
                if (i % HilbertCurveOrder > HilbertCurveOrder - dim)
                {
                    h[element] |= P << i % HilbertCurveOrder;
                    h[element + 1] |= P >> HilbertCurveOrder - i % HilbertCurveOrder;
                }
                else
                {
                    h[element] |= P << (Int32)(i - element * HilbertCurveOrder);
                }

                J = Calc_J(P, dim);
                xJ = J - 1;
                T = Calc_T(P);
                tT = T;
                for (i -= dim, mask >>= 1; i >= 0; i -= dim, mask >>= 1)
                {
                    A = 0U;
                    for (j = 0; j < dim; j++)
                    {
                        if ((indexes[j] & mask) != 0U)
                        {
                            A |= g_mask[j];
                        }
                    }

                    W ^= tT;
                    tS = A ^ W;
                    S = Calc_tS_tT(xJ, tS, dim);
                    P = Calc_P2(S, dim, g_mask);

                    // add in DIM bits to hcode
                    element = (UInt32)(i / HilbertCurveOrder);
                    if (i % HilbertCurveOrder > HilbertCurveOrder - dim)
                    {
                        h[element] |= P << i % HilbertCurveOrder;
                        h[element + 1] |= P >> HilbertCurveOrder - i % HilbertCurveOrder;
                    }
                    else
                    {
                        h[element] |= P << (Int32)(i - element * HilbertCurveOrder);
                    }

                    if (i > 0)
                    {
                        T = Calc_T(P);
                        tT = Calc_tS_tT(xJ, T, dim);
                        J = Calc_J(P, dim);
                        xJ += J - 1;
                    }
                }

                return h;
            }

            private UInt32 Calc_P2(UInt32 S, Int32 dim, UInt32[] g_mask)
            {
                Int32 i;
                UInt32 p;
                p = S & g_mask[0];
                for (i = 1; i < dim; i++)
                {
                    if ((S & g_mask[i] ^ (p >> 1) & g_mask[i]) != 0)
                        p |= g_mask[i];
                }

                return p;
            }

            private UInt32 Calc_J(UInt32 P, Int32 dim)
            {
                Int32 i;
                UInt32 j = (UInt32)dim;
                for (i = 1; i < dim; i++)
                {
                    if ((P >> i & 1) == (P & 1))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if (i != dim)
                    j -= (UInt32)i;
                return j;
            }

            private UInt32 Calc_T(UInt32 P)
            {
                if (P < 3)
                    return 0;
                if (P % 2 != 0)
                    return (P - 1) ^ (P - 1) / 2;
                return (P - 2) ^ (P - 2) / 2;
            }

            private UInt32 Calc_tS_tT(UInt32 xJ, UInt32 val, Int32 dim)
            {
                UInt32 retval, temp1, temp2;
                retval = val;
                if (xJ % dim != 0)
                {
                    temp1 = val >> (Int32)(xJ % dim);
                    temp2 = val << (Int32)(dim - xJ % dim);
                    retval = temp1 | temp2;
                    retval &= ((UInt32)1 << dim) - 1;
                }

                return retval;
            }
        }

        private readonly ISpaceFillingCurveEncoder encoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="HilbertRTree"/> class using
        /// the default <see cref="HilbertEncoder"/> as a space filling curve.
        /// </summary>
        public HilbertRTree()
            : this(new HilbertEncoder()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HilbertRTree"/> class using
        /// the specified <see cref="ISpaceFillingCurveEncoder"/> as a space filling curve implementation.
        /// </summary>
        /// <param name="encoder">the space filling curve implementation used to encode coordinate values to one dimensional integers</param>
        public HilbertRTree(ISpaceFillingCurveEncoder encoder)
        {
            this.encoder = encoder;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HilbertRTree"/> class using
        /// the default <see cref="HilbertEncoder"/> as a space filling curve and the
        /// specifield limits to children count.
        /// </summary>
        /// <param name="minChildren">The minimum number of child nodes.</param>
        /// <param name="maxChildren">The maximum number of child nodes.</param>
        public HilbertRTree(Int32 minChildren, Int32 maxChildren)
            : this(minChildren, maxChildren, new HilbertEncoder()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HilbertRTree"/> class using
        /// the specified <see cref="ISpaceFillingCurveEncoder"/> as a space filling curve implementation
        /// and the specifield limits to children count.
        /// </summary>
        /// <param name="minChildren">The minimum children.</param>
        /// <param name="maxChildren">The maximum children.</param>
        /// <param name="encoder">the space filling curve implementation used to encode coordinate values to one dimensional integers</param>
        public HilbertRTree(Int32 minChildren, Int32 maxChildren, ISpaceFillingCurveEncoder encoder)
            : base(minChildren, maxChildren)
        {
            this.encoder = encoder;
        }

        protected class HilbertNode : Node, IComparable
        {
            public HilbertNode(Int32 maxChildren)
                : base(maxChildren)
            {
                this.LargestHilbertValue = new BigInteger(0);
            }

            public HilbertNode(HilbertNode parent)
                : base(parent) { }

            public HilbertNode(IBasicGeometry geometry, BigInteger largestHilbertValue, HilbertNode parent = null)
                : base(geometry, parent)
            {
                this.LargestHilbertValue = largestHilbertValue;
            }

            public BigInteger LargestHilbertValue { get; private set; }

            public Int32 CompareTo(Object obj)
            {
                HilbertNode other = (HilbertNode)obj;
                return BigInteger.Compare(this.LargestHilbertValue, other.LargestHilbertValue);
            }
        }

        private HilbertNode ChooseLeafContainer(HilbertNode leaf)
        {
            Node node = this.Root;

            while (!node.IsLeafContainer)
            {
                HilbertNode lastChild = (HilbertNode)node.Children[node.ChildrenCount - 1];
                // select the child with the minimum LHV which is greater than "value"
                // if there are no such children, select the last child (the one with the greatest LHV)
                node = lastChild.LargestHilbertValue <= leaf.LargestHilbertValue ? lastChild :
                    node.Children.Find(n => ((HilbertNode)n).LargestHilbertValue > leaf.LargestHilbertValue);
            }

            return (HilbertNode)node;
        }

        private HilbertNode ChooseSibling(HilbertNode node)
        {
            if (node.Parent == null)
                return null;
            List<HilbertNode> siblings = new List<HilbertNode>(node.Parent.Children.Count);
            node.Parent.Children.ForEach(child => siblings.Add((HilbertNode)child));
            Int32 index = siblings.IndexOf(node);
            HilbertNode chosenSibling = null;
            if (index + 1 < siblings.Count)
                chosenSibling = siblings[index + 1];
            if ((chosenSibling == null || chosenSibling.IsFull) && index > 0)
                chosenSibling = siblings[index - 1];
            return chosenSibling;
        }

        private HilbertNode HandleOverflow(HilbertNode node, HilbertNode leaf)
        {
            // check for a sibling node
            HilbertNode sibling = this.ChooseSibling(node);

            // create a sorted list (based on Hilbert values) from all leaves of the original node and the sibling node,
            // and of the new leaf element
            List<HilbertNode> leaves = new List<HilbertNode>();
            node.Children?.ForEach(child => leaves.Add((HilbertNode)child));
            sibling?.Children?.ForEach(child => leaves.Add((HilbertNode)child));
            leaves.Add(leaf);
            leaves.Sort();

            // then we redistribute these children evenly across the 2 or 3 nodes:
            // - between the original and its sibling if there was one, and it was not full
            // - between the original and a new node if there wasn't any sibling
            // - between the original, the sibling and the new node, if there was a sibling, but it was full
            // the original node is assumed to be always full here (hence the method name HandleOverflow)
            List<HilbertNode> distributionList = new List<HilbertNode>();
            HilbertNode newNode = null;
            distributionList.Add(node);
            if (sibling == null || sibling.IsFull)
            {
                newNode = new HilbertNode((HilbertNode)node.Parent);
                distributionList.Add(newNode);
            }

            if (sibling != null)
            {
                distributionList.Insert(sibling.LargestHilbertValue < node.LargestHilbertValue ? 0 : 1, sibling);
            }

            distributionList.ForEach(n => n.Children?.Clear());
            Int32 distributionFactor = Convert.ToInt32((decimal)node.MaxChildren / distributionList.Count);
            IEnumerator<HilbertNode> enumerator = leaves.GetEnumerator();
            distributionList.ForEach(container =>
            {
                for (Int32 i = 0; i < distributionFactor; i++)
                {
                    if (!enumerator.MoveNext())
                        break;
                    container.AddChild(enumerator.Current);
                }
            });

            return newNode;
        }

        private void AdjustTree(ISet<HilbertNode> cooperatingNodes, HilbertNode node, HilbertNode newNode)
        {
            // TODO ezt a feltételt átgondolni (lehet hogy egy iterációval hamarabb áll meg mint kellene)
            while (!cooperatingNodes.Contains((HilbertNode)this.Root))
            {
                // propagate node split upward
                HilbertNode nParent = (HilbertNode)node.Parent;
                HilbertNode pParent = null;
                if (newNode != null)
                {
                    if (nParent.IsFull)
                    {
                        pParent = this.HandleOverflow(nParent, newNode);
                    }
                    else
                    {
                        // TODO insert newNode in nParent correctly
                    }
                }

                // adjust parent level
                ISet<HilbertNode> parents = new HashSet<HilbertNode>();
                foreach (HilbertNode cooperatingNode in cooperatingNodes)
                {
                    parents.Add((HilbertNode)cooperatingNode.Parent);
                }
                // TODO adjust MBRs and LHVs in parents

                cooperatingNodes = parents;
                newNode = pParent;
            }
        }

        private BigInteger GetHilbertValue(IBasicGeometry geometry)
        {
            Coordinate coordinate;

            if (geometry is IBasicPoint)
            {
                coordinate = ((IBasicPoint)geometry).Coordinate;
            } else
            {
                coordinate = geometry.Envelope.Center;
            }

            return this.encoder.Encode(coordinate);
        }
    }
}
