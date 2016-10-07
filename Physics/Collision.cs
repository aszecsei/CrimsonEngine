using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    /// <summary>
    /// Information returned by a collision in 2D physics.
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// The incoming collider involved in the collision.
        /// </summary>
        public Collider collider;

        /// <summary>
        /// The specific points of contact with the incoming collider.
        /// </summary>
        public List<Vector2> contacts;

        /// <summary>
        /// Whether the collision was disabled or not.
        /// </summary>
        public bool enabled;

        /// <summary>
        /// The incoming GameObject involved in the collision.
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// The relative linear velocity of the two colliding objects (Read Only).
        /// </summary>
        public readonly Vector2 relativeVelocity;

        /// <summary>
        /// The incoming rigidbody involved in the collision.
        /// </summary>
        public Rigidbody rigidbody;

        /// <summary>
        /// The Transform of the incoming object involved in the collision.
        /// </summary>
        public Transform transform;
    }
}
