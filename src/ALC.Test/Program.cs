using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using ALCProxy.Proxy;

namespace ALC.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            AssemblyLoadContext alc = new AssemblyLoadContext("TestContext", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test", isGeneric: false);
            //Console.WriteLine(t.DoThing());
            //Console.WriteLine(t.DoThing2(10, new List<string> { "Letters", "test", "hello world!" }));
            Test2 t2 = new Test2();
            Console.WriteLine(t.DoThing3(5, t2));
            Console.WriteLine(t2.test);
        }
    }


    public interface ITest
    {
        public string DoThing();
        public int DoThing2(int a, List<string> list);
        public int DoThing3(int a, Test2 t);
    }
    public class Test2 {
        public int test = 3;

        public void DoThingy()
        {
            test++;
        }
    }
    //public class GenericClass<T> : ITest
    //{
    //    string instance = "Hello!";
    //    T instance2;

    //    public string DoThing()
    //    {
    //        return instance.ToString();
    //    }
    //    public int DoThing2(int a, List<string> list)
    //    {
    //        return instance.Length;
    //    }
    //}
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

        public int DoThing3(int a, Test2 t)
        {
            t.DoThingy();
            return 5;
        }
    }
}
