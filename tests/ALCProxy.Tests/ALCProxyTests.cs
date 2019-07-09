// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
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
    }
    public class ALCProxyTests
    {
        //[Benchmark]
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

        }
        //[Benchmark]
        //public void BenchmarkBasicProxy()
        //{
        //    AssemblyLoadContext alc = new AssemblyLoadContext("BenchmarkBasicProxy", isCollectible: true);
        //    ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test");
        //    foreach (var iteration in Benchmark.Iterations)
        //    {
        //        // Any per-iteration setup can go here.
        //        using (iteration.StartMeasurement())
        //        {
        //            // Code to be measured goes here.
        //            Assert.Equal("BenchmarkBasicProxy", t.PrintContext());
        //        }
        //        // ...per-iteration cleanup
        //    }
        //}
        [Fact]
        public void TestUnload() //TODO fix unloading so we can continue working on this test
        {
            //TODO change to CWT?
            //var cwt = new ConditionalWeakTable<string, AssemblyLoadContext>();
            AssemblyLoadContext alc = new AssemblyLoadContext("TestUnload", isCollectible: true);
            //cwt.Add("Test", alc);
            var wrAlc = new WeakReference(alc, false);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test"); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal("TestUnload", t.PrintContext());
            Assembly[] a1 = AppDomain.CurrentDomain.GetAssemblies();
            alc.Unload();
            alc = null;
            //t = null;
            GC.Collect();
            //Test that the proxy can no longer make calls
            Assert.ThrowsAny<Exception>(t.PrintContext);
            t = null;
            GC.Collect();
            GC.Collect();
            GC.Collect();
            Assembly[] a2 = AppDomain.CurrentDomain.GetAssemblies();
            //Check that a1 and a2 have different assemblies, to see if the ones used for the proxy been unloaded from the AppDomain
            Assert.NotEqual(a1, a2);
            //Test that the ALC is truly gone
            //Assert.True(cwt.TryGetValue("Test", out alc));

            //Assert.ThrowsAny<Exception>(() => alc.Unload()); //TODO this breaks all the tests
        }
        //[Benchmark]
        [Fact]
        public void TestSimpleGenerics()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestSimpleGenerics", isCollectible: true);
            IGeneric<string> t = ProxyBuilder<IGeneric<string>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(string) }); //The one referenced through the comm object, to test that the reference is removed

            Assert.Equal("TestSimpleGenerics", t.PrintContext());
            Assert.Equal("Hello!", t.DoThing4("Hello!"));
        }
        //[Benchmark]
        [Fact]
        public void TestUserGenerics()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestUserGenerics", isCollectible: true);
            IGeneric<Test2> t = ProxyBuilder<IGeneric<Test2>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(Test2) }); //The one referenced through the comm object, to test that the reference is removed
            Assert.Equal(new Test2().ToString(), t.DoThing4(new Test2()));
        }
        [Fact]
        public void TestGenericMethods()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("TestGenericMethods", isCollectible: true);
            IGeneric<Test2> t = ProxyBuilder<IGeneric<Test2>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(Test2) }); //The one referenced through the comm object, to test that the reference is removed
            //Test generic methods
            Assert.Equal(new Test2().ToString(), t.GenericMethodTest<Test2>());
        }
    }
}
