﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    public class SpriteRenderer : Renderer
    {
        public Material material = new Material();

        public Color TintColor = Color.white;

        public Vector2 Origin = Vector2.zero;

        public bool flipHorizontal = false;
        public bool flipVertical = false;

        public override Physics.Bounds Bounds()
        {
            return new Physics.Bounds((Vector2)GameObject.transform.GlobalPosition - Origin, Vector2.Scale(new Vector2(material.DiffuseTexture.Width, material.DiffuseTexture.Height), GameObject.transform.GlobalScale));
        }

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
            if(material.DiffuseTexture != null)
            {
                spriteBatch.Draw(texture: material.DiffuseTexture, 
                    position: Vector2.Scale(Vector2.downRight, GameObject.transform.GlobalPosition), 
                    color: TintColor, 
                    rotation: GameObject.transform.GlobalRotation, 
                    origin: Origin, 
                    scale: GameObject.transform.GlobalScale,
                    effects:effect);
            }
            else
            {
                // no need to draw, since it's transparent!
            }
        }

        public override void DrawNormal(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (material.NormalTexture != null)
            {
                spriteBatch.Draw(texture: material.NormalTexture,
                    position: Vector2.Scale(Vector2.downRight, GameObject.transform.GlobalPosition),
                    color: TintColor,
                    rotation: GameObject.transform.GlobalRotation,
                    origin: Origin,
                    scale: GameObject.transform.GlobalScale,
                    effects:effect);
            }
            else
            {
                // TODO: Draw blank normal map
            }
        }

        public override void DrawEmissive(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (material.EmissiveTexture != null)
            {
                spriteBatch.Draw(texture: material.EmissiveTexture,
                    position: Vector2.Scale(Vector2.downRight, GameObject.transform.GlobalPosition),
                    color: TintColor,
                    rotation: GameObject.transform.GlobalRotation,
                    origin: Origin,
                    scale: GameObject.transform.GlobalScale,
                    effects:effect);
            }
            else
            {
                // TODO: Draw blank emissive map
            }
        }
    }
}
