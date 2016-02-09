 
using System;
using NUnit.Framework;

namespace AutoLazy.Tests
{
    [TestFixture]
    public class InstanceGuidTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static Guid GetGuid()
        {
            ++_getCount;
            return Guid.NewGuid();
        }

        [Lazy]
        public static Guid GuidProp
        {
            get
            {
                ++_propCount;
                return Guid.NewGuid();
            }
        }

        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetGuid();
            Assert.AreEqual(1, _getCount);
            var second = GetGuid();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = GuidProp;
            Assert.AreEqual(1, _propCount);
            var second = GuidProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

    [TestFixture]
    public class StaticGuidTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static Guid GetGuid()
        {
            ++_getCount;
            return Guid.NewGuid();
        }

        [Lazy]
        public static Guid GuidProp
        {
            get
            {
                ++_propCount;
                return Guid.NewGuid();
            }
        }

        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetGuid();
            Assert.AreEqual(1, _getCount);
            var second = GetGuid();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = GuidProp;
            Assert.AreEqual(1, _propCount);
            var second = GuidProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

    [TestFixture]
    public class InstanceStringTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static String GetString()
        {
            ++_getCount;
            return Guid.NewGuid().ToString();
        }

        [Lazy]
        public static String StringProp
        {
            get
            {
                ++_propCount;
                return Guid.NewGuid().ToString();
            }
        }

        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetString();
            Assert.AreEqual(1, _getCount);
            var second = GetString();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = StringProp;
            Assert.AreEqual(1, _propCount);
            var second = StringProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

    [TestFixture]
    public class StaticStringTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static String GetString()
        {
            ++_getCount;
            return Guid.NewGuid().ToString();
        }

        [Lazy]
        public static String StringProp
        {
            get
            {
                ++_propCount;
                return Guid.NewGuid().ToString();
            }
        }

        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetString();
            Assert.AreEqual(1, _getCount);
            var second = GetString();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = StringProp;
            Assert.AreEqual(1, _propCount);
            var second = StringProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

    [TestFixture]
    public class InstanceGuidWithTryTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static Guid GetGuid()
        {
            try {
            ++_getCount;
            return Guid.NewGuid();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
        }

        [Lazy]
        public static Guid GuidProp
        {
            get
            {
            try {
                ++_propCount;
                return Guid.NewGuid();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
            }
        }

        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetGuid();
            Assert.AreEqual(1, _getCount);
            var second = GetGuid();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = GuidProp;
            Assert.AreEqual(1, _propCount);
            var second = GuidProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

    [TestFixture]
    public class StaticGuidWithTryTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static Guid GetGuid()
        {
            try {
            ++_getCount;
            return Guid.NewGuid();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
        }

        [Lazy]
        public static Guid GuidProp
        {
            get
            {
            try {
                ++_propCount;
                return Guid.NewGuid();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
            }
        }

        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetGuid();
            Assert.AreEqual(1, _getCount);
            var second = GetGuid();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = GuidProp;
            Assert.AreEqual(1, _propCount);
            var second = GuidProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

    [TestFixture]
    public class InstanceStringWithTryTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static String GetString()
        {
            try {
            ++_getCount;
            return Guid.NewGuid().ToString();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
        }

        [Lazy]
        public static String StringProp
        {
            get
            {
            try {
                ++_propCount;
                return Guid.NewGuid().ToString();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
            }
        }

        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetString();
            Assert.AreEqual(1, _getCount);
            var second = GetString();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = StringProp;
            Assert.AreEqual(1, _propCount);
            var second = StringProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

    [TestFixture]
    public class StaticStringWithTryTests
    {
        private static int _getCount;
        private static int _propCount;

        [Lazy]
        public static String GetString()
        {
            try {
            ++_getCount;
            return Guid.NewGuid().ToString();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
        }

        [Lazy]
        public static String StringProp
        {
            get
            {
            try {
                ++_propCount;
                return Guid.NewGuid().ToString();
            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }
            }
        }

        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, _getCount);
            var first = GetString();
            Assert.AreEqual(1, _getCount);
            var second = GetString();
            Assert.AreEqual(1, _getCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, _propCount);
            var first = StringProp;
            Assert.AreEqual(1, _propCount);
            var second = StringProp;
            Assert.AreEqual(1, _propCount);
            Assert.AreEqual(first, second);
        }
    }

}