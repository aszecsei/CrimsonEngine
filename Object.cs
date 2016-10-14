using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    /// <summary>
    /// Base class for all objects CrimsonEngine can reference.
    /// </summary>
    public abstract class Object
    {
        private string _name;

        public string name
        {
            get
            {
                if(this is Component)
                {
                    return (this as Component).GameObject.name;
                }
                return _name;
            }
            set
            {
                if(this is Component)
                {
                    (this as Component).GameObject.name = value;
                }
                else
                {
                    _name = value;
                }
            }
        }

        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Removes a gameobject, component or asset.
        /// </summary>
        /// <remarks>
        /// The object obj will be destroyed now or if a time is specified t seconds from now. If obj is a Component it will remove the component
        /// from the GameObject and destroy it. If obj is a GameObject it will destroy the GameObject, all its components and all transform children
        /// of the GameObject. Actual object destruction is always delayed until after the current Update loop, but will always be done before rendering.
        /// </remarks>
        /// <param name="obj">The object to destroy.</param>
        /// <param name="t">The optional amount of time to delay before destroying the object.</param>
        public static void Destroy(Object obj, float t = 0.0F)
        {
            if (obj is GameObject)
            {
                foreach(Transform mT in (obj as GameObject).transform.GetChildren())
                {
                    Destroy(mT.GameObject, t);
                }
            }
            SceneManager.CurrentScene.DestroyedObjects.Add(Time.time + t, obj);
        }

        /// <summary>
        /// Makes the object target not be destroyed automatically when loading a new scene.
        /// </summary>
        /// <remarks>
        /// When loading a new level all objects in the scene are destroyed, then the objects in the new level are loaded. In order to preserve an object during
        /// level loading call DontDestroyOnLoad on it. If the object is a component or game object then its entire transform hierarchy will not be destroyed either.
        /// </remarks>
        /// <param name="target"></param>
        public static void DontDestroyOnLoad(Object target)
        {
            // TODO: Implement this.
        }

        public static implicit operator bool (Object obj)
        {
            if (obj != null)
                return true;
            return false;
        }
    }
}
