using System;
using System.Reflection;
using System.Runtime.Loader;
using HelloWorld;

namespace ALCProxy
{

    public class ProxyBuilder <T, I> where T : I
    {

        private readonly AssemblyLoadContext _alc;
        private readonly string _assemblyPath;
        private readonly Assembly _a;
        public object instanceObj;


        public ProxyBuilder(AssemblyLoadContext alc, string assemblyPath)
        {
            _alc = alc;
            _assemblyPath = assemblyPath;
            _a = _alc.LoadFromAssemblyPath(_assemblyPath);
        }


        private void CreateInstanceObj()
        {
            Type t = _a.GetType(_a.GetName().Name + "." + typeof(T).Name);
            instanceObj = Activator.CreateInstance(t);
        }

        public I CreateInstanceAndUnwrap()
        {
            I obj = ALCDispatch.Create<I, ALCDispatch>();
            return obj;
        }
    }

    public class ALCDispatch : DispatchProxy 
    {
        private readonly IClientObject _client;

        public ALCDispatch()
        {
            //TODO Dependency Injection to allow for extensibility?
            _client = new ClientObject();
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _client.SendMethod(targetMethod, args);
        }
    }
}
