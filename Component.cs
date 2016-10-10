using System;
using System.Collections;
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

        /// <summary>
        /// This method is called after the game object has been set and the component's
        /// values have been initialized.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// This method is called every tick to update game logic.
        /// </summary>
        /// <param name="gameTime">The amount of time since the last method call.</param>
        public virtual void Update() { }

        public virtual void FixedUpdate() { }

        public virtual void LateUpdate() { }

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
            if (GameObject.GetComponentOrSubclass<T>() == null)
            {
                GameObject.AddComponent<U>();
            }
        }

        /// <summary>
        /// Starts a given coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to begin.</param>
        public void StartCoroutine(IEnumerator coroutine)
        {
            SceneManager.CurrentScene.StartCoroutine(coroutine);
        }
    }
}
