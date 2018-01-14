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
        public HilbertRTree(Int32 maxChildren)
            : this(maxChildren, new HilbertEncoder()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HilbertRTree"/> class using
        /// the specified <see cref="ISpaceFillingCurveEncoder"/> as a space filling curve implementation
        /// and the specifield limits to children count.
        /// </summary>
        /// <param name="minChildren">The minimum children.</param>
        /// <param name="maxChildren">The maximum children.</param>
        /// <param name="encoder">the space filling curve implementation used to encode coordinate values to one dimensional integers</param>
        public HilbertRTree(Int32 maxChildren, ISpaceFillingCurveEncoder encoder)
            : base(maxChildren * 2 / 3, maxChildren)
        {
            this.encoder = encoder;
        }

        protected class HilbertNode : Node, IComparable
        {
            public HilbertNode(Int32 maxChildren)
                : base(maxChildren)
            {
                this.LargestHilbertValue = new BigInteger(-1);
            }

            public HilbertNode(HilbertNode parent)
                : base(parent)
            {
                this.LargestHilbertValue = new BigInteger(-1);
            }

            public HilbertNode(IBasicGeometry geometry, BigInteger hilbertValue, HilbertNode parent = null)
                : base(geometry, parent)
            {
                this.LargestHilbertValue = hilbertValue;
            }

            public BigInteger LargestHilbertValue { get; private set; }

            /// <summary>
            /// Adds a new child to the node.
            /// </summary>
            /// <param name="child">The child node.</param>
            /// <exception cref="ArgumentException">If <code>child</code> is not an instance of <see cref="HilbertNode"/>.</exception>
            public override void AddChild(Node child)
            {
                if (!(child is HilbertNode))
                {
                    throw new ArgumentException("Children of a HilbertNode shall also be instances of HilbertNode.");
                }

                // update parent, initialize Children list if necessary
                child.Parent = this;
                if (this.Children == null)
                    this.Children = new List<Node>(this.MaxChildren);

                // get the LHV of the child
                BigInteger lhv = ((HilbertNode)child).LargestHilbertValue;

                // insert the new child in a sorted manner (according to LHV values)
                Int32 index = this.Children.FindIndex(c => ((HilbertNode)c).LargestHilbertValue > lhv);
                if (index >= 0)
                    this.Children.Insert(index, child);
                else
                    this.Children.Add(child);

                // update LHV
                if (lhv > this.LargestHilbertValue)
                {
                    this.LargestHilbertValue = lhv;
                }

                // update MBR
                this.CorrectBounding(child.Envelope);
            }

            /// <summary>
            /// Removes a child of the node.
            /// </summary>
            /// <param name="node">The node to be removed.</param>
            /// <exception cref="ArgumentException">If <code>node</code> is not an instance of <see cref="HilbertNode"/>.</exception>
            public override void RemoveChild(Node node)
            {
                if (!(node is HilbertNode))
                {
                    throw new ArgumentException("Children of a HilbertNode shall also be instances of HilbertNode.");
                }

                // remove the child, and update MBR
                base.RemoveChild(node);

                // update LHV
                BigInteger lhv = ((HilbertNode)node).LargestHilbertValue;
                if (this.LargestHilbertValue == lhv)
                {
                    this.LargestHilbertValue = this.ChildrenCount > 0 ?
                        ((HilbertNode)this.Children[this.ChildrenCount - 1]).LargestHilbertValue :
                        new BigInteger(-1);
                }
            }

            /// <summary>
            /// Clears all the children from this <see cref="HilbertNode"/>.
            /// </summary>
            public void ClearChildren()
            {
                this.Children = null;
                this.CorrectBounding();
                this.LargestHilbertValue = new BigInteger(-1);
            }

            /// <summary>
            /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
            /// </summary>
            /// <param name="obj">An object to compare with this instance.</param>
            /// <returns>
            /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.
            /// </returns>
            public Int32 CompareTo(Object obj)
            {
                HilbertNode other = (HilbertNode)obj;
                return BigInteger.Compare(this.LargestHilbertValue, other.LargestHilbertValue);
            }
        }

        /// <summary>
        /// Checks the children counts.
        /// </summary>
        /// <param name="minChildren">The minimum children.</param>
        /// <param name="maxChildren">The maximum children.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <code>maxChildren</code> is not a positive integer which can be divided by 3
        /// or
        /// <code>minChildren</code> is different than the 2/3 of <code>maxChildren</code>
        /// </exception>
        protected override void CheckChildrenCounts(int minChildren, int maxChildren)
        {
            if (maxChildren < 3 || maxChildren % 3 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxChildren),
                    "should be a positive integer which can be divided by 3");
            }

            if (minChildren != maxChildren * 2 / 3)
            {
                throw new ArgumentOutOfRangeException(nameof(minChildren),
                    "should be equal to the 2/3 of " + nameof(maxChildren));
            }
        }

        /// <summary>
        /// Creates the root node (when initializing, or clearing the tree).
        /// </summary>
        /// <param name="maxChildren">The maximum number of children nodes for a node.</param>
        protected override void CreateRootNode(int maxChildren)
        {
            this.Root = new HilbertNode(maxChildren);
        }

        /// <summary>
        /// Adds a geometry by creating a new leaf node.
        /// </summary>
        /// <param name="geometry">The geometry to be added.</param>
        protected override void AddGeometry(IBasicGeometry geometry)
        {
            HilbertNode leaf = new HilbertNode(geometry, this.GetHilbertValue(geometry));
            HilbertNode leafContainer = this.ChooseLeafContainer(leaf);

            if (!this.IsFull(leafContainer))
            {
                leafContainer.AddChild(leaf);
                return;
            }

            HilbertNode newNode = this.HandleOverflow(leafContainer, leaf);
            HilbertNode rightRoot = this.AdjustTree(leafContainer, newNode);
            this.IncreaseHeight(rightRoot);
        }

        /// <summary>
        /// Removes the specified geometry from the tree.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>
        ///   <c>true</c>, if the tree contains the geometry, otherwise, <c>false</c>.
        /// </returns>
        protected override Boolean RemoveGeometry(IBasicGeometry geometry)
        {
            // search for the leaf container which contains the specified geometry, if not found, return false
            Node l = null;
            this.FindLeafContainer(geometry, this.Root, ref l);
            if (l == null)
                return false;

            // if found, remove the leaf with the specified geometry
            HilbertNode leafContainer = (HilbertNode)l;
            leafContainer.RemoveChild(
                    leafContainer.Children.Find(child => child.Geometry == geometry));

            // then check for a potential underflow
            if (leafContainer.ChildrenCount < this.MinChildren)
            {
                // underflow happened
                this.HandleUnderflow(leafContainer);
            }

            // adjust parents
            this.AdjustTree(leafContainer);
            return true;
        }

        private void IncreaseHeight(HilbertNode rightRoot)
        {
            if (rightRoot == null)
                return;

            HilbertNode newRoot = new HilbertNode(this.MaxChildren);
            newRoot.AddChild(this.Root);
            newRoot.AddChild(rightRoot);
            this.Root = newRoot;
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
            Tuple<HilbertNode, HilbertNode> siblings = this.ChooseSiblings(node);
            // prefer the sibling to the right side
            return siblings.Item2.IsFull ? siblings.Item1 : siblings.Item2;
        }

        private Tuple<HilbertNode, HilbertNode> ChooseSiblings(HilbertNode node)
        {
            if (node.Parent?.Children == null)
                return Tuple.Create<HilbertNode, HilbertNode>(null, null);

            List<Node> siblings = node.Parent.Children;
            Int32 index = siblings.IndexOf(node);

            HilbertNode left = null;
            HilbertNode right = null;
            if (index + 1 < siblings.Count)
                right = (HilbertNode)siblings[index + 1];
            if (index > 0)
                left = (HilbertNode)siblings[index - 1];
            return Tuple.Create<HilbertNode, HilbertNode>(left, right);
        }

        /// <summary>
        /// Handles the scenario where an overflow happens (the chosen leaf container is full).
        /// </summary>
        /// <remarks>
        /// It handles overflows by first trying to search for a suitable sibling under the same parent to insert the new leaf into.
        /// If this fails (either because there are no siblings, or because they are all full), it will split the node (or two
        /// nodes if there was at least one sibling) into 2 or 3 nodes, and distributes their children (and the new node) evenly amongst them.
        /// </remarks>
        /// <param name="node">The node where the overflow happened.</param>
        /// <param name="leaf">The leaf (or any new node if this is an internal level) to be inserted.</param>
        /// <returns>The newly created node if a split was inevitable.</returns>
        private HilbertNode HandleOverflow(HilbertNode node, HilbertNode leaf)
        {
            // check for a sibling node
            HilbertNode sibling = this.ChooseSibling(node);

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

            this.RedistributeChildrenEvenly(distributionList, leaf);

            return newNode;
        }

        private void HandleUnderflow(HilbertNode node)
        {
            // check for sibling nodes
            Tuple<HilbertNode, HilbertNode> siblings = this.ChooseSiblings(node);
            if (this.HasSpareChild(siblings.Item1) || this.HasSpareChild(siblings.Item2))
            {
                // if there are at least one sibling with more than the minimum amount of children, we redistribute
                List<HilbertNode> distributionList = new List<HilbertNode>();
                if (siblings.Item1 != null)
                    distributionList.Add(siblings.Item1);
                distributionList.Add(node);
                if (siblings.Item2 != null)
                    distributionList.Add(siblings.Item2);
                this.RedistributeChildrenEvenly(distributionList);
            }
            else if (siblings.Item1 != null && siblings.Item2 != null)
            {
                // if there are two siblings but both of the have the minimum number of children, we delete a node, then redistribute
                List<HilbertNode> distributionList = new List<HilbertNode>();
                distributionList.Add(siblings.Item1);
                distributionList.Add(siblings.Item2);
                List<HilbertNode> additionalChildren = new List<HilbertNode>();
                node.Children.ForEach(child => additionalChildren.Add((HilbertNode)child));
                node.Parent.RemoveChild(node);
                this.RedistributeChildrenEvenly(distributionList, additionalChildren: additionalChildren);
            }
        }

        private Boolean HasSpareChild(HilbertNode node)
        {
            return node != null && node.ChildrenCount > this.MinChildren;
        }

        private Boolean IsFull(HilbertNode node)
        {
            return node == this.Root ? node.ChildrenCount == this.MinChildren * 2 : node.IsFull;
        }

        private void RedistributeChildrenEvenly(List<HilbertNode> nodes, HilbertNode additionalChild = null, List<HilbertNode> additionalChildren = null)
        {
            List<HilbertNode> children = new List<HilbertNode>();
            nodes.ForEach(node =>
            {
                node?.Children?.ForEach(child => children.Add((HilbertNode)child));
            });

            if (additionalChild != null)
                children.Add(additionalChild);

            if (additionalChildren != null)
                children.AddRange(additionalChildren);

            children.Sort();

            nodes.ForEach(n => n.ClearChildren());
            Int32 distributionFactor = Convert.ToInt32((decimal)children.Count / nodes.Count);
            IEnumerator<HilbertNode> enumerator = children.GetEnumerator();
            nodes.ForEach(container =>
            {
                for (Int32 i = 0; i < distributionFactor; i++)
                {
                    if (!enumerator.MoveNext())
                        break;
                    container.AddChild(enumerator.Current);
                }
            });
        }

        /// <summary>
        /// Adjusts the tree ascending from the leaf level up to the root.
        /// </summary>
        /// <param name="node">The node where the overflow happened.</param>
        /// <param name="newNode">The new node, if a split was inevitable in <see cref="HandleOverflow(HilbertNode, HilbertNode)"/>, otherwise it's <code>null</code>.</param>
        /// <returns>The new <see cref="HilbertNode"/> on the root level, if the root node had to be split.</returns>
        private HilbertNode AdjustTree(HilbertNode node, HilbertNode newNode = null)
        {
            while (node != this.Root)
            {
                // propagate node split upward
                HilbertNode nParent = (HilbertNode)node.Parent;
                HilbertNode pParent = null;
                if (newNode != null)
                {
                    if (this.IsFull(nParent))
                        pParent = this.HandleOverflow(nParent, newNode);
                    else
                        nParent.AddChild(newNode);
                }

                node = nParent;
                newNode = pParent;
            }

            return newNode;
        }

        /// <summary>
        /// Gets the hilbert value of the specified geometry using the <see cref="ISpaceFillingCurveEncoder"/> specified at construction.
        /// </summary>
        /// <param name="geometry">The geometry of which Hilbert value shall be calculated.</param>
        /// <returns>The Hilbert value of the geometry as calculated by the <see cref="ISpaceFillingCurveEncoder"/> specified at construction time.</returns>
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
