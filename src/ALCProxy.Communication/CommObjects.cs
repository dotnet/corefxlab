using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace ALCProxy
{
    public interface IServerObject
    {
        void ReturnResult(Type t, object instance);
        object CallObject(MethodInfo method, object[] args);
    }

    public interface IClientObject
    {
        object SendMethod(MethodInfo method, object[] args);
        void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath);
    }

    public class ClientObject : DispatchProxy, IClientObject
    {
        //Can't make this an IServerObject directly due to the type-isolation barrier
        private IServerObject _server;
        public ClientObject()
        {

        }

        private Type FindType(string typeName, Assembly a)
        {
            //find the type we're looking for
            Type t = null;
            foreach (Type ty in a.GetTypes())
            {

                if (ty.Name.Equals(typeName))
                {
                    t = ty;
                    break;
                }
            }
            if (t == null)
            {
                //no type asked for in the assembly
                throw new Exception();
            }

            return t;
        }

        public void SetUpServer(AssemblyLoadContext alc, string typeName, string assemblyPath)
        {

            Assembly a = alc.LoadFromAssemblyPath(assemblyPath);

            //find the type we're looking for
            Type t = FindType(typeName, a);

            //Load this assembly in so we can get the server into the ALC
            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ClientObject)).CodeBase.Substring(8));



            //TODO: Actually get the server into the ALC because while this does work, it's currently just the exact same stuff as it was before but with even more reflection.
            _server = (IServerObject)ServerDispatch<IServerObject>.Create<ServerDispatch<IServerObject>>(t);
        }

        public object SendMethod(MethodInfo method, object[] args)
        {
            return _server.CallObject(method, args);
        }

        //This should always be a "SendMethod" option, with 2 args, the methodinfo of what needs to be sent and the args for said method
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {

            throw new NotImplementedException();
        }
    }

    public class ServerDispatch<I> : DispatchProxy
    {
        public object instance;
        internal static I Create<Dispatch>(Type type)
        {
            object proxy = Create<I, ServerDispatch<I>>();
            ((ServerDispatch<I>)proxy).SetParameters(type, new Type[] { }, new object[] { });
            return (I)proxy;
        }
        private void SetParameters(Type instanceType, Type[] argTypes, object[] constructorArgs)
        {
            instance = instanceType.GetConstructor(argTypes).Invoke(constructorArgs);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            //The first arg is the methodinfo, the second is the list of args
            MethodInfo m = instance.GetType().GetMethod(((MethodInfo)args[0]).Name);
            return m.Invoke(instance, (object[])args[1]);
        }
    }

}
