using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics.Contacts;

namespace CrimsonEngine.Physics
{
    class ContactListener
    {
        public static void PreSolve(Contact contact, ref FarseerPhysics.Collision.Manifold oldManifold)
        {

        }

        public static void PostSolve(Contact contact, ContactVelocityConstraint impulse)
        {

        }

        public static bool BeginContact(Contact contact)
        {
            bool isTrigger = false;

            GameObject goA = null;
            GameObject goB = null;

            foreach (GameObject go in SceneManager.CurrentScene.ActiveGameObjects)
            {
                Rigidbody r = go.GetComponent<Rigidbody>();
                if (r && r.body == contact.FixtureA.Body)
                {
                    goA = go;
                    if (r.IsTrigger)
                        isTrigger = true;
                }
                else if (r && r.body == contact.FixtureB.Body)
                {
                    goB = go;
                    if (r.IsTrigger)
                        isTrigger = true;
                }

                if (goA != null && goB != null)
                    break;
            }
            if (!isTrigger)
            {
                // TODO: construct collision data
                goA.BroadcastMessage("OnCollisionEnter", goB.GetComponent<Rigidbody>());
                goB.BroadcastMessage("OnCollisionEnter", goA.GetComponent<Rigidbody>());
            }
            else
            {
                goA.BroadcastMessage("OnTriggerEnter", goB.GetComponent<Rigidbody>());
                goB.BroadcastMessage("OnTriggerEnter", goA.GetComponent<Rigidbody>());
            }


            return true;
        }

        public static void EndContact(Contact contact)
        {
            bool isTrigger = false;

            GameObject goA = null;
            GameObject goB = null;

            foreach (GameObject go in SceneManager.CurrentScene.ActiveGameObjects)
            {
                Rigidbody r = go.GetComponent<Rigidbody>();
                if (r && r.body == contact.FixtureA.Body)
                {
                    goA = go;
                    if (r.IsTrigger)
                        isTrigger = true;
                }
                else if (r && r.body == contact.FixtureB.Body)
                {
                    goB = go;
                    if (r.IsTrigger)
                        isTrigger = true;
                }

                if (goA != null && goB != null)
                    break;
            }
            if(!isTrigger)
            {
                goA.BroadcastMessage("OnCollisionExit", goB.GetComponent<Rigidbody>());
                goB.BroadcastMessage("OnCollisionExit", goA.GetComponent<Rigidbody>());
            }
            else
            {
                goA.BroadcastMessage("OnTriggerExit", goB.GetComponent<Rigidbody>());
                goB.BroadcastMessage("OnTriggerExit", goA.GetComponent<Rigidbody>());
            }
        }
    }
}
