using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    public class GameObject
    {
        private Dictionary<Type, Component> Components;

        public bool isActive = true;

        public GameObject()
        {
            Components = new Dictionary<Type, Component>();

            // All GameObjects have a transform
            AddComponent<Transform>();
        }

        public GameObject AddComponent<T>() where T : Component, new()
        {
            return AddComponent<T>(new T());
        }

        public GameObject AddComponent<T>(T component) where T : Component, new()
        {
            component.GameObject = this;
            Components[typeof(T)] = component;
            return this;
        }

        public GameObject RemoveComponent<T>() where T : Component
        {
            Components.Remove(typeof(T));
            return this;
        }

        public T GetComponent<T>() where T : Component
        {
            if (Components.ContainsKey(typeof(T)))
                return (T)Components[typeof(T)];
            else
                return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<Type, Component> kvp in Components)
            {
                if(kvp.Value.isActive)
                    kvp.Value.Update(gameTime);
            }
        }

        public void DrawDiffuse(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var renderers = Components.Values.OfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if(r.isActive)
                    r.DrawDiffuse(spriteBatch, gameTime);
            }
        }

        public void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var renderers = Components.Values.OfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if (r.isActive)
                    r.DrawNormal(spriteBatch, gameTime);
            }
        }

        public void DrawSpecular(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var renderers = Components.Values.OfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if (r.isActive)
                    r.DrawSpecular(spriteBatch, gameTime);
            }
        }

        public void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var renderers = Components.Values.OfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if (r.isActive)
                    r.DrawEmissive(spriteBatch, gameTime);
            }
        }

        public Transform Transform
        {
            get
            {
                return GetComponent<Transform>();
            }
            set
            {
                AddComponent<Transform>(value);
            }
        }
    }
}
