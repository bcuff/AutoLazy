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

        [Test]
        public void MockGeneric_with_null_string_should_be_lazy()
        {
            var target = new MockGeneric<string>(() => null);

            Assert.AreEqual(0, target.GetValueCount);

            var first = target.GetValue();

            Assert.AreEqual(1, target.GetValueCount);

            var second = target.GetValue();

            Assert.AreEqual(first, second);
            Assert.AreEqual(1, target.GetValueCount);
        }

        [Test]
        public void MockGeneric_with_exception_string_should_be_lazy()
        {
            var target = new MockGeneric<object>(() => throw new ExpectedException());

            Assert.AreEqual(0, target.GetValueCount);

            object first = null;
            ExpectedException firstException = null;
            try
            {
                first = target.GetValue();
            }
            catch (ExpectedException e )
            {
                firstException = e;
            }

            Assert.IsNotNull(firstException);
            Assert.AreEqual(1, target.GetValueCount);

            object second = null;
            ExpectedException secondException = null;
            try
            {
                second = target.GetValue();
            }
            catch (ExpectedException e)
            {
                secondException = e;
            }

            Assert.IsNotNull(secondException);
            Assert.AreSame(firstException, secondException );
            Assert.AreEqual(1, target.GetValueCount);
            Assert.AreEqual(first, second);

        }

    }

    public class ExpectedException:Exception
    {

    }

}
