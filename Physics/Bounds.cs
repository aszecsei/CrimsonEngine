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
        public readonly Vector2 Center;
        public readonly Vector2 Size;

        public float Left
        {
            get
            {
                return Center.X - (Size.X / 2.0f);
            }
        }

        public float Right
        {
            get
            {
                return Center.X + (Size.X / 2.0f);
            }
        }

        public float Top
        {
            get
            {
                return Center.Y + (Size.Y / 2.0f);
            }
        }

        public float Bottom
        {
            get
            {
                return Center.Y - (Size.Y / 2.0f);
            }
        }

        public Bounds(Vector2 center, Vector2 size)
        {
            Center = center;
            Size = size;
        }

        public Bounds(float left, float top, float right, float bottom)
        {
            Center = new Vector2((left + right) / 2.0f, (top + bottom) / 2.0f);
            Size = new Vector2(Math.Abs(right - left), Math.Abs(top - bottom));
        }

        public bool CollidesWith(Bounds otherBounds)
        {
            if(otherBounds.Left > Right || otherBounds.Top < Bottom || otherBounds.Right < Left || otherBounds.Bottom > Top)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
