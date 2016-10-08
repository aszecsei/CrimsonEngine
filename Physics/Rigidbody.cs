using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CrimsonEngine.Physics
{
    public class Rigidbody : Component
    {
        public bool awake { get; private set; }
        public float awakeThreshold = 0.1f;
        public Vector2 Velocity = Vector2.Zero;

        private bool isOnGround = false;
        public List<Rigidbody> holding = new List<Rigidbody>();

        public float gravity = 1.0f;

        public Collider attachedCollider
        {
            get
            {
                return GameObject.GetComponentOrSubclass<Collider>();
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Requires<Collider, BoxCollider>();
            awake = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(Velocity.LengthSquared() >= (awakeThreshold * awakeThreshold))
            {
                awake = true;
            }

            if(awake)
            {
                if (!isOnGround)
                {
                    Velocity += (Physics2D.Gravity * gravity);
                }

                Vector2 vel = Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 gravityProjection = Physics2D.Gravity * (Vector2.Dot(vel, Physics2D.Gravity) / Vector2.Dot(Physics2D.Gravity, Physics2D.Gravity));
                Vector3 oldPos = new Vector3(GameObject.Transform.GlobalPosition.X, GameObject.Transform.GlobalPosition.Y, GameObject.Transform.GlobalPosition.Z);
                GameObject.Transform.GlobalPosition += new Vector3(gravityProjection.X, gravityProjection.Y, 0);

                // check all collisions
                List<Collider> allColliders = SceneManager.CurrentScene.GetAllColliders();
                allColliders.Remove(attachedCollider);

                isOnGround = false;
                foreach(Collider c in allColliders)
                {
                    if(attachedCollider.IsTouching(c))
                    {
                        isOnGround = true;
                        GameObject.Transform.GlobalPosition = oldPos;
                        
                        if(c.attachedRigidbody != null)
                        {
                            c.attachedRigidbody.holding.Add(this);
                        }

                        awake = false;
                    }
                }

                Vector2 orthogonalProjection = vel - gravityProjection;
                oldPos = new Vector3(GameObject.Transform.GlobalPosition.X, GameObject.Transform.GlobalPosition.Y, GameObject.Transform.GlobalPosition.Z);
                GameObject.Transform.GlobalPosition += new Vector3(orthogonalProjection.X, orthogonalProjection.Y, 0);

                bool didMove = true;

                // check all collisions
                foreach (Collider c in allColliders)
                {
                    if (attachedCollider.IsTouching(c))
                    {
                        didMove = false;
                        if (c.attachedRigidbody != null)
                        {
                            c.attachedRigidbody.awake = true;

                            // Transfer of momentum!
                            c.attachedRigidbody.Velocity += (orthogonalProjection / (float)gameTime.ElapsedGameTime.TotalSeconds);
                        }
                        Velocity -= (orthogonalProjection / (float)gameTime.ElapsedGameTime.TotalSeconds);
                        GameObject.Transform.GlobalPosition = oldPos;
                    }
                }

                if(didMove)
                {
                    List<Rigidbody> toRemove = new List<Rigidbody>();
                    foreach (Rigidbody r in holding)
                    {
                        r.Velocity = Velocity;
                        if(!r.isOnGround)
                        {
                            toRemove.Add(r);
                        }
                    }
                    foreach(Rigidbody r in toRemove)
                    {
                        holding.Remove(r);
                        r.awake = true;
                    }
                }
            }
        }
    }
}
