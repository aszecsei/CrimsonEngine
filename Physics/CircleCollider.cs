using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    class CircleCollider : Collider
    {
        /// <summary>
        /// The radius of the circle collider.
        /// </summary>
        public float radius = 1f;

        public override bool IsTouching(Collider collider)
        {
            // First, check if the bounds are touching. This is computationally easy, so if we don't need to do anything else,
            // why bother?
            Bounds mBounds = this.Bounds();
            Bounds oBounds = collider.Bounds();
            if (!mBounds.CollidesWith(oBounds))
            {
                return false;
            }

            if(collider is BoxCollider)
            {
                // TODO: Implement this.
                throw new NotImplementedException();
            }
            else if(collider is CircleCollider)
            {
                // Calculate distance between center points
                float distSq = Vector2.DistanceSquared(offset + Helpers.extractFromVector3(GameObject.Transform.GlobalPosition), collider.offset + Helpers.extractFromVector3(collider.GameObject.Transform.GlobalPosition));
                float combinedSizes = (radius + (collider as CircleCollider).radius);
                combinedSizes *= combinedSizes;
                // GOD this was easy
                return distSq < combinedSizes;
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
            Bounds b = Bounds();
            if (b.Left > point.X || b.Right < point.X || b.Top < point.Y || b.Bottom > point.Y)
                return false;

            // Get distance squared from point to offset
            float distSq = Vector2.DistanceSquared(b.Center, point);
            return distSq < (radius * radius);
        }

        public override Bounds Bounds()
        {
            return new Bounds(offset + Helpers.extractFromVector3(GameObject.Transform.GlobalPosition), new Vector2(radius));
        }

        public override void Update()
        {
            base.Update();
            if (Physics2D.drawDebugPhysics)
            {
                Debug.DrawDebugCircle(offset + Helpers.extractFromVector3(GameObject.Transform.GlobalPosition), radius, Color.Green, 1);
            }
        }
    }
}
