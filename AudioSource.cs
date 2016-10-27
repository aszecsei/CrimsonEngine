using Microsoft.Xna.Framework.Audio;

namespace CrimsonEngine
{
    public class AudioSource : Component
    {
        internal AudioEmitter emitter = new Microsoft.Xna.Framework.Audio.AudioEmitter();

        public SoundEffect clip;
        private SoundEffectInstance _instance;

        /// <summary>
        /// Sets the Doppler scale for this AudioSource.
        /// </summary>
        public float dopplerLevel { get { return emitter.DopplerScale; } set { emitter.DopplerScale = value; } }

        /// <summary>
        /// Allows AudioSource to play even though AudioListener.pause is set to true. This is useful
        /// for the menu element sounds or background music in pause menus.
        /// </summary>
        public bool ignoreListenerPause = false;

        /// <summary>
        /// This makes the audio source not take into account the volume of the audio listener.
        /// </summary>
        public bool ignoreListenerVolume = false;

        /// <summary>
        /// Is the clip playing right now (Read Only)?
        /// </summary>
        public bool isPlaying
        {
            get { return _instance != null && _instance.State == SoundState.Playing; }
        }

        /// <summary>
        /// Is the audio clip looping?
        /// </summary>
        private bool _loop = false;
        public bool loop
        {
            get { return _loop; }
            set
            {
                _loop = value;
                if (_instance != null)
                    _instance.IsLooped = value;
            }
        }

        /// <summary>
        /// Un- / Mutes the AudioSource. Mute sets the volume=0, Un-Mute restore the original volume.
        /// </summary>
        public bool mute = false;

        /// <summary>
        /// Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo.
        /// </summary>
        public float panStereo = 0f;

        /// <summary>
        /// The pitch of the audio source.
        /// </summary>
        public float pitch = 1f;

        /// <summary>
        /// If set to true, the audio source will automatically start playing on awake.
        /// </summary>
        public bool playOnAwake = true;

        /// <summary>
        /// Enables or disables spatialization.
        /// </summary>
        public bool spatialize = true;

        /// <summary>
        /// Whether the Audio Source should be updated in the fixed or dynamic update.
        /// </summary>
        public AudioVelocityUpdateMode velocityUpdateMode = AudioVelocityUpdateMode.Auto;

        /// <summary>
        /// The volume of the audio source (0.0 to 1.0).
        /// </summary>
        public float volume = 1f;

        /// <summary>
        /// Pauses playing the clip.
        /// </summary>
        public void Pause()
        {
            if (_instance != null)
                _instance.Pause();
        }

        /// <summary>
        /// Plays the clip.
        /// </summary>
        public void Play()
        {
            if(_instance == null)
            {
                _instance = clip.CreateInstance();
                _instance.IsLooped = loop;
                _instance.Pan = panStereo;
                _instance.Pitch = pitch;
                _instance.Volume = volume * (ignoreListenerVolume ? 1.0f : AudioListener.volume) * (mute ? 0f : 1f);
            }

            _instance.Play();
        }

        /// <summary>
        /// Plays the clip with a delay specified in seconds.
        /// </summary>
        /// <param name="delay">Delay time specified in seconds.</param>
        public void PlayDelayed(float delay)
        {

        }

        /// <summary>
        /// Plays an AudioClip, and scales the AudioSource volume by volumeScale.
        /// </summary>
        /// <param name="clip">The clip being played.</param>
        /// <param name="volumeScale">The scale of the volume (0-1).</param>
        public void PlayOneShot(SoundEffect clip, float volumeScale = 1.0f)
        {
            clip.Play(volume * volumeScale * (ignoreListenerVolume ? 1.0f : AudioListener.volume) * (mute ? 0f : 1f), pitch, panStereo);
        }

        /// <summary>
        /// Stops playing the clip.
        /// </summary>
        public void Stop()
        {
            if (_instance != null)
                _instance.Stop();
        }

        /// <summary>
        /// Unpause the paused playback of this AudioSource.
        /// </summary>
        /// <remarks>
        /// This function is similar to calling Play () on a paused AudioSource, except that it will not create a new playback voice if it is not currently paused.
        /// </remarks>
        public void UnPause()
        {
            if (_instance != null)
                _instance.Resume();
        }

        void Start()
        {
            if (playOnAwake)
                Play();
        }

        void Update()
        {
            if(spatialize)
            {
                if (velocityUpdateMode == AudioVelocityUpdateMode.Dynamic || (velocityUpdateMode == AudioVelocityUpdateMode.Auto && GetComponent<Physics.Rigidbody>() == null))
                {
                    emitter.Position = transform.GlobalPosition;
                    _instance.Apply3D(AudioListener._listener.listener, emitter);
                }
            }
        }

        void FixedUpdate()
        {
            if (spatialize)
            {
                if (velocityUpdateMode == AudioVelocityUpdateMode.Fixed || (velocityUpdateMode == AudioVelocityUpdateMode.Auto && GetComponent<Physics.Rigidbody>() != null))
                {
                    emitter.Position = transform.GlobalPosition;
                    _instance.Apply3D(AudioListener._listener.listener, emitter);
                }
            }
        }
    }
}
