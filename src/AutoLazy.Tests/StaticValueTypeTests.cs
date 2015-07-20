using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AutoLazy.Tests
{
    [TestFixture]
    public class StaticValueTypeTests
    {
        private static int _getGuidCount;
        private static int _guidPropCount;

        [Lazy]
        public static Guid GetGuid()
        {
            ++_getGuidCount;
            return Guid.NewGuid();
        }

        [Lazy]
        public static Guid GuidProp
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
