using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine
{
    class Helpers
    {
        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        public static Vector2 rotate(Vector2 point, Vector2 origin, float rotation)
        {
            Vector2 centered = point - origin;
            return new Vector2(centered.X * (float)Math.Cos(rotation) - centered.Y * (float)Math.Sin(rotation), centered.X * (float)Math.Sin(rotation) + centered.Y * (float)Math.Cos(rotation)) + origin;
        }
    }
}
