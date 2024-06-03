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
    public struct fp2x2 : IEquatable<fp2x2>
    {
        public const int Size = 32;

        public static readonly fp2x2 one = new fp2x2(fp2.one, fp2.one);
        public static readonly fp2x2 minus_one = new fp2x2(fp2.minus_one, fp2.minus_one);
        public static readonly fp2x2 zero = new fp2x2(fp2.zero, fp2.zero);

        [FieldOffset(0)] public fp2 c0;
        [FieldOffset(16)] public fp2 c1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp2x2(fp2 c0, fp2 c1)
        {
            this.c0 = c0;
            this.c1 = c1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp2x2(fp c00, fp c01, fp c10, fp c11)
        {
            this.c0 = new fp2(c00, c01);
            this.c1 = new fp2(c10, c11);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator +(fp2x2 a, fp2x2 b)
        {
            a.c0 += b.c0;
            a.c1 += b.c1;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator -(fp2x2 a, fp2x2 b)
        {
            a.c0 -= b.c0;
            a.c1 -= b.c1;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator -(fp2x2 a)
        {
            a.c0 = -a.c0;
            a.c1 = -a.c1;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator *(fp2x2 a, fp2x2 b)
        {
            a.c0 *= b.c0;
            a.c1 *= b.c1;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator *(fp2x2 a, fp b)
        {
            a.c0 *= b;
            a.c1 *= b;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator *(fp b, fp2x2 a)
        {
            a.c0 *= b;
            a.c1 *= b;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator /(fp2x2 a, fp2x2 b)
        {
            a.c0 /= b.c0;
            a.c1 /= b.c1;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator /(fp2x2 a, fp b)
        {
            a.c0 /= b;
            a.c1 /= b;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 operator /(fp b, fp2x2 a)
        {
            a.c0 = b / a.c0;
            a.c1 = b / a.c1;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(fp2x2 a, fp2x2 b)
        {
            return a.c0 == b.c0 && a.c1 == b.c1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(fp2x2 a, fp2x2 b)
        {
            return a.c0 != b.c0 || a.c1 != b.c1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int2x2(fp2x2 value) =>
            new int2x2((int2)value.c0, (int2)value.c1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp2x2(int2x2 value) =>
            new fp2x2((fp2)value.c0, (fp2)value.c1);

        public override bool Equals(object obj) =>
            obj is fp2x2 other && this == other;

        public bool Equals(fp2x2 other) =>
            this == other;

        public override int GetHashCode()
        {
            unchecked
            {
                return (c0.GetHashCode() * 397) ^ c1.GetHashCode();
            }
        }

        public override string ToString() =>
            $"{c0}\n{c1}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 ParseUnsafe(Vector2 c0, Vector2 c1) =>
            new fp2x2(fp2.ParseUnsafe(c0), fp2.ParseUnsafe(c1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 ParseUnsafe(float2 c0, float2 c1) =>
            new fp2x2(fp2.ParseUnsafe(c0), fp2.ParseUnsafe(c1));

        public class EqualityComparer : IEqualityComparer<fp2x2>
        {
            public static readonly EqualityComparer Instance = new EqualityComparer();

            private EqualityComparer()
            {
            }

            bool IEqualityComparer<fp2x2>.Equals(fp2x2 x, fp2x2 y) =>
                x == y;

            int IEqualityComparer<fp2x2>.GetHashCode(fp2x2 obj) =>
                obj.GetHashCode();
        }

        /// <summary>
        /// Computes a float2x2 matrix representing a counter-clockwise rotation by an angle in radians.
        /// </summary>
        /// <remarks>
        /// A positive rotation angle will produce a counter-clockwise rotation and a negative rotation angle will
        /// produce a clockwise rotation.
        /// </remarks>
        /// <param name="angle">Rotation angle in radians.</param>
        /// <returns>Returns the 2x2 rotation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 Rotate(fp angle)
        {
            fp s, c;
            fixmath.SinCos(angle, out s, out c);
            return new fp2x2(
                c, -s,
                s, c
            );
        }

        /// <summary>Returns a float2x2 matrix representing a uniform scaling of both axes by s.</summary>
        /// <param name="s">The scaling factor.</param>
        /// <returns>The float2x2 matrix representing uniform scale by s.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 Scale(fp s) =>
            new fp2x2(
                s, fp._0,
                fp._0, s
            );

        /// <summary>Returns a float2x2 matrix representing a non-uniform axis scaling by x and y.</summary>
        /// <param name="x">The x-axis scaling factor.</param>
        /// <param name="y">The y-axis scaling factor.</param>
        /// <returns>The float2x2 matrix representing a non-uniform scale.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 Scale(fp x, fp y) =>
            new fp2x2(
                x, fp._0,
                fp._0, y
            );

        /// <summary>Returns a float2x2 matrix representing a non-uniform axis scaling by the components of the float2 vector v.</summary>
        /// <param name="v">The float2 containing the x and y axis scaling factors.</param>
        /// <returns>The float2x2 matrix representing a non-uniform scale.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x2 Scale(fp2 v) =>
            Scale(v.x, v.y);
    }
}