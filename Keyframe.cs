using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public struct Keyframe
    {
        /// <summary>
        /// Describes the tangent when approaching this point from the previous point in the curve.
        /// </summary>
        /// <remarks>
        /// The angle needs to be expressed in radians.
        /// 
        /// The tangent is really just the gradient of the slope, as in "change in y / change in x"
        /// </remarks>
        public float inTangent;

        /// <summary>
        /// Describes the tangent when leaving this point towards the next point in the curve.
        /// </summary>
        /// <remarks>
        /// The angle needs to be expressed in radians.
        /// 
        /// The tangent is really just the gradient of the slope, as in "change in y / change in x"
        /// </remarks>
        public float outTangent;

        /// <summary>
        /// The time of the keyframe.
        /// </summary>
        /// <remarks>
        /// In a 2D graph you could think of this as the x-value.
        /// </remarks>
        public float time;

        /// <summary>
        /// The value of the curve at keyframe.
        /// </summary>
        /// <remarks>
        /// In a 2D graph you could think of this as the y-value.
        /// </remarks>
        public float value;

        /// <summary>
        /// Create a keyframe.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        public Keyframe(float time, float value)
        {
            this.time = time;
            this.value = value;
            this.inTangent = 0f;
            this.outTangent = 0f;
        }

        /// <summary>
        /// Create a keyframe.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <param name="inTangent"></param>
        /// <param name="outTangent"></param>
        public Keyframe(float time, float value, float inTangent, float outTangent)
        {
            this.time = time;
            this.value = value;
            this.inTangent = inTangent;
            this.outTangent = outTangent;
        }
    }
}
