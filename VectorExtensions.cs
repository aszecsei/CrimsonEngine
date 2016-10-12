using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine
{
    public static class VectorExtensions
    {
        public static Vector2 right
        {
            get
            {
                return new Vector2(1, 0);
            }
        }

        public static Vector2 left
        {
            get
            {
                return new Vector2(-1, 0);
            }
        }

        public static Vector2 up
        {
            get
            {
                return new Vector2(0, 1);
            }
        }

        public static Vector2 down
        {
            get
            {
                return new Vector2(0, -1);
            }
        }

        /// <summary>
        /// Returns the angle between two vectors in degrees.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static float Angle(Vector2 left, Vector2 right)
        {
            return (float)Math.Acos(Vector2.Dot(left, right)) * Mathf.RAD_TO_DEG;
        }

        public static Vector2 Normalized(Vector2 value)
        {
            Vector2 v = new Vector2(value.x, value.y);
            v.Normalize();
            return v;
        }
    }
}
