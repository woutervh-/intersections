using UnityEngine;

namespace Math.Numerics
{
    public static partial class SpecialFunctions
    {
        static int g = 7;
        static float[] p = {
            0.99999999999980993f, 676.5203681218851f, -1259.1392167224028f,
            771.32342877765313f, -176.61502916214059f, 12.507343278686905f,
            -0.13857109526572012f, 9.9843695780195716e-6f, 1.5056327351493116e-7f
        };

        public static Complex32 Gamma(Complex32 z)
        {
            // Reflection formula
            if (z.Real < 0.5)
            {
                return Mathf.PI / (Complex32.Sin(Mathf.PI * z) * Gamma(1 - z));
            }
            else
            {
                z -= 1;
                Complex32 x = p[0];
                for (var i = 1; i < g + 2; i++)
                {
                    x += p[i] / (z + i);
                }
                Complex32 t = z + g + 0.5f;
                return Complex32.Sqrt(2 * Mathf.PI) * (Complex32.Pow(t, z + 0.5f)) * Complex32.Exp(-t) * x;
            }
        }
    }
}
