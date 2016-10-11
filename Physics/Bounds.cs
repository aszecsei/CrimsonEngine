using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    public class Bounds
    {
        public Vector2 center = Vector2.Zero;
        public Vector2 size = Vector2.Zero;

        public Vector2 extents
        {
            get
            {
                return size / 2f;
            }
        }

        public Vector2 max
        {
            get
            {
                return center + extents;
            }
        }

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

        public Bounds(float left, float top, float right, float bottom)
        {
            this.center = new Vector2((left + right) / 2f, (top + bottom) / 2f);
            this.size = new Vector2(right - left, top - bottom);
        }

        /// <summary>
        /// Expand the bounds by increasing its size by amount along each side.
        /// </summary>
        /// <param name="amount"></param>
        public void Expand(float amount)
        {
            this.size.X += amount;
            this.size.Y += amount;
        }


    }
}
