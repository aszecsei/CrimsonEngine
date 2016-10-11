using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    /// <summary>
    /// Store a collection of Keyframes that can be evaluated over time.
    /// </summary>
    public class AnimationCurve
    {
        public Keyframe[] keys;

        public int length { get { return keys.Length; } }

        /// <summary>
        /// The behavior of the animation after the last keyframe.
        /// </summary>
        public WrapMode postWrapMode;

        /// <summary>
        /// The behavior of the animation before the first keyframe.
        /// </summary>
        public WrapMode preWrapMode;

        /// <summary>
        /// Retrieves the key at index. (Read Only)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Keyframe this[int index]
        {
            get { return keys[index]; }
        }

        /// <summary>
        /// Add a new key to the curve.
        /// </summary>
        /// <remarks>
        /// Smooth tangents are automatically computed for the key. Returns the index of the added key. If no key could be added because there is already another keyframe at the same time -1 will be returned.
        /// </remarks>
        /// <param name="time">The time at which to add the key (horizontal axis in the curve graph).</param>
        /// <param name="value">The value for the key (vertical axis in the curve graph).</param>
        /// <returns>The index of the added key, or -1 if the key could not be added.</returns>
        public int AddKey(float time, float value)
        {
            return AddKey(new Keyframe(time, value));
        }

        /// <summary>
        /// Add a new key to the curve.
        /// </summary>
        /// <remarks>
        /// Returns the index of the added key. If no key could be added because there is already another keyframe at the same time -1 will be returned.
        /// </remarks>
        /// <param name="key">The key to add to the curve.</param>
        /// <returns>The index of the added key, or -1 if the key could not be added.</returns>
        public int AddKey(Keyframe key)
        {
            foreach(Keyframe k in keys)
            {
                if (k.time == key.time)
                    return -1;
            }

            Keyframe[] newKeys = new Keyframe[length + 1];
            int counter = 0;
            int offset = 0;
            int keyIndex = 0;
            foreach (Keyframe k in keys)
            {
                if (offset == 0 && k.time > key.time)
                {
                    newKeys[counter] = key;
                    keyIndex = counter;
                    offset = 1;
                }
                newKeys[counter + offset] = k;
                counter++;
            }

            keys = newKeys;

            return keyIndex;
        }

        /// <summary>
        /// Removes a key.
        /// </summary>
        /// <param name="index">The index of the key to remove.</param>
        public void RemoveKey(int index)
        {
            Keyframe[] newKeys = new Keyframe[length - 1];
            int offset = 0;
            for (int i=0; i<length; i++)
            {
                if (i == index)
                {
                    offset = -1;
                    continue;
                }
                newKeys[i + offset] = keys[i];
            }
        }

        /// <summary>
        /// Smooth the in and out tangents of the keyframe at index.
        ///
        /// A weight of 0 evens out tangents.
        /// </summary>
        /// <param name="index">The index of the keyframe to be smoothed.</param>
        /// <param name="weight">The smoothing weight to apply to the keyframe's tangents.</param>
        public void SmoothTangents(int index, float weight)
        {

        }

        /// <summary>
        /// Evaluate the curve at time.
        /// </summary>
        /// <param name="time">The time within the curve you want to evaluate (the horizontal axis in the curve graph).</param>
        /// <returns>The value of the curve, at the point in time specified.</returns>
        public float Evaluate(float time)
        {
            // TODO: Implement this

            // for now, average the values between the two closest times
            Keyframe leftKey = new Keyframe(0f, 0f);
            Keyframe rightKey = new Keyframe(0f, 0f);
            for (int i=0; i<length - 1; i++)
            {
                if (Mathf.Approximately(keys[i].time, time))
                {
                    return keys[i].value;
                }
                else if(keys[i+1].time > time)
                {
                    leftKey = keys[i];
                    rightKey = keys[i + 1];
                }
            }

            return Mathf.Lerp(leftKey.value, rightKey.value, (time - leftKey.time) / (rightKey.time - leftKey.time));
        }

        /// <summary>
        /// Creates an animation curve from an arbitrary number of keyframes.
        /// </summary>
        /// <param name="keys">An array of Keyframes used to define the curve.</param>
        public AnimationCurve(params Keyframe[] keys)
        {
            this.keys = keys;
        }
    }
}
