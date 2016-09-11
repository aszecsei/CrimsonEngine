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
        public virtual void DrawDiffuse(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public virtual void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public virtual void DrawSpecular(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public virtual void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
    }
}
