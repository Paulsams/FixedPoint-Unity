using System;
using NUnit.Framework;

namespace Paulsams.FixedPoint.Tests
{
    public class FixmathTests
    {
        [Test]
        public void CountLeadingZerosTest()
        {
            Assert.AreEqual(fixmath.CountLeadingZeroes(5435345), 9);
            Assert.AreEqual(fixmath.CountLeadingZeroes(4), 29);
        }

        [Test]
        public void ExpTest()
        {
            var from = -5f;
            var to = 5f;
            var delta = 0.001f;

            for (float v = from; v < to; v += delta)
            {
                var correctAnswer = (float)Math.Exp(v);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.Exp(parsedFp);
                Assert.AreEqual(answer.AsFloat, correctAnswer, 0.01f);
            }

            from = 5f;
            to = 5.33f;
            delta = 0.001f;

            for (float v = from; v < to; v += delta)
            {
                var correctAnswer = (float)Math.Exp(v);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.Exp(parsedFp);
                Assert.AreEqual(answer.AsFloat, correctAnswer, 1f);
            }
        }

        [Test]
        public void Atan_2Test()
        {
            var from = -1f;
            var to = 1f;
            var delta = 0.001f;

            for (float v = from; v < to; v += delta)
            {
                var correctAnswer = (float)Math.Atan(v);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.AtanApproximated(parsedFp);
                Assert.AreEqual(answer.AsFloat, correctAnswer, 0.01f);
            }
        }

        [Test]
        public void AtanTest()
        {
            var from = -1f;
            var to = 1f;
            var delta = 0.001f;

            for (float v = from; v < to; v += delta)
            {
                var correctAnswer = (float)Math.Atan(v);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.Atan(parsedFp);
                Assert.AreEqual(answer.AsFloat, correctAnswer, 0.01f);
            }
        }

        [Test]
        public void Atan2Test()
        {
            var from1 = 0.1f;
            var to1 = 10f;
            var from2 = 0.1f;
            var to2 = 10f;
            var delta = 0.01f;

            for (float x = from1; x < to1; x += delta)
            {
                for (float y = from2; y < to2; y += delta)
                {
                    var correctAnswer = (float)Math.Atan2(x, y);
                    var parsedFp1 = fp.ParseUnsafe(x);
                    var parsedFp2 = fp.ParseUnsafe(y);
                    var answer = fixmath.Atan2(parsedFp1, parsedFp2);
                    Assert.AreEqual(answer.AsFloat, correctAnswer, 0.01f);
                }
            }
        }

        [Test]
        public void TanTest()
        {
            for (long i = fp.minus_one.value; i <= fp._1.value; i++)
            {
                fp val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = fixmath.Tan(val);
                Assert.AreEqual(answer.AsDouble, Math.Tan(dValue), 0.001);
            }
        }

        [Test]
        public void AcosTest()
        {
            for (long i = fp.minus_one.value; i <= fp._1.value; i++)
            {
                fp val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = fixmath.Acos(val);
                Assert.AreEqual(answer.AsDouble, Math.Acos(dValue), 0.0001);
            }
        }

        [Test]
        public void AsinTest()
        {
            for (long i = fp.minus_one.value; i <= fp._1.value; i++)
            {
                fp val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = fixmath.Asin(val);
                Assert.AreEqual(answer.AsDouble, Math.Asin(dValue), 0.0001);
            }
        }

        [Test]
        public void CosTest()
        {
            for (long i = -fp.pi.value * 2; i <= fp.pi.value * 2; i++)
            {
                fp val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = fixmath.Cos(val);
                Assert.AreEqual(answer.AsDouble, Math.Cos(dValue), 0.001);
            }
        }

        [Test]
        public void SinTest()
        {
            for (long i = -fp.pi.value * 2; i <= fp.pi.value * 2; i++)
            {
                fp val;
                val.value = i;
                var dValue = val.AsDouble;
                var answer = fixmath.Sin(val);
                Assert.AreEqual(answer.AsDouble, Math.Sin(dValue), 0.001);
            }
        }

        [Test]
        public void SinCosTest()
        {
            for (long i = -fp.pi.value * 2; i <= fp.pi.value * 2; i++)
            {
                fp val;
                val.value = i;
                var dValue = val.AsDouble;

                fixmath.SinCos(val, out var sin, out var cos);
                Assert.AreEqual(sin.AsDouble, Math.Sin(dValue), 0.001);
                Assert.AreEqual(cos.AsDouble, Math.Cos(dValue), 0.001);
            }
        }

        [Test]
        public void RcpTest()
        {
            var value = fp._0_25;
            var result = fixmath.Rcp(value);
            Assert.AreEqual(result, fp._4);

            value = fp._4;
            result = fixmath.Rcp(value);
            Assert.AreEqual(result, fp._0_25);
        }

        [Test]
        public void SqrtTest()
        {
            var from = 0.1f;
            var to = 65000;
            var delta = 0.1f;

            for (float v = from; v < to; v += delta)
            {
                var correct = (float)Math.Sqrt(v);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.Sqrt(parsedFp);
                Assert.AreEqual(answer.AsFloat, correct, 0.01f);
            }
        }

        [Test]
        public void FloorTest()
        {
            var value = fp._0_25;
            var result = fixmath.Floor(value);
            Assert.AreEqual(result, fp._0);

            result = fixmath.Floor(-value);
            Assert.AreEqual(result, -fp._1);
        }

        [Test]
        public void CeilTest()
        {
            var value = fp._0_25;
            var result = fixmath.Ceil(value);
            Assert.AreEqual(result, fp._1);

            result = fixmath.Ceil(-fp._4 - fp._0_25);
            Assert.AreEqual(result, -fp._4);
        }

        [Test]
        public void RoundToIntTest()
        {
            var value = fp._5 + fp._0_25;
            var result = fixmath.RoundToInt(value);
            Assert.AreEqual(result, 5);

            result = fixmath.RoundToInt(value + fp._0_33);
            Assert.AreEqual(result, 6);

            result = fixmath.RoundToInt(value + fp._0_25);
            Assert.AreEqual(result, 6);
        }

        [Test]
        public void MinTest()
        {
            var value1 = fp._0_25;
            var value2 = fp._0_33;
            var result = fixmath.Min(value1, value2);
            Assert.AreEqual(result, value1);

            result = fixmath.Min(-value1, -value2);
            Assert.AreEqual(result, -value2);
        }

        [Test]
        public void MaxTest()
        {
            var value1 = fp._0_25;
            var value2 = fp._0_33;
            var result = fixmath.Max(value1, value2);
            Assert.AreEqual(result, value2);

            result = fixmath.Max(-value1, -value2);
            Assert.AreEqual(result, -value1);
        }

        [Test]
        public void AbsTest()
        {
            var from = -5;
            var to = 5;
            var delta = 0.1f;

            for (float v = from; v < to; v += delta)
            {
                var correctAnswer = Math.Abs(v);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.Abs(parsedFp);

                Assert.AreEqual(answer.AsFloat, correctAnswer, 0.1f);
            }
        }

        [Test]
        public void ClampTest()
        {
            var from = -5;
            var to = 5;
            var delta = 0.1f;

            for (float v = from; v < to; v += delta)
            {
                var correctAnswer = Math.Clamp(v, -3, 3);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.Clamp(parsedFp, -fp._3, fp._3);
                Assert.AreEqual(answer.AsFloat, correctAnswer, 0.1f);
            }
        }


        [Test]
        public void LerpTest()
        {
            var result = fixmath.Lerp(fp._2, fp._4, fp._0_25);
            Assert.AreEqual(result, fp._2 + fp._0_50);

            result = fixmath.Lerp(fp._2, fp._4, fp._0);
            Assert.AreEqual(result, fp._2);

            result = fixmath.Lerp(fp._2, fp._4, fp._1);
            Assert.AreEqual(result, fp._4);

            result = fixmath.Lerp(fp._2, fp._4, fp._0_50);
            Assert.AreEqual(result, fp._3);
        }

        [Test]
        public void MulAndDivideTest()
        {
            fp a = 5;

            {
                var result1 = a * fp._0_01;
                Assert.AreEqual(result1.AsFloat, 0.05f, 0.01f);

                var result2 = fp._0_01 * a;
                Assert.AreEqual(result2.AsFloat, 0.05f, 0.01f);

                var result3 = fp._0_01 * fp._0_01;
                Assert.AreEqual(result3.AsFloat, 0.001f, 0.002f);
            }

            {
                var result1 = a / fp._2;
                Assert.AreEqual(result1.AsFloat, 2.5f, 0.01f);

                var result2 = fp._0_01 / a;
                Assert.AreEqual(result2.AsFloat, 0.002f, 0.002f);
            }

            {
                var result1 = a / new fp2(10, 5);
                Assert.AreEqual(result1.x.AsFloat, 0.5f, 0.01f);
                Assert.AreEqual(result1.y.AsFloat, 1f, 0.01f);
            }
        }

        [Test]
        public void PlusAndMinisTest()
        {
            fp a = 5;

            var result1 = a + fp._0_01;
            Assert.AreEqual(result1.AsFloat, 5.01f, 0.01f);

            var result2 = a + fp._0_01;
            Assert.AreEqual(result2.AsFloat, 5.01f, 0.01f);

            var result3 = fp._0_01 + fp._0_01;
            Assert.AreEqual(result3.AsFloat, 0.02f, 0.002f);

            var result4 = a + fp._0_01;
            Assert.AreEqual(result4.AsFloat, 5.01f, 0.01f);

            var result5 = a + fp._0_01;
            Assert.AreEqual(result5.AsFloat, 5.01f, 0.01f);

            var result6 = fp._0_01 + fp._0_01;
            Assert.AreEqual(result6.AsFloat, 0.02f, 0.002f);
        }

        [Test]
        public void SignTest()
        {
            var from = -5;
            var to = 5;
            var delta = 0.12f;

            for (float v = from; v < to; v += delta)
            {
                var correctAnswer = Math.Sign(v);
                var parsedFp = fp.ParseUnsafe(v);
                var answer = fixmath.Sign(parsedFp);
                Assert.AreEqual(answer.AsFloat, correctAnswer, 0.1f);
            }
        }

        [Test]
        public void IsOppositeSignTest()
        {
            var result = fixmath.IsOppositeSign(fp._0_25, -fp._0_20);
            Assert.AreEqual(result, true);

            result = fixmath.IsOppositeSign(fp._0_25, fp._0_20);
            Assert.AreEqual(result, false);

            result = fixmath.IsOppositeSign(-fp._0_25, -fp._0_20);
            Assert.AreEqual(result, false);
        }
    }
}