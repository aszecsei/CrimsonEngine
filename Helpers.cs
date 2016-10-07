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
            return new Vector2((point.X - origin.X) * (float)Math.Cos(rotation), (point.Y - origin.Y) * (float)Math.Sin(rotation));
        }
    }
}
