using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine.Physics
{
    public class ContactPoint
    {
        /// <summary>
        /// The collider attached to the object receiving the collision message.
        /// </summary>
        public Rigidbody collider;

        /// <summary>
        /// Surface normal at the contact point.
        /// </summary>
        public Vector2 normal;

        /// <summary>
        /// The incoming collider involved in the collision at this contact point.
        /// </summary>
        public Rigidbody otherCollider;

        /// <summary>
        /// The point of contact between the two colliders in world space.
        /// </summary>
        public Vector2 point;
    }
}
