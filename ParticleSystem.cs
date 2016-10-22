using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    [RequireComponent(typeof(ParticleSystemRenderer))]
    public class Particle
    {
        /// <summary>
        /// The angular velocity of the particle.
        /// </summary>
        public float angularVelocity;

        /// <summary>
        /// The lifetime of the particle.
        /// </summary>
        public float lifetime;

        /// <summary>
        /// The position of the particle.
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// The random seed of the particle.
        /// </summary>
        public int randomSeed;

        /// <summary>
        /// The rotation of the particle.
        /// </summary>
        public float rotation;

        /// <summary>
        /// The initial color of the particle. The current color of the particle
        /// is calculated procedurally based on this value and the active color modules.
        /// </summary>
        public Color startColor;

        /// <summary>
        /// The starting lifetime of the particle.
        /// </summary>
        public float startLifetime;

        /// <summary>
        /// The initial size of the particle. The current size of the particle is calculated
        /// procedurally based on this value and the active size modules.
        /// </summary>
        public float startSize;

        /// <summary>
        /// The velocity of the particle.
        /// </summary>
        public Vector2 velocity;

        /// <summary>
        /// Calculate the current color of the particle by applying the relevant curves to its
        /// startColor property.
        /// </summary>
        /// <returns></returns>
        public Color GetCurrentColor()
        {
            return startColor;
        }

        /// <summary>
        /// Calculate the current size of the particle by applying the relevant curves to its
        /// startSize property.
        /// </summary>
        /// <returns></returns>
        public float GetCurrentSize()
        {
            return startSize;
        }

        internal void Update(float deltaTime)
        {
            lifetime -= deltaTime;
            position += velocity * deltaTime;
            rotation += angularVelocity * deltaTime;
        }

        internal Material material;
    }

    public struct CollisionModule
    {
        // TODO: Implement this
    }

    public struct ColorBySpeedModule
    {
        // TODO: Implement this
    }

    public struct ColorOverLifetimeModule
    {
        // TODO: Implement this
    }

    public struct EmissionModule
    {
        /// <summary>
        /// The mode in which particles are emitted.
        /// </summary>
        public enum ParticleSystemEmissionType
        {
            /// <summary>
            /// Emit over time.
            /// </summary>
            Time,
            /// <summary>
            /// Emit when emitter moves.
            /// </summary>
            Distance
        }

        /// <summary>
        /// The current number of bursts.
        /// </summary>
        public int burstCount;

        /// <summary>
        /// Enable/disable the Emission module.
        /// </summary>
        public bool enabled;

        // TODO: Make this a curve
        /// <summary>
        /// The rate at which new particles are spawned.
        /// </summary>
        public float rate;

        /// <summary>
        /// The emission type.
        /// </summary>
        public ParticleSystemEmissionType type;
    }

    public struct EmitParams
    {
        // TODO: Implement this
    }

    public struct ForceOverLifetimeModule
    {
        
    }

    public struct InheritVelocityModule
    {

    }

    public struct LimitVelocityOverLifetimeModule
    {

    }

    public struct RotationBySpeedModule
    {

    }

    public struct RotationOverLifetimeModule
    {

    }

    public struct ShapeModule
    {

    }

    public struct SizeBySpeedModule
    {

    }

    public struct SizeOverLifetimeModule
    {

    }

    public struct TextureSheetAnimation
    {

    }

    public struct VelocityOverLifetimeModule
    {

    }

    [RequireComponent(typeof(ParticleSystemRenderer))]
    public class ParticleSystem : Component
    {
        private Particle[] particles;
        private bool _isPlaying = true;
        private int _numParticles = 0;
        private uint _randomSeed = 0;

        public CollisionModule collision = new CollisionModule();
        public ColorBySpeedModule colorBySpeed = new ColorBySpeedModule();
        public ColorOverLifetimeModule colorOverLifetime = new ColorOverLifetimeModule();

        /// <summary>
        /// The duration of the particle system in seconds (Read Only).
        /// </summary>
        public float duration
        {
            get
            {
                // TODO: Implement this
                return 0f;
            }
        }

        public EmissionModule emission = new EmissionModule();
        public ForceOverLifetimeModule forceOverLifetime = new ForceOverLifetimeModule();
        public float gravityModifier = 1.0f;
        public InheritVelocityModule inheritVelocity = new InheritVelocityModule();

        
        public bool isPaused
        {
            get { return !_isPlaying; }
            set { _isPlaying = !value; }
        }
        public bool isPlaying
        {
            get { return _isPlaying; }
            set { _isPlaying = value; }
        }
        public LimitVelocityOverLifetimeModule limitVelocityOverLifetime = new LimitVelocityOverLifetimeModule();
        public bool loop = true;
        public List<Material> materials = new List<Material>();
        public int maxParticles = 1000;

        public int particleCount
        {
            get { return _numParticles; }
        }

        public float playbackSpeed = 1f;
        public bool playOnAwake = true;

        public uint randomSeed
        {
            get { return _randomSeed; }
            set { _randomSeed = value; useAutoRandomSeed = false; }
        }
        public ParticleSystemRenderer renderer;
        public RotationBySpeedModule rotationBySpeed = new RotationBySpeedModule();
        public RotationOverLifetimeModule rotationOverLifetime = new RotationOverLifetimeModule();
        public Space simulationSpace = Space.World;
        public SizeBySpeedModule sizeBySpeed = new SizeBySpeedModule();
        public SizeOverLifetimeModule sizeOverLifetime = new SizeOverLifetimeModule();
        public Color startColor = Color.white;
        public float startDelay = 0f;
        public float startLifetime = 5f;
        public float startRotation = 0f;
        public float startSize = 1f;
        public Vector2 startSpeed = Vector2.zero;
        public TextureSheetAnimation textureSheetAnimation = new TextureSheetAnimation();
        public float time;
        public bool useAutoRandomSeed = true;
        public VelocityOverLifetimeModule velocityOverLifetime = new VelocityOverLifetimeModule();

        public void Clear()
        {

        }

        private Random random;

        public void Emit(int count)
        {
            for(int i=0; i< Mathf.Min(count, maxParticles - _numParticles); i++)
            {
                Particle p = new Particle();

                p.angularVelocity = 0f;
                p.lifetime = startLifetime;
                p.material = materials[random.Next(materials.Count)];
                p.position = simulationSpace == Space.Local ? Vector2.zero : (Vector2)GameObject.transform.GlobalPosition;
                p.rotation = startRotation;
                p.startColor = startColor;
                p.startLifetime = startLifetime;
                p.startSize = startSize;
                p.velocity = startSpeed;

                particles[_numParticles + i] = p;
            }
            _numParticles += count;
        }
        public void Emit(EmitParams emitParams, int count)
        {

        }

        private void CleanUp()
        {
            int pointer = 0;
            int removed = 0;
            for (int traverse=0; traverse<_numParticles; traverse++)
            {
                if(particles[traverse].lifetime <= 0f)
                {
                    particles[traverse] = null;
                    removed++;
                }
                else
                {
                    if (pointer != traverse)
                    {
                        particles[pointer] = particles[traverse];
                        particles[traverse] = null;
                    }
                    pointer++;
                }
            }

            _numParticles -= removed;
        }

        public int GetParticles(Particle[] particles)
        {
            int numParticles = Math.Min(_numParticles, particles.Length);
            for (int i=0; i<numParticles; i++)
            {
                particles[i] = this.particles[i];
            }
            return numParticles;
        }

        public bool IsAlive()
        {
            // TODO: Implement this
            return true;
        }

        public void Pause()
        {
            _isPlaying = false;
        }

        public void Play()
        {
            _isPlaying = true;
        }

        public void SetParticles(Particle[] particles, int size)
        {
            for(int i=0; i< size; i++)
            {
                this.particles[i] = particles[i];
            }
            for(int i= size; i<this.particles.Length; i++)
            {
                this.particles[i] = null;
            }
            this._numParticles = size;
        }

        public void Simulate(float t, bool restart = true, bool fixedTimestep = true)
        {
            for(int i=0; i<_numParticles; i++)
            {
                particles[i].Update(t);
            }
        }

        public void Stop()
        {

        }

        void Start()
        {
            particles = new Particle[maxParticles];
            renderer = GetComponent<ParticleSystemRenderer>();
            random = useAutoRandomSeed ? new Random() : new Random((int)randomSeed);
            emission.type = EmissionModule.ParticleSystemEmissionType.Time;
            emission.rate = 60;
            emission.enabled = true;
        }

        void Update()
        {
            // do the modules
            HandleEmission();

            time += Time.deltaTime;
            for (int i = 0; i < _numParticles; i++)
            {
                particles[i].Update(Time.deltaTime);
            }

            CleanUp();
        }

        private float _timePassedSinceEmission = 0f;
        private void HandleEmission()
        {
            if(emission.enabled)
            {
                if (emission.type == EmissionModule.ParticleSystemEmissionType.Time)
                {
                    // get the number of particles emitted by the delta time
                    _timePassedSinceEmission += Time.deltaTime;
                    while (_timePassedSinceEmission > (1f / emission.rate))
                    {
                        Emit(1);
                        _timePassedSinceEmission -= (1f / emission.rate);
                    }
                }
                else
                {
                    // TODO: Implement distance-based particle emission
                }
            }
        }
    }
}
