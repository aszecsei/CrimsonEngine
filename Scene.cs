using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

using CrimsonEngine.Physics;
using System.Collections;

namespace CrimsonEngine
{
    public class Scene
    {
        #region Variables
        public Color backgroundColor = Color.black;

        internal List<GameObject> ActiveGameObjects = new List<GameObject>();
        internal SortedList<float, Object> DestroyedObjects = new SortedList<float, Object>();
        internal List<GameObject> InstantiatedGameObjects = new List<GameObject>();

        internal Dictionary<string, List<GameObject>> taggedObjects = new Dictionary<string, List<GameObject>>();

        public bool shouldDrawFrameRate = false;
        public bool clampMouseToWindow = false;

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

        private Vector2 _internalResolution = Vector2.zero;
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

        private List<IEnumerator> Coroutines = new List<IEnumerator>();
        private List<IEnumerator> shouldRunNextFrame = new List<IEnumerator>();
        private List<IEnumerator> shouldRunAtEndOfFrame = new List<IEnumerator>();
        private List<IEnumerator> shouldRunAtFixedTimestep = new List<IEnumerator>();
        private SortedList<float, IEnumerator> shouldRunAfterTimes = new SortedList<float, IEnumerator>();

        private static readonly BlendState maxBlend = new BlendState()
        {
            AlphaBlendFunction = BlendFunction.Max,
            ColorBlendFunction = BlendFunction.Max,
            AlphaDestinationBlend = Blend.One,
            AlphaSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            ColorSourceBlend = Blend.One
        };

        private int numFixedUpdates = 0;
        private bool hasStarted = false;
        #endregion

        #region Constructor
        public Scene(GraphicsDevice graphicsDevice, Vector2 internalResolution)
        {
            this.graphicsDevice = graphicsDevice;

            _internalResolution = internalResolution;

            diffuseRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.x, (int)internalResolution.y);
            normalRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.x, (int)internalResolution.y);
            lightRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.x, (int)internalResolution.y);
            shadowRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.x, (int)internalResolution.y);
            depthRenderTarget = new RenderTarget2D(graphicsDevice, (int)internalResolution.x, (int)internalResolution.y);

            Physics2D.Initialize();
        }
        #endregion

        #region Public Functions
        public void StartCoroutine(IEnumerator e)
        {
            Coroutines.Add(e);
        }

        public void Update(GameTime gameTime)
        {
            HandleEndOfFrameCoroutines();

            HandleDestroyObjects();

            HandleStart();

            Time.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * Time.timeScale;
            Time.time += Time.deltaTime;

            shouldRunNextFrame.Clear();

            int numTimesToRunFixedUpdate = 1;
            if (Time.fixedDeltaTime != 0)
            {
                numTimesToRunFixedUpdate = (int)Math.Floor((float)(Time.time - Time.fixedTime) / Time.fixedDeltaTime);
            }

            // Never run our physics solver more than 5 times per frame
            if (numTimesToRunFixedUpdate > 5)
            {
                float dT = numTimesToRunFixedUpdate * Time.fixedDeltaTime;
                Time.actualFixedDeltaTime = Time.fixedDeltaTime / 5;
                numTimesToRunFixedUpdate = 5;
            }
            numFixedUpdates = numTimesToRunFixedUpdate;

            for (int i = 0; i < numTimesToRunFixedUpdate; i++)
            {
                HandleFixedUpdate();
                HandleWaitForFixedUpdate();
                HandlePhysicsUpdate();
                HandleOnTrigger();
                HandleOnCollision();
            }

            if (clampMouseToWindow)
            {
                ClampMouseToWindow();
            }

            HandleUpdate();

            HandleCoroutines();

            HandleAnimationUpdate();

            // Insertion-sort the GameObject list
            // Since teleportation in the z-direction is rare!
            // So the sorting is often nearly-sorted.
            GameObject temp = null;
            for (int i = 1; i < ActiveGameObjects.Count; i++)
            {
                int j = i;

                while ((j > 0) && (ActiveGameObjects[j].transform.GlobalPosition.z > ActiveGameObjects[j - 1].transform.GlobalPosition.z))
                {
                    int k = j - 1;
                    temp = ActiveGameObjects[k];
                    ActiveGameObjects[k] = ActiveGameObjects[j];
                    ActiveGameObjects[j] = temp;
                    j--;
                }
            }
        }
        #endregion

        #region Internal Methods
        internal void ClampMouseToWindow()
        {
            float aspectRatio;
            if (InternalResolution != Vector2.zero)
            {
                aspectRatio = InternalResolution.x / InternalResolution.y;
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

            mPos.x = MathHelper.Clamp(mPos.x, viewport.Left, viewport.Right);
            mPos.y = MathHelper.Clamp(mPos.y, viewport.Top, viewport.Bottom);

            Mouse.SetPosition((int)mPos.x, (int)mPos.y);
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawScene(spriteBatch, gameTime);

            float aspectRatio = InternalResolution.x / InternalResolution.y;

            Texture2D whiteRectangle = ResourceManager.GetResource<Texture2D>("Pixel");

           spriteBatch.Begin();

            float actualAspectRatio = (float)graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;
            if (actualAspectRatio > aspectRatio)
            {
                // the viewport is wider than the actual view
                // so we should pillarbox

                int sub = (int)Math.Round(graphicsDevice.Viewport.Height * aspectRatio);
                int difference = (graphicsDevice.Viewport.Width - sub) / 2;
                spriteBatch.Draw(whiteRectangle, new Rectangle(0, 0, difference, graphicsDevice.Viewport.Height), Color.black);
                spriteBatch.Draw(whiteRectangle, new Rectangle(graphicsDevice.Viewport.Width - difference, 0, difference, graphicsDevice.Viewport.Height), Color.black);
            }
            else if (actualAspectRatio < aspectRatio)
            {
                // we should letterbox

                int sub = (int)Math.Round(graphicsDevice.Viewport.Width / aspectRatio);
                int difference = (graphicsDevice.Viewport.Height - sub) / 2;
                spriteBatch.Draw(whiteRectangle, new Rectangle(0, 0, graphicsDevice.Viewport.Width, difference), Color.black);
                spriteBatch.Draw(whiteRectangle, new Rectangle(0, graphicsDevice.Viewport.Height - difference, graphicsDevice.Viewport.Width, difference), Color.black);
            }

            spriteBatch.End();

            Debug.Draw(spriteBatch, gameTime);

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

                var fps = string.Format("FPS: {0}\nDelta Time: {1}\nFixed Updates Per Frame: {2}", averageFramesPerSecond, Time.deltaTime, numFixedUpdates);

                spriteBatch.Begin();
                spriteBatch.DrawString(ResourceManager.GetResource<SpriteFont>("Debug Font"), fps, new Vector2(10, 10), Color.white);
                spriteBatch.End();
            }
        }
        #endregion

        #region Private Methods
        private void HandleStart()
        {
            while (InstantiatedGameObjects.Count > 0)
            {
                InstantiatedGameObjects[0].SendMessage("Start");
                ActiveGameObjects.Add(InstantiatedGameObjects[0]);
                InstantiatedGameObjects.RemoveAt(0);
            }
        }

        private void HandleFixedUpdate()
        {
            Time.fixedTime += Time.fixedDeltaTime;

            foreach (GameObject go in ActiveGameObjects)
            {
                if (go.activeInHierarchy)
                {
                    go.SendMessage("FixedUpdate");
                }
            }
        }

        private void HandleWaitForFixedUpdate()
        {
            List<IEnumerator> mFixed = new List<IEnumerator>(shouldRunAtFixedTimestep);
            foreach (IEnumerator e in mFixed)
            {
                shouldRunAtFixedTimestep.Remove(e);
                HandleCoroutine(e);
            }
        }

        private void HandlePhysicsUpdate()
        {
            foreach(GameObject go in ActiveGameObjects)
            {
                Rigidbody r = go.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.UpdateBodyPosition();
                }
            }
            Physics2D.Simulate();
            foreach (GameObject go in ActiveGameObjects)
            {
                Rigidbody r = go.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.ResetAfterMoveRot();
                    r.UpdateTransform();
                }
            }
        }

        private void HandleOnTrigger()
        {

        }

        private void HandleOnCollision()
        {

        }

        private void HandleUpdate()
        {
            foreach(GameObject go in ActiveGameObjects)
            {
                go.SendMessage("Update");
            }
        }

        private void HandleCoroutines()
        {
            foreach (IEnumerator coroutine in Coroutines)
            {
                HandleCoroutine(coroutine);
            }

            List<float> remove = new List<float>();
            foreach (KeyValuePair<float, IEnumerator> kvp in shouldRunAfterTimes)
            {
                if (kvp.Key < Time.time)
                {
                    Coroutines.Add(kvp.Value);
                    remove.Add(kvp.Key);
                }
                else
                {
                    break;
                }
            }
            foreach (float k in remove)
            {
                shouldRunAfterTimes.Remove(k);
            }

            Coroutines = new List<IEnumerator>(shouldRunNextFrame);
        }

        private void HandleAnimationUpdate()
        {

        }

        private void HandleLateUpdate()
        {
            foreach(GameObject go in ActiveGameObjects)
            {
                go.SendMessage("LateUpdate");
            }
        }

        private void HandleEndOfFrameCoroutines()
        {
            List<IEnumerator> crs = new List<IEnumerator>(shouldRunAtEndOfFrame);
            shouldRunAtEndOfFrame.Clear();
            foreach (IEnumerator coroutine in crs)
            {
                HandleCoroutine(coroutine);
            }
        }

        private void HandleDestroyObjects()
        {
            while(DestroyedObjects.Count > 0)
            {
                KeyValuePair<float, Object> first = DestroyedObjects.First();
                if (first.Key > Time.time)
                    break;

                if(first.Value is GameObject)
                {
                    (first.Value as GameObject).SendMessage("OnDestroy");
                    ActiveGameObjects.Remove(first.Value as GameObject);
                }
                else if(first.Value is Component)
                {
                    (first.Value as Component).Invoke("OnDestroy");
                    (first.Value as Component).GameObject.RemoveComponent(first.Value.GetType());
                }
                
                DestroyedObjects.Remove(first.Key);
            }
        }

        private void HandleCoroutine(IEnumerator coroutine)
        {
            if (!coroutine.MoveNext())
            {
                // This coroutine has finished
                return;
            }

            if (!(coroutine.Current is YieldInstruction))
            {
                // This coroutine yielded null, or some other value we don't understand; run it next frame.
                shouldRunNextFrame.Add(coroutine);
                return;
            }

            if (coroutine.Current is WaitForSeconds)
            {
                WaitForSeconds wait = (WaitForSeconds)coroutine.Current;
                shouldRunAfterTimes.Add(Time.time + wait.duration, coroutine);
            }
            else if (coroutine.Current is WaitForEndOfFrame)
            {
                shouldRunAtEndOfFrame.Add(coroutine);
            }
            else if (coroutine.Current is WaitForFixedUpdate)
            {
                shouldRunAtFixedTimestep.Add(coroutine);
            }
        }

        private bool shouldDraw(GameObject go)
        {
            return go.activeInHierarchy && (go.transform.GlobalPosition.z >= Camera2D.main.transform.GlobalPosition.z);
        }

        private void DrawScene(SpriteBatch spriteBatch, GameTime gameTime)
        {
            graphicsDevice.SetRenderTarget(diffuseRenderTarget);

            graphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin(transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp);

            foreach (GameObject go in ActiveGameObjects)
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

            foreach (GameObject go in ActiveGameObjects)
            {
                if (shouldDraw(go))
                {
                    go.DrawNormal(spriteBatch, gameTime);
                }
            }

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(depthRenderTarget);
            graphicsDevice.Clear(Color.white);

            // first, find the total depth range of the thing
            float minDepth = Camera2D.main.transform.GlobalPosition.z;
            float maxDepth = ActiveGameObjects[0].transform.GlobalPosition.z;
            float depthRange = maxDepth - minDepth;
            if (depthRange == 0)
                depthRange = 1;

            // using this, we can calculate a float from 0-1 representing the depth of a game object
            Effect depthMap = ResourceManager.GetResource<Effect>("DepthMap");
            spriteBatch.Begin(effect: depthMap, transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.BackToFront);
            float currentDepth = minDepth;
            foreach (GameObject go in ActiveGameObjects)
            {
                if (shouldDraw(go))
                {
                    if (currentDepth != go.transform.GlobalPosition.z)
                    {
                        currentDepth = go.transform.GlobalPosition.z;
                        spriteBatch.End();
                        spriteBatch.Begin(effect: depthMap, transformMatrix: Camera2D.main.TranslationMatrix, samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.BackToFront);
                    }
                    float depthValue = (((go.transform.GlobalPosition.z - minDepth) / depthRange) * 0.8f);
                    depthMap.Parameters["depth"].SetValue(depthValue);
                    depthMap.CurrentTechnique.Passes[0].Apply();
                    go.DrawDiffuse(spriteBatch, gameTime);
                }
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            graphicsDevice.Clear(Color.black);

            /*
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(diffuseRenderTarget, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
            */

            Effect defaultLit = ResourceManager.GetResource<Effect>("Default Lit");

            foreach (GameObject go in ActiveGameObjects)
            {
                if (go.transform)
                {
                    Light l = go.GetComponent<Light>();
                    if (l != null)
                    {
                        spriteBatch.Begin(effect: defaultLit, samplerState: SamplerState.PointClamp, blendState: maxBlend);
                        defaultLit.Parameters["NormalMap"].SetValue(normalRenderTarget);
                        defaultLit.Parameters["DepthMap"].SetValue(depthRenderTarget);
                        defaultLit.Parameters["MinDepth"].SetValue(minDepth);
                        defaultLit.Parameters["DepthRange"].SetValue(depthRange);
                        defaultLit.Parameters["DiffuseLightDirection"].SetValue(new Vector3((float)Math.Cos(gameTime.TotalGameTime.TotalSeconds), (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds), 15));
                        defaultLit.Parameters["DiffuseIntensity"].SetValue(0f);
                        Vector2 lightScreenLoc = new Vector2(go.transform.GlobalPosition.x, go.transform.GlobalPosition.y);
                        lightScreenLoc = Camera2D.main.WorldToScreen(lightScreenLoc);
                        Vector3 ls3 = new Vector3(lightScreenLoc.x, lightScreenLoc.y, go.transform.GlobalPosition.z);
                        defaultLit.Parameters["LightLocationScreen"].SetValue(ls3);
                        defaultLit.Parameters["LightColor"].SetValue(((Microsoft.Xna.Framework.Color)l.Color).ToVector4());
                        defaultLit.Parameters["LightIntensity"].SetValue(l.Intensity);
                        defaultLit.Parameters["LightRange"].SetValue(l.Range * Camera2D.main.Zoom);

                        defaultLit.Parameters["LightLength"].SetValue(l.Length * Camera2D.main.Zoom);
                        defaultLit.Parameters["LightRotation"].SetValue((l.Rotation + 180) / 180f * (float)Math.PI);
                        defaultLit.Parameters["LightConeAngle"].SetValue(l.ConeAngle / 180f * (float)Math.PI);
                        defaultLit.Parameters["LightPenumbraAngle"].SetValue(l.PenumbraAngle / 180f * (float)Math.PI);
                        defaultLit.Parameters["BackgroundColor"].SetValue(((Microsoft.Xna.Framework.Color)backgroundColor).ToVector4());

                        defaultLit.CurrentTechnique.Passes[0].Apply();
                        spriteBatch.Draw(diffuseRenderTarget, new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), Color.white);
                        spriteBatch.End();
                    }
                }
            }

        }
        #endregion
    }
}
