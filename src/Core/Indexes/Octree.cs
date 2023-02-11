// <copyright file="Octree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace AEGIS.Indexes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a 3D Octree, which contains a collection of <see cref="IBasicGeometry" /> instances.
    /// </summary>
    /// <author>Ákos Horváth, Roland Krisztandl</author>
    public class Octree : QuadTree
    {
        /// <summary>
        /// Represents a node of the Octree.
        /// </summary>
        protected class OctreeNode : QuadTreeNode
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="OctreeNode"/> class.
            /// </summary>
            /// <param name="envelope">The envelope of the node.</param>
            public OctreeNode(Envelope envelope)
                : base(envelope)
            {
            }

            /// <summary>
            /// Creates the children nodes of a node.
            /// </summary>
            protected new void CreateChildren()
            {
                Double minX = this.Envelope.MinX;
                Double midX = this.Envelope.MinX + (this.Envelope.MaxX - this.Envelope.MinX) / 2;
                Double maxX = this.Envelope.MaxX;

                Double minY = this.Envelope.MinY;
                Double midY = this.Envelope.MinY + (this.Envelope.MaxY - this.Envelope.MinY) / 2;
                Double maxY = this.Envelope.MaxY;

                Double minZ = this.Envelope.MinZ;
                Double midZ = this.Envelope.MinZ + (this.Envelope.MaxZ - this.Envelope.MinZ) / 2;
                Double maxZ = this.Envelope.MaxZ;

                this.children.Add(new OctreeNode(new Envelope(minX, midX, minY, midY, minZ, midZ)));
                this.children.Add(new OctreeNode(new Envelope(minX, midX, minY, midY, midZ, maxZ)));
                this.children.Add(new OctreeNode(new Envelope(minX, midX, midY, maxY, minZ, midZ)));
                this.children.Add(new OctreeNode(new Envelope(minX, midX, midY, maxY, midZ, maxZ)));
                this.children.Add(new OctreeNode(new Envelope(midX, maxX, minY, midY, minZ, midZ)));
                this.children.Add(new OctreeNode(new Envelope(midX, maxX, minY, midY, midZ, maxZ)));
                this.children.Add(new OctreeNode(new Envelope(midX, maxX, midY, maxY, minZ, midZ)));
                this.children.Add(new OctreeNode(new Envelope(midX, maxX, midY, maxY, midZ, maxZ)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Octree" /> class.
        /// </summary>
        /// <param name="envelope">The maximum indexed region.</param>
        public Octree(Envelope envelope)
        {
            this.root = new OctreeNode(envelope);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Octree" /> class.
        /// </summary>
        /// <param name="geometries">The geometries to add to the tree.</param>
        public Octree(IEnumerable<IBasicGeometry> geometries)
        {
            Envelope bound = Envelope.FromEnvelopes(geometries.Select(geometry => geometry.Envelope));
            this.root = new OctreeNode(bound);

            this.Add(geometries);
        }

        /// <summary>
        /// Clears all geometries from the index.
        /// </summary>
        public override void Clear()
        {
            this.root = new OctreeNode(this.root.Envelope);
        }

        /// <summary>
        /// Creates a new tree based on an unindexed geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        protected override void CreateNew(IBasicGeometry geometry)
        {
            IEnumerable<IBasicGeometry> allGeometries = this.Search(this.root.Envelope);
            this.root = new OctreeNode(Envelope.FromEnvelopes(this.root.Envelope, geometry.Envelope));
            this.Add(geometry);
            this.Add(allGeometries);
        }
    }
}
