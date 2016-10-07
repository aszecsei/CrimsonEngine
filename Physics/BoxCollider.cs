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

        new public Bounds Bounds
        {
            get
            {
                // This holds true when ignoring rotation

                // TODO: Change this based on rotation.

                Vector3 gp = GameObject.Transform.GlobalPosition;
                return new Bounds(new Vector2(offset.X + gp.X, offset.Y + gp.Y), size);
            }
        }

        public override bool IsTouching(Collider collider)
        {
            // First, check if the bounds are touching. This is computationally easy.
            Bounds mBounds = this.Bounds;
            Bounds oBounds = collider.Bounds;
            if(!mBounds.CollidesWith(oBounds))
            {
                return false;
            }

            // If the bounds are colliding, we should do more interesting calculations to detect collision.
            if(collider is BoxCollider)
            {
                // TODO: Perform non-axis aligned box collision.
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
