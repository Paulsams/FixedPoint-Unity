using System;
using System.Globalization;
using NUnit.Framework;

namespace Paulsams.FixedPoint.Tests
{
    public class FixedPointTests
    {
        private const string _precisionFormat = "F4";
        private const int _rounding = 5;

        [Test]
        public void ToStringTest()
        {
            var originalFp = fp._1 - fp._0_01;
            Assert.AreEqual(
                Math.Round(originalFp.AsDouble, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture),
                "0.9900"
            );

            originalFp = fp._1 - fp._0_01 * fp._0_01;
            Assert.AreEqual(
                Math.Round(originalFp.AsDouble, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture),
                "0.9999"
            );

            originalFp = fp._1;
            Assert.AreEqual(
                Math.Round(originalFp.AsDouble, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture),
                "1.0000"
            );

            originalFp = fp._1 + fp._0_01;
            Assert.AreEqual(
                Math.Round(originalFp.AsDouble, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture),
                "1.0100"
            );

            originalFp = fp._1 + fp._0_01 * fp._0_01;
            Assert.AreEqual(
                Math.Round(originalFp.AsDouble, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture),
                "1.0001"
            );

            originalFp = fp._0_01;
            Assert.AreEqual(
                Math.Round(originalFp.AsDouble, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture),
                "0.0100"
            );

            originalFp = fp._0_50;
            Assert.AreEqual(
                Math.Round(originalFp.AsDouble, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture),
                "0.5000"
            );
        }

        [Test]
        public void SlowStringParsingTest()
        {
            Assert.AreEqual(fp.Parse("5").AsFloatRounded, 5f, 0.0001f);
            Assert.AreEqual(fp.Parse("5.").AsFloatRounded, 5f, 0.0001f);
            Assert.AreEqual(fp.Parse(".1").AsFloatRounded, 0.1f, 0.0001f);
            Assert.AreEqual(fp.Parse("5.1").AsFloatRounded, 5.1f, 0.0001f);
            Assert.AreEqual(fp.Parse("5.45111111").AsFloatRounded, 5.4511f, 0.0001f);

            Assert.AreEqual(fp.Parse("-5").AsFloatRounded, -5f, 0.0001f);
            Assert.AreEqual(fp.Parse("-5.").AsFloatRounded, -5f, 0.0001f);
            Assert.AreEqual(fp.Parse("-.1").AsFloatRounded, -0.1f, 0.0001f);
            Assert.AreEqual(fp.Parse("-5.1").AsFloatRounded, -5.1f, 0.0001f);
        }

        [Test]
        public void SlowFromToStringTest()
        {
            var from = -100.0;
            var to = 100.0;
            var delta = 0.0001;

            for (var v = from; v < to; v += delta)
            {
                var parsedString = Math.Round(v, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture);
                var parsedFp = fp.Parse(parsedString);
                var convertedBackFloat = parsedFp.AsDouble;
                Assert.AreEqual(convertedBackFloat, v, 0.0001f);
            }
        }

        [Test]
        public void FromToStringTest()
        {
            var from = -100.0;
            var to = 100.0;
            var delta = 0.0001;

            for (var v = from; v < to; v += delta)
            {
                var parsedString = Math.Round(v, _rounding).ToString(_precisionFormat, CultureInfo.InvariantCulture);
                var parsedFp = fp.ParseUnsafe(parsedString);
                var convertedBackFloat = parsedFp.AsDouble;
                Assert.AreEqual(convertedBackFloat, v, 0.0001);
            }
        }

        [Test]
        public void FromFloatTest()
        {
            var from = -100.0;
            var to = 100.0;
            var delta = 0.0001;

            for (var v = from; v < to; v += delta)
            {
                var parsedFp = fp.ParseUnsafe((float)v);
                var convertedBackFloat = parsedFp.AsDouble;
                Assert.AreEqual(convertedBackFloat, v, 0.0001);
            }
        }

        [Test]
        public void AsIntTest()
        {
            var from = -65000f;
            var to = 65000;
            var delta = 0.1f;

            for (float v = from; v < to; v += delta)
            {
                var originalInt = (int)Math.Floor(v);
                var parsedFp = fp.ParseUnsafe(v);
                var convertedBack = parsedFp.AsInt;
                Assert.AreEqual(convertedBack, originalInt);
            }
        }
    }
}