// Licensed to the .NET Foundation under one or more agreements. 
// The .NET Foundation licenses this file to you under the MIT license. 
// See the LICENSE file in the project root for more information. 

using System;
using System.Reflection;
using System.Runtime.Loader;
using ALCProxy.Communication;

namespace ALCProxy.Proxy
{
    /// <summary>
    /// ProxyBuilder has static methods that function similarly to AppDomain.CreateInstanceAndUnwrap().
    /// </summary>
    /// <typeparam name="I">The interface the proxy is inheriting from</typeparam>
    /// <typeparam name="T">The Client being used to cross the ALC boundary. For users, custom made Clients inheriting from ALCProxy.Communication.ALCCLient only need to implement serialization</typeparam>
    public static class ProxyBuilder<I, T> where T : IProxyClient//T is the specific serializer we want to use
    {

        /// <summary>
        /// Creates a proxy object in the given ALC
        /// </summary>
        /// <param name="alc">The AssemblyLoadContext to create the proxy in</param>
        /// <param name="assemblyName">The AssemblyName of the assembly where the desired type to be proxied resides</param>
        /// <param name="typeName">The name of the target type to be proxied from the assembly to load</param>
        /// <returns>A DispatchProxy presented as the target object</returns>
        public static I CreateInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName)
        {
            //Create the object in the ALC
            return CreateInstanceAndUnwrap(alc, assemblyName, typeName, new object[] { });
        }

        /// <summary>
        /// Creates a proxy object from a generic type in the given ALC
        /// </summary>
        /// <param name="alc">The AssemblyLoadContext to create the proxy in</param>
        /// <param name="assemblyName">The AssemblyName of the assembly where the desired type to be proxied resides</param>
        /// <param name="typeName">The name of the target type to be proxied from the assembly to load</param>
        /// <param name="constructorParams">A list of arguments to pass to the object's constructor</param>
        /// <returns>A DispatchProxy presented as the target object</returns>
        public static I CreateInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, object[] constructorParams)
        {
            //Create the object in the ALC
            I obj = ALCDispatch<I,T>.Create(alc, assemblyName, typeName, constructorParams);
            return obj;
        }

        /// <summary>
        /// Creates a proxy object from a generic type in the given ALC
        /// </summary>
        /// <param name="alc">The AssemblyLoadContext to create the proxy in</param>
        /// <param name="assemblyName">The AssemblyName of the assembly where the desired type to be proxied resides</param>
        /// <param name="typeName">The name of the target type to be proxied from the assembly to load</param>
        /// <param name="constructorParams">A list of arguments to pass to the object's constructor</param>
        /// <param name="genericTypes">A list of types to be used to make the generic type</param>
        /// <returns>A DispatchProxy presented as the target object</returns>
        public static I CreateGenericInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, object[] constructorParams, Type[] genericTypes)
        {
            I obj = ALCDispatch<I,T>.CreateGeneric(alc, assemblyName, typeName, constructorParams, genericTypes);
            return obj;
        }

        /// <summary>
        /// Creates a proxy object from a generic type in the given ALC
        /// </summary>
        /// <param name="alc">The AssemblyLoadContext to create the proxy in</param>
        /// <param name="assemblyName">The AssemblyName of the assembly where the desired type to be proxied resides</param>
        /// <param name="typeName">The name of the target type to be proxied from the assembly to load</param>
        /// <param name="constructorParams">A list of arguments to pass to the object's constructor</param>
        /// <param name="genericTypes">A list of types to be used to make the generic type</param>
        /// <returns>A DispatchProxy presented as the target object</returns>
        public static I CreateGenericInstanceAndUnwrap(AssemblyLoadContext alc, AssemblyName assemblyName, string typeName, Type[] genericTypes)
        {
            return CreateGenericInstanceAndUnwrap(alc, assemblyName, typeName, new object[] { }, genericTypes);
        }
    }

    /// <summary>
    /// The DispatchProxy object that represents the proxied object created through the ProxyBuilder
    /// </summary>
    /// <typeparam name="I">The interface the DispatchProxy inherits from</typeparam>
    /// <typeparam name="T">The IProxyClient type that the proxy will use to communicate with the proxied object</typeparam>
    public class ALCDispatch<I, T> : DispatchProxy where T : IProxyClient // I is the specific client you want to use.
    {
        private IProxyClient _client; // IProxyClient

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
            return _client.SendMethod(targetMethod, args); // Whenever we call the method, instead we send the request to the client to make it to target
        }
    }
}
