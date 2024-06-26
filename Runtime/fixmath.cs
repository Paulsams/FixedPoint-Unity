﻿using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Paulsams.FixedPoint
{
    public partial struct fixmath
    {
        private static readonly fp _atan2Number1 = new fp(-883);
        private static readonly fp _atan2Number2 = new fp(3767);
        private static readonly fp _atan2Number3 = new fp(7945);
        private static readonly fp _atan2Number4 = new fp(12821);
        private static readonly fp _atan2Number5 = new fp(21822);
        private static readonly fp _atan2Number6 = new fp(65536);
        private static readonly fp _atan2Number7 = new fp(102943);
        private static readonly fp _atan2Number8 = new fp(205887);
        private static readonly fp _atanApproximatedNumber1 = new fp(16036);
        private static readonly fp _atanApproximatedNumber2 = new fp(4345);

        private static readonly byte[] _bsrLookup =
        {
            0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30, 8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26,
            5, 4, 31
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BitScanReverse(uint num)
        {
            num |= num >> 1;
            num |= num >> 2;
            num |= num >> 4;
            num |= num >> 8;
            num |= num >> 16;
            return _bsrLookup[(num * 0x07C4ACDDU) >> 27];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountLeadingZeroes(uint num) =>
            num == 0 ? 32 : BitScanReverse(num) ^ 31;

        /// <param name="num">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Sin(fp num)
        {
            num.value %= fp.pi2.value;
            num *= fp.one_div_pi2;
            var raw = fixlut.sin(num.value);
            fp result;
            result.value = raw;
            return result;
        }

        /// <param name="num">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Cos(fp num)
        {
            num.value %= fp.pi2.value;
            num *= fp.one_div_pi2;
            return new fp(fixlut.cos(num.value));
        }

        /// <param name="num">Angle in radians</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Tan(fp num)
        {
            num.value %= fp.pi2.value;
            num *= fp.one_div_pi2;
            return new fp(fixlut.tan(num.value));
        }

        /// <param name="num">Cos [-1, 1]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Acos(fp num) =>
            new fp(fixlut.acos(num.value));

        /// <param name="num">Sin [-1, 1]</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Asin(fp num) =>
            new fp(fixlut.asin(num.value));

        /// <param name="num">Tan</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Atan(fp num) =>
            Atan2(num, fp._1);

        /// <param name="num">Tan [-1, 1]</param>
        /// Max error ~0.0015
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp AtanApproximated(fp num)
        {
            var absX = Abs(num);
            return fp.pi_quarter * num -
                   num * (absX - fp._1) * (_atanApproximatedNumber1 + _atanApproximatedNumber2 * absX);
        }

        /// <param name="x">Denominator</param>
        /// <param name="y">Numerator</param>
        public static fp Atan2(fp y, fp x)
        {
            var absX = Abs(x);
            var absY = Abs(y);
            var t3 = absX;
            var t1 = absY;
            var t0 = Max(t3, t1);
            t1 = Min(t3, t1);
            t3 = fp._1 / t0;
            t3 = t1 * t3;
            var t4 = t3 * t3;
            t0 = _atan2Number1;
            t0 = t0 * t4 + _atan2Number2;
            t0 = t0 * t4 - _atan2Number3;
            t0 = t0 * t4 + _atan2Number4;
            t0 = t0 * t4 - _atan2Number5;
            t0 = t0 * t4 + _atan2Number6;
            t3 = t0 * t3;
            t3 = absY > absX ? _atan2Number7 - t3 : t3;
            t3 = x < fp._0 ? _atan2Number8 - t3 : t3;
            t3 = y < fp._0 ? -t3 : t3;
            return t3;
        }

        /// <param name="num">Angle in radians</param>
        /// <param name="sin">Output parameter for sin value</param>
        /// <param name="cos">Output parameter for cos value</param>
        public static void SinCos(fp num, out fp sin, out fp cos)
        {
            num.value %= fp.pi2.value;
            num *= fp.one_div_pi2;
            fixlut.sin_cos(num.value, out var sinVal, out var cosVal);
            sin.value = sinVal;
            cos.value = cosVal;
        }

        public static fp Rcp(fp num) =>
            //(fp.1 << 16)
            new(4294967296 / num.value);

        public static fp Rsqrt(fp num) =>
            //(fp.1 << 16)
            new(4294967296 / Sqrt(num).value);

        public static fp Sqrt(fp num)
        {
            fp r;

            if (num.value == 0)
            {
                r.value = 0;
            }
            else
            {
                var b = (num.value >> 1) + 1L;
                var c = (b + (num.value / b)) >> 1;

                while (c < b)
                {
                    b = c;
                    c = (b + (num.value / b)) >> 1;
                }

                r.value = b << (fixlut.PRECISION >> 1);
            }

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Floor(fp num)
        {
            num.value = num.value >> fixlut.PRECISION << fixlut.PRECISION;
            return num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Ceil(fp num)
        {
            var fractions = num.value & 0x000000000000FFFFL;

            if (fractions == 0)
                return num;

            num.value = num.value >> fixlut.PRECISION << fixlut.PRECISION;
            num.value += fixlut.ONE;
            return num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Fractions(fp num) =>
            new fp(num.value & 0x000000000000FFFFL);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(fp num)
        {
            var fraction = num.value & 0x000000000000FFFFL;
            return fraction >= fixlut.HALF ? num.AsInt + 1 : num.AsInt;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Min(fp a, fp b) =>
            a.value < b.value ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int a, int b) =>
            a < b ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Min(long a, long b) =>
            a < b ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Max(fp a, fp b) =>
            a.value > b.value ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int a, int b) =>
            a > b ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Max(long a, long b) =>
            a > b ? a : b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Abs(fp num) =>
            new fp(num.value < 0 ? -num.value : num.value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Clamp(fp num, fp min, fp max)
        {
            if (num.value < min.value)
                return min;
            return num.value > max.value ? max : num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int num, int min, int max) =>
            num < min ? min : num > max ? max : num;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long num, long min, long max) =>
            num < min ? min : num > max ? max : num;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Clamp01(fp num)
        {
            if (num.value < 0)
                return fp._0;
            return num.value > fp._1.value ? fp._1 : num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Lerp(fp from, fp to, fp t)
        {
            t = Clamp01(t);
            return from + (to - from) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Lerp(bool from, bool to, fp t) =>
            t.value > fp._0_50.value ? to : from;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Repeat(fp value, fp length) =>
            Clamp(value - Floor(value / length) * length, 0, length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp LerpAngle(fp from, fp to, fp t)
        {
            var num = Repeat(to - from, fp.pi2);
            return Lerp(from, from + (num > fp.pi ? num - fp.pi2 : num), t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp NormalizeRadians(fp angle)
        {
            angle.value %= fixlut.PI;
            return angle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp LerpUnclamped(fp from, fp to, fp t) =>
            from + (to - from) * t;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Sign(fp num) =>
            num.value < fixlut.ZERO ? fp.minus_one : fp._1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOppositeSign(fp a, fp b) =>
            (a.value ^ b.value) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp SetSameSign(fp target, fp reference) =>
            IsOppositeSign(target, reference) ? target * fp.minus_one : target;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Pow2(int power) =>
            new fp(fixlut.ONE << power);
      
        /// <summary>Returns the fp2 row vector result of a matrix multiplication between a fp2 row vector and a fp2x2 matrix.</summary>
        /// <param name="a">Left hand side argument of the matrix multiply.</param>
        /// <param name="b">Right hand side argument of the matrix multiply.</param>
        /// <returns>The computed matrix multiplication.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 mul(fp2 a, fp2x2 b) =>
            new fp2(
                a.x * b.c0.x + a.y * b.c0.y,
                a.x * b.c1.x + a.y * b.c1.y
            );

        public static fp Exp(fp num)
        {
            if (num == fp._0) return fp._1;
            if (num == fp._1) return fp.e;
            if (num.value >= 2097152) return fp.max;
            if (num.value <= -786432) return fp._0;

            var neg = num.value < 0;
            if (neg) num = -num;

            var result = num + fp._1;
            var term = num;

            for (var i = 2; i < 30; i++)
            {
                term *= num / (fp)i;
                result += term;

                if (term.value < 500 && ((i > 15) || (term.value < 20)))
                    break;
            }

            return neg ? fp._1 / result : result;
        }
    }
}