using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public class Vector3
    {
        #region Static Veriables
        public static Vector3 back
        {
            get { return new Vector3(0, 0, -1); }
        }

        public static Vector3 down
        {
            get { return new Vector3(0, -1, 0); }
        }

        public static Vector3 forward
        {
            get { return new Vector3(0, 0, 1); }
        }

        public static Vector3 left
        {
            get { return new Vector3(-1, 0, 0); }
        }

        public static Vector3 one
        {
            get { return new Vector3(1, 1, 1); }
        }

        public static Vector3 right
        {
            get { return new Vector3(1, 0, 0); }
        }

        public static Vector3 up
        {
            get { return new Vector3(0, 1, 0); }
        }

        public static Vector3 zero
        {
            get { return new Vector3(0, 0, 0); }
        }
        #endregion

        private Microsoft.Xna.Framework.Vector3 _internal;

        #region Variables
        public float magnitude
        {
            get { return _internal.Length(); }
        }

        public Vector3 normalized
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
                else if (index == 2)
                    return z;
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

        public float z
        {
            get { return _internal.Z; }
            set { _internal.Z = value; }
        }
        #endregion

        #region Constructors
        public Vector3(float x, float y, float z)
        {
            _internal = new Microsoft.Xna.Framework.Vector3(x, y, z);
        }

        private Vector3(Microsoft.Xna.Framework.Vector3 inter)
        {
            _internal = inter;
        }
        #endregion

        #region Public Functions
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return ((Vector3)obj) == this;
        }

        public override int GetHashCode()
        {
            return _internal.GetHashCode();
        }

        public void Normalize()
        {
            _internal.Normalize();
        }

        public void Set(float new_x, float new_y, float new_z)
        {
            _internal.X = new_x;
            _internal.Y = new_y;
        }

        public override string ToString()
        {
            return string.Format("Vector3({0}, {1}, {2})", x, y, z);
        }
        #endregion

        #region Static Functions
        /// <summary>
        /// Returns the angle in degrees between from and to.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float Angle(Vector3 from, Vector3 to)
        {
            return Mathf.Acos(Vector3.Dot(from.normalized, to.normalized)) * Mathf.RAD_TO_DEG;
        }

        /// <summary>
        /// Returns a copy of vector with its magnitude clamped to maxLength.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
        {
            float magSq = vector.sqrMagnitude;
            float maxSq = maxLength * maxLength;
            if(magSq < maxSq || Mathf.Approximately(magSq, maxSq))
            {
                return vector;
            }
            else
            {
                return vector.normalized * maxLength;
            }
        }

        /// <summary>
        /// Cross Product of two vectors.
        /// </summary>
        /// <remarks>
        /// The cross product of two vectors results in a third vector which is perpendicular to the two input vectors.
        /// The result's magnitude is equal to the magnitudes of the two inputs multiplied together and then multiplied
        /// by the sine of the angle between the inputs. You can determine the direction of the result vector using the "left hand rule".
        /// </remarks>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return Microsoft.Xna.Framework.Vector3.Cross(lhs, rhs);
        }

        /// <summary>
        /// Returns the distance between a and b.
        /// </summary>
        /// <remarks>
        /// Vector3.Distance(a,b) is the same as (a-b).magnitude.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Distance(Vector3 a, Vector3 b)
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
        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return Microsoft.Xna.Framework.Vector3.Dot(lhs, rhs);
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
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
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
        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(Mathf.LerpUnclamped(a.x, b.x, t), Mathf.LerpUnclamped(a.y, b.y, t), Mathf.LerpUnclamped(a.z, b.z, t));
        }

        /// <summary>
        /// Returns a vector that is made from the largest components of two vectors.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector3 Max(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
        }

        public static Vector3 Min(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
        }

        /// <summary>
        /// Moves a point current towards target.
        /// </summary>
        /// <remarks>
        /// This is essentially the same as Vector3.Lerp but instead the function will ensure that the speed never
        /// exceeds maxDistanceDelta. Negative values of maxDistanceDelta pushes the vector away from target.
        /// </remarks>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        /// <returns></returns>
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            Vector3 dist = target - current;
            float mag = dist.magnitude;
            if (mag > maxDistanceDelta)
            {
                return current + (dist * maxDistanceDelta / mag);
            }
            return target;
        }

        /// <summary>
        /// Makes vectors normalized and orthogonal to each other.
        /// </summary>
        /// <remarks>
        /// Normalizes normal. Normalizes tangent and makes sure it is orthogonal to normal (that is, angle between them is 90 degrees).
        /// </remarks>
        /// <param name="normal"></param>
        /// <param name="tangent"></param>
        public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent)
        {
            tangent -= Vector3.Project(tangent, normal);
            normal.Normalize();
            tangent.Normalize();
        }

        /// <summary>
        /// Makes vectors normalized and orthogonal to each other.
        /// </summary>
        /// <remarks>
        /// Normalizes normal. Normalizes tangent and makes sure it is orthogonal to normal.
        /// Normalizes binormal and makes sure it is orthogonal to both normal and tangent.
        /// 
        /// Points in space are usually specified with coordinates in the standard XYZ axis system. However, you can interpret any three
        /// vectors as "axes" if they are normalized (ie, have a magnitude of 1) and are orthogonal (ie, perpendicular to each other).
        /// 
        /// Creating your own coordinate axes is useful, say, if you want to scale a mesh in arbitrary directions rather than just along the
        /// XYZ axes - you can transform the vertices to your own coordinate system, scale them and then transform back. Often, a transformation
        /// like this will be carried out along only one axis while the other two are either left as they are or treated equally. For example, a
        /// stretching effect can be applied to a mesh by scaling up on one axis while scaling down proportionally on the other two. This means
        /// that once the first axis vector is specified, it doesn't greatly matter what the other two are as long as they are normalized and orthogonal.
        /// OrthoNormalize can be used to ensure the first vector is normal and then generate two normalized, orthogonal vectors for the other two axes.
        /// </remarks>
        /// <param name="normal"></param>
        /// <param name="tangent"></param>
        /// <param name="binormal"></param>
        public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent, ref Vector3 binormal)
        {
            tangent -= Vector3.Project(tangent, normal);
            binormal -= Vector3.Project(binormal, normal);
            binormal -= Vector3.Project(binormal, tangent);
            normal.Normalize();
            tangent.Normalize();
            binormal.Normalize();
        }

        /// <summary>
        /// Projects a vector onto another vector.
        /// </summary>
        /// <remarks>
        /// To understand vector projection, imagine that onNormal is resting on a line pointing in its direction. Somewhere along that line will be the nearest
        /// point to the tip of vector. The projection is just onNormal rescaled so that it reaches that point on the line.
        /// 
        /// The function will return a zero vector if onNormal is almost zero.
        /// 
        /// An example of the usage of projection is a rail-mounted gun that should slide so that it gets as close as possible to a target object. The projection
        /// of the target heading along the direction of the rail can be used to move the gun by applying a force to a rigidbody, say.
        /// </remarks>
        /// <param name="vector"></param>
        /// <param name="onNormal"></param>
        /// <returns></returns>
        public static Vector3 Project(Vector3 vector, Vector3 onNormal)
        {
            if (onNormal == Vector3.zero)
                return Vector3.zero;

            return Vector3.Dot(vector, onNormal) * onNormal / onNormal.sqrMagnitude;
        }

        /// <summary>
        /// Projects a vector onto a plane defined by a normal orthogonal to the plane.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
        public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
        {
            if (planeNormal == Vector3.zero)
                throw new Exception("The zero vector cannot be used to define a plane. Take a linear algebra course.");

            // Generate a basis
            Vector3 b1 = Vector3.zero;
            Vector3 b2 = Vector3.zero;
            if (!Mathf.Approximately(planeNormal.x, 0f))
            {
                b1 = new Vector3(-1f * planeNormal.y, planeNormal.x, 0f);
                b2 = new Vector3(-1f * planeNormal.z, 0f, planeNormal.x);
                b2 -= Vector3.Project(b2, b1);
            }
            else if(!Mathf.Approximately(planeNormal.y, 0f))
            {
                b1 = new Vector3(1, 0, 0);
                b2 = new Vector3(0, -planeNormal.z, planeNormal.y);
            }
            else if(!Mathf.Approximately(planeNormal.z, 0f))
            {
                b1 = new Vector3(1, 0, 0);
                b2 = new Vector3(0, 1, 0);
            }

            return Vector3.Project(vector, b1) + Vector3.Project(vector, b2);
        }

        /// <summary>
        /// Reflects a vector off the plane defined by a normal.
        /// </summary>
        /// <remarks>
        /// The inNormal vector defines a plane (a plane's normal is the vector that is perpendicular to its surface). the inDirection vector is treated as a directional
        /// arrow coming in to the plane. The returned value is a vector of equal magnitude to inDirection but with its direction reflected.
        /// </remarks>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        /// <returns></returns>
        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
        {
            return inDirection - (2 * Vector3.Dot(inDirection, inNormal) / inNormal.sqrMagnitude) * inNormal;
        }

        /// <summary>
        /// Rotates a vector current towards target.
        /// </summary>
        /// <remarks>
        /// This function is similar to MoveTowards except that the vector is treated as a direction rather than a position. The current vector will be rotated round
        /// toward the target direction by an angle of maxRadiansDelta, although it will land exactly on the target rather than overshoot. If the magnitudes of current
        /// and target are different then the magnitude of the result will be linearly interpolated during the rotation. If a negative value is used for maxRadiansDelta,
        /// the vector will rotate away from target/ until it is pointing in exactly the opposite direction, then stop.
        /// </remarks>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxRadiansDelta"></param>
        /// <param name="maxMagnitudeDelta"></param>
        /// <returns></returns>
        public static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta)
        {
            // TODO: Implement this
            throw new NotImplementedException();
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
        public static Vector3 Scale(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        /// Spherically interpolates between two vectors.
        /// </summary>
        /// <remarks>
        /// Interpolates between a and b by amount t. The difference between this and linear interpolation (aka, "lerp") is that the vectors are treated as directions rather
        /// than points in space. The direction of the returned vector is interpolated by the angle and its magnitude is interpolated between the magnitudes of from and to.
        /// 
        /// The parameter t is clamped to the range [0, 1].
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
        {
            // TODO: Implement this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Spherically interpolates between two vectors.
        /// </summary>
        /// <remarks>
        /// Interpolates between a and b by amount t. The difference between this and linear interpolation (aka, "lerp") is that the vectors are treated as directions rather
        /// than points in space. The direction of the returned vector is interpolated by the angle and its magnitude is interpolated between the magnitudes of from and to.
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Vector3 SlerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gradually changes a vector towards a desired goal over time.
        /// </summary>
        /// <remarks>
        /// The vector is smoothed by some spring-damper like function, which will never overshoot.
        /// The most common use is for smoothing a follow camera.
        /// </remarks>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="currentVelocity"></param>
        /// <param name="smoothTime"></param>
        /// <param name="deltaTime"></param>
        /// <param name="maxSpeed"></param>
        /// <returns></returns>
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float deltaTime, float maxSpeed = Mathf.INFINITY)
        {
            float ySpeed = currentVelocity.y;
            float xSpeed = currentVelocity.x;
            float zSpeed = currentVelocity.z;
            Vector3 result = new Vector3(Mathf.SmoothDamp(current.x, target.x, ref xSpeed, smoothTime, maxSpeed, deltaTime), Mathf.SmoothDamp(current.y, target.y, ref ySpeed, smoothTime, maxSpeed, deltaTime), Mathf.SmoothDamp(current.z, target.z, ref zSpeed, smoothTime, maxSpeed, deltaTime));
            currentVelocity.x = xSpeed;
            currentVelocity.y = ySpeed;
            currentVelocity.z = zSpeed;
            return result;
        }

        internal static Vector3 Transform(Vector3 value, Microsoft.Xna.Framework.Matrix matrix)
        {
            return Microsoft.Xna.Framework.Vector3.Transform(value, matrix);
        }
        #endregion

        #region Operators
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-1 * a.x, -1 * a.y, -1 * a.z);
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !Mathf.Approximately(lhs.x, rhs.x) || !Mathf.Approximately(lhs.y, rhs.y) || !Mathf.Approximately(lhs.z, rhs.z);
        }

        public static Vector3 operator *(Vector3 a, float d)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator *(float d, Vector3 a)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator /(Vector3 a, float d)
        {
            return new Vector3(a.x / d, a.y / d, a.z / d);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return Mathf.Approximately(lhs.x, rhs.x) && Mathf.Approximately(lhs.y, rhs.y) && Mathf.Approximately(lhs.z, rhs.z);
        }

        public static implicit operator Microsoft.Xna.Framework.Vector3(Vector3 v3)
        {
            return v3._internal;
        }

        public static implicit operator Vector3(Microsoft.Xna.Framework.Vector3 v3)
        {
            return new Vector3(v3);
        }
        #endregion
    }
}
