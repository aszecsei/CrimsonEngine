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

        public Camera2D()
        {
            Zoom = 1.0f;
            Transform.GlobalPosition = new Vector3(0, 0, -1);
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
                return Matrix.CreateTranslation(-(int)Transform.GlobalPosition.x, -(int)Transform.GlobalPosition.y, 0) *
                       Matrix.CreateRotationZ(MathHelper.ToRadians(Transform.GlobalRotation)) *
                       Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                       Matrix.CreateTranslation((Vector3)ViewportCenter);
            }
        }

        public Rectangle ViewportBounds
        {
            get
            {
                Vector2 viewportCorner = ScreenToWorld(new Vector2(0, 0));
                Vector2 viewportBottomCorner = ScreenToWorld(SceneManager.CurrentScene.InternalResolution);
                return new Rectangle((int)viewportCorner.x, (int)viewportCorner.y,
                    (int)(viewportBottomCorner.x - viewportCorner.x),
                    (int)(viewportBottomCorner.y - viewportCorner.y));
            }
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Vector2 result = Vector2.Transform(worldPosition, TranslationMatrix);
            result.y *= -1;
            return result;
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            if(SceneManager.CurrentScene.InternalResolution != Vector2.zero)
            {
                screenPosition.x *= SceneManager.CurrentScene.InternalResolution.x / SceneManager.CurrentScene.GraphicsDevice.Viewport.Width;
                screenPosition.y *= SceneManager.CurrentScene.InternalResolution.y / SceneManager.CurrentScene.GraphicsDevice.Viewport.Height;
            }
            Vector2 result = Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
            result.y *= -1;
            return result;
        }

        public Vector3 WorldToScreen(Vector3 worldPosition)
        {
            return Vector3.Transform(worldPosition, TranslationMatrix);
        }
    }
}
