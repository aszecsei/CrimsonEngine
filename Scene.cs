using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

using CrimsonEngine.Physics;

namespace CrimsonEngine
{
    public class Scene
    {
        public Color backgroundColor = Color.CornflowerBlue;

        public List<GameObject> GameObjects;

        public bool shouldDrawFrameRate = false;
        public bool shouldDrawPhysicsBounds = false;

        private float averageFramesPerSecond;
        private float currentFramesPerSecond;

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        private GraphicsDevice graphicsDevice;
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return graphicsDevice;
            }
        }

        private Vector2 _internalResolution = Vector2.Zero;
        public Vector2 InternalResolution
        {
            get
            {
                return _internalResolution;
            }
        }

        private RenderTarget2D diffuseRenderTarget;
        private RenderTarget2D normalRenderTarget;
        private RenderTarget2D lightRenderTarget;
        private RenderTarget2D shadowRenderTarget;
        private RenderTarget2D depthRenderTarget;

        private static readonly BlendState maxBlend = new BlendState()
        {
            AlphaBlendFunction = BlendFunction.Max,
            ColorBlendFunction = BlendFunction.Max,
            AlphaDestinationBlend = Blend.One,
            AlphaSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            ColorSourceBlend = Blend.One
        };

        public Scene(GraphicsDevice graphicsDevice, Vector2 internalResolution)
        {
            this.graphicsDevice = graphicsDevice;
            GameObjects = new List<GameObject>();
            _internalResolution = internalResolution;

            diffuseRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.X, (int)internalResolution.Y);
            normalRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.X, (int)internalResolution.Y);
            lightRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.X, (int)internalResolution.Y);
            shadowRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.X, (int)internalResolution.Y);
            depthRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.X, (int)internalResolution.Y);
        }

        public GameObject InstantiateGameObject()
        {
            GameObject result = new GameObject();
            GameObjects.Add(result);
            return result;
        }

        public void InstantiateGameObject(GameObject go)
        {
            GameObjects.Add(go);
        }

        public void DestroyGameObject(GameObject go)
        {
            GameObjects.Remove(go);
        }

        public void ClampMouseToWindow()
        {
            float aspectRatio;
            if (InternalResolution != Vector2.Zero)
            {
                aspectRatio = InternalResolution.X / InternalResolution.Y;
            }
            else
            {
                aspectRatio = (float)graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;
            }

            // constrain the mouse position
            MouseState ms = Mouse.GetState();
            Vector2 mPos = ms.Position.ToVector2();

            Rectangle viewport = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

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

        public void Update(GameTime gameTime)
        {

            // ClampMouseToWindow();

            foreach (GameObject go in GameObjects)
            {
                if (go.isActive)
                {
                    go.Update(gameTime);
                }
            }

            // Insertion-sort the GameObject list
            // Since teleportation in the z-direction is rare!
            // So the sorting is often nearly-sorted.
            GameObject temp = null;
            for(int i=1; i<GameObjects.Count; i++)
            {
                int j = i;

                while((j > 0) && (GameObjects[j].Transform.GlobalPosition.Z > GameObjects[j - 1].Transform.GlobalPosition.Z))
                {
                    int k = j - 1;
                    temp = GameObjects[k];
                    GameObjects[k] = GameObjects[j];
                    GameObjects[j] = temp;
                    j--;
                }
            }
        }

        public List<Collider> GetAllColliders()
        {
            List<Collider> result = new List<Collider>();
            foreach (GameObject go in GameObjects)
            {
                if(go.isActive)
                {
                    Collider c = go.GetComponentOrSubclass<Collider>();
                    if(c != null && c.isActive)
                    {
                        result.Add(c);
                    }
                }
            }
            return result;
        }

        private bool shouldDraw(GameObject go)
        {
            return go.isActive && (go.Transform.GlobalPosition.Z >= Camera2D.main.Transform.GlobalPosition.Z);
        }

        private void DrawScene(SpriteBatch spriteBatch, GameTime gameTime)
        {
            graphicsDevice.SetRenderTarget(diffuseRenderTarget);

            graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp);

            foreach (GameObject go in GameObjects)
            {
                if (shouldDraw(go))
                {
                    go.DrawDiffuse(spriteBatch, gameTime);
                }
            }

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(normalRenderTarget);

            graphicsDevice.Clear(new Color(0.5f, 0.5f, 1.0f));

            spriteBatch.Begin(transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp);

            foreach (GameObject go in GameObjects)
            {
                if (shouldDraw(go))
                {
                    go.DrawNormal(spriteBatch, gameTime);
                }
            }

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(depthRenderTarget);
            graphicsDevice.Clear(Color.White);

            // first, find the total depth range of the thing
            float minDepth = Camera2D.main.Transform.GlobalPosition.Z;
            float maxDepth = GameObjects[0].Transform.GlobalPosition.Z;
            float depthRange = maxDepth - minDepth;
            if (depthRange == 0)
                depthRange = 1;

            // using this, we can calculate a float from 0-1 representing the depth of a game object
            Effect depthMap = ResourceManager.GetResource<Effect>("DepthMap");
            spriteBatch.Begin(effect: depthMap, transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp, blendState:BlendState.AlphaBlend, sortMode:SpriteSortMode.BackToFront);
            float currentDepth = minDepth;
            foreach (GameObject go in GameObjects)
            {
                if(shouldDraw(go))
                {
                    if(currentDepth != go.Transform.GlobalPosition.Z)
                    {
                        currentDepth = go.Transform.GlobalPosition.Z;
                        spriteBatch.End();
                        spriteBatch.Begin(effect: depthMap, transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.BackToFront);
                    }
                    float depthValue = (((go.Transform.GlobalPosition.Z - minDepth) / depthRange) * 0.8f);
                    depthMap.Parameters["depth"].SetValue(depthValue);
                    depthMap.CurrentTechnique.Passes[0].Apply();
                    go.DrawDiffuse(spriteBatch, gameTime);
                }
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            graphicsDevice.Clear(Color.Black);

            /*
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(diffuseRenderTarget, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
            */
            
            Effect defaultLit = ResourceManager.GetResource<Effect>("Default Lit");

            foreach (GameObject go in GameObjects)
            {
                if(go.isActive)
                {
                    Light l = go.GetComponent<Light>();
                    if(l != null)
                    {
                        spriteBatch.Begin(effect: defaultLit, samplerState: SamplerState.PointClamp, blendState: maxBlend);
                        defaultLit.Parameters["NormalMap"].SetValue(normalRenderTarget);
                        defaultLit.Parameters["DepthMap"].SetValue(depthRenderTarget);
                        defaultLit.Parameters["MinDepth"].SetValue(minDepth);
                        defaultLit.Parameters["DepthRange"].SetValue(depthRange);
                        defaultLit.Parameters["DiffuseLightDirection"].SetValue(new Vector3((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds), (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds), 15));
                        defaultLit.Parameters["DiffuseIntensity"].SetValue(0f);
                        Vector3 lightScreenLoc = go.Transform.GlobalPosition;
                        lightScreenLoc.Y *= -1;
                        lightScreenLoc = Camera2D.main.WorldToScreen(lightScreenLoc);
                        lightScreenLoc.Z = go.Transform.GlobalPosition.Z;
                        defaultLit.Parameters["LightLocationScreen"].SetValue(lightScreenLoc);
                        defaultLit.Parameters["LightColor"].SetValue(l.Color.ToVector4());
                        defaultLit.Parameters["LightIntensity"].SetValue(l.Intensity);
                        defaultLit.Parameters["LightRange"].SetValue(l.Range * Camera2D.main.Zoom);

                        defaultLit.Parameters["LightLength"].SetValue(l.Length * Camera2D.main.Zoom);
                        defaultLit.Parameters["LightRotation"].SetValue((l.Rotation + 180) / 180f * (float)Math.PI);
                        defaultLit.Parameters["LightConeAngle"].SetValue(l.ConeAngle / 180f * (float)Math.PI);
                        defaultLit.Parameters["LightPenumbraAngle"].SetValue(l.PenumbraAngle / 180f * (float)Math.PI);
                        defaultLit.Parameters["BackgroundColor"].SetValue(backgroundColor.ToVector4());
                        
                        defaultLit.CurrentTechnique.Passes[0].Apply();
                        spriteBatch.Draw(diffuseRenderTarget, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
                        spriteBatch.End();
                    }
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawScene(spriteBatch, gameTime);

            float aspectRatio = InternalResolution.X / InternalResolution.Y;

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

            if (shouldDrawPhysicsBounds)
            {
                Texture2D pixel = ResourceManager.GetResource<Texture2D>("Pixel");
                spriteBatch.Begin(transformMatrix:Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp);
                foreach(GameObject go in SceneManager.CurrentScene.GameObjects)
                {
                    if(go.isActive)
                    {
                        Collider c = go.GetComponent<BoxCollider>();
                        if(c != null)
                        {
                            Bounds b = c.Bounds();
                            Vector2 dTL = Vector2.Transform(new Vector2(b.Left, -1 * b.Top), Camera2D.main.TranslationMatrix);
                            Vector2 dBR = Vector2.Transform(new Vector2(b.Right, -1 * b.Bottom), Camera2D.main.TranslationMatrix);
                            Vector2 dSize = dBR - dTL;
                            Rectangle dest = new Rectangle(dTL.ToPoint(), dSize.ToPoint());
                            if (c.attachedRigidbody == null)
                                spriteBatch.Draw(pixel, dest, Color.Green);
                            else if(c.attachedRigidbody.awake)
                                spriteBatch.Draw(pixel, dest, Color.Red);
                            else
                                spriteBatch.Draw(pixel, dest, Color.Blue);
                        }
                    }
                }
                spriteBatch.End();
            }

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
        }
    }
}
