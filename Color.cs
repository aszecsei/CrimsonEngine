using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public class Color
    {
        Microsoft.Xna.Framework.Color _internal;

        #region Static Variables
        public static Color black { get { return new Color(0, 0, 0, 1); } }
        public static Color blue { get { return new Color(0, 0, 1, 1); } }
        public static Color clear { get { return new Color(0, 0, 0, 0); } }
        public static Color cyan { get { return new Color(0, 1, 1, 1); } }
        public static Color gray { get { return new Color(0.5f, 0.5f, 0.5f, 1); } }
        public static Color green { get { return new Color(0, 1, 0, 1); } }
        public static Color grey { get { return gray; } }
        public static Color magenta { get { return new Color(1, 0, 1, 1); } }
        public static Color red { get { return new Color(1, 0, 0, 1); } }
        public static Color white { get { return new Color(1, 1, 1, 1); } }
        public static Color yellow { get { return new Color(1, 0.92f, 0.016f, 1); } }
        #endregion

        #region Variables
        private float _r;
        private float _g;
        private float _b;
        private float _a;

        public float r
        {
            get
            {
                return _r;
            }
            set
            {
                _internal.R = (byte)Mathf.RoundToInt(_r * 255);
            }
        }

        public float g
        {
            get
            {
                return _g;
            }
            set
            {
                _internal.G = (byte)Mathf.RoundToInt(_g * 255);
            }
        }

        public float b
        {
            get
            {
                return _b;
            }
            set
            {
                _internal.B = (byte)Mathf.RoundToInt(_b * 255);
            }
        }

        public float a
        {
            get
            {
                return _a;
            }
            set
            {
                _internal.A = (byte)Mathf.RoundToInt(_a * 255);
            }
        }
        #endregion

        #region Constructors
        public Color(float r, float g, float b, float a)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
            _internal = new Microsoft.Xna.Framework.Color(r, g, b, a);
        }

        public Color(float r, float g, float b)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = 1;
            _internal = new Microsoft.Xna.Framework.Color(r, g, b, 1f);
        }
        #endregion

        #region Public Functions
        #endregion

        #region Static Functions
        public static Color Lerp(Color a, Color b, float t)
        {
            return new Color(Mathf.Lerp(a.r, b.r, t), Mathf.Lerp(a.g, b.g, t), Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
        }
        #endregion

        #region Operators
        public static implicit operator Microsoft.Xna.Framework.Color(Color c)
        {
            return c._internal;
        }
        #endregion
    }
}
