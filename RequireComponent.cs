using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    /// <summary>
    /// The RequireComponent attribute automatically adds required components as dependencies.
    /// </summary>
    /// <remarks>
    /// When you add a script which uses RequireComponent to a GameObject, the required component will
    /// automatically be added to the GameObject. This is useful to avoid setup errors. For example a
    /// script might require that a Rigidbody is always added to the same GameObject. Using RequireComponent
    /// this will be done automatically, thus you can never get the setup wrong. Note that RequireComponent
    /// only checks for missing dependencies during the moment the component is added to a GameObject. Existing
    /// instances of the component whose GameObject lacks the new dependencies will not have those dependencies
    /// automatically added.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireComponent : Attribute
    {
        public Type requiredType;

        public RequireComponent(Type t)
        {
            requiredType = t;
        }
    }
}
