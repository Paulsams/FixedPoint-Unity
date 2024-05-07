using System.IO;
using UnityEngine;

namespace Paulsams.FixedPoint
{
    public static class Paths
    {
        public static readonly string ToPackage =
            Path.Combine(Application.dataPath.Remove(Application.dataPath.Length - 6),
                "Packages", "com.paulsams.fixed-point");

        public static readonly string ToRuntime = Path.Combine(ToPackage, "Runtime");

        public static class CodeGen
        {
            public static readonly string ToLUTGeneration = Path.Combine(ToRuntime, "LUT");
        }

        public static class Tests
        {
            public static readonly string ToOutputFolderForPlots = Path.Combine(ToPackage, "Tests", "Output~");
        }
    }
}