using NUnit.Framework;
using UnityEngine;

namespace Paulsams.FixedPoint.Tests {
    public class RandomTests {
        [Test]
        public void BoolTest()
        {
            var random = new Random(645);
            Assert.AreEqual(true, random.NextBool());
            
            random.SetState(435);
            Assert.AreEqual(false, random.NextBool());
        }
        
        [Test]
        public void IntTest()
        {
            var random = new Random(645);
            Assert.AreEqual(-1975191795, random.NextInt());
            
            random.SetState(435);
            Assert.AreEqual(-2030414680, random.NextInt());
        }
        
        [Test]
        public void IntMaxTest()
        {
            var random = new Random(345345346);
            for (uint i = 5; i < 100; i++)
                Assert.Less(random.NextInt(30), 31);
        }
        
        [Test]
        public void IntMinMaxTest()
        {
            var random = new Random(345345346);
            for (var i = 0; i < 100; i++)
                Assert.That(random.NextInt(-30, 30), Is.InRange(-30, 30));
        }
        
        [Test]
        public void FpTest()
        {
            var random = new Random(645);
            Assert.AreEqual(fp.ParseRaw(2628L),  random.NextFp());

            random.SetState(435);
            Assert.AreEqual(fp.ParseRaw(1786L), random.NextFp());
        }
        
        [Test]
        public void FpMaxTest()
        {
            var random = new Random(345345346);
            for (uint i = 5; i < 100; i++) {
                var val = random.NextFp(fp._100);
                Assert.That(val, Is.LessThan(fp._100));
            }
        }
        
        [Test]
        public void FpMinMaxTest()
        {
            var random = new Random(345345346);
            for (uint i = 5; i < 100; i++) {
                var val = random.NextFp(fp._99, fp._100);
                Assert.That(val, Is.InRange(fp._99, fp._100));
            }
        }
    }
}