using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    public class BoxCollider : Collider
    {
        public Vector2 size;

        /// <summary>
        /// An axis-aligned bounding box (AABB) that encloses the collider.
        /// </summary>
        new public Bounds Bounds
        {
            get
            {
                // Calculate the positions of all 4 corners of the box after all rotation is applied
                float left = offset.X - (size.X / 2);
                float right = offset.X + (size.X / 2);
                float top = offset.Y + (size.Y / 2);
                float bottom = offset.Y - (size.Y / 2);
                Vector2 topLeft = new Vector2(left, top);
                Vector2 topRight = new Vector2(right, top);
                Vector2 bottomLeft = new Vector2(left, bottom);
                Vector2 bottomRight = new Vector2(right, bottom);

                topLeft = Helpers.rotate(topLeft, Vector2.Zero, GameObject.Transform.GlobalRotation);
                topRight = Helpers.rotate(topRight, Vector2.Zero, GameObject.Transform.GlobalRotation);
                bottomLeft = Helpers.rotate(bottomLeft, Vector2.Zero, GameObject.Transform.GlobalRotation);
                bottomRight = Helpers.rotate(bottomRight, Vector2.Zero, GameObject.Transform.GlobalRotation);

                // Those 4 corners are going to have the minimum/maximum bounds for any rotation, and will define our new
                // AABB.
                Vector3 gp = GameObject.Transform.GlobalPosition;
                left = Math.Min(Math.Min(Math.Min(topLeft.X, topRight.X), bottomLeft.X), bottomRight.X) + gp.X;
                right = Math.Max(Math.Max(Math.Max(topLeft.X, topRight.X), bottomLeft.X), bottomRight.X) + gp.X;
                bottom = Math.Min(Math.Min(Math.Min(topLeft.Y, topRight.Y), bottomLeft.Y), bottomRight.Y) + gp.Y;
                top = Math.Max(Math.Max(Math.Max(topLeft.Y, topRight.Y), bottomLeft.Y), bottomRight.Y) + gp.Y;

                return new Bounds(left, top, right, bottom);
            }
        }

        

        public override bool IsTouching(Collider collider)
        {
            // First, check if the bounds are touching. This is computationally easy, so if we don't need to do anything else,
            // why bother?
            Bounds mBounds = this.Bounds;
            Bounds oBounds = collider.Bounds;
            if(!mBounds.CollidesWith(oBounds))
            {
                return false;
            }

            // If the bounds are colliding, we should do more interesting calculations to detect collision.
            if(collider is BoxCollider)
            {
                // TODO: Perform SAT-flavored collision detection.
                // For now, just return true.
                return true;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsTouchingLayers(int layerMask)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public override bool OverlapPoint(Vector2 point)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }
    }
}
