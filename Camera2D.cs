using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine
{
    public class Camera2D : GameObject
    {
        public float Zoom { get; set; }

        public static Camera2D main;

        public Vector2 ViewportCenter
        {
            get
            {
                return (SceneManager.CurrentScene.InternalResolution != Vector2.zero ? SceneManager.CurrentScene.InternalResolution * 0.5f : new Vector2(SceneManager.CurrentScene.GraphicsDevice.Viewport.Width, SceneManager.CurrentScene.GraphicsDevice.Viewport.Height) * 0.5f);
            }
        }

        public Physics.Bounds CameraBounds
        {
            get
            {
                var tl = ScreenToWorld(Vector2.zero);
                var tr = ScreenToWorld(new Vector2(SceneManager.CurrentScene.GraphicsDevice.Viewport.Width, 0));
                var bl = ScreenToWorld(new Vector2(0, SceneManager.CurrentScene.GraphicsDevice.Viewport.Height));
                var br = ScreenToWorld(new Vector2(SceneManager.CurrentScene.GraphicsDevice.Viewport.Width, SceneManager.CurrentScene.GraphicsDevice.Viewport.Height));
                var min = new Vector2(
                    MathHelper.Min(tl.x, MathHelper.Min(tr.x, MathHelper.Min(bl.x, br.x))),
                    MathHelper.Min(tl.y, MathHelper.Min(tr.y, MathHelper.Min(bl.y, br.y))));
                var max = new Vector2(
                    MathHelper.Max(tl.x, MathHelper.Max(tr.x, MathHelper.Max(bl.x, br.x))),
                    MathHelper.Max(tl.y, MathHelper.Max(tr.y, MathHelper.Max(bl.y, br.y))));
                return new Physics.Bounds(max.y, min.x, min.y, max.x);
            }
        }

        public Camera2D() : base()
        {
            Zoom = 1.0f;
            transform.GlobalPosition = new Vector3(0, 0, -1);
        }

        public void ZoomToFit(int width, int height)
        {
            float zoomW = (SceneManager.CurrentScene.InternalResolution != Vector2.zero ? SceneManager.CurrentScene.InternalResolution.x : SceneManager.CurrentScene.GraphicsDevice.Viewport.Width) / (float)width;
            float zoomH = (SceneManager.CurrentScene.InternalResolution != Vector2.zero ? SceneManager.CurrentScene.InternalResolution.y : SceneManager.CurrentScene.GraphicsDevice.Viewport.Height) / (float)height;

            Zoom = Math.Min(zoomW, zoomH);
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-transform.GlobalPosition.x, transform.GlobalPosition.y, 0) *
                       Matrix.CreateRotationZ(MathHelper.ToRadians(transform.GlobalRotation)) *
                       Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                       Matrix.CreateTranslation((Vector3)ViewportCenter);
            }
        }

        public Matrix InverseTranslationMatrix { get { return Matrix.Invert(TranslationMatrix); } }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Vector2 newWorld = Vector2.Scale(Vector2.downRight, worldPosition);
            return Vector2.Transform(newWorld, TranslationMatrix);
        }

        public Vector3 WorldToScreen(Vector3 worldPosition)
        {
            Vector3 newWorld = new Vector3(worldPosition.x, worldPosition.y * -1, worldPosition.z);
            return Vector3.Transform(newWorld, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Scale(Vector2.downRight, Vector2.Transform(screenPosition, InverseTranslationMatrix));
        }
    }
}
