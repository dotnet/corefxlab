// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using ALCProxy.Proxy;
using Xunit;

namespace ALCProxy.Tests
{
    public interface ITest
    {
        public string DoThing();

        public int DoThing2(int a, List<string> list);
    }
    public class Test2 { }
    public class GenericClass<T> : ITest
    {
        string instance = "Hello!";
        T instance2;

        public string DoThing()
        {
            return instance.ToString();
        }
        public int DoThing2(int a, List<string> list)
        {
            return instance.Length;
        }
    }
    public class Test : ITest
    {
        public string DoThing()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }
        public int DoThing2(int a, List<string> list)
        {
            Console.WriteLine(a);

            return a + list[0].Length;
        }
    }
    public class ALCProxyTests
    {
        [Fact]
        public void TestBasicContextLoading()
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            AssemblyLoadContext alc = new AssemblyLoadContext("TestContext", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test", isGeneric: false);
            Assert.Equal("TestContext", t.DoThing());
            Assert.Equal(17, t.DoThing2(10, new List<string> { "Letters", "test", "hello world!"}));
        }

        [Fact]
        public void TestUnload()
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            AssemblyLoadContext alc = new AssemblyLoadContext("TestContext", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test", isGeneric: false); //The one referenced through the comm object, to test that the reference is removed

            Assert.Equal("TestContext", t.DoThing());

            alc.Unload();
            GC.Collect();
            Assert.ThrowsAny<Exception>(t.DoThing);


            //TODO: check that the ALC is properly gone if there are no references to it
            //ConditionalWeakTable < ALCProxyTests, ITest > t2 = new ConditionalWeakTable<ALCProxyTests, ITest>();
            //Assert.ThrowsAny<Exception>();
        }


        [Fact]
        public void TestGenerics()
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            AssemblyLoadContext alc = new AssemblyLoadContext("TestContext", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", isGeneric: true); //The one referenced through the comm object, to test that the reference is removed


            Assert.Equal("Hello!", t.DoThing());

        }
    }
}
