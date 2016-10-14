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

        public Color TintColor = Color.white;

        public Vector2 Origin = Vector2.zero;

        public bool flipHorizontal = false;
        public bool flipVertical = false;

        private SpriteEffects effect
        {
            get
            {
                if (!flipHorizontal && !flipVertical)
                    return SpriteEffects.None;
                else if (flipHorizontal && flipVertical)
                    return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                else if (flipHorizontal)
                    return SpriteEffects.FlipHorizontally;
                else
                    return SpriteEffects.FlipVertically;
            }
        }

        public override void DrawDiffuse(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if(Diffuse  != null)
            {
                spriteBatch.Draw(texture: Diffuse, 
                    position: new Vector2(GameObject.transform.GlobalPosition.x, -GameObject.transform.GlobalPosition.y), 
                    color: TintColor, 
                    rotation: GameObject.transform.GlobalRotation, 
                    origin: Origin, 
                    scale: new Vector2(GameObject.transform.GlobalScale.x, GameObject.transform.GlobalScale.y),
                    effects:effect);
            }
            else
            {
                // no need to draw, since it's transparent!
            }
        }

        public override void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Normal != null)
            {
                spriteBatch.Draw(texture: Normal,
                    position: new Vector2(GameObject.transform.GlobalPosition.x, -GameObject.transform.GlobalPosition.y),
                    color: TintColor,
                    rotation: GameObject.transform.GlobalRotation,
                    origin: Origin,
                    scale: new Vector2(GameObject.transform.GlobalScale.x, GameObject.transform.GlobalScale.y),
                    effects:effect);
            }
            else
            {
                // TODO: Draw blank normal map
            }
        }

        public override void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Emissive != null)
            {
                spriteBatch.Draw(texture: Emissive,
                    position: new Vector2(GameObject.transform.GlobalPosition.x, -GameObject.transform.GlobalPosition.y),
                    color: TintColor,
                    rotation: GameObject.transform.GlobalRotation,
                    origin: Origin,
                    scale: new Vector2(GameObject.transform.GlobalScale.x, GameObject.transform.GlobalScale.y),
                    effects:effect);
            }
            else
            {
                // TODO: Draw blank emissive map
            }
        }

        public override void DrawSpecular(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Specular != null)
            {
                spriteBatch.Draw(texture: Specular,
                    position: new Vector2(GameObject.transform.GlobalPosition.x, -GameObject.transform.GlobalPosition.y),
                    color: TintColor,
                    rotation: GameObject.transform.GlobalRotation,
                    origin: Origin,
                    scale: new Vector2(GameObject.transform.GlobalScale.x, GameObject.transform.GlobalScale.y),
                    effects: effect);
            }
            else
            {
                // TODO: Draw blank specular map
            }
        }
    }
}
