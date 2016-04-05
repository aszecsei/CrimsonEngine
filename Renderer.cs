using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    public abstract class Renderer : Component
    {
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
