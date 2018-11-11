using NUnit.Framework;

namespace AutoLazy.Tests.NonGeneric
{
    [TestFixture]
    public class NonGenericTests
    {
        #region Instantial

        [Test]
        public void MockNonGeneric_with_int_should_be_lazy()
        {
            var target = new MockNonGeneric(() => 123);
            Assert.AreEqual(0, target.GetValueCount);
            var first = target.GetValue();
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, target.GetValueCount);
            var second = target.GetValue();
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, target.GetValueCount);
        }

        [Test]
        public void MockNonGenericKeyed_with_int_should_be_lazy()
        {
            var target = new MockNonGenericKeyed(() => 123);
            Assert.AreEqual(0, target.GetValueCount);
            var first = target.GetValue(1);
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, target.GetValueCount);
            var second = target.GetValue(1);
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, target.GetValueCount);
        }

        #endregion
        
        #region Static

        [Test]
        public void MockNonGenericStatic_with_int_should_be_lazy()
        {
            MockNonGenericStatic.Factory = () => 123;
            Assert.AreEqual(0, MockNonGenericStatic.GetValueCount);
            var first = MockNonGenericStatic.GetValue();
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, MockNonGenericStatic.GetValueCount);
            var second = MockNonGenericStatic.GetValue();
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, MockNonGenericStatic.GetValueCount);
        }

        [Test]
        public void MockNonGenericStaticKeyed_with_int_should_be_lazy()
        {
            MockNonGenericStaticKeyed.Factory = i => 123;
            Assert.AreEqual(0, MockNonGenericStaticKeyed.GetValueCount);
            var first = MockNonGenericStaticKeyed.GetValue(1);
            Assert.AreEqual(123, first);
            Assert.AreEqual(1, MockNonGenericStaticKeyed.GetValueCount);
            var second = MockNonGenericStaticKeyed.GetValue(1);
            Assert.AreEqual(first, second);
            Assert.AreEqual(1, MockNonGenericStaticKeyed.GetValueCount);
        }

        #endregion
    }
}
