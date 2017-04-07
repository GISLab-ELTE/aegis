namespace AEGIS.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a 2D Quad-tree, which contains a collection of <see cref="IBasicGeometry" /> instances.
    /// </summary>
    public class QuadTree : ISpatialIndex
    {
        /// <summary>
        /// Represents a node of the Quad-tree.
        /// </summary>
        protected class QuadTreeNode
        {
            /// <summary>
            /// The envelope of the node.
            /// </summary>
            private readonly Envelope envelope;

            /// <summary>
            /// The children nodes of this node. Each internal node must always have 4 children.
            /// </summary>
            private readonly List<QuadTreeNode> children;

            /// <summary>
            /// The geometries stored in this node.
            /// </summary>
            private readonly List<IBasicGeometry> contents;

            /// <summary>
            /// Initializes a new instance of the <see cref="QuadTreeNode"/> class.
            /// </summary>
            /// <param name="envelope">The region of the node.</param>
            public QuadTreeNode(Envelope envelope)
            {
                this.envelope = envelope;
                this.children = new List<QuadTreeNode>(4);
                this.contents = new List<IBasicGeometry>();
            }

            /// <summary>
            /// Gets the region of the node.
            /// </summary>
            /// <value>The region of the node.</value>
            public Envelope Envelope
            {
                get
                {
                    return this.envelope;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the node is a leaf node.
            /// </summary>
            /// <value><c>true</c> if the node is a leaf node, otherwise <c>false</c>.</value>
            public Boolean IsLeaf { get { return this.children.Count == 0; } }

            /// <summary>
            /// Gets a value indicating whether the node is empty or not.
            /// </summary>
            /// <value><c>true</c> if the node is a leaf node and has no geometries, otherwise <c>false</c>.</value>
            public Boolean IsEmpty
            {
                get
                {
                    return this.contents.Count == 0 && this.IsLeaf;
                }
            }

            /// <summary>
            /// Gets the contents of this node.
            /// </summary>
            /// <value>The geometries stored in this node.</value>
            public List<IBasicGeometry> Contents { get { return this.contents; } }

            /// <summary>
            /// Gets the number of geometries in this subtree.
            /// </summary>
            public int NumberOfGeometries
            {
                get
                {
                    int count = 0;
                    foreach (QuadTreeNode node in this.children)
                        count += node.NumberOfGeometries;

                    count += this.contents.Count;
                    return count;
                }
            }

            /// <summary>
            /// Searches this subtree for geometries contained in the envelope.
            /// </summary>
            /// <param name="envelope">The envelope which has to contain the geometries.</param>
            /// <returns>A list of geometries from the tree which are inside the envelope.</returns>
            public IEnumerable<IBasicGeometry> Search(Envelope envelope)
            {
                List<IBasicGeometry> result = new List<IBasicGeometry>();

                // Recursively call Search for children whose bound intersects with the envelope. 
                foreach (QuadTreeNode child in this.children)
                {
                    if (child.IsEmpty)
                        continue;

                    if (envelope.Intersects(child.envelope))
                        result.AddRange(child.Search(envelope));
                }

                // Adding the valid contents from this node
                foreach (IBasicGeometry content in this.contents)
                {
                    if (envelope.Contains(content.Envelope))
                        result.Add(content);
                }

                return result;
            }

            /// <summary>
            /// Adds a geometry to this subtree.
            /// </summary>
            /// <param name="geometry">The geometry to be added.</param>
            public void Add(IBasicGeometry geometry)
            {
                // If this is a leaf node without contents, add geometry to this node and return
                if (this.IsEmpty)
                {
                    this.contents.Add(geometry);
                    return;
                }

                // If this is a leaf node with contents, create children and subdivide current contents between the children
                if (this.children.Count == 0)
                {
                    this.CreateChildren();
                    this.SubdivideContents();
                }

                // Find child which has an envelope that contains current geometry, and recursively call Add on that child, then return
                foreach (QuadTreeNode child in this.children)
                {
                    if (child.envelope.Contains(geometry.Envelope))
                    {
                        child.Add(geometry);
                        return;
                    }
                }

                // If we got here it means that no this node's envelope contains geometry, but no child's envelope does. Add geometry to this node.
                this.contents.Add(geometry);
            }

            /// <summary>
            /// Removes a geometry from the tree.
            /// </summary>
            /// <param name="geometry">The geometry to be removed.</param>
            /// <returns><c>true</c> if the removal was successful, otherwise <c>false</c>.</returns>
            public Boolean Remove(IBasicGeometry geometry)
            {
                if (this.Contents.Remove(geometry))
                    return true;

                foreach (QuadTreeNode child in this.children)
                {
                    if (child.envelope.Contains(geometry.Envelope))
                    {
                        return child.Remove(geometry);
                    }
                }

                return false;
            }

            /// <summary>
            /// Subdivides a nodes geometries between its children.
            /// </summary>
            private void SubdivideContents()
            {
                List<IBasicGeometry> markedForRemove = new List<IBasicGeometry>();

                foreach (IBasicGeometry geometry in this.contents)
                {
                    foreach (QuadTreeNode child in this.children)
                    {
                        if (child.Envelope.Contains(geometry.Envelope))
                        {
                            child.Add(geometry);
                            markedForRemove.Add(geometry);
                        }
                    }
                }

                foreach (IBasicGeometry geometry in markedForRemove)
                    this.contents.Remove(geometry);
            }

            /// <summary>
            /// Creates the children nodes of a node.
            /// </summary>
            private void CreateChildren()
            {
                double midX = this.envelope.MinX + (this.envelope.MaxX - this.envelope.MinX) / 2;
                double midY = this.envelope.MinY + (this.envelope.MaxY - this.envelope.MinY) / 2;

                this.children.Add(new QuadTreeNode(new Envelope(this.envelope.MinX, midX, this.envelope.MinY, midY)));
                this.children.Add(new QuadTreeNode(new Envelope(midX, this.envelope.MaxX, this.envelope.MinY, midY)));
                this.children.Add(new QuadTreeNode(new Envelope(this.envelope.MinX, midX, midY, this.envelope.MaxY)));
                this.children.Add(new QuadTreeNode(new Envelope(midX, this.envelope.MaxX, midY, this.envelope.MaxY)));
            }
        }

        /// <summary>
        /// The root of the tree.
        /// </summary>
        private QuadTreeNode root;

        /// <summary>
        /// Gets a value indicating whether the index is read-only.
        /// </summary>
        /// <value><c>true</c> if the index is read-only; otherwise, <c>false</c>.</value>
        public Boolean IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of indexed geometries.
        /// </summary>
        /// <value>The number of indexed geometries.</value>
        public int NumberOfGeometries
        {
            get
            {
                return this.root.NumberOfGeometries;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadTree" /> class.
        /// </summary>
        /// <param name="bound">The maximum indexed region.</param>
        public QuadTree(Envelope bound)
        {
            this.root = new QuadTreeNode(bound);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadTree" /> class.
        /// </summary>
        /// <param name="geometries">The geometries to add to the tree.</param>
        public QuadTree(IEnumerable<IBasicGeometry> geometries)
        {
            Envelope bound = Envelope.FromEnvelopes(geometries.Select(geometry => geometry.Envelope));
            this.root = new QuadTreeNode(bound);

            this.Add(geometries);
        }

        /// <summary>
        /// Adds a geometry to the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public void Add(IBasicGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "Cannot add null geometry.");

            if (this.root.Envelope.Contains(geometry.Envelope))
            {
                this.root.Add(geometry);
            }
            else
            {
                this.CreateNew(geometry);
            }
        }

        /// <summary>
        /// Adds multiple geometries to the index.
        /// </summary>
        /// <param name="geometries">The geometry collection.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public void Add(IEnumerable<IBasicGeometry> geometries)
        {
            if (geometries == null)
                throw new ArgumentNullException("geometries", "Items to add must not be null.");

            foreach (IBasicGeometry geometry in geometries)
                this.Add(geometry);
        }

        /// <summary>
        /// Determines whether the specified geometry is indexed.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the specified geometry is indexed; otherwise <c>false</c>.</returns>
        public bool Contains(IBasicGeometry geometry)
        {
            if (geometry == null)
                return false;
            if (geometry.Envelope.Equals(Envelope.Undefined))
                return false;

            foreach (IBasicGeometry foundGeometry in this.Search(geometry.Envelope))
            {
                if (foundGeometry.Equals(geometry))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the specified geometry from the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the geometry is indexed; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public bool Remove(IBasicGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "Geometry to remove must not be null.");

            return this.root.Remove(geometry);
        }

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public bool Remove(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "Envelope to remove must not be null.");

            Boolean result = false;
            foreach (IBasicGeometry geometry in this.Search(envelope))
            {
                if (this.root.Remove(geometry))
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="geometries">The list of geometries within the envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public bool Remove(Envelope envelope, out List<IBasicGeometry> geometries)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "Envelope to remove must not be null.");

            geometries = this.Search(envelope).ToList();
            foreach (IBasicGeometry geometry in geometries)
            {
                this.root.Remove(geometry);
            }

            return geometries.Count == 0 ? false : true;
        }

        /// <summary>
        /// Searches the index for any geometries contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of indexed geometries located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public IEnumerable<IBasicGeometry> Search(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "Search envelope must not be null.");

            return this.root.Search(envelope);
        }

        /// <summary>
        /// Clears all geometries from the index.
        /// </summary>
        public void Clear()
        {
            this.root = new QuadTreeNode(this.root.Envelope);
        }

        /// <summary>
        /// Creates a new tree based on an unindexed geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        private void CreateNew(IBasicGeometry geometry)
        {
            IEnumerable<IBasicGeometry> allGeometries = this.Search(this.root.Envelope);
            this.root = new QuadTreeNode(Envelope.FromEnvelopes(this.root.Envelope, geometry.Envelope));
            this.Add(geometry);
            this.Add(allGeometries);
        }
    }
}
