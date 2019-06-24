using System;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.VisualBasic.CompilerServices;

namespace ALCProxy
{

    public interface ITest
    {
        public void DoThing();
    }

    public class Test : ITest
    {
        public static void Main()
        {
            AssemblyLoadContext alc = new AssemblyLoadContext("", isCollectible: true);
            ITest t = ProxyBuilder<ITest>.CreateInstanceAndUnwrap(alc, Assembly.GetExecutingAssembly().CodeBase.Substring(8), "Test");
            t.DoThing();

            ITest t2 = new Test();
            t2.DoThing();
        }

        public void DoThing()
        {
            Console.WriteLine("Hello from somewhere in the middle of nowhere!");
            var a = Assembly.GetExecutingAssembly();
            Console.WriteLine(AssemblyLoadContext.GetLoadContext(a).Name);
        }
    }


    public static class ProxyBuilder<T> //T is the interface type we want the object to extend
    {
        public static T CreateInstanceAndUnwrap(AssemblyLoadContext alc, string assemblyPath, string typeName)
        {
            //Create the object in the ALC
            T obj = ALCDispatch2<T>.Create(alc, typeName, assemblyPath);

            return obj;
        }
    }

    public class ALCDispatch<T, I> : DispatchProxy where I : IClientObject //T is the TargetObject type, I is the specific client you want to use.
    {
        private readonly IClientObject _client; //ClientObject
        public ALCDispatch(AssemblyLoadContext alc, string typeName, string a)
        {
            _client = (IClientObject)Activator.CreateInstance(typeof(I)); //Creates the client that sends stuff
            _client.SetUpServer(alc, typeName, a);
        }
        internal static object Create<Dispatch>(AssemblyLoadContext alc, string typeName, string a)
        {
            return new ALCDispatch<T, I>(alc, typeName, a);
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _client.SendMethod(targetMethod, args); //Whenever we call the method, instead we send the request to the client to make it to target
        }
    }

    public class ALCDispatch2<I> : DispatchProxy //T is the TargetObject type, I is the specific client you want to use.
    {
        private IClientObject _client; //ClientObject
        internal static I Create(AssemblyLoadContext alc, string typeName, string a)
        {
            object proxy = Create<I, ALCDispatch2<I>>();
            ((ALCDispatch2<I>)proxy).SetParameters(alc, typeName, a);
            return (I)proxy;
        }
        private void SetParameters(AssemblyLoadContext alc, string typeName, string a)
        {
            // _client = (IClientObject)Activator.CreateInstance(typeof(ClientObject)); //Creates the client that sends stuff

            _client = (IClientObject)typeof(ClientObject).GetConstructor(new Type[] { typeof(Type) }).Invoke(new object[] { typeof(I) });
            ;
            _client.SetUpServer(alc, typeName, a);
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _client.SendMethod(targetMethod, args); //Whenever we call the method, instead we send the request to the client to make it to target
        }
    }
}
