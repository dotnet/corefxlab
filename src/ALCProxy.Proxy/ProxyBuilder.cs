// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Reflection;
using System.Runtime.Loader;
using ALCProxy.Communication;

namespace ALCProxy.Proxy
{
    public static class ProxyBuilder<I, T> where T : IProxyClient//T is the specific serializer we want to use
    {
        public static I CreateInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName)
        {
            //Create the object in the ALC
            return CreateInstanceAndUnwrap(alc, assemblyName, typeName, new object[] { });
        }
        public static I CreateInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, object[] constructorParams)
        {
            //Create the object in the ALC
            I obj = ALCDispatch<I,T>.Create(alc, assemblyName, typeName, constructorParams);
            return obj;
        }
        public static I CreateGenericInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, object[] constructorParams, Type[] genericTypes)
        {
            I obj = ALCDispatch<I,T>.CreateGeneric(alc, assemblyName, typeName, constructorParams, genericTypes);
            return obj;
        }
        public static I CreateGenericInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, Type[] genericTypes)
        {
            return CreateGenericInstanceAndUnwrap(alc, assemblyName, typeName, new object[] { }, genericTypes);
        }
    }
    public class ALCDispatch<I, T> : DispatchProxy where T : IProxyClient //I is the specific client you want to use.
    {
        private IProxyClient _client; //ClientObject
        internal static I Create(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, object[] constructorParams)
        {
            if (alc == null || assemblyName == null || typeName == null)
                throw new ArgumentNullException("Error with inputted parameter");
            if (constructorParams == null)
                constructorParams = new object[] { };

            object proxy = Create<I, ALCDispatch<I,T>>();
            ((ALCDispatch<I,T>)proxy).SetParameters(alc, typeName, assemblyName, constructorParams, null);
            return (I)proxy;
        }
        internal static I CreateGeneric(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, object[] constructorParams, Type[] genericTypes)
        {
            if (alc == null || assemblyName == null || typeName == null || genericTypes == null)
                throw new ArgumentNullException("Error with inputted parameter");
            if (constructorParams == null)
                constructorParams = new object[] { };
            if (Convert.ToInt32(typeName.Split("`")[1]) != genericTypes.Length)
                throw new ArgumentException("Wrong number of generic types for the given typeName");

            object proxy = Create<I, ALCDispatch<I,T>>();
            ((ALCDispatch<I,T>)proxy).SetParameters(alc, typeName, assemblyName, constructorParams,  genericTypes);
            return (I)proxy;
        }
        private void SetParameters(AssemblyLoadContext alc, string typeName, AssemblyName assemblyName, object[] constructorParams, Type[] genericTypes)
        {
            _client = (IProxyClient)typeof(T).GetConstructor(new Type[] { typeof(Type) }).Invoke(new object[] { typeof(I) });
            _client.SetUpServer(alc, typeName, assemblyName,constructorParams,  genericTypes);
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return _client.SendMethod(targetMethod, args); //Whenever we call the method, instead we send the request to the client to make it to target
        }
    }
}
