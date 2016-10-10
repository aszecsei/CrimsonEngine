using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public class WaitForSeconds : YieldInstruction
    {
        public float duration = 0.0f;

        public WaitForSeconds(float timeToWait = 1.0f)
        {
            this.duration = timeToWait;
        }
    }
}
