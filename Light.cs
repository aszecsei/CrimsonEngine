using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine
{
    public class Light : Component
    {
        public Color Color = Color.White;
        public float Intensity = 1f;
        public float Range = 200f;
        public float Length = 0f;
        public float Rotation = 0f;
        public float ConeAngle = 360f;
        public float PenumbraAngle = 10f;
    }
}
