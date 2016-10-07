using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine.Physics
{
    public class Rigidbody : Component
    {
        public Rigidbody()
        {
            this.Requires<Collider, BoxCollider>();
        }
    }
}
