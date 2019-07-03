// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Reflection;
using System.Runtime.Loader;
using ALCProxy.Communication;

namespace ALCProxy.Proxy
{
    public static class ProxyBuilder<T> //T is the interface type we want the object to extend
    {
        public static T CreateInstanceAndUnwrap(AssemblyLoadContext alc, string assemblyPath, string typeName)
        {
            //Create the object in the ALC
            T obj = ALCDispatch<T>.Create(alc, assemblyPath, typeName);
            return obj;
        }

        public static T CreateGenericInstanceAndUnwrap(AssemblyLoadContext alc, string assemblyPath, string typeName, Type[] genericTypes)
        {
            T obj = ALCDispatch<T>.CreateGeneric(alc, assemblyPath, typeName, genericTypes);
            return obj;
        }
    }
    public class ALCDispatch<I> : System.Reflection.DispatchProxy //T is the TargetObject type, I is the specific client you want to use.
    {
        private IClientObject _client; //ClientObject

        internal static I Create(AssemblyLoadContext alc, string assemblyPath, string typeName)
        {
            object proxy = Create<I, ALCDispatch<I>>();
            ((ALCDispatch<I>)proxy).SetParameters(alc, typeName, assemblyPath, null);
            return (I)proxy;
        }
        
        internal static I CreateGeneric(AssemblyLoadContext alc, string assemblyPath, string typeName, Type[] genericTypes) //TODO: build in generics to get working
        {
            object proxy = Create<I, ALCDispatch<I>>();
            ((ALCDispatch<I>)proxy).SetParameters(alc, typeName, assemblyPath, genericTypes);
            return (I)proxy;
        }

        private void SetParameters(AssemblyLoadContext alc, string typeName, string assemblyPath, Type[] genericTypes)
        {
            _client = (IClientObject)typeof(ClientObject).GetConstructor(new Type[] { typeof(Type) }).Invoke(new object[] { typeof(I) });
            //_client = (IClientObject)Activator.CreateInstance(typeof(ClientObject<I>));
            _client.SetUpServer(alc, typeName, assemblyPath, genericTypes);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _client.SendMethod(targetMethod, args); //Whenever we call the method, instead we send the request to the client to make it to target
        }
    }
}
