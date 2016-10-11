using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrimsonEngine
{
    abstract class DebugShape
    {
        protected virtual Texture2D BlankTexture()
        {
            return ResourceManager.GetResource<Texture2D>("Pixel");
        }

        protected virtual void DrawPolygon(SpriteBatch spriteBatch, Vector2[] vertex, int count, Color color, int lineWidth)
        {
            if(count > 0)
            {
                for(int i=0; i<count-1; i++)
                {
                    DrawLineSegment(spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
                }
                DrawLineSegment(spriteBatch, vertex[count - 1], vertex[0], color, lineWidth);
            }
        }

        protected virtual void DrawLineSegment(SpriteBatch spriteBatch, Vector2 from, Vector2 to, Color color, int lineWidth)
        {
            // translate to screen-space
            Vector2 screenFrom = from;
            screenFrom.Y *= -1;
            screenFrom = Vector2.Transform(screenFrom, Camera2D.main.TranslationMatrix);
            Vector2 screenTo = to;
            screenTo.Y *= -1;
            screenTo = Vector2.Transform(screenTo, Camera2D.main.TranslationMatrix);

            float angle = (float)Math.Atan2(screenFrom.Y - screenTo.Y, screenFrom.X - screenTo.X);
            float length = Vector2.Distance(screenFrom, screenTo);
            spriteBatch.Draw(BlankTexture(), screenTo, null, color, angle, Vector2.Zero, new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }

    class DebugRect : DebugShape
    {
        public Vector2 center;
        public Vector2 size;
        public Color color;
        public int lineWidth;

        private float Left
        {
            get
            {
                return center.X - (size.X / 2);
            }
        }

        private float Right
        {
            get
            {
                return center.X + (size.X / 2);
            }
        }

        private float Top
        {
            get
            {
                return center.Y + (size.Y / 2);
            }
        }

        private float Bottom
        {
            get
            {
                return center.Y - (size.Y / 2);
            }
        }

        public DebugRect(Vector2 center, Vector2 size, Color color, int lineWidth)
        {
            this.center = center;
            this.size = size;
            this.color = color;
            this.lineWidth = lineWidth;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2[] vertex = new Vector2[4];
            vertex[0] = new Vector2(Left, Top);
            vertex[1] = new Vector2(Right, Top);
            vertex[2] = new Vector2(Right, Bottom);
            vertex[3] = new Vector2(Left, Bottom);

            DrawPolygon(spriteBatch, vertex, 4, color, lineWidth);
        }
    }

    class DebugCircle : DebugShape
    {
        public Vector2 center;
        public float radius;
        Color color;
        int lineWidth;
        int numVerts;

        public DebugCircle(Vector2 center, float radius, Color color, int lineWidth, int numVerts)
        {
            this.center = center;
            this.radius = radius;
            this.color = color;
            this.lineWidth = lineWidth;
            this.numVerts = numVerts;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2[] vertex = new Vector2[numVerts];
            double increment = Math.PI * 2.0 / numVerts;
            double theta = 0.0;

            for(int i=0; i<numVerts; i++)
            {
                vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                theta += increment;
            }

            DrawPolygon(spriteBatch, vertex, numVerts, color, lineWidth);
        }
    }

    class DebugLine : DebugShape
    {
        public Vector2 from;
        public Vector2 to;
        public Color color;
        public int lineWidth;

        public DebugLine(Vector2 from, Vector2 to, Color color, int lineWidth)
        {
            this.from = from;
            this.to = to;
            this.color = color;
            this.lineWidth = lineWidth;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawLineSegment(spriteBatch, from, to, color, lineWidth);
        }
    }

    class DebugPolygon : DebugShape
    {
        public Vector2[] verts;
        public Color color;
        public int lineWidth;

        public DebugPolygon(Vector2[] verts, Color color, int lineWidth)
        {
            this.verts = verts;
            this.color = color;
            this.lineWidth = lineWidth;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawPolygon(spriteBatch, verts, verts.Length, color, lineWidth);
        }
    }

    public class Debug
    {
        private static List<DebugShape> toDraw = new List<DebugShape>();

        public static void DrawDebugRectangle(Vector2 center, Vector2 size, Color color, int lineWidth)
        {
            toDraw.Add(new DebugRect(center, size, color, lineWidth));
        }

        public static void DrawDebugCircle(Vector2 center, float radius, Color color, int lineWidth, int numVerts = 16)
        {
            toDraw.Add(new DebugCircle(center, radius, color, lineWidth, numVerts));
        }

        public static void DrawDebugLine(Vector2 from, Vector2 to, Color color, int lineWidth)
        {
            toDraw.Add(new DebugLine(from, to, color, lineWidth));
        }

        public static void DrawDebugPolygon(Vector2[] verts, Color color, int lineWidth)
        {
            toDraw.Add(new DebugPolygon(verts, color, lineWidth));
        }

        internal static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach(DebugShape ds in toDraw)
            {
                ds.Draw(spriteBatch, gameTime);
            }
            spriteBatch.End();
            toDraw.Clear();
        }
    }
}
