using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FarseerPhysics.Dynamics;

namespace CrimsonEngine.Physics
{
    public static class Physics2D
    {
        internal const float unitToPixel = 8f;
        internal const float pixelToUnit = 1 / unitToPixel;

        /// <summary>
        /// Acceleration due to gravity.
        /// </summary>
        public static Vector2 Gravity = new Vector2(0, -50f);

        public static bool drawDebugPhysics = true;

        public const PhysicsLayer AllLayers = PhysicsLayer.AllLayers;

        /// <summary>
        /// A rigidbody cannot sleep if its angular velocity is above this tolerance.
        /// </summary>
        public static float angularSleepTolerance = 2f;

        /// <summary>
        /// The scale factor that controls how fast overlaps are resolved.
        /// </summary>
        public static float baumgarteScale = 0.2f;

        /// <summary>
        /// The scale factor that controls how fast TOI overlaps are resolved.
        /// </summary>
        public static float baumgarteTOIScale = 0.75f;

        /// <summary>
        /// Whether or not to stop reporting collision callbacks immediately if any of the objects in the collision are deleted/moved.
        /// </summary>
        public static bool changeStopsCallbacks = false;

        /// <summary>
        /// The color used by the debugger to show all asleep colliders (collider is asleep when the body is asleep).
        /// </summary>
        public static Color colliderAsleepColor = Color.blue;

        /// <summary>
        /// The color used by the debugger to show all awake colliders (collider is awake when the body is awake).
        /// </summary>
        public static Color colliderAwakeColor = Color.green;

        /// <summary>
        /// The color used by the debugger to show all collider contacts.
        /// </summary>
        public static Color colliderContactColor = Color.red;

        /// <summary>
        /// The color used by the debugger to show all colliders without rigidbodies.
        /// </summary>
        public static Color colliderNoRigidbodyColor = Color.white;

        /// <summary>
        /// Layer mask constant that includes all layers participating in raycasts by default.
        /// </summary>
        public const PhysicsLayer DefaultRaycastLayers = PhysicsLayer.AllLayers;

        /// <summary>
        /// Layer mask constant for the default layer that ignores raycasts.
        /// </summary>
        public static PhysicsLayer IgnoreRaycastLayer
        {
            get
            {
                return ~DefaultRaycastLayers;
            }
        }

        /// <summary>
        /// A rigidbody cannot sleep if its linear velocity is above this tolerance.
        /// </summary>
        public static float linearSleepTolerance = 0.01f;

        /// <summary>
        /// The maximum angular position correction used when solving contraints. This helps to prevent overshoot.
        /// </summary>
        public static float maxAngularCorrection = 8f;

        /// <summary>
        /// The maximum linear position correction used when solving constraints. This helps to prevent overshoot.
        /// </summary>
        public static float maxLinearCorrection = 0.2f;

        /// <summary>
        /// The maximum angular speed of a rigidbody per physics update. Increasing this can cause numerical problems.
        /// </summary>
        public static float maxRotationSpeed = 360f;

        /// <summary>
        /// The maximum linear speed of a rigidbody per physics update. Increasing this can cause numerical problems.
        /// </summary>
        public static float maxTranslationSpeed = 100f;

        /// <summary>
        /// The minimum contact penetration radius allowed before any separation impulse force is applied. Extreme caution
        /// should be used when modifying this value as making this smaller means that polygons will have an insufficient
        /// buffer for continuous collision and making it larger may create artefacts for vertex collision.
        /// </summary>
        public static float minPenetrationForPenalty = 0.01f;

        /// <summary>
        /// The number of iterations of the physics solver when considering objects' positions.
        /// A higher number of iterations will improve accuracy at the expense of processing overhead.
        /// </summary>
        public static int positionIterations = 8;

        /// <summary>
        /// Do raycasts detect colliders configured as triggers?
        /// </summary>
        public static bool queriesHitTriggers = true;

        /// <summary>
        /// Do ray/line casts that start inside a collider(s) detect those collider(s)?
        /// </summary>
        public static bool queriesStartInColliders = true;

        /// <summary>
        /// The time in seconds that a rigidbody must be still before it will go to sleep.
        /// </summary>
        public static float timeToSleep = 0.5f;

        /// <summary>
        /// The number of iterations of the physics solver when considering objects' velocities.
        /// A higher number of interations will improve accuracy at the expense of processing overhead.
        /// </summary>
        public static int velocityIterations = 5;

        /// <summary>
        /// Any collisions with a relative linear velocity below this threshold will be treated as inelastic.
        /// </summary>
        public static float velocityThreshold = 1f;

        /// <summary>
        /// Checks whether the collision detection system will ignore all collisions/triggers between collider1
        /// and collider2 or not.
        /// </summary>
        /// <param name="collider1">The first collider to compare to collider2.</param>
        /// <param name="collider2">The second collider to compare to collider1.</param>
        /// <returns></returns>
        public static bool GetIgnoreCollision(Rigidbody collider1, Rigidbody collider2)
        {
            foreach(Tuple<Rigidbody, Rigidbody> t in ignoredCollisions)
            {
                if ((t.Item1 == collider1 || t.Item2 == collider1) && (t.Item1 == collider2 || t.Item2 == collider2))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Should collisions between the specified layers be ignored?
        /// </summary>
        /// <param name="layer1">ID of first layer.</param>
        /// <param name="layer2">ID of second layer.</param>
        /// <returns></returns>
        public static bool GetIgnoreLayerCollision(Category layer1, Category layer2)
        {
            foreach (Tuple<Category, Category> t in ignoredCollisionLayers)
            {
                if ((t.Item1 == layer1 || t.Item2 == layer1) && (t.Item1 == layer2 || t.Item2 == layer2))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Get the collision layer mask that indicates which layer(s) the specified layer can collide with.
        /// </summary>
        /// <param name="layer">The layer to retrieve the collision layer mask for.</param>
        /// <returns>A mask where each bit indicates a layer and whether it can collide with layer or not.</returns>
        public static Category GetLayerCollisionMask(Category layer)
        {
            Category result = Category.All;
            foreach (Tuple<Category, Category> t in ignoredCollisionLayers)
            {
                if (t.Item1 == layer)
                    result &= ~t.Item2;
                if (t.Item2 == layer)
                    result &= ~t.Item1;
            }
            return result;
        }

        /// <summary>
        /// Makes the collision detection system ignore all collisions/triggers between collider1 and collider2.
        /// </summary>
        /// <remarks>
        /// Ignoring collisions refers to any type of interaction between the selected colliders i.e. no collision or
        /// trigger interaction will occur. Collision layers are first checked to see the two layers can interact and if
        /// not then no interactions take place. Following that, ignoring specific colliders interactions will occur.
        ///
        /// IgnoreCollision has a few limitations: 1) It is not persistent.This means that the ignore collision state will
        /// not be stored in the editor when saving a scene. 2) You can only apply the ignore collision to colliders in active
        /// game objects. When deactivating the collider the IgnoreCollision state will be lost and you have to call
        /// Physics2D.IgnoreCollision again.
        /// </remarks>
        /// <param name="collider1">The first collider to compare to collider2.</param>
        /// <param name="collider2">The second collider to compare to collider1.</param>
        /// <param name="ignore">Whether collisions/triggers between collider1 and collider2 should be ignored or not.</param>
        public static void IgnoreCollision(Rigidbody collider1, Rigidbody collider2, bool ignore = true)
        {
            if(ignore)
            {
                collider1.body.IgnoreCollisionWith(collider2.body);
                collider2.body.IgnoreCollisionWith(collider1.body);
                ignoredCollisions.Add(new Tuple<Rigidbody, Rigidbody>(collider1, collider2));
            }
            else
            {
                collider1.body.RestoreCollisionWith(collider2.body);
                collider2.body.RestoreCollisionWith(collider1.body);
                for(int i=0; i<ignoredCollisions.Count; i++)
                {
                    if ((ignoredCollisions[i].Item1 == collider1 || ignoredCollisions[i].Item2 == collider1) && (ignoredCollisions[i].Item1 == collider2 || ignoredCollisions[i].Item2 == collider2))
                        ignoredCollisions.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Choose whether to detect or ignore collisions between a specified pair of layers.
        /// </summary>
        /// <param name="layer1">ID of the first layer.</param>
        /// <param name="layer2">ID of the second layer.</param>
        /// <param name="ignore">Should collisions between these layers be ignored?</param>
        public static void IgnoreLayerCollision(PhysicsLayer layer1, PhysicsLayer layer2, bool ignore = true)
        {
            if(ignore)
            {
                // TODO: Implement this
                // throw new NotImplementedException();
            }
            else
            {
                // TODO: Implement this
                // throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Check whether collider1 is touching collider2 or not.
        /// </summary>
        /// <remarks>
        /// It is important to understand that checking if colliders are touching or not is performed against the last
        /// physics system update i.e. the state of touching colliders at that time. If you have just added a new Rigidboy
        /// or have moved a Rigidboy but a physics update has not yet taken place then the colliders will not be shown as
        /// touching. The touching state is identical to that indicated by the physics collision or trigger callbacks.
        /// </remarks>
        /// <param name="collider1">The collider to check if it is touching collider2.</param>
        /// <param name="collider2">The collider to check if it is touching collider1.</param>
        /// <returns>Whether collider1 is touching collider2 or not.</returns>
        public static bool IsTouching(Rigidbody collider1, Rigidbody collider2)
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the collider is touching any colliders on the specified layerMask or not.
        /// </summary>
        /// <remarks>
        /// It is important to understand that checking if colliders are touching or not is performed against the last physics
        /// system update i.e. the state of touching colliders at that time. If you have just added a new Rigidbody or have moved
        /// a Rigidbody but a physics update has not yet taken place then the colliders will not be shown as touching. The touching
        /// state is identical to that indicated by the physics collision or trigger callbacks.
        /// </remarks>
        /// <param name="collider">The collider to check if it is touching colliders on the layerMask.</param>
        /// <param name="layerMask">Any colliders on any of these layers count as touching.</param>
        /// <returns>Whether the collider is touching any colliders on the specified layerMask or not.</returns>
        public static bool IsTouchingLayers(Rigidbody collider, PhysicsLayer layerMask = AllLayers)
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Casts a line against colliders in the scene.
        /// </summary>
        /// <remarks>
        /// A linecast is an imaginary line between two points in world space. Any object making contact with the beam can be detected and
        /// reported. This differs from the similar raycast in that raycasting specifies the line using an origin and direction.
        ///
        /// This function returns a RaycastHit2D object when the line contacts a Collider in the scene.The layerMask can be used to detect
        /// objects selectively only on certain layers (this allows you to apply the detection only to enemy characters, for example). The
        /// direction of the line is assumed to extend from the start point to the end point.Only the first collider encountered in that direction
        /// will be reported.Although the Z axis is not relevant for rendering or collisions in 2D, you can use the minDepth and maxDepth parameters
        /// to filter objects based on their Z coordinate.
        ///
        /// Linecasts are useful for determining lines of sight, targets hit by gunfire and for many other purposes in gameplay.
        ///
        /// Note that this function will allocate memory for the returned RaycastHit2D object. You can use LinecastNonAlloc to avoid this overhead
        /// if you need to make linecasts frequently.
        ///
        /// Additionally, this will also detect Collider(s) at the start of the line.In this case the line is starting inside the Collider and doesn't
        /// intersect the Collider surface. This means that the collision normal cannot be calculated in which case the collision normal returned is set
        /// to the inverse of the line vector being tested. This can easily be detected because such results are always at a RaycastHit2D fraction of zero.
        /// </remarks>
        /// <param name="start">The start point of the line in world space.</param>
        /// <param name="end">The end point of the line in world space.</param>
        /// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
        /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
        /// <returns>The cast results returned.</returns>
        public static RaycastHit Linecast(Vector2 start, Vector2 end, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Casts a line against colliders in the scene.
        /// </summary>
        /// <remarks>
        /// A linecast is an imaginary line between two points in world space. Any object making contact with the beam can be detected and reported.
        ///
        /// This function is similar to the Linecast function except that all colliders that are in contact with the line are reported. The line is
        /// assumed to run from its start point to its end point; colliders will be placed in the returned array in order of distance from the start of the line.
        ///
        /// Linecasts are useful for determining lines of sight, targets hit by gunfire and for many other purposes in gameplay.
        ///
        /// Note that this function will allocate memory for the returned RaycastHit2D array.You can use LinecastNonAlloc to avoid this overhead if
        /// you need to make linecasts frequently.
        ///
        /// Additionally, this will also detect Collider(s) at the start of the line.In this case the line is starting inside the Collider and doesn't
        /// intersect the Collider surface. This means that the collision normal cannot be calculated in which case the collision normal returned is set
        /// to the inverse of the line vector being tested. This can easily be detected because such results are always at a RaycastHit2D fraction of zero.
        /// </remarks>
        /// <param name="start">The start point of the line in world space.</param>
        /// <param name="end">The end point of the line in world space.</param>
        /// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
        /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
        /// <returns></returns>
        public static RaycastHit[] LinecastAll(Vector2 start, Vector2 end, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Casts a line against colliders in the scene.
        /// </summary>
        /// <remarks>
        /// A linecast is an imaginary line between two points in world space. Any object making contact with the beam can be detected and reported.
        /// This differs from the similar raycast in that the raycast specifies the line using an origin and direction.
        ///
        /// This function is similar to the LinecastAll function except that the results are returned in the supplied array.The integer return value
        /// is the number of objects that intersect the line(possibly zero) but the results array will not be resized if it doesn't contain enough
        /// elements to report all the results. The significance of this is that no memory is allocated for the results and so garbage collection performance
        /// is improved when linecasts are performed frequently. The line is assumed to run from its start point to its end point; colliders will be placed
        /// in the returned array in order of distance from the start of the line.
        ///
        /// Additionally, this will also detect Collider(s) at the start of the line.In this case the line is starting inside the Collider and doesn't intersect
        /// the Collider surface. This means that the collision normal cannot be calculated in which case the collision normal returned is set to the inverse of
        /// the line vector being tested. This can easily be detected because such results are always at a RaycastHit2D fraction of zero.
        /// </remarks>
        /// <param name="start">The start point of the line in world space.</param>
        /// <param name="end">The end point of the line in world space.</param>
        /// <param name="results">Returned array of objects that intersect the line.</param>
        /// <param name="layerMask">Filter to detect Colliders only on certain layers.</param>
        /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
        /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
        /// <returns></returns>
        public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit[] results, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if a collider falls within a rectangular area.
        /// </summary>
        /// <remarks>
        /// The rectangle is defined by two diagonally opposite corner coordinates in world space. You can think of these as top-left and bottom-right but the
        /// test will still work if the ordering of the points is reversed. The optional layerMask allows the test to check only for objects on specific layers.
        ///
        /// Although the Z axis is not relevant for rendering or collisions in 2D, you can use the minDepth and maxDepth parameters to filter objects based on their
        /// Z coordinate.If more than one collider falls within the area then the one returned will be the one with the lowest Z coordinate value. Null is returned
        /// if there are no colliders in the area.
        /// </remarks>
        /// <param name="pointA">One corner of the rectangle.</param>
        /// <param name="pointB">Diagonally opposite corner of the rectangle.</param>
        /// <param name="layerMask">Filter to check objects only on specific layers.</param>
        /// <param name="minDepth">Only include objects with a Z coordinate (depth) greater than or equal to this value.</param>
        /// <param name="maxDepth">Only include objects with a Z coordinate (depth) less than or equal to this value.</param>
        /// <returns></returns>
        public static Rigidbody OverlapArea(Vector2 pointA, Vector2 pointB, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Rigidbody[] results, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody OverlapBox(Vector2 point, Vector2 size, float angle, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Rigidbody[] results, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody OverlapCircle(Vector2 point, float radius, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody[] OverlapCircleAll(Vector2 point, float radius, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody[] OverlapCircleNonAlloc(Vector2 point, float radius, Rigidbody[] results, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody OverlapPoint(Vector2 point, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static Rigidbody[] OverlapPointAll(Vector2 point, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static int OverlapPointNonAlloc(Vector2 point, Rigidbody[] results, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static RaycastHit Raycast(Vector2 origin, Vector2 direction, float distance = float.MaxValue, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            RaycastHit rh = null;
            Func<Fixture, Microsoft.Xna.Framework.Vector2, Microsoft.Xna.Framework.Vector2, float, float> get_first_callback = delegate (Fixture fixture, Microsoft.Xna.Framework.Vector2 point, Microsoft.Xna.Framework.Vector2 normal, float fraction)
            {
                rh = new RaycastHit();
                rh.centroid = origin;
                rh.collider = fixture;
                rh.distance = Vector2.Distance(point * unitToPixel, origin);
                rh.fraction = fraction * unitToPixel;
                rh.normal = normal * unitToPixel;
                rh.point = point * unitToPixel;
                foreach(GameObject go in SceneManager.CurrentScene.ActiveGameObjects)
                {
                    Rigidbody r = go.GetComponent<Rigidbody>();
                    if (r != null)
                    {
                        if(r.body.FixtureList.Contains(fixture))
                        {
                            rh.rigidbody = r;
                        }
                    }
                }
                rh.transform = rh.rigidbody.GameObject.transform;
                return fraction;
            };

            Vector2 point2 = new Vector2(direction.x, direction.y);
            point2.Normalize();
            point2 *= distance;
            point2 += origin;
            world.RayCast(get_first_callback, origin * pixelToUnit, point2 * pixelToUnit);
            return rh;
        }

        public static RaycastHit[] RaycastAll(Vector2 origin, Vector2 direction, float distance = float.MaxValue, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit[] results, float distance = float.MaxValue, PhysicsLayer layerMask = DefaultRaycastLayers, float minDepth = float.MinValue, float maxDepth = float.MaxValue)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        public static void SetLayerCollisionMask(PhysicsLayer layer, PhysicsLayer layerMask)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        private static List<Tuple<Rigidbody, Rigidbody>> ignoredCollisions = new List<Tuple<Rigidbody, Rigidbody>>();

        private static List<Tuple<Category, Category>> ignoredCollisionLayers = new List<Tuple<Category, Category>>();

        internal static FarseerPhysics.Dynamics.World world;

        internal static void Initialize()
        {
            world = new World(Gravity);
            world.ContactManager.PreSolve = new PreSolveDelegate(ContactListener.PreSolve);
            world.ContactManager.PostSolve += new PostSolveDelegate(ContactListener.PostSolve);
            world.ContactManager.BeginContact += new BeginContactDelegate(ContactListener.BeginContact);
            world.ContactManager.EndContact += new EndContactDelegate(ContactListener.EndContact);

            FarseerPhysics.Settings.AllowSleep = true;
            FarseerPhysics.Settings.VelocityIterations = velocityIterations;
            FarseerPhysics.Settings.PositionIterations = positionIterations;
        }

        internal static void Simulate()
        {
            world.Step(Time.actualFixedDeltaTime);
        }
    }
}
