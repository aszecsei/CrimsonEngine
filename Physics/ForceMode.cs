using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine.Physics
{
    /// <summary>
    /// Option for how to apply a force using <see cref="Rigidbody.AddForce(Microsoft.Xna.Framework.Vector2, ForceMode)"/>.
    /// </summary>
    public enum ForceMode
    {
        /// <summary>
        /// Add a force to the rigidbody, using its mass.
        /// </summary>
        Force,
        /// <summary>
        /// Add an instant force impulse to the rigidbody, using its mass.
        /// </summary>
        Impulse
    }
}
