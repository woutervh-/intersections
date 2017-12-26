using UnityEngine;

namespace Math.Numerics
{
    /// <summary>
    /// Double-precision trigonometry toolkit.
    /// </summary>
    public static class Trigonometry
    {
        /// <summary>
        /// Constant to convert a degree to grad.
        /// </summary>
        private const double DegreeToGradConstant = 10.0 / 9.0;

        /// <summary>
        /// Converts a degree (360-periodic) angle to a grad (400-periodic) angle.
        /// </summary>
        /// <param name="degree">The degree to convert.</param>
        /// <returns>The converted grad angle.</returns>
        public static double DegreeToGrad(double degree)
        {
            return degree * DegreeToGradConstant;
        }
        
        /// <summary>
        /// Trigonometric Sine of a <c>Complex</c> number.
        /// </summary>
        /// <param name="value">The complex value.</param>
        /// <returns>The sine of the complex number.</returns>
        public static Complex32 Sin(this Complex32 value)
        {
            if (value.IsReal())
            {
                return new Complex32(Mathf.Sin(value.Real), 0.0f);
            }

            return new Complex32(
                Mathf.Sin(value.Real) * (float)System.Math.Cosh(value.Imaginary),
                Mathf.Cos(value.Real) * (float)System.Math.Sinh(value.Imaginary));
        }

        /// <summary>
        /// Trigonometric Cosine of a <c>Complex</c> number.
        /// </summary>
        /// <param name="value">The complex value.</param>
        /// <returns>The cosine of a complex number.</returns>
        public static Complex32 Cos(this Complex32 value)
        {
            if (value.IsReal())
            {
                return new Complex32(Mathf.Cos(value.Real), 0.0f);
            }

            return new Complex32(
                Mathf.Cos(value.Real) * (float)System.Math.Cosh(value.Imaginary),
                -Mathf.Sin(value.Real) * (float)System.Math.Sinh(value.Imaginary));
        }

        /// <summary>
        /// Trigonometric Tangent of a <c>Complex</c> number.
        /// </summary>
        /// <param name="value">The complex value.</param>
        /// <returns>The tangent of the complex number.</returns>
        public static Complex32 Tan(this Complex32 value)
        {
            if (value.IsReal())
            {
                return new Complex32(Mathf.Tan(value.Real), 0.0f);
            }

            var cosr = Mathf.Cos(value.Real);
            var sinhi = (float)System.Math.Sinh(value.Imaginary);
            var denom = (cosr * cosr) + (sinhi * sinhi);

            return new Complex32(Mathf.Sin(value.Real) * cosr / denom, sinhi * (float)System.Math.Cosh(value.Imaginary) / denom);
        }
    }
}
