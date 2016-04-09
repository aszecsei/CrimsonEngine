using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    public abstract class Component
    {
        public bool isActive = true;

        public virtual void Update(GameTime gameTime) { }

        public GameObject GameObject
        {
            get; set;
        }
    }
}
