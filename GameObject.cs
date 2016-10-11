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
        internal void Awake()
        {
            foreach(Component c in Components.Values)
            {
                if(c.isActive)
                {
                    c.Awake();
                }
            }
        }

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

        public GameObject AddComponent<T>(T component) where T : Component
        {
            component.GameObject = this;
            Components[typeof(T)] = component;
            (component as Component).Initialize();
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

        public T GetComponentOrSubclass<T>() where T : Component
        {
            foreach(Type t in Components.Keys)
            {
                if(Helpers.IsSameOrSubclass(typeof(T), t))
                {
                    return (T)Components[t];
                }
            }
            return null;
        }

        public void Update()
        {
            foreach (KeyValuePair<Type, Component> kvp in Components)
            {
                if(kvp.Value.isActive)
                    kvp.Value.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (KeyValuePair<Type, Component> kvp in Components)
            {
                if (kvp.Value.isActive && !(kvp.Value is Physics.Rigidbody))
                    kvp.Value.FixedUpdate();
            }
        }

        public void PhysicsUpdate()
        {
            foreach (KeyValuePair<Type, Component> kvp in Components)
            {
                if (kvp.Value.isActive && (kvp.Value is Physics.Rigidbody))
                    kvp.Value.FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (KeyValuePair<Type, Component> kvp in Components)
            {
                if (kvp.Value.isActive)
                    kvp.Value.LateUpdate();
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

        /// <summary>
        /// Send a message to all components of a game object. Note that this will call functions on deactivated
        /// components, so as to enable them to awaken in response to messages.
        /// </summary>
        /// <param name="name">The name of the function to call.</param>
        /// <param name="parameterArray">The parameters of the function to call.</param>
        public void SendMessage(String name, params object[] parameterArray)
        {
            Type[] types = new Type[parameterArray.Length];
            for(int i = 0; i < parameterArray.Length; i++)
            {
                types[0] = parameterArray[0].GetType();
            }

            foreach(Component c in Components.Values)
            {
                System.Reflection.MethodInfo mI = c.GetType().GetMethod(name, types);
                if (mI != null)
                {
                    mI.Invoke(c, parameterArray);
                }
            }
        }

        /// <summary>
        /// Calls the method named on every component in this game object or any of its descendants.
        /// </summary>
        /// <param name="name">The name of the method to call.</param>
        /// <param name="parameterArray">The parameters to pass to the method.</param>
        public void BroadcastMessage(String name, params object[] parameterArray)
        {
            SendMessage(name, parameterArray);

            foreach(Transform t in Transform.GetChildren())
            {
                t.GameObject.BroadcastMessage(name, parameterArray);
            }
        }

        /// <summary>
        /// Calls the method named on every component in this game object and on every ancestor of the behaviour.
        /// </summary>
        /// <param name="name">The name of the method to call.</param>
        /// <param name="parameterArray">The parameters to pass to the method.</param>
        public void SendMessageUpwards(String name, params object[] parameterArray)
        {
            SendMessage(name, parameterArray);
            if(Transform.Parent != null)
            {
                Transform.Parent.GameObject.SendMessageUpwards(name, parameterArray);
            }
        }

        public void Destroy()
        {
            SceneManager.CurrentScene.DestroyGameObject(this);
        }

        public void Instantiate(GameObject go)
        {
            SceneManager.CurrentScene.InstantiateGameObject(go);
        }

        public GameObject Instantiate()
        {
            return SceneManager.CurrentScene.InstantiateGameObject();
        }

        public Physics.PhysicsLayer layer = Physics.PhysicsLayer.Layer1;
    }
}
