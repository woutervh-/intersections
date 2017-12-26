using UnityEngine;

namespace Math.Numerics
{
    public struct Complex32
    {
        /// <summary>
        /// The real component of the complex number.
        /// </summary>
        private readonly float _real;

        /// <summary>
        /// The imaginary component of the complex number.
        /// </summary>
        private readonly float _imag;

        /// <summary>
        /// Initializes a new instance of the Complex32 structure with the given real
        /// and imaginary parts.
        /// </summary>
        /// <param name="real">The value for the real component.</param>
        /// <param name="imaginary">The value for the imaginary component.</param>
        public Complex32(float real, float imaginary)
        {
            _real = real;
            _imag = imaginary;
        }

        /// <summary>
        /// Creates a complex number from a point's polar coordinates.
        /// </summary>
        /// <returns>A complex number.</returns>
        /// <param name="magnitude">The magnitude, which is the distance from the origin (the intersection of the x-axis and the y-axis) to the number.</param>
        /// <param name="phase">The phase, which is the angle from the line to the horizontal axis, measured in radians.</param>
        public static Complex32 FromPolarCoordinates(float magnitude, float phase)
        {
            return new Complex32(magnitude * Mathf.Cos(phase), magnitude * Mathf.Sin(phase));
        }

        /// <summary>
        /// Returns a new <see cref="T:MathNet.Numerics.Complex32" /> instance
        /// with a real number equal to zero and an imaginary number equal to zero.
        /// </summary>
        public static readonly Complex32 Zero = new Complex32(0.0f, 0.0f);

        /// <summary>
        /// Returns a new <see cref="T:MathNet.Numerics.Complex32" /> instance
        /// with a real number equal to one and an imaginary number equal to zero.
        /// </summary>
        public static readonly Complex32 One = new Complex32(1.0f, 0.0f);

        /// <summary>
        /// Returns a new <see cref="T:MathNet.Numerics.Complex32" /> instance
        /// with a real number equal to zero and an imaginary number equal to one.
        /// </summary>
        public static readonly Complex32 ImaginaryOne = new Complex32(0, 1);

        /// <summary>
        /// Returns a new <see cref="T:MathNet.Numerics.Complex32" /> instance
        /// with real and imaginary numbers positive infinite.
        /// </summary>
        public static readonly Complex32 PositiveInfinity = new Complex32(float.PositiveInfinity, float.PositiveInfinity);

        /// <summary>
        /// Returns a new <see cref="T:MathNet.Numerics.Complex32" /> instance
        /// with real and imaginary numbers not a number.
        /// </summary>
        public static readonly Complex32 NaN = new Complex32(float.NaN, float.NaN);

        /// <summary>
        /// Gets the real component of the complex number.
        /// </summary>
        /// <value>The real component of the complex number.</value>
        public float Real
        {
            get { return _real; }
        }

        /// <summary>
        /// Gets the real imaginary component of the complex number.
        /// </summary>
        /// <value>The real imaginary component of the complex number.</value>
        public float Imaginary
        {
            get { return _imag; }
        }

        /// <summary>
        /// Gets the phase or argument of this <c>Complex32</c>.
        /// </summary>
        /// <remarks>
        /// Phase always returns a value bigger than negative Pi and
        /// smaller or equal to Pi. If this <c>Complex32</c> is zero, the Complex32
        /// is assumed to be positive real with an argument of zero.
        /// </remarks>
        /// <returns>The phase or argument of this <c>Complex32</c></returns>
        public float Phase
        {
            // NOTE: the special case for negative real numbers fixes negative-zero value behavior. Do not remove.
            get { return _imag == 0f && _real < 0f ? Mathf.PI : Mathf.Atan2(_imag, _real); }
        }

        /// <summary>
        /// Gets the magnitude (or absolute value) of a complex number.
        /// </summary>
        /// <remarks>Assuming that magnitude of (inf,a) and (a,inf) and (inf,inf) is inf and (NaN,a), (a,NaN) and (NaN,NaN) is NaN</remarks>
        /// <returns>The magnitude of the current instance.</returns>
        public float Magnitude
        {
            get
            {
                if (float.IsNaN(_real) || float.IsNaN(_imag))
                    return float.NaN;
                if (float.IsInfinity(_real) || float.IsInfinity(_imag))
                    return float.PositiveInfinity;
                float a = Mathf.Abs(_real);
                float b = Mathf.Abs(_imag);
                if (a > b)
                {
                    float tmp = b / a;
                    return a * Mathf.Sqrt(1.0f + tmp * tmp);

                }
                if (a == 0.0f) // one can write a >= float.Epsilon here
                {
                    return b;
                }
                else
                {
                    float tmp = a / b;
                    return b * Mathf.Sqrt(1.0f + tmp * tmp);
                }
            }
        }

        /// <summary>
        /// Gets the squared magnitude (or squared absolute value) of a complex number.
        /// </summary>
        /// <returns>The squared magnitude of the current instance.</returns>
        public float MagnitudeSquared
        {
            get { return (_real * _real) + (_imag * _imag); }
        }

        /// <summary>
        /// Gets the unity of this complex (same argument, but on the unit circle; exp(I*arg))
        /// </summary>
        /// <returns>The unity of this <c>Complex32</c>.</returns>
        public Complex32 Sign
        {
            get
            {
                if (float.IsPositiveInfinity(_real) && float.IsPositiveInfinity(_imag))
                {
                    return new Complex32(Mathf.Sqrt(1f / 2f), Mathf.Sqrt(1f / 2f));
                }

                if (float.IsPositiveInfinity(_real) && float.IsNegativeInfinity(_imag))
                {
                    return new Complex32(Mathf.Sqrt(1f / 2f), -Mathf.Sqrt(1f / 2f));
                }

                if (float.IsNegativeInfinity(_real) && float.IsPositiveInfinity(_imag))
                {
                    return new Complex32(-Mathf.Sqrt(1f / 2f), -Mathf.Sqrt(1f / 2f));
                }

                if (float.IsNegativeInfinity(_real) && float.IsNegativeInfinity(_imag))
                {
                    return new Complex32(-Mathf.Sqrt(1f / 2f), Mathf.Sqrt(1f / 2f));
                }

                // don't replace this with "Magnitude"!
                var mod = SpecialFunctions.Hypotenuse(_real, _imag);
                if (mod == 0.0f)
                {
                    return Zero;
                }

                return new Complex32(_real / mod, _imag / mod);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is zero.
        /// </summary>
        /// <returns><c>true</c> if this instance is zero; otherwise, <c>false</c>.</returns>
        public bool IsZero()
        {
            return _real == 0.0f && _imag == 0.0f;
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is one.
        /// </summary>
        /// <returns><c>true</c> if this instance is one; otherwise, <c>false</c>.</returns>
        public bool IsOne()
        {
            return _real == 1.0f && _imag == 0.0f;
        }

        /// <summary>
        /// Gets a value indicating whether the <c>Complex32</c> is the imaginary unit.
        /// </summary>
        /// <returns><c>true</c> if this instance is ImaginaryOne; otherwise, <c>false</c>.</returns>
        public bool IsImaginaryOne()
        {
            return _real == 0.0f && _imag == 1.0f;
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c>evaluates
        /// to a value that is not a number.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is <see cref="NaN"/>; otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool IsNaN()
        {
            return float.IsNaN(_real) || float.IsNaN(_imag);
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> evaluates to an
        /// infinite value.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is infinite; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// True if it either evaluates to a complex infinity
        /// or to a directed infinity.
        /// </remarks>
        public bool IsInfinity()
        {
            return float.IsInfinity(_real) || float.IsInfinity(_imag);
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> is real.
        /// </summary>
        /// <returns><c>true</c> if this instance is a real number; otherwise, <c>false</c>.</returns>
        public bool IsReal()
        {
            return _imag == 0.0f;
        }

        /// <summary>
        /// Gets a value indicating whether the provided <c>Complex32</c> is real and not negative, that is &gt;= 0.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is real nonnegative number; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRealNonNegative()
        {
            return _imag == 0.0f && _real >= 0;
        }

        /// <summary>
        /// Exponential of this <c>Complex32</c> (exp(x), E^x).
        /// </summary>
        /// <returns>
        /// The exponential of this complex number.
        /// </returns>
        public Complex32 Exponential()
        {
            var exp = Mathf.Exp(_real);
            if (IsReal())
            {
                return new Complex32(exp, 0.0f);
            }

            return new Complex32(exp * Mathf.Cos(_imag), exp * Mathf.Sin(_imag));
        }

        /// <summary>
        /// Natural Logarithm of this <c>Complex32</c> (Base E).
        /// </summary>
        /// <returns>The natural logarithm of this complex number.</returns>
        public Complex32 NaturalLogarithm()
        {
            if (IsRealNonNegative())
            {
                return new Complex32(Mathf.Log(_real), 0.0f);
            }

            return new Complex32(0.5f * Mathf.Log(MagnitudeSquared), Phase);
        }

        /// <summary>
        /// Common Logarithm of this <c>Complex32</c> (Base 10).
        /// </summary>
        /// <returns>The common logarithm of this complex number.</returns>
        public Complex32 CommonLogarithm()
        {
            return NaturalLogarithm() / Mathf.Log(10f);
        }

        /// <summary>
        /// Logarithm of this <c>Complex32</c> with custom base.
        /// </summary>
        /// <returns>The logarithm of this complex number.</returns>
        public Complex32 Logarithm(float baseValue)
        {
            return NaturalLogarithm() / Mathf.Log(baseValue);
        }

        /// <summary>
        /// Raise this <c>Complex32</c> to the given value.
        /// </summary>
        /// <param name="exponent">
        /// The exponent.
        /// </param>
        /// <returns>
        /// The complex number raised to the given exponent.
        /// </returns>
        public Complex32 Power(Complex32 exponent)
        {
            if (IsZero())
            {
                if (exponent.IsZero())
                {
                    return One;
                }

                if (exponent.Real > 0f)
                {
                    return Zero;
                }

                if (exponent.Real < 0f)
                {
                    return exponent.Imaginary == 0f
                        ? new Complex32(float.PositiveInfinity, 0f)
                        : new Complex32(float.PositiveInfinity, float.PositiveInfinity);
                }

                return NaN;
            }

            return (exponent * NaturalLogarithm()).Exponential();
        }

        /// <summary>
        /// Raise this <c>Complex32</c> to the inverse of the given value.
        /// </summary>
        /// <param name="rootExponent">
        /// The root exponent.
        /// </param>
        /// <returns>
        /// The complex raised to the inverse of the given exponent.
        /// </returns>
        public Complex32 Root(Complex32 rootExponent)
        {
            return Power(1 / rootExponent);
        }

        /// <summary>
        /// The Square (power 2) of this <c>Complex32</c>
        /// </summary>
        /// <returns>
        /// The square of this complex number.
        /// </returns>
        public Complex32 Square()
        {
            if (IsReal())
            {
                return new Complex32(_real * _real, 0.0f);
            }

            return new Complex32((_real * _real) - (_imag * _imag), 2 * _real * _imag);
        }

        /// <summary>
        /// The Square Root (power 1/2) of this <c>Complex32</c>
        /// </summary>
        /// <returns>
        /// The square root of this complex number.
        /// </returns>
        public Complex32 SquareRoot()
        {
            if (IsRealNonNegative())
            {
                return new Complex32(Mathf.Sqrt(_real), 0.0f);
            }

            Complex32 result;

            var absReal = Mathf.Abs(Real);
            var absImag = Mathf.Abs(Imaginary);
            double w;
            if (absReal >= absImag)
            {
                var ratio = Imaginary / Real;
                w = Mathf.Sqrt(absReal) * Mathf.Sqrt(0.5f * (1.0f + Mathf.Sqrt(1.0f + (ratio * ratio))));
            }
            else
            {
                var ratio = Real / Imaginary;
                w = Mathf.Sqrt(absImag) * Mathf.Sqrt(0.5f * (Mathf.Abs(ratio) + Mathf.Sqrt(1.0f + (ratio * ratio))));
            }

            if (Real >= 0.0f)
            {
                result = new Complex32((float)w, (float)(Imaginary / (2.0f * w)));
            }
            else if (Imaginary >= 0.0f)
            {
                result = new Complex32((float)(absImag / (2.0 * w)), (float)w);
            }
            else
            {
                result = new Complex32((float)(absImag / (2.0 * w)), (float)-w);
            }

            return result;
        }

        /// <summary>
        /// Evaluate all square roots of this <c>Complex32</c>.
        /// </summary>
        public Tuple<Complex32, Complex32> SquareRoots()
        {
            var principal = SquareRoot();
            return new Tuple<Complex32, Complex32>(principal, -principal);
        }

        /// <summary>
        /// Evaluate all cubic roots of this <c>Complex32</c>.
        /// </summary>
        public Tuple<Complex32, Complex32, Complex32> CubicRoots()
        {
            float r = Mathf.Pow(Magnitude, 1f / 3f);
            float theta = Phase / 3f;
            const float shift = Mathf.PI * 2f / 3f;
            return new Tuple<Complex32, Complex32, Complex32>(
                FromPolarCoordinates(r, theta),
                FromPolarCoordinates(r, theta + shift),
                FromPolarCoordinates(r, theta - shift));
        }

        /// <summary>
        /// Equality test.
        /// </summary>
        /// <param name="complex1">One of complex numbers to compare.</param>
        /// <param name="complex2">The other complex numbers to compare.</param>
        /// <returns><c>true</c> if the real and imaginary components of the two complex numbers are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Complex32 complex1, Complex32 complex2)
        {
            return complex1.Equals(complex2);
        }

        /// <summary>
        /// Inequality test.
        /// </summary>
        /// <param name="complex1">One of complex numbers to compare.</param>
        /// <param name="complex2">The other complex numbers to compare.</param>
        /// <returns><c>true</c> if the real or imaginary components of the two complex numbers are not equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Complex32 complex1, Complex32 complex2)
        {
            return !complex1.Equals(complex2);
        }

        /// <summary>
        /// Unary addition.
        /// </summary>
        /// <param name="summand">The complex number to operate on.</param>
        /// <returns>Returns the same complex number.</returns>
        public static Complex32 operator +(Complex32 summand)
        {
            return summand;
        }

        /// <summary>
        /// Unary minus.
        /// </summary>
        /// <param name="subtrahend">The complex number to operate on.</param>
        /// <returns>The negated value of the <paramref name="subtrahend"/>.</returns>
        public static Complex32 operator -(Complex32 subtrahend)
        {
            return new Complex32(-subtrahend._real, -subtrahend._imag);
        }

        /// <summary>Addition operator. Adds two complex numbers together.</summary>
        /// <returns>The result of the addition.</returns>
        /// <param name="summand1">One of the complex numbers to add.</param>
        /// <param name="summand2">The other complex numbers to add.</param>
        public static Complex32 operator +(Complex32 summand1, Complex32 summand2)
        {
            return new Complex32(summand1._real + summand2._real, summand1._imag + summand2._imag);
        }

        /// <summary>Subtraction operator. Subtracts two complex numbers.</summary>
        /// <returns>The result of the subtraction.</returns>
        /// <param name="minuend">The complex number to subtract from.</param>
        /// <param name="subtrahend">The complex number to subtract.</param>
        public static Complex32 operator -(Complex32 minuend, Complex32 subtrahend)
        {
            return new Complex32(minuend._real - subtrahend._real, minuend._imag - subtrahend._imag);
        }

        /// <summary>Addition operator. Adds a complex number and float together.</summary>
        /// <returns>The result of the addition.</returns>
        /// <param name="summand1">The complex numbers to add.</param>
        /// <param name="summand2">The float value to add.</param>
        public static Complex32 operator +(Complex32 summand1, float summand2)
        {
            return new Complex32(summand1._real + summand2, summand1._imag);
        }

        /// <summary>Subtraction operator. Subtracts float value from a complex value.</summary>
        /// <returns>The result of the subtraction.</returns>
        /// <param name="minuend">The complex number to subtract from.</param>
        /// <param name="subtrahend">The float value to subtract.</param>
        public static Complex32 operator -(Complex32 minuend, float subtrahend)
        {
            return new Complex32(minuend._real - subtrahend, minuend._imag);
        }

        /// <summary>Addition operator. Adds a complex number and float together.</summary>
        /// <returns>The result of the addition.</returns>
        /// <param name="summand1">The float value to add.</param>
        /// <param name="summand2">The complex numbers to add.</param>
        public static Complex32 operator +(float summand1, Complex32 summand2)
        {
            return new Complex32(summand2._real + summand1, summand2._imag);
        }

        /// <summary>Subtraction operator. Subtracts complex value from a float value.</summary>
        /// <returns>The result of the subtraction.</returns>
        /// <param name="minuend">The float vale to subtract from.</param>
        /// <param name="subtrahend">The complex value to subtract.</param>
        public static Complex32 operator -(float minuend, Complex32 subtrahend)
        {
            return new Complex32(minuend - subtrahend._real, -subtrahend._imag);
        }

        /// <summary>Multiplication operator. Multiplies two complex numbers.</summary>
        /// <returns>The result of the multiplication.</returns>
        /// <param name="multiplicand">One of the complex numbers to multiply.</param>
        /// <param name="multiplier">The other complex number to multiply.</param>
        public static Complex32 operator *(Complex32 multiplicand, Complex32 multiplier)
        {
            return new Complex32(
                (multiplicand._real * multiplier._real) - (multiplicand._imag * multiplier._imag),
                (multiplicand._real * multiplier._imag) + (multiplicand._imag * multiplier._real));
        }

        /// <summary>Multiplication operator. Multiplies a complex number with a float value.</summary>
        /// <returns>The result of the multiplication.</returns>
        /// <param name="multiplicand">The float value to multiply.</param>
        /// <param name="multiplier">The complex number to multiply.</param>
        public static Complex32 operator *(float multiplicand, Complex32 multiplier)
        {
            return new Complex32(multiplier._real * multiplicand, multiplier._imag * multiplicand);
        }

        /// <summary>Multiplication operator. Multiplies a complex number with a float value.</summary>
        /// <returns>The result of the multiplication.</returns>
        /// <param name="multiplicand">The complex number to multiply.</param>
        /// <param name="multiplier">The float value to multiply.</param>
        public static Complex32 operator *(Complex32 multiplicand, float multiplier)
        {
            return new Complex32(multiplicand._real * multiplier, multiplicand._imag * multiplier);
        }

        /// <summary>Division operator. Divides a complex number by another.</summary>
        /// <remarks>Enhanced Smith's algorithm for dividing two complex numbers </remarks>
        /// <see cref="InternalDiv(float, float, float, float, bool)"/>
        /// <returns>The result of the division.</returns>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        public static Complex32 operator /(Complex32 dividend, Complex32 divisor)
        {
            if (dividend.IsZero() && divisor.IsZero())
            {
                return NaN;
            }

            if (divisor.IsZero())
            {
                return PositiveInfinity;
            }
            float a = dividend.Real;
            float b = dividend.Imaginary;
            float c = divisor.Real;
            float d = divisor.Imaginary;
            if (Mathf.Abs(d) <= Mathf.Abs(c))
                return InternalDiv(a, b, c, d, false);
            return InternalDiv(b, a, d, c, true);
        }
        /// <summary>
        ///  Helper method for dividing.
        /// </summary>
        /// <param name="a">Re first</param>
        /// <param name="b">Im first</param>
        /// <param name="c">Re second</param>
        /// <param name="d">Im second</param>
        /// <param name="swapped"></param>
        /// <returns></returns>
        private static Complex32 InternalDiv(float a, float b, float c, float d, bool swapped)
        {
            float r = d / c;
            float t = 1 / (c + d * r);
            float e, f;
            if (r != 0.0f) // one can use r >= float.Epsilon || r <= float.Epsilon instead
            {
                e = (a + b * r) * t;
                f = (b - a * r) * t;
            }
            else
            {
                e = (a + d * (b / c)) * t;
                f = (b - d * (a / c)) * t;
            }
            if (swapped)
                f = -f;
            return new Complex32(e, f);
        }

        /// <summary>Division operator. Divides a float value by a complex number.</summary>
        /// <remarks>Algorithm based on Smith's algorithm</remarks>
        /// <see cref="InternalDiv(float, float, float, float, bool)"/> 
        /// <returns>The result of the division.</returns>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        public static Complex32 operator /(float dividend, Complex32 divisor)
        {
            if (dividend == 0.0f && divisor.IsZero())
            {
                return NaN;
            }

            if (divisor.IsZero())
            {
                return PositiveInfinity;
            }
            float c = divisor.Real;
            float d = divisor.Imaginary;
            if (Mathf.Abs(d) <= Mathf.Abs(c))
                return InternalDiv(dividend, 0, c, d, false);
            return InternalDiv(0, dividend, d, c, true);
        }

        /// <summary>Division operator. Divides a complex number by a float value.</summary>
        /// <returns>The result of the division.</returns>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        public static Complex32 operator /(Complex32 dividend, float divisor)
        {
            if (dividend.IsZero() && divisor == 0.0f)
            {
                return NaN;
            }

            if (divisor == 0.0f)
            {
                return PositiveInfinity;
            }

            return new Complex32(dividend._real / divisor, dividend._imag / divisor);
        }

        /// <summary>
        /// Computes the conjugate of a complex number and returns the result.
        /// </summary>
        public Complex32 Conjugate()
        {
            return new Complex32(_real, -_imag);
        }

        /// <summary>
        /// Returns the multiplicative inverse of a complex number.
        /// </summary>
        public Complex32 Reciprocal()
        {
            if (IsZero())
            {
                return Zero;
            }

            return 1.0f / this;
        }

        #region IEquatable<Complex32> Members

        /// <summary>
        /// Checks if two complex numbers are equal. Two complex numbers are equal if their
        /// corresponding real and imaginary components are equal.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if the two objects are the same object, or if their corresponding
        /// real and imaginary components are equal, <c>false</c> otherwise.
        /// </returns>
        /// <param name="other">
        /// The complex number to compare to with.
        /// </param>
        public bool Equals(Complex32 other)
        {
            if (IsNaN() || other.IsNaN())
            {
                return false;
            }

            if (IsInfinity() && other.IsInfinity())
            {
                return true;
            }

            return _real == other._real && _imag == other._imag;
        }

        /// <summary>
        /// The hash code for the complex number.
        /// </summary>
        /// <returns>
        /// The hash code of the complex number.
        /// </returns>
        /// <remarks>
        /// The hash code is calculated as
        /// System.Math.Exp(ComplexMath.Absolute(complexNumber)).
        /// </remarks>
        public override int GetHashCode()
        {
            int hash = 27;
            hash = (13 * hash) + _real.GetHashCode();
            hash = (13 * hash) + _imag.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Checks if two complex numbers are equal. Two complex numbers are equal if their
        /// corresponding real and imaginary components are equal.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if the two objects are the same object, or if their corresponding
        /// real and imaginary components are equal, <c>false</c> otherwise.
        /// </returns>
        /// <param name="obj">
        /// The complex number to compare to with.
        /// </param>
        public override bool Equals(object obj)
        {
            return (obj is Complex32) && Equals((Complex32)obj);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Explicit conversion of a real decimal to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The decimal value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Complex32(decimal value)
        {
            return new Complex32((float)value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real byte to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The byte value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(byte value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real short to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The short value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(short value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a signed byte to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The signed byte value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(sbyte value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a unsgined real short to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The unsgined short value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(ushort value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real int to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The int value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(int value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real long to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The long value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(long value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real uint to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The uint value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(uint value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real ulong to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The ulong value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(ulong value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real float to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The float value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(float value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        /// Implicit conversion of a real double to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The double value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Complex32(double value)
        {
            return new Complex32((float)value, 0.0f);
        }

        /// <summary>
        /// Converts this <c>Complex32</c> to a <see cref="Complex"/>.
        /// </summary>
        /// <returns>A <see cref="Complex"/> with the same values as this <c>Complex32</c>.</returns>
        public Complex32 ToComplex()
        {
            return new Complex32(_real, _imag);
        }

        #endregion

        /// <summary>
        /// Returns the additive inverse of a specified complex number.
        /// </summary>
        /// <returns>The result of the real and imaginary components of the value parameter multiplied by -1.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Negate(Complex32 value)
        {
            return -value;
        }

        /// <summary>
        /// Computes the conjugate of a complex number and returns the result.
        /// </summary>
        /// <returns>The conjugate of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Conjugate(Complex32 value)
        {
            return value.Conjugate();
        }

        /// <summary>
        /// Adds two complex numbers and returns the result.
        /// </summary>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <param name="left">The first complex number to add.</param>
        /// <param name="right">The second complex number to add.</param>
        public static Complex32 Add(Complex32 left, Complex32 right)
        {
            return left + right;
        }

        /// <summary>
        /// Subtracts one complex number from another and returns the result.
        /// </summary>
        /// <returns>The result of subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <param name="left">The value to subtract from (the minuend).</param>
        /// <param name="right">The value to subtract (the subtrahend).</param>
        public static Complex32 Subtract(Complex32 left, Complex32 right)
        {
            return left - right;
        }

        /// <summary>
        /// Returns the product of two complex numbers.
        /// </summary>
        /// <returns>The product of the <paramref name="left" /> and <paramref name="right" /> parameters.</returns>
        /// <param name="left">The first complex number to multiply.</param>
        /// <param name="right">The second complex number to multiply.</param>
        public static Complex32 Multiply(Complex32 left, Complex32 right)
        {
            return left * right;
        }

        /// <summary>
        /// Divides one complex number by another and returns the result.
        /// </summary>
        /// <returns>The quotient of the division.</returns>
        /// <param name="dividend">The complex number to be divided.</param>
        /// <param name="divisor">The complex number to divide by.</param>
        public static Complex32 Divide(Complex32 dividend, Complex32 divisor)
        {
            return dividend / divisor;
        }
        /// <summary>
        /// Returns the multiplicative inverse of a complex number.
        /// </summary>
        /// <returns>The reciprocal of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Reciprocal(Complex32 value)
        {
            return value.Reciprocal();
        }

        /// <summary>
        /// Returns the square root of a specified complex number.
        /// </summary>
        /// <returns>The square root of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Sqrt(Complex32 value)
        {
            return value.SquareRoot();
        }

        /// <summary>
        /// Gets the absolute value (or magnitude) of a complex number.
        /// </summary>
        /// <returns>The absolute value of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static double Abs(Complex32 value)
        {
            return value.Magnitude;
        }

        /// <summary>
        /// Returns e raised to the power specified by a complex number.
        /// </summary>
        /// <returns>The number e raised to the power <paramref name="value" />.</returns>
        /// <param name="value">A complex number that specifies a power.</param>
        public static Complex32 Exp(Complex32 value)
        {
            return value.Exponential();
        }

        /// <summary>
        /// Returns a specified complex number raised to a power specified by a complex number.
        /// </summary>
        /// <returns>The complex number <paramref name="value" /> raised to the power <paramref name="power" />.</returns>
        /// <param name="value">A complex number to be raised to a power.</param>
        /// <param name="power">A complex number that specifies a power.</param>
        public static Complex32 Pow(Complex32 value, Complex32 power)
        {
            return value.Power(power);
        }

        /// <summary>
        /// Returns a specified complex number raised to a power specified by a single-precision floating-point number.
        /// </summary>
        /// <returns>The complex number <paramref name="value" /> raised to the power <paramref name="power" />.</returns>
        /// <param name="value">A complex number to be raised to a power.</param>
        /// <param name="power">A single-precision floating-point number that specifies a power.</param>
        public static Complex32 Pow(Complex32 value, float power)
        {
            return value.Power(power);
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified complex number.
        /// </summary>
        /// <returns>The natural (base e) logarithm of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Log(Complex32 value)
        {
            return value.NaturalLogarithm();
        }

        /// <summary>
        /// Returns the logarithm of a specified complex number in a specified base.
        /// </summary>
        /// <returns>The logarithm of <paramref name="value" /> in base <paramref name="baseValue" />.</returns>
        /// <param name="value">A complex number.</param>
        /// <param name="baseValue">The base of the logarithm.</param>
        public static Complex32 Log(Complex32 value, float baseValue)
        {
            return value.Logarithm(baseValue);
        }

        /// <summary>
        /// Returns the base-10 logarithm of a specified complex number.
        /// </summary>
        /// <returns>The base-10 logarithm of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Log10(Complex32 value)
        {
            return value.CommonLogarithm();
        }

        /// <summary>
        /// Returns the sine of the specified complex number.
        /// </summary>
        /// <returns>The sine of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Sin(Complex32 value)
        {
            return Trigonometry.Sin(value.ToComplex());
        }

        /// <summary>
        /// Returns the cosine of the specified complex number.
        /// </summary>
        /// <returns>The cosine of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Cos(Complex32 value)
        {
            return Trigonometry.Cos(value.ToComplex());
        }

        /// <summary>
        /// Returns the tangent of the specified complex number.
        /// </summary>
        /// <returns>The tangent of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static Complex32 Tan(Complex32 value)
        {
            return Trigonometry.Tan(value.ToComplex());
        }

        public override string ToString()
        {
            return "(" + Real + "," + Imaginary + ")";
        }
    }
}
