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
        public static T CreateInstanceAndUnwrap(AssemblyLoadContext alc, string assemblyPath, string typeName, bool isGeneric)
        {
            //Create the object in the ALC
            T obj = ALCDispatch<T>.Create(alc, typeName, assemblyPath, isGeneric);
            return obj;
        }
    }
    public class ALCDispatch<I> : System.Reflection.DispatchProxy //T is the TargetObject type, I is the specific client you want to use.
    {
        private IClientObject _client; //ClientObject

        public void Unload()
        {

        }

        internal static I Create(AssemblyLoadContext alc, string typeName, string assemblyPath)
        {
            return Create(alc, typeName, assemblyPath, false);
        }
        
        internal static I Create(AssemblyLoadContext alc, string typeName, string assemblyPath, bool isGeneric) //TODO: build in generics to get working
        {
            object proxy = Create<I, ALCDispatch<I>>();
            ((ALCDispatch<I>)proxy).SetParameters(alc, typeName, assemblyPath);
            return (I)proxy;
        }
        private void SetParameters(AssemblyLoadContext alc, string typeName, string assemblyPath)
        {
            _client = (IClientObject)typeof(ClientObject).GetConstructor(new Type[] { typeof(Type) }).Invoke(new object[] { typeof(I) });
            //_client = (IClientObject)Activator.CreateInstance(typeof(ClientObject<I>));
            _client.SetUpServer(alc, typeName, assemblyPath, false);
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _client.SendMethod(targetMethod, args); //Whenever we call the method, instead we send the request to the client to make it to target
        }
    }
}
