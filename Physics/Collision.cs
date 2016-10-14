using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine.Physics
{
    public class Collision
    {
        /// <summary>
        /// The incoming rigidbody involved in the collision.
        /// </summary>
        public Rigidbody rigidbody;

        /// <summary>
        /// The specific points of contact with the incoming rigidbody.
        /// </summary>
        public ContactPoint[] contacts;

        /// <summary>
        /// Whether the collision was disabled or not.
        /// </summary>
        public bool enabled;

        /// <summary>
        /// The incoming GameObject involved in the collision.
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// The relative linear velocity of the two colliding objects.
        /// </summary>
        public Vector2 relativeVelocity { get; internal set; }

        /// <summary>
        /// The Transform of the incoming object involved in the collision.
        /// </summary>
        public Transform transform;
    }
}
