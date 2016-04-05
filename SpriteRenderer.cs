using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    public class SpriteRenderer : Renderer
    {
        public Texture2D Diffuse = null;
        public Texture2D Normal = null;
        public Texture2D Specular = null;
        public Texture2D Emissive = null;

        public Color TintColor = Color.White;

        public Vector2 Origin = Vector2.Zero;

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(Diffuse  != null)
            {
                spriteBatch.Draw(texture: Diffuse, 
                    position: new Vector2(GameObject.Transform.GlobalPosition.X - Origin.X, GameObject.Transform.GlobalPosition.Y - Origin.Y), 
                    color: TintColor, 
                    rotation: GameObject.Transform.GlobalRotation, 
                    origin: Origin, 
                    layerDepth: GameObject.Transform.GlobalPosition.Z,
                    scale: new Vector2(GameObject.Transform.GlobalScale.X, GameObject.Transform.GlobalScale.Y));
            }
        }
    }
}
