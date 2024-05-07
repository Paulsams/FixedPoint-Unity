using System;
using System.Collections.Generic;

namespace Paulsams.FixedPoint.LUTGenerator
{
    public static class Data
    {
        public const int Precision = 16;
        public const long One = 1 << Precision;

        public const int SinValueCount = 512;
        public const int SinCosValueCount = 512;
        public const int TanValueCount = 512;
        public const int AcosValueCount = 512;
        public const int AsinValueCount = 512;

        public static readonly List<int> SinLut = new List<int>(SinValueCount + 1);
        public static readonly List<int> SinCosLut = new List<int>(SinCosValueCount * 2 + 2);
        public static readonly List<int> TanLut = new List<int>(TanValueCount + 1);
        public static readonly List<int> AcosLut = new List<int>(AcosValueCount + 2);
        public static readonly List<int> AsinLut = new List<int>(AsinValueCount + 2);

        static Data()
        {
            for (var i = 0; i < SinCosValueCount; i++)
            {
                var angle = 2 * Math.PI * i / SinCosValueCount;

                var sinValue = Math.Sin(angle);
                var movedSin = sinValue * One;
                var roundedSin = movedSin > 0 ? movedSin + 0.5f : movedSin - 0.5f;
                SinLut.Add((int)roundedSin);
            }

            SinLut.Add(SinLut[0]);

            for (var i = 0; i < SinCosValueCount; i++)
            {
                var angle = 2 * Math.PI * i / SinCosValueCount;

                var sinValue = Math.Sin(angle);
                var movedSin = sinValue * One;
                var roundedSin = movedSin > 0 ? movedSin + 0.5f : movedSin - 0.5f;
                SinCosLut.Add((int)roundedSin);

                var cosValue = Math.Cos(angle);
                var movedCos = cosValue * One;
                var roundedCos = movedCos > 0 ? movedCos + 0.5f : movedCos - 0.5f;
                SinCosLut.Add((int)roundedCos);
            }

            SinCosLut.Add(SinCosLut[0]);
            SinCosLut.Add(SinCosLut[1]);

            for (var i = 0; i < TanValueCount; i++)
            {
                var angle = 2 * Math.PI * i / TanValueCount;

                var value = Math.Tan(angle);
                var moved = value * One;
                var rounded = moved > 0 ? moved + 0.5f : moved - 0.5f;
                TanLut.Add((int)rounded);
            }

            TanLut.Add(TanLut[0]);

            for (var i = 0; i < AsinValueCount; i++)
            {
                var angle = 2f * i / AsinValueCount;
                angle -= 1;

                if (i == AsinValueCount - 1)
                    angle = 1;

                var value = Math.Asin(angle);
                var moved = value * One;
                var rounded = moved > 0 ? moved + 0.5f : moved - 0.5f;
                AsinLut.Add((int)rounded);
            }

            //Special handling for value of 1, as graph is not symmetric
            AsinLut.Add(AsinLut[AsinValueCount - 1]);
            AsinLut.Add(AsinLut[AsinValueCount - 1]);

            for (var i = 0; i < AcosValueCount; i++)
            {
                var angle = 2f * i / AcosValueCount;
                angle -= 1;

                if (i == AcosValueCount - 1)
                    angle = 1;

                var value = Math.Acos(angle);
                var moved = value * One;
                var rounded = moved > 0 ? moved + 0.5f : moved - 0.5f;
                AcosLut.Add((int)rounded);
            }

            //Special handling for value of 1, as graph is not symmetric
            AcosLut.Add(AcosLut[AcosValueCount - 1]);
            AcosLut.Add(AcosLut[AcosValueCount - 1]);
        }
    }
}