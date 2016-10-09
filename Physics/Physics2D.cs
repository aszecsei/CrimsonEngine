using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    public static class Physics2D
    {
        public static Vector2 Gravity = new Vector2(0, -0.9f);

        public static bool drawDebugPhysics = true;
    }
}
