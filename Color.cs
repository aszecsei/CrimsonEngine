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
        
        #endregion

        #region Constructors
        public Color(float r, float g, float b, float a)
        {
            _internal = new Microsoft.Xna.Framework.Color(r, g, b, a);
        }

        public Color(float r, float g, float b)
        {
            _internal = new Microsoft.Xna.Framework.Color(r, g, b);
        }

        private Color(Microsoft.Xna.Framework.Color color)
        {
            _internal = color;
        }
        #endregion

        #region Public Functions
        #endregion

        #region Static Functions
        #endregion

        #region Operators
        public static implicit operator Microsoft.Xna.Framework.Color(Color c)
        {
            return c._internal;
        }

        public static implicit operator Color(Microsoft.Xna.Framework.Color c)
        {
            return new Color(c);
        }
        #endregion
    }
}
