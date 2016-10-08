using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    public abstract class Collider : Component
    {
        protected struct Rect
        {
            public Vector2 topLeft;
            public Vector2 topRight;
            public Vector2 bottomLeft;
            public Vector2 bottomRight;
        }

        /// <summary>
        /// The Rigidbody attached to the Collider's GameObject.
        /// </summary>
        public Rigidbody attachedRigidbody
        {
            get
            {
                return GameObject.GetComponent<Rigidbody>();
            }
        }

        /// <summary>
        /// Is this collider configured as a trigger?
        /// </summary>
        public bool isTrigger = false;

        /// <summary>
        /// The local offset of the collider geometry.
        /// </summary>
        public Vector2 offset = Vector2.Zero;

        /// <summary>
        /// Checks whether this collider is touching the collider or not.
        /// </summary>
        /// <param name="collider">The collider to check if it is touching this collider.</param>
        /// <returns>Whether the collider is touching this collider or not.</returns>
        public abstract bool IsTouching(Collider collider);

        /// <summary>
        /// Checks whether this collider is touching any collider on the specified layerMask or not.
        /// </summary>
        /// <param name="layerMask">Any colliders on any of these layers count as touching.</param>
        /// <returns>Whether this collider is touching any colliders on the specified layerMask or not.</returns>
        public abstract bool IsTouchingLayers(int layerMask);

        /// <summary>
        /// Check if a collider overlaps a point in space.
        /// </summary>
        /// <param name="point">A point in world space.</param>
        /// <returns>Does point overlap the collider?</returns>
        public abstract bool OverlapPoint(Vector2 point);

        /// <summary>
        /// An axis-aligned bounding box (AABB) that encloses the collider.
        /// </summary>
        public abstract Bounds Bounds();
    }
}
