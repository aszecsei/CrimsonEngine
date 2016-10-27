using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    /// <summary>
    /// Describes when an AudioSource or AudioListener is updated.
    /// </summary>
    public enum AudioVelocityUpdateMode
    {
        /// <summary>
        /// Updates the source or listener in the fixed update loop if it is attached to a Rigidbody, dynamic otherwise.
        /// </summary>
        Auto,
        /// <summary>
        /// Updates the source or listener in the fixed update loop.
        /// </summary>
        Fixed,
        /// <summary>
        /// Updates the source or listener in the dynamic update loop.
        /// </summary>
        Dynamic
    }

    public class AudioListener : Component
    {
        internal Microsoft.Xna.Framework.Audio.AudioListener listener = new Microsoft.Xna.Framework.Audio.AudioListener();
        internal static AudioListener _listener = null;
        public AudioListener()
        {
            if (_listener != null)
                throw new Exception("Only one audio listener may be present in a scene.");
            _listener = this;
        }

        /// <summary>
        /// The paused state of the audio system.
        /// </summary>
        /// <remarks>
        /// If set to true, all AudioSources playing will be paused. This works in the same way as pausing the game in the editor.
        /// While the pause-state is true, the AudioSettings.dspTime will be frozen and further AudioSource play requests will start
        /// off paused. If you want certain sounds to still play during the pause, you need to set the ignoreListenerPause property
        /// on the AudioSource to true for these. This is typically menu item sounds or background music for the menu. Any scheduled
        /// play requests will be frozen in time, so that if you scheduled a sound to play after 3 seconds and paused the audio system
        /// 1 second after this, the scheduled sounds will start playing 2 seconds after unpausing.
        /// </remarks>
        public static bool pause = false;

        /// <summary>
        /// Controls the game sound volume (0.0 to 1.0).
        /// </summary>
        public static float volume = 1f;

        /// <summary>
        /// This lets you set whether the Audio Listener should be updated in the fixed or dynamic update.
        /// </summary>
        /// <remarks>
        /// Make sure this is set to update in the same update loop as the Audio Listener is moved in if you are experiencing problems
        /// with Doppler effect simulation. The default setting will automatically set the listener to be updated in the fixed update
        /// loop if it is attached to a rigidbody, and dynamic otherwise.
        /// </remarks>
        public AudioVelocityUpdateMode velocityUpdateMode = AudioVelocityUpdateMode.Auto;

        void Update()
        {
            if(velocityUpdateMode == AudioVelocityUpdateMode.Dynamic || (velocityUpdateMode == AudioVelocityUpdateMode.Auto && GetComponent<Physics.Rigidbody>() == null))
            {
                listener.Position = transform.GlobalPosition;
            }
        }

        void FixedUpdate()
        {
            if (velocityUpdateMode == AudioVelocityUpdateMode.Fixed || (velocityUpdateMode == AudioVelocityUpdateMode.Auto && GetComponent<Physics.Rigidbody>() != null))
            {
                listener.Position = transform.GlobalPosition;
            }
        }
    }
}
