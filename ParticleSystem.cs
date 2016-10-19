using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public struct Particle
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
        // TODO: Implement this
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

    public struct VelocityOverLifetimeModule
    {

    }

    public class ParticleSystem : Component
    {
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

        private bool _isPlaying = true;
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


        private Particle[] particles;

        void Start()
        {
            particles = new Particle[2];
        }

        void FixedUpdate()
        {

        }
    }
}
