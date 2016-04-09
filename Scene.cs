using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

namespace CrimsonEngine
{
    public class Scene
    {
        public Color backgroundColor = Color.CornflowerBlue;

        public List<GameObject> GameObjects;

        public bool shouldDrawFrameRate = false;

        private float averageFramesPerSecond;
        private float currentFramesPerSecond;

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        private bool fitToAspectRatio = false;
        private float aspectRatio;

        private GraphicsDevice graphicsDevice;

        public Scene(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            GameObjects = new List<GameObject>();
        }

        public GameObject InstantiateGameObject()
        {
            GameObject result = new GameObject();
            GameObjects.Add(result);
            return result;
        }

        public void FitToAspectRatio(float aspectRatio)
        {
            fitToAspectRatio = true;
            this.aspectRatio = aspectRatio;
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
            if(fitToAspectRatio)
            {
                // constrain the mouse position
                MouseState ms = Mouse.GetState();
                Vector2 mPos = ms.Position.ToVector2();

                Rectangle viewport = Rectangle.Empty;

                float actualAspectRatio = (float)graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;
                if (actualAspectRatio > aspectRatio)
                {
                    // the viewport is wider than the actual view
                    // so we should pillarbox

                    int sub = (int)Math.Round(graphicsDevice.Viewport.Height * aspectRatio);
                    int difference = (graphicsDevice.Viewport.Width - sub) / 2;

                    viewport = new Rectangle(difference, 0, graphicsDevice.Viewport.Width - (difference * 2), graphicsDevice.Viewport.Height);
                }
                else if (actualAspectRatio < aspectRatio)
                {
                    // we should letterbox

                    int sub = (int)Math.Round(graphicsDevice.Viewport.Width / aspectRatio);
                    int difference = (graphicsDevice.Viewport.Height - sub) / 2;

                    viewport = new Rectangle(0, difference, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height - (difference * 2));
                }

                mPos.X = MathHelper.Clamp(mPos.X, viewport.Left, viewport.Right);
                mPos.Y = MathHelper.Clamp(mPos.Y, viewport.Top, viewport.Bottom);

                Mouse.SetPosition((int)mPos.X, (int)mPos.Y);
            }

            foreach (GameObject go in GameObjects)
            {
                if (go.isActive)
                {
                    go.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // RenderTarget2D rt = new RenderTarget2D(graphicsDevice, 480, 270);

            // graphicsDevice.SetRenderTarget(rt);

            graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp);

            foreach (GameObject go in GameObjects)
            {
                if (go.isActive)
                {
                    go.Draw(spriteBatch, gameTime);
                }
            }

            spriteBatch.End();

            
            if (shouldDrawFrameRate)
            {
                currentFramesPerSecond = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;
                _sampleBuffer.Enqueue(currentFramesPerSecond);
                if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
                {
                    _sampleBuffer.Dequeue();
                    averageFramesPerSecond = _sampleBuffer.Average(i => i);
                }
                else
                {
                    averageFramesPerSecond = currentFramesPerSecond;
                }

                var fps = string.Format("FPS: {0}", averageFramesPerSecond);

                spriteBatch.Begin();
                spriteBatch.DrawString(ResourceManager.GetResource<SpriteFont>("Debug Font"), fps, new Vector2(10, 10), Color.White);
                spriteBatch.End();
            }

            /*

            graphicsDevice.SetRenderTarget(null);

            graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(rt, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();

    */
            if (fitToAspectRatio)
            {
                using (Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1))
                {
                    whiteRectangle.SetData(new[] { Color.White });

                    spriteBatch.Begin();

                    float actualAspectRatio = (float)graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;
                    if (actualAspectRatio > aspectRatio)
                    {
                        // the viewport is wider than the actual view
                        // so we should pillarbox

                        int sub = (int)Math.Round(graphicsDevice.Viewport.Height * aspectRatio);
                        int difference = (graphicsDevice.Viewport.Width - sub) / 2;
                        spriteBatch.Draw(whiteRectangle, new Rectangle(0, 0, difference, graphicsDevice.Viewport.Height), Color.Black);
                        spriteBatch.Draw(whiteRectangle, new Rectangle(graphicsDevice.Viewport.Width - difference, 0, difference, graphicsDevice.Viewport.Height), Color.Black);
                    }
                    else if (actualAspectRatio < aspectRatio)
                    {
                        // we should letterbox

                        int sub = (int)Math.Round(graphicsDevice.Viewport.Width / aspectRatio);
                        int difference = (graphicsDevice.Viewport.Height - sub) / 2;
                        spriteBatch.Draw(whiteRectangle, new Rectangle(0, 0, graphicsDevice.Viewport.Width, difference), Color.Black);
                        spriteBatch.Draw(whiteRectangle, new Rectangle(0, graphicsDevice.Viewport.Height - difference, graphicsDevice.Viewport.Width, difference), Color.Black);
                    }

                    spriteBatch.End();
                }
            }

        }
    }
}
