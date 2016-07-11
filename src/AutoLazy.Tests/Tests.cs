











 

using System;
using NUnit.Framework;

namespace AutoLazy.Tests
{

    public static class MockStaticGuid
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public static class MockStaticGuidGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public static class MockStaticGuidWithTry
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public static class MockStaticGuidWithTryGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public static class MockStaticString
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public static class MockStaticStringGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public static class MockStaticStringWithTry
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public static class MockStaticStringWithTryGenericType<T>
    {
        public static int GetCount;
        public static int PropCount;

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
    }


    public class MockInstanceGuid
    {
        public int GetCount;
        public int PropCount;

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
    }


    public class MockInstanceGuidGenericType<T>
    {
        public int GetCount;
        public int PropCount;

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
    }


    public class MockInstanceGuidWithTry
    {
        public int GetCount;
        public int PropCount;

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
    }


    public class MockInstanceGuidWithTryGenericType<T>
    {
        public int GetCount;
        public int PropCount;

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
    }


    public class MockInstanceString
    {
        public int GetCount;
        public int PropCount;

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
    }


    public class MockInstanceStringGenericType<T>
    {
        public int GetCount;
        public int PropCount;

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
    }


    public class MockInstanceStringWithTry
    {
        public int GetCount;
        public int PropCount;

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
    }


    public class MockInstanceStringWithTryGenericType<T>
    {
        public int GetCount;
        public int PropCount;

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
    }


}