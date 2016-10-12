using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace CrimsonEngine.Physics
{
    public class Rigidbody : Component
    {
        internal Body body;

        private FarseerPhysics.Collision.AABB AABB
        {
            get
            {
                FarseerPhysics.Collision.AABB result = new FarseerPhysics.Collision.AABB();
                result.LowerBound = new Vector2(float.MaxValue, float.MaxValue);
                result.UpperBound = new Vector2(float.MinValue, float.MinValue);
                List<Fixture> fixList = body.FixtureList;
                foreach(Fixture f in fixList)
                {
                    FarseerPhysics.Collision.AABB fixAABB;
                    int childCount = f.Shape.ChildCount;
                    for(int child = 0; child < childCount; ++child)
                    {
                        f.GetAABB(out fixAABB, child);
                        result.Combine(ref result, ref fixAABB);
                    }
                }

                return result;
            }
        }

        public Bounds bounds
        {
            get
            {
                return new Bounds(AABB.Center * Physics2D.unitToPixel, new Vector2(AABB.Width * Physics2D.unitToPixel, AABB.Height * Physics2D.unitToPixel));
            }
        }

        public Vector2 size
        {
            get
            {
                return new Vector2(AABB.Width * Physics2D.unitToPixel, AABB.Height * Physics2D.unitToPixel);
            }
        }

        public float restitution
        {
            get
            {
                return body.Restitution;
            }
            set
            {
                body.Restitution = value;
            }
        }

        public float friction
        {
            get
            {
                return body.Friction;
            }
            set
            {
                body.Friction = value;
            }
        }

        /// <summary>
        /// Coefficient of angular drag.
        /// </summary>
        public float angularDrag
        {
            get
            {
                return body.AngularDamping;
            }
            set
            {
                body.AngularDamping = value;
            }
        }

        /// <summary>
        /// Angular velocity in degrees per second.
        /// </summary>
        public float angularVelocity
        {
            get
            {
                return body.AngularVelocity;
            }
            set
            {
                body.AngularVelocity = value;
            }
        }

        /// <summary>
        /// The center of mass of the rigidbody in local space.
        /// </summary>
        public Vector2 centerOfMass
        {
            get
            {
                return body.LocalCenter;
            }
            set
            {
                body.LocalCenter = value;
            }
        }

        private CollisionDetectionMode _collisionDetectionMode = CollisionDetectionMode.Discrete;
        /// <summary>
        /// The method used by the physics engine to check if two objects have collided.
        /// </summary>
        public CollisionDetectionMode collisionDetectionMode
        {
            get
            {
                return _collisionDetectionMode;
            }
            set
            {
                _collisionDetectionMode = value;
                if(_collisionDetectionMode == CollisionDetectionMode.Discrete)
                {
                    body.IsBullet = false;
                }
                else
                {
                    body.IsBullet = true;
                }
            }
        }

        /// <summary>
        /// Coefficient of drag.
        /// </summary>
        public float drag
        {
            get
            {
                return body.LinearDamping;
            }
            set
            {
                body.LinearDamping = value;
            }
        }

        /// <summary>
        /// Controls whether physics will change the rotation of the object.
        /// </summary>
        public bool freezeRotation
        {
            get
            {
                return body.FixedRotation;
            }
            set
            {
                body.FixedRotation = value;
            }
        }

        /// <summary>
        /// The degree to which this object is affected by gravity.
        /// </summary>
        public float gravityScale
        {
            get
            {
                return body.GravityScale;
            }
            set
            {
                body.GravityScale = value;
            }
        }

        /// <summary>
        /// The rigidbody rotational inertia.
        /// </summary>
        public float inertia
        {
            get
            {
                return body.Inertia;
            }
            set
            {
                body.Inertia = value;
            }
        }

        /// <summary>
        /// Should this rigidbody be taken out of physics control?
        /// </summary>
        /// <remarks>
        /// If this property is set to true then the rigidbody will stop reacting to collisions and applied forces. This can be useful
        /// when an object should usually be controlled "kinematically" (ie, non-physically) but then sometimes needs physics
        /// for realism. For example, a human character is usually not implemented using physics but may sometimes be
        /// thrown through the air and collide with objects as the result of an impact or explosion.
        /// </remarks>
        public bool isKinematic
        {
            get { return body.IsKinematic; }
            set { body.IsKinematic = value; }
        }

        public bool isStatic
        {
            get { return body.IsStatic; }
            set { body.IsStatic = value; }
        }

        /// <summary>
        /// Mass of the rigidbody.
        /// </summary>
        /// <remarks>
        /// The mass is given in arbitrary units but the basic physical principles of mass apply. From Newton's classic equation
        /// force = mass x accelelation, it is apparent that the larger an object's mass, the more force it requires to accelerate it
        /// to a given velocity. Also, mass affects momentum, which is significant during collisions; an object with large mass
        /// will be moved less by a collision than an object with lower mass.
        /// </remarks>
        public float mass
        {
            get { return body.Mass;  }
            set { body.Mass = value; }
        }

        /// <summary>
        /// The position of the rigidbody.
        /// </summary>
        public Vector2 position
        {
            get
            {
                return body.Position * Physics2D.unitToPixel;
            }
            set
            {
                body.Position = value * Physics2D.pixelToUnit;
                Vector3 p = GameObject.Transform.GlobalPosition;
                p.x = value.y;
                p.y = value.y;
                GameObject.Transform.GlobalPosition = p;
            }
        }

        /// <summary>
        /// The rotation of the rigidbody.
        /// </summary>
        public float rotation
        {
            get
            {
                return body.Rotation;
            }
            set
            {
                body.Rotation = value;
                GameObject.Transform.GlobalRotation = value;
            }
        }

        /// <summary>
        /// Indicates whether the rigidbody should be simulated or not by the physics system.
        /// </summary>
        public bool simulated
        {
            get { return body.Enabled; }
            set { body.Enabled = value; }
        }

        /// <summary>
        /// The sleep state that the rigidbody will initially be in.
        /// </summary>
        public RigidbodySleepMode sleepMode = RigidbodySleepMode.StartAwake;

        private bool _useAutoMass = false;
        /// <summary>
        /// Should the total rigidbody mass be automatically calculated from the density of attached colliders?
        /// </summary>
        public bool useAutoMass
        {
            get
            {
                return _useAutoMass;
            }
            set
            {
                _useAutoMass = value;
                body.ResetMassData();
            }
        }

        /// <summary>
        /// Linear velocity of the rigidbody.
        /// </summary>
        public Vector2 velocity
        {
            get { return body.LinearVelocity; }
            set { body.LinearVelocity = value; }
        }

        /// <summary>
        /// Gets the center of mass of the rigidbody in global space.
        /// </summary>
        public Vector2 worldCenterOfMass
        {
            get { return body.WorldCenter; }
        }

        /// <summary>
        /// Apply a force to the rigidbody.
        /// </summary>
        /// <param name="force">Components of the force in the X and Y axes.</param>
        /// <param name="mode">The method used to apply the specified force.</param>
        public void AddForce(Vector2 force, ForceMode mode = ForceMode.Force)
        {
            if(mode == ForceMode.Force)
            {
                body.ApplyForce(force);
            }
            else
            {
                body.ApplyLinearImpulse(force);
            }
        }

        /// <summary>
        /// Apply a force at a given position in space.
        /// </summary>
        /// <remarks>
        /// The AddForce function applies a force that acts straight through the rigidbody's centre of mass and so produces
        /// only positional movement and no rotation. AddForceAtPosition can apply the force at any position in world space
        /// and will typically also apply a torque to the object which will set it rotating. Note that for the purposes of this
        /// function, the rigidbody is just a coordinate space of infinite size, so there is no reason why the force needs to be
        /// applied within the confines of the object's graphic or colliders.
        /// </remarks>
        /// <param name="force">Components of the force in the X and Y axes.</param>
        /// <param name="position">Position in world space to apply the force.</param>
        /// <param name="mode">The method used to apply the specified force.</param>
        public void AddForceAtPosition(Vector2 force, Vector2 position, ForceMode mode = ForceMode.Force)
        {
            if(mode == ForceMode.Force)
            {
                body.ApplyForce(force, position);
            }
            else
            {
                body.ApplyLinearImpulse(force, position);
            }
        }

        /// <summary>
        /// Adds a force to the rigidbody relative to its coordinate system.
        /// </summary>
        /// <remarks>
        /// The force is specified as two separate components in the X and Y directions. The object will be accelerated by
        /// the force according to the law force = mass x acceleration - the larger the mass, the greater the force required
        /// to accelerate to a given speed.
        /// </remarks>
        /// <param name="relativeForce">Components of the force in the X and Y axes.</param>
        /// <param name="mode">The method used to apply the specified force.</param>
        public void AddRelativeForce(Vector2 relativeForce, ForceMode mode = ForceMode.Force)
        {
            // TODO: Implement this
        }

        /// <summary>
        /// Apply a torque at the rigidbody's centre of mass.
        /// </summary>
        /// <remarks>
        /// A torque is conceptually a force being applied at the end of an imaginary lever, with the fulcrum at the centre of
        /// mass.A torque of five units could thus be equivalent to a force of five units pushing on the end of a lever one unit
        /// long, or a force of one unit on a lever five units long. Our units are arbitrary but the principle that
        /// torque = force x lever length still applies.
        /// </remarks>
        /// <param name="torque">Torque to apply.</param>
        /// <param name="mode">The force mode to use.</param>
        public void AddTorque(float torque, ForceMode mode = ForceMode.Force)
        {
            if(mode == ForceMode.Force)
            {
                body.ApplyTorque(torque);
            }
            else
            {
                body.ApplyAngularImpulse(torque);
            }
        }

        /// <summary>
        /// Get a local space point given the point in rigidbody global space.
        /// </summary>
        /// <param name="point">The global space point to transform into local space.</param>
        /// <returns>The local space point of the point in rigidbody global space.</returns>
        public Vector2 GetPoint(Vector2 point)
        {
            return body.GetLocalPoint(point);
        }

        /// <summary>
        /// The velocity of the rigidbody at the point in global space.
        /// </summary>
        /// <remarks>
        /// GetPointVelocity will take the angularVelocity of the rigidbody into account when calculating the velocity.
        /// </remarks>
        /// <param name="point">The global space point to calculate velocity for.</param>
        /// <returns>The velocity of the rigidbody at the point in global space.</returns>
        public Vector2 GetPointVelocity(Vector2 point)
        {
            return body.GetLinearVelocityFromWorldPoint(point);
        }

        /// <summary>
        /// Get a global space point given the point in rigidbody local space.
        /// </summary>
        /// <param name="relativePoint">The local space point to transform into global space.</param>
        /// <returns>The global space point of the point in rigidbody local space.</returns>
        public Vector2 GetRelativePoint(Vector2 relativePoint)
        {
            return body.GetWorldPoint(relativePoint);
        }

        /// <summary>
        /// The velocity of the rigidbody at the point in local space.
        /// </summary>
        /// <remarks>
        /// GetRelativePointVelocity will take the angularVelocity of the rigidbody into account when calculating the velocity.
        /// </remarks>
        /// <param name="relativePoint">The local space point to calculate velocity for.</param>
        /// <returns>The velocity of the rigidbody at the point in local space.</returns>
        public Vector2 GetRelativePointVelocity(Vector2 relativePoint)
        {
            return body.GetLinearVelocityFromLocalPoint(relativePoint);
        }

        /// <summary>
        /// Get a global space vector given the vector in rigidBody local space.
        /// </summary>
        /// <param name="relativeVector">The local space vector to transform into a global space vector.</param>
        /// <returns>The global space vector of the vector in rigidBody local space.</returns>
        public Vector2 GetRelativeVector(Vector2 relativeVector)
        {
            return body.GetWorldVector(relativeVector);
        }

        /// <summary>
        /// Get a local space vector given the vector vector in rigidBody global space.
        /// </summary>
        /// <param name="vector">	The global space vector to transform into a local space vector.</param>
        /// <returns>The local space vector of the vector vector in rigidBody global space.</returns>
        public Vector2 GetVector(Vector2 vector)
        {
            return body.GetLocalVector(vector);
        }

        /// <summary>
        /// Is the rigidbody "awake"?
        /// </summary>
        /// <remarks>
        /// Sleeping is an optimisation that is used to temporarily remove an object from physics simulation when it
        /// is at rest. This function tells if the rigidbody is currently awake.
        /// </remarks>
        /// <returns>Whether or not the rigidbody is "awake".</returns>
        public bool IsAwake()
        {
            return body.Awake;
        }

        /// <summary>
        /// Is the rigidbody "sleeping"?
        /// </summary>
        /// <remarks>
        /// Sleeping is an optimisation that is used to temporarily remove an object from physics simulation when it is at rest.
        /// This function tells if the rigidbody is currently sleeping.
        /// </remarks>
        /// <returns>Whether or not the rigidbody is "sleeping".</returns>
        public bool IsSleeping()
        {
            return !body.Awake;
        }

        /// <summary>
        /// Check whether any of the collider(s) attached to this rigidbody are touching the collider or not.
        /// </summary>
        /// <remarks>
        /// It is important to understand that checking if colliders are touching or not is performed against the last physics system update
        /// i.e.the state of touching colliders at that time. If you have just added a new Collider or have moved a Collider but a physics
        /// update has not yet taken place then the colliders will not be shown as touching.The touching state is identical to that indicated
        /// by the physics collision or trigger callbacks.
        /// </remarks>
        /// <param name="collider">The collider to check if it is touching any of the collider(s) attached to this rigidbody.</param>
        /// <returns>Whether the collider is touching any of the collider(s) attached to this rigidbody or not.</returns>
        public bool IsTouching(Rigidbody collider)
        {
            foreach(FarseerPhysics.Dynamics.Contacts.Contact c in Physics2D.world.ContactList)
            {
                if(body.FixtureList.Contains(c.FixtureA) || body.FixtureList.Contains(c.FixtureB))
                {
                    if (collider.body.FixtureList.Contains(c.FixtureA) || collider.body.FixtureList.Contains(c.FixtureB))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether any of the collider(s) attached to this rigidbody are touching any colliders on the specified layerMask or not.
        /// </summary>
        /// <remarks>
        /// It is important to understand that checking if colliders are touching or not is performed against the last physics system update
        /// i.e.the state of touching colliders at that time. If you have just added a new Collider or have moved a Collider but a physics
        /// update has not yet taken place then the colliders will not be shown as touching.The touching state is identical to that indicated
        /// by the physics collision or trigger callbacks.
        /// </remarks>
        /// <param name="layerMask">Any colliders on any of these layers count as touching.</param>
        /// <returns>Whether any of the collider(s) attached to this rigidbody are touching any colliders on the specified layerMask or not.</returns>
        public bool IsTouchingLayers(PhysicsLayer layerMask = Physics2D.AllLayers)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Moves the rigidbody to position.
        ///</summary>
        /// <remarks>
        /// Moves the rigidbody to the specified position by calculating the appropriate linear velocity required to move the rigidbody to that
        /// position during the next physics update.During the move, neither gravity or linear drag will affect the body.This causes the object
        /// to rapidly move from the existing position, through the world, to the specified position.
        ///
        /// Because this feature allows a rigidbody to be moved rapidly to the specified position through the world, any colliders attached to
        /// the rigidbody will react as expected i.e. they will produce collisions and/or triggers. This also means that if the colliders produce
        /// a collision then it will affect the rigidbody movement and potentially stop it from reaching the specified position during the next
        /// physics update. If the rigidbody is kinematic then any collisions won't affect the rigidbody itself and will only affect any other
        /// dynamic colliders.
        ///
        /// 2D rigidbodies have a fixed limit on how fast they can move therefore attempting to move large distances over short time - scales can
        /// result in the rigidbody not reaching the specified position during the next physics update. It is recommended that you use this for
        /// relatively small distance movements only.
        ///
        /// It is important to understand that the actual position change will only occur during the next physics update therefore calling this
        /// method repeatedly without waiting for the next physics update will result in the last call being used. For this reason, it is recommended
        /// that it is called during the FixedUpdate callback.
        /// </remarks>
        /// <param name="position">The new position for the Rigidbody object.</param>
        public void MovePosition(Vector2 position)
        {
            Vector2 linearVelocity = position - position;
            this.AddForce(linearVelocity, ForceMode.Impulse);
        }

        /// <summary>
        /// Rotates the rigidbody to angle (given in degrees).
        /// </summary>
        /// <remarks>
        /// Rotates the rigidbody to the specified angle by calculating the appropriate angular velocity required to rotate the rigidbody to that
        /// angle during the next physics update.During the move, angular drag won't affect the body. This causes the object to rapidly move from
        /// the existing angle to the specified angle.
        /// 
        /// Because this feature allows a rigidbody to be rotated rapidly to the specified angle, any colliders attached to the rigidbody will react
        /// as expected i.e. they will produce collisions and/or triggers. This also means that if the colliders produce a collision then it will affect
        /// the rigidbody movement and potentially stop it from reaching the specified angle during the next physics update. If the rigidbody is kinematic
        /// then any collisions won't affect the rigidbody itself and will only affect any other dynamic colliders.
        ///
        /// 2D rigidbodies have a fixed limit on how fast they can rotate therefore attempting to rotate large angles over short time - scales can result
        /// in the rigidbody not reaching the specified angle during the next physics update. It is recommended that you use this for relatively small
        /// rotational movements only.
        ///
        /// It is important to understand that the actual rotation change will only occur during the next physics update therefore calling this method
        /// repeatedly without waiting for the next physics update will result in the last call being used. For this reason, it is recommended that it is
        /// called during the FixedUpdate callback.
        /// </remarks>
        /// <param name="angle">The new rotation angle for the Rigidbody object.</param>
        public void MoveRotation(float angle)
        {
            float rot = (((float)Math.PI * angle) / 180.0f) - rotation;
            if (rot > 180.0f)
                rot -= 180.0f;
            if (rot < 0.0f)
                rot += 180f;

            this.AddTorque(rot, ForceMode.Impulse);
        }

        /// <summary>
        /// Check if any of the Rigidbody colliders overlap a point in space.
        /// </summary>
        /// <param name="point">A point in world space.</param>
        /// <returns>Whether the point overlapped any of the Rigidbody colliders.</returns>
        public bool OverlapPoint(Vector2 point)
        {
            Microsoft.Xna.Framework.Vector2 p = point;
            foreach(Fixture f in body.FixtureList)
            {
                if (f.TestPoint(ref p))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Make the rigidbody "sleep".
        /// </summary>
        /// <remarks>
        /// Sleeping is an optimisation that is used to temporarily remove an object from physics simulation when it is at rest. This function
        /// makes the rigidbody sleep - it is sometimes desirable to enable this manually rather than allowing automatic sleeping with the sleepMode property.
        /// </remarks>
        public void Sleep()
        {
            body.Awake = false;
        }

        /// <summary>
        /// Disables the "sleeping" state of a rigidbody.
        /// </summary>
        /// <remarks>
        /// Sleeping is an optimisation that is used to temporarily remove an object from physics simulation when it is at rest. This function wakes up a
        /// rigidbody that is currently sleeping.
        /// </remarks>
        public void WakeUp()
        {
            body.Awake = true;
        }

        public void AddBoxCollider(Vector2 offset, Vector2 size, float density)
        {
            PolygonShape ps = new PolygonShape(PolygonTools.CreateRectangle(size.x * Physics2D.pixelToUnit / 2, size.x * Physics2D.pixelToUnit / 2, offset * Physics2D.pixelToUnit, 0), density);
            body.CreateFixture(ps);
        }

        public void AddCircleCollider(Vector2 offset, float radius, float density)
        {
            CircleShape cs = new CircleShape(radius * Physics2D.pixelToUnit, density);
            cs.Position = offset * Physics2D.pixelToUnit;
            body.CreateFixture(cs);
        }

        public override void Initialize()
        {
            base.Initialize();

            body = FarseerPhysics.Factories.BodyFactory.CreateBody(Physics2D.world);
            Vector2 pos = GameObject.Transform.GlobalPosition;
            pos.x = pos.x * Physics2D.pixelToUnit;
            pos.y = pos.y * Physics2D.pixelToUnit;
            body.Position = pos;
            body.Rotation = GameObject.Transform.GlobalRotation;
            body.BodyType = BodyType.Dynamic;
            
            if (sleepMode == RigidbodySleepMode.StartAsleep)
            {
                body.Awake = false;
                body.SleepingAllowed = true;
            }
            else
            {
                if(sleepMode == RigidbodySleepMode.NeverSleep)
                {
                    body.SleepingAllowed = false;
                }
                else
                {
                    body.SleepingAllowed = true;
                }
                body.Awake = true;
            }
        }

        public override void Update()
        {
            body.Position = ((Microsoft.Xna.Framework.Vector2)((Vector2)transform.GlobalPosition)) * Physics2D.pixelToUnit;

            Color c = Color.white;
            int lw = 1;

            if(!isStatic)
            {
                if(IsAwake())
                {
                    c = Color.green;
                }
                else
                {
                    c = Color.blue;
                }
            }
            foreach(Fixture f in body.FixtureList)
            {
                if(f.Shape is CircleShape)
                {
                    CircleShape cs = (f.Shape as CircleShape);
                    Debug.DrawDebugCircle(position + ((Vector2)cs.Position * Physics2D.unitToPixel), cs.Radius * Physics2D.unitToPixel, c, lw);
                }
                else if(f.Shape is PolygonShape)
                {
                    PolygonShape ps = (f.Shape as PolygonShape);
                    Vector2[] verts = new Vector2[ps.Vertices.Count];
                    int count = 0;
                    foreach(Vector2 v in ps.Vertices)
                    {
                        verts[count] = position + (v * Physics2D.unitToPixel);
                        count++;
                    }
                    Debug.DrawDebugPolygon(verts, c, lw);
                }
            }
        }
    }
}
