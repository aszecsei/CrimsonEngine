using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq.Expressions;

namespace CrimsonEngine
{
    public class GameObject : Object
    {
        #region Variables
        private Dictionary<Type, Component> Components;

        public bool activeInHierarchy
        {
            get
            {
                if (transform.Parent == null)
                    return activeSelf;
                else
                    return activeSelf && transform.Parent.GameObject.activeInHierarchy;
            }
        }

        private bool _activeSelf = true;

        /// <summary>
        /// The local active state of this GameObject. (Read Only)
        /// </summary>
        /// <remarks>
        /// This returns the local active state of this GameObject, which is set using GameObject.SetActive. Note that a GameObject
        /// may be inactive because a parent is not active, even if this returns true. This state will then be used once all parents
        /// are active. Use GameObject.activeInHierarchy if you want to check if the GameObject is actually treated as active in the scene.
        /// </remarks>
        public bool activeSelf
        {
            get { return _activeSelf; }
        }

        public Physics.PhysicsLayer layer = Physics.PhysicsLayer.Layer1;

        /// <summary>
        /// The tag of this game object.
        /// </summary>
        private string _tag = null;
        public string tag
        {
            get { return _tag; }
            set
            {
                if(_tag != null)
                {
                    List<GameObject> prevTagList = SceneManager.CurrentScene.taggedObjects[_tag];
                    prevTagList.Remove(this);
                    SceneManager.CurrentScene.taggedObjects[_tag] = prevTagList;
                }
                List<GameObject> newTagList;
                if (SceneManager.CurrentScene.taggedObjects.ContainsKey(value))
                    newTagList = SceneManager.CurrentScene.taggedObjects[value];
                else
                    newTagList = new List<GameObject>();
                newTagList.Add(this);
                SceneManager.CurrentScene.taggedObjects[value] = newTagList;
                _tag = value;
            }
        }

        public Transform transform
        {
            get
            {
                return GetComponent<Transform>();
            }
            set
            {
                Components[typeof(Transform)] = value;
            }
        }
        #endregion Variables

        #region Constructors
        public GameObject()
        {
            Components = new Dictionary<Type, Component>();
            this.name = "GameObject";
            // All GameObjects have a transform
            AddComponent<Transform>();
            SceneManager.CurrentScene.InstantiatedGameObjects.Add(this);
        }

        public GameObject(string name)
        {
            Components = new Dictionary<Type, Component>();
            this.name = name;

            AddComponent<Transform>();
            SceneManager.CurrentScene.InstantiatedGameObjects.Add(this);
        }
        #endregion

        #region Public Functions
        public T AddComponent<T>() where T : Component, new()
        {
            return (T)AddComponent(typeof(T));
        }

        public T AddComponent<T>(T component) where T : Component
        {
            component.GameObject = this;
            Components[typeof(T)] = component;
            foreach (object attributes in component.GetType().GetCustomAttributes(true))
            {
                if (attributes is RequireComponent)
                {
                    Type reqType = (attributes as RequireComponent).requiredType;
                    if (!Components.ContainsKey(reqType))
                        AddComponent(reqType);
                }
            }
            component.Invoke("Awake");
            return component;
        }

        public Component AddComponent(Type componentType)
        {
            Component c = (Component)Activator.CreateInstance(componentType);
            c.GameObject = this;
            Components[componentType] = c;
            foreach (object attributes in c.GetType().GetCustomAttributes(true))
            {
                if (attributes is RequireComponent)
                {
                    Type reqType = (attributes as RequireComponent).requiredType;
                    if(!Components.ContainsKey(reqType))
                        AddComponent(reqType);
                }
            }
            c.Invoke("Awake");
            return c;
        }

        /// <summary>
        /// Calls the method named on every component in this game object or any of its descendants.
        /// </summary>
        /// <param name="name">The name of the method to call.</param>
        /// <param name="parameterArray">The parameters to pass to the method.</param>
        public void BroadcastMessage(String name, params object[] parameterArray)
        {
            SendMessage(name, parameterArray);

            foreach (Transform t in transform.GetChildren())
            {
                t.GameObject.BroadcastMessage(name, parameterArray);
            }
        }

        public bool CompareTag(string tag)
        {
            return tag.Equals(tag);
        }

        public T GetComponent<T>() where T : Component
        {
            if (Components.ContainsKey(typeof(T)))
                return (T)Components[typeof(T)];
            else
                return null;
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
            for (int i = 0; i < parameterArray.Length; i++)
            {
                types[0] = parameterArray[0].GetType();
            }

            foreach (Component c in Components.Values)
            {
                c.Invoke(name, parameterArray);
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
            if (transform.Parent != null)
            {
                transform.Parent.GameObject.SendMessageUpwards(name, parameterArray);
            }
        }

        /// <summary>
        /// Activates/Deactivates the GameObject.
        /// </summary>
        /// <remarks>
        /// Note that a GameObject may be inactive because a parent is not active. In that case, calling SetActive() will not activate it,
        /// but only set the local state of the GameObject, which can be checked using GameObject.activeSelf. This state will then be used
        /// once all parents are active.
        /// 
        /// Making a GameObject inactive will disable every component, turning off any attached renderers, colliders, rigidbodies, scripts,
        /// etc... Any scripts that you have attached to the GameObject will no longer have Update() called, for example.
        /// </remarks>
        /// <param name="value">Activate or deactivate the object.</param>
        public void SetActive(bool value)
        {
            _activeSelf = value;
        }
        #endregion

        #region Internal Functions
        internal bool hasRenderer
        {
            get
            {
                var renderers = Components.Values.OfType<Renderer>();
                return renderers.Count() > 0;
            }
        }

        internal void DrawDiffuse(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var renderers = Components.Values.OfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if(r.enabled)
                    r.DrawDiffuse(spriteBatch, gameTime);
            }
        }

        internal void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var renderers = Components.Values.OfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if (r.enabled)
                    r.DrawNormal(spriteBatch, gameTime);
            }
        }

        internal void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var renderers = Components.Values.OfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if (r.enabled)
                    r.DrawEmissive(spriteBatch, gameTime);
            }
        }

        internal void RemoveComponent(Type t)
        {
            Components.Remove(t);
        }
        #endregion

        #region Static Functions
        /// <summary>
        /// Finds a GameObject by name and returns it.
        /// </summary>
        /// <remarks>
        /// This function only returns active GameObjects. If no GameObject with name can be found, null is returned.
        /// If name contains a '/' character, it traverses the hierarchy like a path name.
        ///
        /// For performance reasons, it is recommended to not use this function every frame.Instead, cache the result
        /// in a member variable at startup, or use GameObject.FindWithTag.
        ///
        /// Note: If you wish to find a child GameObject, it is often easier to use Transform.FindChild.
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject Find(string name)
        {
            string[] names = name.Split('/');
            foreach(GameObject go in SceneManager.CurrentScene.ActiveGameObjects)
            {
                if (go.name == names[0])
                {
                    GameObject g = FindIn(names, 1, go);
                    if (g != null)
                        return g;
                }
            }

            foreach(GameObject go in SceneManager.CurrentScene.InstantiatedGameObjects)
            {
                if (go.name == names[0])
                {
                    GameObject g = FindIn(names, 1, go);
                    if (g != null)
                        return g;
                }
            }

            return null;
        }

        private static GameObject FindIn(string[] names, int count, GameObject g)
        {
            foreach(Transform t in g.transform.GetChildren())
            {
                if(t.GameObject.name == names[count])
                {
                    if(count == names.Length - 1)
                    {
                        return t.GameObject;
                    }
                    else
                    {
                        GameObject gg = FindIn(names, count + 1, t.GameObject);
                        if (gg != null)
                            return gg;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a list of active GameObjects tagged tag. Returns empty array if no GameObject was found.
        /// </summary>
        /// <param name="tag">The name of the tag to search GameObjects for.</param>
        /// <returns></returns>
        public static GameObject[] FindGameObjectsWithTag(string tag)
        {
            List<GameObject> taggedObjs;
            SceneManager.CurrentScene.taggedObjects.TryGetValue(tag, out taggedObjs);
            if (taggedObjs != null)
                return taggedObjs.ToArray();

            return new GameObject[0];
        }

        /// <summary>
        /// Returns one active GameObject tagged tag. Returns null if no GameObject was found.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns></returns>
        public static GameObject FindWithTag(string tag)
        {
            List<GameObject> taggedObjs;
            SceneManager.CurrentScene.taggedObjects.TryGetValue(tag, out taggedObjs);
            if (taggedObjs != null && taggedObjs.Count > 0)
                return taggedObjs[0];

            return null;
        }
        #endregion
    }
}
