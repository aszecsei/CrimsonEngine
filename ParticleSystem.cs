using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    public class Particle
    {
        public Vector2 position = Vector2.zero;
        public Vector2 velocity = Vector2.zero;

        public float rotation = 0f;
        public float angularVelocity = 0f;

        public Vector2 scale = Vector2.one;

        public Material material;
        public Color color = Color.white;

        public float initialLifetime = 5.0f;
        public float ttl;
        public ParticleSystem parent { get; internal set; }

        public Random random;
    }

    public abstract class ParticleComponent
    {
        public bool enabled = true;

        public virtual void Initialize(Particle p) { }

        public virtual void Update(Particle p, float deltaTime) { }

        public virtual void UpdateParticleSystem(ParticleSystem ps, float deltaTime) { }
    }

    public class GravityParticleComponent : ParticleComponent
    {
        public Vector2 gravity = new Vector2(0, -10f);

        public override void Update(Particle p, float deltaTime)
        {
            p.velocity += gravity * deltaTime;
        }
    }

    public class ColorOverLifetimeParticleComponent : ParticleComponent
    {
        public Color initialColor = Color.white;
        public Color finalColor = Color.clear;

        public override void Initialize(Particle p)
        {
            p.color = initialColor;
        }

        public override void Update(Particle p, float deltaTime)
        {
            p.color = Color.Lerp(initialColor, finalColor, (p.initialLifetime - p.ttl) / p.initialLifetime);
        }
    }

    public class SizeOverLifetimeParticleComponent : ParticleComponent
    {
        public float initialSize = 0.25f;
        public float finalSize = 0f;

        public override void Initialize(Particle p)
        {
            p.scale = new Vector2(initialSize, initialSize);
        }

        public override void Update(Particle p, float deltaTime)
        {
            float interpScale = Mathf.Lerp(initialSize, finalSize, (p.initialLifetime - p.ttl) / p.initialLifetime);
            p.scale = new Vector2(interpScale, interpScale);
        }
    }

    public class EmitterParticleComponent : ParticleComponent
    {
        public float rate = 60;
        private float _timeSinceEmit = 0f;

        public override void Initialize(Particle p)
        {
            p.velocity = new Vector2(p.random.Next(-5, 5), 0);
            p.scale = new Vector2(0.25f, 0.25f);
            p.initialLifetime = (float)p.random.NextDouble() * 2 + 3;
        }

        public override void UpdateParticleSystem(ParticleSystem ps, float deltaTime)
        {
            _timeSinceEmit += deltaTime;
            float inverseRate = 1f / rate;
            while(_timeSinceEmit >= inverseRate)
            {
                ps.Emit();
                _timeSinceEmit -= inverseRate;
            }
        }
    }

    public enum ParticleRenderMode
    {
        AlphaBlending,
        Normal
    }

    public class ParticleSystem : Renderer
    {
        private Particle[] _particles;
        public uint maxParticles = 1000;
        private uint _numParticles = 0;
        public uint numParticles { get { return _numParticles; } }
        public List<ParticleComponent> components = new List<ParticleComponent>();
        public bool fixedUpdate = false;
        public List<Material> materials = new List<Material>();
        private Random random;
        private int _randomSeed = 0;
        public int randomSeed { set { _useRandomSeed = true; _randomSeed = value; } }
        private bool _useRandomSeed = false;
        public Space space = Space.World;

        public float lifetime = 5.0f;

        public ParticleRenderMode renderMode = ParticleRenderMode.AlphaBlending;

        public T AddParticleComponent<T>() where T : ParticleComponent, new()
        {
            T result = new T();
            components.Add(result);
            return result;
        }

        public uint Emit(int i)
        {
            int toAdd = (int)Mathf.Min(i, maxParticles - _numParticles);
            for (int j=0; j<toAdd; j++)
            {
                Emit();
            }
            return _numParticles;
        }

        public Particle Emit()
        {
            if(_numParticles < maxParticles)
            {
                _particles[_numParticles] = new Particle();

                /* Set up the particle */
                _particles[_numParticles].parent = this;
                _particles[_numParticles].material = materials[random.Next(materials.Count)];
                _particles[_numParticles].random = new Random(random.Next());
                _particles[_numParticles].initialLifetime = lifetime;

                if (space == Space.World)
                    _particles[_numParticles].position = transform.GlobalPosition;
                else
                    _particles[_numParticles].position = Vector2.zero;

                foreach(ParticleComponent pc in components)
                {
                    pc.Initialize(_particles[_numParticles]);
                }

                _particles[_numParticles].ttl = _particles[_numParticles].initialLifetime;
                _numParticles++;
            }
            else
            {
                // here for debugging purposes
            }
            return _particles[_numParticles - 1];
        }

        public void Flush()
        {
            int counter = 0;
            uint removed = 0;
            for(int pointer = 0; pointer<_numParticles; pointer++)
            {
                if(_particles[pointer].ttl <= 0f)
                {
                    _particles[pointer] = null;
                    removed++;
                }
                else
                {
                    if(counter != pointer)
                    {
                        _particles[counter] = _particles[pointer];
                        _particles[pointer] = null;
                    }
                    counter++;
                }
            }
            _numParticles -= removed;
        }

        public void Simulate(float deltaTime)
        {
            for(int i=0; i<_numParticles; i++)
            {
                Particle p = _particles[i];
                p.ttl -= deltaTime;

                foreach (ParticleComponent pc in components)
                {
                    pc.Update(p, deltaTime);
                }

                p.position += p.velocity * deltaTime;
                p.rotation += p.angularVelocity * deltaTime;
            }
            foreach (ParticleComponent pc in components)
            {
                pc.UpdateParticleSystem(this, deltaTime);
            }
            Flush();
        }

        public override void DrawDiffuse(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < _numParticles; i++)
            {
                Particle p = _particles[i];

                if (p.material.DiffuseTexture != null)
                {
                    spriteBatch.Draw(p.material.DiffuseTexture, position: Vector2.Scale(new Vector2(1, -1), p.position + ((space == Space.World) ? Vector2.zero : (Vector2)transform.GlobalPosition)), color: p.color, rotation: p.rotation, scale: p.scale);
                }
                else
                {
                    // TODO: Draw blank diffuse?
                }
            }
        }

        public override void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < _numParticles; i++)
            {
                Particle p = _particles[i];

                if (p.material.EmissiveTexture != null)
                {
                    spriteBatch.Draw(p.material.EmissiveTexture, position: Vector2.Scale(new Vector2(1, -1), p.position + ((space == Space.World) ? Vector2.zero : (Vector2)transform.GlobalPosition)), color: p.color, rotation: p.rotation, scale: p.scale);
                }
                else
                {
                    // TODO: Draw blank emissive
                }
            }
        }

        public override void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < _numParticles; i++)
            {
                Particle p = _particles[i];

                if (p.material.NormalTexture != null)
                {
                    spriteBatch.Draw(p.material.NormalTexture, position: Vector2.Scale(new Vector2(1, -1), p.position + ((space == Space.World) ? Vector2.zero : (Vector2)transform.GlobalPosition)), color: p.color, rotation: p.rotation, scale: p.scale);
                }
                else
                {
                    // TODO: Draw blank normal
                }
            }
        }

        void Start()
        {
            _particles = new Particle[maxParticles];
            if (_useRandomSeed)
                random = new Random(_randomSeed);
            else
                random = new Random();
        }

        void Update()
        {
            if(!fixedUpdate)
                Simulate(Time.deltaTime);
        }

        void FixedUpdate()
        {
            if (fixedUpdate)
                Simulate(Time.actualFixedDeltaTime);
        }
    }
}
