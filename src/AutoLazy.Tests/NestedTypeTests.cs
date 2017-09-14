using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AutoLazy.Tests
{
    [TestFixture]
    public class NestedTypeTests
    {
        private class Test
        {
            public static int Count;

            [Lazy]
            public static int GetLazy()
            {
                return ++Count;
            }
        }

        [Test]
        public void Nested_types_should_be_instrumented()
        {
            Assert.AreEqual(1, Test.GetLazy());
            Assert.AreEqual(1, Test.GetLazy());
        }
    }
}
