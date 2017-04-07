namespace AEGIS.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a 3D Octree, which contains a collection of <see cref="IBasicGeometry" /> instances.
    /// </summary>
    public class Octree : QuadTree
    {
        /// <summary>
        /// Represents a node of the Octree.
        /// </summary>
        protected class OctreeNode : QuadTreeNode
        {
            /// <summary>
            /// The children nodes of this node. Each internal node must always have 8 children.
            /// </summary>
            private readonly List<OctreeNode> children;

            /// <summary>
            /// Initializes a new instance of the <see cref="OctreeNode"/> class.
            /// </summary>
            /// <param name="envelope">The envelope of the node.</param>
            public OctreeNode(Envelope envelope)
                : base(envelope)
            {
                this.children = new List<OctreeNode>(8);
            }

            /// <summary>
            /// Gets the children of the node.
            /// </summary>
            /// /// <value>The children of the node.</value>
            public new List<OctreeNode> Children
            {
                get
                {
                    return this.children;
                }
            }

            /// <summary>
            /// Creates the children nodes of a node.
            /// </summary>
            protected new void CreateChildren()
            {
                double minX = this.Envelope.MinX;
                double midX = this.Envelope.MinX + (this.Envelope.MaxX - this.Envelope.MinX) / 2;
                double maxX = this.Envelope.MaxX;

                double minY = this.Envelope.MinY;
                double midY = this.Envelope.MinY + (this.Envelope.MaxY - this.Envelope.MinY) / 2;
                double maxY = this.Envelope.MaxY;

                double minZ = this.Envelope.MinZ;
                double midZ = this.Envelope.MinZ + (this.Envelope.MaxZ - this.Envelope.MinZ) / 2;
                double maxZ = this.Envelope.MaxZ;

                this.AddChild(minX, midX, minY, midY, minZ, midZ);
                this.AddChild(minX, midX, minY, midY, midZ, maxZ);
                this.AddChild(minX, midX, midY, maxY, minZ, midZ);
                this.AddChild(minX, midX, midY, maxY, midZ, maxZ);
                this.AddChild(midX, maxX, minY, midY, minZ, midZ);
                this.AddChild(midX, maxX, minY, midY, midZ, maxZ);
                this.AddChild(midX, maxX, midY, maxY, minZ, midZ);
                this.AddChild(midX, maxX, midY, maxY, midZ, maxZ);
            }

            /// <summary>
            /// Creates a child node for this node.
            /// </summary>
            /// <param name="x1">The first X coordinate of the child.</param>
            /// <param name="x2">The second X coordinate of the child.</param>
            /// <param name="y1">The first Y coordinate of the child.</param>
            /// <param name="y2">The second Y coordinate of the child.</param>
            /// <param name="z1">The first Z coordinate of the child.</param>
            /// <param name="z2">The second Z coordinate of the child.</param>
            private void AddChild(double x1, double x2, double y1, double y2, double z1, double z2)
            {
                this.Children.Add(new OctreeNode(new Envelope(x1, x2, y1, y2, z1, z2)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Octree" /> class.
        /// </summary>
        /// <param name="envelope">The maximum indexed region.</param>
        public Octree(Envelope envelope)
            : base(envelope)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Octree" /> class.
        /// </summary>
        /// <param name="geometries">The geometries to add to the tree.</param>
        public Octree(IEnumerable<IBasicGeometry> geometries)
            : base(geometries)
        {
        }
    }
}
