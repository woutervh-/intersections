using UnityEngine;

namespace Math.Numerics
{
    public static partial class SpecialFunctions
    {
        /// <summary>
        /// Numerically stable hypotenuse of a right angle triangle, i.e. <code>(a,b) -> sqrt(a^2 + b^2)</code>
        /// </summary>
        /// <param name="a">The length of side a of the triangle.</param>
        /// <param name="b">The length of side b of the triangle.</param>
        /// <returns>Returns <code>sqrt(a<sup>2</sup> + b<sup>2</sup>)</code> without underflow/overflow.</returns>
        public static Complex32 Hypotenuse(Complex32 a, Complex32 b)
        {
            if (a.Magnitude > b.Magnitude)
            {
                var r = b.Magnitude / a.Magnitude;
                return a.Magnitude * Mathf.Sqrt(1 + (r * r));
            }

            if (b != 0.0f)
            {
                // NOTE (ruegg): not "!b.AlmostZero()" to avoid convergence issues (e.g. in SVD algorithm)
                var r = a.Magnitude / b.Magnitude;
                return b.Magnitude * Mathf.Sqrt(1 + (r * r));
            }

            return 0f;
        }

        /// <summary>
        /// Numerically stable hypotenuse of a right angle triangle, i.e. <code>(a,b) -> sqrt(a^2 + b^2)</code>
        /// </summary>
        /// <param name="a">The length of side a of the triangle.</param>
        /// <param name="b">The length of side b of the triangle.</param>
        /// <returns>Returns <code>sqrt(a<sup>2</sup> + b<sup>2</sup>)</code> without underflow/overflow.</returns>
        public static float Hypotenuse(float a, float b)
        {
            if (Mathf.Abs(a) > Mathf.Abs(b))
            {
                float r = b / a;
                return Mathf.Abs(a) * Mathf.Sqrt(1 + (r * r));
            }

            if (b != 0.0)
            {
                // NOTE (ruegg): not "!b.AlmostZero()" to avoid convergence issues (e.g. in SVD algorithm)
                float r = a / b;
                return Mathf.Abs(b) * Mathf.Sqrt(1 + (r * r));
            }

            return 0f;
        }
    }
}
