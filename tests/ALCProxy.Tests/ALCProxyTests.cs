// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using ALCProxy.Proxy;
using Xunit;

namespace ALCProxy.Tests
{
    public interface ITest
    {
        public string GetContextName();
        public int ReturnIntWhilePassingInList(int a, List<string> list);
        public int ReturnIntWhilePassingInUserType(int a, Test2 t);
        public Test2 ReturnUserType();
        public int SimpleMethodReturnsInt();
    }

    public interface IGeneric<T>
    {
        public string GetContextName();
        public int PassInList(int a, List<string> list);
        public int PassInUserType(int a, Test2 t);
        public string PassInGenericType(T t);
        public string GenericMethodTest<I>();
    }

    public class Test2
    {
        public int testValue;
        public Test2()
        {
            testValue = 5;
        }
        public Test2(int start)
        {
            testValue = start;
        }
        public void DoThingy()
        {
            testValue++;
        }
    }
    public class GenericClass<T> : IGeneric<T>
    {
        private readonly string _instance = "testString";
        private T _instance2;
        public GenericClass()
        {
        }
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
        public int PassInUserType(int a, Test2 t)
        {
            t.DoThingy();
            return 6;
        }
        public string PassInGenericType(T tester)
        {
            return tester.ToString();
        }
    }

    public class Test : ITest
    {
        public string GetContextName()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }
        public int ReturnIntWhilePassingInList(int a, List<string> list)
        {
            Console.WriteLine(a);

            return a + list[0].Length;
        }
        public int ReturnIntWhilePassingInUserType(int a, Test2 t)
        {
            t.DoThingy();
            return 5;
        }

        public Test2 ReturnUserType()
        {
            return new Test2();
        }
        public int SimpleMethodReturnsInt()
        {
            return 3;
        }
    }

    public class TestObjectParamConst : ITest
    {
        int aa;
        string bb;
        Test2 tt;
        public TestObjectParamConst(int a, string b)
        {
            this.aa = a;
            this.bb = b;
            tt = new Test2();
        }
        public TestObjectParamConst(int a, string b, Test2 t)
        {
            this.aa = a;
            this.bb = b;
            tt = t;
        }
        public int ReturnIntWhilePassingInList(int a, List<string> list)
        {
            return aa;
        }

        public int ReturnIntWhilePassingInUserType(int a, Test2 t)
        {
            return aa + a;
        }

        public string GetContextName()
        {
            return bb;
        }

        public Test2 ReturnUserType()
        {
            return tt;
        }

        public int SimpleMethodReturnsInt()
        {
            return tt.testValue;
        }
    }
    public class ALCProxyTests
    {
        [Fact]
        public void TestBasicContextLoading()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestBasicContextLoading", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test");

            //Test basic functionality
            Assert.Equal("TestBasicContextLoading", t.GetContextName());
            Assert.Equal(17, t.ReturnIntWhilePassingInList(10, new List<string> { "Letters", "test", "hello world!"}));

            //Tests for call-by-value functionality, to make sure there aren't any problems with editing objects in the other ALC
            Test2 t2 = new Test2(3);
            Assert.Equal(3, t2.testValue);
            Assert.Equal(5, t.ReturnIntWhilePassingInUserType(5, t2));
            Assert.Equal(3, t2.testValue);

            alc.Unload();
        }
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestUnload() 
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
        WeakReference GetWeakRefALC()
        {
            //This seems to be neccesary to keep the ALC creation and unloading within a separate method to allow for it to be collected correctly, this needs to be investigated as to why
            var alc = new AssemblyLoadContext("TestUnload2", isCollectible: true);
            WeakReference alcWeakRef = new WeakReference(alc, trackResurrection: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test"); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal("TestUnload2", t.GetContextName());

            //The unload only seems to work here, not anywhere outside the method which is strange
            alc.Unload();
            Assert.ThrowsAny<Exception>(t.GetContextName);
            return alcWeakRef;
        }
        [Fact]
        public void TestSimpleGenerics()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestSimpleGenerics", isCollectible: true);
            IGeneric<string> t = ProxyBuilder<IGeneric<string>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(string) }); //The one referenced through the comm object, to test that the reference is removed
            
            Assert.Equal("TestSimpleGenerics", t.GetContextName());
            Assert.Equal("Hello!", t.PassInGenericType("Hello!"));


            alc.Unload();
        }
        [Fact]
        public void TestUserGenerics()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestUserGenerics", isCollectible: true);
            IGeneric<Test2> t = ProxyBuilder<IGeneric<Test2>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(Test2) }); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal(new Test2().ToString(), t.PassInGenericType(new Test2()));

            alc.Unload();
        }
        [Fact]
        public void TestGenericMethods()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestGenericMethods", isCollectible: true);
            IGeneric<Test2> t = ProxyBuilder<IGeneric<Test2>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(Test2) }); //The one referenced through the comm object, to test that the reference is removed
            //Test generic methods
            Assert.Equal(new Test2().ToString(), t.GenericMethodTest<Test2>());

            alc.Unload();
        }
        [Fact]
        public void TestUserReturnTypes()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestBasicContextLoading", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test");
            Assert.Equal(new Test2().ToString(), t.ReturnUserType().ToString());

            alc.Unload();
        }
        [Fact]
        public void TestConstructorParams()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestConstructorParams", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "TestObjectParamConst", new object[] { 55, "testString" });
            Assert.Equal(55, t.ReturnIntWhilePassingInList(3, new List<string>()));
            Assert.Equal(58, t.ReturnIntWhilePassingInUserType(3, new Test2()));
            Assert.Equal("testString", t.GetContextName());
            Assert.Equal(5, t.SimpleMethodReturnsInt());
        }
        [Fact]
        //TODO currently this breaks due to some type cast errors ("Test2 isn't the same type as Test2"). This may be due to how we load the assemblies
        //into the ALC, but we need to investigate this further to figure out what's going wrong.
        public void TestConstructorUserParams()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestConstructorUserParams", isCollectible: true);
            //Test when putting user objects into the constructor
            Test2 test = new Test2(100);
            ITest t2 = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "TestObjectParamConst", new object[] { 60, "testString", test });
            Assert.Equal(60, t2.ReturnIntWhilePassingInList(3, new List<string>()));
            Assert.Equal(63, t2.ReturnIntWhilePassingInUserType(3, new Test2()));
            Assert.Equal("testString", t2.GetContextName());
            Assert.NotEqual(test, t2.ReturnUserType());
            Assert.Equal(test.testValue, t2.SimpleMethodReturnsInt());
        }
    }

}
