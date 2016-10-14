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
    public abstract class Component : Object
    {
        /// <summary>
        /// Enabled Behaviours are Updated, disabled Behaviours are not.
        /// </summary>
        public bool enabled = true;

        /// <summary>
        /// Has the Behaviour had enabled called.
        /// </summary>
        public bool isActiveAndEnabled
        {
            get
            {
                return enabled;
            }
        }

        /// <summary>
        /// The game object this component is attached to. A component is always attached to a game object.
        /// </summary>
        public GameObject GameObject
        {
            get; set;
        }

        /// <summary>
        /// The tag of this game object.
        /// </summary>
        public string tag;

        public Transform transform
        {
            get
            {
                return GameObject.transform;
            }
            set
            {
                GameObject.transform = value;
            }
        }

        /// <summary>
        /// Calls the method named methodName on every MonoBehaviour in this game object or any of its children.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameter"></param>
        /// <param name="options"></param>
        public void BroadcastMessage(string methodName, params object[] parameters)
        {
            GameObject.BroadcastMessage(methodName, parameters);
        }

        /// <summary>
        /// Is this game object tagged with tag?
        /// </summary>
        /// <param name="tag">The tag to compare.</param>
        /// <returns></returns>
        public bool CompareTag(string tag)
        {
            return tag.Equals(tag);
        }

        public T GetComponent<T>() where T : Component
        {
            return GameObject.GetComponent<T>();
        }
        
        /// <summary>
        /// Requires the inclusion of the given component. If the game object doesn't contain this component, it instantiates it.
        /// </summary>
        /// <typeparam name="T">The component to require.</typeparam>
        public void Requires<T>() where T : Component, new()
        {
            if (GameObject.GetComponent<T>() == null)
            {
                GameObject.AddComponent<T>();
            }
        }

        /// <summary>
        /// Calls the method named methodName on every MonoBehaviour in this game object.
        /// </summary>
        /// <param name="methodName">Name of method to call.</param>
        /// <param name="parameters">Optional parameter value(s) for the method.</param>
        public void SendMessage(string methodName, params object[] parameters)
        {
            GameObject.SendMessage(methodName, parameters);
        }

        /// <summary>
        /// Calls the method named methodName on every MonoBehaviour in this game object and on every ancestor of the behaviour.
        /// </summary>
        /// <param name="methodName">Name of method to call.</param>
        /// <param name="parameters">Optional parameter value(s) for the method.</param>
        public void SendMessageUpwards(string methodName, params object[] parameters)
        {
            GameObject.SendMessageUpwards(methodName, parameters);
        }

        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Starts a given coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to begin.</param>
        public void StartCoroutine(IEnumerator coroutine)
        {
            SceneManager.CurrentScene.StartCoroutine(coroutine);
        }

        public void Invoke(string methodName, params object[] parameters)
        {
            System.Reflection.BindingFlags bf = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            System.Reflection.MethodInfo mI = GetType().GetMethod(methodName, bf);
            if (mI != null)
            {
                mI.Invoke(this, null);
            }
        }
    }
}
