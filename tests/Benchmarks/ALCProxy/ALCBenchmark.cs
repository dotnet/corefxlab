using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using ALCProxy.Proxy;
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
        private static AssemblyLoadContext alc = new AssemblyLoadContext("BenchmarkContext", isCollectible: true);
        private string assemblyString = alc.LoadFromAssemblyPath(Assembly.GetExecutingAssembly().CodeBase.Substring(8)).CodeBase.Substring(8);
        private ITest testObject = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test");
        private ITest controlObject = new Test();
        private IGeneric<Test2> genericObject = ProxyBuilder<IGeneric<Test2>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(Test2) });
        private IGeneric<Test2> genericControl = new GenericClass<Test2>();
        private Test2 userInput;

        [GlobalSetup]
        public void Setup()
        {
            userInput = new Test2();
        }
        [Benchmark]
        public object CreateProxyObject()
        {
            // alc = new AssemblyLoadContext("BenchmarkContext", isCollectible: true);

            return ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, assemblyString, "Test");
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
            return ProxyBuilder<IGeneric<Test2>>.CreateGenericInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "GenericClass", new Type[] { typeof(Test2) });
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
    }
}
