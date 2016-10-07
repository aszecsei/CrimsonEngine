using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    public abstract class Component
    {
        /// <summary>
        /// Whether or not the current component is currently running.
        /// </summary>
        public bool isActive = true;

        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// The GameObject that owns the component instance.
        /// </summary>
        public GameObject GameObject
        {
            get; set;
        }

        /// <summary>
        /// Requires the inclusion of the given component. If the game object doesn't contain this component, it instantiates it.
        /// </summary>
        /// <typeparam name="T">The component to require.</typeparam>
        public void Requires<T>() where T : Component, new()
        {
            Requires<T, T>();
        }

        /// <summary>
        /// Requires the inclusion of the given component. If the game object doesn't contain this component, it instantiates its given subclass.
        /// </summary>
        /// <typeparam name="T">The component to require.</typeparam>
        /// <typeparam name="U">The component subclass to instantiate.</typeparam>
        public void Requires<T, U>() where T : Component where U : T, new()
        {
            if (GameObject.GetComponent<T>() == null)
            {
                GameObject.AddComponent<U>();
            }
        }
    }
}
