using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public class Time
    {
        public static float time = 0.0f;
        public static float deltaTime = 0.0f;
        public static float fixedDeltaTime = 0.0f;
        public static float fixedTime = 0.0f;
        public static float timeScale = 1.0f;
        public static float fixedTimestep = 1.0f / 30.0f;
    }
}
