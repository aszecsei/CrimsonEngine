using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace CrimsonEngine.Physics
{
    /// <summary>
    /// Information returned about an object detected by a raycast in 2D physics.
    /// </summary>
    /// <remarks>
    /// A raycast is used to detect objects that lie along the path of a ray and is conceptually like firing a laser
    /// beam into the scene and observing which objects are hit by it. The RaycastHit class is used by Physics2D.Raycast
    /// and other functions to return information about the objects detected by raycasts.
    /// </remarks>
    public class RaycastHit
    {
        /// <summary>
        /// The centroid of the primitive used to perform the cast.
        /// </summary>
        /// <remarks>
        /// When the RaycastHit2D is returned from line or ray casting, the centroid is identical to the return point property
        /// however when using cast methods that use a geometry shape (as opposed to a simple point) such as circle or box casting,
        /// the centroid is the center of the respective shape when it is in contact with the returned point.
        ///
        /// The centroid is useful in determining the position the cast shape should be for it to collider at the contact point.
        /// Note that the point takes into account any rotation specified for the shape when it was cast.
        /// </remarks>
        public Vector2 centroid;

        /// <summary>
        /// The collider hit by the ray.
        /// </summary>
        /// <remarks>
        /// This can be useful if the hit object has more than one collider - this property can be used to determine the specific
        /// collider rather than just the object.
        ///
        /// Note that some functions that return a single RaycastHit2D will leave the collider as NULL which indicates nothing hit.
        /// RaycastHit2D implements an implicit conversion operator converting to bool which checks this property allowing it to be
        /// used as a simple condition check for whether a hit occurred or not.
        /// </remarks>
        public Fixture collider;

        /// <summary>
        /// The distance from the ray origin to the impact point.
        /// </summary>
        public float distance;

        /// <summary>
        /// Fraction of the distance along the ray that the hit occurred.
        /// </summary>
        /// <remarks>
        /// If the ray's direction vector is normalised then this value is simply the distance between the origin and the hit point.
        /// If the direction is not normalised then this distance is expressed as a "fraction" (which could be greater than 1) of the
        /// vector's magnitude.
        /// </remarks>
        public float fraction;

        /// <summary>
        /// The normal vector of the surface hit by the ray.
        /// </summary>
        /// <remarks>
        /// The normal vector of a surface is the vector that points outward perpenidularly at a given point on that surface. This vector
        /// can be useful in raycasting as a way to determine reflections or ricochets from projectiles or to align a character so that it
        /// stands upright on the surface.
        ///
        /// Note that if a hit occurs starting inside a collider then the collision normal will be simply the opposite direction of the line/ray query.
        /// </remarks>
        public Vector2 normal;

        /// <summary>
        /// The point in world space where the ray hit the collider's surface.
        /// </summary>
        /// <remarks>
        /// The exact point of contact can be useful for positioning graphic effects (such as explosion or blood splatters) and for determining
        /// which specific part of an object was hit.
        /// </remarks>
        public Vector2 point;

        /// <summary>
        /// The Rigidbody2D attached to the object that was hit.
        /// </summary>
        public Rigidbody rigidbody;

        /// <summary>
        /// The Transform of the object that was hit.
        /// </summary>
        public Transform transform;

        public static implicit operator bool(RaycastHit rh)
        {
            return rh.collider != null;
        }
    }
}
