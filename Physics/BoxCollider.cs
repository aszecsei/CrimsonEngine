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
        public Vector2 size = Vector2.One;

        protected Rect rotated
        {
            get
            {
                Rect result = new Rect();

                // Calculate the positions of all 4 corners of the box after all rotation is applied
                float left = offset.X - (size.X / 2);
                float right = offset.X + (size.X / 2);
                float top = offset.Y + (size.Y / 2);
                float bottom = offset.Y - (size.Y / 2);
                Vector2 topLeft = new Vector2(left, top);
                Vector2 topRight = new Vector2(right, top);
                Vector2 bottomLeft = new Vector2(left, bottom);
                Vector2 bottomRight = new Vector2(right, bottom);

                result.topLeft = Helpers.rotate(topLeft, Vector2.Zero, GameObject.Transform.GlobalRotation);
                result.topRight = Helpers.rotate(topRight, Vector2.Zero, GameObject.Transform.GlobalRotation);
                result.bottomLeft = Helpers.rotate(bottomLeft, Vector2.Zero, GameObject.Transform.GlobalRotation);
                result.bottomRight = Helpers.rotate(bottomRight, Vector2.Zero, GameObject.Transform.GlobalRotation);

                return result;
            }
        }

        protected Rect rotatedGlobal
        {
            get
            {
                Rect result = rotated;

                Vector3 gp = GameObject.Transform.GlobalPosition;
                Vector2 gp2 = new Vector2(gp.X, gp.Y);
                result.bottomLeft += gp2;
                result.bottomRight += gp2;
                result.topLeft += gp2;
                result.topRight += gp2;

                return result;
            }
        }

        /// <summary>
        /// An axis-aligned bounding box (AABB) that encloses the collider.
        /// </summary>
        public override Bounds Bounds()
        {
                Rect rot = rotated;

                // Those 4 corners are going to have the minimum/maximum bounds for any rotation, and will define our new
                // AABB.
                Vector3 gp = GameObject.Transform.GlobalPosition;
                float left = Math.Min(Math.Min(Math.Min(rot.topLeft.X, rot.topRight.X), rot.bottomLeft.X), rot.bottomRight.X) + gp.X;
                float right = Math.Max(Math.Max(Math.Max(rot.topLeft.X, rot.topRight.X), rot.bottomLeft.X), rot.bottomRight.X) + gp.X;
                float bottom = Math.Min(Math.Min(Math.Min(rot.topLeft.Y, rot.topRight.Y), rot.bottomLeft.Y), rot.bottomRight.Y) + gp.Y;
                float top = Math.Max(Math.Max(Math.Max(rot.topLeft.Y, rot.topRight.Y), rot.bottomLeft.Y), rot.bottomRight.Y) + gp.Y;

                return new Bounds(left, top, right, bottom);
        }

        public List<Vector2> Normals
        {
            get
            {
                List<Vector2> result = new List<Vector2>();

                Rect rot = rotated;

                Vector2 left = new Vector2(-1, 1);
                result.Add((rot.topLeft - rot.bottomLeft) * left);
                result.Add((rot.bottomLeft - rot.bottomRight) * left);

                return result;
            }
        }

        /// <summary>
        /// Checks whether this collider is touching the collider or not.
        /// </summary>
        /// <param name="collider">The collider to check if it is touching this collider.</param>
        /// <returns>Whether the collider is touching this collider or not.</returns>
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

            if (collider is BoxCollider)
            {
                // Perform SAT-flavored collision detection.
                BoxCollider bc = collider as BoxCollider;

                List<Vector2> normals = Normals;
                normals.AddRange(bc.Normals);

                Rect mRect = rotatedGlobal;
                Rect oRect = bc.rotatedGlobal;

                Vector2[] mPoints = new Vector2[4] { mRect.topLeft, mRect.topRight, mRect.bottomRight, mRect.bottomLeft };
                Vector2[] oPoints = new Vector2[4] { oRect.topLeft, oRect.topRight, oRect.bottomRight, oRect.bottomLeft };

                foreach (Vector2 axis in normals)
                {
                    // Obtain the min-max projection on this
                    float min_proj_this = Vector2.Dot(mPoints[0], axis);
                    int min_dot_this = 0;
                    float max_proj_this = Vector2.Dot(mPoints[0], axis);
                    int max_dot_this = 0;

                    for(int i=1; i<mPoints.Length; i++)
                    {
                        float curr_proj = Vector2.Dot(mPoints[i], axis);
                        if(min_proj_this > curr_proj)
                        {
                            min_proj_this = curr_proj;
                            min_dot_this = i;
                        }
                        if(curr_proj > max_proj_this)
                        {
                            max_proj_this = curr_proj;
                            max_dot_this = i;
                        }
                    }

                    // Obtain the min-max projection on other
                    float min_proj_other = Vector2.Dot(oPoints[0], axis);
                    int min_dot_other = 0;
                    float max_proj_other = Vector2.Dot(oPoints[0], axis);
                    int max_dot_other = 0;

                    for (int i = 1; i < oPoints.Length; i++)
                    {
                        float curr_proj = Vector2.Dot(oPoints[i], axis);
                        if (min_proj_other > curr_proj)
                        {
                            min_proj_other = curr_proj;
                            min_dot_other = i;
                        }
                        if (curr_proj > max_proj_other)
                        {
                            max_proj_other = curr_proj;
                            max_dot_other = i;
                        }
                    }

                    bool isSeparated = max_proj_other < min_proj_this || max_proj_this < min_proj_other;
                    if (isSeparated)
                        return false;
                }

                return true;
            }
            else if(collider is CircleCollider)
            {
                // TODO: Implement this.
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Checks whether this collider is touching any collider on the specified layerMask or not.
        /// </summary>
        /// <param name="layerMask">Any colliders on any of these layers count as touching.</param>
        /// <returns>Whether this collider is touching any colliders on the specified layerMask or not.</returns>
        public override bool IsTouchingLayers(int layerMask)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if a collider overlaps a point in space.
        /// </summary>
        /// <param name="point">A point in world space.</param>
        /// <returns>Does point overlap the collider?</returns>
        public override bool OverlapPoint(Vector2 point)
        {
            Bounds b = Bounds();
            if (b.Left > point.X || b.Right < point.X || b.Top < point.Y || b.Bottom > point.Y)
                return false;

            // Perform SAT-flavored collision detection.
            List<Vector2> normals = Normals;

            Rect mRect = rotatedGlobal;

            Vector2[] mPoints = new Vector2[4] { mRect.topLeft, mRect.topRight, mRect.bottomRight, mRect.bottomLeft };

            foreach (Vector2 axis in normals)
            {
                // Obtain the min-max projection on this
                float min_proj_this = Vector2.Dot(mPoints[0], axis);
                int min_dot_this = 0;
                float max_proj_this = Vector2.Dot(mPoints[0], axis);
                int max_dot_this = 0;

                for (int i = 1; i < mPoints.Length; i++)
                {
                    float curr_proj = Vector2.Dot(mPoints[i], axis);
                    if (min_proj_this > curr_proj)
                    {
                        min_proj_this = curr_proj;
                        min_dot_this = i;
                    }
                    if (curr_proj > max_proj_this)
                    {
                        max_proj_this = curr_proj;
                        max_dot_this = i;
                    }
                }

                // Obtain the min-max projection on other
                float proj = Vector2.Dot(point, axis);

                bool isSeparated = proj < min_proj_this || max_proj_this < proj;
                if (isSeparated)
                    return false;
            }

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Physics2D.drawDebugPhysics)
            {
                Color c = Color.Green;
                if(attachedRigidbody != null)
                {
                    if(attachedRigidbody.awake == true)
                    {
                        c = Color.Red;
                    } else
                    {
                        c = Color.Blue;
                    }
                }
                Debug.DrawDebugRectangle(offset + Helpers.extractFromVector3(GameObject.Transform.GlobalPosition), size, c, 1);
            }
        }
    }
}
