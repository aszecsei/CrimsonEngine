using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public class Scene
    {
        public Color backgroundColor = Color.CornflowerBlue;

        public List<GameObject> GameObjects;

        public Camera2D CurrentCamera;

        public Scene()
        {
            GameObjects = new List<GameObject>();
        }

        public void InstantiateGameObject(GameObject go)
        {
            GameObjects.Add(go);
        }

        public void DestroyGameObject(GameObject go)
        {
            GameObjects.Remove(go);
        }

        public void Update(GameTime gameTime)
        {
            foreach(GameObject go in GameObjects)
            {
                if(go.isActive)
                {
                    go.Update(gameTime);
                }
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, GameTime gameTime)
        {
            graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(transformMatrix: CurrentCamera.TranslationMatrix, samplerState: SamplerState.PointClamp);
            foreach (GameObject go in GameObjects)
            {
                if (go.isActive)
                {
                    go.Draw(spriteBatch, gameTime);
                }
            }

            spriteBatch.End();
        }
    }
}
