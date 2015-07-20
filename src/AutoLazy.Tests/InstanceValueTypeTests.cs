using System;
using System.Linq;
using NUnit.Framework;

namespace AutoLazy.Tests
{
    [TestFixture]
    public class InstanceValueTypeTests
    {
        private int _getGuidCount;
        private int _guidPropCount;

        [Lazy]
        public Guid GetGuid()
        {
            ++_getGuidCount;
            return Guid.NewGuid();
        }

        [Lazy]
        public Guid GuidProp
        {
            get
            {
                ++_guidPropCount;
                return Guid.NewGuid();
            }
        }

        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, _getGuidCount);
            var first = GetGuid();
            Assert.AreEqual(1, _getGuidCount);
            var second = GetGuid();
            Assert.AreEqual(1, _getGuidCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, _guidPropCount);
            var first = GuidProp;
            Assert.AreEqual(1, _guidPropCount);
            var second = GuidProp;
            Assert.AreEqual(1, _guidPropCount);
            Assert.AreEqual(first, second);
        }
    }

}
