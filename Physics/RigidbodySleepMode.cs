using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine.Physics
{
    /// <summary>
    /// Settings for a rigidbody's inital sleep state.
    /// </summary>
    public enum RigidbodySleepMode
    {
        /// <summary>
        /// Rigidbody never automatically sleeps.
        /// </summary>
        NeverSleep,
        /// <summary>
        /// Rigidbody is initially awake.
        /// </summary>
        StartAwake,
        /// <summary>
        /// Rigidybody is initially asleep.
        /// </summary>
        StartAsleep
    }
}
