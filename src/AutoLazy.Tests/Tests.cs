











 

using System;
using NUnit.Framework;

namespace AutoLazy.Tests
{

    public static class MockStaticGuid
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static Guid GetGuid()
        {

            ++GetCount;
            return Guid.NewGuid();

        }

        [Lazy]
        public static Guid GuidProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid();

            }
        }

        [Lazy]
        public static Guid GetGuidByKey(Guid key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class StaticGuidTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuid.GetCount);
            var first = MockStaticGuid.GetGuid();
            Assert.AreEqual(1, MockStaticGuid.GetCount);
            var second = MockStaticGuid.GetGuid();
            Assert.AreEqual(1, MockStaticGuid.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuid.PropCount);
            var first = MockStaticGuid.GuidProp;
            Assert.AreEqual(1, MockStaticGuid.PropCount);
            var second = MockStaticGuid.GuidProp;
            Assert.AreEqual(1, MockStaticGuid.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuid.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = MockStaticGuid.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuid.GetKeyedCount);
            var second = MockStaticGuid.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuid.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticGuid.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuid.GetKeyedCount);
            second = MockStaticGuid.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuid.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public static class MockStaticGuidGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static Guid GetGuid()
        {

            ++GetCount;
            return Guid.NewGuid();

        }

        [Lazy]
        public static Guid GuidProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid();

            }
        }

        [Lazy]
        public static Guid GetGuidByKey(Guid key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class StaticGuidGenericTypeTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidGenericType<System.Guid>.GetCount);
            var first = MockStaticGuidGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidGenericType<System.Guid>.GetCount);
            var second = MockStaticGuidGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidGenericType<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidGenericType<System.Guid>.PropCount);
            var first = MockStaticGuidGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidGenericType<System.Guid>.PropCount);
            var second = MockStaticGuidGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidGenericType<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidGenericType<System.Guid>.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = MockStaticGuidGenericType<System.Guid>.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuidGenericType<System.Guid>.GetKeyedCount);
            var second = MockStaticGuidGenericType<System.Guid>.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuidGenericType<System.Guid>.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticGuidGenericType<System.Guid>.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuidGenericType<System.Guid>.GetKeyedCount);
            second = MockStaticGuidGenericType<System.Guid>.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuidGenericType<System.Guid>.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public static class MockStaticGuidWithTry
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static Guid GetGuid()
        {

            try {

            ++GetCount;
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

                ++PropCount;
                return Guid.NewGuid();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public static Guid GetGuidByKey(Guid key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class StaticGuidWithTryTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidWithTry.GetCount);
            var first = MockStaticGuidWithTry.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTry.GetCount);
            var second = MockStaticGuidWithTry.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTry.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidWithTry.PropCount);
            var first = MockStaticGuidWithTry.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTry.PropCount);
            var second = MockStaticGuidWithTry.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTry.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidWithTry.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = MockStaticGuidWithTry.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuidWithTry.GetKeyedCount);
            var second = MockStaticGuidWithTry.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuidWithTry.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticGuidWithTry.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuidWithTry.GetKeyedCount);
            second = MockStaticGuidWithTry.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuidWithTry.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public static class MockStaticGuidWithTryGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static Guid GetGuid()
        {

            try {

            ++GetCount;
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

                ++PropCount;
                return Guid.NewGuid();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public static Guid GetGuidByKey(Guid key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class StaticGuidWithTryGenericTypeTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidWithTryGenericType<System.Guid>.GetCount);
            var first = MockStaticGuidWithTryGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTryGenericType<System.Guid>.GetCount);
            var second = MockStaticGuidWithTryGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTryGenericType<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidWithTryGenericType<System.Guid>.PropCount);
            var first = MockStaticGuidWithTryGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTryGenericType<System.Guid>.PropCount);
            var second = MockStaticGuidWithTryGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTryGenericType<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticGuidWithTryGenericType<System.Guid>.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = MockStaticGuidWithTryGenericType<System.Guid>.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuidWithTryGenericType<System.Guid>.GetKeyedCount);
            var second = MockStaticGuidWithTryGenericType<System.Guid>.GetGuidByKey(key1);
            Assert.AreEqual(1, MockStaticGuidWithTryGenericType<System.Guid>.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticGuidWithTryGenericType<System.Guid>.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuidWithTryGenericType<System.Guid>.GetKeyedCount);
            second = MockStaticGuidWithTryGenericType<System.Guid>.GetGuidByKey(key2);
            Assert.AreEqual(2, MockStaticGuidWithTryGenericType<System.Guid>.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public static class MockStaticString
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static String GetString()
        {

            ++GetCount;
            return Guid.NewGuid().ToString();

        }

        [Lazy]
        public static String StringProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid().ToString();

            }
        }

        [Lazy]
        public static String GetStringByKey(String key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class StaticStringTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticString.GetCount);
            var first = MockStaticString.GetString();
            Assert.AreEqual(1, MockStaticString.GetCount);
            var second = MockStaticString.GetString();
            Assert.AreEqual(1, MockStaticString.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticString.PropCount);
            var first = MockStaticString.StringProp;
            Assert.AreEqual(1, MockStaticString.PropCount);
            var second = MockStaticString.StringProp;
            Assert.AreEqual(1, MockStaticString.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticString.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = MockStaticString.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticString.GetKeyedCount);
            var second = MockStaticString.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticString.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticString.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticString.GetKeyedCount);
            second = MockStaticString.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticString.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public static class MockStaticStringGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static String GetString()
        {

            ++GetCount;
            return Guid.NewGuid().ToString();

        }

        [Lazy]
        public static String StringProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid().ToString();

            }
        }

        [Lazy]
        public static String GetStringByKey(String key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class StaticStringGenericTypeTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringGenericType<System.String>.GetCount);
            var first = MockStaticStringGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringGenericType<System.String>.GetCount);
            var second = MockStaticStringGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringGenericType<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringGenericType<System.String>.PropCount);
            var first = MockStaticStringGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringGenericType<System.String>.PropCount);
            var second = MockStaticStringGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringGenericType<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringGenericType<System.String>.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = MockStaticStringGenericType<System.String>.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticStringGenericType<System.String>.GetKeyedCount);
            var second = MockStaticStringGenericType<System.String>.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticStringGenericType<System.String>.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticStringGenericType<System.String>.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticStringGenericType<System.String>.GetKeyedCount);
            second = MockStaticStringGenericType<System.String>.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticStringGenericType<System.String>.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public static class MockStaticStringWithTry
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static String GetString()
        {

            try {

            ++GetCount;
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

                ++PropCount;
                return Guid.NewGuid().ToString();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public static String GetStringByKey(String key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class StaticStringWithTryTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringWithTry.GetCount);
            var first = MockStaticStringWithTry.GetString();
            Assert.AreEqual(1, MockStaticStringWithTry.GetCount);
            var second = MockStaticStringWithTry.GetString();
            Assert.AreEqual(1, MockStaticStringWithTry.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringWithTry.PropCount);
            var first = MockStaticStringWithTry.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTry.PropCount);
            var second = MockStaticStringWithTry.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTry.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringWithTry.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = MockStaticStringWithTry.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticStringWithTry.GetKeyedCount);
            var second = MockStaticStringWithTry.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticStringWithTry.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticStringWithTry.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticStringWithTry.GetKeyedCount);
            second = MockStaticStringWithTry.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticStringWithTry.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public static class MockStaticStringWithTryGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;
        public static int GetKeyedCount;

        [Lazy]
        public static String GetString()
        {

            try {

            ++GetCount;
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

                ++PropCount;
                return Guid.NewGuid().ToString();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public static String GetStringByKey(String key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class StaticStringWithTryGenericTypeTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringWithTryGenericType<System.String>.GetCount);
            var first = MockStaticStringWithTryGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringWithTryGenericType<System.String>.GetCount);
            var second = MockStaticStringWithTryGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringWithTryGenericType<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringWithTryGenericType<System.String>.PropCount);
            var first = MockStaticStringWithTryGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTryGenericType<System.String>.PropCount);
            var second = MockStaticStringWithTryGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTryGenericType<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {

            Assert.AreEqual(0, MockStaticStringWithTryGenericType<System.String>.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = MockStaticStringWithTryGenericType<System.String>.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticStringWithTryGenericType<System.String>.GetKeyedCount);
            var second = MockStaticStringWithTryGenericType<System.String>.GetStringByKey(key1);
            Assert.AreEqual(1, MockStaticStringWithTryGenericType<System.String>.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = MockStaticStringWithTryGenericType<System.String>.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticStringWithTryGenericType<System.String>.GetKeyedCount);
            second = MockStaticStringWithTryGenericType<System.String>.GetStringByKey(key2);
            Assert.AreEqual(2, MockStaticStringWithTryGenericType<System.String>.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceGuid
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public Guid GetGuid()
        {

            ++GetCount;
            return Guid.NewGuid();

        }

        [Lazy]
        public Guid GuidProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid();

            }
        }

        [Lazy]
        public Guid GetGuidByKey(Guid key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class InstanceGuidTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            var instance = new MockInstanceGuid();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            var instance = new MockInstanceGuid();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {
            var instance = new MockInstanceGuid();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceGuidGenericType<T>
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public Guid GetGuid()
        {

            ++GetCount;
            return Guid.NewGuid();

        }

        [Lazy]
        public Guid GuidProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid();

            }
        }

        [Lazy]
        public Guid GetGuidByKey(Guid key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class InstanceGuidGenericTypeTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            var instance = new MockInstanceGuidGenericType<System.Guid>();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            var instance = new MockInstanceGuidGenericType<System.Guid>();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {
            var instance = new MockInstanceGuidGenericType<System.Guid>();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceGuidWithTry
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public Guid GetGuid()
        {

            try {

            ++GetCount;
            return Guid.NewGuid();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }

        [Lazy]
        public Guid GuidProp
        {
            get
            {

            try {

                ++PropCount;
                return Guid.NewGuid();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public Guid GetGuidByKey(Guid key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class InstanceGuidWithTryTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            var instance = new MockInstanceGuidWithTry();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            var instance = new MockInstanceGuidWithTry();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {
            var instance = new MockInstanceGuidWithTry();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceGuidWithTryGenericType<T>
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public Guid GetGuid()
        {

            try {

            ++GetCount;
            return Guid.NewGuid();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }

        [Lazy]
        public Guid GuidProp
        {
            get
            {

            try {

                ++PropCount;
                return Guid.NewGuid();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public Guid GetGuidByKey(Guid key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class InstanceGuidWithTryGenericTypeTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            var instance = new MockInstanceGuidWithTryGenericType<System.Guid>();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetGuid();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            var instance = new MockInstanceGuidWithTryGenericType<System.Guid>();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.GuidProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetGuidByKey_should_be_lazy()
        {
            var instance = new MockInstanceGuidWithTryGenericType<System.Guid>();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid();
            var key2 = Guid.NewGuid();

            var first = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetGuidByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetGuidByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceString
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public String GetString()
        {

            ++GetCount;
            return Guid.NewGuid().ToString();

        }

        [Lazy]
        public String StringProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid().ToString();

            }
        }

        [Lazy]
        public String GetStringByKey(String key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class InstanceStringTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            var instance = new MockInstanceString();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            var instance = new MockInstanceString();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {
            var instance = new MockInstanceString();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceStringGenericType<T>
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public String GetString()
        {

            ++GetCount;
            return Guid.NewGuid().ToString();

        }

        [Lazy]
        public String StringProp
        {
            get
            {

                ++PropCount;
                return Guid.NewGuid().ToString();

            }
        }

        [Lazy]
        public String GetStringByKey(String key)
        {

            ++GetKeyedCount;
            return key;

        }
    }

    [TestFixture]
    public class InstanceStringGenericTypeTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            var instance = new MockInstanceStringGenericType<System.String>();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            var instance = new MockInstanceStringGenericType<System.String>();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {
            var instance = new MockInstanceStringGenericType<System.String>();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceStringWithTry
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public String GetString()
        {

            try {

            ++GetCount;
            return Guid.NewGuid().ToString();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }

        [Lazy]
        public String StringProp
        {
            get
            {

            try {

                ++PropCount;
                return Guid.NewGuid().ToString();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public String GetStringByKey(String key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class InstanceStringWithTryTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            var instance = new MockInstanceStringWithTry();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            var instance = new MockInstanceStringWithTry();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {
            var instance = new MockInstanceStringWithTry();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


    public class MockInstanceStringWithTryGenericType<T>
    {
        public int GetCount;
        public int PropCount;
        public int GetKeyedCount;

        [Lazy]
        public String GetString()
        {

            try {

            ++GetCount;
            return Guid.NewGuid().ToString();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }

        [Lazy]
        public String StringProp
        {
            get
            {

            try {

                ++PropCount;
                return Guid.NewGuid().ToString();

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

            }
        }

        [Lazy]
        public String GetStringByKey(String key)
        {

            try {

            ++GetKeyedCount;
            return key;

            } catch (Exception e) {
                throw new Exception("foo", e);
            } finally {
                Console.WriteLine("Finally!");
            }

        }
    }

    [TestFixture]
    public class InstanceStringWithTryGenericTypeTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            var instance = new MockInstanceStringWithTryGenericType<System.String>();
            Assert.AreEqual(0, instance.GetCount);
            var first = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            var second = instance.GetString();
            Assert.AreEqual(1, instance.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            var instance = new MockInstanceStringWithTryGenericType<System.String>();
            Assert.AreEqual(0, instance.PropCount);
            var first = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            var second = instance.StringProp;
            Assert.AreEqual(1, instance.PropCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GetStringByKey_should_be_lazy()
        {
            var instance = new MockInstanceStringWithTryGenericType<System.String>();
            Assert.AreEqual(0, instance.GetKeyedCount);
            var key1 = Guid.NewGuid().ToString();
            var key2 = Guid.NewGuid().ToString();

            var first = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            var second = instance.GetStringByKey(key1);
            Assert.AreEqual(1, instance.GetKeyedCount);
            Assert.AreEqual(first, second);

            first = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            second = instance.GetStringByKey(key2);
            Assert.AreEqual(2, instance.GetKeyedCount);
            Assert.AreEqual(first, second);
        }
    }


}