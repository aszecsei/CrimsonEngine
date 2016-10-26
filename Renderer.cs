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
        public abstract Physics.Bounds Bounds();

        public abstract void DrawDiffuse(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime);

        public abstract void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
