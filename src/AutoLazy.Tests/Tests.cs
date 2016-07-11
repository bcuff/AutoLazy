 
using System;
using NUnit.Framework;

namespace AutoLazy.Tests
{
    public class MockStaticGuid
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

    public class MockStaticGuidGenericMethod
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
    public class StaticGuidGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidGenericMethod.GetCount);
            var first = MockStaticGuidGenericMethod.GetGuid();
            Assert.AreEqual(1, MockStaticGuidGenericMethod.GetCount);
            var second = MockStaticGuidGenericMethod.GetGuid();
            Assert.AreEqual(1, MockStaticGuidGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidGenericMethod.PropCount);
            var first = MockStaticGuidGenericMethod.GuidProp;
            Assert.AreEqual(1, MockStaticGuidGenericMethod.PropCount);
            var second = MockStaticGuidGenericMethod.GuidProp;
            Assert.AreEqual(1, MockStaticGuidGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockStaticGuidGenericType<T>
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

    public class MockStaticGuidGenericTypeGenericMethod<T>
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
    public class StaticGuidGenericTypeGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidGenericTypeGenericMethod<System.Guid>.GetCount);
            var first = MockStaticGuidGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidGenericTypeGenericMethod<System.Guid>.GetCount);
            var second = MockStaticGuidGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidGenericTypeGenericMethod<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidGenericTypeGenericMethod<System.Guid>.PropCount);
            var first = MockStaticGuidGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidGenericTypeGenericMethod<System.Guid>.PropCount);
            var second = MockStaticGuidGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidGenericTypeGenericMethod<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockStaticGuidWithTry
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

    public class MockStaticGuidWithTryGenericMethod
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
    public class StaticGuidWithTryGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidWithTryGenericMethod.GetCount);
            var first = MockStaticGuidWithTryGenericMethod.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTryGenericMethod.GetCount);
            var second = MockStaticGuidWithTryGenericMethod.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTryGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidWithTryGenericMethod.PropCount);
            var first = MockStaticGuidWithTryGenericMethod.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTryGenericMethod.PropCount);
            var second = MockStaticGuidWithTryGenericMethod.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTryGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockStaticGuidWithTryGenericType<T>
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

    public class MockStaticGuidWithTryGenericTypeGenericMethod<T>
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
    public class StaticGuidWithTryGenericTypeGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.GetCount);
            var first = MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.GetCount);
            var second = MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.PropCount);
            var first = MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.PropCount);
            var second = MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockStaticGuidWithTryGenericTypeGenericMethod<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockStaticString
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

    public class MockStaticStringGenericMethod
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
    public class StaticStringGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringGenericMethod.GetCount);
            var first = MockStaticStringGenericMethod.GetString();
            Assert.AreEqual(1, MockStaticStringGenericMethod.GetCount);
            var second = MockStaticStringGenericMethod.GetString();
            Assert.AreEqual(1, MockStaticStringGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringGenericMethod.PropCount);
            var first = MockStaticStringGenericMethod.StringProp;
            Assert.AreEqual(1, MockStaticStringGenericMethod.PropCount);
            var second = MockStaticStringGenericMethod.StringProp;
            Assert.AreEqual(1, MockStaticStringGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockStaticStringGenericType<T>
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

    public class MockStaticStringGenericTypeGenericMethod<T>
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
    public class StaticStringGenericTypeGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringGenericTypeGenericMethod<System.String>.GetCount);
            var first = MockStaticStringGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringGenericTypeGenericMethod<System.String>.GetCount);
            var second = MockStaticStringGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringGenericTypeGenericMethod<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringGenericTypeGenericMethod<System.String>.PropCount);
            var first = MockStaticStringGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringGenericTypeGenericMethod<System.String>.PropCount);
            var second = MockStaticStringGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringGenericTypeGenericMethod<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockStaticStringWithTry
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

    public class MockStaticStringWithTryGenericMethod
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
    public class StaticStringWithTryGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringWithTryGenericMethod.GetCount);
            var first = MockStaticStringWithTryGenericMethod.GetString();
            Assert.AreEqual(1, MockStaticStringWithTryGenericMethod.GetCount);
            var second = MockStaticStringWithTryGenericMethod.GetString();
            Assert.AreEqual(1, MockStaticStringWithTryGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringWithTryGenericMethod.PropCount);
            var first = MockStaticStringWithTryGenericMethod.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTryGenericMethod.PropCount);
            var second = MockStaticStringWithTryGenericMethod.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTryGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockStaticStringWithTryGenericType<T>
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

    public class MockStaticStringWithTryGenericTypeGenericMethod<T>
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
    public class StaticStringWithTryGenericTypeGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringWithTryGenericTypeGenericMethod<System.String>.GetCount);
            var first = MockStaticStringWithTryGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringWithTryGenericTypeGenericMethod<System.String>.GetCount);
            var second = MockStaticStringWithTryGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockStaticStringWithTryGenericTypeGenericMethod<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockStaticStringWithTryGenericTypeGenericMethod<System.String>.PropCount);
            var first = MockStaticStringWithTryGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTryGenericTypeGenericMethod<System.String>.PropCount);
            var second = MockStaticStringWithTryGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockStaticStringWithTryGenericTypeGenericMethod<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuid
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
    public class InstanceGuidTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuid.GetCount);
            var first = MockInstanceGuid.GetGuid();
            Assert.AreEqual(1, MockInstanceGuid.GetCount);
            var second = MockInstanceGuid.GetGuid();
            Assert.AreEqual(1, MockInstanceGuid.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuid.PropCount);
            var first = MockInstanceGuid.GuidProp;
            Assert.AreEqual(1, MockInstanceGuid.PropCount);
            var second = MockInstanceGuid.GuidProp;
            Assert.AreEqual(1, MockInstanceGuid.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuidGenericMethod
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
    public class InstanceGuidGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidGenericMethod.GetCount);
            var first = MockInstanceGuidGenericMethod.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidGenericMethod.GetCount);
            var second = MockInstanceGuidGenericMethod.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidGenericMethod.PropCount);
            var first = MockInstanceGuidGenericMethod.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidGenericMethod.PropCount);
            var second = MockInstanceGuidGenericMethod.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuidGenericType<T>
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
    public class InstanceGuidGenericTypeTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidGenericType<System.Guid>.GetCount);
            var first = MockInstanceGuidGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidGenericType<System.Guid>.GetCount);
            var second = MockInstanceGuidGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidGenericType<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidGenericType<System.Guid>.PropCount);
            var first = MockInstanceGuidGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidGenericType<System.Guid>.PropCount);
            var second = MockInstanceGuidGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidGenericType<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuidGenericTypeGenericMethod<T>
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
    public class InstanceGuidGenericTypeGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidGenericTypeGenericMethod<System.Guid>.GetCount);
            var first = MockInstanceGuidGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidGenericTypeGenericMethod<System.Guid>.GetCount);
            var second = MockInstanceGuidGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidGenericTypeGenericMethod<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidGenericTypeGenericMethod<System.Guid>.PropCount);
            var first = MockInstanceGuidGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidGenericTypeGenericMethod<System.Guid>.PropCount);
            var second = MockInstanceGuidGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidGenericTypeGenericMethod<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuidWithTry
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
    public class InstanceGuidWithTryTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTry.GetCount);
            var first = MockInstanceGuidWithTry.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTry.GetCount);
            var second = MockInstanceGuidWithTry.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTry.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTry.PropCount);
            var first = MockInstanceGuidWithTry.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTry.PropCount);
            var second = MockInstanceGuidWithTry.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTry.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuidWithTryGenericMethod
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
    public class InstanceGuidWithTryGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTryGenericMethod.GetCount);
            var first = MockInstanceGuidWithTryGenericMethod.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericMethod.GetCount);
            var second = MockInstanceGuidWithTryGenericMethod.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTryGenericMethod.PropCount);
            var first = MockInstanceGuidWithTryGenericMethod.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericMethod.PropCount);
            var second = MockInstanceGuidWithTryGenericMethod.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuidWithTryGenericType<T>
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
    public class InstanceGuidWithTryGenericTypeTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTryGenericType<System.Guid>.GetCount);
            var first = MockInstanceGuidWithTryGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericType<System.Guid>.GetCount);
            var second = MockInstanceGuidWithTryGenericType<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericType<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTryGenericType<System.Guid>.PropCount);
            var first = MockInstanceGuidWithTryGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericType<System.Guid>.PropCount);
            var second = MockInstanceGuidWithTryGenericType<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericType<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceGuidWithTryGenericTypeGenericMethod<T>
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
    public class InstanceGuidWithTryGenericTypeGenericMethodTests
    {
        [Test]
        public void GetGuid_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.GetCount);
            var first = MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.GetCount);
            var second = MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.GetGuid();
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void GuidProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.PropCount);
            var first = MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.PropCount);
            var second = MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.GuidProp;
            Assert.AreEqual(1, MockInstanceGuidWithTryGenericTypeGenericMethod<System.Guid>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceString
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
    public class InstanceStringTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceString.GetCount);
            var first = MockInstanceString.GetString();
            Assert.AreEqual(1, MockInstanceString.GetCount);
            var second = MockInstanceString.GetString();
            Assert.AreEqual(1, MockInstanceString.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceString.PropCount);
            var first = MockInstanceString.StringProp;
            Assert.AreEqual(1, MockInstanceString.PropCount);
            var second = MockInstanceString.StringProp;
            Assert.AreEqual(1, MockInstanceString.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceStringGenericMethod
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
    public class InstanceStringGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringGenericMethod.GetCount);
            var first = MockInstanceStringGenericMethod.GetString();
            Assert.AreEqual(1, MockInstanceStringGenericMethod.GetCount);
            var second = MockInstanceStringGenericMethod.GetString();
            Assert.AreEqual(1, MockInstanceStringGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringGenericMethod.PropCount);
            var first = MockInstanceStringGenericMethod.StringProp;
            Assert.AreEqual(1, MockInstanceStringGenericMethod.PropCount);
            var second = MockInstanceStringGenericMethod.StringProp;
            Assert.AreEqual(1, MockInstanceStringGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceStringGenericType<T>
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
    public class InstanceStringGenericTypeTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringGenericType<System.String>.GetCount);
            var first = MockInstanceStringGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringGenericType<System.String>.GetCount);
            var second = MockInstanceStringGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringGenericType<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringGenericType<System.String>.PropCount);
            var first = MockInstanceStringGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringGenericType<System.String>.PropCount);
            var second = MockInstanceStringGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringGenericType<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceStringGenericTypeGenericMethod<T>
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
    public class InstanceStringGenericTypeGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringGenericTypeGenericMethod<System.String>.GetCount);
            var first = MockInstanceStringGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringGenericTypeGenericMethod<System.String>.GetCount);
            var second = MockInstanceStringGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringGenericTypeGenericMethod<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringGenericTypeGenericMethod<System.String>.PropCount);
            var first = MockInstanceStringGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringGenericTypeGenericMethod<System.String>.PropCount);
            var second = MockInstanceStringGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringGenericTypeGenericMethod<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceStringWithTry
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
    public class InstanceStringWithTryTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTry.GetCount);
            var first = MockInstanceStringWithTry.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTry.GetCount);
            var second = MockInstanceStringWithTry.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTry.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTry.PropCount);
            var first = MockInstanceStringWithTry.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTry.PropCount);
            var second = MockInstanceStringWithTry.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTry.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceStringWithTryGenericMethod
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
    public class InstanceStringWithTryGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTryGenericMethod.GetCount);
            var first = MockInstanceStringWithTryGenericMethod.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTryGenericMethod.GetCount);
            var second = MockInstanceStringWithTryGenericMethod.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTryGenericMethod.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTryGenericMethod.PropCount);
            var first = MockInstanceStringWithTryGenericMethod.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTryGenericMethod.PropCount);
            var second = MockInstanceStringWithTryGenericMethod.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTryGenericMethod.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceStringWithTryGenericType<T>
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
    public class InstanceStringWithTryGenericTypeTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTryGenericType<System.String>.GetCount);
            var first = MockInstanceStringWithTryGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTryGenericType<System.String>.GetCount);
            var second = MockInstanceStringWithTryGenericType<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTryGenericType<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTryGenericType<System.String>.PropCount);
            var first = MockInstanceStringWithTryGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTryGenericType<System.String>.PropCount);
            var second = MockInstanceStringWithTryGenericType<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTryGenericType<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

    public class MockInstanceStringWithTryGenericTypeGenericMethod<T>
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
    public class InstanceStringWithTryGenericTypeGenericMethodTests
    {
        [Test]
        public void GetString_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.GetCount);
            var first = MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.GetCount);
            var second = MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.GetString();
            Assert.AreEqual(1, MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.GetCount);
            Assert.AreEqual(first, second);
        }

        [Test]
        public void StringProp_should_be_lazy()
        {
            Assert.AreEqual(0, MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.PropCount);
            var first = MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.PropCount);
            var second = MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.StringProp;
            Assert.AreEqual(1, MockInstanceStringWithTryGenericTypeGenericMethod<System.String>.PropCount);
            Assert.AreEqual(first, second);
        }
    }

}