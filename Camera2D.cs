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

        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public static Camera2D main;

        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
            }
        }

        public Camera2D()
        {
            Zoom = 1.0f;
        }

        public void ZoomToFit(int width, int height)
        {
            float zoomW = (float)ViewportWidth / (float)width;
            float zoomH = (float)ViewportHeight / (float)height;

            Zoom = Math.Min(zoomW, zoomH);
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Transform.GlobalPosition.X, -(int)Transform.GlobalPosition.Y, 0) *
                       Matrix.CreateRotationZ(MathHelper.ToRadians(Transform.GlobalRotation)) *
                       Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                       Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public Rectangle ViewportBounds
        {
            get
            {
                Vector2 viewportCorner = ScreenToWorld(new Vector2(0, 0));
                Vector2 viewportBottomCorner = ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));
                return new Rectangle((int)viewportCorner.X, (int)viewportCorner.Y,
                    (int)(viewportBottomCorner.X - viewportCorner.X),
                    (int)(viewportBottomCorner.Y - viewportCorner.Y));
            }
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }
    }
}
