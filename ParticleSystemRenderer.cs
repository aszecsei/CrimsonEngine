using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    /// <summary>
    /// The sorting mode for particle systems.
    /// </summary>
    public enum ParticleSystemSortMode
    {
        /// <summary>
        /// No sorting.
        /// </summary>
        None,
        /// <summary>
        /// Sort the oldest particles to the front.
        /// </summary>
        OldestInFront,
        /// <summary>
        /// Sort the youngest particles to the front.
        /// </summary>
        YoungestInFront
    }

    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemRenderer : Renderer
    {
        public float maxParticleSize;
        public float minParticleSize;
        public Vector2 pivot = Vector2.zero;
        public ParticleSystemSortMode sortMode;

        private ParticleSystem particleSystem;
        private Particle[] particles;

        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            particles = new Particle[particleSystem.maxParticles];
        }

        public override void DrawDiffuse(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int numParticles = particleSystem.GetParticles(particles);
            for(int i=0; i<numParticles; i++)
            {
                Particle p = particles[i];
                Vector2 origin = new Vector2(p.material.DiffuseTexture.Width / 2, p.material.DiffuseTexture.Height / 2);
                spriteBatch.Draw(p.material.DiffuseTexture, p.position + (particleSystem.simulationSpace == Space.Local ? (Vector2)transform.GlobalPosition : Vector2.zero),
                    null, p.GetCurrentColor(), p.rotation, origin, p.GetCurrentSize(), SpriteEffects.None, 0f);
            }
        }

        public override void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int numParticles = particleSystem.GetParticles(particles);
            for (int i = 0; i < numParticles; i++)
            {
                Particle p = particles[i];
                if(p.material.EmissiveTexture != null)
                {
                    Vector2 origin = new Vector2(p.material.EmissiveTexture.Width / 2, p.material.EmissiveTexture.Height / 2);
                    spriteBatch.Draw(p.material.EmissiveTexture, p.position + (particleSystem.simulationSpace == Space.Local ? Vector2.zero : (Vector2)transform.GlobalPosition),
                        null, p.GetCurrentColor(), p.rotation, origin, p.GetCurrentSize(), SpriteEffects.None, 0f);
                }
                else
                {
                    // draw black
                }
            }
        }

        public override void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int numParticles = particleSystem.GetParticles(particles);
            for (int i = 0; i < numParticles; i++)
            {
                Particle p = particles[i];
                if (p.material.NormalTexture != null)
                {
                    Vector2 origin = new Vector2(p.material.NormalTexture.Width / 2, p.material.NormalTexture.Height / 2);
                    spriteBatch.Draw(p.material.NormalTexture, p.position + (particleSystem.simulationSpace == Space.Local ? Vector2.zero : (Vector2)transform.GlobalPosition),
                        null, p.GetCurrentColor(), p.rotation, origin, p.GetCurrentSize(), SpriteEffects.None, 0f);
                }
                else
                {
                    // draw blank normal map
                }
            }
        }
    }
}
