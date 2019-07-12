// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Threading;
using ALCProxy.Proxy;
using Microsoft.Xunit.Performance;
using Xunit;

namespace ALCProxy.Tests
{
    public interface ITest
    {
        public string PrintContext();
        public int DoThing2(int a, List<string> list);
        public int DoThing3(int a, Test2 t);
        public Test2 ReturnUserType();
        public int SimpleMethod();
    }

    public interface IGeneric<T>
    {
        public string PrintContext();
        public int DoThing2(int a, List<string> list);
        public int DoThing3(int a, Test2 t);
        public string DoThing4(T t);
        public string GenericMethodTest<I>();
    }

    public class Test2
    {
        public int test;
        public Test2()
        {
            test = 5;
        }
        public Test2(int start)
        {
            test = start;
        }
        public void DoThingy()
        {
            test++;
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
        public string PrintContext()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }
        public string GenericMethodTest<I>()
        {
            return typeof(I).ToString();
        }
        public int DoThing2(int a, List<string> list)
        {
            return _instance.Length;
        }
        public int DoThing3(int a, Test2 t)
        {
            t.DoThingy();
            return 6;
        }
        public string DoThing4(T tester)
        {
            return tester.ToString();
        }
    }

    public class Test : ITest
    {
        public string PrintContext()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }
        public int DoThing2(int a, List<string> list)
        {
            Console.WriteLine(a);

            return a + list[0].Length;
        }
        public int DoThing3(int a, Test2 t)
        {
            t.DoThingy();
            return 5;
        }

        public Test2 ReturnUserType()
        {
            return new Test2();
        }
        public int SimpleMethod()
        {
            return 3;
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
            Assert.Equal("TestBasicContextLoading", t.PrintContext());
            Assert.Equal(17, t.DoThing2(10, new List<string> { "Letters", "test", "hello world!"}));

            //Tests for call-by-value functionality, to make sure there aren't any problems with editing objects in the other ALC
            Test2 t2 = new Test2(3);
            Assert.Equal(3, t2.test);
            Assert.Equal(5, t.DoThing3(5, t2));
            Assert.Equal(3, t2.test);

            alc.Unload();
        }
        [Fact]
        public void TestUnload() 
        {
            WeakReference wrAlc2;
            System.Diagnostics.Debugger.Break();
            ITest t = GetALC(out wrAlc2);
            Assert.ThrowsAny<Exception>(t.PrintContext);
            System.Diagnostics.Debugger.Break();
            for (int i = 0; wrAlc2.IsAlive && (i < 10); i++)
            {
                //Console.WriteLine(GC.GetTotalMemory(true));
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            System.Diagnostics.Debugger.Break();
            Assert.False(wrAlc2.IsAlive);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        ITest GetALC(out WeakReference alcWeakRef)
        {
            //This seems to be neccesary to keep the ALC creation and unloading within a separate method to allow for it to be collected correctly, this needs to be investigated as to why
            var alc = new AssemblyLoadContext("TestUnload2", isCollectible: true);
            alcWeakRef = new WeakReference(alc, trackResurrection: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test"); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal("TestUnload2", t.PrintContext());

            //The unload only seems to work here, not anywhere outside the method which is strange
            alc.Unload();
            return t;
        }
        [Fact]
        public void TestSimpleGenerics()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestSimpleGenerics", isCollectible: true);
            IGeneric<string> t = ProxyBuilder<IGeneric<string>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(string) }); //The one referenced through the comm object, to test that the reference is removed
            
            Assert.Equal("TestSimpleGenerics", t.PrintContext());
            Assert.Equal("Hello!", t.DoThing4("Hello!"));


            alc.Unload();
        }
        [Fact]
        public void TestUserGenerics()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestUserGenerics", isCollectible: true);
            IGeneric<Test2> t = ProxyBuilder<IGeneric<Test2>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(Test2) }); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal(new Test2().ToString(), t.DoThing4(new Test2()));

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
    }

}
