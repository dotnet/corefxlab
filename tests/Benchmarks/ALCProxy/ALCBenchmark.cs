// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using ALCProxy.Proxy;
using ALCProxy.TestInterface;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.ALCProxy
{
    public interface ITest
    {
        string GetContextName();
        int MethodWithGenericTypeParameter(int a, List<string> list);
        int MethodWithUserTypeParameter(int a, Test2 t);
        Test2 ReturnUserType();
        int SimpleMethod();
        string CallUsingMultipleParameters(int a, int b, string s, Test2 t, int c, int x, int y, int[] z, Test2 tt);
    }

    public interface IGeneric<T>
    {
        string GetContextName();
        int MethodWithGenericTypeParameter(int a, List<string> list);
        int MethodWithUserTypeParameter(int a, Test2 t);
        string MethodWithDirectGenericParameters(T t);
        string GenericMethodTest<I>();
    }

    public class Test2
    {
        public int mutableMember;
        public Test2()
        {
            mutableMember = 5;
        }
        public Test2(int start)
        {
            mutableMember = start;
        }
        public void IncrementMutable()
        {
            mutableMember++;
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
        public int MethodWithGenericTypeParameter(int a, List<string> list)
        {
            return _instance.Length;
        }
        public int MethodWithUserTypeParameter(int a, Test2 t)
        {
            t.IncrementMutable();
            return 6;
        }
        public string MethodWithDirectGenericParameters(T tester)
        {
            return tester.ToString();
        }
    }

    public class Test : ITest
    {
        public string CallUsingMultipleParameters(int a, int b, string s, Test2 t, int c, int x, int y, int[] z, Test2 tt)
        {
            return (a + b + (c*x*y)).ToString() + s + t.ToString() + tt.ToString() + z.ToString(); 
        }

        public string GetContextName()
        {
            var a = Assembly.GetExecutingAssembly();
            return AssemblyLoadContext.GetLoadContext(a).Name;
        }
        public int MethodWithGenericTypeParameter(int a, List<string> list)
        {
            Console.WriteLine(a);

            return a + list[0].Length;
        }
        public int MethodWithUserTypeParameter(int a, Test2 t)
        {
            t.IncrementMutable();
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
    public class ALCBenchmark
    {
        private static TestAssemblyLoadContext alc = new TestAssemblyLoadContext("BenchmarkContext", isCollectible: true);
        private static TestAssemblyLoadContext alc2;
        private AssemblyName assemblyName = alc.LoadFromAssemblyPath(Assembly.GetExecutingAssembly().Location).GetName();
        private ITest testObject;
        private ITest controlObject = new Test();
        private IGeneric<Test2> genericObject;
        private IGeneric<Test2> genericControl = new GenericClass<Test2>();
        private Test2 userInput;
        private AssemblyName name;

        [GlobalSetup]
        public void Setup()
        {
            userInput = new Test2();
            testObject = ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, Assembly.GetAssembly(typeof(Test)).GetName(), "Benchmarks.ALCProxy.Test");
            genericObject = ProxyBuilder<IGeneric<Test2>, ClientDispatch>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetAssembly(typeof(GenericClass<>)).GetName(), "Benchmarks.ALCProxy.GenericClass`1", new Type[] { typeof(Test2) });
            string path = Assembly.GetAssembly(typeof(Test)).Location.Split("Benchmarks")[0] + "ALCProxy.TestAssembly\\bin\\Release\\netcoreapp3.0\\ALCProxy.TestAssembly.dll";
            name = AssemblyName.GetAssemblyName(path);
            alc2 = new TestAssemblyLoadContext("BenchmarkExternalContext", path, isCollectible: true);
        }

        [Benchmark]
        public object CreateProxyObject()
        {
            return ProxyBuilder<ITest, ClientDispatch>.CreateInstanceAndUnwrap(alc, assemblyName, "Benchmarks.ALCProxy.Test");
        }

        [Benchmark]
        public object CreateExternalAssemblyProxyObject()
        {
            return ProxyBuilder<IExternalClass, ClientDispatch>.CreateInstanceAndUnwrap(alc2, name, "ALCProxy.TestAssembly.ExternalClass", new object[] { });
        }

        [Benchmark]
        public object CreateControlObject()
        {
            return new Test();
        }

        [Benchmark]
        public object CallSimpleMethodThroughProxy()
        {
            return testObject.SimpleMethod();
        }

        [Benchmark]
        public object CallSimpleMethodControl()
        {
            return controlObject.SimpleMethod();
        }

        [Benchmark]
        public object CreateGenericProxy()
        {
            return ProxyBuilder<IGeneric<Test2>, ClientDispatch>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().GetName(), "Benchmarks.ALCProxy.GenericClass`1", new Type[] { typeof(Test2) });
        }

        [Benchmark]
        public object CreateGenericControl()
        {
            return new GenericClass<Test2>();
        }
        
        [Benchmark]
        public object CallSimpleMethodGeneric()
        {
            return genericObject.GetContextName();
        }
        
        [Benchmark]
        public object CallSimpleMethodGenericControl()
        {
            return genericControl.GetContextName();
        }
        
        [Benchmark]
        public object UserTypeParameters()
        {
            return testObject.MethodWithUserTypeParameter(3, new Test2());
        }
        
        [Benchmark]
        public object UserTypeParametersControl()
        {
            return controlObject.MethodWithUserTypeParameter(3, new Test2());
        }
        
        [Benchmark]
        public object UserTypeParameters2()
        {
            return testObject.MethodWithUserTypeParameter(3, userInput);
        }
        
        [Benchmark]
        public object UserTypeParametersControl2()
        {
            return controlObject.MethodWithUserTypeParameter(3, userInput);
        }

        [Benchmark]
        public object SerializeManyParameters()
        {
            return testObject.CallUsingMultipleParameters(1,2,"3",new Test2(), 44, 1, 3, new int[] { 3,4,5}, new Test2());
        }

        [Benchmark]
        public object SerializeManyParametersControl()
        {
            return controlObject.CallUsingMultipleParameters(1, 2, "3", new Test2(), 44, 1, 3, new int[] { 3, 4, 5 }, new Test2());
        }
    }
    class TestAssemblyLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

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
