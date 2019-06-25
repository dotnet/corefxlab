using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Transactions;
using Microsoft.VisualBasic.CompilerServices;

namespace ALCProxy
{
    public interface IServerObject
    {
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
        private object _server;
        private Type _intType;
        public ClientObject(Type interfaceType)
        {
            _intType = interfaceType;
        }

        public static void Main()
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
            Type objType = FindType(typeName, a);
            // object instance = Activator.CreateInstance(t);

            //Load this assembly in so we can get the server into the ALC

            Assembly aaa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(DispatchProxy)).CodeBase.Substring(8));
            //aaa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(System.Activator)).CodeBase.Substring(8));


            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ClientObject)).CodeBase.Substring(8));
            Type serverType = FindType("ServerDispatch`1", aa);
            //Set up all the generics to allow for the serverDispatch to be created correctly
            Type constructedType = serverType.MakeGenericType(_intType);


            object s = Activator.CreateInstance(constructedType);

            MethodInfo m = constructedType.GetMethod(
                "Create",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy, null, new Type[] { typeof(Type) }, null);
            m = m.MakeGenericMethod(constructedType);
            _server = m.Invoke(s, new object[] { objType });


        }

        public object SendMethod(MethodInfo method, object[] args)
        {

            //MethodInfo m = _server.GetType().GetMethod("CallObject");
            //return m.Invoke(_server, new object[] { method, args });
            return method.Invoke(_server, args);
            //MethodInfo m = _server.GetType().GetMethod(((MethodInfo)args[0]).Name);
            //return m.Invoke(_server, (object[])args[1]);

        }

        //This should always be a "SendMethod" option, with 2 args, the methodinfo of what needs to be sent and the args for said method
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return SendMethod(targetMethod, args);
        }
    }
    /// <summary>
    /// Currently using a DispatchProxy directly here leads to an error when calling the "Create" method: System.NotSupportedException: 'A non-collectible assembly may not reference a collectible assembly.'
    /// So for now I'm doing some custom IL generation until I can figure out how to get around that problem, which seems to be sorting things out since it's all dynamic and manually set to be collectible.
    /// </summary>
    /// <typeparam name="I"></typeparam>

    public class ServerDispatch<I> : ALCProxy.Communication.DispatchProxy, IServerObject
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
            MethodInfo m = instance.GetType().GetMethod(targetMethod.Name);
            return m.Invoke(instance, args);
        }

        public object CallObject(MethodInfo method, object[] args)
        {
            throw new NotImplementedException();
        }
    }

}
