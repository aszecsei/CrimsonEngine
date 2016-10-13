using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CrimsonEngine
{
    public class Vector2
    {
        #region Static Veriables
        public static Vector2 down
        {
            get { return new Vector2(0, -1); }
        }

        public static Vector2 left
        {
            get { return new Vector2(-1, 0); }
        }

        public static Vector2 one
        {
            get { return new Vector2(1, 1);  }
        }

        public static Vector2 right
        {
            get { return new Vector2(1, 0); }
        }

        public static Vector2 up
        {
            get { return new Vector2(0, 1); }
        }

        public static Vector2 zero
        {
            get { return new Vector2(0, 0); }
        }
        #endregion

        private Microsoft.Xna.Framework.Vector2 _internal;

        #region Variables
        public float magnitude
        {
            get { return _internal.Length(); }
        }

        public Vector2 normalized
        {
            get
            {
                return this / this.magnitude;
            }
        }

        public float sqrMagnitude { get { return _internal.LengthSquared(); } }

        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return x;
                else if (index == 1)
                    return y;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public float x
        {
            get
            {
                return _internal.X;
            }
            set
            {
                _internal.X = value;
            }
        }

        public float y
        {
            get
            {
                return _internal.Y;
            }
            set
            {
                _internal.Y = value;
            }
        }
        #endregion

        #region Constructors
        public Vector2(float x, float y)
        {
            _internal = new Microsoft.Xna.Framework.Vector2(x, y);
        }

        private Vector2(Microsoft.Xna.Framework.Vector2 inter)
        {
            _internal = inter;
        }
        #endregion

        #region Public Functions
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return ((Vector2)obj) == this;
        }

        public override int GetHashCode()
        {
            return _internal.GetHashCode();
        }

        public void Normalize()
        {
            _internal.Normalize();
        }

        public void Set(float new_x, float new_y)
        {
            _internal.X = new_x;
            _internal.Y = new_y;
        }

        public override string ToString()
        {
            return string.Format("Vector2({0}, {1})", x, y);
        }
        #endregion

        #region Static Functions
        /// <summary>
        /// Returns the angle in degrees between from and to.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Angle(Vector2 from, Vector2 to)
        {
            return Mathf.Acos(Vector2.Dot(from.normalized, to.normalized)) * Mathf.RAD_TO_DEG;
        }

        /// <summary>
        /// Returns a copy of vector with its magnitude clamped to maxLength.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            float magSq = vector.sqrMagnitude;
            float maxSq = maxLength * maxLength;
            if (magSq < maxSq || Mathf.Approximately(magSq, maxSq))
            {
                return vector;
            }
            else
            {
                return vector.normalized * maxLength;
            }
        }

        /// <summary>
        /// Returns the distance between a and b.
        /// </summary>
        /// <remarks>
        /// Vector2.Distance(a,b) is the same as (a-b).magnitude.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance(Vector2 a, Vector2 b)
        {
            return (a - b).magnitude;
        }

        /// <summary>
        /// Dot Product of two vectors.
        /// </summary>
        /// <remarks>
        /// Returns lhs . rhs.
        /// 
        /// For normalized vectors Dot returns 1 if they point in exactly the same direction;
        /// -1 if they point in completely opposite directions; and a number in between for other
        /// cases (e.g. Dot returns zero if vectors are perpendicular).
        /// 
        /// For vectors of arbitrary length the Dot return values are similar: they get larger when
        /// the angle between vectors decreases.
        /// </remarks>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static float Dot(Vector2 lhs, Vector2 rhs)
        {
            return Microsoft.Xna.Framework.Vector2.Dot(lhs, rhs);
        }

        /// <summary>
        /// Linearly interpolates between vectors a and b by t.
        /// </summary>
        /// <remarks>
        /// The parameter t is clamped to the range [0, 1].
        /// 
        /// When t = 0 returns a. 
        /// When t = 1 return b. 
        /// When t = 0.5 returns the midpoint of a and b.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t));
        }

        /// <summary>
        /// Linearly interpolates between vectors a and b by t.
        /// </summary>
        /// <remarks>
        /// When t = 0 returns a. 
        /// When t = 1 return b. 
        /// When t = 0.5 returns the midpoint of a and b.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(Mathf.LerpUnclamped(a.x, b.x, t), Mathf.LerpUnclamped(a.y, b.y, t));
        }

        /// <summary>
        /// Returns a vector that is made from the largest components of two vectors.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector2 Max(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
        }

        public static Vector2 Min(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
        }

        /// <summary>
        /// Moves a point current towards target.
        /// </summary>
        /// <remarks>
        /// This is essentially the same as Vector2.Lerp but instead the function will ensure that the speed never
        /// exceeds maxDistanceDelta. Negative values of maxDistanceDelta pushes the vector away from target.
        /// </remarks>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            Vector2 dist = target - current;
            float mag = dist.magnitude;
            if(mag > maxDistanceDelta)
            {
                return current + (dist * maxDistanceDelta / mag);
            }
            return target;
        }

        /// <summary>
        /// Reflects a vector off the vector defined by a normal.
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        /// <returns></returns>
        public static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal)
        {
            return inDirection - (2 * Vector2.Dot(inDirection, inNormal) / inNormal.sqrMagnitude) * inNormal;
        }

        /// <summary>
        /// Rotates a vector about an origin.
        /// </summary>
        /// <param name="point">The point to rotate.</param>
        /// <param name="origin">The origin to rotate about.</param>
        /// <param name="rotation">The rotation, in radians.</param>
        /// <returns></returns>
        public static Vector2 Rotate(Vector2 point, Vector2 origin, float rotation)
        {
            Vector2 centered = point - origin;
            return new Vector2(centered.x * (float)Math.Cos(rotation) - centered.y * (float)Math.Sin(rotation), centered.x * (float)Math.Sin(rotation) + centered.y * (float)Math.Cos(rotation)) + origin;
        }

        /// <summary>
        /// Multiplies two vectors component-wise.
        /// </summary>
        /// <remarks>
        /// Every component in the result is a component of a multiplied by the same component of b.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 Scale(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        /// <summary>
        /// Gradually changes a vector towards a desired goal over time.
        /// </summary>
        /// <remarks>
        /// The vector is smoothed by some spring-damper like function, which will never overshoot.
        /// </remarks>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <param name="deltaTime"></param>
        /// <param name="maxSpeed"></param>
        /// <returns></returns>
        public static Vector2 SmoothDamp(Vector2 current, Vector2 target, ref Vector2 currentVelocity, float smoothTime, float deltaTime, float maxSpeed = Mathf.INFINITY)
        {
            float ySpeed = currentVelocity.y;
            float xSpeed = currentVelocity.x;
            Vector2 result = new Vector2(Mathf.SmoothDamp(current.x, target.x, ref xSpeed, smoothTime, maxSpeed, deltaTime), Mathf.SmoothDamp(current.y, target.y, ref ySpeed, smoothTime, maxSpeed, deltaTime));
            currentVelocity.x = xSpeed;
            currentVelocity.y = ySpeed;
            return result;
        }

        internal static Vector2 Transform(Vector2 value, Microsoft.Xna.Framework.Matrix matrix)
        {
            return Microsoft.Xna.Framework.Vector2.Transform(value, matrix);
        }
        #endregion

        #region Operators
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-1 * a.x, -1 * a.y);
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !Mathf.Approximately(lhs.x, rhs.x) || !Mathf.Approximately(lhs.y, rhs.y);
        }

        public static Vector2 operator *(Vector2 a, float d)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator *(float d, Vector2 a)
        {
            return new Vector2(a.x * d, a.y * d);
        }

        public static Vector2 operator /(Vector2 a, float d)
        {
            return new Vector2(a.x / d, a.y / d);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return Mathf.Approximately(lhs.x, rhs.x) && Mathf.Approximately(lhs.y, rhs.y);
        }

        public static implicit operator Vector2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public static implicit operator Vector3(Vector2 v2)
        {
            return new Vector3(v2.x, v2.y, 0);
        }

        public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2 v2)
        {
            return v2._internal;
        }

        public static implicit operator Vector2(Microsoft.Xna.Framework.Vector2 v2)
        {
            return new Vector2(v2);
        }
        #endregion
    }
}
