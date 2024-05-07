using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Paulsams.FixedPoint.LUTGenerator
{
    public static class Writer
    {
        public const string Header =
            "namespace FixedPoint {\n    public static partial class fixlut {\n        public static readonly";

        public const string Footer = "\n        };\n    }\n}";
        public const int EntriesPerLine = 10;

        public static string Output = string.Empty;
        public static int EntryIndex = 0;
        public static int MaxEntryIndex;

        private static readonly Dictionary<Type, string> _friendlyNames = new Dictionary<Type, string>
        {
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(byte)] = "byte",
            [typeof(sbyte)] = "sbyte",
        };

        public static void Write(IList list, string fieldName)
        {
            EntryIndex = 0;
            MaxEntryIndex = list.Count - 1;
            Output = $"{Header} {_friendlyNames[list[0].GetType()]}[] {fieldName} = {{";

            foreach (var i in list)
                AddEntry(i.ToString());

            Output += Footer;

            File.WriteAllText(Path.Combine(Paths.CodeGen.ToLUTGeneration, $"{fieldName}.cs"), Output);
        }

        public static void AddEntry(string entry)
        {
            Output += EntryIndex % EntriesPerLine == 0 ? "\n            " : " ";
            Output += entry;

            if (EntryIndex != MaxEntryIndex)
                Output += ",";

            EntryIndex++;
        }
    }
}