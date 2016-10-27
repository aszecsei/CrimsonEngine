using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimsonEngine
{
    public static class Mathf
    {
        public const float PI = (float)Math.PI;
        public const float DEG_TO_RAD = PI / 180f;
        public const float RAD_TO_DEG = 180f / PI;
        public const float INFINITY = float.MaxValue;
        public const float NEGATIVE_INFINITY = float.MinValue;
        public const float EPSILON = float.Epsilon;

        public static float tolerance = 0.00001f;

        /// <summary>
        /// Returns the absolute value of f
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Abs(float f)
        {
            return Math.Abs(f);
        }

        /// <summary>
        /// Returns the arc-cosine of f - the angle in radians whose cosine is f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Acos(float f)
        {
            return (float)Math.Acos(f);
        }

        /// <summary>
        /// Compares two floating point values if they are similar.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Approximately(float a, float b)
        {
            return Mathf.Abs(a - b) < tolerance;
        }

        /// <summary>
        /// Returns the arc-sine of f - the angle in radians whose sine is f.
        /// </summary>
        /// <param name="f"></param>
        public static float Asin(float f)
        {
            return (float)Math.Asin(f);
        }

        /// <summary>
        /// Returns the angle in radians whose Tan is y/x.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        /// <summary>
        /// Returns the smallest integer greater to or equal to f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Ceil(float f)
        {
            return (float)Math.Ceiling(f);
        }

        /// <summary>
        /// Returns the smallest integer greater to or equal to f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int CeilToInt(float f)
        {
            return (int)Math.Ceiling(f);
        }

        /// <summary>
        /// Clamps a value between a minimum float and maximum float value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            return Mathf.Min(Mathf.Max(value, min), max);
        }

        /// <summary>
        /// Clamps value between 0 and 1 and returns value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Clamp01(float value)
        {
            return Clamp(value, 0f, 1f);
        }

        /// <summary>
        /// Returns the closest power of two value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ClosestPowerOfTwo(int value)
        {
            float log = Log(value, 2);
            int logFloor = FloorToInt(log);
            int logCeil = CeilToInt(log);
            float lower = Pow(2, logFloor);
            float highter = Pow(2, logCeil);
            if (value - lower < highter - value)
                return (int)lower;
            else
                return (int)highter;
        }

        /// <summary>
        /// Returns the cosine of angle f in radians.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Cos(float f)
        {
            return (float)Math.Cos(f);
        }

        /// <summary>
        /// Calculates the shortest difference between two given angles given in degrees.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float DeltaAngle(float current, float target)
        {
            float v1 = target - current;
            while (v1 > 180)
                v1 -= 360;
            while (v1 < -180)
                v1 += 360;

            return v1;
        }

        /// <summary>
        /// Returns e raised 
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        public static float Exp(float power)
        {
            return (float)Math.Exp(power);
        }

        /// <summary>
        /// Returns the largest integer smaller to or equal to f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Floor(float f)
        {
            return (float)Math.Floor(f);
        }

        /// <summary>
        /// Returns the largest integer smaller to or equal to f.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int FloorToInt(float f)
        {
            return (int)Math.Floor(f);
        }

        /// <summary>
        /// Calculates the linear parameter t that produces the interpolant value within the range [a, b].
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float InverseLerp(float a, float b, float value)
        {
            if (Approximately(b - a, 0f))
                throw new DivideByZeroException();
            return (value - a) / (b - a);
        }

        /// <summary>
        /// Returns true if the value is power of two.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPowerOfTwo(int value)
        {
            return Approximately(Mathf.Pow(Mathf.FloorToInt(Mathf.Log(value, 2)), 2), value);
        }

        /// <summary>
        /// Linearly interpolates between a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float t)
        {
            t = Mathf.Clamp01(t);
            return (1 - t) * a + t * b;
        }

        /// <summary>
        /// Same as Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float LerpAngle(float a, float b, float t)
        {
            while (a > b)
                a -= 360;
            float result = Lerp(a, b, t);
            while (result > 360)
                result -= 360;
            while (result < 0)
                result += 360;
            return result;
        }

        /// <summary>
        /// Linearly interpolates between a and b by t with no limit to t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float LerpUnclamped(float a, float b, float t)
        {
            return (1 - t) * a + t * b;
        }

        public static float Log(float f, float p)
        {
            return (float)Math.Log(f, p);
        }

        public static float Log(float f)
        {
            return (float)Math.Log(f);
        }

        public static float Log10(float f)
        {
            return (float)Math.Log10(f);
        }

        public static float Max(float a, float b)
        {
            return (float)Math.Max(a, b);
        }

        public static float Min(float a, float b)
        {
            return (float)Math.Min(a, b);
        }

        public static float Pow(float a, float b)
        {
            return (float)Math.Pow(a, b);
        }

        public static float Pow(float f)
        {
            return f * f;
        }

        public static float Round(float value)
        {
            return (float)Math.Round(value);
        }

        public static int RoundToInt(float value)
        {
            return (int)Math.Round(value);
        }

        public static float Sign(float f)
        {
            return (float)Math.Sign(f);
        }

        public static float Sin(float f)
        {
            return (float)Math.Sin(f);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float deltaTime, float maxSpeed = Mathf.INFINITY)
        {
            float d = current;
            float c = currentVelocity;
            float b = (3f * target - 2 * currentVelocity * smoothTime - 3 * current) / (-1f * Mathf.Pow(smoothTime));
            float a = (2 * target - currentVelocity * smoothTime - 2 * current) / (-1f * Mathf.Pow(smoothTime, 3));

            float posNext = a * Mathf.Pow(deltaTime, 3) + b * Mathf.Pow(deltaTime) + c * deltaTime + d;
            posNext = Mathf.Min(current + maxSpeed * deltaTime, posNext);
            float vNext = 3 * a * Mathf.Pow(deltaTime) + 2 * b * deltaTime + c;
            vNext = Mathf.Min(maxSpeed, vNext);
            currentVelocity = vNext;
            return posNext;
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }

        /// <summary>
        /// Returns the tangent of angle f in radians.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Tan(float value)
        {
            return (float)Math.Tan(value);
        }
    }
}
