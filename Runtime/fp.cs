using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Paulsams.FixedPoint
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = Size)]
    public struct fp : IEquatable<fp>, IComparable<fp>, IComparable
    {
        public const int Size = 8;

        public static readonly fp max = new fp(long.MaxValue);
        public static readonly fp min = new fp(long.MinValue);
        public static readonly fp usable_max = new fp(2147483648L);
        public static readonly fp usable_min = -usable_max;

        public static readonly fp _0 = 0;
        public static readonly fp _1 = 1;
        public static readonly fp _2 = 2;
        public static readonly fp _3 = 3;
        public static readonly fp _4 = 4;
        public static readonly fp _5 = 5;
        public static readonly fp _6 = 6;
        public static readonly fp _7 = 7;
        public static readonly fp _8 = 8;
        public static readonly fp _9 = 9;
        public static readonly fp _10 = 10;
        public static readonly fp _99 = 99;
        public static readonly fp _100 = 100;
        public static readonly fp _200 = 200;

        public static readonly fp _0_01 = _1 / _100;
        public static readonly fp _0_02 = _0_01 * 2;
        public static readonly fp _0_03 = _0_01 * 3;
        public static readonly fp _0_04 = _0_01 * 4;
        public static readonly fp _0_05 = _0_01 * 5;
        public static readonly fp _0_10 = _1 / 10;
        public static readonly fp _0_20 = _0_10 * 2;
        public static readonly fp _0_25 = _1 / 4;
        public static readonly fp _0_33 = _1 / 3;
        public static readonly fp _0_50 = _1 / 2;
        public static readonly fp _0_75 = _1 - _0_25;
        public static readonly fp _0_95 = _1 - _0_05;
        public static readonly fp _0_99 = _1 - _0_01;
        public static readonly fp _1_01 = _1 + _0_01;
        public static readonly fp _1_10 = _1 + _0_10;
        public static readonly fp _1_50 = _1 + _0_50;

        public static readonly fp minus_one = -1;
        public static readonly fp pi = new fp(205887L);
        public static readonly fp pi2 = pi * 2;
        public static readonly fp pi_quarter = pi * _0_25;
        public static readonly fp pi_half = pi * _0_50;
        public static readonly fp one_div_pi2 = 1 / pi2;
        public static readonly fp deg2rad = new fp(1143L);
        public static readonly fp rad2deg = new fp(3754936L);
        public static readonly fp epsilon = new fp(1);
        public static readonly fp e = new fp(178145L);

        [FieldOffset(0)] public long value;

        public long AsLong => value >> fixlut.PRECISION;
        public int AsInt => (int)(value >> fixlut.PRECISION);
        public float AsFloat => value / 65536f;
        public float AsFloatRounded => (float)Math.Round(value / 65536f, 5);
        public double AsDouble => value / 65536d;
        public double AsDoubleRounded => Math.Round(value / 65536d, 5);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal fp(long v) => value = v;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator -(fp a)
        {
            a.value = -a.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator +(fp a)
        {
            a.value = +a.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator +(fp a, fp b)
        {
            a.value += b.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator +(fp a, int b)
        {
            a.value += (long)b << fixlut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator +(int a, fp b)
        {
            b.value = ((long)a << fixlut.PRECISION) + b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator -(fp a, fp b)
        {
            a.value -= b.value;
            return a;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator -(fp a, int b)
        {
            a.value -= (long)b << fixlut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator -(int a, fp b)
        {
            b.value = ((long)a << fixlut.PRECISION) - b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator *(fp a, fp b)
        {
            a.value = a.value * b.value >> fixlut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator *(fp a, int b)
        {
            a.value *= b;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator *(int a, fp b)
        {
            b.value *= a;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator /(fp a, fp b)
        {
            a.value = (a.value << fixlut.PRECISION) / b.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator /(fp a, int b)
        {
            a.value /= b;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator /(int a, fp b)
        {
            b.value = ((long)a << 32) / b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator %(fp a, fp b)
        {
            a.value %= b.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator %(fp a, int b)
        {
            a.value %= (long)b << fixlut.PRECISION;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp operator %(int a, fp b)
        {
            b.value = ((long)a << fixlut.PRECISION) % b.value;
            return b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(fp a, fp b) =>
            a.value < b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(fp a, int b) =>
            a.value < (long)b << fixlut.PRECISION;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(int a, fp b) =>
            (long)a << fixlut.PRECISION < b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(fp a, fp b) =>
            a.value <= b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(fp a, int b) =>
            a.value <= (long)b << fixlut.PRECISION;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(int a, fp b) =>
            (long)a << fixlut.PRECISION <= b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(fp a, fp b) =>
            a.value > b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(fp a, int b) =>
            a.value > (long)b << fixlut.PRECISION;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(int a, fp b) =>
            (long)a << fixlut.PRECISION > b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(fp a, fp b) =>
            a.value >= b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(fp a, int b) =>
            a.value >= (long)b << fixlut.PRECISION;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(int a, fp b) =>
            (long)a << fixlut.PRECISION >= b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fp a, fp b) =>
            a.value == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fp a, int b) =>
            a.value == (long)b << fixlut.PRECISION;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(int a, fp b) =>
            (long)a << fixlut.PRECISION == b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fp a, fp b) =>
            a.value != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fp a, int b) =>
            a.value != (long)b << fixlut.PRECISION;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(int a, fp b) =>
            (long)a << fixlut.PRECISION != b.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp(int value) =>
            new fp() { value = (long)value << fixlut.PRECISION };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(fp value) =>
            (int)(value.value >> fixlut.PRECISION);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(fp value) =>
            value.value >> fixlut.PRECISION;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(fp value) =>
            value.value / 65536f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(fp value) =>
            value.value / 65536d;

        int IComparable.CompareTo(object obj) => CompareTo((fp)obj);
        public int CompareTo(fp other) => value.CompareTo(other.value);

        public bool Equals(fp other) => value == other.value;
        public override bool Equals(object obj) => obj is fp other && this == other;

        public override int GetHashCode() => value.GetHashCode();

        public override string ToString()
        {
            var corrected = Math.Round(AsDouble, 5);
            return corrected.ToString("F5", CultureInfo.InvariantCulture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp ParseRaw(long value) =>
            new fp(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp Parse(long value) =>
            new fp(value << fixlut.PRECISION);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp ParseUnsafe(float value) =>
            new fp((long)(value * fixlut.ONE + 0.5f * (value < 0 ? -1 : 1)));

        public static fp ParseUnsafe(string value)
        {
            var doubleValue = double.Parse(value, CultureInfo.InvariantCulture);
            var longValue = (long)(doubleValue * fixlut.ONE + 0.5d * (doubleValue < 0 ? -1 : 1));
            return new fp(longValue);
        }

        /// <summary>
        /// Deterministically parses FP value out of a string
        /// </summary>
        /// <param name="value">Trimmed string to parse</param>
        public static fp Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return _0;

            bool negative;

            var startIndex = 0;
            // ReSharper disable once AssignmentInConditionalExpression
            if (negative = (value[0] == '-'))
                startIndex = 1;

            var pointIndex = value.IndexOf('.');
            if (pointIndex < startIndex)
            {
                return startIndex == 0
                    ? ParseInteger(value)
                    : -ParseInteger(value.Substring(startIndex, value.Length - startIndex));
            }

            var result = new fp();

            if (pointIndex > startIndex)
            {
                var integerString = value.Substring(startIndex, pointIndex - startIndex);
                result += ParseInteger(integerString);
            }

            if (pointIndex == value.Length - 1)
                return negative ? -result : result;

            var fractionString = value.Substring(pointIndex + 1, value.Length - pointIndex - 1);
            if (fractionString.Length > 0)
                result += ParseFractions(fractionString);

            return negative ? -result : result;
        }

        private static fp ParseInteger(string format) =>
            Parse(long.Parse(format, CultureInfo.InvariantCulture));

        private static fp ParseFractions(string format)
        {
            format = format.Length < 5 ? format.PadRight(5, '0') : format.Substring(0, 5);
            return ParseRaw(long.Parse(format, CultureInfo.InvariantCulture) * 65536 / 100000);
        }

        public class Comparer : IComparer<fp>
        {
            public static readonly Comparer Instance = new Comparer();

            private Comparer()
            {
            }

            int IComparer<fp>.Compare(fp x, fp y) =>
                x.value.CompareTo(y.value);
        }

        public class EqualityComparer : IEqualityComparer<fp>
        {
            public static readonly EqualityComparer Instance = new EqualityComparer();

            private EqualityComparer()
            {
            }

            bool IEqualityComparer<fp>.Equals(fp x, fp y) =>
                x.value == y.value;

            int IEqualityComparer<fp>.GetHashCode(fp num) =>
                num.value.GetHashCode();
        }
    }
}