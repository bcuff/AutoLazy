using System;
using NUnit.Framework;

namespace AutoLazy.Tests.Generic
{
    [TestFixture]
    public class GenericTests
    {
        #region Instantial

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

        [Test]
        public void MockGenericKeyed_with_int_should_be_lazy()
        {
            var target = new MockGenericKeyed<int>(() => 123);
            Assert.AreEqual(0, target.GetValueCount);
            var first = target.GetValue(1);
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, target.GetValueCount);
            var second = target.GetValue(1);
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, target.GetValueCount);
        }

        [Test]
        public void MockGenericKeyed_with_string_should_be_lazy()
        {
            var target = new MockGenericKeyed<string>(() => Guid.NewGuid().ToString("n"));
            Assert.AreEqual(0, target.GetValueCount);
            var first = target.GetValue(1);
            Assert.AreNotEqual(null, first);
            Assert.AreEqual(1, target.GetValueCount);
            var second = target.GetValue(1);
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, target.GetValueCount);
        }

        #endregion

        #region Static

        [Test]
        public void MockGenericStatic_with_int_should_be_lazy()
        {
            MockGenericStatic<int>.Factory = () => 123;
            Assert.AreEqual(0, MockGenericStatic<int>.GetValueCount);
            var first = MockGenericStatic<int>.GetValue();
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, MockGenericStatic<int>.GetValueCount);
            var second = MockGenericStatic<int>.GetValue();
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, MockGenericStatic<int>.GetValueCount);
        }

        [Test]
        public void MockGenericStatic_with_string_should_be_lazy()
        {
            MockGenericStatic<string>.Factory = () => Guid.NewGuid().ToString("n");
            Assert.AreEqual(0, MockGenericStatic<string>.GetValueCount);
            var first = MockGenericStatic<string>.GetValue();
            Assert.AreNotEqual(null, first);
            Assert.AreEqual(1, MockGenericStatic<string>.GetValueCount);
            var second = MockGenericStatic<string>.GetValue();
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, MockGenericStatic<string>.GetValueCount);
        }

        [Test]
        public void MockGenericStaticKeyed_with_int_should_be_lazy()
        {
            MockGenericStaticKeyed<int>.Factory = i => 123;
            Assert.AreEqual(0, MockGenericStaticKeyed<int>.GetValueCount);
            var first = MockGenericStaticKeyed<int>.GetValue(1);
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, MockGenericStaticKeyed<int>.GetValueCount);
            var second = MockGenericStaticKeyed<int>.GetValue(1);
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, MockGenericStaticKeyed<int>.GetValueCount);
        }

        [Test]
        public void MockGenericStaticKeyed_with_string_should_be_lazy()
        {
            MockGenericStaticKeyed<string>.Factory = i => Guid.NewGuid().ToString("n");
            Assert.AreEqual(0, MockGenericStaticKeyed<string>.GetValueCount);
            var first = MockGenericStaticKeyed<string>.GetValue("test");
            Assert.AreNotEqual(null, first);
            Assert.AreEqual(1, MockGenericStaticKeyed<string>.GetValueCount);
            var second = MockGenericStaticKeyed<string>.GetValue("test");
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, MockGenericStaticKeyed<string>.GetValueCount);
        }

        #endregion
    }
}
