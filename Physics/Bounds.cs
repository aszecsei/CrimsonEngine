using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FarseerPhysics.Collision;

namespace CrimsonEngine.Physics
{
    /// <summary>
    /// Represents an axis aligned bounding box.
    /// </summary>
    /// <remarks>
    /// An axis-aligned bounding box, or AABB for short, is a box aligned with coordinate axes and
    /// fully enclosing some object. Because the box is never rotated with respect to the axes, it
    /// can be defined by just its center and extents, or alternatively by min and max points.
    /// </remarks>
    public class Bounds
    {
        /// <summary>
        /// The center of the bounding box.
        /// </summary>
        public Vector2 center;

        /// <summary>
        /// The total size of the box. This is always twice as large as the extents.
        /// </summary>
        public Vector2 size;

        /// <summary>
        /// The extents of the box. This is always half of the size.
        /// </summary>
        public Vector2 extents
        {
            get
            {
                return size / 2.0f;
            }
        }

        /// <summary>
        /// The maximal point of the box. This is always equal to center+extents.
        /// </summary>
        public Vector2 max
        {
            get
            {
                return center + extents;
            }
        }

        /// <summary>
        /// The minimal point of the box. This is always equal to center-extents.
        /// </summary>
        public Vector2 min
        {
            get
            {
                return center - extents;
            }
        }

        public Bounds(Vector2 center, Vector2 size)
        {
            this.center = center;
            this.size = size;
        }

        public Bounds(float top, float left, float bottom, float right)
        {
            this.center = new Vector2((left + right) / 2f, (top + bottom) / 2f);
            this.size = new Vector2(Mathf.Abs(right - left), Mathf.Abs(top - bottom));
        }

        public static bool Collides(Bounds lhs, Bounds rhs)
        {
            if (lhs.min.x > rhs.max.x || rhs.min.x > lhs.max.x || lhs.min.y > rhs.max.y || rhs.min.y > lhs.max.y)
                return false;
            return true;
        }

        public Bounds Expand(float size)
        {
            return new Bounds(this.center, this.size + new Vector2(size, size));
        }
    }
}
