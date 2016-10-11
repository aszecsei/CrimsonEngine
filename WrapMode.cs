using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    /// <summary>
    /// Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.
    /// </summary>
    public enum WrapMode
    {
        /// <summary>
        /// When time reaches the end of the animation clip, the clip will automatically stop playing and time will be reset to beginning of the clip.
        /// </summary>
        Once,
        /// <summary>
        /// When time reaches the end of the animation clip, time will continue at the beginning.
        /// </summary>
        Loop,
        /// <summary>
        /// When time reaches the end of the animation clip, time will ping pong back between beginning and end.
        /// </summary>
        PingPong,
        /// <summary>
        /// 	Reads the default repeat mode set higher up.
        /// </summary>
        Default,
        /// <summary>
        /// Plays back the animation. When it reaches the end, it will keep playing the last frame and never stop playing.
        /// </summary>
        ClampForever
    }
}
