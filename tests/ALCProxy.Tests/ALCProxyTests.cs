// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using ALCProxy.Proxy;
using Xunit;

namespace ALCProxy.Tests
{
    public interface ITest
    {
        int specialInt
        {
            get;

            set;
        }
        public string GetContextName();
        public int ReturnIntWhilePassingInList(int a, List<string> list);
        public int ReturnIntWhilePassingInUserType(int a, TestParameter t);
        public TestParameter ReturnUserType();
        public int SimpleMethodReturnsInt();
    }

    public interface IGeneric<T>
    {
        public string GetContextName();
        public int PassInList(int a, List<string> list);
        public int PassInUserType(int a, TestParameter t);
        public string PassInGenericType(T t);
        public string GenericMethodTest<I>();
    }
    [Serializable]
    public class TestParameter
    {
        public int testValue;
        public TestParameter()
        {
            testValue = 5;
        }
        public TestParameter(int start)
        {
            testValue = start;
        }
        public void DoThingy()
        {
            testValue++;
        }
    }
    [Serializable]
    public class GenericClass<T> : IGeneric<T>
    {
        private readonly string _instance = "testString";
        private readonly T  _instance2;
        public GenericClass() { }
        public GenericClass(T t)
        {
            _instance2 = t;
        }
        public string GetContextName()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }
        public string GenericMethodTest<I>()
        {
            return typeof(I).ToString();
        }
        public int PassInList(int a, List<string> list)
        {
            return _instance.Length;
        }
        public int PassInUserType(int a, TestParameter t)
        {
            t.DoThingy();
            return 6;
        }
        public string PassInGenericType(T tester)
        {
            return tester.ToString();
        }
    }
    [Serializable]
    public class Test : ITest
    {
        public Test()
        {
            specialInt = 5;
        }

        public int specialInt { get; set; }


        public string GetContextName()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }
        int ITest.ReturnIntWhilePassingInList(int a, List<string> list)
        {
            Console.WriteLine(a);
            return a + list[0].Length;
        }
        public int ReturnIntWhilePassingInUserType(int a, TestParameter t)
        {
            t.DoThingy();
            return 5;
        }

        TestParameter ITest.ReturnUserType()
        {
            return new TestParameter();
        }
        public int SimpleMethodReturnsInt()
        {
            return 3;
        }
    }
    [Serializable]
    public class TestObjectParamConst : ITest
    {
        readonly int _aa;
        readonly string _bb;
        readonly TestParameter _tt;

        public int specialInt { get => _aa; set => throw new NotImplementedException(); }

        public TestObjectParamConst(int a, string b)
        {
            _aa = a;
            _bb = b;
            _tt = new TestParameter();
        }
        public TestObjectParamConst(int a, string b, TestParameter t)
        {
            _aa = a;
            _bb = b;
            _tt = t;
        }
        public int ReturnIntWhilePassingInList(int a, List<string> list)
        {
            return _aa;
        }

        public int ReturnIntWhilePassingInUserType(int a, TestParameter t)
        {
            return _aa + a;
        }

        public string GetContextName()
        {
            return _bb;
        }

        public TestParameter ReturnUserType()
        {
            return _tt;
        }

        public int SimpleMethodReturnsInt()
        {
            return _tt.testValue;
        }
    }
    public class ALCProxyTests
    {
        private readonly string _testAssemblyPath;

        public ALCProxyTests () 
        {        
            _testAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ALCProxy.TestAssembly.dll");
        }

        [Fact]
        public void SimpleObjectsProxiedCorrectly()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("SimpleObjectsProxiedCorrectly", isCollectible: true);
            ITest t = ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "ALCProxy.Tests.Test");

            // Test basic functionality
            Assert.Equal("SimpleObjectsProxiedCorrectly", t.GetContextName());
            Assert.Equal(17, t.ReturnIntWhilePassingInList(10, new List<string> { "Letters", "test", "hello world!"}));

            // Tests for call-by-value functionality, to make sure there aren't any problems with editing objects in the other ALC
            TestParameter t2 = new TestParameter(3);
            Assert.Equal(3, t2.testValue);
            Assert.Equal(5, t.ReturnIntWhilePassingInUserType(5, t2));
            Assert.Equal(3, t2.testValue);
        }

        [Fact]
        public void SimpleObjectWithPropertiesProxied()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("SimpleObjectsProxiedCorrectly", isCollectible: true);
            ITest t = ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "ALCProxy.Tests.Test");

            // Test basic functionality of properties
            Assert.Equal("SimpleObjectsProxiedCorrectly", t.GetContextName());
            Assert.Equal(5, t.specialInt);
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void UnloadabilityWorksCorrectly() 
        {
            WeakReference wrAlc2 = GetWeakRefALC();
            for (int i = 0; wrAlc2.IsAlive && (i < 10); i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.False(wrAlc2.IsAlive);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private WeakReference GetWeakRefALC()
        {
            // This seems to be neccesary to keep the ALC creation and unloading within a separate method to allow for it to be collected correctly, this needs to be investigated as to why
            var alc = new TestAssemblyLoadContext("UnloadabilityWorksCorrectly", isCollectible: true);
            WeakReference alcWeakRef = new WeakReference(alc, trackResurrection: true);
            ITest t = ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "Test"); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal("UnloadabilityWorksCorrectly", t.GetContextName());

            // The unload only seems to work here, not anywhere outside the method which is strange
            alc.Unload();
            Assert.ThrowsAny<InvalidOperationException>(t.GetContextName);
            return alcWeakRef;
        }

        [Fact]
        public void NonUserGenericTypesExecuteCorrectly()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("NonUserGenericTypesExecuteCorrectly", isCollectible: true);
            IGeneric<string> t = ProxyBuilder<IGeneric<string>, ClientDispatch>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "GenericClass`1", new Type[] { typeof(string) }); //The one referenced through the comm object, to test that the reference is removed
            
            Assert.Equal("NonUserGenericTypesExecuteCorrectly", t.GetContextName());
            Assert.Equal("Hello!", t.PassInGenericType("Hello!"));

            alc.Unload();
        }

        [Fact]
        public void UserGenericTypesCorrectlyCreated()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("UserGenericTypesCorrectlyCreated", isCollectible: true);
            IGeneric<TestParameter> t = ProxyBuilder<IGeneric<TestParameter>, ClientDispatch>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "GenericClass`1", new Type[] { typeof(TestParameter) }); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal(new TestParameter().ToString(), t.PassInGenericType(new TestParameter()));

            alc.Unload();
        }

        [Fact]
        public void UserGenericTypeMethodsExecuteCorrectly()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("GenericMethodsExecuteCorrectly", isCollectible: true);
            IGeneric<TestParameter> t = ProxyBuilder<IGeneric<TestParameter>, ClientDispatch>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "GenericClass`1", new Type[] { typeof(TestParameter) }); //The one referenced through the comm object, to test that the reference is removed
            // Test generic methods
            Assert.Equal(new TestParameter().ToString(), t.GenericMethodTest<TestParameter>());

            alc.Unload();
        }

        [Fact]
        public void ReturnTypesCorrectlyGiveBackUserDefinedTypes()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("ReturnTypesCorrectlyGiveBackUserDefinedTypes", isCollectible: true);
            ITest t = ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "Test");
            Assert.Equal(new TestParameter().ToString(), t.ReturnUserType().ToString());

            alc.Unload();
        }

        [Fact]
        public void CreateObjectWithNonUserParamsInConstructor()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("CreateObjectWithNonUserParamsInConstructor", isCollectible: true);
            ITest t = ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "TestObjectParamConst", new object[] { 55, "testString" });
            Assert.Equal(55, t.ReturnIntWhilePassingInList(3, new List<string>()));
            Assert.Equal(58, t.ReturnIntWhilePassingInUserType(3, new TestParameter()));
            Assert.Equal("testString", t.GetContextName());
            Assert.Equal(5, t.SimpleMethodReturnsInt());
        }

        [Fact]
        public void CreateObjectWithUserParamsInConstructor()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("CreateObjectWithUserParamsInConstructor", isCollectible: true);
            // Test when putting user objects into the constructor
            TestParameter test = new TestParameter(100);
            ITest t2 = ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(true), "TestObjectParamConst", new object[] { 60, "testString", test });
            Assert.Equal(60, t2.ReturnIntWhilePassingInList(3, new List<string>()));
            Assert.Equal(63, t2.ReturnIntWhilePassingInUserType(3, new TestParameter()));
            Assert.Equal("testString", t2.GetContextName());
            Assert.NotEqual(test, t2.ReturnUserType());
            Assert.Equal(test.testValue, t2.SimpleMethodReturnsInt());
        }

        // As a note, to test these, we need to manually build any of the external classes before running tests, 
        // or you will error out with a TypeLoadException when the creation of the proxy is attempted
        [Fact]
        public void CanLoadOustideAssemblyWithSharedInterface()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("CanLoadOustideAssemblyWithSharedInterface", _testAssemblyPath, isCollectible: true);
            ALCProxy.TestInterface.IExternalClass a = ProxyBuilder<ALCProxy.TestInterface.IExternalClass, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.LoadFile(_testAssemblyPath).GetName(true), "ExternalClass", new object[] { });
            Assert.Equal(5, a.GetUserParameter(5));
            Assert.Equal("CanLoadOustideAssemblyWithSharedInterface", a.GetCurrentContext());
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "test1", "test1" },
                { "Hello", "world!" }
            };
            var list = new List<string>
            {
                "Hello world!",
                "Hello world!"
            };
            Assert.Equal(list, a.PassGenericObjects(dict));
        }

        /// <summary>
        /// The test assembly isn't built with the updated IExternalClass, replicating what it would look like if the interface recieved an update
        /// </summary>
        [Fact]
        public void CanLoadOustideAssemblyWithoutSharedInterface()
        {
            TestAssemblyLoadContext alc = new TestAssemblyLoadContext("CanLoadOustideAssemblyWithoutSharedInterface", _testAssemblyPath, isCollectible: true);
            ALCProxy.TestInterfaceUpdated.IExternalClass a = ProxyBuilder<ALCProxy.TestInterfaceUpdated.IExternalClass, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.LoadFile(_testAssemblyPath).GetName(true), "ExternalClass", new object[] { });
            Assert.Equal(5, a.GetUserParameter(5));
            Assert.Equal("CanLoadOustideAssemblyWithoutSharedInterface", a.GetCurrentContext());
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "test1", "test1" },
                { "Hello", "world!" }
            };
            var list = new List<string>
            {
                "Hello world!",
                "Hello world!"
            };
            Assert.Equal(list, a.PassGenericObjects(dict));
            Assert.Throws<MissingMethodException>(a.AdditionalUpdateMethod);
        }
    }

    class TestAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public TestAssemblyLoadContext(string name, bool isCollectible) : base(name, isCollectible)
        {
            _resolver = new AssemblyDependencyResolver(Assembly.GetExecutingAssembly().Location);
        }

        public TestAssemblyLoadContext(string name, string mainExecPath, bool isCollectible) : base(name, isCollectible)
        {
            _resolver = new AssemblyDependencyResolver(mainExecPath);
        }

        protected override Assembly Load(AssemblyName name)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(name);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}
