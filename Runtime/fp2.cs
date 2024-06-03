using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace Paulsams.FixedPoint
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = Size)]
    public struct fp2 : IEquatable<fp2>
    {
        public const int Size = 16;

        public static readonly fp2 left = new fp2(-fp._1, fp._0);
        public static readonly fp2 right = new fp2(fp._1, fp._0);
        public static readonly fp2 up = new fp2(fp._0, fp._1);
        public static readonly fp2 down = new fp2(fp._0, -fp._1);
        public static readonly fp2 one = new fp2(fp._1, fp._1);
        public static readonly fp2 minus_one = new fp2(fp.minus_one, fp.minus_one);
        public static readonly fp2 zero = new fp2(fp._0, fp._0);

        [FieldOffset(0)] public fp x;
        [FieldOffset(8)] public fp y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp2(fp x, fp y)
        {
            this.x.value = x.value;
            this.y.value = y.value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator +(fp2 a, fp2 b)
        {
            a.x.value += b.x.value;
            a.y.value += b.y.value;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator -(fp2 a, fp2 b)
        {
            a.x.value -= b.x.value;
            a.y.value -= b.y.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator -(fp2 a)
        {
            a.x.value = -a.x.value;
            a.y.value = -a.y.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator *(fp2 a, fp2 b)
        {
            a.x.value = (a.x.value * b.x.value) >> fixlut.PRECISION;
            a.y.value = (a.y.value * b.y.value) >> fixlut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator *(fp2 a, fp b)
        {
            a.x.value = (a.x.value * b.value) >> fixlut.PRECISION;
            a.y.value = (a.y.value * b.value) >> fixlut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator *(fp b, fp2 a)
        {
            a.x.value = (a.x.value * b.value) >> fixlut.PRECISION;
            a.y.value = (a.y.value * b.value) >> fixlut.PRECISION;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator /(fp2 a, fp2 b)
        {
            a.x.value = (a.x.value << fixlut.PRECISION) / b.x.value;
            a.y.value = (a.y.value << fixlut.PRECISION) / b.y.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator /(fp2 a, fp b)
        {
            a.x.value = (a.x.value << fixlut.PRECISION) / b.value;
            a.y.value = (a.y.value << fixlut.PRECISION) / b.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 operator /(fp b, fp2 a)
        {
            a.x.value = (b.value << fixlut.PRECISION) / a.x.value;
            a.y.value = (b.value << fixlut.PRECISION) / a.y.value;

            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fp2 a, fp2 b) =>
            a.x.value == b.x.value && a.y.value == b.y.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fp2 a, fp2 b) =>
            a.x.value != b.x.value || a.y.value != b.y.value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int2(fp2 value) =>
            new int2(value.x.AsInt, value.y.AsInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp2(int2 value) =>
            new fp2(value.x, value.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2Int(fp2 value) =>
            new Vector2Int(value.x.AsInt, value.y.AsInt);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp2(Vector2Int value) =>
            new fp2(value.x, value.y);

        public override bool Equals(object obj) =>
            obj is fp2 other && this == other;

        public bool Equals(fp2 other) =>
            this == other;

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }

        public override string ToString() =>
            $"({x}, {y})";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 ParseUnsafe(Vector2 value) =>
            new fp2(fp.ParseUnsafe(value.x), fp.ParseUnsafe(value.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2 ParseUnsafe(float2 value) =>
            new fp2(fp.ParseUnsafe(value.x), fp.ParseUnsafe(value.y));

        public class EqualityComparer : IEqualityComparer<fp2>
        {
            public static readonly EqualityComparer Instance = new EqualityComparer();

            private EqualityComparer()
            {
            }

            bool IEqualityComparer<fp2>.Equals(fp2 x, fp2 y) =>
                x == y;

            int IEqualityComparer<fp2>.GetHashCode(fp2 obj) =>
                obj.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp2 Normalize() =>
            fixmath.Normalize(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp Magnitude() =>
            fixmath.Magnitude(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp MagnitudeSqr() =>
            fixmath.MagnitudeSqr(this);
    }
}