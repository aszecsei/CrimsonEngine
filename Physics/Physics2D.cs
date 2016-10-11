using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    public static class Physics2D
    {
        internal const float unitToPixel = 32f;
        internal const float pixelToUnit = 1 / unitToPixel;

        /// <summary>
        /// Acceleration due to gravity.
        /// </summary>
        public static Vector2 Gravity = new Vector2(0, -50f);

        public static bool drawDebugPhysics = true;

        public const uint AllLayers = 0xffffffff;

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
        public static Color colliderAsleepColor = Color.Blue;

        /// <summary>
        /// The color used by the debugger to show all awake colliders (collider is awake when the body is awake).
        /// </summary>
        public static Color colliderAwakeColor = Color.Green;

        /// <summary>
        /// The color used by the debugger to show all collider contacts.
        /// </summary>
        public static Color colliderContactColor = Color.Red;

        /// <summary>
        /// The color used by the debugger to show all colliders without rigidbodies.
        /// </summary>
        public static Color colliderNoRigidbodyColor = Color.White;

        /// <summary>
        /// Layer mask constant that includes all layers participating in raycasts by default.
        /// </summary>
        public static uint DefaultRaycastLayers;

        /// <summary>
        /// Layer mask constant for the default layer that ignores raycasts.
        /// </summary>
        public static uint IgnoreRaycastLayer
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
        public static int velocityIterations = 3;

        /// <summary>
        /// Any collisions with a relative linear velocity below this threshold will be treated as inelastic.
        /// </summary>
        public static float velocityThreshold = 1f;

        internal static FarseerPhysics.Dynamics.World world;

        internal static void Initialize()
        {
            world = new FarseerPhysics.Dynamics.World(Gravity);

            FarseerPhysics.Settings.AllowSleep = true;
            FarseerPhysics.Settings.VelocityIterations = velocityIterations;
            FarseerPhysics.Settings.PositionIterations = positionIterations;
        }

        internal static void Simulate()
        {
            world.Step(Time.actualFixedDeltaTime);
            foreach(GameObject go in SceneManager.CurrentScene.GameObjects)
            {
                Rigidbody r = go.GetComponent<Rigidbody>();
                if(r != null)
                {
                    Vector3 pos = go.Transform.GlobalPosition;
                    pos.X = r.position.X;
                    pos.Y = r.position.Y;
                    go.Transform.GlobalPosition = pos;
                }
            }
        }
    }
}
