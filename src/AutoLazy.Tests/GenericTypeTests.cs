using System;
using System.Linq;
using NUnit.Framework;

namespace AutoLazy.Tests
{
    [TestFixture]
    public class GenericTypeTests
    {
        [Test]
        public void MockGeneric_with_int_should_be_lazy()
        {
            var target = new MockGeneric<int>(() => 123);
            Assert.AreEqual(0, target.GetValueCount);
            var first = target.GetValue();
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, target.GetValueCount);
            var second = target.GetValue();
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, target.GetValueCount);
        }

        [Test]
        public void MockGeneric_with_string_should_be_lazy()
        {
            var target = new MockGeneric<string>(() => Guid.NewGuid().ToString("n"));
            Assert.AreEqual(0, target.GetValueCount);
            var first = target.GetValue();
            Assert.AreNotEqual(null, first);
            Assert.AreEqual(1, target.GetValueCount);
            var second = target.GetValue();
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, target.GetValueCount);
        }
    }
}
