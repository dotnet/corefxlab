// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ALCProxy.Communication
{
    public class ClientObject : ALCProxy.Communication.DispatchProxy, IClientObject
    {
        //Can't make this an IServerObject directly due to the type-isolation barrier
        private object _server;
        private Type _intType;
        public ClientObject(Type interfaceType)
        {
            _intType = interfaceType;
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
            //Load this assembly in so we can get the server into the ALC
            Assembly aa = alc.LoadFromAssemblyPath(Assembly.GetAssembly(typeof(ClientObject)).CodeBase.Substring(8));
            Type serverType = FindType("ServerDispatch`1", aa);
            //Set up all the generics to allow for the serverDispatch to be created correctly
            Type constructedType = serverType.MakeGenericType(_intType);
            object s = Activator.CreateInstance(constructedType);
            //Get the "Create<ObjectInterface>" method to generate the serverside proxy within the target ALC
            MethodInfo m = constructedType.GetMethod(
                "Create",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy, null, new Type[] { typeof(Type) }, null);
            m = m.MakeGenericMethod(constructedType);
            _server = m.Invoke(s, new object[] { objType });
        }
        public object SendMethod(MethodInfo method, object[] args)
        {
            return method.Invoke(_server, args);
        }
        //This should always be a "SendMethod" option, with 2 args, the methodinfo of what needs to be sent and the args for said method
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return SendMethod(targetMethod, args);
        }
    }
    public class ServerDispatch<ObjectInterface> : ALCProxy.Communication.DispatchProxy, IServerObject
    {
        public object instance;
        internal static ObjectInterface Create<Dispatch>(Type type)
        {
            object proxy = Create<ObjectInterface, ServerDispatch<ObjectInterface>>();
            ((ServerDispatch<ObjectInterface>)proxy).SetParameters(type, new Type[] { }, new object[] { });
            return (ObjectInterface)proxy;
        }
        private void SetParameters(Type instanceType, Type[] argTypes, object[] constructorArgs)
        {
            instance = instanceType.GetConstructor(argTypes).Invoke(constructorArgs);
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            //Type[] argTypes = args.Select(obj => ConvertType(obj.GetType())).ToArray();
            Type[] argTypes = args.Select(obj => obj.GetType()).ToArray();
            MethodInfo[] mm = instance.GetType().GetMethods();
            MethodInfo m = instance.GetType().GetMethod(targetMethod.Name, argTypes);
            return m.Invoke(instance, args);
        }

        //TODO Get type conversion working
        public Type ConvertType(Type toConvert)
        {
            string assemblyPath = Assembly.GetAssembly(toConvert).CodeBase.Substring(8);
            if (toConvert.IsPrimitive || assemblyPath.Contains("System.Private.CoreLib")) //Can't load/dont want to load extra types from System.Private.CoreLib
            {
                return toConvert;
            }
             return Assembly.LoadFrom(Assembly.GetAssembly(toConvert).CodeBase.Substring(8)).GetType(toConvert.FullName);

        }
    }
}
