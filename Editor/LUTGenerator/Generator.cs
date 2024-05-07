namespace Paulsams.FixedPoint.LUTGenerator
{
    internal static class Generator
    {
        public static void Main()
        {
            Writer.Write(Data.SinLut, "SinLut");
            Writer.Write(Data.SinCosLut, "SinCosLut");
            Writer.Write(Data.TanLut, "TanLut");
            Writer.Write(Data.AcosLut, "AcosLut");
            Writer.Write(Data.AsinLut, "AsinLut");
        }
    }
}